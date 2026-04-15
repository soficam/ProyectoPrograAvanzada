using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectoPrograAvanzada.Filters;
using ProyectoPrograAvanzada.Models;
using ProyectoPrograAvanzada.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoPrograAvanzada.Controllers
{
    [SessionAuthorize]
    public class PedidoesController : Controller
    {
        private readonly AppDbContext _context;

        public PedidoesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Pedidoes
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Pedidos
                .Where(p => p.Activo)
                .Include(p => p.Cliente)
                .Include(p => p.Usuario);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Pedidoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedido = await _context.Pedidos
            .Include(p => p.Cliente)
            .Include(p => p.Usuario)
            .Include(p => p.PedidoDetalles)
                .ThenInclude(d => d.Producto)
            .FirstOrDefaultAsync(m => m.Id == id);

            if (pedido == null)
            {
                return NotFound();
            }

            return View(pedido);
        }

        // GET: Pedidoes/Create
        public IActionResult Create()
        {
            ViewData["ClienteId"] = new SelectList(
                _context.Clientes.Where(c => c.Activo).OrderBy(c => c.Nombre),
                "Id",
                "Nombre"
            );

            ViewData["UsuarioId"] = new SelectList(
                _context.Usuarios.Where(u => u.Activo).OrderBy(u => u.Nombre),
                "Id",
                "Nombre"
            );

            ViewData["Productos"] = _context.Productos
                .Where(p => p.Activo)
                .OrderBy(p => p.Nombre)
                .ToList();

            var vm = new PedidoCreateViewModel();
            vm.Detalles.Add(new PedidoDetalleCreateItemViewModel());

            return View(vm);
        }



        // POST: Pedidoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PedidoCreateViewModel vm)
        {
            ViewData["ClienteId"] = new SelectList(
                _context.Clientes.Where(c => c.Activo).OrderBy(c => c.Nombre),
                "Id",
                "Nombre",
                vm.ClienteId
            );

            ViewData["UsuarioId"] = new SelectList(
                _context.Usuarios.Where(u => u.Activo).OrderBy(u => u.Nombre),
                "Id",
                "Nombre",
                vm.UsuarioId
            );

            ViewData["Productos"] = _context.Productos
                .Where(p => p.Activo)
                .OrderBy(p => p.Nombre)
                .ToList();

            if (vm.Detalles == null || !vm.Detalles.Any())
            {
                ModelState.AddModelError("", "Debe agregar al menos un producto al pedido.");
                return View(vm);
            }

            vm.Detalles = vm.Detalles
                .Where(d => d.ProductoId > 0 && d.Cantidad > 0)
                .ToList();

            if (!vm.Detalles.Any())
            {
                ModelState.AddModelError("", "Debe agregar al menos un detalle válido.");
                return View(vm);
            }

            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            decimal subtotal = 0;
            decimal impuestos = 0;

            var detallesCalculados = new List<PedidoDetalle>();

            foreach (var item in vm.Detalles)
            {
                var producto = await _context.Productos
                    .FirstOrDefaultAsync(p => p.Id == item.ProductoId && p.Activo);

                if (producto == null)
                {
                    ModelState.AddModelError("", $"No se encontró el producto con Id {item.ProductoId}.");
                    return View(vm);
                }

                decimal precioUnit = producto.Precio ?? 0m;
                decimal impuestoPorc = producto.ImpuestoPorc ?? 0m;
                decimal descuento = item.Descuento ?? 0m;
                decimal baseLinea = (precioUnit * item.Cantidad) - descuento;

                if (baseLinea < 0)
                    baseLinea = 0;

                decimal impuestoLinea = baseLinea * (impuestoPorc / 100m);
                decimal totalLinea = baseLinea + impuestoLinea;

                subtotal += baseLinea;
                impuestos += impuestoLinea;

                detallesCalculados.Add(new PedidoDetalle
                {
                    ProductoId = producto.Id,
                    Cantidad = item.Cantidad,
                    PrecioUnit = precioUnit,
                    Descuento = descuento,
                    ImpuestoPorc = impuestoPorc,
                    TotalLinea = totalLinea,
                    Activo = true
                });
            }

            decimal total = subtotal + impuestos;

            var pedido = new Pedido
            {
                ClienteId = vm.ClienteId,
                UsuarioId = vm.UsuarioId,
                Fecha = DateTime.Now,
                Subtotal = subtotal,
                Impuestos = impuestos,
                Total = total,
                Estado = vm.Estado,
                Activo = true
            };

            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();

            foreach (var detalle in detallesCalculados)
            {
                detalle.PedidoId = pedido.Id;
                _context.PedidoDetalles.Add(detalle);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }



        // GET: Pedidoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido == null)
            {
                return NotFound();
            }
            ViewData["ClienteId"] = new SelectList(
                _context.Clientes.Where(c => c.Activo).OrderBy(c => c.Nombre),
                "Id",
                "Nombre",
                pedido.ClienteId
             );

            ViewData["UsuarioId"] = new SelectList(
                _context.Usuarios.Where(u => u.Activo).OrderBy(u => u.Nombre),
                "Id",
                "Nombre",
                pedido.UsuarioId
            );
            return View(pedido);
        }

        // POST: Pedidoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ClienteId,UsuarioId,Fecha,Subtotal,Impuestos,Total,Estado,Activo")] Pedido pedido)
        {
            if (id != pedido.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pedido);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PedidoExists(pedido.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClienteId"] = new SelectList(
                _context.Clientes.Where(c => c.Activo).OrderBy(c => c.Nombre), 
                "Id",
                "Nombre",
                pedido.ClienteId
             );

            ViewData["UsuarioId"] = new SelectList(
                _context.Usuarios.Where(u => u.Activo).OrderBy(u => u.Nombre),
                "Id",
                "Nombre",
                pedido.UsuarioId
            );
            return View(pedido);
        }


        // GET: Pedidoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedido = await _context.Pedidos
                .Include(p => p.Cliente)
                .Include(p => p.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pedido == null)
            {
                return NotFound();
            }

            return View(pedido);
        }

        // POST: Pedidoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pedido = await _context.Pedidos
                .Include(p => p.PedidoDetalles)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pedido == null)
            {
                return NotFound();
            }

            pedido.Activo = false;

            foreach (var detalle in pedido.PedidoDetalles)
            {
                detalle.Activo = false;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PedidoExists(int id)
        {
            return _context.Pedidos.Any(e => e.Id == id);
        }

        [HttpGet]
        public IActionResult PagoDemo()
        {
            return View("Pago");
        }

    }
}

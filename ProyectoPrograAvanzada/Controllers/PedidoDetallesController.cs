using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectoPrograAvanzada.Filters;
using ProyectoPrograAvanzada.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoPrograAvanzada.Controllers
{
    [SessionAuthorize]
    public class PedidoDetallesController : Controller
    {
        private readonly AppDbContext _context;

        public PedidoDetallesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: PedidoDetalles
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.PedidoDetalles.Include(p => p.Pedido).Include(p => p.Producto);
            return View(await appDbContext.ToListAsync());
        }

        // GET: PedidoDetalles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedidoDetalle = await _context.PedidoDetalles
                .Include(p => p.Pedido)
                .Include(p => p.Producto)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pedidoDetalle == null)
            {
                return NotFound();
            }

            return View(pedidoDetalle);
        }

        // GET: PedidoDetalles/Create
        public IActionResult Create()
        {
            ViewData["PedidoId"] = new SelectList(_context.Pedidos, "Id", "Id");
            ViewData["ProductoId"] = new SelectList(_context.Productos, "Id", "Id");
            return View();
        }

        // POST: PedidoDetalles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PedidoId,ProductoId,Cantidad,PrecioUnit,Descuento,ImpuestoPorc,TotalLinea,Activo")] PedidoDetalle pedidoDetalle)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pedidoDetalle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PedidoId"] = new SelectList(_context.Pedidos, "Id", "Id", pedidoDetalle.PedidoId);
            ViewData["ProductoId"] = new SelectList(_context.Productos, "Id", "Id", pedidoDetalle.ProductoId);
            return View(pedidoDetalle);
        }

        // GET: PedidoDetalles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedidoDetalle = await _context.PedidoDetalles.FindAsync(id);
            if (pedidoDetalle == null)
            {
                return NotFound();
            }
            ViewData["PedidoId"] = new SelectList(_context.Pedidos, "Id", "Id", pedidoDetalle.PedidoId);
            ViewData["ProductoId"] = new SelectList(_context.Productos, "Id", "Id", pedidoDetalle.ProductoId);
            return View(pedidoDetalle);
        }

        // POST: PedidoDetalles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PedidoId,ProductoId,Cantidad,PrecioUnit,Descuento,ImpuestoPorc,TotalLinea,Activo")] PedidoDetalle pedidoDetalle)
        {
            if (id != pedidoDetalle.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pedidoDetalle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PedidoDetalleExists(pedidoDetalle.Id))
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
            ViewData["PedidoId"] = new SelectList(_context.Pedidos, "Id", "Id", pedidoDetalle.PedidoId);
            ViewData["ProductoId"] = new SelectList(_context.Productos, "Id", "Id", pedidoDetalle.ProductoId);
            return View(pedidoDetalle);
        }

        // GET: PedidoDetalles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedidoDetalle = await _context.PedidoDetalles
                .Include(p => p.Pedido)
                .Include(p => p.Producto)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pedidoDetalle == null)
            {
                return NotFound();
            }

            return View(pedidoDetalle);
        }

        // POST: PedidoDetalles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pedidoDetalle = await _context.PedidoDetalles.FindAsync(id);
            if (pedidoDetalle != null)
            {
                _context.PedidoDetalles.Remove(pedidoDetalle);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PedidoDetalleExists(int id)
        {
            return _context.PedidoDetalles.Any(e => e.Id == id);
        }
    }
}

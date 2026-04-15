using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoPrograAvanzada.Models;
using ProyectoPrograAvanzada.Models.ViewModels;
using ProyectoPrograAvanzada.Services;
using System.Linq;

namespace ProyectoPrograAvanzada.Controllers
{
    public class PagoController : Controller
    {
        private readonly PagoService _pagoService;
        private readonly AppDbContext _context;


        public PagoController(PagoService pagoService, AppDbContext context)
        {
            _pagoService = pagoService;
            _context = context;
        }

      

        [HttpPost]
        public IActionResult ProcesarPago(CarritoViewModel carrito, string Metodo, string NumeroTarjeta, string NombreTarjeta, string Expiracion, string CVV, string PaypalEmail)
        {
            int? usuarioId = HttpContext.Session.GetInt32("UsuarioId");

            if (carrito == null || carrito.Items == null || !carrito.Items.Any())
            {
                return RedirectToAction("Index", "Carrito");
            }

            bool pagoExitoso = Metodo == "Tarjeta" || Metodo == "PayPal";

            if (pagoExitoso)
            {
                var pedido = new Pedido
                {

                    ClienteId = 1,
                    UsuarioId = (int)usuarioId,
                    Cliente = _context.Clientes.Find(1),
                     Usuario = _context.Usuarios.Find(usuarioId),
                    Fecha = DateTime.Now,
                    Impuestos = carrito.Impuestos,
                    Subtotal = carrito.Subtotal,
                    Estado = "Pagado",
                    Activo = true,
                    Total= carrito.Total,
                    PedidoDetalles = carrito.Items.Select(item => new PedidoDetalle
                    {
                        Descuento = 0,
                        ProductoId = item.ProductoId,
                        Cantidad = item.Cantidad,
                        PrecioUnit = item.Precio,
                        ImpuestoPorc = item.ImpuestoPorc,
                        TotalLinea = (item.Cantidad * item.Precio) 
                                     + ((item.Cantidad * item.Precio) * item.ImpuestoPorc / 100),
                        Activo = true
                    }).ToList()
                };

                _context.Pedidos.Add(pedido);
                _context.SaveChanges();

                return RedirectToAction("Confirmacion", "Pago", new { id = pedido.Id });



            }

            return View("Pago", carrito);
        }

        public IActionResult Confirmacion(int id)
        {
            var pedido = _context.Pedidos
                .Include(p => p.PedidoDetalles)
                .ThenInclude(d => d.Producto)
                .FirstOrDefault(p => p.Id == id);

            if (pedido == null) return NotFound();

            foreach (var detalle in pedido.PedidoDetalles)
            {
                var producto = _context.Productos.Find(detalle.ProductoId);
                if (producto != null)
                {
                    producto.Stock -= detalle.Cantidad; // rebaja el stock
                    _context.Productos.Update(producto);
                }
            }

            _context.SaveChanges();


            return View("~/Views/Pedidoes/Confirmacion.cshtml", pedido);
            ;
        }

        public IActionResult MisPedidos()
        {
            int? usuarioId = HttpContext.Session.GetInt32("UsuarioId");

            var pedidos = _context.Pedidos
                .Include(p => p.PedidoDetalles)
                .ThenInclude(d => d.Producto)
                .Where(p => p.UsuarioId == usuarioId)
                .OrderByDescending(p => p.Fecha)
                .ToList();

            return View("~/Views/Pedidoes/MisPedidos.cshtml", pedidos);
        }

    }


}

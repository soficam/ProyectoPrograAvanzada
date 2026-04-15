using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoPrograAvanzada.Helpers;
using ProyectoPrograAvanzada.Models;
using ProyectoPrograAvanzada.Models.ViewModels;
using System;
using System.Linq;

public class CarritoController : Controller
{
    private readonly AppDbContext _context;

    public CarritoController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var usuarioRol = HttpContext.Session.GetString("UsuarioRol");

        if (usuarioRol != "Cliente")
        {
            return RedirectToAction("Login", "Account");
        }

        var carrito = CarritoSessionHelper.ObtenerCarrito(HttpContext.Session);
        return View(carrito);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Agregar(int productoId)
    {
        var usuarioRol = HttpContext.Session.GetString("UsuarioRol");

        if (usuarioRol != "Cliente")
        {
            return RedirectToAction("Login", "Account");
        }

        var producto = await _context.Productos
            .FirstOrDefaultAsync(p => p.Id == productoId && p.Activo);

        if (producto == null)
        {
            return RedirectToAction("Index", "Catalogo");
        }

        var carrito = CarritoSessionHelper.ObtenerCarrito(HttpContext.Session);

        var itemExistente = carrito.Items.FirstOrDefault(x => x.ProductoId == productoId);

        if (itemExistente != null)
        {
            if (itemExistente.Cantidad < itemExistente.StockDisponible)
            {
                itemExistente.Cantidad++;
            }
        }
        else
        {
            carrito.Items.Add(new CarritoItemViewModel
            {
                ProductoId = producto.Id,
                Nombre = producto.Nombre,
                ImagenUrl = producto.ImagenUrl,
                Precio = producto.Precio ?? 0m,
                ImpuestoPorc = producto.ImpuestoPorc ?? 0m,
                Cantidad = 1,
                StockDisponible = producto.Stock ?? 0
            });
        }

        CarritoSessionHelper.GuardarCarrito(HttpContext.Session, carrito);

        return RedirectToAction("Index", "Catalogo");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Aumentar(int productoId)
    {
        var carrito = CarritoSessionHelper.ObtenerCarrito(HttpContext.Session);

        var item = carrito.Items.FirstOrDefault(x => x.ProductoId == productoId);
        if (item != null && item.Cantidad < item.StockDisponible)
        {
            item.Cantidad++;
        }

        CarritoSessionHelper.GuardarCarrito(HttpContext.Session, carrito);
        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Disminuir(int productoId)
    {
        var carrito = CarritoSessionHelper.ObtenerCarrito(HttpContext.Session);

        var item = carrito.Items.FirstOrDefault(x => x.ProductoId == productoId);
        if (item != null)
        {
            item.Cantidad--;

            if (item.Cantidad <= 0)
            {
                carrito.Items.Remove(item);
            }
        }

        CarritoSessionHelper.GuardarCarrito(HttpContext.Session, carrito);
        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Eliminar(int productoId)
    {
        var carrito = CarritoSessionHelper.ObtenerCarrito(HttpContext.Session);

        var item = carrito.Items.FirstOrDefault(x => x.ProductoId == productoId);
        if (item != null)
        {
            carrito.Items.Remove(item);
        }

        CarritoSessionHelper.GuardarCarrito(HttpContext.Session, carrito);
        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Limpiar()
    {
        CarritoSessionHelper.LimpiarCarrito(HttpContext.Session);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult PagoDemo(CarritoViewModel carrito)
    {
        var usuarioRol = HttpContext.Session.GetString("UsuarioRol");
        if (usuarioRol != "Cliente")
        {
            return RedirectToAction("Login", "Account");
        }

        if (carrito == null || carrito.Items == null || !carrito.Items.Any())
        {
            
            return RedirectToAction("Index");
        }

        return View("~/Views/Pedidoes/Pago.cshtml", carrito);

    }

}
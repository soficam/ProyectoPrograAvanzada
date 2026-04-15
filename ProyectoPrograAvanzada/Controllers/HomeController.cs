using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoPrograAvanzada.Filters;
using ProyectoPrograAvanzada.Models;
using System.Diagnostics;

namespace ProyectoPrograAvanzada.Controllers
{
    [SessionAuthorize]
    public class HomeController : Controller
    {

        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var usuarioRol = HttpContext.Session.GetString("UsuarioRol");

            if (usuarioRol == "Cliente")
            {
                return RedirectToAction("Index", "Catalogo");
            }

            ViewBag.TotalPedidos = _context.Pedidos.Count(p => p.Activo);
            ViewBag.TotalProductos = _context.Productos.Count(p => p.Activo);
            ViewBag.TotalClientes = _context.Clientes.Count(c => c.Activo);
            ViewBag.TotalUsuarios = _context.Usuarios.Count(u => u.Activo);

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        //Test de error 500
        public IActionResult TestError()
        {
            throw new Exception("Error de prueba para manejo de excepciones");
        }

        public IActionResult TestDivision()
        {
            int x = 0;
            int resultado = 5 / x;

            return View();
        }
    }
}

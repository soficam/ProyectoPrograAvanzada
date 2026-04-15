using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoPrograAvanzada.Models;

namespace ProyectoPrograAvanzada.Controllers
{
    public class CatalogoController : Controller
    {
        private readonly AppDbContext _context;

        public CatalogoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? busqueda, int? categoriaId, decimal? precioMin, decimal? precioMax)
        {
            var query = _context.Productos
                .Include(p => p.Categoria)
                .Where(p => p.Activo)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(busqueda))
            {
                query = query.Where(p => p.Nombre.Contains(busqueda));
            }

            if (categoriaId.HasValue && categoriaId.Value > 0)
            {
                query = query.Where(p => p.CategoriaId == categoriaId.Value);
            }

            if (precioMin.HasValue)
            {
                query = query.Where(p => (p.Precio ?? 0) >= precioMin.Value);
            }

            if (precioMax.HasValue)
            {
                query = query.Where(p => (p.Precio ?? 0) <= precioMax.Value);
            }

            var productos = await query
                .OrderBy(p => p.Nombre)
                .ToListAsync();

            ViewBag.Categorias = await _context.Categoria
                .Where(c => c.Activo)
                .OrderBy(c => c.Nombre)
                .ToListAsync();

            ViewBag.Busqueda = busqueda;
            ViewBag.CategoriaId = categoriaId;
            ViewBag.PrecioMin = precioMin;
            ViewBag.PrecioMax = precioMax;

            return View(productos);
        }


        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(p => p.Id == id && p.Activo);

            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

    }
}
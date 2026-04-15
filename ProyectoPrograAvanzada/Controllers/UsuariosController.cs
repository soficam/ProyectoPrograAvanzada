using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectoPrograAvanzada.Filters;
using ProyectoPrograAvanzada.Helpers;
using ProyectoPrograAvanzada.Models;
using ProyectoPrograAvanzada.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoPrograAvanzada.Controllers
{
    [SessionAuthorize]
    [RoleAuthorize("Sistema")]
    public class UsuariosController : Controller
    {
        private readonly AppDbContext _context;

        public UsuariosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Usuarios
        public IActionResult Index(string busqueda)
        {
            var usuarios = from u in _context.Usuarios
                           select u;

            if (!string.IsNullOrEmpty(busqueda))
            {
                usuarios = usuarios.Where(u => u.Nombre.Contains(busqueda));
            }

            ViewBag.Busqueda = busqueda;

            return View(usuarios.ToList());
        }


        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            var vm = new UsuarioFormViewModel
            {
                Activo = true
            };

            return View(vm);
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UsuarioFormViewModel vm)
        {
            if (string.IsNullOrWhiteSpace(vm.Contrasena))
            {
                ModelState.AddModelError("Contrasena", "La contraseña es obligatoria.");
            }

            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var usuario = new Usuario
            {
                Nombre = vm.Nombre,
                Correo = vm.Correo,
                Rol = vm.Rol,
                Activo = vm.Activo,
                ContrasenaHash = PasswordHelper.HashPassword(vm.Contrasena!)
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            var vm = new UsuarioFormViewModel
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Correo = usuario.Correo,
                Rol = usuario.Rol,
                Activo = usuario.Activo
            };

            return View(vm);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UsuarioFormViewModel vm)
        {
            if (id != vm.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            usuario.Nombre = vm.Nombre;
            usuario.Correo = vm.Correo;
            usuario.Rol = vm.Rol;
            usuario.Activo = vm.Activo;

            // Solo cambia contraseña si el usuario escribió una nueva
            if (!string.IsNullOrWhiteSpace(vm.Contrasena))
            {
                usuario.ContrasenaHash = PasswordHelper.HashPassword(vm.Contrasena);
            }

            try
            {
                _context.Update(usuario);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Usuarios.Any(e => e.Id == vm.Id))
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

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.Id == id);
        }
    }
}

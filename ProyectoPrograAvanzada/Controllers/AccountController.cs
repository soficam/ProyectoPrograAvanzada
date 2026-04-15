using Microsoft.AspNetCore.Mvc;
using ProyectoPrograAvanzada.Models;
using ProyectoPrograAvanzada.Helpers;
using ProyectoPrograAvanzada.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ProyectoPrograAvanzada.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetInt32("UsuarioId") != null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        //POST LOGIN
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string Usuario, string Contrasena)
        {
            ViewBag.Usuario = Usuario;

            if (string.IsNullOrWhiteSpace(Usuario) || string.IsNullOrWhiteSpace(Contrasena))
            {
                ModelState.AddModelError("", "Debe ingresar usuario y contraseña.");
                return View();
            }

            var usuario = _context.Usuarios
                .FirstOrDefault(u => u.Correo == Usuario && u.Activo);

            if (usuario == null)
            {
                ModelState.AddModelError("", "Usuario no encontrado o inactivo.");
                return View();
            }

            bool passwordValida = PasswordHelper.VerifyPassword(Contrasena, usuario.ContrasenaHash);

            if (!passwordValida)
            {
                ModelState.AddModelError("", "Contraseña incorrecta.");
                return View();
            }

            HttpContext.Session.SetInt32("UsuarioId", usuario.Id);
            HttpContext.Session.SetString("UsuarioNombre", usuario.Nombre);
            HttpContext.Session.SetString("UsuarioRol", usuario.Rol);

            if (usuario.Rol == "Cliente")
            {
                return RedirectToAction("Index", "Catalogo");
            }

            return RedirectToAction("Index", "Home");
        }

        //Cerrar sesión
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Catalogo");
        }

        //Acceso denegado
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        //Olvido de contraseña
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        //Register Get
        [HttpGet]
        public IActionResult Register()
        {
            return View(new ClienteRegisterViewModel());
        }

        //Register Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(ClienteRegisterViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            bool correoExisteEnUsuarios = await _context.Usuarios
                .AnyAsync(u => u.Correo == vm.Correo);

            if (correoExisteEnUsuarios)
            {
                ModelState.AddModelError("Correo", "Ya existe una cuenta registrada con este correo.");
                return View(vm);
            }

            bool correoExisteEnClientes = await _context.Clientes
                .AnyAsync(c => c.Correo == vm.Correo);

            if (correoExisteEnClientes)
            {
                ModelState.AddModelError("Correo", "Ya existe un cliente registrado con este correo.");
                return View(vm);
            }

            bool cedulaExiste = await _context.Clientes
                .AnyAsync(c => c.Cedula == vm.Cedula);

            if (cedulaExiste)
            {
                ModelState.AddModelError("Cedula", "Ya existe un cliente registrado con esta cédula.");
                return View(vm);
            }

            var usuario = new Usuario
            {
                Nombre = vm.Nombre,
                Correo = vm.Correo,
                Rol = "Cliente",
                Activo = true,
                ContrasenaHash = PasswordHelper.HashPassword(vm.Contrasena)
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            var cliente = new Cliente
            {
                Nombre = vm.Nombre,
                Cedula = vm.Cedula,
                Correo = vm.Correo,
                Telefono = vm.Telefono,
                Direccion = vm.Direccion,
                UsuarioId = usuario.Id,
                Activo = true
            };

            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();

            TempData["RegistroExitoso"] = "Cuenta creada correctamente. Ahora puede iniciar sesión.";
            return RedirectToAction("Login");
        }



    }
}
using Microsoft.AspNetCore.Mvc;

namespace ProyectoPrograAvanzada.Controllers
{
    [Route("Error")]
    public class ErrorController : Controller
    {
        [Route("")]
        public IActionResult Index()
        {
            Response.StatusCode = 500;
            return View();
        }

        [Route("HttpError404")]
        public IActionResult HttpError404()
        {
            Response.StatusCode = 404;
            return View();
        }

        [Route("HttpError500")]
        public IActionResult HttpError500()
        {
            Response.StatusCode = 500;
            return View();
        }

        [Route("StatusCode/{statusCode}")]
        public IActionResult StatusCodeHandler(int statusCode)
        {
            return statusCode switch
            {
                404 => RedirectToAction(nameof(HttpError404)),
                500 => RedirectToAction(nameof(HttpError500)),
                _ => RedirectToAction(nameof(Index))
            };
        }
    }
}

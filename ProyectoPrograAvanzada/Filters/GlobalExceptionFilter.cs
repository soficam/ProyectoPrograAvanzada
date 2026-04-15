using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ProyectoPrograAvanzada.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<GlobalExceptionFilter> _logger;

        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception,
                "Se produjo una excepción no controlada en {Controller}/{Action}.",
                context.RouteData.Values["controller"],
                context.RouteData.Values["action"]);

            context.ExceptionHandled = true;
            context.Result = new RedirectToActionResult("HttpError500", "Error", null);
        }
    }
}

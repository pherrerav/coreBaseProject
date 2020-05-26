using Microsoft.AspNetCore.Mvc;

namespace baseproject.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/{StatusCode}")]
        public IActionResult StatusCodeHandle(int statusCode)
        {
            switch (statusCode)
            {
                case 404:
                    ViewBag.ErrorTitle = "Página no encontrada";
                    ViewBag.ErrorMessage = "Disculpe, la página solicitada no ha sido encontrada.";
                    break;
                case 500:
                    ViewBag.ErrorTitle = "Error interno";
                    ViewBag.ErrorMessage = "Disculpe, se ha presentado un error en la aplicación.";
                    break;
                case 403:
                    ViewBag.ErrorTitle = "Acceso denegado";
                    ViewBag.ErrorMessage = "Disculpe, se le ha denegado el acceso a esta página.";
                    break;

            }
            ViewBag.ErrorCode = statusCode;
            return View(statusCode);
        }
    }
}
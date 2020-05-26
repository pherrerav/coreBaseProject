using baseproject.core.logsList;
using baseproject.core.perfil;
using baseproject.core.seguridad;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace baseproject.Controllers
{
    public class LogsController : Controller
    {
        private readonly IPerfilServicio perfilService;
        private readonly ILogsListServicio logsService;
        private readonly IAuthenticationService seguridadService;
        public LogsController(ILogsListServicio logsService, IPerfilServicio perfilService, 
            IAuthenticationService seguridadService)
        {
            this.logsService = logsService;
            this.perfilService = perfilService;
            this.seguridadService = seguridadService;
        }
        public IActionResult Index(int id)
        {
            string usuario = HttpContext.Session.GetString("_User");
            if (usuario == null)
                return RedirectToAction("Login", "Security");
            else
            {
                int perfil = Convert.ToInt16(HttpContext.Session.GetString("_Profile"));
                int validarAcceso = seguridadService.ValidarAcceso(id, perfil);
                if (validarAcceso == 1)
                {
                    ViewBag.Name = usuario;
                    ViewBag.Modulo = "Seguridad";
                    ViewBag.Pagina = "Logs del Sistema";
                    return View();
                }
                else
                    return RedirectToRoute("error", new { statusCode = 403 });
            }
        }
        public IActionResult GetLogsFiles()
        {
            var files = logsService.GetLogsFiles();
            return new JsonResult(files);
        }

    }
}
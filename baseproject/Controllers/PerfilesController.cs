using baseproject.core.perfil;
using baseproject.core.seguridad;
using baseproject.data.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using proyectomvc.core.pagina;
using System;

namespace baseproject.Controllers
{
    public class PerfilesController : Controller
    {
        private readonly IPerfilServicio perfilService;
        private readonly IPaginaServicio paginaService;
        private readonly IAuthenticationService seguridadService;
        public PerfilesController(IPerfilServicio perfilService, IPaginaServicio paginaService,
            IAuthenticationService seguridadService)
        {
            this.perfilService = perfilService;
            this.paginaService = paginaService;
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
                    ViewBag.paginas = paginaService.GetPaginas();
                    ViewBag.Pagina = "Perfiles del Sistema";
                    return View();
                }
                else
                    return RedirectToRoute("error", new { statusCode = 403 });
            }
        }
        public IActionResult GetPerfiles()
        {
            var perfiles = perfilService.GetPerfiles();
            return new JsonResult(perfiles);
        }
        [HttpPost]
        public IActionResult GetPerfilPaginas(int Id)
        {
            var paginas = perfilService.BuscarPaginaPerfil(Id);
            return new JsonResult(paginas);
        }
        [HttpPost]
        public int AgregarPerfil(PerfilViewModel perfilVm)
        {
            int resultado;
            if (ModelState.IsValid)
                resultado = perfilService.AgregarPerfil(perfilVm);
            else
                resultado = -1;
            return resultado;
        }
        [HttpPost]
        public int ModificarPerfil(PerfilViewModel perfilVm)
        {
            int resultado;
            if (ModelState.IsValid)
                resultado = perfilService.ModificarPerfil(perfilVm);
            else
                resultado = -1;
            return resultado;
        }
    }
}

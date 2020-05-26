using baseproject.core.perfil;
using baseproject.core.seguridad;
using baseproject.core.usuario;
using baseproject.data.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace baseproject.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly IPerfilServicio perfilService;
        private readonly IUsuarioServicio usuarioService;
        private readonly IAuthenticationService seguridadService;
        public UsuariosController(IUsuarioServicio usuarioService, IPerfilServicio perfilService,
            IAuthenticationService seguridadService)
        {
            this.usuarioService = usuarioService;
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
                    ViewBag.perfiles = perfilService.GetPerfiles();
                    ViewBag.Pagina = "Usuarios del Sistema";
                    return View();
                }
                else
                    return RedirectToRoute("error", new { statusCode = 403 });
            }
        }
        public IActionResult GetUsuarios()
        {
            var usuarios = usuarioService.GetUsuarios();
            return new JsonResult(usuarios);
        }

        [HttpPost]
        public int AgregarUsuario(UsuarioViewModel usuarioVm)
        {
            int resultado;
            if (ModelState.IsValid)
                resultado = usuarioService.AgregarUsuario(usuarioVm);
            else
                resultado = -1;
            return resultado;
        }

        [HttpPost]
        public int ModificarUsuario(UsuarioViewModel usuarioVm)
        {
            int resultado;
            if (ModelState.IsValid)
                resultado = usuarioService.ModificarUsuario(usuarioVm);
            else
                resultado = -1;
            return resultado;
        }

        [HttpPost]
        public int EliminarUsuario(int id)
        {
            int resultado = usuarioService.EliminarUsuario(id);
            return resultado;
        }
    }
}
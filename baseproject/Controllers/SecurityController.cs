using baseproject.core.seguridad;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace baseproject.Controllers
{
    public class SecurityController : Controller
    {
        private readonly IAuthenticationService authService;

        public SecurityController(IAuthenticationService authService)
        {
            this.authService = authService;
        }
      
        public IActionResult Login()
        {
            string usuario = HttpContext.Session.GetString("_User");
            if (usuario != null)
                return RedirectToAction("Index", "Usuarios");
            else
                return View();
        }
        [HttpPost]
        public int CheckLogin(string usuario, string clave)
        {
            int resultado = authService.Login(usuario, clave);
            return resultado;
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("login");
        }
    }
}
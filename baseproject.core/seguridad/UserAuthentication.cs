using baseproject.data.Repositorio.perfil;
using baseproject.data.Repositorio.usuario;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using Microsoft.AspNetCore.Http;
using baseproject.data.Models;
using System.DirectoryServices;

namespace baseproject.core.seguridad
{
    public class UserAuthentication : IAuthenticationService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string DisplayNameAttribute = "DisplayName";
        private const string SAMAccountNameAttribute = "SAMAccountName";
        private readonly ILogger<UserAuthentication> _logger;
        private readonly LdapConfig config;
        private readonly IUsuarioRepositorio repoUsuario;
        private readonly IPerfilRepositorio repoPerfil;
        const string SessionUser = "_User";
        const string SessionProfile = "_Profile";
       

        public UserAuthentication(IOptions<LdapConfig> config, ILogger<UserAuthentication> logger,
            IUsuarioRepositorio repoUsuario, IPerfilRepositorio repoPerfil, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            this.config = config.Value;
            this.repoUsuario = repoUsuario;
            this.repoPerfil = repoPerfil;
            _httpContextAccessor = httpContextAccessor;
        }
        public int Login(string userName, string password)
        {
            int resultado;
            try
            {
                var user = AdAuthentication(userName, password);
                if (null != user)
                {
                    _httpContextAccessor.HttpContext.Session.SetString(SessionUser, userName);
                    // var perfil = SistemValidation(userName);
                    resultado = 1;
                    /*if (perfil == -1)//si el usuario no existe en el sistema, se le asigna el perfil de cliente
                    {
                        _httpContextAccessor.HttpContext.Session.SetString(SessionUser, userName);
                        _httpContextAccessor.HttpContext.Session.SetString(SessionProfile, PerfilCliente);
                    }
                    else
                    {
                        _httpContextAccessor.HttpContext.Session.SetString(SessionUser, userName);
                        _httpContextAccessor.HttpContext.Session.SetString(SessionProfile, perfil.ToString());
                    }*/
                }
                else
                    resultado = -2;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                resultado = -1;
            }

            return resultado;
        }
        public Authentication AdAuthentication(string userName, string password)
        {
            try
            {
                using (DirectoryEntry entry = new DirectoryEntry(config.Path + config.UserDomainName,
                    userName, password))
                {
                    using (DirectorySearcher searcher = new DirectorySearcher(entry))
                    {
                        searcher.Filter = String.Format("({0}={1})", SAMAccountNameAttribute, userName);
                        searcher.PropertiesToLoad.Add(DisplayNameAttribute);
                        searcher.PropertiesToLoad.Add(SAMAccountNameAttribute);
                        var result = searcher.FindOne();
                        if (result != null)
                        {
                            var displayName = result.Properties[DisplayNameAttribute];
                            var samAccountName = result.Properties[SAMAccountNameAttribute];
                            return new Authentication
                            {
                                DisplayName = displayName == null || displayName.Count <= 0 ? null : displayName[0].ToString(),
                                UserName = samAccountName == null || samAccountName.Count <= 0 ? null : samAccountName[0].ToString()
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return null;
        }


        public int SistemValidation(string userName)
        {
            var perfil = repoUsuario.GetUserPerfil(userName);
            return perfil;
        }

        public int ValidarAcceso(int pagina, int perfil)
        {
            int resultado = repoPerfil.ValidarAcceso(pagina, perfil);
            return resultado;
        }
    }

}

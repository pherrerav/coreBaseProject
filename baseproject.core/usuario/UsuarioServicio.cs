using AutoMapper;
using baseproject.data.Models;
using baseproject.data.Repositorio.usuario;
using baseproject.data.ViewModels;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace baseproject.core.usuario
{
    public class UsuarioServicio : IUsuarioServicio
    {
        private readonly IUsuarioRepositorio repoUsuario;
        private readonly ILogger<UsuarioServicio> _logger;
        private readonly IMapper _mapper;

        // Assign the object in the constructor for dependency injection
        public UsuarioServicio(IMapper mapper, IUsuarioRepositorio repoUsuario, ILogger<UsuarioServicio> logger)
        {
            _mapper = mapper;
            _logger = logger;
            this.repoUsuario = repoUsuario;
        }

        public IEnumerable<UsuarioViewModel> GetUsuarios()
        {
            var usuarios = _mapper.Map<IEnumerable<Usuario>, List<UsuarioViewModel>>(repoUsuario.GetAll());
            return (usuarios);
        }
        public int AgregarUsuario(UsuarioViewModel usuarioObj)
        {
            var resultado = 1;
            try
            {
                if (IsDuplicate(usuarioObj.CodigoUsuario))
                    resultado = -2;
                else {
                    repoUsuario.Insert(_mapper.Map<UsuarioViewModel, Usuario>(usuarioObj));
                    repoUsuario.Save();
                } 
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                resultado = -1;
            }
            return resultado;
        }
        public int ModificarUsuario(UsuarioViewModel usuarioObj)
        {
            var resultado = 1;
            try
            {
                repoUsuario.Update(_mapper.Map<UsuarioViewModel, Usuario>(usuarioObj));
                repoUsuario.Save();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                resultado = -1;
            }
            return resultado;
        }
        public int EliminarUsuario(int id)
        {
            var resultado = 1;
            try
            {
                repoUsuario.Delete(id);
                repoUsuario.Save();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                resultado = -1;
            }
            return resultado;
        }

        public int GetUserPerfil(string CodigoUsuario)
        {
            return repoUsuario.GetUserPerfil(CodigoUsuario);
        }
        public bool IsDuplicate(string CodigoUsuario)
        {
            return repoUsuario.IsDuplicate(CodigoUsuario);
        }
    }
}

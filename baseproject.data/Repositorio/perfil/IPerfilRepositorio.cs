using baseproject.data.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace baseproject.data.Repositorio.perfil
{
    public interface IPerfilRepositorio : IRepositorio<Perfil>
    {
        IEnumerable GetPerfiles();
        int GuardarPerfil(Perfil obj);
        int ModificarPerfil(Perfil obj);
        int EliminarPaginaPerfil(int perfilId);
        int AgregarPaginaPerfil(Perfil obj);
        ICollection BuscarPaginaPerfil(int id);
        int ValidarAcceso(int pagina, int perfil);
        bool IsDuplicate(string perfil);
    }
}

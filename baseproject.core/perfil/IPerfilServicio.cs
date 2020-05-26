using baseproject.data.ViewModels;
using System.Collections;

namespace baseproject.core.perfil
{
    public interface IPerfilServicio
    {
        IEnumerable GetPerfiles();
        ICollection BuscarPaginaPerfil(int id);
        int AgregarPerfil(PerfilViewModel perfilObj);
        int ModificarPerfil(PerfilViewModel perfilObj);
    }
}

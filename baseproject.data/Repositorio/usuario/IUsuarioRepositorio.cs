using baseproject.data.Models;

namespace baseproject.data.Repositorio.usuario
{
    public interface IUsuarioRepositorio : IRepositorio<Usuario>
    {
        bool IsDuplicate(string CodigoUsuario);
        int GetUserPerfil(string CodigoUsuario);
    }
}

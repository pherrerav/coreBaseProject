using System.Linq;
using baseproject.data.Models;
using baseproject.data.Repositorio.usuario;

namespace baseproject.data.Repositorio
{
    public class UsuarioRepositorio : Repositorio<Usuario>, IUsuarioRepositorio
    {
        public UsuarioRepositorio(MyDbContext dbContext)
        : base(dbContext)
        {
            table = dbContext.Set<Usuario>();
        }

        public int GetUserPerfil(string CodigoUsuario)
        {
            var perfil =
                 (from c in table
                  where c.CodigoUsuario == CodigoUsuario
                  select new { c.PerfilId }).SingleOrDefault();

            if (perfil == null)
                return -1;
            else
                return perfil.PerfilId;
        }

        public bool IsDuplicate(string codigoUsuario)
        {
            return table.Any(x => x.CodigoUsuario == codigoUsuario);
        }
    }
}

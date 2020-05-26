using baseproject.data.Models;
using System.Collections.Generic;

namespace baseproject.data.ViewModels
{
    public class PerfilViewModel
    {
        public int? Id { get; set; }
        public string PerfilNombre { get; set; }
        public virtual ICollection<Pagina_Perfil> Paginas_Perfiles { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace baseproject.data.Models
{
    public class Pagina_Perfil
    {
        public int PaginaId { get; set; }
        public Pagina Pagina { get; set; }
        public int PerfilId { get; set; }
        public Perfil Perfil { get; set; }
    }
}

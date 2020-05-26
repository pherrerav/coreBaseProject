using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace baseproject.data.Models
{
    public class Perfil
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(30)]
        public string PerfilNombre { get; set; }
        public ICollection<Usuario> Usuario { get; set; }
        public virtual ICollection<Pagina_Perfil> Paginas_Perfiles { get; set; }
    }
}

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace baseproject.data.Models
{
    public class Pagina
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [MaxLength(50)]
        [Required]
        [Column("Pagina")]
        public string PaginaNombre { get; set; }
        public virtual ICollection<Pagina_Perfil> Paginas_Perfiles { get; set; }
    }
}

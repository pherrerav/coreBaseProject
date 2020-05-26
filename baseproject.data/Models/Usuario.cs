using System;
using System.ComponentModel.DataAnnotations;

namespace baseproject.data.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(30)]
        public string CodigoUsuario { get; set; }
        [Required]
        [StringLength(30)]
        public string Nombre { get; set; }
        [Required]
        [StringLength(60)]
        public string Apellidos { get; set; }
        public int PerfilId { get; set; }
        public Perfil Perfil { get; set; }
        [Required]
        public DateTime FechaCreado { get; set; }
        [Required]
        public byte Estado { get; set; }
    }
}

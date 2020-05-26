

namespace baseproject.data.ViewModels
{
    public class UsuarioViewModel
    {
        public int? Id { get; set; }
        public string CodigoUsuario { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public byte Estado { get; set; }
        public int PerfilId { get; set; }
    }
}

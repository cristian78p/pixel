namespace Red_Social_Voluntarios.Models
{
    public class Usuario
    {
        public int Id { get; set; } 
        public string Correo { get; set; }
        public string NombreUsuario { get; set; }
        public string Contrasena { get; set; }

        public Usuario() { }

        public Usuario(string correo, string nombreUsuario, string contrasena)
        {
            Correo = correo;
            NombreUsuario = nombreUsuario;
            Contrasena = contrasena;
        }
    }
}

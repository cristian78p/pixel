namespace Red_Social_Voluntarios
{
    public class Usuario
    {
        public string Correo { get; set; }
        public string NombreUsuario { get; set; }
        public string Contrasena { get; set; }

        public Usuario(string correo, string nombreUsuario, string contrasena)
        {
            Correo = correo;
            NombreUsuario = nombreUsuario;
            Contrasena = contrasena;
        }
    }
}
    



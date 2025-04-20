namespace Red_Social_Voluntarios.Views
{
    public class Vista_Usuario
    {
        public string PedirCorreo()
        {
            Console.Write("Ingrese su correo electrónico: ");
            return Console.ReadLine();
        }

        public string PedirNombreUsuario()
        {
            Console.Write("Ingrese su nombre de usuario: ");
            return Console.ReadLine();
        }

        public string PedirContrasena()
        {
            Console.Write("Ingrese su contraseña: ");
            return Console.ReadLine();
        }

        public void MostrarMensaje(string mensaje)
        {
            Console.WriteLine(mensaje);
        }
    }
}
}

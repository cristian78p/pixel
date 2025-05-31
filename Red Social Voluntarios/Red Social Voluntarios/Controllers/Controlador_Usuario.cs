using Red_Social_Voluntarios.Models;
using Red_Social_Voluntarios.Views;
using System.Collections.Generic;

namespace Red_Social_Voluntarios.Controllers
{
    public class Controlador_Usuario

    {
        private Vista_Usuario vista;

        public Controlador_Usuario()
        {
            vista = new Vista_Usuario();
        }

        public void RegistrarUsuario(List<Usuario> listaUsuarios)
        {
            string correo = vista.PedirCorreo();
            string nombreUsuario = vista.PedirNombreUsuario();
            string contrasena = vista.PedirContrasena();

            if (ValidarDatos(correo, nombreUsuario, contrasena))
            {
                Usuario nuevoUsuario = new Usuario();
                listaUsuarios.Add(nuevoUsuario);
                vista.MostrarMensaje("Registro exitoso");
                //  Par la base de datos :)
            }
        }

        private bool ValidarDatos(string correo, string usuario, string contrasena)
        {
            if (string.IsNullOrWhiteSpace(correo) || string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(contrasena))
            {
                vista.MostrarMensaje("Todos los campos son obligatorios.");
                return false;
            }

            if (!correo.Contains("@") || !correo.Contains("."))
            {
                vista.MostrarMensaje("El correo electrónico no es válido.");
                return false;
            }

            return true;
        }

        public void IniciarSesion(List<Usuario> usuariosRegistrados)
        {
            string correo = vista.PedirCorreo();
            string contrasena = vista.PedirContrasena();

            Usuario usuarioEncontrado = usuariosRegistrados.FirstOrDefault(u => u.Correo == correo && u.Contrasena == contrasena);

            if (usuarioEncontrado != null)
            {
                vista.MostrarMensaje($"¡Bienvenido, {usuarioEncontrado.NombreUsuario}!");
            }
            else
            {
                vista.MostrarMensaje("Correo o contraseña incorrectos.");
            }
        }

    }
}

    


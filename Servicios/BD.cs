using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Dominio;

namespace Servicios
{
    public class BD
    {
        readonly string connectionString = "data source=localhost\\SQLSERVER;initial catalog=Proyecto_Final_Integrador;trusted_connection=true";

        public int RegistrarUsuarioBD(Usuario NuevoUsuario)
        {
            string queryString = "Insert into Usuario (Email, PasswordHash, FechaRegistro, Activo, Nombre, Apellido, Telefono) values (@Email, @Password, getdate(), 1, @Nombre, @Apellido, @Telefono); Select SCOPE_IDENTITY()";
            int resultado = 0;
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(queryString, sqlConnection))
            {
                command.Parameters.AddWithValue("@Email", NuevoUsuario.EmailUsuario);
                command.Parameters.AddWithValue("@Password", NuevoUsuario.PasswordUsuario);
                command.Parameters.AddWithValue("@Nombre", NuevoUsuario.NombreUsuario);
                command.Parameters.AddWithValue("@Apellido", NuevoUsuario.ApellidoUsuario);
                command.Parameters.AddWithValue("@Telefono", NuevoUsuario.TelefonoUsuario);

                try
                {
                    sqlConnection.Open();
                    resultado = Int32.Parse(command.ExecuteScalar().ToString());
                }
                catch (SqlException)
                {
                    return resultado;
                }
            }
            return resultado; 
        }

    }
}

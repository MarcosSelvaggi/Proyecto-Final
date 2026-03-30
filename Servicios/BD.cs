using Dominio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios
{
    public class BD
    {
        readonly string connectionString = "data source=localhost\\SQLSERVER;initial catalog=Proyecto_Final_Integrador;trusted_connection=true";
        //readonly string connectionString = "data source=localhost\\SQLEXPRESS;initial catalog=Proyecto_Final_Integrador;trusted_connection=true";

        public int RegistrarUsuarioBD(Usuario NuevoUsuario)
        {

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand("RegistrarUsuariosNuevos", sqlConnection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("@Email", SqlDbType.NVarChar).Value = NuevoUsuario.EmailUsuario.ToString();
                command.Parameters.Add("@Password", SqlDbType.NVarChar).Value = NuevoUsuario.PasswordUsuario.ToString();
                command.Parameters.Add("@Nombre", SqlDbType.NVarChar).Value = NuevoUsuario.NombreUsuario.ToString();
                command.Parameters.Add("@Apellido", SqlDbType.NVarChar).Value = NuevoUsuario.ApellidoUsuario.ToString();
                command.Parameters.Add("@Telefono", SqlDbType.NVarChar).Value = NuevoUsuario.TelefonoUsuario.ToString();

                try
                {
                    sqlConnection.Open();
                    return command.ExecuteNonQuery();
                }
                catch (SqlException)
                {
                    return 0;
                }
            }
        }


        public Usuario LogearUsuario(string email, string password)
        {
            string query = @"SELECT 
                        U.Nombre,
                        U.Apellido,
                        U.Telefono,
                        U.Activo,
                        U.PasswordHash,
                        P.Descripcion,
                        C.Direccion,
                        C.Localidad,
                        C.Departamento,
                        C.Provincia,
                        U.Email
                    FROM Usuario U
                    LEFT JOIN Prestador P ON U.IdUsuario = P.IdUsuario
                    LEFT JOIN Cliente C ON U.IdUsuario = C.IdUsuario
                    WHERE U.Email = @Email";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Email", email);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        string passwordHash = reader["PasswordHash"].ToString();

                        if (BCrypt.Net.BCrypt.EnhancedVerify(password, passwordHash))
                        {
                            Usuario usuario = new Usuario();

                            //usuario.Prestador = new Prestador();
                            //usuario.Cliente = new Cliente();

                            usuario.NombreUsuario = reader["Nombre"].ToString();
                            usuario.ApellidoUsuario = reader["Apellido"].ToString();
                            usuario.TelefonoUsuario = reader["Telefono"].ToString();
                            usuario.UsuarioActivo = (bool)reader["Activo"];
                            usuario.EmailUsuario = reader["Email"].ToString();

                            usuario.Prestador.DescripcionPrestador = reader["Descripcion"].ToString();

                            usuario.Cliente.Provincia = reader["Provincia"].ToString();
                            usuario.Cliente.Departamento = reader["Departamento"].ToString();
                            usuario.Cliente.Localidad = reader["Localidad"].ToString();
                            usuario.Cliente.DireccionCliente = reader["Direccion"].ToString();


                            //El stored procedure ya carga el valor 'No ingresado' a los clientes y prestadores
                            /*
                            if (reader["Descripcion"] != DBNull.Value)
                                usuario.Prestador.DescripcionPrestador = reader["Descripcion"].ToString();

                            if (reader["Direccion"] != DBNull.Value)
                                usuario.Cliente.DireccionCliente = reader["Direccion"].ToString();

                            if (reader["Localidad"] != DBNull.Value)
                                usuario.Cliente.Localidad = reader["Localidad"].ToString();

                            if (reader["Provincia"] != DBNull.Value)
                                usuario.Cliente.Provincia = reader["Provincia"].ToString();
                            */
                            return usuario;
                        }
                    }
                }
                catch (SqlException)
                {
                    return null;
                }
            }

            return null;
        }

        public bool EmailExiste(string email)
        {
            string query = "SELECT COUNT(*) FROM Usuario WHERE Email = @Email";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Email", email);

                try
                {
                    conn.Open();
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
                catch (SqlException)
                {
                    return false;
                }
            }
        }

        public bool TelefonoExiste(string telefono)
        {
            string query = "SELECT COUNT(*) FROM Usuario WHERE Telefono = @Telefono";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Telefono", telefono);

                try
                {
                    conn.Open();
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
                catch (SqlException)
                {
                    return false;
                }
            }
        }

        public bool actualizarDireccionCliente(Usuario UsuarioActualizado)
        {
            string query = "update Cliente set Provincia = @Provincia, Departamento = @Departamento," +
                " Localidad = @Localidad, Direccion = @Direccion";

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, sqlConnection))
            {
                try
                {
                    command.Parameters.AddWithValue("@Provincia", UsuarioActualizado.Cliente.Provincia);
                    command.Parameters.AddWithValue("@Departamento", UsuarioActualizado.Cliente.Departamento);
                    command.Parameters.AddWithValue("@Localidad", UsuarioActualizado.Cliente.Localidad);
                    command.Parameters.AddWithValue("@Direccion", UsuarioActualizado.Cliente.DireccionCliente);

                    sqlConnection.Open();
                    command.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true; 
        }

    }
}



// BORRADORES


//Cambiado por lo que está abajo, lo dejo para ver la diferencia.

/*string queryString = "Insert into Usuario (Email, PasswordHash, FechaRegistro, Activo, Nombre, Apellido, Telefono)" +
                    " values (@Email, @Password, getdate(), 1, @Nombre, @Apellido, @Telefono); Select SCOPE_IDENTITY()";*/

//string queryString = "EXEC RegistrarUsuariosNuevos @Email = @EmailNet, @Password = @PasswordNet, " +
//    "@Nombre = @NombreNet, @Apellido = ApellidoNet, @Telefono = TelefonoNet; Select SCOPE_IDENTITY()";

//using (SqlConnection sqlConnection = new SqlConnection(connectionString))
//using (SqlCommand command = new SqlCommand(queryString, sqlConnection))
//{
//    command.Parameters.AddWithValue("@EmailNet", NuevoUsuario.EmailUsuario);
//    command.Parameters.AddWithValue("@PasswordNet", NuevoUsuario.PasswordUsuario);
//    command.Parameters.AddWithValue("@NombreNet", NuevoUsuario.NombreUsuario);
//    command.Parameters.AddWithValue("@ApellidoNet", NuevoUsuario.ApellidoUsuario);
//    command.Parameters.AddWithValue("@TelefonoNet", NuevoUsuario.TelefonoUsuario);

//Esto ejecuta el StoredProcedure de manera correcta, el anterior deja los valores como ApellidoNet, NombreNet, PasswordNet, etc.
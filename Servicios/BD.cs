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
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand("RegistrarUsuariosNuevos", sqlConnection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@Email", SqlDbType.NVarChar).Value = NuevoUsuario.EmailUsuario.ToString();
                command.Parameters.AddWithValue("@Password", SqlDbType.NVarChar).Value = NuevoUsuario.PasswordUsuario.ToString();
                command.Parameters.AddWithValue("@Nombre", SqlDbType.NVarChar).Value = NuevoUsuario.NombreUsuario.ToString();
                command.Parameters.AddWithValue("@Apellido", SqlDbType.NVarChar).Value = NuevoUsuario.ApellidoUsuario.ToString();
                command.Parameters.AddWithValue("@Telefono", SqlDbType.NVarChar).Value = NuevoUsuario.TelefonoUsuario.ToString();

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


    public Usuario LogearUsuario(string Email, string Password)
        {
            Usuario UsuarioLogeado = new Usuario();
            string queryString = "Select PasswordHash from Usuario where Email = @Email";

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(queryString, sqlConnection))
            {
                command.Parameters.AddWithValue("@Email", Email);

                try
                {
                    sqlConnection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        if (BCrypt.Net.BCrypt.EnhancedVerify(Password, reader[0].ToString()))
                        {
                            reader.Close();
                            string queryStringUserCorrect = "select U.Nombre, U.Apellido, U.Telefono, U.Activo, " +
                                "P.Descripcion, C.Direccion, C.Localidad, C.Provincia, U.Email " +
                                "from Usuario U inner join Prestador P on U.IdUsuario = P.IdUsuario " +
                                "inner join Cliente C on U.IdUsuario = C.IdUsuario where U.Email = @Email"; 

                            using (SqlConnection sqlConnectionUserCorrect = new SqlConnection(connectionString))
                            using (SqlCommand commandUserCorrect = new SqlCommand(queryStringUserCorrect, sqlConnection))
                            {
                                commandUserCorrect.Parameters.AddWithValue("@Email", Email);

                                try
                                {
                                    sqlConnectionUserCorrect.Open();
                                    SqlDataReader readerUserCorrect = commandUserCorrect.ExecuteReader();

                                    while (readerUserCorrect.Read())
                                    {
                                        UsuarioLogeado.NombreUsuario = readerUserCorrect[0].ToString();
                                        UsuarioLogeado.ApellidoUsuario = readerUserCorrect[1].ToString();
                                        UsuarioLogeado.TelefonoUsuario = readerUserCorrect[2].ToString();
                                        UsuarioLogeado.UsuarioActivo = (bool)readerUserCorrect[3];
                                        UsuarioLogeado.Prestador.DescripcionPrestador = readerUserCorrect[4].ToString();
                                        UsuarioLogeado.Cliente.DireccionCliente = readerUserCorrect[5].ToString();
                                        UsuarioLogeado.Cliente.Localidad = readerUserCorrect[6].ToString();
                                        UsuarioLogeado.Cliente.Provincia = readerUserCorrect[7].ToString();

                                        return UsuarioLogeado;
                                    }

                                }
                                catch (SqlException)
                                {
                                    return UsuarioLogeado;
                                }
                            }   
                        } 
                    }
                }
                catch (SqlException)
                {
                    return UsuarioLogeado;
                }
            }
            return UsuarioLogeado; 
        }
    }
}
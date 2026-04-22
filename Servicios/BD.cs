using Dominio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;


namespace Servicios
{
    public class BD
    {
        //readonly string connectionString = "data source=localhost\\SQLSERVER;initial catalog=Proyecto_Final_Integrador;trusted_connection=true";
        readonly string connectionString = "data source=localhost\\SQLEXPRESS;initial catalog=Proyecto_Final_Integrador;trusted_connection=true";

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

                SqlParameter returnValue = new SqlParameter();
                returnValue.Direction = ParameterDirection.ReturnValue;
                command.Parameters.Add(returnValue);

                try
                {
                    sqlConnection.Open();
                    command.ExecuteNonQuery();

                    return (int)returnValue.Value; 
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex.Message);
                    return 0;
                }
            }
        }

        public Usuario LogearUsuario(string email, string password)
        {
            string query = @"SELECT
                        U.IdUsuario,
                        P.IdPrestador,
                        U.Nombre,
                        U.Apellido,
                        U.Telefono,
                        U.Activo,
                        U.PasswordHash,
                        P.Descripcion,
                        C.Direccion,
                        C.Localidad,
                        C.LocalidadId,
                        C.IdCliente,
                        C.Departamento,
                        C.Provincia,
                        U.Email,
                        ZP.IdLocalidad,
                        D.DisponibilidadPrestador
                    FROM Usuario U
                    LEFT JOIN Prestador P ON U.IdUsuario = P.IdUsuario
                    LEFT JOIN Cliente C ON U.IdUsuario = C.IdUsuario
                    LEFT JOIN ZonasPrestador ZP on ZP.IdPrestador = P.IdPrestador
                    LEFT JOIN Disponibilidad D on D.IdPrestador = P.IdPrestador
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
                            usuario.Prestador = new Prestador();
                            usuario.Cliente = new Cliente();

                            usuario.IdUsuario = Int32.Parse(reader["IdUsuario"].ToString());
                            usuario.NombreUsuario = reader["Nombre"].ToString();
                            usuario.ApellidoUsuario = reader["Apellido"].ToString();
                            usuario.TelefonoUsuario = reader["Telefono"].ToString();
                            usuario.UsuarioActivo = (bool)reader["Activo"];
                            usuario.EmailUsuario = reader["Email"].ToString();

                            usuario.Prestador.DescripcionPrestador = reader["Descripcion"].ToString();
                            usuario.Prestador.ZonasPrestador = reader["IdLocalidad"].ToString(); 
                            usuario.Prestador.HorariosPrestador = reader["DisponibilidadPrestador"].ToString();

                            usuario.Cliente.Provincia = reader["Provincia"].ToString();
                            usuario.Cliente.Departamento = reader["Departamento"].ToString();
                            usuario.Cliente.Localidad = reader["Localidad"].ToString();
                            usuario.Cliente.IdLocalidad = reader["LocalidadId"].ToString();
                            usuario.Cliente.DireccionCliente = reader["Direccion"].ToString();


                            if (reader["IdCliente"] != DBNull.Value)
                            {
                                usuario.Cliente.IdCliente = Convert.ToInt32(reader["IdCliente"]);
                            }
                            if (reader["IdPrestador"] != DBNull.Value)
                            {
                                usuario.Prestador.IdPrestador = Convert.ToInt32(reader["IdPrestador"]);
                            }
                            if (usuario.Prestador != null && usuario.Prestador.IdPrestador > 0)
                            {
                                usuario.Prestador.Servicios = TraerServiciosPrestador(usuario.Prestador.IdPrestador);
                            }
                            else
                            {
                                usuario.Prestador.Servicios = new List<ServiciosPrestador>();
                            }

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
            string query = @"UPDATE Cliente 
                     SET Provincia = @Provincia, 
                         Departamento = @Departamento,
                         Localidad = @Localidad,
                         LocalidadId = @LocalidadId,
                         Direccion = @Direccion
                     WHERE IdUsuario = @IdUsuario";

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, sqlConnection))
            {
                try
                {
                    command.Parameters.AddWithValue("@Provincia", UsuarioActualizado.Cliente.Provincia);
                    command.Parameters.AddWithValue("@Departamento", UsuarioActualizado.Cliente.Departamento);
                    command.Parameters.AddWithValue("@Localidad", UsuarioActualizado.Cliente.Localidad);
                    command.Parameters.AddWithValue("@LocalidadId", UsuarioActualizado.Cliente.IdLocalidad);
                    command.Parameters.AddWithValue("@Direccion", UsuarioActualizado.Cliente.DireccionCliente);
                    command.Parameters.AddWithValue("@IdUsuario", UsuarioActualizado.IdUsuario);

                    sqlConnection.Open();
                    return command.ExecuteNonQuery() > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }
        }

        public List<ServiciosPrestador> TraerServiciosPrestador(int idPrestador)
        {
            List<ServiciosPrestador> serviciosPrestador = new List<ServiciosPrestador>();

            string query = "SELECT IdServicio, PrecioHora FROM PrestadorServicio WHERE IdPrestador = @IdPrestador";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@IdPrestador", idPrestador);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    serviciosPrestador.Add(new ServiciosPrestador
                    {
                        IdServicio = Convert.ToInt32(reader["IdServicio"]),
                        Precio = Convert.ToDecimal(reader["PrecioHora"])
                    });
                }
            }
            return serviciosPrestador;
        }
        public List<Servicio> TraerServiciosBD()
        {
            List<Servicio> servicios = new List<Servicio>();
            string query = "SELECT IdServicio, Nombre, Descripcion FROM Servicios ORDER BY Nombre";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    servicios.Add(new Servicio
                    {
                        IdServicio = Convert.ToInt32(reader["IdServicio"]),
                        Nombre = reader["Nombre"].ToString(),
                        Descripcion = reader["Descripcion"].ToString()
                    });
                }
            }
            return servicios;
        }

        public bool AsignarServiciosPrestador(int idPrestador, List<(int IdServicio, decimal PrecioHora)> servicios)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                foreach (var serv in servicios)
                {
                    string query = @"INSERT INTO PrestadorServicio (IdPrestador, IdServicio, PrecioHora)
                             VALUES (@IdPrestador, @IdServicio, @PrecioHora)";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@IdPrestador", idPrestador);
                    cmd.Parameters.AddWithValue("@IdServicio", serv.IdServicio);
                    cmd.Parameters.AddWithValue("@PrecioHora", serv.PrecioHora);
                    cmd.ExecuteNonQuery();
                }
            }
            return true;
        }
        public int ActualizarPrestadorBD(Usuario usuario)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();
                try
                {
                    string queryPrestador = @"
                    IF EXISTS (SELECT 1 FROM Prestador WHERE IdUsuario = @IdUsuario)
                    BEGIN
                        UPDATE Prestador
                        SET Descripcion = @Descripcion
                        WHERE IdUsuario = @IdUsuario
                    END
                    ELSE
                    BEGIN
                        INSERT INTO Prestador (IdUsuario, Descripcion)
                        VALUES (@IdUsuario, @Descripcion)
                    END";

                    SqlCommand cmdPrestador = new SqlCommand(queryPrestador, conn, transaction);
                    cmdPrestador.Parameters.AddWithValue("@IdUsuario", usuario.IdUsuario);
                    cmdPrestador.Parameters.AddWithValue("@Descripcion", usuario.Prestador.DescripcionPrestador ?? "No ingresado");
                    cmdPrestador.ExecuteNonQuery();

                    // Obtener IdPrestador
                    string queryIdPrestador = "SELECT IdPrestador FROM Prestador WHERE IdUsuario = @IdUsuario";
                    SqlCommand cmdId = new SqlCommand(queryIdPrestador, conn, transaction);
                    cmdId.Parameters.AddWithValue("@IdUsuario", usuario.IdUsuario);
                    int idPrestador = Convert.ToInt32(cmdId.ExecuteScalar());

                    // Borrar servicios anteriores
                    string deleteServicios = "DELETE FROM PrestadorServicio WHERE IdPrestador = @IdPrestador";
                    SqlCommand cmdDelete = new SqlCommand(deleteServicios, conn, transaction);
                    cmdDelete.Parameters.AddWithValue("@IdPrestador", idPrestador);
                    cmdDelete.ExecuteNonQuery();

                    // Insertar servicios nuevos
                    foreach (var serv in usuario.Prestador.Servicios)
                    {
                        string insertPrestadorServicio = @"
                        INSERT INTO PrestadorServicio (IdPrestador, IdServicio, PrecioHora)
                        VALUES (@IdPrestador, @IdServicio, @PrecioHora)";
                        SqlCommand cmdInsert = new SqlCommand(insertPrestadorServicio, conn, transaction);
                        cmdInsert.Parameters.AddWithValue("@IdPrestador", idPrestador);
                        cmdInsert.Parameters.AddWithValue("@IdServicio", serv.IdServicio);
                        cmdInsert.Parameters.AddWithValue("@PrecioHora", serv.Precio);
                        cmdInsert.ExecuteNonQuery();
                    }

                    string ActualizarZonas = "Update ZonasPrestador set IdLocalidad = @IdLocalidad where IdPrestador = @IdPrestador";
                    SqlCommand CmdZonas = new SqlCommand(ActualizarZonas, conn, transaction);
                    CmdZonas.Parameters.AddWithValue("@IdLocalidad", usuario.Prestador.ZonasPrestador);
                    CmdZonas.Parameters.AddWithValue("@IdPrestador", idPrestador);
                    CmdZonas.ExecuteNonQuery();

                    string ActualizarHorarios = "Update Disponibilidad set DisponibilidadPrestador = @Disponibilidad where IdPrestador = @IdPrestador";
                    SqlCommand CmdDisponibilidad = new SqlCommand(ActualizarHorarios, conn, transaction);
                    CmdDisponibilidad.Parameters.AddWithValue("@Disponibilidad", usuario.Prestador.HorariosPrestador);
                    CmdDisponibilidad.Parameters.AddWithValue("@IdPrestador", idPrestador);
                    CmdDisponibilidad.ExecuteNonQuery();

                    transaction.Commit();
                    return idPrestador;
                }
                catch
                {
                    transaction.Rollback();
                    return 0;
                }
            }
        }
        public int ObtenerIdUsuarioPorEmail(string email)
        {
            string query = "SELECT IdUsuario FROM Usuario WHERE Email = @Email";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Email", email);

                try
                {
                    conn.Open();
                    object resultado = cmd.ExecuteScalar(); 

                    if (resultado != null && int.TryParse(resultado.ToString(), out int idUsuario))
                    {
                        return idUsuario; 
                    }
                    else
                    {
                        return 0; 
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Error al obtener IdUsuario: " + ex.Message);
                    return 0;
                }
            }
        }

        public List<Usuario> DevolverPrestadores(Usuario usuario, int Servicio)
        {
            List<Usuario> PrestadoresEncontrados = new List<Usuario>();

            string query = @"SELECT
                        U.IdUsuario,
                        U.Nombre,
                        U.Apellido,
                        U.Telefono,
                        U.Activo,
                        U.Email,
                        P.Descripcion,
                        PS.IdServicio,
                        P.IdPrestador,
                        P.Descripcion,
                        PS.PrecioHora
                    FROM Usuario U
                    INNER JOIN Prestador P ON U.IdUsuario = P.IdUsuario
                    INNER JOIN ZonasPrestador ZP ON ZP.IdPrestador = P.IdPrestador
                    INNER JOIN PrestadorServicio PS ON PS.IdPrestador = P.IdPrestador
                    WHERE ZP.IdLocalidad LIKE @Localidad
                    AND PS.IdServicio = @IdServicio";

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            using (SqlCommand Command = new SqlCommand(query, sqlConnection))
            {
                Command.Parameters.AddWithValue("@Localidad", "%" + usuario.Cliente.IdLocalidad + "%");
                Command.Parameters.AddWithValue("@IdServicio", Servicio); // ← faltaba el @

                try
                {
                    sqlConnection.Open();
                    SqlDataReader Reader = Command.ExecuteReader();

                    while (Reader.Read())
                    {
                        Usuario Usuario = new Usuario();

                        Usuario.IdUsuario = Convert.ToInt32(Reader["IdUsuario"]);  // ← era usuario (minúscula)
                        Usuario.NombreUsuario = Reader["Nombre"].ToString();
                        Usuario.ApellidoUsuario = Reader["Apellido"].ToString();
                        Usuario.TelefonoUsuario = Reader["Telefono"].ToString();
                        Usuario.UsuarioActivo = (bool)Reader["Activo"];
                        Usuario.EmailUsuario = Reader["Email"].ToString();
                        Usuario.Prestador.IdPrestador = Convert.ToInt32(Reader["IdPrestador"]);
                        Usuario.Prestador.DescripcionPrestador = Reader["Descripcion"].ToString();
                        Dominio.ServiciosPrestador servicio = new Dominio.ServiciosPrestador()
                        {
                            IdServicio = Int32.Parse(Reader["IdServicio"].ToString()),
                            Precio = Decimal.Parse(Reader["PrecioHora"].ToString())
                        };
                        Usuario.Prestador.Servicios.Add(servicio);

                        // Precio del servicio filtrado, listo para el slider
                        Usuario.Prestador.Servicios = new List<ServiciosPrestador>
                {
                    new ServiciosPrestador
                    {
                        IdServicio = Servicio,
                        Precio = Convert.ToDecimal(Reader["PrecioHora"])
                    }
                };

                        PrestadoresEncontrados.Add(Usuario);
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex.Message);
                    return PrestadoresEncontrados;
                }
            }

            return PrestadoresEncontrados;
        }

        public bool CambiarPassword (string EmailUsuario,  string Password)
        {
            string query = "Update Usuario set PasswordHash = @Password where Email = @Email";

            using (SqlConnection Connection = new SqlConnection(connectionString))
            using (SqlCommand Command = new SqlCommand(query, Connection))
            {
                try
                {
                    Command.Parameters.AddWithValue("Email", EmailUsuario);
                    Command.Parameters.AddWithValue("Password", Password);

                    Connection.Open();
                    Command.ExecuteNonQuery();

                    return true;
                }
                catch (Exception)
                {
                    return false;
                }

            }
        }
        //public bool CrearSolicitudTurno(int idCliente, int idPrestador, int idServicio, string mensaje)
        //{
        //    string query = @"INSERT INTO Turno 
        //            (IdCliente, IdPrestador, IdServicio, Mensaje)
        //            VALUES (@IdCliente, @IdPrestador, @IdServicio, @Mensaje)";
        //
        //    using (SqlConnection conn = new SqlConnection(connectionString))
        //    using (SqlCommand cmd = new SqlCommand(query, conn))
        //    {
        //        cmd.Parameters.AddWithValue("@IdCliente", idCliente);
        //        cmd.Parameters.AddWithValue("@IdPrestador", idPrestador);
        //        cmd.Parameters.AddWithValue("@IdServicio", idServicio);
        //        cmd.Parameters.AddWithValue("@Mensaje", (object)mensaje ?? DBNull.Value);
        //
        //        conn.Open();
        //        return cmd.ExecuteNonQuery() > 0;
        //    }
        //}
        public bool CrearSolicitudTurno(int idCliente, int idPrestador, int idServicio, string mensaje)
        {
            bool Turno = false;

            using (SqlConnection Connection = new SqlConnection(connectionString))
            {
                Connection.Open();
                SqlTransaction SqlTransaction = Connection.BeginTransaction();
                try
                {
                    string Query = @"INSERT INTO Turno 
                    (IdCliente, IdPrestador, IdServicio, Mensaje)
                    VALUES (@IdCliente, @IdPrestador, @IdServicio, @Mensaje)";

                    SqlCommand Command = new SqlCommand(Query, Connection, SqlTransaction);

                    Command.Parameters.AddWithValue("@IdCliente", idCliente);
                    Command.Parameters.AddWithValue("@IdPrestador", idPrestador);
                    Command.Parameters.AddWithValue("@IdServicio", idServicio);
                    Command.Parameters.AddWithValue("@Mensaje", (object)mensaje ?? DBNull.Value);


                    if (Command.ExecuteNonQuery() > 0)
                    {

                        string QueryDatosPrestador = @"Select 
                                                       U.Nombre, U.Email
                                                       from usuario U 
                                                       Inner join Prestador P On P.IdUsuario = U.IdUsuario
                                                       where U.IdUsuario = @IdUsuario";

                        SqlCommand CommandDatosPrestador = new SqlCommand(QueryDatosPrestador, Connection, SqlTransaction);
                        CommandDatosPrestador.Parameters.AddWithValue("@IdUsuario", idPrestador);

                        try
                        {
                            SqlDataReader SqlDataReader = CommandDatosPrestador.ExecuteReader();

                            if (SqlDataReader.Read())
                            {
                                EmailService EmailService = new EmailService();
                                EmailService.EnviarMailAlPrestador(SqlDataReader["Nombre"].ToString(), SqlDataReader["Email"].ToString(), mensaje);
                                SqlDataReader.Close(); 
                            }

                        }
                        catch (Exception)
                        {
                            Turno = false;
                        }
                    }
                    SqlTransaction.Commit();
                    Turno = true;
                }
                catch (Exception)
                {
                    SqlTransaction.Rollback();
                    Turno = false;
                }
            }

            return Turno;
        }

        public DataTable TraerTurnosCliente(int idCliente)
        {
            string query = @"SELECT 
                        T.IdTurno,
                        T.Estado,
                        T.FechaSolicitud,
                        T.Mensaje,
                        S.Nombre AS Servicio,
                        U.Nombre,
                        U.Apellido,
                        U.Telefono,
                        U.Email,
                        P.Descripcion
                    FROM Turno T
                    INNER JOIN Prestador P ON P.IdPrestador = T.IdPrestador
                    INNER JOIN Usuario U ON U.IdUsuario = P.IdUsuario
                    INNER JOIN Servicios S ON S.IdServicio = T.IdServicio
                    WHERE T.IdCliente = @IdCliente
                    ORDER BY T.FechaSolicitud DESC";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@IdCliente", idCliente);

                conn.Open();
                DataTable tabla = new DataTable();
                tabla.Load(cmd.ExecuteReader());

                return tabla;
            }
        }

        public DataTable TraerTurnosPrestador(int idPrestador)
        {
            string query = @"SELECT 
                        T.IdTurno,
                        T.FechaSolicitud,
                        T.Estado,
                        T.Mensaje,
                        S.Nombre AS Servicio,
                        U.Nombre,
                        U.Apellido,
                        U.Telefono,
                        U.Email,
                        C.Provincia,
                        C.Localidad,
                        C.Direccion
                    FROM Turno T
                    INNER JOIN Cliente C ON C.IdCliente = T.IdCliente
                    INNER JOIN Usuario U ON U.IdUsuario = C.IdUsuario
                    INNER JOIN Servicios S ON S.IdServicio = T.IdServicio
                    WHERE T.IdPrestador = @IdPrestador
                    AND T.FechaSolicitud >= DATEADD(DAY, -10, GETDATE())
                    ORDER BY T.FechaSolicitud DESC";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@IdPrestador", idPrestador);

                conn.Open();
                DataTable tabla = new DataTable();
                tabla.Load(cmd.ExecuteReader());

                return tabla;
            }
        }

        //public bool ActualizarEstadoTurno(int idTurno, string estado)
        //{
        //    string query = @" UPDATE Turno 
        //                      SET Estado = @Estado
        //                      WHERE IdTurno = @IdTurno
        //                      AND Estado = 'Pendiente'";

        //    using (SqlConnection conn = new SqlConnection(connectionString))
        //    using (SqlCommand cmd = new SqlCommand(query, conn))
        //    {
        //        cmd.Parameters.AddWithValue("@Estado", estado);
        //        cmd.Parameters.AddWithValue("@IdTurno", idTurno);

        //        try
        //        {
        //            conn.Open();
        //            return cmd.ExecuteNonQuery() > 0;
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }
        //}

        /*public bool ActualizarEstadoTurno(int idTurno, string estado, string NombrePrestador)
        {
            bool Turno = false; 
            string query = @" UPDATE Turno 
                              SET Estado = @Estado
                              WHERE IdTurno = @IdTurno
                              AND Estado = 'Pendiente'";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Estado", estado);
                cmd.Parameters.AddWithValue("@IdTurno", idTurno);

                try
                {
                    conn.Open();
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        string QueryDatosCliente = @"Select U.Nombre, U.Email
                                                   From Usuario U 
                                                   Inner join Cliente C on C.IdUsuario = U.IdUsuario
                                                   Inner Join Turno T on T.IdCliente = C.IdCliente
                                                   Where IdTurno = @IdTurno";
                        using (SqlConnection Connection = new SqlConnection(connectionString))
                        using (SqlCommand Command = new SqlCommand(QueryDatosCliente, Connection))
                        {
                            Command.Parameters.AddWithValue("@IdTurno", idTurno);
                            Connection.Open();

                            try
                            {
                                SqlDataReader SqlDataReader = Command.ExecuteReader();

                                while (SqlDataReader.Read())
                                {
                                    EmailService EmailService = new EmailService();
                                    if (estado == "Aceptado")
                                    {
                                        if (EmailService.EnviarMailAlCliente(SqlDataReader["Nombre"].ToString(), NombrePrestador, SqlDataReader["Email"].ToString()))
                                        {
                                            Turno = true;
                                        }
                                    }
                                }
                                     
                            }
                            catch (Exception)
                            {
                                Turno = false; 
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Turno = false; 
                }
            }
            return Turno; 
        }
        */

        public bool ActualizarEstadoTurno(int idTurno, string estado, string NombrePrestador)
        {
            bool actualizado = false;

            string query = @"UPDATE Turno 
                             SET Estado = @Estado
                             WHERE IdTurno = @IdTurno
                             AND Estado = 'Pendiente'";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Estado", estado);
                cmd.Parameters.AddWithValue("@IdTurno", idTurno);

                conn.Open();
                actualizado = cmd.ExecuteNonQuery() > 0;
            }

            if (!actualizado)
                return false;

            try
            {
                string queryDatos = @"SELECT U.Nombre, U.Email
                                      FROM Usuario U 
                                      INNER JOIN Cliente C ON C.IdUsuario = U.IdUsuario
                                      INNER JOIN Turno T ON T.IdCliente = C.IdCliente
                                      WHERE T.IdTurno = @IdTurno";

                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand(queryDatos, conn))
                {
                    cmd.Parameters.AddWithValue("@IdTurno", idTurno);
                    conn.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            EmailService email = new EmailService();

                            if (estado == "Aceptado")
                                email.EnviarMailAlCliente(reader["Nombre"].ToString(), NombrePrestador, reader["Email"].ToString());
                            else if (estado == "Rechazado")
                                email.EnviarRechazoMailAlCliente(reader["Nombre"].ToString(), NombrePrestador, reader["Email"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return true;
        }
        public bool ActualiarInformacionUsuario(Usuario UsuarioModificado)
        {
            string Query = @"Update Usuario Set
                           Email = @Email,
                           Nombre = @Nombre,                            
                           Apellido = @Apellido,
                           Telefono = @Telefono
                           Where IdUsuario = @IdUsuario";

            using (SqlConnection Connection = new SqlConnection(connectionString))
            using (SqlCommand Command = new SqlCommand(Query, Connection))
            {
                Command.Parameters.AddWithValue("@Email", UsuarioModificado.EmailUsuario);
                Command.Parameters.AddWithValue("@Nombre", UsuarioModificado.NombreUsuario);
                Command.Parameters.AddWithValue("@Apellido", UsuarioModificado.ApellidoUsuario);
                Command.Parameters.AddWithValue("@Telefono", UsuarioModificado.TelefonoUsuario);
                Command.Parameters.AddWithValue("@IdUsuario", UsuarioModificado.IdUsuario);

                try
                {
                    Connection.Open();
                    return Command.ExecuteNonQuery() > 0;
                }
                catch (Exception)
                {
                    return false; 
                }
            }
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
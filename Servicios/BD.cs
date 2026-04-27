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
using Microsoft.Extensions.Configuration;
using System.Web.UI.WebControls;


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
        public int ContarPrestadores(int idServicio, string idLocalidad)
        {
            if (string.IsNullOrEmpty(idLocalidad))
                return 0;

            string query = @"
                            SELECT COUNT(DISTINCT P.IdPrestador)
                            FROM Usuario U
                            INNER JOIN Prestador P ON U.IdUsuario = P.IdUsuario
                            INNER JOIN ZonasPrestador ZP ON ZP.IdPrestador = P.IdPrestador
                            INNER JOIN PrestadorServicio PS ON PS.IdPrestador = P.IdPrestador
                            WHERE ',' + ZP.IdLocalidad + ',' LIKE '%,' + @Localidad + ',%'
                            AND PS.IdServicio = @IdServicio";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Localidad", idLocalidad);
                cmd.Parameters.AddWithValue("@IdServicio", idServicio);

                conn.Open();
                return (int)cmd.ExecuteScalar();
            }
        }
        public int ObtenerIdServicio(string nombreServicio)
        {
            string query = @"
                            SELECT TOP 1 IdServicio 
                            FROM Servicios 
                            WHERE LOWER(Nombre) LIKE @Nombre";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Nombre", "%" + nombreServicio.ToLower() + "%");

                try
                {
                    conn.Open();
                    object result = cmd.ExecuteScalar();

                    if (result != null)
                        return Convert.ToInt32(result);

                    return 0; // no lo encontró
                }
                catch
                {
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
                        U.FotoPerfil,
                        U.PasswordHash,
                        P.Descripcion,
                        P.Calificacion,
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
                            usuario.FotoPerfil = reader["FotoPerfil"] == DBNull.Value ? null : reader["FotoPerfil"].ToString();

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
                        U.FotoPerfil,
                        P.Descripcion,
                        PS.IdServicio,
                        P.IdPrestador,
                        P.Descripcion,
                        P.Calificacion,
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
                Command.Parameters.AddWithValue("@IdServicio", Servicio);

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
                        Usuario.FotoPerfil = Reader["FotoPerfil"] == DBNull.Value ? null : Reader["FotoPerfil"].ToString();
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

        public bool CambiarPassword(string EmailUsuario, string Password)
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

        public int CrearSolicitudTurno(int idCliente, int idPrestador, int idServicio, string mensaje)
        {
            using (SqlConnection Connection = new SqlConnection(connectionString))
            {
                Connection.Open();
                SqlTransaction SqlTransaction = Connection.BeginTransaction();
                try
                {
                    string Query = @"INSERT INTO Turno 
                                    (IdCliente, IdPrestador, IdServicio, Mensaje)
                                    VALUES (@IdCliente, @IdPrestador, @IdServicio, @Mensaje);
                                    SELECT SCOPE_IDENTITY();";

                    SqlCommand Command = new SqlCommand(Query, Connection, SqlTransaction);
                    Command.Parameters.AddWithValue("@IdCliente", idCliente);
                    Command.Parameters.AddWithValue("@IdPrestador", idPrestador);
                    Command.Parameters.AddWithValue("@IdServicio", idServicio);
                    Command.Parameters.AddWithValue("@Mensaje", (object)mensaje ?? DBNull.Value);

                    int idTurno = Convert.ToInt32(Command.ExecuteScalar());

                    if (idTurno > 0)
                    {
                        string QueryDatosPrestador = @"
                                SELECT U.Nombre, U.Email
                                FROM Usuario U 
                                INNER JOIN Prestador P ON P.IdUsuario = U.IdUsuario
                                WHERE P.IdPrestador = @IdPrestador";

                        SqlCommand CommandDatosPrestador = new SqlCommand(QueryDatosPrestador, Connection, SqlTransaction);
                        CommandDatosPrestador.Parameters.AddWithValue("@IdPrestador", idPrestador);

                        try
                        {
                            SqlDataReader reader = CommandDatosPrestador.ExecuteReader();
                            if (reader.Read())
                            {
                                EmailService EmailService = new EmailService();
                                EmailService.EnviarMailAlPrestador(reader["Nombre"].ToString(), reader["Email"].ToString(), mensaje);
                                reader.Close();
                            }
                        }
                        catch { }
                    }

                    SqlTransaction.Commit();
                    return idTurno;
                }
                catch
                {
                    SqlTransaction.Rollback();
                    return 0;
                }
            }
        }

        public DataTable TraerTurnosCliente(int idCliente)
        {
            string query = @"SELECT 
                        T.IdTurno,
                        T.Estado,
                        T.IdCliente,
                        T.IdPrestador,
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
                        T.IdCliente,
                        T.IdPrestador,
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

        public bool CalificarTurno(string IdTurno, string Comentario, string Calificacion)
        {
            bool ResultadoCalificacion = false;

            using (SqlConnection SqlConnection = new SqlConnection(connectionString))
            using (SqlCommand SqlCommand = new SqlCommand("CalificarTurnos", SqlConnection))
            {
                SqlCommand.CommandType = CommandType.StoredProcedure;

                SqlCommand.Parameters.AddWithValue("@IdTurno", IdTurno);
                SqlCommand.Parameters.AddWithValue("@Comentario", Comentario);
                SqlCommand.Parameters.AddWithValue("@Calificacion", Calificacion);

                try
                {
                    SqlConnection.Open();
                    ResultadoCalificacion = SqlCommand.ExecuteNonQuery() > 0;

                }
                catch (Exception)
                {
                    throw;
                }
            }
            return ResultadoCalificacion;
        }

        public bool GuardarFotoPerfilBD(int idUsuario, string base64)
        {
            string query = "UPDATE Usuario SET FotoPerfil = @FotoPerfil WHERE IdUsuario = @IdUsuario";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@FotoPerfil", (object)base64 ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);
                try
                {
                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }


        public string ObtenerFotoPerfil(int idUsuario)
        {
            string query = "SELECT FotoPerfil FROM Usuario WHERE IdUsuario = @IdUsuario";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);
                try
                {
                    conn.Open();
                    var result = cmd.ExecuteScalar();
                    return (result == null || result == DBNull.Value) ? null : result.ToString();
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }


        //MENSAJERIA
        public int ObtenerOCrearConversacion(int idTurno, int idCliente, int idPrestador)
        {
            string queryBuscar = @"
                                SELECT IdConversacion 
                                FROM Conversacion 
                                WHERE IdTurno = @IdTurno";

            string queryCrear = @"
                                INSERT INTO Conversacion (IdTurno, IdCliente, IdPrestador)
                                VALUES (@IdTurno, @IdCliente, @IdPrestador);
                                SELECT SCOPE_IDENTITY();";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmdBuscar = new SqlCommand(queryBuscar, conn);
                cmdBuscar.Parameters.AddWithValue("@IdTurno", idTurno);
                object resultado = cmdBuscar.ExecuteScalar();

                if (resultado != null)
                    return Convert.ToInt32(resultado);

                SqlCommand cmdCrear = new SqlCommand(queryCrear, conn);
                cmdCrear.Parameters.AddWithValue("@IdTurno", idTurno);
                cmdCrear.Parameters.AddWithValue("@IdCliente", idCliente);
                cmdCrear.Parameters.AddWithValue("@IdPrestador", idPrestador);
                return Convert.ToInt32(cmdCrear.ExecuteScalar());
            }
        }

        public bool EnviarMensaje(int idConversacion, int idEmisor, string texto)
        {
            string query = @"
                            INSERT INTO MensajeInbox (IdConversacion, IdEmisor, Texto)
                            VALUES (@IdConversacion, @IdEmisor, @Texto)";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@IdConversacion", idConversacion);
                cmd.Parameters.AddWithValue("@IdEmisor", idEmisor);
                cmd.Parameters.AddWithValue("@Texto", texto.Trim());
                try
                {
                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
                catch { return false; }
            }
        }

        public DataTable TraerMensajesConversacion(int idConversacion)
        {
            string query = @"
                            SELECT TOP 200
                                M.IdMensaje,
                                M.IdEmisor,
                                M.Texto,
                                M.FechaEnvio,
                                M.Leido,
                                U.Nombre,
                                U.Apellido,
                                U.FotoPerfil
                            FROM MensajeInbox M
                            INNER JOIN Usuario U ON U.IdUsuario = M.IdEmisor
                            WHERE M.IdConversacion = @IdConversacion
                            ORDER BY M.FechaEnvio ASC";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@IdConversacion", idConversacion);
                conn.Open();
                DataTable dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                return dt;
            }
        }

        public DataTable TraerConversacionesUsuario(int idUsuario)
        {
            // Trae todas las conversaciones donde el usuario es cliente O prestador
            string query = @"
                            SELECT
                                C.IdConversacion,
                                C.IdTurno,
                                T.Estado        AS EstadoTurno,
                                S.Nombre        AS Servicio,
                                CASE WHEN CL.IdUsuario = @IdUsuario 
                                     THEN UP.Nombre   ELSE UC.Nombre   END AS OtroNombre,
                                CASE WHEN CL.IdUsuario = @IdUsuario 
                                     THEN UP.Apellido ELSE UC.Apellido END AS OtroApellido,
                                CASE WHEN CL.IdUsuario = @IdUsuario 
                                     THEN UP.FotoPerfil ELSE UC.FotoPerfil END AS OtroFoto,
                                (SELECT TOP 1 Texto 
                                 FROM MensajeInbox 
                                 WHERE IdConversacion = C.IdConversacion 
                                 ORDER BY FechaEnvio DESC)              AS UltimoMensaje,
                                (SELECT TOP 1 FechaEnvio 
                                 FROM MensajeInbox 
                                 WHERE IdConversacion = C.IdConversacion 
                                 ORDER BY FechaEnvio DESC)              AS FechaUltimo,
                                (SELECT COUNT(*) 
                                 FROM MensajeInbox 
                                 WHERE IdConversacion = C.IdConversacion 
                                   AND Leido = 0 
                                   AND IdEmisor <> @IdUsuario)          AS NoLeidos
                            FROM Conversacion C
                            INNER JOIN Turno    T  ON T.IdTurno    = C.IdTurno
                            INNER JOIN Servicios S  ON S.IdServicio = T.IdServicio
                            INNER JOIN Cliente   CL ON CL.IdCliente = C.IdCliente
                            INNER JOIN Prestador PR ON PR.IdPrestador = C.IdPrestador
                            INNER JOIN Usuario   UC ON UC.IdUsuario = CL.IdUsuario
                            INNER JOIN Usuario   UP ON UP.IdUsuario = PR.IdUsuario
                            WHERE (
                                (CL.IdUsuario = @IdUsuario AND ISNULL(C.EliminadoPorCliente, 0) = 0)
                                OR
                                (PR.IdUsuario = @IdUsuario AND ISNULL(C.EliminadoPorPrestador, 0) = 0)
                            )
                            ORDER BY FechaUltimo DESC";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);
                conn.Open();
                DataTable dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                return dt;
            }
        }

        public bool MarcarMensajesLeidos(int idConversacion, int idUsuarioLector)
        {
            string query = @"
                            UPDATE MensajeInbox 
                            SET Leido = 1
                            WHERE IdConversacion = @IdConversacion 
                              AND IdEmisor <> @IdUsuario 
                              AND Leido = 0";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@IdConversacion", idConversacion);
                cmd.Parameters.AddWithValue("@IdUsuario", idUsuarioLector);
                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    return true;
                }
                catch { return false; }
            }
        }

        public int ContarMensajesNoLeidos(int idUsuario)
        {
            string query = @"
                            SELECT COUNT(*) 
                            FROM MensajeInbox M
                            INNER JOIN Conversacion C ON C.IdConversacion = M.IdConversacion
                            INNER JOIN Cliente   CL ON CL.IdCliente   = C.IdCliente
                            INNER JOIN Prestador PR ON PR.IdPrestador = C.IdPrestador
                            WHERE (
                                (CL.IdUsuario = @IdUsuario AND ISNULL(C.EliminadoPorCliente, 0) = 0)
                                OR
                                (PR.IdUsuario = @IdUsuario AND ISNULL(C.EliminadoPorPrestador, 0) = 0)
                            )
                              AND M.IdEmisor <> @IdUsuario
                              AND M.Leido = 0";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);
                conn.Open();
                return (int)cmd.ExecuteScalar();
            }
        }
        public bool EliminarConversacion(int idConversacion, int idUsuario)
        {
            string query = @"
                            UPDATE Conversacion SET
                                EliminadoPorCliente   = CASE WHEN CL.IdUsuario = @IdUsuario THEN 1 ELSE EliminadoPorCliente END,
                                EliminadoPorPrestador = CASE WHEN PR.IdUsuario = @IdUsuario THEN 1 ELSE EliminadoPorPrestador END
                            FROM Conversacion C
                            INNER JOIN Cliente   CL ON CL.IdCliente   = C.IdCliente
                            INNER JOIN Prestador PR ON PR.IdPrestador = C.IdPrestador
                            WHERE C.IdConversacion = @IdConversacion
                                AND (CL.IdUsuario = @IdUsuario OR PR.IdUsuario = @IdUsuario)";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@IdConversacion", idConversacion);
                cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);
                try { conn.Open(); return cmd.ExecuteNonQuery() > 0; }
                catch { return false; }
            }
        }
        public int ObtenerOtroUsuarioDeConversacion(int idConversacion, int idEmisor)
        {
            string query = @"
                            SELECT 
                                CASE WHEN CL.IdUsuario = @IdEmisor THEN PR.IdUsuario ELSE CL.IdUsuario END
                            FROM Conversacion C
                            INNER JOIN Cliente   CL ON CL.IdCliente   = C.IdCliente
                            INNER JOIN Prestador PR ON PR.IdPrestador = C.IdPrestador
                            WHERE C.IdConversacion = @IdConversacion";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@IdConversacion", idConversacion);
                cmd.Parameters.AddWithValue("@IdEmisor", idEmisor);
                conn.Open();
                var result = cmd.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : 0;
            }
        }
        public DataRow TraerPerfilPrestador(int idUsuario)
        {
            string query = @"
                            SELECT
                                U.Nombre, U.Apellido, U.Email, U.Telefono, U.FotoPerfil,
                                P.IdPrestador, P.Descripcion,
                                ZP.IdLocalidad,
                                D.DisponibilidadPrestador
                            FROM Usuario U
                            INNER JOIN Prestador P ON U.IdUsuario = P.IdUsuario
                            LEFT JOIN ZonasPrestador ZP ON ZP.IdPrestador = P.IdPrestador
                            LEFT JOIN Disponibilidad D ON D.IdPrestador = P.IdPrestador
                            WHERE U.IdUsuario = @IdUsuario";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);
                conn.Open();
                DataTable dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                return dt.Rows.Count > 0 ? dt.Rows[0] : null;
            }
        }

        public DataTable TraerServiciosDePrestador(int idPrestador)
        {
            string query = @"
                            SELECT S.IdServicio, S.Nombre AS NombreServicio, PS.PrecioHora AS Precio
                            FROM PrestadorServicio PS
                            INNER JOIN Servicios S ON S.IdServicio = PS.IdServicio
                            WHERE PS.IdPrestador = @IdPrestador";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@IdPrestador", idPrestador);
                conn.Open();
                DataTable dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                return dt;
            }
        }




        public bool AceptarTurnoConFecha(int idTurno, int idPrestador, DateTime fecha)
        {
            string query = @"UPDATE Turno 
                             SET Estado = 'Aceptado', 
                                 FechaProgramada = @Fecha 
                             WHERE IdTurno = @IdTurno AND IdPrestador = @IdPrestador";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.Add("@Fecha", SqlDbType.Date).Value = fecha;
                cmd.Parameters.Add("@IdTurno", SqlDbType.Int).Value = idTurno;
                cmd.Parameters.Add("@IdPrestador", SqlDbType.Int).Value = idPrestador;

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }


        /// Trae todos los turnos aceptados de un prestador en un mes/año dado.
        /// Devuelve IdTurno, FechaProgramada, HoraProgramada, NombreCliente, ApellidoCliente, Servicio.

        public DataTable TraerTurnosPrestadorPorMes(int idPrestador, int anio, int mes)
        {
            string query = @"
        SELECT 
            T.IdTurno, 
            T.FechaProgramada, 
            U.Nombre AS NombreCliente, 
            U.Apellido AS ApellidoCliente, 
            S.Nombre AS Servicio 
        FROM Turno T 
        INNER JOIN Cliente C  ON T.IdCliente   = C.IdCliente 
        INNER JOIN Usuario U  ON C.IdUsuario   = U.IdUsuario 
        INNER JOIN Servicios S ON T.IdServicio = S.IdServicio 
        WHERE T.IdPrestador     = @IdPrestador 
          AND T.Estado          = 'Aceptado' 
          AND T.FechaProgramada IS NOT NULL 
          AND YEAR(T.FechaProgramada)  = @Anio 
          AND MONTH(T.FechaProgramada) = @Mes";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.Add("@IdPrestador", SqlDbType.Int).Value = idPrestador;
                cmd.Parameters.Add("@Anio", SqlDbType.Int).Value = anio;
                cmd.Parameters.Add("@Mes", SqlDbType.Int).Value = mes;

                DataTable dt = new DataTable();
                conn.Open();
                dt.Load(cmd.ExecuteReader());
                return dt;
            }
        }


        /// Trae la disponibilidad del prestador (string con días laborales).

        public string TraerDisponibilidadPrestador(int idPrestador)
        {
            string query = "SELECT DisponibilidadPrestador FROM Disponibilidad WHERE IdPrestador = @IdPrestador";
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@IdPrestador", idPrestador);
                conn.Open();
                var result = cmd.ExecuteScalar();
                return result == null ? "" : result.ToString();
            }
        }


    }
}

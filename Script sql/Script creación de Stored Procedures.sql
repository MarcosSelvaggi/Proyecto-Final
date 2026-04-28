USE Proyecto_Final_Integrador;
GO

CREATE OR ALTER PROCEDURE RegistrarUsuariosNuevos
    @Email NVARCHAR(256),
    @Password NVARCHAR(256),
    @Nombre NVARCHAR(100),
    @Apellido NVARCHAR(100),
    @Telefono NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        -- 1. Insert Usuario
        INSERT INTO Usuario (Email, PasswordHash, Nombre, Apellido, Telefono)
        VALUES (@Email, @Password, @Nombre, @Apellido, @Telefono);

        DECLARE @IdUsuario INT = SCOPE_IDENTITY();

        -- 2. Insert Cliente
        INSERT INTO Cliente (IdUsuario, Provincia, Departamento, Localidad, LocalidadId, Direccion)
        VALUES (@IdUsuario, 'No ingresado', 'No ingresado', 'No ingresado', 'No ingresado', 'No ingresado');

        -- 3. Insert Prestador
        INSERT INTO Prestador (IdUsuario, Descripcion)
        VALUES (@IdUsuario, 'No ingresado');

        DECLARE @IdPrestador INT = SCOPE_IDENTITY();

        -- 4. Insert ZonasPrestador (solo lo válido)
        INSERT INTO ZonasPrestador (IdPrestador, IdLocalidad)
        VALUES (@IdPrestador, 'No ingresado');

        -- 5. Insert Disponibilidad
        INSERT INTO Disponibilidad (IdPrestador, DisponibilidadPrestador)
        VALUES (@IdPrestador, 'Domingos,0,0,0,Lunes,0,0,0,Martes,0,0,0,Miércoles,0,0,0,Jueves,0,0,0,Viernes,0,0,0,Sábados,0,0,0') 
        --El primer 0 es del check, los otros son el horario de inicio y fin en ese orden

        COMMIT TRANSACTION;

        RETURN 1; -- éxito
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;

        -- Opcional: podés ver el error real
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage, 16, 1);

        RETURN 0; -- error
    END CATCH
END;
GO


CREATE OR ALTER PROCEDURE CalificarTurnos
    @IdTurno int,
    @Comentario VARCHAR(200),
    @Calificacion int
AS 
BEGIN 
    BEGIN TRY
        Begin Transaction 
            Declare @IdPrestador int;
            Select @IdPrestador = IdPrestador from Turno where IdTurno = @IdTurno; 

            Declare @IdCliente int;
            Select @IdCliente = IdCliente from Turno where @IdTurno = @IdTurno; 

            Insert into Calificaciones (IdTurno, IdCliente, IdPrestador, Calificacion, Comentario) 
            values (@IdTurno, @IdCliente, @IdPrestador, @Calificacion, @Comentario); 

            -- Para que no chille el SQL, el promedio lo dejo en una variable y después le paso la variable
            Declare @PromedioCalificaciones float;
            Select @PromedioCalificaciones = ROUND(CAST(AVG(Cast(Calificaciones.Calificacion as FLOAT)) AS FLOAT), 2,1)
            from Calificaciones where Calificaciones.IdPrestador = @IdPrestador

            Update Prestador set Calificacion = @PromedioCalificaciones

            Update Turno set Estado = 'Calificado' where IdTurno = @IdTurno

        Commit Transaction 
    END TRY 
    BEGIN CATCH 
        Rollback Transaction 
    END CATCH
END
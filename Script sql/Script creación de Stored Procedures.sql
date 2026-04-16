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
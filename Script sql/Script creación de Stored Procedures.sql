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
    SET NOCOUNT OFF;  -- para que devuelva filas afectadas

    BEGIN TRY
        BEGIN TRANSACTION;
       
        INSERT INTO Usuario (Email, PasswordHash, Nombre, Apellido, Telefono)
        VALUES (@Email, @Password, @Nombre, @Apellido, @Telefono);

        DECLARE @Id INT;
        SET @Id = SCOPE_IDENTITY();
     
        INSERT INTO Cliente (IdUsuario, Provincia, Departamento, Localidad, Direccion)
        VALUES (@Id, 'No ingresado', 'No ingresado', 'No ingresado', 'No ingresado');

        INSERT INTO Prestador (IdUsuario, Descripcion)
        VALUES (@Id, 'No ingresado');

        INSERT INTO ZonasPrestador (IdPrestador, IdLocalidad)
        VALUES (@Id, 'No ingresado');

        INSERT INTO ZonasPrestador (IdPrestador, IdZona)
        VALUES (@Id, 'No ingresado');

        COMMIT TRANSACTION;
      
        SELECT 1 AS FilasAfectadas;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        
        SELECT 0 AS FilasAfectadas;
    END CATCH
END;
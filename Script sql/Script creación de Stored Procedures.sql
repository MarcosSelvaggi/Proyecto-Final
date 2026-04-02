use Proyecto_Final_Integrador 
go

Create or alter Procedure RegistrarUsuariosNuevos
	@Email nvarchar(256),
	@Password nvarchar(256),
	@Nombre nvarchar(100),
	@Apellido nvarchar(100),
	@Telefono nvarchar(20)
AS
BEGIN 
	Insert into Usuario (Email, PasswordHash, Nombre, Apellido, Telefono)
	values (@Email, @Password, @Nombre, @Apellido, @Telefono);
	
	Declare @Id as int; 
	Set @Id = SCOPE_IDENTITY();

	Insert into Cliente (IdUsuario, Provincia, Departamento, Localidad, Direccion) values (@Id, 'No ingresado', 'No ingresado','No ingresado', 'No ingresado');
	Insert into Prestador (IdUsuario, Descripcion) values (@Id, 'No ingresado'); 
	Insert into ZonasPrestador(IdPrestador, IdLocalidad) values (@Id, '');
	insert into ZonasPrestador(IdPrestador, IdZona) values (@Id,'No ingresado')
END; 
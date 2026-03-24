use master 
go 

if not exists (select * from sys.databases where name = 'Proyecto_Final_Integrador')
begin
create database Proyecto_Final_Integrador;
end 
go

use Proyecto_Final_Integrador 
go

create table MetodosPago(
	IdMetodoPago int primary key identity(1,1) not null,
	NombreMetodoPago nvarchar(100) not null
);

create table Usuario(
	IdUsuario int primary key identity(1,1) not null, 
	Email nvarchar(256) not null unique, 
	PasswordHash nvarchar (256) not null, 
	FechaRegistro datetime not null default getdate(),
	Activo bit not null default 1, 
	Nombre nvarchar(100) not null,
	Apellido nvarchar (100) not null,
	Telefono nvarchar (20) not null
);

create table Cliente(
	IdCliente int primary key identity(1,1) not null,                        
	IdUsuario int foreign key references Usuario(IdUsuario) not null,
	Provincia nvarchar (100) not null,
	Localidad nvarchar (100) not null,
	Direccion nvarchar(100) not null
);

create table Prestador(
	IdPrestador int primary key identity(1,1) not null, 
	IdUsuario int foreign key references Usuario(IdUsuario) not null,
	Descripcion nvarchar(400) not null,
);

create table Servicios(
	IdServicio int primary key identity(1,1) not null, 
	Nombre nvarchar(100) not null, 
	Descripcion nvarchar(400) not null
);

create table PrestadorServicio(
	IdPrestadorServicio int primary key identity(1,1) not null,
	IdPrestador int foreign key references Prestador(IdPrestador) not null,
	IdServicio int foreign key references Servicios(IdServicio) not null,
	PrecioHora money not null check (PrecioHora > 0)
); 

create table PrestadorMetodoPago(
	IdPrestadorMetodoPago int primary key identity(1,1) not null, 
	IdPrestador int foreign key references Prestador(IdPrestador) not null, 
	IdMetodoPago int foreign key references MetodosPago(IdMetodoPago) not null
); 

create table Disponibilidad(
	IdDisponibilidad int primary key identity(1,1) not null, 
	IdPrestador int foreign key references Prestador(IdPrestador) not null,
	DiaSemana int not null check (DiaSemana BETWEEN 1 AND 7),
	HoraInicio time not null,
	HoraFin time not null,
);

create table Turno(
	IdTurno int primary key identity(1,1) not null, 
	IdCliente int foreign key references Cliente(IdCliente) not null, 
	IdPrestador int foreign key references Prestador(IdPrestador) not null, 
	FechaHoraInicio datetime not null,
	FechaHoraFin datetime not null,
);

create table ZonasPrestador(
	IdZona int primary key identity(1,1) not null, 
	IdPrestador int foreign key references Prestador(IdPrestador) not null,
	IdLocalidad varchar(4086) not null
)


GO 
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

	Insert into Cliente (IdUsuario, Provincia, Localidad, Direccion) values (@Id, 'No ingresado', 'No ingresado', 'No ingresado');
	Insert into Prestador (IdUsuario, Descripcion) values (@Id, 'No ingresado'); 
	Insert into ZonasPrestador(IdPrestador, IdLocalidad) values (@Id, '');
END; 


	select U.Nombre, U.Apellido, U.Telefono, U.Activo, P.Descripcion, C.Direccion, C.Localidad, C.Provincia, U.Email
	from Usuario U inner join Prestador P on U.IdUsuario = P.IdUsuario
	inner join Cliente C on U.IdUsuario = C.IdUsuario where U.Email like '%@mail.com'
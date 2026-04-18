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
	Departamento nvarchar(100) not null,
	Localidad nvarchar (100) not null,
	LocalidadId nvarchar(100) not null,
	Direccion nvarchar(100) not null
);

create table Prestador(
	IdPrestador int primary key identity(1,1) not null, 
	IdUsuario int foreign key references Usuario(IdUsuario) not null,
	Descripcion nvarchar(400) not null,
);

create table Servicios(
	IdServicio int primary key identity(1,1) not null, 
	Nombre nvarchar(100) unique not null, 
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
	DisponibilidadPrestador varchar(300) not null
);

create table Turno(
	IdTurno int primary key identity(1,1) not null, 
	IdCliente int foreign key references Cliente(IdCliente) not null, 
	IdPrestador int foreign key references Prestador(IdPrestador) not null, 
	IdServicio int foreign key references Servicios(IdServicio) not null,
	Mensaje nvarchar(500) null,
	FechaSolicitud datetime default getdate()
);

create table ZonasPrestador(
	IdZona int primary key identity(1,1) not null, 
	IdPrestador int foreign key references Prestador(IdPrestador) not null,
	IdLocalidad varchar(4086) not null
)


GO 

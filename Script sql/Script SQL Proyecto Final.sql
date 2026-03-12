use master 
go 

if not exists (select * from sys.databases where name = 'Proyecto_Final_Integrador')
begin
create database Proyecto_Final_Integrador;
end 
go

use Proyecto_Final_Integrador 
go

create table Zona(
	IdZona int primary key identity(1,1) not null,
	NombreZona nvarchar(100) not null
);

create table MetodosPago(
	IdMetodoPago int primary key identity(1,1) not null,
	NombreMetodoPago nvarchar(100) not null
);

create table Usuario(
	IdUsuario int primary key identity(1,1) not null, 
	Email nvarchar(256) not null, 
	PasswordHash nvarchar (256) not null, 
	FechaRegistro datetime not null,
	Activo bit not null, 
	Nombre nvarchar(100) not null,
	Apellido nvarchar (100) not null,
	Telefono nvarchar (20) not null
);

create table Cliente(
	IdCliente int primary key identity(1,1) not null,                        
	IdUsuario int foreign key references Usuario(IdUsuario) not null,
	Direccion nvarchar(100) not null
);

create table Prestador(
	IdPrestador int primary key identity(1,1) not null, 
	IdUsuario int foreign key references Usuario(IdUsuario) not null,
	Descripcion nvarchar(400) not null,
	IdZona int foreign key references Zona(IdZona) not null
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
	PrecioHora money not null
); 

create table PrestadorMetodoPago(
	IdPrestadorMetodoPago int primary key identity(1,1) not null, 
	IdPrestador int foreign key references Prestador(IdPrestador) not null, 
	IdMetodoPago int foreign key references MetodosPago(IdMetodoPago) not null
); 

create table Disponibilidad(
	IdDisponibilidad int primary key identity(1,1) not null, 
	IdPrestador int foreign key references Prestador(IdPrestador) not null,
	Descripcion nvarchar(400) not null
);

create table turnos(
	IdTurno int primary key identity(1,1) not null, 
	IdCliente int foreign key references Cliente(IdCliente) not null, 
	IdPrestador int foreign key references Prestador(IdPrestador) not null, 
	DiaTurno datetime not null
);

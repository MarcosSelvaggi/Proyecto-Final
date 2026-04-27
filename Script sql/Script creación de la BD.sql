USE master;
GO

IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'Proyecto_Final_Integrador')
BEGIN
    CREATE DATABASE Proyecto_Final_Integrador;
END
GO
 
USE Proyecto_Final_Integrador;
GO

 
CREATE TABLE Usuario (
    IdUsuario     INT           PRIMARY KEY IDENTITY(1,1) NOT NULL,
    Email         NVARCHAR(256) NOT NULL UNIQUE,
    PasswordHash  NVARCHAR(256) NOT NULL,
    FechaRegistro DATETIME      NOT NULL DEFAULT GETDATE(),
    Activo        BIT           NOT NULL DEFAULT 1,
    Nombre        NVARCHAR(100) NOT NULL,
    Apellido      NVARCHAR(100) NOT NULL,
    Telefono      NVARCHAR(20)  NOT NULL,
    FotoPerfil    NVARCHAR(MAX) NULL          
);
 
CREATE TABLE Cliente (
    IdCliente    INT           PRIMARY KEY IDENTITY(1,1) NOT NULL,
    IdUsuario    INT           NOT NULL REFERENCES Usuario(IdUsuario),
    Provincia    NVARCHAR(100) NOT NULL,
    Departamento NVARCHAR(100) NOT NULL,
    Localidad    NVARCHAR(100) NOT NULL,
    LocalidadId  NVARCHAR(100) NOT NULL,
    Direccion    NVARCHAR(100) NOT NULL
);
 
CREATE TABLE Prestador (
    IdPrestador INT           PRIMARY KEY IDENTITY(1,1) NOT NULL,
    IdUsuario   INT           NOT NULL REFERENCES Usuario(IdUsuario),
    Descripcion NVARCHAR(400) NOT NULL, 
    Calificacion float default 0 not null
);
 
CREATE TABLE Servicios (
    IdServicio  INT           PRIMARY KEY IDENTITY(1,1) NOT NULL,
    Nombre      NVARCHAR(100) NOT NULL UNIQUE,
    Descripcion NVARCHAR(400) NOT NULL
);
 
CREATE TABLE PrestadorServicio (
    IdPrestadorServicio INT   PRIMARY KEY IDENTITY(1,1) NOT NULL,
    IdPrestador         INT   NOT NULL REFERENCES Prestador(IdPrestador),
    IdServicio          INT   NOT NULL REFERENCES Servicios(IdServicio),
    PrecioHora          MONEY NOT NULL CHECK (PrecioHora > 0)
);
 
CREATE TABLE Disponibilidad (
    IdDisponibilidad       INT          PRIMARY KEY IDENTITY(1,1) NOT NULL,
    IdPrestador            INT          NOT NULL REFERENCES Prestador(IdPrestador),
    DisponibilidadPrestador VARCHAR(300) NOT NULL
);
 
CREATE TABLE Turno (
    IdTurno       INT           PRIMARY KEY IDENTITY(1,1) NOT NULL,
    IdCliente     INT           NOT NULL REFERENCES Cliente(IdCliente),
    IdPrestador   INT           NOT NULL REFERENCES Prestador(IdPrestador),
    IdServicio    INT           NOT NULL REFERENCES Servicios(IdServicio),
    Mensaje       NVARCHAR(500) NULL,
    FechaSolicitud DATETIME     DEFAULT GETDATE(),
    FechaProgramada DATE NULL,
    Estado        NVARCHAR(50)  DEFAULT 'Pendiente'
);
 
CREATE TABLE ZonasPrestador (
    IdZona      INT          PRIMARY KEY IDENTITY(1,1) NOT NULL,
    IdPrestador INT          NOT NULL REFERENCES Prestador(IdPrestador),
    IdLocalidad VARCHAR(4086) NOT NULL
);
 
CREATE TABLE Calificaciones (
    IdCalificacion INT          PRIMARY KEY IDENTITY(1,1) NOT NULL,
    IdTurno        INT          NOT NULL REFERENCES Turno(IdTurno),
    IdCliente      INT          NOT NULL REFERENCES Cliente(IdCliente),
    IdPrestador    INT          NOT NULL REFERENCES Prestador(IdPrestador),
    Calificacion   FLOAT        NOT NULL,
    Comentario     VARCHAR(400) NULL
);

CREATE TABLE Conversacion (
    IdConversacion  INT      PRIMARY KEY IDENTITY(1,1) NOT NULL,
    IdTurno         INT      NOT NULL REFERENCES Turno(IdTurno),
    IdCliente       INT      NOT NULL REFERENCES Cliente(IdCliente),
    IdPrestador     INT      NOT NULL REFERENCES Prestador(IdPrestador),
    EliminadoPorCliente BIT NOT NULL DEFAULT 0,
    EliminadoPorPrestador BIT NOT NULL DEFAULT 0,
    FechaCreacion   DATETIME NOT NULL DEFAULT GETDATE()
);

CREATE TABLE MensajeInbox (
    IdMensaje       INT            PRIMARY KEY IDENTITY(1,1) NOT NULL,
    IdConversacion  INT            NOT NULL REFERENCES Conversacion(IdConversacion),
    IdEmisor        INT            NOT NULL REFERENCES Usuario(IdUsuario),
    Texto           NVARCHAR(1000) NOT NULL,
    FechaEnvio      DATETIME       NOT NULL DEFAULT GETDATE(),
    Leido           BIT            NOT NULL DEFAULT 0
);
GO

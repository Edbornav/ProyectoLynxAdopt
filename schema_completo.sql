-- =============================================
-- SCHEMA COMPLETO CON STORED PROCEDURES
-- SUPABASE / POSTGRESQL
-- =============================================


-- -------------------------------------------
-- TABLAS
-- -------------------------------------------

CREATE TABLE Especie (
    EspecieID INT          GENERATED ALWAYS AS IDENTITY PRIMARY KEY NOT NULL,
    Nombre    VARCHAR(50)  NOT NULL  
);

CREATE TABLE Raza (
    RazaID    INT          GENERATED ALWAYS AS IDENTITY PRIMARY KEY NOT NULL,
    EspecieID INT          NOT NULL,
    Nombre    VARCHAR(50)  NOT NULL,
    FOREIGN KEY (EspecieID) REFERENCES Especie(EspecieID)
);

CREATE TABLE Usuarios (
    UsuarioID     INT           GENERATED ALWAYS AS IDENTITY PRIMARY KEY NOT NULL,
    Correo        VARCHAR(50)   NOT NULL,
    PasswordHash  VARCHAR(255)  NOT NULL,
    TipoUsuario   VARCHAR(50)   NOT NULL CHECK (TipoUsuario IN ('Adoptante', 'Administrador')),
    Estatus       VARCHAR(50)   NOT NULL CHECK (Estatus IN ('Activo', 'Inactivo')),
    FechaRegistro DATE
);

CREATE TABLE Adoptante (
    AdoptanteID     INT         GENERATED ALWAYS AS IDENTITY PRIMARY KEY NOT NULL,
    UsuarioID       INT         NOT NULL,
    Nombre          VARCHAR(50) NOT NULL,
    ApellidoPaterno VARCHAR(50) NOT NULL,
    ApellidoMaterno VARCHAR(50) NOT NULL,
    Telefono        VARCHAR(10) NOT NULL,
    FechaNacimiento DATE,
    FOREIGN KEY (UsuarioID) REFERENCES Usuarios(UsuarioID)
);

CREATE TABLE PerfilAdoptante (
    PerfilAdoptanteID                 INT          GENERATED ALWAYS AS IDENTITY PRIMARY KEY NOT NULL,
    AdoptanteUsuarioID                INT          NOT NULL,
    DescripcionCasa                   VARCHAR(200) NOT NULL,
    DescripcionMascotas               VARCHAR(100) NOT NULL,
    DescripcionExperienciaConMascotas VARCHAR(100) NOT NULL,
    FOREIGN KEY (AdoptanteUsuarioID) REFERENCES Adoptante(AdoptanteID)
);

CREATE TABLE Administrador (
    AdministradorID INT         GENERATED ALWAYS AS IDENTITY PRIMARY KEY NOT NULL,
    UsuarioID       INT         NOT NULL,
    Nombre          VARCHAR(50) NOT NULL,
    ApellidoPaterno VARCHAR(50) NOT NULL,
    ApellidoMaterno VARCHAR(50) NOT NULL,
    Telefono        VARCHAR(10) NOT NULL,
    FOREIGN KEY (UsuarioID) REFERENCES Usuarios(UsuarioID)
);

CREATE TABLE Refugio (
    RefugioID       INT          GENERATED ALWAYS AS IDENTITY PRIMARY KEY NOT NULL,
    Nombre          VARCHAR(50)  NOT NULL,
    Descripcion     VARCHAR(150) NOT NULL,
    Direccion       VARCHAR(50)  NOT NULL,
    Telefono        VARCHAR(10)  NOT NULL,
    Correo          VARCHAR(50)  NOT NULL,
    Estatus         VARCHAR(20)  NOT NULL CHECK (Estatus IN ('Activo', 'Inactivo')),
    FechaDeRegistro DATE         NOT NULL
);

CREATE TABLE RefugioAdministradores (
    RefugioID       INT  NOT NULL,
    UsuarioAdminID  INT  NOT NULL,
    AsignacionFecha DATE NOT NULL,
    PRIMARY KEY (RefugioID, UsuarioAdminID),
    FOREIGN KEY (RefugioID)      REFERENCES Refugio(RefugioID),
    FOREIGN KEY (UsuarioAdminID) REFERENCES Administrador(AdministradorID)
);

CREATE TABLE Animales (
    AnimalID        INT          GENERATED ALWAYS AS IDENTITY PRIMARY KEY NOT NULL,
    RefugioID       INT          NOT NULL,
    RazaID          INT          NOT NULL,
    Nombre          VARCHAR(50)  NOT NULL,
    Sexo            VARCHAR(20)  NOT NULL CHECK (Sexo IN ('Macho', 'Hembra')),
    FechaNacimiento DATE,
    Descripcion     VARCHAR(150) NOT NULL,
    Estatus         VARCHAR(20)  NOT NULL CHECK (Estatus IN ('Disponible', 'En Proceso', 'Adoptado')),
    FechaRegistro   DATE,
    FOREIGN KEY (RefugioID) REFERENCES Refugio(RefugioID),
    FOREIGN KEY (RazaID)    REFERENCES Raza(RazaID)
);

CREATE TABLE SolicitudAdopcion (
    SolicitudID      INT          GENERATED ALWAYS AS IDENTITY PRIMARY KEY NOT NULL,
    RefugioID        INT          NOT NULL,
    AdoptanteID      INT          NOT NULL,
    MensajeAdoptante VARCHAR(150) NOT NULL,
    Estatus          VARCHAR(20)  NOT NULL DEFAULT 'Pendiente' CHECK (Estatus IN ('Pendiente', 'Aprobada', 'Rechazada')),
    FechaRegistro    DATE         NOT NULL DEFAULT CURRENT_DATE,
    FOREIGN KEY (RefugioID)   REFERENCES Refugio(RefugioID),
    FOREIGN KEY (AdoptanteID) REFERENCES Adoptante(AdoptanteID)
);

CREATE TABLE SolicitudAnimales (
    SolicitudID INT NOT NULL,
    AnimalID    INT NOT NULL,
    PRIMARY KEY (SolicitudID, AnimalID),
    FOREIGN KEY (SolicitudID) REFERENCES SolicitudAdopcion(SolicitudID),
    FOREIGN KEY (AnimalID)    REFERENCES Animales(AnimalID)
);

CREATE TABLE Citas (
    CitaID        INT         GENERATED ALWAYS AS IDENTITY PRIMARY KEY NOT NULL,
    SolicitudID   INT         NOT NULL,
    FechaHoraCita TIMESTAMP   NOT NULL,
    EstadoCita    VARCHAR(20) NOT NULL CHECK (EstadoCita IN ('Pendiente', 'Confirmada', 'Cancelada', 'Realizada')),
    FOREIGN KEY (SolicitudID) REFERENCES SolicitudAdopcion(SolicitudID)
);


-- -------------------------------------------
-- SP: ESPECIE
-- -------------------------------------------

CREATE OR REPLACE FUNCTION sp_get_especies()
RETURNS TABLE (EspecieID INT, Nombre VARCHAR)
LANGUAGE plpgsql AS $$
BEGIN
    RETURN QUERY SELECT e.EspecieID, e.Nombre FROM Especie e;
END; $$;

CREATE OR REPLACE FUNCTION sp_get_especie_by_id(p_id INT)
RETURNS TABLE (EspecieID INT, Nombre VARCHAR)
LANGUAGE plpgsql AS $$
BEGIN
    RETURN QUERY SELECT e.EspecieID, e.Nombre FROM Especie e WHERE e.EspecieID = p_id;
END; $$;

CREATE OR REPLACE FUNCTION sp_insert_especie(p_nombre VARCHAR)
RETURNS VOID
LANGUAGE plpgsql AS $$
BEGIN
    INSERT INTO Especie (Nombre) VALUES (p_nombre);
END; $$;

CREATE OR REPLACE FUNCTION sp_update_especie(p_id INT, p_nombre VARCHAR)
RETURNS VOID
LANGUAGE plpgsql AS $$
BEGIN
    UPDATE Especie SET Nombre = p_nombre WHERE EspecieID = p_id;
END; $$;


-- -------------------------------------------
-- SP: RAZA
-- -------------------------------------------

CREATE OR REPLACE FUNCTION sp_get_razas()
RETURNS TABLE (RazaID INT, EspecieID INT, Nombre VARCHAR)
LANGUAGE plpgsql AS $$
BEGIN
    RETURN QUERY SELECT r.RazaID, r.EspecieID, r.Nombre FROM Raza r;
END; $$;

CREATE OR REPLACE FUNCTION sp_get_raza_by_id(p_id INT)
RETURNS TABLE (RazaID INT, EspecieID INT, Nombre VARCHAR)
LANGUAGE plpgsql AS $$
BEGIN
    RETURN QUERY SELECT r.RazaID, r.EspecieID, r.Nombre FROM Raza r WHERE r.RazaID = p_id;
END; $$;

CREATE OR REPLACE FUNCTION sp_get_razas_by_especie(p_especieid INT)
RETURNS TABLE (RazaID INT, EspecieID INT, Nombre VARCHAR)
LANGUAGE plpgsql AS $$
BEGIN
    RETURN QUERY SELECT r.RazaID, r.EspecieID, r.Nombre FROM Raza r WHERE r.EspecieID = p_especieid;
END; $$;

CREATE OR REPLACE FUNCTION sp_insert_raza(p_especieid INT, p_nombre VARCHAR)
RETURNS VOID
LANGUAGE plpgsql AS $$
BEGIN
    INSERT INTO Raza (EspecieID, Nombre) VALUES (p_especieid, p_nombre);
END; $$;

CREATE OR REPLACE FUNCTION sp_update_raza(p_id INT, p_especieid INT, p_nombre VARCHAR)
RETURNS VOID
LANGUAGE plpgsql AS $$
BEGIN
    UPDATE Raza SET EspecieID = p_especieid, Nombre = p_nombre WHERE RazaID = p_id;
END; $$;


-- -------------------------------------------
-- SP: USUARIOS
-- -------------------------------------------

CREATE OR REPLACE FUNCTION sp_get_usuarios()
RETURNS TABLE (UsuarioID INT, Correo VARCHAR, TipoUsuario VARCHAR, Estatus VARCHAR, FechaRegistro DATE)
LANGUAGE plpgsql AS $$
BEGIN
    RETURN QUERY SELECT u.UsuarioID, u.Correo, u.TipoUsuario, u.Estatus, u.FechaRegistro FROM Usuarios u;
END; $$;

CREATE OR REPLACE FUNCTION sp_get_usuario_by_id(p_id INT)
RETURNS TABLE (UsuarioID INT, Correo VARCHAR, TipoUsuario VARCHAR, Estatus VARCHAR, FechaRegistro DATE)
LANGUAGE plpgsql AS $$
BEGIN
    RETURN QUERY SELECT u.UsuarioID, u.Correo, u.TipoUsuario, u.Estatus, u.FechaRegistro FROM Usuarios u WHERE u.UsuarioID = p_id;
END; $$;

CREATE OR REPLACE FUNCTION sp_insert_usuario(
    p_correo       VARCHAR,
    p_passwordhash VARCHAR,
    p_tipousuario  VARCHAR,
    p_estatus      VARCHAR
)
RETURNS integer
LANGUAGE plpgsql 
AS $$
DECLARE
    p_id integer;
BEGIN
    INSERT INTO Usuarios (Correo, PasswordHash, TipoUsuario, Estatus, FechaRegistro)
    VALUES (p_correo, p_passwordhash, p_tipousuario, p_estatus, CURRENT_DATE)
    returning UsuarioID into p_id;

    RETURN p_id;
END; $$;

CREATE OR REPLACE FUNCTION sp_update_usuario(
    p_id          INT,
    p_correo      VARCHAR,
    p_tipousuario VARCHAR,
    p_estatus     VARCHAR
)
RETURNS VOID
LANGUAGE plpgsql AS $$
BEGIN
    UPDATE Usuarios
    SET Correo = p_correo, TipoUsuario = p_tipousuario, Estatus = p_estatus
    WHERE UsuarioID = p_id;
END; $$;

CREATE OR REPLACE FUNCTION sp_desactivar_usuario(p_id INT)
RETURNS VOID
LANGUAGE plpgsql AS $$
BEGIN
    UPDATE Usuarios SET Estatus = 'Inactivo' WHERE UsuarioID = p_id;
END; $$;

CREATE OR REPLACE FUNCTION sp_login(p_correo VARCHAR)
RETURNS TABLE (UsuarioID INT, Correo VARCHAR, PasswordHash VARCHAR, TipoUsuario VARCHAR, Estatus VARCHAR)
LANGUAGE plpgsql AS $$
BEGIN
    RETURN QUERY
    SELECT u.UsuarioID, u.Correo, u.PasswordHash, u.TipoUsuario, u.Estatus
    FROM Usuarios u WHERE u.Correo = p_correo AND u.Estatus = 'Activo';
END; $$;


-- -------------------------------------------
-- SP: ADOPTANTE
-- -------------------------------------------

CREATE OR REPLACE FUNCTION sp_get_adoptantes()
RETURNS TABLE (
    AdoptanteID INT, UsuarioID INT, Nombre VARCHAR,
    ApellidoPaterno VARCHAR, ApellidoMaterno VARCHAR,
    Telefono VARCHAR, FechaNacimiento DATE
)
LANGUAGE plpgsql AS $$
BEGIN
    RETURN QUERY
    SELECT a.AdoptanteID, a.UsuarioID, a.Nombre,
           a.ApellidoPaterno, a.ApellidoMaterno,
           a.Telefono, a.FechaNacimiento
    FROM Adoptante a;
END; $$;

CREATE OR REPLACE FUNCTION sp_get_adoptante_by_id(p_id INT)
RETURNS TABLE (
    AdoptanteID INT, UsuarioID INT, Nombre VARCHAR,
    ApellidoPaterno VARCHAR, ApellidoMaterno VARCHAR,
    Telefono VARCHAR, FechaNacimiento DATE
)
LANGUAGE plpgsql AS $$
BEGIN
    RETURN QUERY
    SELECT a.AdoptanteID, a.UsuarioID, a.Nombre,
           a.ApellidoPaterno, a.ApellidoMaterno,
           a.Telefono, a.FechaNacimiento
    FROM Adoptante a WHERE a.AdoptanteID = p_id;
END; $$;

CREATE OR REPLACE FUNCTION sp_insert_adoptante(
    p_usuarioid       INT,
    p_nombre          VARCHAR,
    p_apellidopaterno VARCHAR,
    p_apellidomaterno VARCHAR,
    p_telefono        VARCHAR,
    p_fechanacimiento DATE
)
RETURNS integer
LANGUAGE plpgsql 
AS $$
DECLARE
    p_id integer;
BEGIN
    INSERT INTO Adoptante (UsuarioID, Nombre, ApellidoPaterno, ApellidoMaterno, Telefono, FechaNacimiento)
    VALUES (p_usuarioid, p_nombre, p_apellidopaterno, p_apellidomaterno, p_telefono, p_fechanacimiento)
    returning AdoptanteID into p_id;

    RETURN p_id;
END; $$;

CREATE OR REPLACE FUNCTION sp_update_adoptante(
    p_id              INT,
    p_nombre          VARCHAR,
    p_apellidopaterno VARCHAR,
    p_apellidomaterno VARCHAR,
    p_telefono        VARCHAR,
    p_fechanacimiento DATE
)
RETURNS VOID
LANGUAGE plpgsql AS $$
BEGIN
    UPDATE Adoptante
    SET Nombre = p_nombre, ApellidoPaterno = p_apellidopaterno,
        ApellidoMaterno = p_apellidomaterno, Telefono = p_telefono,
        FechaNacimiento = p_fechanacimiento
    WHERE AdoptanteID = p_id;
END; $$;

CREATE OR REPLACE FUNCTION sp_desactivar_adoptante(p_id INT)
RETURNS VOID
LANGUAGE plpgsql AS $$
BEGIN
    UPDATE Usuarios u
    SET Estatus = 'Inactivo'
    FROM Adoptante a
    WHERE a.AdoptanteID = p_id AND u.UsuarioID = a.UsuarioID;
END; $$;


-- -------------------------------------------
-- SP: PERFIL ADOPTANTE
-- -------------------------------------------

CREATE OR REPLACE FUNCTION sp_get_perfiles_adoptante()
RETURNS TABLE (
    PerfilAdoptanteID INT, AdoptanteUsuarioID INT,
    DescripcionCasa VARCHAR, DescripcionMascotas VARCHAR,
    DescripcionExperienciaConMascotas VARCHAR
)
LANGUAGE plpgsql AS $$
BEGIN
    RETURN QUERY
    SELECT p.PerfilAdoptanteID, p.AdoptanteUsuarioID, p.DescripcionCasa,
           p.DescripcionMascotas, p.DescripcionExperienciaConMascotas
    FROM PerfilAdoptante p;
END; $$;

CREATE OR REPLACE FUNCTION sp_get_perfil_adoptante_by_id(p_id INT)
RETURNS TABLE (
    PerfilAdoptanteID INT, AdoptanteUsuarioID INT,
    DescripcionCasa VARCHAR, DescripcionMascotas VARCHAR,
    DescripcionExperienciaConMascotas VARCHAR
)
LANGUAGE plpgsql AS $$
BEGIN
    RETURN QUERY
    SELECT p.PerfilAdoptanteID, p.AdoptanteUsuarioID, p.DescripcionCasa,
           p.DescripcionMascotas, p.DescripcionExperienciaConMascotas
    FROM PerfilAdoptante p WHERE p.AdoptanteUsuarioID = p_id;
END; $$;

CREATE OR REPLACE FUNCTION sp_insert_perfil_adoptante(
    p_adoptanteid                       INT,
    p_descripcioncasa                   VARCHAR,
    p_descripcionmascotas               VARCHAR,
    p_descripcionexperienciaconmascotas VARCHAR
)
RETURNS VOID
LANGUAGE plpgsql AS $$
BEGIN
    INSERT INTO PerfilAdoptante (AdoptanteUsuarioID, DescripcionCasa, DescripcionMascotas, DescripcionExperienciaConMascotas)
    VALUES (p_adoptanteid, p_descripcioncasa, p_descripcionmascotas, p_descripcionexperienciaconmascotas);
END; $$;

CREATE OR REPLACE FUNCTION sp_update_perfil_adoptante(
    p_id                                INT,
    p_descripcioncasa                   VARCHAR,
    p_descripcionmascotas               VARCHAR,
    p_descripcionexperienciaconmascotas VARCHAR
)
RETURNS VOID
LANGUAGE plpgsql AS $$
BEGIN
    UPDATE PerfilAdoptante
    SET DescripcionCasa = p_descripcioncasa,
        DescripcionMascotas = p_descripcionmascotas,
        DescripcionExperienciaConMascotas = p_descripcionexperienciaconmascotas
    WHERE PerfilAdoptanteID = p_id;
END; $$;


-- -------------------------------------------
-- SP: ADMINISTRADOR
-- -------------------------------------------

CREATE OR REPLACE FUNCTION sp_get_administradores()
RETURNS TABLE (
    AdministradorID INT, UsuarioID INT, Nombre VARCHAR,
    ApellidoPaterno VARCHAR, ApellidoMaterno VARCHAR,
    Telefono VARCHAR
)
LANGUAGE plpgsql AS $$
BEGIN
    RETURN QUERY
    SELECT a.AdministradorID, a.UsuarioID, a.Nombre,
           a.ApellidoPaterno, a.ApellidoMaterno,
           a.Telefono
    FROM Administrador a;
END; $$;

CREATE OR REPLACE FUNCTION sp_get_administrador_by_id(p_id INT)
RETURNS TABLE (
    AdministradorID INT, UsuarioID INT, Nombre VARCHAR,
    ApellidoPaterno VARCHAR, ApellidoMaterno VARCHAR,
    Telefono VARCHAR
)
LANGUAGE plpgsql AS $$
BEGIN
    RETURN QUERY
    SELECT a.AdministradorID, a.UsuarioID, a.Nombre,
           a.ApellidoPaterno, a.ApellidoMaterno,
           a.Telefono
    FROM Administrador a WHERE a.AdministradorID = p_id;
END; $$;

CREATE OR REPLACE FUNCTION sp_insert_administrador(
    p_usuarioid       INT,
    p_nombre          VARCHAR,
    p_apellidopaterno VARCHAR,
    p_apellidomaterno VARCHAR,
    p_telefono        VARCHAR
)
RETURNS VOID
LANGUAGE plpgsql AS $$
BEGIN
    INSERT INTO Administrador (UsuarioID, Nombre, ApellidoPaterno, ApellidoMaterno, Telefono)
    VALUES (p_usuarioid, p_nombre, p_apellidopaterno, p_apellidomaterno, p_telefono);
END; $$;

CREATE OR REPLACE FUNCTION sp_update_administrador(
    p_id              INT,
    p_nombre          VARCHAR,
    p_apellidopaterno VARCHAR,
    p_apellidomaterno VARCHAR,
    p_telefono        VARCHAR
)
RETURNS VOID
LANGUAGE plpgsql AS $$
BEGIN
    UPDATE Administrador
    SET Nombre = p_nombre, ApellidoPaterno = p_apellidopaterno,
        ApellidoMaterno = p_apellidomaterno, Telefono = p_telefono
    WHERE AdministradorID = p_id;
END; $$;

CREATE OR REPLACE FUNCTION sp_desactivar_administrador(p_id INT)
RETURNS VOID
LANGUAGE plpgsql AS $$
BEGIN
    UPDATE Usuarios u
    SET Estatus = 'Inactivo'
    FROM Administrador a
    WHERE a.AdministradorID = p_id AND u.UsuarioID = a.UsuarioID;
END; $$;


-- -------------------------------------------
-- SP: REFUGIO
-- -------------------------------------------

CREATE OR REPLACE FUNCTION sp_get_refugios()
RETURNS TABLE (
    RefugioID INT, Nombre VARCHAR, Descripcion VARCHAR,
    Direccion VARCHAR, Telefono VARCHAR, Correo VARCHAR,
    Estatus VARCHAR, FechaDeRegistro DATE
)
LANGUAGE plpgsql AS $$
BEGIN
    RETURN QUERY
    SELECT r.RefugioID, r.Nombre, r.Descripcion, r.Direccion,
           r.Telefono, r.Correo, r.Estatus, r.FechaDeRegistro
    FROM Refugio r;
END; $$;

CREATE OR REPLACE FUNCTION sp_get_refugio_by_id(p_id INT)
RETURNS TABLE (
    RefugioID INT, Nombre VARCHAR, Descripcion VARCHAR,
    Direccion VARCHAR, Telefono VARCHAR, Correo VARCHAR,
    Estatus VARCHAR, FechaDeRegistro DATE
)
LANGUAGE plpgsql AS $$
BEGIN
    RETURN QUERY
    SELECT r.RefugioID, r.Nombre, r.Descripcion, r.Direccion,
           r.Telefono, r.Correo, r.Estatus, r.FechaDeRegistro
    FROM Refugio r WHERE r.RefugioID = p_id;
END; $$;


CREATE OR REPLACE FUNCTION sp_insert_refugio(
    p_nombre      VARCHAR,
    p_descripcion VARCHAR,
    p_direccion   VARCHAR,
    p_telefono    VARCHAR,
    p_correo      VARCHAR,
    p_estatus     VARCHAR
)
RETURNS INTEGER 
Declare
    p_id integer;
LANGUAGE plpgsql 
AS $$
BEGIN
    INSERT INTO Refugio (Nombre, Descripcion, Direccion, Telefono, Correo, Estatus, FechaDeRegistro)
    VALUES (p_nombre, p_descripcion, p_direccion, p_telefono, p_correo, p_estatus, CURRENT_DATE)
    returning RefugioID into p_id;

    return p_id;
END; $$;

CREATE OR REPLACE FUNCTION sp_update_refugio(
    p_id          INT,
    p_nombre      VARCHAR,
    p_descripcion VARCHAR,
    p_direccion   VARCHAR,
    p_telefono    VARCHAR,
    p_correo      VARCHAR,
    p_estatus     VARCHAR
)
RETURNS VOID
LANGUAGE plpgsql AS $$
BEGIN
    UPDATE Refugio
    SET Nombre = p_nombre, Descripcion = p_descripcion, Direccion = p_direccion,
        Telefono = p_telefono, Correo = p_correo, Estatus = p_estatus
    WHERE RefugioID = p_id;
END; $$;

CREATE OR REPLACE FUNCTION sp_desactivar_refugio(p_id INT)
RETURNS VOID
LANGUAGE plpgsql AS $$
BEGIN
    UPDATE Refugio SET Estatus = 'Inactivo' WHERE RefugioID = p_id;
END; $$;


-- -------------------------------------------
-- SP: REFUGIO ADMINISTRADORES
-- -------------------------------------------

CREATE OR REPLACE FUNCTION sp_get_refugio_administradores()
RETURNS TABLE (RefugioID INT, UsuarioAdminID INT, AsignacionFecha DATE)
LANGUAGE plpgsql AS $$
BEGIN
    RETURN QUERY SELECT ra.RefugioID, ra.UsuarioAdminID, ra.AsignacionFecha FROM RefugioAdministradores ra;
END; $$;

CREATE OR REPLACE FUNCTION sp_get_admins_by_refugio(p_refugioid INT)
RETURNS TABLE (RefugioID INT, UsuarioAdminID INT, AsignacionFecha DATE)
LANGUAGE plpgsql AS $$
BEGIN
    RETURN QUERY
    SELECT ra.RefugioID, ra.UsuarioAdminID, ra.AsignacionFecha
    FROM RefugioAdministradores ra WHERE ra.RefugioID = p_refugioid;
END; $$;

CREATE OR REPLACE FUNCTION sp_insert_refugio_administrador(
    p_refugioid      INT,
    p_usuarioadminid INT
)
RETURNS VOID
LANGUAGE plpgsql AS $$
BEGIN
    INSERT INTO RefugioAdministradores (RefugioID, UsuarioAdminID, AsignacionFecha)
    VALUES (p_refugioid, p_usuarioadminid, CURRENT_DATE);
END; $$;


-- -------------------------------------------
-- SP: ANIMALES
-- -------------------------------------------

CREATE OR REPLACE FUNCTION sp_get_animales()
RETURNS TABLE (
    AnimalID INT, RefugioID INT, RazaID INT, Nombre VARCHAR,
    Sexo VARCHAR, FechaNacimiento DATE, Descripcion VARCHAR,
    Estatus VARCHAR, FechaRegistro DATE
)
LANGUAGE plpgsql AS $$
BEGIN
    RETURN QUERY
    SELECT a.AnimalID, a.RefugioID, a.RazaID, a.Nombre,
           a.Sexo, a.FechaNacimiento, a.Descripcion,
           a.Estatus, a.FechaRegistro
    FROM Animales a;
END; $$;

CREATE OR REPLACE FUNCTION sp_get_animal_by_id(p_id INT)
RETURNS TABLE (
    AnimalID INT, RefugioID INT, RazaID INT, Nombre VARCHAR,
    Sexo VARCHAR, FechaNacimiento DATE, Descripcion VARCHAR,
    Estatus VARCHAR, FechaRegistro DATE
)
LANGUAGE plpgsql AS $$
BEGIN
    RETURN QUERY
    SELECT a.AnimalID, a.RefugioID, a.RazaID, a.Nombre,
           a.Sexo, a.FechaNacimiento, a.Descripcion,
           a.Estatus, a.FechaRegistro
    FROM Animales a WHERE a.AnimalID = p_id;
END; $$;

CREATE OR REPLACE FUNCTION sp_get_animales_by_refugio(p_refugioid INT)
RETURNS TABLE (
    AnimalID INT, RefugioID INT, RazaID INT, Nombre VARCHAR,
    Sexo VARCHAR, FechaNacimiento DATE, Descripcion VARCHAR,
    Estatus VARCHAR, FechaRegistro DATE
)
LANGUAGE plpgsql AS $$
BEGIN
    RETURN QUERY
    SELECT a.AnimalID, a.RefugioID, a.RazaID, a.Nombre,
           a.Sexo, a.FechaNacimiento, a.Descripcion,
           a.Estatus, a.FechaRegistro
    FROM Animales a WHERE a.RefugioID = p_refugioid;
END; $$;

CREATE OR REPLACE FUNCTION sp_get_animales_disponibles()
RETURNS TABLE (
    AnimalID INT, RefugioID INT, RazaID INT, Nombre VARCHAR,
    Sexo VARCHAR, FechaNacimiento DATE, Descripcion VARCHAR,
    Estatus VARCHAR, FechaRegistro DATE
)
LANGUAGE plpgsql AS $$
BEGIN
    RETURN QUERY
    SELECT a.AnimalID, a.RefugioID, a.RazaID, a.Nombre,
           a.Sexo, a.FechaNacimiento, a.Descripcion,
           a.Estatus, a.FechaRegistro
    FROM Animales a WHERE a.Estatus = 'Disponible';
END; $$;

CREATE OR REPLACE FUNCTION sp_insert_animal(
    p_refugioid       INT,
    p_razaid          INT,
    p_nombre          VARCHAR,
    p_sexo            VARCHAR,
    p_fechanacimiento DATE,
    p_descripcion     VARCHAR,
    p_estatus         VARCHAR
)
RETURNS VOID
LANGUAGE plpgsql AS $$
BEGIN
    INSERT INTO Animales (RefugioID, RazaID, Nombre, Sexo, FechaNacimiento, Descripcion, Estatus, FechaRegistro)
    VALUES (p_refugioid, p_razaid, p_nombre, p_sexo, p_fechanacimiento, p_descripcion, p_estatus, CURRENT_DATE);
END; $$;

CREATE OR REPLACE FUNCTION sp_update_animal(
    p_id              INT,
    p_razaid          INT,
    p_nombre          VARCHAR,
    p_sexo            VARCHAR,
    p_fechanacimiento DATE,
    p_descripcion     VARCHAR,
    p_estatus         VARCHAR
)
RETURNS VOID
LANGUAGE plpgsql AS $$
BEGIN
    UPDATE Animales
    SET RazaID = p_razaid, Nombre = p_nombre, Sexo = p_sexo,
        FechaNacimiento = p_fechanacimiento, Descripcion = p_descripcion,
        Estatus = p_estatus
    WHERE AnimalID = p_id;
END; $$;

CREATE OR REPLACE FUNCTION sp_desactivar_animal(p_id INT)
RETURNS VOID
LANGUAGE plpgsql AS $$
BEGIN
    UPDATE Animales SET Estatus = 'Adoptado' WHERE AnimalID = p_id;
END; $$;


-- -------------------------------------------
-- SP: SOLICITUD ADOPCION
-- -------------------------------------------

CREATE OR REPLACE FUNCTION sp_get_solicitudes()
RETURNS TABLE (
    SolicitudID INT, RefugioID INT, AdoptanteID INT,
    MensajeAdoptante VARCHAR, Estatus VARCHAR, FechaRegistro DATE
)
LANGUAGE plpgsql AS $$
BEGIN
    RETURN QUERY
    SELECT s.SolicitudID, s.RefugioID, s.AdoptanteID,
           s.MensajeAdoptante, s.Estatus, s.FechaRegistro
    FROM SolicitudAdopcion s;
END; $$;

CREATE OR REPLACE FUNCTION sp_get_solicitud_by_id(p_id INT)
RETURNS TABLE (
    SolicitudID INT, RefugioID INT, AdoptanteID INT,
    MensajeAdoptante VARCHAR, Estatus VARCHAR, FechaRegistro DATE
)
LANGUAGE plpgsql AS $$
BEGIN
    RETURN QUERY
    SELECT s.SolicitudID, s.RefugioID, s.AdoptanteID,
           s.MensajeAdoptante, s.Estatus, s.FechaRegistro
    FROM SolicitudAdopcion s WHERE s.SolicitudID = p_id;
END; $$;

CREATE OR REPLACE FUNCTION sp_get_solicitudes_by_adoptante(p_adoptanteid INT)
RETURNS TABLE (
    SolicitudID INT, RefugioID INT, AdoptanteID INT,
    MensajeAdoptante VARCHAR, Estatus VARCHAR, FechaRegistro DATE
)
LANGUAGE plpgsql AS $$
BEGIN
    RETURN QUERY
    SELECT s.SolicitudID, s.RefugioID, s.AdoptanteID,
           s.MensajeAdoptante, s.Estatus, s.FechaRegistro
    FROM SolicitudAdopcion s WHERE s.AdoptanteID = p_adoptanteid;
END; $$;

CREATE OR REPLACE FUNCTION sp_get_solicitudes_by_refugio(p_refugioid INT)
RETURNS TABLE (
    SolicitudID INT, RefugioID INT, AdoptanteID INT,
    MensajeAdoptante VARCHAR, Estatus VARCHAR, FechaRegistro DATE
)
LANGUAGE plpgsql AS $$
BEGIN
    RETURN QUERY
    SELECT s.SolicitudID, s.RefugioID, s.AdoptanteID,
           s.MensajeAdoptante, s.Estatus, s.FechaRegistro
    FROM SolicitudAdopcion s WHERE s.RefugioID = p_refugioid;
END; $$;

CREATE OR REPLACE FUNCTION sp_insert_solicitud(
    p_refugioid        INT,
    p_adoptanteid      INT,
    p_mensajeadoptante VARCHAR
)
RETURNS integer
LANGUAGE plpgsql AS $$
BEGIN
    INSERT INTO SolicitudAdopcion (RefugioID, AdoptanteID, MensajeAdoptante)
    VALUES (p_refugioid, p_adoptanteid, p_mensajeadoptante)
    returning SolicitudID into p_id;
    
    return p_id;
END; $$;

CREATE OR REPLACE FUNCTION sp_update_estatus_solicitud(p_id INT, p_estatus VARCHAR)
RETURNS VOID
LANGUAGE plpgsql AS $$
BEGIN
    UPDATE SolicitudAdopcion SET Estatus = p_estatus WHERE SolicitudID = p_id;
END; $$;

CREATE OR REPLACE FUNCTION sp_desactivar_solicitud(p_id INT)
RETURNS VOID
LANGUAGE plpgsql AS $$
BEGIN
    UPDATE SolicitudAdopcion SET Estatus = 'Rechazada' WHERE SolicitudID = p_id;
END; $$;


-- -------------------------------------------
-- SP: SOLICITUD ANIMALES
-- -------------------------------------------

CREATE OR REPLACE FUNCTION sp_get_animales_by_solicitud(p_solicitudid INT)
RETURNS TABLE (SolicitudID INT, AnimalID INT)
LANGUAGE plpgsql AS $$
BEGIN
    RETURN QUERY
    SELECT sa.SolicitudID, sa.AnimalID
    FROM SolicitudAnimales sa WHERE sa.SolicitudID = p_solicitudid;
END; $$;

CREATE OR REPLACE FUNCTION sp_insert_solicitud_animal(p_solicitudid INT, p_animalid INT)
RETURNS VOID
LANGUAGE plpgsql AS $$
BEGIN
    INSERT INTO SolicitudAnimales (SolicitudID, AnimalID) VALUES (p_solicitudid, p_animalid);
END; $$;


-- -------------------------------------------
-- TABLA: IMAGENES (polimórfica)
-- -------------------------------------------

CREATE TABLE Imagenes (
    ImagenID      INT          GENERATED ALWAYS AS IDENTITY PRIMARY KEY NOT NULL,
    EntidadTipo   VARCHAR(50)  NOT NULL,
    EntidadID     INT          NOT NULL,
    Url           VARCHAR(500) NOT NULL,
    Orden         INT          NOT NULL DEFAULT 0,
    NombreArchivo VARCHAR(255) NOT NULL,
    FechaSubida   DATE         NOT NULL DEFAULT CURRENT_DATE
);

CREATE INDEX idx_imagenes_entidad ON Imagenes (EntidadTipo, EntidadID);


-- -------------------------------------------
-- SP: IMAGENES
-- -------------------------------------------

CREATE OR REPLACE FUNCTION sp_get_imagenes()
RETURNS TABLE (
    ImagenID INT, EntidadTipo VARCHAR, EntidadID INT,
    Url VARCHAR, Orden INT, NombreArchivo VARCHAR, FechaSubida DATE
)
LANGUAGE plpgsql AS $$
BEGIN
    RETURN QUERY
    SELECT i.ImagenID, i.EntidadTipo, i.EntidadID,
           i.Url, i.Orden, i.NombreArchivo, i.FechaSubida
    FROM Imagenes i;
END; $$;

CREATE OR REPLACE FUNCTION sp_get_imagen_by_id(p_id INT)
RETURNS TABLE (
    ImagenID INT, EntidadTipo VARCHAR, EntidadID INT,
    Url VARCHAR, Orden INT, NombreArchivo VARCHAR, FechaSubida DATE
)
LANGUAGE plpgsql AS $$
BEGIN
    RETURN QUERY
    SELECT i.ImagenID, i.EntidadTipo, i.EntidadID,
           i.Url, i.Orden, i.NombreArchivo, i.FechaSubida
    FROM Imagenes i WHERE i.ImagenID = p_id;
END; $$;

CREATE OR REPLACE FUNCTION sp_get_imagenes_by_entidad(p_entidadtipo VARCHAR, p_entidadid INT)
RETURNS TABLE (
    ImagenID INT, EntidadTipo VARCHAR, EntidadID INT,
    Url VARCHAR, Orden INT, NombreArchivo VARCHAR, FechaSubida DATE
)
LANGUAGE plpgsql AS $$
BEGIN
    RETURN QUERY
    SELECT i.ImagenID, i.EntidadTipo, i.EntidadID,
           i.Url, i.Orden, i.NombreArchivo, i.FechaSubida
    FROM Imagenes i
    WHERE i.EntidadTipo = p_entidadtipo AND i.EntidadID = p_entidadid
    ORDER BY i.Orden;
END; $$;

CREATE OR REPLACE FUNCTION sp_insert_imagen(
    p_entidadtipo  VARCHAR,
    p_entidadid    INT,
    p_url          VARCHAR,
    p_orden        INT,
    p_nombrearchivo VARCHAR
)
RETURNS VOID
LANGUAGE plpgsql AS $$
BEGIN
    INSERT INTO Imagenes (EntidadTipo, EntidadID, Url, Orden, NombreArchivo)
    VALUES (p_entidadtipo, p_entidadid, p_url, p_orden, p_nombrearchivo);
END; $$;

CREATE OR REPLACE FUNCTION sp_update_imagen(
    p_id           INT,
    p_url          VARCHAR,
    p_orden        INT,
    p_nombrearchivo VARCHAR
)
RETURNS VOID
LANGUAGE plpgsql AS $$
BEGIN
    UPDATE Imagenes
    SET Url = p_url, Orden = p_orden, NombreArchivo = p_nombrearchivo
    WHERE ImagenID = p_id;
END; $$;

CREATE OR REPLACE FUNCTION sp_delete_imagen(p_id INT)
RETURNS VOID
LANGUAGE plpgsql AS $$
BEGIN
    DELETE FROM Imagenes WHERE ImagenID = p_id;
END; $$;


-- -------------------------------------------
-- SP: CITAS
-- -------------------------------------------

CREATE OR REPLACE FUNCTION sp_get_citas()
RETURNS TABLE (CitaID INT, SolicitudID INT, FechaHoraCita TIMESTAMP, EstadoCita VARCHAR)
LANGUAGE plpgsql AS $$
BEGIN
    RETURN QUERY SELECT c.CitaID, c.SolicitudID, c.FechaHoraCita, c.EstadoCita FROM Citas c;
END; $$;

CREATE OR REPLACE FUNCTION sp_get_cita_by_id(p_id INT)
RETURNS TABLE (CitaID INT, SolicitudID INT, FechaHoraCita TIMESTAMP, EstadoCita VARCHAR)
LANGUAGE plpgsql AS $$
BEGIN
    RETURN QUERY SELECT c.CitaID, c.SolicitudID, c.FechaHoraCita, c.EstadoCita FROM Citas c WHERE c.CitaID = p_id;
END; $$;

CREATE OR REPLACE FUNCTION sp_get_citas_by_solicitud(p_solicitudid INT)
RETURNS TABLE (CitaID INT, SolicitudID INT, FechaHoraCita TIMESTAMP, EstadoCita VARCHAR)
LANGUAGE plpgsql AS $$
BEGIN
    RETURN QUERY
    SELECT c.CitaID, c.SolicitudID, c.FechaHoraCita, c.EstadoCita
    FROM Citas c WHERE c.SolicitudID = p_solicitudid;
END; $$;

CREATE OR REPLACE FUNCTION sp_insert_cita(
    p_solicitudid   INT,
    p_fechahoracita TIMESTAMP,
    p_estadocita    VARCHAR
)
RETURNS VOID
LANGUAGE plpgsql AS $$
BEGIN
    INSERT INTO Citas (SolicitudID, FechaHoraCita, EstadoCita)
    VALUES (p_solicitudid, p_fechahoracita, p_estadocita);
END; $$;

CREATE OR REPLACE FUNCTION sp_update_estado_cita(p_id INT, p_estadocita VARCHAR)
RETURNS VOID
LANGUAGE plpgsql AS $$
BEGIN
    UPDATE Citas SET EstadoCita = p_estadocita WHERE CitaID = p_id;
END; $$;

CREATE OR REPLACE FUNCTION sp_update_cita(
    p_id            INT,
    p_fechahoracita TIMESTAMP,
    p_estadocita    VARCHAR
)
RETURNS VOID
LANGUAGE plpgsql AS $$
BEGIN
    UPDATE Citas SET FechaHoraCita = p_fechahoracita, EstadoCita = p_estadocita WHERE CitaID = p_id;
END; $$;

CREATE OR REPLACE FUNCTION sp_desactivar_cita(p_id INT)
RETURNS VOID
LANGUAGE plpgsql AS $$
BEGIN
    UPDATE Citas SET EstadoCita = 'Cancelada' WHERE CitaID = p_id;
END; $$;

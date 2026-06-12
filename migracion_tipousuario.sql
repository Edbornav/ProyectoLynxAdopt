-- 1. Agregar columna TipoUsuario (nullable, sin CHECK aún)
ALTER TABLE Usuarios ADD COLUMN TipoUsuario VARCHAR(50);

-- 2. Actualizar CHECK constraint (postgreSQL no crea automática si no existía antes)
ALTER TABLE Usuarios DROP CONSTRAINT IF EXISTS usuarios_tipousuario_check;
ALTER TABLE Usuarios ADD CONSTRAINT usuarios_tipousuario_check
    CHECK (TipoUsuario IS NULL OR TipoUsuario IN ('Adoptante', 'Refugio', 'Administrador'));

-- 3. Re-crear sp_insert_usuario sin p_tipousuario
CREATE OR REPLACE FUNCTION sp_insert_usuario(
    p_correo       VARCHAR,
    p_passwordhash VARCHAR,
    p_estatus      VARCHAR
)
RETURNS INTEGER
LANGUAGE plpgsql
AS $$
DECLARE
    p_id INTEGER;
BEGIN
    INSERT INTO Usuarios (Correo, PasswordHash, Estatus, FechaRegistro)
    VALUES (p_correo, p_passwordhash, p_estatus, CURRENT_DATE)
    RETURNING UsuarioID INTO p_id;
    RETURN p_id;
END; $$;

-- 4. Re-crear sp_login con TipoUsuario en el SELECT
CREATE OR REPLACE FUNCTION sp_login(p_correo VARCHAR)
RETURNS TABLE (UsuarioID INT, Correo VARCHAR, PasswordHash VARCHAR, TipoUsuario VARCHAR, Estatus VARCHAR)
LANGUAGE plpgsql AS $$
BEGIN
    RETURN QUERY
    SELECT u.UsuarioID, u.Correo, u.PasswordHash, u.TipoUsuario, u.Estatus
    FROM Usuarios u WHERE u.Correo = p_correo AND u.Estatus = 'Activo';
END; $$;

-- 5. Re-crear sp_get_usuarios con TipoUsuario
CREATE OR REPLACE FUNCTION sp_get_usuarios()
RETURNS TABLE (UsuarioID INT, Correo VARCHAR, TipoUsuario VARCHAR, Estatus VARCHAR, FechaRegistro DATE)
LANGUAGE plpgsql AS $$
BEGIN
    RETURN QUERY SELECT u.UsuarioID, u.Correo, u.TipoUsuario, u.Estatus, u.FechaRegistro FROM Usuarios u;
END; $$;

-- 6. Re-crear sp_get_usuario_by_id con TipoUsuario
CREATE OR REPLACE FUNCTION sp_get_usuario_by_id(p_id INT)
RETURNS TABLE (UsuarioID INT, Correo VARCHAR, TipoUsuario VARCHAR, Estatus VARCHAR, FechaRegistro DATE)
LANGUAGE plpgsql AS $$
BEGIN
    RETURN QUERY SELECT u.UsuarioID, u.Correo, u.TipoUsuario, u.Estatus, u.FechaRegistro FROM Usuarios u WHERE u.UsuarioID = p_id;
END; $$;

-- 7. Re-crear sp_update_usuario (actualiza TipoUsuario)
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

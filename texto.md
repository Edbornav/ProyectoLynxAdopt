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
-- Agrega FotoUrl a los SPs de Animales mediante subconsulta a Imagenes
-- Nota: DROP + CREATE porque CREATE OR REPLACE no permite cambiar RETURN TYPE

DROP FUNCTION IF EXISTS sp_get_animales() CASCADE;
CREATE OR REPLACE FUNCTION sp_get_animales()
RETURNS TABLE (
    AnimalID INT, RefugioID INT, RazaID INT, Nombre VARCHAR,
    Sexo VARCHAR, FechaNacimiento DATE, Descripcion VARCHAR,
    Estatus VARCHAR, FechaRegistro DATE, FotoUrl VARCHAR
)
LANGUAGE plpgsql AS $$
BEGIN
    RETURN QUERY
    SELECT a.AnimalID, a.RefugioID, a.RazaID, a.Nombre,
           a.Sexo, a.FechaNacimiento, a.Descripcion,
           a.Estatus, a.FechaRegistro,
           (SELECT i.Url FROM Imagenes i WHERE i.EntidadTipo = 'Animal' AND i.EntidadID = a.AnimalID ORDER BY i.Orden LIMIT 1) AS FotoUrl
    FROM Animales a;
END; $$;

DROP FUNCTION IF EXISTS sp_get_animal_by_id(p_id INT) CASCADE;
CREATE OR REPLACE FUNCTION sp_get_animal_by_id(p_id INT)
RETURNS TABLE (
    AnimalID INT, RefugioID INT, RazaID INT, Nombre VARCHAR,
    Sexo VARCHAR, FechaNacimiento DATE, Descripcion VARCHAR,
    Estatus VARCHAR, FechaRegistro DATE, FotoUrl VARCHAR
)
LANGUAGE plpgsql AS $$
BEGIN
    RETURN QUERY
    SELECT a.AnimalID, a.RefugioID, a.RazaID, a.Nombre,
           a.Sexo, a.FechaNacimiento, a.Descripcion,
           a.Estatus, a.FechaRegistro,
           (SELECT i.Url FROM Imagenes i WHERE i.EntidadTipo = 'Animal' AND i.EntidadID = a.AnimalID ORDER BY i.Orden LIMIT 1) AS FotoUrl
    FROM Animales a WHERE a.AnimalID = p_id;
END; $$;

DROP FUNCTION IF EXISTS sp_get_animales_by_refugio(p_refugioid INT) CASCADE;
CREATE OR REPLACE FUNCTION sp_get_animales_by_refugio(p_refugioid INT)
RETURNS TABLE (
    AnimalID INT, RefugioID INT, RazaID INT, Nombre VARCHAR,
    Sexo VARCHAR, FechaNacimiento DATE, Descripcion VARCHAR,
    Estatus VARCHAR, FechaRegistro DATE, FotoUrl VARCHAR
)
LANGUAGE plpgsql AS $$
BEGIN
    RETURN QUERY
    SELECT a.AnimalID, a.RefugioID, a.RazaID, a.Nombre,
           a.Sexo, a.FechaNacimiento, a.Descripcion,
           a.Estatus, a.FechaRegistro,
           (SELECT i.Url FROM Imagenes i WHERE i.EntidadTipo = 'Animal' AND i.EntidadID = a.AnimalID ORDER BY i.Orden LIMIT 1) AS FotoUrl
    FROM Animales a WHERE a.RefugioID = p_refugioid;
END; $$;

DROP FUNCTION IF EXISTS sp_get_animales_disponibles() CASCADE;
CREATE OR REPLACE FUNCTION sp_get_animales_disponibles()
RETURNS TABLE (
    AnimalID INT, RefugioID INT, RazaID INT, Nombre VARCHAR,
    Sexo VARCHAR, FechaNacimiento DATE, Descripcion VARCHAR,
    Estatus VARCHAR, FechaRegistro DATE, FotoUrl VARCHAR
)
LANGUAGE plpgsql AS $$
BEGIN
    RETURN QUERY
    SELECT a.AnimalID, a.RefugioID, a.RazaID, a.Nombre,
           a.Sexo, a.FechaNacimiento, a.Descripcion,
           a.Estatus, a.FechaRegistro,
           (SELECT i.Url FROM Imagenes i WHERE i.EntidadTipo = 'Animal' AND i.EntidadID = a.AnimalID ORDER BY i.Orden LIMIT 1) AS FotoUrl
    FROM Animales a WHERE a.Estatus = 'Disponible';
END; $$;

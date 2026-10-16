-- Migración: corregir tipo Url en funciones de Imagenes (VARCHAR -> TEXT)
-- Ejecutar en Supabase SQL Editor

DROP FUNCTION IF EXISTS sp_get_imagenes() CASCADE;
DROP FUNCTION IF EXISTS sp_get_imagen_by_id(INT) CASCADE;
DROP FUNCTION IF EXISTS sp_get_imagenes_by_entidad(VARCHAR, INT) CASCADE;

CREATE OR REPLACE FUNCTION sp_get_imagenes()
RETURNS TABLE (
    ImagenID INT, EntidadTipo VARCHAR, EntidadID INT,
    Url TEXT, Orden INT, NombreArchivo VARCHAR, FechaSubida DATE
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
    Url TEXT, Orden INT, NombreArchivo VARCHAR, FechaSubida DATE
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
    Url TEXT, Orden INT, NombreArchivo VARCHAR, FechaSubida DATE
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

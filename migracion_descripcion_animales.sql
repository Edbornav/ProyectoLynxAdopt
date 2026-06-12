-- Migración: ampliar columna Descripcion en Animales
-- Ejecutar en Supabase SQL Editor

ALTER TABLE Animales ALTER COLUMN Descripcion TYPE VARCHAR(500);

-- También agregar sp_get_perfil_adoptante_by_adoptante
CREATE OR REPLACE FUNCTION sp_get_perfil_adoptante_by_adoptante(p_adoptanteid INT)
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
    FROM PerfilAdoptante p WHERE p.AdoptanteUsuarioID = p_adoptanteid;
END; $$;

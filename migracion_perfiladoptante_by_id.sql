-- Bugfix: sp_get_perfil_adoptante_by_id filtraba por AdoptanteUsuarioID en vez de PerfilAdoptanteID
-- La función se llamaba "by_id" pero usaba WHERE p.AdoptanteUsuarioID = p_id
-- Causaba KeyNotFoundException en PUT /PerfilesAdoptante/{id}

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
    FROM PerfilAdoptante p WHERE p.PerfilAdoptanteID = p_id;
END; $$;

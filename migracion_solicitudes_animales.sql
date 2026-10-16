-- Migration: Add duplicate check, update animal status on solicitud create/approve/reject

-- 1. New SP that creates solicitud + links animal + validates + sets En Proceso atomically
CREATE OR REPLACE FUNCTION sp_insert_solicitud_completa(
    p_refugioid INT,
    p_adoptanteid INT,
    p_mensajeadoptante VARCHAR,
    p_animalid INT
)
RETURNS INTEGER
LANGUAGE plpgsql
AS $$
DECLARE
    p_solicitudid INTEGER;
    v_estatus VARCHAR;
BEGIN
    -- Check animal exists and is Disponible
    SELECT Estatus INTO v_estatus FROM Animales WHERE AnimalID = p_animalid;
    IF NOT FOUND THEN
        RAISE EXCEPTION 'Animal no encontrado';
    END IF;
    IF v_estatus != 'Disponible' THEN
        RAISE EXCEPTION 'El animal no esta disponible para adopcion';
    END IF;

    -- Check duplicate: same adoptante + same animal with active solicitud
    IF EXISTS (
        SELECT 1 FROM SolicitudAdopcion sa
        JOIN SolicitudAnimales san ON sa.SolicitudID = san.SolicitudID
        WHERE sa.AdoptanteID = p_adoptanteid
          AND san.AnimalID = p_animalid
          AND sa.Estatus IN ('Pendiente', 'Aprobada')
    ) THEN
        RAISE EXCEPTION 'Ya existe una solicitud activa para este animal';
    END IF;

    -- Insert solicitud
    INSERT INTO SolicitudAdopcion (RefugioID, AdoptanteID, MensajeAdoptante)
    VALUES (p_refugioid, p_adoptanteid, p_mensajeadoptante)
    RETURNING SolicitudID INTO p_solicitudid;

    -- Link animal
    INSERT INTO SolicitudAnimales (SolicitudID, AnimalID) VALUES (p_solicitudid, p_animalid);

    -- Update animal to En Proceso
    UPDATE Animales SET Estatus = 'En Proceso' WHERE AnimalID = p_animalid;

    RETURN p_solicitudid;
END; $$;

-- 2. Modify sp_update_estatus_solicitud to also update linked animal status
DROP FUNCTION IF EXISTS sp_update_estatus_solicitud(INT, VARCHAR) CASCADE;

CREATE OR REPLACE FUNCTION sp_update_estatus_solicitud(p_id INT, p_estatus VARCHAR)
RETURNS VOID
LANGUAGE plpgsql
AS $$
BEGIN
    UPDATE SolicitudAdopcion SET Estatus = p_estatus WHERE SolicitudID = p_id;

    IF p_estatus = 'Aprobada' THEN
        UPDATE Animales SET Estatus = 'Adoptado'
        WHERE AnimalID IN (SELECT AnimalID FROM SolicitudAnimales WHERE SolicitudID = p_id);
    ELSIF p_estatus = 'Rechazada' THEN
        UPDATE Animales SET Estatus = 'Disponible'
        WHERE AnimalID IN (SELECT AnimalID FROM SolicitudAnimales WHERE SolicitudID = p_id);
    END IF;
END; $$;

-- 3. Modify sp_desactivar_solicitud to also revert animal to Disponible
DROP FUNCTION IF EXISTS sp_desactivar_solicitud(INT) CASCADE;

CREATE OR REPLACE FUNCTION sp_desactivar_solicitud(p_id INT)
RETURNS VOID
LANGUAGE plpgsql
AS $$
BEGIN
    UPDATE SolicitudAdopcion SET Estatus = 'Rechazada' WHERE SolicitudID = p_id;

    UPDATE Animales SET Estatus = 'Disponible'
    WHERE AnimalID IN (SELECT AnimalID FROM SolicitudAnimales WHERE SolicitudID = p_id);
END; $$;

-- 4. Defense in depth: validate animal availability in sp_insert_solicitud_animal
CREATE OR REPLACE FUNCTION sp_insert_solicitud_animal(p_solicitudid INT, p_animalid INT)
RETURNS VOID
LANGUAGE plpgsql AS $$
DECLARE
    v_estatus VARCHAR;
BEGIN
    SELECT Estatus INTO v_estatus FROM Animales WHERE AnimalID = p_animalid;
    IF v_estatus IS NULL THEN
        RAISE EXCEPTION 'Animal no encontrado';
    END IF;
    IF v_estatus IN ('Adoptado', 'En Proceso') THEN
        RAISE EXCEPTION 'El animal no esta disponible';
    END IF;
    INSERT INTO SolicitudAnimales (SolicitudID, AnimalID) VALUES (p_solicitudid, p_animalid);
END; $$;

-- =============================================
-- SEED DATA PARA PRUEBAS
-- Uso: Copiar y pegar en Supabase SQL Editor
-- No depende de IDs fijos — busca por nombre
-- =============================================

-- 1. ESPECIES (solo si no existen)
INSERT INTO Especie (Nombre)
SELECT 'Perro' WHERE NOT EXISTS (SELECT 1 FROM Especie WHERE Nombre = 'Perro');
INSERT INTO Especie (Nombre)
SELECT 'Gato'  WHERE NOT EXISTS (SELECT 1 FROM Especie WHERE Nombre = 'Gato');

-- 2. RAZAS (solo si no existen)
DO $$
DECLARE
    v_perro_id INT;
    v_gato_id  INT;
BEGIN
    SELECT EspecieID INTO v_perro_id FROM Especie WHERE Nombre = 'Perro';
    SELECT EspecieID INTO v_gato_id  FROM Especie WHERE Nombre = 'Gato';

    INSERT INTO Raza (EspecieID, Nombre)
    SELECT v_perro_id, r.nombre FROM (VALUES
        ('Labrador'), ('Pastor Alemán'), ('Bulldog'),
        ('Golden Retriever'), ('Husky'), ('Chihuahua'),
        ('Poodle'), ('Beagle'), ('Dálmata'), ('Schnauzer')
    ) AS r(nombre)
    WHERE NOT EXISTS (SELECT 1 FROM Raza WHERE EspecieID = v_perro_id AND Nombre = r.nombre);

    INSERT INTO Raza (EspecieID, Nombre)
    SELECT v_gato_id, r.nombre FROM (VALUES
        ('Persa'), ('Siamés'), ('Maine Coon'),
        ('Bengalí'), ('Sphynx'), ('Angora'),
        ('Scottish Fold'), ('Ragdoll'), ('Británico'), ('Abisinio')
    ) AS r(nombre)
    WHERE NOT EXISTS (SELECT 1 FROM Raza WHERE EspecieID = v_gato_id AND Nombre = r.nombre);
END $$;

-- 3. ANIMALES (20 por cada refugio activo)
-- Se salta si el animal con mismo nombre ya existe en ese refugio
DO $$
DECLARE
    ref RECORD;
    v_perro_ids   INT[] := ARRAY(SELECT RazaID FROM Raza r JOIN Especie e ON r.EspecieID = e.EspecieID WHERE e.Nombre = 'Perro' ORDER BY r.RazaID);
    v_gato_ids    INT[] := ARRAY(SELECT RazaID FROM Raza r JOIN Especie e ON r.EspecieID = e.EspecieID WHERE e.Nombre = 'Gato'  ORDER BY r.RazaID);
BEGIN
    FOR ref IN SELECT RefugioID FROM Refugio WHERE Estatus = 'Activo' LOOP
        -- Perros (10)
        INSERT INTO Animales (RefugioID, RazaID, Nombre, Sexo, FechaNacimiento, Descripcion, Estatus, FechaRegistro)
        SELECT ref.RefugioID, v_perro_ids[1],  'Max',    'Macho',   '2022-03-15', 'Perro juguetón y cariñoso.',             'Disponible', CURRENT_DATE
        WHERE NOT EXISTS (SELECT 1 FROM Animales WHERE RefugioID = ref.RefugioID AND Nombre = 'Max'    AND RazaID = v_perro_ids[1]);
        INSERT INTO Animales (RefugioID, RazaID, Nombre, Sexo, FechaNacimiento, Descripcion, Estatus, FechaRegistro)
        SELECT ref.RefugioID, v_perro_ids[2],  'Luna',   'Hembra',  '2021-07-20', 'Pastor alemán tranquilo.',               'Disponible', CURRENT_DATE
        WHERE NOT EXISTS (SELECT 1 FROM Animales WHERE RefugioID = ref.RefugioID AND Nombre = 'Luna'   AND RazaID = v_perro_ids[2]);
        INSERT INTO Animales (RefugioID, RazaID, Nombre, Sexo, FechaNacimiento, Descripcion, Estatus, FechaRegistro)
        SELECT ref.RefugioID, v_perro_ids[3],  'Rocky',  'Macho',   '2023-01-10', 'Bulldog cachorro dócil.',                'Disponible', CURRENT_DATE
        WHERE NOT EXISTS (SELECT 1 FROM Animales WHERE RefugioID = ref.RefugioID AND Nombre = 'Rocky'  AND RazaID = v_perro_ids[3]);
        INSERT INTO Animales (RefugioID, RazaID, Nombre, Sexo, FechaNacimiento, Descripcion, Estatus, FechaRegistro)
        SELECT ref.RefugioID, v_perro_ids[4],  'Bella',  'Hembra',  '2020-11-05', 'Golden Retriever ama el agua.',           'Disponible', CURRENT_DATE
        WHERE NOT EXISTS (SELECT 1 FROM Animales WHERE RefugioID = ref.RefugioID AND Nombre = 'Bella'  AND RazaID = v_perro_ids[4]);
        INSERT INTO Animales (RefugioID, RazaID, Nombre, Sexo, FechaNacimiento, Descripcion, Estatus, FechaRegistro)
        SELECT ref.RefugioID, v_perro_ids[5],  'Koda',   'Macho',   '2022-08-12', 'Husky enérgico.',                        'Disponible', CURRENT_DATE
        WHERE NOT EXISTS (SELECT 1 FROM Animales WHERE RefugioID = ref.RefugioID AND Nombre = 'Koda'   AND RazaID = v_perro_ids[5]);
        INSERT INTO Animales (RefugioID, RazaID, Nombre, Sexo, FechaNacimiento, Descripcion, Estatus, FechaRegistro)
        SELECT ref.RefugioID, v_perro_ids[6],  'Taco',   'Macho',   '2023-05-30', 'Chihuahua pequeño y gracioso.',           'Disponible', CURRENT_DATE
        WHERE NOT EXISTS (SELECT 1 FROM Animales WHERE RefugioID = ref.RefugioID AND Nombre = 'Taco'   AND RazaID = v_perro_ids[6]);
        INSERT INTO Animales (RefugioID, RazaID, Nombre, Sexo, FechaNacimiento, Descripcion, Estatus, FechaRegistro)
        SELECT ref.RefugioID, v_perro_ids[7],  'Copito', 'Hembra',  '2021-02-14', 'Poodle hipoalergénico.',                  'Disponible', CURRENT_DATE
        WHERE NOT EXISTS (SELECT 1 FROM Animales WHERE RefugioID = ref.RefugioID AND Nombre = 'Copito' AND RazaID = v_perro_ids[7]);
        INSERT INTO Animales (RefugioID, RazaID, Nombre, Sexo, FechaNacimiento, Descripcion, Estatus, FechaRegistro)
        SELECT ref.RefugioID, v_perro_ids[8],  'Snoopy', 'Macho',   '2022-06-01', 'Beagle curioso y amigable.',              'Disponible', CURRENT_DATE
        WHERE NOT EXISTS (SELECT 1 FROM Animales WHERE RefugioID = ref.RefugioID AND Nombre = 'Snoopy' AND RazaID = v_perro_ids[8]);
        INSERT INTO Animales (RefugioID, RazaID, Nombre, Sexo, FechaNacimiento, Descripcion, Estatus, FechaRegistro)
        SELECT ref.RefugioID, v_perro_ids[9],  'Dally',  'Hembra',  '2020-09-18', 'Dálmata elegante y activa.',              'Disponible', CURRENT_DATE
        WHERE NOT EXISTS (SELECT 1 FROM Animales WHERE RefugioID = ref.RefugioID AND Nombre = 'Dally'  AND RazaID = v_perro_ids[9]);
        INSERT INTO Animales (RefugioID, RazaID, Nombre, Sexo, FechaNacimiento, Descripcion, Estatus, FechaRegistro)
        SELECT ref.RefugioID, v_perro_ids[10], 'Oso',    'Macho',   '2021-12-25', 'Schnauzer protector y divertido.',        'Disponible', CURRENT_DATE
        WHERE NOT EXISTS (SELECT 1 FROM Animales WHERE RefugioID = ref.RefugioID AND Nombre = 'Oso'    AND RazaID = v_perro_ids[10]);

        -- Gatos (10)
        INSERT INTO Animales (RefugioID, RazaID, Nombre, Sexo, FechaNacimiento, Descripcion, Estatus, FechaRegistro)
        SELECT ref.RefugioID, v_gato_ids[1],  'Luna',   'Hembra',  '2022-04-10', 'Persa blanca de pelo largo.',             'Disponible', CURRENT_DATE
        WHERE NOT EXISTS (SELECT 1 FROM Animales WHERE RefugioID = ref.RefugioID AND Nombre = 'Luna'   AND RazaID = v_gato_ids[1]);
        INSERT INTO Animales (RefugioID, RazaID, Nombre, Sexo, FechaNacimiento, Descripcion, Estatus, FechaRegistro)
        SELECT ref.RefugioID, v_gato_ids[2],  'Simba',  'Macho',   '2021-08-22', 'Siamés vocal y cariñoso.',                'Disponible', CURRENT_DATE
        WHERE NOT EXISTS (SELECT 1 FROM Animales WHERE RefugioID = ref.RefugioID AND Nombre = 'Simba'  AND RazaID = v_gato_ids[2]);
        INSERT INTO Animales (RefugioID, RazaID, Nombre, Sexo, FechaNacimiento, Descripcion, Estatus, FechaRegistro)
        SELECT ref.RefugioID, v_gato_ids[3],  'Thor',   'Macho',   '2020-01-15', 'Maine Coon gigante y gentil.',            'Disponible', CURRENT_DATE
        WHERE NOT EXISTS (SELECT 1 FROM Animales WHERE RefugioID = ref.RefugioID AND Nombre = 'Thor'   AND RazaID = v_gato_ids[3]);
        INSERT INTO Animales (RefugioID, RazaID, Nombre, Sexo, FechaNacimiento, Descripcion, Estatus, FechaRegistro)
        SELECT ref.RefugioID, v_gato_ids[4],  'Shelby', 'Hembra',  '2023-03-08', 'Bengalí manchada muy activa.',            'Disponible', CURRENT_DATE
        WHERE NOT EXISTS (SELECT 1 FROM Animales WHERE RefugioID = ref.RefugioID AND Nombre = 'Shelby' AND RazaID = v_gato_ids[4]);
        INSERT INTO Animales (RefugioID, RazaID, Nombre, Sexo, FechaNacimiento, Descripcion, Estatus, FechaRegistro)
        SELECT ref.RefugioID, v_gato_ids[5],  'Yoda',   'Macho',   '2022-11-30', 'Sphynx temperamental pero apegado.',      'Disponible', CURRENT_DATE
        WHERE NOT EXISTS (SELECT 1 FROM Animales WHERE RefugioID = ref.RefugioID AND Nombre = 'Yoda'   AND RazaID = v_gato_ids[5]);
        INSERT INTO Animales (RefugioID, RazaID, Nombre, Sexo, FechaNacimiento, Descripcion, Estatus, FechaRegistro)
        SELECT ref.RefugioID, v_gato_ids[6],  'Nieve',  'Hembra',  '2021-05-05', 'Angora blanca de pelo sedoso.',           'Disponible', CURRENT_DATE
        WHERE NOT EXISTS (SELECT 1 FROM Animales WHERE RefugioID = ref.RefugioID AND Nombre = 'Nieve'  AND RazaID = v_gato_ids[6]);
        INSERT INTO Animales (RefugioID, RazaID, Nombre, Sexo, FechaNacimiento, Descripcion, Estatus, FechaRegistro)
        SELECT ref.RefugioID, v_gato_ids[7],  'Gizmo',  'Macho',   '2023-07-19', 'Scottish Fold orejas dobladas y tierno.',  'Disponible', CURRENT_DATE
        WHERE NOT EXISTS (SELECT 1 FROM Animales WHERE RefugioID = ref.RefugioID AND Nombre = 'Gizmo'  AND RazaID = v_gato_ids[7]);
        INSERT INTO Animales (RefugioID, RazaID, Nombre, Sexo, FechaNacimiento, Descripcion, Estatus, FechaRegistro)
        SELECT ref.RefugioID, v_gato_ids[8],  'Mochi',  'Hembra',  '2020-12-01', 'Ragdoll que se relaja al cargarla.',      'Disponible', CURRENT_DATE
        WHERE NOT EXISTS (SELECT 1 FROM Animales WHERE RefugioID = ref.RefugioID AND Nombre = 'Mochi'  AND RazaID = v_gato_ids[8]);
        INSERT INTO Animales (RefugioID, RazaID, Nombre, Sexo, FechaNacimiento, Descripcion, Estatus, FechaRegistro)
        SELECT ref.RefugioID, v_gato_ids[9],  'Oliver', 'Macho',   '2022-09-14', 'Británico gris de carácter tranquilo.',   'Disponible', CURRENT_DATE
        WHERE NOT EXISTS (SELECT 1 FROM Animales WHERE RefugioID = ref.RefugioID AND Nombre = 'Oliver' AND RazaID = v_gato_ids[9]);
        INSERT INTO Animales (RefugioID, RazaID, Nombre, Sexo, FechaNacimiento, Descripcion, Estatus, FechaRegistro)
        SELECT ref.RefugioID, v_gato_ids[10], 'Zafiro', 'Hembra',  '2021-06-28', 'Abisinio ágil y curioso.',               'Disponible', CURRENT_DATE
        WHERE NOT EXISTS (SELECT 1 FROM Animales WHERE RefugioID = ref.RefugioID AND Nombre = 'Zafiro' AND RazaID = v_gato_ids[10]);
    END LOOP;
END $$;

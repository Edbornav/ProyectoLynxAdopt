let _especies = [];
let _razas = [];
let _imagenFile = null;

window.initAgregarMascota = async function() {
    const session = JSON.parse(localStorage.getItem('session') || '{}');
    if (!session.usuarioID) { redirect('02_inicio_sesion'); return; }

    try {
        const admin = await apiGet('/Administradores/por-usuario/' + session.usuarioID);
        if (!admin) { redirect('02_inicio_sesion'); return; }

        _especies = await apiGet('/Especies');
        _razas = await apiGet('/Razas');

        const selEspecie = document.getElementById('petEspecie');
        selEspecie.innerHTML = '<option value="">Selecciona especie</option>';
        _especies.forEach(e => {
            const opt = document.createElement('option');
            opt.value = e.especieID;
            opt.textContent = e.nombre;
            selEspecie.appendChild(opt);
        });
    } catch (e) {
        console.error('Error cargando especies:', e);
    }
};

window.cargarRazas = function() {
    const especieID = parseInt(document.getElementById('petEspecie').value);
    const selRaza = document.getElementById('petRaza');
    selRaza.innerHTML = '<option value="">Selecciona raza</option>';

    if (!especieID) return;

    const filtradas = _razas.filter(r => r.especieID === especieID);
    filtradas.forEach(r => {
        const opt = document.createElement('option');
        opt.value = r.razaID;
        opt.textContent = r.nombre;
        selRaza.appendChild(opt);
    });
};

window.triggerUpload = function() {
    document.getElementById('fileInput').click();
};

window.previewImg = function(event) {
    const file = event.target.files[0];
    if (!file) return;
    _imagenFile = file;
    document.getElementById('imgPlaceholder').innerHTML =
        '<img src="' + URL.createObjectURL(file) + '" style="width:100%;height:100%;object-fit:cover;border-radius:inherit;"/>';
};

window.publishPet = async function() {
    const session = JSON.parse(localStorage.getItem('session') || '{}');
    if (!session.usuarioID) { alert('Sesión no encontrada.'); return; }

    const nombre = document.getElementById('petNameTitle').textContent.trim();
    const especieID = parseInt(document.getElementById('petEspecie').value);
    const razaID = parseInt(document.getElementById('petRaza').value);
    const sexo = document.getElementById('petSexo').value;
    const edad = parseInt(document.getElementById('petEdad').value);
    const color = document.getElementById('petColor').value.trim();
    const personalidad = document.getElementById('petPersonalidad').value.trim();
    const descripcion = document.getElementById('petDescripcion').value.trim();

    if (!nombre || !especieID || !razaID || !sexo) {
        alert('Por favor llena los campos obligatorios (nombre, especie, raza, sexo).');
        return;
    }

    try {
        const admin = await apiGet('/Administradores/por-usuario/' + session.usuarioID);
        const adminsRefugio = await apiGet('/RefugioAdministradores/por-administrador/' + admin.administradorID);
        const refugioID = adminsRefugio[0].refugioID;

        const fechaNac = edad ? new Date() : null;
        if (edad && fechaNac) fechaNac.setFullYear(fechaNac.getFullYear() - edad);

        const descCompleta = (descripcion ? descripcion : '') +
            (personalidad ? '\nPersonalidad: ' + personalidad : '') +
            (color ? '\nColor: ' + color : '');

        const res = await apiPost('/Animales', {
            refugioID: refugioID,
            razaID: razaID,
            nombre: nombre,
            sexo: sexo,
            fechaNacimiento: edad ? fechaNac.toISOString() : null,
            descripcion: descCompleta || 'Sin descripción',
            estatus: 'Disponible'
        });

        const animalID = res.id;

        if (_imagenFile) {
            const formData = new FormData();
            formData.append('archivo', _imagenFile);
            formData.append('entidadTipo', 'Animal');
            formData.append('entidadID', animalID);
            formData.append('orden', '1');
            await apiUploadFile('/Imagenes', formData);
        }

        // Save health check flags as part of description or tags
        const healthTags = [];
        if (document.getElementById('healthVacunas').checked) healthTags.push('Vacunas al día');
        if (document.getElementById('healthEsterilizado').checked) healthTags.push('Esterilizado');
        if (document.getElementById('healthDesparasitado').checked) healthTags.push('Desparasitado');
        if (document.getElementById('healthMicrochip').checked) healthTags.push('Microchip');
        if (document.getElementById('healthRevision').checked) healthTags.push('Revisión veterinaria');

        if (healthTags.length > 0) {
            await apiPut('/Animales/' + animalID, {
                razaID: razaID,
                nombre: nombre,
                sexo: sexo,
                fechaNacimiento: edad ? fechaNac.toISOString() : null,
                descripcion: descCompleta + '\nSalud: ' + healthTags.join(', ') || 'Sin descripción',
                estatus: 'Disponible'
            });
        }

        alert('Mascota publicada correctamente.');
        redirect('06_inicio_refugio');
    } catch (e) {
        console.error('Error al publicar mascota:', e);
        alert('Error al publicar la mascota. Intenta de nuevo.');
    }
};
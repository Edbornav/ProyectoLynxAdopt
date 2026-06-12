let _regFotoFile = null;

window.previewRegFoto = function(event) {
    const file = event.target.files[0];
    if (!file) return;
    _regFotoFile = file;
    document.getElementById('regFotoPreview').innerHTML =
        '<img src="' + URL.createObjectURL(file) + '" style="width:100%;height:100%;object-fit:cover;"/>';
};

async function register() {
    const session = JSON.parse(localStorage.getItem('session') || '{}');
    if (!session.usuarioID) {
        alert('No se encontró tu sesión. Por favor inicia sesión de nuevo.');
        redirect('02_inicio_sesion');
        return;
    }

    const nombre   = document.getElementById('regNombre').value.trim();
    const apPat    = document.getElementById('regApellidoPaterno').value.trim();
    const apMat    = document.getElementById('regApellidoMaterno').value.trim();
    const telefono = document.getElementById('regTelefono').value.trim();
    const fechaNac = document.getElementById('regFechaNacimiento').value;

    if (!nombre || !apPat || !apMat || !telefono) {
        alert('Por favor llena todos los campos obligatorios.');
        return;
    }
    if (!/^\d{10}$/.test(telefono)) {
        alert('El teléfono debe tener exactamente 10 dígitos.');
        return;
    }

    try {
        const formData = new FormData();
        formData.append('usuarioID', session.usuarioID);
        formData.append('nombre', nombre);
        formData.append('apellidoPaterno', apPat);
        formData.append('apellidoMaterno', apMat);
        formData.append('telefono', telefono);
        if (fechaNac) formData.append('fechaNacimiento', fechaNac);
        if (_regFotoFile) formData.append('foto', _regFotoFile);

        await apiUploadFile('/Adoptantes/con-foto', formData);

        const resToken = await apiPut('/Usuarios/' + session.usuarioID, {
            correo: session.correo,
            tipoUsuario: 'Adoptante',
            estatus: 'Activo'
        });

        session.token = resToken.token;
        session.tipoUsuario = 'Adoptante';
        localStorage.setItem('session', JSON.stringify(session));

        redirect('03_catalogo_adoptante');
    } catch (error) {
        console.error('Error al crear adoptante:', error);
        alert('Ocurrió un error al guardar tu perfil. Intenta de nuevo.');
    }
}
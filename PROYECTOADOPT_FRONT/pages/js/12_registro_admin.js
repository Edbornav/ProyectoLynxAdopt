let _refLogoFile = null;

window.previewRefLogo = function(event) {
    const file = event.target.files[0];
    if (!file) return;
    _refLogoFile = file;
    document.getElementById('refLogoPreview').innerHTML =
        '<img src="' + URL.createObjectURL(file) + '" style="width:100%;height:100%;object-fit:cover;"/>';
};

async function registerAdmin() {
    const session = JSON.parse(localStorage.getItem('session') || '{}');
    if (!session.usuarioID) {
        alert('No se encontró tu sesión. Por favor inicia sesión de nuevo.');
        redirect('02_inicio_sesion');
        return;
    }

    const nombre      = document.getElementById('regNombre').value.trim();
    const apPat       = document.getElementById('regApellidoPaterno').value.trim();
    const apMat       = document.getElementById('regApellidoMaterno').value.trim();
    const telefono    = document.getElementById('regTelefono').value.trim();
    const refNombre   = document.getElementById('refNombre').value.trim();
    const refDesc     = document.getElementById('refDescripcion').value.trim();
    const refDir      = document.getElementById('refDireccion').value.trim();
    const refTel      = document.getElementById('refTelefono').value.trim();
    const refCorreo   = document.getElementById('refCorreo').value.trim();

    if (!nombre || !apPat || !apMat || !telefono || !refNombre || !refDir || !refTel) {
        alert('Por favor llena todos los campos obligatorios.');
        return;
    }
    if (!/^\d{10}$/.test(telefono)) {
        alert('El teléfono de contacto debe tener exactamente 10 dígitos.');
        return;
    }
    if (!/^\d{10}$/.test(refTel)) {
        alert('El teléfono del refugio debe tener exactamente 10 dígitos.');
        return;
    }

    try {
        const resToken = await apiPut('/Usuarios/' + session.usuarioID, {
            correo:     session.correo,
            tipoUsuario: 'Administrador',
            estatus:    'Activo'
        });

        session.token = resToken.token;
        session.tipoUsuario = 'Administrador';
        localStorage.setItem('session', JSON.stringify(session));

        const resAdmin = await apiPost('/Administradores', {
            usuarioID:       session.usuarioID,
            nombre:          nombre,
            apellidoPaterno: apPat,
            apellidoMaterno: apMat,
            telefono:        telefono
        });

        const administradorID = resAdmin.id;

        const formData = new FormData();
        formData.append('nombre', refNombre);
        formData.append('descripcion', refDesc || '');
        formData.append('direccion', refDir);
        formData.append('telefono', refTel);
        formData.append('correo', refCorreo || '');
        formData.append('estatus', 'Activo');
        if (_refLogoFile) formData.append('logo', _refLogoFile);

        const resRefugio = await apiUploadFile('/Refugios/con-logo', formData);

        const refugioID = resRefugio.id;

        await apiPost('/RefugioAdministradores', {
            refugioID:       refugioID,
            usuarioAdminID:  administradorID
        });

        redirect('06_inicio_refugio');
    } catch (error) {
        console.error('Error al registrar administrador:', error);
        alert('Ocurrió un error al registrar. Intenta de nuevo.');
    }
}
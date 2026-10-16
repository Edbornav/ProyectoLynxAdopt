let _solicitudActiva = null;

window.initSolicitudesRefugio = async function() {
    const session = JSON.parse(localStorage.getItem('session') || '{}');
    if (!session.usuarioID) { redirect('02_inicio_sesion'); return; }

    try {
        const admin = await apiGet('/Administradores/por-usuario/' + session.usuarioID);
        if (!admin) { redirect('02_inicio_sesion'); return; }

        const adminsRefugio = await apiGet('/RefugioAdministradores/por-administrador/' + admin.administradorID);
        if (adminsRefugio.length === 0) return;

        const refugioID = adminsRefugio[0].refugioID;
        const solicitudes = await apiGet('/SolicitudesAdopcion/por-refugio/' + refugioID);
        const animales = await apiGet('/Animales/por-refugio/' + refugioID);

        const list = document.getElementById('solicitudesList');
        list.innerHTML = '';

        for (const sol of solicitudes) {
            const adoptante = await apiGet('/Adoptantes/' + sol.adoptanteID);
            let animal = null;
            try {
                const solAnimales = await apiGet('/SolicitudesAnimales/' + sol.solicitudID);
                if (solAnimales && solAnimales.length > 0) {
                    animal = animales.find(a => a.animalID === solAnimales[0].animalID);
                }
            } catch (e) {}
            if (!animal) animal = animales.find(a => a.refugioID === sol.refugioID);

            const row = document.createElement('div');
            row.className = 'sol-row';
            row.innerHTML =
                '<div class="sol-avatar">👤</div>' +
                '<div class="sol-name">' + (adoptante ? adoptante.nombre + ' ' + adoptante.apellidoPaterno : '--') + '</div>' +
                '<div class="sol-pet">' + (animal ? animal.nombre : '--') + '</div>' +
                '<div class="action-row"><button class="action-btn btn-accept" onclick="event.stopPropagation();aprobacionRapida(' + sol.solicitudID + ')">✅</button><button class="action-btn btn-reject" onclick="event.stopPropagation();rechazoRapido(' + sol.solicitudID + ')">❌</button></div>' +
                '<div style="text-align:right;"><span class="status-badge status-' + (sol.estatus === 'Pendiente' ? 'pending' : sol.estatus === 'Aprobada' ? 'approved' : 'rejected') + '">' + sol.estatus + '</span></div>';

            row.onclick = function() { verPerfilAdoptante(sol.solicitudID); };
            list.appendChild(row);
        }
    } catch (e) {
        console.error('Error cargando solicitudes:', e);
    }
};

window.aprobacionRapida = async function(solicitudID) {
    try {
        await apiPut('/SolicitudesAdopcion/' + solicitudID, { estatus: 'Aprobada' });
        window.initSolicitudesRefugio();
    } catch (e) {
        console.error('Error al aprobar:', e);
    }
};

window.rechazoRapido = async function(solicitudID) {
    try {
        await apiPut('/SolicitudesAdopcion/' + solicitudID, { estatus: 'Rechazada' });
        window.initSolicitudesRefugio();
    } catch (e) {
        console.error('Error al rechazar:', e);
    }
};

window.verPerfilAdoptante = async function(solicitudID) {
    try {
        const sol = await apiGet('/SolicitudesAdopcion/' + solicitudID);
        _solicitudActiva = sol;
        const adoptante = await apiGet('/Adoptantes/' + sol.adoptanteID);

        document.getElementById('modalName').textContent = (adoptante ? adoptante.nombre + ' ' + adoptante.apellidoPaterno + ' ' + adoptante.apellidoMaterno : '--');
        document.getElementById('modalStatus').textContent = sol.estatus;
        document.getElementById('modalStatus').className = 'status-badge status-' + (sol.estatus === 'Pendiente' ? 'pending' : sol.estatus === 'Aprobada' ? 'approved' : 'rejected');
        document.getElementById('modalMensaje').textContent = sol.mensajeAdoptante || 'Sin mensaje';

        const infoGrid = document.getElementById('modalInfoGrid');
        infoGrid.innerHTML =
            '<div><strong>📞 Teléfono:</strong><br>' + (adoptante ? adoptante.telefono : '--') + '</div>' +
            '<div><strong>🎂 Fecha de nacimiento:</strong><br>' + (adoptante && adoptante.fechaNacimiento ? adoptante.fechaNacimiento.split('T')[0] : 'N/D') + '</div>';

        document.getElementById('modalOverlay').style.display = 'flex';
    } catch (e) {
        console.error('Error cargando perfil:', e);
    }
};

window.cerrarModalPerfil = function() {
    document.getElementById('modalOverlay').style.display = 'none';
};

window.aprobarSolicitud = async function() {
    if (!_solicitudActiva) return;
    try {
        await apiPut('/SolicitudesAdopcion/' + _solicitudActiva.solicitudID, { estatus: 'Aprobada' });
        alert('Solicitud aprobada.');
        cerrarModalPerfil();
        window.initSolicitudesRefugio();
    } catch (e) {
        console.error('Error al aprobar:', e);
    }
};

window.rechazarSolicitud = async function() {
    if (!_solicitudActiva) return;
    try {
        await apiPut('/SolicitudesAdopcion/' + _solicitudActiva.solicitudID, { estatus: 'Rechazada' });
        alert('Solicitud rechazada.');
        cerrarModalPerfil();
        window.initSolicitudesRefugio();
    } catch (e) {
        console.error('Error al rechazar:', e);
    }
};
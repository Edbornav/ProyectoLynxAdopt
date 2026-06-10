window.initInicioRefugio = async function() {
    const session = JSON.parse(localStorage.getItem('session') || '{}');
    if (!session.usuarioID) { redirect('02_inicio_sesion'); return; }

    try {
        const admin = await apiGet('/Administradores/por-usuario/' + session.usuarioID);
        if (!admin) { redirect('02_inicio_sesion'); return; }

        const adminsRefugio = await apiGet('/RefugioAdministradores/por-administrador/' + admin.administradorID);
        if (adminsRefugio.length === 0) { redirect('12_registro_admin'); return; }

        const refugioID = adminsRefugio[0].refugioID;
        const refugio = await apiGet('/Refugios/' + refugioID);

        document.getElementById('refNombre').textContent = refugio.nombre;
        document.getElementById('refDescripcion').textContent = refugio.descripcion || 'Sin descripción';

        const animales = await apiGet('/Animales/por-refugio/' + refugioID);
        document.getElementById('statMascotas').textContent = animales.length;
        document.getElementById('statAdoptados').textContent = animales.filter(a => a.estatus === 'Adoptado').length;

        const solicitudes = await apiGet('/SolicitudesAdopcion/por-refugio/' + refugioID);
        document.getElementById('statSolicitudes').textContent = solicitudes.length;
        document.getElementById('notifBadge').textContent = solicitudes.filter(s => s.estatus === 'Pendiente').length;

        const grid = document.getElementById('refugioPetsGrid');
        grid.innerHTML = '';
        animales.forEach(a => {
            const card = document.createElement('div');
            card.className = 'pet-card';
            card.innerHTML = '<div class="pet-img">🐾</div>' +
                '<div class="pet-info">' +
                '<div class="pet-name">' + a.nombre + '</div>' +
                '<div class="pet-meta"><span>' + a.sexo + '</span></div>' +
                '<div class="pet-status ' + (a.estatus === 'Disponible' ? 'status-disponible' : 'status-otro') + '">' + a.estatus + '</div>' +
                '</div>';
            grid.appendChild(card);
        });
    } catch (e) {
        console.error('Error cargando inicio refugio:', e);
    }
};
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

        try {
            const imagenesRef = await apiGet(`/Imagen?entidadTipo=Refugio&entidadId=${refugioID}`);
            if (imagenesRef.length > 0) {
                const logoEl = document.getElementById('refLogo');
                if (logoEl) logoEl.innerHTML = `<img src="${imagenesRef[0].url}" alt="${refugio.nombre}" style="width:100%;height:100%;object-fit:cover;border-radius:12px;" onerror="this.outerHTML='🏠'"/>`;
                const photoEl = document.getElementById('refPhotoCol');
                if (photoEl) photoEl.innerHTML = `<img src="${imagenesRef[0].url}" alt="${refugio.nombre}" style="width:100%;height:100%;object-fit:cover;border-radius:12px;" onerror="this.outerHTML='🐾'"/>`;
            }
        } catch (e) {}

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
            card.innerHTML = '<div class="pet-img">' + (a.fotoUrl ? '<img src="' + a.fotoUrl + '" alt="' + a.nombre + '" style="width:100%;height:100%;object-fit:cover;border-radius:inherit;" onerror="this.outerHTML=\'🐾\'"/>' : '🐾') + '</div>' +
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
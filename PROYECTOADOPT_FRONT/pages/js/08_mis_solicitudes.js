window.initMisSolicitudes = async function() {
    const session = JSON.parse(localStorage.getItem('session') || '{}');
    if (!session.usuarioID) { redirect('02_inicio_sesion'); return; }

    try {
        const adoptante = await apiGet('/Adoptantes/por-usuario/' + session.usuarioID);
        if (!adoptante) { redirect('04_registro'); return; }

        const solicitudes = await apiGet('/SolicitudesAdopcion/por-adoptante/' + adoptante.adoptanteID);

        const list = document.getElementById('solicitudesList');
        list.innerHTML = '';

        if (solicitudes.length === 0) {
            list.innerHTML = '<div style="text-align:center;padding:40px;color:var(--text-mid);font-weight:600;">No has realizado ninguna solicitud de adopción aún.</div>';
            return;
        }

        const animales = await apiGet('/Animales');
        const refugios = await apiGet('/Refugios');

        for (const sol of solicitudes) {
            let animal = null;
            try {
                const solAnimales = await apiGet('/SolicitudesAnimales/' + sol.solicitudID);
                if (solAnimales && solAnimales.length > 0) {
                    animal = animales.find(a => a.animalID === solAnimales[0].animalID);
                }
            } catch (e) {}
            if (!animal) animal = animales.find(a => a.refugioID === sol.refugioID);
            const refugio = refugios.find(r => r.refugioID === sol.refugioID);

            const row = document.createElement('div');
            row.className = 'solicitud-row';
            row.innerHTML = '<div class="sol-avatar">🐾</div>' +
                '<div class="sol-name">' + (animal ? animal.nombre : '--') + '</div>' +
                '<div style="flex:1;text-align:right;display:flex;gap:20px;justify-content:flex-end;align-items:center;">' +
                '<span style="font-size:0.85rem;color:var(--text-mid);">' + (refugio ? refugio.nombre : '') + '</span>' +
                '<span class="status-badge status-' + (sol.estatus === 'Pendiente' ? 'pending' : sol.estatus === 'Aprobada' ? 'approved' : 'rejected') + '">' + sol.estatus + '</span>' +
                '</div>';
            list.appendChild(row);
        }
    } catch (e) {
        console.error('Error cargando mis solicitudes:', e);
    }
};
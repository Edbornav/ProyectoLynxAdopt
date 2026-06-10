let _detalleAnimal = null;

window.initDetalle = async function() {
    const animalID = parseInt(routeParams.id || routeParams.animalId);
    if (!animalID) { redirect('03_catalogo_adoptante'); return; }

    try {
        const animal = await apiGet('/Animales/' + animalID);
        _detalleAnimal = animal;

        const refugio = await apiGet('/Refugios/' + animal.refugioID);
        document.getElementById('detailRefugioBadge').textContent = '🏠 ' + refugio.nombre;

        const imagenes = await apiGet('/Imagenes?entidadTipo=Animal&entidadId=' + animalID);
        if (imagenes.length > 0) {
            document.getElementById('detailImgCol').innerHTML =
                '<img src="' + imagenes[0].url + '" alt="' + animal.nombre + '" style="width:100%;height:100%;object-fit:cover;border-radius:inherit;"/>' +
                '<div class="refugio-badge" id="detailRefugioBadge">🏠 ' + refugio.nombre + '</div>';
        }

        const razas = await apiGet('/Razas');
        const raza = razas.find(r => r.razaID === animal.razaID);

        document.getElementById('detailPetName').textContent = animal.nombre;
        document.getElementById('detailEstatus').textContent = animal.estatus;
        document.getElementById('detailRaza').textContent = raza ? raza.nombre : '--';
        document.getElementById('detailEdad').textContent = animal.fechaNacimiento ? calcularEdad(animal.fechaNacimiento) + ' años' : 'N/D';
        document.getElementById('detailSexo').textContent = animal.sexo;
        document.getElementById('detailStory').textContent = animal.descripcion;
    } catch (e) {
        console.error('Error cargando detalle:', e);
    }
};

window.abrirModal = function() {
    document.getElementById('modalOverlay').style.display = 'flex';
};

window.cerrarModal = function() {
    document.getElementById('modalOverlay').style.display = 'none';
};

window.enviarSolicitud = async function() {
    const session = JSON.parse(localStorage.getItem('session') || '{}');
    if (!session.usuarioID) {
        alert('Debes iniciar sesión para adoptar.');
        redirect('02_inicio_sesion');
        return;
    }

    try {
        const adoptante = await apiGet('/Adoptantes/por-usuario/' + session.usuarioID);
        if (!adoptante) {
            alert('Completa tu perfil de adoptante antes de solicitar una adopción.');
            redirect('04_registro');
            return;
        }

        const res = await apiPost('/SolicitudesAdopcion', {
            refugioID: _detalleAnimal.refugioID,
            adoptanteID: adoptante.adoptanteID,
            mensajeAdoptante: 'Solicitud de adopción para ' + _detalleAnimal.nombre
        });

        await apiPost('/SolicitudesAnimales', {
            solicitudID: res.id,
            animalID: _detalleAnimal.animalID
        });

        alert('Solicitud enviada correctamente.');
        cerrarModal();
        redirect('08_mis_solicitudes');
    } catch (e) {
        console.error('Error al enviar solicitud:', e);
        alert('Error al enviar la solicitud. Intenta de nuevo.');
    }
};

function calcularEdad(fechaNacimiento) {
    const hoy = new Date();
    const nacimiento = new Date(fechaNacimiento);
    const años = hoy.getFullYear() - nacimiento.getFullYear();
    const meses = hoy.getMonth() - nacimiento.getMonth();
    return meses < 0 || (meses === 0 && hoy.getDate() < nacimiento.getDate()) ? años - 1 : años;
}
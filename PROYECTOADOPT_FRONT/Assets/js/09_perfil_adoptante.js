const API_BASE_URL = 'https://localhost:7143/api';

let adoptanteData   = null;
let perfilData      = null;

// ── Helpers ────────────────────────────────────────────────
function getUsuarioID() {
    return localStorage.getItem('adoptanteUsuarioID')
        || sessionStorage.getItem('adoptanteUsuarioID');
}

function redirect(page) {
    window.location.href = `#${page}`;
}

// ── Cargar datos ───────────────────────────────────────────
async function cargarPerfil() {
    const usuarioID = getUsuarioID();

    if (!usuarioID) {
        alert('No se encontró tu sesión. Por favor inicia sesión.');
        window.location.href = '#02_inicio_sesion';
        return;
    }

    try {
        const [resAdoptante, resPerfil] = await Promise.all([
            fetch(`${API_BASE_URL}/Adoptantes/${usuarioID}`),
            fetch(`${API_BASE_URL}/PerfilesAdoptante/${usuarioID}`)
        ]);

        adoptanteData = resAdoptante.ok ? await resAdoptante.json() : null;
        perfilData    = resPerfil.ok    ? await resPerfil.json()    : null;

        renderNombre();
        renderInfoPersonal();
        renderPerfilAdoptante();

    } catch (err) {
        console.error('Error cargando perfil:', err);
        alert('❌ Error de conexión. Verifica que el servidor esté corriendo.');
    }
}

// ── Render nombre y rol ────────────────────────────────────
function renderNombre() {
    if (!adoptanteData) return;

    document.getElementById('profileName').textContent =
        `${adoptanteData.nombre} ${adoptanteData.apellidoPaterno}`;
    document.getElementById('profileRole').textContent = 'Adoptante';
}

// ── Render información personal ────────────────────────────
function renderInfoPersonal() {
    const grid = document.getElementById('profileInfoGrid');

    if (!adoptanteData) {
        grid.innerHTML = '<p style="color:var(--text-mid)">No se encontró información.</p>';
        return;
    }

    const fecha = adoptanteData.fechaNacimiento
        ? new Date(adoptanteData.fechaNacimiento).toLocaleDateString('es-MX')
        : 'No registrada';

    grid.innerHTML = `
        <div class="info-item">
            <label>Nombre(s)</label>
            <input type="text" id="editNombre" value="${adoptanteData.nombre}"/>
        </div>
        <div class="info-item">
            <label>Apellido paterno</label>
            <input type="text" id="editApellidoPaterno" value="${adoptanteData.apellidoPaterno}"/>
        </div>
        <div class="info-item">
            <label>Apellido materno</label>
            <input type="text" id="editApellidoMaterno" value="${adoptanteData.apellidoMaterno}"/>
        </div>
        <div class="info-item">
            <label>Teléfono</label>
            <input type="tel" id="editTelefono" value="${adoptanteData.telefono}"/>
        </div>
        <div class="info-item">
            <label>Fecha de nacimiento</label>
            <input type="date" id="editFechaNacimiento"
                value="${adoptanteData.fechaNacimiento
                    ? adoptanteData.fechaNacimiento.split('T')[0]
                    : ''}"/>
        </div>
    `;
}

// ── Render perfil adoptante ────────────────────────────────
function renderPerfilAdoptante() {
    const section = document.getElementById('profilePerfilSection');

    if (!perfilData) {
        section.innerHTML = `
            <p style="color:var(--text-mid);margin-bottom:12px;">
                Aún no has completado tu perfil de adoptante.
            </p>
            <button class="btn btn-orange btn-sm" onclick="redirect('04b_test_perfil')">
                ✏️ Completar perfil
            </button>`;
        return;
    }

    section.innerHTML = `
        <div class="info-item full-col">
            <label>🏠 Descripción de tu casa</label>
            <textarea id="editDescCasa" rows="3">${perfilData.descripcionCasa}</textarea>
        </div>
        <div class="info-item full-col">
            <label>🐶 ¿Tienes otras mascotas?</label>
            <textarea id="editDescMascotas" rows="3">${perfilData.descripcionMascotas}</textarea>
        </div>
        <div class="info-item full-col">
            <label>⭐ Experiencia con mascotas</label>
            <textarea id="editDescExperiencia" rows="3">${perfilData.descripcionExperienciaConMascotas}</textarea>
        </div>
    `;
}

// ── Guardar cambios ────────────────────────────────────────
async function guardarCambios() {
    const usuarioID = getUsuarioID();

    const nombre          = document.getElementById('editNombre').value.trim();
    const apellidoPaterno = document.getElementById('editApellidoPaterno').value.trim();
    const apellidoMaterno = document.getElementById('editApellidoMaterno').value.trim();
    const telefono        = document.getElementById('editTelefono').value.trim();
    const fechaNacimiento = document.getElementById('editFechaNacimiento').value;

    if (!nombre || !apellidoPaterno || !apellidoMaterno || !telefono) {
        alert('Por favor llena todos los campos obligatorios.');
        return;
    }

    try {
        // Actualizar adoptante
        const resAdoptante = await fetch(`${API_BASE_URL}/Adoptantes/${adoptanteData.adoptanteID}`, {
            method: 'PUT',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({
                usuarioID:       parseInt(usuarioID),
                nombre,
                apellidoPaterno,
                apellidoMaterno,
                telefono,
                fechaNacimiento: fechaNacimiento || null
            })
        });

        if (!resAdoptante.ok) {
            alert('Error al actualizar información personal.');
            return;
        }

        // Actualizar perfil adoptante si existe
        if (perfilData) {
            const descCasa       = document.getElementById('editDescCasa').value.trim();
            const descMascotas   = document.getElementById('editDescMascotas').value.trim();
            const descExperiencia= document.getElementById('editDescExperiencia').value.trim();

            const resPerfil = await fetch(`${API_BASE_URL}/PerfilesAdoptante/${perfilData.perfilAdoptanteID}`, {
                method: 'PUT',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({
                    descripcionCasa:                   descCasa,
                    descripcionMascotas:               descMascotas,
                    descripcionExperienciaConMascotas: descExperiencia
                })
            });

            if (!resPerfil.ok) {
                alert('Error al actualizar perfil de adoptante.');
                return;
            }
        }

        alert('✅ Cambios guardados correctamente.');
        await cargarPerfil(); // recarga los datos actualizados

    } catch (err) {
        console.error('Error guardando cambios:', err);
        alert('❌ Error de conexión. Verifica que el servidor esté corriendo.');
    }
}

// ── Init ───────────────────────────────────────────────────
document.addEventListener('DOMContentLoaded', cargarPerfil);
const _session = JSON.parse(localStorage.getItem('session') || '{}');
if (!_session.adoptanteId) { redirect('02_inicio_sesion'); }

let _perfilData = null;

(async function() {
  try {
    const [adoptante, perfiles] = await Promise.all([
      apiGet(`/Adoptantes/${_session.adoptanteId}`),
      apiGet('/PerfilesAdoptante')
    ]);

    document.getElementById('profileName').textContent =
      `${adoptante.nombre} ${adoptante.apellidoPaterno} ${adoptante.apellidoMaterno}`;

    document.getElementById('profileInfoGrid').innerHTML = `
      <div class="fg"><label>Nombre</label><input id="edtNombre" value="${escapeHtml(adoptante.nombre)}"/></div>
      <div class="fg"><label>Apellido Paterno</label><input id="edtApellidoP" value="${escapeHtml(adoptante.apellidoPaterno)}"/></div>
      <div class="fg"><label>Apellido Materno</label><input id="edtApellidoM" value="${escapeHtml(adoptante.apellidoMaterno)}"/></div>
      <div class="fg"><label>Teléfono</label><input id="edtTelefono" value="${escapeHtml(adoptante.telefono)}"/></div>
      <div class="fg"><label>Fecha de Nacimiento</label><input id="edtFechaNac" type="date" value="${adoptante.fechaNacimiento ? adoptante.fechaNacimiento.split('T')[0] : ''}"/></div>
    `;

    _perfilData = perfiles.find(p => p.adoptanteUsuarioID === _session.usuarioId) || null;

    document.getElementById('profilePerfilSection').innerHTML = `
      <div class="perfil-card">
        <div class="q">🏠 Describe tu casa</div>
        <textarea id="edtCasa" class="option-textarea" rows="2">${escapeHtml(_perfilData ? _perfilData.descripcionCasa : '')}</textarea>
      </div>
      <div class="perfil-card">
        <div class="q">🐱 ¿Tienes mascotas actualmente?</div>
        <textarea id="edtMascotas" class="option-textarea" rows="2">${escapeHtml(_perfilData ? _perfilData.descripcionMascotas : '')}</textarea>
      </div>
      <div class="perfil-card">
        <div class="q">💪 Experiencia con mascotas</div>
        <textarea id="edtExperiencia" class="option-textarea" rows="2">${escapeHtml(_perfilData ? _perfilData.descripcionExperienciaConMascotas : '')}</textarea>
      </div>
    `;

    try {
      const solicitudes = await apiGet(`/SolicitudesAdopcion/por-adoptante/${_session.adoptanteId}`);
      document.getElementById('statSolicitudes').textContent = solicitudes.length;
      document.getElementById('statAdoptados').textContent = solicitudes.filter(s => s.estatus === 'Aprobada' || s.estatus === 'Aprobado').length;
    } catch (e) {
      try {
        const todas = await apiGet('/SolicitudesAdopcion');
        const misSol = todas.filter(s => s.adoptanteID === _session.adoptanteId);
        document.getElementById('statSolicitudes').textContent = misSol.length;
        document.getElementById('statAdoptados').textContent = misSol.filter(s => s.estatus === 'Aprobada' || s.estatus === 'Aprobado').length;
      } catch (e2) {}
    }
  } catch (e) {
    console.error('Error cargando perfil:', e);
  }
})();

window.guardarCambios = async function() {
  try {
    await apiPut(`/Adoptantes/${_session.adoptanteId}`, {
      nombre: document.getElementById('edtNombre').value.trim(),
      apellidoPaterno: document.getElementById('edtApellidoP').value.trim(),
      apellidoMaterno: document.getElementById('edtApellidoM').value.trim(),
      telefono: document.getElementById('edtTelefono').value.trim(),
      fechaNacimiento: document.getElementById('edtFechaNac').value || null
    });

    const perfilDto = {
      descripcionCasa: document.getElementById('edtCasa').value.trim(),
      descripcionMascotas: document.getElementById('edtMascotas').value.trim(),
      descripcionExperienciaConMascotas: document.getElementById('edtExperiencia').value.trim()
    };

    if (_perfilData) {
      await apiPut(`/PerfilesAdoptante/${_perfilData.perfilAdoptanteID}`, perfilDto);
    } else {
      await apiPost('/PerfilesAdoptante', { ...perfilDto, adoptanteUsuarioID: _session.usuarioId });
    }

    alert('Cambios guardados correctamente');
  } catch (e) {
    alert('Error al guardar: ' + e.message);
  }
};

window.cerrarSesion = function() {
  localStorage.removeItem('session');
  redirect('01_seleccion_rol');
};

function escapeHtml(str) {
  if (!str) return '';
  return str.replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;');
}

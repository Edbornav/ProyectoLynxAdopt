let _perfilData = null;
let _adoptanteID = null;
let _session = null;

window.initPerfilAdoptante = async function() {
  _session = JSON.parse(localStorage.getItem('session') || '{}');
  if (!_session.usuarioID) { redirect('02_inicio_sesion'); return; }

  try {
    const resp = await apiGet('/Adoptantes/por-usuario/' + _session.usuarioID);
    if (!resp) { redirect('04_registro'); return; }
    _adoptanteID = resp.adoptanteID;

    const [perfiles] = await Promise.all([
      apiGet('/PerfilesAdoptante')
    ]);

    document.getElementById('profileName').textContent =
      `${resp.nombre} ${resp.apellidoPaterno} ${resp.apellidoMaterno}`;

    document.getElementById('profileInfoGrid').innerHTML = `
      <div class="fg"><label>Nombre</label><input id="edtNombre" value="${escapeHtml(resp.nombre)}"/></div>
      <div class="fg"><label>Apellido Paterno</label><input id="edtApellidoP" value="${escapeHtml(resp.apellidoPaterno)}"/></div>
      <div class="fg"><label>Apellido Materno</label><input id="edtApellidoM" value="${escapeHtml(resp.apellidoMaterno)}"/></div>
      <div class="fg"><label>Teléfono</label><input id="edtTelefono" value="${escapeHtml(resp.telefono)}"/></div>
      <div class="fg"><label>Fecha de Nacimiento</label><input id="edtFechaNac" type="date" value="${resp.fechaNacimiento ? resp.fechaNacimiento.split('T')[0] : ''}"/></div>
    `;

    _perfilData = perfiles.find(p => p.adoptanteUsuarioID === _adoptanteID) || null;

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
      const solicitudes = await apiGet(`/SolicitudesAdopcion/por-adoptante/${_adoptanteID}`);
      document.getElementById('statSolicitudes').textContent = solicitudes.length;
      document.getElementById('statAdoptados').textContent = solicitudes.filter(s => s.estatus === 'Aprobada' || s.estatus === 'Aprobado').length;
    } catch (e) {
      try {
        const todas = await apiGet('/SolicitudesAdopcion');
        const misSol = todas.filter(s => s.adoptanteID === _adoptanteID);
        document.getElementById('statSolicitudes').textContent = misSol.length;
        document.getElementById('statAdoptados').textContent = misSol.filter(s => s.estatus === 'Aprobada' || s.estatus === 'Aprobado').length;
      } catch (e2) {}
    }
  } catch (e) {
    console.error('Error cargando perfil:', e);
  }
};

window.guardarCambios = async function() {
  if (!_adoptanteID) {
    alert('Perfil no cargado. Intenta de nuevo.');
    return;
  }
  try {
    await apiPut(`/Adoptantes/${_adoptanteID}`, {
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
      await apiPost('/PerfilesAdoptante', { ...perfilDto, adoptanteUsuarioID: _adoptanteID });
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

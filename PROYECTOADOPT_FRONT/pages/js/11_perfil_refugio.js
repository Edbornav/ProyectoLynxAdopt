window.initPerfilRefugio = async function() {
  const refugioId = parseInt(routeParams.refugioId || routeParams.id, 10);
  if (!refugioId) { document.getElementById('refNombre').textContent = 'Refugio no especificado'; return; }

  try {
    const [refugio, animales, miembros, imagenesRefugio] = await Promise.all([
      apiGet(`/Refugios/${refugioId}`),
      apiGet(`/Animales/por-refugio/${refugioId}`),
      apiGet(`/RefugioAdministradores/${refugioId}`),
      apiGet(`/Imagen?entidadTipo=Refugio&entidadId=${refugioId}`).catch(() => [])
    ]);

    document.getElementById('refNombre').textContent = refugio.nombre;

    const logo = imagenesRefugio.length > 0 ? imagenesRefugio[0].url : null;
    const logoEl = document.getElementById('refLogo');
    if (logo && logoEl) {
      logoEl.innerHTML = `<img src="${logo}" alt="${refugio.nombre}" style="width:100%;height:100%;object-fit:cover;border-radius:12px;" onerror="this.outerHTML='🏠'"/>`;
    }
    document.getElementById('refDireccion').textContent = refugio.direccion;
    document.getElementById('refTelefono').textContent = refugio.telefono;
    document.getElementById('refEmail').textContent = refugio.correo;
    document.getElementById('refDescripcion').textContent = refugio.descripcion;

    document.getElementById('refStatMascotas').textContent = animales.length;
    document.getElementById('refStatAdoptados').textContent = animales.filter(a => a.estatus === 'Adoptado').length;
    document.getElementById('refStatMiembros').textContent = miembros.length;

    let razas = [];
    try {
      razas = await apiGet('/Razas');
    } catch (e) {}

    const grid = document.getElementById('refPetsGrid');
    if (animales.length === 0) {
      grid.innerHTML = '<p style="grid-column:1/-1;color:var(--text-mid);font-weight:600;">No hay animales registrados</p>';
    } else {
      grid.innerHTML = animales.map(a => {
        const raza = razas.find(r => r.razaID === a.razaID);
        const edad = a.fechaNacimiento ? calcularEdad(a.fechaNacimiento) : 'Desconocida';
        const emoji = a.sexo === 'Hembra' ? '🐱' : '🐶';
        return `
          <a href="#05_detalle_mascota?id=${a.animalID}" class="pet-card">
            <div class="pet-card-img">${a.fotoUrl ? `<img src="${a.fotoUrl}" alt="${a.nombre}" style="width:100%;height:100%;object-fit:cover;border-radius:inherit;" onerror="this.outerHTML='${emoji}'"/>` : emoji}</div>
            <div class="pet-card-body">
              <div class="pet-name">${escapeHtml(a.nombre)}</div>
              <div class="pet-meta">${raza ? escapeHtml(raza.nombre) : 'Raza desconocida'} · ${edad}</div>
            </div>
          </a>
        `;
      }).join('');
    }
  } catch (e) {
    console.error('Error cargando refugio:', e);
    document.getElementById('refNombre').textContent = 'Error al cargar';
  }
};

function calcularEdad(fechaStr) {
  const nac = new Date(fechaStr);
  const hoy = new Date();
  let edad = hoy.getFullYear() - nac.getFullYear();
  const m = hoy.getMonth() - nac.getMonth();
  if (m < 0 || (m === 0 && hoy.getDate() < nac.getDate())) edad--;
  return edad + (edad === 1 ? ' año' : ' años');
}

function escapeHtml(str) {
  if (!str) return '';
  return str.replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;');
}

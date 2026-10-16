const app = document.getElementById('app');
let routeParams = {};

const pageInits = {
  '03_catalogo_adoptante': 'initCatalogo',
  '04b_test_perfil': 'initTestPerfil',
  '05_detalle_mascota': 'initDetalle',
  '06_inicio_refugio': 'initInicioRefugio',
  '07_solicitudes_refugio': 'initSolicitudesRefugio',
  '08_mis_solicitudes': 'initMisSolicitudes',
  '09_perfil_adoptante': 'initPerfilAdoptante',
  '10_agregar_mascota': 'initAgregarMascota',
  '11_perfil_refugio': 'initPerfilRefugio'
};

function redirect(page) {
  history.pushState(null, '', '#' + page);
  navigate('#' + page);
}

function loadFragment(url) {
  return fetch(url)
    .then(res => { if (!res.ok) throw new Error(`Error cargando ${url}`); return res.text(); })
    .then(html => {
      const temp = document.createElement('div');
      temp.innerHTML = html;
      const scripts = temp.querySelectorAll('script');
      scripts.forEach(s => {
        const ns = document.createElement('script');
        if (s.src) { ns.src = s.src; ns.async = false; }
        else { ns.textContent = s.textContent; }
        s.replaceWith(ns);
      });
      app.innerHTML = '';
      while (temp.firstChild) app.appendChild(temp.firstChild);
    });
}

const centeredPages = ['01_seleccion_rol', '02_inicio_sesion', '02_Registrarse', '04_registro', '12_registro_admin'];

function navigate(hash) {
  const cleaned = hash.replace('#', '');
  const qIdx = cleaned.indexOf('?');
  let page, qs;
  if (qIdx >= 0) {
    page = cleaned.substring(0, qIdx);
    qs = cleaned.substring(qIdx + 1);
    routeParams = Object.fromEntries(new URLSearchParams(qs));
  } else {
    page = cleaned;
    routeParams = {};
  }
  if (centeredPages.includes(page)) {
    app.classList.add('centered');
  } else {
    app.classList.remove('centered');
  }
  loadFragment('pages/' + page + '.html').then(() => {
    const fnName = pageInits[page];
    if (fnName && window[fnName]) window[fnName]();
  });
}

document.addEventListener('click', e => {
  const link = e.target.closest('a[href^="#"]');
  if (link) {
    e.preventDefault();
    const hash = link.getAttribute('href');
    history.pushState(null, '', hash);
    navigate(hash);
  }
});

window.addEventListener('popstate', () => navigate(window.location.hash));

navigate(window.location.hash || '#02_inicio_sesion');
const app = document.getElementById('app');
let routeParams = {};

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

const centeredPages = ['01_seleccion_rol', '02_inicio_sesion', '04_registro'];

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
  loadFragment('pages/' + page + '.html');
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

navigate(window.location.hash || '#01_seleccion_rol');
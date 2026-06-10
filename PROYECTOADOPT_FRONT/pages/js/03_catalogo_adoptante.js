

let todosLosAnimales =[]; //guarda a los animales que se obtiene de la api 
let refugioActivo=null; // 
let especiesFiltradas=[]; 
let _razas = [];
let _refugios = [];

async function fetchAnimales() {
    const res = await fetch (`${API_BASE_URL}/Animales`);
    return await res.json();

}

async function fetchRefugios() {
    const res =await fetch (`${API_BASE_URL}/Refugios`);
    return await res.json();

}

async function fetchEspecies(){
    const res = await fetch (`${API_BASE_URL}/Especies`);
    return await res.json();

}

async function fetchRazas(){
    const res = await fetch (`${API_BASE_URL}/Razas`);
    return await res.json();

}

// Renderiza los refugios como chips seleccionables en la interfaz 
function renderRefugios(refugios){
    const container = document.getElementById('refugioSelector');
    container.innerHTML='';

    const chipTodos = document.createElement('div');
    chipTodos.className='refugio-chip active';
    chipTodos.textContent='Todos';
    chipTodos.onclick=()=> seleccionarRefugio(null, chipTodos);
    container.appendChild(chipTodos);

    refugios.forEach(refugio=>{
        const chip = document.createElement('div');
        chip.className = 'refugio-chip';
        chip.textContent = refugio.nombre;
        chip.onclick = () => seleccionarRefugio(refugio.refugioID, chip);
        container.appendChild(chip);

    });

}

// Renderiza los animales filtrados en la interfaz segun el refugio seleccionado
function seleccionarRefugio(id, chip){
    refugioActivo = id;
    document.querySelectorAll('.refugio-chip').forEach(c=> c.classList.remove('active'));
    chip.classList.add('active');
    renderAnimales(filtrarAnimales());

}

// Renderiza las especies como checkboxes para filtrar los animales por especie
function renderEspeciesFiltros(especies){
    const container = document.getElementById('especiesFiltros');
    container.innerHTML='';

    especies.forEach(e =>{
        const label =document.createElement('label');
        label.className='especie-filtro';
        label.innerHTML=`<input type="checkbox" value="${e.especieID}" onchange="onEspecieChange()"/>${e.nombre}`;
        container.appendChild(label);
    });
}

function verDetalle(animalID){
    redirect('05_detalle_mascota?id=' + animalID);
}

window.initCatalogo = async function init(){
    try {
        const [animales, refugios, especies, razas] = await Promise.all([
            fetchAnimales(), fetchRefugios(), fetchEspecies(), fetchRazas()
        ]);
        todosLosAnimales = animales;
        _razas = razas;
        _refugios = refugios;
        renderRefugios(refugios);
        renderEspeciesFiltros(especies);
        renderAnimales(animales);
    } catch (error) {
        console.error('Error al cargar el catálogo:', error);
    }
};

// Filtra los animales segun el refugio activo y las especies seleccionadas
function onEspecieChange(){
    especiesFiltradas = Array.from(document.querySelectorAll('#especiesFiltros input:checked')).map(i=>parseInt(i.value));
}

//logica para filtrar los animales segun el refugio activo, las especies seleccionadas y la edad máxima ingresada por el usuario
function filtrarAnimales(){
    const edadMax = parseInt(document.getElementById('ageRange').value) || Infinity;

    const razasDeEspecies = new Set(_razas.filter(r => especiesFiltradas.includes(r.especieID)).map(r => r.razaID));

    return todosLosAnimales.filter(a=>{
    const porRefugio = refugioActivo === null || a.refugioID === refugioActivo;
    const porEspecie = especiesFiltradas.length ===0 || razasDeEspecies.has(a.razaID);
    const edad = a.fechaNacimiento ? calcularEdad(a.fechaNacimiento):0;
    const porEdad = edad <= edadMax;
    return porRefugio && porEspecie && porEdad;
    
    });

}

function aplicarFiltros(){
    renderAnimales(filtrarAnimales());
}

function calcularEdad(fechaNacimiento){
    const hoy = new Date();
    const nacimiento = new Date(fechaNacimiento);
    const años = hoy.getFullYear() - nacimiento.getFullYear();
    const meses = hoy.getMonth() - nacimiento.getMonth();
    return meses <0 ||(meses ===0 && hoy.getDate() < nacimiento.getDate()) ? años -1 : años;
}

function renderAnimales(animales){
    const grid = document.getElementById('petsGrid');
    grid.innerHTML='';

    if (animales.length === 0) {
        grid.innerHTML = `
            <div style="grid-column:1/-1;text-align:center;padding:40px;color:var(--text-mid);">
            No se encontraron animales con esos filtros.
            </div>`;
        return;
    }

    animales.forEach(a => {
        const card = document.createElement('div');
        card.className = 'pet-card';
        card.innerHTML = `
            <div class="pet-img">
                ${a.fotoUrl
                    ? `<img src="${a.fotoUrl}" alt="${a.nombre}" style="width:100%;height:100%;object-fit:cover;border-radius:inherit;"/>`
                    : '🐾'}
            </div>
            <div class="pet-info">
                <div class="pet-name">${a.nombre}</div>
                <div class="pet-meta">
                    <span> ${a.sexo}</span>
                    <span>${a.fechaNacimiento ? calcularEdad(a.fechaNacimiento) + ' años' : 'N/D'}</span>
                </div>
                <div class="pet-status ${a.estatus === 'Disponible' ? 'status-disponible' : 'status-otro'}">
                    ${a.estatus}
                </div>
                <button class="btn-solicitar" onclick="verDetalle(${a.animalID})">
                    Ver detalle
                </button>
                <a href="#11_perfil_refugio?refugioId=${a.refugioID}" class="btn-solicitar" style="display:inline-block;text-align:center;margin-top:6px;background:var(--beige);color:var(--text-dark);text-decoration:none;">
                    🏠 Ver refugio
                </a>
            </div>`;
        grid.appendChild(card);
    });
}

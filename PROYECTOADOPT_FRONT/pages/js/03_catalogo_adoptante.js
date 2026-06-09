

let todosLosAnimales =[]; //guarda a los animales que se obtiene de la api 
let refugioActivo=null; // 
let especiesFiltradas=[]; 

async function fetchAnimales() {
    const res = await fetch ('${API_BASE_URL}/Animales');
    return await res.json();

}

async function fetchRefugios() {
    const res =await fetch ('${API_BASE_URL}/Refugios');
    return await res.json();

}

async function fetchEspecies(){
    const res = await fetch ('${API_BASE_URL}/Especies');
    return await res.json();

}

// Renderiza los refugios como chips seleccionables en la interfaz 
function renderRefugios(refugios){
    const container = document.getElementById('RefugioSelector');
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
        chip.onclick = () => seleccionarRefugio(refugio, chip);
        container.appendChild(chip);

    });

}

// Renderiza los animales filtrados en la interfaz segun el refugio seleccionado
function seleccionarRefugio(id, chip){
    refugioActivo = id;
    document.querySelectorAll('.refugio_chip').forEach(c=> c.classList.remove('active'));
    chip.classList.add('active');
    renderAnimales(filtrarAnimales());

}

// Renderiza las especies como checkboxes para filtrar los animales por especie
function renderEspeciesFiltros(especies){
    const container = document.getElementById('especiesFiltos');
    container.innerHTML='';

    especies.forEach(e =>{
        const label =document.createElement('label');
        label.className='especie-filtro';
        label.innerHTML='<input type="checkbox" value="${e.especieId}" onchange="onEspecieChange()"/>${e.nombre}';
        container.appendChild(label);
    });
}

// Filtra los animales segun el refugio activo y las especies seleccionadas
function onEspecieChange(){
    especiesFiltradas = Array.from(document.querySelectorAll('#especiesFiltros input:checked')).map(i=>parseInt(i.value));
}

//logica para filtrar los animales segun el refugio activo, las especies seleccionadas y la edad máxima ingresada por el usuario
function filtrarAnimales(){
    const edadMax = parseInt(document.getElementById('edadMax').value) || Infinity;

    return todosLosAnimales.filter(a=>{
    const porREfugio = refugioActivo === null || a.refugioId === refugioActivo; // Si no hay refugio activo, mostrar todos, sino filtrar por el refugio seleccionado
    const porEspecie = especiesFiltradas.length ===0 || especiesFiltradas.includes(a.razaId); // Si no hay especies filtradas, mostrar todas, sino filtrar por las especies seleccionadas
    const edad = a.fechaNacimiento ? calcularEdad(a.fechaNacimiento):0; // Si no hay fecha de nacimiento, asumir edad 0 para que no se excluya por edad
    const porEdad = edad <= edadMax; // Filtrar por edad máxima
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
            </div>
        `;
        grid.appendChild(card);
    });

    function verDetalle(animalId){
    console.log('Ver detalle del animal con ID:', animalId);
        // Aquí puedes redirigir a una página de detalle o mostrar un modal con la información del animal
    
    }


    async function init(){
    try {
        //hace varias peticiones al mismo tiempo para obtener los animales, refugios y especies, y luego renderiza la interfaz con esos datos   
        const [animales, refugios, especies] = await Promise.all([
            fetchAnimales(),
            fetchRefugios(),
            fetchEspecies()
        ]);
        todosLosAnimales = animales;
        renderRefugios(refugios);
        renderEspeciesFiltros(especies);
        renderAnimales(todosLosAnimales);
    } catch (error) {
        console.error('Error al cargar los datos:', error);
        const grid = document.getElementById('petsGrid');
        grid.innerHTML = `
            <div style="grid-column:1/-1;text-align:center;padding:40px;color:var(--text-mid);">
            Ocurrió un error al cargar los animales. Por favor, intenta nuevamente más tarde.
            </div>`;
    
    }
}
document.addEventListener('DOMContentLoaded', init);
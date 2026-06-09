
//indica en que paso vas y guarda las respuestas del usuario en un objeto para luego enviarlas al servidor y crear el perfil del adoptante con esa información
let pasoActual = 0;
const respuestas = { descripcionCasa: '', descripcionMascotas: '', descripcionExperienciaConMascotas: '' };

//preguntas del perfil adoptante
const preguntas = [
    {
        titulo: 'Cuéntanos sobre tu casa',
        descripcion: '¿Cómo es el espacio donde vivirá la mascota?',
        placeholder: 'Ej: Vivo en un departamento pequeño con balcón, hay espacio suficiente para caminar...',
        campo: 'descripcionCasa'
    },
    {
        titulo: '¿Tienes otras mascotas?',
        descripcion: 'Describe si convives actualmente con otros animales.',
        placeholder: 'Ej: Tengo un perro de 3 años muy sociable, convive bien con otros animales...',
        campo: 'descripcionMascotas'
    },
    {
        titulo: 'Experiencia con mascotas',
        descripcion: 'Cuéntanos si has tenido mascotas antes y cómo fue esa experiencia.',
        placeholder: 'Ej: He tenido gatos toda mi vida, sé cómo cuidarlos, vacunarlos y llevarlos al veterinario...',
        campo: 'descripcionExperienciaConMascotas'
    }
];


//Render pregunta actual 
function renderPregunta() {
    const p    = preguntas[pasoActual];
    const card = document.getElementById('questionCard');

    card.innerHTML = `
        <div class="question-title">${p.titulo}</div>
        <p class="question-desc">${p.descripcion}</p>
        <textarea
            id="respuestaActual"
            class="question-textarea"
            placeholder="${p.placeholder}"
            rows="5"
            oninput="guardarRespuesta()"
        >${respuestas[p.campo]}</textarea>
        <div class="char-count" id="charCount">${respuestas[p.campo].length} / 500</div>
    `;

    // Barra de progreso
    const porcentaje = ((pasoActual + 1) / preguntas.length) * 100;
    document.getElementById('progressBar').style.width = `${porcentaje}%`;

    // Botones
    document.getElementById('btnPrev').style.display = pasoActual === 0 ? 'none' : '';
    document.getElementById('btnNext').textContent =
        pasoActual === preguntas.length - 1 ? 'Guardar perfil ' : 'Siguiente →';
}

//Guardar respuesta en memoria 
//guarda la respuesta del usuario en el objeto respuestas y si el usuario retrocede la respuesta se mantiene
function guardarRespuesta() {
    const campo = preguntas[pasoActual].campo;
    const valor = document.getElementById('respuestaActual').value;
    respuestas[campo] = valor;

    document.getElementById('charCount').textContent = `${valor.length} / 500`;
}

//Navegación 
function siguientePaso() {
    guardarRespuesta();
// Validación simple: asegurarse de que el campo no esté vacío antes de avanzar al siguiente paso o guardar el perfil
    const campo = preguntas[pasoActual].campo;
    if (!respuestas[campo].trim()) {
        alert('Por favor responde antes de continuar.');
        return;
    }

    if (pasoActual < preguntas.length - 1) {
        pasoActual++;
        renderPregunta();
    } else {
        guardarPerfil();
    }
}

function pasoAnterior() {
    guardarRespuesta();
    if (pasoActual > 0) {
        pasoActual--;
        renderPregunta();
    }
}

//Guardar perfil en API 
//envía las respuestas del usuario al servidor para crear el perfil del adoptante, asociándolo con el ID del usuario que se obtuvo al iniciar sesión
async function guardarPerfil() {
    // Obtiene el ID del usuario adoptante desde el almacenamiento local o de sesión
    const adoptanteUsuarioID = localStorage.getItem('adoptanteUsuarioID') //
                            || sessionStorage.getItem('adoptanteUsuarioID');

    if (!adoptanteUsuarioID) {
        alert('No se encontró tu sesión. Por favor inicia sesión de nuevo.');
        window.location.href = '#02_inicio_sesion';
        return;
    }

    const btnNext = document.getElementById('btnNext');
    btnNext.disabled    = true;
    btnNext.textContent = 'Guardando...';

    // Envía las respuestas al servidor para crear el perfil del adoptante
    try {
        const res = await fetch(`${API_BASE_URL}/PerfilesAdoptante`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({
                adoptanteUsuarioID:                 parseInt(adoptanteUsuarioID),
                descripcionCasa:                    respuestas.descripcionCasa,
                descripcionMascotas:                respuestas.descripcionMascotas,
                descripcionExperienciaConMascotas:  respuestas.descripcionExperienciaConMascotas
            })
        });

        if (!res.ok) {
            alert('Error al guardar el perfil. Intenta de nuevo.');
            btnNext.disabled    = false;
            btnNext.textContent = 'Guardar perfil ';
            return;
        }

        alert(' Perfil guardado correctamente.');
        window.location.href = '#03_catalogo_adoptante';

    } catch (err) {
        console.error('Error guardando perfil:', err);
        alert('Error de conexión. Verifica que el servidor esté corriendo.');
        btnNext.disabled    = false;
        btnNext.textContent = 'Guardar perfil ';
    }
}

//  Init 
document.addEventListener('DOMContentLoaded', () => {
    renderPregunta();
});
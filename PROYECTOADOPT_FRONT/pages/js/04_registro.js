


let rolseleccionado ='adoptante';

// Función para seleccionar el rol del usuario (adoptante o refugio) y mostrar u ocultar campos según la selección
function selectRole(el, rol){
    rolseleccionado = rol;
    document.querySelectorAll('.role-option').forEach(r => r.classList.remove('selected'));
    el.classList.add('selected');

    // Muestra u oculta el campo de fecha de nacimiento según el rol seleccionado
    const campoFecha = document.getElementById('campoFechaNacimiento');
    campoFecha.style.display = rol === 'adoptante' ? '' : 'none';
}

// Función para validar los campos del formulario de registro antes de enviarlo al servidor y asegurarse de que los datos ingresados sean correctos y completos
function validar() {
    const nombre   = document.getElementById('regNombre').value.trim();
    const apPat    = document.getElementById('regApellidoPaterno').value.trim();
    const apMat    = document.getElementById('regApellidoMaterno').value.trim();
    const email    = document.getElementById('regEmail').value.trim();
    const password = document.getElementById('regPassword').value;
    const confirmar= document.getElementById('regConfirmar').value;
    const telefono = document.getElementById('regTelefono').value.trim();

    if (!nombre || !apPat || !apMat || !email || !password || !confirmar || !telefono) {
        alert('Por favor llena todos los campos obligatorios.');
        return false;
    }
    // Validación de formato de correo electrónico, longitud de contraseña, coincidencia de contraseñas y formato de teléfono
    if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email)) {
        alert('Correo electrónico inválido.');
        return false;
    }
    // Validación de contraseña: mínimo 8 caracteres, debe coincidir con el campo de confirmación
    if (password.length < 8) {
        alert('La contraseña debe tener mínimo 8 caracteres.');
        return false;
    }
    // Validación de confirmación de contraseña
    if (password !== confirmar) {
        alert('Las contraseñas no coinciden.');
        return false;
    }
    // Validación de teléfono: debe tener exactamente 10 dígitos
    if (!/^\d{10}$/.test(telefono)) {
        alert('El teléfono debe tener exactamente 10 dígitos.');
        return false;
    }

    return true;
}

// Función para enviar los datos del formulario de registro al servidor y crear un nuevo usuario, adoptante o administrador según el rol seleccionado
async function register(){
    if(!validar()) return;

    const nombre = document.getElementById('regNombre').value.trim();
    const apPat = document.getElementById('regApellidoPaterno').value.trim();
    const apMat = document.getElementById('regApellidoMaterno').value.trim();
    const email = document.getElementById('regEmail').value.trim();
    const password = document.getElementById('regPassword').value;
    const telefono = document.getElementById('regTelefono').value.trim();
 try {
        // 1. Crear usuario
        const resUsuario = await fetch(`${API_BASE_URL}/Usuarios`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({
                correo:      email,
                tipoUsuario: rolSeleccionado,
                estatus:     'Activo'
            })
        });

        if (!resUsuario.ok) {
            alert('Error al crear el usuario. Intenta de nuevo.');
            return;
        }

        const usuario = await resUsuario.json();

        // 2. Crear adoptante 
        if (rolSeleccionado === 'Adoptante') {
            const resAdoptante = await fetch(`${API_BASE_URL}/Adoptantes`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({
                    usuarioID:       usuario.usuarioID,
                    nombre:          nombre,
                    apellidoPaterno: apPat,
                    apellidoMaterno: apMat,
                    telefono:        telefono,
                    fechaNacimiento: fechaNac || null // Si el campo de fecha de nacimiento está vacío, se envía null al servidor 
                })
            });

            if (!resAdoptante.ok) {
                alert('Error al crear el perfil de adoptante.');
                return;
            }

        } else if (rolSeleccionado === 'Administrador') {
            const resAdmin = await fetch(`${API_BASE_URL}/Administradores`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({
                    usuarioID:       usuario.usuarioID,
                    nombre:          nombre,
                    apellidoPaterno: apPat,
                    apellidoMaterno: apMat,
                    telefono:        telefono
                })
            });

            if (!resAdmin.ok) {
                alert('Error al crear el perfil de administrador.');
                return;
            }
        }

        alert('Registro exitoso. Ahora puedes iniciar sesión.');
    } catch (error) {
        console.error('Error durante el registro:', error);
        alert('Ocurrió un error durante el registro. Intenta de nuevo.');
    }}
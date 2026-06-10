async function login() {
    const correo = document.getElementById('loginEmail').value.trim();
    const password = document.getElementById('loginPassword').value.trim();

    if (!correo || !password){
        alert("Porfavor Agrega tu correo y contraseña");
        return;
    }

    try{
        const respuesta = await apiPost('/Usuarios/login',{
            correo : correo,
            password : password

        });
        const sesion = {
            token : respuesta.token,
            usuarioID : respuesta.usuarioID,
            correo : respuesta.correo,
            tipoUsuario : respuesta.tipoUsuario
        }

        localStorage.setItem('session', JSON.stringify(sesion))
        //denemos de tener en cuenta que esto solo funcionara despues de registrar al usuario como un adoptante o administratdor por eso el de registrarse no tiene esta condicion...
        if (respuesta.tipoUsuario === 'Adoptante'){
            redirect('03_catalogo_adoptante');
        } else if(respuesta.tipoUsuario === 'Administrador'){
            redirect('06_inicio_refugio')
        } 
        redirect('01_seleccion_rol');
    } catch (error) {
        console.error('Error en el inicio de sesion', error);
        alert('Los datos son invalidos, vuelve a intentarlo.');
        
    }
}

async function registrarse() {
    const correo = document.getElementById('regEmail').value.trim();
    const password = document.getElementById('regPassword').value;
    const confirmar = document.getElementById('regConfirmar').value;

    if (!correo || !password || !confirmar) {
        alert('Por favor llena todos los campos.');
        return;
    }
    if (password.length < 8) {
        alert('La contraseña debe tener mínimo 8 caracteres.');
        return;
    }
    if (password !== confirmar) {
        alert('Las contraseñas no coinciden.');
        return;
    }

    try {
  
        await apiPost('/Usuarios', {
            correo: correo,
            password: password,
            tipoUsuario: 'Adoptante',
            estatus: 'Activo'
        });

       
        const respuesta = await apiPost('/Usuarios/login', {
            correo: correo,
            password: password
        });

     
        const sesion = {
            token: respuesta.token,
            usuarioID: respuesta.usuarioID,
            correo: respuesta.correo,
            tipoUsuario: respuesta.tipoUsuario
        };
        localStorage.setItem('session', JSON.stringify(sesion));
        redirect('01_seleccion_rol');
    } catch (error) {
        console.error('Error en registro:', error);
        alert('Error al crear la cuenta. Intenta de nuevo.');
    }
}
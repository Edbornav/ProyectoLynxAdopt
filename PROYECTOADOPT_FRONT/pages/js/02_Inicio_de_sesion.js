async function Login() {
    const correo = document.getElementById('loginEmail').value.trim();
    const password = document.getElementById('loginPassword').value.trim();

    if (!correo || !password){
        alert("Porfavor Agrega tu correo y contraseña");
        return;
    }

    try{
        const respuesta = await apiPost('Usuarios/login',{
            corre: correo,
            passowrd : password

        });
        const sesion = {
            token : respuesta.token,
            usuarioID : respuesta.usuarioID,
            correo : respuesta.correo,
            tipoUsuario : respuesta.tipoUsuario
        }

        localStorage.setItem('session', JSON.stringify(sesion))

        if (respuesta.tipoUsuario === 'Adoptante'){
            redirect('03_catalogo_adoptante');
        } else if (respuesta.tipoUsuario === 'Administrador'){
            redirect('06_inicio_refugio')
        } else {
            redirect('01_seleccion_rol')
        }
    } catch (error) {
        console.error('Error en el inicio de sesion', error);
        alert('Los datos son invalidos, vuelve a intentarlo.');
        
    }
}
document.addEventListener("DOMContentLoaded", function () {
    const btnLogin = document.getElementById("btnLogin");

    btnLogin.addEventListener("click", async function (e) {
        
        e.preventDefault();

        const correo = document.getElementById("Correo").value.trim();
        const clave = document.getElementById("Clave").value.trim();

        
        if (correo === "" || clave === "") {
            Swal.fire({
                icon: 'warning',
                title: 'Atención',
                text: 'Debe ingresar correo y clave'
            });
            return;
        }

        const modelo = {
            Correo: correo,
            Clave: clave
        };

       
        btnLogin.disabled = true;

        try {
            const response = await fetch("/Login/Login", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(modelo)
            });

          
            const data = await response.json();

            if (data.estado) {
              
                const tokenRecibido = data.token || data.Token;

                if (tokenRecibido) {
                   
                    localStorage.setItem("token", tokenRecibido);

                    console.log("Token guardado con éxito:", localStorage.getItem("token"));
                } else {
                    console.warn("El servidor respondió con éxito pero el TOKEN llegó vacío.");
                }

               
                Swal.fire({
                    icon: 'success',
                    title: '¡Bienvenido!',
                    text: 'Iniciando sesión...',
                    showConfirmButton: false,
                    timer: 1500
                });

                
                setTimeout(() => {
                    window.location.href = data.url;
                }, 1500);

            } else {
                
                Swal.fire({
                    icon: 'error',
                    title: 'Error de acceso',
                    text: data.mensaje || "Credenciales incorrectas"
                });
                btnLogin.disabled = false;
            }
        } catch (error) {
            console.error("Error en la petición fetch:", error);
            Swal.fire({
                icon: 'error',
                title: 'Error de servidor',
                text: 'No se pudo conectar con el sistema. Intente más tarde.'
            });
            btnLogin.disabled = false;
        }
    });
});
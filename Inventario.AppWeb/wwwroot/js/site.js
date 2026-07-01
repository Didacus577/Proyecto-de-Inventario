$.ajaxSetup({
    beforeSend: function (xhr, settings) {        
        const esUrlInterna = !settings.url.startsWith('http://') && !settings.url.startsWith('https://')
            || settings.url.includes('localhost:5107');

        if (esUrlInterna) {
            const token = localStorage.getItem("token");
            if (token) {
                xhr.setRequestHeader('Authorization', 'Bearer ' + token);
            }
        }
    },
    error: function (xhr) {
      
        if (xhr.status === 401) {
            toastr.error("Su sesión ha expirado. Redirigiendo...");
            setTimeout(() => {
                window.location.href = "/Login/IniciaSesion";
            }, 1500);
        }
     
        else if (xhr.status === 403) {
            toastr.error("No tienes los permisos de rol necesarios para realizar esta acción.");
        }
    }
});
let tablaUsuarios;
let modalAdd;
let modalUpdate;

$(document).ready(function () {
    // 1. Configuración Toastr
    toastr.options = { "closeButton": true, "progressBar": true, "positionClass": "toast-top-right", "timeOut": "3000" };

    // 2. Inicializar Modales
    modalAdd = new bootstrap.Modal(document.getElementById('modalUsuario'));
    modalUpdate = new bootstrap.Modal(document.getElementById('modalEditarUsuario'));

    // 3. DataTable
    tablaUsuarios = $('#tbUsuarios').DataTable({
        responsive: true,
        ajax: { url: '/Usuario/UsuarioLista', type: "GET", dataSrc: "data" },
        columns: [
            { data: "idUsuario" },
            { data: "nombreUsuario" },
            { data: "correo" },
            { data: "nombreRol" },
            {
                data: "idUsuario",
                render: function (data) {
                    return `<div class="text-center">
                                <button class="btn btn-sm btn-warning me-1" onclick="mostrarModalEditar(${data})"><i class="fa-solid fa-pen"></i></button>
                                <button class="btn btn-sm btn-danger" onclick="eliminarUsuario(${data})"><i class="fa-solid fa-trash"></i></button>
                            </div>`;
                },
                orderable: false
            }
        ],
        language: { url: "https://cdn.datatables.net/plug-ins/1.11.5/i18n/es-ES.json" }
    });

    // 4. Formulario Crear
    $('#formUsuario').on('submit', function (e) {
        e.preventDefault();
        const modelo = {
            nombreUsuario: $('#nombreUsuario').val(),
            correo: $('#correo').val(),
            clave: $('#clave').val(),
            idRol: parseInt($('#rol').val())
        };
        ejecutarAjax("/Usuario/CrearUsuario", "POST", modelo, modalAdd);
    });

    // 5. Formulario Editar (Con Clave)
    $('#formEditarUsuario').on('submit', function (e) {
        e.preventDefault();
        const clave = $('#claveEditar').val();
        if (clave !== "" && clave !== $('#confirmarClaveEditar').val()) {
            toastr.warning("Las nuevas contraseñas no coinciden");
            return;
        }

        const modelo = {
            idUsuario: parseInt($('#idUsuarioEditar').val()),
            nombreUsuario: $('#nombreUsuarioEditar').val(),
            correo: $('#correoEditar').val(),
            idRol: parseInt($('#rolEditar').val()),
            clave: clave // Se envía si no es vacía
        };
        ejecutarAjax("/Usuario/EditarUsuario", "PUT", modelo, modalUpdate);
    });
});



function cargarRoles(selectId, valorSeleccionado = null) {
    $.get('/Roles/ListaRoles', function (res) {
        const $select = $(selectId);
        $select.empty().append('<option value="">Seleccionar rol...</option>');
        res.data.forEach(item => {
            $select.append(`<option value="${item.idRol}">${item.nombreRol}</option>`);
        });
        if (valorSeleccionado) $select.val(valorSeleccionado);
    });
}

window.abrirModalRegistro = () => {
    $('#formUsuario')[0].reset();
    cargarRoles('#rol');
    modalAdd.show();
};

window.mostrarModalEditar = (id) => {
    $.get(`/Usuario/Obtener?idUsuario=${id}`, function (res) {
        if (res.estado) {
            $('#idUsuarioEditar').val(res.objeto.idUsuario);
            $('#nombreUsuarioEditar').val(res.objeto.nombreUsuario);
            $('#correoEditar').val(res.objeto.correo);
            $('#claveEditar, #confirmarClaveEditar').val(""); // Limpiar campos clave
            cargarRoles('#rolEditar', res.objeto.idRol);
            modalUpdate.show();
        }
    });
};

function ejecutarAjax(url, metodo, data, modal) {
    $.ajax({
        url: url,
        type: metodo,
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(data),
        success: function (res) {
            if (res.estado) {
                toastr.success(res.mensaje);
                modal.hide();
                tablaUsuarios.ajax.reload(null, false);
            }
        },
        error: function (xhr) {
            toastr.error(xhr.responseJSON ? xhr.responseJSON.mensaje : "Error");
        }
    });
}

window.eliminarUsuario = (id) => {
    Swal.fire({ title: "¿Eliminar?", icon: "warning", showCancelButton: true, confirmButtonText: "Sí" }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: `/Usuario/EliminarUsuario?idUsuario=${id}`,
                type: 'DELETE',
                success: function (res) {
                    toastr.success(res.mensaje);
                    tablaUsuarios.ajax.reload(null, false);
                }
            });
        }
    });
};
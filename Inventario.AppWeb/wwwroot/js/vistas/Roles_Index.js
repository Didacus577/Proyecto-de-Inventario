let tablaData;
let modalAdd;
let modalUpdate;

$(document).ready(function () {
    // Configuración de Toastr
    toastr.options = { "closeButton": true, "progressBar": true, "positionClass": "toast-top-right" };

    // Inicialización de Modales (Asegúrate que los IDs coincidan con los Partial Views)
    const elModalAdd = document.getElementById('modalAgregarRol');
    const elModalUpdate = document.getElementById('modalEditarRol');

    if (elModalAdd) modalAdd = new bootstrap.Modal(elModalAdd);
    if (elModalUpdate) modalUpdate = new bootstrap.Modal(elModalUpdate);

    // DataTable
    tablaData = $('#tbdata').DataTable({
        responsive: true,
        ajax: {
            url: '/Roles/ListaRoles',
            type: 'GET',
            dataType: 'json',
            dataSrc: 'data'
        },
        columns: [
            { data: 'idRol' },
            { data: 'nombreRol' },
            {
                data: null,
                render: function (data) {
                    return `
                        <div class="text-center">
                            <button class="btn btn-warning btn-sm me-1" onclick="cargarParaEditar(${data.idRol})">
                                <i class="bi bi-pencil"></i>
                            </button>
                            <button class="btn btn-danger btn-sm" onclick="eliminarRol(${data.idRol})">
                                <i class="bi bi-trash"></i>
                            </button>
                        </div>`;
                },
                orderable: false
            }
        ],
        language: { url: "https://cdn.datatables.net/plug-ins/1.11.5/i18n/es-ES.json" }
    });

    // Evento Submit: CREAR
    $('#formAgregarRol').on('submit', function (e) {
        e.preventDefault();
        const modelo = {
            nombreRol: $('#nuevoRolNombre').val()
        };
        ejecutarAjax("/Roles/CrearRol", "POST", modelo, modalAdd);
    });

    // Evento Submit: EDITAR
    $('#formEditarRol').on('submit', function (e) {
        e.preventDefault();
        const modelo = {
            idRol: parseInt($('#editarRolId').val()),
            nombreRol: $('#editarRolNombre').val()
        };
        ejecutarAjax("/Roles/EditarRol", "PUT", modelo, modalUpdate);
    });
});

// Funciones Globales
window.nuevoRol = () => {
    $('#formAgregarRol')[0].reset();
    modalAdd.show();
};

window.cargarParaEditar = (id) => {
    $.get(`/Roles/ObtenerRol?idRol=${id}`, function (res) {
        // res ahora es directamente el objeto VMRoles según tu controlador
        $('#editarRolId').val(res.idRol);
        $('#editarRolNombre').val(res.nombreRol);
        modalUpdate.show();
    }).fail(function () {
        toastr.error("No se pudo obtener la información del rol");
    });
};

function ejecutarAjax(url, metodo, data, modal) {
    $.ajax({
        url: url,
        type: metodo,
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(data),
        success: function (res) {
            toastr.success(res.mensaje);
            modal.hide();
            tablaData.ajax.reload(null, false);
        },
        error: function (xhr) {
            toastr.error(xhr.responseText || "Error en la operación");
        }
    });
}

window.eliminarRol = (id) => {
    Swal.fire({
        title: "¿Eliminar Rol?",
        text: "Esta acción no se puede deshacer",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#d33",
        confirmButtonText: "Sí, eliminar"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: `/Roles/EliminarRol?idRol=${id}`,
                type: "DELETE",
                success: function (res) {
                    toastr.success(res.mensaje);
                    tablaData.ajax.reload(null, false);
                },
                error: function (xhr) { toastr.error(xhr.responseText); }
            });
        }
    });
};
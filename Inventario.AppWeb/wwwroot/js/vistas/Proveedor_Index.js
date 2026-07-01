let tablaData;
let modalAgregar;
let modalEditar;

$(document).ready(function () {
    // Configuración global de Toastr para mensajes tipo notificación (arriba a la derecha)
    toastr.options = {
        "closeButton": true,
        "progressBar": true,
        "positionClass": "toast-top-right",
        "timeOut": "3000"
    };

    // 1. Inicializar Modales de Bootstrap
    modalAgregar = new bootstrap.Modal(document.getElementById('modalProveedor'));
    modalEditar = new bootstrap.Modal(document.getElementById('modalEditarProveedor'));

    // 2. Inicializar DataTable con carga asíncrona
    tablaData = $('#tbProveedor').DataTable({
        responsive: true,
        ajax: {
            "url": "/Proveedor/ListaProveedor",
            "type": "GET",
            "dataType": "json",
            "dataSrc": "data" // Coincide con el objeto 'data' que devuelve tu controlador
        },
        columns: [
            { data: 'idProveedor' },
            { data: 'nombreProveedor' },
            { data: 'telefono' },
            { data: 'correo' },
            { data: 'direccion' },
            {
                data: null,
                render: function (data) {
                    return `<div class="text-center">
                        <button class="btn btn-warning btn-sm me-2 btn-editar-tabla">
                            <i class="fa-solid fa-pen-to-square"></i>
                        </button>
                        <button class="btn btn-danger btn-sm" onclick="eliminarProveedor(${data.idProveedor})">
                            <i class="fa-solid fa-trash"></i>
                        </button>
                    </div>`;
                },
                orderable: false
            }
        ],
        language: { url: "https://cdn.datatables.net/plug-ins/1.11.5/i18n/es-ES.json" },
        dom: 'lrtip'
    });

    // 3. Abrir modal de nuevo proveedor y limpiar formulario
    $('#btnNuevo').on('click', function () {
        $("#formProveedor")[0].reset();
        modalAgregar.show();
    });

    // 4. Capturar datos para Editar (Al hacer clic en el botón naranja de la tabla)
    $('#tbProveedor tbody').on('click', '.btn-editar-tabla', function () {
        const fila = $(this).closest('tr');
        const data = tablaData.row(fila).data();

        // Llenar los campos del modal de edición
        $("#txtIdEditar").val(data.idProveedor);
        $("#txtNombreEditar").val(data.nombreProveedor);
        $("#txtTelefonoEditar").val(data.telefono);
        $("#txtCorreoEditar").val(data.correo);
        $("#txtDireccionEditar").val(data.direccion);

        modalEditar.show();
    });

    // 5. Evento Submit para CREAR PROVEEDOR
    $(document).on("submit", "#formProveedor", function (e) {
        e.preventDefault();

        const objeto = {
            idProveedor: 0,
            nombreProveedor: $("#nombreProveedor").val(),
            correo: $("#correo").val(),
            telefono: $("#telefono").val(),
            direccion: $("#direccion").val()
        };

        ejecutarPeticion("/Proveedor/CrearProveedor", "POST", objeto, modalAgregar);
    });

    // 6. Evento Submit para EDITAR PROVEEDOR (Update)
    $(document).on("submit", "#formEditarProveedor", function (e) {
        e.preventDefault();

        const objeto = {
            idProveedor: parseInt($("#txtIdEditar").val()),
            nombreProveedor: $("#txtNombreEditar").val(),
            correo: $("#txtCorreoEditar").val(),
            telefono: $("#txtTelefonoEditar").val(),
            direccion: $("#txtDireccionEditar").val()
        };

        ejecutarPeticion("/Proveedor/EditarProveedor", "PUT", objeto, modalEditar);
    });
});

/**
 * Función genérica para procesar Crear y Editar vía AJAX
 */
function ejecutarPeticion(url, metodo, data, modal) {
    $.ajax({
        url: url,
        type: metodo,
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(data),
        success: function (res) {
            // Verifica que el controlador devuelva 'estado: true'
            if (res.estado) {
                toastr.success(res.mensaje); // Mensaje de éxito estilo Toastr
                modal.hide();
                // Recarga la tabla automáticamente sin refrescar la página completa
                tablaData.ajax.reload(null, false);
            } else {
                Swal.fire("Error", res.mensaje, "error");
            }
        },
        error: function () {
            toastr.error("Error de comunicación con el servidor");
        }
    });
}

/**
 * Función para Eliminar Proveedor
 */
function eliminarProveedor(id) {
    Swal.fire({
        title: "¿Deseas eliminar este proveedor?",
        text: "Esta acción no se puede deshacer",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: "Sí, eliminar",
        cancelButtonText: "Cancelar"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: `/Proveedor/EliminarProveedor?idProveedor=${id}`,
                type: "DELETE",
                success: function (res) {
                    if (res.estado) {
                        toastr.success(res.mensaje || "Eliminado correctamente");
                        tablaData.ajax.reload(null, false);
                    } else {
                        toastr.error(res.mensaje);
                    }
                },
                error: function () {
                    toastr.error("Error al intentar eliminar");
                }
            });
        }
    });
}
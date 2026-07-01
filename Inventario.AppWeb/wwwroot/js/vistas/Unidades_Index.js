let tablaData;
let modalAdd;
let modalUpdate;

$(document).ready(function () {
    // Configuración Toastr
    toastr.options = { "closeButton": true, "progressBar": true, "positionClass": "toast-top-right", "timeOut": "3000" };

    modalAdd = new bootstrap.Modal(document.getElementById('modalAgregarUnidad'));
    modalUpdate = new bootstrap.Modal(document.getElementById('modalEditarUnidad'));

    tablaData = $("#tbdata").DataTable({
        responsive: true,
        ajax: {
            "url": "/UnidadMedida/MedidaLista",
            "type": "GET",
            "datatype": "json"
        },
        columns: [
            { data: "idUnidad" },
            { data: "nombreUnidad" },
            { data: "abreviatura" },
            {
                data: null,
                render: function (data) {
                    return `
                        <div class="text-center">
                            <button class="btn btn-warning btn-sm me-2 btn-editar"><i class="bi bi-pencil-square"></i></button>
                            <button class="btn btn-danger btn-sm btn-eliminar"><i class="bi bi-trash"></i></button>
                        </div>`;
                },
                orderable: false
            }
        ],
        language: { url: "https://cdn.datatables.net/plug-ins/1.11.5/i18n/es-ES.json" }
    });

    // Abrir modal de edición
    $('#tbdata tbody').on('click', '.btn-editar', function () {
        const fila = $(this).closest('tr');
        const data = tablaData.row(fila).data();

        $("#txtIdEditar").val(data.idUnidad);
        $("#txtNombreEditar").val(data.nombreUnidad);
        $("#txtAbreviaturaEditar").val(data.abreviatura);

        modalUpdate.show();
    });

    // Submit para Crear
    $(document).on("submit", "#formUnidadNuevo", function (e) {
        e.preventDefault();
        const objeto = {
            idUnidad: 0,
            nombreUnidad: $("#txtNombreNuevo").val(),
            abreviatura: $("#txtAbreviaturaNuevo").val()
        };
        ejecutarAjax("/UnidadMedida/CrearMedida", "POST", objeto, modalAdd);
    });

    // Submit para Editar
    $(document).on("submit", "#formUnidadEditar", function (e) {
        e.preventDefault();
        const objeto = {
            idUnidad: parseInt($("#txtIdEditar").val()),
            nombreUnidad: $("#txtNombreEditar").val(),
            abreviatura: $("#txtAbreviaturaEditar").val()
        };
        ejecutarAjax("/UnidadMedida/EditarMedida", "PUT", objeto, modalUpdate);
    });
});

function nuevaUnidad() {
    $("#formUnidadNuevo")[0].reset();
    modalAdd.show();
}

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
                tablaData.ajax.reload(null, false);
            } else {
                Swal.fire("Error", res.mensaje, "error");
            }
        }
    });
}

// Eliminar
$(document).on('click', '.btn-eliminar', function () {
    const fila = $(this).closest('tr');
    const data = tablaData.row(fila).data();

    Swal.fire({
        title: "¿Eliminar unidad?",
        text: data.nombreUnidad,
        icon: "warning",
        showCancelButton: true,
        confirmButtonText: "Sí, eliminar"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: `/UnidadMedida/EliminarMedida?idMedida=${data.idUnidad}`,
                type: "DELETE",
                success: function (res) {
                    if (res.estado) {
                        toastr.success(res.mensaje);
                        tablaData.ajax.reload(null, false);
                    } else {
                        toastr.error(res.mensaje);
                    }
                }
            });
        }
    });
});
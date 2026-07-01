let tablaData;


let modalAgregar;
let modalEditar;

$(document).ready(function () {
    
    
    modalAgregar = new bootstrap.Modal(document.getElementById('modalMarca'));
    modalEditar = new bootstrap.Modal(document.getElementById('modalEditarMarca'));

    tablaData = $('#tbMarcas').DataTable({
        responsive: true,
        "ajax": {
            "url": "/Marca/ListaMarca",
            "type": "GET",
            "datatype": "json",
            "dataSrc": "data"
        },
        "columns": [
            { "data": "idMarca" },
            { "data": "nombreMarca" },
            { "data": "descripcion" },
            {
                "data": "idMarca",
                "render": function (data, type, row) {
                    return `<div class="text-center">
                        <button class="btn btn-warning btn-sm me-2" onclick="abrirEditar(${JSON.stringify(row).replace(/"/g, '&quot;')})">
                            <i class="fa-solid fa-pen-to-square"></i>
                        </button>
                        <button class="btn btn-danger btn-sm" onclick="eliminarMarca(${data})">
                            <i class="fa-solid fa-trash"></i>
                        </button>
                    </div>`;
                },
                "orderable": false
            }
        ],
        "language": { "url": "https://cdn.datatables.net/plug-ins/1.11.5/i18n/es-ES.json" },
        dom: 'lrtip'
    });

   
    $('#searchMarca').on('keyup', function () { tablaData.search(this.value).draw(); });
    $('#btnRefrescar').on('click', function () { tablaData.ajax.reload(); });

 
    $('#btnNuevo').on('click', function () {
        $("#formMarca")[0].reset();
    });

  
    $("#formMarca").submit(function (e) {
        e.preventDefault();
        const objeto = {
            idMarca: 0,
            nombreMarca: $("#nombreMarca").val().trim(),
            descripcion: $("#descripcion").val().trim()
        };
        ejecutarPeticion("/Marca/CrearMarca", "POST", objeto, modalAgregar);
    });

   
    $("#formEditarMarca").submit(function (e) {
        e.preventDefault();
        const objeto = {
            idMarca: parseInt($("#idMarcaEditar").val()),
            nombreMarca: $("#nombreMarcaEditar").val().trim(),
            descripcion: $("#descripcionEditar").val().trim()
        };
        ejecutarPeticion("/Marca/EditarMarca", "PUT", objeto, modalEditar);
    });
});



function abrirEditar(row) {
    $("#idMarcaEditar").val(row.idMarca);
    $("#nombreMarcaEditar").val(row.nombreMarca);
    $("#descripcionEditar").val(row.descripcion);
    modalEditar.show();
}

function ejecutarPeticion(url, metodo, data, modal) {
    $.ajax({
        url: url,
        type: metodo,
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(data),
        success: function (res) {
            toastr.success(res.mensaje || "Operación exitosa");
            modal.hide();
            tablaData.ajax.reload();
        },
        error: function (err) {
            toastr.error("Error al procesar la solicitud");
        }
    });
}

function eliminarMarca(id) {
    Swal.fire({
        title: "¿Eliminar?",
        text: "Esta acción es permanente",
        icon: "warning",
        showCancelButton: true,
        confirmButtonText: "Sí, borrar"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: "/Marca/EliminarMarca?idMarca=" + id,
                type: "DELETE",
                success: function () {
                    tablaData.ajax.reload();
                    toastr.success("Eliminado correctamente");
                }
            });
        }
    });
}
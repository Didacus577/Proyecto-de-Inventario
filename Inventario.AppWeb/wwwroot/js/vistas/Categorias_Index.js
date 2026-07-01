let tablaCategorias;
let modalAdd;
let modalUpdate;

$(document).ready(function () {

    toastr.options = {
        "closeButton": true,
        "progressBar": true,
        "positionClass": "toast-top-right",
        "timeOut": "3000"
    };

   
    modalAdd = new bootstrap.Modal(document.getElementById('modalCategoria'));
    modalUpdate = new bootstrap.Modal(document.getElementById('modalEditarCategoria'));

   
    tablaCategorias = $('#tbCategorias').DataTable({
        responsive: true,
        ajax: {
            url: '/Categoria/ListarCategorias',
            type: 'GET',            
            dataSrc: 'data'
        },
        columns: [
            { data: 'idCategoria' },
            { data: 'nombreCategoria' },
            { data: 'descripcionCategoria' }, 
            {
                data: null,
                render: function (data) {
                    return `
                        <div class="text-center">
                            <button class="btn btn-warning btn-sm me-1" onclick="editarCategoria(${data.idCategoria})">
                                <i class="fa-solid fa-pen"></i>
                            </button>
                            <button class="btn btn-danger btn-sm" onclick="eliminarCategoria(${data.idCategoria})">
                                <i class="fa-solid fa-trash"></i>
                            </button>
                        </div>`;
                },
                orderable: false
            }
        ],
        language: { url: "https://cdn.datatables.net/plug-ins/1.11.5/i18n/es-ES.json" }
    });

    
    $('#formCategoria').on('submit', function (e) {
        e.preventDefault();
        const objeto = {
            nombreCategoria: $('#nombreCategoria').val(),
            descripcionCategoria: $('#descripcion').val() // Captura del ID 'descripcion' hacia la propiedad 'descripcionCategoria'
        };
        ejecutarAjax("/Categoria/CrearCategoria", "POST", objeto, modalAdd);
    });

   
    $('#formEditarCategoria').on('submit', function (e) {
        e.preventDefault();
        const objeto = {
            idCategoria: parseInt($('#idCategoriaEditar').val()),
            nombreCategoria: $('#nombreCategoriaEditar').val(),
            descripcionCategoria: $('#descripcionEditar').val()
        };
        ejecutarAjax("/Categoria/EditarCategoria", "PUT", objeto, modalUpdate);
    });
});

window.nuevaCategoria = function () {
    $("#formCategoria")[0].reset();
    modalAdd.show();
}


window.editarCategoria = function (id) {
    $.get(`/Categoria/ObtenerCategoria?idCategoria=${id}`, function (res) {
        if (res.estado) {
            $('#idCategoriaEditar').val(res.objeto.idCategoria);
            $('#nombreCategoriaEditar').val(res.objeto.nombreCategoria);
            $('#descripcionEditar').val(res.objeto.descripcionCategoria);
            modalUpdate.show();
        } else {
            toastr.error(res.mensaje || "Error al obtener datos");
        }
    });
};


window.eliminarCategoria = function (id) {
    Swal.fire({
        title: "¿Eliminar categoría?",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#d33",
        confirmButtonText: "Sí, eliminar"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: `/Categoria/CategoriaEliminar?idCategoria=${id}`,
                type: "DELETE",
                success: function (res) {
                    if (res.estado) {
                        toastr.success(res.mensaje);
                        tablaCategorias.ajax.reload(null, false);
                    } else {
                        toastr.error(res.mensaje);
                    }
                }
            });
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
                tablaCategorias.ajax.reload(null, false);
            } else {
                Swal.fire("Error", res.mensaje, "error");
            }
        },
        error: function () {
            toastr.error("Error en el servidor");
        }
    });
}
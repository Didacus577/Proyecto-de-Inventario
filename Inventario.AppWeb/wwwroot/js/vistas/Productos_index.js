let tablaProductos;
let modalAdd;
let modalUpdate;
let modalView; // Modal de solo lectura para detalles
let ROL_VISTA = "lector";

$.ajaxSetup({
    beforeSend: function (xhr, settings) {
        if (!settings.url.startsWith('http://') && !settings.url.startsWith('https://')) {
            const token = localStorage.getItem("token") || sessionStorage.getItem("token");
            if (token) {
                xhr.setRequestHeader("Authorization", "Bearer " + token);
            }
        }
    },
    error: function (xhr) {
        if (xhr.status === 401) {
            window.location.href = "/Login/IniciaSesion";
        }
    }
});

$(document).ready(function () {
    toastr.options = { "closeButton": true, "progressBar": true, "positionClass": "toast-top-right", "timeOut": "3000" };

    const rolAtributo = $('#tbProductos').data('rol');
    if (rolAtributo) {
        ROL_VISTA = rolAtributo.toLowerCase();
    }

    
    if (ROL_VISTA === "admin" || ROL_VISTA === "operador") {
        modalAdd = new bootstrap.Modal(document.getElementById('modalProducto'));
        modalUpdate = new bootstrap.Modal(document.getElementById('modalEditarProducto'));
    }

   
    modalView = new bootstrap.Modal(document.getElementById('modalVerProducto'));

    llenarSelect('/Categoria/ListarCategorias', '#filtroCategoria', 'nombreCategoria', 'nombreCategoria');
    llenarSelect('/Marca/ListaMarca', '#filtroMarca', 'nombreMarca', 'nombreMarca');

    tablaProductos = $('#tbProductos').DataTable({
        responsive: true,
        ajax: {
            url: '/Producto/ListarProductos',
            type: "GET",
            dataSrc: "data"
        },
        columns: [
            { data: "nombreProducto" },
            { data: "nombreCategoria" },
            { data: "nombreMarca" },
            { data: "stock" },
            {
                data: "estado",
                render: function (data) {
                    return data ? '<span class="badge bg-success">Activo</span>' : '<span class="badge bg-danger">Inactivo</span>';
                }
            },
            {
                data: "idProducto",
                render: function (data) {
                   
                    const btnVer = `<button class="btn btn-sm btn-info text-white me-1" onclick="mostrarModalVer(${data})" title="Ver Detalles"><i class="fa-solid fa-eye"></i></button>`;

                
                    if (ROL_VISTA === "admin") {
                        return `<div class="text-center">
                                    ${btnVer}
                                    <button class="btn btn-sm btn-warning me-1" onclick="mostrarModalEditar(${data})" title="Editar"><i class="fa-solid fa-pen"></i></button>
                                    <button class="btn btn-sm btn-danger" onclick="eliminarProducto(${data})" title="Eliminar"><i class="fa-solid fa-trash"></i></button>
                                </div>`;
                    }

             
                    if (ROL_VISTA === "operador") {
                        return `<div class="text-center">
                                    ${btnVer}
                                    <button class="btn btn-sm btn-warning" onclick="mostrarModalEditar(${data})" title="Editar">
                                        <i class="fa-solid fa-pen me-1"></i> Editar
                                    </button>
                                </div>`;
                    }

                   
                    return `<div class="text-center">
                                ${btnVer}
                                <span class="badge bg-secondary opacity-75"><i class="fa-solid fa-lock me-1"></i> Solo Lectura</span>
                            </div>`;
                },
                orderable: false
            }
        ],
        language: { url: "https://cdn.datatables.net/plug-ins/1.11.5/i18n/es-ES.json" }
    });

    $.fn.dataTable.ext.search.push(
        function (settings, data, dataIndex) {
            if (settings.nTable.id !== 'tbProductos') return true;

            const catSeleccionada = $('#filtroCategoria').val();
            const marcaSeleccionada = $('#filtroMarca').val();
            const estadoSeleccionado = $('#filtroEstado').val();

            const columnaCategoria = data[1];
            const columnaMarca = data[2];
            const columnaEstado = data[4];

            if (catSeleccionada && columnaCategoria !== catSeleccionada) return false;
            if (marcaSeleccionada && columnaMarca !== marcaSeleccionada) return false;
            if (estadoSeleccionado && columnaEstado !== estadoSeleccionado) return false;

            return true;
        }
    );

    $('#filtroCategoria, #filtroMarca, #filtroEstado').on('change', function () {
        tablaProductos.draw();
    });

    $('#btnLimpiarFiltros').on('click', function () {
        $('#filtroCategoria').val('');
        $('#filtroMarca').val('');
        $('#filtroEstado').val('');
        tablaProductos.draw();
    });

    if (ROL_VISTA === "admin" || ROL_VISTA === "operador") {
        $('#formProducto').on('submit', function (e) {
            e.preventDefault();

            const modelo = {
                idProducto: 0,
                nombreProducto: $('#txtNombre').val(),
                descripcion: $('#txtDescripcion').val(),
                stock: parseInt($('#txtStock').val()) || 0,
                stockMinimo: parseInt($('#txtStockMinimo').val()) || 0,
                idCategoria: parseInt($('#cboCategoria').val()),
                nombreCategoria: "",
                idMarca: parseInt($('#cboMarca').val()),
                nombreMarca: "",
                idProveedor: parseInt($('#cboProveedor').val()),
                nombreProveedor: "",
                idUnidad: parseInt($('#cboUnidad').val()),
                nombreUnidad: "",
                fechaExpiracion: $('#txtFechaExpiracion').val() || null,
                estado: true
            };

            ejecutarAjax("/Producto/Crear", "POST", modelo, modalAdd);
        });

        $('#formEditarProducto').on('submit', function (e) {
            e.preventDefault();

            const modelo = {
                idProducto: parseInt($('#idProductoEditar').val()),
                nombreProducto: $('#nombreProductoEditar').val(),
                descripcion: $('#descripcionEditar').val(),
                stock: parseInt($('#stockEditar').val()) || 0,
                stockMinimo: parseInt($('#stockMinimoEditar').val()) || 0,
                idCategoria: parseInt($('#idCategoriaEditar').val()),
                nombreCategoria: "",
                idMarca: parseInt($('#idMarcaEditar').val()),
                nombreMarca: "",
                idProveedor: parseInt($('#idProveedorEditar').val()),
                nombreProveedor: "",
                idUnidad: parseInt($('#idUnidadEditar').val()),
                nombreUnidad: "",
                fechaExpiracion: $('#fechaExpiracionEditar').val() || null,
                estado: $('#estadoEditar').val() === "true"
            };

            ejecutarAjax("/Producto/Editar", "PUT", modelo, modalUpdate);
        });
    }
});

function llenarSelect(url, selectId, propId, propNombre) {
    return $.get(url, function (res) {
        const $select = $(selectId);
        const lista = res.data ? res.data : res;
        $select.empty().append('<option value="">Seleccionar...</option>');
        lista.forEach(item => {
            $select.append(`<option value="${item[propId]}">${item[propNombre]}</option>`);
        });
    });
}

function cargarSelectsEditar() {
    return Promise.all([
        llenarSelect('/Categoria/ListarCategorias', '#idCategoriaEditar', 'idCategoria', 'nombreCategoria'),
        llenarSelect('/Marca/ListaMarca', '#idMarcaEditar', 'idMarca', 'nombreMarca'),
        llenarSelect('/Proveedor/ListaProveedor', '#idProveedorEditar', 'idProveedor', 'nombreProveedor'),
        llenarSelect('/UnidadMedida/MedidaLista', '#idUnidadEditar', 'idUnidad', 'nombreUnidad')
    ]);
}

function cargarSelectsAgregar() {
    return Promise.all([
        llenarSelect('/Categoria/ListarCategorias', '#cboCategoria', 'idCategoria', 'nombreCategoria'),
        llenarSelect('/Marca/ListaMarca', '#cboMarca', 'idMarca', 'nombreMarca'),
        llenarSelect('/Proveedor/ListaProveedor', '#cboProveedor', 'idProveedor', 'nombreProveedor'),
        llenarSelect('/UnidadMedida/MedidaLista', '#cboUnidad', 'idUnidad', 'nombreUnidad')
    ]);
}


window.abrirModalRegistro = () => {
    if (ROL_VISTA !== "admin" && ROL_VISTA !== "operador") return;
    $('#formProducto')[0].reset();
    $('#txtIdProducto').val("0");
    cargarSelectsAgregar().then(() => {
        modalAdd.show();
    });
};

window.mostrarModalEditar = (id) => {
    if (ROL_VISTA !== "admin" && ROL_VISTA !== "operador") return;
    $.get(`/Producto/Obtener?idProducto=${id}`, function (res) {
        if (res.estado) {
            const p = res.objeto;
            cargarSelectsEditar().then(() => {
                $('#idProductoEditar').val(p.idProducto);
                $('#nombreProductoEditar').val(p.nombreProducto);
                $('#descripcionEditar').val(p.descripcion);
                $('#stockEditar').val(p.stock);
                $('#stockMinimoEditar').val(p.stockMinimo);

                if (p.fechaExpiracion) {
                    $('#fechaExpiracionEditar').val(p.fechaExpiracion.split('T')[0]);
                } else {
                    $('#fechaExpiracionEditar').val("");
                }

                $('#idCategoriaEditar').val(p.idCategoria);
                $('#idMarcaEditar').val(p.idMarca);
                $('#idProveedorEditar').val(p.idProveedor);
                $('#idUnidadEditar').val(p.idUnidad);
                $('#estadoEditar').val(p.estado.toString());

                modalUpdate.show();
            });
        } else {
            toastr.error("No se pudo obtener la información");
        }
    });
};


window.mostrarModalVer = (id) => {
    $.get(`/Producto/Obtener?idProducto=${id}`, function (res) {
        if (res.estado) {
            const p = res.objeto;

            $('#lblNombre').val(p.nombreProducto || 'N/A');
            $('#lblCategoria').val(p.nombreCategoria || 'N/A');
            $('#lblMarca').val(p.nombreMarca || 'N/A');
            $('#lblProveedor').val(p.nombreProveedor || 'N/A');
            $('#lblUnidad').val(p.nombreUnidad || 'N/A');
            $('#lblStock').val(p.stock ?? 0);
            $('#lblStockMinimo').val(p.stockMinimo ?? 0);
            $('#lblDescripcion').val(p.descripcion || 'Sin descripción disponible.');

            if (p.fechaExpiracion) {
                const fecha = new Date(p.fechaExpiracion);
                $('#lblFechaExpiracion').val(fecha.toLocaleDateString('es-ES', { timeZone: 'UTC' }));
            } else {
                $('#lblFechaExpiracion').val("No expira / No especificado");
            }

            const badgeEstado = p.estado
                ? '<span class="badge bg-success fs-6 px-3"><i class="fa-solid fa-check-circle me-1"></i> Activo</span>'
                : '<span class="badge bg-danger fs-6 px-3"><i class="fa-solid fa-ban me-1"></i> Inactivo</span>';
            $('#lblEstadoContainer').html(badgeEstado);

            modalView.show();
        } else {
            toastr.error("No se pudo obtener el detalle del producto");
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
                if (modal) modal.hide();
                tablaProductos.ajax.reload(null, false);
            } else {
                toastr.error(res.mensaje);
            }
        },
        error: function (xhr) {
            console.error("Error en servidor:", xhr.responseText);
            if (xhr.status === 403 || xhr.status === 401) {
                toastr.error("No autorizado. Tu rol actual no permite realizar esta modificación.");
            } else {
                toastr.error("Error crítico al procesar la solicitud");
            }
        }
    });
}

window.eliminarProducto = (id) => {
    if (ROL_VISTA !== "admin") return;
    Swal.fire({
        title: "¿Eliminar Producto?",
        text: "Esta acción no se puede deshacer",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Sí, eliminar",
        cancelButtonText: "Cancelar"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: `/Producto/Eliminar?idProducto=${id}`,
                type: "DELETE",
                success: function (res) {
                    if (res.estado) {
                        toastr.success(res.mensaje);
                        tablaProductos.ajax.reload(null, false);
                    } else {
                        toastr.error(res.mensaje);
                    }
                },
                error: function (xhr) {
                    if (xhr.status === 403 || xhr.status === 401) {
                        toastr.error("No autorizado para eliminar este producto.");
                    } else {
                        toastr.error("No se pudo conectar con el servidor");
                    }
                }
            });
        }
    });
};
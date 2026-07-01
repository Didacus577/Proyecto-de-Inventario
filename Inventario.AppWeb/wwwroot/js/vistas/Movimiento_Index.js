let tablaMovimientos;
let modalAdd;
let modalUpdate;
let stockActualProducto = 0;
let idUsuarioDelRegistro = 0; 
let ROL_MOVIMIENTO = "admin"; 



$(document).ready(function () {
    toastr.options = { "closeButton": true, "progressBar": true, "positionClass": "toast-top-right" };

  
    const rolAtributo = $('#tbdata').data('role') || $('#tbdata').data('rol');
    if (rolAtributo) {
        ROL_MOVIMIENTO = rolAtributo.toLowerCase();
    }

    modalAdd = new bootstrap.Modal(document.getElementById('modalAdd'));
    modalUpdate = new bootstrap.Modal(document.getElementById('modalUpdate'));

    tablaMovimientos = $('#tbdata').DataTable({
        responsive: true,
        ajax: {
            url: '/Movimiento/ListarHistorial',
            type: "GET",
            dataSrc: "data"
        },
        columns: [
           
            {
                data: null,
                render: row => {
                    const f = row.fechaMovimiento || row.FechaMovimiento;
                    return f ? new Date(f).toLocaleString() : "";
                }
            },
            {
                data: null,
                render: row => row.nombreProducto || row.NombreProducto || ""
            },
          
            {
                data: null,
                render: row => {
                    const d = row.tipoMovimiento || row.TipoMovimiento || "";
                    return `<span class="badge bg-${d === 'Entrada' ? 'success' : 'danger'}">${d}</span>`;
                }
            },
          
            {
                data: null,
                render: row => row.cantidad !== undefined ? (row.cantidad ?? row.Cantidad) : ""
            },
           
            {
                data: null,
                render: row => row.nombreUsuario || row.NombreUsuario || ""
            },
          
            {
                data: null,
                render: function (row) {
                    const id = row.idMovimiento || row.IdMovimiento;

                    
                    if (ROL_MOVIMIENTO === "admin") {
                        return `<div class="text-center">
                                    <button class="btn btn-sm btn-warning me-1" onclick="mostrarModalEditar(${id})"><i class="fa-solid fa-pen"></i></button>
                                    <button class="btn btn-sm btn-danger" onclick="eliminarMovimiento(${id})"><i class="fa-solid fa-trash"></i></button>
                                </div>`;
                    }
                    
                    return `<div class="text-center">
                                <button class="btn btn-sm btn-warning" onclick="mostrarModalEditar(${id})"><i class="fa-solid fa-pen me-1"></i> Corregir</button>
                            </div>`;
                },
                orderable: false
            }
        ],
        language: { url: "https://cdn.datatables.net/plug-ins/1.11.5/i18n/es-ES.json" }
    });

    $(document).on('change', '#cboProductoAdd', function () {
        stockActualProducto = $('option:selected', this).data('stock') ?? 0;
        $('#stockInfoAdd').text('Stock disponible: ' + stockActualProducto);
    });

    $('#btnGuardarNuevo').on('click', function () {
        const tipo = $('#cboTipoAdd').val();
        const cantidad = parseInt($('#txtCantidadAdd').val());
        if (tipo === "Salida" && cantidad > stockActualProducto) {
            toastr.error(`Stock insuficiente (${stockActualProducto})`);
            return;
        }
        const modelo = {
            idProducto: parseInt($('#cboProductoAdd').val()),
            tipoMovimiento: tipo,
            cantidad: cantidad,
            idUsuario: 1
        };
        ejecutarAjaxMovimiento("/Movimiento/Crear", "POST", modelo, modalAdd);
    });

    $('#formUpdate').on('submit', function (e) {
        e.preventDefault();
        const modelo = {
            idMovimiento: parseInt($('#txtIdMovimientoUpdate').val()),
            idProducto: parseInt($('#cboProductoUpdate').val()),
            tipoMovimiento: $('#cboTipoUpdate').val(),
            cantidad: parseInt($('#txtCantidadUpdate').val()),
            idUsuario: idUsuarioDelRegistro
        };
        ejecutarAjaxMovimiento("/Movimiento/Editar", "PUT", modelo, modalUpdate);
    });
});

function cargarProductos(selectId, valorSeleccionado = null) {
    $.get('/Producto/ListarProductos', function (res) {
        const $select = $(selectId);
        $select.empty().append('<option value="">Seleccionar...</option>');
        const lista = res.data || res;
        lista.forEach(item => {
            $select.append(`<option value="${item.idProducto}" data-stock="${item.stock}">${item.nombreProducto}</option>`);
        });
        if (valorSeleccionado) $select.val(valorSeleccionado);
    });
}

window.abrirModalRegistro = () => {
    $('#formAdd')[0].reset();
    $('#stockInfoAdd').text('');
    cargarProductos('#cboProductoAdd');
    modalAdd.show();
};

window.mostrarModalEditar = (id) => {
    $.get(`/Movimiento/Obtener?idMovimiento=${id}`, function (res) {
        const data = res.idMovimiento || res.IdMovimiento ? res : res.objeto;
        if (data) {
            $('#txtIdMovimientoUpdate').val(data.idMovimiento || data.IdMovimiento);
            $('#cboTipoUpdate').val(data.tipoMovimiento || data.TipoMovimiento);
            $('#txtCantidadUpdate').val(data.cantidad || data.Cantidad);
            idUsuarioDelRegistro = data.idUsuario || data.IdUsuario;
            cargarProductos('#cboProductoUpdate', data.idProducto || data.IdProducto);
            modalUpdate.show();
        }
    });
};

function ejecutarAjaxMovimiento(url, metodo, data, modal) {
    $.ajax({
        url: url,
        type: metodo,
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(data),
        success: function (res) {
            if (res.mensaje || res.estado) {
                toastr.success(res.mensaje || "Éxito");
                modal.hide();
                tablaMovimientos.ajax.reload(null, false);
            }
        },
        error: (xhr) => toastr.error(xhr.responseJSON?.mensaje || xhr.responseText || "Error")
    });
}

window.eliminarMovimiento = (id) => {
    Swal.fire({ title: "¿Eliminar?", icon: "warning", showCancelButton: true }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: `/Movimiento/Eliminar?idMovimiento=${id}`,
                type: 'DELETE',
                success: (res) => {
                    toastr.success(res.mensaje);
                    tablaMovimientos.ajax.reload(null, false);
                },
                error: (xhr) => toastr.error(xhr.responseText || "No se pudo eliminar.")
            });
        }
    });
};
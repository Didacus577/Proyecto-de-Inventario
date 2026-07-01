
window.descargarReporteMovimientosPDF = () => {
   
    const tablaOriginal = document.getElementById('tbdata');

    if (!tablaOriginal || tablaOriginal.rows.length <= 1) {
        toastr.warning("No hay datos disponibles en el historial de movimientos para exportar.");
        return;
    }

 
    const rolVista = (tablaOriginal.getAttribute('data-role') || tablaOriginal.getAttribute('data-rol') || 'Usuario').toUpperCase();

    toastr.info("Preparando el PDF de Movimientos...", "Por favor espere", { timeOut: 1500 });

   
    const tablaClonada = tablaOriginal.cloneNode(true);

    
    const filas = tablaClonada.querySelectorAll('tr');
    filas.forEach(fila => {
        if (fila.lastElementChild) {
            fila.removeChild(fila.lastElementChild);
        }
    });

    
    const contenedorReporte = document.createElement('div');
    contenedorReporte.innerHTML = `
        <div style="padding: 35px; font-family: 'Segoe UI', Helvetica, Arial, sans-serif; color: #212529;">
            
            <div style="text-align: center; margin-bottom: 30px; border-bottom: 3px solid #0d6efd; padding-bottom: 15px;">
                <h1 style="color: #0d6efd; margin: 0; font-size: 26px; font-weight: 700; letter-spacing: 0.5px; text-transform: uppercase;">
                    Reporte de Movimientos
                </h1>
                <p style="color: #6c757d; margin: 6px 0 0 0; font-size: 14px; font-weight: 500;">
                    Historial de Entradas, Salidas y Ajustes de Stock
                </p>
                <p style="color: #495057; margin: 4px 0 0 0; font-size: 12px; font-style: italic;">
                    Fecha de impresión: ${new Date().toLocaleDateString('es-ES')} a las ${new Date().toLocaleTimeString('es-ES')}
                </p>
            </div>
            
            <div class="table-responsive" style="margin-top: 10px;">
                ${tablaClonada.outerHTML}
            </div>            
           
        </div>
    `;

    const opciones = {
        margin: 12,
        filename: `Reporte_Movimientos_${new Date().toISOString().slice(0, 10)}.pdf`,
        image: { type: 'jpeg', quality: 0.98 },
        html2canvas: { scale: 2, useCORS: true, logging: false },
        jsPDF: { unit: 'mm', format: 'letter', orientation: 'landscape' }
    };


    html2pdf()
        .set(opciones)
        .from(contenedorReporte)
        .save()
        .then(() => {
            toastr.success("PDF de Movimientos descargado con éxito.");
        })
        .catch(err => {
            console.error("Error al exportar PDF:", err);
            toastr.error("No se pudo procesar la exportación del archivo.");
        });
};
document.addEventListener("DOMContentLoaded", function () {

    // Función para crear gráfico
    function crearGrafico(idCanvas, labels, data, color, titulo) {
        const ctx = document.getElementById(idCanvas).getContext('2d');
        new Chart(ctx, {
            type: 'bar',
            data: {
                labels: labels,
                datasets: [{
                    label: titulo,
                    data: data,
                    backgroundColor: color,
                    borderColor: color.replace('0.6', '1'),
                    borderWidth: 1
                }]
            },
            options: {
                responsive: true,
                plugins: {
                    legend: { display: false },
                    title: {
                        display: true,
                        text: titulo
                    }
                },
                scales: {
                    y: {
                        beginAtZero: true
                    }
                }
            }
        });
    }

    // Función genérica para obtener datos y crear gráfico
    function cargarGrafico(endpoint, idCanvas, color, titulo) {
        fetch(endpoint)
            .then(response => response.json())
            .then(res => {
                console.log(`Datos de ${titulo}:`, res); // 👈 Verificar qué llega
                if (!res.data || res.data.length === 0) {
                    console.warn(`No hay datos para ${titulo}`);
                    return;
                }

                // Detectar propiedad NombreProducto / nombreProducto
                const labels = res.data.map(p => p.NombreProducto ?? p.nombreProducto ?? "Sin nombre");
                const cantidades = res.data.map(p => p.Cantidad ?? p.cantidad ?? 0);

                crearGrafico(idCanvas, labels, cantidades, color, titulo);
            })
            .catch(err => console.error(`Error cargando ${titulo}:`, err));
    }

    // Cargar ambos gráficos
    cargarGrafico('/Dashboard/TopSalidas', 'productosSalida', 'rgba(255, 99, 132, 0.6)', 'Productos con más salida');
    cargarGrafico('/Dashboard/TopEntradas', 'productosEntrada', 'rgba(54, 162, 235, 0.6)', 'Productos con más entrada');

});

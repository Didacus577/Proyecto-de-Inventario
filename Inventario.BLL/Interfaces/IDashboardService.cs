using Inventario.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Inventario.BLL.Interfaces
{
    public interface IDashboardService
    {
        Task<List<MovimientosInventario>> TopSalidas(int dias = 7, int top = 3);
        Task<List<MovimientosInventario>> TopEntradas(int dias = 7, int top = 3);
    }
}

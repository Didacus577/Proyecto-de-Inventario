using Inventario.DTOS;
using Inventario.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Inventario.BLL.Interfaces
{
    public interface IMarcaService
    {
        Task<List<MarcasDTO>> Lista();
        Task<MarcasDTO> Crear(MarcasDTO entidad);
        Task<bool> Editar(MarcasDTO entidad);
        Task<bool> Eliminar(int idMarca);
        Task<MarcasDTO> ObtenerPorId(int idMarca);
    }
}

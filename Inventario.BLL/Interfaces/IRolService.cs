using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventario.DTOS;
using Inventario.Entity;

namespace Inventario.BLL.Interfaces
{
    public interface IRolService
    {
        
        Task<List<RolDTO>> Lista();

       
        Task<RolDTO> Crear(RolDTO entidad);

        
        Task<bool> Editar(RolDTO entidad);

        
        Task<bool> Eliminar(int idRol);

        
        Task<RolDTO> ObtenerPorId(int idRol);
    }
}

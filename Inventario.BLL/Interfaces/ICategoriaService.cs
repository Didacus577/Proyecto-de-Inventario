using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventario.DTOS;
using Inventario.Entity;

namespace Inventario.BLL.Interfaces
{
    public interface ICategoriaService
    {
        Task<List<CategoriaDTO>> Lista();

        Task<CategoriaDTO> Crear(CategoriaDTO entidad);
        Task<bool> Editar(CategoriaDTO entidad);
        Task<bool> Eliminar(int idcategoria);
        Task<CategoriaDTO> ObtenerPorId(int idcategoria);


    }
}

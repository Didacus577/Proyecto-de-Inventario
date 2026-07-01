using Inventario.Entity;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Inventario.DAL.Interfaces
{
    public interface IProductosRepository : IGenericRepository<Productos>
    {

        Task<int> EjecutarSP(string nombreSP, List<SqlParameter> parametros);

        
        
    }
}

using Inventario.DAL.Implementacion;
using Inventario.DAL.Interfaces;
using Inventario.DAL.SistemaInventarioContext;
using Inventario.Entity;
using Microsoft.Data.SqlClient; 
using Microsoft.EntityFrameworkCore;


public class ProductosRepository : GenericRepository<Productos>, IProductosRepository
{
    private readonly SistemaInventarioContext _dbContext;
    public ProductosRepository(SistemaInventarioContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    
    public async Task<int> EjecutarSP(string nombreSP, List<SqlParameter> parametros)
    {
       
        string sqlCommand = $"EXEC {nombreSP} ";
        string[] paramNames = parametros.Select(p => p.ParameterName).ToArray();
        sqlCommand += string.Join(", ", paramNames);

      
        try
        {
            // Ejecución asíncrona del comando SQL
            int resultado = await _dbContext.Database.ExecuteSqlRawAsync(sqlCommand, parametros.ToArray());
            return resultado;
        }
        catch (Exception ex)
        {
            // Puedes registrar el error aquí o dejar que se maneje en el BLL
            throw new Exception($"Error al ejecutar el SP {nombreSP} en DAL: {ex.Message}", ex);
        }
    }

    
}
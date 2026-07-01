using Inventario.BLL.Implementacion;
using Inventario.BLL.Interfaces;
using Inventario.DAL.Implementacion;
using Inventario.DAL.Interfaces;
using Inventario.DAL.SistemaInventarioContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Inventario.BLL.Servicios;


namespace Inventario.IOC
{
    public static class Dependencia
    {
        public static void InyectarDependencia(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<SistemaInventarioContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("CadenaSql"));
            });

            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            

            services.AddScoped<IRolService, RolService>();
            services.AddScoped<IProductosService, ProductoService>();
            services.AddScoped<IProveedorService, ProveedorService>();
            services.AddScoped<IProductosRepository, ProductosRepository>();
            services.AddScoped<IMovimientoService, MovimientoService>();
            services.AddScoped<IUsuariosService, UsuariosService>();
            services.AddScoped<IUtilidadesService, UtilidadesService>();
            services.AddScoped<IUnidadesService, UnidadesMedidaService>();
            services.AddScoped<ICategoriaService, CategoriaSevice>();
            services.AddScoped<IMarcaService, MarcaService>();
            services.AddScoped<IProveedorService, ProveedorService>();          
            services.AddScoped<ILoginService, LoginService >();
            services.AddScoped<AuthService>();
        }
    }
}

using Inventario.AppWeb.Utilidades.Automapper;
using Inventario.IOC;
using Inventario.AppWeb.Extensions;
using Inventario.BLL.Utilidades.Automapper;
using Microsoft.Extensions.DependencyInjection;


namespace Inventario.AppWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

           
            builder.Services.AddControllersWithViews();

            
            builder.Services.AddSession();

            // Acceso al HttpContext (necesario para leer sesión en Layout)
            builder.Services.AddHttpContextAccessor();

            
            builder.Services.InyectarDependencia(builder.Configuration);

            // AutoMapper
            builder.Services.AddAutoMapper(cfg => { }, typeof(AutoMapperProfile));

            builder.Services.AddAutoMapper(cfg => { }, typeof(EntidadDtoProfile));
           

            builder.Services.AddJwtAuthentication(builder.Configuration);


            var app = builder.Build();

            // ================================
            //  Middlewares del pipeline
            // ================================
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            // ? IMPORTANTE: antes de auth
            app.UseSession();

            app.UseAuthentication(); // Fundamental: Debe ir antes de Authorization
            app.UseAuthorization();


            // ================================
            //  Ruta por defecto ? Login
            // ================================
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Login}/{action=IniciaSesion}/{id?}"
            );

            app.Run();
        }
    }
}

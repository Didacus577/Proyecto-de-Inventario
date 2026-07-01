using Inventario.AppWeb.Models.ViewModels;
using Inventario.Entity;
using Inventario.DTOS;
using AutoMapper;

using Inventario.AplicacionWeb.Models.ViewModels;

namespace Inventario.AppWeb.Utilidades.Automapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            #region MovimientoInventario

            
            CreateMap<MovimientoInventarioDTO, VMMovimiento>()
           .ForMember(dest => dest.IdMovimiento, opt => opt.MapFrom(src => src.IdMovimiento))
           .ForMember(dest => dest.FechaMovimiento, opt => opt.MapFrom(src => src.FechaMovimiento))
           .ForMember(dest => dest.IdProducto, opt => opt.MapFrom(src => src.IdProducto))
    
           .ForMember(dest => dest.NombreProducto, opt => opt.MapFrom(src => src.NombreProducto ?? "Sin nombre"))
           .ForMember(dest => dest.Cantidad, opt => opt.MapFrom(src => src.Cantidad))
           .ForMember(dest => dest.TipoMovimiento, opt => opt.MapFrom(src => src.TipoMovimiento ?? "N/A"))
           .ForMember(dest => dest.IdUsuario, opt => opt.MapFrom(src => src.IdUsuario))    
           .ForMember(dest => dest.NombreUsuario, opt => opt.MapFrom(src => src.NombreUsuario ?? "Sin usuario"));

            
            CreateMap<VMMovimiento, MovimientoInventarioDTO>()
                .ForMember(dest => dest.IdMovimiento, opt => opt.MapFrom(src => src.IdMovimiento))
                .ForMember(dest => dest.FechaMovimiento, opt => opt.MapFrom(src => src.FechaMovimiento))
                .ForMember(dest => dest.IdProducto, opt => opt.MapFrom(src => src.IdProducto))
                .ForMember(dest => dest.Cantidad, opt => opt.MapFrom(src => src.Cantidad))
                .ForMember(dest => dest.TipoMovimiento, opt => opt.MapFrom(src => src.TipoMovimiento))
                .ForMember(dest => dest.IdUsuario, opt => opt.MapFrom(src => src.IdUsuario))
                
                .ForMember(dest => dest.NombreProducto, opt => opt.Ignore())
                .ForMember(dest => dest.NombreUsuario, opt => opt.Ignore());
            #endregion

            #region Roles


            CreateMap<RolDTO, VMRoles>()
                .ForMember(dest => dest.IdRol,
                    origen => origen.MapFrom(src => src.IdRol))
                .ForMember(dest => dest.NombreRol,
                    origen => origen.MapFrom(src => src.NombreRol));
            
            CreateMap<VMRoles, RolDTO>()
                .ForMember(dest => dest.IdRol,
                    origen => origen.MapFrom(src => src.IdRol))
                .ForMember(dest => dest.NombreRol,
                    origen => origen.MapFrom(src => src.NombreRol));

            #endregion

            #region Usuario

            CreateMap<UsuarioDTO, VMUsuarios>()
           .ForMember(dest => dest.IdUsuario, opt => opt.MapFrom(src => src.IdUsuario))
           .ForMember(dest => dest.NombreUsuario, opt => opt.MapFrom(src => src.NombreUsuario))
           .ForMember(dest => dest.Correo, opt => opt.MapFrom(src => src.Correo))    
           .ForMember(dest => dest.IdRol, opt => opt.MapFrom(src => src.IdRol))
           .ForMember(dest => dest.NombreRol, opt => opt.MapFrom(src => src.NombreRol))           
           .ForMember(dest => dest.Clave, opt => opt.MapFrom(src => src.Clave));

           
            CreateMap<VMUsuarios, UsuarioDTO>()
            .ForMember(dest => dest.IdUsuario, opt => opt.MapFrom(src => src.IdUsuario))
            .ForMember(dest => dest.NombreUsuario, opt => opt.MapFrom(src => src.NombreUsuario))
            .ForMember(dest => dest.Correo, opt => opt.MapFrom(src => src.Correo))                
            .ForMember(dest => dest.IdRol, opt => opt.MapFrom(src => src.IdRol))
            .ForMember(dest => dest.NombreRol, opt => opt.MapFrom(src => src.NombreRol))                
            .ForMember(dest => dest.Clave, opt => opt.MapFrom(src => src.Clave));
            #endregion


            #region UnidadesMedida

            CreateMap<UnidadMedidaDTO, VMUnidades>()
             .ForMember(dest => dest.IdUnidad,
                  origen => origen.MapFrom(src => src.IdUnidad))
             .ForMember(dest => dest.NombreUnidad,
                  origen => origen.MapFrom(src => src.NombreUnidad))
             .ForMember(dest => dest.Abreviatura,
                  origen => origen.MapFrom(src => src.Abreviatura));

            
            CreateMap<VMUnidades, UnidadMedidaDTO>()
                .ForMember(dest => dest.IdUnidad,
                    origen => origen.MapFrom(src => src.IdUnidad))
                .ForMember(dest => dest.NombreUnidad,
                    origen => origen.MapFrom(src => src.NombreUnidad))
                .ForMember(dest => dest.Abreviatura,
                    origen => origen.MapFrom(src => src.Abreviatura));
            #endregion

            #region Categoria

            // De Entidad → ViewModel
            CreateMap<CategoriaDTO, VMCategoria>()
                  .ForMember(dest => dest.IdCategoria,
                    origen => origen.MapFrom(src => src.IdCategoria))
                .ForMember(dest => dest.NombreCategoria,
                    origen => origen.MapFrom(src => src.NombreCategoria))
                .ForMember(dest => dest.DescripcionCategoria,
                    origen => origen.MapFrom(src => src.Descripcion));

            CreateMap<VMCategoria, CategoriaDTO>()
                 .ForMember(dest => dest.IdCategoria,
                    origen => origen.MapFrom(src => src.IdCategoria))
                .ForMember(dest => dest.NombreCategoria,
                    origen => origen.MapFrom(src => src.NombreCategoria))
                .ForMember(dest => dest.Descripcion,
                    origen => origen.MapFrom(src => src.DescripcionCategoria));

            #endregion

            #region Marcas
            CreateMap<MarcasDTO, VMMarca>()
                .ForMember(dest => dest.IdMarca, opt => opt.MapFrom(src => src.IdMarca))
                .ForMember(dest => dest.NombreMarca, opt => opt.MapFrom(src => src.NombreMarca))
                .ForMember(dest => dest.Descripcion, opt => opt.MapFrom(src => src.Descripcion));

            CreateMap<VMMarca, MarcasDTO>()
                .ForMember(dest => dest.IdMarca, opt => opt.MapFrom(src => src.IdMarca))
                .ForMember(dest => dest.NombreMarca, opt => opt.MapFrom(src => src.NombreMarca))
                .ForMember(dest => dest.Descripcion, opt => opt.MapFrom(src => src.Descripcion));

            #endregion

            #region Producto
            CreateMap<ProductosDTO, VMProducto>()
     // Mapeo directo de campos escalares
             .ForMember(dest => dest.IdProducto, opt => opt.MapFrom(src => src.IdProducto))
             .ForMember(dest => dest.NombreProducto, opt => opt.MapFrom(src => src.NombreProducto))
             .ForMember(dest => dest.Descripcion, opt => opt.MapFrom(src => src.Descripcion))
             .ForMember(dest => dest.Stock, opt => opt.MapFrom(src => src.Stock))
             .ForMember(dest => dest.StockMinimo, opt => opt.MapFrom(src => src.StockMinimo))
             .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => src.Estado))

     // CORRECCIÓN: Formateo de fecha para el input HTML (yyyy-MM-dd)
             .ForMember(dest => dest.FechaExpiracion, opt => opt.MapFrom(src =>
               src.FechaExpiracion.HasValue ? src.FechaExpiracion.Value.ToString("yyyy-MM-dd") : null))

     // Mapeo de CLAVES FORÁNEAS
             .ForMember(dest => dest.IdCategoria, opt => opt.MapFrom(src => src.IdCategoria))
             .ForMember(dest => dest.IdMarca, opt => opt.MapFrom(src => src.IdMarca))
             .ForMember(dest => dest.IdProveedor, opt => opt.MapFrom(src => src.IdProveedor))
             .ForMember(dest => dest.IdUnidad, opt => opt.MapFrom(src => src.IdUnidad))

     // Mapeo de NOMBRES (Con control de nulos para evitar que se detenga la depuración)
             .ForMember(dest => dest.NombreCategoria, opt => opt.MapFrom(src => src.NombreCategoria ?? "Sin nombre"))
             .ForMember(dest => dest.NombreMarca, opt => opt.MapFrom(src => src.NombreMarca ?? "Sin nombre"))
             .ForMember(dest => dest.NombreProveedor, opt => opt.MapFrom(src => src.NombreProveedor ?? "Sin nombre"))
             .ForMember(dest => dest.NombreUnidad, opt => opt.MapFrom(src => src.NombreUnidad ?? "Sin nombre"));

            // --- VM a DTO (Para guardar cambios) ---
            // --- VM a DTO (Para Guardar y Editar) ---
            CreateMap<VMProducto, ProductosDTO>()
                .ForMember(dest => dest.IdProducto, opt => opt.MapFrom(src => src.IdProducto))
                .ForMember(dest => dest.NombreProducto, opt => opt.MapFrom(src => src.NombreProducto))
                .ForMember(dest => dest.Descripcion, opt => opt.MapFrom(src => src.Descripcion))
                .ForMember(dest => dest.Stock, opt => opt.MapFrom(src => src.Stock))
                .ForMember(dest => dest.StockMinimo, opt => opt.MapFrom(src => src.StockMinimo))
                .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => src.Estado))

                // CORRECCIÓN PARA DateOnly?:
                .ForMember(dest => dest.FechaExpiracion, opt => opt.MapFrom(src =>
                    string.IsNullOrEmpty(src.FechaExpiracion)
                    ? (DateOnly?)null
                    : DateOnly.Parse(src.FechaExpiracion))) // El input date envía "yyyy-MM-dd", compatible con Parse

                .ForMember(dest => dest.IdCategoria, opt => opt.MapFrom(src => src.IdCategoria))
                .ForMember(dest => dest.IdMarca, opt => opt.MapFrom(src => src.IdMarca))
                .ForMember(dest => dest.IdProveedor, opt => opt.MapFrom(src => src.IdProveedor))
                .ForMember(dest => dest.IdUnidad, opt => opt.MapFrom(src => src.IdUnidad))

                // Ignorar propiedades de navegación en el destino
                .ForMember(dest => dest.NombreCategoria, opt => opt.Ignore())
                .ForMember(dest => dest.NombreMarca, opt => opt.Ignore())
                .ForMember(dest => dest.NombreProveedor, opt => opt.Ignore())
                .ForMember(dest => dest.NombreUnidad, opt => opt.Ignore());
            #endregion


            #region Proveedores

            // De Entidad → ViewModel
            CreateMap<ProveedoresDTO, VMProveedor>()
                .ForMember(dest => dest.IdProveedor,
                    origen => origen.MapFrom(src => src.IdProveedor))
                .ForMember(dest => dest.NombreProveedor,
                    origen => origen.MapFrom(src => src.NombreProveedor))
                .ForMember(dest => dest.Telefono,
                    origen => origen.MapFrom(src => src.Telefono))
                .ForMember(dest => dest.Correo,
                    origen => origen.MapFrom(src => src.Correo))
                .ForMember(dest => dest.Direccion,
                    origen => origen.MapFrom(src => src.Direccion));

            // De ViewModel → Entidad
            CreateMap<VMProveedor, ProveedoresDTO>()
                .ForMember(dest => dest.IdProveedor,
                    origen => origen.MapFrom(src => src.IdProveedor))
                .ForMember(dest => dest.NombreProveedor,
                    origen => origen.MapFrom(src => src.NombreProveedor))
                .ForMember(dest => dest.Telefono,
                    origen => origen.MapFrom(src => src.Telefono))
                .ForMember(dest => dest.Correo,
                    origen => origen.MapFrom(src => src.Correo))
                .ForMember(dest => dest.Direccion,
                    origen => origen.MapFrom(src => src.Direccion));
                

            #endregion



        }
    }
}

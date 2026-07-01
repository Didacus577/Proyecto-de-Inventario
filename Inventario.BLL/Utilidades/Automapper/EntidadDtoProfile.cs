using Inventario.Entity;
using AutoMapper;
using Inventario.DTOS;



namespace Inventario.BLL.Utilidades.Automapper
{
    public class EntidadDtoProfile : Profile
    {
        public EntidadDtoProfile()
        {
            #region MovimientoInventario
           
            CreateMap<MovimientosInventario, MovimientoInventarioDTO>()
                .ForMember(dest => dest.IdMovimiento,
                    opt => opt.MapFrom(src => src.IdMovimiento))

                .ForMember(dest => dest.FechaMovimiento,
                    opt => opt.MapFrom(src => src.FechaMovimiento))

                .ForMember(dest => dest.Cantidad,
                    opt => opt.MapFrom(src => src.Cantidad))

                .ForMember(dest => dest.TipoMovimiento,
                    opt => opt.MapFrom(src => src.TipoMovimiento ?? "N/A"))

                
                .ForMember(dest => dest.IdProducto,
                    opt => opt.MapFrom(src => src.IdProducto))

                .ForMember(dest => dest.NombreProducto,
                    opt => opt.MapFrom(src => src.IdProductoNavigation != null
                        ? src.IdProductoNavigation.NombreProducto
                        : "Producto no encontrado"))

                
                .ForMember(dest => dest.IdUsuario,
                    opt => opt.MapFrom(src => src.IdUsuario))

                .ForMember(dest => dest.NombreUsuario,
                    opt => opt.MapFrom(src => src.IdUsuarioNavigation != null
                        ? src.IdUsuarioNavigation.NombreUsuario
                        : "Usuario no identificado"));


            
            CreateMap<MovimientoInventarioDTO, MovimientosInventario>()
                .ForMember(dest => dest.IdMovimiento,
                    opt => opt.MapFrom(src => src.IdMovimiento))

                .ForMember(dest => dest.FechaMovimiento,
                    opt => opt.MapFrom(src => src.FechaMovimiento))

                .ForMember(dest => dest.IdProducto,
                    opt => opt.MapFrom(src => src.IdProducto))

                .ForMember(dest => dest.Cantidad,
                    opt => opt.MapFrom(src => src.Cantidad))

                .ForMember(dest => dest.TipoMovimiento,
                    opt => opt.MapFrom(src => src.TipoMovimiento))

                .ForMember(dest => dest.IdUsuario,
                    opt => opt.MapFrom(src => src.IdUsuario))

               
                .ForMember(dest => dest.IdProductoNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.IdUsuarioNavigation, opt => opt.Ignore());

            #endregion

            #region Roles

            // De Entidad → ViewModel
            CreateMap<Roles, RolDTO>()
                .ForMember(dest => dest.IdRol,
                    origen => origen.MapFrom(src => src.IdRol))
                .ForMember(dest => dest.NombreRol,
                    origen => origen.MapFrom(src => src.NombreRol));

            // De ViewModel → Entidad
            CreateMap<RolDTO, Roles>()
                .ForMember(dest => dest.IdRol,
                    origen => origen.MapFrom(src => src.IdRol))
                .ForMember(dest => dest.NombreRol,
                    origen => origen.MapFrom(src => src.NombreRol));

            #endregion

            #region  Usuario
           
            CreateMap<Usuarios, UsuarioDTO>()
                .ForMember(dest => dest.IdUsuario,
                    opt => opt.MapFrom(src => src.IdUsuario))

                .ForMember(dest => dest.NombreUsuario,
                    opt => opt.MapFrom(src => src.NombreUsuario))

                .ForMember(dest => dest.Correo,
                    opt => opt.MapFrom(src => src.Correo))

               .ForMember(dest => dest.Clave, 
               opt => opt.Ignore())

                .ForMember(dest => dest.IdRol,
                    opt => opt.MapFrom(src => src.IdRol))

               
                .ForMember(dest => dest.NombreRol,
                    opt => opt.MapFrom(src => src.IdRolNavigation != null
                        ? src.IdRolNavigation.NombreRol
                        : "Sin Rol"));
           
          
            CreateMap<UsuarioDTO, Usuarios>()
                .ForMember(dest => dest.IdUsuario,
                    opt => opt.MapFrom(src => src.IdUsuario))

                .ForMember(dest => dest.NombreUsuario,
                    opt => opt.MapFrom(src => src.NombreUsuario))

                .ForMember(dest => dest.Correo,
                    opt => opt.MapFrom(src => src.Correo))

                .ForMember(dest => dest.Clave,
                    opt => opt.MapFrom(src => src.Clave))

                .ForMember(dest => dest.IdRol,
                    opt => opt.MapFrom(src => src.IdRol))

                .ForMember(dest => dest.IdRolNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.MovimientosInventarios, opt => opt.Ignore());

            #endregion


            #region UnidadesMedida

            CreateMap<UnidadesMedida, UnidadMedidaDTO>()
             .ForMember(dest => dest.IdUnidad,
                  origen => origen.MapFrom(src => src.IdUnidad))
             .ForMember(dest => dest.NombreUnidad,
                  origen => origen.MapFrom(src => src.NombreUnidad))
             .ForMember(dest => dest.Abreviatura,
                  origen => origen.MapFrom(src => src.Abreviatura));

            // De ViewModel → Entidad 
            CreateMap<UnidadMedidaDTO, UnidadesMedida>()
                .ForMember(dest => dest.IdUnidad,
                    origen => origen.MapFrom(src => src.IdUnidad))
                .ForMember(dest => dest.NombreUnidad,
                    origen => origen.MapFrom(src => src.NombreUnidad))
                .ForMember(dest => dest.Abreviatura,
                    origen => origen.MapFrom(src => src.Abreviatura));
            #endregion

            #region Categoria

            
            CreateMap<Categoria, CategoriaDTO>()
                  .ForMember(dest => dest.IdCategoria,
                    origen => origen.MapFrom(src => src.IdCategoria))
                .ForMember(dest => dest.NombreCategoria,
                    origen => origen.MapFrom(src => src.NombreCategoria))
                .ForMember(dest => dest.Descripcion,
                    origen => origen.MapFrom(src => src.Descripcion));

            CreateMap<CategoriaDTO, Categoria>()
                 .ForMember(dest => dest.IdCategoria,
                    origen => origen.MapFrom(src => src.IdCategoria))
                .ForMember(dest => dest.NombreCategoria,
                    origen => origen.MapFrom(src => src.NombreCategoria))
                .ForMember(dest => dest.Descripcion,
                    origen => origen.MapFrom(src => src.Descripcion));

            #endregion

            #region Marcas

            CreateMap<Marcas, MarcasDTO>()
                .ForMember(dest => dest.IdMarca, opt => opt.MapFrom(src => src.IdMarca))
                .ForMember(dest => dest.NombreMarca, opt => opt.MapFrom(src => src.NombreMarca))
                .ForMember(dest => dest.Descripcion, opt => opt.MapFrom(src => src.Descripcion));

            CreateMap<MarcasDTO, Marcas>()
                .ForMember(dest => dest.IdMarca, opt => opt.MapFrom(src => src.IdMarca))
                .ForMember(dest => dest.NombreMarca, opt => opt.MapFrom(src => src.NombreMarca))
                .ForMember(dest => dest.Descripcion, opt => opt.MapFrom(src => src.Descripcion));

            #endregion

            #region Producto
            CreateMap<Productos, ProductosDTO>()
                 
                  .ForMember(dest => dest.IdProducto, origen => origen.MapFrom(src => src.IdProducto))
                  .ForMember(dest => dest.NombreProducto, origen => origen.MapFrom(src => src.NombreProducto))
                  .ForMember(dest => dest.Descripcion, origen => origen.MapFrom(src => src.Descripcion))
                  .ForMember(dest => dest.Stock, origen => origen.MapFrom(src => src.Stock))
                  .ForMember(dest => dest.StockMinimo, origen => origen.MapFrom(src => src.StockMinimo))
                  .ForMember(dest => dest.Estado, origen => origen.MapFrom(src => src.Estado))
                  .ForMember(dest => dest.FechaExpiracion, origen => origen.MapFrom(src => src.FechaExpiracion))

                  
                  .ForMember(dest => dest.IdCategoria, origen => origen.MapFrom(src => src.IdCategoria))
                  .ForMember(dest => dest.IdMarca, origen => origen.MapFrom(src => src.IdMarca))
                  .ForMember(dest => dest.IdProveedor, origen => origen.MapFrom(src => src.IdProveedor))
                  .ForMember(dest => dest.IdUnidad, origen => origen.MapFrom(src => src.IdUnidad))

                  
                  .ForMember(dest => dest.NombreCategoria,
                      origen => origen.MapFrom(src => src.IdCategoriaNavigation.NombreCategoria))

                  .ForMember(dest => dest.NombreMarca,
                      origen => origen.MapFrom(src => src.IdMarcaNavigation.NombreMarca))

                  .ForMember(dest => dest.NombreProveedor,
                      origen => origen.MapFrom(src => src.IdProveedorNavigation.NombreProveedor))

                  .ForMember(dest => dest.NombreUnidad,
                      origen => origen.MapFrom(src => src.IdUnidadNavigation.NombreUnidad));

          
            CreateMap<ProductosDTO, Productos>()                
                .ForMember(dest => dest.IdProducto,
                origen => origen.MapFrom(src => src.IdProducto))
                .ForMember(dest => dest.NombreProducto,
                origen => origen.MapFrom(src => src.NombreProducto))
                .ForMember(dest => dest.Descripcion,
                origen => origen.MapFrom(src => src.Descripcion))
                .ForMember(dest => dest.Stock,
                origen => origen.MapFrom(src => src.Stock))
                .ForMember(dest => dest.StockMinimo,
                origen => origen.MapFrom(src => src.StockMinimo))
                .ForMember(dest => dest.Estado,
                origen => origen.MapFrom(src => src.Estado))
                .ForMember(dest => dest.FechaExpiracion,
                origen => origen.MapFrom(src => src.FechaExpiracion))

               
                .ForMember(dest => dest.IdCategoria,
                origen => origen.MapFrom(src => src.IdCategoria))
                .ForMember(dest => dest.IdMarca,
                origen => origen.MapFrom(src => src.IdMarca))
                .ForMember(dest => dest.IdProveedor,
                origen => origen.MapFrom(src => src.IdProveedor))
                .ForMember(dest => dest.IdUnidad,
                origen => origen.MapFrom(src => src.IdUnidad))

                
                .ForMember(dest => dest.IdCategoriaNavigation, origen => origen.Ignore())
                .ForMember(dest => dest.IdMarcaNavigation, origen => origen.Ignore())
                .ForMember(dest => dest.IdProveedorNavigation, origen => origen.Ignore())
                .ForMember(dest => dest.IdUnidadNavigation, origen => origen.Ignore())
                .ForMember(dest => dest.MovimientosInventarios, origen => origen.Ignore());



            #endregion


            #region Proveedores

            // De Entidad → ViewModel
            CreateMap<Proveedores, ProveedoresDTO>()
                  .ForMember(dest => dest.IdProveedor,
                    origen => origen.MapFrom(src => src.IdProveedor))
                .ForMember(dest => dest.NombreProveedor,
                    origen => origen.MapFrom(src => src.NombreProveedor));

            CreateMap<ProveedoresDTO, Proveedores>()
                 .ForMember(dest => dest.IdProveedor,
                    origen => origen.MapFrom(src => src.IdProveedor))
                 .ForMember(dest => dest.Telefono,
                    origen => origen.MapFrom(src=> src.Telefono))
                 .ForMember(dest => dest.Correo,
                    origen => origen.MapFrom(src => src.Correo))
                 .ForMember(dest => dest.Direccion,
                    origen => origen.MapFrom(src => src.Direccion));

            #endregion

           



        }
    }
}

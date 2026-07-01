using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Inventario.Entity;

namespace Inventario.DAL.SistemaInventarioContext;

public partial class SistemaInventarioContext : DbContext
{
    public SistemaInventarioContext()
    {
    }

    public SistemaInventarioContext(DbContextOptions<SistemaInventarioContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Categoria> Categoria { get; set; }

    public virtual DbSet<Marcas> Marcas { get; set; }

    public virtual DbSet<MovimientosInventario> MovimientosInventarios { get; set; }

    public virtual DbSet<Productos> Productos { get; set; }

    public virtual DbSet<Proveedores> Proveedores { get; set; }

    public virtual DbSet<Roles> Roles { get; set; }

    public virtual DbSet<UnidadesMedida> UnidadesMedida { get; set; }

    public virtual DbSet<Usuarios> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.IdCategoria).HasName("PK__Categori__A3C02A10330AF282");

            entity.Property(e => e.Descripcion).HasMaxLength(200);
            entity.Property(e => e.NombreCategoria).HasMaxLength(100);
        });

        modelBuilder.Entity<Marcas>(entity =>
        {
            entity.HasKey(e => e.IdMarca).HasName("PK__Marcas__4076A88742736EA1");

            entity.Property(e => e.Descripcion).HasMaxLength(200);
            entity.Property(e => e.NombreMarca).HasMaxLength(100);
        });

        modelBuilder.Entity<MovimientosInventario>(entity =>
        {
            entity.HasKey(e => e.IdMovimiento).HasName("PK__Movimien__881A6AE03A4F4D25");

            entity.ToTable("MovimientosInventario");

            entity.HasIndex(e => e.FechaMovimiento, "MovimientosInventario_Fecha").IsDescending();

            entity.Property(e => e.FechaMovimiento)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TipoMovimiento).HasMaxLength(50);

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.MovimientosInventarios)
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Movimientos_Productos");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.MovimientosInventarios)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Movimientos_Usuarios");
        });

        modelBuilder.Entity<Productos>(entity =>
        {
            entity.HasKey(e => e.IdProducto).HasName("PK__Producto__098892104ACA8C02");

            entity.HasIndex(e => e.NombreProducto, "IX_Productos_Nombre");

            entity.HasIndex(e => e.IdCategoria, "Productos_Categoria");

            entity.HasIndex(e => e.IdMarca, "Productos_Marcas");

            entity.HasIndex(e => e.IdProveedor, "Productos_Proveedores");

            entity.HasIndex(e => e.IdUnidad, "Productos_Unidades");

            entity.Property(e => e.Descripcion).HasMaxLength(250);
            entity.Property(e => e.Estado).HasDefaultValue(true);
            entity.Property(e => e.NombreProducto).HasMaxLength(150);

            entity.HasOne(d => d.IdCategoriaNavigation).WithMany(p => p.Productos)
                .HasForeignKey(d => d.IdCategoria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Productos_Categoria");

            entity.HasOne(d => d.IdMarcaNavigation).WithMany(p => p.Productos)
                .HasForeignKey(d => d.IdMarca)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Productos_Marcas");

            entity.HasOne(d => d.IdProveedorNavigation).WithMany(p => p.Productos)
                .HasForeignKey(d => d.IdProveedor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Productos_Proveedores");

            entity.HasOne(d => d.IdUnidadNavigation).WithMany(p => p.Productos)
                .HasForeignKey(d => d.IdUnidad)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Productos_Unidades");
        });

        modelBuilder.Entity<Proveedores>(entity =>
        {
            entity.HasKey(e => e.IdProveedor).HasName("PK__Proveedo__E8B631AF6673201E");

            entity.HasIndex(e => e.NombreProveedor, "Proveedores_Nombre");

            entity.Property(e => e.Correo).HasMaxLength(150);
            entity.Property(e => e.Direccion).HasMaxLength(250);
            entity.Property(e => e.NombreProveedor).HasMaxLength(150);
            entity.Property(e => e.Telefono).HasMaxLength(12);
        });

        modelBuilder.Entity<Roles>(entity =>
        {
            entity.HasKey(e => e.IdRol).HasName("PK__Roles__2A49584CB80E3668");

            entity.Property(e => e.NombreRol).HasMaxLength(100);
        });

        modelBuilder.Entity<UnidadesMedida>(entity =>
        {
            entity.HasKey(e => e.IdUnidad).HasName("PK__Unidades__437725E612511B5E");

            entity.Property(e => e.Abreviatura).HasMaxLength(10);
            entity.Property(e => e.NombreUnidad).HasMaxLength(100);
        });

        modelBuilder.Entity<Usuarios>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__Usuarios__5B65BF973621D57C");

            entity.HasIndex(e => e.Correo, "UQ__Usuarios__60695A199F5A7DFC").IsUnique();

            entity.Property(e => e.Clave).HasMaxLength(8);
            entity.Property(e => e.Correo).HasMaxLength(150);
            entity.Property(e => e.NombreUsuario).HasMaxLength(100);

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdRol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Usuarios_Roles");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

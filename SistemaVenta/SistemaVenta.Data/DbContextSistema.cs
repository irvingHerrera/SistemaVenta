using Microsoft.EntityFrameworkCore;
using SietemaVenta.Entity.Almacen;
using SietemaVenta.Entity.Usuarios;
using SietemaVenta.Entity.Ventas;
using SistemaVenta.Data.Mapping.Almacen;
using SistemaVenta.Data.Mapping.Usuario;
using SistemaVenta.Data.Mapping.Ventas;

namespace SistemaVenta.Data
{
    public class DbContextSistema : DbContext
    {
        public DbSet<Categoria> Categoria { get; set; }
        public DbSet<Articulo> Articulo { get; set; }
        public DbSet<Rol> Rol { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Persona> Persona { get; set; }
        public DbSet<Ingreso> Ingreso { get; set; }
        public DbSet<DetalleIngreso> DetalleIngreso { get; set; }
        public DbSet<Venta> Venta { get; set; }
        public DbSet<DetalleVenta> DetalleVenta { get; set; }

        public DbContextSistema(DbContextOptions<DbContextSistema> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new CategoriaMap());
            modelBuilder.ApplyConfiguration(new ArticuloMap());
            modelBuilder.ApplyConfiguration(new RolMap());
            modelBuilder.ApplyConfiguration(new UsuarioMap());
            modelBuilder.ApplyConfiguration(new PersonaMap());
            modelBuilder.ApplyConfiguration(new IngresoMap());
            modelBuilder.ApplyConfiguration(new DetalleIngresoMap());
            modelBuilder.ApplyConfiguration(new VentaMap());
            modelBuilder.ApplyConfiguration(new DetalleVentaMap());
        }
    }
}

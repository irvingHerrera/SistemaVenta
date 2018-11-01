using Microsoft.EntityFrameworkCore;
using SietemaVenta.Entity.Almacen;
using SistemaVenta.Data.Mapping.Almacen;

namespace SistemaVenta.Data
{
    public class DbContextSistema : DbContext
    {
        public DbSet<Categoria> Categoria { get; set; }

        public DbContextSistema(DbContextOptions<DbContextSistema> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new CategoriaMap());
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SietemaVenta.Entity.Almacen;

namespace SistemaVenta.Data.Mapping.Almacen
{
    public class CategoriaMap : IEntityTypeConfiguration<Categoria>
    {
        public void Configure(EntityTypeBuilder<Categoria> builder)
        {
            builder.ToTable("categoria")
                .HasKey(c => c.IdCategoria);
            builder.Property(c => c.Nombre)
                .HasMaxLength(50);
            builder.Property(c => c.Descripcion)
                .HasMaxLength(256);
        }
    }
}

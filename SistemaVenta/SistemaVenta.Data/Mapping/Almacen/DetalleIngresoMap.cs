using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SietemaVenta.Entity.Almacen;

namespace SistemaVenta.Data.Mapping.Almacen
{
    public class DetalleIngresoMap : IEntityTypeConfiguration<DetalleIngreso>
    {
        public void Configure(EntityTypeBuilder<DetalleIngreso> builder)
        {
            builder.ToTable("detalle_ingreso")
                .HasKey(d => d.IdDetalleIngreso);
            builder.Property(d => d.IdDetalleIngreso).HasColumnName("iddetalle_ingreso");

        }
    }
}

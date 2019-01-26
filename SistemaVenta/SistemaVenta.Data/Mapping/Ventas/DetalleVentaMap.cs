using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SietemaVenta.Entity.Ventas;
using System;

namespace SistemaVenta.Data.Mapping.Ventas
{
    public class DetalleVentaMap : IEntityTypeConfiguration<DetalleVenta>
    {
        public void Configure(EntityTypeBuilder<DetalleVenta> builder)
        {
            builder.ToTable("detalle_venta")
               .HasKey(d => d.IdDetalleVenta);
            builder.Property(d => d.IdDetalleVenta).HasColumnName("iddetalle_venta");
        }
    }
}

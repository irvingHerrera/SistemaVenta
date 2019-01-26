using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SietemaVenta.Entity.Ventas;
using System;

namespace SistemaVenta.Data.Mapping.Ventas
{
    public class VentaMap : IEntityTypeConfiguration<Venta>
    {
        public void Configure(EntityTypeBuilder<Venta> builder)
        {
            builder.ToTable("venta")
                .HasKey(v => v.IdVenta);
            builder.HasOne(i => i.Persona)
                .WithMany(p => p.Venta)
                .HasForeignKey(v => v.IdCliente);
            builder.Property(i => i.SerieComprobante).HasColumnName("serie_comprobante");
            builder.Property(i => i.TipoComprobante).HasColumnName("tipo_comprobante");
            builder.Property(i => i.FechaHora).HasColumnName("fecha_hora");
            builder.Property(i => i.NumComprobante).HasColumnName("num_comprobante");
        }
    }
}

using Microsoft.EntityFrameworkCore;
using SietemaVenta.Entity.Almacen;

namespace SistemaVenta.Data.Mapping.Almacen
{
    public class IngresoMap : IEntityTypeConfiguration<Ingreso>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Ingreso> builder)
        {
            builder.ToTable("ingreso")
                .HasKey(i => i.IdIngreso);
            builder.HasOne(i => i.Persona)
                .WithMany(p => p.Ingreso)
                .HasForeignKey(i => i.IdProveedor);
            builder.Property(i => i.SerieComprobante).HasColumnName("serie_comprobante");
            builder.Property(i => i.TipoComprobante).HasColumnName("tipo_comprobante");
            builder.Property(i => i.FechaHora).HasColumnName("fecha_hora");
            builder.Property(i => i.NumComprobante).HasColumnName("num_comprobante");
            
        }
    }
}

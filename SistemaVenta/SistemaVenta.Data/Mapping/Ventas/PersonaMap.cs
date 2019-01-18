using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SietemaVenta.Entity.Ventas;
using System;

namespace SistemaVenta.Data.Mapping.Ventas
{
    public class PersonaMap : IEntityTypeConfiguration<Persona>
    {
        public void Configure(EntityTypeBuilder<Persona> builder)
        {
            builder.ToTable("persona")
                .HasKey(u => u.IdPersona);
            builder.Property(u => u.TipoDocumento).HasColumnName("tipo_documento");
            builder.Property(u => u.NumDocumento).HasColumnName("num_documento");
        }
    }
}

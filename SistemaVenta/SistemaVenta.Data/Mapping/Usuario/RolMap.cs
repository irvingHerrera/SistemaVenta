using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SietemaVenta.Entity.Usuarios;
using System;

namespace SistemaVenta.Data.Mapping.Usuario
{
    public class RolMap : IEntityTypeConfiguration<Rol>
    {
        public void Configure(EntityTypeBuilder<Rol> builder)
        {
            builder.ToTable("rol")
               .HasKey(r => r.IdRol);
        }
    }
}

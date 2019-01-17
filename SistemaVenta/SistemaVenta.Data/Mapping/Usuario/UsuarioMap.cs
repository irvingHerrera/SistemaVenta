using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SietemaVenta.Entity.Usuarios;
using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaVenta.Data.Mapping.Usuario
{
    public class UsuarioMap : IEntityTypeConfiguration<SietemaVenta.Entity.Usuarios.Usuario>
    {
        public void Configure(EntityTypeBuilder<SietemaVenta.Entity.Usuarios.Usuario> builder)
        {
            builder.ToTable("usuario")
                .HasKey(u => u.IdUsuario);
            builder.Property(u => u.tipoDocumento).HasColumnName("tipo_documento");
            builder.Property(u => u.NumDocumento).HasColumnName("num_documento");
            builder.Property(u => u.PasswordHash).HasColumnName("password_hash");
            builder.Property(u => u.PasswordSalt).HasColumnName("password_salt");
        }
    }
}

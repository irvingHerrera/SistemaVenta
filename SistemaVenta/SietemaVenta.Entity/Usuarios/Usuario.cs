using SietemaVenta.Entity.Almacen;
using SietemaVenta.Entity.Ventas;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SietemaVenta.Entity.Usuarios
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        [Required]
        public int IdRol { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre no debe tener más de 100 caracteres, ni menos de 3 caracteres. ")]
        public string Nombre { get; set; }
        public string TipoDocumento { get; set; }
        public string NumDocumento { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public byte[] PasswordHash { get; set; }
        [Required]
        public byte[] PasswordSalt { get; set; }
        public bool Condicion { get; set; }

        public Rol Rol { get; set; }

        public ICollection<Ingreso> Ingreso { get; set; }
        public ICollection<Venta> Venta { get; set; }
    }
}

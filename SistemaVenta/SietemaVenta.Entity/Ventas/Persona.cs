using SietemaVenta.Entity.Almacen;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SietemaVenta.Entity.Ventas
{
    public class Persona
    {
        public int IdPersona { get; set; }
        [Required]
        public string TipoPersona { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre no debe tener más de 100 caracteres, ni menos de 3 caracteres. ")]
        public string Nombre { get; set; }
        public string TipoDocumento { get; set; }
        public string NumDocumento { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }

        public ICollection<Ingreso> Ingreso { get; set; }
    }
}

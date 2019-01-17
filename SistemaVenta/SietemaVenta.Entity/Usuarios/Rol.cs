using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SietemaVenta.Entity.Usuarios
{
    public class Rol
    {
        public int IdRol { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "El nombre no debe tener más de 30 caracteres, ni menos de 3 caracteres. ")]
        public string Nombre { get; set; }
        [StringLength(256)]
        public string Descripcion { get; set; }
        public bool Condicion { get; set; }

        public ICollection<Usuario> Usuario { get; set; }
    }
}

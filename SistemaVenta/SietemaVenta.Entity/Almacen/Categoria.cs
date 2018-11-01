using System.ComponentModel.DataAnnotations;

namespace SietemaVenta.Entity.Almacen
{
    public class Categoria
    {
        public int IdCategoria { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El nombre no debe tener más de 50 caracteres, ni menos de 3 caracteres. ")]
        public string Nombre { get; set; }
        [StringLength(256)]
        public string Descripcion { get; set; }
        public bool Condicion { get; set; }
    }
}

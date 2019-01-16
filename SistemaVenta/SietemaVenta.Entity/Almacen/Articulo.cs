using System.ComponentModel.DataAnnotations;

namespace SietemaVenta.Entity.Almacen
{
    public class Articulo
    {
        public int IdArticulo { get; set; }
        [Required]
        public int IdCategoria { get; set; }
        public string Codigo { get; set; }
        [StringLength(50, MinimumLength = 3, 
            ErrorMessage = "El nombre no debe tener más de 50 caracteres, ni menos de 3 caracteres. ")]
        public string Nombre { get; set; }
        [Required]
        public decimal PrecioVenta { get; set; }
        [Required]
        public int Stock { get; set; }
        public int Descripcion { get; set; }
        public bool Condicion { get; set; }

        public Categoria Categoria { get; set; }
    }
}

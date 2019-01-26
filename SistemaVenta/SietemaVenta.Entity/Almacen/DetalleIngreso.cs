using System.ComponentModel.DataAnnotations;

namespace SietemaVenta.Entity.Almacen
{
    public class DetalleIngreso
    {
        public int IdDetalleIngreso { get; set; }
        [Required]
        public int IdIngreso { get; set; }
        [Required]
        public int IdArticulo { get; set; }
        [Required]
        public int Cantidad { get; set; }
        [Required]
        public decimal Precio { get; set; }

        public Ingreso Ingreso { get; set; }
        public Articulo Articulo { get; set; }
    }
}

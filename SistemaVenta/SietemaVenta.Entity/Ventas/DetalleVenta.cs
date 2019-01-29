using SietemaVenta.Entity.Almacen;
using System.ComponentModel.DataAnnotations;

namespace SietemaVenta.Entity.Ventas
{
    public class DetalleVenta
    {
        public int IdDetalleVenta { get; set; }
        [Required]
        public int IdVenta { get; set; }
        [Required]
        public int IdArticulo { get; set; }
        [Required]
        public int Cantidad { get; set; }
        [Required]
        public decimal Precio { get; set; }
        [Required]
        public decimal Descuento { get; set; }

        public Venta Venta { get; set; }
        public Articulo Articulo { get; set; }
    }
}

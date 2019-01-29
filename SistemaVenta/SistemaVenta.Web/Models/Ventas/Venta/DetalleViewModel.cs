using System.ComponentModel.DataAnnotations;

namespace SistemaVenta.Web.Models.Ventas.Venta
{
    public class DetalleViewModel
    {
        [Required]
        public int IdArticulo { get; set; }
        [Required]
        public string Articulo { get; set; }
        [Required]
        public int Cantidad { get; set; }
        [Required]
        public decimal Precio { get; set; }
        [Required]
        public decimal Descuento { get; set; }
    }
}

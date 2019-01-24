using System.ComponentModel.DataAnnotations;

namespace SistemaVenta.Web.Models.Almacen.Ingreso
{
    public class DetalleViewModel
    {
        [Required]
        public int IdArticulo { get; set; }
        [Required]
        public int Cantidad { get; set; }
        [Required]
        public decimal Precio { get; set; }
    }
}

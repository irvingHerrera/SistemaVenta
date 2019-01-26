using SietemaVenta.Entity.Usuarios;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SietemaVenta.Entity.Ventas
{
    public class Venta
    {
        [Required]
        public int IdVenta { get; set; }
        [Required]
        public int IdCliente { get; set; }
        [Required]
        public int IdUsuario { get; set; }
        [Required]
        public string TipoComprobante { get; set; }
        public string SerieComprobante { get; set; }
        [Required]
        public string NumComprobante { get; set; }
        [Required]
        public DateTime FechaHora { get; set; }
        [Required]
        public decimal Impuesto { get; set; }
        [Required]
        public decimal Total { get; set; }
        [Required]
        public string Estado { get; set; }

        public ICollection<DetalleVenta> DetalleVenta { get; set; }
        public Usuario Usuario { get; set; }
        public Persona Persona { get; set; }
    }
}

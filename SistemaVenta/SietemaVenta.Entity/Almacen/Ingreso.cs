using SietemaVenta.Entity.Usuarios;
using SietemaVenta.Entity.Ventas;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SietemaVenta.Entity.Almacen
{
    public class Ingreso
    {
        [Required]
        public int IdIngreso { get; set; }
        [Required]
        public int IdProveedor { get; set; }
        [Required]
        public int IdUsuario { get; set; }
        [Required]
        public string TipoComprobante { get; set; }
        public string SerieComprobante { get; set; }
        [Required]
        public int NumComprobante { get; set; }
        [Required]
        public DateTime FechaHora { get; set; }
        [Required]
        public decimal Impuesto { get; set; }
        [Required]
        public decimal Total { get; set; }
        [Required]
        public string Estado { get; set; }

        public ICollection<DetalleIngreso> DetalleIngreso { get; set; }
        public Usuario Usuario { get; set; }
        public Persona Persona { get; set; }
    }
}

using System;

namespace SistemaVenta.Web.Models.Almacen.Ingreso
{
    public class IngresoViewModel
    {
        public int IdIngreso { get; set; }
        public int IdProveedor { get; set; }
        public string Proveedor { get; set; }
        public int IdUsuario { get; set; }
        public string Usuario { get; set; }
        public string TipoComprobante { get; set; }
        public string SerieComprobante { get; set; }
        public string NumComprobante { get; set; }
        public DateTime FechaHora { get; set; }
        public decimal Impuesto { get; set; }
        public decimal Total { get; set; }
        public string Estado { get; set; }

    }
}

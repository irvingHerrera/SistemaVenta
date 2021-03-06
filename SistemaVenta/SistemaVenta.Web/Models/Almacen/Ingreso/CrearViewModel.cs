﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SistemaVenta.Web.Models.Almacen.Ingreso
{
    public class CrearViewModel
    {
        [Required]
        public int IdProveedor { get; set; }
        [Required]
        public int IdUsuario { get; set; }
        [Required]
        public string TipoComprobante { get; set; }
        public string SerieComprobante { get; set; }
        [Required]
        public string NumComprobante { get; set; }
        [Required]
        public decimal Impuesto { get; set; }
        [Required]
        public decimal Total { get; set; }

        [Required]
        public List<DetalleViewModel> Detalle { get; set; }

    }
}

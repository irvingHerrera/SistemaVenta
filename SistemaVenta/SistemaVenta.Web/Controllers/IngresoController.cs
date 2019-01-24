using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SietemaVenta.Entity.Almacen;
using SistemaVenta.Data;
using SistemaVenta.Web.Models.Almacen.Ingreso;

namespace SistemaVenta.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngresoController : ControllerBase
    {
        private readonly DbContextSistema _context;

        public IngresoController(DbContextSistema context)
        {
            _context = context;
        }

        [Authorize(Roles = "Almacenero,Administrador")]
        // GET: api/Ingreso/Listar
        [HttpGet("[action]")]
        public async Task<IEnumerable<IngresoViewModel>> Listar()
        {
            var ingreso = await _context.Ingreso
                                .Include(i => i.Persona)
                                .Include(i => i.Usuario)
                                .OrderByDescending(i => i.IdIngreso)
                                .Take(100)
                                .ToListAsync();
            return ingreso.Select(i => new IngresoViewModel
            {
                IdIngreso = i.IdIngreso,
                IdProveedor = i.IdProveedor,
                IdUsuario = i.IdUsuario,
                Proveedor = i.Persona.Nombre,
                Usuario = i.Usuario.Nombre,
                TipoComprobante = i.TipoComprobante,
                SerieComprobante = i.SerieComprobante,
                NumComprobante = i.NumComprobante,
                Impuesto = i.Impuesto,
                Total = i.Total,
                FechaHora = i.FechaHora,
                Estado = i.Estado
            });
        }

        // POST: api/Ingreso/Crear
        [Authorize(Roles = "Almacenero,Administrador")]
        [HttpPost("[action]")]
        public async Task<IActionResult> Crear([FromBody] CrearViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var fechaHora = DateTime.Now;

            var ingreso = new Ingreso
            {
                IdProveedor = model.IdProveedor,
                IdUsuario = model.IdUsuario,
                TipoComprobante = model.TipoComprobante,
                SerieComprobante = model.SerieComprobante,
                NumComprobante = model.NumComprobante,
                FechaHora = fechaHora,
                Impuesto = model.Impuesto,
                Total = model.Total,
                Estado = "Aceptado"
            };

            
            try
            {
                _context.Ingreso.Add(ingreso);
                await _context.SaveChangesAsync();

                var idIgreso = ingreso.IdIngreso;

                foreach (var detalle in model.Detalle)
                {
                    var detalleModel = new DetalleIngreso
                    {
                        IdIngreso = idIgreso,
                        IdArticulo = detalle.IdArticulo,
                        Cantidad = detalle.Cantidad,
                        Precio = detalle.Precio
                    };

                    _context.DetalleIngreso.Add(detalleModel);

                }

                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        private bool IngresoExists(int id)
        {
            return _context.Ingreso.Any(e => e.IdIngreso == id);
        }
    }
}
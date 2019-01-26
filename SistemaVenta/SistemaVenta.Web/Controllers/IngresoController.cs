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

        // GET: api/Ingreso/Listar
        [Authorize(Roles = "Almacenero,Administrador")]
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

        // GET: api/Ingreso/Listar
        [Authorize(Roles = "Almacenero,Administrador")]
        [HttpGet("[action]/{texto}")]
        public async Task<IEnumerable<IngresoViewModel>> ListarFiltro([FromRoute] string texto)
        {

            var ingreso = await _context.Ingreso
                                .Include(i => i.Persona)
                                .Include(i => i.Usuario)
                                .Where(i => i.NumComprobante.Contains(texto))
                                .OrderByDescending(i => i.IdIngreso)
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


        // GET: api/Ingreso/ListarDetalles
        [Authorize(Roles = "Almacenero,Administrador")]
        [HttpGet("[action]/{idIngreso}")]
        public async Task<IEnumerable<DetalleViewModel>> ListarDetalles([FromRoute] int idIngreso)
        {
            var detalle = await _context.DetalleIngreso
                                .Include(a => a.Articulo)
                                .Where(d => d.IdIngreso == idIngreso)
                                .ToListAsync();

            return detalle.Select(i => new DetalleViewModel
            {
                IdArticulo = i.IdArticulo,
                Articulo = i.Articulo.Nombre,
                Cantidad = i.Cantidad,
                Precio = i.Precio

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

        [Authorize(Roles = "Almacenero,Administrador")]
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> Anular([FromRoute]  int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var ingreso = await _context.Ingreso.FirstOrDefaultAsync(c => c.IdIngreso == id);

            if (ingreso == null)
            {
                return NotFound();
            }

            ingreso.Estado = "Anulado";

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }

            return Ok();
        }

        private bool IngresoExists(int id)
        {
            return _context.Ingreso.Any(e => e.IdIngreso == id);
        }
    }
}
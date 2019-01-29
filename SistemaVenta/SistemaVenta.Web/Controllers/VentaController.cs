using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SietemaVenta.Entity.Ventas;
using SistemaVenta.Data;
using SistemaVenta.Web.Models.Ventas.Venta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaVenta.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentaController : ControllerBase
    {
        private readonly DbContextSistema _context;

        public VentaController(DbContextSistema context)
        {
            _context = context;
        }

        // GET: api/Venta/Listar
        [Authorize(Roles = "Vendedor,Administrador")]
        [HttpGet("[action]")]
        public async Task<IEnumerable<VentaViewModel>> Listar()
        {
            var venta = await _context.Venta
                                .Include(v => v.Usuario)
                                .Include(v => v.Persona)
                                .OrderByDescending(v => v.IdVenta)
                                .Take(100)
                                .ToListAsync();

            return venta.Select(v => new VentaViewModel
            {
                IdVenta = v.IdVenta,
                IdCliente = v.IdCliente,
                IdUsuario = v.IdUsuario,
                Cliente = v.Persona.Nombre,
                Usuario = v.Usuario.Nombre,
                TipoComprobante = v.TipoComprobante,
                SerieComprobante = v.SerieComprobante,
                NumComprobante = v.NumComprobante,
                Impuesto = v.Impuesto,
                Total = v.Total,
                FechaHora = v.FechaHora,
                Estado = v.Estado
            });
        }

        // GET: api/Venta/ListarFiltro/texto
        [Authorize(Roles = "Vendedor,Administrador")]
        [HttpGet("[action]/{texto}")]
        public async Task<IEnumerable<VentaViewModel>> ListarFiltro([FromRoute] string texto)
        {

            var venta = await _context.Venta
                                .Include(v => v.Usuario)
                                .Include(v => v.Persona)
                                .Where(i => i.NumComprobante.Contains(texto))
                                .OrderByDescending(v => v.IdVenta)
                                .ToListAsync();

            return venta.Select(v => new VentaViewModel
            {
                IdVenta = v.IdVenta,
                IdCliente = v.IdCliente,
                IdUsuario = v.IdUsuario,
                Cliente = v.Persona.Nombre,
                Usuario = v.Usuario.Nombre,
                TipoComprobante = v.TipoComprobante,
                SerieComprobante = v.SerieComprobante,
                NumComprobante = v.NumComprobante,
                Impuesto = v.Impuesto,
                Total = v.Total,
                FechaHora = v.FechaHora,
                Estado = v.Estado
            });
        }


        // GET: api/Venta/ListarDetalles
        [Authorize(Roles = "Vendedor,Administrador")]
        [HttpGet("[action]/{idVenta}")]
        public async Task<IEnumerable<DetalleViewModel>> ListarDetalles([FromRoute] int idVenta)
        {
            var detalle = await _context.DetalleVenta
                                .Include(v => v.Venta)
                                .Where(d => d.IdVenta == idVenta)
                                .ToListAsync();

            return detalle.Select(v => new DetalleViewModel
            {
                IdArticulo = v.IdArticulo,
                Articulo = v.Articulo.Nombre,
                Cantidad = v.Cantidad,
                Precio = v.Precio,
                Descuento = v.Descuento

            });
        }

        // POST: api/Venta/Crear
        [Authorize(Roles = "Vendedor,Administrador")]
        [HttpPost("[action]")]
        public async Task<IActionResult> Crear([FromBody] CrearViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var fechaHora = DateTime.Now;

            var venta = new Venta
            {
                IdCliente = model.IdCliente,
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
                _context.Venta.Add(venta);
                await _context.SaveChangesAsync();

                var idVenta = venta.IdVenta;

                foreach (var detalle in model.Detalle)
                {
                    var detalleModel = new DetalleVenta
                    {
                        IdVenta = idVenta,
                        IdArticulo = detalle.IdArticulo,
                        Cantidad = detalle.Cantidad,
                        Precio = detalle.Precio,
                        Descuento = detalle.Descuento
                    };

                    _context.DetalleVenta.Add(detalleModel);

                }

                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        [Authorize(Roles = "Vendedor,Administrador")]
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> Anular([FromRoute]  int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var venta = await _context.Venta.FirstOrDefaultAsync(c => c.IdVenta == id);

            if (venta == null)
            {
                return NotFound();
            }

            venta.Estado = "Anulado";

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

        private bool VentaExists(int id)
        {
            return _context.Venta.Any(e => e.IdVenta == id);
        }
    }
}
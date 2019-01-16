using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SietemaVenta.Entity.Almacen;
using SistemaVenta.Data;
using SistemaVenta.Web.Models.Almacen.Articulo;

namespace SistemaVenta.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticuloController : ControllerBase
    {
        private readonly DbContextSistema _context;

        public ArticuloController(DbContextSistema context)
        {
            _context = context;
        }

        // GET: api/Articulo/Listar
        [HttpGet("[action]")]
        public async Task<IEnumerable<ArticuloViewModel>> Listar()
        {
            var articulo = await _context.Articulo.Include(a => a.Categoria).ToListAsync();
            return articulo.Select(a => new ArticuloViewModel
            {
                IdArticulo = a.IdArticulo,
                IdCategoria = a.IdCategoria,
                Nombre = a.Nombre,
                Condicion = a.Condicion,
                Descripcion = a.Descripcion,
                Categoria = a.Categoria.Nombre,
                Codigo = a.Codigo,
                PrecioVenta = a.PrecioVenta,
                Stock = a.Stock
            });
        }

        // GET: api/Articulo/Mostrar/5
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> Mostrar([FromRoute] int id)
        {
            var articulo = await _context.Articulo.Include(a => a.Categoria).FirstAsync(a => a.IdArticulo == id);

            if (articulo == null)
            {
                return NotFound();
            }

            return Ok(new ArticuloViewModel
            {
                IdArticulo = articulo.IdArticulo,
                IdCategoria = articulo.IdCategoria,
                Nombre = articulo.Nombre,
                Descripcion = articulo.Descripcion,
                Condicion = articulo.Condicion,
                Categoria = articulo.Categoria.Nombre,
                Codigo = articulo.Codigo,
                PrecioVenta = articulo.PrecioVenta,
                Stock = articulo.Stock
            });
        }

        // PUT: api/Articulo/Actualizar
        [HttpPut("[action]")]
        public async Task<IActionResult> Actualizar([FromBody] ArticuloViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model.IdArticulo <= 0)
            {
                return BadRequest();
            }

            var articulo = await _context.Articulo
                            .FirstOrDefaultAsync(a => a.IdArticulo == model.IdArticulo);

            if (articulo == null)
            {
                return NotFound();
            }

            articulo.IdCategoria = model.IdCategoria;
            articulo.Nombre = model.Nombre;
            articulo.Descripcion = model.Descripcion;
            articulo.PrecioVenta = model.PrecioVenta;
            articulo.Stock = model.Stock;
            articulo.Codigo = model.Codigo;

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

        // POST: api/Articulo/Crear
        [HttpPost("[action]")]
        public async Task<IActionResult> Crear([FromBody] ArticuloViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var articulo = new Articulo
            {
                IdCategoria = model.IdCategoria,
                Nombre = model.Nombre,
                Descripcion = model.Descripcion,
                Condicion = true,
                PrecioVenta = model.PrecioVenta,
                Codigo = model.Codigo,
                Stock = model.Stock
                
            };

            _context.Articulo.Add(articulo);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        // DELETE: api/Articulo/5
        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> Eliminar([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var articulo = await _context.Articulo.FindAsync(id);
            if (articulo == null)
            {
                return NotFound();
            }

            _context.Articulo.Remove(articulo);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

            return Ok(articulo);
        }

        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> Desactivar([FromRoute]  int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var articulo = await _context.Articulo.FirstOrDefaultAsync(a => a.IdArticulo == id);

            if (articulo == null)
            {
                return NotFound();
            }

            articulo.Condicion = false;

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

        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> Activar([FromRoute]  int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var articulo = await _context.Articulo.FirstOrDefaultAsync(a => a.IdArticulo == id);

            if (articulo == null)
            {
                return NotFound();
            }

            articulo.Condicion = true;

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

        private bool ArticuloExists(int id)
        {
            return _context.Articulo.Any(e => e.IdArticulo == id);
        }
    }
}
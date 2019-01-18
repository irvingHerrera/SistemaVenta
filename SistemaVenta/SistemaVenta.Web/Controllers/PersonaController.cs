using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SietemaVenta.Entity.Ventas;
using SistemaVenta.Data;
using SistemaVenta.Web.Models.Ventas.Persona;

namespace SistemaVenta.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonaController : ControllerBase
    {
        private readonly DbContextSistema _context;

        public PersonaController(DbContextSistema context)
        {
            _context = context;
        }

        // GET: api/Persona/ListarCliente
        [HttpGet("[action]")]
        public async Task<IEnumerable<PersonaViewModel>> ListarCliente()
        {
            var persona = await _context.Persona.Where(p => p.TipoPersona.Equals("cliente")).ToListAsync();
            return persona.Select(p => new PersonaViewModel
            {
                IdPersona = p.IdPersona,
                TipoPersona = p.TipoPersona,
                Nombre = p.Nombre,
                TipoDocumento = p.TipoDocumento,
                NumDocumento = p.NumDocumento,
                Direccion = p.Direccion,
                Telefono = p.Telefono,
                Email = p.Email,
               
            });
        }

        // GET: api/Persona/ListarProvedor
        [HttpGet("[action]")]
        public async Task<IEnumerable<PersonaViewModel>> ListarProvedor()
        {
            var persona = await _context.Persona.Where(p => p.TipoPersona.Equals("proveedor")).ToListAsync();
            return persona.Select(p => new PersonaViewModel
            {
                IdPersona = p.IdPersona,
                TipoPersona = p.TipoPersona,
                Nombre = p.Nombre,
                TipoDocumento = p.TipoDocumento,
                NumDocumento = p.NumDocumento,
                Direccion = p.Direccion,
                Telefono = p.Telefono,
                Email = p.Email,

            });
        }

        // POST: api/Persona/Crear
        [HttpPost("[action]")]
        public async Task<IActionResult> Crear([FromBody] PersonaViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var email = model.Email.ToLower();

            var persona = new Persona
            {
                TipoPersona = model.TipoPersona,
                Nombre = model.Nombre,
                TipoDocumento = model.TipoDocumento,
                NumDocumento = model.NumDocumento,
                Direccion = model.Direccion,
                Telefono = model.Telefono,
                Email = model.Email.ToLower(),

            };

            _context.Persona.Add(persona);
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

        // PUT: api/Persona/Actualizar
        [HttpPut("[action]")]
        public async Task<IActionResult> Actualizar([FromBody] PersonaViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model.IdPersona <= 0)
            {
                return BadRequest();
            }

            var persona = await _context.Persona
                            .FirstOrDefaultAsync(p => p.IdPersona == model.IdPersona);

            if (persona == null)
            {
                return NotFound();
            }

            persona.TipoPersona = model.TipoPersona;
            persona.Nombre = model.Nombre;
            persona.TipoDocumento = model.TipoDocumento;
            persona.NumDocumento = model.NumDocumento;
            persona.Telefono = model.Telefono;
            persona.Direccion = model.Direccion;
            persona.Email = model.Email.ToLower();

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


        private bool PersonaExists(int id)
        {
            return _context.Persona.Any(e => e.IdPersona == id);
        }
    }
}
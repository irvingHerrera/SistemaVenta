using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaVenta.Data;
using SistemaVenta.Web.Models.Usuarios.Rol;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaVenta.Web.Controllers
{
    [Authorize(Roles = "Administrador")]
    [Route("api/[controller]")]
    [ApiController]
    public class RolController : ControllerBase
    {
        private readonly DbContextSistema _context;

        public RolController(DbContextSistema context)
        {
            _context = context;
        }

        // GET: api/Rol/Listar
        [HttpGet("[action]")]
        public async Task<IEnumerable<RolViewModel>> Listar()
        {
            var rol = await _context.Rol.ToListAsync();
            return rol.Select(r => new RolViewModel
            {
                IdRol = r.IdRol,
                Nombre = r.Nombre,
                Condicion = r.Condicion,
                Descripcion = r.Descripcion
            });
        }

        // GET: api/Rol/Select
        [HttpGet("[action]")]
        public async Task<IEnumerable<RolViewModel>> Select()
        {
            var rol = await _context.Rol.Where(r => r.Condicion == true).ToListAsync();
            return rol.Select(r => new RolViewModel
            {
                IdRol = r.IdRol,
                Nombre = r.Nombre,
            });
        }

        private bool RolExists(int id)
        {
            return _context.Rol.Any(e => e.IdRol == id);
        }
    }
}
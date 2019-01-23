using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SietemaVenta.Entity.Usuarios;
using SistemaVenta.Data;
using SistemaVenta.Web.Models.Usuarios.Usuario;

namespace SistemaVenta.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly DbContextSistema _context;
        private readonly IConfiguration _config;

        public UsuarioController(DbContextSistema context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        // GET: api/Usuario/Listar
        [Authorize(Roles = "Administrador")]
        [HttpGet("[action]")]
        public async Task<IEnumerable<UsuarioViewModel>> Listar()
        {
            var usuario = await _context.Usuario.Include(u => u.Rol).ToListAsync();
            return usuario.Select(u => new UsuarioViewModel
            {
                IdUsuario = u.IdUsuario,
                IdRol = u.IdRol,
                Rol = u.Rol.Nombre,
                Nombre = u.Nombre,
                tipoDocumento = u.TipoDocumento,
                NumDocumento = u.NumDocumento,
                Direccion = u.Direccion,
                Telefono = u.Telefono,
                Email = u.Email,
                PasswordHash = u.PasswordHash,
                Condicion = u.Condicion
            });
        }

        // POST: api/Usuario/Crear
        [Authorize(Roles = "Administrador")]
        [HttpPost("[action]")]
        public async Task<IActionResult> Crear([FromBody] UsuarioViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var email = model.Email.ToLower();

            if (await _context.Usuario.AnyAsync(u => u.Email.Equals(email)))
            {
                return BadRequest("El email ya existe");
            }

            CrearPasswordHash(model.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var usuario = new Usuario
            {
                IdRol = model.IdRol,
                Nombre = model.Nombre,
                TipoDocumento = model.tipoDocumento,
                NumDocumento = model.NumDocumento,
                Direccion = model.Direccion,
                Telefono = model.Telefono,
                Email = model.Email.ToLower(),
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Condicion = true, 

            };

            _context.Usuario.Add(usuario);
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

        // PUT: api/Articulo/Actualizar
        [Authorize(Roles = "Administrador")]
        [HttpPut("[action]")]
        public async Task<IActionResult> Actualizar([FromBody] UsuarioViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model.IdUsuario <= 0)
            {
                return BadRequest();
            }

            var usuario = await _context.Usuario
                            .FirstOrDefaultAsync(u => u.IdUsuario == model.IdUsuario);

            if (usuario == null)
            {
                return NotFound();
            }

            usuario.IdRol = model.IdRol;
            usuario.Nombre = model.Nombre;
            usuario.TipoDocumento = model.tipoDocumento;
            usuario.NumDocumento = model.NumDocumento;
            usuario.Telefono = model.Telefono;
            usuario.Direccion = model.Direccion;
            usuario.Email = model.Email.ToLower();

            if (model.ActPassword)
            {
                CrearPasswordHash(model.Password, out byte[] passwordHash, out byte[] passwordSalt);

                usuario.PasswordHash = passwordHash;
                usuario.PasswordSalt = passwordSalt;
            }
            

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

        [Authorize(Roles = "Administrador")]
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> Desactivar([FromRoute]  int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var usuario = await _context.Usuario.FirstOrDefaultAsync(a => a.IdUsuario == id);

            if (usuario == null)
            {
                return NotFound();
            }

            usuario.Condicion = false;

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

        [Authorize(Roles = "Administrador")]
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> Activar([FromRoute]  int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var usuario = await _context.Usuario.FirstOrDefaultAsync(u => u.IdUsuario == id);

            if (usuario == null)
            {
                return NotFound();
            }

            usuario.Condicion = true;

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

        // POST: api/Usuario/Login
        [HttpPost("[action]")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            var email = model.Email.ToLower();

            var usuario = await _context.Usuario
                                .Where(u => u.Condicion == true)
                                .Include(u => u.Rol)
                                .FirstOrDefaultAsync(u => u.Email.Equals(email));

            if (usuario == null)
            {
                return NotFound();
            }

            if (!VerificarPasswordHash(model.Password, usuario.PasswordHash, usuario.PasswordSalt))
            {
                return NotFound();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, usuario.Rol.Nombre),
                new Claim("idusuario", usuario.IdUsuario.ToString()),
                new Claim("rol", usuario.Rol.Nombre),
                new Claim("nombre", usuario.Nombre),
            };

            return Ok(new { token = GenerarToken(claims) });
        }

        private string GenerarToken(List<Claim> claims)
        {
            try
            {
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                        issuer: _config["Jwt:Issuer"],
                        audience: _config["Jwt:Issuer"],
                        expires: DateTime.Now.AddMinutes(30),
                        signingCredentials: creds,
                        claims: claims
                    );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            
        }

        private bool VerificarPasswordHash(string password, byte[] passwordHashAlmanenado, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return new ReadOnlySpan<byte>(passwordHashAlmanenado)
                       .SequenceEqual(new ReadOnlySpan<byte>(passwordHash));
            }
        }

        private void CrearPasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuario.Any(e => e.IdUsuario == id);
        }
    }
}
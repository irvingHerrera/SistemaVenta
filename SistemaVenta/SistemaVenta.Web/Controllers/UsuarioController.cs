﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public UsuarioController(DbContextSistema context)
        {
            _context = context;
        }

        // GET: api/Usuario/Listar
        [HttpGet("[action]")]
        public async Task<IEnumerable<UsuarioViewModel>> Listar()
        {
            var usuario = await _context.Usuario.Include(u => u.Rol).ToListAsync();
            return usuario.Select(u => new UsuarioViewModel
            {
                IdUsuario = u.IdUsuario,
                IdRol = u.IdUsuario,
                Rol = u.Rol.Nombre,
                Nombre = u.Nombre,
                tipoDocumento = u.tipoDocumento,
                NumDocumento = u.NumDocumento,
                Direccion = u.Direccion,
                Telefono = u.Telefono,
                Email = u.Email,
                PasswordHash = u.PasswordHash,
                Condicion = u.Condicion
            });
        }

        // POST: api/Usuario/Crear
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
                tipoDocumento = model.tipoDocumento,
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
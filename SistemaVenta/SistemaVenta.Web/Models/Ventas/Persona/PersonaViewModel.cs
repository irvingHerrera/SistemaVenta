using System.ComponentModel.DataAnnotations;

namespace SistemaVenta.Web.Models.Ventas.Persona
{
    public class PersonaViewModel
    {
        [Required]
        public int IdPersona { get; set; }
        [Required]
        public string TipoPersona { get; set; }
        [StringLength(100, MinimumLength = 3,
            ErrorMessage = "El nombre no debe tener más de 100 caracteres, ni menos de 3 caracteres. ")]
        public string Nombre { get; set; }
        public string TipoDocumento { get; set; }
        public string NumDocumento { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
namespace ApiTienda.DTOs
{
    public class EditarAdminDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
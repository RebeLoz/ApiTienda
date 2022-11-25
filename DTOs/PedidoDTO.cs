using System.ComponentModel.DataAnnotations;
namespace ApiTienda.DTOs
{
    public class PedidoDTO
    {
        [Required(ErrorMessage = "El campo {0} es requerido")] //
        [StringLength(maximumLength: 150, ErrorMessage = "El campo {0} solo puede tener hasta 150 caracteres")]
        public string Name { get; set; }
    }
}
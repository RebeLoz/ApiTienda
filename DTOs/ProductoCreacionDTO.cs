using System.ComponentModel.DataAnnotations;
namespace ApiTienda.DTOs
{
    public class ProductoCreacionDTO
    {
        [Required]
        [StringLength(maximumLength: 250, ErrorMessage = "El campo {0} solo puede tener hasta 250 caracteres")]
        public string Name { get; set; }
        public DateTime FechaCreacion { get; set; }
        public List<int> PedidosIds { get; set; }
    }
}
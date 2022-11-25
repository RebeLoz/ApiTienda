using System.ComponentModel.DataAnnotations;
using ApiTienda.Entidades;
namespace ApiTienda.Entidades
{
    public class Producto
    {
        public int Id { get; set; }
        [Required]
        [StringLength(maximumLength: 20, ErrorMessage = "El campo {0} solo puede tener hasta 20 caracteres.")]
        public string Name { get; set; }
        public List<Clientes> Clientes { get; set; }
        public List<PedidoProducto> PedidoProducto { get; set; }
    }
}
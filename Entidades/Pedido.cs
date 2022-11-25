using System.ComponentModel.DataAnnotations;
namespace ApiTienda.Entidades
{
    public class Pedido
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [StringLength(maximumLength: 20, ErrorMessage = "El campo {0} solo puede tener hasta 20 caracteres.")]
        public string Name { get; set; }
        public string Cliente { get; set; }
        public string Direccion { get; set; }
        public List<PedidoProducto> PedidoProducto { get; set; }
    }
}
namespace ApiTienda.DTOs
{
    public class PedidoDTOConProductos : GetPedidoDTO
    {
        public List<ProductoDTO> Productos { get; set; }
    }
}
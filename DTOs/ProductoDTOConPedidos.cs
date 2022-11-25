namespace ApiTienda.DTOs
{
    public class ProductoDTOCoPedidos : ProductoDTO
    {
        public List<GetPedidoDTO> Pedidos { get; set; }
    }
}
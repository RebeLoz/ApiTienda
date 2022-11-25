namespace ApiTienda.Entidades
{
    public class PedidoProducto
    {
        public int PedidoId { get; set; }
        public int ProductoId { get; set; }
        public int Orden { get; set; }
        public Pedido Pedido { get; set; }
        public Producto Producto { get; set; }
    }
}
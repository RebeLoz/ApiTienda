using ApiTienda.DTOs;
namespace ApiTienda.DTOs
{
    public class ProductoDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime FechaCreacion { get; set; }
        public List<ClienteDTO> Tipos { get; set; }
    }
}
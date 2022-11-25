using Microsoft.AspNetCore.Identity;
namespace ApiTienda.Entidades
{
    public class Clientes
    {
        public int Id { get; set; }
        public string Informacion { get; set; }
        public int ProductoId { get; set; }
        public Producto Producto { get; set; }
        public string UsuarioId { get; set; }
        public IdentityUser Usuario { get; set; }
    }
}
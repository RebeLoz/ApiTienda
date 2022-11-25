using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiTienda.DTOs;
using ApiTienda.Entidades;
using ApiTienda;
namespace ApiTienda.Controllers
{
    [ApiController]
    [Route("productos")]
    public class ProductosController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;
        public ProductosController(ApplicationDbContext context, IMapper mapper)
        {
            this.dbContext = context;
            this.mapper = mapper;
        }

        [HttpGet]
        [HttpGet("/listadoProducto")]
        public async Task<ActionResult<List<Producto>>> GetAll()
        {
            return await dbContext.Productos.ToListAsync();
        }

        [HttpGet("{id:int}", Name = "obtenerProducto")]
        //public async Task<ActionResult<ProductoDTOConPedidos>> GetById(int id)
      //  {
       //     var producto = await dbContext.Productos
       //         .Include(productoDB => productoDB.PedidoProducto)
       //         .ThenInclude(pedidoProductoDB => pedidoProductoDB.Pedido)
        //        .Include(tipoDB => tipoDB.Tipos)
        //        .FirstOrDefaultAsync(x => x.Id == id);
//
         //   if (producto == null)
         //   {
         //       return NotFound();
        //    }
        //    producto.PedidoProducto = producto.PedidoProducto.OrderBy(x => x.Orden).ToList();
       //     return mapper.Map<ProductoDTOConPedidos>(producto);
       // }

        [HttpPost]
        public async Task<ActionResult> Post(ProductoCreacionDTO productoCreacionDTO)
        {

            if (productoCreacionDTO.PedidosIds == null)
            {
                return BadRequest("No se puede pedir un producto sin hacer pedidos.");
            }

            var spedidosIds = await dbContext.Pedidos
                .Where(pedidoBD => productoCreacionDTO.PedidosIds.Contains(pedidoBD.Id)).Select(x => x.Id).ToListAsync();

           // if (productoCreacionDTO.PedidosIds.Count != pedidoIds.Count)
            {
                //return BadRequest("No existe uno de los pedidos enviados");
            }
            var producto = mapper.Map<Producto>(productoCreacionDTO);
            dbContext.Add(producto);
            await dbContext.SaveChangesAsync();
            var productoDTO = mapper.Map<ProductoDTO>(producto);
            return CreatedAtRoute("obtenerProducto", new { id = producto.Id }, productoDTO);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, ProductoCreacionDTO productoCreacionDTO)
        {
            var productoDB = await dbContext.Productos
                .Include(x => x.PedidoProducto)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (productoDB == null)
            {
                return NotFound();
            }
            productoDB = mapper.Map(productoCreacionDTO, productoDB);
            await dbContext.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var exist = await dbContext.Productos.AnyAsync(x => x.Id == id);
            if (!exist)
            {
                return NotFound("El Recurso no fue encontrado.");
            }
            dbContext.Remove(new Producto { Id = id });
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        private void OrdenarPorPedidos(Producto producto)
        {
            if (producto.PedidoProducto != null)
            {
                for (int i = 0; i < producto.PedidoProducto.Count; i++)
                {
                    producto.PedidoProducto[i].Orden = i;
                }
            }
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult> Patch(int id, JsonPatchDocument<ProductoPatchDTO> patchDocument)
        {
            if (patchDocument == null) { return BadRequest(); }
            var productoDB = await dbContext.Productos.FirstOrDefaultAsync(x => x.Id == id);
            if (productoDB == null) { return NotFound(); }
            var productoDTO = mapper.Map<ProductoPatchDTO>(productoDB);
            patchDocument.ApplyTo(productoDTO);
            var isValid = TryValidateModel(productoDTO);
            if (!isValid)
            {
                return BadRequest(ModelState);
            }
            mapper.Map(productoDTO, productoDB);
            await dbContext.SaveChangesAsync();
            return NoContent();
        }
    }
}
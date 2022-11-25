using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiTienda.DTOs;
using ApiTienda.Entidades;

namespace ApiTienda.Controllers
{
    [ApiController]
    [Route("pedidos")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
    public class PedidosController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;
        public PedidosController(ApplicationDbContext context, IMapper mapper, IConfiguration configuration)
        {
            this.dbContext = context;
            this.mapper = mapper;
            this.configuration = configuration;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<GetPedidoDTO>>> Get()
        {
            var pedidos = await dbContext.Pedidos.ToListAsync();
            return mapper.Map<List<GetPedidoDTO>>(pedidos);
        }

        [HttpGet("{id:int}", Name = "obtenerpedido")]
        public async Task<ActionResult<PedidoDTOConProductos>> Get(int id)
        {
            var pedido = await dbContext.Pedidos
                .Include(pedidoDB => pedidoDB.PedidoProducto)
                .ThenInclude(pedidoProductoDB => pedidoProductoDB.Producto)
                .FirstOrDefaultAsync(pedidoBD => pedidoBD.Id == id);
            if (pedido == null)
            {
                return NotFound();
            }
            return mapper.Map<PedidoDTOConProductos>(pedido);
        }

        [HttpGet("{nombre}")]
        public async Task<ActionResult<List<GetPedidoDTO>>> Get([FromRoute] string nombre)
        {
            var pedidos = await dbContext.Pedidos.Where(pedidoBD => pedidoBD.Name.Contains(nombre)).ToListAsync();
            return mapper.Map<List<GetPedidoDTO>>(pedidos);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] PedidoDTO pedidoDto)
        {
            var existePedidoMismoNombre = await dbContext.Pedidos.AnyAsync(x => x.Name == pedidoDto.Name);
            if (existePedidoMismoNombre)
            {
                return BadRequest($"Ya existe un cliente con el mismo nombre de {pedidoDto.Name}");
            }
            var pedido = mapper.Map<Pedido>(pedidoDto);
            dbContext.Add(pedido);
            await dbContext.SaveChangesAsync();
            var pedidoDTO = mapper.Map<GetPedidoDTO>(pedido);
            return CreatedAtRoute("obtenerpedido", new { id = pedido.Id }, pedidoDTO);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(PedidoDTO pedidoCreacionDTO, int id)
        {
            var exist = await dbContext.Pedidos.AnyAsync(x => x.Id == id);
            if (!exist)
            {
                return NotFound();
            }
            var pedido = mapper.Map<Pedido>(pedidoCreacionDTO);
            pedido.Id = id;
            dbContext.Update(pedido);
            await dbContext.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var exist = await dbContext.Pedidos.AnyAsync(x => x.Id == id);
            if (!exist)
            {
                return NotFound("Error");
            }
            dbContext.Remove(new Pedido()
            {
                Id = id
            });
            await dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
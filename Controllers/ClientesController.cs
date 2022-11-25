using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.EntityFrameworkCore;
using ApiTienda.DTOs;
using ApiTienda.Entidades;
using ApiTienda.Migrations;

namespace ApiTienda.Controllers
{
    [ApiController]
    [Route("productos/{productoId:int}/clientes")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ClientesController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;
        private readonly UserManager<IdentityUser> userManager;

        public ClientesController(ApplicationDbContext dbContext, IMapper mapper,
            UserManager<IdentityUser> userManager)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<List<ClienteDTO>>> Get(int productoId)
        {
            var existeProducto = await dbContext.Productos.AnyAsync(productoDB => productoDB.Id == productoId);
            if (!existeProducto)
            {
                return NotFound();
            }
            var clientes = await dbContext.Clientes.Where(clienteDB => clienteDB.ProductoId == productoId).ToListAsync();
            return mapper.Map<List<ClienteDTO>>(clientes);
        }

        [HttpGet("{id:int}", Name = "obtenerCliente")]
        public async Task<ActionResult<ClienteDTO>> GetById(int id)
        {
            var cliente = await dbContext.Clientes.FirstOrDefaultAsync(clienteDB => clienteDB.Id == id);
            if (cliente == null)
            {
                return NotFound();
            }
            return mapper.Map<ClienteDTO>(cliente);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Post(int productoId, ClienteCreacionDTO clienteCreacionDTO)
        {
            var emailClaim = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
            var email = emailClaim.Value;
            var usuario = await userManager.FindByEmailAsync(email);
            var usuarioId = usuario.Id;
            var existeProducto = await dbContext.Productos.AnyAsync(productoDB => productoDB.Id == productoId);
            if (!existeProducto)
            {
                return NotFound();
            }
            var cliente = mapper.Map<Clientes>(clienteCreacionDTO);
            cliente.ProductoId = productoId;
            cliente.UsuarioId = usuarioId;
            dbContext.Add(cliente);
            await dbContext.SaveChangesAsync();
            var clienteDTO = mapper.Map<ClienteDTO>(cliente);
            return CreatedAtRoute("obtenerCliente", new { id = cliente.Id, productoId = productoId }, clienteDTO);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int productoId, int id, ClienteCreacionDTO tipoCreacionDTO)
        {
            var existeProducto = await dbContext.Productos.AnyAsync(productoDB => productoDB.Id == productoId);
            if (!existeProducto)
            {
                return NotFound();
            }
            var existeCliente = await dbContext.Clientes.AnyAsync(clienteDB => clienteDB.Id == id);
            if (!existeCliente)
            {
                return NotFound();
            }
            var cliente = mapper.Map<Clientes>(clienteCreacionDTO);
            cliente.Id = id;
            cliente.ProductoId = productoId;
            dbContext.Update(cliente);
            await dbContext.SaveChangesAsync();
            return NoContent();
        }
    }
}
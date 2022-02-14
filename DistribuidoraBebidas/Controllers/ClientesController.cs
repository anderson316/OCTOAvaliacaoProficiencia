using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DistribuidoraBebidas.Controllers
{
    [ApiController]
    public class ClientesController : Controller
    {
        private readonly ILogger<ClientesController> _logger;

        public ClientesController(ILogger<ClientesController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("api/[controller]")]
        public ActionResult<List<Cliente>> Get(string? nome = null)
        {
            return ClientesService.GetAll(nome);
        }

        [HttpGet]
        [Route("api/[controller]/pages")]
        public ActionResult<List<Cliente>> GetPaginas([FromHeader] int pagina,string? nome = null)
        {
            return Ok(ClientesService.GetPaginas(pagina, nome));
        }

        [HttpGet]
        [Route("api/[controller]/{id}")]
        public ActionResult<Cliente> GetId(int id)
        {
            if (ClientesService.GetPorID(id) != null)
            {
                return ClientesService.GetPorID(id);
            }
            throw new ArgumentNullException("ID informado não existe");
        }

        [HttpPost]
        [Route("api/[controller]")]
        public IActionResult Create(Cliente cliente)
        {
            try
            {
                ClientesService.CriarCliente(cliente);
                return CreatedAtAction(nameof(Create), new { id = cliente.ClienteID }, cliente);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut]
        [Route("api/[controller]/{id}")]
        public ActionResult Edit(int id, Cliente cliente)
        {
            try
            {
                ClientesService.EditarCliente(id, cliente);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        [Route("api/[controller]/{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                ClientesService.DeletarCliente(id);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}

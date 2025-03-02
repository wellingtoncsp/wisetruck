using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WiseTruck.API.Models;
using WiseTruck.API.Services;

namespace WiseTruck.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [SwaggerTag("Gerenciamento de registros de pedágios")]
    public class PedagioController : ControllerBase
    {
        private readonly IPedagioService _pedagioService;

        public PedagioController(IPedagioService pedagioService)
        {
            _pedagioService = pedagioService;
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Listar todos os pedágios",
            Description = "Retorna uma lista com todos os registros de pedágios"
        )]
        [SwaggerResponse(200, "Lista de pedágios retornada com sucesso")]
        public async Task<ActionResult<IEnumerable<Pedagio>>> GetPedagios()
        {
            try
            {
                var pedagios = await _pedagioService.GetAllPedagiosAsync();
                return Ok(pedagios);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Obter pedágio por ID",
            Description = "Retorna os detalhes de um registro de pedágio específico"
        )]
        [SwaggerResponse(200, "Pedágio encontrado com sucesso")]
        [SwaggerResponse(404, "Pedágio não encontrado")]
        public async Task<ActionResult<Pedagio>> GetPedagio(int id)
        {
            try
            {
                var pedagio = await _pedagioService.GetPedagioByIdAsync(id);
                return Ok(pedagio);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Pedágio com ID {id} não encontrado.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        [HttpGet("viagem/{viagemId}")]
        [SwaggerOperation(
            Summary = "Listar pedágios por viagem",
            Description = "Retorna todos os pedágios registrados em uma viagem específica"
        )]
        [SwaggerResponse(200, "Lista de pedágios da viagem retornada com sucesso")]
        [SwaggerResponse(404, "Viagem não encontrada")]
        public async Task<ActionResult<IEnumerable<Pedagio>>> GetPedagiosPorViagem(int viagemId)
        {
            try
            {
                var pedagios = await _pedagioService.GetPedagiosPorViagemAsync(viagemId);
                return Ok(pedagios);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        [HttpGet("viagem/{viagemId}/total")]
        [SwaggerOperation(
            Summary = "Relatório de pedágios por viagem",
            Description = "Retorna um relatório com o total gasto em pedágios em uma viagem específica"
        )]
        [SwaggerResponse(200, "Relatório gerado com sucesso")]
        [SwaggerResponse(404, "Viagem não encontrada")]
        public async Task<ActionResult<decimal>> GetTotalPedagiosPorViagem(int viagemId)
        {
            try
            {
                var total = await _pedagioService.GetTotalPedagiosPorViagemAsync(viagemId);
                return Ok(total);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Registrar novo pedágio",
            Description = "Adiciona um novo registro de pedágio a uma viagem"
        )]
        [SwaggerResponse(201, "Pedágio registrado com sucesso")]
        [SwaggerResponse(400, "Dados inválidos")]
        [SwaggerResponse(404, "Viagem não encontrada")]
        public async Task<ActionResult<Pedagio>> PostPedagio(Pedagio pedagio)
        {
            try
            {
                var novoPedagio = await _pedagioService.CreatePedagioAsync(pedagio);
                return CreatedAtAction(nameof(GetPedagio), new { id = novoPedagio.Id }, novoPedagio);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary = "Atualizar pedágio",
            Description = "Atualiza as informações de um registro de pedágio existente"
        )]
        [SwaggerResponse(200, "Pedágio atualizado com sucesso")]
        [SwaggerResponse(404, "Pedágio não encontrado")]
        [SwaggerResponse(400, "Dados inválidos")]
        public async Task<IActionResult> PutPedagio(int id, Pedagio pedagio)
        {
            try
            {
                var pedagioAtualizado = await _pedagioService.UpdatePedagioAsync(id, pedagio);
                return Ok(pedagioAtualizado);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Pedágio com ID {id} não encontrado.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary = "Excluir pedágio",
            Description = "Remove um registro de pedágio do sistema"
        )]
        [SwaggerResponse(200, "Pedágio excluído com sucesso")]
        [SwaggerResponse(404, "Pedágio não encontrado")]
        public async Task<IActionResult> DeletePedagio(int id)
        {
            try
            {
                await _pedagioService.DeletePedagioAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Pedágio com ID {id} não encontrado.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }
    }
} 
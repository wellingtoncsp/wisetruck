using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WiseTruck.API.Models;
using WiseTruck.API.Services;

namespace WiseTruck.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [SwaggerTag("Gerenciamento de caminhões da frota")]
    public class CaminhaoController : ControllerBase
    {
        private readonly ICaminhaoService _caminhaoService;

        public CaminhaoController(ICaminhaoService caminhaoService)
        {
            _caminhaoService = caminhaoService;
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Listar todos os caminhões",
            Description = "Retorna uma lista com todos os caminhões cadastrados no sistema"
        )]
        [SwaggerResponse(200, "Lista de caminhões retornada com sucesso")]
        public async Task<ActionResult<IEnumerable<Caminhao>>> GetCaminhoes()
        {
            try
            {
                var caminhoes = await _caminhaoService.GetAllCaminhoesAsync();
                return Ok(caminhoes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        [HttpGet("disponiveis")]
        [SwaggerOperation(
            Summary = "Listar caminhões disponíveis",
            Description = "Retorna apenas os caminhões com status 'Disponível'"
        )]
        [SwaggerResponse(200, "Lista de caminhões disponíveis retornada com sucesso")]
        public async Task<ActionResult<IEnumerable<Caminhao>>> GetCaminhoesDisponiveis()
        {
            try
            {
                var caminhoes = await _caminhaoService.GetCaminhoesDisponiveisAsync();
                return Ok(caminhoes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Obter caminhão por ID",
            Description = "Retorna os detalhes de um caminhão específico"
        )]
        [SwaggerResponse(200, "Caminhão encontrado com sucesso")]
        [SwaggerResponse(404, "Caminhão não encontrado")]
        public async Task<ActionResult<Caminhao>> GetCaminhao(int id)
        {
            try
            {
                var caminhao = await _caminhaoService.GetCaminhaoByIdAsync(id);
                return Ok(caminhao);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Caminhão com ID {id} não encontrado.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Cadastrar novo caminhão",
            Description = "Adiciona um novo caminhão à frota. O status deve ser: 'Disponível', 'Em Viagem' ou 'Em Manutenção'"
        )]
        [SwaggerResponse(201, "Caminhão criado com sucesso")]
        [SwaggerResponse(400, "Dados inválidos")]
        public async Task<ActionResult<Caminhao>> PostCaminhao(Caminhao caminhao)
        {
            try
            {
                var novoCaminhao = await _caminhaoService.CreateCaminhaoAsync(caminhao);
                return CreatedAtAction(nameof(GetCaminhao), new { id = novoCaminhao.Id }, novoCaminhao);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message, details = ex.InnerException?.Message });
            }
        }

        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary = "Atualizar caminhão",
            Description = "Atualiza os dados de um caminhão existente"
        )]
        [SwaggerResponse(200, "Caminhão atualizado com sucesso")]
        [SwaggerResponse(404, "Caminhão não encontrado")]
        [SwaggerResponse(400, "Dados inválidos")]
        public async Task<IActionResult> PutCaminhao(int id, Caminhao caminhao)
        {
            try
            {
                var caminhaoAtualizado = await _caminhaoService.UpdateCaminhaoAsync(id, caminhao);
                return Ok(caminhaoAtualizado);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Caminhão com ID {id} não encontrado.");
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
            Summary = "Excluir caminhão",
            Description = "Remove um caminhão da frota"
        )]
        [SwaggerResponse(200, "Caminhão excluído com sucesso")]
        [SwaggerResponse(404, "Caminhão não encontrado")]
        public async Task<IActionResult> DeleteCaminhao(int id)
        {
            try
            {
                await _caminhaoService.DeleteCaminhaoAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Caminhão com ID {id} não encontrado.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }
    }
} 
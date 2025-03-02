using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WiseTruck.API.Models;
using WiseTruck.API.Services;

namespace WiseTruck.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [SwaggerTag("Gerenciamento de registros de abastecimentos")]
    public class AbastecimentoController : ControllerBase
    {
        private readonly IAbastecimentoService _abastecimentoService;

        public AbastecimentoController(IAbastecimentoService abastecimentoService)
        {
            _abastecimentoService = abastecimentoService;
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Listar todos os abastecimentos",
            Description = "Retorna uma lista com todos os registros de abastecimentos"
        )]
        [SwaggerResponse(200, "Lista de abastecimentos retornada com sucesso")]
        public async Task<ActionResult<IEnumerable<Abastecimento>>> GetAbastecimentos()
        {
            try
            {
                var abastecimentos = await _abastecimentoService.GetAllAbastecimentosAsync();
                return Ok(abastecimentos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Obter abastecimento por ID",
            Description = "Retorna os detalhes de um registro de abastecimento específico"
        )]
        [SwaggerResponse(200, "Abastecimento encontrado com sucesso")]
        [SwaggerResponse(404, "Abastecimento não encontrado")]
        public async Task<ActionResult<Abastecimento>> GetAbastecimento(int id)
        {
            try
            {
                var abastecimento = await _abastecimentoService.GetAbastecimentoByIdAsync(id);
                return Ok(abastecimento);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Abastecimento com ID {id} não encontrado.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        [HttpGet("viagem/{viagemId}")]
        [SwaggerOperation(
            Summary = "Listar abastecimentos por viagem",
            Description = "Retorna todos os abastecimentos registrados em uma viagem específica"
        )]
        [SwaggerResponse(200, "Lista de abastecimentos da viagem retornada com sucesso")]
        [SwaggerResponse(404, "Viagem não encontrada")]
        public async Task<ActionResult<IEnumerable<Abastecimento>>> GetAbastecimentosPorViagem(int viagemId)
        {
            try
            {
                var abastecimentos = await _abastecimentoService.GetAbastecimentosPorViagemAsync(viagemId);
                return Ok(abastecimentos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        [HttpGet("viagem/{viagemId}/consumo")]
        [SwaggerOperation(
            Summary = "Calcular consumo da viagem",
            Description = "Retorna o consumo médio de combustível (km/l) de uma viagem específica"
        )]
        [SwaggerResponse(200, "Cálculo realizado com sucesso")]
        [SwaggerResponse(404, "Viagem não encontrada")]
        public async Task<ActionResult<decimal>> GetConsumoMedioPorViagem(int viagemId)
        {
            try
            {
                var totalLitros = await _abastecimentoService.GetTotalLitrosPorViagemAsync(viagemId);
                return Ok(totalLitros);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Registrar novo abastecimento",
            Description = "Adiciona um novo registro de abastecimento a uma viagem, incluindo quantidade de litros e valor"
        )]
        [SwaggerResponse(201, "Abastecimento registrado com sucesso")]
        [SwaggerResponse(400, "Dados inválidos")]
        [SwaggerResponse(404, "Viagem não encontrada")]
        public async Task<ActionResult<Abastecimento>> PostAbastecimento(Abastecimento abastecimento)
        {
            try
            {
                var novoAbastecimento = await _abastecimentoService.CreateAbastecimentoAsync(abastecimento);
                return CreatedAtAction(nameof(GetAbastecimento), new { id = novoAbastecimento.Id }, novoAbastecimento);
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
            Summary = "Atualizar abastecimento",
            Description = "Atualiza as informações de um registro de abastecimento existente"
        )]
        [SwaggerResponse(200, "Abastecimento atualizado com sucesso")]
        [SwaggerResponse(404, "Abastecimento não encontrado")]
        [SwaggerResponse(400, "Dados inválidos")]
        public async Task<IActionResult> PutAbastecimento(int id, Abastecimento abastecimento)
        {
            try
            {
                var abastecimentoAtualizado = await _abastecimentoService.UpdateAbastecimentoAsync(id, abastecimento);
                return Ok(abastecimentoAtualizado);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Abastecimento com ID {id} não encontrado.");
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
            Summary = "Excluir abastecimento",
            Description = "Remove um registro de abastecimento do sistema"
        )]
        [SwaggerResponse(200, "Abastecimento excluído com sucesso")]
        [SwaggerResponse(404, "Abastecimento não encontrado")]
        public async Task<IActionResult> DeleteAbastecimento(int id)
        {
            try
            {
                await _abastecimentoService.DeleteAbastecimentoAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Abastecimento com ID {id} não encontrado.");
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

        [HttpGet("relatorio/viagem/{viagemId}")]
        [SwaggerOperation(
            Summary = "Relatório de custos de abastecimento",
            Description = "Retorna um relatório com o total gasto em combustível em uma viagem específica"
        )]
        [SwaggerResponse(200, "Relatório gerado com sucesso")]
        [SwaggerResponse(404, "Viagem não encontrada")]
        public async Task<ActionResult<decimal>> GetTotalAbastecimentosPorViagem(int viagemId)
        {
            try
            {
                var total = await _abastecimentoService.GetTotalAbastecimentosPorViagemAsync(viagemId);
                return Ok(total);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }
    }
} 
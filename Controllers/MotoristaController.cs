using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WiseTruck.API.Models;
using WiseTruck.API.Services;

namespace WiseTruck.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [SwaggerTag("Gerenciamento de motoristas")]
    public class MotoristaController : ControllerBase
    {
        private readonly IMotoristaService _motoristaService;

        public MotoristaController(IMotoristaService motoristaService)
        {
            _motoristaService = motoristaService;
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Listar todos os motoristas",
            Description = "Retorna uma lista com todos os motoristas cadastrados"
        )]
        [SwaggerResponse(200, "Lista de motoristas retornada com sucesso")]
        public async Task<ActionResult<IEnumerable<Motorista>>> GetMotoristas()
        {
            try
            {
                var motoristas = await _motoristaService.GetAllMotoristasAsync();
                return Ok(motoristas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        [HttpGet("cnh-valida")]
        public async Task<ActionResult<IEnumerable<Motorista>>> GetMotoristasComCNHValida()
        {
            try
            {
                var motoristas = await _motoristaService.GetMotoristasComCNHValidaAsync();
                return Ok(motoristas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Obter motorista por ID",
            Description = "Retorna os detalhes de um motorista específico"
        )]
        [SwaggerResponse(200, "Motorista encontrado com sucesso")]
        [SwaggerResponse(404, "Motorista não encontrado")]
        public async Task<ActionResult<Motorista>> GetMotorista(int id)
        {
            try
            {
                var motorista = await _motoristaService.GetMotoristaByIdAsync(id);
                return Ok(motorista);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Motorista com ID {id} não encontrado.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Cadastrar novo motorista",
            Description = "Adiciona um novo motorista ao sistema"
        )]
        [SwaggerResponse(201, "Motorista cadastrado com sucesso")]
        [SwaggerResponse(400, "Dados inválidos")]
        public async Task<ActionResult<Motorista>> PostMotorista(Motorista motorista)
        {
            try
            {
                var novoMotorista = await _motoristaService.CreateMotoristaAsync(motorista);
                return CreatedAtAction(nameof(GetMotorista), new { id = novoMotorista.Id }, novoMotorista);
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
            Summary = "Atualizar motorista",
            Description = "Atualiza os dados de um motorista existente"
        )]
        [SwaggerResponse(200, "Motorista atualizado com sucesso")]
        [SwaggerResponse(404, "Motorista não encontrado")]
        [SwaggerResponse(400, "Dados inválidos")]
        public async Task<IActionResult> PutMotorista(int id, Motorista motorista)
        {
            try
            {
                var motoristaAtualizado = await _motoristaService.UpdateMotoristaAsync(id, motorista);
                return Ok(motoristaAtualizado);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Motorista com ID {id} não encontrado.");
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
            Summary = "Excluir motorista",
            Description = "Remove um motorista do sistema"
        )]
        [SwaggerResponse(200, "Motorista excluído com sucesso")]
        [SwaggerResponse(404, "Motorista não encontrado")]
        public async Task<IActionResult> DeleteMotorista(int id)
        {
            try
            {
                await _motoristaService.DeleteMotoristaAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Motorista com ID {id} não encontrado.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }
    }
} 
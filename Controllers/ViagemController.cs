using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WiseTruck.API.Models;
using WiseTruck.API.Services;

namespace WiseTruck.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [SwaggerTag("Gerenciamento de viagens e roteiros")]
    public class ViagemController : ControllerBase
    {
        private readonly IViagemService _viagemService;

        public ViagemController(IViagemService viagemService)
        {
            _viagemService = viagemService;
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Listar todas as viagens",
            Description = "Retorna uma lista com todas as viagens registradas no sistema"
        )]
        [SwaggerResponse(200, "Lista de viagens retornada com sucesso")]
        public async Task<ActionResult<IEnumerable<Viagem>>> GetViagens()
        {
            try
            {
                var viagens = await _viagemService.GetAllViagensAsync();
                return Ok(viagens);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Obter viagem por ID",
            Description = "Retorna os detalhes de uma viagem específica, incluindo caminhão, motorista, pedágios e abastecimentos"
        )]
        [SwaggerResponse(200, "Viagem encontrada com sucesso")]
        [SwaggerResponse(404, "Viagem não encontrada")]
        public async Task<ActionResult<Viagem>> GetViagem(int id)
        {
            try
            {
                var viagem = await _viagemService.GetViagemByIdAsync(id);
                return Ok(viagem);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Viagem com ID {id} não encontrada.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        [HttpGet("{id}/detalhes")]
        public async Task<ActionResult<Viagem>> GetViagemDetalhes(int id)
        {
            try
            {
                var viagem = await _viagemService.GetViagemComDetalhesAsync(id);
                return Ok(viagem);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Viagem com ID {id} não encontrada.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        [HttpGet("em-andamento")]
        [SwaggerOperation(
            Summary = "Listar viagens em andamento",
            Description = "Retorna todas as viagens com status 'Em Andamento'"
        )]
        [SwaggerResponse(200, "Lista de viagens em andamento retornada com sucesso")]
        public async Task<ActionResult<IEnumerable<Viagem>>> GetViagensEmAndamento()
        {
            try
            {
                var viagens = await _viagemService.GetViagensEmAndamentoAsync();
                return Ok(viagens);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        [HttpGet("caminhao/{caminhaoId}")]
        public async Task<ActionResult<IEnumerable<Viagem>>> GetViagensPorCaminhao(int caminhaoId)
        {
            try
            {
                var viagens = await _viagemService.GetViagensPorCaminhaoAsync(caminhaoId);
                return Ok(viagens);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        [HttpGet("motorista/{motoristaId}")]
        public async Task<ActionResult<IEnumerable<Viagem>>> GetViagensPorMotorista(int motoristaId)
        {
            try
            {
                var viagens = await _viagemService.GetViagensPorMotoristaAsync(motoristaId);
                return Ok(viagens);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Registrar nova viagem",
            Description = "Cria uma nova viagem associando um caminhão e um motorista. O status inicial será 'Planejada'"
        )]
        [SwaggerResponse(201, "Viagem registrada com sucesso")]
        [SwaggerResponse(400, "Dados inválidos ou caminhão/motorista indisponíveis")]
        public async Task<ActionResult<Viagem>> CreateViagem(Viagem viagem)
        {
            try
            {
                var novaViagem = await _viagemService.CreateViagemAsync(viagem);
                return CreatedAtAction(nameof(GetViagem), new { id = novaViagem.Id }, novaViagem);
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
            Summary = "Atualizar viagem",
            Description = "Atualiza os dados de uma viagem existente, incluindo status e informações de rota"
        )]
        [SwaggerResponse(200, "Viagem atualizada com sucesso")]
        [SwaggerResponse(404, "Viagem não encontrada")]
        [SwaggerResponse(400, "Dados inválidos")]
        public async Task<ActionResult<Viagem>> UpdateViagem(int id, Viagem viagem)
        {
            try
            {
                var viagemAtualizada = await _viagemService.UpdateViagemAsync(id, viagem);
                return Ok(viagemAtualizada);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Viagem com ID {id} não encontrada.");
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

        [HttpPost("{id}/iniciar")]
        [SwaggerOperation(
            Summary = "Iniciar viagem",
            Description = "Altera o status da viagem para 'Em Andamento' e registra a data/hora de início"
        )]
        [SwaggerResponse(200, "Viagem iniciada com sucesso")]
        [SwaggerResponse(404, "Viagem não encontrada")]
        [SwaggerResponse(400, "Viagem não pode ser iniciada")]
        public async Task<ActionResult<Viagem>> IniciarViagem(int id)
        {
            try
            {
                var viagem = await _viagemService.IniciarViagemAsync(id);
                return Ok(viagem);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Viagem com ID {id} não encontrada.");
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

        [HttpPost("{id}/finalizar")]
        [SwaggerOperation(
            Summary = "Finalizar viagem",
            Description = "Altera o status da viagem para 'Finalizada', registra a data/hora de término e libera o caminhão e motorista"
        )]
        [SwaggerResponse(200, "Viagem finalizada com sucesso")]
        [SwaggerResponse(404, "Viagem não encontrada")]
        [SwaggerResponse(400, "Viagem não pode ser finalizada")]
        public async Task<ActionResult<Viagem>> FinalizarViagem(int id)
        {
            try
            {
                var viagem = await _viagemService.FinalizarViagemAsync(id);
                return Ok(viagem);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Viagem com ID {id} não encontrada.");
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

        [HttpPost("{id}/cancelar")]
        public async Task<ActionResult<Viagem>> CancelarViagem(int id)
        {
            try
            {
                var viagem = await _viagemService.CancelarViagemAsync(id);
                return Ok(viagem);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Viagem com ID {id} não encontrada.");
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
            Summary = "Excluir viagem",
            Description = "Remove uma viagem do sistema. Só é possível excluir viagens com status 'Planejada'"
        )]
        [SwaggerResponse(200, "Viagem excluída com sucesso")]
        [SwaggerResponse(404, "Viagem não encontrada")]
        [SwaggerResponse(400, "Viagem não pode ser excluída")]
        public async Task<ActionResult> DeleteViagem(int id)
        {
            try
            {
                await _viagemService.DeleteViagemAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Viagem com ID {id} não encontrada.");
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
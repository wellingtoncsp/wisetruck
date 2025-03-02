using WiseTruck.API.Models;

namespace WiseTruck.API.Services
{
    public interface IViagemService
    {
        Task<IEnumerable<Viagem>> GetAllViagensAsync();
        Task<Viagem> GetViagemByIdAsync(int id);
        Task<Viagem> GetViagemComDetalhesAsync(int id);
        Task<IEnumerable<Viagem>> GetViagensEmAndamentoAsync();
        Task<IEnumerable<Viagem>> GetViagensPorCaminhaoAsync(int caminhaoId);
        Task<IEnumerable<Viagem>> GetViagensPorMotoristaAsync(int motoristaId);
        Task<Viagem> CreateViagemAsync(Viagem viagem);
        Task<Viagem> UpdateViagemAsync(int id, Viagem viagem);
        Task<Viagem> IniciarViagemAsync(int id);
        Task<Viagem> FinalizarViagemAsync(int id);
        Task<Viagem> CancelarViagemAsync(int id);
        Task DeleteViagemAsync(int id);
    }
} 
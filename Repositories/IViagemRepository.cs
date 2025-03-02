using WiseTruck.API.Models;

namespace WiseTruck.API.Repositories
{
    public interface IViagemRepository : IBaseRepository<Viagem>
    {
        Task<IEnumerable<Viagem>> GetViagensEmAndamentoAsync();
        Task<IEnumerable<Viagem>> GetViagensPorCaminhaoAsync(int caminhaoId);
        Task<IEnumerable<Viagem>> GetViagensPorMotoristaAsync(int motoristaId);
        Task<Viagem> GetViagemComDetalhesAsync(int viagemId);
    }
} 
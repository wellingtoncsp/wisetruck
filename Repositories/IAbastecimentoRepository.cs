using WiseTruck.API.Models;

namespace WiseTruck.API.Repositories
{
    public interface IAbastecimentoRepository : IBaseRepository<Abastecimento>
    {
        Task<IEnumerable<Abastecimento>> GetAbastecimentosPorViagemAsync(int viagemId);
        Task<decimal> GetTotalAbastecimentosPorViagemAsync(int viagemId);
        Task<decimal> GetTotalLitrosPorViagemAsync(int viagemId);
    }
} 
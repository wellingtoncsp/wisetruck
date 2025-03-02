using WiseTruck.API.Models;

namespace WiseTruck.API.Services
{
    public interface IAbastecimentoService
    {
        Task<IEnumerable<Abastecimento>> GetAllAbastecimentosAsync();
        Task<Abastecimento> GetAbastecimentoByIdAsync(int id);
        Task<IEnumerable<Abastecimento>> GetAbastecimentosPorViagemAsync(int viagemId);
        Task<decimal> GetTotalAbastecimentosPorViagemAsync(int viagemId);
        Task<decimal> GetTotalLitrosPorViagemAsync(int viagemId);
        Task<Abastecimento> CreateAbastecimentoAsync(Abastecimento abastecimento);
        Task<Abastecimento> UpdateAbastecimentoAsync(int id, Abastecimento abastecimento);
        Task DeleteAbastecimentoAsync(int id);
    }
} 
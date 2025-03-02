using WiseTruck.API.Models;

namespace WiseTruck.API.Repositories
{
    public interface IPedagioRepository : IBaseRepository<Pedagio>
    {
        Task<IEnumerable<Pedagio>> GetPedagiosPorViagemAsync(int viagemId);
        Task<decimal> GetTotalPedagiosPorViagemAsync(int viagemId);
    }
} 
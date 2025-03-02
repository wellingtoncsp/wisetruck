using WiseTruck.API.Models;

namespace WiseTruck.API.Services
{
    public interface IPedagioService
    {
        Task<IEnumerable<Pedagio>> GetAllPedagiosAsync();
        Task<Pedagio> GetPedagioByIdAsync(int id);
        Task<IEnumerable<Pedagio>> GetPedagiosPorViagemAsync(int viagemId);
        Task<decimal> GetTotalPedagiosPorViagemAsync(int viagemId);
        Task<Pedagio> CreatePedagioAsync(Pedagio pedagio);
        Task<Pedagio> UpdatePedagioAsync(int id, Pedagio pedagio);
        Task DeletePedagioAsync(int id);
    }
} 
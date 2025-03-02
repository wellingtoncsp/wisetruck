using WiseTruck.API.Models;

namespace WiseTruck.API.Repositories
{
    public interface IMotoristaRepository : IBaseRepository<Motorista>
    {
        Task<IEnumerable<Motorista>> GetMotoristasComCNHValidaAsync();
        Task<bool> ExisteCNHAsync(string cnh);
    }
} 
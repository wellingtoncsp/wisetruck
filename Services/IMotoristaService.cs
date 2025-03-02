using WiseTruck.API.Models;

namespace WiseTruck.API.Services
{
    public interface IMotoristaService
    {
        Task<IEnumerable<Motorista>> GetAllMotoristasAsync();
        Task<Motorista> GetMotoristaByIdAsync(int id);
        Task<IEnumerable<Motorista>> GetMotoristasComCNHValidaAsync();
        Task<Motorista> CreateMotoristaAsync(Motorista motorista);
        Task<Motorista> UpdateMotoristaAsync(int id, Motorista motorista);
        Task DeleteMotoristaAsync(int id);
        Task<bool> ValidarCNHUnicaAsync(string cnh, int? motoristaId = null);
    }
} 
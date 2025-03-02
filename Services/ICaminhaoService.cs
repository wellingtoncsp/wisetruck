using WiseTruck.API.Models;

namespace WiseTruck.API.Services
{
    public interface ICaminhaoService
    {
        Task<IEnumerable<Caminhao>> GetAllCaminhoesAsync();
        Task<Caminhao> GetCaminhaoByIdAsync(int id);
        Task<IEnumerable<Caminhao>> GetCaminhoesDisponiveisAsync();
        Task<Caminhao> CreateCaminhaoAsync(Caminhao caminhao);
        Task<Caminhao> UpdateCaminhaoAsync(int id, Caminhao caminhao);
        Task DeleteCaminhaoAsync(int id);
        Task<bool> ValidarPlacaUnicaAsync(string placa, int? caminhaoId = null);
    }
} 
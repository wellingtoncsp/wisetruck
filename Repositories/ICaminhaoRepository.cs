using WiseTruck.API.Models;

namespace WiseTruck.API.Repositories
{
    public interface ICaminhaoRepository : IBaseRepository<Caminhao>
    {
        Task<IEnumerable<Caminhao>> GetCaminhoesDisponiveisAsync();
        Task<bool> ExistePlacaAsync(string placa);
    }
} 
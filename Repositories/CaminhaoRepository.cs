using Microsoft.EntityFrameworkCore;
using WiseTruck.API.Data;
using WiseTruck.API.Models;

namespace WiseTruck.API.Repositories
{
    public class CaminhaoRepository : BaseRepository<Caminhao>, ICaminhaoRepository
    {
        public CaminhaoRepository(WiseTruckContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Caminhao>> GetCaminhoesDisponiveisAsync()
        {
            return await _dbSet.Where(c => c.Status == "Disponível").ToListAsync();
        }

        public async Task<bool> ExistePlacaAsync(string placa)
        {
            return await _dbSet.AnyAsync(c => c.Placa == placa);
        }
    }
} 
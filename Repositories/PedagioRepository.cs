using Microsoft.EntityFrameworkCore;
using WiseTruck.API.Data;
using WiseTruck.API.Models;

namespace WiseTruck.API.Repositories
{
    public class PedagioRepository : BaseRepository<Pedagio>, IPedagioRepository
    {
        public PedagioRepository(WiseTruckContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Pedagio>> GetPedagiosPorViagemAsync(int viagemId)
        {
            return await _dbSet
                .Where(p => p.ViagemId == viagemId)
                .OrderBy(p => p.DataPagamento)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalPedagiosPorViagemAsync(int viagemId)
        {
            return await _dbSet
                .Where(p => p.ViagemId == viagemId)
                .SumAsync(p => p.Valor);
        }
    }
} 
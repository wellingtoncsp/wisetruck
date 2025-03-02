using Microsoft.EntityFrameworkCore;
using WiseTruck.API.Data;
using WiseTruck.API.Models;

namespace WiseTruck.API.Repositories
{
    public class AbastecimentoRepository : BaseRepository<Abastecimento>, IAbastecimentoRepository
    {
        public AbastecimentoRepository(WiseTruckContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Abastecimento>> GetAbastecimentosPorViagemAsync(int viagemId)
        {
            return await _dbSet
                .Where(a => a.ViagemId == viagemId)
                .OrderBy(a => a.DataAbastecimento)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalAbastecimentosPorViagemAsync(int viagemId)
        {
            return await _dbSet
                .Where(a => a.ViagemId == viagemId)
                .SumAsync(a => a.Valor);
        }

        public async Task<decimal> GetTotalLitrosPorViagemAsync(int viagemId)
        {
            return await _dbSet
                .Where(a => a.ViagemId == viagemId)
                .SumAsync(a => a.Litros);
        }
    }
} 
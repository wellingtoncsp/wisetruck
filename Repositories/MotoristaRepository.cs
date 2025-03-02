using Microsoft.EntityFrameworkCore;
using WiseTruck.API.Data;
using WiseTruck.API.Models;

namespace WiseTruck.API.Repositories
{
    public class MotoristaRepository : BaseRepository<Motorista>, IMotoristaRepository
    {
        public MotoristaRepository(WiseTruckContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Motorista>> GetMotoristasComCNHValidaAsync()
        {
            var dataAtual = DateTime.Now.Date;
            return await _dbSet.Where(m => m.ValidadeCNH >= dataAtual).ToListAsync();
        }

        public async Task<bool> ExisteCNHAsync(string cnh)
        {
            return await _dbSet.AnyAsync(m => m.CNH == cnh);
        }
    }
} 
using Microsoft.EntityFrameworkCore;
using WiseTruck.API.Data;
using WiseTruck.API.Models;

namespace WiseTruck.API.Repositories
{
    public class ViagemRepository : BaseRepository<Viagem>, IViagemRepository
    {
        public ViagemRepository(WiseTruckContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Viagem>> GetViagensEmAndamentoAsync()
        {
            return await _dbSet
                .Where(v => v.Status == "Em Andamento")
                .Include(v => v.Caminhao)
                .Include(v => v.Motorista)
                .ToListAsync();
        }

        public async Task<IEnumerable<Viagem>> GetViagensPorCaminhaoAsync(int caminhaoId)
        {
            return await _dbSet
                .Where(v => v.CaminhaoId == caminhaoId)
                .Include(v => v.Motorista)
                .OrderByDescending(v => v.DataInicio)
                .ToListAsync();
        }

        public async Task<IEnumerable<Viagem>> GetViagensPorMotoristaAsync(int motoristaId)
        {
            return await _dbSet
                .Where(v => v.MotoristaId == motoristaId)
                .Include(v => v.Caminhao)
                .OrderByDescending(v => v.DataInicio)
                .ToListAsync();
        }

        public async Task<Viagem> GetViagemComDetalhesAsync(int viagemId)
        {
            return await _dbSet
                .Include(v => v.Caminhao)
                .Include(v => v.Motorista)
                .Include(v => v.Pedagios)
                .Include(v => v.Abastecimentos)
                .FirstOrDefaultAsync(v => v.Id == viagemId);
        }
    }
} 
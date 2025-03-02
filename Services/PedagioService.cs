using WiseTruck.API.Models;
using WiseTruck.API.Repositories;

namespace WiseTruck.API.Services
{
    public class PedagioService : IPedagioService
    {
        private readonly IPedagioRepository _pedagioRepository;
        private readonly IViagemRepository _viagemRepository;

        public PedagioService(
            IPedagioRepository pedagioRepository,
            IViagemRepository viagemRepository)
        {
            _pedagioRepository = pedagioRepository;
            _viagemRepository = viagemRepository;
        }

        public async Task<IEnumerable<Pedagio>> GetAllPedagiosAsync()
        {
            return await _pedagioRepository.GetAllAsync();
        }

        public async Task<Pedagio> GetPedagioByIdAsync(int id)
        {
            var pedagio = await _pedagioRepository.GetByIdAsync(id);
            if (pedagio == null)
                throw new KeyNotFoundException($"Pedágio com ID {id} não encontrado.");
            return pedagio;
        }

        public async Task<IEnumerable<Pedagio>> GetPedagiosPorViagemAsync(int viagemId)
        {
            return await _pedagioRepository.GetPedagiosPorViagemAsync(viagemId);
        }

        public async Task<decimal> GetTotalPedagiosPorViagemAsync(int viagemId)
        {
            return await _pedagioRepository.GetTotalPedagiosPorViagemAsync(viagemId);
        }

        public async Task<Pedagio> CreatePedagioAsync(Pedagio pedagio)
        {
            var viagem = await _viagemRepository.GetByIdAsync(pedagio.ViagemId);
            if (viagem == null)
                throw new InvalidOperationException("Viagem não encontrada.");

            if (viagem.Status != "Em Andamento")
                throw new InvalidOperationException("Só é possível registrar pedágios em viagens em andamento.");

            await _pedagioRepository.AddAsync(pedagio);
            await _pedagioRepository.SaveChangesAsync();
            return pedagio;
        }

        public async Task<Pedagio> UpdatePedagioAsync(int id, Pedagio pedagio)
        {
            var existingPedagio = await GetPedagioByIdAsync(id);
            var viagem = await _viagemRepository.GetByIdAsync(pedagio.ViagemId);

            if (viagem.Status != "Em Andamento")
                throw new InvalidOperationException("Só é possível atualizar pedágios de viagens em andamento.");

            existingPedagio.Valor = pedagio.Valor;
            existingPedagio.Localizacao = pedagio.Localizacao;
            existingPedagio.DataPagamento = pedagio.DataPagamento;

            await _pedagioRepository.UpdateAsync(existingPedagio);
            await _pedagioRepository.SaveChangesAsync();
            return existingPedagio;
        }

        public async Task DeletePedagioAsync(int id)
        {
            var pedagio = await GetPedagioByIdAsync(id);
            var viagem = await _viagemRepository.GetByIdAsync(pedagio.ViagemId);

            if (viagem.Status != "Em Andamento")
                throw new InvalidOperationException("Só é possível excluir pedágios de viagens em andamento.");

            await _pedagioRepository.DeleteAsync(pedagio);
            await _pedagioRepository.SaveChangesAsync();
        }
    }
} 
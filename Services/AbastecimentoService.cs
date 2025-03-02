using WiseTruck.API.Models;
using WiseTruck.API.Repositories;

namespace WiseTruck.API.Services
{
    public class AbastecimentoService : IAbastecimentoService
    {
        private readonly IAbastecimentoRepository _abastecimentoRepository;
        private readonly IViagemRepository _viagemRepository;

        public AbastecimentoService(
            IAbastecimentoRepository abastecimentoRepository,
            IViagemRepository viagemRepository)
        {
            _abastecimentoRepository = abastecimentoRepository;
            _viagemRepository = viagemRepository;
        }

        public async Task<IEnumerable<Abastecimento>> GetAllAbastecimentosAsync()
        {
            return await _abastecimentoRepository.GetAllAsync();
        }

        public async Task<Abastecimento> GetAbastecimentoByIdAsync(int id)
        {
            var abastecimento = await _abastecimentoRepository.GetByIdAsync(id);
            if (abastecimento == null)
                throw new KeyNotFoundException($"Abastecimento com ID {id} não encontrado.");
            return abastecimento;
        }

        public async Task<IEnumerable<Abastecimento>> GetAbastecimentosPorViagemAsync(int viagemId)
        {
            return await _abastecimentoRepository.GetAbastecimentosPorViagemAsync(viagemId);
        }

        public async Task<decimal> GetTotalAbastecimentosPorViagemAsync(int viagemId)
        {
            return await _abastecimentoRepository.GetTotalAbastecimentosPorViagemAsync(viagemId);
        }

        public async Task<decimal> GetTotalLitrosPorViagemAsync(int viagemId)
        {
            return await _abastecimentoRepository.GetTotalLitrosPorViagemAsync(viagemId);
        }

        public async Task<Abastecimento> CreateAbastecimentoAsync(Abastecimento abastecimento)
        {
            var viagem = await _viagemRepository.GetByIdAsync(abastecimento.ViagemId);
            if (viagem == null)
                throw new InvalidOperationException("Viagem não encontrada.");

            if (viagem.Status != "Em Andamento")
                throw new InvalidOperationException("Só é possível registrar abastecimentos em viagens em andamento.");

            await _abastecimentoRepository.AddAsync(abastecimento);
            await _abastecimentoRepository.SaveChangesAsync();
            return abastecimento;
        }

        public async Task<Abastecimento> UpdateAbastecimentoAsync(int id, Abastecimento abastecimento)
        {
            var existingAbastecimento = await GetAbastecimentoByIdAsync(id);
            var viagem = await _viagemRepository.GetByIdAsync(abastecimento.ViagemId);

            if (viagem.Status != "Em Andamento")
                throw new InvalidOperationException("Só é possível atualizar abastecimentos de viagens em andamento.");

            existingAbastecimento.Litros = abastecimento.Litros;
            existingAbastecimento.Valor = abastecimento.Valor;
            existingAbastecimento.Localizacao = abastecimento.Localizacao;
            existingAbastecimento.DataAbastecimento = abastecimento.DataAbastecimento;

            await _abastecimentoRepository.UpdateAsync(existingAbastecimento);
            await _abastecimentoRepository.SaveChangesAsync();
            return existingAbastecimento;
        }

        public async Task DeleteAbastecimentoAsync(int id)
        {
            var abastecimento = await GetAbastecimentoByIdAsync(id);
            var viagem = await _viagemRepository.GetByIdAsync(abastecimento.ViagemId);

            if (viagem.Status != "Em Andamento")
                throw new InvalidOperationException("Só é possível excluir abastecimentos de viagens em andamento.");

            await _abastecimentoRepository.DeleteAsync(abastecimento);
            await _abastecimentoRepository.SaveChangesAsync();
        }
    }
} 
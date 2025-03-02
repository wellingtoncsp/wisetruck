using WiseTruck.API.Models;
using WiseTruck.API.Repositories;

namespace WiseTruck.API.Services
{
    public class CaminhaoService : ICaminhaoService
    {
        private readonly ICaminhaoRepository _caminhaoRepository;

        public CaminhaoService(ICaminhaoRepository caminhaoRepository)
        {
            _caminhaoRepository = caminhaoRepository;
        }

        public async Task<IEnumerable<Caminhao>> GetAllCaminhoesAsync()
        {
            return await _caminhaoRepository.GetAllAsync();
        }

        public async Task<Caminhao> GetCaminhaoByIdAsync(int id)
        {
            var caminhao = await _caminhaoRepository.GetByIdAsync(id);
            if (caminhao == null)
                throw new KeyNotFoundException($"Caminhão com ID {id} não encontrado.");
            return caminhao;
        }

        public async Task<IEnumerable<Caminhao>> GetCaminhoesDisponiveisAsync()
        {
            return await _caminhaoRepository.GetCaminhoesDisponiveisAsync();
        }

        public async Task<Caminhao> CreateCaminhaoAsync(Caminhao caminhao)
        {
            try 
            {
                if (await ValidarPlacaUnicaAsync(caminhao.Placa))
                {
                    await _caminhaoRepository.AddAsync(caminhao);
                    await _caminhaoRepository.SaveChangesAsync();
                    return caminhao;
                }
                throw new InvalidOperationException("Já existe um caminhão com esta placa.");
            }
            catch (Exception ex)
            {
                // Log mais detalhado
                Console.WriteLine($"Erro ao criar caminhão: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                    Console.WriteLine($"Inner Stack Trace: {ex.InnerException.StackTrace}");
                }
                throw; // Re-throw para manter o comportamento original
            }
        }

        public async Task<Caminhao> UpdateCaminhaoAsync(int id, Caminhao caminhao)
        {
            var existingCaminhao = await GetCaminhaoByIdAsync(id);
            
            if (await ValidarPlacaUnicaAsync(caminhao.Placa, id))
            {
                existingCaminhao.Modelo = caminhao.Modelo;
                existingCaminhao.Placa = caminhao.Placa;
                existingCaminhao.DataFabricacao = caminhao.DataFabricacao;
                existingCaminhao.Status = caminhao.Status;

                await _caminhaoRepository.UpdateAsync(existingCaminhao);
                await _caminhaoRepository.SaveChangesAsync();
                return existingCaminhao;
            }
            throw new InvalidOperationException("Já existe um caminhão com esta placa.");
        }

        public async Task DeleteCaminhaoAsync(int id)
        {
            var caminhao = await GetCaminhaoByIdAsync(id);
            await _caminhaoRepository.DeleteAsync(caminhao);
            await _caminhaoRepository.SaveChangesAsync();
        }

        public async Task<bool> ValidarPlacaUnicaAsync(string placa, int? caminhaoId = null)
        {
            var caminhoes = await _caminhaoRepository.FindAsync(c => c.Placa == placa);
            return !caminhoes.Any() || (caminhaoId.HasValue && caminhoes.Count() == 1 && caminhoes.First().Id == caminhaoId);
        }
    }
} 
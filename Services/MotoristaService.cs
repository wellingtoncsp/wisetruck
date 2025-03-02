using WiseTruck.API.Models;
using WiseTruck.API.Repositories;

namespace WiseTruck.API.Services
{
    public class MotoristaService : IMotoristaService
    {
        private readonly IMotoristaRepository _motoristaRepository;

        public MotoristaService(IMotoristaRepository motoristaRepository)
        {
            _motoristaRepository = motoristaRepository;
        }

        public async Task<IEnumerable<Motorista>> GetAllMotoristasAsync()
        {
            return await _motoristaRepository.GetAllAsync();
        }

        public async Task<Motorista> GetMotoristaByIdAsync(int id)
        {
            var motorista = await _motoristaRepository.GetByIdAsync(id);
            if (motorista == null)
                throw new KeyNotFoundException($"Motorista com ID {id} não encontrado.");
            return motorista;
        }

        public async Task<IEnumerable<Motorista>> GetMotoristasComCNHValidaAsync()
        {
            return await _motoristaRepository.GetMotoristasComCNHValidaAsync();
        }

        public async Task<Motorista> CreateMotoristaAsync(Motorista motorista)
        {
            if (await ValidarCNHUnicaAsync(motorista.CNH))
            {
                await _motoristaRepository.AddAsync(motorista);
                await _motoristaRepository.SaveChangesAsync();
                return motorista;
            }
            throw new InvalidOperationException("Já existe um motorista com esta CNH.");
        }

        public async Task<Motorista> UpdateMotoristaAsync(int id, Motorista motorista)
        {
            var existingMotorista = await GetMotoristaByIdAsync(id);
            
            if (await ValidarCNHUnicaAsync(motorista.CNH, id))
            {
                existingMotorista.Nome = motorista.Nome;
                existingMotorista.CNH = motorista.CNH;
                existingMotorista.ValidadeCNH = motorista.ValidadeCNH;

                await _motoristaRepository.UpdateAsync(existingMotorista);
                await _motoristaRepository.SaveChangesAsync();
                return existingMotorista;
            }
            throw new InvalidOperationException("Já existe um motorista com esta CNH.");
        }

        public async Task DeleteMotoristaAsync(int id)
        {
            var motorista = await GetMotoristaByIdAsync(id);
            await _motoristaRepository.DeleteAsync(motorista);
            await _motoristaRepository.SaveChangesAsync();
        }

        public async Task<bool> ValidarCNHUnicaAsync(string cnh, int? motoristaId = null)
        {
            var motoristas = await _motoristaRepository.FindAsync(m => m.CNH == cnh);
            return !motoristas.Any() || (motoristaId.HasValue && motoristas.Count() == 1 && motoristas.First().Id == motoristaId);
        }
    }
} 
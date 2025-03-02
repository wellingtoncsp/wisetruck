using WiseTruck.API.Models;
using WiseTruck.API.Repositories;

namespace WiseTruck.API.Services
{
    public class ViagemService : IViagemService
    {
        private readonly IViagemRepository _viagemRepository;
        private readonly ICaminhaoRepository _caminhaoRepository;
        private readonly IMotoristaRepository _motoristaRepository;

        public ViagemService(
            IViagemRepository viagemRepository,
            ICaminhaoRepository caminhaoRepository,
            IMotoristaRepository motoristaRepository)
        {
            _viagemRepository = viagemRepository;
            _caminhaoRepository = caminhaoRepository;
            _motoristaRepository = motoristaRepository;
        }

        public async Task<IEnumerable<Viagem>> GetAllViagensAsync()
        {
            return await _viagemRepository.GetAllAsync();
        }

        public async Task<Viagem> GetViagemByIdAsync(int id)
        {
            var viagem = await _viagemRepository.GetByIdAsync(id);
            if (viagem == null)
                throw new KeyNotFoundException($"Viagem com ID {id} não encontrada.");
            return viagem;
        }

        public async Task<Viagem> GetViagemComDetalhesAsync(int id)
        {
            var viagem = await _viagemRepository.GetViagemComDetalhesAsync(id);
            if (viagem == null)
                throw new KeyNotFoundException($"Viagem com ID {id} não encontrada.");
            return viagem;
        }

        public async Task<IEnumerable<Viagem>> GetViagensEmAndamentoAsync()
        {
            return await _viagemRepository.GetViagensEmAndamentoAsync();
        }

        public async Task<IEnumerable<Viagem>> GetViagensPorCaminhaoAsync(int caminhaoId)
        {
            return await _viagemRepository.GetViagensPorCaminhaoAsync(caminhaoId);
        }

        public async Task<IEnumerable<Viagem>> GetViagensPorMotoristaAsync(int motoristaId)
        {
            return await _viagemRepository.GetViagensPorMotoristaAsync(motoristaId);
        }

        public async Task<Viagem> CreateViagemAsync(Viagem viagem)
        {
            var caminhao = await _caminhaoRepository.GetByIdAsync(viagem.CaminhaoId);
            if (caminhao == null)
                throw new InvalidOperationException("Caminhão não encontrado.");
            
            if (caminhao.Status != "Disponível")
                throw new InvalidOperationException("Caminhão não está disponível para viagem.");

            if (viagem.MotoristaId.HasValue)
            {
                var motorista = await _motoristaRepository.GetByIdAsync(viagem.MotoristaId.Value);
                if (motorista == null)
                    throw new InvalidOperationException("Motorista não encontrado.");
                
                if (motorista.ValidadeCNH < DateTime.Now)
                    throw new InvalidOperationException("CNH do motorista está vencida.");
            }

            viagem.Status = "Planejada";
            await _viagemRepository.AddAsync(viagem);
            await _viagemRepository.SaveChangesAsync();

            return viagem;
        }

        public async Task<Viagem> UpdateViagemAsync(int id, Viagem viagem)
        {
            var existingViagem = await GetViagemByIdAsync(id);
            
            if (existingViagem.Status != "Planejada")
                throw new InvalidOperationException("Só é possível alterar viagens planejadas.");

            existingViagem.CaminhaoId = viagem.CaminhaoId;
            existingViagem.MotoristaId = viagem.MotoristaId;
            existingViagem.Origem = viagem.Origem;
            existingViagem.Destino = viagem.Destino;
            existingViagem.DistanciaKm = viagem.DistanciaKm;
            existingViagem.DataInicio = viagem.DataInicio;

            await _viagemRepository.UpdateAsync(existingViagem);
            await _viagemRepository.SaveChangesAsync();

            return existingViagem;
        }

        public async Task<Viagem> IniciarViagemAsync(int id)
        {
            var viagem = await GetViagemByIdAsync(id);
            
            if (viagem.Status != "Planejada")
                throw new InvalidOperationException("Só é possível iniciar viagens planejadas.");

            if (!viagem.MotoristaId.HasValue)
                throw new InvalidOperationException("Não é possível iniciar uma viagem sem motorista.");

            var caminhao = await _caminhaoRepository.GetByIdAsync(viagem.CaminhaoId);
            caminhao.Status = "Em Viagem";
            await _caminhaoRepository.UpdateAsync(caminhao);

            viagem.Status = "Em Andamento";
            viagem.DataInicio = DateTime.Now;
            await _viagemRepository.UpdateAsync(viagem);
            await _viagemRepository.SaveChangesAsync();

            return viagem;
        }

        public async Task<Viagem> FinalizarViagemAsync(int id)
        {
            var viagem = await GetViagemByIdAsync(id);
            
            if (viagem.Status != "Em Andamento")
                throw new InvalidOperationException("Só é possível finalizar viagens em andamento.");

            var caminhao = await _caminhaoRepository.GetByIdAsync(viagem.CaminhaoId);
            caminhao.Status = "Disponível";
            await _caminhaoRepository.UpdateAsync(caminhao);

            viagem.Status = "Concluída";
            viagem.DataFim = DateTime.Now;
            await _viagemRepository.UpdateAsync(viagem);
            await _viagemRepository.SaveChangesAsync();

            return viagem;
        }

        public async Task<Viagem> CancelarViagemAsync(int id)
        {
            var viagem = await GetViagemByIdAsync(id);
            
            if (viagem.Status == "Concluída")
                throw new InvalidOperationException("Não é possível cancelar uma viagem concluída.");

            if (viagem.Status == "Em Andamento")
            {
                var caminhao = await _caminhaoRepository.GetByIdAsync(viagem.CaminhaoId);
                caminhao.Status = "Disponível";
                await _caminhaoRepository.UpdateAsync(caminhao);
            }

            viagem.Status = "Cancelada";
            viagem.DataFim = DateTime.Now;
            await _viagemRepository.UpdateAsync(viagem);
            await _viagemRepository.SaveChangesAsync();

            return viagem;
        }

        public async Task DeleteViagemAsync(int id)
        {
            var viagem = await GetViagemByIdAsync(id);
            
            if (viagem.Status != "Planejada" && viagem.Status != "Cancelada")
                throw new InvalidOperationException("Só é possível excluir viagens planejadas ou canceladas.");

            await _viagemRepository.DeleteAsync(viagem);
            await _viagemRepository.SaveChangesAsync();
        }
    }
} 
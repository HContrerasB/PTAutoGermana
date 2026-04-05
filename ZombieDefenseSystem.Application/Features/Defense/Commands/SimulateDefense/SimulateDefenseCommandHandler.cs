using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZombieDefenseSystem.Application.Abstractions.Persistence;
using ZombieDefenseSystem.Application.Abstractions.Services;
using ZombieDefenseSystem.Domain.Entities;

namespace ZombieDefenseSystem.Application.Features.Defense.Commands.SimulateDefense
{
    public class SimulateDefenseCommandHandler
    : IRequestHandler<SimulateDefenseCommand, SimulateDefenseCommandResponse>
    {
        private readonly IZombieRepository _zombieRepository;
        private readonly ISimulationRepository _simulationRepository;
        private readonly IEliminatedZombieRepository _eliminatedZombieRepository;
        private readonly IDefenseStrategyOptimizer _optimizer;

        public SimulateDefenseCommandHandler(
            IZombieRepository zombieRepository,
            ISimulationRepository simulationRepository,
            IEliminatedZombieRepository eliminatedZombieRepository,
            IDefenseStrategyOptimizer optimizer)
        {
            _zombieRepository = zombieRepository;
            _simulationRepository = simulationRepository;
            _eliminatedZombieRepository = eliminatedZombieRepository;
            _optimizer = optimizer;
        }

        public async Task<SimulateDefenseCommandResponse> Handle(
            SimulateDefenseCommand request,
            CancellationToken cancellationToken)
        {
            var zombies = await _zombieRepository.GetActiveAsync(cancellationToken);

            var strategy = _optimizer.Calculate(
                zombies,
                request.Bullets,
                request.SecondsAvailable);

            var simulation = new Simulation
            {
                Fecha = DateTime.UtcNow,
                TiempoDisponible = request.SecondsAvailable,
                BalasDisponibles = request.Bullets,
                PuntajeTotal = strategy.TotalScore,
                TiempoUsado = strategy.SecondsUsed,
                BalasUsadas = strategy.BulletsUsed,
                TotalZombiesEliminados = strategy.Strategy.Sum(x => x.Cantidad)
            };

            await _simulationRepository.AddAsync(simulation, cancellationToken);
            await _simulationRepository.SaveChangesAsync(cancellationToken);

            var eliminados = strategy.Strategy.Select(item => new EliminatedZombie
            {
                ZombieId = item.ZombieId,
                SimulacionId = simulation.Id,
                CantidadEliminados = item.Cantidad,
                PuntosObtenidos = item.PuntajeTotal,
                Timestamp = DateTime.UtcNow
            }).ToList();

            if (eliminados.Any())
            {
                await _eliminatedZombieRepository.AddRangeAsync(eliminados, cancellationToken);
                await _simulationRepository.SaveChangesAsync(cancellationToken);
            }

            return new SimulateDefenseCommandResponse
            {
                SimulationId = simulation.Id,
                StrategyResult = strategy,
                TotalZombiesEliminados = simulation.TotalZombiesEliminados
            };
        }
    }
}

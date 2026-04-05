using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZombieDefenseSystem.Application.Features.Defense.Queries.GetOptimalDefenseStrategy;
using ZombieDefenseSystem.Domain.Entities;

namespace ZombieDefenseSystem.Application.Abstractions.Persistence
{
    public interface ISimulationRepository
    {
        Task AddAsync(Simulation simulation, CancellationToken cancellationToken = default);
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
        Task<IReadOnlyList<SimulationRankingItemResponse>> GetRankingAsync(CancellationToken cancellationToken = default);
    }
}

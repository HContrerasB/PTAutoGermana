using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZombieDefenseSystem.Application.Abstractions.Persistence;

namespace ZombieDefenseSystem.Application.Features.Defense.Queries.GetOptimalDefenseStrategy
{
    public class GetSimulationRankingQueryHandler
    : IRequestHandler<GetSimulationRankingQuery, List<SimulationRankingItemResponse>>
    {
        private readonly ISimulationRepository _simulationRepository;

        public GetSimulationRankingQueryHandler(ISimulationRepository simulationRepository)
        {
            _simulationRepository = simulationRepository;
        }

        public async Task<List<SimulationRankingItemResponse>> Handle(
            GetSimulationRankingQuery request,
            CancellationToken cancellationToken)
        {
            var ranking = await _simulationRepository.GetRankingAsync(cancellationToken);
            return ranking.ToList();
        }
    }
}

using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZombieDefenseSystem.Application.Abstractions.Persistence;
using ZombieDefenseSystem.Application.Abstractions.Services;

namespace ZombieDefenseSystem.Application.Features.Defense.Queries.GetOptimalDefenseStrategy
{
    public class GetOptimalDefenseStrategyQueryHandler
    : IRequestHandler<GetOptimalDefenseStrategyQuery, OptimalDefenseStrategyResponse>
    {
        private readonly IZombieRepository _zombieRepository;
        private readonly IDefenseStrategyOptimizer _optimizer;

        public GetOptimalDefenseStrategyQueryHandler(
            IZombieRepository zombieRepository,
            IDefenseStrategyOptimizer optimizer)
        {
            _zombieRepository = zombieRepository;
            _optimizer = optimizer;
        }

        public async Task<OptimalDefenseStrategyResponse> Handle(
            GetOptimalDefenseStrategyQuery request,
            CancellationToken cancellationToken)
        {
            var zombies = await _zombieRepository.GetActiveAsync(cancellationToken);

            return _optimizer.Calculate(
                zombies,
                request.Bullets,
                request.SecondsAvailable);
        }
    }
}

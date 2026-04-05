using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZombieDefenseSystem.Application.Features.Defense.Queries.GetOptimalDefenseStrategy
{
    public class GetSimulationRankingQuery : IRequest<List<SimulationRankingItemResponse>>
    {
    }
}

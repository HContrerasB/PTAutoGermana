using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZombieDefenseSystem.Application.Features.Defense.Queries.GetOptimalDefenseStrategy;

namespace ZombieDefenseSystem.Application.Features.Defense.Commands.SimulateDefense
{
    public class SimulateDefenseCommandResponse
    {
        public int SimulationId { get; set; }
        public OptimalDefenseStrategyResponse StrategyResult { get; set; } = new();
        public int TotalZombiesEliminados { get; set; }

    }
}

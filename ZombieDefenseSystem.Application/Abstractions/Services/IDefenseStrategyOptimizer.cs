using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZombieDefenseSystem.Application.Features.Defense.Queries.GetOptimalDefenseStrategy;
using ZombieDefenseSystem.Domain.Entities;

namespace ZombieDefenseSystem.Application.Abstractions.Services
{
    public interface IDefenseStrategyOptimizer
    {
        OptimalDefenseStrategyResponse Calculate(
            IReadOnlyCollection<Zombie> zombies,
            int bulletsAvailable,
            int secondsAvailable);
    }
}

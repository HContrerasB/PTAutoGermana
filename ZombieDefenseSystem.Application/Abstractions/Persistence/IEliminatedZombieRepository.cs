using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZombieDefenseSystem.Domain.Entities;

namespace ZombieDefenseSystem.Application.Abstractions.Persistence
{
    public interface IEliminatedZombieRepository
    {
        Task AddRangeAsync(IEnumerable<EliminatedZombie> eliminados, CancellationToken cancellationToken = default);
    }
}

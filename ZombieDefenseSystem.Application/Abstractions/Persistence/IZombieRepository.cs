using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZombieDefenseSystem.Domain.Entities;

namespace ZombieDefenseSystem.Application.Abstractions.Persistence
{
    public interface IZombieRepository
    {
        Task<IReadOnlyList<Zombie>> GetActiveAsync(CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Zombie>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Zombie?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task AddAsync(Zombie zombie, CancellationToken cancellationToken = default);
        void Update(Zombie zombie);
        void Delete(Zombie zombie);
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZombieDefenseSystem.Application.Abstractions.Persistence;
using ZombieDefenseSystem.Domain.Entities;

namespace ZombieDefenseSystem.Infrastructure.Persistence.Repositories
{
    public class EliminatedZombieRepository : IEliminatedZombieRepository
    {
        private readonly AppDbContext _context;

        public EliminatedZombieRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddRangeAsync(IEnumerable<EliminatedZombie> eliminados, CancellationToken cancellationToken = default)
        {
            await _context.Eliminados.AddRangeAsync(eliminados, cancellationToken);
        }
    }
}

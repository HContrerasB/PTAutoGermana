using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZombieDefenseSystem.Application.Abstractions.Persistence;
using ZombieDefenseSystem.Domain.Entities;

namespace ZombieDefenseSystem.Infrastructure.Persistence.Repositories
{
    public class ZombieRepository : IZombieRepository
    {
        private readonly AppDbContext _context;

        public ZombieRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<Zombie>> GetActiveAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Zombies
                .AsNoTracking()
                .Where(x => x.Activo)
                .OrderByDescending(x => x.NivelAmenaza)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Zombie>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Zombies
                .AsNoTracking()
                .OrderByDescending(x => x.Activo)
                .ThenByDescending(x => x.NivelAmenaza)
                .ThenBy(x => x.Tipo)
                .ToListAsync(cancellationToken);
        }

        public async Task<Zombie?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Zombies
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task AddAsync(Zombie zombie, CancellationToken cancellationToken = default)
        {
            await _context.Zombies.AddAsync(zombie, cancellationToken);
        }

        public void Update(Zombie zombie)
        {
            _context.Zombies.Update(zombie);
        }

        public void Delete(Zombie zombie)
        {
            _context.Zombies.Remove(zombie);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}

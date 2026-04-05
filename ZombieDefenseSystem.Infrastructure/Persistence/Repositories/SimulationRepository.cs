using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZombieDefenseSystem.Application.Abstractions.Persistence;
using ZombieDefenseSystem.Application.Features.Defense.Queries.GetOptimalDefenseStrategy;
using ZombieDefenseSystem.Domain.Entities;

namespace ZombieDefenseSystem.Infrastructure.Persistence.Repositories
{
    public class SimulationRepository : ISimulationRepository
    {
        private readonly AppDbContext _context;

        public SimulationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Simulation simulation, CancellationToken cancellationToken = default)
        {
            await _context.Simulations.AddAsync(simulation, cancellationToken);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<SimulationRankingItemResponse>> GetRankingAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Simulations
                .AsNoTracking()
                .OrderByDescending(x => x.PuntajeTotal)
                .ThenBy(x => x.BalasUsadas)
                .ThenBy(x => x.TiempoUsado)
                .ThenByDescending(x => x.Fecha)
                .Select(x => new SimulationRankingItemResponse
                {
                    SimulationId = x.Id,
                    Fecha = x.Fecha,
                    TiempoDisponible = x.TiempoDisponible,
                    BalasDisponibles = x.BalasDisponibles,
                    PuntajeTotal = x.PuntajeTotal,
                    TiempoUsado = x.TiempoUsado,
                    BalasUsadas = x.BalasUsadas,
                    TotalZombiesEliminados = x.TotalZombiesEliminados
                })
                .ToListAsync(cancellationToken);
        }
    }
}

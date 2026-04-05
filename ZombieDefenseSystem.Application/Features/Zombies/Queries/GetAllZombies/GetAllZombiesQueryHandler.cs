using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZombieDefenseSystem.Application.Abstractions.Persistence;

namespace ZombieDefenseSystem.Application.Features.Zombies.Queries.GetAllZombies
{
    public class GetAllZombiesQueryHandler : IRequestHandler<GetAllZombiesQuery, List<ZombieResponse>>
    {
        private readonly IZombieRepository _zombieRepository;

        public GetAllZombiesQueryHandler(IZombieRepository zombieRepository)
        {
            _zombieRepository = zombieRepository;
        }

        public async Task<List<ZombieResponse>> Handle(GetAllZombiesQuery request, CancellationToken cancellationToken)
        {
            var zombies = await _zombieRepository.GetAllAsync(cancellationToken);

            return zombies.Select(z => new ZombieResponse
            {
                Id = z.Id,
                Tipo = z.Tipo,
                TiempoDisparo = z.TiempoDisparo,
                BalasNecesarias = z.BalasNecesarias,
                PuntajeBase = z.PuntajeBase,
                NivelAmenaza = z.NivelAmenaza,
                Activo = z.Activo
            }).ToList();
        }
    }
}

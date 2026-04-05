using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZombieDefenseSystem.Application.Abstractions.Persistence;

namespace ZombieDefenseSystem.Application.Features.Zombies.Queries.GetAllZombies
{
    public class GetZombieByIdQueryHandler : IRequestHandler<GetZombieByIdQuery, ZombieResponse?>
    {
        private readonly IZombieRepository _zombieRepository;

        public GetZombieByIdQueryHandler(IZombieRepository zombieRepository)
        {
            _zombieRepository = zombieRepository;
        }

        public async Task<ZombieResponse?> Handle(GetZombieByIdQuery request, CancellationToken cancellationToken)
        {
            var zombie = await _zombieRepository.GetByIdAsync(request.Id, cancellationToken);

            if (zombie is null)
                return null;

            return new ZombieResponse
            {
                Id = zombie.Id,
                Tipo = zombie.Tipo,
                TiempoDisparo = zombie.TiempoDisparo,
                BalasNecesarias = zombie.BalasNecesarias,
                PuntajeBase = zombie.PuntajeBase,
                NivelAmenaza = zombie.NivelAmenaza,
                Activo = zombie.Activo
            };
        }
    }
}

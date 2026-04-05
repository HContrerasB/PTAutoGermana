using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZombieDefenseSystem.Application.Abstractions.Persistence;
using ZombieDefenseSystem.Domain.Entities;

namespace ZombieDefenseSystem.Application.Features.Zombies.Commands.CreateZombie
{
    public class CreateZombieCommandHandler : IRequestHandler<CreateZombieCommand, int>
    {
        private readonly IZombieRepository _zombieRepository;

        public CreateZombieCommandHandler(IZombieRepository zombieRepository)
        {
            _zombieRepository = zombieRepository;
        }

        public async Task<int> Handle(CreateZombieCommand request, CancellationToken cancellationToken)
        {
            var zombie = new Zombie
            {
                Tipo = request.Tipo,
                TiempoDisparo = request.TiempoDisparo,
                BalasNecesarias = request.BalasNecesarias,
                PuntajeBase = request.PuntajeBase,
                NivelAmenaza = request.NivelAmenaza,
                Activo = request.Activo
            };

            await _zombieRepository.AddAsync(zombie, cancellationToken);
            await _zombieRepository.SaveChangesAsync(cancellationToken);

            return zombie.Id;
        }
    }
}

using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZombieDefenseSystem.Application.Abstractions.Persistence;

namespace ZombieDefenseSystem.Application.Features.Zombies.Commands.UpdateZombie
{
    public class UpdateZombieCommandHandler : IRequestHandler<UpdateZombieCommand, bool>
    {
        private readonly IZombieRepository _zombieRepository;

        public UpdateZombieCommandHandler(IZombieRepository zombieRepository)
        {
            _zombieRepository = zombieRepository;
        }

        public async Task<bool> Handle(UpdateZombieCommand request, CancellationToken cancellationToken)
        {
            var zombie = await _zombieRepository.GetByIdAsync(request.Id, cancellationToken);

            if (zombie is null)
                return false;

            zombie.Tipo = request.Tipo;
            zombie.TiempoDisparo = request.TiempoDisparo;
            zombie.BalasNecesarias = request.BalasNecesarias;
            zombie.PuntajeBase = request.PuntajeBase;
            zombie.NivelAmenaza = request.NivelAmenaza;
            zombie.Activo = request.Activo;

            _zombieRepository.Update(zombie);
            await _zombieRepository.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}

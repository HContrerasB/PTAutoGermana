using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZombieDefenseSystem.Application.Abstractions.Persistence;

namespace ZombieDefenseSystem.Application.Features.Zombies.Commands.DeleteZombie
{
    public class DeleteZombieCommandHandler : IRequestHandler<DeleteZombieCommand, bool>
    {
        private readonly IZombieRepository _zombieRepository;

        public DeleteZombieCommandHandler(IZombieRepository zombieRepository)
        {
            _zombieRepository = zombieRepository;
        }

        public async Task<bool> Handle(DeleteZombieCommand request, CancellationToken cancellationToken)
        {
            var zombie = await _zombieRepository.GetByIdAsync(request.Id, cancellationToken);

            if (zombie is null)
                return false;

            _zombieRepository.Delete(zombie);
            await _zombieRepository.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}

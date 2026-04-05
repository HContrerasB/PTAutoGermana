using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZombieDefenseSystem.Application.Features.Zombies.Commands.DeleteZombie
{
    public class DeleteZombieCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}

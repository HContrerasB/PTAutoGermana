using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZombieDefenseSystem.Application.Features.Zombies.Queries.GetAllZombies
{
    public class GetAllZombiesQuery : IRequest<List<ZombieResponse>>
    {
    }
}

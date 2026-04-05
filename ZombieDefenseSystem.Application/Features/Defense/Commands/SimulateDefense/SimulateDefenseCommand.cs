using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZombieDefenseSystem.Application.Features.Defense.Commands.SimulateDefense
{
    public class SimulateDefenseCommand : IRequest<SimulateDefenseCommandResponse>
    {
        public int Bullets { get; set; }
        public int SecondsAvailable { get; set; }
    }
}

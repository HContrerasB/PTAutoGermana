using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZombieDefenseSystem.Application.Features.Zombies.Commands.UpdateZombie
{
    public class UpdateZombieCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public int TiempoDisparo { get; set; }
        public int BalasNecesarias { get; set; }
        public int PuntajeBase { get; set; }
        public int NivelAmenaza { get; set; }
        public bool Activo { get; set; }
    }
}

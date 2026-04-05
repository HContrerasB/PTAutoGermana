using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZombieDefenseSystem.Application.Features.Zombies.Commands.CreateZombie
{
    public class CreateZombieCommandValidator : AbstractValidator<CreateZombieCommand>
    {
        public CreateZombieCommandValidator()
        {
            RuleFor(x => x.Tipo)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.TiempoDisparo)
                .GreaterThan(0);

            RuleFor(x => x.BalasNecesarias)
                .GreaterThan(0);

            RuleFor(x => x.PuntajeBase)
                .GreaterThan(0);

            RuleFor(x => x.NivelAmenaza)
                .GreaterThan(0);
        }
    }
}

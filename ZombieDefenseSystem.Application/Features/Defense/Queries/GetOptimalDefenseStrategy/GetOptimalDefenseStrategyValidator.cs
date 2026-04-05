using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZombieDefenseSystem.Application.Features.Defense.Queries.GetOptimalDefenseStrategy
{
    public class GetOptimalDefenseStrategyValidator : AbstractValidator<GetOptimalDefenseStrategyQuery>
    {
        public GetOptimalDefenseStrategyValidator()
        {
            RuleFor(x => x.Bullets)
                .GreaterThan(0)
                .WithMessage("La cantidad de balas debe ser mayor que cero.");

            RuleFor(x => x.SecondsAvailable)
                .GreaterThan(0)
                .WithMessage("La cantidad de segundos disponibles debe ser mayor que cero.");
        }
    }
}

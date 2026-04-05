using MediatR;
using Microsoft.AspNetCore.Mvc;
using ZombieDefenseSystem.Application.Features.Defense.Commands.SimulateDefense;
using ZombieDefenseSystem.Application.Features.Defense.Queries.GetOptimalDefenseStrategy;

namespace ZombieDefenseSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DefenseController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DefenseController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("optimal-strategy")]
        [ProducesResponseType(typeof(OptimalDefenseStrategyResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetOptimalStrategy(
            [FromQuery] int bullets,
            [FromQuery] int secondsAvailable,
            CancellationToken cancellationToken)
        {
            if (bullets <= 0 || secondsAvailable <= 0)
            {
                return BadRequest(new
                {
                    message = "Los parámetros bullets y secondsAvailable deben ser mayores que cero."
                });
            }

            var query = new GetOptimalDefenseStrategyQuery
            {
                Bullets = bullets,
                SecondsAvailable = secondsAvailable
            };

            var result = await _mediator.Send(query, cancellationToken);

            return Ok(result);
        }
        [HttpPost("simulate")]
        [ProducesResponseType(typeof(SimulateDefenseCommandResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Simulate(
       [FromBody] SimulateDefenseCommand command,
       CancellationToken cancellationToken)
        {
            if (command.Bullets <= 0 || command.SecondsAvailable <= 0)
            {
                return BadRequest(new
                {
                    message = "Los parámetros bullets y secondsAvailable deben ser mayores que cero."
                });
            }

            var result = await _mediator.Send(command, cancellationToken);

            return Ok(result);
        }

        [HttpGet("ranking")]
        [ProducesResponseType(typeof(List<SimulationRankingItemResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetRanking(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetSimulationRankingQuery(), cancellationToken);
            return Ok(result);
        }
    }
}


using MediatR;
using Microsoft.AspNetCore.Mvc;
using ZombieDefenseSystem.Application.Features.Zombies.Commands.CreateZombie;
using ZombieDefenseSystem.Application.Features.Zombies.Commands.DeleteZombie;
using ZombieDefenseSystem.Application.Features.Zombies.Commands.UpdateZombie;
using ZombieDefenseSystem.Application.Features.Zombies.Queries.GetAllZombies;

namespace ZombieDefenseSystem.Api.Controllers
{
    [ApiController]
[Route("api/[controller]")]
    public class ZombiesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ZombiesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetAllZombiesQuery(), cancellationToken);
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetZombieByIdQuery { Id = id }, cancellationToken);

            if (result is null)
                return NotFound(new { message = $"No se encontró el zombie con id {id}." });

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateZombieCommand command, CancellationToken cancellationToken)
        {
            var id = await _mediator.Send(command, cancellationToken);

            return CreatedAtAction(nameof(GetById), new { id }, new
            {
                message = "Zombie creado correctamente.",
                id
            });
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateZombieCommand command, CancellationToken cancellationToken)
        {
            if (id != command.Id)
            {
                return BadRequest(new { message = "El id de la ruta no coincide con el id del body." });
            }

            var updated = await _mediator.Send(command, cancellationToken);

            if (!updated)
                return NotFound(new { message = $"No se encontró el zombie con id {id}." });

            return Ok(new { message = "Zombie actualizado correctamente." });
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var deleted = await _mediator.Send(new DeleteZombieCommand { Id = id }, cancellationToken);

            if (!deleted)
                return NotFound(new { message = $"No se encontró el zombie con id {id}." });

            return Ok(new { message = "Zombie eliminado correctamente." });
        }
    }
}

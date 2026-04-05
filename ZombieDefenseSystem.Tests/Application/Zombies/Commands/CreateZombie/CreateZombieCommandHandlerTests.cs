using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZombieDefenseSystem.Application.Abstractions.Persistence;
using ZombieDefenseSystem.Application.Features.Zombies.Commands.CreateZombie;
using ZombieDefenseSystem.Domain.Entities;

namespace ZombieDefenseSystem.Tests.Application.Zombies.Commands.CreateZombie
{
    public class CreateZombieCommandHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldCreateZombie_AndReturnId()
        {
            // Arrange
            var zombieRepositoryMock = new Mock<IZombieRepository>();

            zombieRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<Zombie>(), It.IsAny<CancellationToken>()))
                .Callback<Zombie, CancellationToken>((zombie, _) => zombie.Id = 10)
                .Returns(Task.CompletedTask);

            var handler = new CreateZombieCommandHandler(zombieRepositoryMock.Object);

            var command = new CreateZombieCommand
            {
                Tipo = "Runner",
                TiempoDisparo = 3,
                BalasNecesarias = 2,
                PuntajeBase = 15,
                NivelAmenaza = 3,
                Activo = true
            };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(10);
            zombieRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Zombie>(), It.IsAny<CancellationToken>()), Times.Once);
            zombieRepositoryMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}

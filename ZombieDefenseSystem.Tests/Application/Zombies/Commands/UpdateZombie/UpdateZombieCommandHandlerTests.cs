using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZombieDefenseSystem.Application.Abstractions.Persistence;
using ZombieDefenseSystem.Application.Features.Zombies.Commands.UpdateZombie;
using ZombieDefenseSystem.Domain.Entities;

namespace ZombieDefenseSystem.Tests.Application.Zombies.Commands.UpdateZombie
{
    public class UpdateZombieCommandHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldUpdateZombie_WhenExists()
        {
            // Arrange
            var zombie = new Zombie
            {
                Id = 1,
                Tipo = "Walker",
                TiempoDisparo = 2,
                BalasNecesarias = 2,
                PuntajeBase = 10,
                NivelAmenaza = 1,
                Activo = true
            };

            var zombieRepositoryMock = new Mock<IZombieRepository>();
            zombieRepositoryMock
                .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(zombie);

            var handler = new UpdateZombieCommandHandler(zombieRepositoryMock.Object);

            var command = new UpdateZombieCommand
            {
                Id = 1,
                Tipo = "Walker Elite",
                TiempoDisparo = 3,
                BalasNecesarias = 3,
                PuntajeBase = 20,
                NivelAmenaza = 2,
                Activo = true
            };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeTrue();
            zombie.Tipo.Should().Be("Walker Elite");
            zombieRepositoryMock.Verify(x => x.Update(It.IsAny<Zombie>()), Times.Once);
            zombieRepositoryMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFalse_WhenZombieDoesNotExist()
        {
            // Arrange
            var zombieRepositoryMock = new Mock<IZombieRepository>();
            zombieRepositoryMock
                .Setup(x => x.GetByIdAsync(999, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Zombie?)null);

            var handler = new UpdateZombieCommandHandler(zombieRepositoryMock.Object);

            var command = new UpdateZombieCommand
            {
                Id = 999,
                Tipo = "Ghost",
                TiempoDisparo = 1,
                BalasNecesarias = 1,
                PuntajeBase = 1,
                NivelAmenaza = 1,
                Activo = true
            };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeFalse();
        }
    }
}

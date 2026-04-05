using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZombieDefenseSystem.Application.Abstractions.Persistence;
using ZombieDefenseSystem.Application.Features.Zombies.Commands.DeleteZombie;
using ZombieDefenseSystem.Domain.Entities;

namespace ZombieDefenseSystem.Tests.Application.Zombies.Commands.DeleteZombie
{
    public class DeleteZombieCommandHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldDeleteZombie_WhenExists()
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

            var handler = new DeleteZombieCommandHandler(zombieRepositoryMock.Object);

            // Act
            var result = await handler.Handle(new DeleteZombieCommand { Id = 1 }, CancellationToken.None);

            // Assert
            result.Should().BeTrue();
            zombieRepositoryMock.Verify(x => x.Delete(It.IsAny<Zombie>()), Times.Once);
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

            var handler = new DeleteZombieCommandHandler(zombieRepositoryMock.Object);

            // Act
            var result = await handler.Handle(new DeleteZombieCommand { Id = 999 }, CancellationToken.None);

            // Assert
            result.Should().BeFalse();
        }
    }
}

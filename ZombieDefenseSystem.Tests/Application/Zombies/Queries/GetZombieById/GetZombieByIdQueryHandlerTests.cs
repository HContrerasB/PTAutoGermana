using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZombieDefenseSystem.Application.Abstractions.Persistence;
using ZombieDefenseSystem.Application.Features.Zombies.Queries.GetAllZombies;
using ZombieDefenseSystem.Domain.Entities;

namespace ZombieDefenseSystem.Tests.Application.Zombies.Queries.GetZombieById
{
    public class GetZombieByIdQueryHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldReturnZombie_WhenExists()
        {
            // Arrange
            var zombieRepositoryMock = new Mock<IZombieRepository>();

            zombieRepositoryMock
                .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Zombie
                {
                    Id = 1,
                    Tipo = "Walker",
                    TiempoDisparo = 2,
                    BalasNecesarias = 2,
                    PuntajeBase = 10,
                    NivelAmenaza = 1,
                    Activo = true
                });

            var handler = new GetZombieByIdQueryHandler(zombieRepositoryMock.Object);

            // Act
            var result = await handler.Handle(new GetZombieByIdQuery { Id = 1 }, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be(1);
            result.Tipo.Should().Be("Walker");
        }

        [Fact]
        public async Task Handle_ShouldReturnNull_WhenZombieDoesNotExist()
        {
            // Arrange
            var zombieRepositoryMock = new Mock<IZombieRepository>();

            zombieRepositoryMock
                .Setup(x => x.GetByIdAsync(999, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Zombie?)null);

            var handler = new GetZombieByIdQueryHandler(zombieRepositoryMock.Object);

            // Act
            var result = await handler.Handle(new GetZombieByIdQuery { Id = 999 }, CancellationToken.None);

            // Assert
            result.Should().BeNull();
        }
    }
}

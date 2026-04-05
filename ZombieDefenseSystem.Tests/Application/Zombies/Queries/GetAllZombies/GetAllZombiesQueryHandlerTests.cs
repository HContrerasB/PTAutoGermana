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

namespace ZombieDefenseSystem.Tests.Application.Zombies.Queries.GetAllZombies
{
    public class GetAllZombiesQueryHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldReturnAllZombies()
        {
            // Arrange
            var zombieRepositoryMock = new Mock<IZombieRepository>();

            zombieRepositoryMock
                .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Zombie>
                {
                new() { Id = 1, Tipo = "Walker", TiempoDisparo = 2, BalasNecesarias = 2, PuntajeBase = 10, NivelAmenaza = 1, Activo = true },
                new() { Id = 2, Tipo = "Tank", TiempoDisparo = 6, BalasNecesarias = 5, PuntajeBase = 40, NivelAmenaza = 5, Activo = true }
                });

            var handler = new GetAllZombiesQueryHandler(zombieRepositoryMock.Object);

            // Act
            var result = await handler.Handle(new GetAllZombiesQuery(), CancellationToken.None);

            // Assert
            result.Should().HaveCount(2);
            result[0].Id.Should().Be(1);
        }
    }
}

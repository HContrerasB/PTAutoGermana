using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZombieDefenseSystem.Application.Abstractions.Persistence;
using ZombieDefenseSystem.Application.Features.Defense.Queries.GetOptimalDefenseStrategy;

namespace ZombieDefenseSystem.Tests.Application.Defense.Queries.GetSimulationRanking
{
    public class GetSimulationRankingQueryHandlerTests
    {
        private readonly Mock<ISimulationRepository> _simulationRepositoryMock = new();

        [Fact]
        public async Task Handle_ShouldReturnRankingList()
        {
            // Arrange
            var ranking = new List<SimulationRankingItemResponse>
        {
            new()
            {
                SimulationId = 1,
                Fecha = DateTime.UtcNow,
                TiempoDisponible = 20,
                BalasDisponibles = 15,
                PuntajeTotal = 100,
                TiempoUsado = 10,
                BalasUsadas = 8
            },
            new()
            {
                SimulationId = 2,
                Fecha = DateTime.UtcNow,
                TiempoDisponible = 25,
                BalasDisponibles = 18,
                PuntajeTotal = 80,
                TiempoUsado = 12,
                BalasUsadas = 9
            }
        };

            _simulationRepositoryMock
                .Setup(x => x.GetRankingAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(ranking);

            var handler = new GetSimulationRankingQueryHandler(_simulationRepositoryMock.Object);

            // Act
            var result = await handler.Handle(new GetSimulationRankingQuery(), CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result[0].SimulationId.Should().Be(1);

            _simulationRepositoryMock.Verify(x => x.GetRankingAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenNoDataExists()
        {
            // Arrange
            _simulationRepositoryMock
                .Setup(x => x.GetRankingAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<SimulationRankingItemResponse>());

            var handler = new GetSimulationRankingQueryHandler(_simulationRepositoryMock.Object);

            // Act
            var result = await handler.Handle(new GetSimulationRankingQuery(), CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }
    }
}

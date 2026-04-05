using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using ZombieDefenseSystem.Application.Abstractions.Persistence;
using ZombieDefenseSystem.Application.Abstractions.Services;
using ZombieDefenseSystem.Application.Features.Defense.Commands.SimulateDefense;
using ZombieDefenseSystem.Application.Features.Defense.Queries.GetOptimalDefenseStrategy;
using ZombieDefenseSystem.Domain.Entities;

namespace ZombieDefenseSystem.Tests.Application.Defense.Commands.SimulateDefense
{
    public class SimulateDefenseCommandHandlerTests
    {
        private readonly Mock<IZombieRepository> _zombieRepositoryMock = new();
        private readonly Mock<ISimulationRepository> _simulationRepositoryMock = new();
        private readonly Mock<IEliminatedZombieRepository> _eliminatedZombieRepositoryMock = new();
        private readonly Mock<IDefenseStrategyOptimizer> _optimizerMock = new();

        [Fact]
        public async Task Handle_ShouldCreateSimulation_AndReturnResponse()
        {
            // Arrange
            var zombies = new List<Zombie>
        {
            new()
            {
                Id = 1,
                Tipo = "Walker",
                BalasNecesarias = 2,
                TiempoDisparo = 2,
                PuntajeBase = 10,
                NivelAmenaza = 1,
                Activo = true
            }
        };

            var strategy = new OptimalDefenseStrategyResponse
            {
                BulletsAvailable = 10,
                SecondsAvailable = 10,
                BulletsUsed = 2,
                SecondsUsed = 2,
                TotalScore = 10,
                Strategy = new List<OptimalDefenseZombieItem>
            {
                new()
                {
                    ZombieId = 1,
                    Tipo = "Walker",
                    BalasNecesarias = 2,
                    TiempoDisparo = 2,
                    PuntajeBase = 10,
                    NivelAmenaza = 1
                }
            }
            };

            _zombieRepositoryMock
                .Setup(x => x.GetActiveAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(zombies);

            _optimizerMock
                .Setup(x => x.Calculate(zombies, 10, 10))
                .Returns(strategy);

            _simulationRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<Simulation>(), It.IsAny<CancellationToken>()))
                .Callback<Simulation, CancellationToken>((simulation, _) => simulation.Id = 99)
                .Returns(Task.CompletedTask);

            var handler = new SimulateDefenseCommandHandler(
                _zombieRepositoryMock.Object,
                _simulationRepositoryMock.Object,
                _eliminatedZombieRepositoryMock.Object,
                _optimizerMock.Object);

            var command = new SimulateDefenseCommand
            {
                Bullets = 10,
                SecondsAvailable = 10
            };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.SimulationId.Should().Be(99);
            result.StrategyResult.TotalScore.Should().Be(10);

            _zombieRepositoryMock.Verify(x => x.GetActiveAsync(It.IsAny<CancellationToken>()), Times.Once);
            _optimizerMock.Verify(x => x.Calculate(It.IsAny<IReadOnlyCollection<Zombie>>(), 10, 10), Times.Once);
            _simulationRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Simulation>(), It.IsAny<CancellationToken>()), Times.Once);
            _simulationRepositoryMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Exactly(2));
            _eliminatedZombieRepositoryMock.Verify(x => x.AddRangeAsync(It.IsAny<IEnumerable<EliminatedZombie>>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldNotInsertEliminatedZombies_WhenStrategyIsEmpty()
        {
            // Arrange
            var zombies = new List<Zombie>();

            var strategy = new OptimalDefenseStrategyResponse
            {
                BulletsAvailable = 10,
                SecondsAvailable = 10,
                BulletsUsed = 0,
                SecondsUsed = 0,
                TotalScore = 0,
                Strategy = new List<OptimalDefenseZombieItem>()
            };

            _zombieRepositoryMock
                .Setup(x => x.GetActiveAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(zombies);

            _optimizerMock
                .Setup(x => x.Calculate(zombies, 10, 10))
                .Returns(strategy);

            _simulationRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<Simulation>(), It.IsAny<CancellationToken>()))
                .Callback<Simulation, CancellationToken>((simulation, _) => simulation.Id = 50)
                .Returns(Task.CompletedTask);

            var handler = new SimulateDefenseCommandHandler(
                _zombieRepositoryMock.Object,
                _simulationRepositoryMock.Object,
                _eliminatedZombieRepositoryMock.Object,
                _optimizerMock.Object);

            // Act
            var result = await handler.Handle(new SimulateDefenseCommand
            {
                Bullets = 10,
                SecondsAvailable = 10
            }, CancellationToken.None);

            // Assert
            result.SimulationId.Should().Be(50);
            result.StrategyResult.TotalScore.Should().Be(0);

            _eliminatedZombieRepositoryMock.Verify(
                x => x.AddRangeAsync(It.IsAny<IEnumerable<EliminatedZombie>>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }
    }
}

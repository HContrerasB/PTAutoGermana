using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using ZombieDefenseSystem.Domain.Entities;
using ZombieDefenseSystem.Infrastructure.Services;
using Xunit;

namespace ZombieDefenseSystem.Tests.Application.Defense
{
    public class DefenseStrategyOptimizerTests
    {
        private readonly DefenseStrategyOptimizer _optimizer;

        public DefenseStrategyOptimizerTests()
        {
            _optimizer = new DefenseStrategyOptimizer();
        }

        [Fact]
        public void Calculate_ShouldReturnEmptyStrategy_WhenZombieListIsEmpty()
        {
            // Arrange
            var zombies = new List<Zombie>();

            // Act
            var result = _optimizer.Calculate(zombies, bulletsAvailable: 10, secondsAvailable: 20);

            // Assert
            result.Should().NotBeNull();
            result.TotalScore.Should().Be(0);
            result.BulletsUsed.Should().Be(0);
            result.SecondsUsed.Should().Be(0);
            result.Strategy.Should().BeEmpty();
        }

        [Fact]
        public void Calculate_ShouldReturnEmptyStrategy_WhenBulletsAreZero()
        {
            // Arrange
            var zombies = new List<Zombie>
        {
            new()
            {
                Id = 1,
                Tipo = "Walker",
                BalasNecesarias = 2,
                TiempoDisparo = 3,
                PuntajeBase = 10,
                NivelAmenaza = 1,
                Activo = true
            }
        };

            // Act
            var result = _optimizer.Calculate(zombies, bulletsAvailable: 0, secondsAvailable: 20);

            // Assert
            result.TotalScore.Should().Be(0);
            result.Strategy.Should().BeEmpty();
        }

        [Fact]
        public void Calculate_ShouldReturnEmptyStrategy_WhenSecondsAreZero()
        {
            // Arrange
            var zombies = new List<Zombie>
        {
            new()
            {
                Id = 1,
                Tipo = "Walker",
                BalasNecesarias = 2,
                TiempoDisparo = 3,
                PuntajeBase = 10,
                NivelAmenaza = 1,
                Activo = true
            }
        };

            // Act
            var result = _optimizer.Calculate(zombies, bulletsAvailable: 10, secondsAvailable: 0);

            // Assert
            result.TotalScore.Should().Be(0);
            result.Strategy.Should().BeEmpty();
        }

        [Fact]
        public void Calculate_ShouldSelectSingleZombie_WhenOnlyOneFits()
        {
            // Arrange
            var zombies = new List<Zombie>
        {
            new()
            {
                Id = 1,
                Tipo = "Walker",
                BalasNecesarias = 2,
                TiempoDisparo = 3,
                PuntajeBase = 10,
                NivelAmenaza = 1,
                Activo = true
            },
            new()
            {
                Id = 2,
                Tipo = "Tank",
                BalasNecesarias = 20,
                TiempoDisparo = 30,
                PuntajeBase = 100,
                NivelAmenaza = 5,
                Activo = true
            }
        };

            // Act
            var result = _optimizer.Calculate(zombies, bulletsAvailable: 5, secondsAvailable: 5);

            // Assert
            result.TotalScore.Should().Be(10);
            result.BulletsUsed.Should().Be(2);
            result.SecondsUsed.Should().Be(3);
            result.Strategy.Should().HaveCount(1);
            result.Strategy[0].ZombieId.Should().Be(1);
        }

        [Fact]
        public void Calculate_ShouldChooseBestCombination_ByMaximumScore()
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
                PuntajeBase = 6,
                NivelAmenaza = 1,
                Activo = true
            },
            new()
            {
                Id = 2,
                Tipo = "Runner",
                BalasNecesarias = 3,
                TiempoDisparo = 3,
                PuntajeBase = 10,
                NivelAmenaza = 3,
                Activo = true
            },
            new()
            {
                Id = 3,
                Tipo = "Brute",
                BalasNecesarias = 4,
                TiempoDisparo = 5,
                PuntajeBase = 12,
                NivelAmenaza = 4,
                Activo = true
            }
        };

            // Recursos: 5 balas y 5 segundos
            // Mejor combinación: Walker + Runner = 16 puntos
            // Brute solo = 12 puntos

            // Act
            var result = _optimizer.Calculate(zombies, bulletsAvailable: 5, secondsAvailable: 5);

            // Assert
            result.TotalScore.Should().Be(16);
            result.BulletsUsed.Should().Be(5);
            result.SecondsUsed.Should().Be(5);
            result.Strategy.Should().HaveCount(2);
            result.Strategy.Select(x => x.ZombieId).Should().BeEquivalentTo(new[] { 1, 2 });
        }

        [Fact]
        public void Calculate_ShouldNeverExceedAvailableResources()
        {
            // Arrange
            var zombies = new List<Zombie>
        {
            new()
            {
                Id = 1,
                Tipo = "Walker",
                BalasNecesarias = 2,
                TiempoDisparo = 1,
                PuntajeBase = 5,
                NivelAmenaza = 1,
                Activo = true
            },
            new()
            {
                Id = 2,
                Tipo = "Runner",
                BalasNecesarias = 3,
                TiempoDisparo = 4,
                PuntajeBase = 9,
                NivelAmenaza = 3,
                Activo = true
            },
            new()
            {
                Id = 3,
                Tipo = "Tank",
                BalasNecesarias = 10,
                TiempoDisparo = 10,
                PuntajeBase = 50,
                NivelAmenaza = 5,
                Activo = true
            }
        };

            // Act
            var result = _optimizer.Calculate(zombies, bulletsAvailable: 5, secondsAvailable: 5);

            // Assert
            result.BulletsUsed.Should().BeLessThanOrEqualTo(5);
            result.SecondsUsed.Should().BeLessThanOrEqualTo(5);
        }

    

        
    }
}

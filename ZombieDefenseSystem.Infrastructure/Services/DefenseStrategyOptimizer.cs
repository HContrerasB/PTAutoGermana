using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZombieDefenseSystem.Application.Abstractions.Services;
using ZombieDefenseSystem.Application.Features.Defense.Queries.GetOptimalDefenseStrategy;
using ZombieDefenseSystem.Domain.Entities;

namespace ZombieDefenseSystem.Infrastructure.Services
{
    public class DefenseStrategyOptimizer : IDefenseStrategyOptimizer
    {
        public OptimalDefenseStrategyResponse Calculate(
            IReadOnlyCollection<Zombie> zombies,
            int bulletsAvailable,
            int secondsAvailable)
        {
            var zombieList = zombies
                .Where(z => z.Activo)
                .ToList();

            if (!zombieList.Any() || bulletsAvailable <= 0 || secondsAvailable <= 0)
            {
                return new OptimalDefenseStrategyResponse
                {
                    BulletsAvailable = bulletsAvailable,
                    SecondsAvailable = secondsAvailable,
                    BulletsUsed = 0,
                    SecondsUsed = 0,
                    TotalScore = 0,
                    Strategy = new List<OptimalDefenseZombieItem>()
                };
            }

            // dp[b, s] = mejor puntaje alcanzable usando b balas y s segundos
            var dp = new int[bulletsAvailable + 1, secondsAvailable + 1];

            // choice[b, s] = índice del zombie elegido para llegar al mejor score en [b,s]
            var choice = new int[bulletsAvailable + 1, secondsAvailable + 1];

            for (int b = 0; b <= bulletsAvailable; b++)
            {
                for (int s = 0; s <= secondsAvailable; s++)
                {
                    choice[b, s] = -1;
                }
            }

            // Unbounded knapsack con dos restricciones:
            // recorremos hacia adelante para permitir reutilizar el mismo tipo
            for (int b = 0; b <= bulletsAvailable; b++)
            {
                for (int s = 0; s <= secondsAvailable; s++)
                {
                    for (int i = 0; i < zombieList.Count; i++)
                    {
                        var zombie = zombieList[i];

                        if (b >= zombie.BalasNecesarias && s >= zombie.TiempoDisparo)
                        {
                            var candidateScore =
                                dp[b - zombie.BalasNecesarias, s - zombie.TiempoDisparo] + zombie.PuntajeBase;

                            if (candidateScore > dp[b, s])
                            {
                                dp[b, s] = candidateScore;
                                choice[b, s] = i;
                            }
                        }
                    }
                }
            }

            // Buscar el mejor puntaje posible dentro del límite total
            var bestScore = 0;
            var bestBullets = 0;
            var bestSeconds = 0;

            for (int b = 0; b <= bulletsAvailable; b++)
            {
                for (int s = 0; s <= secondsAvailable; s++)
                {
                    if (dp[b, s] > bestScore)
                    {
                        bestScore = dp[b, s];
                        bestBullets = b;
                        bestSeconds = s;
                    }
                }
            }

            // Reconstrucción
            var selectedCounts = new Dictionary<int, int>();
            var currentBullets = bestBullets;
            var currentSeconds = bestSeconds;

            while (currentBullets > 0 && currentSeconds > 0)
            {
                var selectedIndex = choice[currentBullets, currentSeconds];

                if (selectedIndex == -1)
                    break;

                var selectedZombie = zombieList[selectedIndex];

                if (!selectedCounts.ContainsKey(selectedZombie.Id))
                    selectedCounts[selectedZombie.Id] = 0;

                selectedCounts[selectedZombie.Id]++;

                currentBullets -= selectedZombie.BalasNecesarias;
                currentSeconds -= selectedZombie.TiempoDisparo;
            }

            var selectedItems = zombieList
                .Where(z => selectedCounts.ContainsKey(z.Id))
                .Select(z =>
                {
                    var cantidad = selectedCounts[z.Id];
                    return new OptimalDefenseZombieItem
                    {
                        ZombieId = z.Id,
                        Tipo = z.Tipo,
                        TiempoDisparo = z.TiempoDisparo,
                        BalasNecesarias = z.BalasNecesarias,
                        PuntajeBase = z.PuntajeBase,
                        NivelAmenaza = z.NivelAmenaza,
                        Cantidad = cantidad,
                        PuntajeTotal = cantidad * z.PuntajeBase,
                        BalasTotales = cantidad * z.BalasNecesarias,
                        TiempoTotal = cantidad * z.TiempoDisparo
                    };
                })
                .OrderByDescending(x => x.NivelAmenaza)
                .ThenByDescending(x => x.PuntajeTotal)
                .ToList();

            return new OptimalDefenseStrategyResponse
            {
                BulletsAvailable = bulletsAvailable,
                SecondsAvailable = secondsAvailable,
                BulletsUsed = selectedItems.Sum(x => x.BalasTotales),
                SecondsUsed = selectedItems.Sum(x => x.TiempoTotal),
                TotalScore = selectedItems.Sum(x => x.PuntajeTotal),
                Strategy = selectedItems
            };
        }
    }
}

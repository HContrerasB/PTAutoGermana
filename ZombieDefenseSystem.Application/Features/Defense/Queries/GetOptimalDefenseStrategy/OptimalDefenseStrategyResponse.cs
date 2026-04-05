using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZombieDefenseSystem.Application.Features.Defense.Queries.GetOptimalDefenseStrategy
{
    public class OptimalDefenseStrategyResponse
    {
        public int BulletsAvailable { get; set; }
        public int SecondsAvailable { get; set; }
        public int BulletsUsed { get; set; }
        public int SecondsUsed { get; set; }
        public int TotalScore { get; set; }
        public List<OptimalDefenseZombieItem> Strategy { get; set; } = new();
    }

    public class OptimalDefenseZombieItem
    {
        public int ZombieId { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public int TiempoDisparo { get; set; }
        public int BalasNecesarias { get; set; }
        public int PuntajeBase { get; set; }
        public int NivelAmenaza { get; set; }

        // NUEVO
        public int Cantidad { get; set; }
        public int PuntajeTotal { get; set; }
        public int BalasTotales { get; set; }
        public int TiempoTotal { get; set; }
    }
}

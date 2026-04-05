using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZombieDefenseSystem.Application.Features.Defense.Queries.GetOptimalDefenseStrategy
{
    public class SimulationRankingItemResponse
    {
        public int SimulationId { get; set; }
        public DateTime Fecha { get; set; }
        public int TiempoDisponible { get; set; }
        public int BalasDisponibles { get; set; }
        public int PuntajeTotal { get; set; }
        public int TiempoUsado { get; set; }
        public int BalasUsadas { get; set; }
        public int TotalZombiesEliminados { get; set; }
    }
}

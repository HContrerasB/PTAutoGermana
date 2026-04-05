using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZombieDefenseSystem.Domain.Entities
{
    public class Simulation
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public int TiempoDisponible { get; set; }
        public int BalasDisponibles { get; set; }
        public int PuntajeTotal { get; set; }
        public int TiempoUsado { get; set; }
        public int BalasUsadas { get; set; }
        public int TotalZombiesEliminados { get; set; }

        public ICollection<EliminatedZombie> Eliminados { get; set; } = new List<EliminatedZombie>();
    }
}

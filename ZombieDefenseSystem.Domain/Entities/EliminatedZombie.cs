using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZombieDefenseSystem.Domain.Entities
{
    public class EliminatedZombie
    {
        public int Id { get; set; }
        public int ZombieId { get; set; }
        public int SimulacionId { get; set; }
        public int CantidadEliminados { get; set; }
        public int PuntosObtenidos { get; set; }
        public DateTime Timestamp { get; set; }

        public Zombie? Zombie { get; set; }
        public Simulation? Simulacion { get; set; }
    }
}

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZombieDefenseSystem.Domain.Entities;

namespace ZombieDefenseSystem.Infrastructure.Persistence.Configurations
{
    public class SimulationConfiguration : IEntityTypeConfiguration<Simulation>
    {
        public void Configure(EntityTypeBuilder<Simulation> builder)
        {
            builder.ToTable("Simulaciones");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Fecha)
                .IsRequired();

            builder.Property(x => x.TiempoDisponible)
                .IsRequired();

            builder.Property(x => x.BalasDisponibles)
                .IsRequired();

            builder.Property(x => x.PuntajeTotal)
                .IsRequired();

            builder.Property(x => x.TiempoUsado)
                .IsRequired();

            builder.Property(x => x.BalasUsadas)
                .IsRequired();

            builder.Property(x => x.TotalZombiesEliminados)
                .IsRequired();
        }
    }
}

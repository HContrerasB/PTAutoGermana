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
    public class EliminatedZombieConfiguration : IEntityTypeConfiguration<EliminatedZombie>
    {
        public void Configure(EntityTypeBuilder<EliminatedZombie> builder)
        {
            builder.ToTable("Eliminados");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.CantidadEliminados)
                .IsRequired();

            builder.Property(x => x.PuntosObtenidos)
                .IsRequired();

            builder.Property(x => x.Timestamp)
                .IsRequired();

            builder.HasOne(x => x.Zombie)
                .WithMany()
                .HasForeignKey(x => x.ZombieId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Simulacion)
                .WithMany(x => x.Eliminados)
                .HasForeignKey(x => x.SimulacionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

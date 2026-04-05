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
    public class ZombieConfiguration : IEntityTypeConfiguration<Zombie>
    {
        public void Configure(EntityTypeBuilder<Zombie> builder)
        {
            builder.ToTable("Zombies");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Tipo)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.TiempoDisparo)
                .IsRequired();

            builder.Property(x => x.BalasNecesarias)
                .IsRequired();

            builder.Property(x => x.PuntajeBase)
                .IsRequired();

            builder.Property(x => x.NivelAmenaza)
                .IsRequired();

            builder.Property(x => x.Activo)
                .HasDefaultValue(true);
        }
    }
}

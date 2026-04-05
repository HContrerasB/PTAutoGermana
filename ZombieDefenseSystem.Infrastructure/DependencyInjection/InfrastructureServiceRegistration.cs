using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZombieDefenseSystem.Application.Abstractions.Persistence;
using ZombieDefenseSystem.Application.Abstractions.Services;
using ZombieDefenseSystem.Infrastructure.Persistence.Repositories;
using ZombieDefenseSystem.Infrastructure.Persistence;
using ZombieDefenseSystem.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;


namespace ZombieDefenseSystem.Infrastructure.DependencyInjection
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(
        configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IZombieRepository, ZombieRepository>();
            services.AddScoped<IDefenseStrategyOptimizer, DefenseStrategyOptimizer>();

            services.AddScoped<ISimulationRepository, SimulationRepository>();
            services.AddScoped<IEliminatedZombieRepository, EliminatedZombieRepository>();

            return services;
        }
    }
}

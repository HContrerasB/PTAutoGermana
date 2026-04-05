using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ZombieDefenseSystem.Api.Configuration;
using ZombieDefenseSystem.Api.Middleware;
using ZombieDefenseSystem.Application.Features.Defense.Queries.GetOptimalDefenseStrategy;
using ZombieDefenseSystem.Infrastructure.DependencyInjection;
using ZombieDefenseSystem.Infrastructure.Persistence;


namespace ZombieDefenseSystem.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            // Configuración de API Key
            builder.Services.Configure<ApiKeyOptions>(
                builder.Configuration.GetSection(ApiKeyOptions.SectionName));
            //primero agregar esto
            // MediatR
            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(GetOptimalDefenseStrategyQuery).Assembly);
            });

            // Infrastructure
            builder.Services.AddInfrastructure(builder.Configuration);
            // CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AngularFront", policy =>
                {
                    policy
                        .WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
            //  luego agregar esto despues de crear el controlador
            // Swagger con soporte para API Key
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Zombie Defense System API",
                    Version = "v1",
                    Description = "API para calcular estrategias óptimas de defensa contra hordas de zombies."
                });

                options.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
                {
                    Description = "Ingrese la API Key usando el header X-API-KEY",
                    Type = SecuritySchemeType.ApiKey,
                    Name = "X-API-KEY",
                    In = ParameterLocation.Header,
                    Scheme = "ApiKeyScheme"
                });

                var securityScheme = new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "ApiKey"
                    }
                };

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { securityScheme, Array.Empty<string>() }
                });
            });

            var app = builder.Build();
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                db.Database.Migrate();

                if (db.Database.CanConnect())
                {
                    Console.WriteLine("SQLite conectado correctamente.");
                }
                else
                {
                    Console.WriteLine("No se pudo conectar a SQLite.");
                }
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors("AngularFront");

            app.UseMiddleware<ExceptionHandlingMiddleware>();
            // Middleware de API Key
            app.UseMiddleware<ApiKeyMiddleware>();
           

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZombieDefenseSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateSqlite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Simulaciones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Fecha = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TiempoDisponible = table.Column<int>(type: "INTEGER", nullable: false),
                    BalasDisponibles = table.Column<int>(type: "INTEGER", nullable: false),
                    PuntajeTotal = table.Column<int>(type: "INTEGER", nullable: false),
                    TiempoUsado = table.Column<int>(type: "INTEGER", nullable: false),
                    BalasUsadas = table.Column<int>(type: "INTEGER", nullable: false),
                    TotalZombiesEliminados = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Simulaciones", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Zombies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Tipo = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    TiempoDisparo = table.Column<int>(type: "INTEGER", nullable: false),
                    BalasNecesarias = table.Column<int>(type: "INTEGER", nullable: false),
                    PuntajeBase = table.Column<int>(type: "INTEGER", nullable: false),
                    NivelAmenaza = table.Column<int>(type: "INTEGER", nullable: false),
                    Activo = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zombies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Eliminados",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ZombieId = table.Column<int>(type: "INTEGER", nullable: false),
                    SimulacionId = table.Column<int>(type: "INTEGER", nullable: false),
                    CantidadEliminados = table.Column<int>(type: "INTEGER", nullable: false),
                    PuntosObtenidos = table.Column<int>(type: "INTEGER", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Eliminados", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Eliminados_Simulaciones_SimulacionId",
                        column: x => x.SimulacionId,
                        principalTable: "Simulaciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Eliminados_Zombies_ZombieId",
                        column: x => x.ZombieId,
                        principalTable: "Zombies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Eliminados_SimulacionId",
                table: "Eliminados",
                column: "SimulacionId");

            migrationBuilder.CreateIndex(
                name: "IX_Eliminados_ZombieId",
                table: "Eliminados",
                column: "ZombieId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Eliminados");

            migrationBuilder.DropTable(
                name: "Simulaciones");

            migrationBuilder.DropTable(
                name: "Zombies");
        }
    }
}

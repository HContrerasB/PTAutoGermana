/* =========================================================
   ZOMBIE HORDE DEFENSE SYSTEM
   Script de creación de base de datos y tablas
   ========================================================= */

-- Crear base de datos 
IF DB_ID('ZombieDefenseDb') IS NULL
BEGIN
    CREATE DATABASE ZombieDefenseDb;
END;
GO

USE ZombieDefenseDb;
GO

/* =========================================================
   Limpieza previa si necesitas re-ejecutar el script
   ========================================================= */
IF OBJECT_ID('dbo.EliminadosAudit', 'U') IS NOT NULL
    DROP TABLE dbo.EliminadosAudit;
GO

IF OBJECT_ID('dbo.Eliminados', 'U') IS NOT NULL
    DROP TABLE dbo.Eliminados;
GO

IF OBJECT_ID('dbo.Simulaciones', 'U') IS NOT NULL
    DROP TABLE dbo.Simulaciones;
GO

IF OBJECT_ID('dbo.Zombies', 'U') IS NOT NULL
    DROP TABLE dbo.Zombies;
GO

/* =========================================================
   Tabla: Zombies
   ========================================================= */
CREATE TABLE dbo.Zombies
(
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Tipo NVARCHAR(100) NOT NULL,
    TiempoDisparo INT NOT NULL,
    BalasNecesarias INT NOT NULL,
    PuntajeBase INT NOT NULL,
    NivelAmenaza INT NOT NULL,
    Activo BIT NOT NULL CONSTRAINT DF_Zombies_Activo DEFAULT (1),
    FechaCreacion DATETIME2 NOT NULL CONSTRAINT DF_Zombies_FechaCreacion DEFAULT (SYSUTCDATETIME()),

    CONSTRAINT UQ_Zombies_Tipo UNIQUE (Tipo),
    CONSTRAINT CK_Zombies_TiempoDisparo CHECK (TiempoDisparo > 0),
    CONSTRAINT CK_Zombies_BalasNecesarias CHECK (BalasNecesarias > 0),
    CONSTRAINT CK_Zombies_PuntajeBase CHECK (PuntajeBase > 0),
    CONSTRAINT CK_Zombies_NivelAmenaza CHECK (NivelAmenaza > 0)
);
GO

/* =========================================================
   Tabla: Simulaciones
   ========================================================= */
CREATE TABLE dbo.Simulaciones
(
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Fecha DATETIME2 NOT NULL CONSTRAINT DF_Simulaciones_Fecha DEFAULT (SYSUTCDATETIME()),
    TiempoDisponible INT NOT NULL,
    BalasDisponibles INT NOT NULL,
    PuntajeTotal INT NOT NULL CONSTRAINT DF_Simulaciones_PuntajeTotal DEFAULT (0),
    TiempoUsado INT NOT NULL CONSTRAINT DF_Simulaciones_TiempoUsado DEFAULT (0),
    BalasUsadas INT NOT NULL CONSTRAINT DF_Simulaciones_BalasUsadas DEFAULT (0),
    TotalZombiesEliminados INT NOT NULL CONSTRAINT DF_Simulaciones_TotalZombiesEliminados DEFAULT (0),
    Observacion NVARCHAR(300) NULL,

    CONSTRAINT CK_Simulaciones_TiempoDisponible CHECK (TiempoDisponible > 0),
    CONSTRAINT CK_Simulaciones_BalasDisponibles CHECK (BalasDisponibles > 0),
    CONSTRAINT CK_Simulaciones_PuntajeTotal CHECK (PuntajeTotal >= 0),
    CONSTRAINT CK_Simulaciones_TiempoUsado CHECK (TiempoUsado >= 0),
    CONSTRAINT CK_Simulaciones_BalasUsadas CHECK (BalasUsadas >= 0),
    CONSTRAINT CK_Simulaciones_TotalZombiesEliminados CHECK (TotalZombiesEliminados >= 0)
);
GO

/* =========================================================
   Tabla: Eliminados
   ========================================================= */
CREATE TABLE dbo.Eliminados
(
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    ZombieId INT NOT NULL,
    SimulacionId INT NOT NULL,
    CantidadEliminados INT NOT NULL CONSTRAINT DF_Eliminados_CantidadEliminados DEFAULT (1),
    PuntosObtenidos INT NOT NULL,
    [Timestamp] DATETIME2 NOT NULL CONSTRAINT DF_Eliminados_Timestamp DEFAULT (SYSUTCDATETIME()),

    CONSTRAINT FK_Eliminados_Zombies
        FOREIGN KEY (ZombieId) REFERENCES dbo.Zombies(Id),

    CONSTRAINT FK_Eliminados_Simulaciones
        FOREIGN KEY (SimulacionId) REFERENCES dbo.Simulaciones(Id),

    CONSTRAINT CK_Eliminados_CantidadEliminados CHECK (CantidadEliminados > 0),
    CONSTRAINT CK_Eliminados_PuntosObtenidos CHECK (PuntosObtenidos >= 0)
);
GO

/* =========================================================
   Índices recomendados
   ========================================================= */
CREATE INDEX IX_Eliminados_SimulacionId ON dbo.Eliminados(SimulacionId);
GO

CREATE INDEX IX_Eliminados_ZombieId ON dbo.Eliminados(ZombieId);
GO

CREATE INDEX IX_Eliminados_SimulacionId_ZombieId ON dbo.Eliminados(SimulacionId, ZombieId);
GO

CREATE INDEX IX_Simulaciones_Fecha ON dbo.Simulaciones(Fecha);
GO


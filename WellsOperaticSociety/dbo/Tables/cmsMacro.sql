CREATE TABLE [dbo].[cmsMacro] (
    [id]                     INT              IDENTITY (1, 1) NOT NULL,
    [macroUseInEditor]       BIT              CONSTRAINT [DF_cmsMacro_macroUseInEditor] DEFAULT ('0') NOT NULL,
    [macroRefreshRate]       INT              CONSTRAINT [DF_cmsMacro_macroRefreshRate] DEFAULT ('0') NOT NULL,
    [macroAlias]             NVARCHAR (255)   NOT NULL,
    [macroName]              NVARCHAR (255)   NULL,
    [macroScriptType]        NVARCHAR (255)   NULL,
    [macroScriptAssembly]    NVARCHAR (255)   NULL,
    [macroXSLT]              NVARCHAR (255)   NULL,
    [macroCacheByPage]       BIT              CONSTRAINT [DF_cmsMacro_macroCacheByPage] DEFAULT ('1') NOT NULL,
    [macroCachePersonalized] BIT              CONSTRAINT [DF_cmsMacro_macroCachePersonalized] DEFAULT ('0') NOT NULL,
    [macroDontRender]        BIT              CONSTRAINT [DF_cmsMacro_macroDontRender] DEFAULT ('0') NOT NULL,
    [macroPython]            NVARCHAR (255)   NULL,
    [uniqueId]               UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_cmsMacro] PRIMARY KEY CLUSTERED ([id] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_cmsMacro_UniqueId]
    ON [dbo].[cmsMacro]([uniqueId] ASC);


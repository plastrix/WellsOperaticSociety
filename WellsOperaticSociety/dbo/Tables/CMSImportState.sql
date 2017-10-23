CREATE TABLE [dbo].[CMSImportState] (
    [Id]               INT              IDENTITY (1, 1) NOT NULL,
    [UniqueIdentifier] UNIQUEIDENTIFIER NOT NULL,
    [Name]             NVARCHAR (250)   NOT NULL,
    [ImportState]      NVARCHAR (MAX)   NOT NULL,
    [Parent]           UNIQUEIDENTIFIER NULL,
    [ImportProvider]   NVARCHAR (250)   NULL,
    CONSTRAINT [PK_CMSImportState] PRIMARY KEY CLUSTERED ([Id] ASC)
);


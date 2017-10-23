CREATE TABLE [dbo].[cmsContentVersion] (
    [id]          INT              IDENTITY (1, 1) NOT NULL,
    [ContentId]   INT              NOT NULL,
    [VersionId]   UNIQUEIDENTIFIER NOT NULL,
    [VersionDate] DATETIME         CONSTRAINT [DF_cmsContentVersion_VersionDate] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_cmsContentVersion] PRIMARY KEY CLUSTERED ([id] ASC)
);


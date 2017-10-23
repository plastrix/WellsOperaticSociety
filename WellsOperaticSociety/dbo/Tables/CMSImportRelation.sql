CREATE TABLE [dbo].[CMSImportRelation] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [UmbracoID]      INT            NOT NULL,
    [DataSourceKey]  NVARCHAR (250) NOT NULL,
    [ImportProvider] NVARCHAR (250) NULL,
    [Updated]        SMALLDATETIME  NULL,
    [CustomId]       NVARCHAR (255) NULL,
    CONSTRAINT [PK_CMSImportRelation] PRIMARY KEY CLUSTERED ([Id] ASC)
);


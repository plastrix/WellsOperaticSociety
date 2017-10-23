CREATE TABLE [dbo].[CMSImportMediaRelation] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [UmbracoMediaId] INT            NOT NULL,
    [SourceUrl]      NVARCHAR (500) NOT NULL,
    [ByteSize]       INT            NULL,
    CONSTRAINT [PK_CMSImportMediaRelation] PRIMARY KEY CLUSTERED ([Id] ASC)
);


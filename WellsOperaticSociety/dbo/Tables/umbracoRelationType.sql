CREATE TABLE [dbo].[umbracoRelationType] (
    [id]               INT              IDENTITY (1, 1) NOT NULL,
    [dual]             BIT              NOT NULL,
    [parentObjectType] UNIQUEIDENTIFIER NOT NULL,
    [childObjectType]  UNIQUEIDENTIFIER NOT NULL,
    [name]             NVARCHAR (255)   NOT NULL,
    [alias]            NVARCHAR (100)   NULL,
    [typeUniqueId]     UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_umbracoRelationType] PRIMARY KEY CLUSTERED ([id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_umbracoRelationType_alias]
    ON [dbo].[umbracoRelationType]([alias] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_umbracoRelationType_name]
    ON [dbo].[umbracoRelationType]([name] ASC);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_umbracoRelationType_UniqueId]
    ON [dbo].[umbracoRelationType]([typeUniqueId] ASC);


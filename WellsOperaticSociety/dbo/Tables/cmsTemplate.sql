CREATE TABLE [dbo].[cmsTemplate] (
    [pk]     INT            IDENTITY (1, 1) NOT NULL,
    [nodeId] INT            NOT NULL,
    [alias]  NVARCHAR (100) NULL,
    [design] NTEXT          NOT NULL,
    CONSTRAINT [PK_cmsTemplate] PRIMARY KEY CLUSTERED ([pk] ASC),
    CONSTRAINT [FK_cmsTemplate_umbracoNode] FOREIGN KEY ([nodeId]) REFERENCES [dbo].[umbracoNode] ([id])
);


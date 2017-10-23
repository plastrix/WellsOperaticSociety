CREATE TABLE [dbo].[cmsDataType] (
    [pk]                  INT            IDENTITY (1, 1) NOT NULL,
    [nodeId]              INT            NOT NULL,
    [propertyEditorAlias] NVARCHAR (255) NOT NULL,
    [dbType]              NVARCHAR (50)  NOT NULL,
    CONSTRAINT [PK_cmsDataType] PRIMARY KEY CLUSTERED ([pk] ASC),
    CONSTRAINT [FK_cmsDataType_umbracoNode_id] FOREIGN KEY ([nodeId]) REFERENCES [dbo].[umbracoNode] ([id])
);


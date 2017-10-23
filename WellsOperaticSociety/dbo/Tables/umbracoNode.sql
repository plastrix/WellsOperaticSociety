CREATE TABLE [dbo].[umbracoNode] (
    [id]             INT              IDENTITY (1, 1) NOT NULL,
    [trashed]        BIT              CONSTRAINT [DF_umbracoNode_trashed] DEFAULT ('0') NOT NULL,
    [parentID]       INT              NOT NULL,
    [nodeUser]       INT              NULL,
    [level]          INT              NOT NULL,
    [path]           NVARCHAR (150)   NOT NULL,
    [sortOrder]      INT              NOT NULL,
    [uniqueID]       UNIQUEIDENTIFIER CONSTRAINT [DF_umbracoNode_uniqueID] DEFAULT (newid()) NOT NULL,
    [text]           NVARCHAR (255)   NULL,
    [nodeObjectType] UNIQUEIDENTIFIER NULL,
    [createDate]     DATETIME         CONSTRAINT [DF_umbracoNode_createDate] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_structure] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_umbracoNode_umbracoNode_id] FOREIGN KEY ([parentID]) REFERENCES [dbo].[umbracoNode] ([id])
);


GO
CREATE NONCLUSTERED INDEX [IX_umbracoNodePath]
    ON [dbo].[umbracoNode]([path] ASC);


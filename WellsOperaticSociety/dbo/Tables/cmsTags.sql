CREATE TABLE [dbo].[cmsTags] (
    [id]       INT            IDENTITY (1, 1) NOT NULL,
    [tag]      NVARCHAR (200) NULL,
    [ParentId] INT            NULL,
    [group]    NVARCHAR (100) NULL,
    CONSTRAINT [PK_cmsTags] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_cmsTags_cmsTags] FOREIGN KEY ([ParentId]) REFERENCES [dbo].[cmsTags] ([id])
);


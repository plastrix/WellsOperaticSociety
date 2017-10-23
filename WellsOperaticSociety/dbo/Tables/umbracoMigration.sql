CREATE TABLE [dbo].[umbracoMigration] (
    [id]         INT            IDENTITY (1, 1) NOT NULL,
    [name]       NVARCHAR (255) NOT NULL,
    [createDate] DATETIME       CONSTRAINT [DF_umbracoMigration_createDate] DEFAULT (getdate()) NOT NULL,
    [version]    NVARCHAR (50)  NOT NULL,
    CONSTRAINT [PK_umbracoMigration] PRIMARY KEY CLUSTERED ([id] ASC)
);


CREATE TABLE [dbo].[cmsTaskType] (
    [id]    INT            IDENTITY (1, 1) NOT NULL,
    [alias] NVARCHAR (255) NOT NULL,
    CONSTRAINT [PK_cmsTaskType] PRIMARY KEY CLUSTERED ([id] ASC)
);


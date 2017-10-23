CREATE TABLE [dbo].[cmsTask] (
    [closed]       BIT            CONSTRAINT [DF_cmsTask_closed] DEFAULT ('0') NOT NULL,
    [id]           INT            IDENTITY (1, 1) NOT NULL,
    [taskTypeId]   INT            NOT NULL,
    [nodeId]       INT            NOT NULL,
    [parentUserId] INT            NOT NULL,
    [userId]       INT            NOT NULL,
    [DateTime]     DATETIME       CONSTRAINT [DF_cmsTask_DateTime] DEFAULT (getdate()) NOT NULL,
    [Comment]      NVARCHAR (500) NULL,
    CONSTRAINT [PK_cmsTask] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_cmsTask_cmsTaskType_id] FOREIGN KEY ([taskTypeId]) REFERENCES [dbo].[cmsTaskType] ([id]),
    CONSTRAINT [FK_cmsTask_umbracoNode_id] FOREIGN KEY ([nodeId]) REFERENCES [dbo].[umbracoNode] ([id]),
    CONSTRAINT [FK_cmsTask_umbracoUser] FOREIGN KEY ([parentUserId]) REFERENCES [dbo].[umbracoUser] ([id]),
    CONSTRAINT [FK_cmsTask_umbracoUser1] FOREIGN KEY ([userId]) REFERENCES [dbo].[umbracoUser] ([id])
);


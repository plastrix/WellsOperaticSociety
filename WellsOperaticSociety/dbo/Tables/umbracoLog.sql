CREATE TABLE [dbo].[umbracoLog] (
    [id]         INT             IDENTITY (1, 1) NOT NULL,
    [userId]     INT             NOT NULL,
    [NodeId]     INT             NOT NULL,
    [Datestamp]  DATETIME        CONSTRAINT [DF_umbracoLog_Datestamp] DEFAULT (getdate()) NOT NULL,
    [logHeader]  NVARCHAR (50)   NOT NULL,
    [logComment] NVARCHAR (4000) NULL,
    CONSTRAINT [PK_umbracoLog] PRIMARY KEY CLUSTERED ([id] ASC)
);


CREATE TABLE [dbo].[cmsContentXml] (
    [nodeId] INT   NOT NULL,
    [xml]    NTEXT NOT NULL,
    CONSTRAINT [PK_cmsContentXml] PRIMARY KEY CLUSTERED ([nodeId] ASC)
);


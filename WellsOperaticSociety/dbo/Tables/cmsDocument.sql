CREATE TABLE [dbo].[cmsDocument] (
    [nodeId]       INT              NOT NULL,
    [published]    BIT              NOT NULL,
    [documentUser] INT              NOT NULL,
    [versionId]    UNIQUEIDENTIFIER NOT NULL,
    [text]         NVARCHAR (255)   NOT NULL,
    [releaseDate]  DATETIME         NULL,
    [expireDate]   DATETIME         NULL,
    [updateDate]   DATETIME         CONSTRAINT [DF_cmsDocument_updateDate] DEFAULT (getdate()) NOT NULL,
    [templateId]   INT              NULL,
    [newest]       BIT              CONSTRAINT [DF_cmsDocument_newest] DEFAULT ('0') NOT NULL,
    CONSTRAINT [PK_cmsDocument] PRIMARY KEY CLUSTERED ([versionId] ASC),
    CONSTRAINT [FK_cmsDocument_umbracoNode_id] FOREIGN KEY ([nodeId]) REFERENCES [dbo].[umbracoNode] ([id])
);


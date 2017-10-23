CREATE TABLE [dbo].[cmsPreviewXml] (
    [nodeId]    INT              NOT NULL,
    [versionId] UNIQUEIDENTIFIER NOT NULL,
    [timestamp] DATETIME         NOT NULL,
    [xml]       NTEXT            NOT NULL,
    CONSTRAINT [PK_cmsContentPreviewXml] PRIMARY KEY CLUSTERED ([nodeId] ASC, [versionId] ASC)
);


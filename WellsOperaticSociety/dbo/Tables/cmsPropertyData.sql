CREATE TABLE [dbo].[cmsPropertyData] (
    [id]             INT              IDENTITY (1, 1) NOT NULL,
    [contentNodeId]  INT              NOT NULL,
    [versionId]      UNIQUEIDENTIFIER NULL,
    [propertytypeid] INT              NOT NULL,
    [dataInt]        INT              NULL,
    [dataDecimal]    DECIMAL (38, 6)  NULL,
    [dataDate]       DATETIME         NULL,
    [dataNvarchar]   NVARCHAR (500)   NULL,
    [dataNtext]      NTEXT            NULL,
    CONSTRAINT [PK_cmsPropertyData] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_cmsPropertyData_cmsPropertyType_id] FOREIGN KEY ([propertytypeid]) REFERENCES [dbo].[cmsPropertyType] ([id]),
    CONSTRAINT [FK_cmsPropertyData_umbracoNode_id] FOREIGN KEY ([contentNodeId]) REFERENCES [dbo].[umbracoNode] ([id])
);


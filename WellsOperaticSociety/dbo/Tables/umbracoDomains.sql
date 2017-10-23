CREATE TABLE [dbo].[umbracoDomains] (
    [id]                    INT            IDENTITY (1, 1) NOT NULL,
    [domainDefaultLanguage] INT            NULL,
    [domainRootStructureID] INT            NULL,
    [domainName]            NVARCHAR (255) NOT NULL,
    CONSTRAINT [PK_umbracoDomains] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_umbracoDomains_umbracoNode_id] FOREIGN KEY ([domainRootStructureID]) REFERENCES [dbo].[umbracoNode] ([id])
);


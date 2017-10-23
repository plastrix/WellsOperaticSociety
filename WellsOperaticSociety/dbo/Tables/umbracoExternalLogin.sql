CREATE TABLE [dbo].[umbracoExternalLogin] (
    [id]            INT             IDENTITY (1, 1) NOT NULL,
    [userId]        INT             NOT NULL,
    [loginProvider] NVARCHAR (4000) NOT NULL,
    [providerKey]   NVARCHAR (4000) NOT NULL,
    [createDate]    DATETIME        CONSTRAINT [DF_umbracoExternalLogin_createDate] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_umbracoExternalLogin] PRIMARY KEY CLUSTERED ([id] ASC)
);


CREATE TABLE [dbo].[umbracoRedirectUrl] (
    [id]            UNIQUEIDENTIFIER NOT NULL,
    [contentKey]    UNIQUEIDENTIFIER NOT NULL,
    [createDateUtc] DATETIME         NOT NULL,
    [url]           NVARCHAR (255)   NOT NULL,
    [urlHash]       NVARCHAR (40)    NOT NULL,
    CONSTRAINT [PK_umbracoRedirectUrl] PRIMARY KEY CLUSTERED ([id] ASC)
);


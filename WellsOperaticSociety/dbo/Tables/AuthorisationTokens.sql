CREATE TABLE [dbo].[AuthorisationTokens] (
    [AuthorisationTokenId] INT            IDENTITY (1, 1) NOT NULL,
    [Token]                NVARCHAR (MAX) NULL,
    [Member]               INT            NOT NULL,
    [Used]                 BIT            NOT NULL,
    [Created]              DATETIME       NOT NULL,
    CONSTRAINT [PK_dbo.AuthorisationTokens] PRIMARY KEY CLUSTERED ([AuthorisationTokenId] ASC)
);


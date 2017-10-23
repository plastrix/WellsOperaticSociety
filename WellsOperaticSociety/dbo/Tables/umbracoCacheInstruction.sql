CREATE TABLE [dbo].[umbracoCacheInstruction] (
    [id]              INT            IDENTITY (1, 1) NOT NULL,
    [utcStamp]        DATETIME       NOT NULL,
    [jsonInstruction] NTEXT          NOT NULL,
    [originated]      NVARCHAR (500) NOT NULL,
    CONSTRAINT [PK_umbracoCacheInstruction] PRIMARY KEY CLUSTERED ([id] ASC)
);


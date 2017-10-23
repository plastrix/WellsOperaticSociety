CREATE TABLE [dbo].[LongServiceAwards] (
    [LongServiceAwardId] INT IDENTITY (1, 1) NOT NULL,
    [Member]             INT NOT NULL,
    [Award]              INT NOT NULL,
    [Hide]               BIT NOT NULL,
    [Awarded]            BIT DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_dbo.LongServiceAwards] PRIMARY KEY CLUSTERED ([LongServiceAwardId] ASC)
);


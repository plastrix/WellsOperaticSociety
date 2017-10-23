CREATE TABLE [dbo].[BoxOfficeTime] (
    [BoxOfficeTimeId] INT            IDENTITY (1, 1) NOT NULL,
    [Date]            DATE           NOT NULL,
    [OpeningTime]     TIME (7)       NOT NULL,
    [ClosingTime]     TIME (7)       NOT NULL,
    [Message]         NVARCHAR (255) NULL
);


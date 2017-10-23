CREATE TABLE [dbo].[Seats] (
    [SeatId]      INT            IDENTITY (1, 1) NOT NULL,
    [SeatNumber]  NVARCHAR (MAX) NOT NULL,
    [Description] NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_dbo.Seats] PRIMARY KEY CLUSTERED ([SeatId] ASC)
);


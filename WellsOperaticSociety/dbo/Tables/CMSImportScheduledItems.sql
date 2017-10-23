CREATE TABLE [dbo].[CMSImportScheduledItems] (
    [ScheduledItemId]  INT           IDENTITY (1, 1) NOT NULL,
    [ScheduleldTaskId] INT           NOT NULL,
    [ScheduledOn]      SMALLDATETIME NOT NULL,
    [ExecutedOn]       SMALLDATETIME NULL,
    [InProgress]       BIT           NULL,
    CONSTRAINT [PK_CMSImportScheduledItems] PRIMARY KEY CLUSTERED ([ScheduledItemId] ASC)
);


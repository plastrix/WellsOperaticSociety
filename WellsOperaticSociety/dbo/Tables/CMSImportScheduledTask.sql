CREATE TABLE [dbo].[CMSImportScheduledTask] (
    [ScheduleId]         INT              IDENTITY (1, 1) NOT NULL,
    [ScheduleGUID]       UNIQUEIDENTIFIER NOT NULL,
    [ImportStateGUID]    UNIQUEIDENTIFIER NOT NULL,
    [ScheduledTaskName]  NVARCHAR (50)    NOT NULL,
    [NotifyEmailAddress] NVARCHAR (250)   NOT NULL,
    [ExecuteEvery]       NVARCHAR (50)    NOT NULL,
    [ExecuteDays]        NVARCHAR (50)    NOT NULL,
    [ExecuteHour]        INT              NOT NULL,
    [ExecuteMinute]      INT              NOT NULL,
    [ImportAsUser]       INT              NULL,
    CONSTRAINT [PK_CMSImportScheduledTask] PRIMARY KEY CLUSTERED ([ScheduleId] ASC)
);


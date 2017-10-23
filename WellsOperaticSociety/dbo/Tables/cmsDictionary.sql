CREATE TABLE [dbo].[cmsDictionary] (
    [pk]     INT              IDENTITY (1, 1) NOT NULL,
    [id]     UNIQUEIDENTIFIER NOT NULL,
    [parent] UNIQUEIDENTIFIER NULL,
    [key]    NVARCHAR (1000)  NOT NULL,
    CONSTRAINT [PK_cmsDictionary] PRIMARY KEY CLUSTERED ([pk] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_cmsDictionary_key]
    ON [dbo].[cmsDictionary]([key] ASC);


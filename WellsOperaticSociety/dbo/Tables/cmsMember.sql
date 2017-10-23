CREATE TABLE [dbo].[cmsMember] (
    [nodeId]    INT             NOT NULL,
    [Email]     NVARCHAR (1000) CONSTRAINT [DF_cmsMember_Email] DEFAULT ('''') NOT NULL,
    [LoginName] NVARCHAR (225)  CONSTRAINT [DF_cmsMember_LoginName] DEFAULT ('''') NOT NULL,
    [Password]  NVARCHAR (1000) CONSTRAINT [DF_cmsMember_Password] DEFAULT ('''') NOT NULL,
    CONSTRAINT [PK_cmsMember] PRIMARY KEY CLUSTERED ([nodeId] ASC),
    CONSTRAINT [FK_cmsMember_umbracoNode_id] FOREIGN KEY ([nodeId]) REFERENCES [dbo].[umbracoNode] ([id])
);


GO
CREATE NONCLUSTERED INDEX [IX_cmsMember_LoginName]
    ON [dbo].[cmsMember]([LoginName] ASC);


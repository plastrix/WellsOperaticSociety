CREATE TABLE [dbo].[MemberRolesInShows] (
    [MemberRolesInShowId] INT            IDENTITY (1, 1) NOT NULL,
    [MemberId]            INT            NOT NULL,
    [Group]               NVARCHAR (MAX) NOT NULL,
    [Role]                NVARCHAR (MAX) NULL,
    [FunctionId]          INT            NOT NULL,
    CONSTRAINT [PK_dbo.MemberRolesInShows] PRIMARY KEY CLUSTERED ([MemberRolesInShowId] ASC)
);


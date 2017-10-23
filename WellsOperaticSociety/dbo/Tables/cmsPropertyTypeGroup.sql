CREATE TABLE [dbo].[cmsPropertyTypeGroup] (
    [id]                INT              IDENTITY (1, 1) NOT NULL,
    [contenttypeNodeId] INT              NOT NULL,
    [text]              NVARCHAR (255)   NOT NULL,
    [sortorder]         INT              NOT NULL,
    [uniqueID]          UNIQUEIDENTIFIER CONSTRAINT [DF_cmsPropertyTypeGroup_uniqueID] DEFAULT (newid()) NOT NULL,
    CONSTRAINT [PK_cmsPropertyTypeGroup] PRIMARY KEY CLUSTERED ([id] ASC)
);


CREATE TABLE [dbo].[cmsPropertyType] (
    [id]                  INT              IDENTITY (1, 1) NOT NULL,
    [dataTypeId]          INT              NOT NULL,
    [contentTypeId]       INT              NOT NULL,
    [propertyTypeGroupId] INT              NULL,
    [Alias]               NVARCHAR (255)   NOT NULL,
    [Name]                NVARCHAR (255)   NULL,
    [sortOrder]           INT              CONSTRAINT [DF_cmsPropertyType_sortOrder] DEFAULT ('0') NOT NULL,
    [mandatory]           BIT              CONSTRAINT [DF_cmsPropertyType_mandatory] DEFAULT ('0') NOT NULL,
    [validationRegExp]    NVARCHAR (255)   NULL,
    [Description]         NVARCHAR (2000)  NULL,
    [UniqueID]            UNIQUEIDENTIFIER CONSTRAINT [DF_cmsPropertyType_UniqueID] DEFAULT (newid()) NOT NULL,
    CONSTRAINT [PK_cmsPropertyType] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_cmsPropertyType_cmsPropertyTypeGroup_id] FOREIGN KEY ([propertyTypeGroupId]) REFERENCES [dbo].[cmsPropertyTypeGroup] ([id])
);


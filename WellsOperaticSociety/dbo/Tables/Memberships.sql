CREATE TABLE [dbo].[Memberships] (
    [MembershipId]         INT            IDENTITY (1, 1) NOT NULL,
    [Member]               INT            NOT NULL,
    [StartDate]            DATETIME       NOT NULL,
    [EndDate]              DATETIME       NOT NULL,
    [MembershipType]       INT            NOT NULL,
    [IsSubscription]       BIT            DEFAULT ((0)) NOT NULL,
    [CancelAtEnd]          BIT            DEFAULT ((0)) NOT NULL,
    [StripeSubscriptionId] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_dbo.Memberships] PRIMARY KEY CLUSTERED ([MembershipId] ASC)
);


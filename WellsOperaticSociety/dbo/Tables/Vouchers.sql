CREATE TABLE [dbo].[Vouchers] (
    [VoucherId]  INT            IDENTITY (1, 1) NOT NULL,
    [FunctionId] INT            NOT NULL,
    [MemberId]   INT            NOT NULL,
    [Key]        NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_dbo.Vouchers] PRIMARY KEY CLUSTERED ([VoucherId] ASC)
);


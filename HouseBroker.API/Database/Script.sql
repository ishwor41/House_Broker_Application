IF DB_ID('HouseBrokerDb') IS NULL
CREATE DATABASE HouseBrokerDb;
GO

USE HouseBrokerDb;
GO


IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [AspNetRoles] (
    [Id] nvarchar(450) NOT NULL,
    [Name] nvarchar(256) NULL,
    [NormalizedName] nvarchar(256) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [AspNetUsers] (
    [Id] nvarchar(450) NOT NULL,
    [UserName] nvarchar(256) NULL,
    [NormalizedUserName] nvarchar(256) NULL,
    [Email] nvarchar(256) NULL,
    [NormalizedEmail] nvarchar(256) NULL,
    [EmailConfirmed] bit NOT NULL,
    [PasswordHash] nvarchar(max) NULL,
    [SecurityStamp] nvarchar(max) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    [PhoneNumber] nvarchar(max) NULL,
    [PhoneNumberConfirmed] bit NOT NULL,
    [TwoFactorEnabled] bit NOT NULL,
    [LockoutEnd] datetimeoffset NULL,
    [LockoutEnabled] bit NOT NULL,
    [AccessFailedCount] int NOT NULL,
    CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Properties] (
    [Id] uniqueidentifier NOT NULL,
    [Title] nvarchar(max) NOT NULL,
    [PropertyType] nvarchar(max) NOT NULL,
    [Location] nvarchar(max) NOT NULL,
    [Price] decimal(18,2) NOT NULL,
    [Features] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Properties] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [AspNetRoleClaims] (
    [Id] int NOT NULL IDENTITY,
    [RoleId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserClaims] (
    [Id] int NOT NULL IDENTITY,
    [UserId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserLogins] (
    [LoginProvider] nvarchar(450) NOT NULL,
    [ProviderKey] nvarchar(450) NOT NULL,
    [ProviderDisplayName] nvarchar(max) NULL,
    [UserId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
    CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserRoles] (
    [UserId] nvarchar(450) NOT NULL,
    [RoleId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserTokens] (
    [UserId] nvarchar(450) NOT NULL,
    [LoginProvider] nvarchar(450) NOT NULL,
    [Name] nvarchar(450) NOT NULL,
    [Value] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
    CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);
GO

CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL;
GO

CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);
GO

CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);
GO

CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);
GO

CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);
GO

CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260510115834_InitialIdentity', N'7.0.20');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [CommissionSettings] (
    [Id] uniqueidentifier NOT NULL,
    [MinPrice] decimal(18,2) NOT NULL,
    [MaxPrice] decimal(18,2) NULL,
    [Percentage] decimal(18,2) NOT NULL,
    [IsActive] bit NOT NULL,
    CONSTRAINT [PK_CommissionSettings] PRIMARY KEY ([Id])
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260510150521_AddCommissionTable', N'7.0.20');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Properties] ADD [CommissionAmount] decimal(18,2) NOT NULL DEFAULT 0.0;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260510162320_Update_Property', N'7.0.20');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Properties] ADD [CreatedBy] nvarchar(max) NOT NULL DEFAULT N'';
GO

ALTER TABLE [Properties] ADD [CreatedDate] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';
GO

ALTER TABLE [Properties] ADD [RecordStatus] int NOT NULL DEFAULT 0;
GO

ALTER TABLE [Properties] ADD [UpdatedBy] nvarchar(max) NULL;
GO

ALTER TABLE [Properties] ADD [UpdatedDate] datetime2 NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260510170911_Update_Property_Records', N'7.0.20');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[CommissionSettings]') AND [c].[name] = N'IsActive');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [CommissionSettings] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [CommissionSettings] DROP COLUMN [IsActive];
GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Properties]') AND [c].[name] = N'UpdatedBy');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Properties] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [Properties] ALTER COLUMN [UpdatedBy] uniqueidentifier NULL;
GO

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Properties]') AND [c].[name] = N'CreatedBy');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [Properties] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [Properties] ALTER COLUMN [CreatedBy] uniqueidentifier NOT NULL;
GO

ALTER TABLE [CommissionSettings] ADD [RecordStatus] int NOT NULL DEFAULT 0;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260510171552_Update_Property_And_SettingCommission', N'7.0.20');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260510172516_Update_Property_And_SettingCommission_Again', N'7.0.20');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var3 sysname;
SELECT @var3 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Properties]') AND [c].[name] = N'CreatedBy');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [Properties] DROP CONSTRAINT [' + @var3 + '];');
ALTER TABLE [Properties] ALTER COLUMN [CreatedBy] uniqueidentifier NULL;
GO

DECLARE @var4 sysname;
SELECT @var4 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Properties]') AND [c].[name] = N'CommissionAmount');
IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [Properties] DROP CONSTRAINT [' + @var4 + '];');
ALTER TABLE [Properties] ALTER COLUMN [CommissionAmount] decimal(18,2) NULL;
GO

ALTER TABLE [Properties] ADD [PropertyImageUrl] nvarchar(max) NOT NULL DEFAULT N'';
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260511020807_Added_Image_Path', N'7.0.20');
GO

COMMIT;
GO
BEGIN TRANSACTION;
GO

IF OBJECT_ID(N'[dbo].[sp_Property_Manage]', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_Property_Manage];
GO

CREATE PROCEDURE [dbo].[sp_Property_Manage]
    @Id UNIQUEIDENTIFIER = NULL,
    @Title NVARCHAR(200) = NULL,
    @PropertyType NVARCHAR(50) = NULL,
    @Location NVARCHAR(200) = NULL,
    @Price DECIMAL(18,2) = NULL,
    @Features NVARCHAR(MAX) = NULL,
    @CommissionAmount DECIMAL(18,2) = NULL,
    @CreatedDate DATETIME = NULL,
    @CreatedBy UNIQUEIDENTIFIER = NULL,
    @RecordStatus INT = 1,
    @UpdatedDate DATETIME = NULL,
    @UpdatedBy UNIQUEIDENTIFIER = NULL,
    @flag CHAR(1),
    @MaxPrice DECIMAL(18,2) = NULL,
    @MinPrice DECIMAL(18,2) = NULL,
    @UserId UNIQUEIDENTIFIER = NULL,
    @ImageUrl NVARCHAR(500) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @flag = 'I'
    BEGIN
        INSERT INTO Properties
        (
            Id, Title, PropertyType, Location, Price, Features,
            CommissionAmount, PropertyImageUrl,
            CreatedDate, CreatedBy, RecordStatus
        )
        VALUES
        (
            NEWID(), @Title, @PropertyType, @Location, @Price, @Features,
            @CommissionAmount, @ImageUrl,
            @CreatedDate, @CreatedBy, @RecordStatus
        );
    END

    ELSE IF @flag = 'U'
    BEGIN
        UPDATE Properties
        SET
            Title = @Title,
            PropertyType = @PropertyType,
            [Location] = @Location,
            Price = @Price,
            Features = @Features,
            CommissionAmount = @CommissionAmount,
            PropertyImageUrl = @ImageUrl,
            UpdatedDate = @UpdatedDate,
            UpdatedBy = @UpdatedBy
        WHERE Id = @Id AND RecordStatus = 1;
    END

    ELSE IF @flag = 'G'
    BEGIN
        SELECT
            Id,
            Title,
            PropertyType,
            Location,
            Price,
            Features,
            CASE WHEN @UserId IS NULL THEN NULL ELSE CommissionAmount END AS CommissionAmount,
            PropertyImageUrl
        FROM Properties
        WHERE Id = @Id AND RecordStatus = 1;
    END

    ELSE IF @flag = 'D'
    BEGIN
        UPDATE Properties
        SET RecordStatus = 0
        WHERE Id = @Id;
    END

    ELSE IF @flag = 'L'
    BEGIN
        SELECT 
            Id, Title, PropertyType, Location, Price, Features,
            CASE WHEN @UserId IS NULL THEN NULL ELSE CommissionAmount END AS CommissionAmount,
            PropertyImageUrl
        FROM Properties
        WHERE RecordStatus = 1
            AND (@Title IS NULL OR Title LIKE '%' + @Title + '%')
            AND (@Features IS NULL OR Features LIKE '%' + @Features + '%')
            AND (@PropertyType IS NULL OR PropertyType = @PropertyType)
            AND (@Location IS NULL OR Location LIKE '%' + @Location + '%')
            AND (@MinPrice IS NULL OR Price >= @MinPrice)
            AND (@MaxPrice IS NULL OR Price <= @MaxPrice)
            AND (@UserId IS NULL OR CreatedBy = @UserId)

        UNION

        SELECT 
            Id, Title, PropertyType, Location, Price, Features,
            NULL AS CommissionAmount,
            PropertyImageUrl
        FROM Properties
        WHERE RecordStatus = 1
            AND @UserId IS NOT NULL
            AND (@Title IS NULL OR Title LIKE '%' + @Title + '%')
            AND (@Features IS NULL OR Features LIKE '%' + @Features + '%')
            AND (@PropertyType IS NULL OR PropertyType = @PropertyType)
            AND (@Location IS NULL OR Location LIKE '%' + @Location + '%')
            AND (@MinPrice IS NULL OR Price >= @MinPrice)
            AND (@MaxPrice IS NULL OR Price <= @MaxPrice)
            AND CreatedBy <> @UserId;
    END
END
GO

COMMIT;
GO

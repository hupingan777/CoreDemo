IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

CREATE TABLE [RoleMains] (
    [Id] int NOT NULL IDENTITY,
    [RoleName] nvarchar(max) NULL,
    [CreateTime] datetime2 NOT NULL,
    CONSTRAINT [PK_RoleMains] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [UserMains] (
    [Id] int NOT NULL IDENTITY,
    [UserName] nvarchar(max) NULL,
    [Address] nvarchar(max) NULL,
    [CreateTime] datetime2 NOT NULL,
    [RoleId] int NOT NULL,
    CONSTRAINT [PK_UserMains] PRIMARY KEY ([Id])
);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200721124127_inital', N'3.1.6');

GO


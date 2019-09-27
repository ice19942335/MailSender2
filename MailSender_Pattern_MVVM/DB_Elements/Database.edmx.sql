
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 12/06/2018 02:13:47
-- Generated from EDMX file: C:\Users\ice19\OneDrive\Desktop\MailSender GIT\MailSender_Pattern_MVVM\DB_Elements\Database.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [MailSenderDB_EF];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------


-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'EmailSenders'
CREATE TABLE [dbo].[EmailSenders] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Email] nvarchar(max)  NOT NULL,
    [Hash] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'EmailSmtps'
CREATE TABLE [dbo].[EmailSmtps] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SmtpServer] nvarchar(max)  NOT NULL,
    [Port] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'EmailRecipients'
CREATE TABLE [dbo].[EmailRecipients] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Email] nvarchar(max)  NOT NULL,
    [Name] nvarchar(max)  NULL,
    [Address] nvarchar(max)  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'EmailSenders'
ALTER TABLE [dbo].[EmailSenders]
ADD CONSTRAINT [PK_EmailSenders]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'EmailSmtps'
ALTER TABLE [dbo].[EmailSmtps]
ADD CONSTRAINT [PK_EmailSmtps]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'EmailRecipients'
ALTER TABLE [dbo].[EmailRecipients]
ADD CONSTRAINT [PK_EmailRecipients]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------
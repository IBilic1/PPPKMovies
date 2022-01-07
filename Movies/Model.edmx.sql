
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 01/03/2022 16:22:25
-- Generated from EDMX file: C:\Users\HT-ICT\Desktop\PPK\12. vježbe\Movies\Movies\Model.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [Apartments];
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

-- Creating table 'Movies'
CREATE TABLE [dbo].[Movies] (
    [IDMovie] int IDENTITY(1,1) NOT NULL,
    [Title] nvarchar(max)  NOT NULL,
    [Genre] nvarchar(max)  NOT NULL,
    [Duration] int  NOT NULL
);
GO

-- Creating table 'UploadedPosters'
CREATE TABLE [dbo].[UploadedPosters] (
    [IDUploadedPoster] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(20)  NOT NULL,
    [ContentType] nvarchar(20)  NOT NULL,
    [Content] varbinary(max)  NOT NULL,
    [MovieIDMovie] int  NOT NULL
);
GO

-- Creating table 'People'
CREATE TABLE [dbo].[People] (
    [IDPerson] int IDENTITY(1,1) NOT NULL,
    [Firstname] nvarchar(max)  NOT NULL,
    [Lastname] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'MoviePerson'
CREATE TABLE [dbo].[MoviePerson] (
    [Movies_IDMovie] int  NOT NULL,
    [People_IDPerson] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [IDMovie] in table 'Movies'
ALTER TABLE [dbo].[Movies]
ADD CONSTRAINT [PK_Movies]
    PRIMARY KEY CLUSTERED ([IDMovie] ASC);
GO

-- Creating primary key on [IDUploadedPoster] in table 'UploadedPosters'
ALTER TABLE [dbo].[UploadedPosters]
ADD CONSTRAINT [PK_UploadedPosters]
    PRIMARY KEY CLUSTERED ([IDUploadedPoster] ASC);
GO

-- Creating primary key on [IDPerson] in table 'People'
ALTER TABLE [dbo].[People]
ADD CONSTRAINT [PK_People]
    PRIMARY KEY CLUSTERED ([IDPerson] ASC);
GO

-- Creating primary key on [Movies_IDMovie], [People_IDPerson] in table 'MoviePerson'
ALTER TABLE [dbo].[MoviePerson]
ADD CONSTRAINT [PK_MoviePerson]
    PRIMARY KEY CLUSTERED ([Movies_IDMovie], [People_IDPerson] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [MovieIDMovie] in table 'UploadedPosters'
ALTER TABLE [dbo].[UploadedPosters]
ADD CONSTRAINT [FK_MovieUploadedPosters]
    FOREIGN KEY ([MovieIDMovie])
    REFERENCES [dbo].[Movies]
        ([IDMovie])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_MovieUploadedPosters'
CREATE INDEX [IX_FK_MovieUploadedPosters]
ON [dbo].[UploadedPosters]
    ([MovieIDMovie]);
GO

-- Creating foreign key on [Movies_IDMovie] in table 'MoviePerson'
ALTER TABLE [dbo].[MoviePerson]
ADD CONSTRAINT [FK_MoviePerson_Movie]
    FOREIGN KEY ([Movies_IDMovie])
    REFERENCES [dbo].[Movies]
        ([IDMovie])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [People_IDPerson] in table 'MoviePerson'
ALTER TABLE [dbo].[MoviePerson]
ADD CONSTRAINT [FK_MoviePerson_Person]
    FOREIGN KEY ([People_IDPerson])
    REFERENCES [dbo].[People]
        ([IDPerson])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_MoviePerson_Person'
CREATE INDEX [IX_FK_MoviePerson_Person]
ON [dbo].[MoviePerson]
    ([People_IDPerson]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------
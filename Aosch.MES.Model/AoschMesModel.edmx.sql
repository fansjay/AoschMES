
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 08/17/2018 13:32:50
-- Generated from EDMX file: G:\Aosch Workspaces\AoschMES\Aosch.MES.Model\AoschMesModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [MES];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Account]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Account];
GO
IF OBJECT_ID(N'[dbo].[ActionURL]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ActionURL];
GO
IF OBJECT_ID(N'[dbo].[Customer]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Customer];
GO
IF OBJECT_ID(N'[dbo].[Department]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Department];
GO
IF OBJECT_ID(N'[dbo].[Employee]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Employee];
GO
IF OBJECT_ID(N'[dbo].[Position]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Position];
GO
IF OBJECT_ID(N'[dbo].[Role]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Role];
GO
IF OBJECT_ID(N'[dbo].[RoleActionURL_Mapping]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RoleActionURL_Mapping];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Account'
CREATE TABLE [dbo].[Account] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Username] nvarchar(50)  NOT NULL,
    [Password] nvarchar(50)  NOT NULL,
    [RoleID] int  NOT NULL,
    [DepartmentID] int  NOT NULL,
    [GroupID] int  NOT NULL,
    [EmployeeID] int  NOT NULL,
    [ICNumber] int  NOT NULL,
    [NickName] nvarchar(50)  NOT NULL,
    [Email] nvarchar(80)  NOT NULL,
    [PhoneNumber] nvarchar(50)  NOT NULL,
    [HomePage] nvarchar(100)  NOT NULL,
    [RegisterTime] datetime  NOT NULL,
    [Description] nvarchar(500)  NOT NULL
);
GO

-- Creating table 'Customer'
CREATE TABLE [dbo].[Customer] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(50)  NOT NULL,
    [CustomerName] nvarchar(50)  NOT NULL,
    [FactoryName] nvarchar(50)  NOT NULL,
    [Telphone] nvarchar(50)  NOT NULL,
    [Wechat] nvarchar(50)  NOT NULL,
    [Email] nvarchar(50)  NOT NULL,
    [FactoryAdress] nvarchar(50)  NOT NULL,
    [LogoImage] nvarchar(500)  NOT NULL,
    [Description] nvarchar(500)  NOT NULL
);
GO

-- Creating table 'Department'
CREATE TABLE [dbo].[Department] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [DepartmentName] nvarchar(50)  NOT NULL,
    [DepartmentType] nvarchar(50)  NOT NULL,
    [Description] nvarchar(500)  NOT NULL
);
GO

-- Creating table 'Employee'
CREATE TABLE [dbo].[Employee] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [RealName] nvarchar(50)  NOT NULL,
    [DepartmentID] int  NOT NULL,
    [GroupID] int  NOT NULL,
    [RegisterTime] datetime  NOT NULL,
    [Sex] smallint  NOT NULL,
    [Birthday] datetime  NOT NULL,
    [Salary] decimal(7,2)  NOT NULL,
    [PositionID] int  NOT NULL,
    [IDCardNumber] nvarchar(18)  NOT NULL,
    [Description] nvarchar(500)  NOT NULL
);
GO

-- Creating table 'Position'
CREATE TABLE [dbo].[Position] (
    [ID] int  NOT NULL,
    [PositionName] nvarchar(50)  NOT NULL,
    [Description] nvarchar(500)  NOT NULL
);
GO

-- Creating table 'Role'
CREATE TABLE [dbo].[Role] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [RoleName] nvarchar(50)  NOT NULL,
    [RoleLevel] int  NOT NULL,
    [Description] nvarchar(500)  NOT NULL
);
GO

-- Creating table 'RoleActionURL_Mapping'
CREATE TABLE [dbo].[RoleActionURL_Mapping] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [RoleID] int  NOT NULL,
    [ActionURLID] int  NOT NULL
);
GO

-- Creating table 'ActionURL'
CREATE TABLE [dbo].[ActionURL] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [MenuName] nvarchar(50)  NOT NULL,
    [PageLogo] nvarchar(200)  NOT NULL,
    [ParentID] int  NULL,
    [URL] nvarchar(100)  NOT NULL,
    [Icon] nvarchar(50)  NOT NULL,
    [Sort] int  NOT NULL,
    [Showable] bit  NOT NULL,
    [IsLeaf] bit  NOT NULL,
    [Description] nvarchar(500)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [ID] in table 'Account'
ALTER TABLE [dbo].[Account]
ADD CONSTRAINT [PK_Account]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Customer'
ALTER TABLE [dbo].[Customer]
ADD CONSTRAINT [PK_Customer]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Department'
ALTER TABLE [dbo].[Department]
ADD CONSTRAINT [PK_Department]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Employee'
ALTER TABLE [dbo].[Employee]
ADD CONSTRAINT [PK_Employee]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Position'
ALTER TABLE [dbo].[Position]
ADD CONSTRAINT [PK_Position]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Role'
ALTER TABLE [dbo].[Role]
ADD CONSTRAINT [PK_Role]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'RoleActionURL_Mapping'
ALTER TABLE [dbo].[RoleActionURL_Mapping]
ADD CONSTRAINT [PK_RoleActionURL_Mapping]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'ActionURL'
ALTER TABLE [dbo].[ActionURL]
ADD CONSTRAINT [PK_ActionURL]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------
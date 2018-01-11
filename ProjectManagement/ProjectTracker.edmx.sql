
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 04/06/2015 12:58:47
-- Generated from EDMX file: C:\VS2013\ProjectManagement\ProjectManagement\ProjectTracker.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [BioStatProject];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_tblInvesttblProject]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Projects] DROP CONSTRAINT [FK_tblInvesttblProject];
GO
IF OBJECT_ID(N'[dbo].[FK_InvestStatusInvest]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Invests] DROP CONSTRAINT [FK_InvestStatusInvest];
GO
IF OBJECT_ID(N'[dbo].[FK_BioStatGroupProject]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Projects] DROP CONSTRAINT [FK_BioStatGroupProject];
GO
IF OBJECT_ID(N'[dbo].[FK_GrantAffilProject]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Projects] DROP CONSTRAINT [FK_GrantAffilProject];
GO
IF OBJECT_ID(N'[dbo].[FK_PilotProject]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Projects] DROP CONSTRAINT [FK_PilotProject];
GO
IF OBJECT_ID(N'[dbo].[FK_HiProgramProject]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Projects] DROP CONSTRAINT [FK_HiProgramProject];
GO
IF OBJECT_ID(N'[dbo].[FK_EthincGroupProject]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Projects] DROP CONSTRAINT [FK_EthincGroupProject];
GO
IF OBJECT_ID(N'[dbo].[FK_JabsomDeptProject]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Projects] DROP CONSTRAINT [FK_JabsomDeptProject];
GO
IF OBJECT_ID(N'[dbo].[FK_JabsomOfficeProject]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Projects] DROP CONSTRAINT [FK_JabsomOfficeProject];
GO
IF OBJECT_ID(N'[dbo].[FK_JabsomAffilProject]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Projects] DROP CONSTRAINT [FK_JabsomAffilProject];
GO
IF OBJECT_ID(N'[dbo].[FK_BioStatPublication]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Publications] DROP CONSTRAINT [FK_BioStatPublication];
GO
IF OBJECT_ID(N'[dbo].[FK_GrantAffilPublication]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Publications] DROP CONSTRAINT [FK_GrantAffilPublication];
GO
IF OBJECT_ID(N'[dbo].[FK_JounralInfoPublication1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Publications] DROP CONSTRAINT [FK_JounralInfoPublication1];
GO
IF OBJECT_ID(N'[dbo].[FK_ProjectPublication]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Publications] DROP CONSTRAINT [FK_ProjectPublication];
GO
IF OBJECT_ID(N'[dbo].[FK_InvestGrantApp]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[GrantApps] DROP CONSTRAINT [FK_InvestGrantApp];
GO
IF OBJECT_ID(N'[dbo].[FK_ProjectGrantApp]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[GrantApps] DROP CONSTRAINT [FK_ProjectGrantApp];
GO
IF OBJECT_ID(N'[dbo].[FK_ProjectFact]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TimeEntries] DROP CONSTRAINT [FK_ProjectFact];
GO
IF OBJECT_ID(N'[dbo].[FK_BioStatFact]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TimeEntries] DROP CONSTRAINT [FK_BioStatFact];
GO
IF OBJECT_ID(N'[dbo].[FK_DateFact]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TimeEntries] DROP CONSTRAINT [FK_DateFact];
GO
IF OBJECT_ID(N'[dbo].[FK_ServiceTypeProjectTracker]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TimeEntries] DROP CONSTRAINT [FK_ServiceTypeProjectTracker];
GO
IF OBJECT_ID(N'[dbo].[FK_JounralPublication]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Publications] DROP CONSTRAINT [FK_JounralPublication];
GO
IF OBJECT_ID(N'[dbo].[FK_ConferencePublication]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Publications] DROP CONSTRAINT [FK_ConferencePublication];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Projects]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Projects];
GO
IF OBJECT_ID(N'[dbo].[Invests]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Invests];
GO
IF OBJECT_ID(N'[dbo].[InvestStatus]', 'U') IS NOT NULL
    DROP TABLE [dbo].[InvestStatus];
GO
IF OBJECT_ID(N'[dbo].[ServiceTypes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ServiceTypes];
GO
IF OBJECT_ID(N'[dbo].[BioStatGroups]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BioStatGroups];
GO
IF OBJECT_ID(N'[dbo].[BioStats]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BioStats];
GO
IF OBJECT_ID(N'[dbo].[GrantApps]', 'U') IS NOT NULL
    DROP TABLE [dbo].[GrantApps];
GO
IF OBJECT_ID(N'[dbo].[GrantAffils]', 'U') IS NOT NULL
    DROP TABLE [dbo].[GrantAffils];
GO
IF OBJECT_ID(N'[dbo].[Pilots]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Pilots];
GO
IF OBJECT_ID(N'[dbo].[HiPrograms]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HiPrograms];
GO
IF OBJECT_ID(N'[dbo].[EthnicGroups]', 'U') IS NOT NULL
    DROP TABLE [dbo].[EthnicGroups];
GO
IF OBJECT_ID(N'[dbo].[JabsomDepts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[JabsomDepts];
GO
IF OBJECT_ID(N'[dbo].[JabsomOffices]', 'U') IS NOT NULL
    DROP TABLE [dbo].[JabsomOffices];
GO
IF OBJECT_ID(N'[dbo].[JabsomAffils]', 'U') IS NOT NULL
    DROP TABLE [dbo].[JabsomAffils];
GO
IF OBJECT_ID(N'[dbo].[Conferences]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Conferences];
GO
IF OBJECT_ID(N'[dbo].[Jounrals]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Jounrals];
GO
IF OBJECT_ID(N'[dbo].[Dates]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Dates];
GO
IF OBJECT_ID(N'[dbo].[Publications]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Publications];
GO
IF OBJECT_ID(N'[dbo].[TimeEntries]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TimeEntries];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Projects'
CREATE TABLE [dbo].[Projects] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [InvestId] int  NOT NULL,
    [Title] nvarchar(max)  NOT NULL,
    [Summary] nvarchar(max)  NOT NULL,
    [DeadLine] datetime  NOT NULL,
    [BioStatGroupId] int  NOT NULL,
    [InitialDate] datetime  NOT NULL,
    [GrantAffilId] int  NOT NULL,
    [PilotId] int  NOT NULL,
    [HiProgramId] int  NOT NULL,
    [EthincGroupId] int  NOT NULL,
    [IsJabsom] bit  NOT NULL,
    [JabsomDeptId] int  NOT NULL,
    [JabsomOfficeId] int  NOT NULL,
    [JabsomAffilId] int  NOT NULL
);
GO

-- Creating table 'Invests'
CREATE TABLE [dbo].[Invests] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [LastName] nvarchar(max)  NOT NULL,
    [FirstName] nvarchar(max)  NOT NULL,
    [Email] nvarchar(max)  NOT NULL,
    [Phone] nvarchar(max)  NOT NULL,
    [InvestStatusId] int  NOT NULL
);
GO

-- Creating table 'InvestStatus'
CREATE TABLE [dbo].[InvestStatus] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [StatusValue] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'ServiceTypes'
CREATE TABLE [dbo].[ServiceTypes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ServiceName] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'BioStatGroups'
CREATE TABLE [dbo].[BioStatGroups] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Member] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'BioStats'
CREATE TABLE [dbo].[BioStats] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [BioStatName] nvarchar(max)  NOT NULL,
    [BioStatType] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'GrantApps'
CREATE TABLE [dbo].[GrantApps] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [InvestId] int  NOT NULL,
    [Status] int  NOT NULL,
    [ProjectId] int  NULL,
    [StartDate] nvarchar(max)  NOT NULL,
    [EndDate] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'GrantAffils'
CREATE TABLE [dbo].[GrantAffils] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [GrantAffilName] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Pilots'
CREATE TABLE [dbo].[Pilots] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [PilotName] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'HiPrograms'
CREATE TABLE [dbo].[HiPrograms] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ProgramName] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'EthnicGroups'
CREATE TABLE [dbo].[EthnicGroups] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [GroupName] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'JabsomDepts'
CREATE TABLE [dbo].[JabsomDepts] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [DeptName] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'JabsomOffices'
CREATE TABLE [dbo].[JabsomOffices] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [OfficeName] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'JabsomAffils'
CREATE TABLE [dbo].[JabsomAffils] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [AffilName] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Conferences'
CREATE TABLE [dbo].[Conferences] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ConfName] nvarchar(max)  NOT NULL,
    [ConfLoc] nvarchar(max)  NOT NULL,
    [StartDate] nvarchar(max)  NOT NULL,
    [EndDate] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Jounrals'
CREATE TABLE [dbo].[Jounrals] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Volume] nvarchar(max)  NOT NULL,
    [Issue] nvarchar(max)  NOT NULL,
    [Page] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Dates'
CREATE TABLE [dbo].[Dates] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Year] nvarchar(max)  NOT NULL,
    [Quarter] nvarchar(max)  NOT NULL,
    [Month] nvarchar(max)  NOT NULL,
    [Week] nvarchar(max)  NOT NULL,
    [Day] nvarchar(max)  NOT NULL,
    [QuarterOfYear] nvarchar(max)  NOT NULL,
    [MonthOfYear] nvarchar(max)  NOT NULL,
    [WeekOfYear] nvarchar(max)  NOT NULL,
    [DayOfYear] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Publications'
CREATE TABLE [dbo].[Publications] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [PubType] int  NOT NULL,
    [Status] int  NOT NULL,
    [BioStatId] int  NOT NULL,
    [GrantAffilId] int  NOT NULL,
    [PubDate] nvarchar(max)  NOT NULL,
    [JounralId] int  NOT NULL,
    [ConferenceId] int  NOT NULL,
    [Title] nvarchar(max)  NOT NULL,
    [Author] nvarchar(max)  NOT NULL,
    [Pmid] nvarchar(max)  NOT NULL,
    [Pmcid] nvarchar(max)  NOT NULL,
    [Comment] nvarchar(max)  NOT NULL,
    [ProjectId] int  NOT NULL,
    [StartDate] nvarchar(max)  NOT NULL,
    [EndDate] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'TimeEntries'
CREATE TABLE [dbo].[TimeEntries] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ProjectId] int  NOT NULL,
    [BioStatId] int  NOT NULL,
    [DateId] int  NOT NULL,
    [Duration] nvarchar(max)  NOT NULL,
    [ServiceTypeId] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Projects'
ALTER TABLE [dbo].[Projects]
ADD CONSTRAINT [PK_Projects]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Invests'
ALTER TABLE [dbo].[Invests]
ADD CONSTRAINT [PK_Invests]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'InvestStatus'
ALTER TABLE [dbo].[InvestStatus]
ADD CONSTRAINT [PK_InvestStatus]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ServiceTypes'
ALTER TABLE [dbo].[ServiceTypes]
ADD CONSTRAINT [PK_ServiceTypes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'BioStatGroups'
ALTER TABLE [dbo].[BioStatGroups]
ADD CONSTRAINT [PK_BioStatGroups]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'BioStats'
ALTER TABLE [dbo].[BioStats]
ADD CONSTRAINT [PK_BioStats]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GrantApps'
ALTER TABLE [dbo].[GrantApps]
ADD CONSTRAINT [PK_GrantApps]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GrantAffils'
ALTER TABLE [dbo].[GrantAffils]
ADD CONSTRAINT [PK_GrantAffils]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Pilots'
ALTER TABLE [dbo].[Pilots]
ADD CONSTRAINT [PK_Pilots]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'HiPrograms'
ALTER TABLE [dbo].[HiPrograms]
ADD CONSTRAINT [PK_HiPrograms]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'EthnicGroups'
ALTER TABLE [dbo].[EthnicGroups]
ADD CONSTRAINT [PK_EthnicGroups]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'JabsomDepts'
ALTER TABLE [dbo].[JabsomDepts]
ADD CONSTRAINT [PK_JabsomDepts]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'JabsomOffices'
ALTER TABLE [dbo].[JabsomOffices]
ADD CONSTRAINT [PK_JabsomOffices]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'JabsomAffils'
ALTER TABLE [dbo].[JabsomAffils]
ADD CONSTRAINT [PK_JabsomAffils]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Conferences'
ALTER TABLE [dbo].[Conferences]
ADD CONSTRAINT [PK_Conferences]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Jounrals'
ALTER TABLE [dbo].[Jounrals]
ADD CONSTRAINT [PK_Jounrals]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Dates'
ALTER TABLE [dbo].[Dates]
ADD CONSTRAINT [PK_Dates]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Publications'
ALTER TABLE [dbo].[Publications]
ADD CONSTRAINT [PK_Publications]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'TimeEntries'
ALTER TABLE [dbo].[TimeEntries]
ADD CONSTRAINT [PK_TimeEntries]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [InvestId] in table 'Projects'
ALTER TABLE [dbo].[Projects]
ADD CONSTRAINT [FK_tblInvesttblProject]
    FOREIGN KEY ([InvestId])
    REFERENCES [dbo].[Invests]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_tblInvesttblProject'
CREATE INDEX [IX_FK_tblInvesttblProject]
ON [dbo].[Projects]
    ([InvestId]);
GO

-- Creating foreign key on [InvestStatusId] in table 'Invests'
ALTER TABLE [dbo].[Invests]
ADD CONSTRAINT [FK_InvestStatusInvest]
    FOREIGN KEY ([InvestStatusId])
    REFERENCES [dbo].[InvestStatus]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_InvestStatusInvest'
CREATE INDEX [IX_FK_InvestStatusInvest]
ON [dbo].[Invests]
    ([InvestStatusId]);
GO

-- Creating foreign key on [BioStatGroupId] in table 'Projects'
ALTER TABLE [dbo].[Projects]
ADD CONSTRAINT [FK_BioStatGroupProject]
    FOREIGN KEY ([BioStatGroupId])
    REFERENCES [dbo].[BioStatGroups]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_BioStatGroupProject'
CREATE INDEX [IX_FK_BioStatGroupProject]
ON [dbo].[Projects]
    ([BioStatGroupId]);
GO

-- Creating foreign key on [GrantAffilId] in table 'Projects'
ALTER TABLE [dbo].[Projects]
ADD CONSTRAINT [FK_GrantAffilProject]
    FOREIGN KEY ([GrantAffilId])
    REFERENCES [dbo].[GrantAffils]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_GrantAffilProject'
CREATE INDEX [IX_FK_GrantAffilProject]
ON [dbo].[Projects]
    ([GrantAffilId]);
GO

-- Creating foreign key on [PilotId] in table 'Projects'
ALTER TABLE [dbo].[Projects]
ADD CONSTRAINT [FK_PilotProject]
    FOREIGN KEY ([PilotId])
    REFERENCES [dbo].[Pilots]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_PilotProject'
CREATE INDEX [IX_FK_PilotProject]
ON [dbo].[Projects]
    ([PilotId]);
GO

-- Creating foreign key on [HiProgramId] in table 'Projects'
ALTER TABLE [dbo].[Projects]
ADD CONSTRAINT [FK_HiProgramProject]
    FOREIGN KEY ([HiProgramId])
    REFERENCES [dbo].[HiPrograms]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_HiProgramProject'
CREATE INDEX [IX_FK_HiProgramProject]
ON [dbo].[Projects]
    ([HiProgramId]);
GO

-- Creating foreign key on [EthincGroupId] in table 'Projects'
ALTER TABLE [dbo].[Projects]
ADD CONSTRAINT [FK_EthincGroupProject]
    FOREIGN KEY ([EthincGroupId])
    REFERENCES [dbo].[EthnicGroups]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_EthincGroupProject'
CREATE INDEX [IX_FK_EthincGroupProject]
ON [dbo].[Projects]
    ([EthincGroupId]);
GO

-- Creating foreign key on [JabsomDeptId] in table 'Projects'
ALTER TABLE [dbo].[Projects]
ADD CONSTRAINT [FK_JabsomDeptProject]
    FOREIGN KEY ([JabsomDeptId])
    REFERENCES [dbo].[JabsomDepts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_JabsomDeptProject'
CREATE INDEX [IX_FK_JabsomDeptProject]
ON [dbo].[Projects]
    ([JabsomDeptId]);
GO

-- Creating foreign key on [JabsomOfficeId] in table 'Projects'
ALTER TABLE [dbo].[Projects]
ADD CONSTRAINT [FK_JabsomOfficeProject]
    FOREIGN KEY ([JabsomOfficeId])
    REFERENCES [dbo].[JabsomOffices]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_JabsomOfficeProject'
CREATE INDEX [IX_FK_JabsomOfficeProject]
ON [dbo].[Projects]
    ([JabsomOfficeId]);
GO

-- Creating foreign key on [JabsomAffilId] in table 'Projects'
ALTER TABLE [dbo].[Projects]
ADD CONSTRAINT [FK_JabsomAffilProject]
    FOREIGN KEY ([JabsomAffilId])
    REFERENCES [dbo].[JabsomAffils]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_JabsomAffilProject'
CREATE INDEX [IX_FK_JabsomAffilProject]
ON [dbo].[Projects]
    ([JabsomAffilId]);
GO

-- Creating foreign key on [BioStatId] in table 'Publications'
ALTER TABLE [dbo].[Publications]
ADD CONSTRAINT [FK_BioStatPublication]
    FOREIGN KEY ([BioStatId])
    REFERENCES [dbo].[BioStats]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_BioStatPublication'
CREATE INDEX [IX_FK_BioStatPublication]
ON [dbo].[Publications]
    ([BioStatId]);
GO

-- Creating foreign key on [GrantAffilId] in table 'Publications'
ALTER TABLE [dbo].[Publications]
ADD CONSTRAINT [FK_GrantAffilPublication]
    FOREIGN KEY ([GrantAffilId])
    REFERENCES [dbo].[GrantAffils]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_GrantAffilPublication'
CREATE INDEX [IX_FK_GrantAffilPublication]
ON [dbo].[Publications]
    ([GrantAffilId]);
GO

-- Creating foreign key on [JounralId] in table 'Publications'
ALTER TABLE [dbo].[Publications]
ADD CONSTRAINT [FK_JounralInfoPublication1]
    FOREIGN KEY ([JounralId])
    REFERENCES [dbo].[Jounrals]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_JounralInfoPublication1'
CREATE INDEX [IX_FK_JounralInfoPublication1]
ON [dbo].[Publications]
    ([JounralId]);
GO

-- Creating foreign key on [ProjectId] in table 'Publications'
ALTER TABLE [dbo].[Publications]
ADD CONSTRAINT [FK_ProjectPublication]
    FOREIGN KEY ([ProjectId])
    REFERENCES [dbo].[Projects]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ProjectPublication'
CREATE INDEX [IX_FK_ProjectPublication]
ON [dbo].[Publications]
    ([ProjectId]);
GO

-- Creating foreign key on [InvestId] in table 'GrantApps'
ALTER TABLE [dbo].[GrantApps]
ADD CONSTRAINT [FK_InvestGrantApp]
    FOREIGN KEY ([InvestId])
    REFERENCES [dbo].[Invests]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_InvestGrantApp'
CREATE INDEX [IX_FK_InvestGrantApp]
ON [dbo].[GrantApps]
    ([InvestId]);
GO

-- Creating foreign key on [ProjectId] in table 'GrantApps'
ALTER TABLE [dbo].[GrantApps]
ADD CONSTRAINT [FK_ProjectGrantApp]
    FOREIGN KEY ([ProjectId])
    REFERENCES [dbo].[Projects]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ProjectGrantApp'
CREATE INDEX [IX_FK_ProjectGrantApp]
ON [dbo].[GrantApps]
    ([ProjectId]);
GO

-- Creating foreign key on [ProjectId] in table 'TimeEntries'
ALTER TABLE [dbo].[TimeEntries]
ADD CONSTRAINT [FK_ProjectFact]
    FOREIGN KEY ([ProjectId])
    REFERENCES [dbo].[Projects]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ProjectFact'
CREATE INDEX [IX_FK_ProjectFact]
ON [dbo].[TimeEntries]
    ([ProjectId]);
GO

-- Creating foreign key on [BioStatId] in table 'TimeEntries'
ALTER TABLE [dbo].[TimeEntries]
ADD CONSTRAINT [FK_BioStatFact]
    FOREIGN KEY ([BioStatId])
    REFERENCES [dbo].[BioStats]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_BioStatFact'
CREATE INDEX [IX_FK_BioStatFact]
ON [dbo].[TimeEntries]
    ([BioStatId]);
GO

-- Creating foreign key on [DateId] in table 'TimeEntries'
ALTER TABLE [dbo].[TimeEntries]
ADD CONSTRAINT [FK_DateFact]
    FOREIGN KEY ([DateId])
    REFERENCES [dbo].[Dates]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_DateFact'
CREATE INDEX [IX_FK_DateFact]
ON [dbo].[TimeEntries]
    ([DateId]);
GO

-- Creating foreign key on [ServiceTypeId] in table 'TimeEntries'
ALTER TABLE [dbo].[TimeEntries]
ADD CONSTRAINT [FK_ServiceTypeProjectTracker]
    FOREIGN KEY ([ServiceTypeId])
    REFERENCES [dbo].[ServiceTypes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ServiceTypeProjectTracker'
CREATE INDEX [IX_FK_ServiceTypeProjectTracker]
ON [dbo].[TimeEntries]
    ([ServiceTypeId]);
GO

-- Creating foreign key on [JounralId] in table 'Publications'
ALTER TABLE [dbo].[Publications]
ADD CONSTRAINT [FK_JounralPublication]
    FOREIGN KEY ([JounralId])
    REFERENCES [dbo].[Jounrals]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_JounralPublication'
CREATE INDEX [IX_FK_JounralPublication]
ON [dbo].[Publications]
    ([JounralId]);
GO

-- Creating foreign key on [ConferenceId] in table 'Publications'
ALTER TABLE [dbo].[Publications]
ADD CONSTRAINT [FK_ConferencePublication]
    FOREIGN KEY ([ConferenceId])
    REFERENCES [dbo].[Conferences]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ConferencePublication'
CREATE INDEX [IX_FK_ConferencePublication]
ON [dbo].[Publications]
    ([ConferenceId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------
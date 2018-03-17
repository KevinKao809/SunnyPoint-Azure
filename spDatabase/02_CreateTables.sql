USE [SunnyPoint]
GO

/****** Object: Table [SunnyPoint].[DevicesProvision] Script Date: 2016/8/27 上午 11:02:13 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


IF OBJECT_ID(N'[SunnyPoint].[AccountProfiles]', N'U') IS NOT NULL
BEGIN
  DROP TABLE [SunnyPoint].[AccountProfiles];
END
CREATE TABLE [SunnyPoint].[AccountProfiles] (
    [Id]                INT            IDENTITY (1, 1) NOT NULL,
    [FirstName]         NVARCHAR (128) NULL,
    [LastName]          NVARCHAR (128) NULL,
	[TimeZone]			INT			   NULL,
    [LoginID]           NVARCHAR (128) NULL,
    [LoginPassword]     NVARCHAR (128) NULL,
    [ProfilePictureUri] NVARCHAR (128) NULL,
    [isCompany]         BIT            DEFAULT ((0)) NULL,
    [Rating]            INT            NULL,
    [Ranking]           INT            NULL,
    [Trips]             INT            NULL,
    [Distance]          FLOAT (53)     NULL,
    [DriveTimeInMin]    INT            NULL,
    [HardBreaks]        INT            NULL,
    [HardAccelerations] INT            NULL,
    [FuelConsumption]   FLOAT (53)     NULL,
    [MaxSpeed]          FLOAT (53)     NULL,
    [AverageSpeed]      FLOAT (53)     NULL,
    [OverSpeeds]         INT            NULL,
    [MidnightDriveInMin]     INT            NULL,
    [CreatedAt]         DATETIME       DEFAULT (getdate()) NULL,
    [UpdatedAt]         DATETIME       DEFAULT (getdate()) NULL,
    [Deleted]           BIT            DEFAULT ((0)) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

GO
CREATE NONCLUSTERED INDEX [IX_AccountProfiles_Column]
    ON [SunnyPoint].[AccountProfiles]([Id] ASC, [LoginID] ASC, [LoginPassword] ASC, [isCompany] ASC, [Deleted] ASC);
GO

IF OBJECT_ID(N'[SunnyPoint].[Devices]', N'U') IS NOT NULL
BEGIN
  DROP TABLE [SunnyPoint].[Devices];
END
CREATE TABLE [SunnyPoint].[Devices] (
    [IoTHubDeviceID]        NVARCHAR (128) NOT NULL,
    [IoTHubHostName]        NVARCHAR (128) NULL,
    [IoTHubProtocol]        NVARCHAR (128) NULL,
    [isActive]              BIT            DEFAULT ((0)) NULL,
    [AccountID]             INT            DEFAULT ((0)) NULL,
    [AccelerationAxis]      NVARCHAR (5)   DEFAULT ('X') NULL,
    [HardAccelerationValue] FLOAT (53)     DEFAULT ((2)) NULL,
    [HardBreakValue]        FLOAT (53)     DEFAULT ((-2)) NULL,
    [RegisterCountry]       NVARCHAR (10)  NULL,
    [CreatedAt]             DATETIME       DEFAULT (getdate()) NULL,
    [UpdatedAt]             DATETIME       DEFAULT (getdate()) NULL,
    [Deleted]               BIT            DEFAULT ((0)) NULL,
    PRIMARY KEY CLUSTERED ([IoTHubDeviceID] ASC)
);

GO
CREATE NONCLUSTERED INDEX [IX_Devices_Column]
    ON [SunnyPoint].[Devices]([IoTHubDeviceID] ASC, [AccountID] ASC, [Deleted] ASC, [isActive] ASC);
	

IF OBJECT_ID(N'[SunnyPoint].[IoTHubs]', N'U') IS NOT NULL
BEGIN
  DROP TABLE [SunnyPoint].[IoTHubs];
END
CREATE TABLE [SunnyPoint].[IoTHubs] (
    [IoTHubHostName]                  NVARCHAR (128) NOT NULL,
    [PrimaryIoTHubConnectionString]   NVARCHAR (256) NULL,
    [SecondaryIoTHubConnectionString] NVARCHAR (256) NULL,
    [CreatedAt]                       DATETIME       DEFAULT (getdate()) NULL,
    [UpdatedAt]                       DATETIME       DEFAULT (getdate()) NULL,
    [Deleted]                         BIT            DEFAULT ((0)) NULL,
    PRIMARY KEY CLUSTERED ([IoTHubHostName] ASC)
);

GO
CREATE NONCLUSTERED INDEX [IX_IoTHubs_Column]
    ON [SunnyPoint].[IoTHubs]([IoTHubHostName] ASC, [Deleted] ASC);
GO

IF OBJECT_ID(N'[SunnyPoint].[TripPoints]', N'U') IS NOT NULL
BEGIN
  DROP TABLE [SunnyPoint].[TripPoints];
END
CREATE TABLE [SunnyPoint].[TripPoints] (
    [Id]                INT             IDENTITY (1, 1) NOT NULL,
    [TripID]            NVARCHAR (128)  NOT NULL,
    [CarID]             NVARCHAR (128)  NULL,
    [IoTHubDeviceID]    NVARCHAR (128)  NOT NULL,
    [MessageType]       NVARCHAR (20)   NULL,
    [RecordedTimeStamp] BIGINT          NULL,
    [Country]           NVARCHAR (10)   NULL,
    [LocalTime]         DATETIME        NULL,
    [MidNightDrive]     BIT             NULL,
    [Altitude]          DECIMAL (12, 9) DEFAULT ((0)) NULL,
    [Latitude]          DECIMAL (12, 9) DEFAULT ((0)) NULL,
    [Longitude]         DECIMAL (12, 9) DEFAULT ((0)) NULL,
    [Speed]             FLOAT (53)      NULL,
    [SpeedLimit]        FLOAT (53)      NULL,
    [OverSpeed]         BIT             NULL,
    [RPM]               FLOAT (53)      NULL,
    [AccelerationX]     FLOAT (53)      NULL,
    [AccelerationY]     FLOAT (53)      NULL,
    [AccelerationZ]     FLOAT (53)      NULL,
    [AccelerationXYZ]   FLOAT (53)      NULL,
    [MAF]               FLOAT (53)      NULL,
    [Temp]              FLOAT (53)      NULL,
    [IdlingStartTime]   BIGINT          NULL,
    [IdlingEndTime]     BIGINT          NULL,
    [IdlingTime]        BIGINT          NULL,
    [TripStartTime]     BIGINT          NULL,
    [TripEndTime]       BIGINT          NULL,
    [TripTime]          BIGINT          NULL,
    [CreatedAt]         DATETIME        DEFAULT (getdate()) NULL,
    [UpdatedAt]         DATETIME        DEFAULT (getdate()) NULL,
    [Deleted]           BIT             DEFAULT ((0)) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

GO
CREATE NONCLUSTERED INDEX [IX_TripPoints_Column]
    ON [SunnyPoint].[TripPoints]([TripID] ASC, [IoTHubDeviceID] ASC, [MessageType] ASC, [RecordedTimeStamp] ASC, [Deleted] ASC);
GO

IF OBJECT_ID(N'[SunnyPoint].[Trips]', N'U') IS NOT NULL
BEGIN
  DROP TABLE [SunnyPoint].[Trips];
END
CREATE TABLE [SunnyPoint].[Trips] (
    [Id]                  NVARCHAR (128)  NOT NULL,
    [CarID]               NVARCHAR (128)  NULL,
    [IoTHubDeviceID]      NVARCHAR (128)  NOT NULL,
    [AccountID]           INT             DEFAULT ((0)) NULL,
    [StartTimeStamp]      BIGINT          NOT NULL,
    [EndTimeStamp]        BIGINT          NULL,
    [StartDateTime]       DATETIME        NULL,
    [StartCountry]        NVARCHAR (20)   NULL,
    [EndCountry]          NVARCHAR (20)   NULL,
    [isComplete]          BIT             DEFAULT ((0)) NULL,
    [Distance]            FLOAT (53)      DEFAULT ((0)) NULL,
    [HardBreaks]          INT             DEFAULT ((0)) NULL,
    [HardAccelerations]   INT             DEFAULT ((0)) NULL,
    [OverSpeeds]          INT             DEFAULT ((0)) NULL,
    [MaxSpeed]            FLOAT (53)      DEFAULT ((0)) NULL,
    [AverageSpeed]        FLOAT (53)      DEFAULT ((0)) NULL,
    [MidNightDriveInSec]  INT             DEFAULT ((0)) NULL,
    [DriveTimeInSec]      INT             DEFAULT ((0)) NULL,
    [IdelingTimeInSec]    INT             NULL,
    [ProcessCompleteFlag] BIT             DEFAULT ((0)) NULL,
    [Rating]              INT             DEFAULT ((0)) NULL,
    [CenterLat]           DECIMAL (12, 9) NULL,
    [CenterLng]           DECIMAL (12, 9) NULL,
    [MaxLat]              DECIMAL (12, 9) NULL,
    [Maxlng]              DECIMAL (12, 9) NULL,
    [MinLat]              DECIMAL (12, 9) NULL,
    [MinLng]              DECIMAL (12, 9) NULL,
    [CreatedAt]           DATETIME        DEFAULT (getdate()) NULL,
    [UpdatedAt]           DATETIME        DEFAULT (getdate()) NULL,
    [Deleted]             BIT             DEFAULT ((0)) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

GO
CREATE NONCLUSTERED INDEX [IX_Trips_Column]
    ON [SunnyPoint].[Trips]([IoTHubDeviceID] ASC, [AccountID] ASC, [StartTimeStamp] ASC, [isComplete] ASC, [ProcessCompleteFlag] ASC, [Deleted] ASC, StartDateTime ASC);


IF OBJECT_ID(N'[SunnyPoint].[DashboardMonthlyData]', N'U') IS NOT NULL
BEGIN
  DROP TABLE [SunnyPoint].[DashboardMonthlyData];
END
CREATE TABLE [SunnyPoint].[DashboardMonthlyData]
(
	[Id]             INT           IDENTITY (1, 1) NOT NULL,
    [AccountID]      INT           NOT NULL,
    [YearMonth]          NVARCHAR (50) NULL,
    [DurationInMin]       INT           NULL,
    [Mileage]        FLOAT (53)    NULL,
    [Score]          INT           NULL,
    [NegativeEvents] INT           NULL,
    [Trips]          INT           NULL, 
	[CreatedAt]      DATETIME      DEFAULT (getdate()) NULL,
    [UpdatedAt]      DATETIME      DEFAULT (getdate()) NULL,
    [Deleted]        BIT           DEFAULT ((0)) NULL,
    CONSTRAINT [PK_DashboardMonthlyData] PRIMARY KEY ([Id]), 
)
GO

CREATE INDEX [IX_DashboardMonthlyData_Column] ON [SunnyPoint].[DashboardMonthlyData] (AccountID, YearMonth)

IF OBJECT_ID(N'[SunnyPoint].[DashboardCurrentData]', N'U') IS NOT NULL
BEGIN
  DROP TABLE [SunnyPoint].[DashboardCurrentData];
END
CREATE TABLE [SunnyPoint].[DashboardCurrentData] (
    [Id]                          INT        IDENTITY (1, 1) NOT NULL,
    [AccountID]                   INT        NOT NULL,
    [CurrentMonthScore]           INT        NULL,
    [PreviousMonthScore]          INT        NULL,
    [CurrentMonthNegativeEvents]  INT        NULL,
    [PreviousMonthNegativeEvents] INT        NULL,
	[CurrentMonthDayTrips]        INT        NULL,
    [PreviousMonthDayTrips]       INT        NULL,
    [CurrentMonthDayMileage]      FLOAT (53)        NULL,
    [PreviousMonthDayMileage]     FLOAT (53)       NULL,
    [HardBreaks]                  INT        NULL,
    [HardAccelerations]           INT        NULL,
    [OverSpeed]                   INT        NULL,
    [Day_NegativeEvents]          FLOAT (53) NULL,
    [Midnight_NegativeEvents]     FLOAT (53) NULL,
    [CreatedAt]                   DATETIME   DEFAULT (getdate()) NULL,
    [UpdatedAt]                   DATETIME   DEFAULT (getdate()) NULL,
    [Deleted]                     BIT        DEFAULT ((0)) NULL,
    CONSTRAINT [PK_DashboardCurrentData] PRIMARY KEY CLUSTERED ([Id] ASC)
);

GO
CREATE NONCLUSTERED INDEX [IX_DashboardCurrentData_Column]
    ON [SunnyPoint].[DashboardCurrentData]([AccountID] ASC);



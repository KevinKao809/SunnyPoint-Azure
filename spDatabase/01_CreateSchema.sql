IF OBJECT_ID(N'[SunnyPoint].[AccountProfiles]', N'U') IS NOT NULL
BEGIN
  DROP TABLE [SunnyPoint].[AccountProfiles];
END
GO

IF OBJECT_ID(N'[SunnyPoint].[DevicesProvisioning]', N'U') IS NOT NULL
BEGIN
  DROP TABLE [SunnyPoint].[DevicesProvisioning];
END
GO

IF OBJECT_ID(N'[SunnyPoint].[Devices]', N'U') IS NOT NULL
BEGIN
  DROP TABLE [SunnyPoint].[Devices];
END
GO

IF OBJECT_ID(N'[SunnyPoint].[Trips]', N'U') IS NOT NULL
BEGIN
  DROP TABLE [SunnyPoint].[Trips];
END
GO

IF OBJECT_ID(N'[SunnyPoint].[TripPoints]', N'U') IS NOT NULL
BEGIN
  DROP TABLE [SunnyPoint].[TripPoints];
END
GO

IF OBJECT_ID(N'[SunnyPoint].[IoTHubs]', N'U') IS NOT NULL
BEGIN
  DROP TABLE [SunnyPoint].[IoTHubs];
END
GO

IF OBJECT_ID(N'[SunnyPoint].[DashboardMonthlyData]', N'U') IS NOT NULL
BEGIN
  DROP TABLE [SunnyPoint].[DashboardMonthlyData];
END

IF OBJECT_ID(N'[SunnyPoint].[DashboardCurrentData]', N'U') IS NOT NULL
BEGIN
  DROP TABLE [SunnyPoint].[DashboardCurrentData];
END

Drop Schema SunnyPoint
Go

CREATE SCHEMA SunnyPoint AUTHORIZATION dbo
Go
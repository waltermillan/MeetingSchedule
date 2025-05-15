USE [MeetingScheduleDb]
GO

/****** Object:  Table [dbo].[users]    Script Date: 16/5/2025 01:11:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[users](
	[id] [uniqueidentifier] NOT NULL,
	[user_name] [nvarchar](max) NOT NULL,
	[name] [nvarchar](max) NOT NULL,
	[password] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_users] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


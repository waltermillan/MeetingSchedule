USE [MeetingScheduleDb]
GO

/****** Object:  Table [dbo].[contact_tags]    Script Date: 16/5/2025 01:11:21 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[contact_tags](
	[id] [uniqueidentifier] NOT NULL,
	[contact_id] [uniqueidentifier] NOT NULL,
	[tag_id] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_contact_tags] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[contact_tags]  WITH CHECK ADD  CONSTRAINT [FK_contact_tags_contacts_contact_id] FOREIGN KEY([contact_id])
REFERENCES [dbo].[contacts] ([id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[contact_tags] CHECK CONSTRAINT [FK_contact_tags_contacts_contact_id]
GO

ALTER TABLE [dbo].[contact_tags]  WITH CHECK ADD  CONSTRAINT [FK_contact_tags_tags_tag_id] FOREIGN KEY([tag_id])
REFERENCES [dbo].[tags] ([id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[contact_tags] CHECK CONSTRAINT [FK_contact_tags_tags_tag_id]
GO


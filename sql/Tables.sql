USE [TestDb]
GO

CREATE SCHEMA [CNVRT]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [CNVRT].[ConvertRequests](
	[RequestKey] [uniqueidentifier] NOT NULL,
	[InputFileId] [int] NULL,
	[OutputFileId] [int] NULL,
 CONSTRAINT [PK_ConvertRequests] PRIMARY KEY CLUSTERED 
(
	[RequestKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [CNVRT].[WorkerStorageFiles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[StorageKey] [nvarchar](500) NOT NULL,
	[StorageType] [int] NOT NULL,
	[FileType] [int] NOT NULL,
	[CreationDate] [datetime] NOT NULL,
 CONSTRAINT [PK_WorkerStorageFiles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [CNVRT].[WorkerStorageFiles] ADD  CONSTRAINT [DF_WSF_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
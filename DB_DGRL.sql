USE [DB_DGRL]
GO
/****** Object:  User [tu]    Script Date: 5/3/2024 11:39:18 AM ******/
CREATE USER [tu] FOR LOGIN [tu] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  Table [dbo].[AccountAdmin]    Script Date: 5/3/2024 11:39:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AccountAdmin](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FullName] [nvarchar](50) NULL,
	[UserName] [varchar](20) NULL,
	[Password] [varchar](255) NULL,
	[CreateBy] [varchar](20) NULL,
	[CreateDate] [datetime] NULL,
	[UpdateDate] [datetime] NULL,
	[IsActive] [tinyint] NULL,
	[RoleId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[UserName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AccountLecturer]    Script Date: 5/3/2024 11:39:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AccountLecturer](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [varchar](10) NULL,
	[Password] [varchar](255) NULL,
	[CreateBy] [varchar](20) NULL,
	[CreateDate] [datetime] NULL,
	[UpdateDate] [datetime] NULL,
	[IsActive] [tinyint] NULL,
	[LecturerId] [char](10) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[UserName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AccountStudent]    Script Date: 5/3/2024 11:39:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AccountStudent](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [varchar](10) NULL,
	[Password] [varchar](255) NULL,
	[CreateBy] [varchar](20) NULL,
	[CreateDate] [datetime] NULL,
	[UpdateDate] [datetime] NULL,
	[IsActive] [tinyint] NULL,
	[StudentId] [char](10) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[UserName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AnswerList]    Script Date: 5/3/2024 11:39:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AnswerList](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[QuestionId] [int] NULL,
	[ContentAnswer] [nvarchar](500) NULL,
	[AnswerScore] [int] NULL,
	[Status] [tinyint] NULL,
	[CreateDate] [datetime] NULL,
	[UpdateDate] [datetime] NULL,
	[UpdateBy] [nvarchar](50) NULL,
	[Checked] [tinyint] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Class]    Script Date: 5/3/2024 11:39:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Class](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [char](7) NULL,
	[CourseId] [char](4) NULL,
	[DepartmentId] [int] NULL,
	[IsActive] [tinyint] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ClassAnswer]    Script Date: 5/3/2024 11:39:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ClassAnswer](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[StudentId] [char](10) NULL,
	[AnswerId] [int] NULL,
	[CreateBy] [char](10) NULL,
	[CreateDate] [datetime] NULL,
	[SemesterId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Classify]    Script Date: 5/3/2024 11:39:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Classify](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[PointMin] [int] NULL,
	[PointMax] [int] NULL,
	[OrderBy] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Course]    Script Date: 5/3/2024 11:39:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Course](
	[Id] [char](4) NOT NULL,
	[Name] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Department]    Script Date: 5/3/2024 11:39:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Department](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Times] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GroupQuestion]    Script Date: 5/3/2024 11:39:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GroupQuestion](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Lecturer]    Script Date: 5/3/2024 11:39:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Lecturer](
	[Id] [char](10) NOT NULL,
	[FullName] [nvarchar](50) NULL,
	[DepartmentId] [int] NULL,
	[PositionId] [char](3) NULL,
	[Birthday] [date] NULL,
	[Email] [varchar](50) NULL,
	[Phone] [varchar](15) NULL,
	[IsActive] [tinyint] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Position]    Script Date: 5/3/2024 11:39:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Position](
	[Id] [char](3) NOT NULL,
	[Name] [nvarchar](20) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[QuestionHisory]    Script Date: 5/3/2024 11:39:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[QuestionHisory](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[QuestionId] [int] NULL,
	[SemesterId] [int] NULL,
	[OrderBy] [int] NULL,
	[CreateBy] [char](10) NULL,
	[CreateDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[QuestionList]    Script Date: 5/3/2024 11:39:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[QuestionList](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ContentQuestion] [nvarchar](500) NULL,
	[TypeQuestionId] [int] NULL,
	[GroupQuestionId] [int] NULL,
	[OrderBy] [int] NULL,
	[Status] [tinyint] NULL,
	[CreateDate] [datetime] NULL,
	[UpdateDate] [datetime] NULL,
	[UpdateBy] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 5/3/2024 11:39:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [nvarchar](100) NULL,
	[IsActive] [tinyint] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SelfAnswer]    Script Date: 5/3/2024 11:39:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SelfAnswer](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[StudentId] [char](10) NULL,
	[AnswerId] [int] NULL,
	[SemesterId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Semester]    Script Date: 5/3/2024 11:39:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Semester](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [char](2) NULL,
	[SchoolYear] [char](10) NULL,
	[DateOpenStudent] [datetime] NULL,
	[DateEndStudent] [datetime] NULL,
	[DateEndClass] [datetime] NULL,
	[DateEndLecturer] [datetime] NULL,
	[IsActive] [tinyint] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Student]    Script Date: 5/3/2024 11:39:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Student](
	[Id] [char](10) NOT NULL,
	[FullName] [nvarchar](50) NULL,
	[Birthday] [date] NULL,
	[Email] [varchar](50) NULL,
	[Phone] [varchar](15) NULL,
	[Gender] [bit] NULL,
	[ClassId] [int] NULL,
	[PositionId] [char](3) NULL,
	[IsActive] [tinyint] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SumaryOfPoint]    Script Date: 5/3/2024 11:39:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SumaryOfPoint](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[StudentId] [char](10) NULL,
	[SemesterId] [int] NULL,
	[SelfPoint] [int] NULL,
	[ClassPoint] [int] NULL,
	[LecturerPoint] [int] NULL,
	[Classify] [nvarchar](50) NULL,
	[UserClass] [char](10) NULL,
	[UserLecturer] [char](10) NULL,
	[UpdateDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TypeQuestion]    Script Date: 5/3/2024 11:39:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TypeQuestion](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AccountAdmin]  WITH CHECK ADD FOREIGN KEY([RoleId])
REFERENCES [dbo].[Roles] ([Id])
GO
ALTER TABLE [dbo].[AccountLecturer]  WITH CHECK ADD FOREIGN KEY([LecturerId])
REFERENCES [dbo].[Lecturer] ([Id])
GO
ALTER TABLE [dbo].[AccountStudent]  WITH CHECK ADD FOREIGN KEY([StudentId])
REFERENCES [dbo].[Student] ([Id])
GO
ALTER TABLE [dbo].[AnswerList]  WITH CHECK ADD FOREIGN KEY([QuestionId])
REFERENCES [dbo].[QuestionList] ([Id])
GO
ALTER TABLE [dbo].[Class]  WITH CHECK ADD FOREIGN KEY([CourseId])
REFERENCES [dbo].[Course] ([Id])
GO
ALTER TABLE [dbo].[Class]  WITH CHECK ADD FOREIGN KEY([DepartmentId])
REFERENCES [dbo].[Department] ([Id])
GO
ALTER TABLE [dbo].[ClassAnswer]  WITH CHECK ADD FOREIGN KEY([AnswerId])
REFERENCES [dbo].[AnswerList] ([Id])
GO
ALTER TABLE [dbo].[ClassAnswer]  WITH CHECK ADD FOREIGN KEY([SemesterId])
REFERENCES [dbo].[Semester] ([Id])
GO
ALTER TABLE [dbo].[ClassAnswer]  WITH CHECK ADD FOREIGN KEY([StudentId])
REFERENCES [dbo].[Student] ([Id])
GO
ALTER TABLE [dbo].[Lecturer]  WITH CHECK ADD FOREIGN KEY([DepartmentId])
REFERENCES [dbo].[Department] ([Id])
GO
ALTER TABLE [dbo].[Lecturer]  WITH CHECK ADD FOREIGN KEY([PositionId])
REFERENCES [dbo].[Position] ([Id])
GO
ALTER TABLE [dbo].[QuestionHisory]  WITH CHECK ADD FOREIGN KEY([QuestionId])
REFERENCES [dbo].[QuestionList] ([Id])
GO
ALTER TABLE [dbo].[QuestionHisory]  WITH CHECK ADD FOREIGN KEY([SemesterId])
REFERENCES [dbo].[Semester] ([Id])
GO
ALTER TABLE [dbo].[QuestionList]  WITH CHECK ADD FOREIGN KEY([GroupQuestionId])
REFERENCES [dbo].[GroupQuestion] ([Id])
GO
ALTER TABLE [dbo].[QuestionList]  WITH CHECK ADD FOREIGN KEY([TypeQuestionId])
REFERENCES [dbo].[TypeQuestion] ([Id])
GO
ALTER TABLE [dbo].[SelfAnswer]  WITH CHECK ADD FOREIGN KEY([AnswerId])
REFERENCES [dbo].[AnswerList] ([Id])
GO
ALTER TABLE [dbo].[SelfAnswer]  WITH CHECK ADD FOREIGN KEY([SemesterId])
REFERENCES [dbo].[Semester] ([Id])
GO
ALTER TABLE [dbo].[SelfAnswer]  WITH CHECK ADD FOREIGN KEY([StudentId])
REFERENCES [dbo].[Student] ([Id])
GO
ALTER TABLE [dbo].[Student]  WITH CHECK ADD FOREIGN KEY([ClassId])
REFERENCES [dbo].[Class] ([Id])
GO
ALTER TABLE [dbo].[Student]  WITH CHECK ADD FOREIGN KEY([PositionId])
REFERENCES [dbo].[Position] ([Id])
GO
ALTER TABLE [dbo].[SumaryOfPoint]  WITH CHECK ADD FOREIGN KEY([SemesterId])
REFERENCES [dbo].[Semester] ([Id])
GO
ALTER TABLE [dbo].[SumaryOfPoint]  WITH CHECK ADD FOREIGN KEY([StudentId])
REFERENCES [dbo].[Student] ([Id])
GO
INSERT INTO  STUDENT VALUES		
('2110900005',N'Nguyễn Thị Vân Anh','20030210','','',1,1,'SV',1),		
('2110900001',N'Nguyễn Hồng An','20031221','','',0,1,'SV',1),		
('2110900069',N'Nguyễn Văn An','20030928','','',0,1,'SV',1),		
('2110900008',N'Nguyễn Viết Chiến','20030318','','',0,1,'SV',1),		
('2110900064',N'Bùi Đức Duy','20030622','','',0,1,'LT',1)	,	
('2110900071',N'Đỗ Ngọc Quý Dương','20031218','','',0,1,'SV',1),		
('2110900062',N'Nguyễn Hải Đăng','20031127','','',0,1,'SV',1),		
('2110900011',N'Quách Ngọc Đức','20030928','','',0,1,'SV',1),		
('2110900020',N'Phạm Quốc Hiệu','20030130','','',0,1,'LT',1),		
('2110900053',N'Trần Hữu Hoàng','20030716','','',0,1,'SV',1),		
('2110900027',N'Nguyễn Văn Khánh','20030914','','',0,1,'SV',1),		
('2110900057',N'Nguyễn Công Lý','20030708','','',0,1,'SV',1),		
('2110900059',N'Phạm Quang Quý','20031126','','',0,1,'LT',1),		
('2110900035',N'Nguyễn Hữu Sang','20030508','','',0,1,'SV',1),		
('2110900090',N'Vũ Hồng Sơn','20030821','','',0,1,'SV',1),			
('2110900072',N'Hoàng Văn Thành','20031031','','',0,1,'SV',1),		
('2110900076',N'Nguyễn Xuân Tiến','20031107','','',0,1,'LT',1),		
('2110900101',N'Nguyễn Văn Toại','20031125','','',0,1,'SV',1),		
('2110900060',N'Phạm Minh Tuấn','20030104','','',0,1,'SV',1),		
('2110900056',N'Nguyễn Đình Thanh Tùng','20031021','','',0,1,'SV',1),			
('2110900078',N'Đào Mạnh Đức Anh','20030310','','',0,1,'SV',1),		
('2110900007',N'Trần Ngọc Bích','20031205','','',1,1,'SV',1),		
('2110900015',N'Nguyễn Bá Dương','20030820','','',0,1,'SV',1),		
('2110900092',N'Lê Đình Dũng','20030227','','',0,1,'SV',1),			
('2110900051',N'Trần Công Định','20030227','','',0,1,'SV',1),		
('2110900066',N'Nguyễn Quốc Khánh','20030902','','',0,1,'SV',1)	
INSERT INTO  AccountStudent (USERNAME,PASSWORD,STUDENTID) VALUES		
('2110900005',N'12345','2110900005'),		
('2110900001',N'12345','2110900001'),		
('2110900069',N'12345','2110900069'),		
('2110900008',N'12345','2110900008'),		
('2110900064',N'12345','2110900064'),		
('2110900071',N'12345','2110900071'),		
('2110900062',N'12345','2110900062'),		
('2110900011',N'12345','2110900011'),		
('2110900020',N'12345','2110900020'),		
('2110900053',N'12345','2110900053'),		
('2110900027',N'12345','2110900027'),		
('2110900057',N'12345','2110900057'),		
('2110900059',N'12345','2110900059'),		
('2110900035',N'12345','2110900035'),		
('2110900090',N'12345','2110900090'),		
('2110900072',N'12345','2110900072'),		
('2110900076',N'12345','2110900076'),		
('2110900101',N'12345','2110900101'),		
('2110900060',N'12345','2110900060'),		
('2110900056',N'12345','2110900056'),		
('2110900078',N'12345','2110900078'),		
('2110900007',N'12345','2110900007'),		
('2110900015',N'12345','2110900015'),		
('2110900092',N'12345','2110900092'),		
('2110900051',N'12345','2110900051'),		
('2110900066',N'12345','2110900066')	
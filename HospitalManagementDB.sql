USE [HospitalManagementDB]
GO
/****** Object:  Table [dbo].[Appointments]    Script Date: 10/6/2026 4:13:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Appointments](
	[AppointmentID] [varchar](36) NOT NULL,
	[PatientID] [varchar](36) NULL,
	[DoctorID] [varchar](36) NULL,
	[AppointmentDate] [datetime] NULL,
	[Status] [varchar](50) NOT NULL,
	[Notes] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[AppointmentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Billing]    Script Date: 10/6/2026 4:13:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Billing](
	[BillingID] [varchar](36) NOT NULL,
	[AppointmentID] [varchar](36) NULL,
	[TotalAmount] [decimal](18, 2) NOT NULL,
	[PaymentStatus] [varchar](50) NOT NULL,
	[BillingDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[BillingID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Doctors]    Script Date: 10/6/2026 4:13:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Doctors](
	[DoctorID] [varchar](36) NOT NULL,
	[UserID] [varchar](36) NULL,
	[FirstName] [varchar](100) NOT NULL,
	[LastName] [varchar](100) NOT NULL,
	[Specialty] [varchar](100) NULL,
	[Department] [varchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[DoctorID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Patients]    Script Date: 10/6/2026 4:13:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Patients](
	[PatientID] [varchar](36) NOT NULL,
	[UserID] [varchar](36) NULL,
	[FirstName] [varchar](100) NOT NULL,
	[LastName] [varchar](100) NOT NULL,
	[DateOfBirth] [datetime] NULL,
	[ContactNumber] [varchar](20) NULL,
	[RegisteredAt] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[PatientID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 10/6/2026 4:13:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserID] [varchar](36) NOT NULL,
	[Username] [varchar](100) NOT NULL,
	[PasswordHash] [varchar](255) NOT NULL,
	[Role] [varchar](50) NOT NULL,
	[CreatedAt] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[Appointments] ([AppointmentID], [PatientID], [DoctorID], [AppointmentDate], [Status], [Notes]) VALUES (N'apt-01', N'pat-01', N'doc-01', CAST(N'2023-11-20T14:00:00.000' AS DateTime), N'Completed', N'Patient reported mild chest pain. EKG is normal.')
GO
INSERT [dbo].[Appointments] ([AppointmentID], [PatientID], [DoctorID], [AppointmentDate], [Status], [Notes]) VALUES (N'apt-02', N'pat-01', N'doc-01', CAST(N'2023-12-20T14:00:00.000' AS DateTime), N'Scheduled', N'Follow-up appointment.')
GO
INSERT [dbo].[Appointments] ([AppointmentID], [PatientID], [DoctorID], [AppointmentDate], [Status], [Notes]) VALUES (N'd73e79d2-1b6a-487a-84ed-2c078c7ae636', N'a6d8a369-7fcd-4048-92f7-9f30ab2367b6', N'doc-01', CAST(N'2026-10-07T15:47:00.000' AS DateTime), N'Completed', NULL)
GO
INSERT [dbo].[Billing] ([BillingID], [AppointmentID], [TotalAmount], [PaymentStatus], [BillingDate]) VALUES (N'247246fb-af37-4420-9f4b-ffe9177a096a', N'd73e79d2-1b6a-487a-84ed-2c078c7ae636', CAST(150.00 AS Decimal(18, 2)), N'Pending', CAST(N'2026-10-06T15:48:29.540' AS DateTime))
GO
INSERT [dbo].[Billing] ([BillingID], [AppointmentID], [TotalAmount], [PaymentStatus], [BillingDate]) VALUES (N'bill-01', N'apt-01', CAST(150.00 AS Decimal(18, 2)), N'Paid', CAST(N'2023-11-20T15:00:00.000' AS DateTime))
GO
INSERT [dbo].[Billing] ([BillingID], [AppointmentID], [TotalAmount], [PaymentStatus], [BillingDate]) VALUES (N'd8d24cb9-6cbc-4dcc-841a-5c0b0e1fb33a', N'apt-02', CAST(150.00 AS Decimal(18, 2)), N'Paid', CAST(N'2026-07-16T19:10:55.000' AS DateTime))
GO
INSERT [dbo].[Doctors] ([DoctorID], [UserID], [FirstName], [LastName], [Specialty], [Department]) VALUES (N'doc-01', N'usr-doc-01', N'Robert', N'Smith', N'Cardiology', N'Heart Center')
GO
INSERT [dbo].[Patients] ([PatientID], [UserID], [FirstName], [LastName], [DateOfBirth], [ContactNumber], [RegisteredAt]) VALUES (N'a6d8a369-7fcd-4048-92f7-9f30ab2367b6', N'9c4a2384-80fa-4c19-8e9b-d6b00687ec24', N'Pending', N'Details', NULL, NULL, CAST(N'2026-07-16T17:33:47.000' AS DateTime))
GO
INSERT [dbo].[Patients] ([PatientID], [UserID], [FirstName], [LastName], [DateOfBirth], [ContactNumber], [RegisteredAt]) VALUES (N'pat-01', N'usr-pat-01', N'Jane', N'Doe', CAST(N'1985-06-15T00:00:00.000' AS DateTime), N'555-0198', CAST(N'2023-01-10T10:35:00.000' AS DateTime))
GO
INSERT [dbo].[Users] ([UserID], [Username], [PasswordHash], [Role], [CreatedAt]) VALUES (N'5fe183df-f53d-4990-b1b7-9a1b0c741be0', N'superadmin@hospitalcare.in  ', N'$2a$11$w6zVmw5fDX3l0EfGEVPrSemT3uODIvoIgzxKDI/la8JIF.I0WCOKu', N'Admin', CAST(N'2026-07-07T19:40:11.987' AS DateTime))
GO
INSERT [dbo].[Users] ([UserID], [Username], [PasswordHash], [Role], [CreatedAt]) VALUES (N'9c4a2384-80fa-4c19-8e9b-d6b00687ec24', N'superadmin  ', N'$2a$11$ewThJ2IBGmT0QcD1gz6d/O0vnwlhwpOypxhyVXIsn/GDpf13w7nC.', N'Patient', CAST(N'2026-07-16T17:33:47.860' AS DateTime))
GO
INSERT [dbo].[Users] ([UserID], [Username], [PasswordHash], [Role], [CreatedAt]) VALUES (N'usr-admin-01', N'admin_sarah', N'$2a$11$ewThJ2IBGmT0QcD1gz6d/O0vnwlhwpOypxhyVXIsn/GDpf13w7nC.', N'Admin', CAST(N'2023-01-01T08:00:00.000' AS DateTime))
GO
INSERT [dbo].[Users] ([UserID], [Username], [PasswordHash], [Role], [CreatedAt]) VALUES (N'usr-doc-01', N'dr_smith', N'$2a$11$ewThJ2IBGmT0QcD1gz6d/O0vnwlhwpOypxhyVXIsn/GDpf13w7nC.', N'Doctor', CAST(N'2023-01-02T09:00:00.000' AS DateTime))
GO
INSERT [dbo].[Users] ([UserID], [Username], [PasswordHash], [Role], [CreatedAt]) VALUES (N'usr-pat-01', N'jane_doe', N'$2a$11$ewThJ2IBGmT0QcD1gz6d/O0vnwlhwpOypxhyVXIsn/GDpf13w7nC.', N'Patient', CAST(N'2023-01-10T10:30:00.000' AS DateTime))
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Billing__8ECDFCA3DA618356]    Script Date: 10/6/2026 4:13:10 PM ******/
ALTER TABLE [dbo].[Billing] ADD UNIQUE NONCLUSTERED 
(
	[AppointmentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Doctors__1788CCADD01C92A0]    Script Date: 10/6/2026 4:13:10 PM ******/
ALTER TABLE [dbo].[Doctors] ADD UNIQUE NONCLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Patients__1788CCAD3E76F0E9]    Script Date: 10/6/2026 4:13:10 PM ******/
ALTER TABLE [dbo].[Patients] ADD UNIQUE NONCLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Appointments]  WITH CHECK ADD FOREIGN KEY([DoctorID])
REFERENCES [dbo].[Doctors] ([DoctorID])
GO
ALTER TABLE [dbo].[Appointments]  WITH CHECK ADD FOREIGN KEY([PatientID])
REFERENCES [dbo].[Patients] ([PatientID])
GO
ALTER TABLE [dbo].[Billing]  WITH CHECK ADD FOREIGN KEY([AppointmentID])
REFERENCES [dbo].[Appointments] ([AppointmentID])
GO
ALTER TABLE [dbo].[Doctors]  WITH CHECK ADD FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Patients]  WITH CHECK ADD FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO

# LocalBMtool

Stored Procedure used in Sql for update Query

Create Procedure UpdateTable
@id int,
@Fname varchar(32),
@Lname varchar(32),
@PersonalEmail varchar (32),
@OfficeEmail varchar (32),
@EmployeeType bit,
@DepartmentId int,
@Experience tinyint,
@Phone varchar (15)

As
Begin
		  IF (@PersonalEmail IS NOT NULL AND @PersonalEmail NOT LIKE '%_@__%.__%')
   BEGIN
      RAISERROR('Invalid email address format', 16, 1)
      RETURN
   END
   	  IF (@OfficeEmail IS NOT NULL AND @officeEmail NOT LIKE '%_@__%.__%')
   BEGIN
      RAISERROR('Invalid email address format', 16, 1)
      RETURN
   END

   IF (@phone IS NOT NULL AND @phone NOT LIKE '[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]')
   BEGIN
      RAISERROR('Invalid phone number format', 16, 1)
      RETURN
   END

   update UserStory19Table
  SET Fname = @Fname,
       Lname = @Lname,
       PersonalEmail = @PersonalEmail,
       OfficeEmail = @OfficeEmail,
       EmployeeType = @EmployeeType,
       DepartmentId = @DepartmentId,
       Experience = @Experience,
       Phone = @Phone
   WHERE id = @id

   End

--To Execute Stored Procedure


EXEC UpdateTable
   @id = 1002,
   @Fname = 'John',
   @Lname = 'Doe',
   @PersonalEmail = 'johndoe@gmail.com',
   @OfficeEmail = 'jdoe@company.com',
   @EmployeeType = 1,
   @DepartmentId = 2,
   @Experience = 5,
   @Phone = '1234967890'


   --//To retrieve from table
   select * from UserStory19Table
   
   
   
   Scrpits:
   
   Database:
   
   USE [master]
GO

/****** Object:  Database [BMtool_Demo]    Script Date: 5/15/2023 1:56:16 PM ******/
CREATE DATABASE [BMtool_Demo]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'BMtool_Demo', FILENAME = N'C:\Users\SAILS-DM292\BMtool_Demo.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'BMtool_Demo_log', FILENAME = N'C:\Users\SAILS-DM292\BMtool_Demo_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [BMtool_Demo].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [BMtool_Demo] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [BMtool_Demo] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [BMtool_Demo] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [BMtool_Demo] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [BMtool_Demo] SET ARITHABORT OFF 
GO

ALTER DATABASE [BMtool_Demo] SET AUTO_CLOSE OFF 
GO

ALTER DATABASE [BMtool_Demo] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [BMtool_Demo] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [BMtool_Demo] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [BMtool_Demo] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [BMtool_Demo] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [BMtool_Demo] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [BMtool_Demo] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [BMtool_Demo] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [BMtool_Demo] SET  DISABLE_BROKER 
GO

ALTER DATABASE [BMtool_Demo] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [BMtool_Demo] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [BMtool_Demo] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [BMtool_Demo] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [BMtool_Demo] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [BMtool_Demo] SET READ_COMMITTED_SNAPSHOT OFF 
GO

ALTER DATABASE [BMtool_Demo] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [BMtool_Demo] SET RECOVERY SIMPLE 
GO

ALTER DATABASE [BMtool_Demo] SET  MULTI_USER 
GO

ALTER DATABASE [BMtool_Demo] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [BMtool_Demo] SET DB_CHAINING OFF 
GO

ALTER DATABASE [BMtool_Demo] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO

ALTER DATABASE [BMtool_Demo] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO

ALTER DATABASE [BMtool_Demo] SET DELAYED_DURABILITY = DISABLED 
GO

ALTER DATABASE [BMtool_Demo] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO

ALTER DATABASE [BMtool_Demo] SET QUERY_STORE = OFF
GO

ALTER DATABASE [BMtool_Demo] SET  READ_WRITE 
GO


2.Table:

USE [BMtool_Demo]
GO

/****** Object:  Table [dbo].[UserStory19Table]    Script Date: 5/15/2023 1:59:22 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[UserStory19Table](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FName] [varchar](50) NOT NULL,
	[LName] [varchar](50) NOT NULL,
	[PersonalEmail] [varchar](50) NULL,
	[OfficeEmail] [varchar](50) NOT NULL,
	[EmployeeType] [bit] NULL,
	[DepartmentId] [int] NULL,
	[Experience] [tinyint] NOT NULL,
	[Phone] [varchar](15) NOT NULL,
 CONSTRAINT [PK_UserStory19Table] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO



3.StoredProc

/****** Object:  StoredProcedure [dbo].[UpdateTable]    Script Date: 5/15/2023 2:00:46 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE Procedure [dbo].[UpdateTable]
@id int,
@Fname varchar(32),
@Lname varchar(32),
@PersonalEmail varchar (32),
@OfficeEmail varchar (32),
@EmployeeType bit,
@DepartmentId int,
@Experience tinyint,
@Phone varchar (15)

As
Begin
		  IF (@PersonalEmail IS NOT NULL AND @PersonalEmail NOT LIKE '%_@__%.__%')
   BEGIN
      RAISERROR('Invalid email address format', 16, 1)
      RETURN
   END
   	  IF (@OfficeEmail IS NOT NULL AND @officeEmail NOT LIKE '%_@__%.__%')
   BEGIN
      RAISERROR('Invalid email address format', 16, 1)
      RETURN
   END

   IF (@phone IS NOT NULL AND @phone NOT LIKE '[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]')
   BEGIN
      RAISERROR('Invalid phone number format', 16, 1)
      RETURN
   END

   update UserStory19Table
  SET Fname = @Fname,
       Lname = @Lname,
       PersonalEmail = @PersonalEmail,
       OfficeEmail = @OfficeEmail,
       EmployeeType = @EmployeeType,
       DepartmentId = @DepartmentId,
       Experience = @Experience,
       Phone = @Phone
   WHERE id = @id

   End
GO






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



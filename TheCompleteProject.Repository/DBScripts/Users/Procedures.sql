

ALTER PROCEDURE sp_FetchUserName                     
 @UserId INT 
AS                                             
BEGIN
			-- EXEC sp_FetchUserName 1
SET NOCOUNT ON;                              
SET TRANSACTION ISOLATION LEVEL READ  UNCOMMITTED;  

SELECT u.UserName FROM Users u
WHERE u.Id = @UserId
END
GO


ALTER PROCEDURE sp_AddUsers                     
 @UserName VARCHAR(100) = NULL
,@Email VARCHAR(100) = NULL
,@Password VARCHAR(100) = NULL
,@RoleId INT = NULL
AS
BEGIN
	-- Exec sp_AddUsers 'ADDTESTONE', 'ADDTESTONE@gmail.com', 'admin@123',1
	
	-- IF Exists(select from [Users] c where c.CompanyCode = @CompanyCode) 
  BEGIN TRY
    BEGIN TRAN
	--DECLARE @UserName VARCHAR(100) = 'ADDTESTONE'
	--DECLARE @Email VARCHAR(100) = 'ADDTESTONE@gmail.com'
	--DECLARE @Password VARCHAR(100) = 'admin@123'
	--DECLARE @RoleId INT = 1
		INSERT INTO [dbo].[Users] (
			 [UserName]
			,[Email]
			,[Password]
			,[RoleId]
			,[IsActive]
			,[CreatedBy]
			,[ModifiedBy]
			,[CreatedDate]
			,[ModifiedDate]
			)
		VALUES (
			 @UserName
			,@Email
			,@Password
			,@RoleId
			,1
			,1
			,1
			,getdate()
			,getdate()
			)

		--SELECT 1 AS Message
    COMMIT
 END TRY

	BEGIN CATCH
		ROLLBACK

		DECLARE @ErrorMessage NVARCHAR(max)

		SELECT @ErrorMessage = 'ERROR MESSAGE : ' + convert(NVARCHAR(max), 
		     ERROR_MESSAGE()) + 'ERROR NUMBER : ' + convert(NVARCHAR(max), 
			 ERROR_NUMBER())  + 'ERROR LINE   : ' + convert(NVARCHAR(max), 
			 ERROR_LINE())   + 'error_procedure:' + convert(NVARCHAR(max), error_procedure())

		PRINT @ErrorMessage
	END CATCH
END
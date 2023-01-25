

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



CREATE PROCEDURE [dbo].[spAddDirectoryChildrenAccess] @ID INT, @UID UNIQUEIDENTIFIER, @AccessID INT, @CustomerID INT, @CreatedByUser UNIQUEIDENTIFIER, @UpdatedBy UNIQUEIDENTIFIER
AS

CREATE TABLE #DirectoryTemp (
	Id int
);
CREATE TABLE #FileTemp (
	Id INT
);
  -- Select directory hierarchy as ret
WITH ret
AS (SELECT
	DirectoryId
	FROM DMSDirectory
	WHERE DirectoryId = @ID
	UNION ALL
	SELECT
	t.DirectoryId
	FROM DMSDirectory t
	INNER JOIN ret r
	ON t.DirectoryParentId = r.DirectoryId
)
-- Insert ret directory Ids into DirectoryTemp table
INSERT INTO #DirectoryTemp (ID)
		SELECT	r.DirectoryId
		FROM ret r

INSERT INTO #FileTemp (ID) 
SELECT fileid FROM DMSDocument d
WHERE d.DirectoryId IN (
SELECT r.ID
FROM #DirectoryTemp r);
 
-- Insert DirectoryTemp Ids newly into UsersFilesAuthorization table
  INSERT INTO DMSUsersFilesAuthorization ([AccessId]
  , [FileId]
  , [CustomerId]
  , [DirectoryId]
  , [UserId]
  , [CreatedByUser]
  , [UpdatedBy]
  , [CreatedOn]
  , [UpdatedOn])
    SELECT
      @AccessID,
      NULL,
      @CustomerID,
      d.Id,
      @UID,
      @CreatedByUser,
      @UpdatedBy,
      GETDATE(),
      GETDATE()
    FROM #DirectoryTemp d
    WHERE NOT EXISTS (SELECT 1 FROM DMSUsersFilesAuthorization WHERE UserId = @UID AND DirectoryId = d.Id AND AccessId = @AccessID);

  -- Drop temp table
  DROP TABLE #DirectoryTemp

  INSERT INTO DMSUsersFilesAuthorization(
       [AccessId]
	  ,[FileId]
      ,[CustomerId]
      ,[DirectoryId]
      ,[UserId]
      ,[CreatedByUser]
      ,[UpdatedBy]
      ,[CreatedOn]
      ,[UpdatedOn])
SELECT @AccessID, f.ID,  @CustomerID, NULL,
	  @UID,
	  @CreatedByUser,
	  @UpdatedBy,
	  GETDATE(),
	  GETDATE()
FROM #FileTemp f WHERE NOT EXISTS(SELECT 1 FROM DMSUsersFilesAuthorization WHERE UserId=@UID AND FileId = f.Id AND AccessId=@AccessID) ;

DROP TABLE #FileTemp
GO

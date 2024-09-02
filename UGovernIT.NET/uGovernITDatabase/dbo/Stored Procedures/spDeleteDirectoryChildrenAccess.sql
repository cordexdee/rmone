
CREATE PROCEDURE [dbo].[spDeleteDirectoryChildrenAccess] @ID INT, @UID UNIQUEIDENTIFIER, @AccessID INT, @CustomerID INT, @CreatedByUser UNIQUEIDENTIFIER, @UpdatedBy UNIQUEIDENTIFIER
AS
CREATE TABLE #DirectoryTemp (
  Id INT
);
CREATE TABLE #FileTemp 
(Id INT);
-- Select directory hierarchy as ret
 WITH ret
 AS (	SELECT
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

DELETE u FROM DMSUsersFilesAuthorization u WHERE u.UserId = @UID AND u.AccessId = @AccessID AND u.DirectoryId  IN (SELECT dt.Id
						  FROM #DirectoryTemp dt)

DROP TABLE #DirectoryTemp

DELETE u FROM DMSUsersFilesAuthorization u WHERE u.UserId = @UID AND u.AccessId = @AccessID AND u.FileId IN (SELECT ID FROM #FileTemp)

DROP TABLE #FileTemp
 

GO

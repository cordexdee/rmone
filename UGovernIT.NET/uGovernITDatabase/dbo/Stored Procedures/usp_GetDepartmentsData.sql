/****************************************************************************************************
Author:Inderjeet Kaur
Date : 16 Nov 2022
exec [usp_GetDepartmentsData] '35525396-E5FE-4692-9239-4DF9305B915B','1', '1'           
****************************************************************************************************/
CREATE proc [dbo].[usp_GetDepartmentsData]
(
@TenantId nvarchar(50),
@EnableDivision bit,
@showDeleted bit
)
As 
Begin
	CREATE TABLE #DEPT_DATA
	(ID INT,
	PARENT_ID INT,
	CUSTOM_ID VARCHAR(15),
	CUSTOM_PARENT_ID VARCHAR(15),
	TITLE VARCHAR(250),
	GLCODE VARCHAR(250),
	MANAGERID VARCHAR(250),
	MANAGER VARCHAR(250),
	DELETED BIT,
	PARENT_DELETED BIT)
	declare @DelClause as varchar(10)

	INSERT INTO #DEPT_DATA
	SELECT c.ID, 0, 'CM' + CAST(c.ID as VARCHAR(15)), '0', TITLE, c.GLCODE, '', '', c.DELETED, 0
	FROM Company c
	WHERE c.TenantID = @TenantId

	if(@EnableDivision = 1)
	BEGIN
		INSERT INTO #DEPT_DATA
		SELECT c.ID, c.CompanyIdLookup, 'DV' + CAST(c.ID as VARCHAR(15)), 'CM' + CAST(c.CompanyIdLookup as VARCHAR(15)), 
		c.TITLE, c.GLCODE, c.ManagerUser, a.Name, c.DELETED, cp.Deleted
		FROM CompanyDivisions c
		LEFT JOIN Company cp
		ON c.CompanyIdLookup = cp.ID
		LEFT JOIN AspNetUsers a
		on c.ManagerUser = a.Id 
		WHERE c.TenantID = @TenantId

		if(@showDeleted = 1)
		BEGIN
			INSERT INTO #DEPT_DATA
			SELECT d.ID, d.DivisionIdLookup, 'DP' + CAST(d.ID as VARCHAR(15)), 'DV' + CAST(d.DivisionIdLookup as VARCHAR(15)),
				d.TITLE, d.GLCODE, d.ManagerUser, a.Name, d.DELETED, cd.Deleted 
			FROM Department d
			LEFT JOIN CompanyDivisions cd
			ON d.DivisionIdLookup = cd.ID
			LEFT JOIN Company c
			ON d.CompanyIdLookup = c.ID
			LEFT JOIN AspNetUsers a
			on d.ManagerUser = a.Id 
			WHERE d.TenantID = @TenantId AND d.DivisionIdLookup IS NOT NULL
		END
		ELSE
		BEGIN
			INSERT INTO #DEPT_DATA
			SELECT d.ID, d.DivisionIdLookup, 'DP' + CAST(d.ID as VARCHAR(15)), 
				CASE cd.Deleted 
					WHEN 0 THEN
						'DV' + CAST(d.DivisionIdLookup as VARCHAR(15))
					ELSE
						'NN' + CAST(cd.CompanyIdLookup as VARCHAR(15))
				END AS CUSTOM_PARENT_ID, 
				d.TITLE, d.GLCODE, d.ManagerUser, a.Name, d.DELETED, cd.Deleted 
			FROM Department d
			LEFT JOIN CompanyDivisions cd
			ON d.DivisionIdLookup = cd.ID
			LEFT JOIN Company c
			ON d.CompanyIdLookup = c.ID
			LEFT JOIN AspNetUsers a
			on d.ManagerUser = a.Id 
			WHERE d.TenantID = @TenantId AND d.DivisionIdLookup IS NOT NULL
		END

		IF((SELECT COUNT(1) FROM Department WHERE TenantID = @TenantId AND DivisionIdLookup IS NULL ) > 0)
		BEGIN
			INSERT INTO #DEPT_DATA
			SELECT -99999, c.ID, 'NN'+ CAST(c.ID as VARCHAR(15)), 'CM' + CAST(c.ID as VARCHAR(15)), '<None>', '', '', '', 
			(select COUNT(1) FROM Department d WHERE d.TenantID = @TenantId AND d.DivisionIdLookup IS NULL AND d.Deleted = 1 and 
			d.CompanyIdLookup = c.ID), c.Deleted
			FROM Company c WHERE c.TenantID = @TenantId		
			and c.ID in 
				(
					select d.CompanyIdLookup
					FROM Company c inner join Department d
					on c.ID = d.CompanyIdLookup
					WHERE c.TenantID = @TenantId and d.DivisionIdLookup IS NULL 
					group by d.CompanyIdLookup
				)
			
			INSERT INTO #DEPT_DATA
			SELECT d.ID, d.DivisionIdLookup, 'DP' + CAST(d.ID as VARCHAR(15)), 'NN' + CAST(d.CompanyIdLookup as VARCHAR(15)), 
			d.TITLE, d.GLCODE, d.ManagerUser, a.Name, d.DELETED, 0
			FROM Department d
			LEFT JOIN AspNetUsers a
			on d.ManagerUser = a.Id 
			WHERE d.TenantID = @TenantId AND d.DivisionIdLookup IS NULL 

		END

		if (@showDeleted = 0)
		BEGIN
			-- Add one NN division for every company where Division is deleted and if any child Department is not deleted.
			INSERT INTO #DEPT_DATA
			SELECT DISTINCT -99999, cd.CompanyIdLookup, 'NN' + CAST(cd.CompanyIdLookup as VARCHAR(15)), 'CM' + CAST(cd.CompanyIdLookup as VARCHAR(15)), 
			'<None>', '', '', '', 0, c.Deleted 
			FROM CompanyDivisions cd
			RIGHT JOIN Company c
			ON cd.CompanyIdLookup = c.ID
			WHERE CD.ID IN
			(
				SELECT DISTINCT PARENT_ID 
				FROM #DEPT_DATA dd 
				WHERE dd.PARENT_DELETED = 1 AND dd.DELETED = 0
			)

			IF((SELECT COUNT(1) FROM CompanyDivisions CD WHERE CD.Deleted = 0 AND CD.CompanyIdLookup IN (SELECT ID FROM Company C WHERE C.DELETED = 1)) > 0) 
			BEGIN
				-- Add one NN company if any deleted company has any undeleted child Divisions and Departments.
				INSERT INTO #DEPT_DATA
				SELECT -999999, 0, 'NN00', 0, '<None>', '', '', '', 0, 0
			END
		END
	END
	else
	BEGIN
		INSERT INTO #DEPT_DATA
		SELECT d.ID, d.CompanyIdLookup, 'DP' + CAST(d.ID as VARCHAR(15)), 'CM' + CAST(d.CompanyIdLookup as VARCHAR(15)),
		d.TITLE, d.GLCODE, d.ManagerUser, a.Name, d.DELETED, cp.Deleted
		FROM Department d
		LEFT JOIN Company cp
		ON d.CompanyIdLookup = cp.ID
		LEFT JOIN AspNetUsers a
		on d.ManagerUser = a.Id 
		WHERE d.TenantID = @TenantId
	END

	SELECT * FROM #DEPT_DATA; 
	DROP TABLE #DEPT_DATA;
End
GO

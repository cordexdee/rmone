
  
CREATE FUNCTION [dbo].[GetMonthListForResourceAllocation] (     
--select months as MName, id from dbo.GetMonthListForResourceAllocation('2021-01-04 00:00:00.000' ,'2021-01-11 00:00:00.000','Daily')  
--Jan-25-21A  
  @StartDate DATETIME,      
  @EndDate   DATETIME,  
  @Mode Char(10)  
)      
RETURNS @months TABLE (      
  [months] VARCHAR(20),      
  [id] VARCHAR(20)      
)      
AS      
BEGIN      
  
  DECLARE @MonthDiff INT, @counter  INT, @Firstdayofyear datetime;  
  SET @counter = 1;      
  IF(@Mode='Monthly')  
  BEGIN  
  SELECT @MonthDiff = DATEDIFF(MONTH, @StartDate, @EndDate);      
    
  WHILE @counter <= 12      
  BEGIN    
      INSERT @months       
   Select  Left(DATENAME(MONTH,DATEADD(MONTH, @counter-1, @StartDate)),3)+'-01-'+right(YEAR(GETDATE()),2) ,MONTH(DATEADD(MONTH, @counter - 1, @StartDate))    
     
   SET @counter = @counter + 1;      
  END  
  END  
  ELSE IF(@Mode='Weekly')  
  BEGIN  
  SELECT @MonthDiff = DATEDIFF(MONTH, @StartDate, @EndDate);      
     
   SELECT @Firstdayofyear= DATEADD(yy, DATEDIFF(yy, 0, GETDATE()), 0)  
   INSERT @months       
   SELECT Left(DATENAME(MONTH,DATEADD(MONTH, 0, @Firstdayofyear)),3)+'-'+right(convert(date,@Firstdayofyear,121),2)+'-' + right(YEAR(@Firstdayofyear),2),0   
     
   INSERT @months       
   SELECT     Left(DATENAME(MONTH,DATEADD(MONTH, 0, AllDates)),3)+'-'+right(convert(date,AllDates,121),2)+'-' + right(YEAR(GETDATE()),2),0   from  
(Select DATEADD(d, number, @StartDate) as AllDates from master..spt_values  
where type = 'p' and number between 0 and datediff(dd, @StartDate, @EndDate)) AS D1  
WHERE DATENAME(dw, D1.AllDates)In('Monday')  
  END  
  ELSE  IF(@Mode='Quaterly')  
  BEGIN  
  WHILE @counter <=4      
  BEGIN  
INSERT @months  
  select Left(DATENAME(MONTH,DATEADD(MONTH, 0, dt)),3)+'-'+right(convert(date,dt,121),2)+'-' + right(YEAR(GETDATE()),2),0 from (  
SELECT DATEADD(qq, DATEDIFF(qq, 0, GETDATE()) + @counter-1, 0) dt  
) temp  
 
SET @counter = @counter + 1;      
  END  
   END  
  ELSE BEGIN
  
	WITH ListDates(AllDates) AS
		(SELECT @StartDate AS DATE
    UNION ALL
    SELECT DATEADD(DAY,1,AllDates)
    FROM ListDates 
    WHERE AllDates < convert(datetime,@EndDate,121)-1)
	INSERT @months  
	SELECT Left(DATENAME(MONTH,DATEADD(MONTH, 0, AllDates)),3)+'-'+right(convert(date,AllDates,121),2)+'-' + right(YEAR(GETDATE()),2),0
	FROM ListDates
  END
  RETURN;      
END
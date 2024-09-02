  
Create FUNCTION [dbo].[GetMonthList] (     
--select months as MName, id from dbo.GetMonthList('2021-01-01 00:00:00.000' ,'2022-01-01 00:00:00.000','Monthly')  
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
  --Changed for Resource allocation Gantt report.
  WHILE @counter <= @MonthDiff      
  BEGIN    
      INSERT @months       
   Select  Left(DATENAME(MONTH,DATEADD(MONTH, @counter-1, @StartDate)),3)+'-01-'+right(YEAR(DATEADD(MONTH, @counter - 1, @StartDate)),2)+'E' ,MONTH(DATEADD(MONTH, @counter - 1, @StartDate))    
   INSERT @months       
   Select  Left(DATENAME(MONTH,DATEADD(MONTH, @counter-1, @StartDate)),3)+'-01-'+right(YEAR(DATEADD(MONTH, @counter - 1, @StartDate)),2)+'A' ,MONTH(DATEADD(MONTH, @counter - 1, @StartDate))    
   SET @counter = @counter + 1;      
  END  
  END  
  ELSE IF(@Mode='Weekly')  
  BEGIN  
  SELECT @MonthDiff = DATEDIFF(MONTH, @StartDate, @EndDate);      
     
   SELECT @Firstdayofyear= DATEADD(yy, DATEDIFF(yy, 0, @StartDate), 0)  
   INSERT @months       
   SELECT Left(DATENAME(MONTH,DATEADD(MONTH, 0, @Firstdayofyear)),3)+'-'+right(convert(date,@Firstdayofyear,121),2)+'-' + right(YEAR(@Firstdayofyear),2)+'E',0   
     
   INSERT @months       
   SELECT     Left(DATENAME(MONTH,DATEADD(MONTH, 0, AllDates)),3)+'-'+right(convert(date,AllDates,121),2)+'-' + right(YEAR(@StartDate),2)+'E',0   from  
(Select DATEADD(d, number, @StartDate) as AllDates from master..spt_values  
where type = 'p' and number between 0 and datediff(dd, @StartDate, @EndDate)) AS D1  
WHERE DATENAME(dw, D1.AllDates)In('Monday')  
INSERT @months       
   SELECT Left(DATENAME(MONTH,DATEADD(MONTH, 0, @Firstdayofyear)),3)+'-'+right(convert(date,@Firstdayofyear,121),2)+'-' + right(YEAR(@Firstdayofyear),2)+'A',0   
      INSERT @months       
   SELECT     Left(DATENAME(MONTH,DATEADD(MONTH, 0, AllDates)),3)+'-'+right(convert(date,AllDates,121),2)+'-' + right(YEAR(@StartDate),2)+'A',0   from  
(Select DATEADD(d, number, @StartDate) as AllDates from master..spt_values  
where type = 'p' and number between 0 and datediff(dd, @StartDate, @EndDate)) AS D1  
WHERE DATENAME(dw, D1.AllDates)In('Monday')  
  END  
  ELSE  
  BEGIN  
  WHILE @counter <=4      
  BEGIN  
INSERT @months  
  select Left(DATENAME(MONTH,DATEADD(MONTH, 0, dt)),3)+'-'+right(convert(date,dt,121),2)+'-' + right(YEAR(@StartDate),2)+'E',0 from (  
SELECT DATEADD(qq, DATEDIFF(qq, 0, @StartDate) + @counter-1, 0) dt  
) temp  
INSERT @months  
  select Left(DATENAME(MONTH,DATEADD(MONTH, 0, dt)),3)+'-'+right(convert(date,dt,121),2)+'-' + right(YEAR(@StartDate),2)+'A',0 from (  
SELECT DATEADD(qq, DATEDIFF(qq, 0, @StartDate) + @counter-1, 0) dt  
) temp  
SET @counter = @counter + 1;      
  END  
   END  
  
  RETURN;      
END  
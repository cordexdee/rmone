


ALTER FUNCTION [dbo].[fnGetWorkingDaysTillDate]  
(  
  @month int  
)  
RETURNS INT  
AS  
BEGIN  
 Declare @StartDate Datetime=''  ,@EndDate Datetime='', @Days int=0;  
 Select @StartDate = dateadd(month, @month - 1, dateadd(year, Year(getDate()) - 1900, 0))  
    Select @EndDate  = dateadd(month, @month,dateadd(year, Year(getDate()) - 1900, -1))   
 --If(Month(@EndDate)=MONTH(GETDATE()))  
 --begin  
 --SET @EndDate=Getdate()  
 --end  
 Set @Days= (select dbo.fnGetWorkingDays(@StartDate,@EndDate))  
 RETURN @Days  
END  
  
  



CREATE procedure [dbo].[usp_fillResourceSummaryUtilizationFooter]
@TenantID varchar(max)='', 
@Fromdate datetime='',      
@Todate datetime='',       
@mode varchar(10) = '',
@AllocationType varchar(50)='',
@IsAssignedallocation bit=''
as
begin
	DECLARE @cols AS NVARCHAR(MAX), @SQLStatement NVARCHAR(MAX) = N'' ,@sumCols AS NVARCHAR(MAX),@i as integer,  @WeekStartDate DateTime ,  
	@totalrecords int=0,@_MonthStartDate DateTime,@_MonthEndDate DateTime  ,@WeekEndDate DateTime    
	IF(@mode='Monthly')      
		Begin      
		 SET @cols = STUFF((SELECT ',' + QUOTENAME(months)       
		    FROM  dbo.GetMonthListForResourceAllocation(@Fromdate,@Todate,@Mode)       
		    FOR XML PATH(''), TYPE      
		    ).value('.', 'NVARCHAR(MAX)')       
		   ,1,1,'')      
		      
		 SET @sumCols = STUFF((SELECT ',' + 'SUM(' + QUOTENAME(months) + ')' + QUOTENAME(months)      
		     FROM  dbo.GetMonthListForResourceAllocation(@Fromdate,@Todate,@Mode)       
		    FOR XML PATH(''), TYPE      
		    ).value('.', 'NVARCHAR(MAX)')       
		   ,1,1,'')   

		select distinct SRPC.UserId,
		  STUFF((SELECT distinct ', ' + SRPC1.ModuleNameLookup
		         from Summary_ResourceProjectComplexity SRPC1
		         where SRPC.UserId = SRPC1.UserId
		            FOR XML PATH(''), TYPE
		            ).value('.', 'NVARCHAR(MAX)') 
		        ,1,2,'') ModuleNameLookup
		into #tmpSummary_ResourceProjectComplexity
		from Summary_ResourceProjectComplexity SRPC;
		
		 SET @SQLStatement =' WITH cte AS (Select u.Id ResourceUser,  
					u.UGITStartDate, 
					u.UGITEndDate, 
					u.Enabled, 
					u.DepartmentLookup, 
					u.GlobalRoleID, 
					u.IsManager,
					u.isRole,
					u.TenantID,
					u.ManagerUser,
					u.JobProfile,
					TSRPC.ModuleNameLookup,       
		t.*, year('''+ CONVERT(varchar, @Todate,121) + ''') Year  from AspNetUsers u left join      
		(Select  ResourceUser ResourceUserAllocated ,' + @cols+ ' from (      
		Select a.ResourceUser, Round((sum(a.PctAllocation)/100),2)PctAllocation,Left(DATENAME(MONTH,DATEADD(MONTH, 0, a.MonthStartDate)),3)+''-''+right(convert(date,a.MonthStartDate,121),2)+''-'' + right(YEAR(a.MonthStartDate),2) MName      
		from ResourceAllocationMonthly a where a.TenantID='''+@tenantid+'''       
		and a.monthstartdate >= '''+ CONVERT(varchar, @Fromdate,121)+ ''' and a.monthstartdate  <='''+ CONVERT(varchar, @Todate,121)+'''  
		and ResourceWorkItem in (select TicketId from [fnGetDivisonStudioTicketWise]('''+@TenantID+''','''','''','''','''') ) 
		group by a.MonthStartDate, a.ResourceUser      
		)s pivot(sum(PctAllocation) for MName  in (' + @cols + ' )) p) t on u.id=t.ResourceUserAllocated 
		left join #tmpSummary_ResourceProjectComplexity TSRPC on TSRPC.UserId = u.Id
		)
		select * from cte'      		
			 
		 Print (@SQLStatement)        
		 EXEC (@SQLStatement)  
		 
		 if exists(select 1 from ResourceUtilizationSummaryFooter where Year=YEAR(@Fromdate))
		 begin
			delete ResourceUtilizationSummaryFooter where Year=Year(@Fromdate)
		 end

		insert into ResourceUtilizationSummaryFooter
		EXEC (@SQLStatement)
		 
	end

end
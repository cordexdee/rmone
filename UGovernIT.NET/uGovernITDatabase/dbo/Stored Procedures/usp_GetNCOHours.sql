CREATE Procedure [dbo].[usp_GetNCOHours]  
@TenantID nvarchar(128) = '',  
@Filter nvarchar(250) = 'Division',  
@Studio nvarchar(250) = '',  
@Division bigint = 0,  
@Sector nvarchar(250) = '',  
@Startdate datetime = '',  
@Enddate datetime = ''   
as  
Begin  
  
 if (@Filter = 'Division')  
 begin  
  select TotalHrs, CASE When TotalHrs between 1000 and 999999  
        THEN ( CONVERT (VARCHAR(10), ( CAST(ROUND(TotalHrs / 1000.0, 1) AS NUMERIC(4, 1)) )) ) + 'K'  
        When TotalHrs >= 1000000  
        THEN ( CONVERT (VARCHAR(10), ( CAST(ROUND(TotalHrs / 1000000.0, 2) AS NUMERIC(6, 2)) )) ) + 'M'  
        Else CONVERT (VARCHAR(10), TotalHrs) End as TotalHrsLabel,  
        DivisionLookup_ as DivisionLookup, cd.Title, ResourceCount as #Projects, ProjectCount as #Resources from (  
     select Sum(temp.HoursTaken) as TotalHrs,  
     count(distinct [temp].ResourceUser) as ResourceCount  
     ,count(distinct rw.SubWorkItem) as ProjectCount, --rw.WorkItemType,
     --temp.ResourceUser, rw.WorkItem, rw.SubWorkItem, rw.StartDate, rw.EndDate, rw.SubSubWorkItem, rw.WorkItemType, temp.Title,  
     --case when cpr.DivisionLookup is null then opm.DivisionLookup else cpr.DivisionLookup end as DivisionLookup  
	case 
		when rw.WorkItemType = 'CPR' then cpr.DivisionLookup 
		when rw.WorkItemType = 'CNS' then cns.DivisionLookup 
		else opm.DivisionLookup end 
	as DivisionLookup_
    from ResourceTimeSheet temp join ResourceWorkItems rw on temp.ResourceWorkItemLookup = rw.ID  
          left join CRMProject cpr on rw.WorkItem = cpr.TicketId
          left join CRMServices cns on rw.WorkItem = cns.TicketId
          left join Opportunity opm on rw.WorkItem = opm.TicketId
          where temp.TenantID=@TenantID   
                and rw.TenantID=@TenantID 
                and temp.WorkDate between @Startdate and @Enddate
          group by case 
                       when rw.WorkItemType='CPR' then cpr.DivisionLookup 
                       when rw.WorkItemType='CNS' then cns.DivisionLookup 
                       else opm.DivisionLookup
                   end 
    ) temp join CompanyDivisions cd on temp.DivisionLookup_ = cd.ID -- This line was corrected to use DivisionLookup_ which is defined in subquery.
 end;  
 else if(@Filter = 'Sector')  
 begin  
   select TotalHrs, CASE When TotalHrs between 1000 and 999999  
         THEN ( CONVERT (VARCHAR(10), ( CAST(ROUND(TotalHrs / 1000.0, 1) AS NUMERIC(4, 1)) )) ) + 'K'  
         When TotalHrs >= 1000000  
         THEN ( CONVERT (VARCHAR(10), ( CAST(ROUND(TotalHrs / 1000000.0, 2) AS NUMERIC(6, 2)) )) ) + 'M'  
         Else CONVERT (VARCHAR(10), TotalHrs) End as TotalHrsLabel,  
         SectorChoice_ as Title, ResourceCount as #Projects, ProjectCount as #Resources from (  
      select Sum(temp.HoursTaken) as TotalHrs,  
      count(distinct [temp].ResourceUser) as ResourceCount  
      ,count(distinct rw.SubWorkItem) as ProjectCount,  
      --temp.ResourceUser, rw.WorkItem, rw.SubWorkItem, rw.StartDate, rw.EndDate, rw.SubSubWorkItem, rw.WorkItemType, temp.Title,  
      --case when cpr.SectorChoice is null then opm.SectorChoice else cpr.SectorChoice end as SectorChoice  
	  	case 
		when rw.WorkItemType = 'CPR' then cpr.SectorChoice 
		when rw.WorkItemType = 'CNS' then cns.SectorChoice 
		else opm.SectorChoice end 
		as SectorChoice_
     from ResourceTimeSheet temp join ResourceWorkItems rw on temp.ResourceWorkItemLookup = rw.ID  
		 left join CRMProject cpr on rw.WorkItem = cpr.TicketId  
		 left join Opportunity opm on rw.WorkItem = opm.TicketId  
		 left join CRMServices cns on rw.WorkItem = opm.TicketId  
     where temp.TenantID=@TenantID   
	     and rw.TenantID = @TenantID and temp.WorkDate between @Startdate and @Enddate  
     group by --cpr.SectorChoice, opm.SectorChoice  
	 	  	case 
				when rw.WorkItemType = 'CPR' then cpr.SectorChoice 
				when rw.WorkItemType = 'CNS' then cns.SectorChoice 
				else opm.SectorChoice 
			end 
  ) temp   
 End;  
  
End;  
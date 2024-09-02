CREATE View [dbo].[vw_TSR]
as
select t.TicketId, a.AssetName, c.Title as CompanyTitle, d.DepartmentDescription , cd.Title as CompanyDivision, fa.Title as FunctionalArea,
       cimpact.Title as Impact, l.LocationDescription as Location,
       cseverity.Title as Severity, t.ID, t.Status, t.OwnerUser, t.ActualHours, 
       t.BusinessManagerUser, t.CloseDate, t.CreationDate, t.PRPUser, t.PRPGroupUser, t.InitiatorUser, t.StageStep, t.TargetStartDate
	    from TSR t left join Assets a on t.AssetLookup = a.ID
                             left join Company c on t.CompanyTitleLookup = c.ID
							left join Department d on t.DepartmentLookup = d.ID
							left join CompanyDivisions cd on t.DivisionLookup = cd.ID
							left join FunctionalAreas fa on t.FunctionalAreaLookup = fa.ID
							left join Config_Module_Impact cimpact on t.ImpactLookup = cimpact.ID
							left join Location l on t.LocationLookup = l.ID
							left join Config_Module_Severity cseverity on t.SeverityLookup = cseverity.ID
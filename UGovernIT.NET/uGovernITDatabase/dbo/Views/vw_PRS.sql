CREATE View [dbo].[vw_PRS] as
select prs.TicketId, app.Description as Application, a.AssetName, c.Description as Company, d.DepartmentDescription as Department,
      cd.Description as CompanyDivision, fa.FunctionalAreaDescription as FunctionalArea, cimpact.Title as Impact,
	  loc.LocationDescription as Location, cpriority.Title as Priority, crequestpri.Title as RequestPriority,
	  cservices.Title as Services, cseverity.Severity as Severity, prs.ID, prs.Status, prs.OwnerUser, prs.ActualHours, 
       prs.BusinessManagerUser, prs.CloseDate, prs.CreationDate, prs.PRPUser, prs.PRPGroupUser, prs.InitiatorUser, prs.StageStep, prs.TargetStartDate
	    from PRS prs left join Applications app on prs.APPTitleLookup = app.ID
                      left join Assets a on prs.AssetLookup = a.ID
					  left join Company c on prs.CompanyTitleLookup = c.ID
					  left join Department d on prs.DepartmentLookup = d.ID
					  left join CompanyDivisions cd on prs.DivisionLookup = cd.ID
					  left join FunctionalAreas fa on prs.FunctionalAreaLookup = fa.ID
					  left join Config_Module_Impact cimpact on prs.ImpactLookup = cimpact.ID
					  left join Location loc on prs.LocationLookup = loc.ID
					  left join Config_Module_Priority cpriority on prs.PriorityLookup = cpriority.ID
					  left join Config_Module_RequestPriority crequestpri on prs.RequestTypeLookup = crequestpri.ID
					  left join Config_Services cservices on prs.ServiceLookUp = cservices.ID
					  left join Config_Module_Severity cseverity on prs.SeverityLookup = cseverity.ID
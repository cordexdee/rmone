insert into FieldConfiguration values('SecurityManager',null,null,'UserField',null,null);
--MK: 1/19/2017
insert into Config_Module_ModuleColumns values('SLADate',null,1,1,'Step Due','NextSLATime',10,0,1,0,'TSR',null,null,null,'10-Step Due');
--MK: 1/20/2017
insert into Config_Module_SLARule(ModuleNameLookup,SLACategory,PriorityLookup,StageTitleLookup, EndStageTitleLookup, 
                                  SLAHours, SLATarget)
                           values('TSR','Assignment',2,13,30,
						          2, 100)
insert into Config_Module_SLARule(ModuleNameLookup,SLACategory,PriorityLookup,StageTitleLookup, EndStageTitleLookup, 
                                  SLAHours, SLATarget)
                           values('TSR','Assignment',3,13,30,
						          4, 100)
insert into Config_Module_SLARule(ModuleNameLookup,SLACategory,PriorityLookup,StageTitleLookup, EndStageTitleLookup, 
                                  SLAHours, SLATarget)
                           values('TSR','Assignment',3,13,30, 4, 100)


insert into FieldConfiguration values('DepartmentLookup','Department','Title','Lookup',NULL,NUll)
--MS:2/17/2107
						         

--MK: 2/15/2017  --   transfered to default app
insert into Config_TabView values(1,'waitingonme','Waiting On Me','Home','',1,'');
insert into Config_TabView values(2,'mytickets','My Request','Home','',2,'');
insert into Config_TabView values(3,'mytask','My Task','Home','',3,'');
insert into Config_TabView values(4,'mydepartment','My Department','Home','',4,'');
insert into Config_TabView values(5,'myproject','My Project','Home','',5,'');
insert into Config_TabView values(6,'documentpendingapprove','Document Pending Approve','Home','',6,'');



--MK: 2/21/2017  --   transfered to default app
insert into FieldConfiguration values('StageActionUsers',null,null,'UserField',null,null);


--MK: 2/23/2017
insert into ModuleUserStatistics(IsActionUser, ModuleName, TicketId, UserRole, Title, UserName) 
                          values(1,'TSR', 'TSR-16-000012', 'Initiator', '', 'a8486028-af12-462d-b836-4081888d1b19');
insert into ModuleUserStatistics(IsActionUser, ModuleName, TicketId, UserRole, Title, UserName) 
                          values(1,'TSR', 'TSR-16-000014', 'Initiator', '', 'a8486028-af12-462d-b836-4081888d1b19');
insert into ModuleUserStatistics(IsActionUser, ModuleName, TicketId, UserRole, Title, UserName) 
                          values(1,'TSR', 'TSR-16-000017', 'Initiator', '', 'a8486028-af12-462d-b836-4081888d1b19');
insert into ModuleUserStatistics(IsActionUser, ModuleName, TicketId, UserRole, Title, UserName) 
                          values(1,'TSR', 'TSR-16-000020', 'Initiator', '', 'a8486028-af12-462d-b836-4081888d1b19');
insert into ModuleUserStatistics(IsActionUser, ModuleName, TicketId, UserRole, Title, UserName) 
                          values(1,'TSR', 'TSR-16-000021', 'Tester', '', 'a8486028-af12-462d-b836-4081888d1b19');
insert into ModuleUserStatistics(IsActionUser, ModuleName, TicketId, UserRole, Title, UserName) 
                          values(1,'TSR', 'TSR-16-000025', 'Tester', '', 'a8486028-af12-462d-b836-4081888d1b19');
insert into ModuleUserStatistics(IsActionUser, ModuleName, TicketId, UserRole, Title, UserName) 
                          values(1,'TSR', 'TSR-16-000026', 'Tester', '', 'a8486028-af12-462d-b836-4081888d1b19');
insert into ModuleUserStatistics(IsActionUser, ModuleName, TicketId, UserRole, Title, UserName) 
                          values(1,'TSR', 'TSR-16-000030', 'Tester', '', 'a8486028-af12-462d-b836-4081888d1b19');
insert into ModuleUserStatistics(IsActionUser, ModuleName, TicketId, UserRole, Title, UserName) 
                          values(1,'TSR', 'TSR-16-000032', 'Tester', '', 'a8486028-af12-462d-b836-4081888d1b19');
insert into ModuleUserStatistics(IsActionUser, ModuleName, TicketId, UserRole, Title, UserName) 
                          values(1,'TSR', 'TSR-16-000042', 'Owner', '', 'a8486028-af12-462d-b836-4081888d1b19');
insert into ModuleUserStatistics(IsActionUser, ModuleName, TicketId, UserRole, Title, UserName) 
                          values(1,'TSR', 'TSR-16-000052', 'Owner', '', 'a8486028-af12-462d-b836-4081888d1b19');
insert into ModuleUserStatistics(IsActionUser, ModuleName, TicketId, UserRole, Title, UserName) 
                          values(1,'TSR', 'TSR-16-000062', 'Owner', '', 'a8486028-af12-462d-b836-4081888d1b19');
insert into ModuleUserStatistics(IsActionUser, ModuleName, TicketId, UserRole, Title, UserName) 
                          values(0,'TSR', 'TSR-16-000072', 'Owner', '', 'a8486028-af12-462d-b836-4081888d1b19');
insert into ModuleUserStatistics(IsActionUser, ModuleName, TicketId, UserRole, Title, UserName) 
                          values(1,'TSR', 'TSR-16-000082', 'Owner', '', 'a8486028-af12-462d-b836-4081888d1b19');
insert into ModuleUserStatistics(IsActionUser, ModuleName, TicketId, UserRole, Title, UserName) 
                          values(0,'TSR', 'TSR-16-000092', 'Owner', '', 'a8486028-af12-462d-b836-4081888d1b19');
insert into ModuleUserStatistics(IsActionUser, ModuleName, TicketId, UserRole, Title, UserName) 
                          values(1,'TSR', 'TSR-16-0000112', 'Owner', '', 'a8486028-af12-462d-b836-4081888d1b19');
insert into ModuleUserStatistics(IsActionUser, ModuleName, TicketId, UserRole, Title, UserName) 
                          values(0,'TSR', 'TSR-16-000022', 'Security Approver', '', 'a8486028-af12-462d-b836-4081888d1b19');
insert into ModuleUserStatistics(IsActionUser, ModuleName, TicketId, UserRole, Title, UserName) 
                          values(0,'TSR', 'TSR-16-000053', 'Security Approver', '', 'a8486028-af12-462d-b836-4081888d1b19');
insert into ModuleUserStatistics(IsActionUser, ModuleName, TicketId, UserRole, Title, UserName) 
                          values(1,'TSR', 'TSR-16-000054', 'Initiator', '', 'a8486028-af12-462d-b836-4081888d1b19');
insert into ModuleUserStatistics(IsActionUser, ModuleName, TicketId, UserRole, Title, UserName) 
                          values(0,'TSR', 'TSR-16-000055', 'Initiator', '', 'a8486028-af12-462d-b836-4081888d1b19');
insert into ModuleUserStatistics(IsActionUser, ModuleName, TicketId, UserRole, Title, UserName) 
                          values(1,'TSR', 'TSR-16-000056', 'Initiator', '', 'a8486028-af12-462d-b836-4081888d1b19');
insert into ModuleUserStatistics(IsActionUser, ModuleName, TicketId, UserRole, Title, UserName) 
                          values(0,'TSR', 'TSR-16-000057', 'Initiator', '', 'a8486028-af12-462d-b836-4081888d1b19');
insert into ModuleUserStatistics(IsActionUser, ModuleName, TicketId, UserRole, Title, UserName) 
                          values(1,'TSR', 'TSR-16-000058', 'Initiator', '', 'a8486028-af12-462d-b836-4081888d1b19');
insert into ModuleUserStatistics(IsActionUser, ModuleName, TicketId, UserRole, Title, UserName) 
                          values(0,'TSR', 'TSR-16-000059', 'Initiator', '', 'a8486028-af12-462d-b836-4081888d1b19');



--MK: 2/28/2017
insert into Config_TabView values(7,'unassigned','UnAssigned','CFT','TSR',1,'');
insert into Config_TabView values(8,'waitingonme','Waiting On Me','CFT','TSR',2,'');
insert into Config_TabView values(9,'myopentickets','My Open Tickets','CFT','TSR',3,'');
insert into Config_TabView values(10,'mygrouptickets','My Group Tickets','CFT','TSR',4,'');
insert into Config_TabView values(11,'departmentticket','My Department Tickets','CFT','TSR',5,'');
insert into Config_TabView values(12,'allopentickets','Open Tickets','CFT','TSR',6,'');
insert into Config_TabView values(13,'allresolvedtickets','Resolved Tickets','CFT','TSR',7,'');
insert into Config_TabView values(14,'alltickets','All','CFT','TSR',8,'');
insert into Config_TabView values(15,'allclosedtickets','Closed','CFT','TSR',9,'');


--MS:03/03/2017
insert into FieldConfiguration values('TextAlignment','ModuleColumns','Title','Choices','Center;#Left;#Right',null,0);


--MK:03/17/2017
insert into FieldConfiguration values('StageType',null,null,'Choices','Initiated;#Assigned;#Resolved;#Tested;#Closed',null);
--MS:03/23/2107
insert into WikiArticles
(AuthorizedToView,IsDeleted,ModuleName,RequestTypeLookup,TicketId,WikiAverageScore,WikiDescription,WikiDiscussionCount,WikiDislikedBy,WikiDislikesCount,WikiFavorites,WikiHistory,WikiLikedBy,WikiLinksCount,WikiServiceRequestCount,WikiViews,Title) 
values('',1,'','','WIK-16-000001','2','Only for testing purpose<br />','8','9','10','11','Created Wiki Article: Testing to Add wiki link from service by: MX2\administrator','1','1','1','1','Testing to Add wiki link from service')


insert into WikiArticles
(AuthorizedToView,IsDeleted,ModuleName,RequestTypeLookup,TicketId,WikiAverageScore,WikiDescription,WikiDiscussionCount,WikiDislikedBy,WikiDislikesCount,WikiFavorites,WikiHistory,WikiLikedBy,WikiLinksCount,WikiServiceRequestCount,WikiViews,Title) 
values('',1,'','3','WIK-17-000002','3','testingwikifordatainsert<br />','8','9','10','11','	Created Wiki Article: testingwiki by: administrator','1','1','1','1','testingwiki') 



--MK: 02/11/2010
insert into Config_ClientAdminConfigurationLists(ClientAdminCategoryLookup, Description, ListName, TabSequence, Title, TenantID) values((select ID from Config_ClientAdminCategory where TenantID=[tenantid] and CategoryName='Projects'), 'Project Standard Workitems', 'ProjectStandardWorkItems', 44, 'Project Standards',[tenantid])



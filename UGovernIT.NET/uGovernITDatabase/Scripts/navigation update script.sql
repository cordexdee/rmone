USE core2
GO

update Config_MenuNavigation set IconUrl= '/Content/images/Menu/SubMenu/executiveDashboard.png' where MenuName = 'Default' and ID in (2830)

update Config_MenuNavigation set IconUrl= '/Content/images/Menu/SubMenu/reports.png' where MenuName = 'Default' and ID in (2827, 2831, 2845)

update Config_MenuNavigation set IconUrl= '/Content/images/resourceMng.png' where MenuName = 'Default' and ID in (2846)

update Config_MenuNavigation set IconUrl= '/Content/images/project.png' where MenuName = 'Default' and ID in (2847)

update Config_MenuNavigation set IconUrl= '/Content/images/Menu/SubMenu/recruitmentAnalytics.png' where MenuName = 'Default' and ID in (2848)





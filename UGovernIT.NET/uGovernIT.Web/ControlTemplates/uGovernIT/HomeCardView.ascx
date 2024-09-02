<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HomeCardView.ascx.cs" Inherits="uGovernIT.Web.HomeCardView" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function ViewUserProfile(s, e) {
        var key = s.keys[e.visibleIndex];
        var url = "/layouts/ugovernit/delegatecontrol.aspx";
        var viewTicketsPath = '<%=viewTicketsPath%>';
        var viewTasksPath = '<%=viewTasksPath%>';
        var userRoleGlobal = '<%=UserRole%>';
        var viewRMMPath = '<%=viewRMMPath%>';
        
        if (e.visibleIndex != 0) {
            var cardId = "<%=UserDashboardCardView.ClientID %>";
            $('#' + cardId).removeClass('cardView-wrapper');
       
        }

        if (userRoleGlobal == 'CRM')
        {
            if (e.visibleIndex == 0)
            {
                //window.parent.UgitOpenPopupDialog(viewTasksPath, 'Module=COM&Status=mytask&UserType=CRM&showalldetail=false;showglobalfilter=true', 'My Tasks', 90, 90, 0, url);
                var gdurl = '<%= viewTasksPage %>' ;
                window.location.href = gdurl;
            }
            if (e.visibleIndex == 1)
            {
                //window.parent.UgitOpenPopupDialog(viewTicketsPath, 'Module=CON&Status=mystatus&UserType=CRM&showalldetail=false;showglobalfilter=true', 'My Contacts', 90, 90, 0, url);
                //window.parent.UgitOpenPopupDialog(viewTicketsPath, 'Module=CON&Status=all&UserType=CRM&showalldetail=false;showglobalfilter=true', 'Contacts', 90, 90, 0, url);
                
              //  window.location.href = viewTicketsPath + "&Module=CON&Status=all&UserType=CRM&showalldetail=false;showglobalfilter=true";
                window.location.href = '<%= viewGridPath %>' +"?Md=CON&St=all";

            }
            if (e.visibleIndex == 2)
            {
                //window.parent.UgitOpenPopupDialog(viewTicketsPath, 'Module=COM&Status=all&UserType=CRM&showalldetail=false;showglobalfilter=true', 'Companies', 90, 90, 0, url);
                window.location.href = '<%= viewGridPath %>' + "?Md=COM&St=all";

            }
            if (e.visibleIndex == 3)
            {
                //window.parent.UgitOpenPopupDialog(viewTicketsPath, 'Module=LEM&Status=mystatus&UserType=CRM&showalldetail=false;showglobalfilter=true', 'My Leads', 90, 90, 0, url);                
                window.location.href = '<%= viewGridPath %>' + "?Md=LEM&St=mystatus";
            }
            if (e.visibleIndex == 4)
            {
               // window.parent.UgitOpenPopupDialog(viewTicketsPath, 'Module=OPM&Status=mystatus&UserType=CRM&showalldetail=false;showglobalfilter=true', 'My Opportunities', 90, 90, 0, url);
                window.location.href = '<%= viewGridPath %>' + "?Md=OPM&St=mystatus";
            }
            if (e.visibleIndex == 5)
            {
               // window.parent.UgitOpenPopupDialog(viewTicketsPath, 'Module=OPM&Status=all&UserType=CRM&showalldetail=false;showglobalfilter=true', 'Opportunities', 90, 90, 0, url);
                window.location.href = '<%= viewGridPath %>' + "?Md=OPM&St=all";//
            }             
        }

        if (userRoleGlobal == 'APM')
        {
            if (e.visibleIndex == 0)
            {
                //window.parent.UgitOpenPopupDialog(viewTasksPath, 'Module=COM&Status=mytask&UserType=CRM&showalldetail=false;showglobalfilter=true', 'My Tasks', 90, 90, 0, url);
                var gdurl = '<%= viewTasksPage %>' ;
                window.location.href = gdurl;
            }
            if (e.visibleIndex == 1)
            {                
                //window.parent.UgitOpenPopupDialog(viewTicketsPath, 'Module=CON&Status=all&UserType=APM&showalldetail=false;showglobalfilter=true', 'Contacts', 90, 90, 0, url);
                window.location.href = '<%= viewGridPath %>' + "?Md=CON&St=all";
            }
            if (e.visibleIndex == 2)
            {
                //window.parent.UgitOpenPopupDialog(viewTicketsPath, 'Module=COM&Status=all&UserType=APM&showalldetail=false;showglobalfilter=true', 'Companies', 90, 90, 0, url);
                window.location.href = '<%= viewGridPath %>' + "?Md=COM&St=all";
            }
            if (e.visibleIndex == 3)
            {
                //window.parent.UgitOpenPopupDialog(viewTicketsPath, 'Module=CPR&Status=mystatus&UserType=APM&showalldetail=false;showglobalfilter=true', 'My Projects', 90, 90, 0, url);                
                window.location.href = '<%= viewGridPath %>' + "?Md=CPR&St=mystatus";
            }
            if (e.visibleIndex == 4)
            {
               // window.parent.UgitOpenPopupDialog(viewTicketsPath, 'Module=CPR&Status=all&UserType=APM&showalldetail=false;showglobalfilter=true', 'All Projects', 90, 90, 0, url);
                window.location.href = '<%= viewGridPath %>' + "?Md=CPR&St=all";
            }
            if (e.visibleIndex == 5)
            {
                //window.parent.UgitOpenPopupDialog(viewTicketsPath, 'Module=CPR&Status=myprojectsclosein4weeks&UserType=APM&showalldetail=false;showglobalfilter=true', 'My Projects Due in 4 Weeks', 90, 90, 0, url);
                window.location.href = '<%= viewGridPath %>' + "?Md=CPR&St=myprojectsclosein4weeks";
            }             
        }

        if (userRoleGlobal == 'Estimator')
        {
            if (e.visibleIndex == 0)
            {
                //window.parent.UgitOpenPopupDialog(viewTasksPath, 'Module=COM&Status=mytask&UserType=CRM&showalldetail=false;showglobalfilter=true', 'My Tasks', 90, 90, 0, url);
                var gdurl = '<%= viewTasksPage %>' ;
                window.location.href = gdurl;
            }
            if (e.visibleIndex == 1)
            {                
                //window.parent.UgitOpenPopupDialog(viewTicketsPath, 'Module=CPR&Status=mystatus&UserType=Estimator&showalldetail=false;showglobalfilter=true', 'My Projects', 90, 90, 0, url);
                window.location.href = '<%= viewGridPath %>' + "?Md=CPR&St=mystatus";
            }
            if (e.visibleIndex == 2)
            {
                //window.parent.UgitOpenPopupDialog(viewTicketsPath, 'Module=CPR&Status=all&UserType=Estimator&showalldetail=false;showglobalfilter=true', 'All Projects', 90, 90, 0, url);
                window.location.href = '<%= viewGridPath %>' + "?Md=CPR&St=all";
            }
            if (e.visibleIndex == 3)
            {
               // window.parent.UgitOpenPopupDialog(viewTicketsPath, 'Module=OPM&Status=mystatus&UserType=Estimator&showalldetail=false;showglobalfilter=true', 'Opportunities', 90, 90, 0, url);                
                window.location.href = '<%= viewGridPath %>' + "?Md=OPM&St=mystatus";
            }
            if (e.visibleIndex == 4)
            {
                //window.parent.UgitOpenPopupDialog(viewTicketsPath, 'Module=OPM&Status=all&UserType=Estimator&showalldetail=false;showglobalfilter=true', 'Opportunities', 90, 90, 0, url);
                window.location.href = '<%= viewGridPath %>' + "?Md=OPM&St=all";
            }                      
        }

        if (userRoleGlobal == 'Core User')
        {
            if (e.visibleIndex == 0)
            {
                //window.parent.UgitOpenPopupDialog(viewTasksPath, 'Module=COM&Status=mytask&UserType=CRM&showalldetail=false;showglobalfilter=true', 'My Tasks', 90, 90, 0, url);
                var gdurl = '<%= viewTasksPage %>' ;
                window.location.href = gdurl;
            }
            if (e.visibleIndex == 1)
            {                
               // window.parent.UgitOpenPopupDialog(viewTicketsPath, 'Module=CON&Status=mystatus&UserType=Core User&showalldetail=false;showglobalfilter=true', 'My Contacts', 90, 90, 0, url);
                window.location.href = '<%= viewGridPath %>' + "?Md=CON&St=mystatus";
            }
            if (e.visibleIndex == 2)
            {
                //window.parent.UgitOpenPopupDialog(viewTicketsPath, 'Module=COM&Status=mystatus&UserType=Core User&showalldetail=false;showglobalfilter=true', 'My Companies', 90, 90, 0, url);
                window.location.href = '<%= viewGridPath %>' + "?Md=COM&St=mystatus";
            }
            if (e.visibleIndex == 3)
            {
                //window.parent.UgitOpenPopupDialog(viewTicketsPath, 'Module=LEM&Status=mystatus&UserType=Core User&showalldetail=false;showglobalfilter=true', 'My Leads', 90, 90, 0, url);                
                window.location.href = '<%= viewGridPath %>' + "?Md=LEM&St=mystatus";
            }
            if (e.visibleIndex == 4)
            {
                //window.parent.UgitOpenPopupDialog(viewTicketsPath, 'Module=OPM&Status=mystatus&UserType=Core User&showalldetail=false;showglobalfilter=true', 'My Opportunities', 90, 90, 0, url);
                window.location.href = '<%= viewGridPath %>' + "?Md=OPM&St=mystatus";
            }  
            if (e.visibleIndex == 5)
            {
                //window.parent.UgitOpenPopupDialog(viewTicketsPath, 'Module=CPR&Status=mystatus&UserType=Core User&showalldetail=false;showglobalfilter=true', 'My Projects', 90, 90, 0, url);
                window.location.href = '<%= viewGridPath %>' + "?Md=CPR&St=mystatus";
            } 
        }

        if (userRoleGlobal == 'CRM Admin') {

            if (e.visibleIndex == 0) {
                //window.parent.UgitOpenPopupDialog(viewTasksPath, 'Module=COM&Status=mytask&UserType=CRM&showalldetail=false;showglobalfilter=true', 'My Tasks', 90, 90, 0, url);
                var gdurl = '<%= viewTasksPage %>' ;
                window.location.href = gdurl;
            }
            if (e.visibleIndex == 1) {
               // window.parent.UgitOpenPopupDialog(viewTicketsPath, 'Module=CON&Status=all&UserType=CRM Admin&showalldetail=false;showglobalfilter=true', 'Contacts', 90, 90, 0, url);
                window.location.href = '<%= viewGridPath %>' + "?Md=CON&St=all";
            }
            if (e.visibleIndex == 2) {
               // window.parent.UgitOpenPopupDialog(viewTicketsPath, 'Module=COM&Status=all&UserType=CRM Admin&showalldetail=false;showglobalfilter=true', 'Companies', 90, 90, 0, url);
                window.location.href = '<%= viewGridPath %>' + "?Md=COM&St=all";
            }
            if (e.visibleIndex == 3) {
               // window.parent.UgitOpenPopupDialog(viewTicketsPath, 'Module=LEM&Status=all&UserType=CRM Admin&showalldetail=false;showglobalfilter=true', 'Leads', 90, 90, 0, url);
                window.location.href = '<%= viewGridPath %>' + "?Md=LEM&St=all";
            }
            if (e.visibleIndex == 4) {
                //window.parent.UgitOpenPopupDialog(viewTicketsPath, 'Module=OPM&Status=mystatus&UserType=CRM Admin&showalldetail=false;showglobalfilter=true', 'My Opportunities', 90, 90, 0, url);
                window.location.href = '<%= viewGridPath %>' + "?Md=OPM&St=mystatus";
            }
            if (e.visibleIndex == 5) {
               // window.parent.UgitOpenPopupDialog(viewTicketsPath, 'Module=CPR&Status=all&UserType=CRM Admin&showalldetail=false;showglobalfilter=true', 'Projects', 90, 90, 0, url);
                window.location.href = '<%= viewGridPath %>' + "?Md=CPR&St=all";
            }
        }

        if (userRoleGlobal == 'PE' || userRoleGlobal == 'PM')
        {
            if (e.visibleIndex == 0)
            {
                //window.parent.UgitOpenPopupDialog(viewTasksPath, 'Module=COM&Status=mytask&UserType=CRM&showalldetail=false;showglobalfilter=true', 'My Tasks', 90, 90, 0, url);
                var gdurl = '<%= viewTasksPage %>' ;
                window.location.href = gdurl;
            }
            if (e.visibleIndex == 1)
            {                
               // window.parent.UgitOpenPopupDialog(viewTicketsPath, 'Module=CON&Status=all&UserType=PE&showalldetail=false;showglobalfilter=true', 'Contacts', 90, 90, 0, url);
                window.location.href = '<%= viewGridPath %>' + "?Md=CON&St=all";
            }
            if (e.visibleIndex == 2)
            {
                //window.parent.UgitOpenPopupDialog(viewTicketsPath, 'Module=COM&Status=all&UserType=PE&showalldetail=false;showglobalfilter=true', 'Companies', 90, 90, 0, url);
                window.location.href = '<%= viewGridPath %>' + "?Md=COM&St=all";
            }
            if (e.visibleIndex == 3)
            {
               // window.parent.UgitOpenPopupDialog(viewTicketsPath, 'Module=CPR&Status=mystatus&UserType=PE&showalldetail=false;showglobalfilter=true', 'My Projects', 90, 90, 0, url);                
                window.location.href = '<%= viewGridPath %>' + "?Md=CPR&St=mystatus";
            }
            if (e.visibleIndex == 4)
            {
               // window.parent.UgitOpenPopupDialog(viewTicketsPath, 'Module=CPR&Status=all&UserType=PE&showalldetail=false;showglobalfilter=true', 'All Projects', 90, 90, 0, url);
                window.location.href = '<%= viewGridPath %>' + "?Md=CPR&St=all";
            }
            if (e.visibleIndex == 5)
            {
               // window.parent.UgitOpenPopupDialog(viewTicketsPath, 'Module=CPR&Status=myprojectsclosein4weeks&UserType=PE&showalldetail=false;showglobalfilter=true', 'My Projects Due in 4 Weeks', 90, 90, 0, url);
                window.location.href = '<%= viewGridPath %>' + "?Md=CPR&St=myprojectsclosein4weeks";
            }             
        }

        if (userRoleGlobal == 'PM Admin')
        {
            if (e.visibleIndex == 0)
            {
                //window.parent.UgitOpenPopupDialog(viewTasksPath, 'Module=COM&Status=mytask&UserType=CRM&showalldetail=false;showglobalfilter=true', 'My Tasks', 90, 90, 0, url);
                var gdurl = '<%= viewTasksPage %>' ;
                window.location.href = gdurl;
            }
            if (e.visibleIndex == 1)
            {                
                //window.parent.UgitOpenPopupDialog(viewTicketsPath, 'Module=OPM&Status=mystatus&UserType=PM Admin&showalldetail=false;showglobalfilter=true', 'My Opportunities', 90, 90, 0, url);
                window.location.href = '<%= viewGridPath %>' + "?Md=OPM&St=mystatus";
            }
            if (e.visibleIndex == 2)
            {
               // window.parent.UgitOpenPopupDialog(viewTicketsPath, 'Module=OPM&Status=all&UserType=PM Admin&showalldetail=false;showglobalfilter=true', 'All Opportunities', 90, 90, 0, url);
                window.location.href = '<%= viewGridPath %>' + "?Md=OPM&St=all";
            }
            if (e.visibleIndex == 3)
            {
                //window.parent.UgitOpenPopupDialog(viewTicketsPath, 'Module=CPR&Status=mystatus&UserType=PM Admin&showalldetail=false;showglobalfilter=true', 'My Projects', 90, 90, 0, url);                
                window.location.href = '<%= viewGridPath %>' + "?Md=CPR&St=mystatus";
            }
            if (e.visibleIndex == 4)
            {
               // window.parent.UgitOpenPopupDialog(viewTicketsPath, 'Module=CPR&Status=all&UserType=PM Admin&showalldetail=false;showglobalfilter=true', 'All Projects', 90, 90, 0, url);
                window.location.href = '<%= viewGridPath %>' + "?Md=CPR&St=all";
            }
            if (e.visibleIndex == 5)
            {
               // window.parent.UgitOpenPopupDialog(viewTicketsPath, 'Module=CPR&Status=closein4weeks&UserType=PM Admin&showalldetail=false;showglobalfilter=true', 'Projects Due in 4 Weeks', 90, 90, 0, url);
                window.location.href = '<%= viewGridPath %>' + "?Md=CPR&St=closein4weeks";
            }             
        }

        if (userRoleGlobal == 'Superintendent')
        {
            if (e.visibleIndex == 0)
            {
                //window.parent.UgitOpenPopupDialog(viewTasksPath, 'Module=COM&Status=mytask&UserType=CRM&showalldetail=false;showglobalfilter=true', 'My Tasks', 90, 90, 0, url);
                var gdurl = '<%= viewTasksPage %>' ;
                window.location.href = gdurl;
            }
            if (e.visibleIndex == 1)
            {                
               // window.parent.UgitOpenPopupDialog(viewTicketsPath, 'Module=CPR&Status=Projectsreadytostart&UserType=Superintendent&showalldetail=false;showglobalfilter=true', 'Projects Ready To Start', 90, 90, 0, url);
                window.location.href = '<%= viewGridPath %>' + "?Md=CPR&St=Projectsreadytostart";
            }
            if (e.visibleIndex == 2)
            {
                //window.parent.UgitOpenPopupDialog(viewTicketsPath, 'Module=CPR&Status=mystatus&UserType=Superintendent&showalldetail=false;showglobalfilter=true', 'My Projects', 90, 90, 0, url);                
                window.location.href = '<%= viewGridPath %>' + "?Md=CPR&St=mystatus";
            }
            if (e.visibleIndex == 3)
            {
                //window.parent.UgitOpenPopupDialog(viewTicketsPath, 'Module=CPR&Status=all&UserType=Superintendent&showalldetail=false;showglobalfilter=true', 'All Projects', 90, 90, 0, url);
                window.location.href = '<%= viewGridPath %>' + "?Md=CPR&St=all";
            }
            if (e.visibleIndex == 4)
            {
                var viewProjAllocPath = '<%= viewProjAllocPath %>';
                window.parent.UgitOpenPopupDialog(viewProjAllocPath, 'Module=RMM&Status=ProjectAllocation&UserType=Superintendent&showalldetail=false;showglobalfilter=true', 'Project Allocation', 90, 90, 0, url);
               
            }     
			if (e.visibleIndex == 5)
            {                
				alert('The data for this will be made available in the next release.')
            }  
			if (e.visibleIndex == 6)
            {             
				alert('The data for this will be made available in the next release.')
            }  			
        }
		
		if (userRoleGlobal == 'Field Operations')
        {
            if (e.visibleIndex == 0)
            {
                //window.parent.UgitOpenPopupDialog(viewTasksPath, 'Module=COM&Status=mytask&UserType=CRM&showalldetail=false;showglobalfilter=true', 'My Tasks', 90, 90, 0, url);
                var gdurl = '<%= viewTasksPage %>';
                window.location.href = gdurl;
            }
            if (e.visibleIndex == 1)
            {                
               // window.parent.UgitOpenPopupDialog(viewTicketsPath, 'Module=CPR&Status=Projectsreadytostart&UserType=Field Operations&showalldetail=false;showglobalfilter=true', 'Projects Ready To Start', 90, 90, 0, url);
                window.location.href = '<%= viewGridPath %>' + "?Md=CPR&St=Projectsreadytostart";
            }
            if (e.visibleIndex == 2)
            {
                //window.parent.UgitOpenPopupDialog(viewTicketsPath, 'Module=CPR&Status=mystatus&UserType=Field Operations&showalldetail=false;showglobalfilter=true', 'My Projects', 90, 90, 0, url);                
                window.location.href = '<%= viewGridPath %>' + "?Md=CPR&St=mystatus";
            }
            if (e.visibleIndex == 3)
            {
               // window.parent.UgitOpenPopupDialog(viewTicketsPath, 'Module=CPR&Status=all&UserType=Field Operations&showalldetail=false;showglobalfilter=true', 'All Projects', 90, 90, 0, url);
                window.location.href = '<%= viewGridPath %>' + "?Md=CPR&St=all";
            }
            if (e.visibleIndex == 4)
            {
             var viewProjAllocPath = '<%= viewProjAllocPath %>';
                window.parent.UgitOpenPopupDialog(viewProjAllocPath, 'Module=RMM&Status=ProjectAllocation&UserType=Superintendent&showalldetail=false;showglobalfilter=true', 'Project Allocation', 90, 90, 0, url);
                
            }     
			if (e.visibleIndex == 5)
            {                
				alert('The data for this will be made available in the next release.')
            }  
			if (e.visibleIndex == 6)
            {             
				alert('The data for this will be made available in the next release.')
            }  			
        }

        if (userRoleGlobal == 'PreCon Admin')
        {
            if (e.visibleIndex == 0)
            {
                //window.parent.UgitOpenPopupDialog(viewTasksPath, 'Module=COM&Status=mytask&UserType=CRM&showalldetail=false;showglobalfilter=true', 'My Tasks', 90, 90, 0, url);
                var gdurl = '<%= viewTasksPage %>' ;
                window.location.href = gdurl;
            }
            if (e.visibleIndex == 1)
            {                
               // window.parent.UgitOpenPopupDialog(viewTicketsPath, 'Module=OPM&Status=all&UserType=PreCon Admin&showalldetail=false;showglobalfilter=true', 'All Opportunities', 90, 90, 0, url);
                window.location.href = '<%= viewGridPath %>' + "?Md=OPM&St=all";
            }
            if (e.visibleIndex == 2)
            {
               // window.parent.UgitOpenPopupDialog(viewTicketsPath, 'Module=CPR&Status=mystatus&UserType=PreCon Admin&showalldetail=false;showglobalfilter=true', 'My Projects', 90, 90, 0, url);    
                window.location.href = '<%= viewGridPath %>' + "?Md=CPR&St=mystatus";
            }
            if (e.visibleIndex == 3)
            {
				//window.parent.UgitOpenPopupDialog(viewTicketsPath, 'Module=CPR&Status=all&UserType=PreCon Admin&showalldetail=false;showglobalfilter=true', 'All Projects', 90, 90, 0, url);
                window.location.href = '<%= viewGridPath %>' + "?Md=CPR&St=all";
            }
            if (e.visibleIndex == 4)
            {
                //window.parent.UgitOpenPopupDialog(viewTicketsPath, 'Module=CPR&Status=Projectsreadytostart&UserType=PreCon Admin&showalldetail=false;showglobalfilter=true', 'Projects Ready To Start', 90, 90, 0, url);
                window.location.href = '<%= viewGridPath %>' + "?Md=CPR&St=Projectsreadytostart";
            }
            if (e.visibleIndex == 5)
            {
                var viewResourceUtilizationPath = '<%= viewResourceUtilizationPath %>';

                window.parent.UgitOpenPopupDialog(viewResourceUtilizationPath, 'Module=RMM&Status=resourceutilization&UserType=PreCon Admin&showalldetail=false;showglobalfilter=true', 'Resource Utilization', 90, 90, 0, url);
            }             
        }	

        if (userRoleGlobal == 'Admin') {

            if (e.visibleIndex == 0) {
                //window.parent.UgitOpenPopupDialog(viewTasksPath, 'Module=COM&Status=mytask&UserType=Admin&showalldetail=false;showglobalfilter=true', 'My Tasks', 90, 90, 0, url);
                var gdurl = '<%= viewTasksPage %>';
                window.location.href = gdurl;
            }
            if (e.visibleIndex == 1) {
                //window.parent.UgitOpenPopupDialog(viewTicketsPath, 'Module=COM&Status=all&UserType=Admin&showalldetail=false;showglobalfilter=true', 'Companies', 90, 90, 0, url);
                window.location.href = '<%= viewGridPath %>' + "?Md=COM&St=all";
            }
            if (e.visibleIndex == 2) {
               // window.parent.UgitOpenPopupDialog(viewTicketsPath, 'Module=CON&Status=all&UserType=Admin&showalldetail=false;showglobalfilter=true', 'Contacts', 90, 90, 0, url);
                window.location.href = '<%= viewGridPath %>' + "?Md=CON&St=all";
            }
            if (e.visibleIndex == 3) {
               // window.parent.UgitOpenPopupDialog(viewTicketsPath, 'Module=LEM&Status=all&UserType=Admin&showalldetail=false;showglobalfilter=true', 'Leads', 90, 90, 0, url);
                window.location.href = '<%= viewGridPath %>' + "?Md=LEM&St=all";
            }
            if (e.visibleIndex == 4) {
               // window.parent.UgitOpenPopupDialog(viewTicketsPath, 'Module=OPM&Status=all&UserType=Admin&showalldetail=false;showglobalfilter=true', 'Opportunities', 90, 90, 0, url);
                window.location.href = '<%= viewGridPath %>' + "?Md=OPM&St=all";
            }
            if (e.visibleIndex == 5) {
                //window.parent.UgitOpenPopupDialog(viewTicketsPath, 'Module=CPR&Status=all&UserType=Admin&showalldetail=false;showglobalfilter=true', 'Projects', 90, 90, 0, url);
                window.location.href = '<%= viewGridPath %>' + "?Md=CPR&St=all";
            }
        }

        if (userRoleGlobal == 'Executive') {

            if (e.visibleIndex == 0) {
                //window.parent.UgitOpenPopupDialog(viewTasksPath, 'Module=COM&Status=mytask&UserType=CRM&showalldetail=false;showglobalfilter=true', 'My Tasks', 90, 90, 0, url);
                var gdurl = '<%= viewTasksPage %>' ;
                window.location.href = gdurl;
            }
            if (e.visibleIndex == 1) {
               // window.parent.UgitOpenPopupDialog(viewTicketsPath, 'Module=OPM&Status=all&UserType=Executive&showalldetail=false;showglobalfilter=true', 'Opportunities', 90, 90, 0, url);
                window.location.href = '<%= viewGridPath %>' + "?Md=OPM&St=all";
            }
            if (e.visibleIndex == 2) {
                //window.parent.UgitOpenPopupDialog(viewTicketsPath, 'Module=CPR&Status=pipeline&UserType=Executive&showalldetail=false;showglobalfilter=true', 'Pipeline', 90, 90, 0, url);
                window.location.href = '<%= viewGridPath %>' + "?Md=CPR&St=pipeline";
            }
            if (e.visibleIndex == 3) {
               // window.parent.UgitOpenPopupDialog(viewTicketsPath, 'Module=CPR&Status=liveprojects&UserType=Executive&showalldetail=false;showglobalfilter=true', 'Live Projects', 90, 90, 0, url);
                window.location.href = '<%= viewGridPath %>' + "?Md=CPR&St=liveprojects";
            }
            if (e.visibleIndex == 4) {
                //window.parent.UgitOpenPopupDialog(viewTicketsPath, 'Module=CPR&Status=startedthismonth&UserType=Executive&showalldetail=false;showglobalfilter=true', 'Projects Started this Month', 90, 90, 0, url);
                window.location.href = '<%= viewGridPath %>' + "?Md=CPR&St=startedthismonth";
            }
            if (e.visibleIndex == 5) {
                //window.parent.UgitOpenPopupDialog(viewTicketsPath, 'Module=CPR&Status=closedthisyear&UserType=Executive&showalldetail=false;showglobalfilter=true', 'Closed this Year ', 90, 90, 0, url);
                window.location.href = '<%= viewGridPath %>' + "?Md=CPR&St=closedthisyear";
            }
            if (e.visibleIndex == 6) {
                window.parent.UgitOpenPopupDialog(viewRMMPath, 'Module=RMM&Status=enabled&UserType=Executive&showalldetail=false;showglobalfilter=true', 'Resources', 90, 90, 0, url);
            }
        }

        if (userRoleGlobal == 'Project Executive') {

            if (e.visibleIndex == 0) {
                //window.parent.UgitOpenPopupDialog(viewTasksPath, 'Module=CPR&Status=mytask&UserType=Project Executive&showalldetail=false;showglobalfilter=true', 'My Tasks', 90, 90, 0, url);
                var gdurl = '<%= viewTasksPage %>';
                window.location.href = gdurl;
            }
            if (e.visibleIndex == 1) {
               // window.parent.UgitOpenPopupDialog(viewTicketsPath, 'Module=CPR&Status=myprojects&UserType=Project Executive&showalldetail=false;showglobalfilter=true', 'My Projects', 90, 90, 0, url);
                window.location.href = '<%= viewGridPath %>' + "?Md=CPR&St=myprojects";
            }
            if (e.visibleIndex == 2) {
               // window.parent.UgitOpenPopupDialog(viewTicketsPath, 'Module=CPR&Status=myactionitems&UserType=Project Executive&showalldetail=false;showglobalfilter=true', 'My Action Items', 90, 90, 0, url);
                window.location.href = '<%= viewGridPath %>' + "?Md=CPR&St=myactionitems";
            }
            if (e.visibleIndex == 3) {
               // window.parent.UgitOpenPopupDialog(viewTicketsPath, 'Module=CPR&Status=liveprojects&UserType=Project Executive&showalldetail=false;showglobalfilter=true', 'Live Projects', 90, 90, 0, url);
                window.location.href = '<%= viewGridPath %>' + "?Md=CPR&St=liveprojects";
            }
            if (e.visibleIndex == 4) {
               // window.parent.UgitOpenPopupDialog(viewTicketsPath, 'Module=CPR&Status=closed&UserType=Project Executive&showalldetail=false;showglobalfilter=true', 'Projects Closed', 90, 90, 0, url);
                window.location.href = '<%= viewGridPath %>' + "?Md=CPR&St=closed";
            }
            if (e.visibleIndex == 5) {
               // window.parent.UgitOpenPopupDialog(viewTicketsPath, 'Module=CPR&Status=pipeline&UserType=Project Executive&showalldetail=false;showglobalfilter=true', 'Pipeline ', 90, 90, 0, url);
                window.location.href = '<%= viewGridPath %>' + "?Md=CPR&St=pipeline";
            }
        }

        //if (userRoleGlobal == 'PM') {
        //    if (e.visibleIndex == 0) {
        //        window.parent.UgitOpenPopupDialog(viewTasksPath, 'Module=CPR&Status=mytask&UserType=PM&showalldetail=false;showglobalfilter=true', 'My Tasks', 90, 90, 0, url);
        //    }
        //    if (e.visibleIndex == 1) {
        //        window.parent.UgitOpenPopupDialog(viewTicketsPath, 'Module=CPR&Status=myprojects&UserType=PM&showalldetail=false;showglobalfilter=true', 'My Projects', 90, 90, 0, url);
        //    }
        //    if (e.visibleIndex == 2) {
        //        window.parent.UgitOpenPopupDialog(viewTicketsPath, 'Module=CPR&Status=myactionitems&UserType=PM&showalldetail=false;showglobalfilter=true', 'My Action Items', 90, 90, 0, url);
        //    }
        //    if (e.visibleIndex == 3) {
        //        window.parent.UgitOpenPopupDialog(viewTicketsPath, 'Module=OPM&Status=all&UserType=PM&showalldetail=false;showglobalfilter=true', 'Opportunities', 90, 90, 0, url);
        //    }
        //    if (e.visibleIndex == 4) {
        //        window.parent.UgitOpenPopupDialog(viewTicketsPath, 'Module=CPR&Status=closethisweek&UserType=PM&showalldetail=false;showglobalfilter=true', 'Projects Closed This Week', 90, 90, 0, url);
        //    }
        //    if (e.visibleIndex == 5) {
        //        window.parent.UgitOpenPopupDialog(viewTicketsPath, 'Module=CPR&Status=closein4weeks&UserType=PM&showalldetail=false;showglobalfilter=true', 'Projects Due in 4 Weeks', 90, 90, 0, url);
        //    }
        //}
    }
  </script>

 <%--div for holding card view for user--%>
            <%--<div id="ugitUserProfileCardViewDiv" runat="server" class="containerdiv" >--%>
                <dx:ASPxCardView ID="UserDashboardCardView" CardLayoutProperties-SettingsItems-ShowCaption="False" KeyFieldName="Query" runat="server" Width="100%" AutoGenerateColumns="false" Border-BorderColor="White" CssClass="homeDb_chartView" OnCustomCallback="innerProjectData_CustomCallback"  >
                    <CardLayoutProperties Styles-Disabled-BorderLeft-BorderColor="White" />
                    <Settings LayoutMode="Flow" />
                    <SettingsSearchPanel Visible="false" />
                    <Styles>
                        <SearchPanel Border-BorderStyle="None" CssClass="searchpanel"></SearchPanel>
                        <FlowCard  Height="56px" CssClass="HomeCard_view colForMd2 col-sm-4 col-xs-12"></FlowCard>
                        <EmptyCard Wrap="True"></EmptyCard>
                    </Styles>
                    <SettingsBehavior AllowSelectByCardClick="true"
                        AllowSelectSingleCardOnly="True" AllowSort="False" />
                    <ClientSideEvents CardClick="ViewUserProfile" />
                    <SettingsPager Position="TopAndBottom" Mode="ShowAllRecords" AlwaysShowPager="true">
                        <PageSizeItemSettings Visible="true" />
                    </SettingsPager>
                </dx:ASPxCardView>
          <%--  </div>--%>
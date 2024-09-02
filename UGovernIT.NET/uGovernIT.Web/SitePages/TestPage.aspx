<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/master/Main.Master" CodeBehind="TestPage.aspx.cs" Inherits="uGovernIT.Web.SitePages.TestPage" %>

<%@ Register Src="~/ControlTemplates/RMONE/ModuleConstraintsListDx.ascx" TagPrefix="ugit" TagName="ModuleConstraintsListDx" %>
<%@ Register Src="~/ControlTemplates/UserWelcomePanel.ascx" TagPrefix="ugit" TagName="UserWelcomePanel" %>
<%@ Register Src="~/ControlTemplates/CoreUI/MyProjectCount.ascx" TagPrefix="ugit" TagName="MyProjectCount" %>
<%@ Register Src="~/ControlTemplates/UnfilledPipelineAllocations.ascx" TagPrefix="ugit" TagName="UnfilledPipelineAllocations" %>
<%@ Register Src="~/ControlTemplates/UnfilledProjectAllocations.ascx" TagPrefix="ugit" TagName="UnfilledProjectAllocations" %>
<%@ Register Src="~/ControlTemplates/UnfilledAllocations.ascx" TagPrefix="ugit" TagName="UnfilledAllocations" %>
<%@ Register Src="~/ControlTemplates/UserProjectPanel.ascx" TagPrefix="ugit" TagName="UserProjectPanel" %>
<%@ Register Src="~/ControlTemplates/RMM/CustomResourceAllocation.ascx" TagPrefix="ugit" TagName="CustomResourceAllocation" %>
<%@ Register Src="~/ControlTemplates/RMONE/NewOPMWizard.ascx" TagPrefix="ugit" TagName="NewOPMWizard" %>
<%@ Register Src="~/ControlTemplates/Bench/BenchReport.ascx" TagPrefix="ugit" TagName="BenchReport" %>












<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderContainer" runat="server">
    
<style>
    
</style>
    <script>
         var ugitConfig = {
                apiBaseUrl: "<%: ConfigurationManager.AppSettings["apiBaseUrl"] %>"
            }
    </script>

    <div>
        <ugit:BenchReport runat="server" ID="BenchReport" />
    </div>
</asp:Content>
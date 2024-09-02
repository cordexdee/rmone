<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProjectCompactView.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.PMM.ProjectCompactView" %>
<%@ Register Src="~/ControlTemplates/PMM/TaskWorkflow.ascx" TagPrefix="ugit" TagName="TaskWorkflow" %>
<%@ Register Src="~/ControlTemplates/uGovernIT/PMMProject/ProjectMonitors.ascx" TagPrefix="uc1" TagName="ProjectMonitors" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<style data-v="<%=UGITUtility.AssemblyVersion %>">

    #projectToolBar.dx-toolbar {
        clear:both;
        background-color:transparent;
    }
    #divmainPanel .dx-drawer-panel-content .dx-scrollable-content {
        background-color:#828b9a;
    }
    #divmainPanel .dx-drawer-panel-content .dx-scrollable-content .dx-list-item {
        color: #f9f8f8;
        font-weight: 400
    }
    /*#divmainPanel .contentView {
        /*min-height:400px
    }*/
    .menupanelbox{
        width:20%;
        float:right;
        background-color:antiquewhite
    }

    .mainpanelbox{
        width:100%;
        float:left;
        clear:both;
        padding:10px 2px;
     }

    .panel-list {
        height: 400px;
    }

    .workflow{
        float:left;
        padding:10px 2px;
        height:200px;
        width:100%;
    }

    .monitorpanel{
        float:left;
        padding:30px 2px;
        height:200px;
        width:100%;
        display:none;
    }

    /*.projectTeam_linkWrap {
        margin-bottom: 24px;
    }*/

    /*.headerCompactView{
        float: left;
        text-align: left;
        padding: 5px;
        font-size: 18px;
        width:100%;
        height: 30px;
    border-bottom: 1px solid #EEE;
    background-color: #F6F7FB;
    color:#7CAFEA;
    padding: 5px;
    -webkit-border-top-left-radius: 5px;
    -webkit-border-top-right-radius: 5px;
    -moz-border-radius-topleft: 5px;
    -moz-border-radius-topright: 5px;
    border-top-left-radius: 5px;
    border-top-right-radius: 5px;
    }*/
</style>
<script data-v="<%=UGITUtility.AssemblyVersion %>">
    var headerText = '<%= headerText%>';
    var ugitContext = {moduleName: "<%= moduleName%>", ticketID:"<%=ticketID%>", projectTitle: "<%= projectTitle%>", fullViewUrl:"<%= fullViewUrl%>"};
    var navigation = [
        { id: "fullview", text: "Full View", icon: "/Content/images/changeView-gray.png" },
        { id: "uGovernIT.NewProjectTask", text: "Task", icon: "/Content/images/Menu/SubMenu/TSK_32x32.svg" },
        { id: "uGovernIT.PMMProject.PMMProjectStatus", text: "Status", icon: "/Content/images/Menu/SubMenu/RCA_32x32.svg" },
        { id: "RMM.CRMProjectAllocationViewNew", text: "Project Team", icon: "/Content/images/recource-management.png" },
        { id: "Shared.CustomTicketRelationShip", text: "Related Items", icon: "/Content/images/Menu/SubMenu/PRS_32x32.svg" },
        { id: "Shared.History", text: "History", icon: "/Content/images/Menu/SubMenu/TSR_32x32.svg" }
        
    ];

    $(function () {
        $("#headerTitle").text(headerText);
        $("#projectToolBar").dxMenu({
            items: navigation,
            orientation: "horizontal",            
            selectedItem: navigation.find(x => x.id == '<%= ctrlName%>'),
            //selectedItem: navigation.find(x => x.id == 'uGovernIT.NewProjectTask'),
            //selectedItem: navigation[1],
            onItemClick: function (data) {
                var selecteditem = data.itemData;
                data.component.option('selectedItem', data.itemData);
                //var activeItem = data.event.currentTarget;
                //$(activeItem).addClass("activeItem");
                //selecteditem.addClass = 
                if (selecteditem.id == "fullview") {
                    window.location.href = ugitContext.fullViewUrl;
                }
                else {
                    headerText = selecteditem.text;
                    cbpMainPanel.PerformCallback(selecteditem.id);
                }
            }
        });


         //$("#projectToolBar").dxToolbar({
         //   items: [{
         //       widget: "dxButton",
         //       location: "after",
         //       options: {
         //           icon: "menu",
         //           onClick: function () {
         //               var drawer = $("#divmainPanel").dxDrawer("instance");
         //               drawer.toggle(); 
         //           }
         //       }
         //   }]
            <%--{
                widget: "dxTextBox",
                location: "before",
                options: {
                    placeholder: "Project Title",
                    readOnly: true,
                    value: ugitContext.projectTitle,
                    width: function () { return $(window.document).width() - 100; },
                    hoverStateEnabled: false,
                    onFocusIn: function (e) {
                        e.component.option("readOnly", false);
                    },
                    onFocusOut: function (e) {
                        ///to-do: api to save change if title is changed.
                        e.component.option("readOnly", true);
                        if (typeof e.component._options.value != "undefined" || e.component._options.value != "") {
                            $.post(ugitConfig.apiBaseUrl + "/api/pmmapi/UpdateProjectTitle?title=" + e.component._options.value + "&ticketId=" + "<%=ticketID%>", function (data) {
                                //e.component.option("value", data);
                            });
                        }
                    }
                }
            }]--%>
        //});

        //$("#divmainPanel").dxDrawer({
        //    opened: false,
        //    closeOnOutsideClick: true,
        //    position: "right",
        //    openedStateMode:"overlap",
        //    template: function () {
        //        var $list = $("<div>").width(200).addClass("panel-list");
        //        return $list.dxList({
        //            dataSource: navigation,
        //            hoverStateEnabled: false,
        //            focusStateEnabled: false,
        //            activeStateEnabled: false,
        //            selectionMode: "single",
        //            onSelectionChanged: function (e) {
        //                var selecteditem = e.addedItems[0];
        //                if (typeof selecteditem != "undefined") {
        //                    if (selecteditem.id == "fullview") {
        //                        window.location.href = ugitContext.fullViewUrl;
        //                    }
        //                    else {
        //                        headerText = selecteditem.text;
                                
        //                        cbpMainPanel.PerformCallback(selecteditem.id);
        //                    }
        //                }
        //            }
        //        });
        //    }
        //});
    });

    function cbpMainPanelEndCallback(s, e) {
        $('#headerTitle').text(headerText);
        $.globalEval($('#dxss_inlineCtrProjectStatus').text());
        $.globalEval($('#dxss_inlineCtrScript').text());
        $.globalEval($('#dxss_inlineCtrHtmlEditor').text());
        $.globalEval($("#dxss_saveprojectmonitors").text());
    }

    $(document).ready(function () {
        $('#projectContainer').parent().addClass("pmm-popup-container");
    });
</script>
<div id="projectContainer">
    <div id="projectToolBar" class="pmmEdit-projectToolBar"></div>
    <div id="divmainPanel" class="mainpanelbox">
        <dx:ASPxCallbackPanel ID="cbpMainPanel" runat="server" ClientInstanceName="cbpMainPanel" OnCallback="cbpMainPanel_Callback" EnableViewState="false">
            <ClientSideEvents EndCallback="function(s, e){cbpMainPanelEndCallback(s,e); }"  />
            <PanelCollection>
                <dx:PanelContent ID="pnlmain" runat="server" ValidateRequestMode="Disabled">
                    <%--<div id="headerTitle" class="headerCompactView"></div>--%>
                    <div id="contentView" runat="server" class="contentView col-md-12" style="clear:both"> </div>
                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxCallbackPanel>

        <%--<div class="workflow" visible="false">
        <ugit:TaskWorkflow runat="server" id="TaskWorkflow"  visible="false"/>
        </div>--%>
        
    </div>
</div>

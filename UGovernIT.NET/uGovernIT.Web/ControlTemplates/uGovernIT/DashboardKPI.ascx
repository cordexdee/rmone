<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DashboardKPI.ascx.cs" Inherits="uGovernIT.Web.DashboardKPI" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Import Namespace="uGovernIT.Utility" %>
<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
    .containermain
    {
        float: left;
        width: 100%;
        padding:1%;
        background: #E1E1E1;
    }

    .containerblockmain
    {
        float: left;
        width: 100%;
        border: 1px solid #000;
    }

    .moduleinfo-m
    {
        float: left;
        width: 100%;
        padding-top: 9px;
        padding-bottom: 15px;
    }

    .generalinfo-table
    {
        border-collapse: collapse;
        width: 100%;
    }

    .basicinfo-td
    {
        width: 60%;
    }

    .basicinfo-td fieldset
    {
        width: 100%;
    }

    .dashboardgroup-td
    {
        width: 40%;
    }

        .dashboardgroup-td fieldset
        {
            float: left;
            width: 95%;
            height: 200px;
        }

    .viewdashboard-td
    {
        width: 60%;
    }

        .viewdashboard-td fieldset
        {
            float: left;
            width: 97%;
        }

    .showinsidebar-td
    {
        width: 40%;
    }

        .showinsidebar-td fieldset
        {
            float: left;
            width: 95%;
            height: 200px;
        }

    .widthtxtbox
    {
        width: 95%;
    }

    .viewdashboard-left
    {
        float: left;
        width: 48%;
    }

    .viewdashboard-right
    {
        float: left;
        width: 48%;
    }

    .vd-widthtxtbox
    {
        width: 150px;
    }

    .showsidebar-div
    {
        float: left;
        width: 65%;
        height: 150px;
        overflow: scroll;
    }

    .showsidebar-checklist
    {
        width: 70%;
    }
    .dpLinkUrl {
        display:none;
    }

    .dashboardgroup-div
    {
        float: left;
        width: 65%;
        height: 150px;
        overflow: scroll;
    }

    .dashboardgroup-checklist
    {
        width: 70%;
    }

    .dashboardgroup-right
    {
        float: left;
        width: 30%;
        text-align: center;
    }

        .dashboardgroup-right .selectall
        {
            float: left;
            width: 100%;
            padding-top: 60px;
        }

            .dashboardgroup-right .selectall input
            {
                width: 90px;
            }

        .dashboardgroup-right .clearall
        {
            float: left;
            width: 100%;
        }

            .dashboardgroup-right .clearall input
            {
                width: 90px;
            }

        .dashboardgroup-right .edit
        {
            float: left;
        }

            .dashboardgroup-right .edit img
            {
                cursor: pointer;
            }

    .showsidebar-right
    {
        float: left;
        width: 30%;
        text-align: center;
    }

        .showsidebar-right .selectall
        {
            float: left;
            width: 100%;
            padding-top: 60px;
        }

            .showsidebar-right .selectall input
            {
                width: 90px;
            }

        .showsidebar-right .clearall
        {
            float: left;
            width: 100%;
        }

            .showsidebar-right .clearall input
            {
                width: 90px;
            }

    .label
    {
        font-weight: bold;
        color:#000;
        font-size:100%;
        padding-left: 0;
    }

    .dimension-table
    {
        border-collapse: collapse;
        width: 100%;
    }

        .dimension-table .edit-td
        {
            width: 49%;
        }

        .dimension-table .list-td
        {
            width: 49%;
            padding-left: 10px;
        }

        .dimension-table fieldset
        {
            float: left;
            width: 98%;
        }

            .dimension-table fieldset fieldset
            {
                float: left;
                width: 95%;
            }

        .dimension-table .contentdiv
        {
            float: left;
            width: 100%;
            padding-top: 2px;
        }

        .dimension-table .content-head
        {
            float: inherit;
            font-weight: bold;
        }

        .dimension-table .content-edit
        {
            float: left;
            padding-left:1px;
        }

        .dimension-table .actiondiv
        {
            float: inherit;
        }

        .dimension-table .content-action
        {
            float: inherit;
        }

        .dimension-table .inputelement
        {
          
        }

        .dimension-table .operatorinput
        {
            width: 80px;
        }

    .dimension-list
    {
        float: left;
        width: 100%;
        max-height:400px;
        overflow-y:auto;
        overflow-x:hidden;
    }

        .dimension-list .dimension-content
        {
            float: left;
            width: inherit;
        }

        .dimension-list .dimension-head
        {
            float: left;
            width: 100%;
        }

        .dimension-list .dimension-detail
        {
            float: left;
            width: 100%;
        }

        .dimension-list .dimension-action
        {
            float: right;
        }

            .dimension-list .dimension-action span
            {
                float: right;
            }

   
    .hide
    {
        display: none;
    }

    .width95
    {
        width: 95%;
    }

    .lbaggragateexp
    {
        width: 95%;
        border: 1px dotted black;
        float: left;
        min-height: 20px;
        padding: 2px;
    }

    .lbfilter
    {
        width: 95%;
        border: 1px dotted black;
        float: left;
        min-height: 20px;
        padding: 2px;
    }

    .lbcolor
    {
        border: 1px dotted black;
    }

    .barcondition
    {
        float: left;
        padding: 2px;
        width: 90%;
    }

    .barcolorlabel
    {
        float: left;
        height: 17px;
        margin-left: 2px;
        width: 17px;
    }

    .ms-formbody
    {
        background: none repeat scroll 0 0 #E8EDED;
        border-top: 1px solid #A5A5A5;
        padding: 3px 6px 4px;
        vertical-align: top;
    }

    .ms-standardheader
    {
        text-align:right;
    }
</style>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_initializeRequest(InitializeRequest);
    prm.add_beginRequest(BeginRequestHandler);
    prm.add_endRequest(EndRequest);
    var notifyId;

    function InitializeRequest(sender, args) {
        var prm = Sys.WebForms.PageRequestManager.getInstance();
    }

    var notifyId = "";
    function AddNotification(msg) {
        if (notifyId == "") {
            notifyId = SP.UI.Notify.addNotification(msg, true);
        }
    }
    function RemoveNotification() {
        SP.UI.Notify.removeNotification(notifyId);
        notifyId = '';
    }
    function BeginRequestHandler(sender, args) {
        AddNotification("Processing ..");
    }

    function EndRequest(sender, args) {
        var s = sender;
        var a = args;
        var msg = null;
        if (a._error != null) {
            switch (args._error.name) {
                case "Sys.WebForms.PageRequestManagerServerErrorException":
                    msg = "PageRequestManagerServerErrorException";
                    break;
                case "Sys.WebForms.PageRequestManagerParserErrorException":
                    msg = "PageRequestManagerParserErrorException";
                    break;
                case "Sys.WebForms.PageRequestManagerTimeoutException":
                    msg = "PageRequestManagerTimeoutException";
                    break;
            }
            args._error.message = "My Custom Error Message " + msg;
            args.set_errorHandled(true);

        }
        else {
            RemoveNotification();
        }
    }

</script>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">

    function selectAllCheckBoxes(checkBoxlistClass) {
        $("." + checkBoxlistClass + " input:checkbox").attr("checked", "checked");
    }

    function clearAllCheckBoxes(checkBoxlistClass) {
        $("." + checkBoxlistClass + " input:checkbox").removeAttr("checked");
    }

    function confirmWhileTemplateSaving(s, e) {
        
        var templateOption = $("#<%=ddlTemplateList.ClientID %> option:selected");
        if (templateOption != null && templateOption.attr("value") != "0") {
            if (confirm("Do you want to override " + templateOption.text() + " template?")) {
                e.processOnServer = true;
            }
            else {
                e.processOnServer = false;
            }
        }
        else {
            if (confirm("Do you want to create new Template?")) {
                e.processOnServer = true;
            }
            else {
                e.processOnServer = false;
            }
        }
    }

    function loadPreview() {
        updateCharts($("#<%=hDashboardID.ClientID %>").val());
    }

    function slDrillDownType_SelectionChange(obj)
    {      
        if (obj.selectedIndex == 2) {
            $(".dpLinkUrl").show();
        }
        else {
            $(".dpLinkUrl").hide();
        }
        
        if (obj.selectedIndex == 1) {
            $("#<%=slNonModuleColumns.ClientID %>").show();
        }
        else {
            $("#<%=slNonModuleColumns.ClientID %>").hide();
        }
    }

    function actionOnTabClick(tabNumber) {
       
        var tab = Number(tabNumber);
        $(".containermain").each(function (i, item) {
            $(item).hide();
        });

        var tabVal = $("#<%=hTabNumber.ClientID%>").val();
        var isValid = true;
        if (tabVal == "1" && tabNumber != "1") {
            //$("#<=btnUpdateGeneralInfo.ClientID %>").get(0).click();
            if (!Page_IsValid) {
                isValid = false;
            }
        }

        if (!isValid) {
            tab = Number(tabVal);
        }
        var activeTab = $(".containermain[id='" + "tabPanel_" + tab + "']");
        activeTab.show();
        $("#<%=hTabNumber.ClientID%>").val(tab);
        tabMenu.SetActiveTabIndex(tab - 1);
       // alert('kunda');
    }
</script>
<asp:HiddenField ID="hTabNumber" runat="server" Value="1" />
<dx:ASPxTabControl ID="tabMenu" ClientInstanceName="tabMenu"  runat="server" ActiveTabIndex="2">
    <Tabs>
        <dx:Tab Text="General" Name="general" />
        <dx:Tab Text="KPIs" Name="kpi"/>
        <dx:Tab Text="Preview" Name="preview"/>
    </Tabs>
    <ClientSideEvents ActiveTabChanged="function(s, e){actionOnTabClick(s.activeTabIndex+1);}" />
    <TabStyle Paddings-PaddingLeft="13px" Paddings-PaddingRight="13px"></TabStyle>
</dx:ASPxTabControl>
<div class="containerblockmain">
    <div style="display: none;" class="containermain " id="tabPanel_1">

        <asp:Panel ID="tabPanelconainer1" runat="server">
            <asp:HiddenField ID="hDashboardID" runat="server" />
            <table cellpadding="0" cellspacing="0" class="generalinfo-table">
                <tr>
                    <td valign="top" class="basicinfo-td">
                        <fieldset>
                            <legend>Basic Info</legend>
                            <div>
                                <div>
                                    <span class="label">Title<b style="color: Red">*</b>:</span> <span>
                                        <asp:CheckBox ID="cbHideTitle" runat="server" Text="Hide Title" TextAlign="Right" /></span>
                                </div>
                                <div>
                                    <span>
                                        <asp:TextBox CssClass="widthtxtbox" MaxLength="200" ValidationGroup="generalInfo"
                                            ID="txtTitle" runat="server"></asp:TextBox></span>
                                </div>
                                <div>
                                    <asp:RequiredFieldValidator Display="Dynamic" ValidationGroup="generalInfo" ID="txtTitlerRFValidator"
                                        runat="server" ControlToValidate="txtTitle" ErrorMessage="Title is Required."></asp:RequiredFieldValidator>
                                </div>
                                <div>
                                    <span class="label">Description:</span> <span>
                                        <asp:CheckBox ID="cbHideDesc" runat="server" Text="Hide Description" TextAlign="Right" /></span>
                                </div>
                                <div>
                                    <span>
                                        <asp:TextBox MaxLength="1000" CssClass="widthtxtbox" TextMode="MultiLine" ID="txtDescription"
                                            runat="server"></asp:TextBox></span>
                                </div>
                                <div>
                                    <div>
                                        <span>
                                            <asp:CheckBox ID="cbStopAutoScale" runat="server" Text="Stop Auto Scale"></asp:CheckBox></span>
                                    </div>
                                </div>
                                <div>
                                    <div>
                                        <span class="label">Icon Url:</span>
                                    </div>
                                    <div>
                                        <span>
                                            <asp:TextBox CssClass="widthtxtbox" ID="txtIconUrl" runat="server"></asp:TextBox></span>
                                    </div>
                                </div>
                                <div>
                                    <div>
                                        <span class="label">Order:</span>
                                    </div>
                                    <div>
                                        <dx:ASPxComboBox ID="cmbOrder" runat="server" DropDownStyle="DropDownList"></dx:ASPxComboBox>
                                    </div>
                                </div>

                            </div>
                            <div>
                            </div>
                        </fieldset>
                    </td>

                </tr>
            </table>
        </asp:Panel>
        <div style="padding-top:15px;">
        <dx:ASPxButton ID="btnUpdateGeneralInfo" runat="server" ValidationGroup="generalInfo" CssClass="primary-blueBtn" Text="Save Changes" OnClick="BtnUpdateGeneralInfo_Click" >

        </dx:ASPxButton>
        </div>
    </div>
    <div style="display: none;" class="containermain " id="tabPanel_2">

        <asp:Panel ID="tabPanelconainer2" runat="server">
            <asp:HiddenField ID="hEditDimension" runat="server" />
            <table cellpadding="0" cellspacing="0" class="dimension-table">
                <tr>
                    <td class="edit-td" valign="top">
                            <table class="ms-formtable" cellpadding="0" cellspacing="0" style="border-collapse: collapse"
                                width="100%">
                                <tr>
                                    <td class="ms-formlabel">
                                        <h3 class="ms-standardheader">Dashboard Table:<b style="color: Red">*</b>:
                                        </h3>
                                    </td>
                                    <td class="ms-formbody">
                                        <asp:DropDownList ValidationGroup="editDimension" CssClass="inputelement" AutoPostBack="true"
                                            OnSelectedIndexChanged="DdlDashboardTable_SelectedIndexChanged" ID="ddlDashboardTable"
                                            runat="server" OnLoad="DdlDashboardTable_Load">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator Display="Dynamic" ID="RequiredFieldValidator2" runat="server"
                                            ControlToValidate="dpLinkTitle" ValidationGroup="ddlDashboardTable" ErrorMessage="Please select fact table."></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="ms-formlabel">
                                        <h3 class="ms-standardheader">Title<b style="color: Red">*</b>:
                                        </h3>
                                    </td>
                                    <td class="ms-formbody">
                                        <span class="content-edit">
                                            <asp:TextBox CssClass="inputelement" ID="dpLinkTitle" runat="server"></asp:TextBox></span>
                                        <span class="content-edit">
                                            <asp:CheckBox ID="cbHideKpiTitle" Text="Hide Title" runat="server" />
                                        </span>

                                        <asp:RequiredFieldValidator Display="Dynamic" ID="RequiredFieldValidator1" runat="server"
                                            ControlToValidate="dpLinkTitle" ValidationGroup="editDimension" ErrorMessage="Please enter link title."></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="ms-formlabel">
                                        <h3 class="ms-standardheader">Format:<i>Use $exp$ Token As Expression</i>
                                        </h3>
                                    </td>
                                    <td class="ms-formbody">
                                        <span class="content-edit" >
                                        <asp:TextBox CssClass="inputelement" ID="dpLinkName" runat="server"></asp:TextBox>
                                        </span>
                                        <span class="content-edit" >
                                           &nbsp;Unit:&nbsp;
                                        </span>
                                       <span class="content-edit" >
                                            <asp:TextBox ID="txtBarUnit" runat="server"></asp:TextBox>
                                       </span>
                                    </td>
                                </tr>
                                <tr id="txtMaxLimitContainer" runat="server" visible="false">
                                    <td class="ms-formlabel">
                                        <h3 class="ms-standardheader">Max Limit:<i>default is 100</i>
                                        </h3>
                                    </td>
                                    <td class="ms-formbody">
                                        <asp:TextBox CssClass="inputelement" ID="txtMaxLimit" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                
                                <tr>
                                    <td class="ms-formlabel">
                                        <h3 class="ms-standardheader">Filter:
                                        </h3>
                                    </td>
                                    <td class="ms-formbody">
                                        <div class="contentdiv">
                                            <span>Date Range:</span>
                                            <span>
                                                <asp:DropDownList  OnLoad="DdlBasicDateFilterDefaultView_Load"
                                                    EnableViewState="true" ID="ddlBasicDateFilterDefaultView" runat="server">
                                                </asp:DropDownList>
                                            </span>
                                            <span>
                                                <asp:DropDownList 
                                                    EnableViewState="true" ID="ddlBasicDateFilterStartField" runat="server">
                                                </asp:DropDownList>
                                            </span>
                                        </div>
                                        <div class="contentdiv">
                                            <span class="content-edit" style="width:95%;">
                                                <asp:TextBox ID="txtFilter" Width="99%" runat="server" TextMode="MultiLine"></asp:TextBox>
                                            </span>
                                            <span class="content-edit" style="padding-top:1px;">
                                                 <img src="/content/images/edit-icon.png" onclick="createBasicFilterFormula(this, '<%= txtFilter.ClientID %>')" />
                                            </span>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="ms-formlabel">
                                        <h3 class="ms-standardheader">Aggregate:
                                        </h3>
                                    </td>
                                    <td class="ms-formbody">
                                          <div class="contentdiv">
                                        <span class="content-edit">
                                            <asp:DropDownList Width="100px" Height="20px" CssClass="operatorinput" ID="ddlDimensionAggragate"
                                                runat="server">
                                                <asp:ListItem Text="(None)" Value=""></asp:ListItem>
                                                <asp:ListItem Text="Count" Value="Count"></asp:ListItem>
                                                <asp:ListItem Text="Sum" Value="sum"></asp:ListItem>
                                                <asp:ListItem Text="Average" Value="avg"></asp:ListItem>
                                                <asp:ListItem Text="Maximum" Value="max"></asp:ListItem>
                                                <asp:ListItem Text="Minimum" Value="min"></asp:ListItem>
                                            </asp:DropDownList>
                                        </span>
                                        <span><asp:CheckBox ID="cbCalculatePct" Text="%" runat="server" /></span>
                                        </div>
                                        <div class="contentdiv">
                                        <span class="content-edit" style="width:95%;">
                                            <asp:TextBox CssClass="inputelement" Width="99%" TextMode="MultiLine" ID="txtAggragateOf" runat="server"></asp:TextBox>
                                        </span>
                                         <span class="content-edit" style="padding-top:1px;">
                                          <img src="/content/images/edit-icon.png" onclick="createAggragateOf(this, '<%= txtAggragateOf.ClientID %>')" />
                                        </span>
                                       </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="ms-formlabel" style="vertical-align:top;">
                                        <h3 class="ms-standardheader">Show Bar:
                                        </h3>
                                    </td>
                                    <td class="ms-formbody">
                                        
                                        <div class="contentdiv">
                                            <asp:CheckBox ID="cbShowBar" AutoPostBack="true" runat="server" OnCheckedChanged="CBShowBar_CheckedChanged"/>
                                        </div>
                                        <asp:Panel ID="pShowBar" runat="server" CssClass="contentdiv pshowbar" Visible="false">
                                            <div class="contentdiv">
                                                <div class="fullwidth" style="padding-bottom:5px;">
                                                    <span class="content-edit">Font Color:</span>
                                                    <dx:ASPxColorEdit Width="100px" ID="txtFontColor" runat="server"></dx:ASPxColorEdit>
                                                </div>
                                                <div class="fullwidth" style="padding-bottom:5px;">
                                                    <span class="content-edit">&nbsp;&nbsp;Default Background:</span>
                                                    <dx:ASPxColorEdit Width="100px" ID="txtBarDefaulColor" runat="server"></dx:ASPxColorEdit>
                                                </div>
                                            </div>
                                            <div class="contentdiv">
                                                <span class="content-edit">Conditions:</span>
                                            </div>
                                            <asp:Panel ID="pBarCond1" runat="server" CssClass="barcondition ">
                                                <span class="content-edit">1.
                                                </span>
                                                <span class="content-edit">
                                                    <asp:TextBox ID="txtCond1Bar" Width="70px" runat="server"></asp:TextBox>
                                                </span>
                                                <span class="content-edit">
                                                    <asp:DropDownList Width="50px" Height="20px" CssClass="operatorinput" ID="txtCond1Operator"
                                                        runat="server">
                                                        <asp:ListItem Text="=" Value="="></asp:ListItem>
                                                        <asp:ListItem Text="!=" Value="!="></asp:ListItem>
                                                        <asp:ListItem Text=">=" Value=">="></asp:ListItem>
                                                        <asp:ListItem Text="<=" Value="<="></asp:ListItem>
                                                        <asp:ListItem Text=">" Value=">"></asp:ListItem>
                                                        <asp:ListItem Text="<" Value="<"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </span>
                                                 <dx:ASPxColorEdit  Width="100px"  ID="txtCond1Color" runat="server"></dx:ASPxColorEdit>
                                             
                                            </asp:Panel>
                                            <asp:Panel ID="pBarCond2" runat="server" CssClass="barcondition">
                                                <span class="content-edit">2.
                                                </span>
                                                <span class="content-edit">
                                                    <asp:TextBox ID="txtCond2Bar" Width="70px" runat="server"></asp:TextBox>
                                                </span>
                                                <span class="content-edit">
                                                    <asp:DropDownList Width="50px" Height="20px" CssClass="operatorinput" ID="txtCond2Operator"
                                                        runat="server">
                                                        <asp:ListItem Text="=" Value="="></asp:ListItem>
                                                        <asp:ListItem Text="!=" Value="!="></asp:ListItem>
                                                        <asp:ListItem Text=">=" Value=">="></asp:ListItem>
                                                        <asp:ListItem Text="<=" Value="<="></asp:ListItem>
                                                        <asp:ListItem Text=">" Value=">"></asp:ListItem>
                                                        <asp:ListItem Text="<" Value="<"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </span>
                                                 <dx:ASPxColorEdit  Width="100px"  ID="txtCond2Color" runat="server"></dx:ASPxColorEdit>
                                               
                                            </asp:Panel>
                                            <asp:Panel ID="pBarCond3" runat="server" CssClass="barcondition">
                                                <span class="content-edit">3.
                                                </span>
                                                <span class="content-edit">
                                                    <asp:TextBox ID="txtCond3Bar" Width="70px" runat="server"></asp:TextBox>
                                                </span>
                                                <span class="content-edit">
                                                    <asp:DropDownList Width="50px" Height="20px" CssClass="operatorinput" ID="txtCond3Operator"
                                                        runat="server">
                                                        <asp:ListItem Text="=" Value="="></asp:ListItem>
                                                        <asp:ListItem Text="!=" Value="!="></asp:ListItem>
                                                        <asp:ListItem Text=">=" Value=">="></asp:ListItem>
                                                        <asp:ListItem Text="<=" Value="<="></asp:ListItem>
                                                        <asp:ListItem Text=">" Value=">"></asp:ListItem>
                                                        <asp:ListItem Text="<" Value="<"></asp:ListItem>
                                                    </asp:DropDownList>

                                                </span>
                                                <dx:ASPxColorEdit  Width="100px"  ID="txtCond3Color" runat="server"></dx:ASPxColorEdit>


                                            </asp:Panel>
                                            <asp:Panel ID="pBarCond4" runat="server" CssClass="barcondition">
                                                <span class="content-edit">4.
                                                </span>
                                                <span class="content-edit">
                                                    <asp:TextBox ID="txtCond4Bar" Width="70px" runat="server"></asp:TextBox>
                                                </span>
                                                <span class="content-edit">
                                                    <asp:DropDownList Width="50px" Height="20px" CssClass="operatorinput" ID="txtCond4Operator"
                                                        runat="server">
                                                        <asp:ListItem Text="=" Value="="></asp:ListItem>
                                                        <asp:ListItem Text="<>" Value="!="></asp:ListItem>
                                                        <asp:ListItem Text=">=" Value=">="></asp:ListItem>
                                                        <asp:ListItem Text="<=" Value="<="></asp:ListItem>
                                                        <asp:ListItem Text=">" Value=">"></asp:ListItem>
                                                        <asp:ListItem Text="<" Value="<"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </span>
                                                <dx:ASPxColorEdit  ID="txtCond4Color" Width="100px" runat="server"></dx:ASPxColorEdit>

                                            </asp:Panel>
                                        </asp:Panel>
                                    </td>
                                </tr>
                             
                                <tr>
                                    <td class="ms-formlabel">
                                        <h3 class="ms-standardheader">Order:
                                        </h3>
                                    </td>
                                    <td class="ms-formbody">
                                        <asp:TextBox CssClass="inputelement" ID="dpLinkOrder" runat="server"></asp:TextBox>
                                    </td>
                                </tr>

                                <tr>
                                    <td class="ms-formlabel">
                                        <h3 class="ms-standardheader">Drill Down Type: <%--<asp:CheckBox ID="cbStopLinkDetail" runat="server" Checked="true" />--%>
                                        </h3>
                                    </td>
                                    <td class="ms-formbody">
                                        <span class="content-edit">
                                            <select id="slDrillDownType" runat="server" on onchange="slDrillDownType_SelectionChange(this);" >
                                                <option value="No Drill Down">No Drill Down</option>
                                                <option value="Default Drill Down">Default Drill Down</option>
                                                <option value="Custom Url">Custom Url</option>
                                            </select>
                                            <asp:TextBox CssClass="dpLinkUrl"  ID="dpLinkUrl" Width="250px" runat="server"></asp:TextBox></span>
                                        <select id="slNonModuleColumns" runat="server" >
                                               
                                            </select>
                                        <span>
                                           <%-- <asp:CheckBox ID="cbDefaultLink" runat="server" Text="Use Default" />--%>
                                        </span>
                                    </td>
                                </tr>

                                   <tr>
                                    <td class="ms-formlabel">
                                        <h3 class="ms-standardheader">Navigation Type:
                                        </h3>
                                    </td>
                                    <td class="ms-formbody">
                                        <asp:RadioButton Text="Navigate" runat="server" ID="dpWindowView" GroupName="viewtype" TextAlign="Right" />
                                        <asp:RadioButton Text="Popup" runat="server" ID="dpPopupView" GroupName="viewtype" TextAlign="Right" />
                                    </td>
                                </tr>

                                <tr>
                                    <td class="ms-formlabel">
                                        <h3 class="ms-standardheader">Other:
                                        </h3>
                                    </td>
                                    <td class="ms-formbody">
                                        <asp:CheckBox ID="dpHideKPI" runat="server" Text="Hide KPI" TextAlign="Right" />
                                        <asp:CheckBox ID="dpUseAsPanel" runat="server" Text="Use As Panel" TextAlign="Right" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="text-align:right">
                                        <div class="actiondiv">
                                            <span class="content-action">
                                                <dx:ASPxButton ID="btnSaveDimension" runat="server" ValidationGroup="editDimension" Text="Save Changes" CssClass="primary-blueBtn"
                                                     OnClick="BtnSaveDimension_Click"></dx:ASPxButton>
                                            </span>
                                            <span class="content-action">
                                                <dx:ASPxButton ID="btnResetDimension" runat="server" Text="Reset" OnClick="BtnResetDimension_Click" CssClass="primary-blueBtn">

                                                </dx:ASPxButton>
                                            </span>

                                            <%--<asp:LinkButton ID="LinkButton1" Visible="false"  runat="server"  Text="&nbsp;&nbsp;Save Changes&nbsp;&nbsp;" ToolTip="Save Changes">
                                            <span class="button-bg">
                                                <b style="float: left; font-weight: normal;">
                                                   Save Changes</b>
                                                </span>
                                            </asp:LinkButton>--%>

                                            <%--<asp:LinkButton ID="btNewbutton" Visible="false"   runat="server"  Text="&nbsp;&nbsp;Reset&nbsp;&nbsp;" ToolTip="Reset" OnClick="BtnResetDimension_Click">
                                            <span class="button-bg">
                                                <b style="float: left; font-weight: normal;">
                                                   Reset</b>
                                                </span>
                                            </asp:LinkButton>--%>
                                        </div>
                                    </td>
                                </tr>
                            </table>         
                    </td>
                    <td valign="top" class="list-td">
                        <fieldset style="width: 96%">
                            <legend>KPIs</legend>
                            <div class="dimension-list">
                                <asp:Repeater ID="rprDimensions" runat="server">
                                    <ItemTemplate>
                                        <div class="dimension-content">
                                            <div class="dimension-head">
                                                <span>
                                                    <%# 
                                                                Container.ItemIndex + 1
                                                    %>.</span><span><b>KPI:</b><i><%# ((DashboardPanelLink)Container.DataItem).Title %></i></span></div>
                                            <div class="dimension-detail">
                                                <span><b>Format: </b><i>
                                                    <%# ((DashboardPanelLink)Container.DataItem).ExpressionFormat %></i></span> <span><b>Title: </b><i>
                                                        <%# ((DashboardPanelLink)Container.DataItem).Title.ToString() %></i>
                                                        <b>Filter: </b><i>
                                                            <%# ((DashboardPanelLink)Container.DataItem).Filter %></i></span> <span><b>Aggregate: </b><i>
                                                                <%# ((DashboardPanelLink)Container.DataItem).AggragateFun %>(<%# ((DashboardPanelLink)Container.DataItem).AggragateOf%>) </i>
                                                            </span>
                                            </div>
                                            <div class="dimension-detail">
                                                <span>
                                                    <b>Hide: </b><i><%# ((DashboardPanelLink)Container.DataItem).IsHide%></i><b>&nbsp;Use As Panel: </b><i><%# ((DashboardPanelLink)Container.DataItem).UseAsPanel%></i>
                                                    <b>Detail View: </b><i><%# ((DashboardPanelLink)Container.DataItem).ScreenView == 1 ? "Popup" : "Window"%></i>
                                                    <b>Order: </b><i><%# ((DashboardPanelLink)Container.DataItem).Order%></i>
                                                </span>
                                            </div>
                                        </div>
                                        <div class="dimension-action">
                                            <span>
                                                <asp:ImageButton CommandArgument='<%# ((DashboardPanelLink)Container.DataItem).LinkID %>' ImageUrl="/content/images/edit-icon.png"
                                                    AlternateText="Edit" ID="btnEditDimension" runat="server" OnClick="BtnEditDimension_Click" />
                                            </span><span>
                                                <asp:ImageButton CommandArgument='<%# ((DashboardPanelLink)Container.DataItem).LinkID %>' ImageUrl="/content/images/delete-icon.png"
                                                    AlternateText="Delete" ID="btnDeleteDimension" runat="server" OnClientClick='<%# Eval("Title", "return confirm(\"Are your sure, you want to delete {0} dimension?\");") %>'
                                                    OnClick="BtnDeleteDimension_Click" />
                                            </span>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                        </fieldset>
                    </td>
                </tr>
            </table>
        </asp:Panel>

    </div>
    <div style="display: none;" class="containermain " id="tabPanel_3">
        <asp:Panel ID="tabPanelconainer4" runat="server">
            <table width="100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td width="40%" rowspan="2">

                        <fieldset>
                            <legend>Appearance</legend>
                            <table cellpadding="0" cellspacing="0" style="border-collapse: collapse; width: 100%">
                                <tr>
                                    <td>
                                        <div>
                                            <span class="label">View Type</span>
                                        </div>
                                        <div>
                                            <span>
                                                
                                                    <asp:DropDownList ID="ddlPanelViewType" runat="server">
                                                        
                                                    </asp:DropDownList>
                                                
                                             </span>
                                          </div>
                                        <div>
                                            <span class="label">Width<b style="color: Red">*</b>:</span>
                                        </div>
                                        <div>
                                            <span>
                                                <asp:TextBox ValidationGroup="apperanceGroup" CssClass="vd-widthtxtbox" ID="txtWidth"
                                                    runat="server" Text="300"></asp:TextBox></span>
                                        </div>
                                        <div>
                                            <asp:RequiredFieldValidator ValidationGroup="apperanceGroup" ID="txtWidthRFValidator"
                                                runat="server" ControlToValidate="txtWidth" Display="Dynamic" ErrorMessage="Please specify width"></asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ValidationGroup="apperanceGroup" ID="txtWidthREValidator"
                                                ValidationExpression="[0-9]+" runat="server" ControlToValidate="txtWidth" Display="Dynamic"
                                                ErrorMessage="Please specify width in digit"></asp:RegularExpressionValidator>
                                        </div>
                                        <div>
                                            <span class="label">Height<b style="color: Red">*</b>:</span>
                                        </div>
                                        <div>
                                            <span>
                                                <asp:TextBox CssClass="vd-widthtxtbox" ValidationGroup="apperanceGroup" ID="txtHeight"
                                                    runat="server" Text="300"></asp:TextBox></span>
                                        </div>
                                        <div>
                                            <asp:RequiredFieldValidator ValidationGroup="apperanceGroup" ID="txtHeightRFValidator"
                                                runat="server" ControlToValidate="txtHeight" Display="Dynamic" ErrorMessage="Please specify height"></asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ValidationGroup="apperanceGroup" ID="txtHeightREValidator"
                                                ValidationExpression="[0-9]+" runat="server" ControlToValidate="txtHeight" Display="Dynamic"
                                                ErrorMessage="Please specify height in digit"></asp:RegularExpressionValidator>
                                        </div>
                                        <div>
                                            <span class="label">Dashboard Theme:</span>
                                        </div>
                                        <div>
                                            <span>
                                                <asp:DropDownList ID="ddlDashboardTheme" CssClass="vd-widthtxtbox" runat="server">
                                                    <asp:ListItem Text="Accent1" Value="Accent1"></asp:ListItem>
                                                    <asp:ListItem Text="Accent2" Value="Accent2"></asp:ListItem>
                                                    <asp:ListItem Text="Accent3" Value="Accent3"></asp:ListItem>
                                                    <asp:ListItem Text="Accent4" Value="Accent4"></asp:ListItem>
                                                    <asp:ListItem Text="Accent5" Value="Accent5"></asp:ListItem>
                                                    <asp:ListItem Text="Accent6" Value="Accent6"></asp:ListItem>
                                                </asp:DropDownList>
                                            </span>
                                        </div>

                                         <div>
                                            <span class="label">Header Font Style:</span>
                                        </div>
                                        <div>
                                            <span>
                                                <span style="float: left">
                                                    <asp:DropDownList ID="ddlHeaderFontStyle" runat="server">
                                                        <asp:ListItem Value="Regular">Regular</asp:ListItem>
                                                        <asp:ListItem Value="Bold">Bold</asp:ListItem>
                                                        <asp:ListItem Value="Italic">Italic</asp:ListItem>
                                                        <asp:ListItem Value="Underline">Underline</asp:ListItem>
                                                    </asp:DropDownList>
                                                </span>
                                                <span style="float: left">
                                                    <asp:DropDownList ID="ddlHeaderFontSize" runat="server">
                                                        <asp:ListItem Value="6pt">6pt</asp:ListItem>
                                                        <asp:ListItem Value="8pt" Selected="True">8pt</asp:ListItem>
                                                        <asp:ListItem Value="10pt">10pt</asp:ListItem>
                                                        <asp:ListItem Value="12pt">12pt</asp:ListItem>
                                                        <asp:ListItem Value="14pt">14pt</asp:ListItem>
                                                        <asp:ListItem Value="18pt">18pt</asp:ListItem>
                                                        <asp:ListItem Value="24pt">24pt</asp:ListItem>
                                                        <asp:ListItem Value="30pt">30pt</asp:ListItem>
                                                        <asp:ListItem Value="36pt">36pt</asp:ListItem>
                                                    </asp:DropDownList>
                                                </span>
                                                <span style="float: left">
                                                    <dx:ASPxColorEdit runat="server" AllowUserInput="true" Width="165px" ClientEnabled="true" ClientInstanceName="ceHeaderFont" ID="ceHeaderFont" Color="#000000">
                                                    </dx:ASPxColorEdit>
                                                </span>
                                            </span>
                                        </div>

                                        <br /><br />
                                        <div>
                                            <span class="label">Font Style:</span>
                                        </div>
                                        <div>
                                            <span>
                                                <span style="float: left">
                                                    <asp:DropDownList ID="ddlFontStyle" runat="server">
                                                        <asp:ListItem Value="Regular">Regular</asp:ListItem>
                                                        <asp:ListItem Value="Bold">Bold</asp:ListItem>
                                                        <asp:ListItem Value="Italic">Italic</asp:ListItem>
                                                        <asp:ListItem Value="Underline">Underline</asp:ListItem>
                                                    </asp:DropDownList>
                                                </span>
                                                <span style="float: left">
                                                    <asp:DropDownList ID="ddlFontSize" runat="server">
                                                        <asp:ListItem Value="6pt">6pt</asp:ListItem>
                                                        <asp:ListItem Value="8pt" Selected="True">8pt</asp:ListItem>
                                                        <asp:ListItem Value="10pt">10pt</asp:ListItem>
                                                        <asp:ListItem Value="12pt">12pt</asp:ListItem>
                                                        <asp:ListItem Value="14pt">14pt</asp:ListItem>
                                                        <asp:ListItem Value="18pt">18pt</asp:ListItem>
                                                        <asp:ListItem Value="24pt">24pt</asp:ListItem>
                                                        <asp:ListItem Value="30pt">30pt</asp:ListItem>
                                                        <asp:ListItem Value="36pt">36pt</asp:ListItem>
                                                    </asp:DropDownList>
                                                </span>
                                                <span style="float: left">
                                                    <dx:ASPxColorEdit runat="server" AllowUserInput="true" Width="165px" ClientEnabled="true" ClientInstanceName="ceFont" ID="ceFont" Color="#000000">
                                                    </dx:ASPxColorEdit>
                                                </span>
                                            </span>
                                        </div>
                                        <%--start--%>
                                        <br /><br /> 
                                         <div>
                                             <span class="label">Hide chart:</span> <span>
                                             <asp:CheckBox ID="CheckBoxHideChart" runat="server"  TextAlign="Right" /></span>
                                        </div>
                                         <br /><br />
                                        <div>
                                            <span class="label">Chart Type</span>
                                        </div>
                                         <div>
                                            <span>
                                                <span style="float: left">
                                                    <asp:DropDownList ID="ddlChartType" runat="server" OnSelectedIndexChanged="ddlChartType_SelectedIndexChanged">
                                                        <asp:ListItem Value="Doughnut">Doughnut</asp:ListItem>
                                                        <asp:ListItem Value="Pie">Pie</asp:ListItem>
                                                          <asp:ListItem Value="DoughnutOnly">Doughnut Only</asp:ListItem>
                                                         <asp:ListItem Value="PieOnly">Pie Only</asp:ListItem>
                                                        <asp:ListItem Value="linechart">Line Chart</asp:ListItem>
                                                        <asp:ListItem Value="squarechart">Square Chart</asp:ListItem>
                                                    </asp:DropDownList>
                                                </span>
                                             </span>
                                          </div>
                                       <%-- end--%>
                                    </td>
                                </tr>
                                <tr id="trLegandAlignment" runat="server" visible="false">
                                    <td><span class="label">Horizontal Alignment :</span></td><td><asp:DropDownList ID="ddlDoughnutOnlyHorizontalPosition" runat="server">
                                        <asp:ListItem Text="Center" Value="center"></asp:ListItem>
                                        <asp:ListItem Text="Left" Value="left"></asp:ListItem>
                                        <asp:ListItem Text="Right" Value="right"></asp:ListItem>
                                                           </asp:DropDownList></td>
                                    <td><span class="label">Vertical Alignmen :</span><asp:DropDownList ID="ddlDoughnutOnlyVerticalPosition" runat="server">
                                          <asp:ListItem Text="Top" Value="top"></asp:ListItem>
                                        <asp:ListItem Text="Bottom" Value="bottom"></asp:ListItem>
                                        </asp:DropDownList></td>
                                </tr>
                                <tr class="donutCardProp">
                                    <td  id="trTextFormat">
                                        <div><span class="label">Display Format :</span></div><div><asp:DropDownList ID="ddlDoughnutOnlyTextFormat" runat="server">
                                            <asp:ListItem Text="" Value="none" Enabled="true"></asp:ListItem>
                                            <asp:ListItem Value="currency" Text="Currency"></asp:ListItem>  
                                            <asp:ListItem Text="Millions" Value="millions"></asp:ListItem>
                                             <asp:ListItem Value="percentage" Text="Percentage"></asp:ListItem>
                                                             </asp:DropDownList></div>
                                    </td>
                                      <td  id="trEnableToolTip">
                                        <div><span class="label"> Show  Legend :</span></div><div><asp:DropDownList ID="ddlEnableToolTip" runat="server">
                                            <asp:ListItem Text="Yes" Value="true"></asp:ListItem>
                                            <asp:ListItem Text="No" Value="false"></asp:ListItem>
                                                                      </asp:DropDownList></div>
                                    </td>
                                     <td  id="trCentreTitle">
                                        <div><span class="label">Centre Title :</span></div><div><asp:TextBox ID="txtCentreTitle" runat="server"></asp:TextBox></div>
                                    </td>
                                 </tr>
                                <tr>
                                    <td>
                                        <dx:ASPxButton ID="btSaveApperanceChanges" Text="Save Changes" runat="server" OnClick="BtSaveApperanceChanges_Click" CssClass="primary-blueBtn">

                                        </dx:ASPxButton>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                        <fieldset>
                            <legend>Template</legend>
                            <asp:HiddenField ID="hfTemplateID" runat="server" />
                            <asp:HiddenField ID="hfOverrideTemplate" runat="server" />
                            <div>
                                <div>
                                    <label>
                                        Override with Template:
                                    </label>
                                </div>
                                <div>
                                    <span>
                                        <asp:DropDownList ID="ddlTemplateList" runat="server" OnPreRender="DdlTemplateList_PreRender">
                                        </asp:DropDownList>
                                    </span>
                                </div>
                            </div>
                            <div>
                                <span>
                                    <dx:ASPxButton ID="btSaveAsTemmplate"  runat="server" Text="Save As Template" OnClick="BtSaveAsTemplate_Click" CssClass="primary-blueBtn">
                                        <ClientSideEvents Click="confirmWhileTemplateSaving" />
                                    </dx:ASPxButton>
                                </span>
                            </div>
                        </fieldset>

                    </td>
                    <td valign="top" align="left">
                        <div style="float: left">
                            <%--<asp:Button ID="btRefreshPreview" runat="server" Text="Refresh" />--%>
                            <dx:ASPxButton ID="btRefreshPreview" runat="server" Text="Refresh" CssClass="primary-blueBtn"></dx:ASPxButton>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:UpdatePanel ID="upPreview" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Panel ID="dashboardPreview" runat="server" OnPreRender="DashboardPreview_PreRender">
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
            </table>
            </table>
        </asp:Panel>
    </div>

</div>


<!--Start:Create formula for basic filter-->
<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
    .filterformulapopup
    {
        float: left;
        width: 440px;
        height: 170px;
        position: absolute;
        z-index: 1000;
        text-align: center;
        background: white;
        border: 2px outset gray;
    }

        .filterformulapopup .sub-container
        {
            float: inherit;
            width: 100%;
        }

        .filterformulapopup .titlediv
        {
            float: left;
            width: 100%;
            padding: 10px 0px 10px 10px;
        }

            .filterformulapopup .titlediv span
            {
                float: inherit;
                font-weight: bold;
                padding-left: 5px;
            }

        .filterformulapopup .contentdiv
        {
            float: inherit;
            width: 100%;
            padding-top: 2px;
        }

        .filterformulapopup .content-head
        {
            float: inherit;
        }

    #formulaStartWith
    {
        width: 60px;
    }

    #formulaOperatorForPopup
    {
        width: 55px;
    }

    #fieldValueForPopup
    {
        width: 80px;
    }

    #formulaBoxForPop
    {
        width: 400px;
    }
</style>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function createBasicFilterFormula(obj, formulatTextBoxID) {

        $("#filterFormulaPopup #formulaTxtBoxID").val(formulatTextBoxID);

        $("#formulaBoxForPop").val("");
        $("#formulatEditorError").html('');
        $("#filterFormulaPopup #formulaStartWith").attr("disabled", true);
        ShowValueBoxBasedOnDataType();
        var currentPosition = $(obj).position();

        var left = currentPosition.left - $("#filterFormulaPopup").width();
        if (left <= 0) {
            left = 20;
        }
        $("#filterFormulaPopup").css({ "display": "block", "top": (currentPosition.top - 20) + "px", "left": (left + 10) + "px" });

    }

    function ShowValueBoxBasedOnDataType() {
        var fieldDropdown = $("#filterFormulaPopup .formulafields");
        var fieldValueContainer = $("#filterFormulaPopup #fieldValueContainerForPopup");
        if (fieldDropdown.attr('datatype') && fieldDropdown.attr('datatype').toLowerCase() == "datetime") {
            fieldValueContainer.html("<input type='text' id='fieldValueForPopup'>");
        }
        else {
            fieldValueContainer.html("<input type='text' id='fieldValueForPopup'>");
        }
    }

    function popupAddFormulaExpression(obj) {
        var fieldStartWith = $("#filterFormulaPopup #formulaStartWith");
        var fieldDropdown = $("#filterFormulaPopup .formulafields");
        var fieldValueBox = $("#filterFormulaPopup #fieldValueForPopup");
        var fieldValueOperator = $("#filterFormulaPopup #formulaOperatorForPopup");

        var fieldOption = $("#filterFormulaPopup .formulafields option:selected");

        //Gets errorbox and clear error
        var errorBox = $("#formulatEditorError");
        errorBox.html('');

        //Gets datatype of field
        var dataType = $.trim(fieldOption.text().toLowerCase());
        if ($.trim(fieldValueBox.val()) != "") {
            if (dataType.indexOf("(string)") > 0) {

            }
            else if (dataType.indexOf("(datetime)") > 0) {

            }
            else {
                if (!Number(fieldValueBox.val()) || Number(fieldValueBox.val()) == NaN) {
                    errorBox.html("Please enter value in digit.");
                    return;
                }
            }
        }
        else {
            errorBox.html("Please enter value.");
            return;
        }

        var fExpression = "";
        if ($.trim($("#formulaBoxForPop").val()) != "") {
            fExpression += fieldStartWith.val() + " ";
        }
        else {
            fieldStartWith.removeAttr("disabled");
        }


        fExpression += "[" + fieldDropdown.val() + "] " + fieldValueOperator.val() + " ";


        if (dataType.indexOf("(string)") > 0) {
            fExpression += "'" + fieldValueBox.val() + "' ";
        }
        else if (dataType.indexOf("(datetime)") > 0) {
            fExpression += "#" + fieldValueBox.val() + "# ";
        }
        else {
            fExpression += fieldValueBox.val() + " ";
        }

        $("#formulaBoxForPop").val($.trim($("#formulaBoxForPop").val()) + " " + fExpression);
    }

    function popupAddFormulaDone(obj) {

        $("#" + $("#filterFormulaPopup #formulaTxtBoxID").val()).val($("#formulaBoxForPop").val());
        $("#filterFormulaPopup").css("display", "none");
    }

    function popupAddFormulaCancel(obj) {

        var fieldStartWith = $("#filterFormulaPopup #formulaStartWith");
        var fieldDropdown = $("#filterFormulaPopup .formulafields");
        var fieldValueBox = $("#filterFormulaPopup #fieldValueForPopup");
        var fieldValueOperator = $("#filterFormulaPopup #formulaOperatorForPopup");
        fieldStartWith.css("disabled", true);

        fieldValueBox.val("");
        $("#filterFormulaPopup #formulaTxtBoxID").val("");
        $("#filterFormulaPopup").css("display", "none");
    }


</script>
<div id="filterFormulaPopup" class="filterformulapopup" style="display: none">
    <div class="sub-container">
        <input type="hidden" id="formulaTxtBoxID" />
        <table>

            <tr>
                <td align="left" valign="top" width="65">
                    <div><span>Start With:</span></div>
                    <div>
                        <span>
                            <select id="formulaStartWith" disabled="disabled">
                                <option value="AND">And</option>
                                <option value="OR">Or</option>
                            </select>
                        </span>
                    </div>
                </td>
                <td align="left" valign="top" width="100">
                    <div><span>Fact table Fields:</span></div>
                    <div>
                        <span id="ftFieldsContainer">
                            <asp:DropDownList ID="ddlFactTableFieldsForFilter" Width="130" CssClass="formulafields" runat="server"></asp:DropDownList>
                        </span>
                    </div>
                </td>
                <td align="left" valign="top" width="50">
                    <div><span>Operator:</span></div>
                    <div>
                        <span>
                            <select id="formulaOperatorForPopup">
                                <option value="=">=</option>
                                <option value="<>">!=</option>
                                <option value="<">&lt;</option>
                                <option value=">">&gt;</option>
                                <option value=">=">&gt;=</option>
                                <option value="<=">&lt;=</option>
                            </select>
                        </span>
                    </div>
                </td>
                <td align="left" valign="top" width="100">
                    <div><span>Value:</span></div>
                    <div>
                        <span id="fieldValueContainerForPopup"></span>
                        <span>
                            <img src="/content/images/add_icon.png" style="cursor: pointer;" alt="Add" onclick="popupAddFormulaExpression(this)" />
                        </span>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <div style="color: Red;" id="formulatEditorError"></div>
                </td>
            </tr>
            <tr>
                <td colspan="4" align="left" valign="top">
                    <textarea id="formulaBoxForPop" rows="3" cols="3">
      </textarea>
                </td>
            </tr>
            <tr>
                <td></td>
                <td></td>
                <td align="right" colspan="2">
                    <input type="button" value="Done" onclick="popupAddFormulaDone(this)" />
                    <input type="button" value="Cancel" onclick="popupAddFormulaCancel(this)" />
                </td>
            </tr>
        </table>
    </div>
</div>
<!--Close:Create formula for basic filter-->


<!--Start:Create Aggregate of-->
<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
    .aggragateofpopup
    {
        float: left;
        width: 440px;
        height: 170px;
        position: absolute;
        z-index: 1000;
        text-align: center;
        background: white;
        border: 2px outset gray;
    }

        .aggragateofpopup .sub-container
        {
            float: inherit;
            width: 100%;
        }

        .aggragateofpopup .titlediv
        {
            float: left;
            width: 100%;
            padding: 10px 0px 10px 10px;
        }

            .aggragateofpopup .titlediv span
            {
                float: inherit;
                font-weight: bold;
                padding-left: 5px;
            }

        .aggragateofpopup .contentdiv
        {
            float: left;
            width: 100%;
            padding-top: 2px;
        }

        .aggragateofpopup .content-head
        {
            float: inherit;
        }

    .aggragateboxforpop
    {
        width: 400px;
    }
</style>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function createAggragateOf(obj, formulatTextBoxID) {

        $("#aggragateOfPopup #aggragateTxtBoxID").val(formulatTextBoxID);

        $("#aggragateOfPopup .aggragateboxforpop").val("");
        $("#aggEditorError").html('');

        var currentPosition = $(obj).position();

        var left = currentPosition.left - $("#aggragateOfPopup").width();
        if (left <= 0) {
            left = 20;
        }
        $("#aggragateOfPopup").css({ "display": "block", "top": (currentPosition.top - 20) + "px", "left": (left + 10) + "px" });

    }
    $(document).on("change", "#<%=ddlChartType.ClientID%>", function () {
        var selectedOption = $(this).children("option:selected").val();
        if (selectedOption.toLowerCase() == 'pieonly') {
            showHideDonughtOnlyControl(true, 'pieonly');
        }
        else if (selectedOption.toLowerCase() == 'doughnutonly') {
            showHideDonughtOnlyControl(true, 'doughnutonly')
        }
        else {
            showHideDonughtOnlyControl(false, '')
        }
    });
    $(document).ready(function () {
        setTimeout(function () {
            var selectedOption = $("#<%=ddlChartType.ClientID%>").children("option:selected").val();
            if (selectedOption.toLowerCase() == 'pieonly') {
                showHideDonughtOnlyControl(true, 'pieonly');
            }
            else if (selectedOption.toLowerCase() == 'doughnutonly') {
                showHideDonughtOnlyControl(true, 'doughnutonly')
            }
            else {
                showHideDonughtOnlyControl(false, '')
            }
            
        }, 500);
    });


    function showHideDonughtOnlyControl(IsHide, chartType) {
        if (IsHide) {
            $("#trTextFormat").show();
            $("#trEnableToolTip").show();
           
            if (chartType.toLowerCase() == 'pieonly') {
                $("#trCentreTitle").hide();
            }
            else {
                $("#trCentreTitle").show();
            }
        }
        else {
            $("#trTextFormat").hide();
            $("#trEnableToolTip").hide();
            $("#trCentreTitle").hide();
        }
    }

    function popupAddAggragateExpression(obj) {

        var fieldDropdown = $("#aggragateOfPopup .formulafields");
        var fieldOption = $("#aggragateOfPopup .formulafields option:selected");

        var fExpression = "[" + fieldDropdown.val() + "]";
        $("#aggragateOfPopup .aggragateboxforpop").val(fExpression);
    }

    function popupAddAggragateDone(obj) {

        $("#" + $("#aggragateOfPopup #aggragateTxtBoxID").val()).val($("#aggragateOfPopup .aggragateboxforpop").val());
        $("#aggragateOfPopup").css("display", "none");
    }

    function popupAddAggragateCancel(obj) {
        var fieldDropdown = $("#aggragateOfPopup .formulafields");
        fieldDropdown.val("");
        $("#aggragateOfPopup .aggragateboxforpop").val("");
        $("#aggragateOfPopup #aggragateTxtBoxID").val("");
        $("#aggragateOfPopup").hide();
    }


</script>
<div id="aggragateOfPopup" class="aggragateofpopup" style="display: none">
    <div class="sub-container">
        <input type="hidden" id="aggragateTxtBoxID" />
        <table>
            <tr>
                <td align="left" valign="top" width="100">
                    <div><span>Fact table Fields:</span></div>
                    <div>
                        <span id="Span1">
                            <asp:DropDownList ID="dllAggragateFields" Width="130" CssClass="formulafields" runat="server"></asp:DropDownList>
                        </span>
                        <span>
                            <img src="/content/images/add_icon.png" style="cursor: pointer;" alt="Add" onclick="popupAddAggragateExpression(this)" />
                        </span>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div style="color: Red;" id="aggEditorError"></div>
                </td>
            </tr>
            <tr>
                <td align="left" valign="top">
                    <textarea class="aggragateboxforpop" rows="3" cols="3">
                    </textarea>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <input type="button" value="Done" onclick="popupAddAggragateDone(this)" />
                    <input type="button" value="Cancel" onclick="popupAddAggragateCancel(this)" />
                </td>
            </tr>
        </table>
    </div>
</div>
<!--Close:Create Aggregate of-->

<!-- Color picker Utility start-->

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    actionOnTabClick(document.getElementById("<%=hTabNumber.ClientID %>").value);
</script>

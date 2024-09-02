<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DashboardSideEditView.ascx.cs" Inherits="uGovernIT.Web.DashboardSideEditView" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    var url = '<%=dashboardUrl%>';

    function editDashboard(obj) {
        var title = '<%=dashboardName%>'
        var dashboardID = '<%=dashboardId%>';
        var dashboardType = '<%=dashboardType%>';
        var params = "";
        if (dashboardType.toLocaleLowerCase() == "panel") {
            params = "control=configdashboardpanel&DashboardID=" + dashboardID;
        }
        else if (dashboardType.toLocaleLowerCase() == "chart") {
            var params = "control=configdashboardchart&DashboardID=" + dashboardID;
        }

        window.parent.UgitOpenPopupDialog(url, params, title, 90, 80, false, escape(window.location.href));
        return false;
    }

    function pickSiteAsset(Url) {
        var siteAsset = unescape(Url);
        $('#<%=txtfileupload.ClientID%>').val(siteAsset);
    }
    function pickSiteAssetFortxtDBBackGround(Url) {
        var siteAsset = unescape(Url);
        $('#<%=txtDBBackGround.ClientID%>').val(siteAsset);
    }

    function pickSiteAssetFortxtfuicon(Url) {
        var siteAsset = unescape(Url);
        $('#<%=txtFuIcon.ClientID%>').val(siteAsset);
    }

</script>
<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
    .hide {
    }

    .servicecategory {
        padding-right: 10px;
        width: 130px;
        text-align: right;
        font-weight: bold;
    }

    .scroll {
        height: 200px;
        overflow: auto;
        width: 300px;
        border: 1px solid gray;
    }
</style>

<style type="text/css">
    .tabmaindiv {
        float: left;
        width: 100%;
    }

    .tabmaindivinner {
        float: left;
    }

    .tabspan {
        float: left;
        padding: 6px;
        margin-right: 2px;
    }

    .containermain {
        float: left;
        width: 100%;
        /*background:#E1E1E1;*/
    }

    .containerblockmain {
        float: left;
        width: 100%;
        border: 1px solid #000;
    }

    .moduleinfo-m {
        float: left;
        width: 100%;
        padding-top: 9px;
        padding-bottom: 15px;
    }

    .generalinfo-table {
        border-collapse: collapse;
        width: 100%;
    }

    .basicinfo-td {
        width: 60%;
    }

        .basicinfo-td fieldset {
            float: left;
            width: 97%;
            height: 200px;
        }

    .dashboardgroup-td {
        width: 40%;
    }

        .dashboardgroup-td fieldset {
            float: left;
            width: 95%;
            height: 200px;
        }

    .viewdashboard-td {
        width: 60%;
    }

        .viewdashboard-td fieldset {
            float: left;
            width: 97%;
        }

    .showinsidebar-td {
        width: 40%;
    }

        .showinsidebar-td fieldset {
            float: left;
            width: 95%;
            height: 200px;
        }

    .widthtxtbox {
        width: 95%;
    }

    .viewdashboard-left {
        float: left;
        width: 48%;
    }

    .viewdashboard-right {
        float: left;
        width: 48%;
    }

    .vd-widthtxtbox {
        width: 150px;
    }

    .showsidebar-div {
        float: left;
        width: 65%;
        height: 150px;
        overflow: scroll;
    }

    .showsidebar-checklist {
        width: 70%;
    }

    .dashboardgroup-div {
        float: left;
        width: 65%;
        height: 150px;
        overflow: scroll;
    }

    .dashboardgroup-checklist {
        width: 70%;
    }

    .dashboardgroup-right {
        float: left;
        width: 30%;
        text-align: center;
    }

        .dashboardgroup-right .selectall {
            float: left;
            width: 100%;
            padding-top: 60px;
        }

            .dashboardgroup-right .selectall input {
                width: 90px;
            }

        .dashboardgroup-right .clearall {
            float: left;
            width: 100%;
        }

            .dashboardgroup-right .clearall input {
                width: 90px;
            }

        .dashboardgroup-right .edit {
            float: left;
        }

            .dashboardgroup-right .edit img {
                cursor: pointer;
            }

    .showsidebar-right {
        float: left;
        width: 30%;
        text-align: center;
    }

        .showsidebar-right .selectall {
            float: left;
            width: 100%;
            padding-top: 60px;
        }

            .showsidebar-right .selectall input {
                width: 90px;
            }

        .showsidebar-right .clearall {
            float: left;
            width: 100%;
        }

            .showsidebar-right .clearall input {
                width: 90px;
            }

    .label {
        font-weight: bold;
    }

    .dimension-table {
        border-collapse: collapse;
        width: 100%;
    }

        .dimension-table .edit-td {
            width: 49%;
        }

        .dimension-table .list-td {
            width: 49%;
            padding-left: 10px;
        }

        .dimension-table fieldset {
            float: left;
            width: 98%;
        }

            .dimension-table fieldset fieldset {
                float: left;
                width: 95%;
            }

        .dimension-table .contentdiv {
            float: inherit;
            width: 92%;
            padding-top: 2px;
        }

        .dimension-table .content-head {
            float: inherit;
            font-weight: bold;
        }

        .dimension-table .content-edit {
            float: inherit;
        }

        .dimension-table .actiondiv {
            float: inherit;
        }

        .dimension-table .content-action {
            float: inherit;
        }

        .dimension-table .inputelement {
            width: 200px;
        }

        .dimension-table .operatorinput {
            width: 80px;
        }

    .dimension-list {
        float: left;
        width: 100%;
    }

        .dimension-list .dimension-content {
            float: left;
            width: inherit;
        }

        .dimension-list .dimension-head {
            float: left;
            width: 100%;
        }

        .dimension-list .dimension-detail {
            float: left;
            width: 100%;
        }

        .dimension-list .dimension-action {
            float: right;
        }

            .dimension-list .dimension-action span {
                float: right;
            }

    .expression-table {
        border-collapse: collapse;
        width: 100%;
    }

        .expression-table .edit-td {
            width: 49%;
        }

        .expression-table .list-td {
            width: 49%;
            padding-left: 10px;
        }

        .expression-table fieldset {
            float: left;
            width: 98%;
        }

            .expression-table fieldset fieldset {
                float: left;
                width: 95%;
            }

        .expression-table .contentdiv {
            float: inherit;
            width: 92%;
            padding-top: 2px;
        }

        .expression-table .content-head {
            float: inherit;
            font-weight: bold;
        }

        .expression-table .content-edit {
            float: inherit;
        }

        .expression-table .actiondiv {
            float: inherit;
        }

        .expression-table .content-action {
            float: inherit;
        }

        .expression-table .inputelement {
            width: 200px;
        }

        .expression-table .operatorinput {
            width: 80px;
        }

    .expression-list {
        float: left;
        width: 100%;
    }

        .expression-list .expression-content {
            float: left;
            width: inherit;
        }

        .expression-list .expression-head {
            float: left;
            width: 100%;
        }

        .expression-list .expression-detail {
            float: left;
            width: 100%;
        }

        .expression-list .expression-action {
            float: right;
        }

            .expression-list .expression-action span {
                float: right;
            }

    .hide {
        display: none;
    }

    .cachechart-container {
    }

        .cachechart-container .contentdiv {
            float: inherit;
            width: 92%;
            padding-top: 2px;
        }

        .cachechart-container .content-head {
            float: inherit;
            font-weight: bold;
        }

        .cachechart-container .content-edit {
            float: inherit;
        }

        .cachechart-container .actiondiv {
            float: inherit;
            padding-top: 5px;
        }

        .cachechart-container .content-action {
            float: inherit;
        }

    .auto-style2 {
        font-weight: bold;
        padding-right: 10px;
        width: 124px;
    }

    .auto-style3 {
        width: 124px;
    }

    .auto-style4 {
        font-weight: bold;
        padding-right: 10px;
        width: 124px;
        height: 33px;
    }

    .auto-style5 {
        height: 33px;
    }

    .auto-style6 {
        font-weight: bold;
        padding-right: 10px;
        width: 124px;
        height: 34px;
    }

    .auto-style7 {
        height: 34px;
    }

    .container {
        width: 99%;
        float: left;
        padding: 5px 0px 0px 5px;
    }
   
</style>




<div id="dvView" runat="server">
    <table class="tblView">
        <tr style="vertical-align:top">
            <td>
                <span style="float: left; text-align: right; font-weight: bold;">View Name<span style="color: red">*</span></span>
            </td><td>:</td>

            <td style="padding:0px 2px;">
                <asp:TextBox ID="txtViewname" runat="server" Height="20px" Width="200px"></asp:TextBox>
            </td>
            <td>
                 <span style="float: left; text-align: right; font-weight: bold;">Authorized To View</span>
            </td><td>:</td>
            <td  style="padding:0px 2px;">
                <ugit:UserValueBox ID="peAuthorizedToView"  Height="20px" runat="server" isMulti="true" />
           
            </td>
            <td>
                <span style="float: left; text-align: right; font-weight: bold; padding-left: 20px;">Width</span>
            </td><td>:</td>

            <td  style="padding:0px 2px;">
                <asp:TextBox ID="txtViewWidth" runat="server" Text="100"  Height="20px" Width="50px"></asp:TextBox><span style="font-weight: bold;">px</span>

            </td>

            <td  style="padding:0px 2px;">
               
                        <dx:ASPxButton ID="btnSaveView" runat="server" Text="Save" ToolTip="Link" CssClass="primary-blueBtn" OnClick="BtnSaveView_Click">
                 <Image Url="/content/images/save.png" Height="12px"></Image>
                        </dx:ASPxButton>

                   

            </td>

        </tr>
    </table>

</div>

<div style="min-height: 300px" class="containermain">
    <div id="dvDashboard" runat="server" class="dvDashboard">
        <div class="container">
            <table style="width: 100%; height: 100%; border-collapse: collapse;" cellspacing="0" cellpadding="0">
                <tr id="trMainPanel" runat="server">
                    <td valign="top" style="width: 300px;"><%--border-right:2px solid gray;--%>
                        <div style="width: 300px; float: left; border-color: blue;">


                            <asp:HiddenField ID="hdnviewid" runat="server" />
                            <asp:TreeView ID="treeViewGroup" runat="server" ShowLines="true" OnSelectedNodeChanged="TreeViewGroup_SelectedNodeChanged" SelectedNodeStyle-BackColor="Turquoise">
                            </asp:TreeView>

                            </>
                        </div>
                    </td>
                    <td valign="top" align="left">


                        <div style="width: 700px; float: left; border-color: blue;">

                            <div>

                                <asp:Panel ID="pnlsideView" runat="server" Visible="false" GroupingText="Add Link">
                                    <table style="width: 100%">

                                        <tr>
                                            <td class="servicecategory">Title <span style="color: red">*</span></td>
                                            <td style="padding-bottom: 5px;">
                                                <asp:TextBox ID="txtTitle" runat="server" Width="300px" Height="20px"></asp:TextBox><asp:CheckBox ID="chkHideTitle" runat="server" Text="Hide Title" TextAlign="Right" />
                                                <asp:RequiredFieldValidator ID="ReqValtxtTitle" runat="server" ErrorMessage="Title is Empty" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtTitle" ValidationGroup="LinkView"></asp:RequiredFieldValidator>
                                                <asp:CustomValidator ID="CustValtxtTitle" runat="server" ControlToValidate="txtTitle" Display="Dynamic" OnServerValidate="CustValtxtTitle_ServerValidate" ValidationGroup="LinkView" ErrorMessage="Title Already Exists in View" SetFocusOnError="true"></asp:CustomValidator>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td class="servicecategory">Description</td>
                                            <td style="padding-bottom: 5px;">
                                                <asp:TextBox ID="txtDescription" runat="server" Width="300px" TextMode="MultiLine"></asp:TextBox>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td class="servicecategory">Link Url <span style="color: red">*</span></td>
                                            <td class="auto-style5">
                                                <asp:TextBox ID="txtUrl" runat="server" Width="300px" Height="20px"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="ReqValtxtUrl" runat="server" ValidationGroup="LinkView" SetFocusOnError="true" Display="Dynamic" ControlToValidate="txtUrl" ErrorMessage="Link Url is Empty" ForeColor="Red"></asp:RequiredFieldValidator>

                                            </td>
                                        </tr>

                                        <tr runat="server" id="trLinkNavigationType">
                                            <td class="servicecategory">Navigation Type</td>
                                            <td>


                                                <asp:RadioButtonList ID="rbLinkNavigationType" runat="server" RepeatColumns="3" RepeatLayout="Table" CssClass="messagetypes">
                                                    <asp:ListItem Text="<label>Navigate</label>" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="<label>Popup</label>" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="<label>New Window</label>" Value="2"></asp:ListItem>
                                                </asp:RadioButtonList>

                                            </td>
                                        </tr>

                                        <tr>
                                            <td class="servicecategory">Image </td>
                                            <td style="padding-bottom: 5px;" class="auto-style7">
                                                <asp:FileUpload ID="fileupload" runat="server" Width="205px" CssClass="hide" Height="25px" />
                                                <br />
                                                <ugit:UGITFileUploadManager ID="txtfileupload" runat="server" hideWiki="true"/>
                                               
                                            </td>

                                        </tr>

                                        <tr>
                                            <td class="servicecategory">Order </td>
                                            <td style="padding-bottom: 5px;">
                                                <asp:TextBox ID="txtLinkOrder" runat="server" Width="300px" Height="20px"></asp:TextBox>
                                            </td>


                                        </tr>
                                        <tr>
                                            <td class="servicecategory">Height <span style="color: red">*</span></td>
                                            <td>
                                                <asp:TextBox ID="txtLinkHeight" runat="server" Width="300px" Height="20px"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="ReqtxtLinkHeight" runat="server" ControlToValidate="txtLinkHeight" Display="Dynamic" ErrorMessage="Height is Empty" ForeColor="Red" SetFocusOnError="true" ValidationGroup="LinkView"></asp:RequiredFieldValidator>
                                                <asp:RangeValidator ID="RangeValtxtLinkHeight" runat="server" Type="Integer" Display="Dynamic" ErrorMessage="Enter value between(1-2000)" ValidationGroup="LinkView" ControlToValidate="txtLinkHeight" MinimumValue="1" MaximumValue="2000"></asp:RangeValidator>
                                            </td>
                                        </tr>


                                        <tr>

                                            <td class="servicecategory"></td>
                                            <td style="padding-left: 174px;">

                                                <dx:ASPxButton ID="BtnSaveLink" runat="server" Text="&nbsp;&nbsp;Save&nbsp;&nbsp;" CssClass="primary-blueBtn" ValidationGroup="LinkView" ToolTip="Save Link" OnClick="BtnSaveLink_Click">
                        
                                <Image Url="/content/images/save.png"></Image>
                         
                                                </dx:ASPxButton>

                                                <dx:ASPxButton ID="BtnDeleteLink" runat="server" Text="&nbsp;&nbsp;Delete &nbsp;&nbsp;" CssClass="primary-blueBtn" ToolTip="Delete Link"  OnClick="BtnDeleteLink_Click">
                                <Image Url="/content/Images/delete-icon.png" ></Image>
                                                    <ClientSideEvents Click="function(s,e){return confirm('Are you sure you want to delete Link?')}" /> 
                                                </dx:ASPxButton>
                                            </td>
                                        </tr>

                                    </table>
                                </asp:Panel>

                                <asp:Panel ID="pnlgroup" runat="server" Visible="false" GroupingText="Add Group View">
                                    <table>

                                        <tr>
                                            <td class="servicecategory">Group  Name <span style="color: red">*</span></td>
                                            <td>
                                                <asp:TextBox ID="txtGroupname" runat="server" Width="200px" Height="20px"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="Reqvalname" runat="server" ErrorMessage="Group Name is Empty" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtGroupname" ValidationGroup="GroupView"></asp:RequiredFieldValidator>
                                                <asp:CustomValidator ID="CustValGroupname" runat="server" Display="Dynamic" OnServerValidate="CustValGroupname_ServerValidate" ControlToValidate="txtGroupname" ValidationGroup="GroupView" ErrorMessage="Group name Already Exists in View" SetFocusOnError="true"></asp:CustomValidator>

                                            </td>
                                        </tr>

                                        <tr>
                                            <td class="servicecategory">Item Order </td>
                                            <td>
                                                <asp:TextBox ID="txtItemorder" runat="server" Width="200px" Height="20px"></asp:TextBox>

                                            </td>
                                        </tr>

                                        <tr>
                                            <td class="servicecategory">Select Dashboards<span style="color: red">*</span></td>
                                            <td>
                                                <div class="scroll">
                                                    <asp:CheckBoxList ID="chkdashboard" runat="server" TextAlign="Right" RepeatDirection="Vertical" RepeatLayout="Flow"></asp:CheckBoxList>
                                                </div>

                                            </td>
                                        </tr>

                                        <tr>

                                            <td colspan="2">
                                                <asp:Label ID="lblchklstmsg" runat="server" ForeColor="Red"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td style="padding-left: 174px;">

                                               <dx:ASPxButton ID="btnAddGroup" runat="server" ValidationGroup="GroupView" CssClass="primary-blueBtn" Text="&nbsp;&nbsp;Save&nbsp;&nbsp;" ToolTip="Link" OnClick="BtnAddGroup_Click" ImagePosition="Right">
                       
                                <Image Url="/content/images/save.png"></Image> 
                                                </dx:ASPxButton>

                                                <dx:ASPxButton  ID="BtnDeleteGroup" runat="server" Text="&nbsp;&nbsp;Delete Dashboard&nbsp;&nbsp;" CssClass="primary-blueBtn" ToolTip="Link" OnClick="BtnDeleteGroup_Click" ImagePosition="Right">
                        
                                <Image Url="/content/Images/delete-icon.png" ></Image>
                                                    <ClientSideEvents Click="function(s,e){return confirm('Are you sure you want to delete Group?');}"  /> 
                                               </dx:ASPxButton>
                                            </td>
                                        </tr>

                                    </table>
                                </asp:Panel>

                                <asp:Panel ID="pnlDashboard" runat="server" Visible="false" GroupingText="DashBoard Property">
                                    <table>
                                        <tr id="trSelectDashBoard" runat="server">
                                            <td class="servicecategory">Select Dashboards<span style="color: red">*</span></td>
                                            <td>
                                                <asp:DropDownList ID="ddlDashBoardList" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlDashBoardList_SelectedIndexChanged"></asp:DropDownList>
                                                <asp:CustomValidator ID="CustomValidatorDashBoard" runat="server" Display="Dynamic" OnServerValidate="CustomValidatorDashBoard_ServerValidate" ControlToValidate="ddlDashBoardList" ValidationGroup="DashboardView" ErrorMessage="DashBoard already exists in view." SetFocusOnError="true"></asp:CustomValidator>
                                            </td>
                                        </tr>


                                        <tr>
                                            <td class="servicecategory">Display Title <span style="color: red">*</span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDisplayName" runat="server" Width="300px"> </asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RqValtrtxtDisplayName" runat="server" SetFocusOnError="true" ErrorMessage="Please enter display name." ForeColor="Red" ValidationGroup="DashboardView" ControlToValidate="txtDisplayName">
                                                </asp:RequiredFieldValidator>
                                            </td>
                                        </tr>

                                        <tr id="trDBBackGround" runat="server">
                                            <td class="servicecategory">Background </td>
                                            <td style="padding-bottom: 5px;" class="auto-style7">
                                                <asp:DropDownList ID="ddlDBBackGround" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlDBBackGround_SelectedIndexChanged">
                                                    <asp:ListItem Text="Default" Value="0" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="Custom" Value="1"></asp:ListItem>
                                                    <%--    <asp:ListItem Text="Icon only" Value="2"></asp:ListItem>
                            <asp:ListItem Text="Background & Icon" Value="3"></asp:ListItem>--%>
                                                </asp:DropDownList>
                                            </td>

                                        </tr>

                                        <tr id="trDBBackgroundImage" runat="server" visible="false">
                                            <td class="servicecategory">Background Image</td>
                                            <td style="padding-bottom: 5px;" class="auto-style7">
                                                <asp:FileUpload ID="fuDBBackGround" runat="server" Width="205px" CssClass="hide" Height="25px" />
                                                <br />
                                                <ugit:UGITFileUploadManager ID="txtDBBackGround" runat="server" hideWiki="true" />
                                               
                                            </td>

                                        </tr>

                                        <tr id="trDBIcon" runat="server" visible="true">
                                            <td class="servicecategory">Icon</td>
                                            <td>
                                                <asp:FileUpload ID="fuIcon" runat="server" CssClass="hide" />
                                                <br />
                                                <ugit:UGITFileUploadManager ID="txtFuIcon" runat="server"  hideWiki="true"/>
                                                
                                            </td>
                                        </tr>

                                        <tr>
                                            <td class="servicecategory">Height <span style="color: red">*</span></td>
                                            <td>
                                                <asp:TextBox ID="txtheight" runat="server" Width="140px"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="reqvalheight" runat="server" SetFocusOnError="true" ForeColor="Red" ErrorMessage="Height is empty." ValidationGroup="DashboardView" ControlToValidate="txtheight"></asp:RequiredFieldValidator>
                                                <asp:RangeValidator ID="RanValheight" runat="server" Type="Integer" ErrorMessage="Enter value between(1-2000)" ValidationGroup="DashboardView" ControlToValidate="txtheight" MinimumValue="1" MaximumValue="2000"></asp:RangeValidator>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td class="servicecategory">Item Order </td>
                                            <td>
                                                <asp:TextBox ID="txtorder" runat="server" Width="140px"></asp:TextBox>
                                            </td>
                                        </tr>

                                        <tr runat="server" id="trDashBoardUrl">
                                            <td class="servicecategory">Url</td>
                                            <td>
                                                <asp:TextBox ID="txtDashboardUrl" runat="server" Width="300px"></asp:TextBox>

                                            </td>
                                        </tr>

                                        <tr runat="server" id="trNavigation">
                                            <td class="servicecategory">Navigation Type</td>
                                            <td>


                                                <asp:RadioButtonList ID="rbNavigationList" runat="server" RepeatColumns="3" RepeatLayout="Table" CssClass="messagetypes">
                                                    <asp:ListItem Text="<label>Navigate</label>" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="<label>Popup</label>" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="<label>New Window</label>" Value="2"></asp:ListItem>
                                                </asp:RadioButtonList>

                                            </td>
                                        </tr>

                                        <tr>
                                            <td></td>
                                            <td style="padding-left: 26px;">
                                                <dx:ASPxButton ID="BtnEditProperty" runat="server" Text="&nbsp;&nbsp;Save&nbsp;&nbsp;" ToolTip="Save" 
                                                    ValidationGroup="DashboardView" CssClass="primary-blueBtn" OnClick="BtnEditProperty_Click">
                                                    <Image Url="/content/images/save.png"></Image>
                                                </dx:ASPxButton>

                                                <%--Edit Dashboard button--%>

                                               <dx:ASPxButton ID="LinkButton1" runat="server" Text="Edit Dashboard" ToolTip="Edit Dashboard" CssClass="primary-blueBtn" ValidationGroup="DashboardView" ImagePosition="Right" >
                                                 <ClientSideEvents Click="function(s,e){return editDashboard(s);}" />
                                                <Image Url="/content/images/edit-icon.png"></Image>
                                                </dx:ASPxButton>

                                               <dx:ASPxButton ID="btnSaveDashBoard" runat="server" Text="&nbsp;&nbsp;Save&nbsp;&nbsp;" CssClass="primary-blueBtn" ValidationGroup="DashboardView" ToolTip="Save" 
                                                   OnClick="btnSaveDashBoard_Click" ImagePosition="Right" >                   
                                                    <Image Url="/content/mages/save.png"></Image>                
                                                </dx:ASPxButton>

                                                <dx:ASPxButton ID="BtnDelelteDB" runat="server" Text="Delete Dashboard" ToolTip="Delete" CssClass="primary-blueBtn" OnClick="BtnDelelteDB_Click" ImagePosition="Right" >                
                                                    <Image Url="/content/images/delete-icon.png"></Image>
                                                    <ClientSideEvents Click="function(s,e){return confirm('Are you sure you want to delete Dashboard?');}" />
                                                </dx:ASPxButton>
                                            </td>
                                        </tr>

                                    </table>
                                </asp:Panel>

                            </div>


                        </div>
                    </td>
                </tr>
                <tr id="trMessage" runat="server">
                    <td colspan="2">
                        <div id="divMessage" runat="server" style="padding-top: 125px; padding-bottom: 100px; text-align: center; color: red; font-size: 14px">
                        </div>
                    </td>
                </tr>



            </table>


        </div>

    </div>

</div>

<div style="padding-top: 15px; float: left">

    <dx:ASPxButton ID="btnDeleteView" runat="server" Text="Delete" ToolTip="Delete" CssClass="primary-blueBtn" Visible="False" OnClick="btnDeleteView_Click" ImagePosition="Right">
          <Image Url="/content/Images/Cancel.png"  Height="14px" ></Image>
        <ClientSideEvents Click="function(s,e){return confirm('Are you sure you want to delete View?');}" />
    </dx:ASPxButton>
</div>
<div style="padding-top: 15px; float: right; display: inline;">
    <dx:ASPxButton ID="BtnNewLink" runat="server" Text="Link" ToolTip="New Link" CssClass="primary-blueBtn" OnClick="BtnNewLink_Click">
            <Image Url="/content/Images/add_icon.png"></Image> 
   </dx:ASPxButton>

    <dx:ASPxButton ID="btnDashBoard" runat="server" Text="Dashboard" ToolTip="New DashBoard" CssClass="primary-blueBtn" OnClick="btnDashBoard_Click">
          <Image Url="/content/images/add_icon.png"></Image>
   </dx:ASPxButton>

    <dx:ASPxButton  ID="btnAddnewGroup" runat="server" Text="Group" ToolTip="New Group" CssClass="primary-blueBtn" OnClick="BtnAddnewGroup_Click" ImagePosition="Right">                      
           <Image Url="/content/images/add_icon.png"></Image>
   </dx:ASPxButton>
    
    <dx:ASPxButton ID="btnClose" runat="server" Text="Close" ToolTip="Close Window" CssClass="primary-blueBtn" OnClick="btnClose_Click" ImagePosition="Right">
          <Image Url="/content/buttonimages/Cancel.png" Height="14px"></Image>                         
   </dx:ASPxButton>

</div>


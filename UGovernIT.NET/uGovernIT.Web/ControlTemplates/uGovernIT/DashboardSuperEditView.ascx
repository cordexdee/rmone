<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DashboardSuperEditView.ascx.cs" Inherits="uGovernIT.Web.DashboardSuperEditView" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">

    var url = '<%=dashboardUrl%>';

    function selectTab(tabPrefix, panelPrefix, index, tabCount) {
        var selectedContainer = null;
        for (var i = 1; i <= tabCount; i++) {
            var tabControl = document.getElementById(tabPrefix + i);
            var panelControl = document.getElementById(panelPrefix + i);
            if (tabControl && panelControl) {
                if (i == index) {

                    tabControl.className = "tabspan ugitsellinkbg ugitsellinkborder";
                    panelControl.style.display = "block";
                    document.getElementById("<%=hTabNumber.ClientID %>").value = i;
                    selectedContainer = panelControl;
                }
                else {
                    tabControl.className = "tabspan ugitlinkbg";
                    panelControl.style.display = "none";
                }
            }
        }
        RefreshIFrame(selectedContainer);
    }

    $(function () {
        <% if (hTabNumber.Value != string.Empty)
    {%>
        var tabNumber = parseInt("<%= hTabNumber.Value %>");
        selectTab('tab_', 'tabPanel_', tabNumber, 2)
        <%}%>

    });

    function fuIconValidateFileUpload(Source, args) {
        var fuData = document.getElementById('<%= fuIcon.ClientID %>');
        var FileUploadPath = fuData.value;

        if (FileUploadPath == '') {
            // There is no file selected
            args.IsValid = true;
        }
        else {
            var Extension = FileUploadPath.substring(FileUploadPath.lastIndexOf('.') + 1).toLowerCase();

            if (Extension == "jpg" || Extension == "jpeg" || Extension == "png" || Extension == "gif") {
                args.IsValid = true; // Valid file type
            }
            else {
                args.IsValid = false; // Not valid file type
                $(Source).html("Please upload image file(png, jpg, gif) only.");
            }
        }
    }

    function editDashboard() {
        var title = '<%=dashboardname%>'
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
        $('#<%=txtFuIcon.ClientID%>').val(siteAsset);
    }
    function ddlFactTableFieldsForFilter_change(obj) {

        var table = $(".ddlFilterFactTable").val();
        var field = $(".ddlFactTableFieldsForFilter").val();
        glDefaultValue.gridView.PerformCallback(table + "~" + field);
    }
</script>
<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
    .hide {
    }

    .servicecategory {
        font-weight: bold;
        padding-right: 10px;
    }

    .dashboard-scroll {
        height: 200px;
        overflow: auto;
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
        background: #E1E1E1;
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

    .container {
        float: left;
        width: 99%;
        padding: 5px 0px 0px 5px;
    }
</style>

<div class="tabmaindiv">
    <div class="tabmaindivinner">
        <span id='tab_1' class="tabspan ugitlinkbg"
            onclick="selectTab('tab_','tabPanel_', 1 ,2)">
            <a href="javascript:void(0);">Select Dashboard</a>
        </span>
        <span id='tab_2' class="tabspan ugitlinkbg"
            onclick="selectTab('tab_','tabPanel_', 2 ,2)">
            <a href="javascript:void(0);">Global Filter</a>
        </span>
    </div>
</div>


<div style="" class="containermain" id="tabPanel_1">
    <table style="">
        <tr>
            <td style="padding: 4px 0px 4px 0px;">View Name</td>
            <td>:<span style="color: red">*</span></td>
            <td style="padding: 4px 0px 4px 0px;">
                <asp:TextBox ID="txtViewname" runat="server" Width="250px"></asp:TextBox></td>
            <td>
                <asp:ImageButton runat="server" ID="ImgDeleteView" ImageUrl="/Content/images/delete-icon.png" OnClick="ImgDeleteView_Click" OnClientClick="return confirm('Are you sure you want to delete View?');" Visible="False" /></td>
            <td>
                <asp:CustomValidator ID="CustSuperver" runat="server" ControlToValidate="txtViewname" SetFocusOnError="true" ErrorMessage="View Name Already Exists In List" OnServerValidate="CustSuperver_ServerValidate" ValidationGroup="GroupView"></asp:CustomValidator>
                <asp:RequiredFieldValidator ID="ReqValViewname" runat="server" ErrorMessage="View Name is Empty" SetFocusOnError="true" ControlToValidate="txtViewname" ValidationGroup="GroupView"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td style="padding: 4px 0px 4px 0px;">Authorized To View</td>
            <td>:</td>
            <td style="padding: 4px 0px 4px 0px;">
                <ugit:UserValueBox ID="peAuthorizedToView" runat="server" isMulti="true" Width="250px" />
            </td>
            <td></td>
            <td></td>
        </tr>
    </table>
    <div class="servicecategory container">
        <div>
        </div>
    </div>
    <div class="container">
        <span style="float: left; font-weight: bold">&nbsp;
        </span>
        <span style="float: left;"></span>
    </div>
    <div class="container">
        <div style="width: 350px; float: left; border-color: blue;">
            <div>
                <asp:HiddenField ID="hTabNumber" runat="server" />
                <asp:HiddenField ID="hdnviewid" runat="server" />
                <asp:TreeView ID="treeViewGroup" runat="server" OnSelectedNodeChanged="TreeViewGroup_SelectedNodeChanged" ShowLines="true" SelectedNodeStyle-BackColor="Turquoise">
                </asp:TreeView>

            </div>
        </div>
        <div style="width: 600px; float: right; border-color: blue;">
            <asp:Panel ID="pnlgroup" runat="server" GroupingText="Add Group View">
                <table style="width: 100%; border-collapse: collapse;" cellspacing="0" cellpadding="0">
                    <tr>
                        <td class="servicecategory">Group  Name<span style="color: red">*</span></td>
                        <td>
                            <asp:TextBox ID="txtname" runat="server" Width="200px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="Reqvalname" runat="server" ErrorMessage="Group Name is Empty" SetFocusOnError="true" ControlToValidate="txtname" ValidationGroup="GroupView"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="CustValGroupname" runat="server" ControlToValidate="txtname" Display="Static" OnServerValidate="CustValGroupname_ServerValidate" ValidationGroup="GroupView" ErrorMessage="Group name Already Exists in View" SetFocusOnError="true"></asp:CustomValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="servicecategory">Item Order</td>
                        <td>
                            <asp:TextBox ID="txtItemorder" runat="server" Width="200px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="servicecategory">Width</td>
                        <td>
                            <asp:TextBox ID="txtwidth" runat="server" Width="200px"></asp:TextBox>
                            <asp:RangeValidator ID="RanValwidth" runat="server" Type="Integer" ErrorMessage="Enter value between(1-1000)" ValidationGroup="GroupView" ControlToValidate="txtwidth" MinimumValue="1" MaximumValue="2000"></asp:RangeValidator>

                        </td>
                    </tr>
                    <tr>
                        <td class="servicecategory">Select Dashboard<span style="color: red">*</span></td>
                        <td>
                            <div class="dashboard-scroll">
                                <asp:CheckBoxList ID="chkdashboard" runat="server" TextAlign="Right" RepeatDirection="Vertical" RepeatLayout="Flow"></asp:CheckBoxList>
                            </div>
                        </td>
                    </tr>

                    <tr>

                        <td colspan="2">&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lblchklstmsg" runat="server" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>
                            <dx:ASPxButton ID="btnAddGroup" runat="server" ValidationGroup="GroupView" CssClass="primary-blueBtn" Text="Save" OnClick="BtnAddGroup_Click"></dx:ASPxButton>
                            <dx:ASPxButton ID="BtnDeleteGroup" runat="server" OnClick="BtnDeleteGroup_Click" CssClass="primary-blueBtn" Text="Delete Group">
                                <ClientSideEvents Click="function(s,e){return confirm('Are you sure you want to delete Group?');}" />
                            </dx:ASPxButton>
                            <dx:ASPxButton ID="btnAddnewGroup" runat="server" Text="Add New Group" CssClass="primary-blueBtn" OnClick="BtnNewGroup_Click"></dx:ASPxButton>
                        </td>
                    </tr>

                </table>
            </asp:Panel>
            <asp:Panel ID="pnlDashboard" runat="server" GroupingText="DashBoard Property" Visible="false">
                <table>
                    <tr>
                        <td class="servicecategory">Use Default</td>
                        <td>
                            <asp:CheckBox ID="chkinherit" AutoPostBack="false" runat="server" Checked="true" /></td>
                    </tr>

                    <tr>
                        <td class="servicecategory">Display Title: <span style="color: red">*</span>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDisplayName" runat="server" Width="140px"> </asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" SetFocusOnError="true" ErrorMessage="Please enter display name" ForeColor="Red" ValidationGroup="Inheritance" ControlToValidate="txtDisplayName">
                            </asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="servicecategory">Icon:</td>
                        <td>
                            <asp:FileUpload ID="fuIcon" runat="server" CssClass="hide" />
                            <br />
                            <ugit:UGITFileUploadManager ID="txtFuIcon" runat="server" hideWiki="true" />
                            <%--<asp:TextBox runat="server" ID="txtFuIcon" Width="300px" Height="20px"></asp:TextBox><br />
                            <asp:LinkButton ID="lnkbtnPickAssets" runat="server" Font-Size="10px" Text="PickFromAsset">Pick From Library</asp:LinkButton>
                            --%><asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="Please upload image file(png, jpg, gif) only." ControlToValidate="fuIcon" Display="Dynamic"
                                ClientValidationFunction="fuIconValidateFileUpload" ValidationGroup="LinkView" OnServerValidate="FuIcon_ServerValidate"></asp:CustomValidator>
                        </td>


                    </tr>
                    <tr>
                        <td class="servicecategory">Height<span style="color: red">*</span></td>
                        <td>
                            <asp:TextBox ID="txtheight" runat="server" Width="140px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="reqvalheight" runat="server" SetFocusOnError="true" ForeColor="Red" ValidationGroup="Inheritance" ControlToValidate="txtheight"></asp:RequiredFieldValidator>
                            <asp:RangeValidator ID="RanValheight" runat="server" Type="Integer" ErrorMessage="Enter value between(1-1000)" ValidationGroup="Inheritance" ControlToValidate="txtheight" MinimumValue="1" MaximumValue="2000"></asp:RangeValidator>
                        </td>

                    </tr>

                    <tr>
                        <td class="servicecategory">Item Order</td>
                        <td>
                            <asp:TextBox ID="txtorder" runat="server" Width="140px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>
                            <dx:ASPxButton ID="BtnEditProperty" runat="server" OnClick="BtnEditProperty_Click" Text="Save" CssClass="primary-blueBtn" ValidationGroup="Inheritance"></dx:ASPxButton>
                            <%--Edit dashboard config button--%>
                            <dx:ASPxButton ID="ButnEditDBProp" runat="server" CssClass="primary-blueBtn" Text="Edit Dashboard">
                                <ClientSideEvents Click="function(s,e){return editDashboard();}" />
                            </dx:ASPxButton>
                            <dx:ASPxButton ID="BtnDelelteDB" runat="server" CssClass="primary-blueBtn" OnClick="BtnDelelteDB_Click" Text="Remove Dashboard">
                                <ClientSideEvents Click="function(s,e){return confirm('Are you sure you want to remove Dashboard?');}" />
                            </dx:ASPxButton>
                            <dx:ASPxButton ID="BtnNewGroup" runat="server" CssClass="primary-blueBtn" OnClick="BtnNewGroup_Click" Text="Add New Group"></dx:ASPxButton>
                        </td>
                </table>
            </asp:Panel>
        </div>
    </div>
</div>

<div style="display: none;" class="containermain" id="tabPanel_2">
    <table style="width: 100%;">
        <tr>
            <td style="width: 200px; vertical-align: top;">
                <asp:TreeView ID="trDashboardFilter" runat="server" SelectedNodeStyle-BackColor="Yellow" OnSelectedNodeChanged="TrDashboardFilter_SelectedNodeChanged">
                </asp:TreeView>
            </td>
            <td>
                <asp:HiddenField ID="hfEditFilter" runat="server" />
                <asp:Panel ID="pEditFilter" runat="server">
                    <table>
                        <tr>
                            <td class="servicecategory">Title<b style="color: Red">*</b>:</td>
                            <td>
                                <asp:TextBox ID="txtFilterTitle" runat="server" CssClass="fullwidth"></asp:TextBox>
                                <asp:RequiredFieldValidator Display="Dynamic" ID="RequiredFieldValidator3" runat="server"
                                    ControlToValidate="txtFilterTitle" ValidationGroup="editGlobalFilter" ErrorMessage="Please enter title"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="servicecategory">Item Order:</td>
                            <td>
                                <asp:TextBox ID="txtFilterItemOrder" runat="server" Text="0"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="servicecategory">Fact Table<b style="color: Red">*</b>:</td>
                            <td>
                                <asp:DropDownList ValidationGroup="editGlobalFilter" CssClass="inputelement ddlFilterFactTable" AutoPostBack="true"
                                    OnSelectedIndexChanged="DdlFilterFactTable_SelectedIndexChanged" ID="ddlFilterFactTable"
                                    runat="server" OnLoad="DdlFilterFactTable_Load">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator Display="Dynamic" ID="RequiredFieldValidator1" runat="server"
                                    ControlToValidate="ddlFilterFactTable" ValidationGroup="editGlobalFilter" ErrorMessage="Please select fact table"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="servicecategory">Filter Field<b style="color: Red">*</b>:</td>
                            <td>
                                <asp:DropDownList CssClass="inputelement ddlFactTableFieldsForFilter" EnableViewState="true" onchange="ddlFactTableFieldsForFilter_change(this)" ID="ddlFactTableFieldsForFilter"
                                    runat="server">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator Display="Dynamic" ID="RequiredFieldValidator2" runat="server"
                                    ControlToValidate="ddlFactTableFieldsForFilter" ValidationGroup="editGlobalFilter" ErrorMessage="Please Select Field"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="servicecategory">Default Value:</td>
                            <td>
                                <dx:ASPxGridLookup SelectionMode="Multiple" OnDataBinding="glDefaultValue_DataBinding" KeyFieldName="Value" ID="glDefaultValue" ClientInstanceName="glDefaultValue" runat="server">
                                    <Columns>
                                        <dx:GridViewCommandColumn ShowSelectCheckbox="true"></dx:GridViewCommandColumn>
                                        <dx:GridViewDataTextColumn FieldName="Value" Caption="Value"></dx:GridViewDataTextColumn>
                                    </Columns>
                                </dx:ASPxGridLookup>
                            </td>
                        </tr>
                        <tr>
                            <td class="servicecategory">Hidden:</td>
                            <td>
                                <dx:ASPxCheckBox ID="chkHidden" runat="server"></dx:ASPxCheckBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">

                                <dx:ASPxButton ID="btAddNewFiler" OnClick="BtAddNewFiler_Click" CssClass="primary-blueBtn" runat="server" Text="Add New Filter" Visible="false">
                                </dx:ASPxButton>
                                <dx:ASPxButton ID="btDeleteFilter" ValidationGroup="editGlobalFilter" CssClass="primary-blueBtn" OnClick="BtDeleteFilter_Click" runat="server" Text="Delete" Visible="false">
                                    <ClientSideEvents Click="function(s,e){return confirm('Are you sure you want to remove filter?');} " />
                                </dx:ASPxButton>
                                <dx:ASPxButton ID="btSaveFilter" ValidationGroup="editGlobalFilter" CssClass="primary-blueBtn" OnClick="BtSaveFilter_Click" runat="server" Text="Save">
                                </dx:ASPxButton>
                                <dx:ASPxButton ID="btCancelFilter" Text="Cancel" runat="server" CssClass="primary-blueBtn" OnClick="BtCancelFilter_Click">
                                </dx:ASPxButton>

                            </td>
                        </tr>
                    </table>

                </asp:Panel>
            </td>
        </tr>
    </table>
</div>

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DashboardIndivisibleEditView.ascx.cs" Inherits="uGovernIT.Web.DashboardIndivisibleEditView" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
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
        selectTab('tab_', 'tabPanel_', tabNumber, 3)
        <%}%>

    });

    function editDashboard() {
        
        var title = '<%=dashboardname%>';
        var dashboardID = '<%=dashboardId%>';
        var dashboardType = $.trim('<%=dashboardType%>');
        var params = "";

        if (dashboardType.toLowerCase() == "panel") {
            params = "control=configdashboardpanel&DashboardID=" + dashboardID;
        }
        else if (dashboardType.toLowerCase() == "chart") {

            params = "control=configdashboardchart&DashboardID=" + dashboardID;
        }

        window.parent.UgitOpenPopupDialog(url, params, title, 90, 80, false, escape(window.location.href));


        return false;
    }

</script>
<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
    .hide {
    }
</style>

<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
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

    .servicecategory {
        font-weight: bold;
        padding-right: 10px;
    }

    .fullwidth {
        width: 98%;
    }

    .container {
        float: left;
        width: 99%;
        padding: 5px 0px 0px 5px;
    }
</style>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function ValidateFileUpload(Source, args) {
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
    function pickSiteAsset(Url) {
        var siteAsset = unescape(Url);
        $('#<%=txtfileupload.ClientID%>').val(siteAsset);
    }

    $(document).ready(function () {
        $('.userValueBox-Table').parent().addClass("userValueBox-searchFilterWrap");
        $('.userValueBox-searchFilterWrap').parent().addClass("userValueBox-searchFilterContainer");
        $('.userValueBox-searchFilterContainer').parents().eq(3).addClass('userValueBox-dropDownWrap');
    });

</script>
<div class="tabmaindiv adminTab-pannel">
    <div class="tabmaindivinner">
        <span id='tab_1' class="tabspan ugitsellinkbg ugitsellinkborder" onclick="selectTab('tab_','tabPanel_', 1 ,2)">
            <a href="javascript:void(0);">Select Dashboard</a> </span>
        <span id='tab_2' class="tabspan ugitlinkbg"
            onclick="selectTab('tab_','tabPanel_', 2 ,3)">
            <a href="javascript:void(0);">Edit</a>
        </span>

        <span id='tab_3' class="tabspan ugitlinkbg"
            onclick="selectTab('tab_','tabPanel_', 3 ,3)">
            <a href="javascript:void(0);">Global Filter</a>
        </span>
    </div>
</div>

<asp:HiddenField ID="hTabNumber" runat="server" />
<asp:HiddenField ID="hdnviewid" runat="server" />
<div class="containermain col-md-12 col-sm-12 col-xs-12 noPadding" id="tabPanel_1">
    <div class="row ms-formtable accomp-popup">
        <div class="col-md-4 col-sm-4 col-xs-6">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Enter View Name<span style="color: red">*</span></h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:TextBox ID="txtviewname" CssClass="asptextbox-asp" runat="server" Width="100%"></asp:TextBox>
                <asp:ImageButton runat="server" ID="ImgDeleteView" ImageUrl="/content/images/delete-icon-new.png" 
                    OnClick="ImgDeleteView_Click" OnClientClick="return confirm('Are you sure you want to delete View?');" 
                    Visible="False" Width="20px" />
                <asp:RequiredFieldValidator ID="ReqvaluveiwName" runat="server" Display="Static" ControlToValidate="txtviewname" 
                    ErrorMessage="View name is Empty"></asp:RequiredFieldValidator>
            </div>
        </div> 
        <div class="col-md-4 col-sm-4 col-xs-6">
             <div class="ms-formlabel">
                 <h3 class="ms-standardheader budget_fieldLabel">Authorized To View</h3>
             </div>
            <div class="ms-formbody accomp_inputField">
                <ugit:UserValueBox ID="peAuthorizedToView" CssClass="userValueBox-dropDown" runat="server" isMulti="true"  
                    Width="100%" />
            </div>
        </div>
    </div>
   <%-- <div class="container">
      
    </div>--%>

    <div class="row container">
        <ugit:ASPxGridView ID="grddashboars" KeyFieldName="Title" Settings-VerticalScrollableHeight="500" Width="100%"
            runat="server" CssClass="customgridview homeGrid" AutoGenerateColumns="false"
            OnHtmlRowPrepared="grddashboars_HtmlRowPrepared" >
            <Columns>
                <dx:GridViewDataColumn>
                    <DataItemTemplate>
                          <asp:CheckBox ID="ChkChecked" runat="server" />
                    </DataItemTemplate>
                </dx:GridViewDataColumn>
                <dx:GridViewDataColumn FieldName="DashboardType" Caption="PanelType" Name="PanelType">
                    <SettingsHeaderFilter Mode="CheckedList"></SettingsHeaderFilter>
                </dx:GridViewDataColumn>
               <dx:GridViewDataColumn FieldName="Title" Caption="Title" Name="Title">
                   <SettingsHeaderFilter Mode="CheckedList"></SettingsHeaderFilter>
               </dx:GridViewDataColumn>
            </Columns>
            <Settings ShowHeaderFilterButton="true" />
            <Styles>
                <Row CssClass="homeGrid_dataRow"></Row>
                <Header CssClass="homeGrid_headerColumn"></Header>
            </Styles>
            <SettingsPager Mode="ShowAllRecords"></SettingsPager>
        </ugit:ASPxGridView>
       
        <div style="height: 20px">
            <asp:Label ID="lblmsg" runat="server" BackColor="Red"></asp:Label>
        </div>
        <div class="row addEditPopup-btnWrap">
            <dx:ASPxButton ID="BtnAddView" runat="server" Text="Save" CssClass="primary-blueBtn" 
                OnClick="BtnAddView_Click"></dx:ASPxButton>
        </div>
    </div>
</div>

<div style="display: none;" class="containermain col-md-12 col-sm-12 col-xs-12" id="tabPanel_2">
    <div id="divhideshow" runat="server" style="float: left;">
        <div class="row">
            <div style="width: 300px">
                <asp:TreeView ID="tvSelectedDashboard" runat="server" ShowLines="true" OnSelectedNodeChanged="TVSelectedDashboard_SelectedNodeChanged" SelectedNodeStyle-BackColor="Turquoise">
                </asp:TreeView>
            </div>
        </div>
        <div class="row">
            <fieldset>
                <legend class="admin-legendLabel">Edit Dashboard</legend>
                    <div class="ms-formtable accomp-popup">
                        <div class="col-md-4 col-sm-4 col-xs-4">
                            <div class="ms-formbody accomp_inputField crm-checkWrap">
                                <asp:CheckBox ID="chkinherit" AutoPostBack="false" Text="Use Default" Checked="true" 
                                    runat="server" TextAlign="Right" />
                            </div>
                        </div>
                        <div class="col-md-4 col-sm-4 col-xs-4">
                            <div class="ms-formlabel">
                                <h3 class="ms-standardheader budget_fieldLabel">Icon</h3>
                            </div>
                            <div class="ms-formbody accomp_inputField">
                                <asp:FileUpload ID="fuIcon" runat="server" CssClass="hide" />
                                <asp:TextBox runat="server" ID="txtfileupload" Width="100%" CssClass="asptextbox-asp"></asp:TextBox><br />
                                <asp:LinkButton ID="lnkbackground" runat="server" Font-Size="12px" class="fileupload" Text="PickFromAsset">Pick From Library</asp:LinkButton>
                            </div>
                        </div>
                        <div class="col-md-4 col-sm-4 col-xs-4">
                            <div class="ms-formlabel">
                                <h3 class="ms-standardheader budget_fieldLabel">Display Title<span style="color: red">*</span></h3>
                            </div>
                            <div class="ms-formbody accomp_inputField">
                                <asp:TextBox ID="txtDisplayName" runat="server" Width="100%" CssClass="asptextbox-asp"> </asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" SetFocusOnError="true" ErrorMessage="Please enter display name" ForeColor="Red" ValidationGroup="Inheritance" ControlToValidate="txtDisplayName">
                                </asp:RequiredFieldValidator>
                            </div>
                         </div>
                        <div class="col-md-4 col-sm-4 col-xs-4" style="clear:both;">
                             <div class="ms-formlabel">
                                <h3 class="ms-standardheader budget_fieldLabel">Height: <span style="color: red">*</span></h3>
                             </div>
                             <div class="ms-formbody accomp_inputField">
                                 <asp:TextBox ID="txtheight" runat="server" Width="100%" CssClass="asptextbox-asp"></asp:TextBox>
                                 <asp:RequiredFieldValidator ID="reqvalheight" runat="server" SetFocusOnError="true" 
                                     ErrorMessage="Height is Empty" ForeColor="Red" ValidationGroup="Inheritance"
                                     ControlToValidate="txtheight">
                                </asp:RequiredFieldValidator>
                                <asp:RangeValidator ID="RanValheight" runat="server" Type="Integer" 
                                    ErrorMessage="Enter value between(1-1000)" ValidationGroup="Inheritance" 
                                    ControlToValidate="txtheight" MinimumValue="1" MaximumValue="2000">
                                </asp:RangeValidator>
                             </div>
                         </div>       
                        <div class="col-md-4 col-sm-4 col-xs-4">
                             <div class="ms-formlabel">
                                <h3 class="ms-standardheader budget_fieldLabel">Width: <span style="color: red">*</span></h3>
                             </div>
                             <div class="ms-formbody accomp_inputField">
                                 <asp:TextBox ID="txtwidth" runat="server" Width="100%" CssClass="asptextbox-asp" ReadOnly="false"></asp:TextBox>
                                 <asp:RequiredFieldValidator ID="ReqValwidth" runat="server" ValidationGroup="Inheritance" 
                                     SetFocusOnError="true" ControlToValidate="txtwidth" ErrorMessage="Width is Empty" 
                                     ForeColor="Red">
                                 </asp:RequiredFieldValidator>
                                 <asp:RangeValidator ID="RanValwidth" runat="server" Type="Integer" 
                                     ErrorMessage="Enter value between(1-1000)" ValidationGroup="Inheritance" 
                                     ControlToValidate="txtwidth" MinimumValue="1" MaximumValue="2000"></asp:RangeValidator>
                             </div>
                         </div>  
                        <div class="col-md-4 col-sm-4 col-xs-4">
                             <div class="ms-formlabel">
                                <h3 class="ms-standardheader budget_fieldLabel">Item Order <span style="color: red">*</span></h3>
                             </div>
                             <div class="ms-formbody accomp_inputField">
                                  <asp:TextBox ID="txtitemorder" runat="server" Width="100%" CssClass="asptextbox-asp" 
                                      ReadOnly="false"></asp:TextBox>
                                  <asp:RequiredFieldValidator ID="ReqValorder" runat="server" ValidationGroup="Inheritance" 
                                      SetFocusOnError="true" ControlToValidate="txtitemorder" ErrorMessage="Item Order is Empty" 
                                      ForeColor="Red">
                                  </asp:RequiredFieldValidator>
                             </div>
                         </div>
                        <div class="col-md-4 col-sm-4 col-xs-4">
                              <div class="ms-formbody accomp_inputField crm-checkWrap">
                                  <asp:CheckBox ID="cbStartFromNewLine" runat="server" Text="Start From New line" TextAlign="Right" />
                              </div>
                         </div>
                        <div class="col-md-4 col-sm-4 col-xs-4">
                            <div class="ms-formlabel">
                                <h3 class="ms-standardheader budget_fieldLabel">Border Type</h3>
                            </div>
                            <div class="ms-formbody accomp_inputField">
                                 <asp:DropDownList ID="ddlBorderType" runat="server" Width="100%" CssClass="itsmDropDownList aspxDropDownList">
                                    <asp:ListItem Value="None">None</asp:ListItem>
                                    <asp:ListItem Value="Rectangle">Rectangle</asp:ListItem>
                                    <asp:ListItem Value="RoundedRectangle">Rounded Rectangle</asp:ListItem>
                                </asp:DropDownList>
                             </div>
                        </div>
                        <div class="col-md-12 col-sm-12 col-xs-12">
                            <div class="row addEditPopup-btnWrap">
                                <dx:ASPxButton ID="ButnEditDBProp" runat="server" Text="Edit Dashboard" CssClass="primary-blueBtn" >
                                    <ClientSideEvents  Click="function(s,e){return editDashboard();}"/>
                                </dx:ASPxButton>
                                <dx:ASPxButton ID="BtnEditDBProp" runat="server" OnClick="BtnEditDBProp_Click" Text="Save" CssClass="primary-blueBtn" ValidationGroup="Inheritance" ></dx:ASPxButton>
                            </div>
                        </div>
                    </div>
            </fieldset>
        </div>
    </div>
</div>

<div style="display: none;" class="containermain col-md-12 col-sm-12 col-xs-12 noPadding" id="tabPanel_3">
    <div class="ms-formtable accomp-popup" style="width: 100%;">
        <div class="row">
            <div style="width: 200px; vertical-align: top;">
                <asp:TreeView ID="trDashboardFilter" runat="server" SelectedNodeStyle-BackColor="Yellow" 
                    OnSelectedNodeChanged="TrDashboardFilter_SelectedNodeChanged">
                </asp:TreeView>
            </div>
        </div>
        <div class="row">
            <asp:HiddenField ID="hfEditFilter" runat="server" />
            <asp:Panel ID="pEditFilter" runat="server">
                <div class="ms-formtable accomp-popup ">
                    <div class="col-md-4 col-sm-4 col-xs-4">
                        <div class="ms-formlabel">
                            <h3 class="ms-standardheader budget_fieldLabel">Title<b style="color: Red">*</b></h3>
                        </div>
                        <div class="ms-formbody accomp_inputField">
                            <asp:TextBox ID="txtFilterTitle" runat="server" Width="100%" CssClass="asptextbox-asp"></asp:TextBox>
                            <asp:RequiredFieldValidator Display="Dynamic" ID="RequiredFieldValidator3" runat="server"
                                ControlToValidate="txtFilterTitle" ValidationGroup="editGlobalFilter" 
                                ErrorMessage="Please enter title"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="col-md-4 col-sm-4 col-xs-4">
                        <div class="ms-formlabel">
                            <h3 class="ms-standardheader budget_fieldLabel">Item Order</h3>
                        </div>
                        <div class="ms-formbody accomp_inputField">
                            <asp:TextBox ID="txtFilterItemOrder" Width="100%" CssClass="asptextbox-asp" runat="server" Text="0"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-4 col-sm-4 col-xs-4">
                        <div class="ms-formlabel">
                            <h3 class="ms-standardheader budget_fieldLabel">Fact Table<b style="color: Red">*</b></h3>
                        </div>
                        <div class="ms-formbody accomp_inputField">
                            <asp:DropDownList ValidationGroup="editGlobalFilter" Width="100%" CssClass="inputelement  itsmDropDownList aspxDropDownList" 
                                AutoPostBack="true" OnSelectedIndexChanged="DdlFilterFactTable_SelectedIndexChanged" 
                                ID="ddlFilterFactTable" runat="server" OnLoad="DdlFilterFactTable_Load">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator Display="Dynamic" ID="RequiredFieldValidator1" runat="server"
                                    ControlToValidate="ddlFilterFactTable" ValidationGroup="editGlobalFilter" ErrorMessage="Please select fact table"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="servicecategory">Filter Field<b style="color: Red">*</b>:</td>
                            <td>
                                <asp:DropDownList CssClass="inputelement" EnableViewState="true" ID="ddlFactTableFieldsForFilter"
                                    runat="server">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator Display="Dynamic" ID="RequiredFieldValidator2" runat="server"
                                    ControlToValidate="ddlFactTableFieldsForFilter" ValidationGroup="editGlobalFilter" ErrorMessage="Please Select Field"></asp:RequiredFieldValidator>
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
                                 <dx:ASPxButton ID="btAddNewFiler" OnClick="BtAddNewFiler_Click" runat="server" Text="Add New Filter" CssClass="primary-blueBtn" Visible="false" ></dx:ASPxButton>
                               <dx:ASPxButton ID="btDeleteFilter" ValidationGroup="editGlobalFilter"  OnClick="BtDeleteFilter_Click" runat="server" Text="Delete" 
                                   Visible="false" CssClass="primary-blueBtn" >
                                   <ClientSideEvents  Click="function(s,e){return confirm('Are you sure you want to remove filter?');}"/>
                               </dx:ASPxButton>
                                <dx:ASPxButton ID="btSaveFilter" ValidationGroup="editGlobalFilter" OnClick="BtSaveFilter_Click" runat="server" Text="Save" CssClass="primary-blueBtn" >

                                </dx:ASPxButton>
                                <dx:ASPxButton ID="btCancelFilter" Text="Cancel" runat="server" CssClass="primary-blueBtn" OnClick="BtCancelFilter_Click" ></dx:ASPxButton>

                            </td>
                        </tr>
                    </table>

                </asp:Panel>
            </td>
        </tr>
    </table>
</div>







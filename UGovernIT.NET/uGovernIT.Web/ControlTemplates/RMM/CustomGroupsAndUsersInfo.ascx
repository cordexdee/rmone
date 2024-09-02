<%@ Control Language="C#" AutoEventWireup="true"  CodeBehind="CustomGroupsAndUsersInfo.ascx.cs" Inherits="uGovernIT.Web.CustomGroupsAndUsersInfo" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%--<%@ Register TagPrefix="ugit" Assembly="uGovernIT.Web" Namespace="uGovernIT.Web" %>--%>
<%@ Register Src="~/ControlTemplates/RMM/UserOrganizationChart.ascx" TagPrefix="uc1" TagName="UserOrganizationChart" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
    .containerdiv {
        float: left;
        width: 100%;
        padding-left: 5px;
        margin-right: 2px;
        padding-right: 3px;
    }

    .containerdiv-top {
        padding-top: 10px;
        margin-bottom: 0px;
    }

    .container-sub {
        float: left;
        margin-right: 5px;
    }

    /*.container-sub-padd {
        margin-right: 5px;
        margin-top: 2px;
        padding-left: 5px;
        font-weight: bold;
    }*/

    .ms-vh2 {
        font-weight: bold !important;
    }
    /* set border around selected page number*/
    .pagerBox td table tr td span {
        /* font-size : larger; */
        border: 1px solid black;
        padding: 0px 3px;
    }

    .addgroupicon {
        float: left;
        padding-top: 2px;
        margin-left: 5px;
        cursor: pointer;
    }

    /*.ExportClass {
        float: left;
        padding-top: 2px;
        padding-left: 4px;
    }*/

    .searchpanel {
        width: 470px;
    }

    /*.viewdropdown {
        padding-left: 184px;
    }*/

        .viewdropdown .dropdowncss {
            /*padding-left: 0px !important;*/
            width: 100px;
        }

    .dxflAGSys {
        width: 0px !important;
        padding-top: 0px;
    }

    .button-red {
        /*float: none;*/
        padding: 0px 5px 5px 5px;
    }

    [id$='ChangeNameProcessImg'] {
        margin-left: 3px !important;
        margin-right: -9px !important;
    }

    [id$='enableuserImg'] {
        margin-left: 3px !important;
        margin-right: -9px !important;
        margin-top: -3px !important;
    }

    [id$='disableuserImg'] {
        margin-left: 3px !important;
        margin-right: -9px !important;
        margin-top: -3px !important;
    }

    .ms-formbody {
        background: none repeat scroll 0 0 #E8EDED;
        border-top: 1px solid #A5A5A5;
        padding: 3px 6px 4px;
        vertical-align: top;
    }

    /*.dxbButton_UGITNavyBlueDevEx div.dxb {
        background: transparent;
        padding: 0px;
        font-family: Poppins;
        font-size: 13px;
        text-align: left;
    }*/
</style>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    var postponedCallbackRequired = false;
    var editGroupUrl = '<%= editGroupUrl %>';
    var returnUrl = '<%=sourceUrl%>';
    function adjustControlSize() {
        setTimeout(function () {
            var height = $(window).height();
            $("#s4-workspace").height(height);
        }, 10);
    }

    function AddUserToGroup(obj) {
       
        if (CallbackPanelGroup.InCallback()) {
            postponedCallbackRequired = true;
        }
        else
        {
            CallbackPanelGroup.PerformCallback("");
            aspxuserpopup.Show();
            
            return false;
        }
    }

    function ImportUsers(obj) {
        window.parent.UgitOpenPopupDialog('<%= importUrl %>', "", 'Import Users', '400px', '200px', 0, escape("<%= Request.Url.AbsolutePath %>"));
        return false;
    }


    function DeleteUser(s, e) {
       
        if ($("#<%=ddlUserGroups.ClientID%>").val() == 0) {
            alert("Please select a group from the dropdown");
            return false;
        }

        var length = grid.GetSelectedRowCount();
        if (length == 0) {
            alert("Please select the user(s) to delete from the group");
            e.processOnServer = false;
            return false;
        }

        if (confirm("Are you sure you want to delete the selected user(s) from this group?")) 
            e.processOnServer = true;
        else
            e.processOnServer = false;
    }
    
    function DeleteSiteUser(s, e) {
        var length = grid.GetSelectedRowCount();
        if (length == 0) {
            alert("Please select the user(s) to delete from the site");
            e.processOnServer = false;
            return false;
        }

        if (confirm("Are you sure you want to delete the selected user(s) from the site? (Choose Cancel button and Reassign Open/ In-progress tickets to another user(s), before deleting User(s).)"))
            e.processOnServer = true;
        else
            e.processOnServer = false;
    }

    function addNewGroup() {
        window.UgitOpenPopupDialog(editGroupUrl, '', 'Add New Group', "40", "53", false, escape("<%= Request.Url.AbsolutePath %>"));
    }

    function addEditGroup() {
        
        if (document.getElementById('<%=ddlUserGroups.ClientID%>').selectedIndex > 1) {
            var groupid = $('#<%=ddlUserGroups.ClientID%>').val();
            var param = "groupid=" + groupid;
            window.UgitOpenPopupDialog(editGroupUrl, param, 'Edit Group', "40", "50", false, escape("<%= Request.Url.AbsolutePath %>"));
        }
        else {
            alert("kindly select group");
        }
    }

    $(document).ready(function () {
        //Dropdownlist Selectedchange event
        $('.dropdowncss').change(function () {
            // Get Dropdownlist seleted item text
            // Get Dropdownlist selected item value
            var toggel = $('#<%=hdntogglevalue.ClientID%>');
            toggel.val($(".dropdowncss").val());

        })
    });


    function DisplayDialog()
    {
        var encodedMessage = DevExpress.utils.string.encodeHtml("You have exceeded number of users in the Trial plan.Please press 'Okay' to Purchase");
            var myDialog = DevExpress.ui.dialog.custom({
                title: "Limitation for Users",
                messageHtml: encodedMessage,
                toolbarItems:[
                { text: "Okay", onClick: function () { window.top.location.href = "/purchase.aspx" } },  
                    { text: "Cancel", onClick: function () {  window.parent.CloseWindowCallback(1, document.location.href); } }  
                ]
                
            });
            myDialog.show().done(function (dialogResult) {
               console.log(dialogResult.buttonText);
            });
    }
    function btnCreateUser_Click(s, e) {
        var checkLimit = '<%=limitExceed%>';

        if (checkLimit.toLowerCase() == 'true') {
            
         // DisplayDialog();    
            window.top.location.href = "/purchase.aspx";
            
        }

        else {

            var userInfoPage = '<%=userInfoPageUrl%>';
            userInfoPage += "?uID=0&newUser=1";
            window.parent.UgitOpenPopupDialog(userInfoPage, "", 'Add New User', "625px", "90");
            return false;
        }
    }
    function btnCreateBulkUser_Click(s, e) {

        var checkLimit = '<%=limitExceed%>';

        if (checkLimit.toLowerCase() == 'true') {
            //DisplayDialog();
            window.top.location.href = "/purchase.aspx";
        }
        else {
            var userInfoPage = '<%=BulkuserInfoPageUrl%>';
            // userInfoPage += "?uID=0&newUser=1";
            window.parent.UgitOpenPopupDialog(userInfoPage, "", 'Add Multiple Users', "750px", "90");
            return false;
        }
    }

    function setFormSubmitToFalse() {
        setTimeout(function () { _spFormOnSubmitCalled = false; }, 3000);
        return true;
    }

    $(document).ready(function () {
        //$(".selectTemplateForQuickTicket_popUp").parent().addClass("quick-ticket-parent");
        //$(".homeDb_gridContainer").parent().addClass("grid-mainContainer");
        //var url = window.location.href;
        //url = url.split('?');
        //if (url != null && url.length == 2) {

        //    sid = url[1].split('&');
           
        //    istrailuserdir = sid[0];
            //var hdnval = $("#redirecttrue").val();
            //var is = isPostBack()
            //alert(is);
           // if (istrailuserdir == "fromMail") {
                //window.parent.UgitOpenPopupDialog('/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=ServicesWizard&serviceID=' + sid[1], '', 'Service:Employee on-boarding', 90, 90, 0, '%2flayouts%2fugovernit%2fdelegatecontrol.aspx')
            //}
            //$("#redirecttrue").val("1");
            ////window.location.pathname = "/Pages/SVCRequests";
            //newUrl = url[0];
            //alert(newUrl);
            //console.log(newUrl);
            //$("#someAnchor").attr("href", "#dataThatYourWebServiceCanUnderstand").click();
            //history.pushState({}, null, "/Pages/RMM");
        //}


    });

    function ViewUserProfile(s, e) {

        var selectedvalue = e.visibleIndex - ((s.pageRowSize - 1) + (s.pageIndex * (s.pageRowSize - 1)) - (s.pageRowSize - 1)) - s.pageIndex;
        if (selectedvalue < 0)
            selectedvalue = selectedvalue + s.pageIndex;

        var key = s.keys[selectedvalue];
        var url = '<%=userInfoPageUrl%>';
        var param = "uID=" + key + "&UpdateUser=1";
        window.parent.UgitOpenPopupDialog(url, param, 'User Details: ', '625px', '90', 0, "");
       
    }

    //$('.dxflAGSys tr').each(function() {
    //    if ($(this).find('td:empty').length) $(this).remove();
    //});​
    function ProcessRequest() {
        var url = '<%=changeNameProcessUrl%>';
        window.parent.UgitOpenPopupDialog(url, '', 'Replace User', '500px', '70', 0, escape("<%= Request.Url.AbsolutePath %>"));
        
    }
    function disableMultiUser() {
       
        var length = grid.GetSelectedRowCount();
        if (length == 0) {
            alert("Please select the user(s) to disable");
            return false;
        }
        else {
            confirmDisablePopup.Show();
            return false;
        }
    }
    function DeleteFromGroupandUpdate(obj) {
        showwaiting.Show();
        hdnkeepAction.Set('action', '');
        hdnkeepAction.Set('action', obj);
        <%--$('#<%=btnDevExDisable.ClientID%>').trigger('click');--%>
    }
    function enableMultiUser() {      
        hdnKeeppopopen.Set('enableMultiuser', false);
        var length = grid.GetSelectedRowCount();
        if (length == 0) {
            alert("Please select the user(s) to enable");
            return false;
        }
        else {
            if (CallbackPanel.InCallback())
                postponedCallbackRequired = true;
            else
                CallbackPanel.PerformCallback();
            hdnKeeppopopen.Set('enableMultiuser', true);
            aspxenableuserpopup.Show();
            return false;
        }       
    }
  <%--  function AddToGroup() {
        $('#<%=btnDevExEnable.ClientID%>').trigger('click');
    }--%>

    function Validate() {
        if ($("#<%=ddlUserGroups.ClientID%>").val() == 0 || $("#<%=ddlUserGroups.ClientID%>").val() == 00) {
            alert("Please select a group from the dropdown");
            return false;
        }
        else
            return true;
    }
    
    function showloadingpanel()
    {
        showwaiting.Show();
    }
    
    function OnEndCallback(s, e) {
       
        if (postponedCallbackRequired) {
            CallbackPanel.PerformCallback();
            postponedCallbackRequired = false;
        }
        
    }
    function OnEndCallbackGroup(s, e) {
        if (postponedCallbackRequired) {
            CallbackPanelGroup.PerformCallback();
            postponedCallbackRequired = false;
        }
        showwaiting.Hide();
    }
    $(document).ready(function () {
         $('.userValueBox-Table').parent().addClass("userValueBox-searchFilterWrap");
         $('.userValueBox-searchFilterWrap').parent().addClass("userValueBox-searchFilterContainer");
    });
</script>
<script data-v="<%=UGITUtility.AssemblyVersion %>">
    function UpdateGridHeight() {
        grid.SetHeight(0);
        var containerHeight = ASPxClientUtils.GetDocumentClientHeight();
        if (document.body.scrollHeight > containerHeight)
            containerHeight = document.body.scrollHeight;
        grid.SetHeight(containerHeight);
    }
    window.addEventListener('resize', function (evt) {
        if (!ASPxClientUtils.androidPlatform)
            return;
        var activeElement = document.activeElement;
        if (activeElement && (activeElement.tagName === "INPUT" || activeElement.tagName === "TEXTAREA") && activeElement.scrollIntoViewIfNeeded)
            window.setTimeout(function () { activeElement.scrollIntoViewIfNeeded(); }, 0);
    });
</script>
<%--<a id="someAnchor"></a>--%>
<asp:HiddenField ID="hdnPageNo" runat="server" Value="1" />
<dx:ASPxHiddenField ID="hdnkeepAction" runat="server" ClientInstanceName="hdnkeepAction"></dx:ASPxHiddenField>
<dx:ASPxHiddenField ID="hdnKeeppopopen" runat="server" ClientInstanceName="hdnKeeppopopen"></dx:ASPxHiddenField>
<dx:ASPxLoadingPanel ID="showwaiting" runat="server" ClientInstanceName="showwaiting" Modal="true" Text="Please wait....."></dx:ASPxLoadingPanel>
<div class="col-md-12 col-sm-12 col-xs-12 noSidePadding rmm-gridContainer">
    <%--<tr>
        <td>
            <div style="float: left; padding-top: 5px; padding-left: 5px;" runat="server" id="newUserDiv">
               <dx:ASPxButton ID="btnCreateUser" runat="server" Text="Create New User" ImagePosition="Right" AutoPostBack="false" >
                   <Image Url="/content/images/add_icon.png"></Image>
                   <ClientSideEvents Click="btnCreateUser_Click" />
               </dx:ASPxButton>                
            </div>
            <div style="float: left; padding-top: 5px; padding-left: 5px;">
                 <dx:ASPxButton ID="btnDeleteFromSite" runat="server" OnClick="btnDeleteFromSite_Click" Visible="false" Text="Delete From Site"  ImagePosition="Right" >
                   <Image Url="/content/images/deleteicon.jpg"></Image>
                   <%--<ClientSideEvents Click="function(s, e) { DeleteSiteUser(this) }" />
                     <ClientSideEvents Click="DeleteSiteUser" />
               </dx:ASPxButton>               
            </div>
            <div id="div1replaceuser" runat="server" visible="false" style="float: left; padding-top: 5px; padding-left: 5px;">
                 <dx:ASPxButton ID="btnChangeNameProcess" runat="server"  Text="Replace User" AutoPostBack="false"  ImagePosition="Right" >
                   <Image Url="/content/images/duplicate1.png"></Image>
                   <ClientSideEvents Click="function(s, e) { ProcessRequest() }" />
               </dx:ASPxButton>                
            </div>
            <div id="divenable" runat="server" visible="false" style="float: left; padding-top: 5px; padding-left: 5px;">
                <dx:ASPxButton ID="btnenableuser" runat="server"   Text="Enable User(s)" AutoPostBack="false"  ImagePosition="Right" >
                   <Image Url="/content/Images/enable-user.png"></Image>
                   <ClientSideEvents Click="function(s, e) { enableMultiUser() }" />                    
               </dx:ASPxButton>                
            </div>
            <div id="divdisable" runat="server" visible="false" style="float: left; padding-top: 5px; padding-left: 5px;">
                <dx:ASPxButton ID="btndisableuser" runat="server"   Text="Disable User(s)" AutoPostBack="false"  ImagePosition="Right" >
                   <Image Url="/content/images/disable-user.png"></Image>
                   <ClientSideEvents Click="function(s, e) { disableMultiUser() }" />
               </dx:ASPxButton>
            </div>
            <div style="float: right; padding-top: 5px; padding-right: 5px;padding-left: 5px;">
                <dx:ASPxButton ID="btnAddInGroup" runat="server" Text="Add to Group" AutoPostBack="false" Visible="false"  ImagePosition="Right" >
                   <Image Url="/content/Images/add-group.jpg"></Image>
                   <ClientSideEvents Click="function(s, e) { AddUserToGroup(this) }" />
               </dx:ASPxButton>
               <dx:ASPxButton ID="btnDeleteFromGroup" runat="server" OnClick="btnDeleteFromGroup_Click" Text="Delete From Group"  Visible="false"  ImagePosition="Right" >
                   <Image Url="/content/Images/remove-group.png"></Image>
                   <ClientSideEvents Click="DeleteUser" />
               </dx:ASPxButton>               
            </div>
        </td>
    </tr>--%>
    <div class="row">
        <div class="containerdiv containerdiv-top rmmDb-Wrap">
            <div class="container-sub container-sub-padd rmmDb-dropDownLabel">User Group:</div>
            <div class="rmmDb-dropDownWrap">
                <div class="container-sub rmm-dropDownList-wrap">
                    <asp:DropDownList Width="250" AutoPostBack="true" OnInit="DdlUserGroups_Init" ID="ddlUserGroups" OnSelectedIndexChanged="ddlUserGroups_SelectedIndexChanged"
                        runat="server" CssClass="aspxDropDownList rmm-dropDownList"></asp:DropDownList>
                </div>
                <asp:Label ID="lbCreateGroup" runat="server" Visible="false" CssClass="addgroupicon">
                    <img  src="/content/Images/add-groupBlue.png" alt="Add" title="Add Group" onclick="addNewGroup()" style="width:18px;" />
                </asp:Label>
                <asp:Label ID="lbEditGroup" runat="server" Visible="false" CssClass="addgroupicon rmmEdit-groupIcon">
                    <img  src="/content/Images/editNewIcon.png" alt="Edit" title="Edit Group" onclick="addEditGroup()" style="width:16px" /></asp:Label>
                <div class="viewdropdown rmmDb-viewdropdownWrap">
                    <asp:DropDownList ID="ddlViews" runat="server" AutoPostBack="true" CssClass="dropdowncss aspxDropDownList rmm-dropDownList">
                        <asp:ListItem Text="List View" Value="List View"></asp:ListItem>
                        <asp:ListItem Text="Card View" Value="Card View"></asp:ListItem>
                        <asp:ListItem Text="Org Chart" Value="Org Chart"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <span class="rmmAction-iconWrap">
                    <asp:LinkButton ID="btnImportUsers" runat="server" Text="&nbsp;&nbsp;Add to Group&nbsp;&nbsp;" Visible="false"
                        ToolTip="Add to Group" OnClientClick="return ImportUsers(this)">
                        <span>
                            <i style="float: left; position: relative; top: -3px;left:2px">
                                <img src="/Content/images/importTasks.png"  style="border:none; width:20px;" title="Import Users" alt=""/>
                            </i> 
                        </span>
                    </asp:LinkButton>
                    <asp:ImageButton ID="onlyExcelExport" runat="server" Visible="false" ToolTip="Export User List" ImageUrl="/content/images/excel_icon.png" 
                        OnClick="onlyExcelExport_Click" OnClientClick="javascript:setFormSubmitToFalse()" CssClass="ExportClass rmmExport-icon" />
                    <asp:HiddenField runat="server" ID="hdntogglevalue" />
                </span>
            </div>
        </div>
            <asp:HiddenField ID="hQueryFilter" runat="server" Value="" />
            <div id="gridviewdiv" runat="server" class="containerdiv rmmHomeDb-GridWrap">
                <div class="pull-right rmmHomeDb-btnContainer">
                    <div class="rmmHomeDb-btnWrap next-cancel-but" runat="server" id="newUserDiv">
                        <dx:ASPxButton ID="btnCreateUser" runat="server" Text="Add New User" ImagePosition="Right" AutoPostBack="false" CssClass="btn btn-sm next" >
                            <Image Url="/content/images/add-groupBlue.png" Width="18px"></Image>
                            <ClientSideEvents Click="btnCreateUser_Click" />
                        </dx:ASPxButton>
                    </div>

                      <div class="rmmHomeDb-btnWrap next-cancel-but" runat="server" id="Div1">
                        <dx:ASPxButton ID="ASPxButton1" runat="server" Text="Add Multiple Users" ImagePosition="Right" AutoPostBack="false" CssClass="btn btn-sm next" >
                            <Image Url="/content/images/add-groupBlue.png" Width="18px"></Image>
                            <ClientSideEvents Click="btnCreateBulkUser_Click" />
                        </dx:ASPxButton>
                    </div>

                    <div class="rmmHomeDb-btnWrap next-cancel-but">
                        <dx:ASPxButton ID="btnDeleteFromSite" runat="server" OnClick="btnDeleteFromSite_Click" Visible="false" Text="Delete From Site" 
                            ImagePosition="Right" CssClass="btn btn-sm next">
                            <Image Url="/content/images/delet-groupBlue.png" Width="18px"></Image>
                            <%--<ClientSideEvents Click="function(s, e) { DeleteSiteUser(this) }" />--%>
                            <ClientSideEvents Click="DeleteSiteUser" />
                        </dx:ASPxButton>
                    </div>

                    <div id="div1replaceuser" runat="server" visible="false" class="false rmmHomeDb-btnWrap next-cancel-but">
                        <dx:ASPxButton ID="btnChangeNameProcess" runat="server" Text="Replace User" AutoPostBack="false" ImagePosition="Right" 
                            CssClass="btn btn-sm next">
                            <Image Url="/content/images/replace-userBlue.png" Width="18px"></Image>
                            <ClientSideEvents Click="function(s, e) { ProcessRequest() }" />
                        </dx:ASPxButton>
                    </div>
                    <div id="divenable" runat="server" visible="false" class="rmmHomeDb-btnWrap next-cancel-but">
                        <dx:ASPxButton ID="btnenableuser" runat="server" Text="Enable User(s)" AutoPostBack="false" ImagePosition="Right" CssClass="btn btn-sm  next">
                            <Image Url="/content/Images/enable-userBlue.png"  Width="18px"></Image>
                            <ClientSideEvents Click="function(s, e) { enableMultiUser() }" />
                        </dx:ASPxButton>
                    </div>
                    <div id="divdisable" runat="server" visible="false" class="rmmHomeDb-btnWrap next-cancel-but">
                        <dx:ASPxButton ID="btndisableuser" runat="server" Text="Disable User(s)" AutoPostBack="false" ImagePosition="Right" CssClass="btn btn-sm next">
                            <Image Url="/content/images/disable-userBlue.png" Width="18px"></Image>
                            <ClientSideEvents Click="function(s, e) { disableMultiUser() }" />
                        </dx:ASPxButton>
                    </div>
                    <div class="rmmHomeDb-btnWrap next-cancel-but">
                        <dx:ASPxButton ID="btnAddInGroup" runat="server" Text="Add to Group" AutoPostBack="false" Visible="false" ImagePosition="Right" 
                            CssClass="btn btn-sm  next">
                            <Image Url="/content/Images/add-groupBlue.png"  Width="18px"></Image>
                            <ClientSideEvents Click="function(s, e) { AddUserToGroup(this) }" />
                        </dx:ASPxButton>
                        <dx:ASPxButton ID="btnDeleteFromGroup" runat="server" OnClick="btnDeleteFromGroup_Click" Text="Delete From Group" Visible="false" ImagePosition="Right"
                            CssClass="btn btn-sm next">
                            <Image Url="/content/Images/delet-groupBlue.png"  Width="18px"></Image>
                            <ClientSideEvents Click="DeleteUser" />
                        </dx:ASPxButton>
                    </div>
                </div>
                <dx:ASPxGridView ID="aspxGridviewFiltered" runat="server" ClientInstanceName="grid"
                    OnHeaderFilterFillItems="aspxGridviewFiltered_HeaderFilterFillItems"  CssClass="customgridview homeGrid" 
                    OnDataBinding="aspxGridviewFiltered_DataBinding" KeyFieldName="Id" Width="100%" Images-HeaderActiveFilter-Url="~/Content/Images/Filter_Red_24.png"
                   OnHtmlDataCellPrepared="aspxGridViewCellPrepared" AutoGenerateColumns="false">
                    <settingsadaptivity adaptivitymode="HideDataCells" allowonlyoneadaptivedetailexpanded="true"></settingsadaptivity>
                    <settingscommandbutton>
                        <ShowAdaptiveDetailButton ButtonType="Button" Styles-Style-CssClass="homeGrid_openBTn"></ShowAdaptiveDetailButton>
                        <HideAdaptiveDetailButton ButtonType="Button" Styles-Style-CssClass="homeGrid_closeBTn"></HideAdaptiveDetailButton>
                    </settingscommandbutton>
                    <SettingsText EmptyDataRow="There are no items to show in this view." />
                    <Settings ShowFilterRowMenu="true" ShowHeaderFilterButton="true" ShowFilterRow="true" ShowFilterBar="Auto" ShowFooter="true" ShowGroupPanel="false" />
                    <SettingsPager Position="TopAndBottom" Mode="ShowPager" AlwaysShowPager="true" PageSize="15">
                        <PageSizeItemSettings Items="10, 15, 20, 25, 50, 75, 100" Visible="true" />
                    </SettingsPager>
                    <SettingsPopup>
                        <HeaderFilter Height="200" />
                    </SettingsPopup>
                    <Styles>
                        <Row CssClass="rmmGridRow CRMstatusGrid_row"></Row>
                        <AlternatingRow Enabled="True" BackColor="#EAEAEA" />
                        <Header Font-Bold="true" CssClass="CRMstatusGrid_headerRow"/>
                        <PagerBottomPanel CssClass="gridPager-bottomPannel"></PagerBottomPanel>
                    </Styles>
                    <SettingsCookies CookiesID="UserManagementlist" Enabled="true" StoreFiltering="true" StorePaging="true" StoreGroupingAndSorting="true" />

                </dx:ASPxGridView>
                 <script type="text/javascript">
                        ASPxClientControl.GetControlCollection().ControlsInitialized.AddHandler(function (s, e) {
                            UpdateGridHeight();
                        });
                        ASPxClientControl.GetControlCollection().BrowserWindowResized.AddHandler(function (s, e) {
                            UpdateGridHeight();
                        });
                </script>
                <asp:ObjectDataSource ID="ticketDataSource" runat="server"></asp:ObjectDataSource>
            </div>

            <%--div for holding card view for user--%>
            <div id="ugitUserProfileCardViewDiv" runat="server" class="containerdiv" visible="false">
                <dx:ASPxCardView ID="ugitUserProfileCardView" CardLayoutProperties-SettingsItems-ShowCaption="False" KeyFieldName="Id" runat="server" Width="100%" AutoGenerateColumns="false">
                    <CardLayoutProperties />
                    <Settings LayoutMode="Flow" />
                    <SettingsSearchPanel Visible="true" />
                    <Styles>
                        <SearchPanel Border-BorderStyle="None" CssClass="searchpanel"></SearchPanel>
                        <FlowCard Width="330px" Height="91px"></FlowCard>
                        <EmptyCard Wrap="True"></EmptyCard>
                    </Styles>
                    <SettingsBehavior AllowSelectByCardClick="true"
                        AllowSelectSingleCardOnly="True" AllowSort="False" />
                    <ClientSideEvents CardClick="ViewUserProfile" />
                    <SettingsPager Position="TopAndBottom" Mode="ShowPager" AlwaysShowPager="true">
                        <PageSizeItemSettings Visible="true" />
                    </SettingsPager>
                </dx:ASPxCardView>

            </div>
            <div id="orgChartdiv" runat="server" class="containerdiv">
            </div>
    </div>
</div>
<dx:ASPxPopupControl ID="msgPopup" runat="server" ClientInstanceName="msgPopup" Modal="True"  PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
    HeaderText="Message" AllowDragging="false" PopupAnimationType="None" EnableViewState="False" CssClass="departmentPopup rejectComment_popUp" width="250px" Height="70px">
    <ClientSideEvents CloseUp="function(s,e){msgPopup.Hide();}" PinnedChanged="" /> 
    <ContentCollection>
        <dx:PopupControlContentControl>
            <div class="col-md-12 col-sm-12 col-xs-12">
                <div class="msgPopup-wrap">
                    <asp:Label ID="lblMessage" runat="server"></asp:Label>
                </div>
            </div>
        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>
<dx:ASPxPopupControl ID="confirmDisablePopup" runat="server" ClientInstanceName="confirmDisablePopup"  Modal="True" Width="450px" 
    CssClass="aspxPopup" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" HeaderText="Disable User:" 
    AllowDragging="false" PopupAnimationType="None" EnableViewState="False" SettingsAdaptivity-Mode="Always">
   <ClientSideEvents CloseUp="function(s,e){confirmDisablePopup.Hide();}" PinnedChanged="" /> 
    <ContentCollection>
        <dx:PopupControlContentControl>
            <dx:ASPxPanel ID="pnldisable" ClientInstanceName="pnldisable" runat="server">
                <PanelCollection>
                    <dx:PanelContent>
                        <div class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap">
                            <div  class="row">
                                <dx:ASPxLabel ID="lblinformativeMsg" runat="server" EncodeHtml="false" CssClass="rmmdisable-userMsg"
                                    Text="This will disable and remove the user(s) from all groups.<br />Are you sure you want to proceed?<br />(Choose Cancel button and Reassign Open/ In-progress tickets to another user(s), before disabling User(s).)" ClientInstanceName="lblinformativeMsg"></dx:ASPxLabel>
                            </div>
                            <div class="row addEditPopup-btnWrap">
                                <dx:ASPxButton ID="delCancel" runat="server" CssClass="secondary-cancelBtn" Text="Cancel" ToolTip="Cancel">
                                    <ClientSideEvents Click="function(s,e){confirmDisablePopup.Hide();}" />
                                </dx:ASPxButton>
                                <dx:ASPxButton ID="btnYes" runat="server" CssClass="primary-blueBtn" Text="Yes" ToolTip="Delete User" OnClick="btnDevExDisable_Click">
                                    <ClientSideEvents Click="function(s, e){DeleteFromGroupandUpdate('yes');}" />
                                </dx:ASPxButton>
                               <%-- <asp:LinkButton ID="btnYes1" runat="server" Text="&nbsp;&nbsp;Yes&nbsp;&nbsp;" ToolTip="Delete User" CommandName="Yes" 
                                            OnClick="btnDevExDisable_Click" OnClientClick="DeleteFromGroupandUpdate('yes');" CssClass="popupBtn_save">
                                                <div class="aspLinkBtn-primaryBlueBtn">
                                                    <span>Yes</span>
                                                </div>
                                        </asp:LinkButton>--%>
                            </div>
                        </div>
                    </dx:PanelContent>
                </PanelCollection>
            </dx:ASPxPanel>
        </dx:PopupControlContentControl>
    </ContentCollection>

</dx:ASPxPopupControl>
<dx:ASPxPopupControl ID="aspxenableuserpopup" runat="server" ClientInstanceName="aspxenableuserpopup" Modal="True" Width="450px" Height="200px" CloseAction="CloseButton"
PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" HeaderText="Enable User(s):" CssClass="departmentPopup rejectComment_popUp" AllowDragging="true" 
    PopupAnimationType="None" EnableViewState="False">    
    <ClientSideEvents CloseUp="function(s,e){aspxenableuserpopup.Hide();}" PinnedChanged="" />  
    <ContentCollection>
        <dx:PopupControlContentControl>
            <dx:ASPxCallbackPanel ID="ASPxCallbackPanelEnableUser" runat="server" ClientInstanceName="CallbackPanel" RenderMode="Div" Width="100%">
                <PanelCollection>
                    <dx:PanelContent ID="PanelContent3" runat="server">
                        <dx:ASPxPanel ID="pnldisableuser" runat="server" ClientInstanceName="pnldisableuser">                    
                            <PanelCollection>
                                <dx:PanelContent>
                                    <div class="col-md-12 col-sm-12 col-xs-12 noPadding">
                                        <div class="row">
                                            <h3 class="ms-standardheader budget_fieldLabel">Add to Groups <%--(will always add to uGovernIT Members)--%>:</h3>
                                        </div>
                                        <div class="row">  
                                            <div class="ms-formtable accomp-popup col-md-12 col-sm-12 col-xs-12 noPadding">
                                                <div class="row rmmRole-field">
                                                    <%--<td class="ms-formlabel">
                                                        <h3 class="ms-standardheader">Select Roles<b style="color: Red">*</b>
                                                        </h3>
                                                    </td>--%>
                                                    <div class="ms-formbody accomp_inputField">                   
                                                         <ugit:LookUpValueBox ID="pplUserAccount" CssClass="lookupValueBox-dropown" FieldName="MultiRoles" runat="server" 
                                                             ShowSelectAllCheckbox="false" />
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="enableUser-actionBtn-wrap">
                                                         <dx:ASPxButton ID="btnCancel" ClientInstanceName="btnCancel" runat="server" Text="Cancel" AutoPostBack="false" 
                                                             CssClass="secondary-cancelBtn">
                                                            <ClientSideEvents Click="function(s,e){aspxenableuserpopup.Hide();}" />
                                                        </dx:ASPxButton>
                                                        <dx:ASPxButton ID="btnSavetogroup" ClientInstanceName="btnSavetogroup" runat="server" Text="Save" OnClick="btnDevExEnable_Click"
                                                            ValidationGroup="ValidateAddUserToGroup" CssClass="primary-blueBtn">
                                                        </dx:ASPxButton>
                                                    </div>
                                                </div>
                                            </div>  
                                        </div>
                                    </div>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dx:ASPxPanel>
                    </dx:PanelContent>
                </PanelCollection>
            </dx:ASPxCallbackPanel>
        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>
<dx:ASPxPopupControl ID="aspxuserpopup" runat="server" ClientInstanceName="aspxuserpopup"  Modal="True" Width="450px" CloseAction="CloseButton" 
PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" HeaderText="Add User To Group" AllowDragging="false" Height="350px"
    PopupAnimationType="None" EnableViewState="False" CssClass="aspxPopup" SettingsAdaptivity-Mode="Always">
    <ClientSideEvents CloseUp="function(s,e){aspxuserpopup.Hide();}" PinnedChanged=""  />  
    <ContentCollection>
        <dx:PopupControlContentControl>
            <dx:ASPxCallbackPanel ID="ASPxCallbackPanelGroupAdd" OnCallback="ASPxCallbackPanelGroupAdd_Callback" runat="server"  ClientInstanceName="CallbackPanelGroup" RenderMode="Div" Width="100%">
               <ClientSideEvents EndCallback="OnEndCallbackGroup"></ClientSideEvents>
                <PanelCollection>
                    <dx:PanelContent ID="PanelContent1" runat="server">
                        <dx:ASPxPanel ID="ASPxPanelGroup" runat="server" ClientInstanceName="ASPxPanelGroup">                    
                            <PanelCollection>
                                <dx:PanelContent>
                                    <div class="col-md-12 col-sm-12 col-xs-12 noPadding configVariable-popupWrap">
                                        <div class="ms-formtable accomp-popup">
                                            <div class="row">
                                                <div class="col-md-6 col-sm-6 col-xs-6 noPadding">
                                                    <div class="ms-formlabel">
                                                        <h3 class="ms-standardheader budget_fieldLabel">Group(s)<b style="color: Red">*</b></h3>
                                                    </div>
                                                    <div class="ms-formbody accomp_inputField">                   
                                                         <ugit:LookUpValueBox ID="rolesListBox" CssClass="lookupValueBox-dropown" FieldName="MultiRoles"
                                                             runat="server" ShowSelectAllCheckbox="false" />
                                                    </div>
                                                </div>
                                                <div class="col-md-6 col-sm-6 col-xs-6 noPadding">
                                                    <div class="ms-formlabel">
                                                        <h3 class="ms-standardheader budget_fieldLabel">Account<b style="color: Red">*</b></h3>
                                                    </div>
                                                    <div class="ms-formbody accomp_inputField">                   
                                                         <ugit:UserValueBox ID="userBoxList"  CssClass="txtbox-width userValueBox-dropDown" 
                                                             SelectionSet="User" isMulti="true" runat="server" />                    
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="ms-formlabel">
                                                    <h3 class="ms-standardheader rmmRole-fieldLabel budget_fieldLabel">Role</h3>
                                                </div>
                                                <div class="ms-formbody accomp_inputField">                   
                                                    <asp:DropDownList runat="server" CssClass=" itsmDropDownList aspxDropDownList" ID="ddlUserRole">
                                                    </asp:DropDownList>                    
                                                </div>
                                            </div>
                                            <div class="row addEditPopup-btnWrap">
                                                <dx:ASPxButton ID="ASPxButtonCancelUserToGroup" ClientInstanceName="btnCancel" runat="server" Text="Cancel" 
                                                    AutoPostBack="false" CssClass="secondary-cancelBtn">
                                                    <ClientSideEvents Click="function(s,e){aspxuserpopup.Hide();}" />
                                                </dx:ASPxButton>
                                                <dx:ASPxButton ID="ASPxButtonAddUserToGroup" ClientInstanceName="btnSavetogroup" runat="server" Text="Save" 
                                                    OnClick="btnAddUserToGroup_Click" ValidationGroup="ValidateAddUserToGroup" CssClass="primary-blueBtn">
                                                </dx:ASPxButton>
                                            </div>
                                        </div>
                                    </div>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dx:ASPxPanel>
                    </dx:PanelContent>
                </PanelCollection>
            </dx:ASPxCallbackPanel>
        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>
<asp:Literal ID="scriptLetral" runat="server"></asp:Literal>

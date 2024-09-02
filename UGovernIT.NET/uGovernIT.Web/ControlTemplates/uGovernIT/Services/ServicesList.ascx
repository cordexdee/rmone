

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ServicesList.ascx.cs" Inherits="uGovernIT.Web.ServicesList" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .StaticMenuStyle a {
        border-width: 4px;
        font: menu 16px arial;
        height: 0;
        padding: 2px 40px;
        text-align: center;
        width: auto;
    }

    .header {
        text-align: left;
        /*height: 30px;*/
        float: left;
        padding: 0px 2px;
    }

    #content {
        width: 100%;
    }

    .gridheader {
        height: 20px;
        background-color: #CED8D9;
        text-align: left;
        font-weight: lighter;
        font-style: normal;
    }

    a:hover {
        text-decoration: underline;
    }

    paddingRight0 {
        padding-right: 0px;
    }

    .archived-row {
        background: #A53421 !important;
    }

        .archived-row td {
            color: #fff !important;
        }

            .archived-row td a:link {
                color: #fff !important;
            }

    .chkisexport input {
        padding: 0px;
        margin: 0px;
    }
</style>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function setFormSubmitToFalse(s, e) {        
        var CheckedRowCount = ServiceGrid.GetSelectedRowCount();
        //setTimeout(function () { _spFormOnSubmitCalled = false; }, 3000);

        if (CheckedRowCount < 1) {
            alert("Please select a service to Export.");
            e.processOnServer = false;
        }
        else
        {
            e.processOnServer = true;
        }
    }
    /*
    function OnGetSelectedIdsForService(selectedValues) {
        if (selectedValues.length < 1) {
            alert("Please select a service to Export.");
            return false;
        }
        return true;
    }
    */
    function MoveToProduction() {

        var serviceIds = $('#<%=hdnServices.ClientID%>').val();
        var url = '<%=moveToProductionUrl%>';
        var params = "isService=true";
        if (serviceIds == "")
            alert("Please select atleast one");

        if (serviceIds != "") {
            params = params + "&serviceId=" + serviceIds;
        }
        window.parent.UgitOpenPopupDialog(url, params, 'Migrate Change(s)', '300px', '150px', 0, escape("<%= Request.Url.AbsolutePath %>"));
    }

    function BtCopyServiceLink_Click(s, e) {
        ServiceGrid.GetSelectedFieldValues('ID;Title', OnGetSelectedFieldValuesForService);
        e.processOnServer = false;
        //return false;
    }
    function OnGetSelectedFieldValuesForService(selectedValues) {
        /* old code
        var sldServiceBox = $(".chkisexport :checked");        
        if (sldServiceBox.length == 0) {
            alert("Please select a service to create link.");
            return false;
        }

        if (sldServiceBox.length > 1) {
            alert("Please select only one service to create link.");
            return false;
        }
        */
        if (selectedValues.length < 1) {
            alert("Please select a service to create link.");
            return false;
        }
        if (selectedValues.length > 1) {
            alert("Please select only one service to create link.");
            return false;
        }

        //var url = "<%= serviceHomeUrl%>" + sldServiceBox.parent().attr("serviceid");
        var url = "<%= serviceHomeUrl%>" + (selectedValues[0][0] || 0);
        serviceLinkBox.SetText(url);
        copyLinkPopup.Show();

        try {
            $(".serviceLinkBox textarea").select();
            document.execCommand('copy');
        } catch (ex) { }
        return false;
    }
   
    function OpenSendSurveyNotification(s, e) {
       
        ServiceGrid.GetSelectedFieldValues('ID;Title;ModuleNameLookup', OnGetSelectedFieldValuesForSurvey);
       
        return false;
    }

    function OnGetSelectedFieldValuesForSurvey(selectedValues) {
        
        if (selectedValues.length < 1) {
            alert("Please select at least one survey.");
            return false;
        }
        if (selectedValues.length > 1) {
            alert("Please select only one survey.");
            return false;
        }
        
        var modulesel = "";
        if (selectedValues[0][2] == null || selectedValues[0][2] == '')
            modulesel = "Generic";
        else
            modulesel = selectedValues[0][2]
        

        var Param = "SelectedModule=" + modulesel + "&surveyName=" + selectedValues[0][1] + "&ServiceID=" + selectedValues[0][0];
        UgitOpenPopupDialog('<%= sendSurveyURL %>', Param, "Send Survey", '80', '90', 0, escape("<%= Request.Url.AbsolutePath %>"));

    }

    function hidesendsurvey(s, e) {
        ServiceGrid.GetSelectedFieldValues('ModuleNameLookup', hidebutton);
    }
    //function hidebutton(selectedval) {
    //    var sval = selectedval[0];
    //    if (selectedval.length == 1) {
    //        //if (sval != null && sval.toLowerCase() !== "generic") {
    //        //    btSendSurvey.SetVisible(false); //$('.sendsurvey').hide();
    //        //}
    //        if (sval == null || sval == '' || sval == 'undefined') {
    //            btSendSurvey.SetVisible(true); //$('.sendsurvey').show();
    //        }
    //        else {
    //            btSendSurvey.SetVisible(false);
    //        }
    //    }
    //    else if (selectedval.length > 1) {
    //        btSendSurvey.SetVisible(false); //$('.sendsurvey').hide();
    //        //  alert("Please select only one survey.");
    //        //  return false;
    //    }
    //    else if (selectedval.length == 0) {
    //        btSendSurvey.SetVisible(true); //$('.sendsurvey').show();
    //        return false;
    //    }
    //}
    function hidebutton(selectedval) {
        <%--$('#<%= btSendSurvey.ClientID %>').show()
        $('#<%= btSendSurvey.ClientID %>').hide()--%>

        var sval = selectedval[0];
        if (selectedval.length == 1) {
            //if (sval != null && sval.toLowerCase() !== "generic") {
            //    btSendSurvey.SetVisible(false); //$('.sendsurvey').hide();
            //}
            if (sval == null || sval == '' || sval == 'undefined') {
                //btSendSurvey.SetVisible(true); //$('.sendsurvey').show();
                $('#<%= btSendSurvey.ClientID %>').show()
            }
            else {
                //btSendSurvey.SetVisible(false);
                $('#<%= btSendSurvey.ClientID %>').hide()
            }
        }
        else if (selectedval.length > 1) {
            $('#<%= btSendSurvey.ClientID %>').hide()
            //btSendSurvey.SetVisible(false); //$('.sendsurvey').hide();
            //  alert("Please select only one survey.");
            //  return false;
        }
        else if (selectedval.length == 0) {
            //btSendSurvey.SetVisible(true); //$('.sendsurvey').show();
            $('#<%= btSendSurvey.ClientID %>').show()
            return false;
        }
    }

    function lnkFullDeleteService_Click(s, e) {
        if (confirm('Are you sure you want to delete?')) {
            e.processOnServer = true;
        }
        else {
            e.processOnServer = false;
        }
    }

    function lnkArchive_Click(s, e) {
        if (confirm('Are you sure you want to archive?')) {
            e.processOnServer = true;
        }
        else {
            e.processOnServer = false;
        }
    }

    function btnPushServices_Click(s, e) {
        if (confirm('Are you sure you want to push?')) {
            e.processOnServer = true;
        }
        else {
            e.processOnServer = false;
        }
    }


    function lnkUnArchive_Click(s, e) {
        if (confirm('Are you sure you want to unarchive?')) {
            e.processOnServer = true;
        }
        else {
            e.processOnServer = false;
        }
    }
    function validateDuplicate(s, e) {

        var selectedServiceName = $("#<%= rdbServiceCategory.ClientID %> input:checked").val();
        var serviceName = "Service";

        if (selectedServiceName != null && selectedServiceName != "" && selectedServiceName == "~ModuleAgent~")
            serviceName = "Module Agent";

        var rowsSelected = ServiceGrid.GetSelectedRowCount();

        if (rowsSelected == 0) {
            alert('Please select at least one ' + serviceName + ' to Duplicate.');
            e.processOnServer = false;
            //return false;
        }
        else if (rowsSelected > 1) {
            alert('Please select only one ' + serviceName + ' to Duplicate.');
             e.processOnServer = false;
            //return false;
        }
    }
</script>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function UpdateGridHeight() {
        ServiceGrid.SetHeight(0);
        var containerHeight = ASPxClientUtils.GetDocumentClientHeight();
        if (document.body.scrollHeight > containerHeight)
            containerHeight = document.body.scrollHeight;
        ServiceGrid.SetHeight(containerHeight);
    }
    window.addEventListener('resize', function (evt) {
        if (!ASPxClientUtils.androidPlatform)
            return;
        var activeElement = document.activeElement;
        if (activeElement && (activeElement.tagName === "INPUT" || activeElement.tagName === "TEXTAREA") && activeElement.scrollIntoViewIfNeeded)
            window.setTimeout(function () { activeElement.scrollIntoViewIfNeeded(); }, 0);
    });
</script>
<asp:HiddenField ID="hdnServices" runat="server" />

<div class="col-md-12 col-sm-12 col-xs-12 formLayout-addPopupContainer">
    <div class="row popup-radioBtnWrap" id="content" style="margin-bottom:10px;">
        <div class="col-md-6 col-sm-6 col-xs-12">
            <asp:RadioButtonList runat="server" ID="rdbServiceCategory"  RepeatDirection="Horizontal" OnSelectedIndexChanged="rdbServiceCategory_SelectedIndexChanged" 
                AutoPostBack="true" CssClass="reportRadio-btnWrap">
                    <asp:ListItem Selected="True" Text="Service" Value="services"></asp:ListItem>
                    <asp:ListItem Text="Module Agent" Value="~ModuleAgent~"></asp:ListItem>
                    <asp:ListItem Text="Module FeedBack" Value="~ModuleFeedback~"></asp:ListItem>
                </asp:RadioButtonList>
            


        </div>
        <div class="col-md-6 col-sm-6 col-xs-12 popupChkWrap crm-checkWrap">
            <asp:CheckBox ID="chkShowDeleted" Text="Show Archived" runat="server" AutoPostBack="true" OnCheckedChanged="chkShowDeleted_CheckedChanged" />            
             <dx:ASPxButton ID="btnPushServices" runat="server" Visible="false" Text="Push Services" CssClass="primary-blueBtn" ToolTip="Push Services to tenant" OnClick="btnPushServices_Click"  AutoPostBack="false">
                <ClientSideEvents Click="btnPushServices_Click"  />
            </dx:ASPxButton>                
        </div>
    </div>
    <div>
        <asp:Label ID="lblPushMessage" Visible="false" runat="server" Text="Services pushed successfully" ForeColor="Green"></asp:Label>
    </div>
    <div class="row">
        <div class="col-md-12 col-sm-12 col-xs-12">
            <ugit:ASPxGridView ID="ServiceGrid" ClientInstanceName="ServiceGrid" runat="server" CssClass="customgridview homeGrid" Width="100%" EnableViewState="false"  OnHtmlRowPrepared="ServiceGrid_HtmlRowPrepared" KeyFieldName="ID" 
                    AutoGenerateColumns="false" DisplayGroupFieldName="true" SettingsBehavior-AutoExpandAllGroups="true" AllowFiltering="true"
                    AllowGroupCollapse="true" OnHeaderFilterFillItems="ServiceGrid_HeaderFilterFillItems" OnCustomColumnSort="ServiceGrid_CustomColumnSort" > 
                    <settingsadaptivity adaptivitymode="HideDataCells" allowonlyoneadaptivedetailexpanded="true" ></settingsadaptivity>
                    <Columns>
                        <dx:GridViewDataColumn Width="4%">
                            <DataItemTemplate>
                                <dx:ASPxCheckBox ID="chkIsExport"  ClientInstanceName="chkIsExport" runat="server" ServiceId='<%# Eval("ID") %>'  
                                    AutoPostBack="false"  OnLoad="cbCheck_Load" ></dx:ASPxCheckBox>                
                            </DataItemTemplate>
                        </dx:GridViewDataColumn>           
                        <dx:GridViewDataColumn Width="2%">
                            <DataItemTemplate>
                                    <a id="aEdit" href="javascript:UgitOpenPopupDialog('<%# Eval("serviceUrl") %>' , '', '<%# PageTitle %>', '95', '95', 0, '<%# Server.UrlEncode(Request.Url.AbsoluteUri) %>')">
                                    <img id="Imgedit" style="border: 0px;" runat="server" src="~/Content/Images/editNewIcon.png" width="16" />
                                </a>
                            </DataItemTemplate>
                        </dx:GridViewDataColumn>
                        <dx:GridViewDataColumn FieldName="ItemOrder" Width="5%" HeaderStyle-HorizontalAlign="Center"></dx:GridViewDataColumn>                       
                        <dx:GridViewDataColumn FieldName="Title" HeaderStyle-HorizontalAlign="Center">
                            <DataItemTemplate>
                                <a href="javascript:UgitOpenPopupDialog('<%# Eval("serviceUrl") %>' , '', '<%# PageTitle %>', '95', '95', 0, '<%# Server.UrlEncode(Request.Url.AbsoluteUri) %>')">
                                    <%# Eval("Title") %>
                                </a>
                            </DataItemTemplate>
                        </dx:GridViewDataColumn>
                        <dx:GridViewDataColumn FieldName="OwnerUser$" Caption="Owner" HeaderStyle-HorizontalAlign="Center"></dx:GridViewDataColumn>
                        <dx:GridViewDataColumn FieldName="ServiceDescription" Caption="Description" HeaderStyle-HorizontalAlign="Center"></dx:GridViewDataColumn>
                        <dx:GridViewDataColumn FieldName="ServiceCategoryType" Caption=" Category Name " GroupIndex="0" Settings-AllowGroup="True">
                           
                        </dx:GridViewDataColumn>
                        <dx:GridViewDataColumn FieldName="ModuleNameLookup" Visible="false"  VisibleIndex="3"  Caption="ModuleName">
                            <DataItemTemplate><%# Eval("ModuleNameLookup") %></DataItemTemplate>
                                                                     
                        </dx:GridViewDataColumn>
                        <%--<dx:GridViewDataColumn FieldName="ModuleTitle" Visible="false" VisibleIndex="4"  Caption="Module">
                            <DataItemTemplate><%# Eval("Title") %></DataItemTemplate>
                        </dx:GridViewDataColumn>--%>
                        <dx:GridViewDataColumn FieldName="ID" Visible="false" Caption="ID"></dx:GridViewDataColumn>
                    </Columns>
                     <settingscommandbutton>
                        <ShowAdaptiveDetailButton ButtonType="Button"   Styles-Style-CssClass="homeGrid_openBTn"></ShowAdaptiveDetailButton>
                        <HideAdaptiveDetailButton ButtonType="Button"  Styles-Style-CssClass="homeGrid_closeBTn"></HideAdaptiveDetailButton>
                    </settingscommandbutton>
                    <Styles>
                        <Row CssClass="homeGrid_dataRow"></Row>
                        <Header CssClass="homeGrid_headerColumn" Font-Bold="true"></Header>
                        <GroupRow CssClass="homeGrid-groupRow"></GroupRow>
                    </Styles>
                    <Settings ShowHeaderFilterButton="true" ShowHeaderFilterBlankItems="true" GroupFormat="{1}" />
                    <SettingsPager Visible="false" Mode="ShowAllRecords"></SettingsPager>
                    <SettingsBehavior AllowSelectByRowClick="true" SortMode="Custom"/>
                    <ClientSideEvents SelectionChanged="hidesendsurvey" />
                <FormatConditions>
                    <dx:GridViewFormatConditionHighlight FieldName="Title" Expression="[Deleted]= 1" Format="Custom" RowStyle-CssClass="formatcolor" ApplyToRow="true" />
                </FormatConditions>
            </ugit:ASPxGridView>
             <script type="text/javascript">
                 try {
                     ASPxClientControl.GetControlCollection().ControlsInitialized.AddHandler(function (s, e) {
                         UpdateGridHeight();
                     });
                     ASPxClientControl.GetControlCollection().BrowserWindowResized.AddHandler(function (s, e) {
                         UpdateGridHeight();
                     });
                 } catch (e) {
                 }                
            </script>
        </div>
        <div class="col-md-12 col-sm-12 col-xs-12">
             <asp:Label ID="lblMessage" Visible="false" runat="server" Text="select at least one record." ForeColor="Red"></asp:Label>
            
        </div>
    </div>
    <div class="row serviceList-footerBtnWrap">
        <div class="col-md-4 col-sm-4 col-xs-12">
            <dx:ASPxButton ID="btCategories" runat="server" Text="Categories" CssClass=" primary-blueBtn" ToolTip="Categories" AutoPostBack="false"></dx:ASPxButton>
            <dx:ASPxButton ID="btSendSurvey" ClientInstanceName="btSendSurvey" CssClass="primary-blueBtn" runat="server" Text="Send Survey" AutoPostBack="false">
                <ClientSideEvents Click="OpenSendSurveyNotification" />
            </dx:ASPxButton>
            <dx:ASPxButton ID="btnDuplicate" runat="server" Text="Duplicate" CssClass=" primary-blueBtn" ToolTip="Duplicate Services" AutoPostBack="false" OnClick="btnDuplicate_Click">
                <ClientSideEvents Click="function(s,e){
                    validateDuplicate();
                    }" />
            </dx:ASPxButton> 
            
        </div>
        <div class="col-md-3 col-sm-3 col-xs-12">
            <dx:ASPxButton ID="lnkFullDeleteService"  runat="server" Visible="false" CssClass="primary-blueBtn" Text="Delete" ToolTip="Delete" OnClick="lnkFullDeleteService_Click" AutoPostBack="false">
                <ClientSideEvents Click="lnkFullDeleteService_Click" />
            </dx:ASPxButton>
            <dx:ASPxButton ID="lnkArchive" runat="server" Visible="false" Text="Archive" CssClass="primary-blueBtn" ToolTip="Archive" OnClick="lnkArchive_Click"  AutoPostBack="false">
                <ClientSideEvents Click="lnkArchive_Click"  />
            </dx:ASPxButton>
            <dx:ASPxButton ID="lnkUnArchive" runat="server" Visible="false" CssClass="primary-blueBtn" Text="UnArchive" ToolTip="UnArchive" OnClick="lnkUnArchive_Click" AutoPostBack="false" >
                <ClientSideEvents Click="lnkUnArchive_Click" />
            </dx:ASPxButton>
        </div>
        <div class="col-md-5 col-sm-5 col-xs-12" style="text-align:right;">
            <dx:ASPxButton ID="btnMigrate" runat="server" Text="Migrate" CssClass="primary-blueBtn" ToolTip="Migrate Stage To Target" Visible="true" OnClick="btnMigrate_Click" ></dx:ASPxButton>
             <dx:ASPxButton ID="btExport" runat="server" Text="Export" CssClass="primary-blueBtn" ToolTip="Export Services" AutoPostBack="false" OnClick="btExport_Click">
                <ClientSideEvents Click="setFormSubmitToFalse" />
            </dx:ASPxButton>
             <dx:ASPxButton ID="btImport" runat="server" Text="Import" CssClass="primary-blueBtn" ToolTip="Import Services" ></dx:ASPxButton>
            <dx:ASPxButton ID="btCopyLink" runat="server" Text="Copy Link" ToolTip="Copy Link" CssClass="primary-blueBtn">
                <ClientSideEvents Click="BtCopyServiceLink_Click" />
            </dx:ASPxButton>
            <dx:ASPxButton ID="btNewbutton" runat="server" Text="New" CssClass="primary-blueBtn" ToolTip="New Request"></dx:ASPxButton>
        </div>
    </div>
</div>

<dx:ASPxPopupControl ID="copyLinkPopup" runat="server" Modal="True" Width="100%" CssClass="aspxPopup" SettingsAdaptivity-Mode="Always" 
    PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="copyLinkPopup"
    HeaderText="Copy Service Link" AllowDragging="false" PopupAnimationType="None" EnableViewState="False">
    <ContentCollection>
        <dx:PopupControlContentControl ID="PopupControlContentControl10" runat="server">
            <div class="col-md-12 col-sm-12 col-xs-12">
                <div class="row">
                    <dx:ASPxMemo CssClass="aspxMemo-linkBox serviceLinkBox" ReadOnly="true" ClientInstanceName="serviceLinkBox" ID="memoServiceLinkBox" Width="100%"
                      runat="server"></dx:ASPxMemo>
                </div>
                <div class="row adminCopyLink-btnWrap">
                    <ul class="adminCopyLink-btnUl">
                        <li runat="server" id="liCancel" class="" onmouseover="this.className='tabhover'" onmouseout="this.className=''">
                            <a id="aCancel" onclick="copyLinkPopup.Hide();" class="cancel-linkBtn" href="javascript:void(0);">Cancel</a>
                        </li>
                    </ul>
                </div>
            </div>
        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>










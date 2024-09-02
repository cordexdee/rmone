<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Issues.ascx.cs" Inherits="uGovernIT.Web.Issues" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script type="text/javascript" id="dxss_Issues" data-v="<%=UGITUtility.AssemblyVersion %>">

     <%--function CheckArchived( controlId) {
        var control = document.getElementById(controlId);
        var hControl = document.getElementById("<%=hdnSelectedUserList.ClientID %>");
        var showBt = document.getElementById("<%=cbShowArchivedIssues.ClientID %>");
        console.log(hControl.value);
         if (control.checked)
            {
                hControl.value = true;
            }
            else
            {
                hControl.value = false;
         }
        console.log(hControl.value);
        showBt.click();
    }--%>


    function UpdateGridHeight() {
        try {
            gridIssues.SetHeight(0);
            var containerHeight = ASPxClientUtils.GetDocumentClientHeight();
            if (document.body.scrollHeight > containerHeight)
                containerHeight = document.body.scrollHeight;
            gridIssues.SetHeight(containerHeight);
        } catch (e) {

        }
    }
    window.addEventListener('resize', function (evt) {
        if (!ASPxClientUtils.androidPlatform)
            return;
        var activeElement = document.activeElement;
        if (activeElement && (activeElement.tagName === "INPUT" || activeElement.tagName === "TEXTAREA") && activeElement.scrollIntoViewIfNeeded)
            window.setTimeout(function () { activeElement.scrollIntoViewIfNeeded(); }, 0);
    });

</script>
<%-- Issues --%>
<div class="mainblock col-md-12 noPadding">
    
    <div class="row">
        <asp:Label CssClass="errormessage-block ugitlight1lightest" runat="server" ID="issuesMessage"></asp:Label>
    </div>
    <div class="row">         
        <ugit:ASPxGridView ID="gridIssues" runat="server" AutoGenerateColumns="False" SettingsText-CommandClearFilter=""
        OnDataBinding="gridIssues_DataBinding" OnCustomCallback="gridIssues_CustomCallback"
        OnHtmlRowPrepared="gridIssues_HtmlRowPrepared" OnHeaderFilterFillItems="gridIssues_HeaderFilterFillItems"  
        ClientInstanceName="gridIssues" Width="100%" KeyFieldName="ID" CssClass="customgridview homeGrid" OnCustomColumnDisplayText="gridIssues_CustomColumnDisplayText" >
            <SettingsAdaptivity AdaptivityMode="HideDataCells" AllowOnlyOneAdaptiveDetailExpanded="true"></SettingsAdaptivity>
            <Columns>
                <dx:GridViewDataTextColumn Width="20px" Caption=" " Settings-AllowHeaderFilter="False" 
                    Settings-AllowSort="False" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                    FieldName="Created">                   
                    <HeaderCaptionTemplate>                        
                         <div onmousedown="return CancelEvent(event)" onmouseup="return CancelEvent(event)">
                             <asp:ImageButton ID="issueAddbtn" OnClientClick=" return newIssue();" runat="server" ImageUrl="~/Content/Images/plus-blue.png" Width="20px" CssClass="statusHeaderButton"/>                       
                        </div>                       
                    </HeaderCaptionTemplate> 
                    <DataItemTemplate>                        
                        <%#  Container.ItemIndex + 1%>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="Issues" FieldName="Title" Settings-AllowSort="True" Width="15%">                     
                                         
                    <DataItemTemplate>
                        <a id="aTitle" runat="server" href=""></a>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="Priority" FieldName="Priority" Width="10%"/>
                <dx:GridViewDataTextColumn Caption="Date Identified" FieldName="StartDate" Width="10%">
                    <PropertiesTextEdit DisplayFormatString="MMM-dd-yyyy" />
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="Due Date" FieldName="DueDate" Width="10%">
                    <PropertiesTextEdit DisplayFormatString="MMM-dd-yyyy" />
                    
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="Status" FieldName="Status" Width="10%"/>
                <dx:GridViewDataTextColumn Caption="Assigned To" FieldName="AssignedTo" Width="10%"/>

                <dx:GridViewDataTextColumn Caption="Description" PropertiesTextEdit-EncodeHtml="false" Settings-AllowHeaderFilter="False" Settings-AllowSort="False" FieldName="Description" Width="10%"/>

                <dx:GridViewDataTextColumn Caption=" " FieldName="ID"  Settings-AllowHeaderFilter="False" Settings-AllowSort="False">
                    <HeaderTemplate>                        
                      <div class="crm-checkWrap" style="float:right; width:auto; display:inline-block;">                        
                        <asp:CheckBox ID="cbHeaderShowArchivedIssues" Text="Show Archived"   runat="server" onClick="issueCheckArchived(this.id);"  OnInit="cbHeaderShowArchivedIssues_Init" />
                      </div>
                    </HeaderTemplate>                   
                    <DataItemTemplate>
                        <div>
                            <a id="aDelete" visible="false" title="Delete" runat="server" href="javascript:" style="float: right">
                                <img id="Img7" runat="server" width="16" src="/Content/Images/grayDelete.png" />
                            </a>
                            <a id="aArchive" runat="server" title="Archive" href="javascript:" style="float: right">
                                <img id="Img12" runat="server" width="16" src="/Content/Images/grayDelete.png" />
                            </a>
                             <a id="aUnArchive" runat="server" title="Unarchive" visible="false" href="javascript:" style="float: right">
                                <img id="Img9" runat="server" src="/Content/Images/unarchive.png" />
                            </a>
                            <a id="aEdit" runat="server" title="Edit" href="javascript:" style="float: right;">
                                <img id="Img8" runat="server" width="16" src="/Content/images/editNewIcon.png" class="pmmStatusAcc_editIcon" />
                            </a>
                           
                        </div>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
            </Columns>
            <SettingsCommandButton>
                <ShowAdaptiveDetailButton ButtonType="Button" Styles-Style-CssClass="homeGrid_openBTn"></ShowAdaptiveDetailButton>
                <HideAdaptiveDetailButton ButtonType="Button" Styles-Style-CssClass="homeGrid_closeBTn"></HideAdaptiveDetailButton>
            </SettingsCommandButton>
            <Styles>
                <Row CssClass="homeGrid_dataRow"></Row>
                <Header CssClass="homeGrid_headerColumn"></Header>
            </Styles>

            <SettingsBehavior AllowSelectByRowClick="false" AutoExpandAllGroups="true" />
            <SettingsPopup>
                <HeaderFilter Height="200" />
            </SettingsPopup>
            <SettingsPager Position="TopAndBottom">
                <PageSizeItemSettings Items="10, 15, 20, 25, 50, 75, 100" />
            </SettingsPager>
            <Settings ShowHeaderFilterButton="true" GridLines="None" />
            <ClientSideEvents />
        </ugit:ASPxGridView>
    </div>
    <div class="row">
        
         <div class="col-md-1 noPadding" style="padding-right:0; margin-top:10px; float:right; display:inline-block; ">
             <div style="display:none">
                     <asp:LinkButton OnClientClick="return newIssue()" ID="LinkButton3" runat="server" Text="Add New" CssClass="aspLinkButton"></asp:LinkButton>
             </div>
        </div>
         <div class="crm-checkWrap" style="float:right; width:auto; display:none; margin-top:15px;">
            <asp:CheckBox ID="cbShowArchivedIssues" Text="Show Archived" OnLoad="cbShowArchivedIssues_Load" AutoPostBack="true" runat="server" />
        </div>
    </div>
    <script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
        ASPxClientControl.GetControlCollection().ControlsInitialized.AddHandler(function (s, e) {
            UpdateGridHeight();
        });
        ASPxClientControl.GetControlCollection().BrowserWindowResized.AddHandler(function (s, e) {
            UpdateGridHeight();
        });
    </script>
   
</div>

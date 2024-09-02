<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewSubContractor.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.uGovernIT.ViewSubContractor" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function CheckSubContractor() {
        // debugger;
        if (<%=sunContratorCount%> > 0) {
            var messagetxt = "uCOREM says: Do you want to overwrite existing subcontractor list?";
            var answer = confirm(messagetxt)
            if (answer) {

                loadingPanel.Show();
                return true;
            }
            return false;
        }
        loadingPanel.Show();
        return true;
    }


    function openTicketDialog(path, params, titleVal, width, height, stopRefresh, returnUrl) {
        window.parent.parent.UgitOpenPopupDialog(path, "&isudlg=1&IsDlg=1", titleVal, width, height, stopRefresh, returnUrl);

     }

</script>
<script data-v="<%=UGITUtility.AssemblyVersion %>">
    function UpdateGridHeight() {
        ASPxGridViewProcoreSubContractor.SetHeight(0);
        var containerHeight = ASPxClientUtils.GetDocumentClientHeight();
        if (document.body.scrollHeight > containerHeight)
            containerHeight = document.body.scrollHeight;
        ASPxGridViewProcoreSubContractor.SetHeight(containerHeight);
    }
    window.addEventListener('resize', function (evt) {
        if (!ASPxClientUtils.androidPlatform)
            return;
        var activeElement = document.activeElement;
        if (activeElement && (activeElement.tagName === "INPUT" || activeElement.tagName === "TEXTAREA") && activeElement.scrollIntoViewIfNeeded)
            window.setTimeout(function () { activeElement.scrollIntoViewIfNeeded(); }, 0);
    });
</script>


<dx:ASPxLoadingPanel ID="LoadingPanel" runat="server" Text="Please Wait..." ClientInstanceName="loadingPanel"
    Modal="True">
</dx:ASPxLoadingPanel>

<div style="width: 100%;">
    <%--<div style="border: 2px solid #CED8D9">--%>
    <div>

    
        <dx:ASPxGridView ID="ASPxGridViewProcoreSubContractor" AutoGenerateColumns="False" runat="server" SettingsText-EmptyDataRow="No record found."
            KeyFieldName="ID" Width="100%" ClientInstanceName="ASPxGridViewProcoreSubContractor" OnHtmlRowPrepared="ASPxGridViewProcoreSubContractor_HtmlRowPrepared" 
            CssClass="CRMstatus_gridContainer homeGrid customgridview">
            <settingsadaptivity adaptivitymode="HideDataCells" allowonlyoneadaptivedetailexpanded="true" ></settingsadaptivity>
            <Columns>

                <%--  <dx:GridViewDataTextColumn FieldName=" " VisibleIndex="0" Width="30px">
                    <DataItemTemplate>
                        <img id="editLink" runat="server" src="/_layouts/15/images/uGovernIT/edit-icon.png" alt="Edit" title="Edit" style="float: right; padding-right: 10px; cursor: pointer;" />
                    </DataItemTemplate>
                    <Settings AllowAutoFilter="False" AllowSort="False" AllowHeaderFilter="False" />
                </dx:GridViewDataTextColumn>--%>

                <dx:GridViewDataDateColumn FieldName="ID"  Visible="false" Caption="ID" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                </dx:GridViewDataDateColumn>

                <dx:GridViewDataColumn FieldName="CommittmentNumber"  Caption="#" CellStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                </dx:GridViewDataColumn>
                <dx:GridViewDataColumn FieldName="Title" Caption="Trade" CellStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                </dx:GridViewDataColumn>

                <dx:GridViewDataTextColumn FieldName="StartDate"  Caption="Created Date" PropertiesTextEdit-DisplayFormatString="MMM-d-yyyy" CellStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataColumn FieldName="Status"  Caption="Status" CellStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                </dx:GridViewDataColumn>
                   <dx:GridViewDataColumn FieldName="CompanyName"  Caption="SubContractor Name" CellStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                </dx:GridViewDataColumn>
                <dx:GridViewDataTextColumn FieldName="Description"  Caption="Description" CellStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" PropertiesTextEdit-EncodeHtml="false" >
                </dx:GridViewDataTextColumn>
            </Columns>
            <settingscommandbutton>
                <ShowAdaptiveDetailButton ButtonType="Button"   Styles-Style-CssClass="homeGrid_openBTn"></ShowAdaptiveDetailButton>
                <HideAdaptiveDetailButton ButtonType="Button"  Styles-Style-CssClass="homeGrid_closeBTn"></HideAdaptiveDetailButton>
            </settingscommandbutton>
            <Settings ShowFooter="false" ShowHeaderFilterButton="false" />
            <SettingsBehavior AllowSort="false" AllowDragDrop="false" />
            <SettingsPopup>
                <HeaderFilter Height="200" />
            </SettingsPopup>
            <SettingsPager Mode="ShowAllRecords"></SettingsPager>
            <Styles AlternatingRow-CssClass="ms-alternatingstrong">
                <GroupRow Font-Bold="true" CssClass="homeGrid-groupRow"></GroupRow>
                <Row CssClass="CRMstatusGrid_row"></Row>
                <Header Font-Bold="true" CssClass="CRMstatusGrid_headerRow"></Header>
                <AlternatingRow CssClass="ms-alternatingstrong"></AlternatingRow>
                <InlineEditCell HorizontalAlign="Center"></InlineEditCell>
            </Styles>
        </dx:ASPxGridView>
    </div>
    <script type="text/javascript">
        ASPxClientControl.GetControlCollection().ControlsInitialized.AddHandler(function (s, e) {
            UpdateGridHeight();
        });
        ASPxClientControl.GetControlCollection().BrowserWindowResized.AddHandler(function (s, e) {
            UpdateGridHeight();
        });
    </script>
    <%--</div>--%>
    <div style="float:left;padding-left:10px;">
    <asp:ImageButton ID="ImgbtnSyncSubContractor" ToolTip="Get Subcontractors From Procore" runat="server" ImageUrl="/_layouts/15/images/uGovernIT/refresh-icon.png" OnClick="ImgbtnSyncSubContractor_Click" />
        </div>
</div>

<table class="ms-formtable" cellpadding="0" cellspacing="0" style="border-collapse: collapse" width="100%">
   <%-- <tr>
        <td>
            <asp:LinkButton ID="lnkbtnSyncSubContractor" runat="server" Text="&nbsp;&nbsp;Get Subcontractors From Procore&nbsp;&nbsp;" ToolTip="Get Subcontractors From Procore" OnClick="lnkbtnSyncSubContractor_Click">
                                    <span class="button-bg">
                                        <b style="float: left; font-weight: normal;">
                                           Get Subcontractors From Procore</b>
                                        <i style="float: left; position: relative; top: -3px;left:2px">
                                            <img src="/_layouts/15/images/uGovernIT/add_icon.png"  style="border:none;" title="" alt=""/>
                                        </i> 
                                    </span>
            </asp:LinkButton>
        </td>
    </tr>--%>
    <tr>
        <td style="text-align: center;">
            <asp:Label ID="lblMessage" runat="server" Visible="false"></asp:Label>
        </td>
    </tr>
</table>


<dx:ASPxPopupControl ID="fieldSetPopupControl" runat="server" Modal="True" Width="400px"
    PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="fieldSetPopupControl"
    HeaderText="Field Set" AllowDragging="false" PopupAnimationType="None" EnableViewState="False">
    <ContentCollection>
        <dx:PopupControlContentControl ID="PopupControlContentControl11" runat="server">
            <div style="float: left; height: auto; width: 100%;" class="first_tier_nav">
                <table style="width: 100%;">
                    <tr>
                        <td style="width: 100px;">
                            <asp:Label ID="lblFieldSetName" runat="server" Text="Field Set Name<b style='color:red;'>*</b>"> </asp:Label>

                        </td>
                        <td>
                            <asp:DropDownList ID="ddlSet" runat="server" Width="200px"></asp:DropDownList>

                        </td>
                    </tr>
                   

                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lblErrorMessage" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                        </td>
                    </tr>

                    <tr>
                        <td colspan="2">
                            <div style="float: right; width: 100%; margin-top: 10px">
                                <ul style="float: right">
                                    <li runat="server" id="liSave" class="" onmouseover="this.className='tabhover'" onmouseout="this.className=''">
                                        <asp:LinkButton ID="lnkbtnSave" CssClass="save" runat="server" OnClick="lnkbtnSave_Click" OnClientClick="return CheckSubContractor();" Text="Sync"></asp:LinkButton>
                                    </li>
                                    
                                    <li runat="server" id="li24" class="" onmouseover="this.className='tabhover'" onmouseout="this.className=''">
                                        <%--<a id="a5" style="color: white" onclick="fieldSetPopupControl.Hide();" class="cancelwhite" href="javascript:void(0);">Cancel</a>--%>
                                        <asp:LinkButton ID="lnkbtnCancel" runat="server" class="cancelwhite" Text="Cancel" OnClick="lnkbtnCancel_Click"></asp:LinkButton>
                                    </li>

                                </ul>
                            </div>
                        </td>
                    </tr>

                </table>
            </div>
        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>

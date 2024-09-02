<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EmailsControl.ascx.cs" Inherits="uGovernIT.Web.EmailsControl" %>

<%@ Register Assembly="DevExpress.Web.ASPxHtmlEditor.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxHtmlEditor" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxSpellChecker.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxSpellChecker" TagPrefix="dx" %>

<%@ Import Namespace="uGovernIT.Utility" %>


<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">


    function adjustControlSize() {
        setTimeout(function () {
            try {
                DXTicketEmails.AdjustControl();
            }
            catch (ex) { }
        }, 10);
    }

    function openTicketDialog(path, params, titleVal, width, height, stopRefresh) {
        window.parent.UgitOpenPopupDialog(path, params, titleVal, '90', '90', 0);
    }


</script>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
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
<script data-v="<%=UGITUtility.AssemblyVersion %>">
    function UpdateGridHeight() {
        DXTicketEmails.SetHeight(0);
        var containerHeight = ASPxClientUtils.GetDocumentClientHeight();
        if (document.body.scrollHeight > containerHeight)
            containerHeight = document.body.scrollHeight;
        DXTicketEmails.SetHeight(containerHeight);
    }
    window.addEventListener('resize', function (evt) {
        if (!ASPxClientUtils.androidPlatform)
            return;
        var activeElement = document.activeElement;
        if (activeElement && (activeElement.tagName === "INPUT" || activeElement.tagName === "TEXTAREA") && activeElement.scrollIntoViewIfNeeded)
            window.setTimeout(function () { activeElement.scrollIntoViewIfNeeded(); }, 0);
    });
</script>

<div class="row">
    <div class="col-md-12 col-sm-12 col-xs-12 noPadding">
        <dx:ASPxGridView ID="ASPxGridViewTicketEmails" AutoGenerateColumns="False" runat="server" SettingsText-EmptyDataRow="No record found."
            KeyFieldName="ID" ClientInstanceName="DXTicketEmails" CssClass="customgridview homeGrid emailControlGrid" Width="100%"
            OnHtmlRowPrepared="ASPxGridViewTicketEmails_HtmlRowPrepared" OnCustomColumnDisplayText="ASPxGridViewTicketEmails_CustomColumnDisplayText">
           <settingsadaptivity adaptivitymode="HideDataCells" allowonlyoneadaptivedetailexpanded="true" ></settingsadaptivity>
            <Columns >
        
               <%-- <dx:GridViewDataTextColumn FieldName=" ">
                    <DataItemTemplate>--%>
                        <%--<img src="/Content/Images/attach.png" alt="Attahcment" style="float: right; padding-right: 10px;" />--%>
                        <%--<img src="/Content/Images/attach.gif" alt="Attahcment" style="float: right; padding-right: 10px; display: <%#  uHelper.StringToBoolean(Eval(DatabaseObjects.Columns.Attachments)) == false ? "none" : "block" %>" />--%>
                  <%--  </DataItemTemplate>
                    <Settings AllowAutoFilter="False" AllowSort="False" AllowHeaderFilter="False" />
                </dx:GridViewDataTextColumn>--%>

                <dx:GridViewDataDateColumn FieldName="TicketEmailType" Caption=" " PropertiesDateEdit-EncodeHtml="false" >
                    <Settings HeaderFilterMode="CheckedList" />
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                    <CellStyle HorizontalAlign="Left"></CellStyle>
                </dx:GridViewDataDateColumn>

                   <dx:GridViewDataDateColumn FieldName="Created" Caption="Created" SortOrder="Ascending">
                    <Settings HeaderFilterMode="CheckedList" />
                </dx:GridViewDataDateColumn>

                <dx:GridViewDataColumn FieldName="EmailIDFrom" Caption="Email From">
                    <Settings HeaderFilterMode="CheckedList" />
                </dx:GridViewDataColumn>

                <dx:GridViewDataColumn FieldName="EmailIDTo"  Caption="Email To"  CellStyle-Wrap="True">
                    <Settings HeaderFilterMode="CheckedList" />
            
                </dx:GridViewDataColumn>
        <%--        <dx:GridViewDataColumn FieldName="EmailIDCC" VisibleIndex="5" Caption="CC">
                    <Settings HeaderFilterMode="CheckedList" />
                </dx:GridViewDataColumn>--%>
                <dx:GridViewDataColumn FieldName="MailSubject" Caption="Subject" >
                    <Settings HeaderFilterMode="CheckedList" />
                </dx:GridViewDataColumn>
                <dx:GridViewDataColumn FieldName="EmailStatus" Caption="Status" >
                    <Settings HeaderFilterMode="CheckedList" />
                </dx:GridViewDataColumn>
            </Columns>
            <settingscommandbutton>
                <ShowAdaptiveDetailButton ButtonType="Button"   Styles-Style-CssClass="homeGrid_openBTn"></ShowAdaptiveDetailButton>
                <HideAdaptiveDetailButton ButtonType="Button"  Styles-Style-CssClass="homeGrid_closeBTn"></HideAdaptiveDetailButton>
            </settingscommandbutton>
            <Settings ShowFooter="false" ShowHeaderFilterButton="true"  />
    
            <SettingsBehavior AllowSort="true" AllowDragDrop="false" AutoExpandAllGroups="true" />
           <%-- <Settings VerticalScrollBarMode="Auto" />--%>
            <SettingsPopup>
                <HeaderFilter Height="100" />
            </SettingsPopup>
            <Styles AlternatingRow-CssClass="ms-alternatingstrong">
                <GroupRow Font-Bold="true" CssClass="homeGrid-groupRow"></GroupRow>
                <Row CssClass="homeGrid_dataRow"></Row>
                <Header Font-Bold="true" CssClass="homeGrid_headerColumn"></Header>
                <%--<AlternatingRow CssClass="ms-alternatingstrong"></AlternatingRow>
                <InlineEditCell HorizontalAlign="Center"></InlineEditCell>--%>
            </Styles>

        </dx:ASPxGridView>
        
    </div>
</div>
    
    


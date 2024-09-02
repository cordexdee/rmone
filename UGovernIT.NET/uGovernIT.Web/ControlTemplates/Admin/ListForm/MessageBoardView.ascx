<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MessageBoardView.ascx.cs" Inherits="uGovernIT.Web.MessageBoardView" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function UpdateGridHeight() {
        _gridView.SetHeight(0);
        var containerHeight = ASPxClientUtils.GetDocumentClientHeight();
        if (document.body.scrollHeight > containerHeight)
            containerHeight = document.body.scrollHeight;
        _gridView.SetHeight(containerHeight);
    }
    window.addEventListener('resize', function (evt) {
        if (!ASPxClientUtils.androidPlatform)
            return;
        var activeElement = document.activeElement;
        if (activeElement && (activeElement.tagName === "INPUT" || activeElement.tagName === "TEXTAREA") && activeElement.scrollIntoViewIfNeeded)
            window.setTimeout(function () { activeElement.scrollIntoViewIfNeeded(); }, 0);
    });
</script>

<div class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap">
    <div class="row">
         <div class="ms-formlabel">
            <h3 class="ms-standardheader budget_fieldLabel">Select Overall Status:</h3>
        </div>
        <div class="ms-formbody accomp_inputField">
            <asp:RadioButtonList ID="rbMessageTypeList"
                runat="server"
                RepeatColumns="5"
                AutoPostBack ="true"
                OnSelectedIndexChanged ="rbMessageTypeList_SelectedIndexChanged"
                RepeatLayout="Table" CssClass="messagetypes custom-radiobuttonlist">
                    
                <asp:ListItem Text="<label class='msgRadio-btnLabel'>Ok</label><img class='msgRadio-btnImg' src='/Content/Images/message_good.png'/>" Value="Ok"></asp:ListItem>
                <asp:ListItem Text="<label class='msgRadio-btnLabel'>Information</label><img class='msgRadio-btnImg' src='/Content/Images/message_information.png'/>" Value="Information"></asp:ListItem>
                <asp:ListItem Text="<label class='msgRadio-btnLabel'>Warning</label><img class='msgRadio-btnImg' src='/Content/Images/message_warning.png'/>" Value="Warning"></asp:ListItem>
                <asp:ListItem Text="<label class='msgRadio-btnLabel'>Critical</label><img class='msgRadio-btnImg' src='/Content/Images/message_critical.png'/>" Value="Critical"></asp:ListItem>
                <asp:ListItem Text="<label class='msgRadio-btnLabel'>None</label>" Value="None"></asp:ListItem>
            </asp:RadioButtonList>
        </div>
    </div>
    <div class="row msgAction-wrap">
        <div class="col-md-4 col-sm-4 col-xs-12 noPadding">
             <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Select Message Type</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:DropDownList ID="ddlMessageType" runat="server" AppendDataBoundItems="true" AutoPostBack="true"
                    OnSelectedIndexChanged="ddlMessageType_SelectedIndexChanged" CssClass="itsmDropDownList aspxDropDownList">
                </asp:DropDownList>
            </div>
        </div>
        <div class="col-md-8 col-sm-8 col-xs-12 noPadding">
            <div class="headerContent-right">
                <div class="headerItem-addItemBtn">
                    <a id="aAddItem_Top" runat="server" href="" class="primary-btn-link">
                        <img id="Img2" runat="server" src="/Content/Images/plus-symbol.png" />
                        <asp:Label ID="Label1" runat="server" Text="Add New Item" CssClass="lbltop"></asp:Label>
                    </a>
                </div>
                
            </div>
        </div>
    </div>
    <div class="row" id="content">
        <div class="col-md-12 col-sm-12 col-xs-12 noPadding" style="margin-bottom:10px;">
            <div class="headerItem-showChkBox" style="margin-top:0px; padding-right: 15px;">
                <div class="crm-checkWrap">
                    <asp:CheckBox ID="chkShowExpired" Text="Show Expired" runat="server" AutoPostBack="true" 
                        OnCheckedChanged="chkShowExpired_CheckedChanged" />
                </div>
            </div>
            <div class="headerItem-showChkBox" style="margin-top:0px; padding-right: 15px;">
                <div class="crm-checkWrap">
                    <asp:CheckBox ID="chkHideIfEmpty" Text="Hide If Empty" runat="server"  Checked="false" AutoPostBack="true" 
                        OnCheckedChanged="chkHideIfEmpty_CheckedChanged" />
                </div>
            </div>
        </div>
        <div class="col-md-12 col-sm-12 col-xs-12 noPadding">
            <ugit:ASPxGridView ID="_gridView" runat="server" ClientInstanceName="_gridView" EnableViewState="false" AutoGenerateColumns="false" 
                KeyFieldName="ID" OnHtmlRowPrepared="_gridView_HtmlRowPrepared" Width="100%" SettingsBehavior-AllowSort="False" CssClass="customgridview homeGrid">
                <settingsadaptivity adaptivitymode="HideDataCells" allowonlyoneadaptivedetailexpanded="true" ></settingsadaptivity>
                <Columns>
                    <dx:GridViewDataTextColumn  Name="aEdit" >
                        <DataItemTemplate>
                            <a id="aEdit" runat="server" href="">
                                <img id="Imgedit" runat="server" src="~/Content/Images/editNewIcon.png" width="16" />
                            </a>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="MessageType" Caption="Type" Width="15%">
                        <DataItemTemplate>
                                <img style="float:left;" id="ImgMessageType" runat="server"  />
                                <asp:Label style="float:left;padding-left: 3px;" ID="lblMessageType" runat="server" Text ='<%#Bind("MessageType") %>'></asp:Label> 
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Expires" Caption="Expires" >
                                   
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Message" >
                        <DataItemTemplate>
                            <a id="aBody" runat="server" href=""></a>
                            <asp:HiddenField runat="server" ID="hiddenBody" Value='<%#Bind("Body") %>' />
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Modified By">
                        <DataItemTemplate>
                            <asp:Label style="float:left;padding-left: 3px;" ID="lblModifiedBy" runat="server" Text ='<%#Bind("ModifiedBy") %>'></asp:Label> 
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Modified On">
                        <DataItemTemplate>
                            <asp:Label style="float:left;padding-left: 3px;" ID="lblModifiedOn" runat="server" Text ='<%#Bind("Modified","{0:MMM-dd-yyyy}") %>'></asp:Label> 
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                </Columns>
                <settingscommandbutton>
                    <ShowAdaptiveDetailButton ButtonType="Button"   Styles-Style-CssClass="homeGrid_openBTn"></ShowAdaptiveDetailButton>
                    <HideAdaptiveDetailButton ButtonType="Button"  Styles-Style-CssClass="homeGrid_closeBTn"></HideAdaptiveDetailButton>
                </settingscommandbutton>
                <Styles>
                    <Row CssClass="homeGrid_dataRow"></Row>
                    <Header CssClass=" homeGrid_headerColumn"></Header>
                </Styles>
            </ugit:ASPxGridView>
            <script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
                ASPxClientControl.GetControlCollection().ControlsInitialized.AddHandler(function (s, e) {
                    UpdateGridHeight();
                });
                ASPxClientControl.GetControlCollection().BrowserWindowResized.AddHandler(function (s, e) {
                    UpdateGridHeight();
                });
            </script>
        </div>
    </div>
    <div class="row bottom-addBtn">
        <div class="headerItem-addItemBtn">
            <a id="aAddItem" runat="server" href="" class="primary-btn-link">
                <img id="Img1" runat="server" src="/Content/Images/plus-symbol.png" />
                <asp:Label ID="LblAddItem" runat="server" Text="Add New Item" CssClass="lbl"></asp:Label>
            </a>
        </div>
    </div>
</div>

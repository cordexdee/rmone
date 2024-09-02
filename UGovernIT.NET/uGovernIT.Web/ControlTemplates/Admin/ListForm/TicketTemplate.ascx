
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TicketTemplate.ascx.cs" Inherits="uGovernIT.Web.Template" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script data-v="<%=UGITUtility.AssemblyVersion %>">
    var isNew = true;
    function Validate() {
        if (isNew) {
            if ($("#<%=txtTemplateName.ClientID%>").val().length == 0) {
                alert("Enter template name");
                return false;
            }
            else {
                cbValidate.PerformCallback("ValidateName");
            }
            return false;
        }
        else {
            isNew = false;
        }
        return true;
    }

    function OnSelectionChanged(s, e) {
        if (e.isSelected) {
            var key = s.GetRowKey(e.visibleIndex);
        }
    }

    function OnRequestTypeChange(obj) {
        $(obj).parent().find("[id$='hndCCValue']").val($(obj).val());
    }


    function cbValidate_EndCallback(s, e) {
        if (typeof (s.cpIsValidated) != 'undefined') {
            if (s.cpIsValidated == true) {
                if (confirm("Template \"" + $("#<%=txtTemplateName.ClientID%>").val() + "\" already exists. Do you want to overwrite?")) {
                    //ClientPopupControl.PerformCallback("overwrite");
                    isNew = false;
                    $("#<%=btSave.ClientID%>").get(0).click();
                }
            }
            else {
                isNew = false;
                $("#<%=btSave.ClientID%>").get(0).click();
            }
        }
    }

    function rowClick(s, e) {
        
    }

    function IsNumeric(e) {
        var keyCode = e.which ? e.which : e.keyCode
        var ret = (keyCode >= 48 && keyCode <= 57);
        return ret;
    }
    function IsFloat(e,obj) {
        debugger;
        var ff = '/[^0-9\.]/g';
        $(obj).val($(obj).val().replace(ff,''));
        if ((e.which != 46 || $(obj).val().indexOf('.') != -1) && (e.which < 48 || e.which > 57)) {
            return false;
        }
        else {
            return true
        }
       
      
       
    }
</script>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
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

    $(document).ready(function () {
        $('.saveAsTemp-popupWrap').parent().addClass('saveAsTemp-popupContainer')
    });
</script>




<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .hide {
        display: none;
    }
</style>

<dx:ASPxCallbackPanel runat="server" ID="cbValidate" ClientInstanceName="cbValidate" OnCallback="cbValidate_Callback"
    Width="100%">
    <PanelCollection>
        <dx:PanelContent ID="PanelContent2" runat="server">
        </dx:PanelContent>
    </PanelCollection>
    <ClientSideEvents EndCallback="cbValidate_EndCallback" />
</dx:ASPxCallbackPanel>

<dx:ASPxPanel ID="pnlTemplate" runat="server" CssClass="saveAsTemp-popupWrap">
    <PanelCollection>
        <dx:PanelContent>
            <div class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap">
                <div class="ms-formtable accomp-popup ">
                    <div class="row">
                        <div style="margin:auto;">
                            <div class="col-md-4 col-xs-12 col-sm-4 noLeftPadding">
                                <div class="ms-formlabel">
                                    <h3 class="ms-standardheader budget_fieldLabel">Template Name:<span style="color: red">*</span></h3>
                                </div>
                                <div class="ms-formbody accomp_inputField">
                                    <asp:TextBox ID="txtTemplateName" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-4 col-xs-12 col-sm-4 noRightPadding">
                                <div class="ms-formlabel">
                                    <h3 class=" ms-standardheader budget_fieldLabel">Template Type:<span style="color: red">*</span></h3>
                                </div>
                                <div class="ms-formbody accomp_inputField">
                                    <asp:DropDownList ID="ddlTicketType" runat="server" CssClass="itsmDropDownList aspxDropDownList">
                                        <asp:ListItem Text="New Ticket" Value="Ticket"></asp:ListItem>
                                        <asp:ListItem Text="Macro" Value="Macro"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="ms-formlabel">
                            <h3 class="ms-standardheader budget_fieldLabel ">
                                <asp:Label ID="lblMessage" runat="server"  Text="Select field(s) to include"></asp:Label>
                            </h3>
                        </div>
                    </div>
                    <div class="row">
                        <ugit:ASPxGridView ID="grid" runat="server" AutoGenerateColumns="False" SettingsText-CommandClearFilter="" 
                            OnHtmlRowPrepared="grid_HtmlRowPrepared" OnDataBinding="grid_DataBinding" OnDataBound="grid_DataBound"
                            ClientInstanceName="grid" Width="100%" KeyFieldName="FieldName" CssClass="customgridview homeGrid">
                            <settingsadaptivity adaptivitymode="HideDataCells" allowonlyoneadaptivedetailexpanded="true" ></settingsadaptivity>
                            <settingscommandbutton>
                                <ShowAdaptiveDetailButton ButtonType="Button"   Styles-Style-CssClass="homeGrid_openBTn"></ShowAdaptiveDetailButton>
                                <HideAdaptiveDetailButton ButtonType="Button"  Styles-Style-CssClass="homeGrid_closeBTn"></HideAdaptiveDetailButton>
                            </settingscommandbutton>
                            <Columns>
                                <dx:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" HeaderStyle-Font-Bold="true">
                                </dx:GridViewCommandColumn>
                                <dx:GridViewDataTextColumn Caption="Tab" GroupIndex="0" CellStyle-HorizontalAlign="Left" HeaderStyle-Font-Bold="true" 
                                    HeaderStyle-HorizontalAlign="Left" FieldName="TabId" />
                                <dx:GridViewDataTextColumn Caption="Display Name" CellStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" 
                                    HeaderStyle-Font-Bold="true" FieldName="FieldDisplayName">
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="Internal Name" Width="100%" FieldName="FieldName" HeaderStyle-Font-Bold="true" CellStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                            </Columns>
                            <Templates>
                                <GroupRowContent>
                                    <%#  "Tab: " + Eval("TabName")%>
                                </GroupRowContent>
                            </Templates>
                            <SettingsBehavior SortMode="DisplayText" AllowSelectByRowClick="false" AllowSelectSingleRowOnly="false" AutoExpandAllGroups="true" />
                            <SettingsPopup>
                                <HeaderFilter Height="200" />
                            </SettingsPopup>
                            <SettingsPager Position="TopAndBottom">
                                <PageSizeItemSettings Items="10, 15, 20, 25, 50, 75, 100" />
                            </SettingsPager>
                            <Styles>
                                <Row CssClass="homeGrid_dataRow"></Row>
                                <Header Font-Bold="true" CssClass="homeGrid_headerColumn"></Header>
                            </Styles>
                            <ClientSideEvents SelectionChanged="OnSelectionChanged" />
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
                    <div class="row addEditPopup-btnWrap">
                        <dx:ASPxButton ID="btSave" runat="server" Text="Save" ToolTip="Save" OnClick="btSave_Click" CssClass="primary-blueBtn">
                            <ClientSideEvents Click="function(s, e){return Validate();}" />
                        </dx:ASPxButton>
                    </div>
                </div>
            </div>
        </dx:PanelContent>
    </PanelCollection>
</dx:ASPxPanel>


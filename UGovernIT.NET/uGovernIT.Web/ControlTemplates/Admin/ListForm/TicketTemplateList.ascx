<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TicketTemplateList.ascx.cs" Inherits="uGovernIT.Web.TicketTemplateList" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script data-v="<%=UGITUtility.AssemblyVersion %>">

    function DeleteTemplate(obj, title) {
        if (confirm("Are you sure you want to delete the template \"" + title + "\"?")) {
            grid.PerformCallback("DELETE|" + obj);
        }
    }

    function OpenTicketTemplate() {

        $("#<%=txtTemplateName.ClientID%>").val('');
        ClientPopupControl.Show();
    }

    function SaveTemplate() {
        if ($("#<%=txtTemplateName.ClientID%>").val().length == 0) {
            alert("Template Name can't be empty.")
            return false;
        }
        //else {
        //    ClientPopupControl.Hide();
        //}

        if (ASPxClientEdit.ValidateGroup('Save')) {
            ClientPopupControl.Hide();
            ClientPopupControl.PerformCallback();
        }
        return false;
    }

    function CancelTemplate() {
        ClientPopupControl.Hide();
        return false;
    }

    function OnEndPopupCallback(s, e) {

        if ($("#<%=hndTemplateId.ClientID%>").val() == "0") {
            if (confirm("Template \"" + $("#<%=txtTemplateName.ClientID%>").val() + "\" already exists. Do you want to overwrite?")) {
                ClientPopupControl.PerformCallback("overwrite");
            }
        }
        else {
            ClientPopupControl.Hide();
            window.parent.parent.UgitOpenPopupDialog("<%=SaveAsTemplateURL %>" + $("#<%=hndTemplateId.ClientID%>").val(), "", "New Template", "950px", "600px", "0", "<%= Uri.EscapeDataString(Request.Url.AbsolutePath)%>");
        }
        return false;
    }


    function MoveToProduction(obj) {
        var url = '<%=moveToProductionUrl%>';
        window.parent.UgitOpenPopupDialog(url, '', 'Migrate Change(s)', '300px', '150px', 0, escape("<%= Request.Url.AbsolutePath %>"));
    }

    function ddlmodule_Validation(s, e) {

    }
</script>


<script data-v="<%=UGITUtility.AssemblyVersion %>">
    function expandAllTask() {
        grid.ExpandAll();
    }
    function collapseAllTask() {
        grid.CollapseAll();
    }
</script>
<div class="fright">
    <asp:Button ID="btnMigrateQuickMacro" runat="server" Text="Migrate" OnClientClick="MoveToProduction(this)" Visible="false" />
</div>
<div class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap">
    <div class="ms-formtable accomp-popup">
        <div class="row" style="margin-top: 7px;" >
            <img src="/content/images/expand-all-new.png" title="Expand All" onclick="expandAllTask()" width="16" />
            <img onclick="collapseAllTask()" src="/content/images/collapse-all-new.png" title="Collapse All" width="16" />
        </div>
        <div class="row">
                <ugit:ASPxGridView ID="grid" runat="server" AutoGenerateColumns="False" SettingsText-CommandClearFilter=""
                OnDataBinding="grid_DataBinding" OnCustomCallback="grid_CustomCallback" CssClass="customgridview homeGrid"
                ClientInstanceName="grid" Width="100%" KeyFieldName="ID">
                <Columns>
                    <dx:GridViewDataTextColumn Caption="Module" GroupIndex="0" CellStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" FieldName="ModuleDescription" />
                    <dx:GridViewDataTextColumn Caption=" " FieldName="Title" Width="50px">
                        <DataItemTemplate>
                            <div>
                                <a id="aEdit" runat="server" href="" onload="aEdit_Load">
                                    <img id="Imgedit" style="width: 16px" runat="server" src="/Content/Images/editNewIcon.png" />
                                </a>
                                <a id="aDelete" runat="server" href="" onload="aDelete_Load">
                                    <img id="ImgPermission" runat="server" src="/Content/images/grayDelete.png" width="16" />
                                </a>
                                <input type="hidden" value='<%# Container.KeyValue %>' />
                            </div>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>

                    <dx:GridViewDataTextColumn Caption="Template Type" HeaderStyle-Font-Bold="true" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" FieldName="TemplateType" Width="130px">
                    </dx:GridViewDataTextColumn>

                    <dx:GridViewDataTextColumn FieldName="Title" Caption="Template Name" HeaderStyle-Font-Bold="true" CellStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                        <DataItemTemplate>
                            <a id="aTitle" runat="server" href="" onload="aTitle_Load"></a>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                </Columns>

                <SettingsBehavior AllowSelectByRowClick="false" AutoExpandAllGroups="true" />
                <SettingsPopup>
                    <HeaderFilter Height="200" />
                </SettingsPopup>
                <SettingsPager Position="TopAndBottom">
                    <PageSizeItemSettings Items="10, 15, 20, 25, 50, 75, 100" />
                </SettingsPager>
                <Settings VerticalScrollableHeight="520" GroupFormat="{1}" VerticalScrollBarStyle="Standard" VerticalScrollBarMode="Auto" />
                <ClientSideEvents />
                <Styles>
                    <Row CssClass="homeGrid_dataRow"></Row>
                    <Header CssClass="homeGrid_headerColumn"></Header>
                    <GroupRow Font-Bold="true" CssClass="homeGrid-groupRow"></GroupRow>
                </Styles>
            </ugit:ASPxGridView>
        </div>
        <dx:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel" Modal="True"></dx:ASPxLoadingPanel>
          
        <div class="row">
            <div  style="float: right; padding-top: 15px; padding-bottom:15px;">
                <a id="a1" runat="server" href="" onclick="OpenTicketTemplate()" class="primary-btn-link">
                    <img id="Img3" runat="server" src="/Content/Images/plus-symbol.png" />
                    <asp:Label ID="Label2" runat="server" Text="New Template" CssClass="phrasesAdd-label"></asp:Label>
                </a>

                <dx:ASPxPopupControl ID="PopupControl" runat="server" CloseAction="CloseButton" CssClass="aspxPopup"
                    PopupVerticalAlign="WindowCenter" PopupHorizontalAlign="WindowCenter" PopupElementID="gridTemplate"
                    ShowFooter="False" Width="300px" Height="150px" HeaderText="New Template" ClientInstanceName="ClientPopupControl" 
                    OnWindowCallback="PopupControl_WindowCallback">
                    <ContentCollection>
                        <dx:PopupControlContentControl ID="PopupControlContentControl" runat="server">
                            <div class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap noPadding">
                                <div class="ms-formtable accomp-popup ">
                                    <div class="row">
                                        <div class="ms-formlabel">
                                            <h3 class="ms-standardheader budget_fieldLabel">Name <b style="color: Red; float: right">*</b></h3>
                                        </div>
                                        <div class="ms-formbody accomp_inputField">
                                            <asp:TextBox ID="txtTemplateName" runat="server" />
                                            <asp:HiddenField ID="hndTemplateId" runat="server" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="ms-formlabel">
                                            <h3 class="ms-standardheader budget_fieldLabel">Module</h3>
                                        </div>
                                        <div class="ms-formbody accomp_inputField">
                                            <ugit:LookUpValueBox ID="ddlModule" runat="server" CssClass="lookupValueBox-dropown" Width="100%" FieldName="ModuleNameLookup"
                                                FilterExpression="EnableModule=1 and EnableQuick=1" />
                                        </div>
                                    </div>
                                    <div class="row addEditPopup-btnWrap">
                                         <dx:ASPxButton ID="btnCancel" runat="server" CssClass="secondary-cancelBtn" Text="Cancel">
                                            <ClientSideEvents Click="CancelTemplate" />
                                        </dx:ASPxButton>
                                        <dx:ASPxButton ID="btnSave" runat="server" CssClass="primary-blueBtn" Text="Save" ValidationGroup="Save">
                                            <ClientSideEvents Click="SaveTemplate" />
                                        </dx:ASPxButton>
                                    </div>
                                </div>
                            </div>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                    <ClientSideEvents EndCallback="OnEndPopupCallback" />
                </dx:ASPxPopupControl>
            </div>
        </div>
    </div>
</div>

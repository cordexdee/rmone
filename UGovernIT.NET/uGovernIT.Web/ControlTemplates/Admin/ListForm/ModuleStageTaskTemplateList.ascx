<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ModuleStageTaskTemplateList.ascx.cs" Inherits="uGovernIT.Web.ModuleStageTaskTemplateList" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script data-v="<%=UGITUtility.AssemblyVersion %>">
    function expandAllTask() {
        InsspGrid_Templates.ExpandAll();
    }
    function collapseAllTask() {
        InsspGrid_Templates.CollapseAll();
    }
</script>
<script data-v="<%=UGITUtility.AssemblyVersion %>">
    function UpdateGridHeight() {
        InsspGrid_Templates.SetHeight(0);
        var containerHeight = ASPxClientUtils.GetDocumentClientHeight();
        if (document.body.scrollHeight > containerHeight)
            containerHeight = document.body.scrollHeight;
        InsspGrid_Templates.SetHeight(containerHeight);
    }
    window.addEventListener('resize', function (evt) {
        if (!ASPxClientUtils.androidPlatform)
            return;
        var activeElement = document.activeElement;
        if (activeElement && (activeElement.tagName === "INPUT" || activeElement.tagName === "TEXTAREA") && activeElement.scrollIntoViewIfNeeded)
            window.setTimeout(function () { activeElement.scrollIntoViewIfNeeded(); }, 0);
    });
</script>
<div id="header" class="col-md-12 col-sm-12 col-xs-12">
    <div class="row">
        <div class="col-md-4 col-sm-4 col-xs-4 noPadding">
            <div class="ms-formtable accomp-popup ">
                <div class="ms-formlabel ">
                    <h3 class="standardheader budget_fieldLabel">Select Module</h3>
                </div>
                <div class=" ms-formbody accomp_inputField">
                    <asp:DropDownList ID="ddlModule" runat="server" AppendDataBoundItems="true" AutoPostBack="true" CssClass="itsmDropDownList aspxDropDownList"
                        OnSelectedIndexChanged="ddlModule_SelectedIndexChanged">
                    </asp:DropDownList>
                </div>
            </div>
        </div>
        <div class="col-md-8 col-sm-8 col-xs-8 noPadding">
            <div style="float: right; padding-top: 20px;">
                <a id="aAddItem_Top" runat="server" href="" class="primary-btn-link">
                    <img id="Img2" runat="server" src="/content/images/plus-symbol.png" />
                    <asp:Label ID="Label1" runat="server" Text="Add New Item" CssClass="phrasesAdd-label"></asp:Label>
                </a>
            </div>
        </div>
    </div>
</div>

<div id="content" class="col-md-12 col-sm-12 col-xs-12">
    <div class="row" style="margin-bottom:10px;">
        <div class="col-md-1 col-sm-1 col-xs-6 noPadding" style="margin-top: 7px">
            <img src="/content/images/expand-all-new.png" title="Expand All" onclick="expandAllTask()" width="16" />
            <img onclick="collapseAllTask()" src="/content/images/collapse-all-new.png" title="Collapse All" width="16" />
        </div>
         <div class="col-md-2 col-sm-2 col-xs-6 noPadding crm-checkWrap" style="margin-top:5px;float:right">
               <asp:CheckBox  ID="dxShowDeleted" Text="Show Disabled" runat="server" TextAlign="right" AutoPostBack="true" OnCheckedChanged="dxShowDeleted_CheckedChanged" style="float:right;"/>
        </div>
    </div>
    <div class="row">
        <ugit:ASPxGridView ID="spGrid_Templates" runat="server" EnableViewState="false" KeyFieldName="ID" ClientInstanceName="InsspGrid_Templates"
            OnHtmlRowCreated="spGrid_Templates_HtmlRowCreated" AutoGenerateColumns="false" Width="100%" CssClass="customgridview homeGrid"
            DisplayGroupFieldName="false" SettingsBehavior-AutoExpandAllGroups="true" AllowFiltering="true"
            AllowGroupCollapse="true" GroupFieldDisplayName="Stage" GroupField="ModuleStageName" HeaderStyle-CssClass="aa" AlternatingRowStyle-BackColor="WhiteSmoke"
            AllowGrouping="true">
            <SettingsAdaptivity AdaptivityMode="HideDataCells" AllowOnlyOneAdaptiveDetailExpanded="true"></SettingsAdaptivity>
            <Columns>
                <dx:GridViewDataColumn FieldName="ID" Visible="false"></dx:GridViewDataColumn>
                <dx:GridViewDataColumn>
                    <DataItemTemplate>
                        <dx:ASPxButton ID="aEdit" runat="server" RenderMode="Link" Image-Url="/Content/images/editNewIcon.png" Image-Width="16">
                        </dx:ASPxButton>
                    </DataItemTemplate>
                </dx:GridViewDataColumn>
                <dx:GridViewDataColumn FieldName="Title" Caption="Title">
                    <DataItemTemplate>
                        <dx:ASPxButton ID="lblTitle" runat="server" RenderMode="Link"></dx:ASPxButton>
                    </DataItemTemplate>
                </dx:GridViewDataColumn>
                <dx:GridViewDataColumn FieldName="AssignedTo" Caption="Assigned To"></dx:GridViewDataColumn>
                <dx:GridViewDataColumn FieldName="UserRoleType" Caption="User Role Type">
                    <DataItemTemplate>
                        <asp:Label ID="lblUserRoleType" runat="server" />
                    </DataItemTemplate>
                </dx:GridViewDataColumn>
                <dx:GridViewDataColumn FieldName="Body" Caption="Description"></dx:GridViewDataColumn>
                <dx:GridViewDataColumn FieldName="ModuleStageName" GroupIndex="0" Settings-AllowGroup="True" Visible="false"></dx:GridViewDataColumn>
            </Columns>
            <SettingsCommandButton>
                <ShowAdaptiveDetailButton ButtonType="Button" Styles-Style-CssClass="homeGrid_openBTn"></ShowAdaptiveDetailButton>
                <HideAdaptiveDetailButton ButtonType="Button" Styles-Style-CssClass="homeGrid_closeBTn"></HideAdaptiveDetailButton>
            </SettingsCommandButton>
            <Styles>
                <Row CssClass="homeGrid_dataRow"></Row>
                <Header CssClass=" homeGrid_headerColumn"></Header>
                <GroupRow CssClass="homeGrid-groupRow"></GroupRow>
            </Styles>
            <FormatConditions>
                <dx:GridViewFormatConditionHighlight FieldName="Title" Expression="[Deleted]=True" ApplyToRow="true" Format="Custom" RowStyle-CssClass="formatcolor"></dx:GridViewFormatConditionHighlight>
            </FormatConditions>
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
    <div class="row" style="padding-top:10px;">
        <a id="aAddItem" runat="server" href="" class="primary-btn-link">
            <img id="Img1" runat="server" src="/content/images/plus-symbol.png" />
            <asp:Label ID="LblAddItem" runat="server" Text="Add New Item" CssClass="phrasesAdd-label"></asp:Label>
        </a>
    </div>
</div>

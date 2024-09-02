<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DashboardQueryImport.ascx.cs" Inherits="uGovernIT.Web.DashboardQueryImport" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .ms-formbody {
        background: none repeat scroll 0 0 #E8EDED;
        border-top: 1px solid #A5A5A5;
        padding: 3px 6px 4px;
        vertical-align: top;
    }

    .ms-formlabel {
        text-align: right;
        width: 190px;
        vertical-align: top;
    }

    .ms-standardheader {
        text-align: right;
    }

    .fileUploadCss {
        width: 320px;
        height: 22px;
        margin: 0;
        border: 1px solid #777;
        float: left;
        -webkit-box-sizing: border-box;
        -moz-box-sizing: border-box;
    }

    input[type="file"].fileUploadCss::-webkit-file-upload-button {
        float: right;
        position: relative;
        top: -1px;
        right: -1px;
    }

    .gridheader {
        height: 20px;
        background-color: #CED8D9;
        text-align: left;
        font-weight: normal;
        border: 1px solid black;
    }

    .gridRow {
        height: 20px;
        text-align: left;
        font-weight: normal;
        border: 1px solid black;
    }
</style>
<script data-v="<%=UGITUtility.AssemblyVersion %>">
    $(function () {
        $('input[type="file"]').change(function (e) {
            var show = false;
            var message = '';
            $.each(e.target.files, function (index, value) {
                if (value.name.endsWith('.zip') || !value.name.endsWith('.xml')) {
                    show = true;
                    return;
                }
            });
            if (show) {
                alert('Only .xml file allowed.');
                return false;
            }

        });
    });

</script>
<dx:ASPxLoadingPanel ID="ldnPanelImportDashboard" ClientInstanceName="ldnPanelImportDashboard" runat="server" Modal="true" Text="Please wait..."></dx:ASPxLoadingPanel>
<div style="float: right; width: 98%; padding-left: 10px;">
    <table class="ms-formtable" cellpadding="0" cellspacing="0" style="border-collapse: collapse" width="100%">

        <%--<tr id="tr3" runat="server">
            <td class="ms-formlabel">
                <h3 class="ms-standardheader">Selecti
                </h3>
            </td>
            <td class="ms-formbody">
                <asp:RadioButtonList ID="rdSelectionType" runat="server" AutoPostBack="true">
                    <asp:ListItem Text="Single" Value="single"></asp:ListItem>
                    <asp:ListItem Text="Multiple" Value="multiple"></asp:ListItem>
                </asp:RadioButtonList>
            </td>

        </tr>--%>
        <tr>
            <td colspan="2">
                <asp:Label ID="lblErrorMessage" runat="server" Text="Please select a file to import." Visible="false" Style="color: red; font-weight: bold;"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="ms-formlabel">
                <h3 class="ms-standardheader">Select File
                </h3>
            </td>
            <td class="ms-formbody">
                <div style="display: block; padding-bottom: 10px;">
                    <input type="file" name="myFiles" accept=".xml" runat="server" id="dashboardFiles" size="36" multiple="multiple" />
                </div>
                <div style="display: block;">
                    <dx:ASPxButton ID="btImportFiles" runat="server" ImagePosition="Right" ToolTip="Upload Dashboard/Query" Text="Upload" CssClass="primary-blueBtn" OnClick="btImportFiles_Click">
                        <%--<Image Url="/content/images/file-upload.png" ></Image>--%>
                    </dx:ASPxButton>
                </div>
            </td>
        </tr>
        <tr id="trServices" runat="server" style="visibility: hidden">
            <td class="ms-formlabel">
                <h3 class="ms-standardheader">Uploaded Files
                </h3>
            </td>
            <td class="ms-formbody">
                <asp:GridView ID="gvDashboardQuery" runat="server" Width="100%" AlternatingRowStyle-BackColor="WhiteSmoke" Style="padding-top: 10px;"
                    HeaderStyle-Height="20px" HeaderStyle-CssClass="gridheader" RowStyle-CssClass="gridRow" HeaderStyle-Font-Bold="false" AutoGenerateColumns="false"
                    GridLines="Both">
                    <Columns>
                        <asp:TemplateField HeaderStyle-Height="2" HeaderText="Files">
                            <ItemTemplate>
                                <asp:Label ID="lblDashboardText" runat="server" Text="<%#Container.DataItem %>"></asp:Label>
                            </ItemTemplate>

                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <dx:ASPxButton ID="btnCreateDashboardQuery" runat="server" ImagePosition="Right" ToolTip="Create Dashboard/Query" Text="Create" CssClass="primary-blueBtn" OnClick="btnCreateDashboardQuery_Click" style="float: right;">
                        <%--<Image Url="/content/images/add_icon.png" ></Image>--%>
                    <ClientSideEvents Click="function(s,e){ ldnPanelImportDashboard.Show();}" />
                </dx:ASPxButton>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:HiddenField ID="hdnDashboardQueryExistForModule" runat="server" Value="false" />
                <asp:HiddenField ID="hdnDashboardQueryExists" runat="server" Value="false" />
                <asp:Label ID="lblErrors" runat="server" Text="The following error(s) exist in the uploaded file. Are you sure you want to proceed?" Visible="false" Style="font-weight: bold; padding-left: 5px;"></asp:Label>
                <asp:Panel ID="pnlErrorSection" runat="server" Style="background: none repeat scroll 0 0 #E8EDED; font-weight: bold; border: 1px black; padding: 5px;" Visible="false">
                    <div id="divError" runat="server" width="100%"></div>
                    <div id="divButtons" style="height: 35px;" runat="server">
                        <dx:ASPxButton ID="btNewService" runat="server" ImagePosition="Right" ToolTip="Proceed To Create" Text="Proceed To Create" CssClass="primary-blueBtn" OnClick="btnCreateDashboardQuery_Click" style="float: right;">
                        </dx:ASPxButton>
                        <dx:ASPxButton ID="btAbort" runat="server" ImagePosition="Right" ToolTip="Abort" Text="Abort" CssClass="primary-blueBtn" OnClick="btnCreateDashboardQuery_Click" style="float: right;">
                        </dx:ASPxButton>
                    </div>
                </asp:Panel>
            </td>
        </tr>
    </table>
</div>

<%--Popup to show Missing columns from Imported script for particular Modules--%>
<dx:ASPxPopupControl ID="missingColumnsContainer" runat="server" AllowDragging="false" ClientInstanceName="missingColumnsContainer" CloseAction="CloseButton" ShowCloseButton="true"
    ShowFooter="false" ShowHeader="true" HeaderText="Columns Missing in Request Lists" PopupVerticalAlign="Middle" PopupHorizontalAlign="WindowCenter" EnableViewState="false" EnableHierarchyRecreation="True"
    CssClass="context-popup-grid" Width="400" MinHeight="200" MaxHeight="400">
    <HeaderStyle Font-Bold="true" ForeColor="Red" />
    <ClientSideEvents CloseUp="function(s, e) { ClosePopup(); }" />
    <ContentCollection>
        <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
            <div class="main-table">
                <p>The following columns from the query script are missing in Request Lists.</p>
            </div>
            <dx:ASPxGridView ID="missingColumnsGrid" runat="server" OnDataBinding="missingColumnsGrid_DataBinding" AutoGenerateColumns="false"
                Width="100%" ClientInstanceName="missingColumnsGrid" EnableViewState="false">
                <Columns>
                    <dx:GridViewDataTextColumn FieldName="#" Width="30%" Caption="#" HeaderStyle-Font-Bold="true"
                        CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="ColumnName" Width="70%" Caption="Columns" HeaderStyle-Font-Bold="true"
                        CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                    </dx:GridViewDataTextColumn>
                </Columns>
                <Settings ShowHeaderFilterButton="false" ShowFooter="false" EnableFilterControlPopupMenuScrolling="false" VerticalScrollBarMode="Auto"
                    VerticalScrollableHeight="300" />
                <SettingsPopup>
                    <HeaderFilter Height="200" />
                </SettingsPopup>
                <SettingsBehavior AllowSort="false" EnableRowHotTrack="false" />
                <SettingsPager Mode="ShowAllRecords" />
                <SettingsDataSecurity AllowInsert="false" AllowEdit="false" AllowDelete="false" />
                <Styles>
                    <Header Font-Bold="true"></Header>
                    <AlternatingRow Enabled="true"></AlternatingRow>
                    <Row Wrap="True"></Row>
                </Styles>
            </dx:ASPxGridView>
            <div class="addEditPopup-btnWrap" style="padding-bottom: 2px !important;">
                <dx:ASPxButton ID="lnkBtnClose" runat="server" Text="Close" ToolTip="Close" OnClick="btnClose_Click" 
                    CssClass="secondary-cancelBtn" Style="float: right;">
                 </dx:ASPxButton>
            </div>
        </dx:PopupControlContentControl>
    </ContentCollection>

</dx:ASPxPopupControl>

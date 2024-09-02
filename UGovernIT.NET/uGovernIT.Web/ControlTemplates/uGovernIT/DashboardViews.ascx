<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DashboardViews.ascx.cs" Inherits="uGovernIT.Web.DashboardViews" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function createNewDashboardView(obj) {
        var type = $("#viewTypes").val();

        var txtType = $("#viewTypes option:selected").text();

        if (txtType == 'Common Dashboards') {
            viewEditPanel.Show();
        }
        else {

            var fullUrl = "<%= delegateUrl %>" + "0&viewType=" + type;;
            window.parent.UgitOpenPopupDialog(fullUrl, "", txtType, '70', '60', 0, escape("<%= Request.Url.AbsolutePath %>"));
        }
    }

    var isValid = false;

    function btnSaveView_Click(s, e) {
        if (!isValid) {
            $('[id$="hndViewType"]').val($("#viewTypes option:selected").text());
            cbCheckText.PerformCallback(txtViewName.GetText());
            e.processOnServer = false;
        }
        else {
            e.processOnServer = true;
            viewEditPanel.Hide();
        }
    }

    function cbCheckText_CallbackComplete(s, e) {
        if (e.result == "Valid") {
            isValid = true;
            btnSaveView.DoClick();
        }
        else {
            txtViewName.SetIsValid(false);
        }
    }
</script>

<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .ms-formbody {
        background: none repeat scroll 0 0 #E8EDED;
        border-top: 1px solid #A5A5A5;
        padding: 3px 6px 0;
        vertical-align: top;
    }

    .ms-formlabel {
        text-align: right;
        width: 250px;
        vertical-align: top;
    }

    .ms-standardheader {
        text-align: right;
    }

    .ms-long {
        font-family: Verdana,sans-serif;
        font-size: 8pt;
        width: 386px;
    }

    tr.alternet {
        background-color: whitesmoke;
    }
</style>

<div class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap">
    <div class="row">
        <ugit:ASPxGridView ID="gridDashbordViews" runat="server" AutoGenerateColumns="False"
            OnDataBinding="gridDashbordViews_DataBinding" ClientInstanceName="gridAllocation" 
            CssClass="customgridview homeGrid"
            Width="100%" KeyFieldName="ID" OnHtmlRowPrepared="gridDashbordViews_HtmlRowPrepared"
            SettingsText-EmptyDataRow="No Dashboard Views" Settings-VerticalScrollBarMode="Auto" Settings-VerticalScrollableHeight="380">
                <Columns>
                    <dx:GridViewDataTextColumn FieldName="ViewType" Caption="View" GroupIndex="0"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="ViewName" Caption="Title" ReadOnly="false" PropertiesTextEdit-NullText="Add New View" EditFormSettings-CaptionLocation="None" EditFormSettings-RowSpan="2" EditFormSettings-Caption="" Width="90%"></dx:GridViewDataTextColumn>
                </Columns>
                <Styles AlternatingRow-CssClass="ms-alternatingstrong">
                    <GroupRow CssClass="homeGrid-groupRow" Font-Bold="true"></GroupRow>
                    <Header CssClass="homeGrid_headerColumn"></Header>
                    <Row CssClass="homeGrid_dataRow"></Row>
                </Styles>
                <SettingsBehavior AutoExpandAllGroups="true" />
                <Settings GroupFormat="{1}" ShowFilterRowMenu="true" />
		<SettingsPager Mode="ShowAllRecords"></SettingsPager>
        </ugit:ASPxGridView>
    </div>
    <div class="row addEditPopup-btnWrap">
        <dx:ASPxButton ID="btnNew" runat="server" AutoPostBack="false" ImagePosition="Left" Text="New" CssClass="primary-blueBtn">
            <Image Url="/content/images/add_icon.png"></Image>
            <ClientSideEvents Click="function(s,e){createNewDashboardView(s);}" />
        </dx:ASPxButton>
        <select id="viewTypes" style="height: 28px" class="selectDropDown">
            <option value="Indivisible Dashboards">Indivisible Dashboards View</option>
            <option value="Super Dashboards">Super Dashboards View</option>
            <option value="Side Dashboards">Side Dashboards</option>
            <option value="Common Dashboards">Common Dashboards</option>
        </select>
    </div>
</div>
<div class="col-md-12 col-sm-12 col-xs-12 noPadding">
     <dx:ASPxPopupControl ID="viewEditPanel" runat="server" CloseAction="OuterMouseClick"
        PopupVerticalAlign="WindowCenter" PopupHorizontalAlign="WindowCenter" CssClass="aspxPopup d-block"
        ShowFooter="false" Width="350px" Height="120px" HeaderText="Dashboard View" ClientInstanceName="viewEditPanel">
        <ContentCollection>
            <dx:PopupControlContentControl ID="viewEditPanelContentControl" runat="server">
                <div class="col-md-12 col-sm-12 col-xs-12 noPadding ms-formtable accomp-popup">
                    <div class="d-flex align-items-end">
                        <div>
                            <div class="ms-formlabel">
                                <h3 class=" ms-standardheader budget_fieldLabel">
                                    View Name<b style="color: Red;">*</b>
                                </h3>
                            </div>
                            <div class="ms-formbody accomp_inputField mb-0">
                                <dx:ASPxTextBox ID="txtViewName" runat="server" CssClass="asptextBox-input" Width="100%" ClientInstanceName="txtViewName">
                                    <ValidationSettings EnableCustomValidation="True" ErrorDisplayMode="ImageWithTooltip"
                                        ErrorText="View Name Is Duplicate">
                                        <RequiredField IsRequired="true" ErrorText="View Name Is Required" />
                                    </ValidationSettings>
                                </dx:ASPxTextBox>
                                <asp:HiddenField ID="hndViewType" runat="server" Value="0" />
                                <dx:ASPxCallback ID="cbCheckText" ClientInstanceName="cbCheckText" runat="server" OnCallback="cbCheckText_Callback">
                                    <ClientSideEvents CallbackComplete="cbCheckText_CallbackComplete" />
                                </dx:ASPxCallback>
                            </div>
                        </div>
                        <div>
                            <dx:ASPxButton ID="btnSaveView" ClientInstanceName="btnSaveView" CssClass="primary-blueBtn" 
                                runat="server" Text="Save"
                                ClientSideEvents-Click="btnSaveView_Click">
                            </dx:ASPxButton>
                        </div>
                    </div>
                </div>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
</div>




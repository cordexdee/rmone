
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TicketActualHoursView.ascx.cs" Inherits="uGovernIT.Web.TicketActualHoursView" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script data-v="<%=UGITUtility.AssemblyVersion %>">
    function onBatchEditEndEditing(s, e) {
        try {
            var totalActionHours = s.cptotalActualHours;
            window.updateActualHours(totalActionHours);
        } catch (ex) { }


        if (aspxGridClientInstance.cpDisableAddButton)
            btAdd.SetEnabled(false);
    }
    function DeleteRecord(s, e) {
        UpdateRecords(s, e);

        if (!aspxGridClientInstance.InCallback())
            btAdd.SetEnabled(true);

    }
    function InsertRecord() {
        setTimeout(function () { aspxGridClientInstance.UpdateEdit(); }, 100);
    }

    function UpdateRecords(s, e) {
        setTimeout(function () { aspxGridClientInstance.UpdateEdit(); }, 100);
    }
    function SaveRecordChanges(s, e) {
        alert('save changes');
    }

    function StopInlineEditing(s, e) {
       
        if (e.visibleIndex == -1)
            e.focusedColumn = s.GetColumnByField("HoursTaken");
        <%if (HideActions)
    {%>

        e.cancel = true;
        <%}%>
    }

    function btAddItem(s, e) {
        s.SetEnabled(false);
        aspxGridClientInstance.AddNewRow();
    }

</script>

<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .divAddWidth {
        width: 32px !important;
    }

    .clsHide {
        display: none;
    }
</style>

<table style="width: 100%; border-collapse: collapse;" cellspacing="0" cellpadding="0">
    <tr>
        <td>
            <div class="fullwidth">
                <ugit:ASPxGridView ID="aspxGrid" runat="server" AutoGenerateColumns="False" OnCommandButtonInitialize="aspxGrid_CommandButtonInitialize" Images-HeaderActiveFilter-Url="/Content/images/Filter_Red_16.png"
                    OnDataBinding="aspxGrid_DataBinding" EnableCallBacks="true" OnCellEditorInitialize="aspxGrid_CellEditorInitialize" OnBatchUpdate="aspxGrid_BatchUpdate" OnRowValidating="aspxGrid_RowValidating" ClientInstanceName="aspxGridClientInstance"
                    Width="100%" OnInitNewRow="aspxGrid_InitNewRow" OnRowDeleting="aspxGrid_RowDeleting" SettingsText-EmptyHeaders="&nbsp;" KeyFieldName="ID" CssClass="customgridview">
                    <Columns>
                        <dx:GridViewDataDateColumn Width="15%" Caption="Date" Settings-ShowEditorInBatchEditMode="true" FieldName="WorkDate" HeaderStyle-Font-Bold="true" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"></dx:GridViewDataDateColumn>
                        <dx:GridViewDataTextColumn Width="20%" Settings-ShowEditorInBatchEditMode="false" Caption="User" FieldName="ResourceUser" HeaderStyle-Font-Bold="true" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"></dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Width="15%" Caption="Time Spent (hrs)" Settings-ShowEditorInBatchEditMode="true" FieldName="HoursTaken" HeaderStyle-Font-Bold="true" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"></dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Width="50%" Caption="Comment" Settings-ShowEditorInBatchEditMode="true" FieldName="Comment" HeaderStyle-Font-Bold="true" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"></dx:GridViewDataTextColumn>
                        <dx:GridViewCommandColumn ShowNewButton="false" Name="CommandColumn" ShowDeleteButton="True" Width="100" ShowCancelButton="false" ShowUpdateButton="false">
                            <FooterTemplate>
                                <dx:ASPxButton Text="Add" ClientInstanceName="btAdd" CssClass="primary-blueBtn" HoverStyle-CssClass="ugit-buttonhover" DisabledStyle-CssClass="ugit-button-disable"
                                    Height="23" AutoPostBack="false" ID="btAdd" runat="server" ImagePosition="Right">
                                    <Image Url="/Content/images/add_icon.png" AlternateText="Add"></Image>
                                    <ClientSideEvents Click="function(s,e){btAddItem(s,e);}" />
                                </dx:ASPxButton>
                            </FooterTemplate>
                        </dx:GridViewCommandColumn>
                    </Columns>
                    <SettingsCommandButton>
                        <NewButton RenderMode="Image" Image-Url="/Content/images/add_icon.png"></NewButton>
                        <DeleteButton RenderMode="Image" Image-Url="/Content/images/delete-iconOld.png"></DeleteButton>
                        <UpdateButton Styles-Style-CssClass="clsHide" RenderMode="Image" Image-Url="/Content/images/save-icon.png"></UpdateButton>
                        <CancelButton Styles-Style-CssClass="clsHide" RenderMode="Image" Image-Url="/Content/images/cancel-icon.png"></CancelButton>
                    </SettingsCommandButton>

                    <TotalSummary>
                        <dx:ASPxSummaryItem FieldName="HoursTaken" SummaryType="Sum" ShowInColumn="HoursTaken" DisplayFormat="{0}" />
                        <dx:ASPxSummaryItem FieldName="WorkDate" SummaryType="Count" ShowInColumn="WorkDate" DisplayFormat="Total:" />
                    </TotalSummary>
                    <SettingsEditing Mode="Batch" NewItemRowPosition="Bottom" BatchEditSettings-EditMode="Row" BatchEditSettings-StartEditAction="Click" />
                    <SettingsText ConfirmDelete="Do you want to delete entry?" />
                    <SettingsPopup>
                        <HeaderFilter Height="200" />
                    </SettingsPopup>
                    <Settings ShowHeaderFilterButton="true" ShowFooter="true" VerticalScrollBarMode="auto" VerticalScrollableHeight="200" />
                    <SettingsPager Mode="ShowAllRecords" />
                    <Styles>
                        <Footer Font-Bold="true" HorizontalAlign="Center"></Footer>
                    </Styles>
                    <SettingsBehavior AllowSort="true" ConfirmDelete="true" />
                    <ClientSideEvents BatchEditStartEditing="function(s,e){StopInlineEditing(s,e);}" EndCallback="function(s,e){onBatchEditEndEditing(s,e)}" BatchEditRowDeleting="function(s,e){DeleteRecord(s,e);}" BatchEditEndEditing="function(s,e){UpdateRecords(s,e);}" />
                </ugit:ASPxGridView>
            </div>
        </td>
    </tr>
</table>

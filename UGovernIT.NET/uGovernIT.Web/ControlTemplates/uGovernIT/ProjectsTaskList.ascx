<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProjectsTaskList.ascx.cs" Inherits="uGovernIT.Web.ProjectsTaskList" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .radiobutton label {
        margin-left: 10px;
    }
</style>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function OnbtnEmailAlertClick(s, e) {
        grid.GetSelectedFieldValues('Id', OnGetSelectedFieldValuesForEmailAlert);
    }

    function OnGetSelectedFieldValuesForEmailAlert(selectedValues) {
        if (selectedValues.length < 1) {
            alert("Please select at least one item.");
            return;
        }
        var param = "tickettaskIds=" + selectedValues + "&EmailAlert=True&AlertType=SVCTask&ModuleName=SVC";
        window.parent.UgitOpenPopupDialog('<%= TaskAlertUrl %>', param, 'Task Alert', '700px', '600px', 0, escape("<%= Request.Url.AbsolutePath %>"));

        return false;
    }

    function openTicketDialog(params, titleVal) {
        window.parent.UgitOpenPopupDialog('<%= ticketurl %>', "TicketId=" + params, titleVal, 90, 80, 0, escape("<%= Request.Url.AbsolutePath %>"));
        return false;
    }

    function showLoader() {
        LoadingPanel.SetText('Loading ...');
        LoadingPanel.Show();
    }
</script>

<dx:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel" Modal="True"></dx:ASPxLoadingPanel>

<div style="float: left; padding-bottom: 4px;">
    <div style="border: solid; border-width: 1px; border-color: #D9DAE0; padding-right: 10px; height: 20px; padding-top: 4px;">
        <asp:RadioButton ID="rbtnAllTasks" runat="server" AutoPostBack="true" Text="All Open Tasks" Checked="true" CssClass="radiobutton" TextAlign="Left" onchange="showLoader()" GroupName="Tasks" OnCheckedChanged="rbtnAllTasks_CheckedChanged" />
        <asp:RadioButton ID="rbtnMyTasks" runat="server" AutoPostBack="true" Text="My Open Tasks" CssClass="radiobutton" TextAlign="Left" onchange="showLoader()" GroupName="Tasks" OnCheckedChanged="rbtnMyTasks_CheckedChanged" />
        <asp:RadioButton ID="rbtnMyGroupTasks" runat="server" AutoPostBack="true" Text="My Group Tasks" CssClass="radiobutton" TextAlign="Left" onchange="showLoader()" GroupName="Tasks" OnCheckedChanged="rbtnMyGroupTasks_CheckedChanged" />
    </div>
</div>

<div style="float: left; width: 100%;">
    <table class="ms-formtable" cellpadding="0" cellspacing="0" style="border-collapse: collapse" width="100%">
        <tr>
            <td>
                <dx:ASPxGridView ID="grid" ClientInstanceName="grid" runat="server" Styles-Header-Font-Bold="true" Settings-GroupFormat="{1}"
                    KeyFieldName="Id" OnCustomCallback="grid_CustomCallback" Width="100%">
                    <Columns>
                        <dx:GridViewCommandColumn ShowSelectCheckbox="true" Caption=" " Width="30px" VisibleIndex="0" SelectAllCheckboxMode="Page">
                        </dx:GridViewCommandColumn>
                        <dx:GridViewDataColumn FieldName="ParentTicketId" Caption="Request Id" Visible="false">
                            <Settings HeaderFilterMode="CheckedList" />
                        </dx:GridViewDataColumn>
                        <dx:GridViewDataTextColumn FieldName="TicketTitle" Caption="Ticket" GroupIndex="0" PropertiesTextEdit-EncodeHtml="false" CellStyle-Font-Bold="true">
                            <Settings HeaderFilterMode="CheckedList" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataColumn FieldName="ItemOrder" Caption="#" Width="45px">
                            <Settings HeaderFilterMode="CheckedList" />
                        </dx:GridViewDataColumn>
                        <dx:GridViewDataColumn FieldName="TaskType" Caption="Type" Width="65px">
                            <Settings HeaderFilterMode="CheckedList" />
                        </dx:GridViewDataColumn>
                        <dx:GridViewDataColumn FieldName="Title" Caption="Title">
                            <Settings HeaderFilterMode="CheckedList" />
                        </dx:GridViewDataColumn>
                        <dx:GridViewDataColumn FieldName="ChildTicketID" Caption="Ticket Id" Width="120px">
                            <Settings HeaderFilterMode="CheckedList" />
                        </dx:GridViewDataColumn>
                        <dx:GridViewDataColumn FieldName="Status" Caption="Status">
                            <Settings HeaderFilterMode="CheckedList" />
                        </dx:GridViewDataColumn>
                        <dx:GridViewDataColumn FieldName="AssignedToUser" Caption="Assigned To">
                            <Settings HeaderFilterMode="CheckedList" />
                        </dx:GridViewDataColumn>
                        <dx:GridViewDataColumn FieldName="Predecessors" Width="70px">
                            <Settings AllowAutoFilter="False" AllowSort="False" AllowHeaderFilter="False" />
                        </dx:GridViewDataColumn>
                        <dx:GridViewDataColumn FieldName="DueDate" Caption="Due Date" Width="85px">
                            <Settings HeaderFilterMode="CheckedList" />
                        </dx:GridViewDataColumn>
                    </Columns>
                    <Settings ShowGroupPanel="true" ShowHeaderFilterButton="true" ShowFilterBar="Visible" />
                    <SettingsPager Position="TopAndBottom" AlwaysShowPager="true" Mode="ShowPager" PageSize="25">
                        <PageSizeItemSettings Items="15,20,25,30,50,75,100,200,500,1000" Visible="true" Position="Right"></PageSizeItemSettings>
                    </SettingsPager>
                    <SettingsBehavior AutoExpandAllGroups="true" />

                    <Styles>
                        <GroupRow Font-Bold="true"></GroupRow>
                    </Styles>
                </dx:ASPxGridView>
            </td>
        </tr>

        <tr>
            <td>
                <dx:ASPxButton ID="btnEmailAlert" runat="server" ImagePosition="Right" Visible="true" Text="Alert" Height="25px" AutoPostBack="false" Style="margin-top: 5px;">
                    <Image Url="/Content/images/MailTo16X16.png"></Image>
                    <ClientSideEvents Click="OnbtnEmailAlertClick" />
                </dx:ASPxButton>
            </td>
        </tr>
    </table>
</div>
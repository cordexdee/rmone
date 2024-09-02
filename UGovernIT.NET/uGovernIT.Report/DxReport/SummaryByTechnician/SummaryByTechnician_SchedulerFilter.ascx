<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SummaryByTechnician_SchedulerFilter.ascx.cs" Inherits="uGovernIT.Report.DxReport.SummaryByTechnician_SchedulerFilter" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<style type="text/css">
#wrapper_1 { clear: left; float: left; position: relative; left: 50%; }
#container_1 { display: block; float: left; position: relative; right: 50%; }
 .primary-blueBtn {
        background: none;
        border: none;
    }

        .primary-blueBtn .dxb {
            background: #4A6EE2;
            color: #FFF;
            border: none;
            border-radius: 4px;
            padding: 3px 13px 2px 13px !important;
            font-size: 12px;
            font-family: 'Poppins', sans-serif;
            font-weight: 500;
        }

    .secondary-cancelBtn {
        background: none;
        border: 1px solid #4A6EE2;
        border-radius: 4px;
        margin-right: 5px;
        margin-top: 1px;
    }

    .dxbButton_UGITNavyBlueDevEx.secondary-cancelBtn .dxb {
        padding: 0px 10px;
        color: #4A6EE2;
        font-size: 12px;
        font-family: 'Poppins', sans-serif;
        font-weight: 500;
    }

    .addEditPopup-btnWrap {
        float: right;
        padding: 10px 0px 20px;
        text-align: right;
    }
</style>
<script>
    $(document).ready(function () {
        var moduleName="<%=ModuleName%>";
        $("#<%=cblSmryByTechModule.ClientID%> input:checkbox[value=" + moduleName + "]").prop('checked', true);
        $("#<%=chkSmryByTechSelectAll.ClientID%>").click(function (event) {
            if (this.checked) {
                $("[id*=cblSmryByTechModule] input:checkbox").prop('checked', true);
                $("#<%=chkSmryByTechSortByModule.ClientID%>").parent().css("display", "block");
                $("#<%=chkSmryByTechSortByModule.ClientID%>").attr('checked', 'checked');
            }
            else {
                $("[id*=cblSmryByTechModule] input:checkbox").prop('checked', false);
                $("#<%=chkSmryByTechSortByModule.ClientID%>").parent().css("display", "none");
                $("#<%=chkSmryByTechSortByModule.ClientID%>").removeAttr('checked');
            }
        });
        $('[id*=cblSmryByTechModule] input[type=checkbox]').change(function () {
            if ($('#<%=cblSmryByTechModule.ClientID%> :checkbox:checked').length > 1) {
                $("#<%=chkSmryByTechSortByModule.ClientID%>").parent().css("display", "block");
                $("#<%=chkSmryByTechSortByModule.ClientID%>").attr('checked', 'checked');
            }
            else {
                $("#<%=chkSmryByTechSortByModule.ClientID%>").parent().css("display", "none");
                $("#<%=chkSmryByTechSortByModule.ClientID%>").removeAttr('checked');

                $("#<%=chkSmryByTechSelectAll.ClientID%>").removeAttr('checked');
            }

            if ($('#<%=cblSmryByTechModule.ClientID%> :checkbox:checked').length != this.length) {
                $("#<%=chkSmryByTechSelectAll.ClientID%>").removeAttr('checked');
            }
        });
    });
    function HideTicketsReportByPRPPopup() {
        var popupCtrl = ASPxClientControl.GetControlCollection().GetByName('<%=PopID%>');
        if (popupCtrl != undefined)
            popupCtrl.Hide();
    }
    function GetTicketsReportByPRPPopup(obj) {
        var params = "";
        var selectedModules = [];
        $("[id*=<%=cblSmryByTechModule.ClientID%>] input:checked").each(function () {
            selectedModules.push($(this).val());
        });

        var byCategory = chkGroupByOption.GetValue();
        var selectedColumns = cblIncludeColumns.GetSelectedValues().join(",")
        if (selectedColumns.length == 0) {
            alert("Please include at least one count.")
        }
        else {
            HideTicketsReportByPRPPopup();
            var datecontrolFrom = dtcSummaryStartDate.GetDate();
            var datecontrolTo = dtcSummaryEndDate.GetDate(); 
            var myDateFrom = (datecontrolFrom.getMonth() + 1) + "/" + datecontrolFrom.getDate() + "/" + datecontrolFrom.getFullYear();
            var myDateTo = (datecontrolTo.getMonth() + 1) + "/" + datecontrolTo.getDate() + "/" + datecontrolTo.getFullYear();//$("#<%=dtcSummaryStartDate.ClientID %>_dtcSummaryStartDateDate").val();
           //$("#<%=dtcSummaryEndDate.ClientID %>_dtcSummaryEndDateDate").val();
            var includeTechnician = chkbxIncludeTechnician.GetValue();
            var selectedITManagers = "all";
            if (glITManagers.GetValue() != null)
                selectedITManagers = glITManagers.GetValue().join(',');
            var isModuleSort = $("#<%=chkSmryByTechSortByModule.ClientID%>").is(':checked');
            var url = '<%=delegateControl%>' + "?control=TicketReportByPRPViewer&SelectedModule=" + selectedModules.join(",") + "&byCategory=" + byCategory + "&IncludeCounts=" + selectedColumns + "&DateFrom=" + myDateFrom + "&DateTo=" + myDateTo + "&includeTechnician=" + includeTechnician + "&selectedITManagers=" + selectedITManagers + "&isModuleSort=" + isModuleSort;
            var popupHeader = "Ticket Summary By Technician";
            // window.UgitOpenPopupDialog(url, params, popupHeader, '90', '90', 0, escape("<%= Request.Url.AbsolutePath %>"));
            $(location).attr("href", url);    
        }
        return false;
    }
    function SetFiltersForSchedule() {
        cbpnlMain.PerformCallback();
        return false;
    }
</script>
<dx:ASPxCallbackPanel ID="cbpnlMain" OnCallback="cbpnlMain_Callback" ClientInstanceName="cbpnlMain" runat="server">
    <ClientSideEvents EndCallback="function(s,e){HideTicketsReportByPRPPopup();}" />
    <PanelCollection>
        <dx:PanelContent>
            <fieldset>
                <legend>Summary By Technician</legend>
                <div id="wrapper_1">
        <div id="container_1">
            <table style="padding-top: 10px;">
                    <tr>
                        <td>
                            <asp:Label ID="Label7" runat="server" Text="Modules" Font-Bold="true"> </asp:Label>
                        </td>
                        <td style="margin: 5px 0 5px 8px;">
                            <!-- select all boxes -->

                            <div style="overflow: auto; max-height: 150px; border: 1px solid black">
                                <asp:CheckBox ID="chkSmryByTechSelectAll" runat="server" Checked="false" Text="Select All" Style="margin-left: 3px; font-weight: 600;" />
                                <asp:CheckBoxList ID="cblSmryByTechModule" runat="server" RepeatDirection="Vertical"></asp:CheckBoxList>
                            </div>
                            <asp:CheckBox ID="chkSmryByTechSortByModule" runat="server" Checked="false" Style="display: none; margin-left: 3px; font-weight: 600;" Text="Sort By Module First" />
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-top: 10px">
                            <asp:Label ID="Label3" runat="server" Text="Group By Category" Font-Bold="true"> </asp:Label>
                        </td>
                        <td style="padding-top: 10px">
                            <div style="float: left">
                                <dx:ASPxCheckBox ID="chkGroupByOption" ClientInstanceName="chkGroupByOption" runat="server" CssClass="rdFilterCriteria">
                                </dx:ASPxCheckBox>
                            </div>

                            <div style="float: left; padding-left: 20px">
                                <asp:Label ID="Label5" runat="server" Text="Include ORP" Font-Bold="true"> </asp:Label>
                                <dx:ASPxCheckBox ClientInstanceName="chkbxIncludeTechnician" ID="chkbxIncludeTechnician" runat="server" Checked="false" CssClass="rdFilterCriteria">
                                </dx:ASPxCheckBox>
                            </div>
                        </td>
                    </tr>
                    <tr style="display: none">
                        <td style="padding-top: 5px;"></td>
                        <td style="padding-top: 5px;"></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label4" runat="server" Text="Include Counts" Font-Bold="true"> </asp:Label>
                        </td>
                        <td style="padding-left: 3px;">

                            <dx:ASPxCheckBoxList ID="cblIncludeColumns" ClientInstanceName="cblIncludeColumns" runat="server" RepeatDirection="Horizontal" CssClass="cblIncludeColumns">
                            </dx:ASPxCheckBoxList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label6" runat="server" Text="Date Range" Font-Bold="true"> </asp:Label></td>
                        <td>
                            <div class="top_right_nav" style="left: 0px; float: left; margin-top: 6px; width: 100%;">
                                <span id="spanNoOfDays" runat="server" visible="false" style="float: left; width: 100%; margin: 5px; margin-left: 0px">
                                   
                                    <span style="float: left;padding-top:5px">From:</span>
                                    <span style="float: left"> <dx:ASPxSpinEdit ID="txtValueFrom"  MaxValue="0" MinValue="-100" Width="55px" runat="server"></dx:ASPxSpinEdit></span>
                                    
                                    <span style="float: left;padding-top:5px">To:</span>
                                    <span style="float: left"><dx:ASPxSpinEdit ID="txtValueTo" MaxValue="0" MinValue="-100" Width="55px" runat="server"></dx:ASPxSpinEdit></span>
                                    <span style="float: left">
                                        <dx:ASPxComboBox ID="ddlDateUnitsFrom" Width="60px" runat="server">
                                            <Items>
                                                <dx:ListEditItem Text="Days" Selected="true" Value="0" />
                                                <dx:ListEditItem Text="Weeks" Value="1" />
                                                <dx:ListEditItem Text="Months" Value="1" />
                                            </Items>
                                        </dx:ASPxComboBox>

                                    </span>
                                </span>
                                <span id="spanDateRange" runat="server">
                                    <b style="padding-top: 4px; float: left; font-weight: normal;">From:</b>
                                    <span>
                                        <dx:ASPxDateEdit ID="dtcSummaryStartDate" runat="server" CssClass="inputTextBox datetimectr111" EditFormat="DateTime" ClientInstanceName="dtcSummaryStartDate"></dx:ASPxDateEdit>
                                      <%--  <SharePoint:DateTimeControl CssClassTextBox="inputTextBox datetimectr111" DateOnly="true" ID="dtcSummaryStartDate" runat="server" ToolTip="" />--%>
                                    </span>

                                    <b style="padding-top: 4px; float: left; font-weight: normal;">To:</b>
                                    <span>
                                        <dx:ASPxDateEdit ID="dtcSummaryEndDate" runat="server" CssClass="inputTextBox datetimectr111" EditFormat="DateTime" ClientInstanceName="dtcSummaryEndDate"></dx:ASPxDateEdit>
                                     <%--   <SharePoint:DateTimeControl CssClassTextBox="inputTextBox datetimectr111" DateOnly="true" ID="dtcSummaryEndDate" runat="server" ToolTip="" />--%>
                                    </span>
                                </span>

                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label8" runat="server" Text="IT Managers" Font-Bold="true"> </asp:Label></td>
                        <td>


                            <dx:ASPxGridLookup GridViewProperties-EnableCallBacks="true" ClientInstanceName="glITManagers" ClientEnabled="true" Visible="true" OnInit="glITManagers_Init" SelectionMode="Multiple" ID="glITManagers" runat="server" KeyFieldName="ID" MultiTextSeparator=", " Width="250px">
                                <Columns>
                                    <dx:GridViewCommandColumn SelectAllCheckboxMode="AllPages" Caption="" ShowSelectCheckbox="True" Width="40px" />
                                    <dx:GridViewDataTextColumn FieldName="Name" Width="180px">

                                        <SettingsHeaderFilter>
                                            <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                        </SettingsHeaderFilter>
                                        <HeaderTemplate>(Select All)</HeaderTemplate>
                                        <Settings AllowSort="False" />
                                    </dx:GridViewDataTextColumn>
                                </Columns>

                                <GridViewProperties>
                                    <Settings VerticalScrollableHeight="120" ShowFilterRow="false" VerticalScrollBarMode="Auto" />
                                    <SettingsBehavior AllowSort="false" AutoExpandAllGroups="true" AllowClientEventsOnLoad="true" />
                                    <SettingsPager Mode="ShowAllRecords">
                                    </SettingsPager>

                                    <SettingsCommandButton>
                                        <ShowAdaptiveDetailButton ButtonType="Image"></ShowAdaptiveDetailButton>

                                        <HideAdaptiveDetailButton ButtonType="Image"></HideAdaptiveDetailButton>
                                    </SettingsCommandButton>
                                </GridViewProperties>
                                <ClientSideEvents />
                            </dx:ASPxGridLookup>

                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;
                        </td>
                        <td style="text-align: right;">
                            <div class="addEditPopup-btnWrap">
                                <dx:ASPxButton ID="lnkCancel" runat="server" Text="Cancel" CssClass="secondary-cancelBtn" OnClick="lnkCancel_Click">
                                </dx:ASPxButton>
                                <dx:ASPxButton ID="lnkSubmit" Text="Schedule Report" runat="server" CssClass="primary-blueBtn">
                                    <ClientSideEvents Click="function(s,e){SetFiltersForSchedule(s);}" />
                                </dx:ASPxButton>
                                <dx:ASPxButton ID="btnBuild" runat="server" Text="Build Report" CssClass="primary-blueBtn" AutoPostBack="false">
                                    <ClientSideEvents Click="function(s,e){GetTicketsReportByPRPPopup(s);}" />
                                </dx:ASPxButton>
                            </div>
                        </td>
                    </tr>
                </table>
        </div>
    </div>    
            </fieldset>
        </dx:PanelContent>
    </PanelCollection>
</dx:ASPxCallbackPanel>

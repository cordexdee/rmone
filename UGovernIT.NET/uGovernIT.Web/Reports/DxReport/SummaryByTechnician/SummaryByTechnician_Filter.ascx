<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SummaryByTechnician_Filter.ascx.cs" Inherits="uGovernIT.ControlTemplates.uGovernIT.SummaryByTechnician_Filter" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<style type="text/css">
    .module-chkListWrap .dxe{
        vertical-align: top;
    }
    .dxeButtonEditButton{
        border: none;
    }
    .dxgvSelectedRow {
        background: #d9e4fd;
    }
    .dxgvFocusedRow{
        background-color: #f4f6fb;
    }
</style>
<script>
    $(document).ready(function () {
        var moduleName ="<%=ModuleName%>";
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

    function GetTicketsReportByPRPPopup(obj) {
                if ($('#<%=cblSmryByTechModule.ClientID%> :checkbox:checked').length < 1) {
             alert("Please select a Module.");
             return false;
                }

        if (dtcSummaryStartDate.GetValue() == null || dtcSummaryEndDate.GetValue() == null)
            return false;
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
            LoadingPanel.SetText('Please wait...');
            LoadingPanel.Show();
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
            var url = '<%=delegateControl%>' + "?reportName=SummaryByTechnician&SelectedModule=" + selectedModules.join(",") + "&byCategory=" + byCategory + "&IncludeCounts=" + selectedColumns + "&DateFrom=" + myDateFrom + "&DateTo=" + myDateTo + "&includeTechnician=" + includeTechnician + "&selectedITManagers=" + selectedITManagers + "&isModuleSort=" + isModuleSort;
            var popupHeader = "Ticket Summary By Technician";
            window.location.href = url;
        }
        return false;
    }
</script>
<%--<style>
    input[type="checkbox"], input[type="radio"] {
    margin: 4px;
}
</style>--%>
<dx:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel" Modal="True"></dx:ASPxLoadingPanel>

<dx:ASPxCallbackPanel ID="cbpnlMain" ClientInstanceName="cbpnlMain" runat="server">
    <ClientSideEvents />
    <PanelCollection>
        <dx:PanelContent>
            <fieldset>
                <legend class="reportTitle">Summary By Technician</legend>
                <div class="col-md-12 col-sm-12 col-xs-12 noPadding ms-formtable accomp-popup" id="wrapper_1">
                    <div class="row" id="container_1">
                        <div class="col-md-6 col-sm-6 col-xs-12">
                            <div class="ms-formlabel">
                                <h3 class="ms-standardheader budget_fieldLabel">
                                    <asp:Label ID="Label7" runat="server" Text="Modules"></asp:Label>
                                </h3>
                            </div>
                            <div class="ms-formbody accomp_inputField">
                                <div class="module-chkListWrap">
                                    <div class="crm-checkWrap">
                                        <asp:CheckBox ID="chkSmryByTechSelectAll" runat="server" Checked="false" Text="Select All"/>
                                    </div>
                                    <div>
                                        <asp:CheckBoxList ID="cblSmryByTechModule" CssClass="crm-checkWrap" runat="server" RepeatDirection="Vertical"></asp:CheckBoxList>
                                    </div>
                                    <div class="crm-checkWrap">
                                        <asp:CheckBox ID="chkSmryByTechSortByModule" runat="server" Checked="false" Style="display: none;" Text="Sort By Module First" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6 col-sm-6 col-xs-12 noPadding">
                            <div class="ms-formlabel">
                                <h3 class="ms-standardheader budget_fieldLabel">
                                    <asp:Label ID="Label4" runat="server" Text="Include Counts"></asp:Label>
                                </h3>
                            </div>
                            <div class="ms-formbody accomp_inputField">
                                <dx:ASPxCheckBoxList ID="cblIncludeColumns" ClientInstanceName="cblIncludeColumns" Height="150px" runat="server" 
                                    RepeatDirection="Vertical"  CssClass="module-chkListWrap cblIncludeColumns">
                                   </dx:ASPxCheckBoxList>
                            </div>
                        </div>
                    </div>

                    <div class="row">

                            <div class="col-lg-3 col-md-3 col-sm-4 col-xs-6">
                                <div class="ms-formlabel">
                                    <h3 class="ms-standardheader budget_fieldLabel">
                                        <asp:Label ID="Label8" runat="server" Text="IT Managers"> </asp:Label>
                                    </h3>
                                </div>

                                <div>
                                    <dx:ASPxGridLookup GridViewProperties-EnableCallBacks="true" ClientInstanceName="glITManagers"                                         ClientEnabled="true" Visible="true" OnInit="glITManagers_Init" SelectionMode="Multiple" ID="glITManagers"                                         runat="server" KeyFieldName="ID" MultiTextSeparator=", " CssClass="aspxGridLookUp-dropDown"                                         GridViewStyles-Row-CssClass="aspxGridloookUp-drpDownRow" Width="100%" GridViewStyles-RowHotTrack-BackColor="White"	                                    GridViewStyles-FilterRow-CssClass="aspxGridLookUp-FilerWrap" DropDownWindowStyle-CssClass="aspxGridLookup-dropDown"                                        GridViewStyles-FilterCell-CssClass="aspxGridLookUp-FilerCell">                                        <Columns>                                            <dx:GridViewCommandColumn SelectAllCheckboxMode="AllPages" Caption="" ShowSelectCheckbox="True" Width="28px"  />                                            <dx:GridViewDataTextColumn FieldName="Name" Width="150px" >                                                <SettingsHeaderFilter>                                                    <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>                                                </SettingsHeaderFilter>                                                <HeaderTemplate>(Select All)</HeaderTemplate>                                                <Settings AllowSort="False" />                                            </dx:GridViewDataTextColumn>                                        </Columns>                                                                         <GridViewProperties>                                            <Settings ShowFilterRow="false" VerticalScrollBarMode="Auto"/>                                            <SettingsBehavior AllowSort="false" AutoExpandAllGroups="true" AllowClientEventsOnLoad="true" />                                            <SettingsPager Mode="ShowAllRecords">                                            </SettingsPager>                                            <%--<SettingsCommandButton>                                                <ShowAdaptiveDetailButton ButtonType="Image"></ShowAdaptiveDetailButton>                                                <HideAdaptiveDetailButton ButtonType="Image"></HideAdaptiveDetailButton>                                            </SettingsCommandButton>--%>                                        </GridViewProperties>                                     <ClientSideEvents />                                    </dx:ASPxGridLookup>                                </div>
                            </div>
                            <div class="col-lg-4 col-md-4 col-sm-6 col-xs-6 pt-4" >
                                <%--<div style="padding-top: 10px">
                                    <asp:Label ID="Label3" runat="server" Text="" Font-Bold="true"> </asp:Label>
                                </div>--%>
                                <div style="padding-top: 10px">
                                    <div style="float: left">
                                        <dx:ASPxCheckBox ID="chkGroupByOption" TextAlign="Right" Text="Group By Category" ClientInstanceName="chkGroupByOption" runat="server" CssClass="rdFilterCriteria">
                                        </dx:ASPxCheckBox>
                                    </div>

                                    <div style="float: left; padding-left: 20px">
                                        <%--<asp:Label ID="Label5" runat="server" Text="" Font-Bold="true"> </asp:Label>--%>
                                        <dx:ASPxCheckBox ClientInstanceName="chkbxIncludeTechnician" TextAlign="Right" Text="Include ORP" ID="chkbxIncludeTechnician" runat="server" Checked="false" CssClass="rdFilterCriteria">
                                        </dx:ASPxCheckBox>
                                    </div>
                                </div>
                            </div>
                        
                            <div class="col-lg-6 col-md-3 col-sm-3 col-xs-3 noPadding" id="spanNoOfDays" runat="server" visible="false">
                                <div class="ms-formlabel row">
                                    <h3 class="standardheader budget_fieldLabel">
                                        <asp:Label ID="Label6" runat="server" Text="Date Range" Font-Bold="true"> </asp:Label>
                                    </h3>
                                </div>
                                 <div class="row">
                                    <div class="col-md-6 col-sm-6 col-xs-6 noPadding">
                                        <div class="ms-formlabel">
                                            <h3 class="ms-standardheader budget_fieldLabel">From:</h3>
                                        </div>
                                        <div class="ms-formbody accomp_inputField">
                                            <dx:ASPxSpinEdit ID="txtValueFrom" MaxValue="0" MinValue="-100" Width="55px" runat="server"></dx:ASPxSpinEdit>
                                        </div>
                                    </div>
                                    <div class="col-md-6 col-sm-6 col-xs-6 noPadding">
                                         <div class="ms-formlabel">
                                            <h3 class="ms-standardheader budget_fieldLabel">To:</h3>
                                        </div>
                                        <div class="ms-formbody accomp_inputField">
                                            <dx:ASPxSpinEdit ID="txtValueTo" MaxValue="0" MinValue="-100" Width="55px" runat="server"></dx:ASPxSpinEdit>
                                            <dx:ASPxComboBox ID="ddlDateUnitsFrom" Width="60px" runat="server">
                                                <Items>
                                                    <dx:ListEditItem Text="Days" Selected="true" Value="0" />
                                                    <dx:ListEditItem Text="Weeks" Value="1" />
                                                    <dx:ListEditItem Text="Months" Value="1" />
                                                </Items>
                                            </dx:ASPxComboBox>
                                        </div>
                                    </div>
                                 </div>
                             </div>
                            <div class="col-lg-5 col-md-5 col-sm-12 col-xs-12 noPadding" id="spanDateRange" runat="server">
                                <div class="row">
                                    <div class="col-md-6 col-sm-4 col-xs-6 noPadding">
                                         <div class="ms-formlabel">
                                            <h3 class="ms-standardheader budget_fieldLabel">From:</h3>
                                        </div>
                                         <div class="ms-formbody accomp_inputField">
                                              <dx:ASPxDateEdit ID="dtcSummaryStartDate" runat="server" CssClass="CRMDueDate_inputField" EditFormat="Date"
                                                  ClientInstanceName="dtcSummaryStartDate" DropDownButton-Image-Width="18" Width="100%"
                                                  DropDownButton-Image-Url="~/Content/Images/calendarNew.png" >
                                                    <validationsettings CausesValidation="true"  ErrorDisplayMode="ImageWithText" RequiredField-IsRequired="true" 
                                                        RequiredField-ErrorText="From Date Required" Display="Dynamic">
                                                    </validationsettings>
                                              </dx:ASPxDateEdit>
                                         </div>
                                    </div>
                                    <div class="col-md-6 col-sm-4 col-xs-6 noPadding">
                                         <div class="ms-formlabel">
                                            <h3 class="ms-standardheader budget_fieldLabel">To:</h3>
                                        </div>
                                        <div class="ms-formbody accomp_inputField">
                                            <dx:ASPxDateEdit ID="dtcSummaryEndDate" runat="server" CssClass="CRMDueDate_inputField" EditFormat="Date"
                                                ClientInstanceName="dtcSummaryEndDate" DropDownButton-Image-Width="18" Width="100%"
                                                  DropDownButton-Image-Url="~/Content/Images/calendarNew.png">
                                                <validationsettings CausesValidation="true"  ErrorDisplayMode="ImageWithText" RequiredField-IsRequired="true"
                                                    RequiredField-ErrorText="To Date Required" Display="Dynamic">
                                                </validationsettings>
                                            </dx:ASPxDateEdit>
                                        </div>
                                    </div>
                                </div>
                            </div>
</div>
                        
                        <div class="col-md-12 col-sm-12 col-xs-12 addEditPopup-btnWrap">
                           <dx:ASPxButton ID="LinkButton3" runat="server" Text="Cancel" ToolTip="Cancel"
                               CssClass="secondary-cancelBtn"></dx:ASPxButton>
                            <dx:ASPxButton ID="LinkButton4" CssClass="primary-blueBtn" runat="server" Text="Build Report" AutoPostBack="false" CausesValidation="true">
                                <ClientSideEvents Click="function(s,e){GetTicketsReportByPRPPopup(s);}" />
                            </dx:ASPxButton>
                          <%--  <ul style="margin-top: 5px; margin-bottom: 5px; border-top: 0px;">
                                <li runat="server" id="Li3" style="float: right;">
                                    <dx:ASPxButton >
                                        <Image Url="../../Content/cancelwhite.png"></Image>
                                    </dx:ASPxButton>
                                </li>
                                <li runat="server" id="LiBuildReport" style="float: right; margin-right: 10px; margin-left: 10px;">
                                    
                                </li>
                            </ul>--%>
                        </div>
                </div>
            </fieldset>
        </dx:PanelContent>
    </PanelCollection>
</dx:ASPxCallbackPanel>

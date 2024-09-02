<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TicketSummary_Filter.ascx.cs" Inherits="uGovernIT.Report.DxReport.TicketSummary_Filter" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<script type="text/javascript">
    $(document).ready(function () {

        var moduleName = "<%=ModuleName%>";
        $("#<%=cblModules.ClientID%> input:checkbox[value=" + moduleName + "]").prop('checked', true);

        $("#<%=chkSelectAll.ClientID%>").click(function (event) {
            if (this.checked) {

                $("[id*=cblModules] input:checkbox").prop('checked', true);
                $("#<%=chkSortByModule.ClientID%>").parent().css("display", "block");
                $("#<%=chkSortByModule.ClientID%>").attr('checked', 'checked');
            }
            else {
                $("[id*=cblModules] input:checkbox").prop('checked', false);
                $("#<%=chkSortByModule.ClientID%>").parent().css("display", "none");
                $("#<%=chkSortByModule.ClientID%>").removeAttr('checked');
            }
        });
        $('[id*=cblModules] input[type=checkbox]').change(function () {

            if ($('#<%=cblModules.ClientID%> :checkbox:checked').length > 1) {
                $("#<%=chkSortByModule.ClientID%>").parent().css("display", "block");
                $("#<%=chkSortByModule.ClientID%>").attr('checked', 'checked');
            }
            else {
                $("#<%=chkSortByModule.ClientID%>").parent().css("display", "none");
                $("#<%=chkSortByModule.ClientID%>").removeAttr('checked');

                $("#<%=chkSelectAll.ClientID%>").removeAttr('checked');
            }
            if ($('#<%=cblModules.ClientID%> :checkbox:checked').length != this.length) {
                var len = this.length;
                $("#<%=chkSelectAll.ClientID%>").removeAttr('checked');
            }
        });

        $('#<%=rdbtnLstFilterCriteria.ClientID %> input[value=Open]').attr('checked', 'checked');
        $('#<%=rdSortCriteria.ClientID %> input[value=oldesttonewest]').attr('checked', 'checked');
        $("#<%=chkSortByModule.ClientID%>").parent().css("display", "none");
        $("#<%=chkSortByModule.ClientID%>").removeAttr('checked');

        var datecontrolTo = document.getElementById("<%=dtToTicketSummary.ClientID %>_dtToTicketSummaryDate");
        var datecontrolFrom = document.getElementById("<%=dtFromTicketSummary.ClientID %>_dtFromTicketSummaryDate");
        if (datecontrolTo && Date.parse(datecontrolTo.value) != "NaN") {
            datecontrolTo.value = "";
        }
        if (datecontrolFrom && Date.parse(datecontrolFrom.value) != "NaN") {
            datecontrolFrom.value = "";
        }
    });
    function GetTicketsReportPopup(obj) {
        if ($('#<%=cblModules.ClientID%> :checkbox:checked').length < 1) {
            alert("Please select a Module.");
            return false;
        }
        var params = "";
        var selectedModules = [];
        var myDateFrom = "";
        var myDateTo = "";
        $("[id*=<%=cblModules.ClientID%>] input:checked").each(function () {
            selectedModules.push($(this).val());
        });
        var SelectedType = $('#<%=rdbtnLstFilterCriteria.ClientID %> input:checked').val();
        var SortType = $('#<%=rdSortCriteria.ClientID %> input:checked').val();
        var isModuleSort = $("#<%=chkSortByModule.ClientID%>").is(':checked');

        var datecontrolTo = dtToTicketSummary.GetDate();
        if (datecontrolTo != undefined && datecontrolTo != "")
            myDateTo = (datecontrolTo.getMonth() + 1).toString().padStart(2, "0") + "/" + datecontrolTo.getDate().toString().padStart(2, "0") + "/" + datecontrolTo.getFullYear();

        var datecontrolFrom = dtFromTicketSummary.GetDate();
        if (datecontrolFrom != undefined && datecontrolFrom != "")
            myDateFrom = (datecontrolFrom.getMonth() + 1).toString().padStart(2, "0") + "/" + datecontrolFrom.getDate().toString().padStart(2, "0") + "/" + datecontrolFrom.getFullYear();
        
        var url = '<%=delegateControl%>' + "?reportName=TicketSummary&SelectedModule=" + selectedModules + "&SelectedType=" + SelectedType + "&SortType=" + SortType + "&IsModuleSort=" + isModuleSort + "&DateFrom=" + myDateFrom + "&DateTo=" + myDateTo;
        var popupHeader = SelectedType + " Tickets Summary";
        window.location.href = url;
        return false;
    }
</script>
<style>
    input[type="checkbox"], input[type="radio"] {
    margin: 4px;
}
</style>
<dx:ASPxCallbackPanel ID="cbpnlMain" ClientInstanceName="cbpnlMain" runat="server" Style="width: 70%; margin: 4% 10% 4% 14%">
    <ClientSideEvents />
    <PanelCollection>
        <dx:PanelContent >
            <fieldset>
                <legend class="reportTitle"><%= LegendTitle %> Summary Options</legend>
                <div class="col-md-12 col-sm-12 col-xs-12 noPadding ms-formtable accomp-popup" id="wrapper_1">
                    <div class="row" id="container_1">
                        <div class="col-md-6 col-sm-6 col-xs-12">
                            <div class="ms-formlabel">
                                <h3 class="ms-standardheader budget_fieldLabel">
                                    <asp:Label ID="lbl" runat="server" Text="Modules"> </asp:Label>
                                </h3>
                            </div>
                            <div class="ms-formbody accomp_inputField">
                                 <div class="module-chkListWrap">
                                     <div class="crm-checkWrap">
                                        <asp:CheckBox ID="chkSelectAll" runat="server" Checked="false" Text="Select All" />
                                     </div>
                                     <div class="">
                                          <asp:CheckBoxList ID="cblModules" runat="server" CssClass="crm-checkWrap" RepeatDirection="Vertical"></asp:CheckBoxList>
                                     </div>
                                     <div class="crm-checkWrap">
                                          <asp:CheckBox ID="chkSortByModule" runat="server" Checked="false" Style="display: none;"/>
                                     </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6 col-sm-6 col-xs-12">
                            <div class="ms-formlabel">
                                <h3 class="ms-standardheader budget_fieldLabel">
                                     <asp:Label ID="lblFilterCriteria" runat="server" Text="Ticket Status"> </asp:Label>
                                </h3>
                            </div>
                            <div class="ms-formbody accomp_inputField">
                                <asp:RadioButtonList ID="rdbtnLstFilterCriteria" runat="server" RepeatDirection="Horizontal" CssClass="rdFilterCriteria">
                                    <asp:ListItem Text="All" Value="All"></asp:ListItem>
                                    <asp:ListItem Text="Open" Value="Open" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="Closed" Value="Closed"></asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                        </div>  
                        <div class="col-md-6 col-sm-6 col-xs-12">
                             <div class="ms-formlabel">
                                <h3 class="ms-standardheader budget_fieldLabel">
                                    <asp:Label ID="Label1" runat="server" Text="Sort By"> </asp:Label>
                                </h3>
                            </div>
                            <div class="ms-formbody accomp_inputField">
                                <asp:RadioButtonList ID="rdSortCriteria" runat="server" RepeatDirection="Horizontal" CssClass="rdFilterCriteria">
                                    <asp:ListItem Text="Oldest First" Value="oldesttonewest" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="Newest First" Value="newesttooldest"></asp:ListItem>
                                    <asp:ListItem Text="Waiting On" Value="waitingon"></asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                        </div>
                        <div class="col-md-6 col-sm-6 col-xs-12">
                            <div class="ms-formlabel">
                                <h3 class="ms-standardheader budget_fieldLabel">
                                    <asp:Label ID="Label2" runat="server" Text="Date Created"> </asp:Label></td>
                                </h3>
                            </div>
                            <div class="row">
                                <div class="col-md-6 col-sm-6 col-xs-6 noPadding">
                                     <div class="ms-formlabel">
                                        <h3 class="ms-standardheader budget_fieldLabel">From:</h3>
                                    </div>
                                    <div class="ms-formbody accomp_inputField">
                                        <dx:ASPxDateEdit ID="dtFromTicketSummary" runat="server" CssClass="CRMDueDate_inputField"
                                            ClientInstanceName="dtFromTicketSummary" DropDownButton-Image-Width="16"
                                            DropDownButton-Image-Url="~/Content/Images/calendarNew.png"></dx:ASPxDateEdit>
                                    </div>
                                </div>
                                <div class="col-md-6 col-sm-6 col-xs-6 noPadding">
                                    <div class="ms-formlabel">
                                        <h3 class="ms-standardheader budget_fieldLabel">To:</h3>
                                    </div>
                                    <div class="ms-formbody accomp_inputField">
                                            <dx:ASPxDateEdit ID="dtToTicketSummary" runat="server" CssClass="CRMDueDate_inputField" DropDownButton-Image-Width="16"
                                            DropDownButton-Image-Url="~/Content/Images/calendarNew.png" ClientInstanceName="dtToTicketSummary"></dx:ASPxDateEdit>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-12 col-sm-12 col-xs-12">
                             <dx:ASPxLoadingPanel ID="reportLoadingPanel" ClientInstanceName="reportLoadingPanel" runat="server" Modal="true" Text="Please wait..."></dx:ASPxLoadingPanel>
                        </div>
                        <div class="col-md-12 col-sm-12 col-xs-12">
                            <div class="row addEditPopup-btnWrap">
                                <dx:ASPxButton ID="LinkButton1" CssClass="secondary-cancelBtn" runat="server" Text="Cancel" AutoPostBack="false" 
                                  OnClick="LinkButton1_Click"></dx:ASPxButton>
                                <dx:ASPxButton ID="LinkButton2" CssClass="primary-blueBtn" runat="server" Text="Build Report" AutoPostBack="false">
                                    <ClientSideEvents Click="function(s,e){GetTicketsReportPopup(s);reportLoadingPanel.Show();}" />
                                </dx:ASPxButton>
                            </div>
                        </div>
                    </div>
                </div>
            </fieldset>
        </dx:PanelContent>
    </PanelCollection>
</dx:ASPxCallbackPanel>

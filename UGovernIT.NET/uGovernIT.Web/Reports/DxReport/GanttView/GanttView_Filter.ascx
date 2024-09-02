<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GanttView_Filter.ascx.cs" Inherits="uGovernIT.DxReport.GanttView_Filter" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<style type="text/css">

    legend {
	    width: auto;
	    border: none;
	    font-size: 12px;
	    font-weight: 600;
    }

    .ms-formbody {
        background: none repeat scroll 0 0 #E8EDED;
        padding: 2px 6px 2px;
        vertical-align: top;
    }

    .ms-formlabel {
        text-align: right;
        padding: 6px 7px 6px 0px;
        width: 100px;
        vertical-align: middle;
        font-weight: normal;
        /*border-top: 1px solid #A5A5A5;*/
    }
    /*.ms-component-formlabel {
        border-top: 1px solid #A5A5A5;
        padding-top: 2px;
        padding-right: 8px;
        padding-bottom: 2px;
        color: #000;
        text-align: right;
        width: 100px;
        vertical-align: middle;
        font-weight: normal;
    }*/
    .full_width {
        width: 100%;
    }

    .main_heading {
        background: none repeat scroll 0 0 #BED0E5;
        float: left;
        font-size: 20px;
        font-weight: bold;
        margin-bottom: 5px;
        padding: 0;
        text-align: center;
        width: 100%;
    }

    .table_lst {
        width: 100%;
    }

    .button_lst {
        width: 70px;
        margin: 5px;
    }

    .listbox_lst {
        width: 150px;
        height: 100px;
    }

    .td_lst {
        /*width: 33%;*/
        padding: 2px 5px;
        vertical-align: middle;
        text-align: center;
    }

    .error {
        color: red;
    }

    .rbutton {
        float: left;
        width: 100px;
        margin-top: 5px;
    }

    .rbutton label {
        vertical-align: middle;
        margin-top: -1px;
    }

    .rbutton input[type="radio"] {
	    float: left;
    }

    .button-refresh {
        background: url('/_layouts/15/images/uGovernIT/refresh-icon.png') no-repeat;
        width: 20px;
        height: 20px;
    }

    .rblistPriority td {
        padding: 0 !important;
    }

    .rblistPriority .dxe > table > tbody > tr > td {
        width: 100px !important;
    }
</style>
<script type="text/javascript">
    function getGanttChart(obj) {
        var UserSelectedColumns = grid.GetSelectedKeysOnPage();
        UserSelectedColumns = UserSelectedColumns.join();
        var params = "";
        var ModuleName = '<%=ModuleName%>';
        var GanttType = "1";  // Constant for project gantt chart.
        var openProject = '';
        if ($("#<%=rbOpen.ClientID%>").is(':checked') == true)
            openProject = 'true';
        else if ($("#<%=rbClose.ClientID%>").is(':checked') == true)
            openProject = 'false';
        else if ($("#<%=rbAll.ClientID%>").is(':checked') == true)
            openProject = 'all';

        var arrColumns = [];
        var value = document.getElementById("<%=ddlProjectSortOrder.ClientID%>");
        var index = value.options[value.selectedIndex].value - 1;
        arrColumns[index] = 'Title';

        value = document.getElementById("<%=ddlPrioritySortOrder.ClientID%>");
        index = value.options[value.selectedIndex].value - 1;
        arrColumns[index] = 'PriorityLookup';

        value = document.getElementById("<%=ddlProjectRank.ClientID%>");
        index = value.options[value.selectedIndex].value - 1;
        arrColumns[index] = 'ProjectRank';

        value = document.getElementById("<%=ddlProgressSortOrder.ClientID%>");
        index = value.options[value.selectedIndex].value - 1;
        arrColumns[index] = 'Status';

        value = document.getElementById("<%=ddlTargetDateSortOrder.ClientID%>");
        index = value.options[value.selectedIndex].value - 1;
        arrColumns[index] = 'DesiredCompletionDate';


        <%--$("#<%=chkOpenProjectOnly.ClientID%>").is(':checked');--%>
        var GroupBy = $("#<%=ddlGroupBy.ClientID%>").val();

        var url = '<%=delegateControl %>' + "?reportName=GanttView&GanttType=" + GanttType + "&GroupBy=" + GroupBy + "&OpenProjectOnly=" + openProject + "&Module=" + ModuleName + "&UserSelectedColumns=" + UserSelectedColumns + "&ColumnsSortOrder=" + arrColumns.join();
        var title = 'Gantt Chart';
        if (openProject == true) {
            title += ' - Open Projects';
        }
        else {
            title += ' - All Projects';
        }

        if (GroupBy > 0) {
            if (GroupBy == 1) {
                title += ' by Priority';
            }
            else if (GroupBy == 2) {
                title += ' by Project Type';
            }
            else if (GroupBy == 3) {
                title += ' by Business Initiative';
            }
        }

        LoadingPanel.Show();
        window.location.href = url;
        //window.parent.UgitOpenPopupDialog(url, params, title, '90', '90', 0, escape(" Request.Url.AbsolutePath"));
        return false;
    }
</script>
<dx:ASPxLoadingPanel ID="LoadingPanel" runat="server" Text="Loading..." CssClass="customeLoader" ClientInstanceName="LoadingPanel" Image-Url="~/Content/IMAGES/ajax-loader.gif" ImagePosition="Top"
    Modal="True">
</dx:ASPxLoadingPanel>
<div id="ganntPopup">

                <div>
                    <asp:Label ID="lblMsg" Text="" runat="server" CssClass="error" />
                </div>

                <table style="margin: 0px auto;">
                    <tr>
                        <td style="vertical-align: top; width: 800px;">

                            <div style="width: 100%; float: left;">
                                <fieldset>
                                    <legend>Build Gantt Chart</legend>
                                    <table style="width: 100%;">
                                        <tr>
                                            <td class="ms-formlabel">
                                                <asp:Label ID="lblGanttType" runat="server" Text="Group By"> </asp:Label>
                                            </td>
                                            <td class="ms-formbody">
                                                <asp:DropDownList ID="ddlGroupBy" runat="server" />
                                            </td>
                                        </tr>


                                    </table>
                                </fieldset>
                            </div>
                            <div style="width: 100%; float: left;">

                                <fieldset>
                                    <legend>Select Project(s):</legend>
                                    <table style="width: 100%;">
                                        <tr>
                                            <td class="ms-formlabel">Project Status</td>
                                            <td class="ms-formbody">
                                                <span class="rbutton">
                                                    <asp:RadioButton ID="rbOpen" runat="server" GroupName="TicketStatus"
                                                        AutoPostBack="false" Checked="true" />
                                                    <label>Open</label>
                                                </span>
                                                <span class="rbutton">
                                                    <asp:RadioButton ID="rbClose" runat="server" GroupName="TicketStatus"
                                                        AutoPostBack="false" />
                                                    <label>Closed</label>
                                                </span>
                                                <span class="rbutton">
                                                    <asp:RadioButton ID="rbAll" runat="server" GroupName="TicketStatus"
                                                        AutoPostBack="false" />
                                                    <label>All</label>
                                                </span>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </div>

                            <div style="width: 100%; float: left;">

                                <fieldset>
                                    <legend>Sort Order:</legend>
                                    <table style="width: 100%;">
                                        <tr>
                                            <td class="ms-formlabel">Project Name</td>
                                            <td class="ms-formbody">
                                                <asp:DropDownList ID="ddlProjectSortOrder" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlProjectSortOrder_SelectedIndexChanged">
                                                    <asp:ListItem Value="1" Text="1" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Value="2" Text="2"></asp:ListItem>
                                                    <asp:ListItem Value="3" Text="3"></asp:ListItem>
                                                    <asp:ListItem Value="4" Text="4"></asp:ListItem>
                                                    <asp:ListItem Value="5" Text="5"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td class="ms-formlabel">Priority</td>
                                            <td class="ms-formbody">
                                                <asp:DropDownList ID="ddlPrioritySortOrder" OnSelectedIndexChanged="ddlPrioritySortOrder_SelectedIndexChanged" AutoPostBack="true" runat="server">
                                                    <asp:ListItem Value="1" Text="1"></asp:ListItem>
                                                    <asp:ListItem Value="2" Text="2" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Value="3" Text="3"></asp:ListItem>
                                                    <asp:ListItem Value="4" Text="4"></asp:ListItem>
                                                    <asp:ListItem Value="5" Text="5"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>

                                            <td class="ms-formlabel">Rank</td>
                                            <td class="ms-formbody">
                                                <asp:DropDownList ID="ddlProjectRank" OnSelectedIndexChanged="ddlProjectRank_SelectedIndexChanged" AutoPostBack="true" runat="server">
                                                    <asp:ListItem Value="1" Text="1"></asp:ListItem>
                                                    <asp:ListItem Value="2" Text="2"></asp:ListItem>
                                                    <asp:ListItem Value="3" Text="3" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Value="4" Text="4"></asp:ListItem>
                                                    <asp:ListItem Value="5" Text="5"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>


                                            <td class="ms-formlabel">Progress</td>
                                            <td class="ms-formbody">
                                                <asp:DropDownList ID="ddlProgressSortOrder" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlProgressSortOrder_SelectedIndexChanged">
                                                    <asp:ListItem Value="1" Text="1"></asp:ListItem>
                                                    <asp:ListItem Value="2" Text="2"></asp:ListItem>
                                                    <asp:ListItem Value="3" Text="3"></asp:ListItem>
                                                    <asp:ListItem Value="4" Text="4" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Value="5" Text="5"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td class="ms-formlabel">Target Date</td>
                                            <td class="ms-formbody">
                                                <asp:DropDownList ID="ddlTargetDateSortOrder" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlTargetDateSortOrder_SelectedIndexChanged">
                                                    <asp:ListItem Value="1" Text="1"></asp:ListItem>
                                                    <asp:ListItem Value="2" Text="2"></asp:ListItem>
                                                    <asp:ListItem Value="3" Text="3"></asp:ListItem>
                                                    <asp:ListItem Value="4" Text="4"></asp:ListItem>
                                                    <asp:ListItem Value="5" Text="5" Selected="True"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>

                                        </tr>
                                    </table>
                                </fieldset>
                            </div>

                            <div class="first_tier_nav" style="padding-top: 0px !important;">
                                <table style="width: 100%;">
                                    <tr>
                                        <td>
                                            <div class="first_tier_nav pt-3">
                                                <dx:ASPxButton ID="btnBuildGantt" runat="server" CssClass="primary-blueBtn" Text="Build Gantt" AutoPostBack="false">
                                                    <ClientSideEvents Click="function(s,e){getGanttChart(s);}" />
                                                </dx:ASPxButton>
                                                <dx:ASPxButton ID="CancelBtn" runat="server" Text="Cancel" CssClass="secondary-cancelBtn" AutoPostBack="false" OnClick="CancelBtn_Click">
                                                </dx:ASPxButton>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>

                        <td style="vertical-align: top; width: 400px;">
                            <div style="width: 100%; float: left;">
                                <fieldset>
                                    <legend>Project Fields</legend>

                                    <dx:ASPxGridView ID="grid" ClientInstanceName="grid" runat="server" KeyFieldName="ID" Width="100%" OnHtmlRowPrepared="grid_HtmlRowPrepared" OnDataBound="grid_DataBound">
                                        <Columns>
                                            <dx:GridViewCommandColumn ShowSelectCheckbox="true" SelectAllCheckboxMode="Page" />
                                            <dx:GridViewDataColumn FieldName="FieldDisplayName" Caption="Field Name" />
                                            <dx:GridViewDataColumn FieldName="DisplayForReport" Visible="false" />
                                            <dx:GridViewDataColumn FieldName="FieldName" Visible="false" />

                                        </Columns>
                                        <SettingsPager Mode="ShowAllRecords"></SettingsPager>
                                    </dx:ASPxGridView>
                                </fieldset>
                            </div>
                        </td>
                    </tr>
                </table>



</div>

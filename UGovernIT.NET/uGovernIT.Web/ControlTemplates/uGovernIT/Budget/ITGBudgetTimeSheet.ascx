
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ITGBudgetTimeSheet.ascx.cs"
    Inherits="uGovernIT.Web.ITGBudgetTimeSheet" %>
<%@ Register Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
    .detailviewmain
    {
        float: left;
        width: 100%;
    }
    .worksheetmessage-m
    {
        text-align: center;
        position: relative;
        top: -10px;
        display: none;
    }
    .worksheetmessage-m1
    {
        float: left;
        padding-left: 7px;
        width: 99%;
    }
    .worksheetheading-m
    {
    }
    .worksheetheading
    {
    }
    .worksheetpanel
    {
    }
    .worksheetpanel-m
    {
        float: left;
        padding: 7px;
        width: 99%;
    }
    .worksheettable
    {
    }
    .worksheetheader
    {
    }
    .paddingfirstcell
    {
    }
    .alncenter
    {
        text-align: center;
    }
    .editpanel
    {
        float: left;
    }
    .editinputwidth
    {
        width: 26px;
        height: 12px;
    }
    .editdropdownwidth
    {
        width: 168px;
    }
    .alnright
    {
        text-align: right;
    }
    .totalborderhorisontal
    {
        border-bottom: 1px solid #6c6e70 !important;
        font-weight: bold;
    }
    .totalbordervartical
    {
        border-left: 1px solid #6c6e70 !important;
        border-right: 1px solid #6c6e70 !important;
        font-weight: bold;
    }
    
    .filteredcalender-m
    {
        float: left;
        position: relative;
        width: 300px;
    }
    .calendertxtbox
    {
        margin-right: 46px;
        visibility: hidden;
    }
    .calendernextweekbt
    {
        float: left;
        left: 194px;
        position: absolute;
        padding-top: 3px;
    }
    .calenderpreweekbt
    {
        float: left;
        padding-right: 4px;
        padding-top: 3px;
    }
    .calenderweektxt
    {
        float: left;
        font-weight: bold;
        left: 17px;
        position: absolute;
        text-align: center;
        top: 1px;
        width: 175px;
        padding-top: 3px;
    }
</style>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function CalculateTotalOnFly(textIndex, control) {
        var listViewId = "<%= projectPlanSheet.ClientID%>";
        var totalControl = document.getElementById(listViewId + "_lbWeekDay" + textIndex + "VTotal");
        var weekTotalControl = document.getElementById(listViewId + "_lbVTotal");
        var weekHTotalControl = document.getElementById("lbHTotalAction");
        //lbHTotalAction 
        var weekHTotal = parseInt(weekHTotalControl.innerHTML);
        var weekTotal = parseInt(weekTotalControl.innerHTML);
        var oldTotal = parseInt(totalControl.innerHTML);
        var oldTxtVal = parseInt(control.getAttribute("oldVal"));
        var nexTxtVal = parseInt(control.value);
        var newTotal = 0;
        if (oldTotal != NaN && nexTxtVal != NaN && oldTxtVal != NaN) {
            newTotal = (oldTotal - oldTxtVal) + nexTxtVal;

            totalControl.innerHTML = newTotal;
            weekTotalControl.innerHTML = (weekTotal - oldTotal) + newTotal;
            weekHTotalControl.innerHTML = (weekHTotal - oldTxtVal) + nexTxtVal;
        }
    }

    function EditItemOnbdClick(rowIndex) {
        var listViewId = "<%= projectPlanSheet.ClientID%>";
        var editBt = document.getElementById(listViewId + "_ctrl" + rowIndex + "_lnkEdit");
        if (editBt) {
            editBt.click();
        }
    }
</script>
<asp:UpdatePanel runat="server" ID="projectPlanUpdatePanel" UpdateMode="Conditional">
    <Triggers>
    </Triggers>
    <ContentTemplate>
        <asp:HiddenField runat="server" ID="subCategoryIdHidden" />
        <asp:HiddenField runat="server" ID="currentStartDate" Value="<%=Convert.ToString(fiscalYearStartDate)%>" />
        <asp:HiddenField runat="server" ID="currentEndDate" Value="<%=Convert.ToString(fiscalYearEndDate%>)" />
        <asp:HiddenField runat="server" ID="currentYearHidden" Value="<%=DateTime.Today.Year%>" />
        <asp:Panel ID="projectPlanPanel" CssClass="worksheetpanel" runat="server">            
            <div class="worksheetpanel-m">
                <asp:ListView ID="projectPlanSheet" Visible="true" runat="server" ItemPlaceholderID="PlaceHolder1"
                    DataKeyNames="NprId" OnItemEditing="projectPlanSheet_ItemEditing" OnItemUpdating="projectPlanSheet_ItemUpdating"
                    OnItemCanceling="projectPlanSheet_ItemCanceling">
                    <LayoutTemplate>
                        <table class="ms-listviewtable worksheettable" style="border-collapse: collapse"
                            width="100%" cellpadding="0" cellspacing="0">
                            <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
                        </table>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr <%# Container.DataItemIndex < 2 ? "style='font-weight:bold'" : "false"%>  ondblclick="EditItemOnbdClick(<%# Container.DataItemIndex %>);">
                            <td class="ms-vb2 paddingfirstcell ">
                                <b><%#  Eval("Title") %></b>
                            </td>
                            <td class="ms-vb2 alncenter">
                                <%# Eval("Month1")%>
                            </td>
                            <td class="ms-vb2 alncenter">
                                <%# Eval("Month2")%>
                            </td>
                            <td class="ms-vb2 alncenter">
                                <%# Eval("Month3")%>
                            </td>
                            <td class="ms-vb2 alncenter">
                                <%# Eval("Month4")%>
                            </td>
                            <td class="ms-vb2 alncenter">
                                <%# Eval("Month5")%>
                            </td>
                            <td class="ms-vb2 alncenter">
                                <%# Eval("Month6")%>
                            </td>
                            <td class="ms-vb2 alncenter">
                                <%# Eval("Month7")%>
                            </td>
                            <td class="ms-vb2 alncenter">
                                <%# Eval("Month8")%>
                            </td>
                            <td class="ms-vb2 alncenter">
                                <%# Eval("Month9")%>
                            </td>
                            <td class="ms-vb2 alncenter">
                                <%# Eval("Month10")%>
                            </td>
                            <td class="ms-vb2 alncenter">
                                <%# Eval("Month11")%>
                            </td>
                            <td class="ms-vb2 alncenter">
                                <%# Eval("Month12")%>
                            </td>
                            <td class="ms-vb2 alncenter totalbordervartical">
                                <%# Eval("Total") %>
                            </td>
                            <td class="ms-vb2 alncenter">
                                <asp:ImageButton runat="server" ID="lnkEdit" CommandName="Edit" ImageUrl="/Content/images/edit-icon.png"
                                    BorderWidth="0" ToolTip="Edit"  Visible='<%# Container.DataItemIndex < 1 ? false : true %>'  />
                            </td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr class="ms-alternatingstrong" <%# Container.DataItemIndex < 2 ? "style='font-weight:bold'" : "false"%>  ondblclick="EditItemOnbdClick(<%# Container.DataItemIndex %>);">
                            <td class="ms-vb2 paddingfirstcell ">
                                <b><%#  Eval("Title") %></b>
                            </td>
                            <td class="ms-vb2 alncenter">
                                <%# Eval("Month1")%>
                            </td>
                            <td class="ms-vb2 alncenter">
                                <%# Eval("Month2")%>
                            </td>
                            <td class="ms-vb2 alncenter">
                                <%# Eval("Month3")%>
                            </td>
                            <td class="ms-vb2 alncenter">
                                <%# Eval("Month4")%>
                            </td>
                            <td class="ms-vb2 alncenter">
                                <%# Eval("Month5")%>
                            </td>
                            <td class="ms-vb2 alncenter">
                                <%# Eval("Month6")%>
                            </td>
                            <td class="ms-vb2 alncenter">
                                <%# Eval("Month7")%>
                            </td>
                            <td class="ms-vb2 alncenter">
                                <%# Eval("Month8")%>
                            </td>
                            <td class="ms-vb2 alncenter">
                                <%# Eval("Month9")%>
                            </td>
                            <td class="ms-vb2 alncenter">
                                <%# Eval("Month10")%>
                            </td>
                            <td class="ms-vb2 alncenter">
                                <%# Eval("Month11")%>
                            </td>
                            <td class="ms-vb2 alncenter">
                                <%# Eval("Month12")%>
                            </td>
                            <td class="ms-vb2 alncenter totalbordervartical">
                                <%# Eval("Total") %>
                            </td>
                            <td class="ms-vb2 alncenter">
                                <asp:ImageButton runat="server" ID="lnkEdit" CommandName="Edit" ImageUrl="/Content/images/edit-icon.png"
                                    BorderWidth="0" ToolTip="Edit"  Visible='<%# Container.DataItemIndex < 1 ? false : true%>' />
                            </td>
                        </tr>
                    </AlternatingItemTemplate>
                    <EditItemTemplate>
                        <tr>
                            <td class="ms-vb2 paddingfirstcell ">
                                <%# Eval("Title") %>
                                <b><asp:HiddenField runat="server" ID="budgetDates" Value='<%# Eval("BudgetDate")%>' /></b>
                            </td>
                            <td class="ms-vb2 alncenter">
                                $<asp:TextBox CssClass="editinputwidth" onChange="javascript:CalculateTotalOnFly(1,this)" 
                                    oldVal='<%# Eval("Month1")%>' ID="txtMonth1" runat="server" Text='<%# Eval("Month1")%>'></asp:TextBox>
                            </td>
                            <td class="ms-vb2 alncenter">
                                $<asp:TextBox CssClass="editinputwidth" onChange="javascript:CalculateTotalOnFly(2,this)"
                                    oldVal='<%# Eval("Month2")%>' ID="txtMonth2" runat="server" Text='<%# Eval("Month2")%>'></asp:TextBox>
                            </td>
                            <td class="ms-vb2 alncenter">
                                $<asp:TextBox CssClass="editinputwidth" onChange="javascript:CalculateTotalOnFly(3,this)"
                                    oldVal='<%# Eval("Month3")%>' ID="txtMonth3" runat="server" Text='<%# Eval("Month3")%>'></asp:TextBox>
                            </td>
                            <td class="ms-vb2 alncenter">
                                $<asp:TextBox CssClass="editinputwidth" onChange="javascript:CalculateTotalOnFly(4,this)"
                                    oldVal='<%# Eval("Month4")%>' ID="txtMonth4" runat="server" Text='<%# Eval("Month4")%>'></asp:TextBox>
                            </td>
                            <td class="ms-vb2 alncenter">
                                $<asp:TextBox CssClass="editinputwidth" onChange="javascript:CalculateTotalOnFly(5,this)"
                                    oldVal='<%# Eval("Month5")%>' ID="txtMonth5" runat="server" Text='<%# Eval("Month5")%>'></asp:TextBox>
                            </td>
                            <td class="ms-vb2 alncenter">
                                $<asp:TextBox CssClass="editinputwidth" onChange="javascript:CalculateTotalOnFly(6,this)"
                                    oldVal='<%# Eval("Month6")%>' ID="txtMonth6" runat="server" Text='<%# Eval("Month6")%>'></asp:TextBox>
                            </td>
                            <td class="ms-vb2 alncenter">
                                $<asp:TextBox CssClass="editinputwidth" onChange="javascript:CalculateTotalOnFly(7,this)"
                                    oldVal='<%# Eval("Month7")%>' ID="txtMonth7" runat="server" Text='<%# Eval("Month7")%>'></asp:TextBox>
                            </td>
                            <td class="ms-vb2 alncenter">
                                $<asp:TextBox CssClass="editinputwidth" onChange="javascript:CalculateTotalOnFly(7,this)"
                                    oldVal='<%# Eval("Month8")%>' ID="txtMonth8" runat="server" Text='<%# Eval("Month8")%>'></asp:TextBox>
                            </td>
                            <td class="ms-vb2 alncenter">
                                $<asp:TextBox CssClass="editinputwidth" onChange="javascript:CalculateTotalOnFly(7,this)"
                                    oldVal='<%# Eval("Month9")%>' ID="txtMonth9" runat="server" Text='<%# Eval("Month9")%>'></asp:TextBox>
                            </td>
                            <td class="ms-vb2 alncenter">
                                $<asp:TextBox CssClass="editinputwidth" onChange="javascript:CalculateTotalOnFly(7,this)"
                                    oldVal='<%# Eval("Month10")%>' ID="txtMonth10" runat="server" Text='<%# Eval("Month10")%>'></asp:TextBox>
                            </td>
                            <td class="ms-vb2 alncenter">
                                $<asp:TextBox CssClass="editinputwidth" onChange="javascript:CalculateTotalOnFly(7,this)"
                                    oldVal='<%# Eval("Month11")%>' ID="txtMonth11" runat="server" Text='<%# Eval("Month11")%>'></asp:TextBox>
                            </td>
                            <td class="ms-vb2 alncenter">
                                $<asp:TextBox CssClass="editinputwidth" onChange="javascript:CalculateTotalOnFly(7,this)"
                                    oldVal='<%# Eval("Month12")%>' ID="txtMonth12" runat="server" Text='<%# Eval("Month12")%>'></asp:TextBox>
                            </td>
                            <td class="ms-vb2 alncenter totalbordervartical">
                                <span id="lbHTotalAction">
                                    <%# Eval("Total") %>
                            </td>
                            <td class="ms-vb2 alncenter">
                                <span>
                                    <asp:LinkButton runat="server" ID="btnUpdate" Text="<img style='border:0px;' src='/Content/images/save-icon.png' alt='Update'" 
                                    BorderWidth="0" ToolTip="Update" CommandName="Update" CommandArgument='<%# Eval("Category") %>'/>                                    
                                </span>
                                <asp:LinkButton ID="btnCancel" runat="server" Text="<img style='border:0px;' src='/Content/images/cancel-icon.png' alt='Cancel'/>"
                                    CommandName="Cancel" />
                            </td>
                        </tr>
                    </EditItemTemplate>
                </asp:ListView>
            </div>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>

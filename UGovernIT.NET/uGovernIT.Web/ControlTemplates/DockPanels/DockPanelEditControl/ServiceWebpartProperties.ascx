<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ServiceWebpartProperties.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.DockPanels.DockPanelEditControl.ServiceWebpartProperties" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script type="text/javascript" id="dxss_ServiceWebPartProperties" data-v="<%=UGITUtility.AssemblyVersion %>">
    $(function () {
        //for oninit on client side
        var selectedvalue = $('#<%= rblServiceViewType.ClientID %> input:checked').val()
        if(selectedvalue == "IconView")
        { 
             chkShowServiceIcons.SetVisible(false);
             cmbIconSize.SetVisible(true);
        }
        else
        { 
            chkShowServiceIcons.SetVisible(true);
            cmbIconSize.SetVisible(false);
        }    

        $('#<%=rblServiceViewType.ClientID%>').on('change', function () {
            var selectedvalue = $('#<%= rblServiceViewType.ClientID %> input:checked').val()
            if(selectedvalue == "IconView")
            { 
                chkShowServiceIcons.SetVisible(false);
                cmbIconSize.SetVisible(true);
            }
            else
            { 
                chkShowServiceIcons.SetVisible(true);
                cmbIconSize.SetVisible(false);
            }         
        });        
    });

</script>

<div class="col-md-12 col-sm-12 col-xs-12  ms-formtable accomp-popup noPadding">
    <div class="row" id="tr8" runat="server">
        <div class="ms-formlabel">
            <h3 class="ms-standardheader budget_fieldLabel">Options</h3>
        </div>
        <div class="ms-formbody accomp_inputField">
            <div>
                <dx:ASPxCheckBox ID="chkServiceCatalog" CssClass="dxcheckbox" runat="server" Text="Service Catalog" TextAlign="Right">
                    <ClientSideEvents
                    Init="function(s,e){ if(s.GetChecked()){chkShowServiceIcons.SetVisible(true)}else{chkShowServiceIcons.SetVisible(false)} }"
                    CheckedChanged="function(s,e){ if(s.GetChecked()){chkShowServiceIcons.SetVisible(true)}else{chkShowServiceIcons.SetVisible(false)} }" />
                </dx:ASPxCheckBox>
            </div>
            <div class="servicecatalog-option">
                <dx:ASPxCheckBox ID="chkShowServiceIcons" CssClass="dxcheckbox" ClientInstanceName="chkShowServiceIcons" 
                    runat="server" Text="Show icons" ></dx:ASPxCheckBox>
            </div>
            <div>
                <span>
                    <asp:RadioButtonList runat="server" ID="rblServiceViewType" AutoPostBack="false" 
                        RepeatDirection="Horizontal" CssClass="custom-radiobuttonlist adminradioBtnList">
                        <asp:ListItem Value="ListView" Text="List View"></asp:ListItem>
                        <asp:ListItem Value="ButtonView" Text="Button View"></asp:ListItem>
                        <asp:ListItem Value="DropdownView" Text="Dropdown View"></asp:ListItem>
                        <asp:ListItem Value="IconView" Text="Icon View"></asp:ListItem>
                    </asp:RadioButtonList>
                </span>
                <dx:ASPxComboBox ID="cmbIconSize" CssClass="aspxComBox-dropDown admin-aspxcomBox" ClientInstanceName="cmbIconSize" runat="server" Caption="Size"
                    Width="50%" ListBoxStyle-CssClass="aspxComboBox-listBox">
                        <Items>
                            <dx:ListEditItem Text="32x32" Value="32" Selected="true" />
                            <dx:ListEditItem Text="50x50" Value="50" />
                            <dx:ListEditItem Text="64x64" Value="64" />
                            <dx:ListEditItem Text="84x84" Value="84" />
                            <dx:ListEditItem Text="100x100" Value="100" />
                        </Items>
                </dx:ASPxComboBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="ms-formlabel">
            <h3 class="ms-standardheader budget_fieldLabel">Panel</h3>
        </div>
        <div class="ms-formbody accomp_inputField">
            <div>
                <dx:ASPxCheckBox ID="chkPanel" CssClass="dxcheckbox" runat="server" Text="Show Panel" TextAlign="Right"></dx:ASPxCheckBox>
            </div>
            <div style="padding-top:5px;">
                <asp:DropDownList ID="ddlPanel" CssClass="itsmDropDownList aspxDropDownList" runat="server"></asp:DropDownList>
            </div>
        </div>
    </div>
</div>
<%--<table class="ms-formtable" cellpadding="0" cellspacing="0" style="border-collapse: collapse" width="100%">--%>
<%--     <tr id="tr12" runat="server">
        <td class="ms-formlabel">
            <h3 class="ms-standardheader">Title</h3>
        </td>
        <td class="ms-formbody">
            <div>
                <div class="fleft">
                <dx:ASPxCheckBox ID="chkTitle" runat="server">
                        <ClientSideEvents
                        Init="function(s,e){ if(s.GetChecked()){txtTitle.SetVisible(true)}else{txtTitle.SetVisible(false)} }"
                        CheckedChanged="function(s,e){ if(s.GetChecked()){txtTitle.SetVisible(true)}else{txtTitle.SetVisible(false)} }" />
                </dx:ASPxCheckBox>
                </div>
                <div class="fleft padding-left5">
                <dx:ASPxTextBox ID="txtTitle" ClientInstanceName="txtTitle" runat="server"></dx:ASPxTextBox>
                </div>
            </div>
        </td>
    </tr>--%>
<%--  <tr id="tr13" runat="server">
        <td class="ms-formlabel">
            <h3 class="ms-standardheader">No. of Preview Tickets</h3>
        </td>
        <td class="ms-formbody">
            <div>
                <dx:ASPxTextBox ID="txtNoOfTickets" runat="server" NullText="No. Of tickets"></dx:ASPxTextBox>
            </div>
        </td>
    </tr>--%>
<%--    <tr id="tr3" runat="server">
        <td class="ms-formlabel">
            <h3 class="ms-standardheader"><%=WaitingOnMeTabName%> Tab (from config var)</h3>
        </td>
        <td class="ms-formbody">
            <div>
                <div class="fleft">
                <dx:ASPxCheckBox ID="chkWaitingOnMeTab" runat="server">
                </dx:ASPxCheckBox>
                </div>
                <div class="fleft padding-left5">
                    <dx:ASPxTextBox ID="txtWaitingOnMeTab" ClientInstanceName="txtWaitingOnMeTab" Visible="false" runat="server"></dx:ASPxTextBox>
                </div>
            </div>
        </td>
    </tr>--%>
<%--     <tr id="tr1" runat="server">
        <td class="ms-formlabel">
            <h3 class="ms-standardheader">My Requests Tab</h3>
        </td>
        <td class="ms-formbody">
            <div>
                <div class="fleft">
                <dx:ASPxCheckBox ID="chkMyReqeustTab" runat="server">
                        <ClientSideEvents
                        Init="function(s,e){ if(s.GetChecked()){txtMyRequestTab.SetVisible(true)}else{txtMyRequestTab.SetVisible(false)} }"
                        CheckedChanged="function(s,e){ if(s.GetChecked()){txtMyRequestTab.SetVisible(true)}else{txtMyRequestTab.SetVisible(false)} }" />
                </dx:ASPxCheckBox>
                </div>
                <div class="fleft padding-left5">
                <dx:ASPxTextBox ID="txtMyRequestTab" ClientInstanceName="txtMyRequestTab" runat="server"></dx:ASPxTextBox>
                </div>
            </div>
        </td>
    </tr>--%>
<%--     <tr id="tr2" runat="server">
        <td class="ms-formlabel">
            <h3 class="ms-standardheader">My Closed Requests Tab</h3>
        </td>
        <td class="ms-formbody">
            <div>
                <div class="fleft">
                <dx:ASPxCheckBox ID="chkMyClosedRequestsTab" runat="server">
                        <ClientSideEvents
                        Init="function(s,e){ if(s.GetChecked()){txtMyClosedRequestsTab.SetVisible(true)}else{txtMyClosedRequestsTab.SetVisible(false)} }"
                        CheckedChanged="function(s,e){ if(s.GetChecked()){txtMyClosedRequestsTab.SetVisible(true)}else{txtMyClosedRequestsTab.SetVisible(false)} }" />
                </dx:ASPxCheckBox>
                </div>
                <div class="fleft padding-left5">
                <dx:ASPxTextBox ID="txtMyClosedRequestsTab" ClientInstanceName="txtMyClosedRequestsTab" runat="server"></dx:ASPxTextBox>
                </div>
            </div>
        </td>
    </tr>--%>
<%--     <tr id="tr4" runat="server">
        <td class="ms-formlabel">
            <h3 class="ms-standardheader">Documents Pending Approval Tab</h3>
        </td>
        <td class="ms-formbody">
            <div>
                <div class="fleft">
                <dx:ASPxCheckBox ID="chkDocumentPendingApprovalTab" runat="server" >
                        <ClientSideEvents
                        Init="function(s,e){ if(s.GetChecked()){txtDocumentPendingApprovalTab.SetVisible(true)}else{txtDocumentPendingApprovalTab.SetVisible(false)} }"
                        CheckedChanged="function(s,e){ if(s.GetChecked()){txtDocumentPendingApprovalTab.SetVisible(true)}else{txtDocumentPendingApprovalTab.SetVisible(false)} }" />
                </dx:ASPxCheckBox>
                </div>
                <div class="fleft padding-left5">
                <dx:ASPxTextBox ID="txtDocumentPendingApprovalTab" ClientInstanceName="txtDocumentPendingApprovalTab" runat="server"></dx:ASPxTextBox>
                </div>
            </div>
        </td>
    </tr>--%>
<%--     <tr id="tr5" runat="server">
        <td class="ms-formlabel">
            <h3 class="ms-standardheader">My Tasks Tab</h3>
        </td>
        <td class="ms-formbody">
            <div>
                <div class="fleft">
                <dx:ASPxCheckBox ID="chkMyTaskTab" runat="server">
                        <ClientSideEvents
                        Init="function(s,e){ if(s.GetChecked()){txtMyTaskTab.SetVisible(true)}else{txtMyTaskTab.SetVisible(false)} }"
                        CheckedChanged="function(s,e){ if(s.GetChecked()){txtMyTaskTab.SetVisible(true)}else{txtMyTaskTab.SetVisible(false)} }" />
                </dx:ASPxCheckBox>
                </div>
                <div class="fleft padding-left5">
                <dx:ASPxTextBox ID="txtMyTaskTab" ClientInstanceName="txtMyTaskTab" runat="server"></dx:ASPxTextBox>
                </div>
            </div>
        </td>
    </tr>--%>
<%--     <tr id="tr6" runat="server">
        <td class="ms-formlabel">
            <h3 class="ms-standardheader">My Department Tab</h3>
        </td>
        <td class="ms-formbody">
            <div>
                <div class="fleft">
                <dx:ASPxCheckBox ID="chkMyDepartmentTab" runat="server">
                        <ClientSideEvents
                        Init="function(s,e){ if(s.GetChecked()){txtMyDepartmentTab.SetVisible(true)}else{txtMyDepartmentTab.SetVisible(false)} }"
                        CheckedChanged="function(s,e){ if(s.GetChecked()){txtMyDepartmentTab.SetVisible(true)}else{txtMyDepartmentTab.SetVisible(false)} }" />
                </dx:ASPxCheckBox>
                </div>
                <div class="fleft padding-left5">
                <dx:ASPxTextBox ID="txtMyDepartmentTab" ClientInstanceName="txtMyDepartmentTab" runat="server"></dx:ASPxTextBox>
                </div>
            </div>
        </td>
    </tr>--%>
<%--      <tr id="trDivisionTab" runat="server">
        <td class="ms-formlabel">
            <h3 class="ms-standardheader">My Division Tab</h3>
        </td>
        <td class="ms-formbody">
            <div>
                <div class="fleft">
                <dx:ASPxCheckBox ID="chkMyDivisionTab" runat="server">
                        <ClientSideEvents
                        Init="function(s,e){ if(s.GetChecked()){txtMyDivisionTab.SetVisible(true)}else{txtMyDivisionTab.SetVisible(false)} }"
                        CheckedChanged="function(s,e){ if(s.GetChecked()){txtMyDivisionTab.SetVisible(true)}else{txtMyDivisionTab.SetVisible(false)} }" />
                </dx:ASPxCheckBox>
                </div>
                <div class="fleft padding-left5">
                <dx:ASPxTextBox ID="txtMyDivisionTab" ClientInstanceName="txtMyDivisionTab" runat="server"></dx:ASPxTextBox>
                </div>
            </div>
        </td>
    </tr>--%>

<%--     <tr id="tr7" runat="server">
        <td class="ms-formlabel">
            <h3 class="ms-standardheader">My Projects Tab</h3>
        </td>
        <td class="ms-formbody">
            <div>
                <div class="fleft">
                <dx:ASPxCheckBox ID="chkMyProjectsTab" runat="server">
                        <ClientSideEvents
                        Init="function(s,e){ if(s.GetChecked()){txtMyProjectsTab.SetVisible(true)}else{txtMyProjectsTab.SetVisible(false)} }"
                        CheckedChanged="function(s,e){ if(s.GetChecked()){txtMyProjectsTab.SetVisible(true)}else{txtMyProjectsTab.SetVisible(false)} }" />
                </dx:ASPxCheckBox>
                </div>
                <div class="fleft padding-left5">
                <dx:ASPxTextBox ID="txtMyProjectsTab" ClientInstanceName="txtMyProjectsTab" runat="server"></dx:ASPxTextBox>
                </div>
            </div>
        </td>
    </tr>--%>
<%--      <tr>
        <td class="ms-formlabel">
             <h3 class="ms-standardheader">Enable New Button</h3>
        </td>
        <td class="ms-formbody">
            <dx:ASPxCheckBox ID="chkEnableNewButton" runat="server" ></dx:ASPxCheckBox>
        </td>
    </tr>--%>
    <%-- <tr>
        <td class="ms-formlabel">
            <h3 class="ms-standardheader">Options</h3>
        </td>
        <td class="ms-formbody">
            <div>--%>
<%--                <dx:ASPxCheckBox ID="chkSMSModules" runat="server" Text="SMS Module" TextAlign="Right"></dx:ASPxCheckBox>
                <dx:ASPxCheckBox ID="chkGovernanceModules" runat="server" Text="Governance Modules" TextAlign="Right"></dx:ASPxCheckBox>--%>
               
           <%-- </div>
            <div class="servicecatalog-option">
                <table id="tblShowServiceIcons">
                    <tr>
                        <td>
                            </td>
                    </tr>
                </table>
                
            </div>
        </td>
    </tr>--%>

<%--    <tr id="tr9" runat="server">
        <td class="ms-formlabel">
            <h3 class="ms-standardheader">Display Ordering:</h3>
        </td>
        <td class="ms-formbody">
            <div>
                <dx:ASPxComboBox ID="ddlMyTicketPanelOrder" runat="server" Width="40" CaptionSettings-ShowColon="false" CaptionSettings-Position="Right" Caption="My Ticket Panel">
                    <Items>
                        <dx:ListEditItem Text="1" Value="1" />
                        <dx:ListEditItem Text="2" Value="2" />
                        <dx:ListEditItem Text="3" Value="3" />
                    </Items>
                </dx:ASPxComboBox>
                <dx:ASPxComboBox ID="ddlServiceCatalogOrder" runat="server" Width="40" CaptionSettings-ShowColon="false"  CaptionSettings-Position="Right" Caption="Service Catalog">
                    <Items>
                        <dx:ListEditItem Text="1" Value="1" />
                        <dx:ListEditItem Text="2" Value="2" />
                        <dx:ListEditItem Text="3" Value="3" />
                    </Items>
                </dx:ASPxComboBox>
                <dx:ASPxComboBox ID="ddlModulePanelOrder" runat="server" Width="40" CaptionSettings-ShowColon="false"  CaptionSettings-Position="Right" Caption="Modules Panel">
                    <Items>
                        <dx:ListEditItem Text="1" Value="1" />
                        <dx:ListEditItem Text="2" Value="2" />
                        <dx:ListEditItem Text="3" Value="3" />
                    </Items>
                </dx:ASPxComboBox>
            </div>
        </td>
    </tr>--%>

<%--     <tr id="tr10" runat="server">
        <td class="ms-formlabel">
            <h3 class="ms-standardheader">Welcome Title:</h3>
        </td>
        <td class="ms-formbody">
            <div>
               <dx:ASPxMemo ID="txtWelcomeTitle" runat="server"></dx:ASPxMemo>
            </div>
        </td>
    </tr>--%>

<%--      <tr id="tr11" runat="server">
        <td class="ms-formlabel">
            <h3 class="ms-standardheader">Welcome Description:</h3>
        </td>
        <td class="ms-formbody">
            <div>
               <dx:ASPxMemo ID="txtWelcomeDesc" runat="server"></dx:ASPxMemo>
            </div>
        </td>
    </tr>--%>

      <%--  <tr>
        <td class="ms-formlabel">
            <h3 class="ms-standardheader">Panel</h3>
        </td>
        <td class="ms-formbody">
            
        </td>
    </tr>

</table>--%>
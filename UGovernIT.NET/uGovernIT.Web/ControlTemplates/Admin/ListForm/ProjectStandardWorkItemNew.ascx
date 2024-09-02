<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProjectStandardWorkItemNew.ascx.cs" Inherits="uGovernIT.Web.ProjectStandardWorkItemNew" %>


<div class="col-md-12 col-sm-12 col-xs-12 noPadding configVariable-popupWrap">
    <div class="ms-formtable accomp-popup">
        <div class="row">
            <div>
                <asp:Label ID="lblError" runat="server" ForeColor="Red" Visible="false"></asp:Label>
            </div>
        </div>
        <div class="row">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Title<b style="color: Red;">*</b></h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:TextBox ID="txtProjectWorkItem" runat="server" ValidationGroup="Save" Width="100%" />
                <asp:RequiredFieldValidator ID="rfvProjectWorkItem" ErrorMessage="Enter Title" ControlToValidate="txtProjectWorkItem" runat="server" CssClass="error" ValidationGroup="Save" Display="Dynamic" />
                <%--<asp:CustomValidator ID="rfcProjectWorkItem" runat="server" ValidationGroup="Save" Display="Dynamic" CssClass="error" ControlToValidate="txtProjectWorkItem" ErrorMessage="Title should be unique." OnServerValidate="rfcProjectWorkItem_ServerValidate"></asp:CustomValidator>--%>
            </div>
        </div>
        <div class="row" id="tr12" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Budget Category</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:Label ID="lbBudgetCategory" runat="server"></asp:Label>
                <asp:UpdatePanel ID="budgetPanel" runat="server">
                    <ContentTemplate>
                        <div class="col-md-6 col-sm-6 col-xs-6 noLeftPadding">
                            <asp:DropDownList ID="ddlBudgetCategory" AutoPostBack="true" runat="server" CssClass="itsmDropDownList aspxDropDownList"
                                OnSelectedIndexChanged="DDLBudgetCategory_SelectedIndexChanged"></asp:DropDownList>
                        </div>
                        <div class="col-md-6 col-sm-6 col-xs-6 noRightPadding">
                            <asp:DropDownList ID="ddlSubBudgetCategory" runat="server" CssClass="itsmDropDownList aspxDropDownList"></asp:DropDownList>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-6 noLeftPadding">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Order</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:TextBox ID="txtItemOrder" runat="server"></asp:TextBox>
                </div>
            </div>
            <div class="col-md-6 col-sm-6 col-xs-6 noRightPadding">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Code</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:TextBox ID="txtCode" runat="server"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Description</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:TextBox ID="txtDescription" runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="row addEditPopup-btnWrap">
            <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" CssClass="secondary-cancelBtn" OnClick="btnCancel_Click"></dx:ASPxButton>
            <dx:ASPxButton ValidationGroup="Task" ID="btnSave" Visible="true" runat="server" Text="Save"
                ToolTip="Save as Template"  CssClass="primary-blueBtn"  OnClick="btnSave_Click"></dx:ASPxButton>
        </div>
    </div>
  
    <dx:ASPxPopupControl ClientInstanceName="ASPxPopupClientControl" Width="250px" ID="pcMessage"
        HeaderText="Error" HeaderStyle-CssClass="error" Modal="true" ShowOnPageLoad="false" runat="server"  PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
                <asp:Panel ID="panelMessage" runat="server" HorizontalAlign="Center" style="text-align:center;">
                   <div>
                    <asp:Label ID="lblMessage" CssClass="error" runat="server"></asp:Label>
                          </div>
                    <div  style="text-align:center;padding-top:5px;display:inline-table">           
                    <asp:Button Text="Restore Archive Item" CssClass="button-bg" runat="server" ID="btnRestore"  Visible="false" />
                        </div> 
                        </asp:Panel>
            </dx:PopupControlContentControl>
        </ContentCollection>  
        <ContentStyle Paddings-Padding="20"></ContentStyle>      
    </dx:ASPxPopupControl>
</div>

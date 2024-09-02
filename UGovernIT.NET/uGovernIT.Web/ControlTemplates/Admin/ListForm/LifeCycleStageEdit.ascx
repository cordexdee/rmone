<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LifeCycleStageEdit.ascx.cs" Inherits="uGovernIT.Web.LifeCycleStageEdit" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script data-v="<%=UGITUtility.AssemblyVersion %>">
    function DeleteCongigVariable() {
        if (confirm('Are you sure want to delete?')) {
            <%=Page.ClientScript.GetPostBackEventReference(lnkDelete1,string.Empty)%>
         }
    }
    function CloseWithoutSaving()
    {
        var sourceURL = "<%= Request["source"] %>";
        window.parent.CloseWindowCallback(sourceURL);
          
    }
</script>

<div class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap">
    <asp:Panel runat="server" ID="lifecycleForm" CssClass="full-width">
        <div class="full-width">
            <asp:Label ID="lbMessage" runat="server" ForeColor="Green"></asp:Label>
        </div>
        <div class="ms-formtable accomp-popup">
            <div class="row" id="trTitle" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Title:<b style="color: Red;">*</b></h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:TextBox ID="txtTitle" CssClass="asptextbox-asp" runat="server" ValidationGroup="Task"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvTitle" runat="server" ValidationGroup="Task" ControlToValidate="txtTitle"
                        Display="Dynamic" ErrorMessage="Please enter title" ForeColor="Red"></asp:RequiredFieldValidator>
                    <asp:CustomValidator ID="cvTitle" ValidationGroup="Task" runat="server" ControlToValidate="txtTitle" ErrorMessage="Stage with this name already exists"
                        OnServerValidate="cvTitle_ServerValidate" Display="Dynamic" ForeColor="Red"></asp:CustomValidator>
                </div>
            </div>
            <div class="row" id="trStep" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Step:<b style="color: Red;">*</b></h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:TextBox CssClass="asptextbox-asp" ID="txtStep" runat="server" ValidationGroup="Task"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvStep" runat="server" ValidationGroup="Task" ControlToValidate="txtStep"
                        Display="Dynamic" ErrorMessage="Please enter step" ForeColor="Red"></asp:RequiredFieldValidator>
                    <asp:RangeValidator ID="rvStep" ValidationGroup="Task" runat="server" ControlToValidate="txtStep" MinimumValue="1" MaximumValue="100"
                        ErrorMessage="Step number must be between 1-100." ForeColor="Red" Display="Dynamic" Type="Integer"></asp:RangeValidator>
                    <asp:CustomValidator ID="cvStep" ValidationGroup="Task" ForeColor="Red" runat="server" ControlToValidate="txtStep" ErrorMessage="Step Number is already assigned to other stage."
                        OnServerValidate="cvStep_ServerValidate" Display="Dynamic"></asp:CustomValidator>
                </div>
            </div>
            <div class="row" id="trWeight" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Weight:<b style="color: Red;">*</b></h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:TextBox CssClass="asptextbox-asp" ID="txtWeight" runat="server" ValidationGroup="Task"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvWeight" runat="server" ValidationGroup="Task" ControlToValidate="txtWeight"
                        Display="Dynamic" ErrorMessage="Please enter weight"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ValidationGroup="Task" ID="revWeight" Display="Dynamic" runat="server" ControlToValidate="txtWeight" ValidationExpression="[0-9]*"
                        ErrorMessage="Weight must be a positive whole number."></asp:RegularExpressionValidator>
                </div>
            </div>
            <div class="row"  id="trIconUrl" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Icon Url</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <ugit:UGITFileUploadManager ID="UGITFileUploadManager1" width="100%" runat="server" AnchorLabel="Upload Icon" hideWiki="true" />
                </div>
           </div>
            <div class="row">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Normal Capacity:<b style="color: Red;">*</b></h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:TextBox CssClass="asptextbox-asp" ID="txtCapacityNormal" runat="server" ValidationGroup="Task"></asp:TextBox>
                    <asp:RegularExpressionValidator ValidationGroup="Task" ID="RegularExpressionValidator1" Display="Dynamic" runat="server"
                        ControlToValidate="txtCapacityNormal" ValidationExpression="[0-9]*"
                        ErrorMessage="Normal Capacity must be a positive whole number."></asp:RegularExpressionValidator>
                </div>
            </div>
           <div class="row">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Max Capacity:<b style="color: Red;">*</b></h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:TextBox CssClass="asptextbox-asp" ID="txtCapacityMax" runat="server" ValidationGroup="Task"></asp:TextBox>
                    <asp:RegularExpressionValidator ValidationGroup="Task" ID="RegularExpressionValidator2" Display="Dynamic" runat="server"
                        ControlToValidate="txtCapacityMax" ValidationExpression="[0-9]*"
                        ErrorMessage="Max Capacity must be a positive whole number."></asp:RegularExpressionValidator>
                </div>
            </div>
            <div class="row" id="trNote" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Description:</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:TextBox CssClass="asptextbox-asp" Rows="5" ID="txtDescription" runat="server" TextMode="MultiLine"></asp:TextBox>
                </div>
            </div>
            <div class="row">
                <div class="addEditPopup-btnWrap">
                     <dx:ASPxButton ValidationGroup="Task" ID="btSaveLifeCycleStage" Visible="true" runat="server" Text="Save"
                        ToolTip="Save as Template" Style="float:right;margin-right:16px;" CssClass="primary-blueBtn"  OnClick="btSaveLifeCycleStage_Click">
                     </dx:ASPxButton>
                     <dx:ASPxButton ID="btnDelete" runat="server" Visible="true" Text="Delete" ToolTip="Delete" AutoPostBack="false" CssClass="secondary-cancelBtn">
                        <ClientSideEvents Click="function(s,e){DeleteCongigVariable();}" />
                     </dx:ASPxButton>
                     <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" CssClass="secondary-cancelBtn" OnClick="btnCancel_Click"></dx:ASPxButton>
                    
                    <asp:LinkButton ID="lnkDelete1" runat="server" OnClick="LnkbtnDelete_Click"></asp:LinkButton>
                </div>
            </div>
        </div>
    </asp:Panel>
</div>

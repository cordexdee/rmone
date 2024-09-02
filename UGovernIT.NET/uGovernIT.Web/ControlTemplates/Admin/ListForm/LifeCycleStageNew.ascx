
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LifeCycleStageNew.ascx.cs" Inherits="uGovernIT.Web.LifeCycleStageNew" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<script data-v="<%=UGITUtility.AssemblyVersion %>">
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
                    <h3 class="ms-standardheader budget_fieldLabel">Title1:<b style="color: Red;">*</b></h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:TextBox ID="txtTitle" CssClass="asptextbox-asp" runat="server" ValidationGroup="Task"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvTitle" runat="server" ValidationGroup="Task" ControlToValidate="txtTitle"
                        Display="Dynamic" ForeColor="Red" ErrorMessage="Please enter title."></asp:RequiredFieldValidator>
                    <asp:CustomValidator ID="cvTitle" ValidationGroup="Task" runat="server" ControlToValidate="txtTitle" ErrorMessage="Stage with this name already exists"
                        OnServerValidate="cvTitle_ServerValidate" Display="Dynamic"></asp:CustomValidator>
                </div>
            </div>
            <div class="row" id="trStep" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Step:<b style="color: Red;">*</b></h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:TextBox CssClass="asptextbox-asp" ID="txtStep" runat="server" ValidationGroup="Task"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvStep" ValidationGroup="Task" runat="server" ControlToValidate="txtStep" ErrorMessage="Please Enter Step"
                         Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                    <asp:RangeValidator ID="rvStep" ValidationGroup="Task" runat="server" ControlToValidate="txtStep" MinimumValue="1" MaximumValue="100"
                        ErrorMessage="Step number must be between 1-100." ForeColor="Red" Display="Dynamic" Type="Integer"></asp:RangeValidator>
                    <asp:CustomValidator ID="cvStep" ValidationGroup="Task" runat="server" ControlToValidate="txtStep" ErrorMessage="Step Number is already assigned to other stage."
                        OnServerValidate="cvStep_ServerValidate" ForeColor="Red" Display="Dynamic"></asp:CustomValidator>
                </div>
            </div>
            <div class="row" id="trWeight" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Weight:</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:TextBox CssClass="asptextbox-asp" ID="txtWeight" runat="server" ValidationGroup="Task"></asp:TextBox>
                    <asp:RegularExpressionValidator ValidationGroup="Task" ID="revWeight" Display="Dynamic" runat="server" ControlToValidate="txtWeight" ValidationExpression="[0-9]*"
                        ErrorMessage="Weight must be a positive whole number." ForeColor="Red"></asp:RegularExpressionValidator>
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
            <div id="trNote" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Description:</h3>
                </div>
                <div class="ms-formbody">
                    <asp:TextBox CssClass="asptextbox-asp" Rows="5" ID="txtDescription" runat="server" TextMode="MultiLine"></asp:TextBox>
                </div>
            </div>
            <div class="row addEditPopup-btnWrap">
                <dx:ASPxButton ValidationGroup="Task" ID="btSaveLifeCycleStage" Visible="true" runat="server" Text="Save"
                    ToolTip="Save as Template" Style="float:right;" CssClass="primary-blueBtn"  OnClick="btSaveLifeCycleStage_Click">
                </dx:ASPxButton>
                <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" style="float: right;" CssClass=" secondary-cancelBtn" OnClick="btnCancel_Click"></dx:ASPxButton>
            </div>
        </div>
    </asp:Panel>
</div>

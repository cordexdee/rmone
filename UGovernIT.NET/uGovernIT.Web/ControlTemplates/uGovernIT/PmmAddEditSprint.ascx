<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PmmAddEditSprint.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.uGovernIT.PmmAddEditSprint" %>

<asp:HiddenField ID="hdnSprintId" runat="server" />

<dx:PopupControlContentControl ID="PopupControlContentControl" runat="server">
    <div id="divSprintPoup" class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap">
        <div class="ms-formtable accomp-popup">
            <div class="row">
                <div class="ms-formlabel" style="padding-top:10px;">
                    <h3 class="ms-standardheader budget_fieldLabel">Title<b style="color: Red;">*</b></h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:TextBox ID="txtTitle" runat="server" ValidationGroup="Task" onchange="txtTitleChange()"></asp:TextBox>
                    <asp:Label ID="lblTitleError" runat="server" Style="display: none;" ForeColor="Red"></asp:Label>
                    <asp:RequiredFieldValidator ID="rfvText" runat="server" ValidationGroup="Task" ControlToValidate="txtTitle"
                        Display="Dynamic" ErrorMessage="Please enter title." ForeColor="Red"></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="row">
                <div class="colXs col-md-6 noLeftPadding">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Start Date<b style="color: Red;">*</b>
                        </h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <%--<SharePoint:DateTimeControl DateOnly="true" CssClassTextBox="dateControl dtStartDate"
                                                    ID="dtcStartDate" runat="server" SelectedDate="10/10/2014 12:35:17"></SharePoint:DateTimeControl>--%>
                        <dx:ASPxDateEdit ID="dtcStartDate" ClientInstanceName="dtcStartDate" EditFormat="Date" EditFormatString="MM/dd/yyyy"
                            CssClass="CRMDueDate_inputField" TextBox="dateControl dtStartDate" runat="server"
                            AutoPostBack="false" DropDownButton-Image-Width="18"
                            DropDownButton-Image-Url="~/Content/Images/calendarNew.png">
                        </dx:ASPxDateEdit>
                        <asp:Label ID="lbStartDate" runat="server" Style="display: none;" ForeColor="Red"></asp:Label>
                    </div>
                </div>
                <div class="colXs col-md-6 noRightPadding">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">End Date<b style="color: Red;">*</b>
                        </h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <%--<SharePoint:DateTimeControl DateOnly="true"
                                                    ID="dtcEndDate" runat="server" CssClassTextBox="dateControl dtEndDate" SelectedDate="10/10/2014 12:35:17"></SharePoint:DateTimeControl>--%>
                        <dx:ASPxDateEdit ID="dtcEndDate" CssClassTextBox="dateControl dtEndDate" runat="server"
                            EditFormatString="MM/dd/yyyy" NullText="MM/dd/yyyy" AutoPostBack="false"
                            CssClass="CRMDueDate_inputField" DropDownButton-Image-Width="18"
                            DropDownButton-Image-Url="~/Content/Images/calendarNew.png">
                        </dx:ASPxDateEdit>
                        <asp:Label ID="dtcDateError" runat="server" Style="display: none;" ForeColor="Red"></asp:Label>
                    </div>
                </div>
            </div>
            <div class="row SprintPdding">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Duration<b style="color: Red;">*</b></h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:Label ID="lblSprintDuration" runat="server"></asp:Label>
                </div>
            </div>
            <div class="row addEditPopup-btnWrap">
                <dx:ASPxButton runat="server" ID="lnkDeleteSprintPopUp" Visible="false" Text="Delete Tasks"
                    CssClass="secondary-cancelBtn">
                    <ClientSideEvents Click="return lnkDeleteTask();" />
                </dx:ASPxButton>
                <dx:ASPxButton CssClass="primary-blueBtn" AutoPostBack="true" ImagePosition="Right" ValidationGroup="Task"
                    ID="lnkSaveSprint" runat="server" Text="Save" OnClick="lnkSaveSprint_Click">
                </dx:ASPxButton>
            </div>
        </div>
    </div>
</dx:PopupControlContentControl>

<script type="text/javascript">
    function txtTitleChange() {
        $("#<%=lblTitleError.ClientID%>").css("display", "none");
        }
</script>

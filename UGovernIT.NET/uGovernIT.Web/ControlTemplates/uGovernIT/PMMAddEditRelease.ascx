<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PMMAddEditRelease.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.uGovernIT.PMMAddEditRelease" %>


<asp:HiddenField ID="hdnReleaseId" runat="server" />
<contentcollection>
    <dx:PopupControlContentControl ID="PopupControlContentControlRelease" runat="server">
        <div id="div2" class="col-md-12 col-sm-12 col-xs-12 noPadding configVariable-popupWrap popupheight">
            <div class="ms-formtable accomp-popup">
                <div class="row">
                    <div class="colXs noLeftPadding">
                        <div class="ms-formlabel">
                            <h3 class="ms-standardheader budget_fieldLabel">Release ID<b style="color: Red;">*</b>
                            </h3>
                        </div>
                        <div class="ms-formbody accomp_inputField">
                            <asp:TextBox ID="txtReleaseID" runat="server" onchange="txtDataChange(this)"></asp:TextBox>
                            <asp:Label ID="lblReleaseID" runat="server" Style="display: none;" ForeColor="Red" errorLabel="true"></asp:Label>
                        </div>
                    </div>
                    <div class="colXs noRightPadding">
                        <div class="ms-formlabel">
                            <h3 class="ms-standardheader budget_fieldLabel">Title<b style="color: Red;">*</b>
                            </h3>
                        </div>
                        <div class="ms-formbody accomp_inputField">
                            <asp:TextBox ID="txtRelease" runat="server" onchange="txtDataChange(this)"></asp:TextBox>
                            <asp:Label ID="lblRelease" runat="server" Style="display: none;" ForeColor="Red" errorLabel="true"></asp:Label>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="colXs noLeftPadding">
                        <div class="ms-formlabel">
                            <h3 class="ms-standardheader budget_fieldLabel">Release Date<b style="color: Red;">*</b>
                            </h3>
                        </div>
                        <div class="ms-formbody accomp_inputField">
                            <%--<SharePoint:DateTimeControl DateOnly="true" CssClassTextBox="dateChangeRelease"
                                                        ID="dtcReleaseDate" runat="server" SelectedDate="10/10/2014 12:35:17"></SharePoint:DateTimeControl>--%>
                            <dx:ASPxDateEdit ID="dtcReleaseDate" ClientInstanceName="dtcReleaseDate" EditFormat="Date"
                                CssClass="CRMDueDate_inputField" CssClassTextBox="dateControl dtStartDate" runat="server"
                                AutoPostBack="false" DropDownButton-Image-Width="18"
                                DropDownButton-Image-Url="~/Content/Images/calendarNew.png">
                            </dx:ASPxDateEdit>
                            <asp:Label ID="lblReleaseDate" runat="server" Style="display: none;" ForeColor="Red" errorLabel="true"></asp:Label>
                        </div>
                    </div>
                </div>
                <div class="row addEditPopup-btnWrap">
                    <dx:ASPxButton runat="server" ID="lnkDeleteReleasePopup" Visible="false" Text="Delete Release"
                        CssClass="secondary-cancelBtn">
                        <ClientSideEvents Click="return lnkDeleteRelease();" />
                    </dx:ASPxButton>
                    <dx:ASPxButton ID="lnkSaveRelease" Visible="true" runat="server" Text="Save" OnClick="lnkSaveRelease_Click"
                        CssClass="primary-blueBtn" ToolTip="Submit">
                    </dx:ASPxButton>
                </div>
            </div>
        </div>
    </dx:PopupControlContentControl>

</contentcollection>
<script>
    function txtDataChange(obj) {

        errorLabelObj = $(obj).siblings('span[errorLabel]');

        if (errorLabelObj != undefined) {
            $(errorLabelObj).text('');
        }
    }
</script>

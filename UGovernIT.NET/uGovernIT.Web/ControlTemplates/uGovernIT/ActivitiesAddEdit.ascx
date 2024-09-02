<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ActivitiesAddEdit.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.uGovernIT.ActivitiesAddEdit" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .activityInfo-label{
        color: #4A6EE2;
        font-size: 14px;
        padding-left: 5px;
    }
    .ms-formbody {
        padding: 3px 6px 4px;
    }

   
</style>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    //function closeFrame() {

    //    window.frameElement.commitPopup();
    //}
    $(document).ready(function () {
        $('.fetch-parent').parent().addClass("activityAddEdit-container");
        $('.userValueBox-Table').parent().addClass("userValueBox-searchFilterWrap");
        $('.userValueBox-searchFilterWrap').parent().addClass("userValueBox-searchFilterContainer");
        $('.aspxComboBox-listBox').parent().addClass('aspxCoboBox-listboxWrap')
    });
</script>




<div class="fetch-parent col-md-12 col-sm-12 col-xs-12">
    <fieldset>
        <%--<p class="activityInfo-label">Activity Info</p>--%>
        <div class="ms-formtable accomp-popup row">
            <div class="col-md-6 col-sm-6 col-xs-12 noPadding" id="trContact" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Contact<i class="required-item"></i></h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <dx:ASPxComboBox ID="ddlContact" runat="server" ValueType="System.String" Width="100%"
                        DropDownStyle="DropDown" IncrementalFilteringMode="Contains" CallbackPageSize="100"  
                        CssClass="CRMDueDate_inputField aspxComBox-dropDown" ListBoxStyle-CssClass="aspxComboBox-listBox">
                    </dx:ASPxComboBox>

                    <div>
                        <asp:RequiredFieldValidator ID="rfvContact" runat="server" ControlToValidate="ddlContact"
                            ErrorMessage="* Enter contact" CssClass="text-error" Display="Dynamic" ValidationGroup="Save"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div class="col-md-6 col-sm-6 col-xs-12 noPadding">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Title *<i class="required-item"></i></h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <asp:TextBox ID="txtTitle" runat="server" CssClass="full-width" />
                        <div>
                            <asp:RequiredFieldValidator ID="rfvTitle" runat="server" ControlToValidate="txtTitle"
                                ErrorMessage="Enter Title" ForeColor="Red" CssClass="text-error" Display="Dynamic" ValidationGroup="Save"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
           
            <div runat="server" id="trAssignee">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Assignee</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <%--<SharePoint:PeopleEditor PrincipalSource="UserInfoList" ID="peAssignee" Width="280px" runat="server" MultiSelect="false" PlaceButtonsUnderEntityEditor="false" AutoPostBack="true"
                        SelectionSet="User" AugmentEntitiesFromUserInfo="false" AllowEmpty="true" />--%>
                    <ugit:UserValueBox ID="peAssignee" SelectionSet="User" runat="server" MultiSelect="false"
                        CssClass="peAssignedTo userValueBox-dropDown"/>
                </div>
            </div>
            <div id="trActivity" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Activity</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:TextBox ID="txtDesc" runat="server" TextMode="MultiLine" CssClass="crmActivity_inputField"></asp:TextBox>
                </div>
            </div>
            <div class="col-md-6 col-sm-6 col-xs-6" runat="server" id="trWhendue"  >
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">When Due *</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <%-- <SharePoint:DateTimeControl DateOnly="true" ID="dtcRecurrEndDate" runat="server"
                        CssClassTextBox="edit-startdate datetimectr datetimectr111 " IsRequiredField="true"></SharePoint:DateTimeControl>--%>
                    <dx:ASPxDateEdit ID="dtcRecurrEndDate" runat="server" CssClass="edit-startdate datetimectr datetimectr111 CRMDueDate_inputField dateEdit-dropDown"                             
                        IsRequiredField="true" DropDownButton-Image-Url="~/Content/Images/calendarNew.png" CalendarProperties-FooterStyle-CssClass="calenderFooterWrap" DropDownButton-Image-Width="18"></dx:ASPxDateEdit>
                    <div>
                        <asp:RequiredFieldValidator ID="rfvRecurrEndDate" runat="server" ControlToValidate="dtcRecurrEndDate"
                            ErrorMessage="Enter Due date" ForeColor="Red" CssClass="text-error" Display="Dynamic" ValidationGroup="Save"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div id="trStatus" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Status</h3>
                </div>
                <div class="ms-formbody accomp_inputField addEditActivity">
                    <dx:ASPxComboBox ID="ddlStatus" runat="server" ValueType="System.String" Width="100%" 
                        ListBoxStyle-CssClass="aspxComboBox-listBox addEditActivity-listBox"
                        IncrementalFilteringMode="StartsWith" DropDownStyle="DropDownList" 
                        CssClass="aspxComBox-dropDown">
                    </dx:ASPxComboBox>
                </div>
            </div>
            <div class="col-md-6 col-sm-6 colForXS" style="display: none;">
                <div >
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Contact Person</h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <dx:ASPxComboBox ID="ddlContactPerson" runat="server" ValueType="System.String"
                            IncrementalFilteringMode="StartsWith" DropDownStyle="DropDownList">
                        </dx:ASPxComboBox>
                    </div>
                </div>
            </div>
        </div>
        <div class="row addEditPopup-btnWrap">
            <dx:ASPxButton ID="LnkbtnDelete" runat="server" CssClass="secondary-cancelBtn" ToolTip="Delete" OnClick="LnkbtnDelete_Click" Text="Delete">
                <ClientSideEvents Click="function(s, e){return confirm('Are you sure you want to delete?');}"/>
            </dx:ASPxButton>
            <dx:ASPxButton ID="btnCancel" runat="server" ToolTip="Cancel" OnClick="btnCancel_Click" Text="Cancel" 
                CssClass="secondary-cancelBtn"></dx:ASPxButton>
            <dx:ASPxButton ID="btnSave" runat="server" ToolTip="Save" Text="Save" ValidationGroup="Save" CssClass="primary-blueBtn"
                OnClick="btnSave_Click"></dx:ASPxButton>
        </div>
    </fieldset>
</div>

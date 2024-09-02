<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="QueryExpression.ascx.cs" Inherits="uGovernIT.Web.QueryExpression" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .ms-formlabel {
        width: 170px;
        text-align: right;
    }

    .full-width {
        width: 90%;
    }

    .ms-formbody {
        background: none repeat scroll 0 0 #E8EDED;
        border-top: 1px solid #A5A5A5;
        padding: 3px 6px 4px;
        vertical-align: top;
    }

    .text-error {
        color: red;
        font-weight: 500;
        margin-top: 5px;
    }

    div.ms-inputuserfield {
        height: 17px;
    }

    .btnDelete {
        float: left;
        margin: 1px;
        color: #fff !important;
        background: url(/_layouts/15/images/uGovernIT/firstnavbgRed.png) repeat-x;
        cursor: pointer;
        padding: 6px;
    }

    .required-item:after {
        content: '*';
        color: red;
        font-weight: bold;
    }
</style>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">

    function validateOperator(ddl) {
        var selectedText = $('option:selected', ddl).text();
        $('#<%= ddlOperator.ClientID%> option:contains("-")').prop("disabled", false);
        $('#<%= ddlOperator.ClientID%> option:contains("+")').prop("disabled", false);
        $('#<%= ddlOperator.ClientID%> option:contains("/")').prop("disabled", false);
        $('#<%= ddlOperator.ClientID%> option:contains("*")').prop("disabled", false);
        $('#<%= ddlOperator.ClientID%> option:contains("And")').prop("disabled", true);
        $('#<%= ddlOperator.ClientID%> option:contains("Or")').prop("disabled", true);
        $('#<%= ddlOperator.ClientID%> option:contains("Substring")').prop("disabled", true);
        $('#<%= ddlOperator.ClientID%> option:contains("Month Year")').prop("disabled", true);
        $('#<%= ddlOperator.ClientID%> option:contains("Year Month")').prop("disabled", true);
        $('#<%= ddlOperator.ClientID%> option:contains("Month")').prop("disabled", true);
        $('#<%= ddlOperator.ClientID%> option:contains("Year")').prop("disabled", true);

        if (selectedText.toLowerCase().indexOf("(datetime)") >= 0 || selectedText.toLowerCase().indexOf("(date)") >= 0) {
            <%--$('#<%= ddlOperator.ClientID%> option:contains("-")').attr('selected', 'selected');--%>
            $('#<%= ddlOperator.ClientID%> option:contains("-")').prop("disabled", false);
            $('#<%= ddlOperator.ClientID%> option:contains("+")').prop("disabled", true);
            $('#<%= ddlOperator.ClientID%> option:contains("/")').prop("disabled", true);
            $('#<%= ddlOperator.ClientID%> option:contains("*")').prop("disabled", true);
            $('#<%= ddlOperator.ClientID%> option:contains("And")').prop("disabled", true);
            $('#<%= ddlOperator.ClientID%> option:contains("Or")').prop("disabled", true);
            $('#<%= ddlOperator.ClientID%> option:contains("Substring")').prop("disabled", true);
            $('#<%= ddlOperator.ClientID%> option:contains("Month Year")').prop("disabled", false);
            $('#<%= ddlOperator.ClientID%> option:contains("Year Month")').prop("disabled", false);
            $('#<%= ddlOperator.ClientID%> option:contains("Month")').prop("disabled", false);
            $('#<%= ddlOperator.ClientID%> option:contains("Year")').prop("disabled", false);
        }
        else if (selectedText.toLowerCase().indexOf("(string)") >= 0) {
            <%--$('#<%= ddlOperator.ClientID%> option:contains("+")').attr('selected', 'selected');--%>
            $('#<%= ddlOperator.ClientID%> option:contains("+")').prop("disabled", false);
            $('#<%= ddlOperator.ClientID%> option:contains("-")').prop("disabled", true);
            $('#<%= ddlOperator.ClientID%> option:contains("/")').prop("disabled", true);
            $('#<%= ddlOperator.ClientID%> option:contains("*")').prop("disabled", true);
            $('#<%= ddlOperator.ClientID%> option:contains("And")').prop("disabled", true);
            $('#<%= ddlOperator.ClientID%> option:contains("Or")').prop("disabled", true);
            $('#<%= ddlOperator.ClientID%> option:contains("Substring")').prop("disabled", false);
            $('#<%= ddlOperator.ClientID%> option:contains("Month Year")').prop("disabled", true);
            $('#<%= ddlOperator.ClientID%> option:contains("Year Month")').prop("disabled", true);
            $('#<%= ddlOperator.ClientID%> option:contains("Month")').prop("disabled", true);
            $('#<%= ddlOperator.ClientID%> option:contains("Year")').prop("disabled", true);
        }
        else if (selectedText.toLowerCase().indexOf("(boolean)") >= 0) {
            $('#<%= ddlOperator.ClientID%> option:contains("And")').prop("disabled", false);
            $('#<%= ddlOperator.ClientID%> option:contains("Or")').prop("disabled", false);

            $('#<%= ddlOperator.ClientID%> option:contains("And")').attr('selected', 'selected');

            $('#<%= ddlOperator.ClientID%> option:contains("-")').prop("disabled", true);
            $('#<%= ddlOperator.ClientID%> option:contains("/")').prop("disabled", true);
            $('#<%= ddlOperator.ClientID%> option:contains("*")').prop("disabled", true);
            $('#<%= ddlOperator.ClientID%> option:contains("+")').prop("disabled", true);
            $('#<%= ddlOperator.ClientID%> option:contains("Substring")').prop("disabled", true);
            $('#<%= ddlOperator.ClientID%> option:contains("Month Year")').prop("disabled", true);
            $('#<%= ddlOperator.ClientID%> option:contains("Year Month")').prop("disabled", true);
            $('#<%= ddlOperator.ClientID%> option:contains("Month")').prop("disabled", true);
            $('#<%= ddlOperator.ClientID%> option:contains("Year")').prop("disabled", true);
        }
    }

    $(function () {
        validateOperator("#<%= ddlFirstColumn.ClientID%>");

        $("#<%= ddlFirstColumn.ClientID%>").change(function () {
            validateOperator(this);
        });
    });

    function ChangeOperator(objddl) {
        var selectedText = $('option:selected', objddl).text();
        if (selectedText == "Substring") {
            $('#<%=trSecondColumn.ClientID%>').css("visibility", "hidden");
        }
        else if (selectedText == "Month Year" || selectedText == "Year Month" || selectedText == "Year" || selectedText == "Month") {
            $('#<%=trSecondColumn.ClientID%>').css("visibility", "hidden");
        }
        else {
            $('#<%=trSecondColumn.ClientID%>').css("visibility", "visible");
        }
    }
</script>


<table cellpadding="0" cellspacing="0" style="border-collapse: collapse; width: 100%">
    <tr>
        <td class="ms-formlabel">
            <h6>Expression Name<i class="required-item"></i></h6>
        </td>
        <td class="ms-formbody">
            <dx:ASPxComboBox ID="cbExpName" runat="server" DropDownStyle="DropDown" IncrementalFilteringMode="StartsWith" Width="212px" 
                OnSelectedIndexChanged="cbExpName_SelectedIndexChanged" AutoPostBack="true"></dx:ASPxComboBox>
            <asp:RequiredFieldValidator ID="rfvExpName" runat="server" ValidationGroup="Save" ControlToValidate="cbExpName"
                Display="Dynamic" InitialValue="" CssClass="text-error">
                <span>Please enter/select an Expression.</span>
            </asp:RequiredFieldValidator>
        </td>
    </tr>
     <tr>
        <td class="ms-formlabel">
            <h6>Expression Display Name<i class="required-item"></i></h6>
        </td>
        <td class="ms-formbody">
            <asp:TextBox ID="txtExpDisplayName" runat="server" Width="200px" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ValidationGroup="Save" ControlToValidate="txtExpDisplayName"
                Display="Dynamic" CssClass="text-error">
                 <span><br />Please enter Expression Display Name.</span>
            </asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr>
        <td class="ms-formlabel">
            <h6>DataType<i class="required-item"></i></h6>
        </td>
        <td class="ms-formbody">
               <asp:DropDownList ID="ddlDataType" runat="server">
                <asp:ListItem Text="Currency" />
                <asp:ListItem Text="Double" />
                <asp:ListItem Text="Percent" />
                <asp:ListItem Text="Percent*100" />
                <asp:ListItem Text="Integer" />
                <asp:ListItem Text="DateTime" />
                <asp:ListItem Text="Date" />
                <asp:ListItem Text="Days" />
                <asp:ListItem Text="Hours" />
                <asp:ListItem Text="Minutes" />
                <asp:ListItem Text="Time" />
                <asp:ListItem Text="String" Selected="True" />
                <asp:ListItem Text="Progress" />
                <asp:ListItem Text="Boolean" />
                <asp:ListItem Text="User" />
                <asp:ListItem Text="MultiUser" />
            </asp:DropDownList>
        </td>
    </tr>

    <tr>
        <td class="ms-formlabel">
            <h6>First Column<i class="required-item"></i></h6>
        </td>
        <td class="ms-formbody">
             <asp:DropDownList ID="ddlFirstColumn" runat="server" Width="212px">
             </asp:DropDownList>
        </td>
    </tr>

    <tr>
        <td class="ms-formlabel">
            <h6>Operator<i class="required-item"></i></h6>
        </td>
        <td class="ms-formbody">
               <asp:DropDownList ID="ddlOperator" runat="server" onchange="ChangeOperator(this)">
                <asp:ListItem Text="+" />
                <asp:ListItem Text="-" />
                <asp:ListItem Text="*" />
                <asp:ListItem Text="/" />      
                <asp:ListItem Text="And" />
                <asp:ListItem Text="Or" />
                <asp:ListItem Text="Substring" />
                <asp:ListItem Text="Month Year" />
                <asp:ListItem Text="Year Month" />
                <asp:ListItem Text="Month" />
                <asp:ListItem Text="Year" />
            </asp:DropDownList>
        </td>        
    </tr>

     <tr id="trSecondColumn" runat="server">
        <td class="ms-formlabel">
            <h6>Second Column<i class="required-item"></i></h6>
        </td>
        <td class="ms-formbody">
          <asp:DropDownList ID="ddlSecondColumn" runat="server" Width="212px">
          </asp:DropDownList>
        </td>
    </tr>

    <tr>
        <td>
            <div class="addEditPopup-btnWrap" style="float: left !important; padding-left: 10px !important;">
                <dx:ASPxButton ID="lnkDelete" runat="server" Text="Delete" ToolTip="Delete" CssClass="secondary-cancelBtn"  OnClick="lnkDelete_Click"></dx:ASPxButton>
            </div>
        </td>
        <td style="float: right; padding-top: 10px;">
            <div class="addEditPopup-btnWrap">
                <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel" CssClass="secondary-cancelBtn"  OnClick="btnCancel_Click"></dx:ASPxButton>
                <dx:ASPxButton ID="btnUpdate" runat="server" Text="Save" ToolTip="Save" CssClass="primary-blueBtn" ValidationGroup="Save" OnClick="btnUpdate_Click"></dx:ASPxButton>
            </div>
        </td>
    </tr>
</table>                                              
                                                      
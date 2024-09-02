<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddProjectSimilarityConfig.ascx.cs" Inherits="uGovernIT.Web.AddProjectSimilarityConfig" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>


<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">

    function hideShowEdit(mainClass) {
        var jsMain = $("." + mainClass);
        var dropdown = jsMain.find("select");
        var editIcon = jsMain.find(".editicon");
        editIcon.hide();
        if (dropdown.val() != "") {
            editIcon.show();
        }
    }
    function hideddlMetricType(action) {
        var category = $("#<%=ddlMetricType.ClientID%> option:selected").text();
        $("#<%=ddlMetricType.ClientID%>").val('');
        $(".ddlMetricType").hide();
        $("#<%=hdnMetricType.ClientID%>").val('');
        if (action == 1) {
            $("#<%=hdnRequestMetricType.ClientID%>").val(category);
            $("#<%=txtMetricType.ClientID%>").val(category);
        }
        else {
            $("#<%=hdnRequestMetricType.ClientID%>").val("");
            $("#<%=txtMetricType.ClientID%>").val("");
        }
    }
    function showddlMetricType() {
        $(".ddlMetricType").show();
        $("#<%=hdnMetricType.ClientID%>").val('');
        $("#<%=ddlMetricType.ClientID%>").val('');
    }

    $(function () {
        hideShowEdit('ddlMetricType');
    });
</script>

<div class="row ms-formtable accomp-popup py-3">
        <div class="col-md-6 col-sm-6 col-xs-6">
            <div id="Div3" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Module<b style="color: Red;">*</b></h3>
                </div>
                <div class="pb-2">
                   <asp:DropDownList ID="ddlModule" runat="server" AutoPostBack="true" CssClass="itsmDropDownList aspxDropDownList" ></asp:DropDownList>
                    <div>
                        <asp:RequiredFieldValidator ID="rfvddlModule" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="ddlModule"
                            ErrorMessage="Select Module "  Display="Dynamic" ValidationGroup="Save" ForeColor="Red"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div id="Div1" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Column Name</h3>
                </div>
                <div class="pb-2">
<%--                    <asp:TextBox ID="txtColumnName" runat="server" />
                    <div>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="txtColumnName"
                            ErrorMessage="Enter Column Name" Display="Dynamic" ForeColor="Red" ValidationGroup="Save"></asp:RequiredFieldValidator>
                    </div>--%>

                    <dx:ASPxComboBox ID="cmbFieldName" runat="server" Width="100%"
                        DropDownStyle="DropDown" TextFormatString="{0}" ListBoxStyle-CssClass="aspxComboBox-listBox"
                        ValueType="System.String" IncrementalFilteringMode="Contains" FilterMinLength="0" EnableSynchronization="True"
                        CallbackPageSize="10">
                        <Columns>
                        </Columns>
                    </dx:ASPxComboBox>
                    <div>
                        <asp:RequiredFieldValidator ID="rfvFieldName" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="cmbFieldName"
                            ErrorMessage="Enter Field Name" Display="Dynamic" ValidationGroup="Save" ForeColor="Red"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div id="tr12" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Title</h3>
                </div>
                <div class="pb-2">
                    <asp:TextBox ID="txtTitle" runat="server" />
                    <div>
                        <asp:RequiredFieldValidator ID="rfvtxtTitle" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="txtTitle"
                            ErrorMessage="Enter Title" Display="Dynamic" ForeColor="Red" ValidationGroup="Save"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div id="Div4" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Weight</h3>
                </div>
                <div class="pb-2">
                    <asp:TextBox ID="txtWeight" runat="server" />
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                         ControlToValidate="txtWeight"
                         ErrorMessage="Enter Number" ForeColor="Red"
                         ValidationExpression="^[0-9]*$" ValidationGroup="Save">
                    </asp:RegularExpressionValidator>
                </div>
            </div>
        </div>
        <div class="col-md-6 col-sm-6 col-xs-6">
            <%--<div id="Div5" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Metric Type<b style="color: Red;">*</b></h3>
                </div>
                <div class="pb-2">
                   <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="true" CssClass="itsmDropDownList aspxDropDownList" ></asp:DropDownList>
                    <div>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="ddlModule"
                            ErrorMessage="Select Metric Type "  Display="Dynamic" ValidationGroup="Save" ForeColor="Red"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>--%>
           <div class="row" id="trMetricType" runat="server">
                        <div class="ms-formlabel">
                            <h3 class="ms-standardheader budget_fieldLabel">Metric Type<b style="color: Red;">*</b></h3>
                        </div>
                        <div class="ms-formbody">
                            <div class="ddlMetricType" id="divddlMetricType" runat="server" style="float: left; width: 100%;">
                                <div class="col-xs-10 noPadding">
                                    <asp:DropDownList ID="ddlMetricType" onchange="hideShowEdit('ddlMetricType')" runat="server" CssClass="aspxDropDownList" AutoPostBack="false">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-xs-2 noPadding">
                                    <img alt="Edit Metric Type" runat="server" class="editicon" id="btMetricEdit"
                                        src="/content/images/editNewIcon.png" width="16" style="cursor: pointer; position: relative; float: right;"
                                        onclick="javascript:$('.divMetricType').attr('style','display:block');hideddlMetricType(1)" />
                                    <img alt="Add Metric Type" id="Img1" width="16" src="/content/images/plus-blue.png" style="cursor: pointer; float: right; margin-right: 10px;"
                                        onclick="javascript:$('.divMetricType').attr('style','display:block');hideddlMetricType(0);" />
                                </div>

                            </div>
                            <div runat="server" id="divMetricType" class="divMetricType" style="display: none; float: left;">
                                <div class="col-xs-10 noPadding">
                                    <asp:TextBox runat="server" ID="txtMetricType" CssClass="txtCategory"></asp:TextBox>
                                    <asp:HiddenField runat="server" ID="hdnRequestMetricType"></asp:HiddenField>
                                </div>
                                <div class="col-xs-2 noPadding">
                                    <img alt="Cancel Metric Type" style="float: right" width="16" src="/content/images/close-blue.png" class="cancelModule"
                                        onclick="javascript:$('.divMetricType').attr('style','display:none');showddlMetricType();" />
                                </div>
                            </div>
                            <div style="width: 120px; padding: 4px 4px 0px; display: inline-block;">
                                <asp:CustomValidator ID="csvdivMetricType" ForeColor="Red" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="txtMetricType"
                                    ErrorMessage="Select Metric Type" Display="Dynamic" OnServerValidate="csvdivMetricType_ServerValidate" ValidationGroup="Save"></asp:CustomValidator>
                            </div>
                        </div>
                    </div>
            <div id="tr13" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Column Type</h3>
                </div>
                <div class="pb-2">
                    <asp:DropDownList ID="ddlColumnType" runat="server">
                        <asp:ListItem Text="--Select--" Value="0" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="MatchValue" Value="MatchValue"></asp:ListItem>
                        <asp:ListItem Text="MatchList" Value="MatchList"></asp:ListItem>
                        <asp:ListItem Text="Ratio" Value="Ratio"></asp:ListItem>
                    </asp:DropDownList>
                    <div>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" InitialValue="0" Enabled="true" runat="server" ControlToValidate="ddlColumnType"
                            ErrorMessage="Enter Column Type" Display="Dynamic" ForeColor="Red" ValidationGroup="Save"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div id="Div2" runat="server">
                <div class="ms-formlabel">
                    <%--<h3 class="ms-standardheader budget_fieldLabel">Stage Weight</h3>--%>
                    <h3 class="ms-standardheader budget_fieldLabel">Score</h3>
                </div>
                <div class="pb-2">
                    <asp:TextBox ID="txtStageWeight" runat="server" />
                    <div>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="txtStageWeight"
                            ErrorMessage="Enter Stage Weight" Display="Dynamic" ForeColor="Red" ValidationGroup="Save"></asp:RequiredFieldValidator>

                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"
                         ControlToValidate="txtStageWeight"
                         ErrorMessage="Enter Number" ForeColor="Red"
                         ValidationExpression="^[0-9]*$" ValidationGroup="Save">
                    </asp:RegularExpressionValidator>
                    </div>
                </div>
            </div>
        </div>


        <div class="col-md-12 col-sm-12 col-xs-12" id="tr4" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Delete</h3>
            </div>
            <div>
                <asp:CheckBox ID="chkDeleted" runat="server" TextAlign="Right" Text="(Prevent use for new Column Name)" />
            </div>
            <div class="ms-formlabel">
                <asp:Label ID="lblErrorMessage" runat="server" ForeColor="Red"></asp:Label>
            </div>
        </div>
        <div class="col-md-12 col-sm-12 col-xs-12 pt-2 text-right">
            <dx:ASPxButton ID="lnkDelete" Visible="false" CssClass="secondary-cancelBtn" Text="Delete" ToolTip="Delete" runat="server"
                OnClick="lnkDelete_Click">
            </dx:ASPxButton>
            <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel" CssClass="secondary-cancelBtn"
                OnClick="btnCancel_Click">
            </dx:ASPxButton>
            <dx:ASPxButton ID="btnSave" runat="server" Text="Save" ToolTip="Save" CssClass="primary-blueBtn" ValidationGroup="Save" OnClick="btnSave_Click">
            </dx:ASPxButton>
        </div>
    </div>
<asp:HiddenField ID="hdnMetricType" runat="server" />
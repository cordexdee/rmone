<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="QueryFilterAddEdit.ascx.cs" Inherits="uGovernIT.Web.QueryFilterAddEdit" %>
<%@ Register Src="~/ControlTemplates/Utility/DateVariableCtrl.ascx" TagPrefix="uGovernIT" TagName="DateVariableCtrl" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .ms-formbody {
        background: none repeat scroll 0 0 #E8EDED;
        border-top: 1px solid #A5A5A5;
        padding: 3px 6px 4px;
        vertical-align: top;
    }

    .ms-formlabel {
        text-align: right;
        width: 190px;
        vertical-align: top;
    }

    .ms-standardheader {
        text-align: right;
    }

    .ms-long {
        font-family: Verdana,sans-serif;
        font-size: 8pt;
        width: 386px;
    }

    .error {
        color: red;
        margin-top: 3px;
        padding-left: 3px;
    }

    .filter-userBox .userValueBox-dropDown .dxgvCSD {
        width: 225px !important;
    }
    .filter-userBox .userValueBox-dropDown table {
        max-width: 225px !important;
    }
    
</style>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    $(document).ready(function () {

        $('.drop-column').each(function () {
            $(this).bind('change', function () {
                if (this != null) {
                    var prefix = this.id.substring(0, this.id.indexOf('drpColumn'));
                    var txt = '#' + prefix + 'txtValue';
                    var chk = '#' + prefix + 'chkValue';
                    var dtc = '#span_datetime';
                    var user = '#span_user';
                    var valuetype = '#' + prefix + 'drpValuetype';
                    change_column(this, valuetype, txt, chk, dtc, user);
                }
            });
        });
        $('.drop-valuetype').each(function () {
            $(this).bind('change', function () {
                if (this != null) {
                    var prefix = this.id.substring(0, this.id.indexOf('drpValuetype'));
                    var txt = '#' + prefix + 'txtValue';
                    var chk = '#' + prefix + 'chkValue';
                    var dtc = '#span_datetime';
                    var user = '#span_user';
                    var column = '#' + prefix + 'drpColumn';
                    change_column(column, this, txt, chk, dtc, user);
                }
            });
        });

    });

    $(function () {
        var drpcolumn = $('.drop-column');
        if (drpcolumn.length > 0) {
            var prefix = $(drpcolumn).attr('id').substring(0, $(drpcolumn).attr('id').indexOf('drpColumn'));
            var txt = '#' + prefix + 'txtValue';
            var chk = '#' + prefix + 'chkValue';
            var dtc = '#span_datetime';
            var user = '#span_user';
            var valuetype = '#' + prefix + 'drpValuetype';
            change_column(drpcolumn, valuetype, txt, chk, dtc, user);
        }
    });

    function change_column(drpcolumn, valuetype, txt, chk, dtc, user) {
        $(txt).hide();
        $(chk).hide();
        $(dtc).hide();
        $(user).hide();
        var typeval = $(valuetype).val();
        var selectedcolumn = $(drpcolumn).val();
        var datatype = selectedcolumn.substring(selectedcolumn.indexOf('(') + 1, selectedcolumn.indexOf(')'));
        datatype = datatype.replace('System.', '');

        $(valuetype).find("option[value='Variable']").attr("disabled", "disabled");
        if (datatype != "DateTime" && datatype != "Date" && typeval == "Variable") {
            $(valuetype).val("Constant");
            typeval = "Constant";
            $(txt).val("");
        }

       $(".tr7").show();
       $('#<%=dvCurrentUser.ClientID%>').hide();
       
        switch (datatype) {
            case 'DateTime':
            case 'Date':
                $(valuetype).find("option[value='Variable']").removeAttr("disabled");
                if (typeval == 'Constant') {
                    var inputdate = '#' + $(drpcolumn).attr('id').replace('drpColumn', 'dtcValue') + '_dtcValueDate';
                    var inputmin = '#' + $(drpcolumn).attr('id').replace('drpColumn', 'dtcValue') + '_dtcValueDateMinutes';
                    var inputhours = '#' + $(drpcolumn).attr('id').replace('drpColumn', 'dtcValue') + '_dtcValueDateHours';
                    $(inputmin).hide();
                    $(inputhours).hide();
                    $(inputdate).css('width', '130px');
                    $(dtc).show();
                }
                else
                    $(txt).show();
                break;
            case 'Boolean':
                if (typeval == 'Constant')
                    $(chk).show();
                else
                    $(txt).show();
                break;
            case 'User':
                if (typeval == 'Constant')
                {
                    $(user).show();
                    $('#<%=dvCurrentUser.ClientID%>').show();

                    if ($(".chkCurrentUser :checked").length > 0)
                        $(".tr7").hide();
                    else 
                        $(".tr7").show();
                }
                else {
                    $(txt).show();
                    $('#<%=dvCurrentUser.ClientID%>').hide();
                }
                break;
            default:
                $(txt).show();
                break;
        }

        $(".trdatevariable").hide();
        if (typeval == 'Constant') {
            $('#trParameterType').hide();
            document.getElementById('<%=lblLabel.ClientID%>').innerHTML = 'Value';
                $('#<%=lblLabel.ClientID%>').val('Value');
            }
            else if (typeval == "Variable") {
                $('#trParameterType').hide();
                $(".tr7").hide();
                $(".trdatevariable").show();
            }
            else {
                $('#trParameterType').show();
                document.getElementById('<%=lblLabel.ClientID%>').innerHTML = 'Parameter Prompt';
            }
    }
    
    function change_chkCurrentUser() {
        if ($(".chkCurrentUser :checked").length > 0) {
            $(".tr7").hide();
        }
        else {
            $(".tr7").show();
        }
    }
</script>

<div style="float: right; width: 98%; padding-left: 10px;">
    <table class="ms-formtable" cellpadding="0" cellspacing="0" style="border-collapse: collapse" width="100%">
        <tr id="trTitle" runat="server">
            <td class="ms-formlabel">
                <h3 class="ms-standardheader">Start With<b style="color: Red;">*</b>
                </h3>
            </td>
            <td class="ms-formbody">
                <asp:DropDownList ID="drpRelOpt" runat="server" Width="50px">
                    <asp:ListItem Value="None" Text=" "> </asp:ListItem>
                    <asp:ListItem Value="AND" Selected="True">AND</asp:ListItem>
                    <asp:ListItem Value="OR">OR</asp:ListItem>
                </asp:DropDownList>
                <div>
                </div>
            </td>
        </tr>
        <tr id="tr1" runat="server">
            <td class="ms-formlabel">
                <h3 class="ms-standardheader">Column
                </h3>
            </td>
            <td class="ms-formbody">
                <asp:DropDownList ID="drpColumn" runat="server" Width="240px" CssClass="drop-column" /></td>
        </tr>

        <tr id="tr6" runat="server">
            <td class="ms-formlabel">
                <h3 class="ms-standardheader">Operator
                </h3>
            </td>
            <td class="ms-formbody">
                <asp:DropDownList ID="drpOpt" runat="server" Width="85px">
                    <asp:ListItem Value="Equal">=</asp:ListItem>
                    <asp:ListItem Value="NotEqual">!=</asp:ListItem>
                    <asp:ListItem Value="GreaterThan">&gt;</asp:ListItem>
                    <asp:ListItem Value="GreaterThanEqualTo">&gt;=</asp:ListItem>
                    <asp:ListItem Value="LessThan">&lt;</asp:ListItem>
                    <asp:ListItem Value="LessThanEqualTo">&lt;=</asp:ListItem>
                    <asp:ListItem Value="like">like</asp:ListItem>
                    <asp:ListItem Value="MemberOf">member of</asp:ListItem>
                </asp:DropDownList></td>
        </tr>

        <tr id="tr3" runat="server">
            <td class="ms-formlabel">
                <h3 class="ms-standardheader">Value Type
                </h3>
            </td>
            <td class="ms-formbody">
                <asp:DropDownList ID="drpValuetype" OnLoad="drpValuetype_Load" runat="server" Width="85px" CssClass="drop-valuetype" style="float: left;">
                    <asp:ListItem Text="Constant" />
                    <asp:ListItem Text="Parameter" />
                    <asp:ListItem Text="Variable" />
                </asp:DropDownList>

                <div id="dvCurrentUser" runat="server" style="display:none; float:left; margin-right: 5px;">
                    <asp:CheckBox ID="chkCurrentUser" runat="server" Text="Current User" Checked="false" onchange="change_chkCurrentUser()" CssClass="chkCurrentUser" TextAlign="Left" style="margin-right: 5px;padding-left: 10px;"   />
                </div>

            </td>
        </tr>

        <tr id="trParameterType">
            <td class="ms-formlabel">
                <h3 class="ms-standardheader">Parameter Type
                </h3>
            </td>
            <td class="ms-formbody">
                <table width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <asp:DropDownList ID="ddlParameterType" runat="server" Width="75px" AutoPostBack="true" CssClass="drop-ParameterType" OnSelectedIndexChanged="ddlParameterType_SelectedIndexChanged">
                                <asp:ListItem Text="" />
                                <asp:ListItem Text="TextBox" />
                                <asp:ListItem Text="DateTime" />
                                <asp:ListItem Text="DropDown" />
                                <asp:ListItem Text="Lookup" />
                                <asp:ListItem Text="User" />
                                <asp:ListItem Text="UserOrGroup" />
                                <asp:ListItem Text="Group" />
                            </asp:DropDownList>
                            <asp:CheckBox ID="chkParamater" runat="server" Text="Required" Checked="true" TextAlign="Left" Style="padding-left: 5px;" />
                        </td>
                    </tr>
                    <tr id="pDropdownProperties" runat="server" visible="false">
                        <td>
                            <table>
                                <tr>
                                    <td>Enter each choice on a separate line:</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox TextMode="MultiLine" Width="100%" Rows="4" ID="txtDropdownOptions" runat="server" CssClass="txtdropdownoptions"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Default Value:
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtDropDownDefaultVal" runat="server" CssClass="txtdropdowndefaultval"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>

                    <tr id="pLookupProperties" runat="server" visible="false">
                        <td>
                            <table>
                                <tr>
                                    <td>Lookup List:
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:DropDownList ID="ddlLookupList" AutoPostBack="true" runat="server" Width="200" OnSelectedIndexChanged="DDLLookupList_SelectedIndexChanged"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="lpModuletr" runat="server" visible="false">
                                    <td>
                                        <div>Module Name:</div>
                                        <div>
                                            <asp:DropDownList Width="200" ID="ddlLPModule" runat="server"></asp:DropDownList>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Lookup Field:
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:DropDownList Width="200" ID="ddlLookupFields" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>

                </table>
            </td>
        </tr>

        <tr id="tr7" runat="server" class="tr7">
            <td class="ms-formlabel">
                <h3 class="ms-standardheader">
                    <asp:Label ID="lblLabel" Text="Value" runat="server"></asp:Label>
                </h3>
            </td>
            <td class="ms-formbody">
                <asp:TextBox ID="txtValue" runat="server" Width="136px" />
                <input id="chkValue" type="checkbox" runat="server" style="display: none; width: 136px;" />
                <div id="span_datetime" style="display: none; width: 136px;">
                    <dx:ASPxDateEdit ID="dtcValue" runat="server" DateOnly="true" />
                
                </div>
                <span id="span_user" style="display: none; width: 225px;" class="filter-userBox">
                    <ugit:UserValueBox ID="ppeValue" Width="225px"  CssClass="userValueBox-dropDown" SelectionSet="User,Group" runat="server" Multi="false" />
                </span>
                <label id="lbl_Value" style="display: none;" runat="server" class="error">*</label></td>
        </tr>
        <tr id="trDateVariable" style="display: none;" class="trdatevariable" runat="server">
            <td class="ms-formlabel">
                <h3 class="ms-standardheader">Value
                </h3>
            </td>
            <td class="ms-formbody">
                <uGovernIT:DateVariableCtrl id="dtVariable" runat="server"></uGovernIT:DateVariableCtrl>
            </td>
        </tr>

        <tr id="tr4" runat="server">
            <td colspan="2" class="ms-formlabel"></td>
        </tr>
    </table>

    <table width="100%">
        <tr id="tr2" runat="server">
            <td align="left" style="padding-top: 5px;">
                <%--<div>
                    <asp:LinkButton ID="LnkbtnDelete" runat="server" Text="&nbsp;&nbsp;Delete&nbsp;&nbsp;" ToolTip="Delete"
                        OnClientClick="return confirm('Are you sure you want to delete?');" OnClick="LnkbtnDelete_Click">
                        <span class="button-bg">
                            <b style="float: left; font-weight: normal;">
                                Delete</b>
                            <i style="float: left; position: relative; top: -3px;left:2px">
                                <img src="/_layouts/15/images/uGovernIT/ButtonImages/cancel.png"  style="border:none;" title="" alt=""/>
                            </i> 
                        </span>
                    </asp:LinkButton>
                </div>--%>
            </td>
            <td align="right" style="padding-top: 5px;">
                <div style="float: right;" class="addEditPopup-btnWrap">
                    <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel" CssClass="secondary-cancelBtn"  OnClick="btnCancel_Click"></dx:ASPxButton>
                    <dx:ASPxButton ID="btnSave" runat="server" Text="Save" ToolTip="Save" CssClass="primary-blueBtn" ValidationGroup="Save" OnClick="btnSave_Click"></dx:ASPxButton>
                </div>
            </td>
        </tr>
    </table>
</div>

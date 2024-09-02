<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ScheduleActionControl.ascx.cs" Inherits="uGovernIT.Web.ScheduleActionControl" %>

<%@ Register Assembly="DevExpress.Web.ASPxHtmlEditor.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxHtmlEditor" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxSpellChecker.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxSpellChecker" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
    .ms-formbody {
        background: none repeat scroll 0 0 #E8EDED;
        border-top: 1px solid #A5A5A5;
        padding: 3px 6px 4px;
        vertical-align: top;
    }

    .ms-standardheader {
        text-align: right;
    }

    .error {
        color: red;
    }

    .pnl-control {
        position: absolute;
        top: 404px;
        left: 100px;
        background-color: #FFF;
    }

    .hide-button {
        display: none;
    }

    .dxeErrorCellSys img,
    .dxeErrorCellSys .dx-acc-s {
        display: none;
        margin-right: 4px;
    }

    .close-button {
        float: right;
    }

    .span-submit {
        float: right;
    }

    .param-col1 {
        width: 100px;
        border: 1px solid #c8ccc6;
    }

    .param-col2 {
        width: 200px;
        border: 1px solid #c8ccc6;
    }

        .param-col2 > input[type="text"] {
            width: 200px;
        }

    .param-value {
        width: 200px;
    }

    .span-header {
        font-weight: bold;
        float: left;
    }

    .parameter-div > table {
        padding: 5px;
    }

    .parameter-div {
        background-color: #EBD9B4;
        border: 1px solid #959583;
        float: left;
        margin: 5px;
        padding: 5px 2px;
        position: absolute;
        width: 325px;
        z-index: 1;
    }

    .ms-formlabel {
        text-align: right;
        width: 168px;
        vertical-align: top;
    }

    .ms-long {
        font-family: Verdana,sans-serif;
        font-size: 8pt;
        width: 386px;
    }

    .auto-style1 {
        text-align: right;
        width: 190px;
        vertical-align: top;
        height: 20px;
    }

    .auto-style2 {
        background: none repeat scroll 0 0 #E8EDED;
        border-top: 1px solid #A5A5A5;
        padding: 3px 6px 4px;
        vertical-align: top;
        height: 20px;
    }
</style>

<div>
    <div id="parameterDiv" runat="server" class="parameter-div" style="display: none;"></div>
    <asp:HiddenField ID="hdnScheduleId" runat="server" />
    <table class="ms-formtable" cellpadding="0" cellspacing="0" style="border-collapse: collapse" width="100%">
        <tr id="trTitle" runat="server">
            <td class="ms-formlabel">
                <h3 class="ms-standardheader">Title<b style="color: Red;">*</b>
                </h3>
            </td>
            <td class="ms-formbody">
                <asp:TextBox ID="txtScheduleTitle" runat="server" Width="386px" />
                <div>
                    <asp:RequiredFieldValidator ID="rfvScheduleTitle" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="txtScheduleTitle"
                        ErrorMessage="Enter Title " Display="Dynamic" ValidationGroup="Save"></asp:RequiredFieldValidator>
                </div>
            </td>
        </tr>
        <tr>
            <td class="ms-formlabel">
                <h3 class="ms-standardheader">Start Time
                </h3>
            </td>
            <td class="ms-formbody">
                <dx:ASPxDateEdit ID="dtcScheduleStartTime" runat="server" EditFormat="Custom" EditFormatString="MM/dd/yyyy" NullText="MM/dd/yyyy" time="hh:mm tt">
                    <TimeSectionProperties>
                        <TimeEditProperties EditFormatString="hh:mm:tt"></TimeEditProperties>
                    </TimeSectionProperties>
                </dx:ASPxDateEdit>
            </td>
        </tr>
        <tr runat="server" id="trAttachformat">
            <td class="ms-formlabel">
                <h3 class="ms-standardheader">Attachment Format
                </h3>
            </td>
            <td class="ms-formbody">
                <asp:DropDownList ID="ddlAttachFormat" runat="server">
                    <asp:ListItem Text="Excel" Value="xls" />
                    <asp:ListItem Text="Pdf" Value="pdf" />
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="ms-formlabel">
                <h3 class="ms-standardheader">Email To
                </h3>
            </td>
            <td class="ms-formbody">
                <asp:TextBox ID="txtScheduleEmailTo" runat="server" Width="386px" />
                <div>
                    <asp:RequiredFieldValidator ID="rfvScheduleEmailTo" ErrorMessage="Enter Email Id." ValidateEmptyText="true" Display="Dynamic" ControlToValidate="txtScheduleEmailTo" runat="server" ValidationGroup="Save" />
                </div>
            </td>
        </tr>
        <tr>
            <td class="ms-formlabel">
                <h3 class="ms-standardheader">Email CC
                </h3>
            </td>
            <td class="ms-formbody">
                <asp:TextBox ID="txtScheduleEmailCC" runat="server" Width="386px" />
            </td>
        </tr>
        <tr>
            <td class="ms-formlabel">
                <h3 class="ms-standardheader">Email Subject
                </h3>
            </td>
            <td class="ms-formbody">
                <asp:TextBox ID="txtScheduleSubject" runat="server" Width="386px" />
                <div>
                    <asp:RequiredFieldValidator ID="rfvScheduleSubject" ErrorMessage="Enter Subject." ValidateEmptyText="true" Display="Dynamic" ControlToValidate="txtScheduleSubject" runat="server" ValidationGroup="Save" />
                </div>
            </td>
        </tr>
        <tr>
            <td class="ms-formlabel">
                <h3 class="ms-standardheader">Email Body
                </h3>
            </td>
            <td class="ms-formbody">
                <asp:TextBox ID="txtScheduleEmailBody" TextMode="MultiLine" CssClass="ms-long" runat="server" Rows="4" cols="20" Width="95%" />
                <div>
                    <asp:RequiredFieldValidator ID="rfvScheduleEmailBody" ErrorMessage="Enter Email Body" ValidateEmptyText="true" Display="Dynamic" ControlToValidate="txtScheduleEmailBody" runat="server" ValidationGroup="Save" />
                </div>
            </td>
        </tr>
        <tr>
            <td class="ms-formlabel">
                <h3 class="ms-standardheader">Recurring
                </h3>
            </td>
            <td class="ms-formbody">
                <asp:CheckBox ID="chkScheduleRecurring" runat="server" AutoPostBack="true" OnCheckedChanged="chkScheduleRecurring_CheckedChanged" ValidationGroup="Save" />
                <table id="recurrTable" runat="server" visible="false">
                    <tr>
                        <td class="ms-formlabel">
                            <h3 class="ms-standardheader">Recurring Interval
                            </h3>
                        </td>
                        <td class="ms-formbody">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtScheduleRecurrInterval" runat="server" Width="50px" />
                                    <span>
                                        <asp:DropDownList ID="ddlIntervalUnit" AutoPostBack="true" OnSelectedIndexChanged="ddlIntervalUnit_SelectedIndexChanged" runat="server">
                                            <asp:ListItem Text="Minutes" />
                                            <asp:ListItem Text="Hours" />
                                            <asp:ListItem Text="Days" />
                                            <asp:ListItem Text="Custom" />
                                        </asp:DropDownList>
                                    </span>
                                    <span>
                                        <asp:CheckBox ID="chkbxBusinessHours" Text="Business Hours" runat="server" />
                                    </span>
                                    <span id="spanCustomRecInterval" runat="server" visible="false">
                                        <asp:CheckBoxList ID="chkbxCustomRecurranceInterval" RepeatLayout="Table" RepeatColumns="7" RepeatDirection="Horizontal" runat="server">
                                            <asp:ListItem Value="0" Text="Sun"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="Mon"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="Tue"></asp:ListItem>
                                            <asp:ListItem Value="3" Text="Wed"></asp:ListItem>
                                            <asp:ListItem Value="4" Text="Thu"></asp:ListItem>
                                            <asp:ListItem Value="5" Text="Fri"></asp:ListItem>
                                            <asp:ListItem Value="6" Text="Sat"></asp:ListItem>
                                        </asp:CheckBoxList>
                                    </span>

                                </ContentTemplate>

                            </asp:UpdatePanel>

                        </td>
                    </tr>
                    <tr>
                        <td class="ms-formlabel">
                            <h3 class="ms-standardheader">Recurring End Date
                            </h3>
                        </td>
                        <td class="ms-formbody">
                            <dx:ASPxDateEdit ID="dtcRecurrEndDate" runat="server" EditFormat="Custom" EditFormatString="MM/dd/yyyy" NullText="MM/dd/yyyy" time="hh:mm tt">
                                <TimeSectionProperties>
                                    <TimeEditProperties EditFormatString="hh:mm:tt"></TimeEditProperties>
                                </TimeSectionProperties>
                            </dx:ASPxDateEdit>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>

        <tr>
            <td class="ms-formlabel">
                <h3 class="ms-standardheader">Enabled</h3>
            </td>
            <td class="ms-formbody">
                <asp:CheckBox ID="chkIsEnable" runat="server" />
            </td>
        </tr>

        <tr id="tr4" runat="server">
            <td colspan="2" class="ms-formlabel"></td>
        </tr>
    </table>
    <input type="hidden" id="isPreview" runat="server" value="false" />
    <input type="hidden" id="paramValue" runat="server" value="false" />

    <table width="100%">
        <tr id="tr2" runat="server">
            <td align="left" style="padding-top: 5px;"></td>
            <td align="right" style="padding-top: 5px;">
                <div style="float: right;" class="addEditPopup-btnWrap">
                    <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel" CssClass="secondary-cancelBtn" OnClick="btnCancel_Click"></dx:ASPxButton>
                    <dx:ASPxButton ID="btnSave" runat="server" Text="Save" AutoPostBack="false" ToolTip="Save" CssClass="primary-blueBtn" ValidationGroup="Save">
                        <ClientSideEvents Click="function(s,e){ShowParameterDiv(event, e);}" />
                    </dx:ASPxButton>
                    <dx:ASPxButton ID="saveButton" runat="server" ClientInstanceName="saveButton" AutoPostBack="true" ClientVisible="false" Text="Save" RenderMode="Link" ToolTip="" OnClick="btnSave_Click"></dx:ASPxButton>
                </div>
            </td>
        </tr>
    </table>
</div>


<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function ShowParameterDiv(evt) {
        var html = $('.parameter-div').html();
        var height = $('.parameter-div').height() + 5;
        var width = $('.parameter-div').width() + 5;
        if (html != '') {
            $(".parameter-div").offset({ left: (evt.pageX - width), top: (evt.pageY - height) - 30 })
            $(".parameter-div").show();
        }
        else {
            $("#<%= isPreview.ClientID %>").val('true');
            saveButton.DoClick();
        }
    }
    function paramButtonClick() {
        var isvalue = true;
        var numParam = $(".param-value").length;
        var paramValue = new Array(numParam);
        var index = 0;
        $(".param-value").each(function () {
            if ($(this).val() != "") {
                paramValue[index] = $(this).val();
                index++;
            }
            else {
                isvalue = false;
            }
        });
        if (isvalue == true) {
            $("#<%= paramValue.ClientID %>").val(paramValue.join(","));
            saveButton.DoClick();
        }
    }

    $(document).ready(function () {
        $('.close-button').click(function () {
            $('.parameter-div').offset({ left: 0, top: 0 });
            $(".parameter-div").hide();
        });
    });
</script>

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ModuleWebpartProperties.ascx.cs" Inherits="uGovernIT.Web.ModuleWebpartProperties" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script data-v="<%=UGITUtility.AssemblyVersion %>">
    function pickFilter(filter)
    {
        filter = unescape(filter).replace(/@/g, "\'");
        $($(".filterexpression").get(0)).text(filter);
        hdnDataFilterExpression.Set("FilterExpression", filter);
    }
    function ClearConditionPicker()
    {
        $($(".filterexpression").get(0)).text('');
        hdnDataFilterExpression.Set("FilterExpression", '');
    }
    function OpenConditionPicker() {
        
    <%--  var Url = '<%= DataFilterUrl%>';
        if ($($(".filterexpression").get(0)).text() != '') {
            Url += "&SkipCondition=" + escape($($(".filterexpression").get(0)).text());
        }
        javascript: UgitOpenPopupDialog(Url, '', 'Filter Expression', '85', '80', 0, '');--%>
    }
   
</script>
<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .ms-formbody {
        background: none repeat scroll 0 0 #E8EDED;
        border-top: 1px solid #A5A5A5;
        padding: 3px 6px 4px;
        vertical-align: top;
    }

    .ms-formlabel {
        text-align: right;
        width: 100px;
        vertical-align: middle;
    }

    .ms-standardheader {
        text-align: right;
    }

    .ms-long {
        font-family: Verdana,sans-serif;
        font-size: 8pt;
        width: 386px;
    }

    .padding-left5 {
        padding-left: 5px;
    }
</style>


<table class="ms-formtable" cellpadding="0" cellspacing="0" style="border-collapse: collapse;overflow-x:auto" width="100%">
    <tr id="tr12" runat="server" >
        <td class="ms-formlabel" style="width:130px!important;">
            <h3 class="ms-standardheader">Title</h3>
        </td>
        <td class="ms-formbody">
            <div>
                <div class="fleft">
                    <dx:ASPxCheckBox ID="chkTitle" runat="server">
                        <ClientSideEvents
                            Init="function(s,e){ if(s.GetChecked()){txtTitle.SetVisible(true)}else{txtTitle.SetVisible(false)} }"
                            CheckedChanged="function(s,e){ if(s.GetChecked()){txtTitle.SetVisible(true)}else{txtTitle.SetVisible(false)} }" />
                    </dx:ASPxCheckBox>
                </div>
                <div class="fleft padding-left5">
                    <dx:ASPxTextBox ID="txtTitle" ClientInstanceName="txtTitle" runat="server"></dx:ASPxTextBox>
                </div>
            </div>
        </td>
    </tr>
   <%-- <tr id="tr2" runat="server" style="display:none">
        <td class="ms-formlabel">
            <h3 class="ms-standardheader">Name</h3>
        </td>
        <td class="ms-formbody">
            <div>                
                <div class="fleft padding-left5">
                    <dx:ASPxTextBox ID="txtName" ClientInstanceName="txtName" runat="server"></dx:ASPxTextBox>
                </div>
            </div>
        </td>
    </tr>--%>
    <tr id="trTitle" runat="server">
        <td class="ms-formlabel">
            <h3 class="ms-standardheader">Module</h3>
        </td>
        <td class="ms-formbody">
            <div>
                <dx:ASPxComboBox ID="ddlModule" EnableCallbackMode="true"  runat="server" OnInit="ddlModule_Init" NullText="Select Module">
                    <ClientSideEvents SelectedIndexChanged="function(s,e){repeaterTabViewControlCallBackPanel.PerformCallback(s.GetValue());}" />
                </dx:ASPxComboBox>
            </div>
        </td>
    </tr>
   
</table>



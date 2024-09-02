<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DashboardReportProperties.ascx.cs" Inherits="uGovernIT.Web.DashboardReportProperties" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .ms-formbody {
        background: none repeat scroll 0 0 #E8EDED;
        border-top: 1px solid #A5A5A5;
        padding: 3px 6px 4px;
        vertical-align: top;
        width: 85%;
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

<table>
    <tr>
         <td class="ms-formlabel">
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
</table>

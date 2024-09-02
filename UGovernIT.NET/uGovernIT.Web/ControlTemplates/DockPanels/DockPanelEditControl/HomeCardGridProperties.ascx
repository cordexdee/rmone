<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HomeCardGridProperties.ascx.cs" Inherits="uGovernIT.Web.HomeCardGridProperties" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>




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
</table>

<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RequestTypeDropDownList.ascx.cs" Inherits="uGovernIT.Web.RequestTypeDropDownList" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dx" %>

<script>
   
</script>

<dx:ASPxTreeList ID="requestTypeTreeList" runat="server" Width="100%" ClientInstanceName="requestTypeTreeList" AutoGenerateColumns="false"
    AutoGenerateServiceColumns="true" OnDataBound="requestTypeTreeList_DataBound" KeyFieldName="ID" ParentFieldName="ParentID" Border-BorderStyle="Solid">
    <Columns>
        <dx:TreeListDataColumn VisibleIndex="0" FieldName="RequestType" >
        </dx:TreeListDataColumn>
    </Columns>
    <Styles>
        <AlternatingNode Enabled="True">
        </AlternatingNode>
    </Styles>
    <SettingsSelection Enabled="True" AllowSelectAll="true" Recursive="true" />
</dx:ASPxTreeList>


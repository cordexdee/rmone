<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EmailContactsDropDown.ascx.cs" Inherits="uGovernIT.Web.EmailContactsDropDown" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web"
    TagPrefix="dx" %>

<dx:ASPxTokenBox ID="ddlUsers" runat="server" CssClass="aspxUserTokenBox-control" Width="100%" IncrementalFilteringMode="Contains" 
    AllowCustomTokens="false" SettingsLoadingPanel-Enabled="false" ShowDropDownOnFocus="Never" ></dx:ASPxTokenBox>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReportUploadControl.ascx.cs" Inherits="uGovernIT.Web.ReportUploadControl" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Src="~/ControlTemplates/uGovernIT/SelectFolder.ascx" TagPrefix="uc1" TagName="SelectFolder" %>


<div class="row" style="margin-top: 40px;">
    <div class="col-md-12" >
        Click
        <asp:HyperLink ID="linkFile" runat="server">here</asp:HyperLink>
        to view file !!
    </div>
    <div class="col-md-12 col-sm-12 col-xs-12">
        <div class="next-cancel-but">
            <dx:ASPxButton ID="btnCancel" runat="server" AutoPostBack="false" CssClass="secondary-cancelBtn" Text="Cancel" ToolTip="Cancel">
                <ClientSideEvents Click="function(s, e){ window.parent.CloseWindowCallback(0, document.location.href); }" />
            </dx:ASPxButton>

            <dx:ASPxButton ID="btnSave" runat="server" CssClass="primary-blueBtn" Text="Save" ToolTip="Save" OnClick="btnOK_Click">
            </dx:ASPxButton>
        </div>
    </div>
</div>





<%--<div style="border: 1px solid #bbbfbf;margin:10px;">
<table width="100%" border="0" cellpadding="0" cellspacing="0">
    <tbody>
        <tr>
            <td style="vertical-align:top;padding:5px;">
               Upload Document
            </td>
            <td>
                <table style="background-color:#E8EDED;width:100%" cellpadding="5">
                    <tr>
                        <td>
                            <b>Folder Name:<span style="color:red;">*</span></b>
                        </td>
                        <td>
                            <span style="float:left;font-weight:bold;"><asp:Label ID="lblSelectedFolder" Text="" runat="server" /></span>
                            <asp:Panel ID="pnlSelectFolder" runat="server" style="float:left;margin-left:10px;"></asp:Panel>
                           <asp:HiddenField id="hdnfolderId" runat="server" />
                            <asp:HiddenField ID="hdnDocName" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Document Name:<span style="color:red;">*</span>
                        </td>
                        <td>
                            <asp:HyperLink ID="linkFile" runat="server"></asp:HyperLink> 
                        </td>
                    </tr>
                    <tr>
                        <td  colspan="2">
                            <asp:Label ID="overwriteMsg" Text="" runat="server" style="color:red;" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Document Tags:
                        </td>
                        <td >
                            <asp:TextBox ID="txtTags" runat="server" Width="300px" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                
            </td>       

        </tr>
    </tbody>
</table>
    </div>

<div style="float:right;">
    <span>
        <asp:Button ID="btnOK" runat="server" Text="OK" OnClick="btnOK_Click" Width="100px" /></span>
    <span>
        <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" Width="100px" /></span>
</div>--%>


<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TicketReOpen.ascx.cs" Inherits="uGovernIT.Web.TicketReOpen" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
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
    .alignUnauthorizedTicketLabel 
    {
        
    }
</style>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function nullcheck()
    {        
        if ($('#<%=txtComment.ClientID%>').val()!="")
        {
            LoadingPanel.Show();
        }
    }

</script>

 <dx:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel"
        Modal="True">
    </dx:ASPxLoadingPanel>
<table cellpadding="0" cellspacing="0" style="border-collapse: collapse; width: 100%">  
    <tr>
        <td ></td><td><asp:Label id="unAuthorizedTickets" runat="server" Visible="false" CssClass="alignUnauthorizedTicketLabel" ForeColor="Red"/></td>
    </tr>  
    
    <tr id="commentTr" runat="server" visible="true">
        <td class="ms-formlabel">
            <h3 class="required-item" style="font-size: 12px";> Re-Open Comment</h3>
        </td>
        <td class="ms-formbody">
            <asp:TextBox ID="txtComment" runat="server" TextMode="MultiLine" Columns="20" rows="2" Width="354px" Height="100px"></asp:TextBox>            
            <div>
                <asp:RequiredFieldValidator ID="rfvComments" runat="server" ControlToValidate="txtComment"
                    ErrorMessage="Enter Comment" CssClass="text-error" Display="Dynamic" ValidationGroup="TicketCancelClose">
                </asp:RequiredFieldValidator>
            </div>
        </td>
    </tr>
    <tr id="actonbtnTr" runat="server" visible="true">
        <td></td>
        <td style="float: right; padding-top: 10px;">
            <asp:LinkButton ID="btnUpdate" runat="server" OnClientClick="nullcheck();" OnClick="btnUpdate_Click" ValidationGroup="TicketCancelClose">
                <span class="button-bg btn-bottom-padding">
                    <b style="float: left; font-weight: normal;">
                        Re-Open</b>
                    <i style="float: left; position: relative;">
                        <img src="/_layouts/15/images/uGovernIT/ButtonImages/return.png"  style="border:none;" title="" alt=""/>
                    </i> 
                </span>
            </asp:LinkButton>

            <asp:LinkButton ID="btnCancel" runat="server" OnClientClick="window.frameElement.commitPopup();">
                <span class="button-bg btn-bottom-padding">
                    <b style="float: left; font-weight: normal;">
                        Cancel</b>
                    <i style="float: left; position: relative;">
                        <img src="/_layouts/15/images/uGovernIT/ButtonImages/cancelwhite.png"  style="border:none;" title="" alt=""/>
                    </i> 
                </span>
            </asp:LinkButton>
        </td>
    </tr>
</table>

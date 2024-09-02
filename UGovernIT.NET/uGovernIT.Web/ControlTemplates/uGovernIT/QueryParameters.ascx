<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="QueryParameters.ascx.cs" Inherits="uGovernIT.Web.QueryParameters" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function ShowHideDateControls(s, e, spinEditId, typeOptionsId) {
        var clientIdPrefix = (s.name).split("cmbDateTimePeriod_");
       
        var cmbSpinEdit = ASPxClientControl.GetControlCollection().GetByName(clientIdPrefix[0] + spinEditId);
        var cmbTypeOptions = ASPxClientControl.GetControlCollection().GetByName(clientIdPrefix[0] + typeOptionsId);
        
        if (s.GetSelectedIndex() == 0) {
            s.SetWidth(100);
            cmbSpinEdit.SetVisible(true);
            cmbTypeOptions.SetVisible(true);
        }
        else {
            s.SetWidth(181);
            cmbSpinEdit.SetVisible(false);
            cmbTypeOptions.SetVisible(false);
        }
    }
</script>


<table style="width:100%">
    <tr>
        <td runat="server" id="tdvalues" > </td>
        <td style="width: 20px;">
            <a runat="server" id="aEditbutton" href="javascript:pcParameter.Show();" >
                <img src="/Content/images/edit-icon.png" alt="edit" />
            </a>
        </td>
    </tr>
</table>


<dx:ASPxPopupControl ID="pcParameter" runat="server" ClientInstanceName="pcParameter"
    HeaderText="Please enter value for parameter(s)" CloseAction="CloseButton" Modal="True"
    PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
    <ContentCollection>
        <dx:PopupControlContentControl>
            <dx:ASPxPanel ID="pnlcontrol" runat="server" Width="400px" DefaultButton="btnOK">
                <PanelCollection>
                    <dx:PanelContent runat="server">
                        <asp:Table ID="tParameter" runat="server" Width="100%">
                            <asp:TableHeaderRow>
                                <asp:TableHeaderCell ColumnSpan="2">
                                    <asp:Label ID="lblMsg" Text="" runat="server" />
                                </asp:TableHeaderCell>
                            </asp:TableHeaderRow>
                            <asp:TableRow runat="server">
                                <asp:TableCell runat="server">
                                    &nbsp;
                                </asp:TableCell><asp:TableCell runat="server" Width="200px">
                                    <dx:ASPxButton ID="btnOK" runat="server" Text="Submit"
                                        Style="float: left; margin-right: 8px" OnClick="btnOK_Click">
                                    </dx:ASPxButton>

                                    <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" AutoPostBack="false"
                                        UseSubmitBehavior="false" Style="float: left; margin-right: 8px">
                                        <ClientSideEvents Click="function(s, e) { pcParameter.Hide();}" />
                                    </dx:ASPxButton>
                                </asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>
                    </dx:PanelContent>
                </PanelCollection>
            </dx:ASPxPanel>
        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>

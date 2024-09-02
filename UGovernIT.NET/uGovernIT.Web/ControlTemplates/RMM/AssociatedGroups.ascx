<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AssociatedGroups.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.RMM.AssociatedGroups" %>

        <dx:ASPxGridView ID="associatedGroupsGridView" runat="server" Width="100%" AutoGenerateColumns="False" KeyFieldName="Id">
            <Columns>
                <dx:GridViewDataTextColumn FieldName="Name" Caption="Groups" VisibleIndex="0">
                </dx:GridViewDataTextColumn>
            </Columns>
        </dx:ASPxGridView>

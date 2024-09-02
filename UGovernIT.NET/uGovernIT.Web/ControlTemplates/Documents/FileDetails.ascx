<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FileDetails.ascx.cs" Inherits="uGovernIT.Web.FileDetails" %>
<div>
    <table style="width: 100%;">
        <tr>
            <td colspan="2">
                <asp:Label runat="server" ID="lblErrorMessage" Text="" Style="color: Red; font-size: x-small;"> </asp:Label>
            </td>
        </tr>

        <tr>
            <td class="ms-formlabel labelClass" style="width: 40%;">
                <h3 class="ms-standardheader  alignRight">
                    <nobr>Document ID</nobr>
                </h3>
            </td>
            <td class="ms-formbody textBoxClass" style="width: 40%;">
                <asp:Literal ID="ltlDocID" runat="server" Text=""></asp:Literal>
            </td>
        </tr>

        <tr>
            <td class="ms-formlabel labelClass" style="width: 40%;">
                <h3 class="ms-standardheader  alignRight">
                    <nobr>Document Name</nobr>
                </h3>
            </td>
            <td class="ms-formbody textBoxClass" style="width: 40%;">
                <asp:Literal ID="ltlDocumentName" runat="server" Text=""></asp:Literal>
            </td>
        </tr>

        <tr>
            <td class="ms-formlabel labelClass" style="width: 40%;">
                <h3 class="ms-standardheader  alignRight">
                    <nobr>Version</nobr>
                </h3>
            </td>
            <td class="ms-formbody textBoxClass" style="width: 40%;">
                <asp:Literal ID="ltlVersion" runat="server" Text=""></asp:Literal>
            </td>
        </tr>

        <tr>
            <td class="ms-formlabel labelClass" style="width: 40%;">
                <h3 class="ms-standardheader  alignRight">
                    <nobr>Document Path</nobr>
                </h3>
            </td>
            <td class="ms-formbody textBoxClass" style="width: 40%;">
                <asp:Literal ID="ltlPath" runat="server" Text=""></asp:Literal>
            </td>
        </tr>

        <tr>
            <td class="ms-formlabel labelClass" style="width: 40%;">
                <h3 class="ms-standardheader  alignRight">
                    <nobr>Created On</nobr>
                </h3>
            </td>
            <td class="ms-formbody textBoxClass" style="width: 40%;">
                <asp:Literal ID="ltlCreatedOn" runat="server" Text=""></asp:Literal>
            </td>
        </tr>

        <tr>
            <td class="ms-formlabel labelClass" style="width: 40%;">
                <h3 class="ms-standardheader alignRight">
                    <nobr>Modified On</nobr>
                </h3>
            </td>
            <td class="ms-formbody textBoxClass" style="width: 40%;">
                <asp:Literal ID="ltlModifiedOn" runat="server" Text=""></asp:Literal>
            </td>
        </tr>

        <tr runat="server" id="noOfFiletr" class="">
            <td class="ms-formlabel labelClass">
                <h3 class="ms-standardheader alignRight">
                    <nobr>Size of File</nobr>
                </h3>
            </td>
            <td class="ms-formbody textBoxClass">
                <asp:Literal ID="ltlSizeOfFile" runat="server" Text=""></asp:Literal>
            </td>
        </tr>

        <tr>
            <td class="ms-formlabel labelClass">
                <h3 class="ms-standardheader alignRight">
                    <nobr>Author</nobr>
                </h3>
            </td>
            <td class="ms-formbody textBoxClass">
                <asp:Literal ID="ltlAuthor" runat="server" Text=""></asp:Literal>
            </td>
        </tr>
        
        <%--  
            <tr runat="server" id="TrDocType1" class="" visible="false">
                <td class="ms-formlabel labelClass">
                    <h3 class="ms-standardheader alignRight">
                        <nobr><asp:Label id="lblDocType1" runat="server"></asp:Label></nobr>
                    </h3>
                </td>
                <td class="ms-formbody textBoxClass">
                    <asp:Literal ID="ltDocType1" runat="server" Text=""></asp:Literal>
                </td>
            </tr>
            <tr runat="server" id="TrDocType2" class="" visible="false">
                <td class="ms-formlabel labelClass">
                    <h3 class="ms-standardheader alignRight">
                        <nobr><asp:Label id="lblDocType2" runat="server"></asp:Label></nobr>
                    </h3>
                </td>
                <td class="ms-formbody textBoxClass">
                    <asp:Literal ID="ltDocType2" runat="server" Text=""></asp:Literal>
                </td>
            </tr>
            <tr runat="server" id="TrDocType3" class="" visible="false">
                <td class="ms-formlabel labelClass">
                    <h3 class="ms-standardheader alignRight">
                        <nobr><asp:Label id="lblDocType3" runat="server"></asp:Label></nobr>
                    </h3>
                </td>
                <td class="ms-formbody textBoxClass">
                    <asp:Literal ID="ltDocType3" runat="server" Text=""></asp:Literal>
                </td>
            </tr>
            <tr runat="server" id="TrDocType4" class="" visible="false">
                <td class="ms-formlabel labelClass">
                    <h3 class="ms-standardheader alignRight">
                        <nobr><asp:Label id="lblDocType4" runat="server"></asp:Label></nobr>
                    </h3>
                </td>
                <td class="ms-formbody textBoxClass">
                    <asp:Literal ID="ltDocType4" runat="server" Text=""></asp:Literal>
                </td>
            </tr>
            <tr runat="server" id="TrDocType5" class="" visible="false">
                <td class="ms-formlabel labelClass">
                    <h3 class="ms-standardheader alignRight">
                        <nobr><asp:Label id="lblDocType5" runat="server"></asp:Label></nobr>
                    </h3>
                </td>
                <td class="ms-formbody textBoxClass">
                    <asp:Literal ID="ltDocType5" runat="server" Text=""></asp:Literal>
                </td>
            </tr>--%>

        <tr runat="server" id="Tr6" class="">
            <td class="ms-formlabel labelClass">
                <h3 class="ms-standardheader alignRight">
                    <nobr>Tags</nobr>
                </h3>
            </td>
            <td class="ms-formbody textBoxClass">
                <asp:Literal ID="ltlTags" runat="server" Text=""></asp:Literal>
            </td>
        </tr>

        <tr runat="server" id="reviewRequiredtr" class="">
            <td class="ms-formlabel labelClass">
                <h3 class="ms-standardheader alignRight">
                    <nobr>Review Required?</nobr>
                </h3>
            </td>
            <td class="ms-formbody textBoxClass">
                <asp:Literal ID="ltlReviewRequired" runat="server" Text=""></asp:Literal>
            </td>
        </tr>
    </table>
</div>

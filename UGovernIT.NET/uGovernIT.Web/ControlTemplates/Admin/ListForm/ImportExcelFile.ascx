<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ImportExcelFile.ascx.cs" Inherits="uGovernIT.Web.ImportExcelFile" %>
<%@ Register Assembly="DevExpress.Web.ASPxSpreadsheet.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxSpreadsheet" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function DeleteRequestTypeConfirm() {
        if ($("#<%=chkDeleteExistingRecord.ClientID %>").is(':checked')) {
            if (confirm("Are you sure you want to delete existing records? This may be affect existing tickets.")) {
                if ($("#<%=chkRuninBackground.ClientID %>").is(':checked')) {
                    alert("Starting background import, you will be notified via email when the import is completed.");
                    return true;
                }
                else {
                    loadingPanel.Show();
                    return true;
                }
            }
            else {
                return false;
            }
        }
        else if ($("#<%=chkRuninBackground.ClientID %>").is(':checked')) {
            alert("Starting background import, you will be notified via email when the import is completed.");
            return true;
        }
        else {
            loadingPanel.Show();
            return true;
        }
    }

    function ShowLoader() {
        loadingPanel.Show();
        return true;
    }

    function openSkippedTiles(skippedTitles) {
        const popupContentTemplate = function () {
            return $('<div>').append(
                skippedTitles
            );
        };

        const popup = $('#DivSkippedDetail').dxPopup({
            contentTemplate: popupContentTemplate,
            width: 300,
            height: 280,
            title: 'Skipped Titles',
            visible: true,
            dragEnabled: true,
            hideOnOutsideClick: false,
            showCloseButton: true,
            position: {
                at: 'center',
                my: 'center',
            },
        }).dxPopup('instance');

        popup.show();
    }

        function OnClosing(s, e) {  
            var isOuterMouseClick = e.closeReason === ASPxClientPopupControlCloseReason.OuterMouseClick;
            if(isOuterMouseClick)
                e.cancel = isOuterMouseClick;  
        }

    
</script>

<dx:ASPxLoadingPanel ID="loadingPanel" runat="server" Text="Please Wait ..." ClientInstanceName="loadingPanel"
    Modal="True">
</dx:ASPxLoadingPanel>

<div class="col-md-12 col-sm-12 col-xs-12 formLayout-addPopupContainer">
    <div class="row">
        <div class="import-label"><span>Select file</span></div>
        <div>&nbsp;</div>
        <div>
            <dx:ASPxSpreadsheet ID="ASPxSpreadsheet1" runat="server" Visible="false"></dx:ASPxSpreadsheet>
            <asp:FileUpload ID="flpImport" runat="server" />
        </div>
    </div>
    <div class="row" id="trUserType" runat="server" visible="false">
        <td>
            <b>Select User Type</b>
        </td>
        <td>
            <dx:ASPxComboBox runat="server" ID="ddlUserType">
                <Items>
                    <dx:ListEditItem Text="AD User" Value="0" Selected="true" />
                    <dx:ListEditItem Text="FBA User" Value="1" />
                </Items>
            </dx:ASPxComboBox>
        </td>
    </div>
    <div class="row">
        <div class="ms-formbody accomp_inputField import-chkBoxContainer">
            <asp:CheckBox ID="chkDeleteExistingRecord" runat="server" Visible="false" Text="Delete Existing Records" CssClass="replaceUser-popupchkbox RMM-checkWrap"/>
        </div>
        <div class="ms-formbody accomp_inputField import-chkBoxContainer">
            <asp:CheckBox ID="chkRuninBackground" runat="server" Checked="false" Text="Run In Background" CssClass="replaceUser-popupchkbox RMM-checkWrap" />
        </div>
    </div>
    <div class="row">
        <div>
            <asp:Label ID="lblMessage" runat="server" Visible="false" ForeColor="Red"></asp:Label>
        </div>
    </div>
    <div class="row addEditPopup-btnWrap">
        <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel" OnClick="btnCancel_Click" CssClass="secondary-cancelBtn">
        </dx:ASPxButton>
        <dx:ASPxButton ID="lnkMasterImport" runat="server" Text="Master Import" ToolTip="Master Import" Visible="false" CssClass="primary-blueBtn" OnClick="lnkMasterImport_Click">
            <ClientSideEvents Click="function(s,e){return ShowLoader();}" />
        </dx:ASPxButton>
        <dx:ASPxButton ID="btnImport" runat="server" Text="Import" ToolTip="Import" OnClick="btnImport_Click" CssClass="primary-blueBtn">
            <ClientSideEvents  Click="function(s, e){return DeleteRequestTypeConfirm();}" />
        </dx:ASPxButton>
        <%--<div class="popupBtnWrap">
            <div class="popuplinkBtn-Cancel ">
               <%-- <asp:LinkButton ID="btnCancel1" runat="server" Text="&nbsp;&nbsp;Cancel&nbsp;&nbsp;" ToolTip="Cancel" CssClass="" OnClick="btnCancel_Click">
                    <span class="import-cancelBtn">
                        <b>Cancel</b>
                    </span>
                </asp:LinkButton>   
            </div>
            <div class="popuplinkBtn-save">
                <asp:LinkButton ID="lnkMasterImport1" Visible="false"  runat="server" Text="&nbsp;&nbsp;Master Import&nbsp;&nbsp;" ToolTip="Master Import" 
                    OnClick="lnkMasterImport_Click" OnClientClick="return ShowLoader();" CssClass="">
                        <span class="btnSave">
                            <b>Master Import</b>
                        </span>
                </asp:LinkButton>
            </div>
            <div class="popuplinkBtn-save">
                <asp:LinkButton ID="btnImport1" runat="server" Visible="false" Text="&nbsp;&nbsp;Import&nbsp;&nbsp;" ToolTip="Import" OnClick="btnImport_Click"
                    OnClientClick="return DeleteRequestTypeConfirm();" CssClass="">
                        <span class="btnSave">
                            <b>Import</b>
                        </span>
                </asp:LinkButton>
            </div>
            
        </div>--%>
    </div>
</div>

<div id="DivSkippedDetail">
</div>

<div>
</div>

<dx:ASPxPopupControl ID="SummaryPopup" runat="server" ClientInstanceName="SummaryPopup"
    Modal="True" Width="450px" Height="210px" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
    HeaderText="Summary" AllowDragging="true" AllowResize="true" PopupAnimationType="None" EnableViewState="False">
    <ClientSideEvents Closing="OnClosing" /> 
    <ContentCollection>
        <dx:PopupControlContentControl>
            <div style="width: 425px;height: 160px;overflow-y: auto;" id="dvSummary" runat="server">
            </div>
        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ServiceImport.ascx.cs" Inherits="uGovernIT.Web.ServiceImport" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .ms-formbody {
        /*background: none repeat scroll 0 0 #E8EDED;
        border-top: 1px solid #A5A5A5;*/
        padding: 3px 6px 4px;
        vertical-align: top;
    }

    /*.ms-formlabel {
        text-align: right;
        width: 190px;
        vertical-align: top;
    }

    .ms-standardheader {
        text-align: right;
    }*/

    .fileUploadCss {
        width: 320px;
        height: 22px;
        margin: 0;
        border: 1px solid #777;
        float: left;
        -webkit-box-sizing: border-box;
        -moz-box-sizing: border-box;
    }

    input[type="file"].fileUploadCss::-webkit-file-upload-button {
        float: right;
        position: relative;
        top: -1px;
        right: -1px;
    }

    .gridheader {
        height: 20px;
        background-color: #CED8D9;
        text-align: left;
        font-weight: normal;
        border: 1px solid black;
    }

    .gridRow {
        height: 20px;
        text-align: left;
        font-weight: normal;
        border: 1px solid black;
    }
    /*.serviceButton {
        float: right;
        padding-right:5px;
    }*/

    .import-error {}
    .import-error div {        padding: 10px;
    text-align: center;
    color: red; }
    .import-error h3 {font-size: 12px;
    padding-top: 10px;
    padding-bottom: 5px;}
    .import-error h4 {    padding-left: 20px;
    padding-bottom: 5px;}
    .import-error h4 b { color:#db4382; }
    .divButtons .ugit-button {
        float:right;
       margin-left:5px;
    }
</style>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    $(function () {
        $('input[type="file"]').change(function (e) {
            var show = false;
            var message = '';
            $.each(e.target.files, function (index, value) {
                if (value.name.endsWith('.zip') || !value.name.endsWith('.xml')) {
                    show = true;
                    return;
                }
            });
            if (show) {
                alert('Only .xml file allowed.');
                btImportFiles.SetEnabled(false);
                return false;
            }
            else {
                btImportFiles.SetEnabled(true);
            }

        });
    });

    function ValidateFileUpload(s, e) {
        if ($('#<%=serviceFiles.ClientID%>').val() == '') {
            alert('Select Service file to Import')
            e.processOnServer = false;
        }
        else {
            e.processOnServer = true;
        }
    }

    function ValidateCreateService(s, e) {
        if ($('#<%=gvServices.ClientID%> tr').length < 2) {
            alert('Upload Service file to Import')
            e.processOnServer = false;
        }
        else {
            e.processOnServer = true;
        }
    }
</script>

<div class="col-md-12 col-sm-12 col-xs-12 formLayout-addPopupContainer">
    <div class="ms-formtable accomp-popup">
        <div class="row">
            <asp:Label ID="lblErrorMessage" runat="server" Text="Please select a file to import service." Visible="false" Style="color: red; font-weight: bold;"></asp:Label>
        </div>
        <div class="row">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Select File</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <div style="display: block; padding-bottom: 10px;">
                    <input type="file" name="myFiles" runat="server" id="serviceFiles" size="36" multiple="multiple" accept=".xml" />
                </div>
                <div style="display: block;">
                    <dx:ASPxButton ID="btImportFiles" runat="server" Text="Upload" ToolTip="Upload" OnClick="btImportFiles_Click" ClientInstanceName="btImportFiles"
                        AutoPostBack="false" CssClass="primary-blueBtn">
                        <%--<Image Url="/content/images/file-upload.png"></Image>--%>
                        <ClientSideEvents Click="ValidateFileUpload" />
                    </dx:ASPxButton>
                </div>
            </div>
        </div>
        <div class="row" id="trServices" runat="server" style="visibility: hidden">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Uploaded Files</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:GridView ID="gvServices" runat="server" Width="100%" AlternatingRowStyle-BackColor="WhiteSmoke" Style="padding-top: 10px;"
                    HeaderStyle-Height="20px" HeaderStyle-CssClass="gridheader" RowStyle-CssClass="gridRow" HeaderStyle-Font-Bold="false" AutoGenerateColumns="false"
                    GridLines="Both">
                    <Columns>
                        <asp:TemplateField HeaderStyle-Height="2" HeaderText="Files">
                            <ItemTemplate>
                                <asp:Label ID="lblServiceText" runat="server" Text="<%#Container.DataItem %>"></asp:Label>
                            </ItemTemplate>

                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>

        <div class="row">
            <div style="float: right;">
                <dx:ASPxButton ID="btnCreateServices" runat="server" CssClass="primary-blueBtn" Text="Create Services" ToolTip="Create Services" OnClick="btnCreateServices_Click">
                    <%--<Image Url="/content/images/editNewIcon.png" Width="16"></Image>--%>
                    <ClientSideEvents Click="ValidateCreateService" />
                </dx:ASPxButton>
            </div>
        </div>

        <div class="row">
            <asp:HiddenField ID="hdnSurveyForModuleExist" runat="server" Value="false" />
            <asp:HiddenField ID="hdnserviceExists" runat="server" Value="false" />
            <asp:Label ID="lblErrors" runat="server" Text="Following errors exists in the service. Are you sure you want to create service?" Visible="false" Style="font-weight: bold; padding-left: 5px; color: red"></asp:Label>
            <asp:Panel ID="pnlErrorSection" runat="server" Style="background: none repeat scroll 0 0 #E8EDED; font-weight: bold; border: 1px black; padding: 5px;" Visible="false">
                <div id="divError" runat="server" class="import-error" width="100%"></div>
                <div id="divButtons" style="height: 35px;" runat="server" class="divButtons row serviceList-footerBtnWrap">
                    <div style="float: right;">
                        <dx:ASPxButton ID="btNewService" CssClass="primary-blueBtn" runat="server" Text="Proceed To Create Services" ToolTip="Upload" OnClick="btnNewServices_Click"></dx:ASPxButton>
                        <dx:ASPxButton Height="23" AutoPostBack="true" CssClass="primary-blueBtn" ImagePosition="Right" OnClick="btnUpdateService_Click" ID="btnUpdateService" Visible="false" runat="server" Text="Update Existing"></dx:ASPxButton>
                    </div>

                    <div style="float: right; padding-right: 5px;">
                        <dx:ASPxButton ID="btAbort" runat="server" CssClass="primary-blueBtn" Text="Abort" ToolTip="Abort" OnClick="btnCancel_Click"></dx:ASPxButton>
                    </div>
                </div>
            </asp:Panel>
        </div>
    </div>
</div>


<%--<tr id="tr3" runat="server">
            <td class="ms-formlabel">
                <h3 class="ms-standardheader">Selecti
                </h3>
            </td>
            <td class="ms-formbody">
                <asp:RadioButtonList ID="rdSelectionType" runat="server" AutoPostBack="true">
                    <asp:ListItem Text="Single" Value="single"></asp:ListItem>
                    <asp:ListItem Text="Multiple" Value="multiple"></asp:ListItem>
                </asp:RadioButtonList>
            </td>

        </tr>--%>


<%-- <tr id="trServicaCategoriers" runat="server" visible="false">
            <td>
                <label id="lblCategory">Category</label>
                <asp:Label ID="lblServiceCategories" Text="Service Category does not exist." runat="server" Visible="false"></asp:Label>
              
            </td>
        </tr>
        <tr id="trServiceOwner" runat="server" visible="false">
            <td>
                <label id="lblOwners">Owner</label>
                <asp:Label ID="lblOwnerExists" Text="Owner does not exist." runat="server"></asp:Label>
            </td>
        </tr>
        <tr id="trAuthiorizedToRun" runat="server" visible="false">
            <td>
                <label id="Label1">Authorize To Run</label>
                <asp:Label ID="lblAuthorizeToRunUser" Text="User does not exist." runat="server"></asp:Label>
            </td>
        </tr>
        <tr id="trServicetasks" runat="server" visible="false">
            <td>
                <label id="Label2">Servicetasks</label>
                <asp:Label ID="lblServiceTasks" runat="server" Visible="false"></asp:Label>
            </td>
        </tr>--%>
        
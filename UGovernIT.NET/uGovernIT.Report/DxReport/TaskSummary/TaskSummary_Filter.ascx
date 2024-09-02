<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TaskSummary_Filter.ascx.cs" Inherits="uGovernIT.Report.DxReport.TaskSummary_Filter" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.XtraReports.v22.1.Web.WebForms, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>
<style>
    .ms-formbody {
        background: none repeat scroll 0 0 #E8EDED;
        border-top: 1px solid #A5A5A5;
        padding: 3px 6px 4px;
        vertical-align: top;
        width: 530px;
    }

    .ms-formlabel {
        text-align: right;
        width: 100px;
        vertical-align: middle;
        font-weight: normal;
    }

    .full_width {
        width: 100%;
    }

    .main_heading {
        background: none repeat scroll 0 0 #BED0E5;
        float: left;
        font-size: 20px;
        font-weight: bold;
        margin-bottom: 5px;
        padding: 0;
        text-align: center;
        width: 100%;
    }

    .table_lst {
        width: 100%;
    }

    .button_lst {
        width: 70px;
        margin: 5px;
    }

    .listbox_lst {
        width: 150px;
        height: 100px;
    }

    .td_lst {
        /*width: 33%;*/
        padding: 5px 5px;
        vertical-align: middle;
        text-align: center;
    }

    .error {
        color: red;
    }

    .rbutton {
        float: left;
        width: 100px;
    }

        .rbutton label {
            vertical-align: middle;
        }
</style>

<script>
    function UpdateButtonStateProjectType() {
        btnMoveAllItemsToRightProjType.SetEnabled(lstProjTypeLHS.GetItemCount() > 0);
        btnMoveAllItemsToLeftProjType.SetEnabled(lstProjTypeRHS.GetItemCount() > 0);
        btnMoveSelectedItemsToRightProjType.SetEnabled(lstProjTypeLHS.GetSelectedItems().length > 0);
        btnMoveSelectedItemsToLeftProjType.SetEnabled(lstProjTypeRHS.GetSelectedItems().length > 0);
    }

    function UpdateButtonStateFuncArea() {
        btnMoveAllItemsToRightFuncArea.SetEnabled(lstFunctionAreaLHS.GetItemCount() > 0);
        btnMoveAllItemsToLeftFuncArea.SetEnabled(lstFunctionAreaRHS.GetItemCount() > 0);
        btnMoveSelectedItemsToRightFuncArea.SetEnabled(lstFunctionAreaLHS.GetSelectedItems().length > 0);
        btnMoveSelectedItemsToLeftFuncArea.SetEnabled(lstFunctionAreaRHS.GetSelectedItems().length > 0);
    }

    function UpdateButtonStateProj() {
        btnMoveAllItemsToRightProj.SetEnabled(lstProjectsLHS.GetItemCount() > 0);
        btnMoveAllItemsToLeftProj.SetEnabled(lstProjectsRHS.GetItemCount() > 0);
        btnMoveSelectedItemsToRightProj.SetEnabled(lstProjectsLHS.GetSelectedItems().length > 0);
        btnMoveSelectedItemsToLeftProj.SetEnabled(lstProjectsRHS.GetSelectedItems().length > 0);
    }
</script>
<asp:ScriptManager runat="server" ID="scrptmanager"></asp:ScriptManager>
<asp:Panel runat="server" ID="pnlFilter">

    <div>
        <asp:Label ID="lblMsg" Text="" runat="server" CssClass="error" />
    </div>
    <table style="width: 808px;">
        <tr>
            <td style="vertical-align: top;">
                <asp:UpdatePanel ID="upPanel" runat="server">
                    <ContentTemplate>
                        <div style="width: 100%">
                            <fieldset>
                                <legend>
                                    <asp:Label ID="lblHeader" Text="Select Project(s):" runat="server" /></legend>
                                <table>
                                    <%--<tr runat="server" id="trmodule" visible="false">
                                        <td class="ms-formlabel">Modules</td>
                                        <td class="ms-formbody">
                                            <asp:DropDownList ID="ddlmodule" runat="server" Width="300px" AutoPostBack="true" OnSelectedIndexChanged="ddlmodule_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>--%>
                                    <tr>
                                        <td class="ms-formlabel">Status</td>
                                        <td class="ms-formbody">
                                            <span class="rbutton">
                                                <asp:RadioButton ID="rbOpen" runat="server" GroupName="TicketStatus"
                                                    AutoPostBack="true" OnCheckedChanged="rbOpen_CheckedChanged" Checked="true" />
                                                <label>Open</label>
                                            </span>
                                            <span class="rbutton">
                                                <asp:RadioButton ID="rbClose" runat="server" GroupName="TicketStatus"
                                                    AutoPostBack="true" OnCheckedChanged="rbOpen_CheckedChanged" />
                                                <label>Closed</label>
                                            </span>
                                            <span class="rbutton">
                                                <asp:RadioButton ID="rbAll" runat="server" GroupName="TicketStatus"
                                                    AutoPostBack="true" OnCheckedChanged="rbOpen_CheckedChanged" />
                                                <label>All</label>
                                            </span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="ms-formlabel">
                                            <asp:Label ID="lblProjectClass" Text="Project Class" runat="server" /></td>
                                        <td class="ms-formbody">
                                            <div>
                                                <table class="table_lst">
                                                    <tr>
                                                        <td>
                                                            <dx:aspxlistbox id="lstProjTypeLHS" runat="server" valuetype="System.Int32" cssclass="list_box"
                                                                width="265px" height="135px" selectionmode="CheckColumn" clientinstancename="lstProjTypeLHS"
                                                                font-names="Verdana" font-size="8pt">
                                                                <ItemStyle Wrap="True" />
                                                                <ClientSideEvents SelectedIndexChanged="function(s, e) { UpdateButtonStateProjectType(); }" />
                                                            </dx:aspxlistbox>
                                                        </td>
                                                        <td class="td_lst">
                                                            <div>
                                                                <dx:aspxbutton id="btnMoveSelectedItemsToRightProjType" runat="server" autopostback="True" text="Add >"
                                                                    width="110px" height="23px" clientenabled="False" onclick="btnMoveSelectedItemsToRightProjType_Click"
                                                                    tooltip="Add selected items" clientinstancename="btnMoveSelectedItemsToRightProjType" cssclass="button_list" font-names="Verdana" font-size="8pt">
                                                                </dx:aspxbutton>
                                                            </div>
                                                            <div>
                                                                <dx:aspxbutton id="btnMoveAllItemsToRightProjType" runat="server" autopostback="True" text="Add All >>"
                                                                    width="110px" height="23px" tooltip="Add all items" onclick="btnMoveAllItemsToRightProjType_Click"
                                                                    clientinstancename="btnMoveAllItemsToRightProjType" font-names="Verdana" font-size="8pt">
                                                                </dx:aspxbutton>
                                                            </div>
                                                            <div style="height: 10px"></div>
                                                            <div>
                                                                <dx:aspxbutton id="btnMoveSelectedItemsToLeftProjType" runat="server" autopostback="True" text="< Remove"
                                                                    width="110px" height="23px" clientenabled="False" onclick="btnMoveSelectedItemsToLeftProjType_Click"
                                                                    tooltip="Remove selected items" clientinstancename="btnMoveSelectedItemsToLeftProjType" font-names="Verdana" font-size="8pt">
                                                                </dx:aspxbutton>
                                                            </div>
                                                            <div class="TopPadding">
                                                                <dx:aspxbutton id="btnMoveAllItemsToLeftProjType" runat="server" autopostback="True" text="<< Remove All"
                                                                    width="110px" height="23px" clientenabled="False" onclick="btnMoveAllItemsToLeftProjType_Click"
                                                                    tooltip="Remove all items" clientinstancename="btnMoveAllItemsToLeftProjType" font-names="Verdana" font-size="8pt">
                                                                </dx:aspxbutton>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <dx:aspxlistbox id="lstProjTypeRHS" runat="server" clientinstancename="lstProjTypeRHS"
                                                                width="265px" height="135px" selectionmode="CheckColumn" cssclass="list_box" font-names="Verdana" font-size="8pt">
                                                                <ItemStyle Wrap="True" />
                                                                <ClientSideEvents SelectedIndexChanged="function(s, e) { UpdateButtonStateProjectType(); }" />
                                                            </dx:aspxlistbox>

                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="ms-formlabel">Functional Area</td>
                                        <td class="ms-formbody">
                                            <div>
                                                <table class="table_lst">
                                                    <tr>
                                                        <td>
                                                            <dx:aspxlistbox id="lstFunctionAreaLHS" runat="server" clientinstancename="lstFunctionAreaLHS"
                                                                width="265px" height="135px" selectionmode="CheckColumn" cssclass="list_box" font-names="Verdana" font-size="8pt">
                                                                <ItemStyle Wrap="True" />
                                                                <ClientSideEvents SelectedIndexChanged="function(s, e) { UpdateButtonStateFuncArea(); }" />
                                                            </dx:aspxlistbox>
                                                        </td>
                                                        <td class="td_lst">
                                                            <div>
                                                                <dx:aspxbutton id="btnMoveSelectedItemsToRightFuncArea" runat="server" autopostback="True" text="Add >"
                                                                    width="110px" height="23px" clientenabled="False" onclick="btnMoveSelectedItemsToRightFuncArea_Click"
                                                                    tooltip="Add selected items" clientinstancename="btnMoveSelectedItemsToRightFuncArea" font-names="Verdana" font-size="8pt">
                                                                </dx:aspxbutton>
                                                            </div>
                                                            <div>
                                                                <dx:aspxbutton id="btnMoveAllItemsToRightFuncArea" runat="server" autopostback="True" text="Add All >>"
                                                                    width="110px" height="23px" tooltip="Add all items" onclick="btnMoveAllItemsToRightFuncArea_Click"
                                                                    clientinstancename="btnMoveAllItemsToRightFuncArea" font-names="Verdana" font-size="8pt">
                                                                </dx:aspxbutton>
                                                            </div>
                                                            <div style="height: 10px"></div>
                                                            <div>
                                                                <dx:aspxbutton id="btnMoveSelectedItemsToLeftFuncArea" runat="server" autopostback="True" text="< Remove"
                                                                    width="110px" height="23px" clientenabled="False" onclick="btnMoveSelectedItemsToLeftFuncArea_Click"
                                                                    tooltip="Remove selected items" clientinstancename="btnMoveSelectedItemsToLeftFuncArea" font-names="Verdana" font-size="8pt">
                                                                </dx:aspxbutton>
                                                            </div>
                                                            <div class="TopPadding">
                                                                <dx:aspxbutton id="btnMoveAllItemsToLeftFuncArea" runat="server" autopostback="True" text="<< Remove All"
                                                                    width="110px" height="23px" clientenabled="False" onclick="btnMoveAllItemsToLeftFuncArea_Click"
                                                                    tooltip="Remove all items" clientinstancename="btnMoveAllItemsToLeftFuncArea" font-names="Verdana" font-size="8pt">
                                                                </dx:aspxbutton>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <dx:aspxlistbox id="lstFunctionAreaRHS" runat="server" clientinstancename="lstFunctionAreaRHS"
                                                                width="265px" height="135px" selectionmode="CheckColumn" cssclass="list_box" font-names="Verdana" font-size="8pt">
                                                                <ItemStyle Wrap="True" />
                                                                <ClientSideEvents SelectedIndexChanged="function(s, e) { UpdateButtonStateFuncArea(); }" />
                                                            </dx:aspxlistbox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="ms-formlabel">
                                            <asp:Label ID="lblProjects" Text="Projects" runat="server" /></td>
                                        <td class="ms-formbody">
                                            <div>
                                                <table class="table_lst">
                                                    <tr>
                                                        <td>
                                                            <dx:aspxlistbox id="lstProjectsLHS" runat="server" clientinstancename="lstProjectsLHS"
                                                                width="265px" height="135px" selectionmode="CheckColumn" cssclass="list_box" font-names="Verdana" font-size="8pt">
                                                                <ItemStyle Wrap="True" />
                                                                <ClientSideEvents SelectedIndexChanged="function(s, e) { UpdateButtonStateProj(); }" />
                                                            </dx:aspxlistbox>
                                                        </td>
                                                        <td class="td_lst">
                                                            <div>
                                                                <dx:aspxbutton id="btnMoveSelectedItemsToRightProj" runat="server" autopostback="True" text="Add >"
                                                                    width="110px" height="23px" clientenabled="False" onclick="btnMoveSelectedItemsToRightProj_Click"
                                                                    tooltip="Add selected items" clientinstancename="btnMoveSelectedItemsToRightProj" font-names="Verdana" font-size="8pt">
                                                                </dx:aspxbutton>
                                                            </div>
                                                            <div>
                                                                <dx:aspxbutton id="btnMoveAllItemsToRightProj" runat="server" autopostback="True" text="Add All >>"
                                                                    width="110px" height="23px" tooltip="Add all items" onclick="btnMoveAllItemsToRightProj_Click"
                                                                    clientinstancename="btnMoveAllItemsToRightProj" font-names="Verdana" font-size="8pt">
                                                                </dx:aspxbutton>
                                                            </div>
                                                            <div style="height: 10px"></div>
                                                            <div>
                                                                <dx:aspxbutton id="btnMoveSelectedItemsToLeftProj" runat="server" autopostback="True" text="< Remove"
                                                                    width="110px" height="23px" clientenabled="False" onclick="btnMoveSelectedItemsToLeftProj_Click"
                                                                    tooltip="Remove selected items" clientinstancename="btnMoveSelectedItemsToLeftProj" font-names="Verdana" font-size="8pt">
                                                                </dx:aspxbutton>
                                                            </div>
                                                            <div class="TopPadding">
                                                                <dx:aspxbutton id="btnMoveAllItemsToLeftProj" runat="server" autopostback="True" text="<< Remove All"
                                                                    width="110px" height="23px" clientenabled="False" onclick="btnMoveAllItemsToLeftProj_Click"
                                                                    tooltip="Remove all items" clientinstancename="btnMoveAllItemsToLeftProj" font-names="Verdana" font-size="8pt">
                                                                </dx:aspxbutton>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <dx:aspxlistbox id="lstProjectsRHS" runat="server" clientinstancename="lstProjectsRHS"
                                                                width="265px" height="135px" selectionmode="CheckColumn" cssclass="list_box" font-names="Verdana" font-size="8pt">
                                                                <ItemStyle Wrap="True" />
                                                                <ClientSideEvents SelectedIndexChanged="function(s, e) { UpdateButtonStateProj(); }" />
                                                            </dx:aspxlistbox>

                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>

                                </table>
                            </fieldset>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>

        </tr>
        <tr>
            <td>
                <div class="first_tier_nav">
                    <table style="width: 100%; float: right">
                        <tr>

                            <td style="text-align: right;">
                                <ul style="margin: 0px;">
                                    <li runat="server" id="Li1" style="float: right;">
                                        <dx:AspxButton id="lnkCancel" runat="server" text="Cancel" onclick="lnkCancel_Click" autopostback="false">
                                                    <Image Url="/content/images/cancelwhite.png"></Image>
                                                </dx:AspxButton>
                                    </li>
                                    <li runat="server" id="liBuild" class=""  style="float: right;">
                                      <dx:ASPxButton ID="lnkBuild" runat="server" Text="Build Report" AutoPostBack="false" OnClick="lnkBuild_Click">
                                              <Image Url="/content/images/GanttChart1.png"></Image>
                                        </dx:ASPxButton>
                                    </li>
                                </ul>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>

</asp:Panel>


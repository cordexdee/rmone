<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SubLocationView.ascx.cs" Inherits="uGovernIT.Web.SubLocationView" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxSpreadsheet.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxSpreadsheet" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script data-v="<%=UGITUtility.AssemblyVersion %>">
    function grdvUserList_click(s, e) {
        var selectedIDs = s.GetSelectedKeysOnPage();
        if (selectedIDs.length > 0) {
            gvSubLocation.PerformCallback("SelectedLocationId|" + selectedIDs[0]);
        }
    }
    function callBeforeSave() {
        if (Page_IsValid) {
            loadingPanel.Show();
        }
    }
    function newLocation(obj) {
        resetEditLocationBox();

        var selectedIDs = gvLocation.GetSelectedKeysOnPage();
        $('.ddlLocation select').val(selectedIDs[0]);
        editLocationBox.SetHeaderText("New Location:");
        editLocationBox.ShowAtElement(obj);
        editLocationBox.Show();
        return false;
    }
    function resetEditLocationBox() {
        $(".editlocationcontent :text").each(function (i, item) {
            $(item).val("");
        });

        $(".errormessage").hide();

        $(".editlocationcontent select").each(function (i, item) {
            $(item).val("");
        });
        $(".editlocationcontent textarea").each(function (i, item) {
            $(item).val("");
        });
        
        $(".editlocationcontent input:hidden").each(function (i, item) {
            $(item).val("");
        });
        $(".editlocationcontent input:checkbox").removeAttr("checked")
    }

    function downloadExcel(s, e) {
        var exportUrl = window.location.href;
        exportUrl += "&initiateExport=true&exportType=excel";
        window.open(exportUrl, "_blank", "height=400,width=600,resizable=0,status=0,toolbar=0,location=0");
        return false;
    }

    function grdvUserList_init(s, e) {
        sampleSplitter.SetHeight($(document).height() - 60);
        s.SetHeight($(document).height() - 65);
        gvSubLocation.SetHeight($(document).height() - 65);
    }
</script>
<dx:ASPxLoadingPanel runat="server" ID="loadingPanel" ClientInstanceName="loadingPanel"></dx:ASPxLoadingPanel>
<dx:ASPxSpreadsheet ID="ASPxSpreadsheet1" runat="server" Visible="false"></dx:ASPxSpreadsheet>
<dx:ASPxPopupControl ClientInstanceName="editLocationBox" ID="editLocationBox" SettingsAdaptivity-Mode="Always"
    ShowFooter="false" ShowHeader="true" CssClass="aspxPopup" HeaderText="Edit Location:" Modal="true" Width="550px"
    runat="server" EnableViewState="false" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" EnableHierarchyRecreation="True">
    <ContentCollection>
        <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
            <div class="col-md-12 col-sm-12 col-xs-12 noPadding">
                <div class="ms-formtable accomp-popup editlocationcontent row">
                    <div class="col-md-6 col-sm-6 col-xs-6 noPadding">
                        <div class="ms-formlabel">
                            <h3 class="ms-standardheader budget_fieldLabel">Location<b style="color: Red;">*</b></h3>
                        </div>
                        <div class="ms-formbody accomp_inputField">
                            <div class="ddlLocation" id="divddlLocation" runat="server" style="float: left; width:100%">
                                <asp:DropDownList ID="ddlLocation" runat="server" Width="100%" CssClass="itsmDropDownList aspxDropDownList" ValidationGroup="Save"></asp:DropDownList>
                                <asp:RequiredFieldValidator InitialValue="0" CssClass="errormessage" ID="RequiredFieldValidator1" ControlToValidate="ddlLocation" runat="server" ValidationGroup="Save" Display="Dynamic" ErrorMessage="Please select location"></asp:RequiredFieldValidator>
                            </div>

                        </div>
                    </div>
                    <div class="col-md-6 col-sm-6 col-xs-6 noPadding">
                        <div class="ms-formlabel">
                            <h3 class="ms-standardheader budget_fieldLabel">Location Tag</h3>
                        </div>
                        <div class="ms-formbody accomp_inputField">
                            <asp:TextBox ID="txtLocationTag" runat="server" />
                            <asp:CustomValidator ID="cvFieldValidator1" runat="server" ValidationGroup="Save" ControlToValidate="txtLocationTag"
                                Display="Dynamic" ErrorMessage="Location Tag already exist,must be unique." CssClass="error"></asp:CustomValidator>
                        </div>
                    </div>
                    <div class="col-md-6 col-sm-6 col-xs-6 noPadding" id="trTitle" runat="server">
                        <div class="ms-formlabel">
                            <h3 class="ms-standardheader budget_fieldLabel">Sub Location<b style="color: Red;">*</b></h3>
                        </div>
                        <div class="ms-formbody accomp_inputField">
                            <asp:HiddenField ID="hdnSelectedLocation" runat="server" />
                            <asp:TextBox ID="txtSubLocationTitle" runat="server" ValidationGroup="Save" />
                            <asp:RequiredFieldValidator CssClass="errormessage" ID="rfvTxtTitle" ControlToValidate="txtSubLocationTitle" runat="server" ValidationGroup="Save" Display="Dynamic" ErrorMessage="Please enter sublocation"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="col-md-6 col-sm-6 col-xs-6 noPadding" id="tr3" runat="server">
                        <div class="ms-formlabel">
                            <h3 class="ms-standardheader budget_fieldLabel">Address 1</h3>
                        </div>
                        <div class="ms-formbody accomp_inputField">
                            <asp:TextBox ID="txtAddress1" runat="server" />
                        </div>
                    </div>
                    <div class="col-md-6 col-sm-6 col-xs-6 noPadding" id="tr5" runat="server">
                        <div class="ms-formlabel">
                            <h3 class="ms-standardheader budget_fieldLabel">Address 2</h3>
                        </div>
                        <div class="ms-formbody accomp_inputField">
                            <asp:TextBox ID="txtAddress2" runat="server" />
                        </div>
                    </div>
                    <div class="col-md-6 col-sm-6 col-xs-6 noPadding" id="tr6" runat="server">
                        <div class="ms-formlabel">
                            <h3 class="ms-standardheader budget_fieldLabel">Zip</h3>
                        </div>
                        <div class="ms-formbody accomp_inputField">
                            <asp:TextBox ID="txtZip" runat="server" />
                            <asp:RegularExpressionValidator ID="regextxtZip" CssClass="errormessage" 
                                ErrorMessage="Only numbers allowed." Display="Dynamic" ControlToValidate="txtZip"
                                runat="server" ValidationExpression="^([0-9]+)$" ValidateEmptyText="true" ValidationGroup="Save" />
                        </div>
                    </div>
                    <div class="col-md-6 col-sm-6 col-xs-6 noPadding" id="tr7" runat="server">
                        <div class="ms-formlabel">
                            <h3 class="ms-standardheader budget_fieldLabel">Phone No</h3>
                        </div>
                        <div class="ms-formbody accomp_inputField">
                            <asp:TextBox ID="txtPhone" runat="server" />
                            <asp:RegularExpressionValidator ID="regextxtPhone" CssClass="errormessage" Display="Dynamic"
                                ErrorMessage="Only numbers allowed." ControlToValidate="txtPhone" runat="server" 
                                ValidationExpression="^([0-9]+)$" ValidateEmptyText="true" ValidationGroup="Save" />
                        </div>
                    </div>
                    <div class="col-md-6 col-sm-6 col-xs-6 noPadding" id="tr1" runat="server">
                        <div class="ms-formlabel">
                            <h3 class="ms-standardheader budget_fieldLabel">Description</h3>
                        </div>
                        <div class="ms-formbody accomp_inputField">
                            <asp:TextBox ID="txtDescription" TextMode="MultiLine" CssClass="ms-long" runat="server" Rows="6"
                                cols="20" /></div>
                    </div>
                    <div class="col-md-6 col-sm-6 col-xs-6 noPadding" id="tr11" runat="server">
                        <div class="ms-formlabel">
                            <h3 class="ms-standardheader budget_fieldLabel">Delete</h3>
                        </div>
                        <div class="ms-formbody accomp_inputField crm-checkWrap ">
                            <asp:CheckBox ID="chkDeleted" runat="server" TextAlign="Right" Text="(Prevent use for new item)" />
                        </div>
                    </div>

                    <div class="col-md-12 col-sm-12 col-xs-12 noPadding">
                        <div class="row addEditPopup-btnWrap">
                            <dx:ASPxButton ValidationGroup="Save" ID="btnSave" Visible="true" runat="server" Text="Save" AutoPostBack="true"
                                ToolTip="Save as Template" Style="float:left;margin-right:16px;" CssClass="primary-blueBtn"  OnClick="btnSaveLocation_Click">
                                <ClientSideEvents Click="function(s,e){callBeforeSave();}" />
                            </dx:ASPxButton>
                             <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" CssClass="secondary-cancelBtn" >
                                <ClientSideEvents Click="function(s,e){editLocationBox.Hide();}" />
                            </dx:ASPxButton>
                        </div>
                    </div>
                </div>
                <%--<asp:LinkButton ID="btnSaveLocation" runat="server" OnClientClick="callBeforeSave();" Text="&nbsp;&nbsp;Save&nbsp;&nbsp;" ToolTip="Save" ValidationGroup="Save" OnClick="btnSaveLocation_Click">
                    <span class="button-bg">
                        <b style="float: left; font-weight: normal;">
                            Save</b>
                        <i style="float: left; position: relative; top: -3px;left:2px">
                            <img src="/_layouts/15/images/uGovernIT/ButtonImages/save.png"  style="border:none;" title="" alt=""/>
                        </i> 
                    </span>
                </asp:LinkButton>
                <a href="javascript:Void(0);" onclick="editLocationBox.Hide();"
                    title="Cancel">
                    <span class="button-bg">
                        <b style="float: left; font-weight: normal;">Cancel</b>
                        <i style="float: left; position: relative; top: -3px; left: 2px">
                            <img src="/_layouts/15/images/uGovernIT/ButtonImages/cancelwhite.png" style="border: none;" title="" alt="" />
                        </i>
                    </span>
                </a>--%>
            </div>
        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>

<div class="col-md-12 col-sm-12 col-xs-12">
    <div class="row" style="margin-top:15px; margin-bottom:15px;">
        <div class="col-md-4 co-sm-4 col-xs-4 noPadding">
            <dx:ASPxButton ID="btnApplyChanges" runat="server" CssClass="primary-blueBtn" Text="Apply Changes" ToolTip="Apply Changes"
            OnClick="btnApplyChanges_Click"></dx:ASPxButton>
        </div>
        <div class="col-md-8 co-sm-8 col-xs-8 noPadding">
            <div style="float:right; display:inline-block; margin-left:5px;">
                <dx:ASPxButton ID="btnExport" runat="server" Text="Export" ToolTip="Export" CssClass="primary-blueBtn">
                     <ClientSideEvents Click="function(s, e){return downloadExcel(s, e);}" />
                 </dx:ASPxButton>
            </div>
            <div style="float:right; display:inline-block; margin-left:5px;">
                <a id="aAddItem" onclick="return newLocation(this)" runat="server" class="primary-btn-link" href="">
                    <img id="Img11" runat="server" src="/Content/Images/plus-symbol.png" width="16" />
                    <asp:Label ID="LblAddItem1" runat="server" Text="Add New Item"></asp:Label>
                </a>
            </div>
             <div class="crm-checkWrap" style="float:right;display:inline-block;">
                  <asp:CheckBox ID="chkShowDeleted" Text="Show Deleted" runat="server" TextAlign="Right"
                      AutoPostBack="true" OnCheckedChanged="chkShowDeleted_CheckedChanged" />
             </div>
         </div>
    </div>
    <div class="row" id="content">
        <table width="100%" align="left">
            <tr>
                <td align="left">
                    <dx:ASPxSplitter ID="contenSplitter" runat="server" Width="100%" ClientInstanceName="sampleSplitter" AllowResize="false">
                        <Panes>
                            <dx:SplitterPane Name="Location" AutoWidth="false" AutoHeight="true" Size="50" ScrollBars="Auto">
                                <ContentCollection>
                                    <dx:SplitterContentControl>
                                        <dx:ASPxGridView Settings-VerticalScrollBarMode="Visible" EnableCallBacks="false" 
                                            KeyFieldName="ID" ID="gvLocation" OnDataBinding="gvLocation_DataBinding" 
                                            runat="server" Width="100%" ClientInstanceName="gvLocation" CssClass="customgridview homeGrid">
                                            <ClientSideEvents Init="grdvUserList_init" SelectionChanged="function(s,e){grdvUserList_click(s,e);}" />
                                            <Styles>
                                                <Row CssClass=""></Row>
                                                <SelectedRow BackColor="#DBEAF9"></SelectedRow>
                                                <Header CssClass="homeGrid_headerColumn"></Header>
                                            </Styles>
                                            <SettingsPager Mode="ShowAllRecords"></SettingsPager>
                                            <SettingsBehavior AllowSelectSingleRowOnly="true" AllowSelectByRowClick="true" AllowSort="true" />
                                            <Columns>
                                                <dx:GridViewDataTextColumn Width="170px" Caption="Location" 
                                                    Settings-AllowHeaderFilter="True" Settings-HeaderFilterMode="CheckedList" 
                                                    FieldName="Title"></dx:GridViewDataTextColumn>
                                            </Columns>
                                        </dx:ASPxGridView>
                                    </dx:SplitterContentControl>


                                </ContentCollection>
                            </dx:SplitterPane>
                            <dx:SplitterPane AutoHeight="true" PaneStyle-CssClass="leftSplitterPane" ScrollBars="Auto" AutoWidth="false" Size="200" Name="leftPane">
                                <ContentCollection>
                                    <dx:SplitterContentControl>
                                        <div class="fullwidth">
                                            <dx:ASPxGridView Settings-VerticalScrollBarMode="Visible" KeyFieldName="ID"
                                                ID="gvSubLocation" OnDataBinding="gvSubLocation_DataBinding" runat="server"
                                                Width="100%" ClientInstanceName="gvSubLocation" CssClass="customgridview homeGrid">
                                                <ClientSideEvents />
                                                <Styles AlternatingRow-CssClass="ms-alternatingstrong">
                                                    <Row CssClass="homeGrid_dataRow"></Row>
                                                    <AlternatingRow CssClass="ms-alternatingstrong"></AlternatingRow>
                                                    <Header CssClass="homeGrid_headerColumn" Font-Bold="true"></Header>
                                                </Styles>
                                                <SettingsPager PageSize="20" Mode="ShowPager"></SettingsPager>

                                                <Columns>
                                                    <dx:GridViewDataTextColumn Caption="" Width="30px">
                                                        <DataItemTemplate>
                                                            <asp:ImageButton ID="btSubLocationEdit" runat="server" Width="16" ImageUrl="/content/Images/editNewIcon.png" OnClick="btSubLocationEdit_Click" OnClientClick="javascript:cancelBubble=true;" CommandArgument='<%# Container.KeyValue %>' />
                                                        </DataItemTemplate>
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Caption="Sub Location">
                                                        <DataItemTemplate>
                                                            <asp:LinkButton ID="lnkSubLocationEdit" runat="server" CommandArgument='<%# Container.KeyValue %>' OnClick="lnkSubLocationEdit_Click" OnClientClick="javascript:cancelBubble=true;">
                                                            <%# Eval("Title") %>
                                                            </asp:LinkButton>
                                                        </DataItemTemplate>
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Caption="Location" FieldName="LocationDetails"></dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Caption="Location Tag" FieldName="LocationTag"></dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Caption="Description" FieldName="Description"></dx:GridViewDataTextColumn>
                                                </Columns>
                                                <FormatConditions>
                                                    <dx:GridViewFormatConditionHighlight FieldName="Description" Format="Custom" ApplyToRow="true" RowStyle-CssClass="formatcolor"                    Expression="[Deleted] = True"></dx:GridViewFormatConditionHighlight>
                                                </FormatConditions>
                                            </dx:ASPxGridView>
                                        </div>
                                    </dx:SplitterContentControl>
                                </ContentCollection>
                            </dx:SplitterPane>
                        </Panes>
                    </dx:ASPxSplitter>
                </td>
            </tr>
        </table>
    </div>
</div>




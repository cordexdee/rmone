<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LocationView.ascx.cs" Inherits="uGovernIT.Web.LocationView" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxSpreadsheet.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxSpreadsheet" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function editRegion(obj, region) {
        $(".editbox :text").val(region);
        var hdnVars = $(".editbox .editboxspan input[type='hidden']");
        $(hdnVars.get(0)).val(region);
        $(hdnVars.get(1)).val("region");
        editBox.ShowAtElement(obj);
        editBox.Show();
        editBox.SetHeaderText("Edit Region:")
        $(".editbox .errormessage").hide();
        $(".editbox .errormessage").text("Please enter region");

        //var url = hdnConfiguration.Get("EditRegionURL");
        //var requestUrl = hdnConfiguration.Get("RequestUrl");
        //UgitOpenPopupDialog(url, 'id=' + id, 'Edit Region', '600px', '300px', 0, escape(requestUrl))
    }

    function editCountry(obj, country) {
        $(".editbox :text").val(country);
        var hdnVars = $(".editbox .editboxspan input[type='hidden']");
        $(hdnVars.get(0)).val(country);
        $(hdnVars.get(1)).val("country");
        editBox.ShowAtElement(obj);
        editBox.Show();
        editBox.SetHeaderText("Edit Country:")
        $(".editbox .errormessage").hide();
        $(".editbox .errormessage").text("Please enter country");
    }

    function editState(obj, state) {
        $(".editbox :text").val(state);
        var hdnVars = $(".editbox .editboxspan input[type='hidden']");
        $(hdnVars.get(0)).val(state);
        $(hdnVars.get(1)).val("state");
        editBox.ShowAtElement(obj);
        editBox.Show();
        editBox.SetHeaderText("Edit State:")
        $(".editbox .errormessage").hide();
        $(".editbox .errormessage").text("Please enter state");

    }

    function newLocation(obj) {
        debugger;
        resetEditLocationBox();
        editLocationBox.SetHeaderText("New Location:");
        editLocationBox.ShowAtElement(obj);
        editLocationBox.Show();
        return false;
    }

    function resetEditLocationBox() {
        $(".editlocationcontent :text").each(function (i, item) {
            $(item).val("");
        });

        $(".editlocationcontent select").each(function (i, item) {
            $(item).val("");
        });

        $(".editlocationcontent input:hidden").each(function (i, item) {
            $(item).val("");
        });
        $(".editlocationcontent input:checkbox").removeAttr("checked")
    }


    function onRegionChanged(s, e) {
        loadingPanel.Show();
    }

    function onCountryChanged(s, e) {

        loadingPanel.Show();
    }

    function onStateChanged(s, e) {
        loadingPanel.Show();
    }

    function onCountryCallEnd(s, e) {
        if (gvCountry.GetFocusedRowIndex() == -1) {
            gvCountry.SetFocusedRowIndex(0);
        }
        gvState.PerformCallback(s.GetFocusedRowIndex());
    }

    function onStateCallEnd(s, e) {
        if (gvState.GetFocusedRowIndex() == -1) {
            gvState.SetFocusedRowIndex(0);
        }
        gvLocation.PerformCallback(s.GetFocusedRowIndex());
    }

    function callBeforeSave() {
        if (Page_IsValid) {
            //loadingPanel.Show();
        }
    }

    function hideddlRegion() {
        $("#<%= ddlRegion.ClientID%>").get(0).selectedIndex = 0;
        $(".ddlRegion").hide();                
        ValidatorEnable(document.getElementById('<%=rfRegion.ClientID %>'), false);
        ValidatorEnable(document.getElementById('<%=rfTxtRegion.ClientID %>'), true);
    }
    function showddlRegion() {
        $(".ddlRegion").show();
        ValidatorEnable(document.getElementById('<%=rfRegion.ClientID %>'), true);
        ValidatorEnable(document.getElementById('<%=rfTxtRegion.ClientID %>'), false);
    }

    function hideddlCountry() {
        $("#<%= ddlCountry.ClientID%>").get(0).selectedIndex = 0;
        $(".ddlCountry").hide();        
        ValidatorEnable(document.getElementById('<%=rfCountry.ClientID %>'), false);                
        ValidatorEnable(document.getElementById('<%=rfTxtCountry.ClientID %>'), true);  
    }
    function showddlCountry() {
        $(".ddlCountry").show();
        ValidatorEnable(document.getElementById('<%=rfCountry.ClientID %>'), true);  
        ValidatorEnable(document.getElementById('<%=rfTxtCountry.ClientID %>'), false);   
    }

    function showddlState() {
        $(".ddlState").show();
        ValidatorEnable(document.getElementById('<%=rfState.ClientID %>'), true);  
        ValidatorEnable(document.getElementById('<%=rfTxtState.ClientID %>'), false);  
    }
    function hideddlState() {
        $("#<%= ddlState.ClientID%>").get(0).selectedIndex = 0;
        $(".ddlState").hide();
        ValidatorEnable(document.getElementById('<%=rfState.ClientID %>'), false);        
        ValidatorEnable(document.getElementById('<%=rfTxtState.ClientID %>'), true);      
    }

    function downloadExcel(obj) {
        var exportUrl = window.location.href;
        exportUrl += "&initiateExport=true&exportType=excel";
        window.open(exportUrl, "_blank", "height=400,width=600,resizable=0,status=0,toolbar=0,location=0");
        return false;
    }

    function OpenImportExcel() {
        window.parent.UgitOpenPopupDialog('<%= importUrl %>', "", 'Import Location', '400px', '200px', 0, escape("<%= Request.Url.AbsolutePath %>"));
        return false;
    }

    function MoveToProduction(obj) {
        var url = '<%=moveToProductionUrl%>';
        var param = "list=" + obj;
        window.parent.UgitOpenPopupDialog(url, param, 'Migrate Change(s)', '300px', '150px', 0, escape("<%= Request.Url.AbsolutePath %>"));
    }

</script>
<dx:ASPxLoadingPanel ID="loadingPanel" runat="server" Text="Wait..." ClientInstanceName="loadingPanel"
    Modal="True">
</dx:ASPxLoadingPanel>
<dx:ASPxHiddenField ID="hdnInformation" runat="server" ClientInstanceName="hdnInformation"></dx:ASPxHiddenField>

<div class="col-md-12 col-sm-12 col-xs-12 formLayout-addPopupContainer">
    <div class="row modules-linkWrap fright">
         <a id="a1" runat="server" onclick="return newLocation(this)" href="" class="primary-btn-link">
            <img id="Img5" runat="server" src="/Content/Images/plus-symbol.png" />
            <asp:Label ID="Label1" runat="server" Text="Add New Item" CssClass="phrasesAdd-label"></asp:Label>
        </a>
        <dx:ASPxButton ID="btnImport" runat="server" Text="Import" Visible="false" ToolTip="Import" CssClass="primary-blueBtn marginLeft15">
            <ClientSideEvents Click="function(s, e){return OpenImportExcel();}" />
        </dx:ASPxButton>
        <dx:ASPxButton ID="btnExport" runat="server" Text="Export" ToolTip="Export" CssClass="primary-blueBtn marginLeft15">
            <ClientSideEvents Click="function(s, e){return downloadExcel(this);}" />
        </dx:ASPxButton>
        <dx:ASPxButton ID="btnMigrateLocation" runat="server" Text="Migrate" ToolTip="Migrate" CssClass="primary-blueBtn" Visible="false">
            <ClientSideEvents Click="function(){MoveToProduction('Location')}" />
        </dx:ASPxButton>
        <dx:ASPxButton ID="btnApplyChanges" runat="server" CssClass="primary-blueBtn marginLeft15" Text="Apply Changes" ToolTip="Apply Changes" OnClick="btnApplyChanges_Click"></dx:ASPxButton>
    </div>
     <div class="row" style="clear:both; padding:15px 0;">
        <div class="showDel-chkWrap crm-checkWrap" style="float:right;">
            <asp:CheckBox ID="chkShowDeleted" Text="Show Deleted&nbsp;" runat="server" AutoPostBack="true" OnCheckedChanged="chkShowDeleted_CheckedChanged" />
        </div>
    </div>
    <div class="row" id="content">
        <dx:ASPxSplitter ID="contenSplitter" runat="server" Width="100%" ClientInstanceName="sampleSplitter" AllowResize="false">
            <Panes>
                <dx:SplitterPane Name="Region" AutoWidth="true" AutoHeight="true" MaxSize="200px" ScrollBars="Auto">
                    <ContentCollection>
                         <dx:SplitterContentControl>
                            <div class="fullwidth">
                                <ugit:ASPxGridView EnableCallBacks="false" KeyFieldName="Region" Width="100%" ClientInstanceName="gvRegion" ID="gvRegion" runat="server" OnHtmlRowPrepared="gvRegion_HtmlRowPrepared">
                                    <SettingsPager Visible="false" Mode="ShowAllRecords"></SettingsPager>
                                    <SettingsBehavior AllowFocusedRow="True" ProcessFocusedRowChangedOnServer="true" />
                                    <ClientSideEvents FocusedRowChanged="function(s, e) {onRegionChanged(s, e);}" />
                                    <Styles AlternatingRow-CssClass="ms-alternatingstrong">
                                        <AlternatingRow CssClass="ms-alternatingstrong"></AlternatingRow>
                                    </Styles>

                                    <Columns>

                                        <dx:GridViewDataTextColumn Caption="" Width="20px">
                                            <DataItemTemplate>
                                                <img src="/Content/Images/editNewIcon.png"  style="cursor: pointer;width:16px;" title="Edit Region" onclick="javascript:cancelBubble=true;editRegion(this, '<%# Container.KeyValue %>')" />
                                            </DataItemTemplate>
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Region">
                                            <DataItemTemplate>
                                                <a href="javascript:void(0);" onclick="javascript:event.cancelBubble=true;editRegion(this, '<%# Container.KeyValue %>')">
                                                    <%# Eval("Region") %>
                                                </a>
                                            </DataItemTemplate>
                                        </dx:GridViewDataTextColumn>
                                    </Columns>
                                    <Styles>
                                        <Row CssClass="homeGrid_dataRow"></Row>
                                        <Header CssClass="homeGrid_headerColumn"></Header>
                                    </Styles>
                                    <FormatConditions>
                                        <dx:GridViewFormatConditionHighlight Format="Custom" ApplyToRow="true" RowStyle-CssClass="formatcolor" Expression="[Deleted] = True"></dx:GridViewFormatConditionHighlight>
                                    </FormatConditions>
                                </ugit:ASPxGridView>
                            </div>
                        </dx:SplitterContentControl>
                    </ContentCollection>
                </dx:SplitterPane>
                <dx:SplitterPane Name="Country" AutoWidth="true" AutoHeight="true" MaxSize="200px" ScrollBars="Auto">
                    <Separator Image-Url="/Content/images/hover-arrow.gif" SeparatorStyle-Paddings-PaddingLeft="1px" SeparatorStyle-Paddings-PaddingRight="1px">
                        <SeparatorStyle>
                            <Paddings PaddingLeft="1px" PaddingRight="1px"></Paddings>
                        </SeparatorStyle>

                        <Image Url="/Content/images/hover-arrow.gif"></Image>
                    </Separator>
                    <ContentCollection>
                        <dx:SplitterContentControl>
                            <div class="fullwidth">
                                <ugit:ASPxGridView EnableCallBacks="false" KeyFieldName="Country" ID="gvCountry" ClientInstanceName="gvCountry" OnDataBinding="gvCountry_DataBinding" OnHtmlRowPrepared="gvCountry_HtmlRowPrepared" OnCustomCallback="gvCountry_CustomCallback" Width="100%" runat="server">
                                    <SettingsPager Visible="false" Mode="ShowAllRecords"></SettingsPager>
                                    <SettingsBehavior AllowFocusedRow="true" ProcessFocusedRowChangedOnServer="true" />
                                    <ClientSideEvents FocusedRowChanged="function(s, e) { onCountryChanged(s,e);}" />
                                    <Styles AlternatingRow-CssClass="ms-alternatingstrong">
                                        <AlternatingRow CssClass="ms-alternatingstrong"></AlternatingRow>
                                    </Styles>
                                    <Columns>
                                        <dx:GridViewDataTextColumn Caption="" Width="20px">
                                            <DataItemTemplate>
                                                <img src="/Content/Images/editNewIcon.png" style="cursor: pointer;width:16px;" title="Edit Country" onclick="javascript:cancelBubble=true;editCountry(this, '<%# Container.KeyValue %>')" />
                                            </DataItemTemplate>
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Country">
                                            <DataItemTemplate>
                                                <a href="javascript:void(0);" onclick="javascript:event.cancelBubble=true;editCountry(this, '<%# Container.KeyValue %>')">
                                                    <%# Eval("Country") %>
                                                </a>
                                            </DataItemTemplate>
                                        </dx:GridViewDataTextColumn>
                                    </Columns>
                                    <Styles>
                                        <Row CssClass="homeGrid_dataRow"></Row>
                                        <Header CssClass="homeGrid_headerColumn"></Header>
                                    </Styles>
                                    <FormatConditions>
                                        <dx:GridViewFormatConditionHighlight Format="Custom" ApplyToRow="true" RowStyle-CssClass="formatcolor" Expression="[Deleted] = True"></dx:GridViewFormatConditionHighlight>
                                    </FormatConditions>
                                </ugit:ASPxGridView>
                            </div>
                        </dx:SplitterContentControl>
                    </ContentCollection>
                </dx:SplitterPane>
                <dx:SplitterPane Name="State" AutoWidth="true" AutoHeight="true" MaxSize="200px" ScrollBars="Auto">
                    <Separator Image-Url="/Content/images/hover-arrow.gif" SeparatorStyle-Paddings-PaddingLeft="1px" SeparatorStyle-Paddings-PaddingRight="1px">
                        <SeparatorStyle>
                            <Paddings PaddingLeft="1px" PaddingRight="1px"></Paddings>
                        </SeparatorStyle>

                        <Image Url="/Content/images/hover-arrow.gif"></Image>
                    </Separator>
                    <ContentCollection>
                        <dx:SplitterContentControl>
                            <div class="fullwidth">
                                <dx:ASPxGridView KeyFieldName="State" EnableCallBacks="false" ID="gvState" ClientInstanceName="gvState" OnDataBinding="gvState_DataBinding" OnHtmlRowPrepared="gvState_HtmlRowPrepared" OnCustomCallback="gvState_CustomCallback" Width="100%" runat="server">
                                    <SettingsPager Visible="false" Mode="ShowAllRecords"></SettingsPager>
                                    <SettingsBehavior AllowFocusedRow="true" ProcessFocusedRowChangedOnServer="true" />
                                    <ClientSideEvents FocusedRowChanged="function(s, e) {onStateChanged(s,e);}" />
                                    <Styles AlternatingRow-CssClass="ms-alternatingstrong">
                                        <AlternatingRow CssClass="ms-alternatingstrong"></AlternatingRow>
                                    </Styles>
                                    <Columns>
                                        <dx:GridViewDataTextColumn Caption="" Width="20px">
                                            <DataItemTemplate>
                                                <img src="/Content/Images/editNewIcon.png"  style="cursor: pointer;width:16px;" title="Edit State" onclick="javascript:cancelBubble=true;editState(this, '<%# Container.KeyValue %>')" />
                                            </DataItemTemplate>
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="State">
                                            <DataItemTemplate>
                                                <a href="javascript:void(0);" onclick="javascript:event.cancelBubble=true;editState(this, '<%# Container.KeyValue %>')">
                                                    <%# Eval("State") %>
                                                </a>
                                            </DataItemTemplate>
                                        </dx:GridViewDataTextColumn>
                                    </Columns>
                                    <Styles>
                                        <Row CssClass="homeGrid_dataRow"></Row>
                                        <Header CssClass="homeGrid_headerColumn"></Header>
                                    </Styles>
                                    <FormatConditions>
                                        <dx:GridViewFormatConditionHighlight Format="Custom" ApplyToRow="true" RowStyle-CssClass="formatcolor" Expression="[Deleted] = True"></dx:GridViewFormatConditionHighlight>
                                    </FormatConditions>
                                </dx:ASPxGridView>
                            </div>
                        </dx:SplitterContentControl>
                    </ContentCollection>
                </dx:SplitterPane>
                <dx:SplitterPane Name="Location" AutoHeight="true" AutoWidth="true">
                    <Separator Image-Url="/Content/images/hover-arrow.gif" SeparatorStyle-Paddings-PaddingLeft="1px" SeparatorStyle-Paddings-PaddingRight="1px">
                        <SeparatorStyle>
                            <Paddings PaddingLeft="1px" PaddingRight="1px"></Paddings>
                        </SeparatorStyle>

                        <Image Url="/Content/images/hover-arrow.gif"></Image>
                    </Separator>

                    <ContentCollection>
                        <dx:SplitterContentControl>
                            <div class="fullwidth">
                                <ugit:ASPxGridView EnableCallBacks="false" KeyFieldName="ID" ID="gvLocation" OnDataBinding="gvLocation_DataBinding" runat="server" Width="100%" OnHtmlRowPrepared="gvLocation_HtmlRowPrepared" ClientInstanceName="gvLocation" OnCustomCallback="gvLocation_CustomCallback">
                                    <ClientSideEvents />
                                    <Styles AlternatingRow-CssClass="ms-alternatingstrong">
                                        <AlternatingRow CssClass="ms-alternatingstrong"></AlternatingRow>
                                    </Styles>
                                    <SettingsPager PageSize="20" Mode="ShowPager"></SettingsPager>

                                    <Columns>
                                        <dx:GridViewDataTextColumn Caption="" Width="20px">
                                            <DataItemTemplate>
                                                <asp:ImageButton ID="btLocationEdit" runat="server" ImageUrl="/Content/Images/editNewIcon.png" style="width:16px;" OnClick="btLocationEdit_Click" OnClientClick="javascript:cancelBubble=true;" CommandArgument='<%# Container.KeyValue %>' />
                                            </DataItemTemplate>
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Location">
                                            <DataItemTemplate>
                                                <asp:LinkButton ID="lnkLocationEdit" runat="server" CommandArgument='<%# Container.KeyValue %>' OnClick="lnkLocationEdit_Click" OnClientClick="javascript:cancelBubble=true;">
                                                <%# Eval("Title") %>
                                                </asp:LinkButton>
                                            </DataItemTemplate>
                                        </dx:GridViewDataTextColumn>
                                    </Columns>
                                     <Styles>
                                        <Row CssClass="homeGrid_dataRow"></Row>
                                        <Header CssClass="homeGrid_headerColumn"></Header>
                                    </Styles>
                                    <FormatConditions>
                                        <dx:GridViewFormatConditionHighlight Format="Custom" ApplyToRow="true" RowStyle-CssClass="formatcolor" Expression="[Deleted] = True"></dx:GridViewFormatConditionHighlight>
                                    </FormatConditions>
                                </ugit:ASPxGridView>
                            </div>
                        </dx:SplitterContentControl>
                    </ContentCollection>
                </dx:SplitterPane>
            </Panes>
        </dx:ASPxSplitter>
    </div>
    <div class="row">
        <div style="float: right; padding: 15px 0 20px;">
            <a id="aAddItem1" runat="server" onclick="return newLocation(this)" href="" class="primary-btn-link">
                <img id="Img1" runat="server" src="/Content/Images/plus-symbol.png" />
                <asp:Label ID="LblAddItem" runat="server" Text="Add New Item" CssClass="phrasesAdd-label"></asp:Label>
            </a>
        </div>
    </div>
</div>
<dx:ASPxPopupControl ClientInstanceName="editBox" ID="editBox" SettingsAdaptivity-Mode="Always"
    ShowFooter="false" ShowHeader="true" CssClass="editbox aspxPopup" HeaderText="Edit Region:" Modal="true"
    runat="server" EnableViewState="false" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" EnableHierarchyRecreation="True">
    <ContentCollection>
        <dx:PopupControlContentControl ID="PopupControlContentControl7" runat="server">
            <div id="baselineBox" class="col-md-12 col-sm-12 col-xs-12">
                <div class="row accomp-popup">
                    <div class="editboxspan ms-formbody accomp_inputField">
                        <asp:TextBox ID="txtTitle" runat="server" ValidationGroup="saveregion"></asp:TextBox>
                        <asp:HiddenField ID="hdnTitle" runat="server" />
                        <asp:HiddenField ID="hdnActionType" runat="server" />
                        <asp:RequiredFieldValidator CssClass="errormessage" ID="rfvTxtTitle" ControlToValidate="txtTitle" runat="server" ValidationGroup="saveregion" Display="Dynamic" ErrorMessage="Please enter region"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="d-flex justify-content-end align-items-center pr-1">
                    <dx:ASPxButton runat="server" CssClass="secondary-cancelBtn" Text="Cancel" ToolTip="Cancel"><ClientSideEvents Click="function(){editBox.Hide();}" /></dx:ASPxButton>
                    <dx:ASPxButton Text="Save" ToolTip="Save" CssClass="primary-blueBtn" OnClick="lnkSaveEdit_Click" ValidationGroup="saveregion" runat="server" ID="lnkSaveEdit"><ClientSideEvents Click="function(s, e){callBeforeSave();}" /></dx:ASPxButton>
                </div>
            </div>
        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>

<dx:ASPxPopupControl ClientInstanceName="editLocationBox" ID="editLocationBox" SettingsAdaptivity-Mode="Always"
    ShowFooter="false" ShowHeader="true" CssClass="editlocationbox aspxPopup" HeaderText="Edit Location:" Modal="true"
    runat="server" EnableViewState="false" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" EnableHierarchyRecreation="True">
    <ContentCollection>
        <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
            <div class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap noPadding">
                <div class="ms-formtable editlocationcontent accomp-popup ">
                    <div class="row" id="trTitle" runat="server">
                        <div class="ms-formlabel">
                            <h3 class="ms-standardheader budget_fieldLabel">Location<b style="color: Red;">*</b></h3>
                        </div>
                        <div class="ms-formbody accomp_inputField">
                            <asp:HiddenField ID="hdnSelectedLocation" runat="server" />
                            <asp:TextBox ID="txtLocationTitle" runat="server" ValidationGroup="Save" />
                            <div>
                                <asp:RequiredFieldValidator ID="rfLocation" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="txtLocationTitle"
                                    ErrorMessage="Please enter Location"  ValidationGroup="SaveLocation"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="ms-formlabel">
                            <h3 class="ms-standardheader budget_fieldLabel">State<b style="color: Red;">*</b></h3>
                        </div>
                        <div class="ms-formbody accomp_inputField">
                            <div class="ddlState" id="divddlState" runat="server" style="width:100%;">
                                <div class="col-md-11 col-sm-11 col-xs-11 noPadding">
                                     <asp:DropDownList ID="ddlState" runat="server" CssClass="itsmDropDownList aspxDropDownList"></asp:DropDownList>   
                                </div>
                                <div class="col-md-1 col-sm-1 col-xs-1 noRightPadding">                             
                                    <img alt="Add State" id="Img3" src="/content/images/plus-blue.png" style="cursor: pointer; width:16px;" 
                                        onclick="javascript:$('.divState').attr('style','display:block');hideddlState();" />
                                </div>
                                <div class="col-xs-12 col-sm-12 noPadding">
                                    <asp:RequiredFieldValidator ID="rfState" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="ddlState"
                                        ErrorMessage="Please enter State"  ValidationGroup="SaveLocation"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div runat="server" id="divState" class="divState" style="display: none; width:100%;">
                                <div class="col-md-11 col-sm-11 col-xs-11 noPadding">
                                    <asp:TextBox runat="server" ID="txtState" CssClass="txtState"></asp:TextBox>
                                </div>
                                <div class="col-md-1 col-sm-1 col-xs-1 noRightPadding"> 
                                    <img alt="Cancel State" width="16" src="/content/images/close-red.png" class="cancelModule" 
                                    onclick="javascript:$('.divState').attr('style','display:none');showddlState();" />
                                </div>
                                 <div class="col-xs-12 col-sm-12 noPadding">
                                     <asp:RequiredFieldValidator ID="rfTxtState" ValidateEmptyText="true" Enabled="false" runat="server" ControlToValidate="txtState"
                                        ErrorMessage="Please enter State"  ValidationGroup="SaveLocation"></asp:RequiredFieldValidator>
                                 </div>
                            </div>                                  
                        </div>
                    </div>
                    <div class="row">
                        <div class="ms-formlabel">
                            <h3 class="ms-standardheader budget_fieldLabel">Country<b style="color: Red;">*</b></h3>
                        </div>
                        <div class="ms-formbody accomp_inputField">
                            <div class="ddlCountry" id="divddlCountry" runat="server" style="float: left; width:100%;">
                                 <div class="col-md-11 col-sm-11 col-xs-11 noPadding">
                                        <asp:DropDownList ID="ddlCountry" runat="server" CssClass="itsmDropDownList aspxDropDownList"></asp:DropDownList>      
                                 </div>
                                 <div class="col-md-1 col-sm-1 col-xs-1 noRightPadding"> 
                                     <img alt="Add Country" id="Img2" src="/content/images/plus-blue.png" style="cursor: pointer; width:16px" 
                                    onclick="javascript:$('.divCountry').attr('style','display:block');hideddlCountry();" />
                                 </div>                        
                                <div class="col-xs-12 col-sm-12 noPadding">
                                    <asp:RequiredFieldValidator ID="rfCountry" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="ddlCountry"
                                        ErrorMessage="Please enter Country"  ValidationGroup="SaveLocation"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div runat="server" id="divCountry" class="divCountry" style="display: none; width:100%;">
                                <div class="col-md-11 col-sm-11 col-xs-11 noPadding">
                                    <asp:TextBox runat="server" ID="txtCountry" CssClass="txtCountry"></asp:TextBox>
                                </div>
                                <div class="col-md-1 col-sm-1 col-xs-1 noRightPadding"> 
                                    <img alt="Cancel Country" width="16" src="/content/images/close-red.png" class="cancelModule" 
                                    onclick="javascript:$('.divCountry').attr('style','display:none');showddlCountry();" />
                                </div>
                                <div class="col-xs-12 col-sm-12 noPadding">
                                    <asp:RequiredFieldValidator ID="rfTxtCountry" ValidateEmptyText="true" Enabled="false" runat="server" ControlToValidate="txtCountry"
                                        ErrorMessage="Please enter Country"  ValidationGroup="SaveLocation"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="ms-formlabel">
                            <h3 class="ms-standardheader budget_fieldLabel">Region<b style="color: Red;">*</b></h3>
                        </div>
                        <div class="ms-formbody accomp_inputField">
                            <div class="ddlRegion" id="divddlRegion" runat="server" style="float: left; width:100%;">
                                <div class="col-md-11 col-sm-11 col-xs-11 noPadding">
                                    <asp:DropDownList ID="ddlRegion" runat="server" CssClass="itsmDropDownList aspxDropDownList"></asp:DropDownList>       
                                </div>
                                <div class="col-md-1 col-sm-1 col-xs-1 noRightPadding">
                                     <img alt="Add Region" id="Img4" src="/content/images/plus-blue.png" style="cursor: pointer; width:16px;" 
                                    onclick="javascript:$('.divRegion').attr('style','display:block');hideddlRegion();" />
                                </div>
                                <div class="col-xs-12 col-sm-12 noPadding">
                                    <asp:RequiredFieldValidator ID="rfRegion" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="ddlRegion"
                                        ErrorMessage="Please enter Region"  ValidationGroup="SaveLocation"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div runat="server" id="divRegion" class="divRegion" style="display: none; float: left; text-align:center; padding-left:10px;">
                                <div class="col-md-11 col-sm-11 col-xs-11 noPadding">
                                     <asp:TextBox runat="server" ID="txtRegion" CssClass="txtRegion"></asp:TextBox>
                                </div>
                                <div class="col-md-1 col-sm-1 col-xs-1 noRightPadding">
                                    <img alt="Cancel Region" width="16" src="/content/images/close-red.png" class="cancelModule" 
                                    onclick="javascript:$('.divRegion').attr('style','display:none');showddlRegion();" />
                                </div>
                                <div class="col-xs-12 col-sm-12 noPadding">
                                     <asp:RequiredFieldValidator ID="rfTxtRegion" ValidateEmptyText="true" Enabled="false" runat="server" ControlToValidate="txtRegion"
                                        ErrorMessage="Please enter Region"  ValidationGroup="SaveLocation"></asp:RequiredFieldValidator>
                                </div>
                            </div>                                
                        </div>
                    </div>
                    <div class="row" id="tr1" runat="server">
                        <div class="ms-formlabel">
                            <h3 class="ms-standardheader budget_fieldLabel">Description</h3>
                        </div>
                        <div class="ms-formbody accomp_inputField">
                            <asp:TextBox ID="txtDescription" TextMode="MultiLine" CssClass="ms-long" runat="server" Rows="6" cols="20" />
                        </div>
                    </div>
                    <div class="row" id="tr11" runat="server">
                        <div class="ms-formlabel">
                            <h3 class="ms-standardheader budget_fieldLabel">Delete</h3>
                        </div>
                        <div class="ms-formbody accomp_inputField crm-checkWrap">
                            <asp:CheckBox ID="chkDeleted" runat="server" Text="(Prevent use for new item)" />
                        </div>
                    </div>
                    <div class="row addEditPopup-btnWrap">
                        <div class="col-md-12 col-sm-12 col-xs-12">
                             <dx:ASPxButton ID="btnCancel" runat="server" CssClass="secondary-cancelBtn" Text="Cancel" OnClick="btnCancel_Click"></dx:ASPxButton>
                             <dx:ASPxButton ID="btnSave" runat="server" Text="Save" CssClass="primary-blueBtn" ToolTip="Save" ValidationGroup="SaveLocation" OnClick="btnSaveLocation_Click">
                                <ClientSideEvents Click="function(s,e){callBeforeSave();}" />
                             </dx:ASPxButton>
                        </div>
                    </div>
                </div>
            </div>
        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>
<dx:ASPxSpreadsheet ID="SpreadSheetDev" runat="server" Visible="false"></dx:ASPxSpreadsheet>

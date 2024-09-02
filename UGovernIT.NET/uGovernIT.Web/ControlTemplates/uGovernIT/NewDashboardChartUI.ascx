<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NewDashboardChartUI.ascx.cs" Inherits="uGovernIT.Web.NewDashboardChartUI" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
    .containermain {
        float: left;
        width: 100%;
        /*background: #E1E1E1;*/
    }

    .containerblockmain {
        float: left;
        width: 99%;
        border: 1px solid #000;
        padding: 8px;
    }

    .moduleinfo-m {
        float: left;
        width: 100%;
        padding-top: 9px;
        padding-bottom: 15px;
    }

    .generalinfo-table {
        border-collapse: collapse;
        width: 100%;
    }

    .basicinfo-td {
        width: 60%;
    }

        .basicinfo-td fieldset {
            float: left;
            width: 97%;
            height: 200px;
        }

    .dashboardgroup-td {
        width: 40%;
    }

        .dashboardgroup-td fieldset {
            float: left;
            width: 95%;
            height: 200px;
        }

    .viewdashboard-td {
        width: 60%;
    }

        .viewdashboard-td fieldset {
            float: left;
            width: 97%;
        }

    .showinsidebar-td {
        width: 40%;
    }

        .showinsidebar-td fieldset {
            float: left;
            width: 95%;
            height: 200px;
        }

    .widthtxtbox {
        width: 95%;
    }

    .viewdashboard-left {
        float: left;
        width: 48%;
    }

    .viewdashboard-right {
        float: left;
        width: 48%;
    }

    .vd-widthtxtbox {
        width: 150px;
    }

    .showsidebar-div {
        float: left;
        width: 65%;
        height: 150px;
        overflow: scroll;
    }

    .showsidebar-checklist {
        width: 70%;
    }

    .dashboardgroup-div {
        float: left;
        width: 65%;
        height: 150px;
        overflow: scroll;
    }

    .dashboardgroup-checklist {
        width: 70%;
    }

    .dashboardgroup-right {
        float: left;
        width: 30%;
        text-align: center;
    }

        .dashboardgroup-right .selectall {
            float: left;
            width: 100%;
            padding-top: 60px;
        }

            .dashboardgroup-right .selectall input {
                width: 90px;
            }

        .dashboardgroup-right .clearall {
            float: left;
            width: 100%;
        }

            .dashboardgroup-right .clearall input {
                width: 90px;
            }

        .dashboardgroup-right .edit {
            float: left;
        }

            .dashboardgroup-right .edit img {
                cursor: pointer;
            }

    .showsidebar-right {
        float: left;
        width: 30%;
        text-align: center;
    }

        .showsidebar-right .selectall {
            float: left;
            width: 100%;
            padding-top: 60px;
        }

            .showsidebar-right .selectall input {
                width: 90px;
            }

        .showsidebar-right .clearall {
            float: left;
            width: 100%;
        }

            .showsidebar-right .clearall input {
                width: 90px;
            }

    .label {
        font-weight: bold;
    }

    .dimension-table {
        border-collapse: collapse;
        width: 100%;
    }

        .dimension-table .edit-td {
            width: 49%;
        }

        .dimension-table .list-td {
            width: 49%;
            padding-left: 10px;
        }

        .dimension-table fieldset {
            float: left;
            width: 98%;
        }

            .dimension-table fieldset fieldset {
                float: left;
                width: 95%;
            }

        .dimension-table .contentdiv {
            float: inherit;
            width: 92%;
            padding-top: 2px;
        }

        .dimension-table .content-head {
            float: inherit;
            font-weight: bold;
        }

        .dimension-table .content-edit {
            float: inherit;
        }

        .dimension-table .actiondiv {
            float: inherit;
        }

        .dimension-table .content-action {
            float: inherit;
        }

        .dimension-table .inputelement {
            width: 200px;
        }

        .dimension-table .operatorinput {
            width: 80px;
        }

    .dimension-list {
        float: left;
        width: 100%;
    }

        .dimension-list .dimension-content {
            float: left;
            width: inherit;
        }

        .dimension-list .dimension-head {
            float: left;
            width: 100%;
        }

        .dimension-list .dimension-detail {
            float: left;
            width: 100%;
        }

        .dimension-list .dimension-action {
            float: right;
        }

            .dimension-list .dimension-action span {
                float: right;
            }

    .expression-table {
        border-collapse: collapse;
        width: 100%;
    }

        .expression-table .edit-td {
            width: 49%;
        }

        .expression-table .list-td {
            width: 49%;
            padding-left: 10px;
        }

        .expression-table fieldset {
            float: left;
            width: 98%;
        }

            .expression-table fieldset fieldset {
                float: left;
                width: 95%;
            }

        .expression-table .contentdiv {
            float: inherit;
            width: 92%;
            padding-top: 2px;
        }

        .expression-table .content-head {
            float: inherit;
            font-weight: bold;
        }

        .expression-table .content-edit {
            float: inherit;
        }

        .expression-table .actiondiv {
            float: inherit;
        }

        .expression-table .content-action {
            float: inherit;
        }

        .expression-table .inputelement {
            width: 200px;
        }

        .expression-table .operatorinput {
            width: 80px;
        }

    .expression-list {
        float: left;
        width: 100%;
    }

        .expression-list .expression-content {
            float: left;
            width: inherit;
        }

        .expression-list .expression-head {
            float: left;
            width: 100%;
        }

        .expression-list .expression-detail {
            float: left;
            width: 100%;
        }

        .expression-list .expression-action {
            float: right;
        }

            .expression-list .expression-action span {
                float: right;
            }

    .hide {
        display: none;
    }

    .cachechart-container {
    }

        .cachechart-container .contentdiv {
            float: inherit;
            width: 92%;
            padding-top: 2px;
        }

        .cachechart-container .content-head {
            float: inherit;
            font-weight: bold;
        }

        .cachechart-container .content-edit {
            float: inherit;
        }

        .cachechart-container .actiondiv {
            float: inherit;
            padding-top: 5px;
        }

        .cachechart-container .content-action {
            float: inherit;
        }


    /*new classes:: start*/
    span.col1, span.col2 {
        width: 130px;
        float: left;
    }

        span.col1 > b, span.col2 > b {
            margin-top: 2px;
            margin-right: 3px;
            float: left;
        }

    .span-label {
        width: 150px;
        float: left;
        font-weight: bold;
    }

    .container {
        float: left;
        min-height: 405px;
        border: solid 1px #808080;
        width: 100%;
    }

    .cssedit {
        background-color: transparent;
        background-repeat: no-repeat;
        border: medium none;
        background-position: right center;
        text-align: left;
        cursor: pointer;
    }

    .makemeweinvisible {
        display: none;
    }

    .query-formatter-div {
        width: 100%;
        height: 370px;
        overflow: auto;
        float: left;
    }

        .query-formatter-div select {
            padding: 2px;
        }

        .query-formatter-div span > input[type='text'] {
            padding: 3px 5px !important;
        }

    .ms-formlabel {
        border-top: 1px solid #A5A5A5;
        color: #000000;
        padding-bottom: 6px;
        padding-right: 8px;
        padding-top: 3px;
        width: 150px;
        text-align: right;
    }

    .ms-formbody {
        background: none repeat scroll 0 0 #E8EDED;
        border-top: 1px solid #A5A5A5;
        padding: 3px 6px 4px;
        vertical-align: top;
    }

    .align {
    }

    .leftlabel {
        padding-right: 3px;
    }

    .rightlabel {
        padding-left: 10px;
        padding-right: 3px;
    }

    .item-saperation {
        padding-bottom: 5px;
    }

    .button-main {
        float: left;
        width: 99%;
        margin: 4px;
    }
    /*new classes:: end*/
    .add-buttom-main {
        padding: 2px 4px;
        margin-left: 5px;
        border: 1px solid #808080;
        cursor: pointer;
    }

    .Alignbutton {
        margin-top: -5px !important;
        /*background: transparent none repeat scroll 0% 0% !important;
        border: medium none !important;
        color: white !important;*/
    }

    .executeClick {
        display: none;
    }

    .triggerSave {
        display: none;
    }

    .savechanges {
        color: white !important;
        vertical-align: middle !important;
        margin-top: -3px !important;
        margin-left: -11px !important;
    }

    .padding-left5 {
        padding-left: 5px;
    }

    .padding-bottom5 {
        padding-bottom: 5px;
    }

    .relative-top3 {
        position: relative;
        top: 3px;
    }
</style>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_initializeRequest(InitializeRequest);
    prm.add_beginRequest(BeginRequestHandler);
    prm.add_endRequest(EndRequest);
    var notifyId;

    function InitializeRequest(sender, args) {
        var prm = Sys.WebForms.PageRequestManager.getInstance();
    }

    var notifyId = "";
    function AddNotification(msg) {
        if (notifyId == "") {
            notifyId = SP.UI.Notify.addNotification(msg, true);
        }
    }
    function RemoveNotification() {
        SP.UI.Notify.removeNotification(notifyId);
        notifyId = '';
    }
    function BeginRequestHandler(sender, args) {
        AddNotification("Processing ..");
    }

    function EndRequest(sender, args) {
        var s = sender;
        var a = args;
        var msg = null;
        if (a._error != null) {
            switch (args._error.name) {
                case "Sys.WebForms.PageRequestManagerServerErrorException":
                    msg = "PageRequestManagerServerErrorException";
                    break;
                case "Sys.WebForms.PageRequestManagerParserErrorException":
                    msg = "PageRequestManagerParserErrorException";
                    break;
                case "Sys.WebForms.PageRequestManagerTimeoutException":
                    msg = "PageRequestManagerTimeoutException";
                    break;
            }
            args._error.message = "My Custom Error Message " + msg;
            args.set_errorHandled(true);

        }
        else {
            RemoveNotification();
        }
    }

</script>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">

    function selectAllCheckBoxes(checkBoxlistClass) {
        $("." + checkBoxlistClass + " input:checkbox").attr("checked", "checked");
    }

    function clearAllCheckBoxes(checkBoxlistClass) {
        $("." + checkBoxlistClass + " input:checkbox").removeAttr("checked");
    }

    function btnPickExpressionFields(obj) {

        var fields = $(".ddlExpressionFormulaFields");
        if (fields.find("option").length > 0) {
            var expressionFormula = $(".cssExpression");
            expressionFormula.val($.trim(expressionFormula.val()) + fields.val());
        }
    }






    function actionOnTabClick(tabNumber) {

        var tabName = $("#<%=hTabName.ClientID%>").val();
        var activeTab = tabMenu.GetTab(tabNumber);

        var previousTab = tabMenu.GetTabByName(tabName);

        $("#<%=hTabName.ClientID%>").val(activeTab.name);
        if (previousTab != null)
            $("#<%=hPreviousTab.ClientID%>").val(previousTab.name);

        loadingPanel.Show();
        $(".next-buttonclick").trigger("click");
    }

    function ddlExpressionOperator_change(obj) {
        if (obj.value == "variance") {
            $('.cssExpression').val("");
            $(".genericexppanel").hide();
            $(".varianceexppanel").show();

            var var1 = $(".varianceexppanel .var1");
            var var2 = $(".varianceexppanel .var2");
            var var3 = $(".varianceexppanel .var3");
            var var1SelectedItem = var1.find("option[value='" + var1.val() + "']");
            var var2SelectedItem = var2.find("option[value='" + var2.val() + "']");
            var var3SelectedValue = var3.val();
            var var3Items = new Array();
            var3Items.push("<option value='" + var1SelectedItem.attr("value") + "'>" + var1SelectedItem.text() + "</option>");
            var3Items.push("<option value='" + var2SelectedItem.attr("value") + "'>" + var2SelectedItem.text() + "</option>");
            var3.html(var3Items.join(""));
        }
        else {
            $(".varianceexppanel").hide();
            $(".genericexppanel").show();
        }
    }

    function ddlVarianceVar1_change(obj) {
        var var1 = $(".varianceexppanel .var1");
        var var2 = $(".varianceexppanel .var2");
        var var3 = $(".varianceexppanel .var3");

        var var1SelectedItem = var1.find("option[value='" + var1.val() + "']");
        var var2SelectedItem = var2.find("option[value='" + var2.val() + "']");
        var var3SelectedValue = var3.val();

        var var3Items = new Array();
        var3Items.push("<option value='" + var1SelectedItem.attr("value") + "'>" + var1SelectedItem.text() + "</option>");
        if (var1SelectedItem.attr("value") != var2SelectedItem.attr("value")) {
            var3Items.push("<option value='" + var2SelectedItem.attr("value") + "'>" + var2SelectedItem.text() + "</option>");
        }
        var3.html(var3Items.join(""));
        if (var1SelectedItem.attr("value") == var3SelectedValue || var2SelectedItem.attr("value") == var3SelectedValue) {
            var3.val(var3SelectedValue);
        }
    }
    function ddlVarianceVar2_change(obj) {
        var var1 = $(".varianceexppanel .var1");
        var var2 = $(".varianceexppanel .var2");
        var var3 = $(".varianceexppanel .var3");

        var var1SelectedItem = var1.find("option[value='" + var1.val() + "']");
        var var2SelectedItem = var2.find("option[value='" + var2.val() + "']");
        var var3SelectedValue = var3.val();

        var var3Items = new Array();
        var3Items.push("<option value='" + var1SelectedItem.attr("value") + "'>" + var1SelectedItem.text() + "</option>");
        if (var1SelectedItem.attr("value") != var2SelectedItem.attr("value")) {
            var3Items.push("<option value='" + var2SelectedItem.attr("value") + "'>" + var2SelectedItem.text() + "</option>");
        }
        var3.html(var3Items.join(""));
        if (var1SelectedItem.attr("value") == var3SelectedValue || var2SelectedItem.attr("value") == var3SelectedValue) {
            var3.val(var3SelectedValue);
        }
    }

    function showFormatSection(s, e) {
        $(".chart-formatmain > div").hide();
        $("." + e.item.name).show();

    }

    $(document).ready(function () {
        try {
            var item = nbpreView.GetSelectedItem();
            $(".chart-formatmain > div").hide();
            $("." + item.name).show();
        } catch (ex) { }

    });

    function hideLevelPropertyTable() {

    }


    function showHideNPointDetail(s, e) {
        var panelObj = $("#" + s.GetMainElement().id).parents(".ms-formbody:eq(0)").find(".chlbxNPointsPanel");
        if (s.GetChecked()) {
            panelObj.removeClass("hide");
        }
        else {
            panelObj.addClass("hide");
        }
    }
    function showHideOrderByDetail(s, e) {
        var panelObj = $("#" + s.GetMainElement().id).parents(".ms-formbody:eq(0)").find(".axisOrderPanel");
        if (s.GetChecked()) {
            panelObj.removeClass("hide");
        }
        else {
            panelObj.addClass("hide");
        }
    }
</script>
<dx:ASPxLoadingPanel ID="loadingPanel" ClientInstanceName="loadingPanel" Modal="True" runat="server" Text="Please Wait..."></dx:ASPxLoadingPanel>
<asp:HiddenField ID="hTabName" runat="server" Value="general" />
<asp:HiddenField ID="hPreviousTab" runat="server" Value="general" />

<asp:HiddenField ID="isJSNextBtnHit" runat="server" Value="" />

<dx:ASPxTabControl ID="tabMenu" ClientInstanceName="tabMenu" Theme="MetropolisBlue" runat="server">
    <Tabs>
        <dx:Tab Text="General" Name="general" />
        <dx:Tab Text="Data Source" Name="datasource" />
        <dx:Tab Text="X-Axis" Name="dimensions" />
        <dx:Tab Text="Expressions" Name="expressions" />
        <dx:Tab Text="Load Frequency" Name="loadfrequency" />
        <dx:Tab Text="Chart Format" Name="preview" />
    </Tabs>
    <ClientSideEvents ActiveTabChanged="function(s, e){actionOnTabClick(s.activeTabIndex);}" />
    <TabStyle Paddings-PaddingLeft="13px" Paddings-PaddingRight="13px"></TabStyle>
</dx:ASPxTabControl>

<asp:HiddenField ID="hDashboardID" runat="server" />
<div class="containerblockmain">
    <div runat="server" visible="false" class="containermain general" id="tabPanelGeneral">
        <asp:Panel ID="tabPanelconainer1" runat="server">
            <table class="main-table">
                <tr>
                    <td class="col1 item-saperation">
                        <div>
                            <div class="span-label">Dashboard Name<b style="color: Red">*</b>:</div>
                            <div class="fleft">
                                <asp:TextBox MaxLength="200" Width="300" Height="16" ValidationGroup="generalInfo"
                                    ID="txtTitle" runat="server"></asp:TextBox>

                                <div>
                                    <asp:CustomValidator ID="cv_Dashboardname" ValidationGroup="generalInfo" OnServerValidate="cv_Dashboardname_ServerValidate" runat="server" ControlToValidate="txtTitle" CssClass="error"
                                        Display="Dynamic" ValidateEmptyText="true" ErrorMessage="Dashboard with this name already exists"></asp:CustomValidator>
                                </div>
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="col1 item-saperation">
                        <div>
                            <div class="span-label">Description :</div>
                            <div class="fleft">
                                <asp:TextBox MaxLength="200" Width="300" Height="100" Columns="6" Rows="6" TextMode="MultiLine" ID="txtDescription"
                                    runat="server"></asp:TextBox>

                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="col1 item-saperation">
                        <div>
                            <span class="span-label">Order:</span>
                        </div>
                        <div>
                            <dx:ASPxComboBox ID="cmbOrder" runat="server" DropDownStyle="DropDownList"></dx:ASPxComboBox>
                        </div>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>


    <div runat="server" visible="false" class="containermain datasource" id="tabPanelDatasource">
        <table class="main-table">

            <tr>
                <td class="col1 item-saperation">
                    <div>
                        <div class="span-label">Data Source <b style="color: Red">*</b>:</div>
                        <div class="fleft">

                            <asp:DropDownList Width="200" AutoPostBack="true" ID="ddlFactTable" runat="server" OnLoad="DdlFactTable_Load" OnSelectedIndexChanged="DdlFactTable_SelectedIndexChanged">
                            </asp:DropDownList>
                        </div>
                    </div>
                </td>

            </tr>
            <tr>
                <td class="col1 item-saperation">
                    <div>
                        <div class="span-label">Select Filter: :</div>
                        <div class="fleft">
                            <dx:ASPxMemo ID="txtBasicFilterFormula" Width="400" Height="100" ClientInstanceName="txtBasicFilterFormula" CssClass="txtBasicFilterFormula" runat="server"></dx:ASPxMemo>

                        </div>
                        <div class="fleft add-buttom-main" onclick="createBasicFilterFormula(this, 'txtBasicFilterFormula')">
                            <img src="/content/images/add_icon.png" style="cursor: pointer;" alt="Add" />
                        </div>
                    </div>
                </td>
            </tr>

        </table>
    </div>


    <div runat="server" visible="false" class="containermain dimensions" id="tabPanelDimensions">
        <div class="fullwidth">
            <dx:ASPxCardView ID="xAxisCardView" OnCardInserting="xAxisCardView_CardInserting" OnCancelCardEditing="xAxisCardView_CancelCardEditing" Border-BorderWidth="0px" EnableTheming="true" OnCardDeleting="xAxisCardView_CardDeleting" OnCardUpdating="xAxisCardView_CardUpdating" Theme="DevEx" SettingsPager-ShowSeparators="false" SettingsPager-ShowEmptyCards="false" Settings-ShowCustomizationPanel="false"
                KeyFieldName="Title" AutoGenerateColumns="False" runat="server" Width="100%" ClientInstanceName="xAxisCardView">
                <SettingsEditing Mode="EditForm" NewCardPosition="Bottom" />
                <SettingsPager Mode="ShowAllRecords">
                    <SettingsTableLayout ColumnCount="2" />
                </SettingsPager>
                <SettingsCommandButton>
                    <EditButton ButtonType="Image">
                        <Image Url="/content/images/edit-icon.png"></Image>
                    </EditButton>
                    <DeleteButton ButtonType="Image">
                        <Image Url="/content/images/delete-icon-new.png"></Image>
                    </DeleteButton>
                </SettingsCommandButton>
                <SettingsText ConfirmDelete="Do you want to delete this express?" />
                <Columns>
                    <dx:CardViewTextColumn FieldName="Title"></dx:CardViewTextColumn>
                    <dx:CardViewComboBoxColumn FieldName="SelectedField" Caption="Group By">
                    </dx:CardViewComboBoxColumn>
                    <dx:CardViewSpinEditColumn FieldName="Sequence"></dx:CardViewSpinEditColumn>
                    <%--<dx:CardViewTextColumn FieldName="Sequence" />--%>
                    <dx:CardViewComboBoxColumn FieldName="DataPointClickEvent">
                        <PropertiesComboBox>
                            <Items>
                                <dx:ListEditItem Text="None" Value="None" />
                                <dx:ListEditItem Text="Next Dimension" Value="NextDimension" />
                                <dx:ListEditItem Text="Detail" Value="Detail" />
                            </Items>
                        </PropertiesComboBox>
                    </dx:CardViewComboBoxColumn>
                    <dx:CardViewTextColumn FieldName="Sequence"></dx:CardViewTextColumn>
                </Columns>
                <CardLayoutProperties ColCount="1">
                    <Items>
                        <dx:CardViewCommandLayoutItem ShowEditButton="true" ShowDeleteButton="true" HorizontalAlign="Right" />
                        <dx:CardViewColumnLayoutItem ColumnName="Title" Caption="Axis Name:" Width="100%" HorizontalAlign="Left" />
                        <dx:CardViewColumnLayoutItem ColumnName="SelectedField" Caption="Dimensions"></dx:CardViewColumnLayoutItem>
                        <dx:CardViewColumnLayoutItem ColumnName="Sequence" Width="200px" />
                        <dx:CardViewColumnLayoutItem ColumnName="DataPointClickEvent" Width="200px" Caption="Action:" />
                        <dx:EditModeCommandLayoutItem HorizontalAlign="Right" />
                    </Items>
                </CardLayoutProperties>
                <EditFormLayoutProperties>
                    <Items>
                    </Items>
                </EditFormLayoutProperties>
            </dx:ASPxCardView>
        </div>
    </div>

    <div runat="server" visible="false" class="containermain expressions" id="tabPanelExpressions">
        <dx:ASPxCardView ID="ExpCardView" ClientInstanceName="ExpCardView" Width="100%" Border-BorderWidth="0px" Theme="DevEx" OnCardInserting="ExpCardView_CardInserting" OnCardDeleting="ExpCardView_CardDeleting" OnCardUpdating="ExpCardView_CardUpdating"
            SettingsPager-ShowSeparators="false" OnCancelCardEditing="ExpCardView_CancelCardEditing"
            SettingsPager-ShowEmptyCards="false" Settings-ShowCustomizationPanel="false" KeyFieldName="Title"
            AutoGenerateColumns="False" runat="server">

            <SettingsEditing Mode="EditForm" BatchEditSettings-EditMode="Card" NewCardPosition="Bottom" />

            <SettingsPager Mode="ShowAllRecords">
                <SettingsTableLayout ColumnCount="2" />
            </SettingsPager>
            <SettingsCommandButton>
                <EditButton ButtonType="Image">
                    <Image Url="/content/images/edit-icon.png"></Image>
                </EditButton>
                <DeleteButton ButtonType="Image">
                    <Image Url="/content/images/delete-icon-new.png"></Image>
                </DeleteButton>
            </SettingsCommandButton>
            <SettingsText ConfirmDelete="Do you want to delete this express?" />

            <Columns>
                <dx:CardViewTextColumn FieldName="Title"></dx:CardViewTextColumn>
                <dx:CardViewComboBoxColumn FieldName="GroupByField" PropertiesComboBox-ValueField="FieldDisplayName" PropertiesComboBox-TextField="FieldName" Caption="Group By" PropertiesComboBox-AllowNull="true">
                </dx:CardViewComboBoxColumn>
                <dx:CardViewComboBoxColumn FieldName="ChartType">
                    <PropertiesComboBox ShowImageInEditBox="true">
                        <Items>
                            <dx:ListEditItem ImageUrl="/Content/images/uGovernIT/Bar.PNG" Text="Bar" Value="Bar" />
                            <dx:ListEditItem ImageUrl="/Content/images/uGovernIT/Bar.PNG" Text="Column" Value="Column" />
                            <dx:ListEditItem ImageUrl="/Content/images/uGovernIT/SideBySideFullStackedBar.png" Text="Stacked Column" Value="StackedColumn" />
                            <dx:ListEditItem ImageUrl="/Content/images/uGovernIT/Doughnut.PNG" Text="Doughnut" Value="Doughnut" />
                            <dx:ListEditItem ImageUrl="/Content/images/uGovernIT/Funnel.PNG" Text="Funnel" Value="Funnel" />
                            <dx:ListEditItem ImageUrl="/Content/images/uGovernIT/Line.PNG" Text="Line" Value="Line" />
                            <dx:ListEditItem ImageUrl="/Content/images/uGovernIT/Pie.PNG" Text="Pie" Value="Pie" />
                            <dx:ListEditItem ImageUrl="/Content/images/uGovernIT/BarStacked.PNG" Text="Stacked Bar" Value="StackedBar" />
                            <dx:ListEditItem ImageUrl="/Content/images/uGovernIT/SPLine.PNG" Text="Spline" Value="Spline" />
                            <dx:ListEditItem ImageUrl="/Content/images/uGovernIT/StepLine.PNG" Text="StepLine" Value="StepLine" />
                        </Items>
                    </PropertiesComboBox>
                </dx:CardViewComboBoxColumn>

                <dx:CardViewColumn Name="FunctionExpression" FieldName="FunctionExpression">
                    <EditItemTemplate>
                        <asp:Panel Style="width: 100%" ID="panelFunction" runat="server" OnInit="panelFunction_Init" OnLoad="panelFunction_Load">
                            <fieldset>
                                <legend>Function:</legend>
                                <div class="contentdiv">
                                    <span class="content-head">Pick field:</span>
                                </div>
                                <div class="contentdiv">
                                    <span class="content-Edit">

                                        <asp:DropDownList EnableViewState="true" OnLoad="ddlExpressionFormulaFields_Load" CssClass="inputelement ddlExpressionFormulaFields" ID="ddlExpressionFormulaFields"
                                            runat="server">
                                        </asp:DropDownList>
                                        <input type="button" value="Pick" id="btnPickExpressionField" onclick="btnPickExpressionFields(this)" />
                                    </span>
                                </div>
                                <div class="contentdiv" id="ddlExpressionFormulaFieldsFunction" style="color: Gray">
                                    You can use following function: Daysdiff(datetimefield1,datetimefield2), Yeardiff(datetimefield1,datetimefield2)
                                </div>
                                <div class="contentdiv">
                                    <span class="content-Edit ddlexpressionoperator-span" style="float: left; padding-right: 1px;">
                                        <asp:DropDownList CssClass="operatorinput" ID="ddlExpressionOperator" runat="server" onchange="ddlExpressionOperator_change(this)">
                                            <asp:ListItem Text="None" Value=""></asp:ListItem>
                                            <asp:ListItem Text="Count" Value="count"></asp:ListItem>
                                            <asp:ListItem Text="Sum" Value="sum"></asp:ListItem>
                                            <asp:ListItem Text="Average" Value="average"></asp:ListItem>
                                            <asp:ListItem Text="Maximum" Value="maximum"></asp:ListItem>
                                            <asp:ListItem Text="Minimum" Value="minimum"></asp:ListItem>
                                            <asp:ListItem Text="Variance" Value="variance"></asp:ListItem>
                                        </asp:DropDownList>
                                    </span><span style="float: left; padding-right: 1px;">Of</span> <span class="content-Edit" style="float: left; padding-right: 1px;">
                                        <asp:Panel ID="varianceExpPanel" runat="server" CssClass="varianceexppanel fleft" Style="display: none; width: 350px;">
                                            <span class="fleft">
                                                <span class="fleft" style="font-weight: bold;">(</span>
                                                <span class="fleft">
                                                    <asp:DropDownList ID="ddlVarianceVar1" CssClass="var1" runat="server" Width="100px" ValidationGroup="editExpression" OnInit="ddlVarianceVar1_Load" onchange="ddlVarianceVar1_change(this);"></asp:DropDownList>
                                                </span><span class="fleft">-</span><span class="fleft">
                                                    <asp:DropDownList ID="ddlVarianceVar2" CssClass="var2" runat="server" Width="100px" OnInit="ddlVarianceVar2_Load" onchange="ddlVarianceVar2_change(this);"></asp:DropDownList>
                                                </span>
                                                <span class="fleft" style="font-weight: bold;">)/(</span>
                                                <span class="fleft">
                                                    <select id="ddlVarianceVar3" runat="server" class="var3" style="width: 100px;"></select>
                                                </span>
                                                <span class="fleft" style="font-weight: bold;">)</span>

                                            </span>

                                        </asp:Panel>
                                        <asp:Panel ID="genericExpPanel" runat="server" CssClass="genericexppanel">
                                            <asp:TextBox TextMode="MultiLine" ID="txtExpression" CssClass="cssExpression" ValidationGroup="editExpression" runat="server" Width="350"></asp:TextBox>
                                        </asp:Panel>
                                    </span><span style="float: left; padding-right: 1px; display: none;">
                                        <asp:CheckBox ID="cbCalculatePercentage" runat="server" Text="%" TextAlign="Right" />
                                    </span>
                                </div>
                                <div class="contentdiv">
                                    <asp:CustomValidator ID="cbVarianceExp" Display="Dynamic" runat="server" ValidationGroup="editExpression" ControlToValidate="ddlVarianceVar1" ErrorMessage="Please select variance paraments."></asp:CustomValidator>
                                </div>
                            </fieldset>
                        </asp:Panel>
                    </EditItemTemplate>
                </dx:CardViewColumn>
                <dx:CardViewColumn Name="Expression" FieldName="ExpressionFormula">
                    <EditItemTemplate>
                        <asp:Panel ID="panelExpressionFormula" runat="server" OnInit="panelExpressionFormula_Init">
                            <div style="float: left">
                                <dx:ASPxMemo ID="txtExpressionFormulas" OnLoad="txtExpressionFormulas_Load" CssClass="txtExpressionFormulas" ClientInstanceName="txtExpressionFormulas" runat="server" Width="300"></dx:ASPxMemo>
                            </div>
                            <div class="fleft add-buttom-main" onclick="createBasicFilterFormula(this, 'txtExpressionFormulas')">
                                <img src="/Content/images/add_icon.png" style="cursor: pointer" alt="Add" />
                            </div>
                        </asp:Panel>
                    </EditItemTemplate>
                </dx:CardViewColumn>
                <dx:CardViewTextColumn FieldName="Order"></dx:CardViewTextColumn>
            </Columns>
            <CardLayoutProperties ColCount="1">
                <Items>
                    <dx:CardViewCommandLayoutItem ShowEditButton="true" ShowDeleteButton="true" ColSpan="1" HorizontalAlign="Right" />
                    <dx:CardViewColumnLayoutItem ColumnName="Title" Caption="Expression Name:" Width="300px" ColSpan="1" HorizontalAlign="Left" />
                    <dx:CardViewColumnLayoutItem ColumnName="GroupByField" ColSpan="1"></dx:CardViewColumnLayoutItem>
                    <dx:CardViewColumnLayoutItem ColumnName="ChartType" Caption="Chart Type" Width="300px" ColSpan="1" />
                    <dx:CardViewColumnLayoutItem ColumnName="FunctionExpression" Caption="Function" ColSpan="1">
                    </dx:CardViewColumnLayoutItem>
                    <dx:CardViewColumnLayoutItem ColumnName="ExpressionFormula" Caption="Formula filter" ColSpan="1"></dx:CardViewColumnLayoutItem>
                    <dx:CardViewColumnLayoutItem ColumnName="Order" Caption="Order" ColSpan="1" Width="300px" />
                    <dx:EditModeCommandLayoutItem HorizontalAlign="Right" ColSpan="1" />
                </Items>
            </CardLayoutProperties>
            <EditFormLayoutProperties>
                <Items>
                </Items>
            </EditFormLayoutProperties>
        </dx:ASPxCardView>
    </div>



    <div runat="server" visible="false" class="containermain loadfrequency" id="tabPanelLoadfrequency">
        <asp:Panel ID="tabPanelContainer4" runat="server">
            <table width="100%" cellpadding="0" cellspacing="0" class="cachechart-container">
                <tr>
                    <td>
                        <fieldset>
                            <legend>Edit</legend>
                            <div>
                                <div class="contentdiv">
                                    <span class="content-Edit">
                                        <asp:CheckBox ID="cbCacheChart" runat="server" Text="Cache Chart" />
                                    </span>
                                </div>
                                <div class="contentdiv">
                                    <span class="content-head">Schedule:</span>
                                </div>
                                <div class="contentdiv">
                                    <span class="content-Edit">
                                        <asp:TextBox ID="txtCacheChartSchedule" runat="server"></asp:TextBox>
                                        min.</span>
                                </div>
                            </div>
                        </fieldset>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>

    <div runat="server" visible="false" class="containermain loadfrequency" id="tabPanelPreview">

        <%--new UI start :: Preview tab--%>
        <div style="float: left; width: 99%;">
            <table style="float: left; width: 99%;">
                <tr>
                    <td style="text-align: initial; padding: 5px; width: 50px" valign="top">
                        <dx:ASPxNavBar ID="nbpreView" ClientInstanceName="nbpreView" runat="server" AllowSelectItem="True" EnableAnimation="True" Theme="Default" Width="100%">
                            <GroupHeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                            <ClientSideEvents ItemClick="function(s,e){showFormatSection(s,e);}" />
                        </dx:ASPxNavBar>
                    </td>

                    <td valign="top">
                        <div class="chart-formatmain">
                            <div class="chart-basic query-formatter-div" style="display: none;">

                                <table class="ms-formtable" cellpadding="0" cellspacing="0" style="border-collapse: collapse" width="100%">
                                    <tr id="Tr3">
                                        <td class="ms-formlabel">Hide Title
                                        </td>
                                        <td class="ms-formbody">
                                            <asp:CheckBox ID="chkHideTitle" runat="server" />
                                        </td>
                                    </tr>
                                    <tr id="qfquerytype">
                                        <td class="ms-formlabel">Hide Actions
                                        </td>
                                        <td class="ms-formbody">
                                            <asp:CheckBox ID="zoomchk" Text="Zoom" runat="server" />
                                            <asp:CheckBox ID="downloadchk" Text="CSV Download" runat="server" />
                                            <asp:CheckBox ID="tablechk" Text="Table" runat="server" />
                                        </td>
                                    </tr>
                                    <tr id="qfnumfontstyle">
                                        <td class="ms-formlabel">Size
                                        </td>
                                        <td class="ms-formbody">
                                            <span class="span-inline">Height:
                                                <asp:TextBox ID="chartHeight" Width="50" runat="server" type="number" Text="0" />px
                                            </span>
                                            <span class="span-inline">Width:
                                                <asp:TextBox ID="chartWidth" Width="50" runat="server" type="number" Text="0" />px
                                            </span>
                                        </td>
                                    </tr>

                                    <tr id="qftitle">
                                        <td class="ms-formlabel">Hide Grid
                                        </td>
                                        <td class="ms-formbody">
                                            <asp:CheckBox ID="hideGrid" Text="" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="ms-formlabel">Background Color
                                        </td>
                                        <td class="ms-formbody">
                                            <dx:ASPxColorEdit EnableCustomColors="true" ID="aspxBGColor" runat="server"></dx:ASPxColorEdit>
                                        </td>
                                    </tr>
                                    <tr id="qflabel">
                                        <td class="ms-formlabel">Pallete
                                        </td>
                                        <td class="ms-formbody">
                                            <span style="float: left">
                                                <dx:ASPxComboBox Theme="DevEx" Width="114" ID="cmbxPallete" runat="server">
                                                    <Items>
                                                        <dx:ListEditItem Text="Apex" Value="Apex" Selected="true" />
                                                        <dx:ListEditItem Text="Aspect" Value="Aspect" />
                                                        <dx:ListEditItem Text="Black and White" Value="Black and White" />
                                                        <dx:ListEditItem Text="Blue" Value="Blue" />
                                                        <dx:ListEditItem Text="Blue Green" Value="Blue Green" />
                                                        <dx:ListEditItem Text="Blue Warm" Value="Blue Warm" />
                                                        <dx:ListEditItem Text="Chameleon" Value="Chameleon" />
                                                        <dx:ListEditItem Text="Civic" Value="Civic" />
                                                        <dx:ListEditItem Text="Concourse" Value="Concourse" />
                                                        <dx:ListEditItem Text="Default" Value="Default" />
                                                        <dx:ListEditItem Text="Equity" Value="Equity" />
                                                        <dx:ListEditItem Text="Flow" Value="Flow" />
                                                        <dx:ListEditItem Text="Foundry" Value="Foundry" />
                                                        <dx:ListEditItem Text="Grayscale" Value="Grayscale" />
                                                        <dx:ListEditItem Text="Green" Value="Green" />
                                                        <dx:ListEditItem Text="Green Yellow" Value="Green Yellow" />
                                                        <dx:ListEditItem Text="In A Fog" Value="In A Fog" />
                                                        <dx:ListEditItem Text="Marquee" Value="Marquee" />
                                                        <dx:ListEditItem Text="Median" Value="Median" />
                                                        <dx:ListEditItem Text="Metro" Value="Metro" />
                                                        <dx:ListEditItem Text="Mixed" Value="Mixed" />
                                                        <dx:ListEditItem Text="Module" Value="Module" />
                                                        <dx:ListEditItem Text="Nature Colors" Value="Nature Colors" />
                                                        <dx:ListEditItem Text="Northern Lights" Value="Northern Lights" />
                                                        <dx:ListEditItem Text="Office" Value="Office" />
                                                        <dx:ListEditItem Text="Office 2013" Value="Office 2013" />
                                                        <dx:ListEditItem Text="Opulent" Value="Opulent" />
                                                        <dx:ListEditItem Text="Orange" Value="Orange" />
                                                        <dx:ListEditItem Text="Orange Red" Value="Orange Red" />
                                                        <dx:ListEditItem Text="Oriel" Value="Oriel" />
                                                        <dx:ListEditItem Text="Paper" Value="Paper" />
                                                        <dx:ListEditItem Text="Pastel Kit" Value="Pastel Kit" />
                                                        <dx:ListEditItem Text="Red" Value="Red" />
                                                        <dx:ListEditItem Text="Red Orange" Value="Red Orange" />
                                                        <dx:ListEditItem Text="Slipstream" Value="Slipstream" />
                                                        <dx:ListEditItem Text="Solstice" Value="Solstice" />
                                                        <dx:ListEditItem Text="Technic" Value="Technic" />
                                                        <dx:ListEditItem Text="Terrecotta Pie" Value="Terrecotta Pie" />
                                                        <dx:ListEditItem Text="The Trees" Value="The Trees" />
                                                        <dx:ListEditItem Text="Trek" Value="Trek" />
                                                        <dx:ListEditItem Text="Urban" Value="Urban" />
                                                        <dx:ListEditItem Text="Verve" Value="Verve" />
                                                        <dx:ListEditItem Text="Yellow" Value="Yellow" />
                                                        <dx:ListEditItem Text="Yellow Orange" Value="Yellow Orange" />
                                                    </Items>
                                                </dx:ASPxComboBox>
                                            </span>

                                        </td>
                                    </tr>
                                    <tr id="qfReversePlotting">
                                        <td class="ms-formlabel">Reverse Plotting
                                        </td>
                                        <td class="ms-formbody">
                                            <asp:CheckBox ID="chkbxReversePlotting" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="ms-formlabel">Basic Filters
                                        </td>
                                        <td class="ms-formbody">
                                            <div>
                                                <div class="padding-bottom5">
                                                    <span>Date Range Filter</span>
                                                    <span class="padding-left5">(Keep both value same if you want to compare against same field)</span>
                                                </div>
                                                <div class="fullwidth padding-bottom5">
                                                    <div class="fleft">
                                                        <dx:ASPxComboBox Width="140" ID="ddlBasicDateFilterStartField" runat="server">
                                                        </dx:ASPxComboBox>
                                                    </div>
                                                    <div class="fleft padding-left5">
                                                        <dx:ASPxComboBox Width="140" ID="ddlBasicDateFilterEndField" runat="server">
                                                        </dx:ASPxComboBox>
                                                    </div>
                                                </div>
                                                <div class="padding-bottom5">
                                                    <span>Default View:</span>
                                                </div>
                                                <div>
                                                    <div class="fleft">
                                                        <dx:ASPxComboBox CssClass="vd-widthtxtbox" OnLoad="DdlBasicDateFilterDefaultView_Load"
                                                            EnableViewState="true" ID="ddlBasicDateFilterDefaultView" runat="server">
                                                        </dx:ASPxComboBox>
                                                    </div>
                                                    <div class="fleft padding-left5">
                                                        <asp:CheckBox ID="cbHideDateFilterBox" runat="server" Text="Hide DropDown" TextAlign="Right" />
                                                    </div>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>


                            <div class="chart-Legend query-formatter-div" style="display: none;">
                                <table class="ms-formtable" cellpadding="0" cellspacing="0" style="border-collapse: collapse" width="100%">
                                    <tr id="Tr7">
                                        <td class="ms-formlabel">Legend
                                        </td>
                                        <td class="ms-formbody">
                                            <asp:CheckBox ID="legendChk" Text="Enable Legend" AutoPostBack="true" runat="server" />
                                        </td>
                                    </tr>
                                    <tr id="lagendAlignmentTr" runat="server" visible="false">
                                        <td class="ms-formlabel">Legend Alignment
                                        </td>
                                        <td class="ms-formbody">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <span class="fright leftlabel">Horizontal:</span>
                                                    </td>
                                                    <td>

                                                        <dx:ASPxComboBox Theme="DevEx" Width="105" ID="cmbxHorizontalAlignment" runat="server">
                                                            <Items>
                                                                <dx:ListEditItem Text="Left Out side" Value="LeftOutside" Selected="true" />
                                                                <dx:ListEditItem Text="Left" Value="Left" />
                                                                <dx:ListEditItem Text="Center" Value="Center" />
                                                                <dx:ListEditItem Text="Right" Value="Right" />
                                                                <dx:ListEditItem Text="Right Out side" Value="RightOutside" />
                                                            </Items>
                                                        </dx:ASPxComboBox>
                                                    </td>
                                                    <td><span class="fright rightlabel">Vertical: </span></td>
                                                    <td>

                                                        <dx:ASPxComboBox Theme="DevEx" Width="119" ID="cmbxVerticalAlignment" runat="server">
                                                            <Items>
                                                                <dx:ListEditItem Text="Top Out side" Value="TopOutside" Selected="true" />
                                                                <dx:ListEditItem Text="Top" Value="Top" />
                                                                <dx:ListEditItem Text="Center" Value="Center" />
                                                                <dx:ListEditItem Text="Bottom" Value="Bottom" />
                                                                <dx:ListEditItem Text="Bottom Out side" Value="BottomOutside" />
                                                            </Items>
                                                        </dx:ASPxComboBox>

                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr id="lagendBoxTr" runat="server" visible="false">
                                        <td class="ms-formlabel">Legend Box Size
                                        </td>
                                        <td class="ms-formbody">
                                            <table>
                                                <tr>
                                                    <td><span class="fright leftlabel">Horizontal:</span>
                                                    </td>
                                                    <td>
                                                        <dx:ASPxComboBox Theme="DevEx" Width="58" ID="cmbxMaxHorizontalPercentage" runat="server">
                                                            <Items>
                                                                <dx:ListEditItem Text="25%" Value="25" Selected="true" />
                                                                <dx:ListEditItem Text="50%" Selected="true" Value="50" />
                                                                <dx:ListEditItem Text="75%" Value="75" />
                                                                <dx:ListEditItem Text="100%" Value="100" />
                                                            </Items>
                                                        </dx:ASPxComboBox>

                                                    </td>
                                                    <td><span class="fright rightlabel">Vertical:</span></td>
                                                    <td colspan="3">

                                                        <dx:ASPxComboBox Theme="DevEx" Width="58" ID="cmbxMaxVerticalPercentage" runat="server">
                                                            <Items>
                                                                <dx:ListEditItem Text="25%" Value="25" Selected="true" />
                                                                <dx:ListEditItem Text="50% " Selected="true" Value="50" />
                                                                <dx:ListEditItem Text="75%" Value="75" />
                                                                <dx:ListEditItem Text="100%" Value="100" />
                                                            </Items>
                                                        </dx:ASPxComboBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>

                                    <tr id="lagendDirectionTr" runat="server" visible="false">
                                        <td class="ms-formlabel">Direction
                                        </td>
                                        <td class="ms-formbody">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <dx:ASPxComboBox Theme="DevEx" Width="110" ID="cmbxDirection" runat="server">
                                                            <Items>
                                                                <dx:ListEditItem Text="Top To Bottom" Value="TopToBottom" Selected="true" />
                                                                <dx:ListEditItem Text="Bottom To Top" Value="BottomToTop" />
                                                                <dx:ListEditItem Text="Left To Right" Value="LeftToRight" />
                                                                <dx:ListEditItem Text="Right To Left" Value="RightToLeft" />
                                                            </Items>
                                                        </dx:ASPxComboBox>
                                                    </td>

                                                </tr>
                                            </table>
                                        </td>
                                    </tr>

                                </table>
                            </div>

                            <asp:Repeater ID="xAxisRepeater" runat="server" OnItemDataBound="xAxisRepeater_ItemDataBound">
                                <ItemTemplate>
                                    <div class="xaxis-<%# Container.ItemIndex %> xaxis-main" style="display: none;">
                                        <table class="ms-formtable" cellpadding="0" cellspacing="0" style="border-collapse: collapse" width="100%">
                                            <tr id="qflabel">
                                                <td class="ms-formlabel">X-Axis
                                    <asp:HiddenField ID="hdnXAxisFormat" runat="server" Value='<%# Eval("Title") %>' />
                                                </td>
                                                <td class="ms-formbody"><%# Eval("Title") %></td>
                                            </tr>
                                            <tr id="Tr1">
                                                <td class="ms-formlabel">Angle                                
                                                </td>
                                                <td class="ms-formbody">
                                                    <dx:ASPxComboBox Theme="DevEx" Width="110" ID="cmbxAngle" TextFormatString="{0}" runat="server">
                                                        <Items>
                                                            <dx:ListEditItem Value="0" Selected="true" Text="0 Degree" />
                                                            <dx:ListEditItem Value="30" Text="30 Degree" />
                                                            <dx:ListEditItem Value="45" Text="45 Degree" />
                                                            <dx:ListEditItem Value="60" Text="60 Degree" />
                                                            <dx:ListEditItem Value="75" Text="75 Degree" />
                                                            <dx:ListEditItem Value="90" Text="90 Degree" />
                                                            <dx:ListEditItem Value="-30" Text="-30 Degree" />
                                                            <dx:ListEditItem Value="-45" Text="-45 Degree" />
                                                            <dx:ListEditItem Value="-60" Text="-60 Degree" />
                                                            <dx:ListEditItem Value="-75" Text="-75 Degree" />
                                                        </Items>
                                                    </dx:ASPxComboBox>

                                                </td>
                                            </tr>
                                            <tr id="Tr12">
                                                <td class="ms-formlabel">Label Max Length                               
                                                </td>
                                                <td class="ms-formbody">
                                                    <dx:ASPxSpinEdit ID="aspxAxisLength" runat="server" Width="50" MinValue="0" MaxValue="500">
                                                    </dx:ASPxSpinEdit>
                                                </td>
                                            </tr>
                                             <tr id="Tr19">
                                                <td class="ms-formlabel">Legend Text Max Length                               
                                                </td>
                                                <td class="ms-formbody">
                                                    <dx:ASPxSpinEdit ID="aspxLegendMaxLength" runat="server" Width="50" MinValue="0" MaxValue="500" >
                                                    </dx:ASPxSpinEdit>
                                                </td>
                                            </tr>
                                            <tr id="Tr2">
                                                <td class="ms-formlabel">Show In Dropdown                                
                                                </td>
                                                <td class="ms-formbody">
                                                    <dx:ASPxCheckBox ID="chkbxShowInDropdown" runat="server"></dx:ASPxCheckBox>
                                                </td>
                                            </tr>
                                            <tr id="Tr5">
                                                <td class="ms-formlabel">Cumulative                                
                                                </td>
                                                <td class="ms-formbody">
                                                    <dx:ASPxCheckBox ID="chkCumulative" runat="server"></dx:ASPxCheckBox>
                                                </td>
                                            </tr>
                                            <tr id="trDateView" runat="server">
                                                <td class="ms-formlabel">Date View                                
                                                </td>
                                                <td class="ms-formbody">
                                                    <dx:ASPxRadioButtonList ID="rdblstDateView" runat="server" RepeatDirection="Horizontal">
                                                        <Items>
                                                            <dx:ListEditItem Text="Year" Value="year" />
                                                            <dx:ListEditItem Text="Month" Value="month" />
                                                            <dx:ListEditItem Text="Day" Value="day" />
                                                        </Items>
                                                    </dx:ASPxRadioButtonList>
                                                </td>
                                            </tr>
                                            <tr id="Tr3">
                                                <td class="ms-formlabel">(N) Points                                
                                                </td>
                                                <td class="ms-formbody">
                                                    <div style="float: left;">
                                                        <dx:ASPxCheckBox ID="chlbxNPoints" runat="server">
                                                            <ClientSideEvents Init="function(s,e){showHideNPointDetail(s,e);}" CheckedChanged="function(s,e){showHideNPointDetail(s,e);}" />
                                                        </dx:ASPxCheckBox>
                                                    </div>
                                                    <asp:Panel ID="chlbxNPointsPanel" runat="server" CssClass="chlbxNPointsPanel hide">
                                                        <div class="fleft padding-left5">
                                                            <dx:ASPxComboBox ID="cmbx1NPointOrder" Width="100px" runat="server" DropDownStyle="DropDown">
                                                                <Items>
                                                                    <dx:ListEditItem Text="Top" Value="descending" Selected="true"></dx:ListEditItem>
                                                                    <dx:ListEditItem Text="Bottom" Value="ascending"></dx:ListEditItem>
                                                                </Items>
                                                            </dx:ASPxComboBox>
                                                        </div>
                                                        <div class="fleft padding-left5">
                                                            <dx:ASPxSpinEdit runat="server" MinValue="0" ID="spinEdtNPoint" NumberType="Integer" Width="50px">
                                                                <Paddings PaddingLeft="10px" />
                                                            </dx:ASPxSpinEdit>
                                                        </div>
                                                        <div class="fleft padding-left5 relative-top3">Of</div>
                                                        <div class="fleft padding-left5">
                                                            <dx:ASPxComboBox ID="cmbxNPointExp" NullText="Expresson" runat="server" DropDownStyle="DropDown" OnInit="npointExpress_Init">
                                                            </dx:ASPxComboBox>
                                                        </div>
                                                        <div class="fleft padding-left5 relative-top3">(Expression)</div>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                            <tr id="Tr4">
                                                <td class="ms-formlabel">Order                                
                                                </td>
                                                <td class="ms-formbody">
                                                    <div class="fleft">
                                                        <dx:ASPxCheckBox ID="chkbxOrder" runat="server">
                                                            <ClientSideEvents Init="function(s,e){showHideOrderByDetail(s,e);}" CheckedChanged="function(s,e){showHideOrderByDetail(s,e);}" />
                                                        </dx:ASPxCheckBox>
                                                    </div>
                                                    <asp:Panel ID="axisOrderPanel" runat="server" CssClass="axisOrderPanel hide">
                                                        <div class="fleft padding-left5">
                                                            <dx:ASPxComboBox ID="cmbxOrderExp" NullText="Order Of" runat="server" DropDownStyle="DropDown" OnInit="orderByExpression_Init">
                                                            </dx:ASPxComboBox>
                                                        </div>
                                                        <div class="fleft padding-left5">
                                                            <dx:ASPxComboBox ID="cmbx1Order" runat="server" DropDownStyle="DropDown">
                                                                <Items>
                                                                    <dx:ListEditItem Text="Ascending" Value="ascending" Selected="true"></dx:ListEditItem>
                                                                    <dx:ListEditItem Text="Descending" Value="descending"></dx:ListEditItem>
                                                                </Items>
                                                            </dx:ASPxComboBox>
                                                        </div>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                             <tr id="trScaleType">
                                                <td class="ms-formlabel">Scale Type                                
                                                </td>
                                                <td class="ms-formbody">
                                                    <dx:ASPxComboBox ID="cbxScaleType" SelectedIndex="0" runat="server" DropDownStyle="DropDown">
                                                        <Items>
                                                            <dx:ListEditItem Text="Auto" Value="Auto" Selected="true"></dx:ListEditItem>
                                                            <dx:ListEditItem Text="DateTime" Value="DateTime"></dx:ListEditItem>
                                                            <dx:ListEditItem Text="Numerical" Value="Numerical"></dx:ListEditItem>
                                                            <dx:ListEditItem Text="Qualitative" Value="Qualitative"></dx:ListEditItem>
                                                        </Items>
                                                    </dx:ASPxComboBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>

                            <asp:Repeater ID="expressionRpter" OnItemDataBound="expressionRpter_ItemDataBound" runat="server">
                                <ItemTemplate>
                                    <div class="expression-<%# Container.ItemIndex %>" style="display: none;">
                                        <table class="ms-formtable" cellpadding="0" cellspacing="0" style="border-collapse: collapse" width="100%">

                                            <tr id="qflabel">
                                                <td class="ms-formlabel">Expression Name
                                     <asp:HiddenField ID="hdnExpressionFormat" runat="server" Value='<%# Eval("Title") %>' />
                                                </td>
                                                <td class="ms-formbody"><%# Eval("Title") %></td>
                                            </tr>


                                            <tr id="Tr6">
                                                <td class="ms-formlabel">Datapoint click action                                 
                                                </td>
                                                <td class="ms-formbody">
                                                    <dx:ASPxComboBox Theme="DevEx" Width="200" ID="expClickAction" runat="server">
                                                        <Items>
                                                            <dx:ListEditItem Value="None" Selected="true" Text="None" />
                                                            <dx:ListEditItem Value="NextDimension" Text="Next Dimension" />
                                                            <dx:ListEditItem Value="Detail" Text="Detail" />
                                                            <dx:ListEditItem Value="Inherit" Text="Inherit From Dimension" />
                                                        </Items>
                                                    </dx:ASPxComboBox>

                                                </td>
                                            </tr>

                                            <tr id="Tr8">
                                                <td class="ms-formlabel">Y Axis:                                  
                                                </td>
                                                <td class="ms-formbody">
                                                    <dx:ASPxComboBox Theme="DevEx" Width="110" ID="cmbxYaxisConfig" runat="server">
                                                        <Items>
                                                            <dx:ListEditItem Value="0" Text="Primary" Selected="true" />
                                                            <dx:ListEditItem Value="1" Text="Secondary" />
                                                        </Items>
                                                    </dx:ASPxComboBox>

                                                </td>
                                            </tr>
                                            <tr id="Tr23">
                                                <td class="ms-formlabel">Data Point Format:                                  
                                                </td>
                                                <td class="ms-formbody">
                                                    <dx:ASPxComboBox Theme="DevEx" Width="110" ID="aspxYAxisPointFormat" runat="server">
                                                        <Items>
                                                            <dx:ListEditItem Value="" Text="Default" />
                                                            <dx:ListEditItem Value="currency" Text="Currency" />
                                                            <dx:ListEditItem Value="mintodays" Text="Min to Days" />
                                                        </Items>
                                                    </dx:ASPxComboBox>

                                                </td>
                                            </tr>
                                            <tr id="Tr1">
                                                <td class="ms-formlabel">Hide Label                               
                                                </td>
                                                <td class="ms-formbody">
                                                    <asp:CheckBox ID="hideLevelChkbx" AutoPostBack="true" CssClass="hideLevelChkbx" OnCheckedChanged="hideLevelChkbx_CheckedChanged" runat="server" />
                                                </td>
                                            </tr>
                                            <tr id="trLabelProperties" runat="server">

                                                <td colspan="2">
                                                    <%--label property for Bar chart--%>
                                                    <table id="barChartlblTable" runat="server" visible="true" style="width: 100%">
                                                        <tr>
                                                            <td class="ms-formlabel">Pattern            
                                                            </td>
                                                            <td class="ms-formbody">
                                                                <div class="fleft">
                                                                    <dx:ASPxTextBox ID="txtLabelPattern" runat="server" Width="110"></dx:ASPxTextBox>
                                                                </div>
                                                                <div class="fleft padding-left5">
                                                                    <span>Type {V}: Expression Value and {A}: X-Axis Value </span>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="ms-formlabel">Label Position                                  
                                                            </td>
                                                            <td class="ms-formbody">
                                                                <dx:ASPxComboBox Theme="DevEx" Width="110" ID="cbxbarLablPosition" runat="server">
                                                                    <Items>
                                                                        <dx:ListEditItem Value="Auto" Selected="true" Text="Auto" />
                                                                        <dx:ListEditItem Value="Top" Text="Top" />
                                                                        <dx:ListEditItem Value="TopInside" Text="Top Inside" />
                                                                        <dx:ListEditItem Value="Center" Selected="true" Text="Center" />
                                                                        <dx:ListEditItem Value="BottomInside" Text="Bottom Inside" />
                                                                    </Items>
                                                                </dx:ASPxComboBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="ms-formlabel">Orientation                                  
                                                            </td>
                                                            <td class="ms-formbody">
                                                                <dx:ASPxComboBox Theme="DevEx" Width="110" ID="cbxbarLabelOrientation" runat="server">
                                                                    <Items>
                                                                        <dx:ListEditItem Value="Horizontal" Selected="true" Text="Horizontal" />
                                                                        <dx:ListEditItem Value="TopToBottom" Text="Top To Bottom" />
                                                                        <dx:ListEditItem Value="BottomToTop" Text="Bottom To Top" />
                                                                    </Items>
                                                                </dx:ASPxComboBox>
                                                            </td>
                                                        </tr>
                                                    </table>

                                                    <%--label property for Column(SideBySideFullStackedBar) chart--%>
                                                    <table id="columnChartlblTable" style="width: 100%" runat="server" visible="true">
                                                        <tr>
                                                            <td class="ms-formlabel">Value As Percent                                  
                                                            </td>
                                                            <td class="ms-formbody">
                                                                <dx:ASPxCheckBox ID="chkSideBySideFullStackedBarValAsPertge" Checked="true" runat="server" />
                                                            </td>
                                                        </tr>
                                                    </table>

                                                    <%--label property for Doughnut chart--%>
                                                    <table id="doughnutChartlblTable" style="width: 100%" runat="server" visible="true">
                                                        <tr>
                                                            <td class="ms-formlabel">Value As Percent                                  
                                                            </td>
                                                            <td class="ms-formbody">
                                                                <dx:ASPxCheckBox ID="chkbxdoughnutLblPercentage" Checked="true" runat="server"></dx:ASPxCheckBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="ms-formlabel">Hole Radius                                  
                                                            </td>
                                                            <td class="ms-formbody">
                                                                <dx:ASPxComboBox Theme="DevEx" Width="110" ID="cbxDNHoleRadius" runat="server">
                                                                    <Items>
                                                                        <dx:ListEditItem Value="60" Selected="true" Text="Default (60%)" />
                                                                        <dx:ListEditItem Value="0" Text="0%" />
                                                                        <dx:ListEditItem Value="15" Text="15%" />
                                                                        <dx:ListEditItem Value="30" Text="30%" />
                                                                        <dx:ListEditItem Value="50" Text="50%" />
                                                                        <dx:ListEditItem Value="75" Text="75%" />
                                                                        <dx:ListEditItem Value="90" Text="90%" />
                                                                        <dx:ListEditItem Value="100" Text="100%" />
                                                                    </Items>
                                                                </dx:ASPxComboBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="ms-formlabel ms-formbody">Label Position                                  
                                                            </td>
                                                            <td class="ms-formbody">

                                                                <dx:ASPxComboBox Theme="DevEx" Width="110" ID="cbxDNLblPosition" runat="server">
                                                                    <Items>
                                                                        <dx:ListEditItem Value="Inside" Text="Inside" />
                                                                        <dx:ListEditItem Value="Outside" Text="Outside" />
                                                                        <dx:ListEditItem Value="TwoColumns" Text="TwoColumns" />
                                                                        <dx:ListEditItem Value="Radial" Selected="true" Text="Radial" />
                                                                    </Items>
                                                                </dx:ASPxComboBox>
                                                            </td>

                                                        </tr>
                                                        <tr>
                                                            <td class="ms-formlabel ms-formbody">Exploded Points                                  
                                                            </td>
                                                            <td class="ms-formbody">

                                                                <dx:ASPxComboBox Theme="DevEx" Width="110" ID="cbxDNExplodedpoint" runat="server">
                                                                    <Items>
                                                                        <dx:ListEditItem Value="None" Selected="true" Text="None" />
                                                                        <dx:ListEditItem Value="All" Text="All" />
                                                                        <dx:ListEditItem Value="MaxValue" Text="Max Value" />
                                                                        <dx:ListEditItem Value="MinValue" Text="Min Value" />
                                                                    </Items>
                                                                </dx:ASPxComboBox>
                                                            </td>

                                                        </tr>
                                                    </table>

                                                    <%--label property for Funnel chart--%>
                                                    <table id="funnelChartlblTable" runat="server" style="width: 100%" visible="true">
                                                        <tr>
                                                            <td class="ms-formlabel">Value As Percent                                  
                                                            </td>
                                                            <td class="ms-formbody">
                                                                <dx:ASPxCheckBox ID="chkfunnelLblAsPercentage" Checked="true" runat="server"></dx:ASPxCheckBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="ms-formlabel">Label Position                                  
                                                            </td>
                                                            <td class="ms-formbody">

                                                                <dx:ASPxComboBox Theme="DevEx" Width="110" ID="cbxFunnellblposition" runat="server">
                                                                    <Items>
                                                                        <dx:ListEditItem Value="LeftColumn" Text="LeftColumn" />
                                                                        <dx:ListEditItem Value="Left" Text="Left" />
                                                                        <dx:ListEditItem Value="Center" Text="Center" />
                                                                        <dx:ListEditItem Value="Right" Selected="true" Text="Right" />
                                                                        <dx:ListEditItem Value="RightColumn" Text="RightColumn" />
                                                                    </Items>
                                                                </dx:ASPxComboBox>
                                                            </td>

                                                        </tr>
                                                        <tr>
                                                            <td class="ms-formlabel ms-formbody">Point Distance                                  
                                                            </td>
                                                            <td class="ms-formbody">
                                                                <asp:TextBox ID="txtFunnelPointDist" Width="50" runat="server" type="number" Text="0" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="ms-formlabel ms-formbody">Height / Width                                  
                                                            </td>
                                                            <td class="ms-formbody">
                                                                <dx:ASPxComboBox Theme="DevEx" Width="110" ID="cbxFunnelHeightWidth" runat="server">
                                                                    <Items>
                                                                        <dx:ListEditItem Value="0.1" Text="0.1" />
                                                                        <dx:ListEditItem Value="0.25" Text="0.25" />
                                                                        <dx:ListEditItem Value="0.5" Text="0.5" />
                                                                        <dx:ListEditItem Value="0.75" Text="0.75" />
                                                                        <dx:ListEditItem Value="1" Selected="true" Text="1" />
                                                                        <dx:ListEditItem Value="2" Text="2" />
                                                                        <dx:ListEditItem Value="4" Text="4" />
                                                                        <dx:ListEditItem Value="6" Text="6" />
                                                                        <dx:ListEditItem Value="8" Text="8" />
                                                                        <dx:ListEditItem Value="10" Text="10" />
                                                                    </Items>
                                                                </dx:ASPxComboBox>
                                                            </td>
                                                            <td class="ms-formlabel ms-formbody">Auto Hieght/Width                                  
                                                            </td>
                                                            <td class="ms-formbody">
                                                                <dx:ASPxCheckBox ID="ChkFunnelAutoHeightWidth" Checked="true" runat="server"></dx:ASPxCheckBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="ms-formlabel ms-formbody">Align to Center                                  
                                                            </td>
                                                            <td class="ms-formbody">
                                                                <dx:ASPxCheckBox ID="ChkFunnelAligntoCenter" runat="server"></dx:ASPxCheckBox>
                                                            </td>
                                                        </tr>
                                                    </table>

                                                    <%--label property for Pie chart--%>
                                                    <table id="pieChartlblTable" runat="server" style="width: 100%" visible="true">
                                                        <tr>
                                                            <td class="ms-formlabel">Value As Percent                                  
                                                            </td>
                                                            <td class="ms-formbody">
                                                                <dx:ASPxCheckBox ID="ChkPieValueAsPer" Checked="true" runat="server"></dx:ASPxCheckBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="ms-formlabel">Label Position                                  
                                                            </td>
                                                            <td class="ms-formbody">

                                                                <dx:ASPxComboBox Theme="DevEx" Width="110" ID="cbxPieLblPosition" runat="server">
                                                                    <Items>
                                                                        <dx:ListEditItem Value="Inside" Text="Inside" />
                                                                        <dx:ListEditItem Value="Outside" Text="Outside" />
                                                                        <dx:ListEditItem Value="TwoColumns" Text="TwoColumns" />
                                                                        <dx:ListEditItem Value="Radial" Selected="true" Text="Radial" />
                                                                    </Items>
                                                                </dx:ASPxComboBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="ms-formlabel ms-formbody">Exploded Points                                  
                                                            </td>
                                                            <td class="ms-formbody">

                                                                <dx:ASPxComboBox Theme="DevEx" Width="110" ID="cbxPieExploadPoints" runat="server">
                                                                    <Items>
                                                                        <dx:ListEditItem Value="None" Selected="true" Text="None" />
                                                                        <dx:ListEditItem Value="All" Text="All" />
                                                                        <dx:ListEditItem Value="MaxValue" Text="Max Value" />
                                                                        <dx:ListEditItem Value="MinValue" Text="Min Value" />
                                                                    </Items>
                                                                </dx:ASPxComboBox>
                                                            </td>
                                                        </tr>
                                                    </table>

                                                    <%--label property for Bar Stacked chart--%>
                                                    <table id="barStackedChartlblTable" runat="server" style="width: 100%" visible="true">
                                                        <tr>
                                                            <td class="ms-formlabel">Pattern            
                                                            </td>
                                                            <td class="ms-formbody">
                                                                <dx:ASPxTextBox ID="txtBarStackedLbPattern" runat="server" Width="110"></dx:ASPxTextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="ms-formlabel">Label Position                                  
                                                            </td>
                                                            <td class="ms-formbody">
                                                                <dx:ASPxComboBox Theme="DevEx" Width="110" ID="cbxBarStackedLblPosition" runat="server">
                                                                    <Items>
                                                                        <dx:ListEditItem Value="Top Inside" Text="Top Inside" />
                                                                        <dx:ListEditItem Value="Center" Selected="true" Text="Center" />
                                                                        <dx:ListEditItem Value="Bottom Inside" Text="Bottom Inside" />
                                                                    </Items>
                                                                </dx:ASPxComboBox>

                                                            </td>

                                                        </tr>
                                                        <tr>
                                                            <td class="ms-formlabel">Orientation                                  
                                                            </td>
                                                            <td class="ms-formbody">
                                                                <dx:ASPxComboBox Theme="DevEx" Width="110" ID="cbxBarStackedLblOrientation" runat="server">
                                                                    <Items>
                                                                        <dx:ListEditItem Value="Horizontal" Selected="true" Text="Horizontal" />
                                                                        <dx:ListEditItem Value="TopToBottom" Text="Top To Bottom" />
                                                                        <dx:ListEditItem Value="BottomToTop" Text="Bottom To Top" />
                                                                    </Items>
                                                                </dx:ASPxComboBox>
                                                            </td>
                                                        </tr>
                                                        <tr></tr>
                                                    </table>

                                                    <%--label property for Line chart--%>
                                                    <table id="lineChartlblTable" runat="server" style="width: 100%" visible="true">
                                                        <tr>
                                                            <td class="ms-formlabel">Pattern            
                                                            </td>
                                                            <td class="ms-formbody">
                                                                <dx:ASPxTextBox ID="aspxLineChartLbPattern" runat="server" Width="110"></dx:ASPxTextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="ms-formlabel">Marker Kind                                  
                                                            </td>
                                                            <td class="ms-formbody">
                                                                <dx:ASPxComboBox Theme="DevEx" Width="110" ID="cbxLineMarkType" runat="server">
                                                                    <Items>
                                                                        <dx:ListEditItem Value="Square" Text="Square" />
                                                                        <dx:ListEditItem Value="Diamond" Text="Diamond" />
                                                                        <dx:ListEditItem Value="Triangle" Text="Triangle" />
                                                                        <dx:ListEditItem Value="Inverted Triangle" Text="Inverted Triangle" />
                                                                        <dx:ListEditItem Value="Circle" Selected="true" Text="Circle" />
                                                                        <dx:ListEditItem Value="Plus" Text="Plus" />
                                                                        <dx:ListEditItem Value="Star 3-points" Text="Star 3-points" />
                                                                        <dx:ListEditItem Value="Star 4-points" Text="Star 4-points" />
                                                                        <dx:ListEditItem Value="Star 5-points" Text="Star 5-points" />
                                                                        <dx:ListEditItem Value="Star 6-points" Text="Star 6-points" />
                                                                        <dx:ListEditItem Value="Star 10-points" Text="Star 10-points" />
                                                                        <dx:ListEditItem Value="Pentagon" Text="Pentagon" />
                                                                        <dx:ListEditItem Value="Hexagon" Text="Hexagon" />
                                                                    </Items>
                                                                </dx:ASPxComboBox>
                                                            </td>

                                                        </tr>
                                                        <tr>
                                                            <td class="ms-formlabel">Marker Size                                  
                                                            </td>
                                                            <td class="ms-formbody">
                                                                <dx:ASPxComboBox Theme="DevEx" Width="110" ID="cbxLineMarkSize" runat="server">
                                                                    <Items>
                                                                        <dx:ListEditItem Value="8" Text="8" />
                                                                        <dx:ListEditItem Value="10" Selected="true" Text="10" />
                                                                        <dx:ListEditItem Value="12" Text="12" />
                                                                        <dx:ListEditItem Value="14" Text="14" />
                                                                        <dx:ListEditItem Value="16" Text="16" />
                                                                        <dx:ListEditItem Value="20" Text="20" />
                                                                        <dx:ListEditItem Value="22" Text="22" />
                                                                        <dx:ListEditItem Value="24" Text="24" />
                                                                        <dx:ListEditItem Value="26" Text="26" />
                                                                        <dx:ListEditItem Value="28" Text="28" />
                                                                        <dx:ListEditItem Value="30" Text="30" />
                                                                    </Items>
                                                                </dx:ASPxComboBox>
                                                            </td>
                                                        </tr>

                                                    </table>
                                                    <%--label property for SPline chart--%>
                                                    <table id="spLineChartlblTable" runat="server" style="width: 100%" visible="true">
                                                        <tr>
                                                            <td class="ms-formlabel">Pattern            
                                                            </td>
                                                            <td class="ms-formbody">
                                                                <dx:ASPxTextBox ID="aspxSPLineLbPattern" runat="server" Width="110"></dx:ASPxTextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="ms-formlabel">Line Tension Percent                                  
                                                            </td>
                                                            <td class="ms-formbody">
                                                                <dx:ASPxComboBox Theme="DevEx" Width="110" ID="cbxSPlineTensionPrcntg" runat="server">
                                                                    <Items>
                                                                        <dx:ListEditItem Value="0" Text="0%" />
                                                                        <dx:ListEditItem Value="15" Text="15%" />
                                                                        <dx:ListEditItem Value="30" Text="30%" />
                                                                        <dx:ListEditItem Value="50" Text="50%" />
                                                                        <dx:ListEditItem Value="75" Selected="true" Text="75%" />
                                                                        <dx:ListEditItem Value="90" Text="90%" />
                                                                        <dx:ListEditItem Value="100" Text="100%" />
                                                                    </Items>
                                                                </dx:ASPxComboBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="ms-formlabel">Marker Kind                                  
                                                            </td>
                                                            <td class="ms-formbody">
                                                                <dx:ASPxComboBox Theme="DevEx" Width="110" ID="cbxSplineMarkerType" runat="server">
                                                                    <Items>
                                                                        <dx:ListEditItem Value="Square" Text="Square" />
                                                                        <dx:ListEditItem Value="Diamond" Text="Diamond" />
                                                                        <dx:ListEditItem Value="Triangle" Text="Triangle" />
                                                                        <dx:ListEditItem Value="Inverted Triangle" Text="Inverted Triangle" />
                                                                        <dx:ListEditItem Value="Circle" Selected="true" Text="Circle" />
                                                                        <dx:ListEditItem Value="Plus" Text="Plus" />
                                                                        <dx:ListEditItem Value="Star 3-points" Text="Star 3-points" />
                                                                        <dx:ListEditItem Value="Star 4-points" Text="Star 4-points" />
                                                                        <dx:ListEditItem Value="Star 5-points" Text="Star 5-points" />
                                                                        <dx:ListEditItem Value="Star 6-points" Text="Star 6-points" />
                                                                        <dx:ListEditItem Value="Star 10-points" Text="Star 10-points" />
                                                                        <dx:ListEditItem Value="Pentagon" Text="Pentagon" />
                                                                        <dx:ListEditItem Value="Hexagon" Text="Hexagon" />
                                                                    </Items>
                                                                </dx:ASPxComboBox>
                                                            </td>

                                                        </tr>
                                                        <tr>
                                                            <td class="ms-formlabel ms-formbody">Marker Size                                  
                                                            </td>
                                                            <td class="ms-formbody">
                                                                <dx:ASPxComboBox Theme="DevEx" Width="110" ID="cbxSplineMarkerSize" runat="server">
                                                                    <Items>
                                                                        <dx:ListEditItem Value="8" Text="8" />
                                                                        <dx:ListEditItem Value="10" Selected="true" Text="10" />
                                                                        <dx:ListEditItem Value="12" Text="12" />
                                                                        <dx:ListEditItem Value="14" Text="14" />
                                                                        <dx:ListEditItem Value="16" Text="16" />
                                                                        <dx:ListEditItem Value="20" Text="20" />
                                                                        <dx:ListEditItem Value="22" Text="22" />
                                                                        <dx:ListEditItem Value="24" Text="24" />
                                                                        <dx:ListEditItem Value="26" Text="26" />
                                                                        <dx:ListEditItem Value="28" Text="28" />
                                                                        <dx:ListEditItem Value="30" Text="30" />
                                                                    </Items>
                                                                </dx:ASPxComboBox>
                                                            </td>
                                                        </tr>
                                                    </table>

                                                    <%--label property for StepLine chart--%>
                                                    <table id="stepLineChartlblTable" style="width: 100%" runat="server" visible="true">
                                                        <tr>
                                                            <td class="ms-formlabel">Pattern            
                                                            </td>
                                                            <td class="ms-formbody">
                                                                <dx:ASPxTextBox ID="aspxStepLineLbPattern" runat="server" Width="110"></dx:ASPxTextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="ms-formlabel">Label Angle                                  
                                                            </td>
                                                            <td class="ms-formbody">
                                                                <dx:ASPxComboBox Theme="DevEx" Width="110" ID="cbxStepLineLblAngle" runat="server">
                                                                    <Items>
                                                                        <dx:ListEditItem Value="0" Text="0" />
                                                                        <dx:ListEditItem Value="45" Selected="true" Text="45" />
                                                                        <dx:ListEditItem Value="90" Text="90" />
                                                                        <dx:ListEditItem Value="135" Text="135" />
                                                                        <dx:ListEditItem Value="180" Text="180" />
                                                                        <dx:ListEditItem Value="225" Text="225" />
                                                                        <dx:ListEditItem Value="270" Text="270" />
                                                                        <dx:ListEditItem Value="315" Text="315" />
                                                                    </Items>
                                                                </dx:ASPxComboBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="ms-formlabel">Marker Kind                                  
                                                            </td>
                                                            <td class="ms-formbody">
                                                                <dx:ASPxComboBox Theme="DevEx" Width="110" ID="cbxStepLineMarkerType" runat="server">
                                                                    <Items>
                                                                        <dx:ListEditItem Value="Square" Selected="true" Text="Square" />
                                                                        <dx:ListEditItem Value="Diamond" Text="Diamond" />
                                                                        <dx:ListEditItem Value="Triangle" Text="Triangle" />
                                                                        <dx:ListEditItem Value="Inverted Triangle" Text="Inverted Triangle" />
                                                                        <dx:ListEditItem Value="Circle" Text="Circle" />
                                                                        <dx:ListEditItem Value="Plus" Text="Plus" />
                                                                        <dx:ListEditItem Value="Star 3-points" Text="Star 3-points" />
                                                                        <dx:ListEditItem Value="Star 4-points" Text="Star 4-points" />
                                                                        <dx:ListEditItem Value="Star 5-points" Text="Star 5-points" />
                                                                        <dx:ListEditItem Value="Star 6-points" Text="Star 6-points" />
                                                                        <dx:ListEditItem Value="Star 10-points" Text="Star 10-points" />
                                                                        <dx:ListEditItem Value="Pentagon" Text="Pentagon" />
                                                                        <dx:ListEditItem Value="Hexagon" Text="Hexagon" />
                                                                    </Items>
                                                                </dx:ASPxComboBox>
                                                            </td>

                                                        </tr>
                                                        <tr>
                                                            <td class="ms-formlabel ms-formbody">Marker Size                                  
                                                            </td>
                                                            <td class="ms-formbody">
                                                                <dx:ASPxComboBox Theme="DevEx" Width="110" ID="cbxStepLineMarkerSize" runat="server">
                                                                    <Items>
                                                                        <dx:ListEditItem Value="8" Text="8" />
                                                                        <dx:ListEditItem Value="10" Text="10" />
                                                                        <dx:ListEditItem Value="12" Text="12" />
                                                                        <dx:ListEditItem Value="14" Text="14" />
                                                                        <dx:ListEditItem Value="16" Text="16" />
                                                                        <dx:ListEditItem Value="20" Selected="true" Text="20" />
                                                                        <dx:ListEditItem Value="22" Text="22" />
                                                                        <dx:ListEditItem Value="24" Text="24" />
                                                                        <dx:ListEditItem Value="26" Text="26" />
                                                                        <dx:ListEditItem Value="28" Text="28" />
                                                                        <dx:ListEditItem Value="30" Text="30" />
                                                                    </Items>
                                                                </dx:ASPxComboBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="ms-formlabel ms-formbody">Inverted                              
                                                            </td>
                                                            <td class="ms-formbody">
                                                                <dx:ASPxCheckBox ID="chkSteplineInverted" runat="server" />

                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>

                                            </tr>
                                            <tr>
                                                <td class="ms-formlabel">Dimensions            
                                                </td>
                                                <td class="ms-formbody">
                                                    <dx:ASPxCheckBoxList ID="chkLineChartimensions" OnLoad="chkLineChartimensions_Load" Border-BorderStyle="None" RepeatDirection="Vertical" runat="server">
                                                        <Items>
                                                        </Items>
                                                    </dx:ASPxCheckBoxList>
                                                </td>
                                            </tr>

                                        </table>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>

                        </div>

                    </td>
                </tr>

            </table>
        </div>

        <%--new UI Ends--%>
    </div>

    <div class="button-main">
        <div class="fleft">
            <span id="preViewTab" runat="server" style="float: right;  padding: 0px 0px 0px 1px;" >
                <dx:ASPxButton ID="btnpreView" runat="server"  Text="Preview" ImagePosition="Left" >
                    <Image Url="/Content/ButtonImages/execute.png"></Image>
                    <ClientSideEvents Click="function(s,e){preViewChart();}" />
                </dx:ASPxButton>
            </span>

        </div>

        <div class="fright">
            <dx:ASPxButton ID="btnMoveToNxtTab" ValidationGroup="generalInfo" OnClick="btnMoveToNxtTab_Click" runat="server" CssClass="primary-blueBtn" Text="Next" ImagePosition="Right" >
                    <Image Url="/content/ButtonImages/next.png"></Image>
                    <ClientSideEvents Click="function(s,e){loadingPanel.Show();}" />
                </dx:ASPxButton>

                <dx:ASPxButton ID="btnsavechanges" ClientInstanceName="btnsavechanges" OnClick="Save_Click" runat="server" AutoPostBack="true" CssClass="primary-blueBtn" Text="Save Changes" ImagePosition="Right">
                    <Image Url="/content/ButtonImages/save.png"></Image>
                </dx:ASPxButton>

            <dx:ASPxButton  ID="btnTab" runat="server" ValidationGroup="generalInfo" OnClick="btnTab_Click" CssClass="next-buttonclick hide" >
                <ClientSideEvents Click="function(s,e){loadingPanel.Show();}" />
            </dx:ASPxButton>
                 
          
        </div>
        
        <%--add new dimension/expression buttons :: Start--%>
        <div class="fleft" id="AddnewDimensionDiv" runat="server" style=" margin-top:5px; margin-left:3px;">
          
                <dx:ASPxButton ID="AddnewDimension" runat="server" OnClick="AddnewDimension_Click" AutoPostBack="true" CssClass="Alignbutton" Text="Add New Dimension" />
          
        </div>
        <div class="fleft" id="AddnewExpressionDiv" runat="server" visible="false" style=" margin-top:5px; margin-left:3px;">
                <dx:ASPxButton ID="btnAddExpr" runat="server" AutoPostBack="true" OnClick="AddnewExpression_Click" ClientInstanceName="btnAddExpr" CssClass="Alignbutton" Text="Add Expression">
                
                </dx:ASPxButton>
        
        </div>

        <%--add new dimension/expression buttons :: End--%>

        
    </div>
</div>


<!--Start:Create formula for basic filter-->
<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
    .filterformulapopup {
        float: left;
        width: 440px;
        height: 170px;
        position: absolute;
        z-index: 1000;
        text-align: center;
        background: white;
        border: 2px outset gray;
    }

        .filterformulapopup .sub-container {
            float: inherit;
            width: 100%;
        }

        .filterformulapopup .titlediv {
            float: left;
            width: 100%;
            padding: 10px 0px 10px 10px;
        }

            .filterformulapopup .titlediv span {
                float: inherit;
                font-weight: bold;
                padding-left: 5px;
            }

        .filterformulapopup .contentdiv {
            float: inherit;
            width: 92%;
            padding-top: 2px;
        }

        .filterformulapopup .content-head {
            float: inherit;
        }

    #formulaStartWith {
        width: 60px;
    }

    #formulaOperatorForPopup {
        width: 55px;
    }

    #fieldValueForPopup {
        width: 80px;
    }

    #formulaBoxForPop {
        width: 400px;
    }
</style>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">

    function createBasicFilterFormula(obj, formulatTextBoxID) {

        var dropdownObj = $("#<%=ddlFactTableFieldsForFilter.ClientID %>");
        $("#filterFormulaPopup #formulaTxtBoxID").val(formulatTextBoxID);

        $("#formulaBoxForPop").val("");
        $("#formulatEditorError").html('');
        $("#filterFormulaPopup #formulaStartWith").attr("disabled", true);
        ShowValueBoxBasedOnDataType();
        var currentPosition = $(obj).position();
        var top = currentPosition.top;
        var left = currentPosition.left - $("#filterFormulaPopup").width();
        if (left <= 0) {
            left = 20;
        }
        if (top <= 0)
            top = 100;
        $("#filterFormulaPopup").css({ "display": "block", "top": (top - 20) + "px", "left": (left + 10) + "px" });

    }

    function ShowValueBoxBasedOnDataType() {
        var fieldDropdown = $("#filterFormulaPopup .formulafields");
        var fieldValueContainer = $("#filterFormulaPopup #fieldValueContainerForPopup");
        if (fieldDropdown.attr('datatype') && fieldDropdown.attr('datatype').toLowerCase() == "datetime") {
            fieldValueContainer.html("<input type='text' id='fieldValueForPopup'>");
        }
        else {
            fieldValueContainer.html("<input type='text' id='fieldValueForPopup'>");
        }
    }

    function popupAddFormulaExpression(obj) {
        
        var fieldStartWith = $("#filterFormulaPopup #formulaStartWith");
        var fieldDropdown = $("#filterFormulaPopup .formulafields");
        var fieldValueBox = $("#filterFormulaPopup #fieldValueForPopup");
        var fieldValueOperator = $("#filterFormulaPopup #formulaOperatorForPopup");

        var fieldOption = $("#filterFormulaPopup .formulafields option:selected");

        //Gets errorbox and clear errorg
        var errorBox = $("#formulatEditorError");
        errorBox.html('');

        //Gets datatype of field
        var dataType = $.trim(fieldOption.text().toLowerCase());
        if ($.trim(fieldValueBox.val()) != "") {
            if (dataType.indexOf("(string)") > 0) {
                if (!dataType.indexOf("TicketId"))
                    fieldValueBox.val(fieldValueBox.val().replace(/[^a-zA-Z0-9 ._]+/g, ''));
            }
            else if (dataType.indexOf("(datetime)") > 0) {

            }
            else {
                if (!Number(fieldValueBox.val()) || Number(fieldValueBox.val()) == NaN) {
                    errorBox.html("Please enter value in digit.");
                    return;
                }
            }
        }
        else {
            errorBox.html("Please enter value.");
            return;
        }

        var fExpression = "";
        if ($.trim($("#formulaBoxForPop").val()) != "") {
            fExpression += fieldStartWith.val() + " ";
        }
        else {
            fieldStartWith.removeAttr("disabled");
        }


        fExpression += "[" + fieldDropdown.val() + "] " + fieldValueOperator.val() + " ";


        if (dataType.indexOf("(string)") > 0) {
            fExpression += "'" + fieldValueBox.val() + "' ";
        }
        else if (dataType.indexOf("(datetime)") > 0) {
            fExpression += "#" + fieldValueBox.val() + "# ";
        }
        else {
            fExpression += fieldValueBox.val() + " ";
        }

        $("#formulaBoxForPop").val($.trim($("#formulaBoxForPop").val()) + " " + fExpression);
    }

    function popupAddFormulaDone(obj) {
        var txtBoxID = $("#filterFormulaPopup #formulaTxtBoxID").val();
        var txtBox = ASPxClientControl.GetControlCollection().GetByName($("." + txtBoxID).attr("id"))
        txtBox.SetValue($("#formulaBoxForPop").val());
        $("#filterFormulaPopup").css("display", "none");
    }

    function popupAddFormulaCancel(obj) {

        var fieldStartWith = $("#filterFormulaPopup #formulaStartWith");
        var fieldDropdown = $("#filterFormulaPopup .formulafields");
        var fieldValueBox = $("#filterFormulaPopup #fieldValueForPopup");
        var fieldValueOperator = $("#filterFormulaPopup #formulaOperatorForPopup");
        fieldStartWith.css("disabled", true);

        fieldValueBox.val("");
        $("#filterFormulaPopup #formulaTxtBoxID").val("");
        $("#filterFormulaPopup").css("display", "none");
    }

    function preViewChart() {
        var charturl = "<%=this.chartUrl%>";
        var titleVal = "Dashboard chart preview";
        window.parent.parent.UgitOpenPopupDialog(charturl, "", titleVal, "90", "90", "", "");

    }

</script>
<div id="filterFormulaPopup" class="filterformulapopup" style="display: none">
    <div class="sub-container">
        <input type="hidden" id="formulaTxtBoxID" />
        <table>

            <tr>
                <td align="left" valign="top" width="65">
                    <div><span>Start With:</span></div>
                    <div>
                        <span>
                            <select id="formulaStartWith" disabled="disabled">
                                <option value="AND">And</option>
                                <option value="OR">Or</option>
                            </select>
                        </span>
                    </div>
                </td>
                <td align="left" valign="top" width="100">
                    <div><span>Fact table Fields:</span></div>
                    <div>
                        <span id="ftFieldsContainer">
                            <asp:DropDownList ID="ddlFactTableFieldsForFilter" Width="130" CssClass="formulafields" runat="server"></asp:DropDownList>
                        </span>
                    </div>
                </td>
                <td align="left" valign="top" width="50">
                    <div><span>Operator:</span></div>
                    <div>
                        <span>
                            <select id="formulaOperatorForPopup">
                                <option value="=">=</option>
                                <option value="<>">!=</option>
                                <option value="<">&lt;</option>
                                <option value=">">&gt;</option>
                            </select>
                        </span>
                    </div>
                </td>
                <td align="left" valign="top" width="100">
                    <div><span>Value:</span></div>
                    <div>
                        <span id="fieldValueContainerForPopup"></span>
                        <span>
                            <img src="/content/images/add_icon.png" style="cursor: pointer;" alt="Add" onclick="popupAddFormulaExpression(this)" />
                        </span>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <div style="color: Red;" id="formulatEditorError"></div>
                </td>
            </tr>
            <tr>
                <td colspan="4" align="left" valign="top">
                    <textarea id="formulaBoxForPop" rows="3" cols="3">
      </textarea>
                </td>
            </tr>
            <tr>
                <td></td>
                <td></td>
                <td align="right" colspan="2">
                    <dx:ASPxButton ID="btnpopAddFormula" runat="server" Text="Done">
                        <ClientSideEvents Click="function(s,e){popupAddFormulaDone(s);}" />
                    </dx:ASPxButton>
                    <dx:ASPxButton ID="btnpopAddFormulaCancel" runat="server" Text="Cancel">
                        <ClientSideEvents Click="function(s,e){popupAddFormulaCancel(s);}" />
                    </dx:ASPxButton>
                    
                </td>
            </tr>
        </table>
    </div>
</div>
<!--Close:Create formula for basic filter-->



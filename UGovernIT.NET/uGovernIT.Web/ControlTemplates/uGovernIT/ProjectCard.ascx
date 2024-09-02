<%--<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>--%>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProjectCard.ascx.cs" Inherits="uGovernIT.Web.ProjectCard" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
    .projectCardView-gpc {
        position:relative;
    }
    .projectCardView-gpc .docIcon {
        position:absolute;
        top:5px;
        left:5px;
    }
</style>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    $(document).ready(function () {
        $('#lblLead').attr('for', $('#<%=chkLead.ClientID%>').attr('id'));
        $('#lblClientContract').attr('for', $('#<%=chkClientContracts.ClientID%>').attr('id'));
        $('#lblPiplelineCPROpenOMP').attr('for', $('#<%=chkPiplelineCPROpenOMP.ClientID%>').attr('id'));
        $('#lblchkConCPR').attr('for', $('#<%=chkConCPR.ClientID%>').attr('id'));
        $('#lblchkClosed').attr('for', $('#<%=chkClosed.ClientID%>').attr('id'));
        $('#lblchkCancelled').attr('for', $('#<%=chkCancelled.ClientID%>').attr('id'));
        $('#lblchkLost').attr('for', $('#<%=chkLost.ClientID%>').attr('id'));
    });

    function OnImageClientClick() {

        if ($("#hndCardType").val() == "Normal") {

            $("#hndCardType").val("Color");
        }
        else {

            $("#hndCardType").val("Normal");
        }

    }

    function cmbViewType_Changed(s, e) {
        if (cmbViewType.GetValue() == "Grid View") {          
            $("#dvProject").show();
            $("#dvProjectCardView").hide();
            $("#dvCustomFilter").hide();
            grdProject.Refresh();
        }
        else {
            $("#dvProject").hide();
            $("#dvProjectCardView").show();
            $("#dvCustomFilter").show();
        }
    }

    $(document).ready(function () {

        cmbViewType_Changed(null, null);
    });
</script>
<script data-v="<%=UGITUtility.AssemblyVersion %>">
   
    function UpdateGridHeight() {
        grdProject.SetHeight(0);
        var containerHeight = ASPxClientUtils.GetDocumentClientHeight();
        if (document.body.scrollHeight > containerHeight)
            containerHeight = document.body.scrollHeight;
        grdProject.SetHeight(containerHeight);
    }
    window.addEventListener('resize', function (evt) {
        if (!ASPxClientUtils.androidPlatform)
            return;
        var activeElement = document.activeElement;
        if (activeElement && (activeElement.tagName === "INPUT" || activeElement.tagName === "TEXTAREA") && activeElement.scrollIntoViewIfNeeded)
            window.setTimeout(function () { activeElement.scrollIntoViewIfNeeded(); }, 0);
    });
</script>

<div class="col-md-12 col-sm-12 col-xs-12 noPadding">
    <div class="row">
        <div style="float: left;padding-bottom: 12px;">
            <dx:ASPxComboBox ID="cmbViewType" ClientInstanceName="cmbViewType" ListBoxStyle-CssClass="aspxComboBox-listBox" 
                OnSelectedIndexChanged="cmbViewType_SelectedIndexChanged" runat="server" CssClass="aspxComBox-dropDown prjctView_selector">
                <Items>
                    <dx:ListEditItem Text="Card View" Value="Card View"/>
                    <dx:ListEditItem Text="Grid View" Value="Grid View" Selected="true"/>
                </Items>
                <ClientSideEvents SelectedIndexChanged="cmbViewType_Changed" />
            </dx:ASPxComboBox>
        </div>
        <div style="float: right;padding-bottom: 12px;">          
            <asp:ImageButton ID="imgExportToExcel" runat="server" ImageUrl="/Content/images/excel_icon.png" ToolTip="Export To Excel" Style="margin-left: 5px; float: right;" OnClick="imgExportToExcel_Click" CssClass="projectView_img" />
            <asp:ImageButton ID="imgExportToPDF" runat="server" ImageUrl="/Content/images/pdf_icon.png" ToolTip="Export To PDF" Style="margin-left: 5px; float: right;" OnClick="imgExportToPDF_Click" CssClass="projectView_img" />
            <asp:ImageButton ID="imgbtnToggleView" runat="server" ImageUrl="/Content/Images/sortByColor.png" ToolTip="Sort by Color/Title" Style="margin-left: 5px; float: right;" OnClick="imgbtnToggleView_Click" OnClientClick="OnImageClientClick()" CssClass="projectView_img"/>
        </div>
        <input type="hidden" id="hndCardType" name="hndCardType" />
    </div>
    <div id="dvProjectCardView" style="display: none" class="row">
        <dx:ASPxCardView CardLayoutProperties-Styles-LayoutGroup-Cell-CssClass="projectCardView-gpc" ID="projectCardView" 
            ClientInstanceName="projectCardView" CardLayoutProperties-SettingsItems-ShowCaption="False" 
            OnHtmlCardPrepared="projectCardView_HtmlCardPrepared" OnCustomColumnDisplayText="projectCardView_CustomColumnDisplayText" 
            KeyFieldName="ID" runat="server" Width="100%" AutoGenerateColumns="false" CssClass="homeGrid projectCardViewGrid">
            <CardLayoutProperties />
            <Settings LayoutMode="Flow" />
            <SettingsSearchPanel Visible="true" />

            <Styles>
                <SearchPanel Border-BorderStyle="None" CssClass="searchpanel projectCard_viewSearch"></SearchPanel>
                <FlowCard Height="130px" CssClass=" col-md-4 col-sm-4 col-xs-12 pojectView_card"></FlowCard>
                <EmptyCard Wrap="True"></EmptyCard>
                <PagerTopPanel CssClass="GridPagerTop"></PagerTopPanel>
            </Styles>

            <SettingsBehavior AllowSelectByCardClick="true"
                AllowSelectSingleCardOnly="True" AllowSort="False" />
            <%--<ClientSideEvents CardClick="ViewUserProfile" />--%>

            <SettingsPager Position="TopAndBottom" Mode="ShowPager" AlwaysShowPager="true" PrevPageButton-Image-Url="/Content/images/pre-arrow.png"
                NextPageButton-Image-Url="/Content/images/next-arrow.png" NextPageButton-Image-Width="15px" PrevPageButton-Image-Width="15px">
                <PageSizeItemSettings Visible="true" />
            </SettingsPager>
        </dx:ASPxCardView>
              <dx:ASPxCardViewExporter CardWidth="310" ID="CardViewExporter" CardViewID="projectCardView" OnRenderBrick="CardViewExporter_RenderBrick" runat="server"></dx:ASPxCardViewExporter> 
    </div>
    <div id="dvProject" style="display: none" class="row">
        <dx:ASPxGridView ID="grdProject" ClientInstanceName="grdProject" AutoGenerateColumns="False" runat="server" 
            OnHtmlRowPrepared="grdProject_HtmlRowPrepared" OnCustomColumnDisplayText="grdProject_CustomColumnDisplayText"
            SettingsText-EmptyDataRow="No record found." KeyFieldName="ID" Width="100%" CssClass="homeGrid customgridview">
            <settingsadaptivity adaptivitymode="HideDataCells" allowonlyoneadaptivedetailexpanded="true" ></settingsadaptivity>
            <columns>
            </columns>
            
            <Settings ShowFooter="true" ShowHeaderFilterButton="true" ShowGroupFooter="VisibleIfExpanded" />
            <SettingsBehavior AllowSort="true" AllowDragDrop="false" AutoExpandAllGroups="true" />
            <SettingsPopup>
                <HeaderFilter Height="200" />
            </SettingsPopup>
           <settingscommandbutton>
                <ShowAdaptiveDetailButton ButtonType="Button" Styles-Style-CssClass="homeGrid_openBTn"></ShowAdaptiveDetailButton>
                <HideAdaptiveDetailButton ButtonType="Button" Styles-Style-CssClass="homeGrid_closeBTn"></HideAdaptiveDetailButton>
            </settingscommandbutton>
            <Styles AlternatingRow-CssClass="ms-alternatingstrong">
                <Row HorizontalAlign="Center" CssClass="homeGrid_dataRow"></Row>
                <GroupRow Font-Bold="true" CssClass="homeGrid-groupRow"></GroupRow>
                <Header Font-Bold="true" HorizontalAlign="Center" CssClass="homeGrid_headerColumn"></Header>
                <AlternatingRow CssClass="ms-alternatingstrong"></AlternatingRow>
                <InlineEditCell HorizontalAlign="Center"></InlineEditCell>
                <Footer Font-Bold="true" HorizontalAlign="Center"></Footer>
                <PagerTopPanel CssClass="GridPagerTop"></PagerTopPanel>
            </Styles>

            <SettingsPager Position="TopAndBottom" Mode="ShowPager" AlwaysShowPager="true"  PageSize="15"
                PrevPageButton-Image-Url="/Content/images/pre-arrow.png" NextPageButton-Image-Url="/Content/images/next-arrow.png" 
                NextPageButton-Image-Width="15px" PrevPageButton-Image-Width="15px" NextPageButton-Image-UrlDisabled="/Content/images/nextDis-arrow.png" 
                PrevPageButton-Image-UrlDisabled="/Content/images/prevDis-arrow .png" AllButton-Image-Width="15px">
                    <PageSizeItemSettings Visible="true" />
            </SettingsPager>

        </dx:ASPxGridView>
    </div>
     
    <script type="text/javascript">
        ASPxClientControl.GetControlCollection().ControlsInitialized.AddHandler(function (s, e) {
            UpdateGridHeight();
        });
        ASPxClientControl.GetControlCollection().BrowserWindowResized.AddHandler(function (s, e) {
            UpdateGridHeight();
        });
</script>
</div>
<div id="dvCustomFilter" class="row projectView-chkboxWrap">
            <div class="cardView_chkWrap lead_chkBox">
                <asp:CheckBox ID="chkLead" runat="server" OnCheckedChanged="chk_CheckedChanged" AutoPostBack="true"/>
                <label class="leadLabel" id="lblLead">&nbsp; Lead  &nbsp;</label>
            </div>

            <div class="cardView_chkWrap ClientContracts_chkBox">
                <asp:CheckBox ID="chkClientContracts" runat="server" OnCheckedChanged="chk_CheckedChanged" AutoPostBack="true" />
                 <label class="leadLabel" id="lblClientContract">&nbsp; Client Contracts  &nbsp;</label>
            </div>

            <div class="cardView_chkWrap PiplelineCPROpenOMP_chkBox">
                <asp:CheckBox ID="chkPiplelineCPROpenOMP" runat="server" OnCheckedChanged="chk_CheckedChanged" AutoPostBack="true" />
                <label class="leadLabel" id="lblPiplelineCPROpenOMP">&nbsp; Pipeline CPR/CNS or Open Opportunity  &nbsp;</label>
            </div>

            <div class="cardView_chkWrap chkConCPR_chkBox">
                <asp:CheckBox ID="chkConCPR" runat="server" OnCheckedChanged="chk_CheckedChanged" AutoPostBack="true" />
                <label class="leadLabel" id="lblchkConCPR">&nbsp; In Construction CPR/CNS &nbsp;</label>
            </div>

            <div class="cardView_chkWrap chkClosed_chkBox">
                <asp:CheckBox ID="chkClosed" runat="server" OnCheckedChanged="chk_CheckedChanged" AutoPostBack="true"/>
                <label class="leadLabel" id="lblchkClosed">&nbsp; Closed &nbsp;  </label>
            </div>

            <div class="cardView_chkWrap chkCancelled_chkBox">
                <asp:CheckBox ID="chkCancelled" runat="server" OnCheckedChanged="chk_CheckedChanged" AutoPostBack="true" />
                <label class="leadLabel" id="lblchkCancelled">&nbsp; Cancelled &nbsp; </label>
            </div>

            <div class="cardView_chkWrap chkLost_chkBox">
                <asp:CheckBox ID="chkLost" runat="server" OnCheckedChanged="chk_CheckedChanged" AutoPostBack="true"/>
                <label class="leadLabel" id="lblchkLost">&nbsp; Opportunity Lost or CPR/CNS Lost &nbsp;</label>
            </div>
            
</div>

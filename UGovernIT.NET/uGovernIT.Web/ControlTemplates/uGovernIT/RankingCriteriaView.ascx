<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RankingCriteriaView.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.uGovernIT.RankingCriteriaView" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    var lastScore = 0;

    function OpenImportRankingCriteria() {
        window.parent.UgitOpenPopupDialog('<%= absoluteUrlImportCheckList %>', "", 'Import Ranking Criteria', '400px', '250px', 0, escape("<%= Request.Url.AbsolutePath %>"));
        return false;
    }

    function OpneEditRankingCriteria(obj, activityId) {
        window.parent.UgitOpenPopupDialog('<%= absoluteUrlCheckList %>' + '&LeadRankingId=' + activityId, "", 'Edit Ranking Criteria', '600px', '600px', 0, escape("<%= Request.Url.AbsolutePath %>"));
        return false;
    }
    
    function UpdateGridHeight() {
        try {
            grid.SetHeight(0);
            var containerHeight = ASPxClientUtils.GetDocumentClientHeight();
            if (document.body.scrollHeight > containerHeight)
                containerHeight = document.body.scrollHeight;

            grid.SetHeight(containerHeight);

        } catch (e) {
        }        
    }

    window.addEventListener('resize', function (evt) {
        try {
            if (!ASPxClientUtils.androidPlatform)
                return;
        } catch (e) {
        }
        
        var activeElement = document.activeElement;
        if (activeElement && (activeElement.tagName === "INPUT" || activeElement.tagName === "TEXTAREA") && activeElement.scrollIntoViewIfNeeded)
            window.setTimeout(function () { activeElement.scrollIntoViewIfNeeded(); }, 0);
    });

    function GetScore(s, e) {
        lastScore = s.GetText()
    }

    function SetScore(s, rowIndex, e) {            
        var ranking = s.GetText();

        if (ranking === lastScore) {
            return false;
        }

        if (ranking < 1 || ranking > 5 || isNaN(ranking) || ranking != parseInt(ranking)) {
            s.SetValue(lastScore);
            alert('Enter Ranking between 1 and 5');
            return false;
        }

        var weight = grid.batchEditApi.GetCellValue(rowIndex, "Weight");
        grid.batchEditApi.SetCellValue(rowIndex, "WeightedScore", (ranking * weight).toFixed(1));
        grid.batchEditApi.SetCellValue(rowIndex, "Ranking", ranking);
    }
              
    function TextBox_TextChanged(s, rowIndex, e) {
        SetScore(s, rowIndex, e);        
    }

    function onFocusedCellChanging(s, e) {
        if (e.cellInfo.column.fieldName == 'Ranking' || e.cellInfo.column.fieldName == 'RankingCriteria' || e.cellInfo.column.fieldName == 'Description' || e.cellInfo.column.fieldName == 'Weight' || e.cellInfo.column.fieldName == 'WeightedScore')
            e.cancel = true;
    }

    function UpdateCriteria(s, e) {
        if (grid.batchEditApi.HasChanges() == true) {
            grid.UpdateEdit();
        }
        else {
            grid.PerformCallback();
        }
        //CloseandRefresh();
        setTimeout(function(){ window.parent.CloseWindowCallback(1, document.location.href); }, 3000);
    }
</script>
<%--<script type="text/javascript">
    $(document).ready(function () {
       alert($('.leadRanking-wrap').parents().attr('class'));
    })
</script>--%>

<div id="content leadRanking-wrap">    
    <ugit:ASPxGridView ID="grdRankingCriteria" ClientInstanceName="grid" runat="server" Width="100%" KeyFieldName="ID" EnableViewState="false" AlternatingRowStyle-BackColor="WhiteSmoke"
        AutoGenerateColumns="false" AllowFiltering="true" SettingsBehavior-AllowSort="false" 
        DataKeyNames="ID" GridLines="None" Visible="false" OnCustomSummaryCalculate="grdRankingCriteria_CustomSummaryCalculate" OnBatchUpdate="grid_BatchUpdate"  Styles-Footer-HorizontalAlign="Left"
        OnSummaryDisplayText="grdRankingCriteria_SummaryDisplayText" CssClass="customgridview homeGrid" OnCustomCallback="ASPxGridView1_CustomCallback">
        <SettingsAdaptivity AdaptivityMode="Off" AllowOnlyOneAdaptiveDetailExpanded="true"></SettingsAdaptivity>
        <Columns>
            <dx:GridViewDataTextColumn Name="aEdit" Width="10px" Visible="false">
                <DataItemTemplate>
                    <asp:ImageButton OnClientClick='<%# string.Format("javascript:return OpneEditRankingCriteria(this, {0})", Eval("ID")) %>'
                        ToolTip="Edit" ID="imgButtonEdit" runat="server" ImageUrl="/Content/images/editNewIcon.png" Style="padding-bottom: 8px;" CssClass="crmActivity_editBtn" />
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn Caption="Ranking" FieldName="Ranking" Width="50" CellStyle-HorizontalAlign="Left" EditCellStyle-HorizontalAlign="Left">
                <DataItemTemplate>
                    <%--<dx:ASPxTextBox ID="txtRanking" runat="server" Text='<%#Bind("Ranking") %>' ClientSideEvents-LostFocus="SetScore" ClientSideEvents-GotFocus="GetScore" OnInit="txtRanking_Init" ></dx:ASPxTextBox>--%>
                    <dx:ASPxTextBox ID="txtRanking" runat="server" Text='<%#Bind("Ranking") %>'  ClientSideEvents-GotFocus="GetScore" OnInit="txtRanking_Init" Width="50" ></dx:ASPxTextBox>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>


            <dx:GridViewDataTextColumn Caption="Ranking Criteria" FieldName="RankingCriteria" ReadOnly="true">
                <FooterTemplate>
                    <div style="font-weight: bold; align-content: center">
                        Total
                    </div>
                </FooterTemplate>
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn Caption="Description" FieldName="Description" ReadOnly="true">                        
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn Caption="Weight" FieldName="Weight" ReadOnly="true" CellStyle-HorizontalAlign="Left" />
            <dx:GridViewDataTextColumn Caption="Weighted Score" FieldName="WeightedScore" ReadOnly="true" CellStyle-HorizontalAlign="Left" >
            </dx:GridViewDataTextColumn>

        </Columns>
        <SettingsEditing Mode="Batch" BatchEditSettings-ShowConfirmOnLosingChanges="false" /> 
        <Settings ShowStatusBar="Visible" />
        <SettingsCommandButton>
            <ShowAdaptiveDetailButton ButtonType="Button" Styles-Style-CssClass="homeGrid_openBTn"></ShowAdaptiveDetailButton>
            <HideAdaptiveDetailButton ButtonType="Button" Styles-Style-CssClass="homeGrid_closeBTn"></HideAdaptiveDetailButton>
        </SettingsCommandButton>
        <Settings ShowGroupFooter="VisibleAlways" ShowFooter="true" />
        <TotalSummary>
            <%--<dx:ASPxSummaryItem FieldName="Ranking" SummaryType="Custom" Tag="" />--%>
            <dx:ASPxSummaryItem FieldName="Weight" SummaryType="Custom"  />
            <dx:ASPxSummaryItem FieldName="WeightedScore" SummaryType="Custom" />
        </TotalSummary>
        <Templates>
            <StatusBar>
                <div class="leadRanking-btnContainer">
                    <dx:ASPxHyperLink ID="hlSave" runat="server" Text="Save and Refresh Lead" CssClass="leadRanking-btn" ForeColor="White" Font-Underline="false">
                        <ClientSideEvents Click="UpdateCriteria" />
                    </dx:ASPxHyperLink>
                    &nbsp;
                    <dx:ASPxHyperLink ID="hlCancel" runat="server" Text="Undo" CssClass="leadRanking-btn"  ForeColor="White" Font-Underline="false">
                        <ClientSideEvents Click="function(s, e){ grid.CancelEdit(); grid.Refresh(); }" />
                    </dx:ASPxHyperLink>
                    <%--&nbsp;                    
                    <asp:LinkButton ID="btnSave" runat="server" Text="Close and Refresh Lead" OnClick="lnkClose_Click"  CssClass="leadRanking-btn"  ForeColor="White" Font-Underline="false"></asp:LinkButton>--%>
                </div>
            </StatusBar>
        </Templates>
        <Styles>
            <Row HorizontalAlign="Center" CssClass="leadRanking-gridRow CRMstatusGrid_row"></Row>
            <Header Font-Bold="true" HorizontalAlign="Center" CssClass="CRMstatusGrid_headerRow"></Header>
        </Styles>
        <%--<ClientSideEvents BatchEditStartEditing="onStartEditing" />--%>
        <ClientSideEvents FocusedCellChanging="onFocusedCellChanging" />
    </ugit:ASPxGridView>
    <script type="text/javascript">
        try {
            ASPxClientControl.GetControlCollection().ControlsInitialized.AddHandler(function (s, e) {
            UpdateGridHeight();
        });
        ASPxClientControl.GetControlCollection().BrowserWindowResized.AddHandler(function (s, e) {
            UpdateGridHeight();
        });
        } catch (e) {
        }        
    </script>
</div>
<div>&nbsp;</div>
<%--<div class="row leadRanking-btnContainer">
    <asp:LinkButton ID="lnkbtnImport" runat="server" Text="&nbsp;&nbsp;Add Subcontractor&nbsp;&nbsp;" ToolTip="Import Ranking Template" OnClientClick="return OpenImportRankingCriteria()">
        <span class="leadRanking-btn">
            <b>Import Ranking Template </b>
            <i></i>
        </span>
    </asp:LinkButton>
    &nbsp;
    <asp:LinkButton ID="lnkClose" runat="server" Text="&nbsp;&nbsp;Close and Refresh Lead&nbsp;&nbsp;" ToolTip="Close and Refresh Lead" OnClick="lnkClose_Click"> 
        <span class="leadRanking-btn">
            <b>Close and Refresh Lead</b>
            <i></i>
        </span>
    </asp:LinkButton>
</div>--%>

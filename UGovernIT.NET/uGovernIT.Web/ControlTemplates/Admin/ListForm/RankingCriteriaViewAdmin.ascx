<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RankingCriteriaViewAdmin.ascx.cs" Inherits="uGovernIT.Web.RankingCriteriaViewAdmin" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .formatcolor {
        background-color: #f85752;
        color: white;
    }
</style>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function OpenAddCheckList() {
        UgitOpenPopupDialog('<%= absoluteUrlCheckList %>' + '&RankingCriteriaId=0', "", 'Add Ranking Criteria', '600px', '500px', 0, escape("<%= Request.Url.AbsolutePath %>"));
        return false;
    }

    function OpenEditCheckListPopup(objCheckListId) {
        window.parent.UgitOpenPopupDialog('<%= absoluteUrlCheckList %>' + '&CheckListId=' + objCheckListId, "", 'Edit CheckList', '500px', '350px', 0, escape("<%= Request.Url.AbsolutePath %>"));
    }

    function OpneEditRankingCriteria(obj, activityId) {
        UgitOpenPopupDialog('<%= absoluteUrlCheckList %>' + '&RankingCriteriaId=' + activityId, "", 'Edit Ranking Criteria', '600px', '500px', 0, escape("<%= Request.Url.AbsolutePath %>"));
        return false;
    }
</script>

<div class="col-md-12 col-sm-12 col-xs-12" style="margin-top:10px;">
    <div class="row">
        <div class="crm-checkWrap" style="float: right; margin-bottom:10px;">
            <asp:CheckBox ID="chkShowDeleted" Text="Show Deleted&nbsp;&nbsp;" runat="server" TextAlign="Left"
                AutoPostBack="true" OnCheckedChanged="chkShowDeleted_CheckedChanged" />    
        </div>
    </div>
    <div class="row">
            <div id="content">
            <ugit:ASPxGridView ID="grdRankingCriteria" ClientInstanceName="grdRankingCriteria" runat="server" Width="100%" 
                KeyFieldName="ID" EnableViewState="false" AlternatingRowStyle-BackColor="WhiteSmoke"
                AutoGenerateColumns="false" AllowFiltering="true" CssClass="customgridview homeGrid"
                DataKeyNames="ID" GridLines="None" SettingsPager-PageSize="15"
                OnSummaryDisplayText="grdRankingCriteria_SummaryDisplayText">
                <Columns>
                    <dx:GridViewDataTextColumn Name="aEdit" Width="10px">
                        <DataItemTemplate>
                            <asp:ImageButton OnClientClick='<%# string.Format("javascript:return OpneEditRankingCriteria(this, {0})", Eval("ID")) %>'
                                ToolTip="Edit" ID="imgButtonEdit" runat="server" ImageUrl="/Content/images/editNewIcon.png" Style="padding-bottom: 8px; width:16px;" CssClass="crmActivity_editBtn" />
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Ranking Criteria" FieldName="RankingCriteria">
                        <FooterTemplate>
                            <div style="font-weight: bold; align-content: center">
                                Total
                            </div>
                        </FooterTemplate>
                    </dx:GridViewDataTextColumn>

                    <dx:GridViewDataTextColumn Caption="Description" FieldName="Description" />
                    <dx:GridViewDataTextColumn Caption="Ranking" FieldName="Ranking" />
                    <dx:GridViewDataTextColumn Caption="Weight" FieldName="Weight" />
                    <dx:GridViewDataTextColumn Caption="Weighted Score" FieldName="WeightedScore" />

                </Columns>
                <FormatConditions>
                    <dx:GridViewFormatConditionHighlight FieldName="RankingCriteria" Format="Custom" ApplyToRow="true" RowStyle-CssClass="formatcolor" Expression="[Deleted] = True"></dx:GridViewFormatConditionHighlight>
                </FormatConditions>
                <Settings ShowGroupFooter="VisibleAlways" ShowFooter="true" />
                <Styles>
                    <Row CssClass="homeGrid_dataRow"></Row>
                    <Header CssClass="homeGrid_headerColumn"></Header>
                </Styles>
                <TotalSummary>
                    <dx:ASPxSummaryItem FieldName="Ranking" SummaryType="Sum" Tag="" />
                    <dx:ASPxSummaryItem FieldName="Weight" SummaryType="Sum" />
                    <dx:ASPxSummaryItem FieldName="WeightedScore" SummaryType="Sum" />
                </TotalSummary>
            </ugit:ASPxGridView>
        </div>
    </div>
    <div class="row addEditPopup-btnWrap">
        <dx:ASPxButton ID="lnkbtnAddRanking" runat="server" Text="Add Ranking Criteria" ToolTip="Add Ranking Criteria"
            CssClass="primary-blueBtn">
            <ClientSideEvents Click="function(s, e){return OpenAddCheckList()}" />
        </dx:ASPxButton>
    </div>
</div>




<%--<asp:LinkButton  Text="&nbsp;&nbsp;Add Subcontractor&nbsp;&nbsp;"  OnClientClick="">
    <span class="button-bg">
        <b style="float: left; font-weight: normal;">
            Add Ranking Criteria </b>
        <i style="float: left; position: relative; top: -3px;left:2px">                           
        </i> 
    </span>
</asp:LinkButton>--%>
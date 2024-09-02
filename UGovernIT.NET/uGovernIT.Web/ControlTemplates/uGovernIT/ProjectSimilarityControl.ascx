<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProjectSimilarityControl.ascx.cs" Inherits="uGovernIT.Web.ProjectSimilarityControl" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .rotate {
        /* Safari */
        -webkit-transform: rotate(-76deg);
        /* Firefox */
        -moz-transform: rotate(-76deg);
        /* IE */
        -ms-transform: rotate(-76deg);
        /* Opera */
        -o-transform: rotate(-76deg);
        /* Internet Explorer */
        filter: progid:DXImageTransform.Microsoft.BasicImage(rotation=3);
        height: 180px;
        width: 40px;
    }
    .firstcolheader {
    width:160px !important;
    }

    .label {
        position: absolute;
        left: -70px;
        top: 83px;
        width:180px;
        white-space: pre-wrap;        
        text-align:left;
        color: #000000 !important;
        font-size: 11px;
    }
    .cellheight {
    height:28px;
    }

    .ms-alternatingstrong {
        background-color: #fbfbfb;
    }
    .type_buttons{
        padding: 3px;
        margin: 5px 0px 2px 0px !important;
    }
    .score-cell {
        border: 4px solid white !important;
    }
    .clear-border {
        border: 0px !important;
    }
</style>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function OnShowScoreChange(obj) {

        if ($(obj).find('input').get(0).checked) {
            $(".score").each(function (i, item) {
                $(item).css('color', '#201f35');
            });
        }
        else {
            HideScore();
        }
    }

    function HideScore() {
        $(".score").each(function (i, item) {
            $(item).css('color', $(item).css('background-color'));
        });
    }

    $(function () {
        adjustControlSize();
    });

    function adjustControlSize() {

        setTimeout(function () {
            $("#s4-workspace").width("100%");
            grid.AdjustControl();
            grid.SetWidth($(window).width() - 30);
            var height = $(window).height();
            grid.SetHeight(height - 70);
        }, 10);
    }
</script>
<table style="width: 100%;">
     <tr>
        <td>
            <asp:CheckBox ID="chkShowScore" runat="server" Text="Show Comparison Scores" onchange="OnShowScoreChange(this);" />
        </td>
    </tr>
    <tr>
        <td>
            <dx:ASPxGridView ID="grid" runat="server" AutoGenerateColumns="False" SettingsText-CommandClearFilter=""
                OnHtmlDataCellPrepared="grid_HtmlDataCellPrepared"
                ClientInstanceName="grid"  KeyFieldName="TicketId">
                <Columns>
                </Columns>
                <Settings ShowFilterRow="false" HorizontalScrollBarMode="Auto"   VerticalScrollBarMode="Auto"/>
                <SettingsBehavior EnableRowHotTrack="false" AllowSelectByRowClick="false" AllowSelectSingleRowOnly="true" AllowSort="false" />
                <SettingsPopup>
                    <HeaderFilter Height="200" />
                </SettingsPopup>
                <SettingsPager Position="TopAndBottom" Mode="ShowAllRecords" >
                    <PageSizeItemSettings ShowAllItem="true" Items="10, 15, 20, 25, 50, 75, 100" />
                </SettingsPager>

            </dx:ASPxGridView>

            <dx:ASPxPanel ID="buttonPanel" runat="server" Width="100%" CssClass="type_buttons"></dx:ASPxPanel>
        </td>
    </tr>
</table>
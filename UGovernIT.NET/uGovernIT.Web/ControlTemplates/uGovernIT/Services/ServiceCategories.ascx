
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ServiceCategories.ascx.cs" Inherits="uGovernIT.Web.ServiceCategories" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
    .ms-listviewtable {
        border: 1px solid #DCDCDC !important;
        border-collapse: separate !important;
        background: #F8F8F8 !important;
    }

        .ms-listviewtable > tbody > tr > th {
        }

        .ms-listviewtable > tbody > tr > td {
            border-bottom: 2px solid #fff !important;
        }


    .ms-viewheadertr th {
        text-align: left;
        font-weight: bold;
    }

    .ms-listviewtable tr.ticketgrouptr {
        background: white;
        font-weight: bold;
    }

    .buttonalg-left {
        float: left;
        margin-top: 5px;
    }

    .buttonalg {
        float: right;
        margin-top: 5px;
    }

    .alertnate {
        background-color: white;
    }
.statusBar a:first-child
{
   display: none;
}
</style>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">

    function editCategory(isNew, cID) {
        var title = "Edit Category";
        if (isNew) {
            title = "New Category";
        }
        var params = "categoryID=" + cID;
        UgitOpenPopupDialog('<%= editServiceCategoryUrl %>', params, title, '90', '90', 0, escape("<%= Request.Url.AbsoluteUri %>" + "/"));
    }

    function reOrderCategory(obj, currentIndex, total) {
        var sectionArray = new Array();
        var sectionOrderBoxs = $(".section-order");
        $.each(sectionOrderBoxs, function (i, item) {
            sectionArray.push($(item).val());
        });
        var undefinedNumber = 0;
        for (var i = 1; i <= total; i++) {
            if ($.inArray(i.toString(), sectionArray) == -1) {
                undefinedNumber = i;
                break;
            }
        }

        var currentVal = parseInt($(obj).val());
        for (var i = 0; i < total; i++) {
            var itrVal = parseInt($(sectionOrderBoxs[i]).val());
            if (i != currentIndex && itrVal == currentVal) {
                $(sectionOrderBoxs[i]).val(undefinedNumber)
            }
        }
    }
    function rCategories_BatchEditEndEditing(s, e)
    {
        var pageRowCount = s.pageRowCount;
        var oldValue = s.batchEditApi.GetCellValue(e.visibleIndex, "ItemOrder");
        var currentValue = e.itemValues[1].value; 
        for (var i = 0; i < pageRowCount; i++)
        {
            if (i == e.visibleIndex)
                continue;
            var val = s.batchEditApi.GetCellValue(i, "ItemOrder");
            if (val == currentValue)
            {
                s.batchEditApi.SetCellValue(i, "ItemOrder", oldValue);
            }
        }
    }
</script>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function OnBatchEditStartEditing(s, e) {
        ToggleButtons(true);
    }
    function OnBatchEditEndEditing(s, e) {
        window.setTimeout(function () {
            if (!s.batchEditApi.HasChanges())
                ToggleButtons(false);
        }, 0);
    }
    function ToggleButtons(enabled) {
        btnUpdate.SetEnabled(enabled);
        btnCancel.SetEnabled(enabled);
    }
    function OnUpdateClick(s, e) {
        rCategories.UpdateEdit();
        ToggleButtons(false);
    }
    function OnCancelClick(s, e) {
        rCategories.CancelEdit();
        ToggleButtons(false);
    }
    function OnCustomButtonClick(s, e) {
        if (e.buttonID == "deleteButton") {
            s.DeleteRow(e.visibleIndex);
            ToggleButtons(true);
        }
    }
    function OnEndCallback(s, e) {
        window.setTimeout(function () {
            if (!s.batchEditApi.HasChanges())
                ToggleButtons(false);
        }, 0);
    }
</script>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function UpdateGridHeight() {
        rCategories.SetHeight(0);
        var containerHeight = ASPxClientUtils.GetDocumentClientHeight();
        if (document.body.scrollHeight > containerHeight)
            containerHeight = document.body.scrollHeight;
        rCategories.SetHeight(containerHeight);
    }
    window.addEventListener('resize', function (evt) {
        if (!ASPxClientUtils.androidPlatform)
            return;
        var activeElement = document.activeElement;
        if (activeElement && (activeElement.tagName === "INPUT" || activeElement.tagName === "TEXTAREA") && activeElement.scrollIntoViewIfNeeded)
            window.setTimeout(function () { activeElement.scrollIntoViewIfNeeded(); }, 0);
    });
</script>

<div class="col-md-12 col-sm-12 col-xs-12 formLayout-addPopupContainer">
    <div class="row">
        <ugit:ASPxGridView ID="rCategories" OnBatchUpdate="rCategories_BatchUpdate" runat="server"  OnHtmlRowPrepared="rCategories_HtmlRowPrepared" KeyFieldName="ID"
            Width="100%" EnableViewState="true" AllowGrouping="true" AutoGenerateColumns="false" CssClass="customgridview homeGrid"  
            Styles-StatusBar-CssClass="statusBar" ClientInstanceName="rCategories">
            <ClientSideEvents BatchEditEndEditing="OnBatchEditEndEditing" BatchEditStartEditing="OnBatchEditStartEditing"  CustomButtonClick="OnCustomButtonClick" 
                EndCallback="OnEndCallback"/>
            <settingsadaptivity adaptivitymode="HideDataCells" allowonlyoneadaptivedetailexpanded="true" ></settingsadaptivity>
            <Columns>
                <dx:GridViewDataTextColumn Name="aEdit" Width="2%">
                <DataItemTemplate>
                    <a id="editLink" runat="server" onclick='<%# string.Format("editCategory(false,{0})", Eval("ID")) %>' >
                        <img id="Imgedit1" runat="server" src="~/Content/Images/editNewIcon.png" width="16" />
                    </a>
                </DataItemTemplate>
                </dx:GridViewDataTextColumn>
               <dx:GridViewDataComboBoxColumn FieldName="ItemOrder"  Width="3%">

               </dx:GridViewDataComboBoxColumn>
            
                <dx:GridViewDataColumn FieldName="CategoryName">
                    <DataItemTemplate>
                         <a id="editLink" runat="server" onclick='<%# string.Format("editCategory(false,{0})", Eval("ID")) %>'><%# Eval("CategoryName") %></a>
                    </DataItemTemplate>
                </dx:GridViewDataColumn>
            </Columns>
            <settingscommandbutton>
                <ShowAdaptiveDetailButton ButtonType="Button"   Styles-Style-CssClass="homeGrid_openBTn"></ShowAdaptiveDetailButton>
                <HideAdaptiveDetailButton ButtonType="Button"  Styles-Style-CssClass="homeGrid_closeBTn"></HideAdaptiveDetailButton>
            </settingscommandbutton>
            <Styles>
                <Row CssClass="homeGrid_dataRow"></Row>
                <Header CssClass="homeGrid_headerColumn" Font-Bold="true"></Header>
            </Styles>
            <SettingsEditing Mode="Batch">
                <BatchEditSettings EditMode="Cell" />
            </SettingsEditing>
            <SettingsEditing Mode="Batch" />
            <Settings ShowStatusBar="Hidden" />
            <Settings ShowHeaderFilterButton="true" />
            <SettingsPager Mode="ShowAllRecords" ></SettingsPager>
            <ClientSideEvents BatchEditEndEditing="rCategories_BatchEditEndEditing" />
        </ugit:ASPxGridView>
        <script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
            ASPxClientControl.GetControlCollection().ControlsInitialized.AddHandler(function (s, e) {
                UpdateGridHeight();
            });
            ASPxClientControl.GetControlCollection().BrowserWindowResized.AddHandler(function (s, e) {
                UpdateGridHeight();
            });
        </script>
    </div>
    <div class="row mappingTab-btnWrap">
        <div class="col-md-12 col-sm-12 col-xs-12 noPadding">
            <div style="float: left;text-align:left;">
                <dx:ASPxButton id="editCategory" CssClass="primary-blueBtn" runat="server" Image-Url="/Content/images/Puzzle.png" Text="New Category" AutoPostBack="false">
                    <ClientSideEvents Click="function(){ editCategory(true, 0); }" />
                </dx:ASPxButton>
            </div>
            <div style="float: right;">
                 <dx:ASPxButton ID="btnCancel" runat="server" CssClass="secondary-cancelBtn" Text="Cancel Changes" ClientInstanceName="btnCancel" AutoPostBack="false" ClientEnabled="false">
                    <ClientSideEvents Click="OnCancelClick" />
                </dx:ASPxButton>
                <dx:ASPxButton ID="btnUpdate" runat="server" CssClass="primary-blueBtn" Text="Save Changes" ClientInstanceName="btnUpdate" AutoPostBack="false" ClientEnabled="false">
                        <ClientSideEvents Click="OnUpdateClick" />
                </dx:ASPxButton>
            </div>
        </div>
    </div>
</div>


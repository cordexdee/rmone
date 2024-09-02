<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReportCardView.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.Shared.ReportCardView" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<%--<link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.4.1/css/bootstrap.min.css" integrity="sha384-Vkoo8x4CGsO3+Hhxv8T/Q5PaXtkKtu6ug5TOeNV6gBiFeWPGFN9MuhOf23Q9Ifjh" crossorigin="anonymous">
<script src="https://stackpath.bootstrapcdn.com/bootstrap/4.4.1/js/bootstrap.min.js" integrity="sha384-wfSDF2E50Y2D1uUdj0O3uMBJnjuUD4Ih7YwaYd1iqfktj0Uod8GCExl3Og8ifwB6" crossorigin="anonymous"></script>--%>

<script data-v="<%=UGITUtility.AssemblyVersion %>">
    //debugger;
    var selectedCategory = '<%=selectedCategory%>';
    var selectedCategoryShortName = '';
    var selectedReport = '';
    var selectedReportType = '';

    function ReloadPage() {
        drawTiles();
    }

    function GetSelectedQueryReport(obj) {
        selectedReport = $("#<%=ddlQueryList.ClientID %> option:selected").val();
        drawTiles();
    }

    function setSelectedCategory(selectedCategoryVal, selectedCategoryShortNameVal, elem) {
        $(".addOpacity").each(function () {
            $(this).addClass("opacity-1");
        });
        elem.removeClass("opacity-1");
        selectedCategory = selectedCategoryVal;
        selectedReport = '';
        selectedReportType = '';
        selectedCategoryShortName = selectedCategoryShortNameVal;
        drawTiles();
    }
</script>

<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .reportListTiles div.dx-tileview-wrapper div.dx-item{
            border: 1px solid #CCC;
            padding: 10px 0;
            border-radius: 10px;
            cursor:pointer;
    }
    .reportListTiles {
        height:auto !important;
    }
    .report-ImgWrap{
        text-align: center;
        width: 100%;
        min-height: 47%;
    }
    .reportListTiles div.dx-tileview-wrapper div.dx-item:hover{
        transform: translateY(-0.05em) scale(1.05);
        transition: 0.3s all cubic-bezier(0.055, 0.62, 0.145, 0.5);
    }
    .card-title{
        font-family: 'Poppins', sans-serif !important;
        font-size: 16px;
        text-align: center;
        color: #4A6EE2;
        width:100%;
        padding:0 5px;
    }
    .wilkiInfo-container {
        font-family: 'Poppins', sans-serif !important;
        font-size: 13px;
        text-align: center;
    }
    .wikiTitle-container {
        padding-top: 15px;
    }
    .reportImage {
        width: 180px;
        height: 175px;
        padding: 68px;
        position: absolute;
        cursor: pointer;
    }
    .reportText {
        padding-top: 114px;
    font-size: 15px;
    line-height: 15px;
    width: 120px;
    text-align: center;
    color: white;
    font-weight: 500;
    position: absolute;
    margin-left: 30px;
    }
    .folderIcon {
        font-size: 180px;
        color: orange;
    }
    .d-flex-wrap {
    flex-wrap: wrap;
    }
   /* .dx-scrollview-content {
    border: 2px solid gray;
    border-radius: 10px;
    }*/
    .itemOuterBorderStyle {
        width: 170px;
        border: 2px solid lightgray;
        height: 150px;
        border-radius: 10px;
    }
    .itemReportImage {
        width: 120px;
        height: 120px;
        margin-left: 25px;
        margin-top: 15px;
        padding-left: 30px;
        padding-bottom: 60px;
        padding-right: 30px;
        padding-top: 10px;
    }
    .itemReportTextTitle {
        padding-top: 85px;
        font-size: 15px;
        width: 170px;
        text-align: center;
        color: black;
        font-weight: 500;
        position: absolute;
        z-index:101;
    }
    .itemReportText {
        padding-top: 110px;
        font-size: 13px;
        width: 170px;
        text-align: center;
        color: black;
        font-weight: 500;
        position: absolute;
    }
    .dx-tile {  
        border: none;  
    }
    .crm-checkWrap input:checked + label::after {
        top: 4px;
        border: solid black;
        border-width: 0 2px 2px 0;
    }
    .crm-checkWrap label::before {
        border: 2px solid black;
    }
    .crm-checkWrap label {
        color: black;
    }
    .reportField-label {
        font-size: 14px;
        color: black;
        font-weight: 500;
    }
    .opacity-1 {
        opacity:0.7;
    }
    a:visited {
        color: white;
        text-decoration: none;
    }
    </style>


<div class="col-md-12 col-sm-12 col-xs-12 noPadding">
    <div class="col-md-12 col-xs-12 col-sm-12" id="filterTable" runat="server" style="padding-left:20px;padding-top:15px">
        <div class="row">
            <div class="col-md-3 col-sm-3 col-xs-12" style="display:none;">
                <div class="reportField-label">Category:</div>
                <div class="reportField-field">
                    <asp:DropDownList ID="ddlCategoryList" class="dropdown" AutoPostBack="true" runat="server"
                     OnSelectedIndexChanged="DdlCategoryList_OnSelectedIndexChanged" CssClass="reportDropDown-list aspxDropDownList"/>
                </div>
            </div>
            <div class="col-md-3 col-sm-3 col-xs-12" style="display:none;">
                <div class="reportField-label">Report:</div>
                <div class="reportField-field">
                    <asp:DropDownList ID="ddlQueryList" runat="server" class="dropdown" CssClass="reportDropDown-list aspxDropDownList" onchange="GetSelectedQueryReport()" />
                </div>
            </div>
            <div class="col-md-3 col-sm-3 col-xs-12" >
                <div class="reportField-label">Search:</div>
                <div class="reportField-field">
                    <div id="reportsSearch"></div>
                </div>
            </div>
            <div class="col-md-9 col-sm-9 col-xs-12" style="display:none" >
                <a href="/Pages/QueryReports" class="btn btn-custom" style="float:right;margin-top:20px;">
                    <img src="/Content/Images/plus-white.png" height="14" />
                    New Query</a>
            </div>
             <div class="col-md-2 col-sm-2 col-xs-12" style="padding-top:35px;display:none;">
                <div class="reportField-field crm-checkWrap">
                    <asp:CheckBox ID="chkShowCustomReports" runat="server" Text="Show Custom Reports" 
                        Checked="true" onclick="ReloadPage()" TextAlign="right" />
                </div>
            </div>
            <%-- <div class="col-md-3 col-sm-3 col-xs-12">
                <div class="reportBtn-wrap">
                    <input type="button" class="reportBtn-run" id="btRunReport" value="Run" onclick="GetSelectedQueryReport();" />
                    <asp:HiddenField ID="hdnbuttonClick" runat="server" />
                </div>
            </div>--%>
        </div>
        <div class="d-flex d-flex-wrap">
            <% foreach(var item in SubCategoriesTuple.OrderBy(x => x.Item4)) { %>
            <a class="d-flex m-2 opacity-1 addOpacity" href="#" onclick="setSelectedCategory('<%=item.Item1%>', '<%=item.Item1%>', $(this))">
                <img src="<%=item.Item2%>" class="reportImage">
                <i class="material-icons folderIcon" <%= "style=color:" + item.Item3 %>>&#xe2c7;</i>
                <div class="reportText"><%=item.Item1%></div>
            </a>
            <%} %>
        </div>
    </div>

    <div class="reportListTiles col-md-12 col-sm-12 m-2"></div> 
    <%--<div class="helpCard-listWrap fleft">
        <div class="wiki-topDiv row">
        </div>
        <br />
        <div class="wiki-btnDiv">
            <div id="btnloadMore" class="wiki-loadMoreBtn"></div>            
        </div>
    </div>--%>
</div>

<script data-v="<%=UGITUtility.AssemblyVersion %>">
    var gridDataSource = {};
    var dashBoardTable = <%=Newtonsoft.Json.JsonConvert.SerializeObject(AllReports.OrderBy(x => x.Item2))%>;
    $(function () {
        drawTiles();
    });

    function drawTiles() {
         gridDataSource = new DevExpress.data.DataSource({
             key: "ID",
             load: function (loadOptions) {
                 return $.getJSON("/api/ReportCard/GetAllReports?&SelectedReport=" + selectedReport + "&SelectedCategory=" + selectedCategory + "&SelectedReportType=" + selectedReportType, function (data) {
                     /*console.log(data);*/
                      return data;
                    });
                },
                insert: function (values) {

                },
                remove: function (key) {

                },
                update: function (key, values) {
                    
                }
            });

        var HelpCardTilesConfig =
            $(".reportListTiles").dxTileView
           ({          
                    noDataText: "No cards available",
                    dataSource: gridDataSource,
                    direction: "vertical",
                    height: 400,
                    baseItemHeight: 160,
                    baseItemWidth: 170,
                    itemTemplate: function (itemData, itemIndex, itemElement) {
                        var html = new Array();                         
                        html.push("<div class='d-flex'>"); 
                        html.push("<img src=" + itemData.ImageUrl + " class='itemReportImage'></img>");
                                    if (itemData.LongTitle != null)
                                        html.push("<div class='itemReportTextTitle card-title' title='" + itemData.LongTitle +"'>");
                                    else
                                        html.push("<div class='itemReportTextTitle card-title'>");
                                    html.push(itemData.Title);
                        html.push("</div>");
                        html.push("<div class='itemReportText'>");
                        if (itemData.ModuleShortName == null) {
                            html.push(selectedCategoryShortName);
                        }
                        else {
                            html.push(itemData.ModuleShortName);
                        }
                        html.push("</div>");
                        html.push("</div>");
                        itemElement.append(html.join(""));
                    },
                    onContentReady: function (obj) {                                   
                    },
                    onItemClick: function (obj)
                    {
                        if (obj != null && obj.itemData != null)
                        {
                            if (obj.itemData.LongTitle != null)
                                ShowReportViewer(obj.itemData.ModuleNameLookup, obj.itemData.Name, obj.itemData.LongTitle, obj.itemData.RouteUrl);
                            else
                                ShowReportViewer(obj.itemData.ModuleNameLookup, obj.itemData.Name, obj.itemData.Title, obj.itemData.RouteUrl);
                        }
                     }
           }).dxTileView('instance');
        
    }


    function ShowReportViewer(moduleName, reportType, reportTitle,routeUrl) {
        var reportUrl = '<%=reportUrl%>';
        var winheight = '90';
        var winWidth = '90';
         //document.getElementById("dvCLJR").style.display = "none";
        if (moduleName == 'CPR' && reportType == 'TicketSummary')
            reportTitle = 'Project Summary'
        if (reportType == 'ResourceAllocationReport')
        {
            winheight = '400px';
            winWidth = '800px';
        }
        var popupHeader = reportTitle;
        var params = "";
        var prefix = "Viewer";        
        var url = reportUrl + "?reportName=" + reportType + "&Module=" + moduleName + "&title=" + reportTitle + "&userId=<%=UserId%>";
        if (routeUrl != null && routeUrl != '' && routeUrl != undefined) {
            url = routeUrl + "&reportName=" + reportType + "&Module=" + moduleName + "&title=" + reportTitle + "&userId=<%=UserId%>";
        }
        window.parent.UgitOpenPopupDialog(url, params, popupHeader, winWidth, winheight, 0, escape("<%= Request.Url.AbsolutePath %>"));
    }
    $(function () {
        $("#reportsSearch").dxSelectBox({
            dataSource: new DevExpress.data.DataSource({
                store: dashBoardTable,
                select: function (itemData) {
                    itemData.preparedName = prepareString(itemData.Item2)
                    return itemData
                }
            }),
            displayExpr: "Item2",
            placeholder: "Search Report",
            searchEnabled: true,
            searchMode: "contains",
            searchExpr: ['Item2', 'preparedName'],
            onValueChanged(e) {
                if (e.value != null && e.value.Item1 == "0") {
                    selectedReport = '';
                    selectedReportType = '';
                    selectedCategory = '';
                }
                else if (e.value != null) {
                    selectedReport = e.value.Item1;
                    selectedReportType = e.value.Item3;
                }
                else {
                    selectedReport = '';
                    selectedReportType = '';
                    selectedCategory = '';
                }
                $(".addOpacity").each(function () {
                    $(this).addClass("opacity-1");
                });
                drawTiles();
            },
        })

    });

    function prepareString(string) {
        if (!string) { return string }
        return string.normalize('NFD').replace(/[\u0300-\u036f]/g, "").toLowerCase();
    }
</script>
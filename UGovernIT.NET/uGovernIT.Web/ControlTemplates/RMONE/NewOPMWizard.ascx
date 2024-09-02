<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NewOPMWizard.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.RMONE.NewOPMWizard" %>

<style>
    .container {
        width: 100%;
        padding:0px;
    }

    .padding {
        padding: 5px;
    }

    .tile {
        border-radius: .5em;
        text-align: center;
        color: black;
        /*background: #dddddd;*/
    }

    .ticket-stage {
        font-size: 20px;
        font-weight: 600;
    }

    .rcorners2 {
        border-radius: 7px;
        border: 1px solid #D3D3D3;
        padding: 7px;
        width: 200px;
        height: 150px;
    }

    .whiteSpace-1 {
        font-weight: 500;
        font-size: 13px;
        margin: 6px 0px;
        color: black;
    }

    .ticket-stage {
        font-size: 20px;
        font-weight: 600;
    }

    .boxAlignClosedOut {
        display: flex;
        flex-direction: column;
        flex-wrap: wrap;
        align-items: center;
        align-content: flex-start;
    }

    .boxAlignPrecon {
        display: flex;
        flex-direction: column;
        flex-wrap: wrap;
        align-items: center;
        align-content: flex-end;
    }

    .boxAlignConst {
        display: flex;
        flex-direction: column;
        flex-wrap: wrap;
        align-items: center;
        align-content: center;
    }

    /*.tileViewContainer .dx-tile {
        text-align: center;
        padding: 2px;
        border-radius: 7px;
        background: lightgray;
    }*/

    .dx-tile-content {
        height: 100%;
    }

    .titleHeader {
        font-size: 16px;
        font-weight: 600;
    }

    #PreconEndDateCss {
        padding-left: 3px;
    }

    #ConstEndDateCss {
        padding-left: 3px;
    }

    #CloseoutEndDateCss {
        padding-left: 3px;
    }

    .button_row{
        display:flex;direction:rtl;
    }

    .similarprojectrow{
        display:flex;
        direction:ltr;
    }

    .dx-tile.itemSelected {
        border-color:darkgray;
        /*border-width:2px;*/
        border-radius:.5em;
    }
    #similarProjectList{
        padding-left:0px;
        max-width:90%;
    }
    .tileTitle {
        font-size: larger;
        height: 44px;
        text-align: center;
        padding: 1px;
        padding-top:3px;
    }
        #form .dx-layout-manager .dx-label-h-align.dx-flex-layout {
    display: -webkit-box;
    display: -ms-flexbox;
    display: flex;
    flex-direction: column;
}
    #form .dx-form-group-with-caption > .dx-form-group-content {
    padding-top: 0px !important;
    margin-top: 0px !important;
    border-top: 0px solid #ddd !important;
    padding-bottom: 15px !important;
}

    .rowTitle {
        font-weight: 500;
        color: black;
    }
</style>
<script type="text/javascript" data-v="<%=uGovernIT.Utility.UGITUtility.AssemblyVersion %>">
    var HeaderTitle = "";
    var ajaxHelperPage = "<%=ajaxHelperPage%>";
    var ContactList = [];
    var allprojects = [];
    var dataModel = {
        preconStartDate: "",
        preconEndDate: "",
        constStartDate: "",
        constEndDate: "",
        closeoutStartDate: "",
        closeoutEndDate: "",
        onHold: false,
        preconDuration: "",
        constDuration: "",
        closeOutDuration: ""
    };
    var Model = {
        RecordId: "",
        Fields: [{
            FieldName: "<%=uGovernIT.Utility.DatabaseObjects.Columns.PreconStartDate%>",
            Value: dataModel.preconStartDate
        }, {
                FieldName: "<%=uGovernIT.Utility.DatabaseObjects.Columns.PreconEndDate%>",
                Value: dataModel.preconEndDate
            }, {
                FieldName: "<%=uGovernIT.Utility.DatabaseObjects.Columns.EstimatedConstructionStart%>",
                Value: dataModel.constStartDate
            }, {
                FieldName: "<%=uGovernIT.Utility.DatabaseObjects.Columns.EstimatedConstructionEnd%>",
                Value: dataModel.constEndDate
            }, {
                FieldName: "<%=uGovernIT.Utility.DatabaseObjects.Columns.CloseoutDate%>",
                Value: dataModel.closeoutEndDate
            }, {
                FieldName: "<%=uGovernIT.Utility.DatabaseObjects.Columns.TicketOnHold%>",
                Value: dataModel.onHold
            }]
    };
    var opmData = [];
    //var opmData = [{ "ID": 30275, "Title": "555 Mission Street 2nd Floor Amenity Space (San Francisco, CA)", "LeadStatus": null, "Status": null }, { "ID": 30305, "Title": "AIG - Regional Office (Woodland Hills, CA)", "LeadStatus": null, "Status": null }, { "ID": 30374, "Title": "Paul Weiss (San Francisco, CA)", "LeadStatus": null, "Status": null }, { "ID": 30382, "Title": "Fidelity Investor Center – Walnut Creek, CA Renovation (Walnut Creek, CA)", "LeadStatus": null, "Status": null }, { "ID": 30392, "Title": "SISTER (West Hollywood, LA)", "LeadStatus": null, "Status": null }, { "ID": 30396, "Title": "UCSF Parnassus Garage Clean/Stripe (San Francisco, CA)", "LeadStatus": null, "Status": null }, { "ID": 30412, "Title": "UCSF RNEW MTZ CRC VAV Ductwork Project (San Francisco, CA)", "LeadStatus": null, "Status": null }, { "ID": 30420, "Title": "342 Howard Street Exterior (San Francisco, CA)", "LeadStatus": null, "Status": null }, { "ID": 30427, "Title": "Simpson Thacher - Office TI (Palo Alto, CA)", "LeadStatus": null, "Status": null }, { "ID": 30480, "Title": "zScaler - studio build - (San Jose, CA)", "LeadStatus": null, "Status": null }];
    var workItemData = [{ "ID": 4390, "Title": "Goldman Sachs SF", "LeadStatus": null, "Status": "Cultivate" }, { "ID": 4565, "Title": "Slack", "LeadStatus": null, "Status": "Cultivate" }, { "ID": 4561, "Title": "Skadden", "LeadStatus": null, "Status": "Cultivate" }, { "ID": 4588, "Title": "DLA Piper", "LeadStatus": null, "Status": "Cultivate" }, { "ID": 4590, "Title": "Post Montgomery Center - CapEx Projects", "LeadStatus": null, "Status": "Cultivate" }, { "ID": 4596, "Title": "Northpond Ventures", "LeadStatus": null, "Status": "Cultivate" }, { "ID": 4622, "Title": "Autodesk ", "LeadStatus": null, "Status": "Assign" }, { "ID": 4624, "Title": "Pantheon ", "LeadStatus": null, "Status": "Assign" }, { "ID": 4625, "Title": "Google 188 Embarcadero", "LeadStatus": null, "Status": "Assign" }, { "ID": 4634, "Title": "Bristoll Myers Squibb - Redwood City Campus Projects", "LeadStatus": null, "Status": "Assign" }, { "ID": 4635, "Title": "CIM - 330 Townsend ", "LeadStatus": null, "Status": "Assign" }, { "ID": 4637, "Title": "Gunderson - Austin, Texas", "LeadStatus": null, "Status": "Assign" }, { "ID": 4638, "Title": "Gunderson - Redwood City", "LeadStatus": null, "Status": "Assign" }, { "ID": 4628, "Title": "88 Kearny - 6th Flr Roof Replacement(aka 66 Kearny)", "LeadStatus": null, "Status": "Assign" }, { "ID": 4652, "Title": "Hogan Lovells - San Francisco", "LeadStatus": null, "Status": "Assign" }, { "ID": 4641, "Title": "Sidley  Austin - Los Angeles", "LeadStatus": null, "Status": "Assign" }, { "ID": 4642, "Title": "Marsh & McLennan Irvine", "LeadStatus": null, "Status": "Assign" }, { "ID": 4643, "Title": "Clark Hill", "LeadStatus": null, "Status": "Assign" }, { "ID": 4659, "Title": "Wharton - 2 Harrison", "LeadStatus": null, "Status": "Cultivate" }, { "ID": 4645, "Title": "Stanford Health Care - 1000 Welch Rd TI", "LeadStatus": null, "Status": "Assign" }, { "ID": 4647, "Title": "Spear Street Capital ", "LeadStatus": null, "Status": "Assign" }, { "ID": 4663, "Title": "MMC - San Jose", "LeadStatus": null, "Status": "Assign" }, { "ID": 4664, "Title": "Greenberg Taurig", "LeadStatus": null, "Status": "Assign" }, { "ID": 4650, "Title": "Cooley - Seattle", "LeadStatus": null, "Status": "Assign" }, { "ID": 4651, "Title": "Google - Hills Plaza(2022)", "LeadStatus": null, "Status": "Assign" }, { "ID": 4653, "Title": "Pony.ai(Confidential) - Fremont / East Bay", "LeadStatus": null, "Status": "Assign" }, { "ID": 4640, "Title": "Deloitte - Sacramento", "LeadStatus": null, "Status": "Assign" }, { "ID": 4656, "Title": "Fremont Group - SF", "LeadStatus": null, "Status": "Assign" }, { "ID": 4658, "Title": "Roche Babois", "LeadStatus": null, "Status": "Assign" }, { "ID": 4660, "Title": "Yelp", "LeadStatus": null, "Status": "Assign" }, { "ID": 4662, "Title": "Gunderson - Boston", "LeadStatus": null, "Status": "Assign" }, { "ID": 4665, "Title": "Open Text", "LeadStatus": null, "Status": "Assign" }, { "ID": 4666, "Title": "kinder's ", "LeadStatus": null, "Status": "Assign" }, { "ID": 4655, "Title": "Twitter Oakland", "LeadStatus": null, "Status": "Assign" }];
    //var similarProjectData = [{ "Title": "T1-Large", "Duration": 84, "Resources": 10, "Hrs": "10,000" },
    //    { "Title": "Google HQ T1", "Duration": 92, "Resources": 12, "Hrs": "15,000" },
    //    { "Title": "Stanford Lab", "Duration": 62, "Resources": 11, "Hrs": "10,000" },
    //    { "Title": "Google HQ T1", "Duration": 92, "Resources": 12, "Hrs": "15,000" },
    //    { "Title": "Stanford Lab", "Duration": 62, "Resources": 11, "Hrs": "10,000" },
    //    { "Title": "Google HQ T1", "Duration": 92, "Resources": 12, "Hrs": "15,000" },
    //    { "Title": "Stanford Lab", "Duration": 62, "Resources": 11, "Hrs": "10,000" },
    //]
    var similarProjectData = [];
    var selectValues = ["Advanced Lead(LED)", "New Opportunity", "Linked Opportunity"];
    var allocationData = [];
    var CreateOPMPostData = {
        SourceTicketId: "",
        SourceModule: "",
        AllocationType: "",
        TemplateID: "",
        ProjectStartDate: "",
        ProjectEndDate: "",
        Allocations: []
    };

    var HideAllocationTemplate = "<%=HideAllocationTemplate%>" == "True" ? true : false; 
    $(function () {
        $("#toast").dxToast({
            message: "Allocations Saved Successfully. ",
            type: "info",
            displayTime: 1000,
            position: "{ my: 'center', at: 'center', of: window }"
        });

        $("#toastBlankAllocation").dxToast({
            message: "Overlapping allocations are not permitted. Save unsuccessful.",
            type: "info",
            displayTime: 600,
            position: "{ my: 'center', at: 'center', of: window }"
        });

        $.get("/api/OPMWizard/GetClientContactDetails", function (data) {

            if (data) {
                ContactList = data;
                getDetails();
            }
        });

        $("#loadpanel").dxLoadPanel({
            message: "Loading...",
            visible: false,
            hideOnOutsideClick: false
        });

        LoadInitialScreen("<%=selectionMode%>");
        var openGridBtn = $("#openGridBtn").dxButton({
            text: 'More...',
            stylingMode: 'text',
            type: 'default',
            onClick: function (e) {
                const similarProjectPopup = $("#popupContainer").dxPopup({
                    title: "All Similar Projects",
                    width: "95%",
                    height: "90%",
                    visible: true,
                    scrolling: true,
                    dragEnabled: true,
                    resizeEnabled: true,
                    contentTemplate: function (contentElement) {
                        contentElement.append(
                            $("<div id='griddisplayalldata' />").dxDataGrid({
                                dataSource: allprojects,
                                keyExpr: 'TicketId',
                                height: "99%",
                                selection: {
                                    mode: 'single',
                                },
                                scrolling: {
                                    mode: 'Standard',
                                },
                                showBorders: true,
                                columns: [
                                    {
                                        dataField: "TicketId",
                                        caption: "Ticket ID",
                                        width: "10%"
                                    },
                                    {
                                        dataField: 'Title',
                                        caption: 'Title',
                                    },
                                    {
                                        dataField: 'Duration',
                                        caption: 'Duration',
                                    },
                                    {
                                        dataField: 'Resources',
                                        caption: 'Resources',
                                    },
                                    {
                                        dataField: 'Hrs',
                                        caption: 'Hours',
                                    },
                                    {
                                        dataField: 'Score',
                                        caption: 'Similarity Score',
                                    }
                                ],
                                onSelectionChanged(selectedItems) {

                                    similarProjectData.forEach(item => item.isSelected = false);
                                    const data = selectedItems.selectedRowsData[0];
                                    data.isSelected = true;
                                    similarProjectData.unshift(data);// Adding data at the first position
                                    var similarProjectTiles = $("#similarProjectList").dxTileView("instance");
                                    similarProjectTiles.option("dataSource", similarProjectData);
                                    //similarProjectTiles.option("height", "240px");
                                    similarProjectTiles.repaint();
                                    $("#popupContainer").dxPopup("instance").hide();
                                    $("#divAllocation").css("display", "block");
                                    
                                    if (dataModel.preconStartDate == '' || dataModel.preconEndDate == ''
                                        || dataModel.constStartDate == '' || dataModel.constEndDate == '') {
                                        DevExpress.ui.dialog.alert("Please Enter Valid Dates On Project.", "Error!");
                                    }
                                    else {
                                        if (new Date(dataModel.constStartDate) <= new Date(dataModel.preconEndDate)
                                            && new Date(dataModel.constEndDate) >= new Date(dataModel.preconStartDate)) {
                                            DevExpress.ui.dialog.alert("Please Enter Valid Dates On Project - Phases precon and construction phase have overlapping schedules", "Error!");
                                        }
                                        else {
                                            $("#loadpanel").dxLoadPanel("instance").option("message", "Loading...");
                                            $("#loadpanel").dxLoadPanel("show");
                                            $.get("/api/rmmapi/GetNormaliseProjectAllocations?baseProjectID=" + data.TicketId + "&projectID=" + Model.RecordId, function (response, status) {
                                                var newData = [];
                                                if (response.Allocations.length > 0) {
                                                    CreateOPMPostData.Allocations = response.Allocations;
                                                    newData = CompactPhaseConstraints(CreateOPMPostData.Allocations);
                                                }
                                                else {
                                                    CreateOPMPostData.Allocations = response.Allocations;
                                                }
                                                var grdAllocations = $("#grdAllocations").dxDataGrid({
                                                    dataSource: newData,
                                                    showBorders: true,
                                                    columns: [
                                                        {
                                                            dataField: 'AssignedToName',
                                                            caption: 'Resource',
                                                            sortOrder: 'asc',
                                                            visible:false
                                                        },
                                                        {
                                                            dataField: 'TypeName',
                                                            caption: 'Role',
                                                            sortOrder: "asc"
                                                        },
                                                        {
                                                            dataField: 'AllocationStartDate',
                                                            caption: 'Start Date',
                                                            dataType: 'date',
                                                            sortOrder: "desc"
                                                        },
                                                        {
                                                            dataField: 'AllocationEndDate',
                                                            caption: 'End Date',
                                                            dataType: 'date',
                                                        },
                                                        {
                                                            dataField: "PctAllocation",
                                                            caption: "% Precon",
                                                            dataType: "text",
                                                            width: '7%',
                                                            cellTemplate: function (container, options) {
                                                                if (options.data.preconRefIds != null) {
                                                                    $("<div id='dataId'>")
                                                                        .append("<span style='float: left;overflow: auto;'><a href='javascript:void(0);' onclick=OpenInternalAllocation('" + options.data.preconRefIds + "');>" + options.data.PctAllocation + "</a></span>")
                                                                        .appendTo(container);
                                                                }
                                                                else {
                                                                    $("<div id='dataId'>")
                                                                        .append("<span style='float: left;overflow: auto;'>" + options.data.PctAllocation + "</span>")
                                                                        .appendTo(container);
                                                                }
                                                            }
                                                        },
                                                        {
                                                            dataField: "PctAllocationConst",
                                                            caption: "% Const.",
                                                            dataType: "text",
                                                            width: '7%',
                                                            cellTemplate: function (container, options) {
                                                                if (options.data.constRefIds != null) {
                                                                    $("<div id='dataId'>")
                                                                        .append("<span style='float: left;overflow: auto;'><a href='javascript:void(0);' onclick=OpenInternalAllocation('" + options.data.constRefIds + "');>" + options.data.PctAllocationConst + "</a></span>")
                                                                        .appendTo(container);
                                                                }
                                                                else {
                                                                    $("<div id='dataId'>")
                                                                        .append("<span style='float: left;overflow: auto;'>" + options.data.PctAllocationConst + "</span>")
                                                                        .appendTo(container);
                                                                }
                                                            }
                                                        },
                                                        {
                                                            dataField: "PctAllocationCloseOut",
                                                            caption: "% Closeout",
                                                            dataType: "text",
                                                            width: '7%',
                                                            cellTemplate: function (container, options) {
                                                                if (options.data.closeOutRefIds != null) {
                                                                    $("<div id='dataId'>")
                                                                        .append("<span style='float: left;overflow: auto;'><a href='javascript:void(0);' onclick=OpenInternalAllocation('" + options.data.closeOutRefIds + "');>" + options.data.PctAllocationCloseOut + "</a></span>")
                                                                        .appendTo(container);
                                                                }
                                                                else {
                                                                    $("<div id='dataId'>")
                                                                        .append("<span style='float: left;overflow: auto;'>" + options.data.PctAllocationCloseOut + "</span>")
                                                                        .appendTo(container);
                                                                }
                                                            }
                                                        }
                                                    ]

                                                });
                                                $("#loadpanel").dxLoadPanel("hide");
                                                $("#btnSaveAllocation").dxButton('instance').option("visible", true);
                                                $("#btnViewAllocation").dxButton('instance').option("visible", false);
                                                $("#chkShowResources").show();
                                                $("#chkShowResources").dxCheckBox('instance').option('value', false);
                                            });
                                        }
                                    }
                                }
                            })
                        )
                    },
                });
                $('#popupContainer').show();
            }
        });

    });
    function getDetails() {
        var btnShowSimilarProject = $("#btnShowSimilarProject").dxButton({
            text: 'Find Similar Projects',
            visible: '<%=EnableSimilarityFunction%>' == 'True' ? true : false,
            onClick: function (e) {
                $("#loadpanel").dxLoadPanel("instance").option("message", "RM One AI agent is finding similar projects...");
                $("#loadpanel").dxLoadPanel("show");

                $.ajax({
                    type: "Get",
                    url: "<%=opmwizardControllerUrl%>/GetSimilarProjects?TicketId=" + Model.RecordId + "&useMinimumSimilarityScore=true" ,
                    contentType: "application/json; charset=utf-8",
                    dataType: "JSON",
                    success: function (data) {
                        $("#loadpanel").dxLoadPanel("hide");
                        allprojects = data;
                        similarProjectData = data.filter((item) => item.Resources > 0).slice(0, 5);
                        var similarProjectList = $("#similarProjectList").dxTileView({
                            dataSource: similarProjectData,
                            baseItemHeight: 100,
                            baseItemWidth: 200,
                            itemMargin: 10,
                            direction: 'horizontal',
                            height: '130px',
                            showScrollbar: true,
                            noDataText: "No Similar Projects Available",
                            elementAttr: { "class": "tileViewContainer" },
                            itemTemplate: function (itemData, itemIndex, itemElement) {

                                $(".tileViewContainer .dx-tile").css("text-align", "center");
                                $(".tileViewContainer .dx-tile").css("padding", "2px");
                                $(".tileViewContainer .dx-tile").css("border-radius", "7px");
                                $(".tileViewContainer .dx-tile").css("background-color", itemData.ColorCode);

                                itemElement.addClass("tile");
                                itemElement.css("background", "#fff");
                                if (itemData.isSelected)
                                    itemElement.addClass("itemSelected");
                                else
                                    itemElement.removeClass("itemSelected");

                                const erpjobidnc = itemData.ERPJobIDNC == null || itemData.ERPJobIDNC == '' ? itemData.TicketId : itemData.ERPJobIDNC;
                                itemElement.append(
                                    `<div class='row tileTitle' title=\"${itemData.Title}\"><b>` + truncateString(itemData.Title, 45) + `</b></div>`,
                                    "<div class='row rowTitle'>" + erpjobidnc + "</div>",
                                    "<div class='row rowTitle'>Similarity Score: <span style='color:" + itemData.ColorCode +"'>" + itemData.Score + "</span></div>",
                                    "<div class='row rowTitle'>Resources:" + itemData.Resources + "</div>"
                                )
                            },
                            onItemRendered: function (e) {
                                if (e.itemData.isSelected)
                                    e.itemElement.addClass("itemSelected");
                                else
                                    e.itemElement.removeClass("itemSelected");
                            },
                            onItemClick: function (e) {
                                e.component._dataSource._items.forEach(function (item) {
                                    item.isSelected = false;
                                });
                                
                                var data = e.itemData;
                                e.itemData.isSelected = true;
                                e.itemElement.addClass("itemSelected");
                                CreateOPMPostData.SourceModule = "OPM";
                                CreateOPMPostData.SourceTicketId = data.TicketId;
                                var similarProjectTiles = $("#similarProjectList").dxTileView("instance");
                                //similarProjectTiles.option("dataSource", similarProjectData);
                                //similarProjectTiles.option("height", "240px");
                                similarProjectTiles.repaint();
                                $("#divAllocation").css("display", "block");
                                if (dataModel.preconStartDate == '' || dataModel.preconEndDate == ''
                                    || dataModel.constStartDate == '' || dataModel.constEndDate == '') {
                                    DevExpress.ui.dialog.alert("Please Enter Valid Dates On Project.", "Error!");
                                }
                                else {
                                    if (new Date(dataModel.constStartDate) <= new Date(dataModel.preconEndDate)
                                        && new Date(dataModel.constEndDate) >= new Date(dataModel.preconStartDate)) {
                                        DevExpress.ui.dialog.alert("Please Enter Valid Dates On Project - Phases precon and construction phase have overlapping schedules", "Error!");
                                    }
                                    else {
                                        $("#loadpanel").dxLoadPanel("instance").option("message", "Loading...");
                                        $("#loadpanel").dxLoadPanel("show");

                                        $.get("/api/rmmapi/GetNormaliseProjectAllocations?baseProjectID=" + data.TicketId + "&projectID=" + Model.RecordId, function (response, status) {
                                            var newData = [];
                                            if (response.Allocations.length > 0) {
                                                CreateOPMPostData.Allocations = response.Allocations;
                                                var newData = CompactPhaseConstraints(CreateOPMPostData.Allocations);
                                            }
                                            else {
                                                CreateOPMPostData.Allocations = response.Allocations;
                                            }
                                            var grdAllocations = $("#grdAllocations").dxDataGrid({
                                                dataSource: newData,
                                                showBorders: true,
                                                columns: [
                                                    {
                                                        dataField: 'AssignedToName',
                                                        caption: 'Resource',
                                                        sortOrder: 'asc',
                                                        visible: false,
                                                    },
                                                    {
                                                        dataField: 'TypeName',
                                                        caption: 'Role',
                                                        sortOrder: "asc",
                                                    },
                                                    {
                                                        dataField: 'AllocationStartDate',
                                                        caption: 'Start Date',
                                                        dataType: 'date',
                                                        sortOrder: "desc",
                                                    },
                                                    {
                                                        dataField: 'AllocationEndDate',
                                                        caption: 'End Date',
                                                        dataType: 'date',
                                                    },
                                                    {
                                                        dataField: "PctAllocation",
                                                        caption: "% Precon",
                                                        dataType: "text",
                                                        width: '7%',
                                                        cellTemplate: function (container, options) {
                                                            if (options.data.preconRefIds != null) {
                                                                $("<div id='dataId'>")
                                                                    .append("<span style='float: left;overflow: auto;'><a href='javascript:void(0);' onclick=OpenInternalAllocation('" + options.data.preconRefIds + "');>" + options.data.PctAllocation + "</a></span>")
                                                                    .appendTo(container);
                                                            }
                                                            else {
                                                                $("<div id='dataId'>")
                                                                    .append("<span style='float: left;overflow: auto;'>" + options.data.PctAllocation + "</span>")
                                                                    .appendTo(container);
                                                            }
                                                        }
                                                    },
                                                    {
                                                        dataField: "PctAllocationConst",
                                                        caption: "% Const.",
                                                        dataType: "text",
                                                        width: '7%',
                                                        cellTemplate: function (container, options) {
                                                            if (options.data.constRefIds != null) {
                                                                $("<div id='dataId'>")
                                                                    .append("<span style='float: left;overflow: auto;'><a href='javascript:void(0);' onclick=OpenInternalAllocation('" + options.data.constRefIds + "');>" + options.data.PctAllocationConst + "</a></span>")
                                                                    .appendTo(container);
                                                            }
                                                            else {
                                                                $("<div id='dataId'>")
                                                                    .append("<span style='float: left;overflow: auto;'>" + options.data.PctAllocationConst + "</span>")
                                                                    .appendTo(container);
                                                            }
                                                        }
                                                    },
                                                    {
                                                        dataField: "PctAllocationCloseOut",
                                                        caption: "% Closeout",
                                                        dataType: "text",
                                                        width: '7%',
                                                        cellTemplate: function (container, options) {
                                                            if (options.data.closeOutRefIds != null) {
                                                                $("<div id='dataId'>")
                                                                    .append("<span style='float: left;overflow: auto;'><a href='javascript:void(0);' onclick=OpenInternalAllocation('" + options.data.closeOutRefIds + "');>" + options.data.PctAllocationCloseOut + "</a></span>")
                                                                    .appendTo(container);
                                                            }
                                                            else {
                                                                $("<div id='dataId'>")
                                                                    .append("<span style='float: left;overflow: auto;'>" + options.data.PctAllocationCloseOut + "</span>")
                                                                    .appendTo(container);
                                                            }
                                                        }
                                                    }
                                                ]

                                            });
                                            $("#loadpanel").dxLoadPanel("hide");
                                            $("#btnSaveAllocation").dxButton('instance').option("visible", true);
                                            $("#btnViewAllocation").dxButton('instance').option("visible", false);
                                            $("#chkShowResources").show();
                                            $("#chkShowResources").dxCheckBox('instance').option('value', false);
                                        });
                                    }
                                }
                            }
                        });
                        $("#SimilarProjectContainer").css("display", "flex");
                    },
                    error: function (xhr, ajaxOptions, thrownError) {

                    }
                })
            }
        });

        var txtProjectName = $("#txtProjectName").dxTextBox({
            placeholder: "Project Name",
            width: "70%",
        });

        var ddlClient = $("#ddlClient").dxSelectBox({
            valueExpr: "TicketId",
            displayExpr: "Title",
            placeholder: "Select Client",
            searchEnabled: true,
            /*dataSource: ContactList,*/
            dataSource: new DevExpress.data.DataSource({
                store: ContactList,
                paginate: true,
            }),
        });

        var ddlType = $("#ddlType").dxSelectBox({
            valueExpr: "ID",
            displayExpr: "Title",
            placeholder: "Select Type",
            dataSource: "/api/OPMWizard/GetRequestTypeList",
        });

        var txtCMIC = $("#txtCMIC").dxTextBox({
            placeholder: "CMIC #"
        });

        var txtNCO = $("#txtNCO").dxTextBox({
            placeholder: "NCO #"
        });

        var dteDueDate = $("#dteDueDate").dxDateBox({
            placeholder: "Bid Due Date",
            type: 'date',
            inputAttr: { 'aria-label': 'Date' },
        });

        var dteConstStart = $("#dteConstStart").dxDateBox({
            type: 'date',
            placeholder: "Const Start",
            inputAttr: { 'aria-label': 'Date' },
        });

        var ddlContractType = $("#ddlContractType").dxSelectBox({
            valueExpr: "ID",
            displayExpr: "Title",
            searchEnabled: true,
            placeholder: "Contract Type Choice",
            dataSource: "/api/OPMWizard/GetChoiceField?ChoiceField=OwnerContractTypeChoice",
        });

        var txtContractValue = $("#txtContractValue").dxTextBox({
            placeholder: "Contract Value"
        });

        var txtAddress = $("#txtAddress").dxTextBox({
            placeholder: "Address"
        });

        var txtCity = $("#txtCity").dxTextBox({
            placeholder: "City"
        });

        var ddlState = $("#ddlState").dxSelectBox({
            valueExpr: "ID",
            displayExpr: "Title",
            searchEnabled: true,
            placeholder: "State",
            dataSource: "/api/OPMWizard/GetStateDetails",
        });

        var txtZip = $("#txtZip").dxTextBox({
            placeholder: "Zip"
        });

        var txtNetRentableSqft = $("#txtNetRentableSqft").dxTextBox({
            placeholder: "Net Rentable Sqft"
        });

        var txtGrossSqft = $("#txtGrossSqft").dxTextBox({
            placeholder: "Gross Sq Ft"
        });

        var ddlComplexity = $("#ddlComplexity").dxSelectBox({
            valueExpr: "ID",
            displayExpr: "Title",
            searchEnabled: true,
            placeholder: "Complexity",
            dataSource: "/api/OPMWizard/GetChoiceField?ChoiceField=CRMProjectComplexityChoice",
        });

        var ddlOPMType = $("#ddlOPMType").dxSelectBox({
            valueExpr: "ID",
            displayExpr: "Title",
            searchEnabled: true,
            placeholder: "Opportunity Type",
            dataSource: "/api/OPMWizard/GetChoiceField?ChoiceField=OpportunityTypeChoice",
        });

        var ddlWinLikelyHood = $("#ddlWinLikelyHood").dxSelectBox({
            valueExpr: "ID",
            displayExpr: "Title",
            searchEnabled: true,
            placeholder: "Win Likelihood",
            dataSource: "/api/OPMWizard/GetChoiceField?ChoiceField=ChanceOfSuccessChoice",
        });

        var txtDescription = $("#txtDescription").dxTextArea({
            placeholder: "Description",
            height: "100"
        });

        var btnSave = $("#btnSummarySave").dxButton({
            text: "Save",
            onClick: function (e) {
                $("#summaryContainer").css("display", "block");
                $("#selectOPM").css("display", "none");
                $("#newOPMContainer").css("display", "none");
                $("#selectLead").css("display", "none");

                let ProjectName = $("#txtProjectName").dxTextBox('instance').option('value');
                let Client = $("#ddlClient").dxSelectBox('instance').option('value');
                let RequestTypeLookup = $("#ddlType").dxSelectBox('instance').option('value');
                let CMIC = $("#txtCMIC").dxTextBox('instance').option('value');
                let NCO = $("#txtNCO").dxTextBox('instance').option('value');
                let DueDate = $("#dteDueDate").dxDateBox('instance').option('value') != "" && $("#dteDueDate").dxDateBox('instance').option('value') != null ? $("#dteDueDate").dxDateBox('instance').option('value').toLocaleDateString('en-US') : null;
                let ConstStartDate = $("#dteConstStart").dxDateBox('instance').option('value') != "" && $("#dteConstStart").dxDateBox('instance').option('value') != null ? $("#dteConstStart").dxDateBox('instance').option('value').toLocaleDateString('en-US') : null;
                let ContractType = $("#ddlContractType").dxSelectBox('instance').option('value');
                let ContractValue = $("#txtContractValue").dxTextBox('instance').option('value');
                let Address = $("#txtAddress").dxTextBox('instance').option('value');
                let City = $("#txtCity").dxTextBox('instance').option('value');
                let State = $("#ddlState").dxSelectBox('instance').option('value');
                let ZipCode = $("#txtZip").dxTextBox('instance').option('value');
                let UsableSqFt = $("#txtNetRentableSqft").dxTextBox('instance').option('value');
                let RetailSqftNum = $("#txtGrossSqft").dxTextBox('instance').option('value');
                let Complexity = $("#ddlComplexity").dxSelectBox('instance').option('value');
                let OPMType = $("#ddlOPMType").dxSelectBox('instance').option('value');
                let WinLikeHood = $("#ddlWinLikelyHood").dxSelectBox('instance').option('value');
                let Description = $("#txtDescription").dxTextArea('instance').option('value');

                let newQuickOpportunityResponse = {
                    ProjectName: ProjectName,
                    Client: Client,
                    RequestTypeLookup: RequestTypeLookup,
                    CMIC: CMIC,
                    NCO: NCO,
                    DueDate: DueDate,
                    ConstStartDate: ConstStartDate,
                    ContractType: ContractType,
                    ContractValue: ContractValue,
                    Address: Address,
                    City: City,
                    State: State,
                    Zip: ZipCode,
                    UsableSqFt: UsableSqFt,
                    RetailSqftNum: RetailSqftNum,
                    Complexity: Complexity,
                    OPMType: OPMType,
                    WinLikeHood: WinLikeHood,
                    Description: Description
                };

                $.post(ugitConfig.apiBaseUrl + "/api/OPMWizard/CreateQuickOpportunity/", newQuickOpportunityResponse).then(function (data, status) {
                    $("#titlehd").text(`${data.TicketId}: ${data.Title}`);
                    setDatesModel(data.TicketId);
                });
            }
        });

        var ddlTemplate = $("#ddlTemplate").dxSelectBox({
            placeholder: "Select Existing Template",
            valueExpr: "ID",
            displayExpr: "Name",
            visible: !HideAllocationTemplate,
            dataSource: "/api/rmmapi/GetProjectTemplates",
            onSelectionChanged: function (e) {
                if (typeof e.selectedItem.Name !== "undefined") {
                    const Id = e.selectedItem.ID;
                    const Name = e.selectedItem.Name;
                    const TicketId = Model.RecordId;
                    
                    if (dataModel.preconStartDate == '' || dataModel.preconEndDate == ''
                        || dataModel.constStartDate == '' || dataModel.constEndDate == '') {
                        //DevExpress.ui.dialog.alert("Please Enter Valid Dates On Project.", "Error!");
                        CreateOPMPostData.TemplateID = Id;
                        openDateAgent(Model.RecordId);
                    }
                    else {
                        if (new Date(dataModel.constStartDate) <= new Date(dataModel.preconEndDate)
                            && new Date(dataModel.constEndDate) >= new Date(dataModel.preconStartDate)) {
                            DevExpress.ui.dialog.alert("Please Enter Valid Dates On Project - Phases precon and construction phase have overlapping schedules", "Error!");
                        }
                        else {
                            LoadTemplateAllocations(Id, TicketId, dataModel.preconStartDate, dataModel.closeoutEndDate);
                        }
                    }
                }
            },
        });

        var btnBack = $("#btnBack").dxButton({
            text: "Back",
            onClick: function (e) {
                //var modedropdown = $("#selectBox").dxSelectBox("instance");
                //modedropdown.reset();
                $("#summaryContainer").css("display", "none");
                $("#selectOPM").css("display", "none");
                $("#newOPMContainer").css("display", "none");
                $("#selectLead").css("display", "none");
            }
        });

        var btnContinue = $("#btnContinue").dxButton({
            text: 'Create New Project',
            onClick: function (e) {
                $("#loadpanel").dxLoadPanel("show");
                CreateOPMPostData.ProjectStartDate = dataModel.preconStartDate;
                CreateOPMPostData.ProjectEndDate = dataModel.closeoutEndDate;
                $.ajax({
                    url: ugitConfig.apiBaseUrl + "/api/OPMWizard/CreateOpportunity",
                    data: JSON.stringify(CreateOPMPostData),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    type: "POST",
                    success: function (response) {
                        if (response.TicketId) {
                            $("#loadpanel").dxLoadPanel("hide");
                            var sourceURL = "<%= Request["source"] %>";
                            let title = "";
                            if ("<%=moduleName%>" == "CPR") {
                                title = "New Construction Project";
                            }
                            else {
                                title = "New Opportunity";
                            }
                            let htmlContent = `<div style='font-size:14px;margin-bottom:10px;font-weight:400;'>${title} Created <a style="color:blue;text-decoration:underline;" href="javascript:" id='myAnchor' onclick="PopupCloseBeforeOpenUgit('<%=ticketURL%>','ticketId=${response.TicketId}','${response.TicketId}: ${response.Title}' , 90, 90, 0, '${sourceURL}');">${response.TicketId}</a> : ${response.Title}.</div>`;
                            window.parent.UgitOpenHTMLPopupDialog(htmlContent, `'${response.Title}'`, '', 'true');
                            DevExpress.ui.dialog.alert(`${response.TicketId}: ${response.Title} Saved Successfully.`, "Alert!");
                            sourceURL += "**refreshDataOnly";
                            window.parent.CloseWindowCallback(1, sourceURL);
                        } else {
                            $("#loadpanel").dxLoadPanel("hide");
                            DevExpress.ui.dialog.alert(`Unable To Create New Opportunity.`, "Alert!");
                        }
                    }
                });
            }
        });

        var btnViewAllocation = $("#btnViewAllocation").dxButton({
            text: 'View Allocations',
            visible: true,
            onClick: function (e) {
                OpenProjectAllocation(Model.RecordId, HeaderTitle, false);
            }
        });

        var btnSaveAllocation = $("#btnSaveAllocation").dxButton({
            text: 'Save Allocations',
            visible:false,
            onClick: function (e) {
                if (CreateOPMPostData.Allocations != null && CreateOPMPostData.Allocations.length > 0) {
                    $.get("/api/rmmapi/GetProjectAllocations?projectID=" + Model.RecordId, function (response, status) {
                        if (response.Allocations.length > 0) {
                            var result = DevExpress.ui.dialog.confirm('Selected Allocations will override current allocations. Do you want to proceed?', 'Confirm');
                            result.done(function (dialogResult) {
                                if (dialogResult) {
                                    SaveAllocations();
                                }
                            });
                        }
                        else
                        {
                            SaveAllocations();
                        }
                    });
                }
                else {
                    DevExpress.ui.dialog.alert("No allocations to save.", "Error");
                }
            }
        });

        var btnCancel = $("#btnCancel").dxButton({
            text: "Cancel",
            onContentReady: function (e) {
                
                $('.ui-dialog-title').text(HeaderTitle);
            }, 
            onClick: function (e) {
                LoadInitialScreen("<%=selectionMode%>");
            }
        })

        $("#divPrecon .rcorners2").click(function (e) {
            openDateAgent(Model.RecordId);
        });

        $("#divConst .rcorners2").click(function (e) {
            openDateAgent(Model.RecordId);
        });

        $("#divCloseout .rcorners2").click(function (e) {
            openDateAgent(Model.RecordId);
        });

        $('#popup').dxPopup({
            visible: false,
            hideOnOutsideClick: true,
            showTitle: true,
            showCloseButton: true,
            title: "Please Enter Valid Dates On Project.",
            width: "auto",
            height: "auto",
            resizeEnabled: true,
            dragEnabled: true,
            contentTemplate: () => {
                const content = $("<div />");
                content.append(
                    $("<div id='form' />").dxForm({
                        formData: dataModel,
                        title: 'Update Dates',
                        items: [{
                            itemType: 'group',
                            name: 'group',
                            caption: '',
                            colCount: 3,
                            items: [{
                                dataField: 'preconStartDate',
                                editorType: 'dxDateBox',
                                editorOptions: {
                                    value: undefined,
                                    type: "date",
                                    displayFormat: "MM/dd/yyyy", 
                                    onValueChanged(e) {
                                        var enteredPreconStartDate = e.value;
                                        let newdate = new Date(enteredPreconStartDate);
                                        if (newdate == 'Invalid Date' || String(newdate.getFullYear()).length > 4) {
                                            //DevExpress.ui.dialog.alert("Please enter a valid date.");
                                            return;
                                        }

                                        if (enteredPreconStartDate != null) {
                                            if (String(enteredPreconStartDate).startsWith("00")) {
                                                enteredPreconStartDate = enteredPreconStartDate.replace(/^.{2}/g, '20');
                                                e.value = enteredPreconStartDate;
                                                dataModel.preconStartDate = enteredPreconStartDate;
                                            }
                                        }
                                        if (dataModel.preconEndDate != '' && dataModel.preconStartDate != '') {
                                            dataModel.preconDuration = GetDurationInWeek(ajaxHelperPage, dataModel.preconStartDate, dataModel.preconEndDate);
                                            $("#form").dxForm("instance").updateData({ 'preconDuration': dataModel.preconDuration });
                                        }
                                    },
                                    displayFormat: {
                                        parser: function (value) {
                                            let parts = value.split('/');
                                            if (3 !== parts.length) {
                                                return;
                                            }
                                            return new Date(parts[2].length < 3 ? Number('20' + parts[2]) : Number(parts[2]), Number(parts[0]) - 1, Number(parts[1]))
                                        },
                                        formatter: function (value) {
                                            return DevExpress.localization.date.format(value, 'shortdate');
                                        }

                                    }
                                },
                                label: {
                                    template: labelTemplate('PreCon Start'),
                                },
                            },
                                {
                                    dataField: 'preconDuration',
                                    editorType: 'dxNumberBox',
                                    editorOptions: {
                                        value: undefined,
                                        onFocusOut(e) {
                                            if (e.component.option('value') != '' && dataModel.preconStartDate != '') {
                                                dataModel.preconEndDate = GetEndDateByWeeks(ajaxHelperPage, dataModel.preconStartDate, e.component.option('value'));
                                                $("#form").dxForm("instance").updateData({ 'preconEndDate': new Date(dataModel.preconEndDate).toLocaleDateString('en-US') });
                                            }
                                        },
                                    },
                                },
                                {
                                    dataField: 'preconEndDate',
                                    editorType: 'dxDateBox',
                                    editorOptions: {
                                        value: undefined,
                                        type: "date",
                                        displayFormat: "MM/dd/yyyy", 
                                        onValueChanged(e) {
                                            var enteredPreconEndDate = e.value;
                                            let newdate = new Date(enteredPreconEndDate);
                                            if (newdate == 'Invalid Date' || String(newdate.getFullYear()).length > 4) {
                                                //DevExpress.ui.dialog.alert("Please enter a valid date.");
                                                return;
                                            }
                                            if (enteredPreconEndDate != null) {
                                                if (String(enteredPreconEndDate).startsWith("00")) {
                                                    enteredPreconEndDate = enteredPreconEndDate.replace(/^.{2}/g, '20');
                                                    e.value = enteredPreconEndDate;
                                                    dataModel.preconEndDate = enteredPreconEndDate;
                                                }
                                            }
                                            if (dataModel.preconEndDate != '' && dataModel.preconStartDate != '') {
                                                dataModel.preconDuration = GetDurationInWeek(ajaxHelperPage, dataModel.preconStartDate, dataModel.preconEndDate);
                                                $("#form").dxForm("instance").updateData({ 'preconDuration': dataModel.preconDuration });
                                            }
                                        },
                                        displayFormat: {
                                            parser: function (value) {
                                                let parts = value.split('/');
                                                if (3 !== parts.length) {
                                                    return;
                                                }
                                                return new Date(parts[2].length < 3 ? Number('20' + parts[2]) : Number(parts[2]), Number(parts[0]) - 1, Number(parts[1]))
                                            },
                                            formatter: function (value) {
                                                return DevExpress.localization.date.format(value, 'shortdate');
                                            }

                                        }
                                    },
                                    label: {
                                        template: labelTemplate('PreCon End'),
                                    },
                                },
                                {
                                    dataField: 'constStartDate',
                                    editorType: 'dxDateBox',
                                    editorOptions: {
                                        value: dataModel.constStartDate,
                                        type: "date",
                                        displayFormat: "MM/dd/yyyy", 
                                        onValueChanged(e) {
                                            var enteredConstStartDate = e.value;
                                            let newdate = new Date(enteredConstStartDate);
                                            if (newdate == 'Invalid Date' || String(newdate.getFullYear()).length > 4) {
                                                //DevExpress.ui.dialog.alert("Please enter a valid date.");
                                                return;
                                            }
                                            if (enteredConstStartDate != null) {
                                                if (String(enteredConstStartDate).startsWith("00")) {
                                                    enteredConstStartDate = enteredConstStartDate.replace(/^.{2}/g, '20');
                                                    e.value = enteredConstStartDate;
                                                    dataModel.constStartDate = enteredConstStartDate;
                                                }
                                            }
                                            if (dataModel.constEndDate != '' && dataModel.constStartDate != '') {
                                                dataModel.constDuration = GetDurationInWeek(ajaxHelperPage, dataModel.constStartDate, dataModel.constEndDate);
                                                $("#form").dxForm("instance").updateData({ 'constDuration': dataModel.constDuration });
                                            }
                                        },
                                        displayFormat: {
                                            parser: function (value) {
                                                let parts = value.split('/');
                                                if (3 !== parts.length) {
                                                    return;
                                                }
                                                return new Date(parts[2].length < 3 ? Number('20' + parts[2]) : Number(parts[2]), Number(parts[0]) - 1, Number(parts[1]))
                                            },
                                            formatter: function (value) {
                                                return DevExpress.localization.date.format(value, 'shortdate');
                                            }

                                        }
                                    },
                                    label: {
                                        template: labelTemplate('Const Start'),
                                    },
                                },
                                {
                                    dataField: 'constDuration',
                                    editorType: 'dxNumberBox',
                                    editorOptions: {
                                        value: undefined,
                                        onFocusOut(e) {
                                            if (e.component.option('value') != '' && dataModel.constStartDate != '') {
                                                dataModel.constEndDate = GetEndDateByWeeks(ajaxHelperPage, dataModel.constStartDate, e.component.option('value'));
                                                $("#form").dxForm("instance").updateData({ 'constEndDate': new Date(dataModel.constEndDate).toLocaleDateString('en-US') });
                                            }
                                        },
                                    },
                                    label: {
                                        template: labelTemplate('PreCon Start'),
                                    },
                                },
                                {
                                    dataField: 'constEndDate',
                                    editorType: 'dxDateBox',
                                    editorOptions: {
                                        value: dataModel.constEndDate,
                                        type: "date",
                                        displayFormat: "MM/dd/yyyy", 
                                        onValueChanged(e) {
                                            var enteredConstEndDate = e.value;
                                            let newdate = new Date(enteredConstEndDate);
                                            if (newdate == 'Invalid Date' || String(newdate.getFullYear()).length > 4) {
                                                //DevExpress.ui.dialog.alert("Please enter a valid date.");
                                                return;
                                            }
                                            if (enteredConstEndDate != null) {
                                                if (String(enteredConstEndDate).startsWith("00")) {
                                                    enteredConstEndDate = enteredConstEndDate.replace(/^.{2}/g, '20');
                                                    e.value = enteredConstEndDate;
                                                }
                                                if (dataModel.constEndDate != '' && dataModel.constStartDate != '') {
                                                    dataModel.constDuration = GetDurationInWeek(ajaxHelperPage, dataModel.constStartDate, dataModel.constEndDate);
                                                    $("#form").dxForm("instance").updateData({ 'constDuration': dataModel.constDuration });
                                                }
                                                if (enteredConstEndDate != null && enteredConstEndDate != e.previousValue) {
                                                    $.ajax({
                                                        type: "GET",
                                                        url: "<%= rmoneControllerUrl %>/GetNextWorkingDateAndTime?dateString=" + new Date(e.value).toLocaleDateString('en-US'),
                                                        contentType: "application/json; charset=utf-8",
                                                        dataType: "json",
                                                        async: false,
                                                        success: function (message) {
                                                            dataModel.closeoutStartDate = new Date(message).toLocaleDateString('en-US');
                                                            dataModel.closeoutEndDate = new Date(GetEndDateByWorkingDays(ajaxHelperPage, message, "<%=closeoutperiod%>")).toISOString();
                                                            dataModel.closeOutDuration = GetDurationInWeek(ajaxHelperPage, dataModel.closeoutStartDate, dataModel.closeoutEndDate);
                                                            $("#form").dxForm("instance").updateData({ 'closeoutEndDate': new Date(dataModel.closeoutEndDate).toLocaleDateString('en-US'), 'closeoutStartDate': dataModel.closeoutStartDate, 'closeOutDuration': dataModel.closeOutDuration });
                                                        },
                                                        error: function (xhr, ajaxOptions, thrownError) {

                                                        }
                                                    });
                                                }
                                            }
                                        },
                                        displayFormat: {
                                            parser: function (value) {
                                                let parts = value.split('/');
                                                if (3 !== parts.length) {
                                                    return;
                                                }
                                                return new Date(parts[2].length < 3 ? Number('20' + parts[2]) : Number(parts[2]), Number(parts[0]) - 1, Number(parts[1]))
                                            },
                                            formatter: function (value) {
                                                return DevExpress.localization.date.format(value, 'shortdate');
                                            }

                                        }
                                    },
                                    label: {
                                        template: labelTemplate('Const End'),
                                    },
                                },
                                {
                                    dataField: 'closeoutStartDate',
                                    editorOptions: {
                                        value: dataModel.closeoutStartDate,
                                        format: 'MMM d, yyyy',
                                        readOnly: true,
                                    },
                                    label: {
                                        template: labelTemplate('Close Out'),
                                    },
                                },
                                {
                                    dataField: 'closeOutDuration',
                                    editorType: 'dxNumberBox',
                                    editorOptions: {
                                        value: undefined,
                                        onFocusOut(e) {
                                            if (e.component.option('value') != '' && dataModel.closeoutStartDate != '') {
                                                dataModel.closeoutEndDate = GetEndDateByWeeks(ajaxHelperPage, dataModel.closeoutStartDate, e.component.option('value'));
                                                $("#form").dxForm("instance").updateData({ 'closeoutEndDate': new Date(dataModel.closeoutEndDate).toLocaleDateString('en-US') });
                                            }
                                        },
                                    },
                                    label: {
                                        template: labelTemplate('PreCon Start'),
                                    },
                                },
                                {
                                    dataField: 'closeoutEndDate',
                                    editorType: 'dxDateBox',
                                    editorOptions: {
                                        value: dataModel.closeoutEndDate,
                                        type: "date",
                                        displayFormat: "MM/dd/yyyy", 
                                        onValueChanged(e) {
                                            var enteredCloseoutEndDate = e.value;
                                            let newdate = new Date(enteredCloseoutEndDate);
                                            if (newdate == 'Invalid Date' || String(newdate.getFullYear()).length > 4) {
                                                //DevExpress.ui.dialog.alert("Please enter a valid date.");
                                                return;
                                            }
                                            if (enteredCloseoutEndDate != null) {
                                                if (String(enteredCloseoutEndDate).startsWith("00")) {
                                                    enteredCloseoutEndDate = enteredCloseoutEndDate.replace(/^.{2}/g, '20');
                                                    e.value = enteredCloseoutEndDate;
                                                    dataModel.closeoutEndDate = enteredCloseoutEndDate;
                                                }
                                            }
                                            if (dataModel.closeoutEndDate != '' && dataModel.closeoutStartDate != '') {
                                                dataModel.closeOutDuration = GetDurationInWeek(ajaxHelperPage, dataModel.closeoutStartDate, dataModel.closeoutEndDate);
                                                $("#form").dxForm("instance").updateData({ 'closeOutDuration': dataModel.closeOutDuration });
                                            }
                                        },
                                        displayFormat: {
                                            parser: function (value) {
                                                let parts = value.split('/');
                                                if (3 !== parts.length) {
                                                    return;
                                                }
                                                return new Date(parts[2].length < 3 ? Number('20' + parts[2]) : Number(parts[2]), Number(parts[0]) - 1, Number(parts[1]))
                                            },
                                            formatter: function (value) {
                                                return DevExpress.localization.date.format(value, 'shortdate');
                                            }

                                        }
                                    },
                                    label: {
                                        template: labelTemplate('Close Out End'),
                                    },
                                },
                                {
                                    dataField: 'onHold',
                                    editorType: 'dxSwitch',
                                    visible: false,
                                    editorOptions: {
                                        width: 100,
                                        value: dataModel.onHold,
                                        switchedOffText: "OFF HOLD",
                                        switchedOnText: "ON HOLD",
                                    },
                                    label: {
                                        template: labelTemplate(''),
                                    },
                                }
                            ]
                        }],
                        onContentReady: function (data) {
                            data.element.find("label[for$='preconDuration']").text("Precon Duration(Weeks)");
                            data.element.find("label[for$='constDuration']").text("Const Duration(Weeks)");
                            data.element.find("label[for$='closeOutDuration']").text("Closeout Duration(Weeks)");
                        }
                    }),
                    $("#saveButton").dxButton({
                        text: 'Save',
                        icon: '/content/Images/saveFile_icon.png',
                        onClick: function (e) {
                            UpdateRecord();
                        }
                    })
                );
                return content;
            },
            onDisposing: function (e) {
                dataModel.preconStartDate = "";
                dataModel.preconEndDate = "";
                dataModel.constStartDate = "";
                dataModel.constEndDate = "";
                dataModel.closeoutEndDate = "";
                dataModel.onHold = false;
            },
            onHidden: function (e) {
                dataModel.preconStartDate = "";
                dataModel.preconEndDate = "";
                dataModel.constStartDate = "";
                dataModel.constEndDate = "";
                dataModel.closeoutEndDate = "";
                dataModel.onHold = false;
                
                setDatesModel(Model.RecordId);
            }
        });

        var chkShowResources = $("#chkShowResources").dxCheckBox({
            text: "Show Resource Names",
            value: false,
            onValueChanged: function (e) {
                
                var grdAllocation = $("#grdAllocations").dxDataGrid("instance");

                if (e.value) {
                    grdAllocation.columnOption('AssignedToName', 'visible', true);
                } else
                    grdAllocation.columnOption('AssignedToName', 'visible', false);
            }
        });

    }

    function OpenProjectAllocation(ticketid, title, openSummaryView) {
        let path = "/Layouts/uGovernIT/DelegateControl.aspx?isdlg=0&isudlg=1&control=CRMProjectAllocationNew&isreadonly=true&opensummaryview=" + openSummaryView + "&ticketId=" + ticketid + "&module=" + CreateOPMPostData.SourceModule;
        UgitOpenPopupDialog(path, "moduleName=" + CreateOPMPostData.SourceModule , title, '95', '95', false, escape(window.location.href));
        //$(".ui-dialog-content.ui-widget-content.ui-dialog-normal").prev().hide();
    }

    function SaveAllocations()
    {
        $("#loadpanel").dxLoadPanel("show");
        $.post("/api/rmmapi/UpdateBatchCRMAllocationsForTemplate/", {
            Allocations: CreateOPMPostData.Allocations, ProjectID: Model.RecordId, PreConStart: dataModel.preconStartDate,
            PreConEnd: dataModel.preconEndDate, ConstStart: dataModel.constStartDate, ConstEnd: dataModel.constEndDate, LastEditedRow: "", OverrideAllocations: true
        }).then(function (response) {
            $("#loadpanel").dxLoadPanel("hide");

            if (response.includes("BlankAllocation")) {
                $("#toastBlankAllocation").dxToast("show");
            }
            else if (response.includes("OverlappingAllocation")) {
                DevExpress.ui.dialog.alert("Overlapping allocations are not permitted. Save unsuccessful.", "Error");
            }
            else if (response.includes("AllocationOutofbounds")) {
                DevExpress.ui.dialog.alert("Allocation date entered is either prior to start date or after the end date of the resource. <br/>Please enter valid dates.", "Error");
            }
            else if (response.includes("DateNotValid")) {
                DevExpress.ui.dialog.alert("Start Date should be less than End Date. Save unsuccessful.", "Alert!");
            }
            else {
                OpenProjectAllocation(Model.RecordId, HeaderTitle, true);
                //$("#toast").dxToast('instance').option("type", "success");
                //$("#toast").dxToast("show");
            }
        }, function (error) { });
    }
</script>
<script type="text/javascript" data-v="<%=uGovernIT.Utility.UGITUtility.AssemblyVersion %>">
    function LoadInitialScreen(selectionmode) {

        $("#loadpanel").dxLoadPanel("show");
        if (selectionmode == "Lead") {

            var URL = "<%= ajaxPageURL %>/GetLeads";
            $.ajax({
                type: "Get",
                url: URL,
                contentType: "application/json; charset=utf-8",
                dataType: "JSON",
                success: function (data) {

                    workItemData = data;

                    const $dataGrid = $("#selectLead").dxDataGrid({
                        dataSource: workItemData,
                        hoverStateEnabled: true,
                        filterRow: { visible: true },
                        scrolling: { mode: 'virtual' },
                        selection: { mode: 'single' },
                        showBorders: true,
                        height: '100%',
                        columns: [{
                            dataField: "ID",
                            width: 40,
                            visible: false
                        },
                        {
                            dataField: "Title",
                            Width: '70%'
                        },
                        {
                            dataField: "LeadStatus",
                            Width: 100,
                            visible: false
                        },
                        {
                            dataField: "Status",
                            Width: '30%'
                        },
                        {
                            dataField: "TicketId",
                            visible: false
                        }
                        ],
                        onSelectionChanged(selectedItems) {
                            
                            const keys = selectedItems.selectedRowKeys;
                            const hasSelection = keys.length;
                            if (hasSelection) {
                                //create opportunity based on selected Lead
                                CreateOPMPostData.SourceTicketId = keys[0].TicketId;
                                CreateOPMPostData.SourceModule = "Lead";
                                CreateOPMPostData.TemplateID = "";

                                $("#summaryContainer").css("display", "block");
                                $("#selectOPM").css("display", "none");
                                $("#newOPMContainer").css("display", "none");
                                $("#selectLead").css("display", "none");
                                setDatesModel(response.TicketId);
                            }

                        },
                    });

                    $("#summaryContainer").css("display", "none");
                    $("#selectOPM").css("display", "none");
                    $("#newOPMContainer").css("display", "none");
                    $("#selectLead").css("display", "block");

                    $("#loadpanel").dxLoadPanel("hide");
                },
                error: function (xhr, ajaxOptions, thrownError) {

                    $("#loadpanel").dxLoadPanel("hide");
                }
            });

        }
        else if (selectionmode == "OpenOpportunity") {
            CreateOPMPostData.SourceModule = "OPM";
            var URL = "<%= ajaxPageURL %>/GetOpportunities";
            $.ajax({
                type: "Get",
                url: URL,
                contentType: "application/json; charset=utf-8",
                dataType: "JSON",
                success: function (data) {
                    opmData = data;
                    const $dataGrid = $("#selectOPM").dxDataGrid({
                        dataSource: opmData,
                        hoverStateEnabled: true,
                        filterRow: { visible: true },
                        scrolling: { mode: 'virtual' },
                        selection: { mode: 'single'},
                        showBorders: true,
                        height: '100%',
                        columns: [{
                            dataField: "ID",
                            width: 60,
                            visible: false
                        },
                        {
                            dataField: "SummaryIcon",
                            caption: "Summary",
                            width: 80,
                            encodeHtml: false,
                        },
                        {
                            dataField: "TicketId",
                            width: "120px",
                            visible: false,
                        },
                        {
                            dataField: "Title",
                            width: "600px"
                        },
                        {
                            dataField: "ERPJobIDNC",
                            caption: "CMIC NC #",
                            width: "100px"
                        },
                        {
                            dataField: "Status",
                            //width: "150px"
                        },

                        {
                            dataField: "OpportunityTargetChoice",
                            caption: "Target",
                            width: 90
                        }
                        ],
                        onSelectionChanged(selectedItems) {
                            debugger;
                            var button = $("#btnShowSimilarProject").dxButton("instance");
                            const keys = selectedItems.selectedRowKeys;
                            const hasSelection = keys.length;
                            HeaderTitle = keys[0].ERPJobIDNC == null || keys[0].ERPJobIDNC == '' ? keys[0].TicketId + ": " + keys[0].Title
                                : keys[0].ERPJobIDNC + ": " + keys[0].Title;
                            $("#titlehd").text(HeaderTitle);

                            if (keys[0].prefixTitle == "Service") {
                                button.option("text", "Find Similar Services");
                            }
                            else {
                                button.option("text", "Find Similar Projects");
                            }
                            if (hasSelection) {
                                $("#summaryContainer").css("display", "block");
                                $("#selectOPM").css("display", "none");
                                $("#newOPMContainer").css("display", "none");
                                $("#selectLead").css("display", "none");
                                setDatesModel(keys[0].TicketId);
                                CreateOPMPostData.SourceTicketId = keys[0].TicketId;
                                CreateOPMPostData.SourceModule = "OPM";
                                CreateOPMPostData.TemplateID = "";
                                CreateOPMPostData.OpportunityTarget = keys[0].OpportunityTargetChoice;
                            }
                        }
                    });


                    $("#summaryContainer").css("display", "none");
                    $("#selectOPM").css("display", "block");
                    $("#selectLead").css("display", "none");
                    $("#newOPMContainer").css("display", "none");
                    $("#loadpanel").dxLoadPanel("hide");
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    $("#loadpanel").dxLoadPanel("hide");
                }
            });

        }
        else if (selectionmode == "NewOpportunity") {
            $("#newOPMContainer").css("display", "block");
            $("#selectOPM").css("display", "none");
            $("#selectLead").css("display", "none");
            $("#summaryContainer").css("display", "none");
        }
        else if (selectionmode == "NewAllocatonsFromProjects") {
            $("#summaryContainer").css("display", "block");
            $("#selectOPM").css("display", "none");
            $("#newOPMContainer").css("display", "none");
            $("#selectLead").css("display", "none");
            $("#btnCancel").hide();
            CreateOPMPostData.SourceTicketId = "<%=ticketId%>";
            Model.RecordId = CreateOPMPostData.SourceTicketId;
            setDatesModel(CreateOPMPostData.SourceTicketId);
            CreateOPMPostData.SourceModule = "<%=moduleName%>";
            $("#titlehd").text("<%=title%>");
            HeaderTitle = "<%=title%>";
            $("#btnContinue").hide();
            $("#loadpanel").dxLoadPanel("hide");
        }
    }


    function LoadTemplateAllocations(templateId, projectId, startDate, endDate) {
        $("#divAllocation").css("display", "block");
        const moduletype = projectId.substring(0, 3);
        if (moduletype == "LEM")
            CreateOPMPostData.SourceModule = "Lead";
        else
            CreateOPMPostData.SourceModule = "OPM";
        CreateOPMPostData.ProjectStartDate = new Date(startDate).toISOString();
        CreateOPMPostData.ProjectEndDate = new Date(endDate).toISOString();

        $.get("/api/rmmapi/GetTemplateAllocations?id=" + templateId + "&projectID=" + projectId + "&StartDate=" + CreateOPMPostData.ProjectStartDate + "&EndDate=" + CreateOPMPostData.ProjectEndDate, function (data, status) {
            if (data) {
                //allocationData = ;
                $("#btnSaveAllocation").dxButton('instance').option("visible", true);
                $("#btnViewAllocation").dxButton('instance').option("visible", false);
                $("#chkShowResources").show();
                $("#chkShowResources").dxCheckBox('instance').option('value', false);
                CreateOPMPostData.Allocations = data;
                var grdAllocations = $("#grdAllocations").dxDataGrid({
                    dataSource: CompactPhaseConstraints(data),
                    showBorders: true,
                    columns: [
                        {
                            dataField: 'AssignedToName',
                            caption: 'Resource',
                            //width: '25%',
                            sortOrder: 'asc',
                            visible:false,
                        },
                        {
                            dataField: 'TypeName',
                            caption: 'Role',
                            //width: '25%',
                            sortOrder: 'asc',
                        },
                        {
                            dataField: 'AllocationStartDate',
                            caption: 'Start Date',
                            dataType: 'date',
                            //width: '15%',
                            sortOrder: 'desc',
                        },
                        {
                            dataField: 'AllocationEndDate',
                            caption: 'End Date',
                            dataType: 'date',
                            //width: '15%',
                        },
                        {
                        dataField: "PctAllocation",
                        caption: "% Precon",
                        dataType: "text",
                        width: '7%',
                        cellTemplate: function (container, options) {
                            if (options.data.preconRefIds != null) {
                                $("<div id='dataId'>")
                                    .append("<span style='float: left;overflow: auto;'><a href='javascript:void(0);' onclick=OpenInternalAllocation('" + options.data.preconRefIds + "');>" + options.data.PctAllocation + "</a></span>")
                                    .appendTo(container);
                            }
                            else {
                                $("<div id='dataId'>")
                                    .append("<span style='float: left;overflow: auto;'>" + options.data.PctAllocation + "</span>")
                                    .appendTo(container);
                                }
                            }
                        },
                        {
                            dataField: "PctAllocationConst",
                            caption: "% Const.",
                            dataType: "text",
                            width: '7%',
                            cellTemplate: function (container, options) {
                                if (options.data.constRefIds != null) {
                                    $("<div id='dataId'>")
                                        .append("<span style='float: left;overflow: auto;'><a href='javascript:void(0);' onclick=OpenInternalAllocation('" + options.data.constRefIds + "');>" + options.data.PctAllocationConst + "</a></span>")
                                        .appendTo(container);
                                }
                                else {
                                    $("<div id='dataId'>")
                                        .append("<span style='float: left;overflow: auto;'>" + options.data.PctAllocationConst + "</span>")
                                        .appendTo(container);
                                }
                            }
                        },
                        {
                            dataField: "PctAllocationCloseOut",
                            caption: "% Closeout",
                            dataType: "text",
                            width: '7%',
                            cellTemplate: function (container, options) {
                                if (options.data.closeOutRefIds != null) {
                                    $("<div id='dataId'>")
                                        .append("<span style='float: left;overflow: auto;'><a href='javascript:void(0);' onclick=OpenInternalAllocation('" + options.data.closeOutRefIds + "');>" + options.data.PctAllocationCloseOut + "</a></span>")
                                        .appendTo(container);
                                }
                                else {
                                    $("<div id='dataId'>")
                                        .append("<span style='float: left;overflow: auto;'>" + options.data.PctAllocationCloseOut + "</span>")
                                        .appendTo(container);
                                }
                            }
                        }
                    ]
                });
            }
            else {
                data = [];
                grdAllocations.option('dataSource', data);
            }
            console.log(data);
            $("#loadpanel").dxLoadPanel("hide");
        });
    }

    function openDateAgent(ticketid) {
        Model.RecordId = ticketid;
        $.get("/api/rmone/GetProjectDates?TicketId=" + ticketid, function (data, status) {
            dataModel.preconStartDate = data.PreconStart == '0001-01-01T00:00:00' ? '' : data.PreconStart;
            dataModel.preconEndDate = data.PreconEnd == '0001-01-01T00:00:00' ? '' : data.PreconEnd;
            dataModel.constStartDate = data.ConstStart == '0001-01-01T00:00:00' ? '' : data.ConstStart;
            dataModel.constEndDate = data.ConstEnd == '0001-01-01T00:00:00' ? '' : data.ConstEnd;
            dataModel.closeoutStartDate = data.CloseoutStart == '0001-01-01T00:00:00' ? '' : new Date(data.CloseoutStart).toLocaleDateString('en-US');
            dataModel.closeoutEndDate = data.Closeout == '0001-01-01T00:00:00' ? '' : data.Closeout;
            dataModel.onHold = data.OnHold;
            dataModel.preconDuration = parseInt(data.PreconDuration) > 0 ? data.PreconDuration : ""; 
            dataModel.constDuration = parseInt(data.ConstDuration) > 0 ? data.ConstDuration : ""; 
            dataModel.closeOutDuration = parseInt(data.CloseOutDuration) > 0 ? data.CloseOutDuration : ""; 
            const popup = $("#popup").dxPopup("instance");
            popup.show();
            let form = $("#form").dxForm("instance");
            form.option("formData", dataModel);
            //form.itemOption("group", "caption", title);
        });
    }

    function setDatesModel(ticketid) {

        $.get("/api/rmone/GetProjectDates?TicketId=" + ticketid, function (data, status) {
            dataModel.preconStartDate = data.PreconStart == '0001-01-01T00:00:00' ? '' : new Date(data.PreconStart).toUGITDateFormat();
            dataModel.preconEndDate = data.PreconEnd == '0001-01-01T00:00:00' ? '' : new Date(data.PreconEnd).toUGITDateFormat();
            dataModel.constStartDate = data.ConstStart == '0001-01-01T00:00:00' ? '' : new Date(data.ConstStart).toUGITDateFormat();
            dataModel.constEndDate = data.ConstEnd == '0001-01-01T00:00:00' ? '' : new Date(data.ConstEnd).toUGITDateFormat();
            dataModel.closeoutStartDate = data.CloseoutStart == '0001-01-01T00:00:00' ? '' : new Date(data.CloseoutStart).toUGITDateFormat();
            dataModel.closeoutEndDate = data.Closeout == '0001-01-01T00:00:00' ? '' : new Date(data.Closeout).toUGITDateFormat();
            dataModel.onHold = data.OnHold;
            Model.RecordId = ticketid;

            if (dataModel.preconStartDate) {
                $("#PreconStartDateCss .rcorners2.whitecolor").text(dataModel.preconStartDate);
            }
            if (dataModel.preconEndDate) {
                $("#PreconEndDateCss .rcorners2.whitecolor").text(dataModel.preconEndDate);
            }
            if (dataModel.constStartDate) {
                $("#ConstStartDateCss .rcorners2.whitecolor").text(dataModel.constStartDate);
            }
            if (dataModel.constEndDate) {
                $("#ConstEndDateCss .rcorners2.whitecolor").text(dataModel.constEndDate);
            }
            if (dataModel.closeoutStartDate) {
                $("#CloseoutStartDateCss .rcorners2.whitecolor").text(dataModel.closeoutStartDate);
            }
            if (dataModel.closeoutEndDate) {
                $("#CloseoutEndDateCss .rcorners2.whitecolor").text(dataModel.closeoutEndDate);
            }
        });
    }

    function labelTemplate(iconName) {
        return (data) => $(`<div><i class='dx-icon dx-icon-${iconName}'></i>${data.text}</div>`);
    }

    function UpdateRecord() {
        if ((dataModel.preconEndDate != null && dataModel.preconEndDate != "") && (dataModel.preconStartDate == null || dataModel.preconStartDate == "")) {
            DevExpress.ui.dialog.alert("Entry of Precon Start Date is required.", "Error!");
            return;
        }
        if (dataModel.preconEndDate != null) {
            if (new Date(dataModel.preconStartDate) > new Date(dataModel.preconEndDate)) {
                DevExpress.ui.dialog.alert("Precon End Date should be after the Precon Start Date.", "Error!");
                return;
            }
        }

        if ((dataModel.constEndDate != null && dataModel.constEndDate != "") && (dataModel.constStartDate == null || dataModel.constStartDate == "")) {
            DevExpress.ui.dialog.alert("Entry of Construction Start Date is required.", "Error!");
            return;
        }
        if (new Date(dataModel.constStartDate) > new Date(dataModel.constEndDate)) {

            if (dataModel.constEndDate == null || dataModel.constEndDate == "") {
                DevExpress.ui.dialog.alert("Entry of Construction End Date is required.", "Error!");
                return;
            }
            DevExpress.ui.dialog.alert("Construction End Date should be after the Construction Start Date.", "Error!");
            return;
        }

        if (new Date(dataModel.constEndDate) > new Date(dataModel.closeoutEndDate)) {
            DevExpress.ui.dialog.alert("Closeout Date should be after the Construction End Date.", "Error!");
            return;
        }
        if (new Date(dataModel.closeoutStartDate) > new Date(dataModel.closeoutEndDate)) {
            DevExpress.ui.dialog.alert("Closeout End Date should be after the Closeout Start Date.", "Error!");
            return;
        }

        if (dataModel.constEndDate == null || dataModel.constEndDate == "") {
            dataModel.closeoutStartDate = null;
            dataModel.closeoutEndDate = null;
        }

        var arrDates = [
            ['Precon Start Date', dataModel.preconStartDate == null ? "" : dataModel.preconStartDate],
            ['Precon End Date', dataModel.preconEndDate == null ? "" : dataModel.preconEndDate],
            ['Construction Start Date', dataModel.constStartDate == null ? "" : dataModel.constStartDate],
            ['Construction End Date', dataModel.constEndDate == null ? "" : dataModel.constEndDate],
            ['Closeout End Date', dataModel.closeoutEndDate == null ? "" : dataModel.closeoutEndDate]
        ];
        for (let i = 0; i < arrDates.length; i++) {
            let newdate = new Date(arrDates[i][1]);
            if (arrDates[i][1] != "") {
                if (newdate == 'Invalid Date' || String(newdate.getFullYear()).length > 4) {
                    DevExpress.ui.dialog.alert("Please enter a valid " + arrDates[i][0]);
                    return;
                }
                Model.Fields[i].Value = newdate.toLocaleDateString('en-US');
            }
            else {
                Model.Fields[i].Value = "";
            }
        }

        $("#loadpanel").dxLoadPanel("show");
        Model.Fields[5].Value = dataModel.onHold == true ? '1' : '0';
        $.ajax({
            type: "POST",
            url: "<%= rmoneControllerUrl %>UpdateRecord",
            data: JSON.stringify(Model),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (message) {
                if (message.Status == true) {
                    //$("#toast").dxToast("show");
                    const popup = $("#popup").dxPopup("instance");
                    popup.hide();
                    $("#PreconStartDateCss .rcorners2.whitecolor").text(isDateValid(dataModel.preconStartDate) ? new Date(dataModel.preconStartDate).toUGITDateFormat() : '');
                    $("#PreconEndDateCss .rcorners2.whitecolor").text(isDateValid(dataModel.preconEndDate) ? new Date(dataModel.preconEndDate).toUGITDateFormat() : '');
                    $("#ConstStartDateCss .rcorners2.whitecolor").text(isDateValid(dataModel.constStartDate) ? new Date(dataModel.constStartDate).toUGITDateFormat() : '');
                    $("#ConstEndDateCss .rcorners2.whitecolor").text(isDateValid(dataModel.constEndDate) ? new Date(dataModel.constEndDate).toUGITDateFormat() : '');
                    $("#CloseoutStartDateCss .rcorners2.whitecolor").text(isDateValid(dataModel.closeoutStartDate) ? new Date(dataModel.closeoutStartDate).toUGITDateFormat() : '');
                    $("#CloseoutEndDateCss .rcorners2.whitecolor").text(isDateValid(dataModel.closeoutEndDate) ? new Date(dataModel.closeoutEndDate).toUGITDateFormat() : '');
                    setTimeout(() => {
                        if (CreateOPMPostData?.TemplateID != null && CreateOPMPostData?.TemplateID != "") {
                            LoadTemplateAllocations(CreateOPMPostData.TemplateID, CreateOPMPostData.SourceTicketId, dataModel.preconStartDate, dataModel.closeoutEndDate);
                        }
                    }, 1000);
                    $("#loadpanel").dxLoadPanel("hide");
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
            }
        });
    }

    function CalculatePctAllocation(dataRow, minDate, maxDate) {
        let totalPercentage = 0;

        let preconStartDate = new Date(dataModel.preconStartDate);

        let closeoutEndDate = new Date(dataModel.closeoutEndDate);

        dataRow = dataRow.filter(x => new Date(x.AllocationEndDate) >= preconStartDate && new Date(x.AllocationStartDate) <= closeoutEndDate);
        /*$.each(dataRow, function (index, e) {
            let allocStartdate = new Date(e.AllocationStartDate);
            let allocEnddate = new Date(e.AllocationEndDate);
            if (allocStartdate < minDate) {
                minDate = allocStartdate;
            }
            if (allocEnddate > maxDate) {
                maxDate = allocEnddate;
            }
        });*/
        if (minDate < preconStartDate) {
            minDate = preconStartDate;
        }
        if (maxDate > closeoutEndDate) {
            maxDate = closeoutEndDate;
        }
        let milli_secs_total = maxDate.getTime() - minDate.getTime();

        // Convert the milli seconds to Days 
        var totalDays = (milli_secs_total / (1000 * 3600 * 24)) + 1;

        $.each(dataRow, function (index, e) {
            let allocStartdate = new Date(e.AllocationStartDate);
            let allocEnddate = new Date(e.AllocationEndDate);
            if (allocStartdate < minDate) {
                allocStartdate = minDate;
            }
            if (allocEnddate > maxDate) {
                allocEnddate = maxDate;
            }
            var milli_secs = allocEnddate.getTime() - allocStartdate.getTime();

            // Convert the milli seconds to Days 
            var daysDiff = (milli_secs / (1000 * 3600 * 24)) + 1;

            let pctAlloc = parseInt(e.PctAllocation);
            totalPercentage += pctAlloc * daysDiff;
        });

        return totalDays > 0 ? Math.ceil(totalPercentage / totalDays) : 0;
    }

    function OpenInternalAllocation(refIds) {
        let ids = refIds.split(";");
        let globaldata = CreateOPMPostData.Allocations;
        if (ids != null && ids.length > 0) {
            let gdata = globaldata.filter(x => ids.includes(String(x.ID)));
            const popupContentTemplate1 = function () {
                let container = $("<div>");
                let datagrid = $("<div id='InternalAllocationGrid'>").dxDataGrid({
                    dataSource: gdata,
                    ID: "grdTemplate",
                    editing: {
                        mode: "cell",
                        allowEditing: false,
                        allowUpdating: false
                    },
                    sorting: {
                        mode: "multiple" // or "multiple" | "none"
                    },
                    scrolling: {
                        mode: 'infinite',
                    },
                    columns: [
                        {
                            dataField: "TypeName",
                            dataType: "text",
                            allowEditing: false,
                            caption: "Role",
                            sortIndex: "1",
                            sortOrder: "asc",
                            width: "40%",
                            cssClass: "cls",
                        },
                        {
                            dataField: "Type",
                            dataType: "text",
                            visible: false
                        },
                        {
                            dataField: "AllocationStartDate",
                            caption: "Start Date",
                            dataType: "date",
                            width: "25%",
                            alignment: 'center',
                            cssClass: "v-align",
                            allowEditing: true,
                            format: 'MMM d, yyyy',
                            sortIndex: "2",
                            sortOrder: "asc",
                        },
                        {
                            dataField: "AllocationEndDate",
                            caption: "End Date",
                            dataType: "date",
                            alignment: 'center',
                            cssClass: "v-align",
                            width: "25%",
                            allowEditing: true,
                            format: 'MMM d, yyyy',
                        },
                        {
                            dataField: "PctAllocation",
                            caption: "% Alloc",
                            dataType: "text",
                            width: "10%"
                        },
                    ],
                    showBorders: true,
                    showRowLines: true,
                });
                container.append(datagrid);
                return container;
            };

            const popup = $("#InternalAllocationGridDialog").dxPopup({
                contentTemplate: popupContentTemplate1,
                width: "700",
                height: "auto",
                showTitle: true,
                title: "View Allocations",
                visible: false,
                dragEnabled: true,
                hideOnOutsideClick: true,
                showCloseButton: true,
                position: {
                    at: 'center',
                    my: 'center',
                },
                onHiding: function () {

                }
            }).dxPopup('instance');

            popup.option({
                contentTemplate: () => popupContentTemplate1()

            });
            popup.show();
        }
    }

    function CompactPhaseConstraints(pData) {
        compactTempData = [];
        let tempData = JSON.parse(JSON.stringify(pData));
        //compactTempData = Object.create(globaldata);
        let isDateInMultiPhase = false;
        $.each(tempData, function (index, e) {
            let data1 = JSON.parse(JSON.stringify(tempData.filter(x => x.AssignedTo == e.AssignedTo && x.Type == e.Type)));
            let internalPhaseData = [];
            let constStartDate = new Date(dataModel.constStartDate);
            let constEndDate = new Date(dataModel.constEndDate);

            let preconStartDate = new Date(dataModel.preconStartDate);
            let preconEndDate = new Date(dataModel.preconEndDate);

            let closeoutStartDate = new Date(dataModel.closeoutStartDate);
            let closeoutEndDate = new Date(dataModel.closeoutEndDate);

            if (!isDateValid(constStartDate) && !isDateValid(constEndDate) && isDateValid(preconStartDate) && isDateValid(preconEndDate)) {
                constStartDate = preconStartDate;
                constEndDate = preconEndDate;
            }

            let internalPrecon = JSON.parse(JSON.stringify(data1.filter(x => new Date(x.AllocationStartDate) < constStartDate)));
            let internalConst = JSON.parse(JSON.stringify(data1.filter(x => new Date(x.AllocationStartDate) >= constStartDate && new Date(x.AllocationEndDate) <= constEndDate)));
            let internalCloseOut = JSON.parse(JSON.stringify(data1.filter(x => new Date(x.AllocationStartDate) > constEndDate)));
            if (internalPrecon.length > 1) {
                let internalPreconTemp = JSON.parse(JSON.stringify(internalPrecon[0]));
                let ids = [];
                let endDateForPctCal = new Date(Math.max.apply(null, internalPrecon.filter(x => new Date(x.AllocationEndDate) >= preconStartDate).map(x => new Date(x.AllocationEndDate))));
                let startDateForPctCal = new Date(Math.min.apply(null, internalPrecon.filter(x => new Date(x.AllocationEndDate) >= preconStartDate).map(x => new Date(x.AllocationStartDate))));
                internalPreconTemp.AllocationEndDate = new Date(Math.max.apply(null, internalPrecon.map(x => new Date(x.AllocationEndDate))));
                internalPreconTemp.AllocationStartDate = new Date(Math.min.apply(null, internalPrecon.map(x => new Date(x.AllocationStartDate))));
                let percentage = CalculatePctAllocation(internalPrecon, startDateForPctCal, endDateForPctCal);
                internalPreconTemp.PctAllocation = percentage <= 0 ? Math.max.apply(null, internalPrecon.map(x => parseInt(x.PctAllocation))) : percentage;

                $.each(internalPrecon, function (index, e) {
                    ids.push(e.ID);
                });
                internalPreconTemp.preconRefIds = ids.join(';');
                internalPhaseData.push(internalPreconTemp);
            }
            else if (internalPrecon.length) {
                internalPhaseData.push(internalPrecon[0]);
            }

            if (internalConst.length > 1) {
                let internalConstTemp = JSON.parse(JSON.stringify(internalConst[0]));
                let ids = [];
                internalConstTemp.AllocationEndDate = new Date(Math.max.apply(null, internalConst.map(x => new Date(x.AllocationEndDate))));
                internalConstTemp.AllocationStartDate = new Date(Math.min.apply(null, internalConst.map(x => new Date(x.AllocationStartDate))));
                internalConstTemp.PctAllocation = CalculatePctAllocation(internalConst, internalConstTemp.AllocationStartDate, internalConstTemp.AllocationEndDate);
                $.each(internalConst, function (index, e) {
                    ids.push(e.ID);
                });
                internalConstTemp.constRefIds = ids.join(';');
                internalPhaseData.push(internalConstTemp);
            }
            else if (internalConst.length) {
                internalPhaseData.push(internalConst[0]);
            }

            if (internalCloseOut.length > 1) {
                let internalCloseOutTemp = JSON.parse(JSON.stringify(internalCloseOut[0]));
                let ids = [];
                let endDateForPctCal = new Date(Math.max.apply(null, internalCloseOut.filter(x => new Date(x.AllocationStartDate) <= closeoutEndDate).map(x => new Date(x.AllocationEndDate))));
                let startDateForPctCal = new Date(Math.min.apply(null, internalCloseOut.filter(x => new Date(x.AllocationStartDate) <= closeoutEndDate).map(x => new Date(x.AllocationStartDate))));
                internalCloseOutTemp.AllocationEndDate = new Date(Math.max.apply(null, internalCloseOut.map(x => new Date(x.AllocationEndDate))));
                internalCloseOutTemp.AllocationStartDate = new Date(Math.min.apply(null, internalCloseOut.map(x => new Date(x.AllocationStartDate))));

                let percentage = CalculatePctAllocation(internalCloseOut, startDateForPctCal, endDateForPctCal);
                internalCloseOutTemp.PctAllocation = percentage <= 0 ? Math.max.apply(null, internalCloseOut.map(x => parseInt(x.PctAllocation))) : percentage;

                $.each(internalCloseOut, function (index, e) {
                    ids.push(e.ID);
                });
                internalCloseOutTemp.closeOutRefIds = ids.join(';');
                internalPhaseData.push(internalCloseOutTemp);
            }
            else if (internalCloseOut.length) {
                internalPhaseData.push(internalCloseOut[0]);
            }
            let remainingData = data1.filter(x => x.AllocationStartDate == "" && x.AllocationEndDate == "");
            if (remainingData.length > 0) {
                internalPhaseData.push(remainingData[0]);
            }
            let data = JSON.parse(JSON.stringify(internalPhaseData));
            let odata = JSON.parse(JSON.stringify(internalPhaseData));
            if (data.length > 0) {
                if (data.length > 1) {
                    data[0].AllocationEndDate = data[data.length - 1].AllocationEndDate
                    if (data.length == 2) {
                        let startDate = new Date(data[0].AllocationStartDate);
                        let endDate = new Date(data[0].AllocationEndDate);
                        let preconStartDate = new Date(dataModel.preconStartDate);
                        let preconEndDate = new Date(dataModel.preconEndDate);

                        let constStartDate = new Date(dataModel.constStartDate);
                        let constEndDate = new Date(dataModel.constEndDate);

                        if (!isDateValid(preconStartDate) && startDate < constStartDate) {
                            preconStartDate = startDate;
                        }
                        if (!isDateValid(preconEndDate) && isDateValid(constStartDate)) {
                            preconEndDate = constStartDate.addDays(-1);
                        }
                        if (startDate < constStartDate && endDate > constEndDate) {
                            data[0].PctAllocation = data[0].PctAllocation;
                            if (new Date(odata[0].AllocationEndDate) < preconEndDate || new Date(odata[0].AllocationStartDate) > preconStartDate) {
                                //let percentage = CalculatePctAllocation([odata[0]], preconStartDate, preconEndDate);
                                //data[0].PctAllocation = percentage <= 0 ? Math.max.apply(null, data.map(x => parseInt(x.PctAllocation))) : percentage;
                                data[0].preconRefIds = data[0].preconRefIds == null || data[0].preconRefIds == '' ? data[0].ID : data[0].preconRefIds;
                            }

                            data[0].PctAllocationCloseOut = data[1].PctAllocation;
                            if (new Date(odata[1].AllocationEndDate) < closeoutEndDate || new Date(odata[1].AllocationStartDate) > closeoutStartDate) {
                                //let percentage = CalculatePctAllocation([odata[1]], closeoutStartDate, closeoutEndDate);
                                //data[0].PctAllocationCloseOut = percentage <= 0 ? Math.max.apply(null, data.map(x => parseInt(x.PctAllocation))) : percentage;
                                data[0].closeOutRefIds = data[1].closeOutRefIds == null || data[1].closeOutRefIds == '' ? data[1].ID : data[1].closeOutRefIds;//data[1].ID;
                            }

                            data[0].PctAllocationConst = 0;
                            data[0].PreconId = data[0].ID;
                            data[0].ConstId = 0;
                            data[0].CloseOutId = data[1].ID;
                            if (data[1].closeOutRefIds != null)
                                data[0].closeOutRefIds = data[1].closeOutRefIds;
                        }
                        else if (startDate < constStartDate) {

                            data[0].PctAllocation = data[0].PctAllocation;
                            if (new Date(odata[0].AllocationEndDate) < preconEndDate || new Date(odata[0].AllocationStartDate) > preconStartDate) {
                                //let percentage = CalculatePctAllocation([odata[0]], preconStartDate, preconEndDate);
                                //data[0].PctAllocation = percentage <= 0 ? Math.max.apply(null, data.map(x => parseInt(x.PctAllocation))) : percentage;
                                data[0].preconRefIds = data[0].preconRefIds == null || data[0].preconRefIds == '' ? data[0].ID : data[0].preconRefIds;
                            }

                            data[0].PctAllocationConst = data[1].PctAllocation;
                            if (new Date(odata[1].AllocationEndDate) < constEndDate || new Date(odata[1].AllocationStartDate) > constStartDate) {
                                //data[0].PctAllocationConst = CalculatePctAllocation([odata[1]], constStartDate, constEndDate);
                                data[0].constRefIds = data[1].constRefIds == null || data[1].constRefIds == '' ? data[1].ID : data[1].constRefIds; //data[1].ID;
                            }

                            data[0].PctAllocationCloseOut = 0;
                            data[0].PreconId = data[0].ID;
                            data[0].ConstId = data[1].ID;
                            data[0].CloseOutId = 0;
                            if (data[1].constRefIds != null)
                                data[0].constRefIds = data[1].constRefIds;
                        }
                        else {

                            data[0].PctAllocationConst = data[0].PctAllocation;
                            if (new Date(odata[0].AllocationEndDate) < constEndDate || new Date(odata[0].AllocationStartDate) > constStartDate) {
                                //data[0].PctAllocationConst = CalculatePctAllocation([odata[0]], constStartDate, constEndDate);
                                data[0].constRefIds = data[0].constRefIds == null || data[0].constRefIds == '' ? data[0].ID : data[0].constRefIds;
                            }

                            data[0].PctAllocationCloseOut = data[1].PctAllocation;
                            if (new Date(odata[1].AllocationEndDate) < closeoutEndDate || new Date(odata[1].AllocationStartDate) > closeoutStartDate) {
                                //let percentage = CalculatePctAllocation([odata[1]], closeoutStartDate, closeoutEndDate);
                                //data[0].PctAllocationCloseOut = percentage <= 0 ? Math.max.apply(null, data.map(x => parseInt(x.PctAllocation))) : percentage;
                                data[0].closeOutRefIds = data[1].closeOutRefIds == null || data[1].closeOutRefIds == '' ? data[1].ID : data[1].closeOutRefIds; //data[1].ID;
                            }

                            data[0].PctAllocation = 0;
                            data[0].PreconId = 0;
                            data[0].ConstId = data[0].ID;
                            data[0].CloseOutId = data[1].ID;
                            if (data[1].closeOutRefIds != null)
                                data[0].closeOutRefIds = data[1].closeOutRefIds;
                        }
                    }
                    else {
                        data[0].PctAllocation = data[0].PctAllocation;
                        if (new Date(odata[0].AllocationEndDate) < preconEndDate || new Date(odata[0].AllocationStartDate) > preconStartDate) {
                            //let percentage = CalculatePctAllocation([odata[0]], preconStartDate, preconEndDate);
                            //data[0].PctAllocation = percentage <= 0 ? Math.max.apply(null, data.map(x => parseInt(x.PctAllocation))) : percentage;
                            data[0].preconRefIds = data[0].preconRefIds == null || data[0].preconRefIds == '' ? data[0].ID : data[0].preconRefIds;
                        }

                        data[0].PctAllocationConst = data[1].PctAllocation;
                        if (new Date(odata[1].AllocationEndDate) < constEndDate || new Date(odata[1].AllocationStartDate) > constStartDate) {
                            //data[0].PctAllocationConst = CalculatePctAllocation([odata[1]], constStartDate, constEndDate);
                            data[0].constRefIds = data[1].constRefIds == null || data[1].constRefIds == '' ? data[1].ID : data[1].constRefIds; //data[1].ID;
                        }

                        data[0].PctAllocationCloseOut = data[2].PctAllocation;
                        if (new Date(odata[2].AllocationEndDate) < closeoutEndDate || new Date(odata[2].AllocationStartDate) > closeoutStartDate) {
                            //let percentage = CalculatePctAllocation([odata[2]], closeoutStartDate, closeoutEndDate);
                            //data[0].PctAllocationCloseOut = percentage <= 0 ? Math.max.apply(null, data.map(x => parseInt(x.PctAllocation))) : percentage;
                            data[0].closeOutRefIds = data[2].closeOutRefIds == null || data[2].closeOutRefIds == '' ? data[2].ID : data[2].closeOutRefIds; //data[2].ID;
                        }

                        data[0].PreconId = data[0].ID;
                        data[0].ConstId = data[1].ID;
                        data[0].CloseOutId = data[2].ID;
                        if (data[1].constRefIds != null)
                            data[0].constRefIds = data[1].constRefIds;
                        if (data[2].closeOutRefIds != null)
                            data[0].closeOutRefIds = data[2].closeOutRefIds;
                    }
                }
                else {
                    let startDate = new Date(data[0].AllocationStartDate);
                    let endDate = new Date(data[0].AllocationEndDate);
                    let preconStartDate = new Date(dataModel.preconStartDate);
                    let preconEndDate = new Date(dataModel.preconEndDate);

                    let constStartDate = new Date(dataModel.constStartDate);
                    let constEndDate = new Date(dataModel.constEndDate);

                    if (!isDateValid(preconStartDate) && startDate < constStartDate) {
                        preconStartDate = startDate;
                    }
                    if (!isDateValid(preconEndDate) && isDateValid(constStartDate)) {
                        preconEndDate = constStartDate.addDays(-1);
                    }
                    if (startDate >= constStartDate && endDate <= constEndDate) {

                        data[0].PctAllocationConst = data[0].PctAllocation;
                        //if (new Date(odata[0].AllocationEndDate) < constEndDate || new Date(odata[0].AllocationStartDate) > constStartDate) {
                        //    data[0].PctAllocationConst = CalculatePctAllocation([odata[0]], constStartDate, constEndDate);
                        //    data[0].constRefIds = data[0].constRefIds == null || data[0].constRefIds == '' ? data[0].ID : data[0].constRefIds; //data[0].ID;
                        //}

                        data[0].PctAllocation = 0;
                        data[0].PctAllocationCloseOut = 0;
                        data[0].PreconId = 0;
                        data[0].ConstId = data[0].ID;
                        data[0].CloseOutId = 0;
                    }
                    else if (constEndDate < startDate) {
                        data[0].PctAllocationCloseOut = data[0].PctAllocation;
                        //if (new Date(odata[0].AllocationEndDate) < closeoutEndDate || new Date(odata[0].AllocationStartDate) > closeoutStartDate) {
                        //    let percentage = CalculatePctAllocation([odata[0]], closeoutStartDate, closeoutEndDate);
                        //    data[0].PctAllocationCloseOut = percentage <= 0 ? data[0].PctAllocationCloseOut : percentage;
                        //    data[0].closeOutRefIds = data[0].closeOutRefIds == null || data[0].closeOutRefIds == '' ? data[0].ID : data[0].closeOutRefIds; //data[0].ID;
                        //}
                        data[0].PctAllocation = 0;
                        data[0].PctAllocationConst = 0;
                        data[0].PreconId = 0;
                        data[0].ConstId = 0;
                        data[0].CloseOutId = data[0].ID;
                    }
                    else {
                        //if (new Date(odata[0].AllocationEndDate) < preconEndDate || new Date(odata[0].AllocationStartDate) > preconStartDate) {
                        //    let percentage = CalculatePctAllocation([odata[0]], preconStartDate, preconEndDate);
                        //    data[0].PctAllocation = percentage <= 0 ? data[0].PctAllocation : percentage;
                        //    data[0].preconRefIds = data[0].preconRefIds == null || data[0].preconRefIds == '' ? data[0].ID : data[0].preconRefIds; //data[0].ID;
                        //}
                        data[0].PctAllocationConst = 0;
                        data[0].PctAllocationCloseOut = 0;
                        data[0].PreconId = data[0].ID;
                        data[0].ConstId = 0;
                        data[0].CloseOutId = 0;
                    }
                }
                if (compactTempData.length == 0) {
                    compactTempData.push(JSON.parse(JSON.stringify(data[0])));
                }
                else if (compactTempData.filter(x => x.AssignedTo == e.AssignedTo && x.Type == e.Type).length == 0) {
                    compactTempData.push(JSON.parse(JSON.stringify(data[0])));
                }
            }
        });
        return compactTempData;
    }

    function openProjectSummaryPage(obj) {
        var ticketid = $(obj).attr("ticketid");
        var ticketitle = $(obj).attr("ticketTitle");
        var params = "TicketId=" + ticketid + "&tickettitle=" + ticketitle;
        window.parent.UgitOpenPopupDialog('<%=NewProjectSummaryPageUrl%>', params, ticketitle, '95', '95', false, "<%=Server.UrlEncode(Request.Url.AbsolutePath) %>");

    }

    function ShowAllSimilarProjects() {
        
        const similarProjectPopup = $("#popupContainer").dxPopup({
            title: "All Similar Projects",
            width: "95%",
            height: "90%",
            visible: true,
            scrolling: true,
            dragEnabled: true,
            resizeEnabled: true,
            contentTemplate: function (contentElement) {
                contentElement.append(
                    $("<div id='griddisplayalldata' />").dxDataGrid({
                        dataSource: allprojects,
                        keyExpr: 'TicketId',
                        height: "99%",
                        selection: {
                            mode: 'single',
                        },
                        scrolling: {
                            mode: 'Standard',
                        },
                        showBorders: true,
                        columns: [
                            {
                                dataField: "TicketId",
                                caption: "Ticket ID",
                                width: "10%"
                            },
                            {
                                dataField: 'Title',
                                caption: 'Title',
                            },
                            {
                                dataField: 'Duration',
                                caption: 'Duration',
                            },
                            {
                                dataField: 'Resources',
                                caption: 'Resources',
                            },
                            {
                                dataField: 'Hrs',
                                caption: 'Hours',
                            },
                            {
                                dataField: 'Score',
                                caption: 'Similarity Score',
                            }
                        ],
                        onSelectionChanged(selectedItems) {

                            similarProjectData.forEach(item => item.isSelected = false);
                            const data = selectedItems.selectedRowsData[0];
                            data.isSelected = true;
                            similarProjectData.unshift(data);// Adding data at the first position
                            var similarProjectTiles = $("#similarProjectList").dxTileView("instance");
                            similarProjectTiles.option("dataSource", similarProjectData);
                            //similarProjectTiles.option("height", "240px");
                            similarProjectTiles.repaint();
                            $("#popupContainer").dxPopup("instance").hide();

                            $.get("/api/rmmapi/GetProjectAllocations?projectID=" + data.TicketId, function (response, status) {
                                if (response.Allocations.length > 0) {

                                    CreateOPMPostData.Allocations = response.Allocations;
                                    var newData = CompactPhaseConstraints(response.Allocations);
                                    var grdAllocations = $("#grdAllocations").dxDataGrid({
                                        dataSource: newData,
                                        showBorders: true,
                                        columns: [
                                            {
                                                dataField: 'TypeName',
                                                caption: 'Role',
                                                width: '40%',
                                                sortOrder: "asc"
                                            },
                                            {
                                                dataField: 'AllocationStartDate',
                                                caption: 'Start Date',
                                                dataType: 'date',
                                                width: '20%',
                                                sortOrder: "desc"
                                            },
                                            {
                                                dataField: 'AllocationEndDate',
                                                caption: 'End Date',
                                                dataType: 'date',
                                                width: '20%'
                                            },
                                            {
                                                dataField: "PctAllocation",
                                                caption: "% Precon",
                                                dataType: "text",
                                                width: '7%'
                                            },
                                            {
                                                dataField: "PctAllocationConst",
                                                caption: "% Const.",
                                                dataType: "text",
                                                width: '7%'
                                            },
                                            {
                                                dataField: "PctAllocationCloseOut",
                                                caption: "% Closeout",
                                                dataType: "text",
                                                width: '7%'
                                            }
                                        ]

                                    });

                                }
                            });
                        }
                    })
                )
            },
        });
        $('#popupContainer').show();
    }
</script>
<script type="text/javascript" data-v="<%=uGovernIT.Utility.UGITUtility.AssemblyVersion %>">
    // JavaScript code to set text in the span after the whole page is loaded
    window.onload = function () {
        
        $('.ui-dialog-title').text(HeaderTitle);
    };
</script>
<div class="container p-1">
    <div class="row">
        <div style="float: right">
            <div id="btnBack" style="display: none;" class="btnAddNew">
            </div>
        </div>
    </div>
    <div class="row pt-2">
        <div class="col-md-12">
            <div id="leadContainer">
                <div id="selectLead" style="display: none">
                </div>
            </div>
            <div id="opmContainer">
                <div id="selectOPM" style="display: none">
                </div>
            </div>
        </div>
    </div>

    <div id="newOPMContainer" style="display: none" class="row">
        <div class="container">
            <div class="row pt-3">
                <div class="col-md-9">
                    <div id="txtProjectName"></div>
                </div>
            </div>
            <div class="row pt-3">
                <div class="col-md-3">
                    <div id="ddlClient"></div>
                </div>
                <div class="col-md-3">
                    <div id="ddlType"></div>
                </div>
                <div class="col-md-3">
                    <div id="txtCMIC"></div>
                </div>
                <div class="col-md-3">
                    <div id="txtNCO"></div>
                </div>
            </div>
            <div class="row pt-3">
                <div class="col-md-3">
                    <div id="dteDueDate"></div>
                </div>
                <div class="col-md-3">
                    <div id="dteConstStart"></div>
                </div>
                <div class="col-md-3">
                    <div id="ddlContractType"></div>
                </div>
                <div class="col-md-3">
                    <div id="txtContractValue"></div>
                </div>

            </div>
            <div class="row pt-3">
                <div class="col-md-3">
                    <div id="txtAddress"></div>
                </div>
                <div class="col-md-3">
                    <div id="txtCity"></div>
                </div>
                <div class="col-md-3">
                    <div id="ddlState"></div>
                </div>
                <div class="col-md-3">
                    <div id="txtZip"></div>
                </div>
            </div>
            <div class="row pt-3">
                <div class="col-md-2">
                    <div id="txtNetRentableSqft"></div>
                </div>
                <div class="col-md-2">
                    <div id="txtGrossSqft"></div>
                </div>
                <div class="col-md-2">
                    <div id="ddlComplexity"></div>
                </div>
                <div class="col-md-2">
                    <div id="ddlOPMType"></div>
                </div>
                <div class="col-md-2">
                    <div id="ddlWinLikelyHood"></div>
                </div>
            </div>
            <div class="row pt-3">
                <div class="col-md-12">
                    <div id="txtDescription"></div>
                </div>

            </div>
            <div class="row pt-3">

                <div class="col-md-2" style="float: right;">
                    <div id="btnSummarySave" class="btnAddNew"></div>
                </div>
                <div class="col-md-1" style="float: right; text-align: right;">
                    <div class="pt-2"><b>1/2</b></div>
                </div>
            </div>
        </div>
    </div>

    <div id="summaryContainer" style="display: none" class="row">
        <div class="container">
            <div class="row p-2">
                <div class="titleHeader">
                    <p id="titlehd"></p>
                </div>
            </div>
            <div class="row p-2">
                <div id="btnShowSimilarProject"></div>
            </div>
            <div class="row">
                <div id="SimilarProjectContainer" class="similarprojectrow" style="display:none">
                    <div id="similarProjectList" ></div>
                    <div style="vertical-align: middle; margin-top: 93px; padding: 0px !important; width:5%">
                        <img id="openGridBtn" src="/content/images/moreoptions_blue.png" onclick="ShowAllSimilarProjects()" />
                    </div>
                </div>
            </div>
            <div class="row p-2">
                <div>
                    <div id="ddlTemplate"></div>
                </div>
            </div>
            <div class="row p-2">
                <div id="divPrecon" class="col-md-4 col-xs-6 col-sm-4 boxAlignPrecon">
                    <div class="ticket-stage" style="color: #52bed9;">Precon</div>
                    <div class="whiteSpace-1">
                        <span id="PreconStartDateCss">
                            <span class='rcorners2 whitecolor'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>
                        </span>
                        <span class="vl" style="border-left: 2px solid #52bed9;"></span>
                        <span id="PreconEndDateCss">
                            <span class='rcorners2 whitecolor'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                        </span>

                    </div>
                </div>
                <div id="divConst" class="col-md-4 col-xs-6 col-sm-4 boxAlignConst">
                    <div class="ticket-stage" style="color: #005c9b">Const.</div>
                    <div class="whiteSpace-1">
                        <span id="ConstStartDateCss"><span class='rcorners2 whitecolor'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span></span>
                        <span class="vl" style="border-left: 2px solid #005c9b;"></span>
                        <span id="ConstEndDateCss"><span class='rcorners2 whitecolor'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span></span>

                    </div>
                </div>
                <div id="divCloseout" class="col-md-4 col-xs-6 col-sm-4 boxAlignClosedOut">
                    <div class="ticket-stage" style="color: #351b82;">Closeout</div>
                    <div class="whiteSpace-1">
                        <span id="CloseoutStartDateCss"><span class='rcorners2 whitecolor'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span></span>
                        <span class="vl" style="border-left: 2px solid #351b82;"></span>
                        <span id="CloseoutEndDateCss"><span class='rcorners2 whitecolor'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span></span>
                    </div>
                </div>
            </div>
            <div class="row pt-3 pl-2">
                <div id="divAllocation" style="display: none">
                    <div id="grdAllocations">
                    </div>
                </div>
            </div>
            <div class="row pt-2 pl-2">
                <div>
                    <div id="chkShowResources" style="display:none"></div>
                </div>
                <div class="button_row">
                    <div id="btnContinue" class="btnAddNew"></div>
                    <div id="btnSaveAllocation" class="btnAddNew"></div>
                    <div id="btnViewAllocation" class="btnAddNew"></div>
                     <div id="btnCancel" class="btnAddNew"></div>
                </div>
            </div>
        </div>
    </div>
</div>


<div id="saveButton" class="btnAddNew mb-3" style="float:right;font-size:14px;margin-right:-5px;">
</div>
<div id="popup"></div>
<div id="loadpanel"></div>
<div id="toast"></div>
<div id="popupContainer" style="display: none;">
    <!-- The DevExpress DataGrid will be rendered here -->

</div>
<div id="InternalAllocationGridDialog"></div>

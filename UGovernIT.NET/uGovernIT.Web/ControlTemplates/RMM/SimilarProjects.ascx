<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SimilarProjects.ascx.cs" Inherits="uGovernIT.Web.SimilarProjects" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .lblShowMore {
        font-size: 29px;
        cursor: pointer;
        line-height: 1;
        padding-bottom: 18px;
        padding-top: 7px;
        margin-left: 0;
    }
</style>

<script data-v="<%=UGITUtility.AssemblyVersion %>">
    var TicketId = '';
    var ProjectID = '<%=ProjectID%>';
    var SearchData = '<%=SearchData%>';
    var Module = '<%=Module%>';
    $(function () {
        $(document).ready(function () {
            $("#loadpanel").dxLoadPanel({
                message: "RM One AI agent is finding similar projects...",
                visible: true
            });
            $.get("/api/OPMWizard/GetSimilarProjects?TicketId=" + ProjectID + "&useMinimumSimilarityScore=true", function (data, status) {
                if (data) {
                    dxDataGrid.option('dataSource', data);
                }
                else {
                    data = [];
                    dxDataGrid.option('dataSource', data);
                }
                $("#DivProjectView").show();
            });
        });

        var dxDataGrid = $("#DivProjectView").dxDataGrid({
            showBorders: true,
            rowAlternationEnabled: true,
            selection: {
                mode: "single"
            },

            groupPanel: {
                visible: false
            },
            paging: {
                pageSize: 5
            },
            pager: {
                visible: false
                //allowedPageSizes: [5, 10, 'all'],
                //showPageSizeSelector: true,
                //showInfo: true,
                //showNavigationButtons: true,
            },
            allowColumnResizing: true,
            onContentReady: function (e) {
                $("#loadpanel").dxLoadPanel({
                    message: "RM One AI agent is finding similar projects...",
                    visible: false
                });
            },
            columns: [
                {
                    dataField: "TicketId",
                    caption: "Ticket ID",
                    width: "10%"
                },
                {
                    dataField: 'Title',
                    caption: 'Title',
                    width: "20%"
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
                    width: "15%"

                }
            ],
            onRowClick: function (e) {
                //alert(e.key.TicketId);                
                TicketId = e.key.TicketId;
                $("#btnAllocations").show();
            }
        }).dxDataGrid('instance');


        $("#btnPreviewAllocations").dxButton({
            text: "Preview Allocations",
            icon: "/content/Images/preview-allocations.png",
            focusStateEnabled: false,
            onClick: function (e) {
                if (TicketId == '') {
                    alert('Select a Record');
                }
                else {
                    var gridurl = "/Layouts/uGovernIT/DelegateControl.aspx?control=similarprojectpreviewallocations&ActualProjectId=" + ProjectID + "&ticketId=" + TicketId + "&module=" + Module;
                    window.parent.UgitOpenPopupDialog(gridurl, "", 'Preview Allocations', '90', '90', 0, escape("<%= Request.Url.AbsolutePath %>"));
                    TicketId = '';
                }
            }
        });

        $("#btnAutofillAllocations").dxButton({
            text: "Autofill Allocations",
            icon: "/content/Images/autofill-allocations.png",
            focusStateEnabled: false,
            onClick: function (e) {
                alert('This feature will be available soon.');
            }
        });

        $("#lblShowMore").click(function () {            
            dxDataGrid.option('pager.visible', true);
            $("#lblShowMore").hide();
        });
    });


</script>
<div id="loadpanel"></div>
<div class="pt-3">
    <div id="DivProjectView"></div>    
    <div class="text-right">
        <label id="lblShowMore" class="lblShowMore" title="Show more">...</label>
    </div>
    <div id="btnAllocations" class="pb-3 text-center" style="display:none;">
        <div id="btnPreviewAllocations" class="btnAddNew"></div>
        <div id="btnAutofillAllocations" class="btnAddNew"></div>
    </div>
</div>

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManageAllocationTemplates.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.RMM.ManageAllocationTemplates" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .parentNode{
        display: flex;
        justify-content: space-evenly;
    }
    #divAllocationGrid .dx-datagrid-headers {
        color: black;
        font-weight: 500;
    }
    #divAllocationGrid .dx-datagrid{  
        font-family: 'Roboto', sans-serif !important;
        font-size:14px;
    }  
</style>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    var IsResourceAdmin = "<%=IsResourceAdmin%>" == "True" ? true : false;
    var templateData = [];
    $.get("/api/rmmapi/GetProjectTemplates", function (data, status) {
        templateData = data;
        $("#divAllocationGrid").dxDataGrid('instance').option('dataSource', templateData);
    });
    $(function () {
        $("#divAllocationGrid").dxDataGrid({
            dataSource: templateData,
            /*height: String(parseInt($(window.parent).height()) - 185),*/
            ID: "grdAllocationGrid",
            paging: { enabled: false, pageSize: 13 },
            filterRow: { visible: true },
            scrolling: {
                mode: "standard"
            },
            columns: [
                {
                    dataField: "Name",
                    dataType: "string",
                    caption: "Template Name",
                    sortIndex: "1",
                    sortOrder: "asc",
                    visible: true,
                    cellTemplate: function (container, options) {
                        $("<div id='dataId'>")
                            .append('<span style="float: left;overflow: auto;">' + options.value + '</span>')
                            .appendTo(container);
                        if (!IsResourceAdmin) {
                            container.attr("onclick", "EditTemplate(" + options.data.ID + ", true)");
                            container.attr("style", "cursor:pointer;");
                        }
                    }
                },
                {
                    width: "80",
                    visible: IsResourceAdmin,
                    allowFiltering: false,
                    cellTemplate: function (container, options) {
                        let rowEelement = $("<div class='parentNode' />");
                        $("<div id='rowEdit' style='text-align:center;'>")
                            .append($("<img>", { "src": "/content/images/editIcon-new.png", "ID": options.data.ID, "TemplateID": options.data.TemplateID, "EditTemplate": options.data.Name, "style": "overflow: auto;cursor: pointer;", "class": "imgEdit", "width": "20px" }))
                            .appendTo(rowEelement);
                        $("<div id='rowDelete' style='text-align:center;'>")
                            .append($("<img>", { "src": "/content/images/deleteIcon-new.png", "deleteID": options.data.ID, "TemplateID": options.data.TemplateID, "Name": options.data.Name, "style": "overflow: auto;cursor: pointer;", "class": "imgDelete", "width": "22px" }))
                            .appendTo(rowEelement);
                        container.append(rowEelement);
                    }
                }
            ],
            selection: {
                mode: "single"
            },
            showRowLines: true,
            rowAlternationEnabled: false,
            showBorders: true,
            repaintChangesOnly: true,
            onCellPrepared: function (e) {
                if (e.column.dataField === "Name") {
                    e.cellElement.css("padding-left", "20px");
                }
            }
        });
    });

    $(document).on("click", "img.imgEdit", function (e) {
        var dataid = e.target.id;
        EditTemplate(dataid, false);
    });

    $(document).on("click", "img.imgDelete", function (e) {
        var result = DevExpress.ui.dialog.confirm("Are you sure you want to delete this template?", "Confirm changes");
        result.done(function (dialogResult) {
            if (dialogResult) {
                let id = $(e.target).attr("deleteid");
                let templateName = $(e.target).attr("Name");
                $.post("/api/rmmapi/DeleteTemplateAllocation", { ID: id, Name: templateName, DeleteTemplate: true }).then(function (response) {
                    var dataGrid = $("#divAllocationGrid").dxDataGrid("instance");
                    dataGrid.option("dataSource", response);
                }, function (error) { });
            }
        });
    });

    function EditTemplate(templateId, readonly) {
        var gridurl = "/Layouts/uGovernIT/DelegateControl.aspx?control=crmtemplateallocationview&requestFromRMM=true&readonly=" + readonly + "&templateID=" + templateId;
        window.parent.UgitOpenPopupDialog(gridurl, "", 'Preview Template', '90', '90', 0, escape("<%= Request.Url.AbsolutePath %>"));
    }
</script>
<div class="container">
  <div class="row justify-content-center pt-3">
    <div class="col-md-12 col-sm-12 col-lg-12">
      <div class="d-flex justify-content-center">
        <div id="divAllocationGrid" style="width: 400px;max-height:600px;">
          <!-- Content of divAllocationGrid goes here -->
        </div>
      </div>
    </div>
  </div>
</div>



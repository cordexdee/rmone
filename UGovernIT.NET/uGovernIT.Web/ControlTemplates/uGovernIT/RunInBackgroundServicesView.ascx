<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RunInBackgroundServicesView.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.uGovernIT.RunInBackgroundServicesView" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">


    $(document).ready(function () {

        $.get("/api/RunInBackgroundServices/GetStatus", function (data, status) {

                       $("#gridContainer").dxDataGrid({
                        dataSource: data,
                        columns: ["Service", "Status","StartDate","EndDate","User"],
                        showBorders: true    });
                });



    $("#showArchive").dxButton({
        text: "ShowArchive",
        onClick: function(e) { 
            //DevExpress.ui.notify("The Weather button was clicked");

            $.get("/api/RunInBackgroundServices/GetArchiveBackgroundProcessStatus", function (data, status) {

                      var grid = $('#gridContainer').dxDataGrid('instance');
                       grid.option('dataSource', data);
                });

        }
    });

    });

         



</script>

<div class="demo-container">
    <div class="archive-container">
        <div id="showArchive" class="showArchiveBtn"></div>
    </div>

    <div class="grid-Container" style="margin-top: 20px">
        <div id="gridContainer" class="runInBg-grid"></div>
    </div>
</div>

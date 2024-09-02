<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ImportAllocationTemplate.ascx.cs" Inherits="uGovernIT.Web.ImportAllocationTemplate" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .findresource-background{
        background:none !important;
    }
    .findresource-background .dxb{
        background:#4fa1d6 !important;
        border-radius:4px;
    }
    .dx-icon-trash,.dx-icon-revert {
        color:red !important;
    }
    .btnAddNew.dx-button-mode-contained:not(.dx-state-hover):hover {
    background-color: #4FA1D6 !important;
    }
    .btnAddNew.dx-button.dx-button-mode-contained.imgBox.find-similar-projects .dxb::after {
    background-image: url(/content/Images/searchNew.png);
    filter: brightness(0) invert(1);
    }
    .btnImport.dx-button.dx-button-mode-contained.imgBox .dxb::after {
    content: '';
    display: inline-block;
    width: 17px;
    height: 17px;
    vertical-align: middle;
    background-repeat: no-repeat;
    background-size: 17px;
    margin-left: 4px;
}
    #divAllocationGrid .dx-icon-trash {
        background-image: url(/content/Images/deleteIcon-new.png);
        color:transparent !important;
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
    var selectedTemplate;
    var grdImportTemplates;
    var templateData = [];
    var IsResourceAdmin = "<%=IsResourceAdmin%>" == "True" ? true : false;
    function importTemplate(s, e) {
        if (ASPxClientEdit.ValidateGroup('template')) {
            var grid = $("#divAllocationGrid").dxDataGrid("instance");
            selectedTemplate = grid.getSelectedRowKeys();
            var projectID = '<%=ProjectID%>';
            if (typeof selectedTemplate[0] !== "undefined") {
                //window.parent.CloseWindowCallback(0, "");
                var gridurl = '';
                if (s.globalName == "btnAutofillAllocations") {
                    var gridurl = "/Layouts/uGovernIT/DelegateControl.aspx?control=allocationtemplategrid&templateID=" + selectedTemplate[0].ID + "&projectID=" + projectID + "&StartDate=" + dteStartDate.GetDate().toISOString() + "&EndDate=" + dteEndDate.GetDate().toISOString() + "&Option=AutofillAllocations";
                }
                else {
                    var gridurl = "/Layouts/uGovernIT/DelegateControl.aspx?control=allocationtemplategrid&templateID=" + selectedTemplate[0].ID + "&projectID=" + projectID + "&StartDate=" + dteStartDate.GetDate().toISOString() + "&EndDate=" + dteEndDate.GetDate().toISOString();
                }

                var ctrl = getUrlParam("ctrl", '');
                if (ctrl == "PMM.ProjectCompactView")
                    UgitOpenPopupDialog(gridurl, "", 'Preview Template: ' + selectedTemplate[0].Name + ' for ' + projectID, '90', '90', 0, escape("<%= Request.Url.AbsolutePath %>"));
                else
                    window.parent.UgitOpenPopupDialog(gridurl, "", 'Preview Template: ' + selectedTemplate[0].Name, '90', '90', 0, escape("<%= Request.Url.AbsolutePath %>"));
            } else {
                DevExpress.ui.dialog.alert('Please Select a Template.', 'Alert', true);
            }
        }
    }

    function SimilarProjects(s, e) {
        var projectID = '<%=ProjectID%>';
        var SearchData = '<%=searchData%>';
        var gridurl = "/Layouts/uGovernIT/DelegateControl.aspx?control=SimilarProjects&projectID=" + projectID + "&SearchData=" + SearchData;
        window.parent.UgitOpenPopupDialog(gridurl, "", 'Similar Projects for : ' + projectID, '80', '50', 0, escape("<%= Request.Url.AbsolutePath %>"));
    }
    function alertMessage() {
        alert('Start Date  for Project  is not specified');
        window.parent.CloseWindowCallback(1);
    }
    $(function () {
        SetDuration();
        grdImportTemplates = $("#divAllocationGrid").dxDataGrid({
            dataSource: templateData,
            height: window.innerHeight - 80,
            ID: "grdAllocationGrid",
            paging: { enabled: false },
            filterRow: { visible: true },
            scrolling: {
                mode: "standard" // or "virtual" | "infinite"
            },
            editing: {
                mode: "row",
                allowEditing: IsResourceAdmin,
                allowUpdating: IsResourceAdmin,
                allowDeleting: IsResourceAdmin,
                useIcons: true
            },
            columns: [
                {
                    dataField: "Name",
                    dataType: "string",
                    sortIndex: "1",
                    sortOrder: "asc",
                    caption: "Template Name",
                    cellTemplate: function (container, options) {
                        $("<div id='dataId'>")
                            .append('<span style:float: left;overflow: auto;>' + options.value + '</span>')
                            .appendTo(container);
                    }
                }
            ],
            selection: {
                mode: "single"
            },
            showRowLines: true,
            rowAlternationEnabled: true,
            showBorders: true,
            onInitialized: function (e) {
                e.component.columnOption("command:edit", "width", 80);
            },
            onRowUpdating: function (e) {
                if (typeof e.newData.Name !== "undefined" && e.newData.Name !== e.oldData.Name) {
                    $.post("/api/rmmapi/UpdateTemplateName", { ID: e.oldData.ID, Name: e.newData.Name }).then(function (response) {
                        templateData = response;
                    }, function (error) { });
                }
            },
            onRowRemoving: function (e) {
                if (!e.cancel) {
                    $.post("/api/rmmapi/DeleteTemplateAllocation", { ID: e.key.ID, Name: e.key.Name, DeleteTemplate: true }).then(function (response) {
                        var dataGrid = $("#divAllocationGrid").dxDataGrid("instance");
                        dataGrid.option("dataSource", response);
                    }, function (error) { });
                }
            },
            onRowValidating: function (e) {
                if (typeof e.newData.Name !== "undefined") {
                    if (templateData.filter(x => x.Name.trim().toLowerCase() == e.newData.Name.trim().toLowerCase()).length > 0) {
                        e.isValid = false;
                        e.errorText = "Template name already exists."
                    }
                }
            }
        });
    });
    
    $(document).on("click", "img.imgDelete", function (e) {
        var tempID = $(this).attr("ID");
        var result = DevExpress.ui.dialog.confirm('Are you sure you want to delete template?', 'Confirm');
        result.done(function (confirmation) {

            if (confirmation) {
                $.post("/api/rmmapi/DeleteTemplateAllocation", { ID: tempID, Name: templateData.Name, DeleteTemplate: true }).then(function (response) {
                     var dataGrid = $("#divAllocationGrid").dxDataGrid("instance");
                                    
                    //var ds = $("#divAllocationGrid").dxDataGrid("getDataSource");
                    dataGrid.option("dataSource", response);
                    //ds.reload();
                }, function (error) { });
            }
        });

    })


    function endDateChange(s, e) {
        SetDuration();
    }

    function SetDuration() {
        var constStartDate = dteStartDate.GetDate();
        if (constStartDate != null && constStartDate != "" && constStartDate != undefined) {
            constStartDate = constStartDate.format('MM/dd/yyyy');
        }

        var constEndDate = dteEndDate.GetDate();
        if (constEndDate != null && constEndDate != "" && constEndDate != undefined) {
            constEndDate = constEndDate.format('MM/dd/yyyy');
        }

        if (constStartDate != null && constStartDate != "" && constEndDate != null && constEndDate != "") {
            var paramsInJson = '{' + '"startDate":"' + constStartDate + '","endDate":"' + constEndDate + '"}';
            $.ajax({
                type: "POST",
                url: "/Layouts/uGovernIT/ajaxhelper.aspx/GetDurationInWeeks",
                data: paramsInJson,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (message) {
                    var resultJson = $.parseJSON(message.d);
                    if (resultJson.messagecode == 2) {
                        txtDuration.SetText(resultJson.duration);
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    //alert(thrownError);
                }
            });

        }
    }
    function startDateChange(s, e) {
        var constStartDate = dteStartDate.GetDate();
        if (constStartDate != null && constStartDate != "" && constStartDate != undefined) {
            constStartDate = constStartDate.format('MM/dd/yyyy');
        }
        var noOfWeeks = txtDuration.GetText();   

        if (constStartDate != null && constStartDate != "" && noOfWeeks != null && noOfWeeks != "" && noOfWeeks > 0) {

            var paramsInJson = '{' + '"startDate":"' + constStartDate + '","noOfWeeks":"' + parseInt(noOfWeeks) + '"}';
            $.ajax({
                type: "POST",
                url: "/Layouts/uGovernIT/ajaxhelper.aspx/GetEndDateByWeeks",
                data: paramsInJson,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (message) {
                    var resultJson = $.parseJSON(message.d);
                    if (resultJson.messagecode == 2) {
                      
                        dteEndDate.SetDate(new Date(resultJson.enddate));
                    }
                    else {

                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    //alert(thrownError);

                }
            });
        }

        //// e.processOnServer = false;

    }
    
    function changeEndateOnDuration(s, e) {

        var constStartDate = dteStartDate.GetDate();
        constStartDate = constStartDate.format('MM/dd/yyyy');
        var noOfWeeks = txtDuration.GetText();
        noOfWeeks = Math.ceil(noOfWeeks);

        if (constStartDate != null && constStartDate != "" && noOfWeeks != null && noOfWeeks != "" && noOfWeeks > 0) {
            var paramsInJson = '{' + '"startDate":"' + constStartDate + '","noOfWeeks":"' + parseInt(noOfWeeks) + '"}';
           
            $.ajax({
                type: "POST",
                url: "/Layouts/uGovernIT/ajaxhelper.aspx/GetEndDateByWeeksProject",
                data: paramsInJson,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (message) {
                    var resultJson = $.parseJSON(message.d);
                    if (resultJson.messagecode == 2) {
                        //console.log(message);
                      
                        dteEndDate.SetDate(new Date(resultJson.enddate));
                    }
                    else {

                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    //alert(thrownError);

                }
            });
        }
    }

    $.get("/api/rmmapi/GetProjectTemplates", function (data, status) {
        templateData = data;
        try {
            var dataGrid = $("#divAllocationGrid").dxDataGrid("instance");
            dataGrid.option("dataSource", templateData);
        } catch (e) { }
    });
</script>
<dx:ASPxPanel ID="pnlImportAllocation" runat="server">
    <PanelCollection>
        <dx:PanelContent>
            <div class="row" style="padding-top: 10px;display:none;">
                <div class="col-md-4 col-sm-6 col-xs-4 noPaddingLeft">
                    <div class="ms-formlabel">
                        <h4 class="ms-standardheader">Project Start Date</h4>
                    </div>
                    <div class="py-2">
                        <%--  <dx:ASPxDateEdit ID="dteStartDate" ClientInstanceName="dteStartDate" runat="server" Width="100px"></dx:ASPxDateEdit>--%>

                        <dx:ASPxDateEdit OnValueChangeClientScript="dateChanged()" ID="dteStartDate" ClientInstanceName="dteStartDate" Width="100%" runat="server" 
                            CssClass="aspTextbox-wrap" Height="28px" DropDownButton-Image-Url="~/Content/Images/calendarNew.png" DropDownButton-Image-Width="16" 
                            DisplayFormatString="MMM d, yyyy">
                            <ValidationSettings ValidationGroup="template" ErrorDisplayMode="ImageWithTooltip">
                                <RequiredField IsRequired="true" ErrorText="Please enter start date." />
                            </ValidationSettings>
                            <ClientSideEvents DateChanged="startDateChange" />
                        </dx:ASPxDateEdit>
                    </div>
                </div>
                <div class="col-md-4 col-sm-6 col-xs-4">
                    <div class="ms-formlabel">
                        <h4 class="ms-standardheader">Duration(Weeks)</h4>
                    </div>
                    <div class="py-2">
                        <dx:ASPxTextBox ID="txtDuration" ClientInstanceName="txtDuration" runat="server" CssClass="aspTextbox-wrap" Width="100%" Height="39px">
                            <ClientSideEvents TextChanged="changeEndateOnDuration" />
                        </dx:ASPxTextBox>
                    </div>
                </div>
                <div class="col-md-4 col-sm-6 col-xs-4 noPaddingLeft">
                    <div class="ms-formlabel">
                        <h4 class="ms-standardheader">Project End Date</h4>
                    </div>
                    <div class="py-2">
                        <dx:ASPxDateEdit OnValueChangeClientScript="dateChanged()" Height="28px" ID="dteEndDate" ClientInstanceName="dteEndDate" runat="server" CssClass="aspTextbox-wrap" 
                            Width="100%" DropDownButton-Image-Url="~/Content/Images/calendarNew.png" DropDownButton-Image-Width="16" DisplayFormatString="MMM d, yyyy">
                            <ValidationSettings ValidationGroup="template" ErrorDisplayMode="ImageWithTooltip">
                                <RequiredField IsRequired="true" ErrorText="Please enter end date." />
                            </ValidationSettings>
                            <ValidationSettings>
                                <RegularExpression ValidationExpression="" />
                            </ValidationSettings>
                            <DateRangeSettings CalendarColumnCount="1" StartDateEditID="dteStartDate" />
                            <ClientSideEvents DateChanged="endDateChange" />
                        </dx:ASPxDateEdit>
                    </div>
                </div>
            </div>

            <div class="row" style="padding-top: 10px;">
                    <div id="divAllocationGrid">
                    </div>
            </div>
            
            <div class="d-flex justify-content-between flex-wrap pt-3" style="float:right;">
                    <dx:ASPxButton ID="btnFindSimilarProjects" ClientInstanceName="btnImport" runat="server" BackColor="Transparent" CssClass="btnAddNew btnImport dx-button dx-button-mode-contained imgBox find-similar-projects" Text="View Similar Projects" ImagePosition="Right" ValidationGroup="template" AutoPostBack="false">
                        <ClientSideEvents Click="function(s,e){ SimilarProjects(s,e);}" />
                    </dx:ASPxButton>

                    <dx:ASPxButton ID="btnImport" ClientInstanceName="btnImport"  runat="server"  CssClass="btnAddNew dx-button dx-button-mode-contained imgBox preview-allocations" Text="View Template" ValidationGroup="template" AutoPostBack="false">
                        <ClientSideEvents Click="function(s,e){importTemplate(s,e);}" />
                    </dx:ASPxButton>

                    <dx:ASPxButton ID="btnAutofillAllocations" ClientInstanceName="btnAutofillAllocations"  runat="server" CssClass="btnAddNew dx-button dx-button-mode-contained imgBox autofill-allocations" Text="Autofill Allocations" ValidationGroup="template" AutoPostBack="false" Visible="false">
                        <ClientSideEvents Click="function(s,e){importTemplate(s,e);}" />
                    </dx:ASPxButton>
            </div>
        </dx:PanelContent>
    </PanelCollection>
</dx:ASPxPanel>



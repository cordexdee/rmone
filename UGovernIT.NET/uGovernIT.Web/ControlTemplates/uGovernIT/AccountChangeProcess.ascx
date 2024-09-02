<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AccountChangeProcess.ascx.cs" Inherits="uGovernIT.Web.AccountChangeProcess" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .labelalignment {
        text-align: right;
    }

    .alltdspace {
        border: 4px solid white;
    }

    .ms-formbody {
        background: none repeat scroll 0 0 #E8EDED;
        border-top: 1px solid #A5A5A5;
        padding: 3px 6px 4px;
        vertical-align: top;
    }

    /*.ms-formlabel {
        text-align: right;
        width: 190px;
        vertical-align: top;
    }*/
</style>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    var isMultiSelect = '<%=IsMultiSelect%>';
    function OnModuleChange() {
        grid.SetVisible(false);
        LoadingPanel.Show();
    }

    function OnRowClickEvent(s, e) {          
        if (s.IsRowSelectedOnPage(e.visibleIndex)) {
            s.UnselectRows(e.visibleIndex);
        }
    }

    function SelectAll() {
        if (ticketGrid != null) {
            ticketGrid.SelectRows()
        }

        if (taskGrid != null) {
            taskGrid.SelectRows()
        }
    }
    function UnselectAll(s, e) {
        if (e.visibleIndex < 0 && !e.isSelected && e.isAllRecordsOnPage && ticketGrid != null) {
            ticketGrid.UnselectRows()
        }
       
        var ticketspageinfo = $(".ticketspageinfo");
        ticketspageinfo.html("Selected: " + s.GetSelectedRowCount() + " of " + s.pageRowCount + "");

        //taskpageinfo
        if (e.visibleIndex < 0 && !e.isSelected && e.isAllRecordsOnPage && taskGrid != null) {
            taskGrid.UnselectRows()
        }

        var taskpageinfo = $(".taskpageinfo");
        taskpageinfo.html("Selected: " + s.GetSelectedRowCount() + " of " + s.pageRowCount + "");
    }

    function UnselectAllTask(s, e) {
        if (e.visibleIndex < 0 && !e.isSelected && e.isAllRecordsOnPage && taskGrid != null) {
            taskGrid.UnselectRows()
        }
        var taskpageinfo = $(".taskpageinfo");
        taskpageinfo.html("Selected: " + s.GetSelectedRowCount() + " of " + s.pageRowCount + "");
    }

     $(document).ready(function () {
         $('.fetch-parent').parent().addClass("popupUp-mainContainer");
         $('.userValueBox-Table').parent().addClass("userValueBox-searchFilterWrap");
         $('.userValueBox-searchFilterWrap').parent().addClass("userValueBox-searchFilterContainer");
    });
</script>

<style type="text/css">
    /*.dxgvCSD
    {
        width: 270px;
    }*/
</style>
<asp:HiddenField ID="hdOldUserId" runat="server" />
<asp:HiddenField ID="hdNewUserId" runat="server" />
<asp:HiddenField ID="hdOldUserName" runat="server" />
<asp:HiddenField ID="hdNewUserName" runat="server" />

<div class="fetch-parent" id="dvPanel1" runat="server">
    <div class="ms-formtable accomp-popup">
        <div class="col-md-6 col-sm-6 colForXS">
            <div class="ms-formlabel" style="word-wrap: break-word;">
                <label class="ms-standardheader budget_fieldLabel">Old User:</label>
            </div>
            <div class="ms-formbody accomp_inputField">
                <ugit:UserValueBox ID="OldUser" runat="server" ValidationGroup="RequestTypeGroup" IsMandatory="true" SelectionSet="User" CssClass="userValueBox-dropDown"></ugit:UserValueBox>
            </div>
        </div>
        <div class="col-md-6 col-sm-6 colForXS">
            <div class="ms-formlabel" >
                <label class="ms-standardheader budget_fieldLabel">New User:</label>
            </div>
            <div class="ms-formbody accomp_inputField">
                <ugit:UserValueBox ID="NewUser" runat="server" ValidationGroup="RequestTypeGroup" IsMandatory="true" SelectionSet="User" CssClass="userValueBox-dropDown"></ugit:UserValueBox>                
                <asp:Label runat="server" ID="lblUserError" Visible="false" ForeColor="Red"></asp:Label>
            </div>
        </div>
        <div class="col-md-6 col-sm-6 colForXS">
            <div class="ms-formlabel">
                <label class="ms-standardheader budget_fieldLabel">Module:</label>
            </div>
            <div class="ms-formbody accomp_inputField">
                <ugit:LookUpValueBox ID="ddlModule" runat="server" FieldName="MultiModules" ClientInstanceName="ddlModule" CssClass="lookUpValueBox-dropDown" 
                    FilterExpression="EnableModule=1" />                
            </div>
        </div>
        <div class="col-md-12 col-sm-12 col-xs-12 noPadding">
            <div class="ms-formlabel">
                <label class="ms-standardheader budget_fieldLabel">Filter:</label>
            </div>
            <div class="ms-formbody accomp_inputField bC-radioBtnWrap">
                <asp:RadioButton ID="rbOpenTicket" runat="server" GroupName="TicketFilter" Text="Open Ticket(s)" Checked="true" CssClass="importChk-radioBtn" /> 	
                <asp:RadioButton ID="rbOpenAndclosedTicket" runat="server" GroupName="TicketFilter" Text="Open & Closed Ticket(s)" CssClass="importChk-radioBtn"/> 
            </div>
        </div>
        <div class="col-md-12 col-sm-12 col-xs-12 noPadding">
            <div class="ms-formlabel">
                <label class="ms-standardheader budget_fieldLabel">Task Filter:</label>
            </div>
            <div class="ms-formbody accomp_inputField bC-radioBtnWrap">
                <asp:RadioButton ID="rbOpenTask" runat="server" GroupName="TaskFilter" Checked="true" Text="Open Task(s)" CssClass="importChk-radioBtn" /> 
                <asp:RadioButton ID="rbOpenAndcompletedTask" runat="server" GroupName="TaskFilter" Text="Open & Complete Task(s)" CssClass="importChk-radioBtn"  /> 
            </div>
        </div>
        <div class="col-md-12 col-sm-12 col-xs-12 noPadding">   
            <%--<div class="ms-formlabel">
                <%--<label>Replace Application Access</label>
            </div>--%>
            <div class="ms-formbody accomp_inputField rmmChkBox-container">
                <asp:CheckBox ID="chkReplaceApplicationAccess" runat="server" Enabled="false" Text="Replace Application Access" CssClass="replaceUser-popupchkbox RMM-checkWrap" />
            </div>
        </div>
        <div class="col-md-12 col-sm-12 col-xs-12 noPadding">
            <%--<td class="ms-formlabel">
                <label>Run In Background</label>
            </td>--%>
            <div class="ms-formbody accomp_inputField rmmChkBox-container">
                <asp:CheckBox ID="chkRunInBackground" runat="server" Text="Run In Background" CssClass="replaceUser-popupchkbox RMM-checkWrap" />
            </div>
        </div>
    </div>
    <div style="bottom: 15px;" class="col-md-12 col-sm-12 col-xs-12 RRMactiobBtn-wrap">
        <div style="display: inline;">
            <dx:ASPxButton ID="btnNext" runat="server" Text="Next >>" OnClick="btnNext_Click" ValidationGroup="ValidateNext" CssClass="RRMsaveBtn primary-blueBtn" ></dx:ASPxButton>
        </div>
    </div>
</div>

<div id="dvPanel2" runat="server" visible="false" class="col-md-12 col-sm-12 col-xs-12 noSidePadding">
    <div class="replaceUser-title row">
        <b>Replace [<asp:Label ID="lbOldUser1" runat="server" Text="Label"></asp:Label>] with [<asp:Label ID="lbNewUser1" runat="server" Text="Label"></asp:Label>]: Select ticket(s) below</b>
    </div>
    <div class="row">
       <span  style="padding: 0px 0px 0px 5px;">
           <b class="ticketspageinfo">Selected: <span id="spSelectedTickets">0</span> of <asp:Label ID="spTotalTickets" runat="server" Text="0"></asp:Label></b>
       </span>
    </div>
    <div class="row fieldWrap">
        <ugit:ASPxGridView ID="ticketGrid" runat="server" AutoGenerateColumns="False" SettingsText-CommandClearFilter="" SettingsBehavior-AllowSort="false"
            OnDataBinding="ticketGrid_DataBinding" OnCustomColumnDisplayText="ticketGrid_CustomColumnDisplayText" 
            OnHtmlRowPrepared="ticketGrid_HtmlRowPrepared" OnHeaderFilterFillItems="ticketGrid_HeaderFilterFillItems"
            OnHtmlDataCellPrepared="ticketGrid_HtmlDataCellPrepared" CssClass="customgridview homeGrid"
            ClientInstanceName="ticketGrid" Width="100%"  KeyFieldName="ID">
            <ClientSideEvents SelectionChanged="function(s,e){UnselectAll(s,e);}" RowClick=" function(s,e){ UnselectAll(s,e); }" />
            <Columns>
            <dx:GridViewCommandColumn Caption=" " ShowSelectCheckbox="true" SelectAllCheckboxMode="Page" VisibleIndex="0" Width="30px" />                    
            <dx:GridViewDataColumn FieldName="ID" Caption="ID" VisibleIndex="5" Visible="false"></dx:GridViewDataColumn>
                <dx:GridViewDataTextColumn FieldName="TicketId" Caption="Ticket Id" VisibleIndex="1" />
            <dx:GridViewDataTextColumn FieldName="Title" Caption="Title" VisibleIndex="2" />
            <dx:GridViewDataDateColumn FieldName="Status" Caption="Status" VisibleIndex="3" />            
            <dx:GridViewDataDateColumn FieldName="StageActionUserTypes" Caption="Role" VisibleIndex="4" />
            </Columns>                       
            <SettingsBehavior EnableRowHotTrack="false"  AllowSelectByRowClick="false" AllowSelectSingleRowOnly="true" />                        
            <SettingsPopup>
                <HeaderFilter Height="200" />
            </SettingsPopup>
            <Styles AlternatingRow-CssClass="ms-alternatingstrong">
                <Row HorizontalAlign="Center" CssClass="CRMstatusGrid_row"></Row>
                <Header Font-Bold="true" HorizontalAlign="Center" CssClass="CRMstatusGrid_headerRow"></Header>
            </Styles>
            <ClientSideEvents RowClick="OnRowClickEvent" SelectionChanged="function(s, e) {
                $('#spSelectedTickets').html(ticketGrid.GetSelectedRowCount())
                //$('#spTotalTickets').html(ticketGrid.VisibleRowCount)                
                if (e.isSelected) {                    
                    var key = s.GetRowKey(e.visibleIndex);
                    if($('.BtnSaveLink').length > 0){  
                            if(isMultiSelect == 'False')
                        {
                            LoadingPanel.SetText('Saving ...');
                            LoadingPanel.Show();      
                        var val = $('.BtnSaveLink').attr('id').replace(/_/g,'$');
                        javascript:__doPostBack(val,'');  
                        }
                    }
                }                             
            }" />
        </ugit:ASPxGridView>
    </div>

    <div class="replaceUser-title row" style="padding-top:10px;">
        <b>Replace [<asp:Label ID="lbOldUser2" runat="server" Text="Label"></asp:Label>] with [<asp:Label ID="lbNewUser2" runat="server" Text="Label"></asp:Label>]: Select task(s) below</b>
    </div>
    <div class="row">
       <span  style="padding: 0px 0px 0px 5px;">
           <b class="taskpageinfo">Selected: <span id="spSelectedTasks">0</span> of <asp:Label id="spTotalTasks" runat="server">0</asp:Label></b>
       </span>
    </div>
    <div class="row fieldWrap">
        <ugit:ASPxGridView ID="taskGrid" runat="server" AutoGenerateColumns="False" SettingsText-CommandClearFilter="" SettingsBehavior-AllowSort="false"
            OnDataBinding="taskGrid_DataBinding" OnCustomColumnDisplayText="taskGrid_CustomColumnDisplayText" 
            OnHtmlRowPrepared="taskGrid_HtmlRowPrepared" OnHeaderFilterFillItems="taskGrid_HeaderFilterFillItems"
            OnHtmlDataCellPrepared="taskGrid_HtmlDataCellPrepared" CssClass="customgridview homeGrid"
            ClientInstanceName="taskGrid"  Width="100%" KeyFieldName="ID">
            <ClientSideEvents SelectionChanged="function(s,e){UnselectAll(s,e);}" RowClick=" function(s,e){ UnselectAll(s,e); }" />
            <Columns>
                <dx:GridViewDataTextColumn FieldName="ParentInstance" Caption=" " GroupIndex="1"></dx:GridViewDataTextColumn>
                <dx:GridViewCommandColumn Caption=" " ShowSelectCheckbox="true" SelectAllCheckboxMode="Page" VisibleIndex="0" Width="30px" />                    
                <dx:GridViewDataColumn FieldName="ID" Caption="ID" VisibleIndex="5" Visible="false"></dx:GridViewDataColumn>
                <dx:GridViewDataTextColumn FieldName="Title" Caption="Title" VisibleIndex="1" />                
                <dx:GridViewDataDateColumn FieldName="Status" Caption="Status" VisibleIndex="2" />            
                <dx:GridViewDataDateColumn FieldName="AssignedTo" Caption="Assigned To" VisibleIndex="3" />
                <dx:GridViewDataDateColumn FieldName="Approver" Caption="Approver" VisibleIndex="4" />
            </Columns>                        
            <SettingsBehavior EnableRowHotTrack="false"  AllowSelectByRowClick="false" AllowSelectSingleRowOnly="true" />                        
            <SettingsPopup>
                <HeaderFilter Height="200" />
            </SettingsPopup>
<%--            <SettingsPager Position="TopAndBottom">
                <PageSizeItemSettings Items="10, 15, 20, 25, 50, 75, 100" />
            </SettingsPager>--%>
            <Styles AlternatingRow-CssClass="ms-alternatingstrong">
                <Row HorizontalAlign="Center" CssClass="CRMstatusGrid_row"></Row>
                <Header Font-Bold="true" HorizontalAlign="Center" CssClass="CRMstatusGrid_headerRow"></Header>
            </Styles>
            <ClientSideEvents RowClick="OnRowClickEvent" SelectionChanged="function(s, e) {
                $('#spSelectedTasks').html(taskGrid.GetSelectedRowCount())
                if (e.isSelected) {
                    var key = s.GetRowKey(e.visibleIndex);
                    if($('.BtnSaveLink').length > 0){  
                            if(isMultiSelect == 'False')
                        {
                            LoadingPanel.SetText('Saving ...');
                            LoadingPanel.Show();      
                        var val = $('.BtnSaveLink').attr('id').replace(/_/g,'$');
                        javascript:__doPostBack(val,'');  
                        }
                    }
                }                             
            }" />
        </ugit:ASPxGridView>
    </div>
    <div class="row replaceUser-btnContainer">
        <div class="replaceUser-BtnWrap">
            <dx:ASPxButton ID="btnPrevious" runat="server" Text="<< Previous" OnClick="btnPrevious_Click" CssClass="primary-blueBtn"></dx:ASPxButton>
            <dx:ASPxButton ID="btnReplaceUser" runat="server" Text="Replace User" OnClick="btnReplaceUser_Click" CssClass="primary-blueBtn"></dx:ASPxButton>
        </div>
     </div>
</div>

<div id="dvPanel3" class="col-md-12 col-sm-12 col-xs-12" runat="server" visible="false">
    <div class="row">
        <div >
            <asp:Label ID="lblStatusMesg" runat="server" Text="Label" CssClass="messageformat replaceUser-msg">Your request is in process, we will send you notification once it is completed.</asp:Label>
        </div>
    </div>
    <div class="row addEditPopup-btnWrap">
        <dx:ASPxButton ID="btnClose" runat="server" Text="Close" OnClick="btnClose_Click" CssClass="secondary-cancelBtn"></dx:ASPxButton>
    </div>
</div>

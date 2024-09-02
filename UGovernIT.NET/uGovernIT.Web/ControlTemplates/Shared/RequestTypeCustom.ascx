<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RequestTypeCustom.ascx.cs" Inherits="uGovernIT.Web.RequestTypeCustom" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style data-v="<%=UGITUtility.AssemblyVersion %>">
    
    .homeGrid_headerColumn table td {
        color: #4A6EE2;
        font-size: 12px;
        font-family: 'Poppins', sans-serif !important;
    }
    .dropDown-wrap{
        padding-top:10px;
    }
</style>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">

    if (typeof (requesttypeboxEdit) == "undefined")
        var requesttypeboxEdit = [];
    requesttypeboxEdit.push({ clienid: "<%=this.ClientID%>", dropdownid:<%= this.DropDownEdit.ClientID%>})
   

     function getRequestTypeEditBox_Ctrl(globalName, dxCtrlID) {
         var item = _.find(requesttypeboxEdit, function (s) {
             return globalName.indexOf(s.clienid) != -1;
         });

         if (!item)
             return null;

         return window[item.clienid + "_" + dxCtrlID];
    }

     function getRequestTypeEditBox_dropdown(globalName) {
         var cliendID = _.find(requesttypeboxEdit, function (s) {
             return globalName.indexOf(s.clienid) != -1;
         });

         if (!cliendID)
             return null;

         var item = _.findWhere(requesttypeboxEdit, { clienid: cliendID.clienid });
         return window[item.dropdownid.id];
     }



    function OnCategoryChanged(categoryDropDown) {
        var subcategoryDropDown = getRequestTypeEditBox_Ctrl(categoryDropDown.globalName, "subcategoryDropDown");

        if (subcategoryDropDown.InCallback())
            categoryDropDown.GetValue();
        else
            subcategoryDropDown.PerformCallback('CATEGORY|' + categoryDropDown.GetValue());

    }
    function OnSubCategoryChanged(subcategoryDropDown) {
        var requestTypeGrid = getRequestTypeEditBox_Ctrl(subcategoryDropDown.globalName, "requestTypeGrid"); 
        if (requestTypeGrid.InCallback())
            subcategoryDropDown.GetValue();
        else
            requestTypeGrid.PerformCallback('SUBCATEGORY|' + subcategoryDropDown.GetValue());

    }
    function OnEndCallback(s, e) {
        var categoryDropDown = getRequestTypeEditBox_Ctrl(s.globalName, "categoryDropDown"); 
        var requestTypeGrid = getRequestTypeEditBox_Ctrl(s.globalName, "requestTypeGrid"); 
        s.SetVisible(true);
        if (!s.InCallback() && s.GetItemCount() == 0) {
            s.SetVisible(false);
            requestTypeGrid.PerformCallback(categoryDropDown.GetValue());
        }
        else
            requestTypeGrid.PerformCallback(categoryDropDown.GetValue());
        requestTypeGrid.Refresh();
    }

   
   
    function onGridSelectionChanged_requesttype(s, e) {
        var requestTypeGrid = getRequestTypeEditBox_Ctrl(s.globalName, "requestTypeGrid"); 
        cbIssueType = getRequestTypeEditBox_Ctrl(s.globalName, "cbIssueType"); 
       
        visibleIndex = e.visibleIndex + 1;
        var key = s.GetRowKey(e.visibleIndex);
        if (key > 0)
            //loadingPane3.ShowInElementByID(s.GetMainElement().id);
            requestTypeGrid.GetSelectedFieldValues("ID;RequestType;Title", function (data) {
            onGridSelectionChanged_requesttype_Callback(s, false ,data);
        });

    }
    function onGridSelectionChanged_requesttype_Callback(s, isIssueType, h) {
        if (h.length == 0) {
            return false;
        }
             
        reqTypeID = h[0][0];
        reqTypeText = h[0][2];
        var ddEditControl = getRequestTypeEditBox_dropdown(s.globalName);
        var cbIssueType = getRequestTypeEditBox_Ctrl(s.globalName, "cbIssueType");
        if (reqTypeID > 0) {
            ddEditControl.HideDropDown();
            ddEditControl.SetKeyValue(reqTypeID);
            if (isIssueType) {
                reqTypeText = h[0][1];
                ddEditControl.SetText(reqTypeText + ' > ' + cbIssueType.GetText());
                ddEditControl.SetKeyValue(reqTypeID + ';#' + cbIssueType.GetText());
            }
            else {
                ddEditControl.SetText(reqTypeText);
            }
        }
        ddEditControl.RaiseValueChanged();

    }

    function onGridSelectionChanged_IssueType(s, e) {
        var cbIssueType = getRequestTypeEditBox_Ctrl(s.globalName, "cbIssueType");
        if (e.visibleIndex != -1) {
            var key = s.GetRowKey(e.visibleIndex);
            cbIssueType.PerformCallback(key)
        }
    }
    function HideLoadingPane3(s, e) {
        var selCategory = getRequestTypeEditBox_Ctrl(s.globalName, "categoryDropDown");
        var subcategoryDropDown = getRequestTypeEditBox_Ctrl(s.globalName, "subcategoryDropDown");
        if (selCategory.GetValue() == null || subcategoryDropDown.GetItemCount() <= 1)
            subcategoryDropDown.SetVisible(false);
    }

    function btnOk_Click(s, e) {
        var cbIssueType = getRequestTypeEditBox_Ctrl(s.name, "cbIssueType");
        var requestTypeGrid = getRequestTypeEditBox_Ctrl(s.name, "requestTypeGrid"); 
        requestTypeGrid.GetSelectedFieldValues("ID;RequestType", function (data) {
            onGridSelectionChanged_requesttype_Callback(s, true ,data);
        });
    }

    function pnlRequestTypeCustom_EndCallback(s, e) {

    }
</script>

<%--<dx:ASPxLoadingPanel ID="loadingPane3" ClientInstanceName="loadingPane3" runat="server" Modal="true" Text="Please Wait..."></dx:ASPxLoadingPanel>--%>
<dx:ASPxHiddenField ID="hdnenableissuetypedropdown" runat="server"></dx:ASPxHiddenField>
<div style="width:650px;">
<div class="col-md-12 col-sm-12 col-xs-12">
    <div class="row">
        <div class="col-md-6 col-sm-6 col-xs-6 noPadding">
            <%--<div class="dropDown-label">
                <span>Category:</span>
            </div>--%>
            <div class="dropDown-wrap categorydroDown-wrap">
                <dx:ASPxComboBox ID="categoryDropDown" runat="server" DropDownStyle="DropDownList" AutoPostBack="false" EnableClientSideAPI="true"
                        IncrementalFilteringMode="StartsWith" TextField="Category" ValueField="Category" CssClass="aspxComBox-dropDown"
                        ListBoxStyle-CssClass="aspxComboBox-listBox">
                        <ClientSideEvents SelectedIndexChanged="function(s, e) { OnCategoryChanged(s); }" />
                </dx:ASPxComboBox>
            </div>
           
        </div>
        <div id="divSubCategoryLabel" class="col-md-6 col-sm-6 col-xs-6 noPadding" runat="server">
            <%--<div class="dropDown-label">
                <span>Sub Category:</span>
            </div>--%>
            <div class="dropDown-wrap">
                <dx:ASPxComboBox ID="subcategoryDropDown" runat="server" DropDownStyle="DropDownList"  TextField="SubCategory" ValueField="SubCategory" EnableClientSideAPI="true"
                     IncrementalFilteringMode="StartsWith"  OnCallback="subcategoryDropDown_Callback"  AutoPostBack="false" CssClass="aspxComBox-dropDown"
                     ListBoxStyle-CssClass="aspxComboBox-listBox" >
                    <ClientSideEvents EndCallback=" OnEndCallback" SelectedIndexChanged="function(s,e){ OnSubCategoryChanged(s);}" />
                </dx:ASPxComboBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12 col-sm-12 col-xs-12 noPadding requestTypeGrid-wrap">
            <dx:ASPxGridView ID="requestTypeGrid" Width="100%"  KeyFieldName="ID" runat="server" 
                OnDataBinding="requestTypeGrid_DataBinding" OnCustomCallback="requestTypeGrid_CustomCallback" 
                EnableCallBacks="true" CssClass="customgridview homeGrid" Styles-SelectedRow-BackColor="#ff6600" >
                <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="true" AllowSelectSingleRowOnly="true" />
                <Styles>
                    <%--<Row CssClass="homeGrid_dataRow"></Row>--%>
                    <Header CssClass="homeGrid_headerColumn"></Header>
                </Styles>
                <Columns>
                    <dx:GridViewDataTextColumn SortOrder="Ascending" SortIndex="0" VisibleIndex="0" Visible="false" FieldName="Category" Caption="Category"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn SortOrder="Ascending" SortIndex="1" VisibleIndex="1" Visible="false" FieldName="SubCategory" Caption="Sub-Category"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn SortOrder="Ascending" SortIndex="2" VisibleIndex="2" FieldName="RequestType"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn SortOrder="Ascending" SortIndex="2" VisibleIndex="2" FieldName="Title" Visible="false"></dx:GridViewDataTextColumn>

                </Columns>
                <Settings ShowFilterRow="true" />
               <SettingsPager Mode="ShowAllRecords" PageSize="10000" />
           <ClientSideEvents EndCallback="HideLoadingPane3" />
                <%--<SettingsLoadingPanel Mode="Default" />--%>
            </dx:ASPxGridView>
        </div>
    </div>
    <div id="trIssueType" runat="server" style="margin-top: 5px; width: 100%">
        <div  style="margin-top: 5px; width: 100%">
            <div style="margin:10px; width: 100%">
            <div style="float:left;"><div style="float:left;">Issue Type:</div>
                <div style="float:left">
            <dx:ASPxComboBox ID="cbIssueType" EnableClientSideAPI="true" runat="server" DropDownStyle="DropDownList" AutoPostBack="false"
                OnCallback="cbIssueType_Callback" ></dx:ASPxComboBox>
            </div>
            </div>
            <div style="float:right;padding-right:20px;">
               <dx:ASPxButton ID="btnOk" runat="server" Text="Ok" Width="50px" AutoPostBack="false" EnableClientSideAPI="true" >
                   <ClientSideEvents Click="btnOk_Click" />
               </dx:ASPxButton>
            </div>
            </div>
        </div>
    </div>
</div>
</div>


<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PickWorkItem.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.RMM.PickWorkItem" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .ms-formbody {
        background: none repeat scroll 0 0 #E8EDED;
        border-top: 1px solid #A5A5A5;
        padding: 3px 6px 4px;
        vertical-align: top;
    }

    .ms-formlabel {
        text-align: right;
        width: 170px;
        vertical-align: top;
        padding-top:7px;
        margin-left: 9px;
    }
    .ms-standardheader 
    {
        text-align: right;
    }
   .width25{width:25px;text-align:right}
    .txt-center {
        text-align:center;
    }
    .pickworkitem-action {
        padding-top:10px;
    }
    .accomp-popup input[type="text"], .accomp-popup select {
        color: inherit;
    }
</style>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function ShowLoadingPanel() {
        LoadingPanel.Show();
        return true;
    }

    function btnFindAllocation_click(s, e) {
        var startDate = '<%=startDate %>';
        var endDate = '<%=endDate %>';
        if (cbLevel2.GetValue() == null || cbLevel3.GetValue()==null) {
            alert('Please Pick Work Item');
        }
        else {
            var url = "<%=  UGITUtility.GetDelegateUrl("uGovernIT.FindResourceAvailability", true, "Find Resource")%>";
            url += "&ticketid=" + cbLevel2.GetValue() + "&pGlobalRoleId=" + cbLevel3.GetValue() + "&pStartDate=" + startDate + "&pEndDate=" + endDate;
            window.parent.UgitOpenPopupDialog(url, "", "Find Allocations for " + cbLevel2.GetValue(), "90", "90", false, "");
        }
    }

     $(document).ready(function () {
        $('.fetch-parent').parent().addClass("rmmFndRes-mainContainer");
    })
</script>
<div class="fetch-parent">
     <dx:ASPxLoadingPanel ID="LoadingPanel" runat="server" Text="Wait ..." ClientInstanceName="LoadingPanel" Modal="True">
        </dx:ASPxLoadingPanel>
    <div class="ms-formtable accomp-popup col-md-12 col-sm-12 col-xs-12">
        <div class="row" id="tr1" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Type
                </h3>
            </div>
            <div class="ms-formbody accomp_inputField">               
                <asp:DropDownList ID="ddlLevel1" CssClass="aspxDropDownList" runat="server" 
                    OnLoad="ddlLevel1_Load" AutoPostBack="true">
                </asp:DropDownList>
            </div>
        </div>
        <div class="row" id="workitem" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Work Item<b style="color: Red;">*</b>
                </h3>
            </div>
            <div class="ms-formbody accomp_inputField">

            <dx:ASPxComboBox ID="cbLevel2" OnLoad="cbLevel2_Load" runat="server" AutoPostBack="true" 
                DropDownStyle="DropDownList" ValueField="LevelTitle" TextField="LevelTitle" TextFormatString="{0}"
                ValueType="System.String" IncrementalFilteringMode="Contains" FilterMinLength="0"  EnableSynchronization = "True"
                CallbackPageSize="10" ClientInstanceName="cbLevel2" CssClass="CRMDueDate_inputField">
                <Columns>                                                      
                                                            
                </Columns>
                                                       
            </dx:ASPxComboBox>

            </div>
        </div>
        <div class="row" id="subitem" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel" id="lbSubitem" runat="server" >Sub Item<b style="color: Red;">*</b></h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <dx:ASPxComboBox ID="cbLevel3" runat="server"  OnLoad="cbLevel3_Load"
                DropDownStyle="DropDownList" TextFormatString="{0}" EncodeHtml="false" ClientInstanceName="cbLevel3"
                ValueType="System.String" IncrementalFilteringMode="Contains" FilterMinLength="0"  EnableSynchronization = "True" 
                    CallbackPageSize="10" CssClass="CRMDueDate_inputField"></dx:ASPxComboBox>
            </div>
        </div>
        <div class="col-md-12 col-sm-12 col-xs-12 findresource-action next-cancel-but" id="action" runat="server" >
                <dx:ASPxButton ID="btnFindAllocation" runat="server" Text="Find Resource" AutoPostBack="false" CssClass="primary-blueBtn next">
                    <%--<Image Url="/Content/images/searchNew.png"></Image>--%>
                    <ClientSideEvents Click="function(s,e){btnFindAllocation_click(s,e);}" />
                </dx:ASPxButton>
        </div>
        </div>
</div>

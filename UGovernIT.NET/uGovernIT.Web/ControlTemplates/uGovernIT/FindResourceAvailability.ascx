<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FindResourceAvailability.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.uGovernIT.FindResourceAvailability" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
    .allocationView {
        width: 215px; height: 45px;
    }
    .allocationView label {
        position:relative;
        top:2px;
        padding-left:3px;
    }
    .availableView {
        width: 330px; height: 45px;
    }
    .availableView label {
        position:relative;
        top:2px;
        padding-left:3px;
    }

    .valueViewMode, .viewProjectMode, .allocationView {
        float: left;
        border: 0px solid #D9DAE0;
        height: 30px;
        padding: 3px 5px 5px 5px;
        border-radius: 4px;
    }
    .fdres {
        float:left; 
        clear:both;
        padding-top: 5px;
    }
</style>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    {
        $(function () {
            var height = $(window).height();
            gvResourceAvailablity.SetHeight(height - 150);


            //$(".radiobutton").click(function () {
            //    loadingPanel.Show();
            //});
        });

        function AdjustSize(grid) {
            var height = $(window).height();
            gvResourceAvailablity.SetHeight(height - 150);

        }

        function ClickOnDrillDown(obj, fieldname, caption) {
            $('#<%= hdnSelectedDate.ClientID%>').val(fieldname);
            $("[id$='btnDrilDown']").get(0).click();
        }

        function ClickOnDrillUP(obj, caption) {
            $('#<%= hdnSelectedDate.ClientID%>').val(caption);
            $("[id$='btnDrilUp']").get(0).click();
        }

        function OnBatchStartEdit(s, e) {

        }

        function OnBatchEditEndEditing(s, e) {
            window.setTimeout(function () {
                var colcount = s.batchEditApi.grid.columns.length;
                for (var i = 3; i < colcount; i++) {
                    if (s.batchEditApi.grid.columns[i].fieldName != null && s.batchEditApi.grid.columns[i].fieldName.indexOf("ALL") < 0) {
                        var projectAllocation = s.batchEditApi.GetCellValue(e.visibleIndex, s.batchEditApi.grid.columns[i]);
                        try {
                            if ($("#checkboxcss_" + e.visibleIndex).is(":checked")) {
                                if (projectAllocation == 0)
                                    s.batchEditApi.SetCellValue(e.visibleIndex, s.batchEditApi.grid.columns[i], 100);
                                continue;
                            }
                            else {
                                if (projectAllocation == 100) {
                                    s.batchEditApi.SetCellValue(e.visibleIndex, s.batchEditApi.grid.columns[i], 0);
                                }
                                continue;

                            }

                            if (projectAllocation == null || !$.isNumeric(projectAllocation) || parseInt(projectAllocation) < 0 || parseInt(projectAllocation) > 100)
                                s.batchEditApi.SetCellValue(e.visibleIndex, s.batchEditApi.grid.columns[i], 0);

                        } catch (e) {
                            s.batchEditApi.SetCellValue(e.visibleIndex, s.batchEditApi.grid.columns[i], 0);
                        }
                    }
                }


            }, 10);
        }


        function ShowLoader() {
            loadingPanel.Show();
        }


        function FillAllocation(obj, objIndex) {
           
            gvResourceAvailablity.StartEditRow(objIndex);
            gvResourceAvailablity.StartEditRow(-1);


        }

        //function showTaskActions(trObj, ID) {
        //    $("[id$='actionButtons" + ID + "']").css("display", "block");
        //}

        //function hideTaskActions(trObj, ID) {
        //    //show description icon
        //    //$("#actionButtons" + taskID).css("display", "none");
        //    $("[id$='actionButtons" + ID + "']").css("display", "none");
        //}

        function findResourceGridViewCallBack(s, e) {
            if (typeof (s.cpfindResourceValidation) != 'undefined') {
                if (s.cpfindResourceValidation != "") {
                    $("#<%= lblMessage.ClientID %>").text(s.cpfindResourceValidation);
                    $("#<%= lblMessage.ClientID %>").css("visibility", "visible")
                }
                else {
                    $("#<%= lblMessage.ClientID %>").text("");
                    $("#<%= lblMessage.ClientID %>").css("visibility", "hidden")
                    
                }
            }
            
        }


        function CancleChanges()
        {
            $("#maindiv input:checkbox").prop("checked", false);
            gvResourceAvailablity.CancelEdit();
            return false;
        }

        function chklstComplexitytype_OnSelectedIndexChanged(s, e) {
                var index = e.index;
                if (e.isSelected) {
                    s.UnselectAll();
                    s.SetSelectedIndex(index);
                }
            }

    }
</script>


<dx:ASPxLoadingPanel ID="loadingPanel" runat="server" Text="Please Wait ..." ClientInstanceName="loadingPanel"
    Modal="True">
</dx:ASPxLoadingPanel>

<asp:HiddenField ID="hdncheckboxPercentage" runat="server" />

<div id="maindiv" style="padding-left: 15px;">
    <div style="padding-left: 0px; padding-top: 10px; padding-bottom: 10px;height: 80px;">
        <asp:HiddenField ID="hdnSelectedGroup" runat="server" />

        <%-- <div>--%>
        <div style="float: left; padding-right: 25px;">
            <fieldset class="availableView">
                <legend>Allocation:</legend>
                <asp:RadioButton ID="rbtnProject" runat="server" Text="Project" GroupName="Allocation" AutoPostBack="true" Checked="true" onchange="ShowLoader()" CssClass="radiobutton" OnCheckedChanged="rbtnProject_CheckedChanged" />
                <asp:RadioButton ID="rbtnTotal" runat="server" Text="Total" GroupName="Allocation" AutoPostBack="true" CssClass="radiobutton" onchange="ShowLoader()" OnCheckedChanged="rbtnTotal_CheckedChanged" />
                <asp:RadioButton ID="rbtnProjectTotal" runat="server" Text="Project/Total" GroupName="Allocation" AutoPostBack="true" onchange="ShowLoader()" CssClass="radiobutton" OnCheckedChanged="rbtnProjectTotal_CheckedChanged" />

            </fieldset>
        </div>

        <div style="float: left; padding-right: 25px;">
            <fieldset class="availableView">
                <legend>Available:</legend>
                <asp:RadioButton ID="rbtnFullyAvailable" runat="server" Text="Fully Available" GroupName="Available" AutoPostBack="true" onchange="ShowLoader()" Checked="true" CssClass="radiobutton" OnCheckedChanged="rbtnFullyAvailable_CheckedChanged" />
                <asp:RadioButton ID="rbtnPartiallyAvailable" runat="server" Text="Partially Available" GroupName="Available" AutoPostBack="true" onchange="ShowLoader()" CssClass="radiobutton" OnCheckedChanged="rbtnPartiallyAvailable_CheckedChanged" />
                <asp:RadioButton ID="rbtnAllResource" runat="server" Text="All Resource" GroupName="Available" AutoPostBack="true" CssClass="radiobutton" onchange="ShowLoader()" OnCheckedChanged="rbtnAllResource_CheckedChanged" />
            </fieldset>
        </div>
        <%--</div>--%>


       <div id="divComplexityType" runat="server" visible="false">
           <fieldset >
               <legend>Complexity:</legend>
               <dx:ASPxCheckBoxList ID="chklstComplexitytype" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal" AutoPostBack="true"
                   OnSelectedIndexChanged="chklstComplexitytype_SelectedIndexChanged">
                            <Paddings Padding="0px" />
                            <Items>
                                <dx:ListEditItem Text="Complexity" Value="0" />
                                <dx:ListEditItem Text="Capacity" Value="1" />
                            </Items>
                            <CaptionCellStyle Height="20px">
                                <Paddings Padding="0px" PaddingBottom="0px" PaddingLeft="0px" PaddingRight="0px" PaddingTop="0px" />
                            </CaptionCellStyle>
                            <RootStyle>
                                <Paddings Padding="0px" />
                            </RootStyle>
                           <Border BorderWidth="0" />
                            <BorderBottom BorderWidth="0" />
                           <BorderLeft BorderWidth="0" />
                           <BorderRight BorderWidth="0" />
                           <BorderTop BorderWidth="0" />
                        </dx:ASPxCheckBoxList>
           </fieldset>
       </div>
    </div>

    <div class="fdres">
    <dx:ASPxGridView ID="gvResourceAvailablity" runat="server" AutoGenerateColumns="false" KeyFieldName="UserIdGroup" OnHtmlDataCellPrepared="gvResourceAvailablity_HtmlDataCellPrepared"
        Theme="DevEx" OnDataBinding="gvResourceAvailablity_DataBinding" ClientInstanceName="gvResourceAvailablity" OnBatchUpdate="gvResourceAvailablity_BatchUpdate" OnRowValidating="gvResourceAvailablity_RowValidating"
        OnHtmlRowPrepared="gvResourceAvailablity_HtmlRowPrepared" OnAfterPerformCallback="gvResourceAvailablity_AfterPerformCallback">
        <Columns>
        </Columns>
        <Settings ShowFooter="false" ShowHeaderFilterButton="true" GroupFormat="{1}" ShowGroupButtons="false" />
        <SettingsBehavior AllowSort="false" AllowDragDrop="false" AutoExpandAllGroups="true" />
        <SettingsEditing Mode="Batch" BatchEditSettings-EditMode="Row" BatchEditSettings-StartEditAction="Click" BatchEditSettings-ShowConfirmOnLosingChanges="false"></SettingsEditing>
        <Settings ShowFooter="true" ShowColumnHeaders="true" ShowStatusBar="Hidden" VerticalScrollBarMode="Auto" VerticalScrollableHeight="0" />
        <SettingsPager Mode="ShowAllRecords"></SettingsPager>
        <Styles>
            <SelectedRow BackColor="#DBEAF9"></SelectedRow>
            <GroupRow ForeColor="Blue"></GroupRow>
        </Styles>

        <ClientSideEvents BatchEditStartEditing="OnBatchStartEdit" BatchEditEndEditing="OnBatchEditEndEditing" Init="function(s, e) { AdjustSize(s);  }" EndCallback="function(s, e) {   findResourceGridViewCallBack(s,e); }" />
     
    </dx:ASPxGridView>
        </div>

    <div class="next-cancel-but" style="float:right; clear:both; padding-top:5px;">
    <asp:Label ID="lblMessage" runat="server" style="padding-left:20px;color:red;vertical-align: -webkit-baseline-middle;"></asp:Label>

            <dx:ASPxButton ID="updateTask" runat="server" Text="Save Changes" CssClass="primary-blueBtn next">
                <ClientSideEvents Click="function(s, e){
                    gvResourceAvailablity.UpdateEdit();
                    }"/>
            </dx:ASPxButton>
            <dx:ASPxButton ID="cancelTask" runat="server" Text="Cancel changes" CssClass="RMMCancel-btn">
                <ClientSideEvents Click="function(s, e){
                    CancleChanges();
                    }"/>
            </dx:ASPxButton>
      </div>


    <div style="display: none;">
        
        <asp:Button ID="btnDrilDown" runat="server" OnClick="btnDrilDown_Click" OnClientClick="ShowLoader()" />
        <asp:Button ID="btnDrilUp" runat="server" OnClick="btnDrilUp_Click" OnClientClick="ShowLoader()" />
    </div>

</div>

<asp:HiddenField ID="hdndtfrom" runat="server" />
<asp:HiddenField ID="hdndtto" runat="server" />
<asp:HiddenField ID="hdndisplayMode" runat="server" />

<asp:HiddenField ID="hdnSelectedDate" runat="server" />

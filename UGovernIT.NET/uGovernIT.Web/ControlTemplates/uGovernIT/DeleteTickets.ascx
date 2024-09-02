<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DeleteTickets.ascx.cs" Inherits="uGovernIT.Web.DeleteTickets" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%--<SharePoint:CssRegistration ID="CssRegistration2" Name="/_layouts/15/1033/STYLES/Themable/ugitThemable.css" runat="server" />--%>
<%@ Import Namespace="uGovernIT.Utility" %>


<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">

    function btnDeleteClick() {
        var isChecked = false;
        var checkCount = $("[CustomType=treeViewControl] input[type=checkbox]:checked").length;
        if (checkCount > 0) {
            isChecked = true;
        }
        if (isChecked) {
            if (confirm("Are you sure you want to delete all the selected items?")) {
                
                loadingdeletetickets.Show();
                return true;
            }
            else {
                loadingdeletetickets.Hide();
                return false;
            }
        }
        else {
            alert("Please select item(s) to delete");
            return false;
        }
    }

    $(function () {
        $("[CustomType=treeViewControl] input[type=checkbox]").click("click", function () {
            OnCheckChange($(this));
        });
    })

    function OnCheckChange(obj) {
        if (obj != null) {
            var table = $(obj).closest("table");
            if (table.next().length > 0 && table.next()[0].tagName == "DIV") {
                //Is Parent CheckBox
                ChangeChildSelection(obj, table);
            }
            else {
                //Is Child CheckBox
                //  ChangeParentSelection(obj);
            }
        }
    }

    function ChangeParentSelection(obj) {
        if (obj != null && $(obj).length > 0) {
            var parentDIV = $(obj).closest("DIV");
            if ($("input[type=checkbox]", parentDIV).length == $("input[type=checkbox]:checked", parentDIV).length) {
                $("input[type=checkbox]", parentDIV.prev()).attr("checked", "checked");
            } else {
                $("input[type=checkbox]", parentDIV.prev()).removeAttr("checked");
            }
            var parentElement = $("input[type=checkbox]", parentDIV.prev());
            if (parentElement != null && parentElement != undefined) {
                ChangeParentSelection(parentElement);
            }
        }
    }

    function ChangeChildSelection(obj, table) {
        if (obj != null && $(obj).length > 0 && $(table) != null && $(table).length > 0) {
            var childDiv = table.next();
            var isChecked = $(obj).is(":checked");
            $("input[type=checkbox]", childDiv).each(function () {
                if (isChecked) {
                    $(this).attr("checked", "checked");
                } else {
                    $(this).removeAttr("checked");
                }
                var newtable = $(this).closest("table");
                if (newtable != null && $(newtable).length > 0) {
                    ChangeChildSelection($(this), newtable);
                }
            });
        }
    }

    function NextClick() {
        if (grid.GetSelectedKeysOnPage().length > 0) {
            hdnTicketIds.Set("IsNextClick", "true");
            hdnTicketIds.Set("SelectedTicketIds", grid.GetSelectedKeysOnPage());
            return true;
        }
        else {
            alert("Please select item(s)");
            return false;
        }
    }

    function lnkSelectionClick(s,e) {
        grid.UnselectAllRowsOnPage();
        e.processOnServer = false;
    }
</script>
<dx:ASPxLoadingPanel ID="loadingdeletetickets" runat="server" Text="Please wait, deleting ...."  Modal="True" ClientInstanceName="loadingdeletetickets"></dx:ASPxLoadingPanel>
<dx:ASPxHiddenField runat="server" ID="hdnTicketIds" ClientInstanceName="hdnTicketIds"></dx:ASPxHiddenField>
<div class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap" id="divList" runat="server" style="margin-top:10px;">
    <div class="ms-formtable accomp-popup">
        <div class="row">
            <ugit:ListPicker ID="lstPicker" runat="server" IsMultiSelect="true" />
        </div>
        <div class="row addEditPopup-btnWrap">
             <dx:ASPxButton ID="btnCancel" runat="server" ToolTip="Next" Text="Next" CssClass="secondary-cancelBtn"
                 OnClick="btnShowTicketInfo_Click">
                 <ClientSideEvents Click="function(s,e){ return NextClick();}" />
            </dx:ASPxButton>
            <dx:ASPxButton ID="btnSave" Visible="true" runat="server" Text="Clear Selection"
                ToolTip="Clear Selection" Style="float:left;margin-right:16px;" CssClass="primary-blueBtn"  >
                 <ClientSideEvents Click="function(s,e){lnkSelectionClick(s,e);}" />
            </dx:ASPxButton>
        </div>
    </div>
</div>

<div class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap" id="divDetails" runat="server" visible="false">
    <div class="ms-formtable accomp-popup">
        <div class="row">
            <label style="margin-bottom: 10px; margin-top:10px; font-weight: bold; font-size: 14px;">All checked items will be deleted! Please uncheck any items that are not to be deleted.</label>
        </div>
        <div class="row">
            <asp:Panel ID="pnlTree" runat="server" CssClass="treeCss deleteTicket-grid"></asp:Panel>
        </div>
        <div class="d-flex justify-content-between align-items-center mt-2">
            <dx:ASPxButton ID="btnDelete" Visible="true" runat="server" Text="Delete" CssClass="btn-danger1" ToolTip="Delete Items" OnClick="btnDelete_Click">
                <ClientSideEvents Click="function(s, e){return btnDeleteClick()}" />
            </dx:ASPxButton>
            <dx:ASPxButton ID="btnPrevious" Visible="true" runat="server" Text="Previous" ToolTip="Previous" OnClick="btnPrevious_Click" CssClass=" primary-blueBtn"></dx:ASPxButton>
        </div>
    </div>

<%--    <div style="bottom: 10px; vertical-align: bottom; margin-top: 8px;" class="fullwidth">
        <div style="float: left; float: left;">
            <asp:LinkButton  Text="&nbsp;&nbsp;New&nbsp;&nbsp;" 
                 Style="float: right; margin-bottom: 10px; float: left;">
                <span class="button-bg">
                     <b style="float: left; font-weight: normal;"><< Previous</b>
                     <i style="float: left; position: relative; top: 1px;left:2px"></i> 
                </span>
            </asp:LinkButton>
        </div>
        <div style="float: right; right: 15px;">
            <asp:LinkButton  Text="&nbsp;&nbsp;New&nbsp;&nbsp;" 
                 Style="float: right; margin-bottom: 10px;" OnClientClick="">
                <span class="button-bg">
                     <b style="float: left; font-weight: normal;">Delete Items</b>
                     <i style="float: left; position: relative; top: -2px;left:2px">
                          <img src="/Content/ButtonImages/cancel.png"  style="border:none;" title="" alt=""/>
                     </i> 
                </span>
            </asp:LinkButton>
        </div>
    </div>--%>

</div>

<div id="divconfirmMsg" visible="false" runat="server" class="col-md-12 col-sm-12 col-xs-12">
    <div class="ms-formtable accomp-popup">
        <div class="row" style="text-align:center;">
            <label class="delete-sucess-msg">Deletion Successful!</label>
            <br />
        </div>
        <div class="row addEditPopup-btnWrap">
            <dx:ASPxButton ID="btnClose" runat="server" Text="Close" CssClass="secondary-cancelBtn" ToolTip="Close" OnClick="btnClose_Click">
            </dx:ASPxButton>
        </div>
    </div>

    <div style="bottom: 10px; position: absolute; vertical-align: bottom; right: 10px">
         
<%--        <b id="btnCancel" title="Close" onclick="window.frameElement.commitPopup();">
            <span class="button-bg">
                <b style="float: left; font-weight: normal;">Close</b>
                <i style="float: left; position: relative; top: -3px; left: 2px">
                    <img src="/Content/ButtonImages/cancelwhite.png" style="border: none;" title="" alt="" />
                </i>
            </span>
        </b>--%>
    </div>
</div>

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomResourceAllocationCard.ascx.cs" Inherits="uGovernIT.Web.CustomResourceAllocationCard" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .disable-state {
    padding-left: 125px;
    top: 141px;
    margin-top: -95px;
}
    .titleHeight{
        height:35px;
    }
    .chkAllAssociate-chkBox{
        padding-top:7px;
        padding-bottom:7px;
        margin-bottom:0px !important;
    }

    .rmmcard-container .dx-button-has-icon .dx-icon {
    width:7px;
    height:12px;
    }
    .usercardcheck {
    visibility:hidden;
    }
</style>
<div class= "rmmcard-container mt-2">    
    <div class="row">
        <div class="col-md-2 noPadding">
            <div id="managerCard" class="rmm-managerCard"></div>
        </div> 
        <div class="col-md-10 text-left" >
            <div id="chkAllAssociate" class="chkAllAssociate-chkBox"></div>
            <div style="display:inline-block;">
                <div class="dvShowAllocationBtn" style="display:none">
                    <div id="showAllocation" ></div>                
                </div> 
            </div>
            <div style="float:right; display:flex">
                    <div id="previousYear" style="border: none; -webkit-box-shadow: none;"></div>    
                    <div id="yearLabel" style="margin-top:4px;font-size:16px;font-weight:500;"></div>
                    <div id="nextYear" style="border: none; -webkit-box-shadow: none;transform: rotate(180deg);"></div>
              </div>
            <hr class="rmmcard" />
            <div id="associateCard" class="associate-Card" ></div>

        </div>
    </div>    
</div>


<script data-v="<%=UGITUtility.AssemblyVersion %>">

    var date = new Date();
    var currentYear = $.cookie('SelectedYear') != null && $.cookie('SelectedYear') != '' && $.isNumeric($.cookie('SelectedYear')) ? parseInt($.cookie('SelectedYear')) : date.getFullYear();
    var checkAllAssociate;
    var associateCard;    
    var associateList = [];
    var containerHeight = "<%=Height != null && Height.Value > 0 ? Convert.ToInt32(Height.Value - 210) : 500 %>";
    //components
    $(function () {

        $("#yearLabel").text(currentYear);

        var nextbtn = $("#nextYear").dxButton({
            icon: "/content/images/RMONE/left-arrow.png",
            onClick: function (e) {
                currentYear = currentYear + 1;
                loadCards(currentYear);
                $.cookie('SelectedYear', currentYear);
                $("#yearLabel").text(currentYear);
            }
        });

        var previousbtn = $("#previousYear").dxButton({
            icon: "/content/images/RMONE/left-arrow.png",
            onClick: function (e) {
                currentYear = currentYear - 1;
                loadCards(currentYear);
                $.cookie('SelectedYear', currentYear);
                $("#yearLabel").text(currentYear);
            }
        });

      var dxBtnShowAllocation =  $("#showAllocation").dxButton({
            text: "Show Allocation",
            visible:false,
            onClick: function (e) {              
                openResourceAllocationDialog('', '', '');
            }
        }).dxButton('instance');

        checkAllAssociate =  $("#chkAllAssociate").dxCheckBox({                
                width: 150,
                text: "Check All",
                visible:false,
                onValueChanged: function (obj) {
                 CheckCardUser('all','chkAllAssociate');
                }

        }).dxCheckBox('instance');

        associateCard = $("#associateCard").dxTileView({                            
                            items: associateList,
                            height: containerHeight,
                            showScrollbar:true,
                            direction:"vertical",
                            baseItemHeight: 230,
                            baseItemWidth: 200,
                            itemMargin: 15,
                            noDataText:"",
                            itemTemplate: function (itemData, itemIndex, itemElement) {
                                var html = new Array();
                                html.push("<div class='resource-card'>");
                                if (itemData.PendingApprovalCount > 0)
                                    html.push("<div class='timesheet-count'  title='Timesheet Pending Approvals' onclick=event.cancelBubble=true;PendingTimesheetApprovals('" + itemData.ID + "','" + itemData.Title +"'); >" + itemData.PendingApprovalCount + "</div>");

                                html.push("<input type='checkbox' id='" + itemIndex + "-" + itemData.ID + "' onclick=event.cancelBubble=true;CheckCardUser('"+itemData.ID+ "',this.id) class='usercardcheck' name='userCheck'  />");

                                html.push("<div class='resource-img-container'>");
                                html.push("<img class='userpicture' src='" + itemData.imageUrl + "'>");
                                //html.push("<img src='/Content/Images/customer1.jpg' >");
                                html.push("</div>");
                                html.push("<div class='cardData-title'><p>" + itemData.Title + "</p></div>");
                                html.push(itemData.UsrEditLinkUrl );                       
                                html.push("<div class='resourceCard-innerData titleHeight'>" + itemData.JobTitle + "</div>");
                                html.push("<div class='resourceCard-innerData'><span># of Allocations </span>" + itemData.ProjectCount + "</div>");
                                html.push("<div class='resourcecard-allocationBar'>" + itemData.AllocationBar + "</div>");
                                html.push("</div>");                            
                                itemElement.append(html.join(""));
                            },
                            onContentReady: function (obj) {
                                setcheckbox(obj);
                            },
                            onItemClick: function (obj) {  
                                if (obj != null && obj.itemData != null){
                                    //singleAssistantClick(obj.itemData.ID);
                                }                               
                            }
                        }).dxTileView('instance');            
    });
    
    //ajax calls
    $(function () {
        loadCards(currentYear);
    });

    function loadCards(year) {
        var hdnChildOf = '<%=hdnChildOf%>';
        var hdnParentOf = '<%=hdnParentOf%>';
        $.ajax({
            url: "/api/rmmcard/GetLoadAsistants?hdnChildOf=" + hdnChildOf + "&hdnParentOf=" + hdnParentOf + "&Year=" + year,
            type: "GET",
            contentType: "application/json",
            success: function (data) {
                if (data != null) {
                    $('.associateCard').show();
                    associateCard.option("items", data.lstAssitantsResource);
                    associateCard.repaint();
                    if (data.lstAssitantsResource.length == 0) {
                        checkAllAssociate.option("visible", false);
                        $('.associate-Card').hide();

                    }

                }
                else {
                    checkAllAssociate.option("visible", false);
                }
            }
        });

        $.ajax({
            url: "/api/rmmcard/GetManager?hdnChildOf=" + hdnChildOf + "&hdnParentOf=" + hdnParentOf + "&Year=" + year,
            type: "GET",
            contentType: "application/json",
            success: function (data) {
                
                if (data != null) {
                    var html = new Array();

                    if (data.PendingApprovalCount > 0)
                        html.push("<div class='timesheet-count' title='Timesheet Pending Approvals' onclick=event.cancelBubble=true;PendingTimesheetApprovals('" + data.ID + "','" + data.Title +"'); >" + data.PendingApprovalCount + "</div>");

                    html.push("<div class='card-inner-container'>");
                    if (data.ManagerLinkUrl != '')
                        html.push(data.ManagerLinkUrl);
                    html.push("<div class='resource-img-container'>");
                    html.push("<img src='" + data.imageUrl + "'>");
                    html.push("</div>");
                    html.push("<div>" + data.UsrEditLinkUrl + "</div>");
                    html.push("<div class='card-inner-data'><span>" + data.JobTitle + "</div>");
                    html.push("<div class='card-inner-data'><span># of Resources </span>" + data.AssitantCount + "</div>");
                    html.push("<div class='card-inner-data'><span># of Allocations </span>" + data.ProjectCount + "</div>");
                    html.push("<div class='card-allocationBar'>" + data.AllocationBar + "</div>");
                    html.push("</div>");
                    html.join('');
                    $("#managerCard").html(html.join(''));

                }
            }
        });
    }

    function singleAssistantClick(userId)
    {
        path = '<%=absoluteUrlEdit%>';
        var IDs = userId;
        path = path + "&ID=" + IDs + "&isRedirectFromCardView=true&singleselection=true" ;  
        UgitOpenPopupDialog(path, '', "Resource Allocation", 90, 90, false, '');
    }

    function openResourceAllocationDialog(path, titleVal, returnUrl) {        
        path = '<%=absoluteUrlEdit%>';
        var IDs = $.cookie('SelectedUsers');
        path = path + "&ID=" + IDs + "&isRedirectFromCardView=true";  
        UgitOpenPopupDialog(path, '', "Resource Allocation", 90, 90, false, '');
    }

    function PendingTimesheetApprovals(Id, Name) {
        window.parent.UgitOpenPopupDialog('<%=timesheetPendingAprvlPath %>&Id=' + Id, "", "Timesheet Pending Approvals for " + Name, "600px", "500px");
    }

</script>
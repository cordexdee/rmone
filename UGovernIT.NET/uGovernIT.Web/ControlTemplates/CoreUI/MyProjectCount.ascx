<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MyProjectCount.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.CoreUI.MyProjectCount" %>

<%@ Import Namespace="uGovernIT.Utility" %>
<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .projects-status {    display: flex;    text-align: center;    width: fit-content;}

.projects-status .state .counts {
    height: 50px;
    width: 50px;
    display: flex;
    align-items: center;
    justify-content: center;
    margin-left: 15px;
    margin-right: 15px;
    border-radius: 50%;
    margin-bottom: 4px;
    line-height: 1;
    cursor: pointer;
    color: grey;
    font-weight: 600;
    background-color: white;
}.projects-status .state.PendingAssignment .counts {    border: 3px solid #f6a155;}.projects-status .state.ConstructionContract .counts {    border: 3px solid #00a150;}.projects-status .state.Closeout .counts {    border: 3px solid #FA6F00;}.projects-status .state.thingToDo .counts {    border: 3px solid black;}.projects-status .state .counts span {    font-size: 16px;}.projects-status .state p {    color: #919293;    font-size: 12px;}.state{
    padding-left:5px;
    display: block;
    margin-left: auto;
    margin-right: auto;
}
.projects-status {
    display: flex;
    text-align: center;
    width: 100%;
}

.CircleLabel{
    font-size: 12px;
    text-align: center;
    color:grey;
    font-family: 'Poppins', sans-serif;
}
</style>

<script data-v="<%=UGITUtility.AssemblyVersion %>">
    var baseUrl = ugitConfig.apiBaseUrl;

    $.get(baseUrl + "/api/CoreRMM/GetExecutiveViewCounts", function (data) {
    
        var divArray = $(".state");
        for (i = 0; i < data.length; i++) {
            const count = parseInt(data[i].Count);
            if (count > 0) {
                $(divArray[i]).html(`<div class='counts' onclick='showProjectList("${data[i].KpiName}")'><span>${data[i].Count}</span></div>
                                            <div class='CircleLabel'>${data[i].KpiLabel}</div>`)
            } else {
                $(divArray[i]).html(`<div class='counts' ><span>${data[i].Count}</span></div>
                                            <div class='CircleLabel'>${data[i].KpiLabel}</div>`)
            }
        }
    });

    function showProjectList(filtername) {
        if (filtername == "Things to do") {
            window.parent.UgitOpenPopupDialog('<%=viewTasksPath%>', "", 'Task', '85%', '900', 0, "", true, true);
        } else {
            window.parent.UgitOpenPopupDialog("/Layouts/uGovernIT/DelegateControl.aspx?control=ExecutiveViewDrillDown&Filter=" + filtername, "", "Projects List", 95, 90, 0, '');
        }

    }
</script>

<div class="projects-status">    <div class="state PendingAssignment">        <div class="counts"><span>0</span></div>        <p>Pending<br>Assignment</p>    </div>    <div class="state ConstructionContract">        <div class="counts"><span>0</span></div>        <p>Construction<br>Contract</p>    </div>    <div class="state Closeout">        <div class="counts"><span>0</span></div>        <p>Closeout<br> Due</p>    </div><%--    <div class="state thingToDo">        <div class="counts"><span>0</span></div>        <p>Things to Do</p>    </div>--%></div>

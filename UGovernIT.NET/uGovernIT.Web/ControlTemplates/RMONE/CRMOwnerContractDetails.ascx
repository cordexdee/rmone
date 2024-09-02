<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CRMOwnerContractDetails.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.RMONE.CRMOwnerContractDetails" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script data-v="<%=UGITUtility.AssemblyVersion %>">
    $(document).ready(function () {
        $(".CRMOwnerConract").parent().removeClass("px-3");
    });
</script>
<div class="CRMOwnerConract" style="display: flex; flex-wrap: wrap; margin-top: 12px;">
    <div class="field_box" style="width: 33.33%;">
        <div class="flex-container">
            <span class="field_box_label">Contract Type</span>
        </div>
        <div>
            <span class=""><%=ContractType%></span>
        </div>
    </div>
    <div class="field_box" style="width: 33.33%;">
        <div class="flex-container">
            <span class="field_box_label">Fee</span>
        </div>
        <div>
            <span class=""><%=FeePct %></span>
        </div>
    </div>
    <div class="field_box" style="width: 33.33%;">
        <div class="flex-container">
            <span class="field_box_label">Contract Amount</span>
        </div>
        <div>
            <span class="">$<%=ApproxContractValue%></span>
        </div>
    </div>
    <div class="field_box" style="width: 33.33%;">
        <div class="flex-container">
            <span class="field_box_label">Commencement</span>
        </div>
        <div>
            <span class=""><%=ConstructionStart %></span>
        </div>
    </div>
    <div class="field_box" style="width: 33.33%;">
        <div class="flex-container">
            <span class="field_box_label">Completion</span>
        </div>
        <div>
            <span class=""><%=ConstructionEnd %></span>
        </div>
    </div>
    <div class="field_box" style="width: 33.33%;">
        <div class="flex-container">
            <span class="field_box_label">Project Type</span>
        </div>
        <div>
            <span class=""><%=RequestType %></span>
        </div>
    </div>
    <div class="field_box" style="width: 33.33%;">
        <div class="flex-container">
            <span class="field_box_label">GL Insurance</span>
        </div>
        <div>
            <span class=""><%=GLInsurance %></span>
        </div>
    </div>
    <div class="field_box" style="width: 33.33%;">
        <div class="flex-container">
            <span class="field_box_label">SDI</span>
        </div>
        <div>
            <span class=""><%=SDI %></span>
        </div>
    </div>
    <div class="field_box" style="width: 33.33%;">
        <div class="flex-container">
            <span class="field_box_label">Bond</span>
        </div>
        <div>
            <span class=""><%=Bond %></span>
        </div>
    </div>
    <%--<%if(ModuleName == "OPM") {  %>
    <div class="field_box" style="width: 33.33%;">
        <div class="flex-container insurance">
            <span class="field_box_label">Insurance</span>
        </div>
        <div>
            <span class=""><%=Insurance%></span>
        </div>
    </div>
    <%} %>--%>
</div>

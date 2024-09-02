<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CPRProjectTitleControl.ascx.cs" Inherits="uGovernIT.Web.CPRProjectTitleControl" %>
<div class="col-md-12 col-sm-12 col-xs-12 summaryTab_container">
    <div class="row" id="trProjectrow" runat="server">
        <div class="col-md-4 ol-sm-4 col-xs-6 paddingNo">
            <div class="cprSummary_labelWrap">
                <label id ="lblEstimateNo" runat="server">Estimate #</label>
            </div>
            <div class="cprSummary_valueWrap">
                <label id ="lblEstimateNoVal" runat="server"></label>
            </div>
        </div>
        <div class="col-md-4 ol-sm-4 col-xs-6 paddingNo">
            <div class="cprSummary_labelWrap">
                <label id ="lblProjectName" runat="server">Project Name</label>
            </div>
            <div class="cprSummary_valueWrap">
                <label id ="lblProjectNameVal" runat="server"></label>
            </div>
        </div>  
        <div class="col-md-4 ol-sm-4 col-xs-6 paddingNo">
            <div class="cprSummary_labelWrap">
                <label id ="lblClient" runat="server">Client</label>
            </div>
            <div class="cprSummary_valueWrap">
                <label id ="lblClientVal" runat="server"></label>
            </div>
        </div>
    </div>
        <div class="row" id="trProjectrow2" runat="server">
<%--            <div class="col-md-4 ol-sm-4 col-xs-6 paddingNo">
                <div class="cprSummary_labelWrap">
                    <label id ="lblProjectNo" runat="server" >Project #</label>
                </div>
                <div class="cprSummary_valueWrap">
                    <label id ="lblProjectNoVal" runat="server"></label>
                </div>
            </div>--%>
            <div class="col-md-4 ol-sm-4 col-xs-6 paddingNo">
                <div class="cprSummary_labelWrap">
                    <label id ="lblERPJobID" runat="server" >CMIC #</label>
                </div>
                <div class="cprSummary_valueWrap">
                    <label id ="lblERPJobIDVal" runat="server"></label>
                </div>
            </div>
            <div class="col-md-4 ol-sm-4 col-xs-6 paddingNo">
                <div class="cprSummary_labelWrap">
                     <label id ="lblProjectType" runat="server">Project Type</label>
                </div>
                <div class="cprSummary_valueWrap">
                    <label id ="lblProjectTypeVal" runat="server"></label>
                </div>
            </div>
        </div>
        <div class="row" id="trOPMProject" runat="server">
            <div class="col-md-6 ol-sm-6 col-xs-6 paddingNo">
                 <div class="cprSummary_labelWrap">
                    <label id ="lblOPMProjectName" runat="server">Project Name</label>
                 </div>
                <div class="cprSummary_valueWrap">
                    <label id ="lblOPMProjectNameval" runat="server"></label>
                </div>
            </div>
            <div class="col-md-6 ol-sm-6 col-xs-6 paddingNo">
                <div class="cprSummary_labelWrap">
                    <label id ="lblOPMProjectClient" runat="server" >Client</label>
                 </div>
                <div class="cprSummary_valueWrap">
                    <label id ="lblOPMProjectClientval" runat="server"></label>
                </div>
            </div>
        </div>

         <div class="row  ugit-tdetaillabel"  id="trOPMProjectDetail" runat="server">
              <div class="col-md-12 ol-sm-12 col-xs-12 paddingNo">
                 <div class="cprSummary_labelWrap ">
                    <label id ="lblOPMProjectDescription" runat="server">Project Description</label>
                 </div>
                <div class="cprSummary_valueWrap">
                    <label id ="lblOPMProjectDescriptionval" runat="server">&nbsp;</label>
                </div>
             </div>
        </div>
</div>

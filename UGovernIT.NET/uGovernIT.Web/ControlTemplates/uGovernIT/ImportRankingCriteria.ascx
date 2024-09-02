<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ImportRankingCriteria.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.uGovernIT.ImportRankingCriteria" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>


<div id="tb1" class="ms-formtable ms-formtable accomp-popup col-md-12 col-sm-12 col-xs-12" style="padding:0px;">
    <div class="row">
        <div class="col-md-12 col-sm-12 col-xs-12" id="trModule" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Are you sure ,You wants to import Ranking Criteria ?
                </h3>
            </div>
            
             <div class="ms-formlabel" runat="server" id="divOveride" visible="false">
                <h3 class="ms-standardheader budget_fieldLabel">Are you sure ,You wants to import Ranking Criteria ?
                </h3>
            </div>
            
            
           <%-- <div class="ms-formbody accomp_inputField">
                <asp:DropDownList ID="ddlCheckListTemplate" runat="server" AppendDataBoundItems="true">
                </asp:DropDownList>

                <asp:RequiredFieldValidator InitialValue="0" ID="ReqCheckListTemplate" Display="Dynamic" runat="server" ControlToValidate="ddlCheckListTemplate"
                     Text="Field Required." ErrorMessage="ErrorMessage"></asp:RequiredFieldValidator>
            </div>--%>
        </div> 
    </div>
    <div class="row">
        <div class="col-md-12 col-sm-12 col-xs-12 importTask_btnWrap">
            <div class="importBTn col-xs-12">
                <asp:LinkButton ID="btnImportCheckListTemplate" runat="server" Text="&nbsp;&nbsp;Import CheckList Template&nbsp;&nbsp;" ToolTip="Import CheckList Template"   OnClick="btnImportRankingCriteria_Click" >
                    <span class="addchkList_btn">
                        <b style="font-weight:500;"> Yes </b>
                        <%--<i style="float: left; position: relative; top: -3px;left:2px">
                            <img src="/Content/images/uGovernIT/save.png"  style="border:none;" title="" alt=""/>
                        </i> --%>
                    </span>
                </asp:LinkButton>
            </div>
            <div class="Import_cancelBtn col-xs-12">
                <asp:LinkButton ID="btnCancel" runat="server" Text="&nbsp;&nbsp;Cancel&nbsp;&nbsp;"
                    ToolTip="Cancel" OnClick="btnCancel_Click">
                    <span class="cancelBtn">
                        <b style="font-weight: 500;">No</b>
                        <%--<i style="float: left; position: relative; top: -3px;left:2px">
                            <img src="/Content/images/uGovernIT/cancelwhite.png"  style="border:none;" title="" alt=""/>
                        </i> --%>
                    </span>
                </asp:LinkButton>
            </div>
        </div>
    </div>
</div>
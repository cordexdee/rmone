<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AdminCatalog.ascx.cs" Inherits="uGovernIT.Web.AdminCatalog" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
    .fleft {
        float: left;
    }

    .cg-d-main {
        position: relative;
    }

    .ctrcontainer {
        float: left;
        padding-left: 10px;
    }

    .cg-dashboardaction-icon {
        float: right;
        padding-left: 3px;
        position: relative;
        right: -14px;
        top: 2px;
    }

    .cg-drilldownback {
        width: 16px;
        height: 16px;
        float: left;
        padding-left: 2px;
        position: relative;
        top: 3px;
        left: 3px;
    }

    .cg-dashboardtopaction-type1 {
        position: relative;
        top: -12px;
        right: 6px;
    }

    .cg-dashboardtopaction-type2 {
        position: relative;
        top: 0px;
        right: 6px;
    }

    .cg-dashboardbottomaction {
        float: right;
        position: relative;
        right: -10px;
        top: -5px;
    }

    .cg-d-contentc {
        position: relative;
        left: -11px;
    }

    .cg-d-description {
        position: absolute;
        padding-left: 10px;
        float: left;
        font-weight: bold;
        width: 275px;
        top: -5px;
        z-index: 10;
    }

    .cg-d-returnactionc {
        float: left;
        position: absolute;
        left: -8px;
    }

    .dashboard-desc {
        font-weight: normal;
        float: left;
        padding-left: 4px;
        font-size: 11px;
    }

    .dashboardkpi-main {
        margin-bottom: 5px;
        display: block;
    }

    .dashboardkpi-main-min {
        margin-bottom: 1px;
        display: block;
    }

    .dashboardkpi-txt {
        padding: 2px;
    }

        .dashboardkpi-txt:hover {
            color: #000;
        }

    .dashboardkpi-td {
        border: 2px outset #F4F4F4;
        background: #F8F8F8;
        padding: 0px 2px;
    }

    .dashboardkpi-a {
        font-size: 12px;
    }

    .dashboardkpi-a-min {
        font-size: 10px;
    }

    .dashboardaction-icon {
        float: right;
        padding-left: 3px;
        position: relative;
        right: -14px;
        top: 2px;
    }

    .service-catalog {
        float: left;
        width: 860px;
        overflow-y: hidden;
        overflow-x: auto;
    }

    * + html .service-catalog {
        width: 860px;
    }

        .service-catalog td:Hover {
        }

    .service-catalog-inner {
        float: left;
        width: 99%;
        margin-left: 3px;
    }

    .populate-services {
        float: left;
        width: 100%;
    }

        .populate-services fieldset {
            float: left;
            width: 97%;
        }

        .populate-services select {
            height: 23px;
            max-width: 300px;
            min-width: 125px;
        }

    .servciecatalog-main {
        float: left;
        width: 860px;
        border: 2px outset lightgray;
    }
    /*width:826px*/
    * + html .servciecatalog-main {
        width: 860px;
    }


    .category-block {
        float: left;
        padding-left: 5px;
        padding-right: 5px;
        font-weight: bold;
    }

    .service-block {
        float: left;
        padding-right: 5px;
        padding-left: 10px;
        font-weight: bold;
    }

    .serviceaction-block {
        float: left;
    }

    .categorylist {
        width: 200px;
    }

    .service-catalog th {
        width: 200px;
    }

    .oneservice-container {
        float: left;
    }

    .service-ul {
        margin: 0;
        padding: 0;
        list-style-type: none;
        text-align: center;
        float: left;
        width: 780px;
        padding: 15px 0;
    }

        .service-ul li {
            display: inline;
            list-style-type: none;
            padding-right: 20px;
            width: 230px;
            float: left;
            padding-bottom: 10px;
        }

    .servicecategory {
        padding-right: 5px;
        font-weight: normal;
        width: 100px;
        border-top: 2px solid black;
        display: table-cell;
    }

    .servicetype {
        padding-right: 5px;
        font-weight: normal;
        max-width: 140px;
        border-top: 2px solid black;
        display: table-cell;
    }
    .serviceitem a{
        color: #000;
    }
    .serviceitem {
        border-bottom: 1px solid #bcbcbc;
        height: 20px;
        padding: 3px 0;
    }

    .serviceitem-first {
        border-top: 1px solid black;
    }

    .service-moreservices {
        float: left;
        padding: 10px;
    }

    .service-alternate {
        background: #F8F8F8;
    }

    .import-data {
        position: absolute;
        margin-left: 22px;
        margin-top: 50px;
        z-index: 10;
    }
</style>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function OpenImportExcel() {
        window.parent.UgitOpenPopupDialog('<%= importUrl %>', "", 'Master Import', '400px', '150px', 0, escape("<%= Request.Url.AbsolutePath %>"));
        return false;
    }
    
</script>

<div id="newDiv" Style="display:none">
    <h3 class="heading">SERVICE PRIME SET-UP</h3>
    <div class="col-md-4 col-sm-2 hidden-xs"></div>
    <div class="col-md-4 col-sm-7 col-xs-12 main-container">
        <ul class="module-list">
            <li class="list">
                <%--<a id="anchorElement" onclick="window.parent.UgitOpenPopupDialog('/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=configvar','','Configuration Variables','80','80', false, '', false)" style="cursor:pointer">--%>
                <a id="anchorElement" style="cursor:pointer">
                    <div class="image">
                        <img src="/Content/Images/configurationVariable.png">
                    </div>
                    <div class="vr-line">
                        <span class="dot"></span>
                    </div>
                    <div class="module-content">
                        <h5 class="module-name" id="">Configuration Variable</h5>
                        <p class="module-discription">Thease control the operation of Core</p>
                    </div>
                </a>
            </li>
            <li class="list">
                <a onclick="window.parent.UgitOpenPopupDialog('/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=moduleview','','Modules','80','80', false, '', false)" style="cursor:pointer">
                    <div class="image">
                        <img src="/Content/Images/modules.png">
                    </div>
                    <div class="vr-line">
                        <span class="dot dot1"></span>
                    </div>
                    <div class="module-content">
                        <h5 class="module-name">Modules</h5>
                        <p class="module-discription">Set up modules and configure them for the specific tenant.</p>
                    </div>
                </a>
            </li>
            <li class="list">
                <a onclick="window.parent.UgitOpenPopupDialog('/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=formlayoutandpermission','','Form Layout','90','90','','')"  style="cursor:pointer">
                    <div class="image">
                        <img src="/Content/Images/formLayout.png">
                    </div>
                    <div class="vr-line">
                        <span class="dot dot2"></span>
                    </div>
                    <div class="module-content">
                        <h5 class="module-name">Form Layout</h5>
                        <p class="module-discription">Design the forms for the tenant from the pre-bulid out of the box forms.</p>
                    </div>
                </a>
            </li>
            <li class="list">
                <a href="#">
                    <div class="image">
                        <img src="/Content/Images/requestList.png">
                    </div>
                    <div class="vr-line">
                        <span class="dot dot3"></span>
                    </div>
                    <div class="module-content">
                        <h5 class="module-name">Request List</h5>
                        <p class="module-discription">Define the specific request list which display summary forms on the landing pages.</p>
                    </div>
                </a>
            </li>
            <li class="list">
                <a href="#">
                    <div class="image">
                        <img src="/Content/Images/requesttype.png">
                    </div>
                    <div class="vr-line">
                        <span class="dot dot4"></span>
                    </div>
                    <div class="module-content">
                        <h5 class="module-name">Request Types</h5>
                        <p class="module-discription">Configure applicable request types for the modules using pre-build request types.</p>
                    </div>
                </a>
            </li>
            <li class="list">
                <a href="#">
                    <div class="image">
                        <img src="/Content/Images/organization.png">
                    </div>
                    <div class="vr-line">
                        <span class="dot dot5"></span>
                    </div>
                    <div class="module-content">
                        <h5 class="module-name">Orginzation</h5>
                        <p class="module-discription">Set up the tenant Departments.</p>
                    </div>
                </a>
            </li>
            <li class="list">
                <a href="#">
                    <div class="image">
                        <img src="/Content/Images/functionalArea.png">
                    </div>
                    <div class="vr-line">
                        <span class="dot dot6"></span>
                    </div>
                    <div class="module-content">
                        <h5 class="module-name">Functional Areas</h5>
                        <p class="module-discription">Set up the functional areas for the tenant.</p>
                    </div>
                </a>
            </li>
            <li class="list">
                <a href="#">
                    <div class="image">
                        <img src="/Content/Images/locations.png">
                    </div>
                    <div class="vr-line">
                        <span class="dot dot7"></span>
                    </div>
                    <div class="module-content">
                        <h5 class="module-name">Locations</h5>
                        <p class="module-discription">Set up the geographic locations.</p>
                    </div>
                </a>
            </li>
            <li class="lastLi">
                <a href="#">
                    <div class="image">
                        <img src="/Content/Images/moduleDefault.png">
                    </div>
                    <div class="lastDot"></div>
                    <div class="module-content">
                        <h5 class="module-name">Module Defaults</h5>
                        <p class="module-discription">Type your text hear.</p>
                    </div>
                </a>
            </li>

        </ul>
    </div>
</div>

<div class="import-data" id="importData">
    <asp:LinkButton ID="btnMasterImport" runat="server" Text="&nbsp;&nbsp;Master Import&nbsp;&nbsp;" Visible="false" ToolTip="Master Import" OnClientClick="return OpenImportExcel();">
            <span class="button-bg">
                <b style="float: left; font-weight: normal;">
                    Master Import</b>
                <i style="float: left; position: relative; top: -3px;left:2px">
                    <img src="/Content/Images/import.png"  style="border:none;" title="" alt=""/>
                </i> 
            </span>
    </asp:LinkButton>
</div>
<div style="padding: 15px " id="repeaterForCatagory">
    <asp:Repeater runat="server" ID="RptCategory" OnItemDataBound="RptCategory_ItemDataBound">
        <ItemTemplate>
            <table cellpadding="0" cellspacing="0" style="border-collapse: collapse;">
                <tr id="Tr1" runat="server">
                    <td class="cg-topleft-corner" align="left"></td>
                    <td class="cg-topmiddle-line" align="right"></td>
                    <td class="cg-topright-corner" align="right"></td>
                </tr>
                
                <tr>
                    <td class="cg-middleleft-line"></td>
                    <td style="background: #F8F8F8; position: relative; width: 900px; z-index:0" valign="top">
                        <div class="dashboard-panel">
                            <asp:Panel ID="Panel1" runat="server" class="cg-d-main dashboardpanelcontainer ">
                                <asp:Panel ID="Panel2" runat="server" CssClass="cg-dashboardtopaction-type1">
                                    <table style="border-collapse: collapse; width: 100%" cellpadding="0" cellspacing="0">
                                        <tr style="height: 20px"></tr>
                                        <tr>
                                            <td style="width: 120px; height: auto; text-align: center; vertical-align: middle;"><b>
                                                <asp:Label ID="LblCategroy" runat="server" Text='<%#Eval("Title")%>'></asp:Label></b></td>
                                            <td valign="top">
                                                <div style="float: left; width: 780px; overflow-x: auto; overfloat-y: hidden;">
                                                    <asp:Repeater ID="RptSubCategory" runat="server" OnItemDataBound="RptSubCategory_ItemDataBound">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="lULTop" runat="server" Text="<ul class='service-ul'>" Visible="false"></asp:Literal>
                                                            <li>
                                                                <table cellspacing="0" border="0" style="border-collapse: collapse;">
                                                                    <tr>
                                                                        <td class="servicecategory" valign="top">
                                                                            <b>
                                                                                <asp:Label ID="LblSubCategroy" runat="server" Text='<%#Eval("CategoryName")%>'></asp:Label></b><br />
                                                                            <br />
                                                                            <img id="imgCategory" runat="server" width="50" src='<%#Eval("ImageUrl")%>' alt="" />


                                                                        </td>

                                                                        <td valign="top" class="servicetype">

                                                                            <asp:Repeater ID="RptItemList" runat="server" OnItemDataBound="RptItemList_ItemDataBound">
                                                                                <ItemTemplate>
                                                                                    <table>
                                                                                        <tr>
                                                                                            <td style="width: 140px; text-align: left;" class="serviceitem">
                                                                                                <asp:LinkButton ID="LnkListName" runat="server" Text='<%#Eval("Title")%>'></asp:LinkButton></td>

                                                                                        </tr>
                                                                                    </table>


                                                                                </ItemTemplate>
                                                                            </asp:Repeater>

                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </li>
                                                            <asp:Literal ID="lULBottom" runat="server" Text="</ul>" Visible="false"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                    </ul>
                                                </div>

                                                <%--<span style="float: left; width: 100%; border-bottom: 2px solid #D9D9D9;"></span>--%>
                                            </td>


                                        </tr>
                                    </table>
                                </asp:Panel>
                            </asp:Panel>
                        </div>
                    </td>
                    <td class="cg-middleright-line"></td>
                </tr>

                <tr id="Tr2" runat="server">
                    <td class="cg-bottomleft-corner" align="left" valign="top"></td>
                    <td class="cg-bottommiddle-line" align="right" valign="top"></td>
                    <td align="right" valign="top" class="cg-bottomright-corner"></td>
                </tr>

                <tr id="Tr3" runat="server" visible="false">
                    <td class="cg-centerleft-line"></td>
                    <td class="cg-centermiddle-line" align="right" valign="top"></td>
                    <td class="cg-mcenterright-line"></td>
                </tr>
            </table>
        </ItemTemplate>
    </asp:Repeater>

    <table cellpadding="0" cellspacing="0" style="border-collapse: collapse;">
        <tr id="topType1" runat="server">
            <td class="cg-topleft-corner" align="left"></td>
            <td class="cg-topmiddle-line" align="right"></td>
            <td class="cg-topright-corner" align="right"></td>
        </tr>
        <tr>
            <td class="cg-middleleft-line"></td>
            <td style="background: #F8F8F8; position: relative; width: 900px;" valign="top">
                <div class="dashboard-panel">
                    <asp:Panel ID="Panel3" runat="server" class="cg-d-main dashboardpanelcontainer ">
                        <asp:Panel ID="dashboardTopAction" runat="server" CssClass="cg-dashboardtopaction-type1">
                            <div>
                                <table style="height: 130px; width: 100%; border-collapse: collapse" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="width: 100px; text-align: center; vertical-align: middle;"><b>System Management</b></td>
                                        <td valign="top" style="width: 780px;">
                                            <table style="width: 100%;">
                                                <tr>
                                                    <td class="serviceitem" style="width: 20%; text-align: center; vertical-align: middle; border: none;">
                                                        <a id="Alicencse" runat="server" href="">
                                                            <img id="ImgLicencse" runat="server" width="100" height="100" style="border: none" src="/Content/ButtonImages/Liecence.png" alt="" />
                                                            <br />
                                                            <b>
                                                                <asp:Label ID="LblLicense" runat="server" Text="Licenses"></asp:Label></b>
                                                        </a>
                                                    </td>
                                                    <td class="serviceitem" style="width: 20%; text-align: center; vertical-align: middle; border: none;" id="tdServiceBlock" runat="server">
                                                        <a id="Aservice" runat="server" href="">
                                                            <img id="ImgServices" runat="server" width="100" height="100" style="border: none" src="/Content/ButtonImages/Services.png" alt="" />
                                                            <br />
                                                            <b>
                                                                <asp:Label ID="LblServices" runat="server" Text="Service Catalog & Agents"></asp:Label></b>
                                                        </a>
                                                    </td>
                                                    <td class="serviceitem" style="width: 20%; text-align: center; vertical-align: middle; border: none;">
                                                        <a id="Achache" runat="server" href="">
                                                            <img id="Imgcache" runat="server" width="100" height="100" style="border: none" src="/Content/ButtonImages/data_cache.png" alt="" />
                                                            <br />
                                                            <b>
                                                                <asp:Label ID="LblCache" runat="server" Text="Cache"></asp:Label></b>
                                                        </a>
                                                    </td>
                                                    <td class="serviceitem" style="width: 20%; text-align: center; vertical-align: middle; border: none;">
                                                        <a id="Adashbord" runat="server" href="">
                                                            <img id="Imgdashboard" runat="server" height="100" style="border: none" width="100" src="/Content/ButtonImages/Dashboards.png" alt="" />
                                                            <br />
                                                            <b>
                                                                <asp:Label ID="Lbldashboard" runat="server" Text="Dashboards & Queries"></asp:Label></b>
                                                        </a>

                                                    </td>
                                                    <td class="serviceitem" style="width: 20%; text-align: center; vertical-align: middle; border:none;">
                                                        <a id="Afacttable" runat="server" href="">
                                                            <img id="ImgFactTable" runat="server" height="100" width="100" style="border: none" src="/Content/ButtonImages/fact_table.png" alt="" />
                                                            <br />
                                                            <b>
                                                                <asp:Label ID="LblFactTable" runat="server" Text="Fact Tables"></asp:Label></b>
                                                        </a>
                                                    </td>
                                                    <td class="serviceitem" style="width: 20%; text-align: center; vertical-align: middle;border:none;">
                                                        <a id="ATenantManagement" runat="server" href="">
                                                            <img id="ImgATenantManagement" runat="server" height="100" width="100" style="border: none" src="/Content/ButtonImages/tenant.png" alt="" />
                                                            <br />
                                                            <b>
                                                                <asp:Label ID="LblTenant" runat="server" Text="Tenant Management"></asp:Label></b>
                                                        </a>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </asp:Panel>
                    </asp:Panel>
                </div>
            </td>
            <td class="cg-middleright-line"></td>
        </tr>

        <tr id="bottomType1" runat="server">
            <td class="cg-bottomleft-corner" align="left" valign="top"></td>
            <td class="cg-bottommiddle-line" align="right" valign="top"></td>
            <td align="right" valign="top" class="cg-bottomright-corner"></td>
        </tr>
    </table>
</div>

<%@ Control Language="C#" CodeBehind="UserEntryControl.ascx.cs" Inherits="uGovernIT.Web.UserEntryControl" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<div id="main">
    <div class="row" style="margin-top: 8%;">
        <div class="col-md-6" style="margin-top: 4%;">

            <p>
                Right People
                   <br />
                Right Project
            </p>

            <div class="row" style="margin-top: 14%; z-index: 101; position: relative;">
                <div class="manageText">Manage Your:</div>
                <div class="col-md-12 col-sm-12" style="display: inline-flex;">
                    <%--<a onclick="<%=createProjectlink%>" class="button">
                    <img src="/Content/Images/plus-white.png" height="16" />
                    Create Project</a>--%>
                     <%--<a style="" href="javascript:window.parent.UgitOpenPopupDialog('<%=url%>','','<%=formTitle %>','680px','500px',0,'<%=Server.UrlEncode(Request.Url.AbsolutePath) %>')" class="button-1">
                        Allocate<span class="glyphicon glyphicon-triangle-right leftArrowIcon"></span></a>--%>
                    <a style="" href="/Pages/ManagerView" class="button-1">
                        Team<span class="glyphicon glyphicon-triangle-right leftArrowIcon"></span></a>
                    <a style="" href="/Pages/CRMProject" class="button-1">
                        Projects<span class="glyphicon glyphicon-triangle-right leftArrowIcon"></span></a>
                    <a href="/Pages/Report" class="button-1">
                        Reports<span class="glyphicon glyphicon-triangle-right leftArrowIcon"></span></a>
                </div>
            </div>
        </div>
        <div class="col-md-6">
            <img src="/Assets/entry-page-2.png" style="width: 100%;" />
        </div>
    </div>

    <script data-v="<%=UGITUtility.AssemblyVersion %>">
        $("#ctl00_ctl00_MainContent_homeCardPannel").hide();
        function openTicketDialog(path, params, titleVal, width, height, stopRefresh, returnUrl) {
            window.parent.UgitOpenPopupDialog(path, params, titleVal, width, height, stopRefresh, returnUrl);
        }
    </script>
    <div class="sticky-image-wrapper">
        <img src="/Assets/entry-page-1.png" />
    </div>
</div>
<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .sticky-image-wrapper {
        position: absolute;
        bottom: 0px;
        left: 0px;
        width: 100%;
        z-index: 100;
    }

    .sticky-image-wrapper img {
        display: grid;
        position: relative;
        width: 10%;
    }

    .button-1 {
        background-color: #6BA439;
        border: none;
        font-family: 'Roboto', sans-serif !important;
        color: white;
        padding: 6px 0px;
        text-align: center;
        text-decoration: none;
        font-size: 20px;
        margin-right: 4%;
        cursor: pointer;
        border-radius: 20px;
        font-weight: 700;
        text-transform: none;
        padding-left:30px;
        width:100%;
    }

    a:visited {
        color: white;
        text-decoration: none;
    }

    .button-1:hover {
        background: #A9C23F;
        color: white;
        text-decoration: none;
    }

    #main {
        position: absolute;
        /*left: 55px;
            top: 60px;*/
        width: 100%;
        height: 100vh;
        background-image: linear-gradient(#182D36 70%, #010102);
        background-size: cover;
        object-fit: cover;
        font-family: sans-serif;
        padding: 30px;
    }

    #main p {
        color: white;
        font-family: 'Roboto', sans-serif !important;
        font-size: 62px;
        font-weight: 600;
    }

    .manageText {
        font-family: 'Roboto', sans-serif !important;
        font-size: 22px;
        margin-bottom: 2%;
        margin-left: 40%;
        color: white;
    }

    .leftArrowIcon {
        padding-top: 7px;
        padding-bottom: 7px;
        float: right;
        padding-right: 20px;
        font-size:14px;
    }
</style>

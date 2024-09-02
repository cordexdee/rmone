<%@ Page Title="" Language="C#" MasterPageFile="~/master/main.Master" AutoEventWireup="true" CodeBehind="UserEntryPage.aspx.cs" Inherits="uGovernIT.Web.SitePages.UserEntryPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderContainer" runat="server">
    <div id="main">
        <div class="row" style="margin-top: 8%;">
            <div class="col-md-5">
                <p style="font-size: 26px;">Live View of</p>
                <p>
                    Right People
                <br />
                    on the
                   <br />
                    Right Project
                </p>
            </div>
            <div class="col-md-7">
                <img width="750" src="/Assets/entry-page-2.png" style="float:right;" />
            </div>
        </div>
        <div class="row" style="margin-top: 3%;z-index:101;position:relative;">
            <div class="col-md-8 col-sm-12" style="display: inline-flex;">
                <a onclick="<%=createProjectlink%>" class="button">
                    <img src="/Content/Images/plus-white.png" height="16" />
                    Create Project</a>
                <a style="" href="javascript:window.parent.UgitOpenPopupDialog('<%=url%>','','<%=formTitle %>','680px','500px',0,'<%=Server.UrlEncode(Request.Url.AbsolutePath) %>')" class="button">
                    <img src="/Content/Images/plus-white.png" height="16" />
                    Allocate Staff</a>
                <a style="" href="/Pages/CRMProject" class="button">
                    <img src="/Content/Images/plus-white.png" height="16" />
                    Edit Project</a>
            </div>
        </div>
        <script>
            $(".dxeImage_UGITNavyBlueDevEx").attr("src", "/Content/Images/RMONE/rm-one-logo.png");
            $("#ctl00_ctl00_MainContent_homeCardPannel").hide();
            function openTicketDialog(path, params, titleVal, width, height, stopRefresh, returnUrl) {
                window.parent.UgitOpenPopupDialog(path, params, titleVal, width, height, stopRefresh, returnUrl);
            }
        </script>
        <div class="sticky-image-wrapper">
            <img src="/Assets/entry-page-1.png" />
        </div>
    </div>
    <style>
        .sticky-image-wrapper {
            position: absolute;
            bottom: 0px;
            left: 0px;
            width: 100%;
            z-index:100;
        }

        .sticky-image-wrapper img {
            display: grid;
            position: relative;
            width: 10%;
        }

        .button {
            background-color: #6BA439;
            border: none;
            font-family: 'Roboto', sans-serif !important;
            color: white;
            padding: 10px 25px;
            text-align: center;
            text-decoration: none;
            font-size: 20px;
            margin-right: 4%;
            cursor: pointer;
            border-radius: 20px;
            font-weight: 700;
            text-transform: none;
        }

        a:visited {
            color: white;
            text-decoration: none;
        }

        .button:hover {
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
                font-weight:600;
            }
    </style>
</asp:Content>

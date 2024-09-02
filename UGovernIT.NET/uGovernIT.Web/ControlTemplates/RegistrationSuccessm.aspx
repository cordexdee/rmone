<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RegistrationSuccessm.aspx.cs" Inherits="uGovernIT.Web.ControlTemplates.RegistrationSuccessm" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <%--<link rel="stylesheet" type="text/css" href="~\Content\newAdmin.css"  />--%>
    <link rel="stylesheet" type="text/css" href="<%= ResolveUrl(@"~\Content\newAdmin.css") + "?v=" + UGITUtility.AssemblyVersion %>"  />
    <!-- Latest compiled and minified CSS -->
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/css/bootstrap.min.css" />

    <!-- jQuery library -->
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.4.1/jquery.min.js"></script>

    <!-- Latest compiled JavaScript -->
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/js/bootstrap.min.js"></script>
</head>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    $(document).ready(function () {
        $("#BodyMsg").find("a").css({ "text-decoration": "underline", "color": "#CFF991" });
    });

</script>
<body>
    <header class="">
        <div class="regCustom-navBar">
            <a class="homeBack-link" href="https://www.rmone.ai/">
                <img class="homeBack-logo" src="../content/images/RMONE/rm-one-logo.png" />
            </a>
        </div>
    </header>
    <section class="mainSection">
        <%--<asp:Button  ID="Home" Text ="Home"/>--%>
        <div class="mainSection-container">
            <div class="mainSection-wrap container">
                <div class="mainSection-textBlock">

                    <%--<a class="homeBack-link" href="http://www.serviceprime.com/">
                         <img class="homeBack-logo" src="../Content/Images/Service_Prime_Logo.svg"/>
                     </a>--%>
                    <h2 runat="server" id="HeaderMsg">Thank You!</h2>
                    <p runat="server" id="BodyMsg"></p>
                </div>
                <div class="mainSection-ImageBlock">
                    <img src="/Content/images/NewAdmin/regSuccess.svg" class="section-bannerImg" />
                </div>
            </div>
        </div>
    </section>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DocumentControl.aspx.cs" Inherits="uGovernIT.Web.DocumentControl" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script async src="https://www.googletagmanager.com/gtag/js?id=UA-122813068-1"></script>
    <script>
      window.dataLayer = window.dataLayer || [];
      function gtag(){dataLayer.push(arguments);}
      gtag('js', new Date());
      gtag('config', 'UA-122813068-1');
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Image  ID="uploadedImages" runat="server" />
<asp:Label ID="lblMsg" runat="server"></asp:Label>
        </div>
    </form>
</body>
</html>

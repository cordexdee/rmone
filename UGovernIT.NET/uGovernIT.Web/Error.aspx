<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="uGovernIT.Web.ErrorMsgPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Error</title>
    <style>
        .ErrorMsg-container{
            margin:auto;
            width:30%;
            text-align:center;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="col-md-12 col-sm-12 col-xs-12">
            <div class="ErrorMsg-container">
                <h2> Sorry, Something went wrong</h2>
                <p>An Error has occured in this page, please contact your Administrator</p>
                <%--<button>Back</button>--%>
                <asp:Label runat="server" ID="lblTimeStamp"></asp:Label>
                <asp:Label runat="server" ID="lblerrorcode"></asp:Label>
            </div>
        </div>
    </form>
</body>
</html>

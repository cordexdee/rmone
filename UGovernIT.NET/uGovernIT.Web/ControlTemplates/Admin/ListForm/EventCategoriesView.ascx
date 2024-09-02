<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EventCategoriesView.ascx.cs" Inherits="uGovernIT.Web.EventCategoriesView" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<style data-v="<%=UGITUtility.AssemblyVersion %>">
    #header {
        text-align: center;
        height: 30px;
        float: left;
        padding: 0px 2px;
        width: 100%;
    }

    #content {
        width: 100%;
    }

    .gridheader {
        height: 20px;
        background-color: #CED8D9;
        text-align: left;
        font-weight:normal;
    }
    a:hover {
        text-decoration:underline;
    }
     a, img {
        border:0px;
    } 
     .formatcolor{
           background-color:#f85752;
            color:white;
     }
 .formatcolor a{   
  
    color:white;
      
}

</style>

<div class="col-md-12 col-sm-12 col-xs-12">
    <div class="row">
        <div style="float:right;padding-top:15px;">
             <a id="aAddItem_Top" runat="server" href="" class="primary-btn-link">
                <img id="Img3" runat="server" src="/Content/Images/plus-symbol.png" />
                <asp:Label ID="Label2" runat="server" Text="Add New Item"></asp:Label>
            </a>
        </div>
    </div>
    <div class="row">
         <ugit:ASPxGridView runat="server" ID="dxgridview" KeyFieldName="ID" AutoGenerateColumns="false" Width="100%" 
             OnHtmlDataCellPrepared="dxgridview_HtmlDataCellPrepared" CssClass="customgridview homeGrid">
            <Columns>
                <dx:GridViewDataTextColumn Name="aEdit" Width="10px">
                     <DataItemTemplate>
                     <a id="editLink" runat="server" href="">
                         <img id="Imgedit" runat="server" width="16" src="~/Content/Images/editNewIcon.png"/>
                     </a>
                     </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="Title" Caption="Title" SortOrder="Ascending">
                     <DataItemTemplate>
                     <a id="editLink" runat="server" href="" ></a>
                         </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="ItemColor" Caption="Color" SortOrder="Ascending" Width="100px"></dx:GridViewDataTextColumn>
            </Columns>
            <Settings ShowHeaderFilterButton="true" />
             <Styles>
                 <Header CssClass="homeGrid_headerColumn"></Header>
                 <Row CssClass="homeGrid_dataRow"></Row>
             </Styles>
        </ugit:ASPxGridView>
    </div>
    <div class="row">
        <div  style="float:right;padding-top:15px;">
            <a id="aAddItem" runat="server" href="" class="primary-btn-link">
                <img id="Img4" runat="server" src="/Content/Images/plus-symbol.png" />
                <asp:Label ID="Label3" runat="server" Text="Add New Item"></asp:Label>
            </a>
        </div>
    </div>
</div>

<div>
   
</div>

  


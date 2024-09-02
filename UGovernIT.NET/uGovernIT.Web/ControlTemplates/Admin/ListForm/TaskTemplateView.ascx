<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TaskTemplateView.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.Admin.ListForm.TaskTemplateView" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>


<div class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap">
    <div class="row">
        <ugit:ASPxGridView runat="server" ID="dxgridview" KeyFieldName="ID" CssClass="customgridview homeGrid" AutoGenerateColumns="false" Width="100%">
            <Columns>
                <dx:GridViewDataTextColumn FieldName="ItemOrder" Caption="#" SortOrder="Ascending" CellStyle-HorizontalAlign="Center" Width="1px"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="Title" Caption="Title" Width="300px"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="StartDate" Caption="Start Date" PropertiesTextEdit-DisplayFormatString="MM/dd/yyyy" CellStyle-HorizontalAlign="Center"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="DueDate" Caption="End Date" PropertiesTextEdit-DisplayFormatString="MM/dd/yyyy" CellStyle-HorizontalAlign="Center">
                </dx:GridViewDataTextColumn>
                
            </Columns>
            <Styles>
                <Row CssClass="homeGrid_dataRow"></Row>
                <Header CssClass="homeGrid_headerColumn"></Header>
            </Styles>
            <Settings ShowHeaderFilterButton="true" />
            <SettingsBehavior AllowSort="true" />

        </ugit:ASPxGridView>
    </div>
</div>

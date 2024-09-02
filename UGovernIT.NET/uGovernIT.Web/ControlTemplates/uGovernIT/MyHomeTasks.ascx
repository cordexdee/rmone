<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MyHomeTasks.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.uGovernIT.MyHomeTasks" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<ugit:ASPxGridView ID="grid" runat="server" AutoGenerateColumns="false" SettingsPager-PageSize="10"    
    UseFixedTableLayout="false"
    ShowHorizontalScrollBar="true"
    Width="100%" KeyFieldName="ID" CssClass="customgridview homeGrid" EnableRowsCache="true">
    <settingsadaptivity adaptivitymode="HideDataCells" allowonlyoneadaptivedetailexpanded="true" ></settingsadaptivity>
    <columns>
        <dx:GridViewDataTextColumn  FieldName="ModuleName" Caption="Module" GroupIndex="0" VisibleIndex="0">    
            <DataItemTemplate>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="ItemOrder" Caption="Item Order"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="Title" Caption="Title"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="PercentComplete" Caption="% Complete"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="Status" Caption="Status"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="AssignedTo" Caption="Assigned To"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="EstimatedHours" Caption="Est. Hrs"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="StartDate" Caption="Start Date"><PropertiesTextEdit DisplayFormatString="MMM-dd-yyyy" /></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="DueDate" Caption="Due Date"><PropertiesTextEdit DisplayFormatString="MMM-dd-yyyy" /></dx:GridViewDataTextColumn>
    </columns>
    <settingscommandbutton>
            <ShowAdaptiveDetailButton ButtonType="Button" Styles-Style-CssClass="homeGrid_openBTn"></ShowAdaptiveDetailButton>
                <HideAdaptiveDetailButton ButtonType="Button" Styles-Style-CssClass="homeGrid_closeBTn"></HideAdaptiveDetailButton>
    </settingscommandbutton>
    <settingspopup>
        <HeaderFilter Height="200" />
    </settingspopup>
    <settingspager>
        <PageSizeItemSettings Position="Right" Visible="true" Items="5,10,15,20,25,50,100"></PageSizeItemSettings>                                                            
    </settingspager>
    <styles>
        <Row CssClass="customrowheight homeGrid_dataRow"></Row>                                                        
    </styles>
    <settingsbehavior allowsort="true" enablerowhottrack="false" />
</ugit:ASPxGridView>
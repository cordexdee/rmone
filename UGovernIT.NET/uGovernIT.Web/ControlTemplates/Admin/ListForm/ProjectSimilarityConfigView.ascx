<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProjectSimilarityConfigView.ascx.cs" Inherits="uGovernIT.Web.ProjectSimilarityConfigView" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>


<div class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap py-2">
    <div class="row">
        <div class="headerContent-right">
            <dx:ASPxButton ID="aAddItem_Top" runat="server" CssClass="primary-blueBtn" Text="Add New Item" AutoPostBack="false"
                Image-Width="12" Image-Url="~/Content/Images/plus-symbol.png">
            </dx:ASPxButton>
        </div>
    </div>
    <div class="row bs d-flex align-items-end" style="margin-bottom:10px;">
        <div class="formLayout-dropDownWrap col-md-4 col-sm-4 col-xs-12">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Select Module:</h3>
            </div>
            <div class="ms-formbody">
                <asp:DropDownList ID="ddlModule" runat="server" AutoPostBack="true" CssClass="itsmDropDownList aspxDropDownList" 
                    OnSelectedIndexChanged="ddlModule_SelectedIndexChanged" ></asp:DropDownList>
            </div>
        </div>
                <div class="formLayout-dropDownWrap col-md-4 col-sm-4 col-xs-12">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Select Metric Type:</h3>
            </div>
            <div class="ms-formbody">
                <asp:DropDownList ID="ddlType" runat="server" AutoPostBack="true" CssClass="itsmDropDownList aspxDropDownList"
                    OnSelectedIndexChanged="ddlType_SelectedIndexChanged">
                </asp:DropDownList>
            </div>
        </div>
        <div class="col-md-8 col-sm-8 col-xs-12 d-flex align-items-center justify-content-end">
            <div class="headerItem-showChkBox">
                <div class="crm-checkWrap">
                    <asp:CheckBox ID="dxshowdeleted" Text="Show Deleted" runat="server" AutoPostBack="true" OnCheckedChanged="dxShowDeleted_CheckedChanged" />
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <dx:ASPxGridView ID="dxgridProjectSimilarity" ClientInstanceName="dxgridProjectSimilarity" runat="server" OnHtmlDataCellPrepared="dxgridProjectSimilarity_HtmlDataCellPrepared" KeyFieldName="ID"
            Width="100%" AutoGenerateColumns="false" CssClass="customgridview homeGrid mt-3" Settings-VerticalScrollBarMode="Visible" Settings-VerticalScrollableHeight="320">
            <Columns>
                <dx:GridViewDataTextColumn Name="aEdit" Width="10px">
                    <DataItemTemplate>
                        <a id="editLink" runat="server" href="">
                            <img id="Imgedit" runat="server" width="16" src="~/Content/Images/editNewIcon.png" />
                        </a>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="Title" Caption="Title" SortOrder="Ascending" Width="100px">
                    <Settings HeaderFilterMode="CheckedList" />
                    <DataItemTemplate>
                        <a id="editLink" runat="server" href=""></a>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="ColumnName" Caption="Column Name" CellStyle-HorizontalAlign="left" Width="100px">
                    <Settings HeaderFilterMode="CheckedList" />
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="ColumnType" Caption="Column Type" CellStyle-HorizontalAlign="left" Width="50px">
                    <Settings HeaderFilterMode="CheckedList" />
                </dx:GridViewDataTextColumn>
                <%--<dx:GridViewDataTextColumn FieldName="StageWeight" Caption="Stage Weight" CellStyle-HorizontalAlign="Center" Width="50px">--%>
                <dx:GridViewDataTextColumn FieldName="StageWeight" Caption="score" CellStyle-HorizontalAlign="Center" Width="50px">
                    <Settings HeaderFilterMode="CheckedList" />
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="Weight" Caption="Weight" CellStyle-HorizontalAlign="Center" Width="50px">
                    <Settings HeaderFilterMode="CheckedList" />
                </dx:GridViewDataTextColumn>
            </Columns>
           
            <Styles>
                <Row CssClass="homeGrid_dataRow"></Row>
                <Header CssClass="homeGrid_headerColumn" Font-Bold="true"></Header>
            </Styles>
            <Settings ShowHeaderFilterButton="true" />
            <SettingsPager Mode="ShowAllRecords"></SettingsPager>
            <FormatConditions>
                <dx:GridViewFormatConditionHighlight FieldName="ColumnName" Expression="[Deleted]=True" ApplyToRow="true" Format="Custom" RowStyle-CssClass="formatcolor"></dx:GridViewFormatConditionHighlight>
            </FormatConditions>
        </dx:ASPxGridView>
        
    </div>
<%--    <div class="row bottom-addBtn">
        <div class="headerItem-addItemBtn">
            <dx:ASPxButton ID="aAddItem" runat="server" CssClass="primary-blueBtn" Text="Add New Item" AutoPostBack="false"
                Image-Width="12" Image-Url="~/Content/Images/plus-symbol.png">
            </dx:ASPxButton>
        </div>
    </div>--%>

    <div class="row" style="display:none">
        <div>&nbsp;</div>
        <div>
            Search columns
        </div>
<%--        <dx:ASPxTokenBox ID="tkFieldName" runat="server" Width="100%" CssClass="aspxUserTokenBox-control" IncrementalFilteringMode="Contains" FilterMinLength="2"></dx:ASPxTokenBox>
        <br />--%>
        <dx:ASPxGridLookup  Visible="true"  
             SelectionMode="Multiple" ID="glFieldName" runat="server" Width="100%" IncrementalFilteringMode="Contains" TextFormatString="{0}"  CssClass="stagesctionusers aspxGridLookUp-dropDown"
            KeyFieldName="COLUMN_NAME" multitextseparator=", " DropDownWindowStyle-CssClass="aspxGridLookup-dropDown">
            <Columns>
                <dx:GridViewCommandColumn ShowSelectCheckbox="True" Width="28px" />
                <dx:GridViewDataTextColumn FieldName="COLUMN_NAME" Width="300px" Caption="Fields:">
                    <settings AutoFilterCondition="Contains" ></settings>                    
                </dx:GridViewDataTextColumn>
            </Columns>

            <GridViewProperties>
                <Settings ShowGroupedColumns="false" ShowFilterRow="true" VerticalScrollBarMode="Auto" />
                <SettingsBehavior AllowSort="false" AllowGroup="true" AutoExpandAllGroups="true" />
                <SettingsPager Mode="ShowAllRecords"></SettingsPager>
            </GridViewProperties>
            
        </dx:ASPxGridLookup>
        
        <dx:ASPxButton ID="btnSave" runat="server" Text="Save" ToolTip="Save" CssClass="primary-blueBtn fright mt-2" ValidationGroup="Save" OnClick="btnSave_Click"></dx:ASPxButton>
    </div>
</div>


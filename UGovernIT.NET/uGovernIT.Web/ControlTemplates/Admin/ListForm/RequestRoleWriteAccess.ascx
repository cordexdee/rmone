<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RequestRoleWriteAccess.ascx.cs" Inherits="uGovernIT.Web.RequestRoleWriteAccess" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<style data-v="<%=UGITUtility.AssemblyVersion %>">   
    #header {
        text-align: left;
        float: left;
        padding: 0px 2px;
        width:100%;
    }

    #content {
        width: 100%;
    }

    .gridheader {
        height: 20px;
        background-color: #CED8D9;
        text-align: left;
        font-weight: normal;
    }
    th {
        font-weight: normal;
    }
    a:hover {
        text-decoration:underline;
    }
     a, img {
        border:0px;
    }
    .fleft
    {
        float:left;
    }
    .fright
    {
        float: right;
    }
</style>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    
    function OnEndCallback(s, e) {
        ResetGridTr();
    }

    $(function () {
       
        ResetGridTr();
        if ($("[id$='chkEditable']")[0].checked) {
            ChangeEditableCheckState(false);
        }
        else {
            ConfigureActionUserControl();
        }
    });

    function ResetGridTr() {
        $(".fieldactionusers tr").each(function () {

            if ($(this).css('display') == 'none') {
                $(this).css('display', '');
            }
        });
    }

    function ConfigureActionUserControl()
    {
        for (var i = 0; i < $("[id$='chkEditable']").length; i++)
        {
            if ($("[id$='chkEditable']")[i].checked && $($("[id$='chkEditable']")[i]).attr("disabled") != 'disabled') {
                $($("[id$='cbFieldMandatory']")[i]).removeAttr("disabled");
                $($("[id$='cbShowEditButton']")[i]).removeAttr("disabled");
                $($("[id$='cbShowWithCheckBox']")[i]).removeAttr("disabled");
                $($("[id$='glActionUser']")[i]).find('input').removeAttr("disabled");
                $($("[id$='glActionUser']")[i]).find('.fieldactionusers-bottom').css('display', '')
            }
            else {
                $($("[id$='cbFieldMandatory']")[i]).attr('disabled', 'disabled');
                $($("[id$='cbShowEditButton']")[i]).attr('disabled', 'disabled');
                $($("[id$='cbShowWithCheckBox']")[i]).attr('disabled', 'disabled');
                $($("[id$='glActionUser']")[i]).find('input').attr('disabled', 'disabled');
                $($("[id$='glActionUser']")[i]).find('.fieldactionusers-bottom').css('display', 'none');
            }
        }
    }


    function ChangeEditableCheckState(val) {
        for (var i = 0; i < $("[id$='chkEditable']").length; i++)
        { 
            if (i > 0)
            {               
                if (val)
                {                  
                    $($("[id$='chkEditable']")[i]).removeAttr("disabled");                   
                }
                else
                {                  
                    $($("[id$='chkEditable']")[i]).attr('disabled', 'disabled');
                    $("[id$='chkEditable']")[i].checked = false;
                }
            }
        }
        ConfigureActionUserControl();
    }

    function OnEditableCheckedChanged(obj,vIndex)
    {
        if (!$(obj).find('input')[0].checked) {
            $(obj).parent().next().next().next().next().find('.fieldactionusers-bottom').css('display', 'none');
            $(obj).parent().next().next().next().next().find('input').attr('disabled', 'disabled');
            $(obj).parent().next().next().next().find('input').attr('disabled', 'disabled');
            $(obj).parent().next().next().find('input').attr('disabled', 'disabled');
            $(obj).parent().next().find('input').attr('disabled', 'disabled');           
        }
        else
        {
            $(obj).parent().next().next().next().next().find('.fieldactionusers-bottom').css('display', '')
            $(obj).parent().next().next().next().next().find('input').removeAttr("disabled");
            $(obj).parent().next().next().next().find('input').removeAttr("disabled");
            $(obj).parent().next().next().find('input').removeAttr("disabled");
            $(obj).parent().next().find('input').removeAttr("disabled");
        }
        if (vIndex == 0)
        {
            ChangeEditableCheckState(!$(obj).find('input')[0].checked);
        }
    }

</script>
<script data-v="<%=UGITUtility.AssemblyVersion %>">
    function UpdateGridHeight() {
        grid.SetHeight(0);
        var containerHeight = ASPxClientUtils.GetDocumentClientHeight();
        if (document.body.scrollHeight > containerHeight)
            containerHeight = document.body.scrollHeight;
        grid.SetHeight(containerHeight);
    }
    window.addEventListener('resize', function (evt) {
        if (!ASPxClientUtils.androidPlatform)
            return;
        var activeElement = document.activeElement;
        if (activeElement && (activeElement.tagName === "INPUT" || activeElement.tagName === "TEXTAREA") && activeElement.scrollIntoViewIfNeeded)
            window.setTimeout(function () { activeElement.scrollIntoViewIfNeeded(); }, 0);
    });
</script>
<div class="col-md-12 col-sm-12 col-xs-12 formLayout-addPopupContainer">
    <div class="row" id="header">
        <div class="ms-formtable accomp-popup" style="display:none">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Select Module:</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:DropDownList ID="ddlModule" runat="server" AppendDataBoundItems="true" AutoPostBack="true" CssClass="itsmDropDownList aspxDropDownList" Visible="false"
                    OnSelectedIndexChanged="ddlModule_SelectedIndexChanged">
                </asp:DropDownList>
            </div>       
        </div>        
    </div>

    <div class="row" id="content">
         <div class="col-md-12 col-sm-12 col-xs-12 noPadding">
             <ugit:ASPxGridView ID="grid"  runat="server" AutoGenerateColumns="False" OnHtmlDataCellPrepared="grid_HtmlDataCellPrepared"
                OnHtmlRowPrepared="grid_HtmlRowPrepared" CssClass="customgridview homeGrid" ClientInstanceName="grid"  EnableCallBacks="true" EnableViewState="false"
                Width="100%"  KeyFieldName="ID">
                 <settingsadaptivity adaptivitymode="HideDataCells" allowonlyoneadaptivedetailexpanded="true" ></settingsadaptivity>
                <Columns >                                   
                    <dx:GridViewDataTextColumn Caption="Step" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" FieldName="StageStep" />
                    <dx:GridViewDataTextColumn Caption="Step Name" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" FieldName="Title" />
                    <dx:GridViewDataTextColumn Caption="Editable" FieldName="ID"  CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" >
                        <DataItemTemplate>                                           
                            <asp:CheckBox ID ="chkEditable" Visible="true"  runat="server" />
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn> 
                    <dx:GridViewDataTextColumn Caption="Mandatory" FieldName="FieldMandatory"  CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" >
                        <DataItemTemplate>                                            
                                <asp:CheckBox ID ="cbFieldMandatory" Visible="true"   runat="server" />
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn> 
                    <dx:GridViewDataTextColumn Caption="Show Edit Button" FieldName="ShowEditButton"  CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                        <DataItemTemplate>
                            <asp:CheckBox ID ="cbShowEditButton" Visible="true"  runat="server" />                                        
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Show With CheckBox" FieldName="ShowWithCheckbox"  CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" >
                        <DataItemTemplate>
                            <asp:CheckBox ID ="cbShowWithCheckBox" Visible="true"  runat="server" />
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Hide In Service" FieldName="HideInServiceMapping"  CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" >
                        <DataItemTemplate>
                            <asp:CheckBox ID ="cbHideInService" Visible="true"   runat="server" />
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <%--<dx:GridViewCommandColumn ShowSelectCheckbox="true"></dx:GridViewCommandColumn>--%>
                    <dx:GridViewDataTextColumn Caption="Additional Action User(s)" FieldName="ActionUser"  CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" >
                        <DataItemTemplate>
                            <dx:ASPxGridLookup  Visible="true" CssClass="fieldactionusers" TextFormatString="{1}" SelectionMode="Multiple" ID="glActionUser" runat="server" KeyFieldName="Name" OnLoad="glActionUser_Load">
                                <Columns>
                                    <dx:GridViewDataTextColumn FieldName="Type" GroupIndex="0" SortOrder="Descending" Visible="false" ></dx:GridViewDataTextColumn>
                                    <dx:GridViewCommandColumn ShowSelectCheckbox="True"  />                      
                                    <dx:GridViewDataTextColumn FieldName="Name" Width="170px" Caption="Choose ActionUsers:">
                                    </dx:GridViewDataTextColumn>
                                </Columns>
                                    <ButtonStyle CssClass="fieldactionusers-bottom"></ButtonStyle>
                                <GridViewProperties>
                                    <Settings ShowGroupedColumns="false"  ShowFilterRow="true" VerticalScrollBarMode="Auto"/>
                                    <SettingsBehavior AllowSort="false" AllowGroup="true"   AutoExpandAllGroups="true" />
                                    <SettingsPager Mode="ShowAllRecords" ></SettingsPager>                        
                                </GridViewProperties>
                                <ClientSideEvents EndCallback="OnEndCallback" />
                            </dx:ASPxGridLookup>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>                                        
                </Columns>
                <settingscommandbutton>
                    <ShowAdaptiveDetailButton ButtonType="Button"   Styles-Style-CssClass="homeGrid_openBTn"></ShowAdaptiveDetailButton>
                    <HideAdaptiveDetailButton ButtonType="Button"  Styles-Style-CssClass="homeGrid_closeBTn"></HideAdaptiveDetailButton>
                </settingscommandbutton>
                <SettingsBehavior   AutoExpandAllGroups="true"  AllowSort="false"  AllowSelectByRowClick="true" AllowSelectSingleRowOnly="true" />
                <Settings GridLines="Horizontal" />
                 <Styles>
                     <Row CssClass="homeGrid_dataRow"></Row>
                     <Header CssClass="homeGrid_headerColumn"></Header>
                 </Styles>
                <SettingsCookies Enabled="false" />                         
                <SettingsPopup>
                        <HeaderFilter Height="200"/>
                </SettingsPopup>
                <SettingsPager Mode="ShowAllRecords" Position="TopAndBottom"></SettingsPager>
            </ugit:ASPxGridView>
            <script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
                ASPxClientControl.GetControlCollection().ControlsInitialized.AddHandler(function (s, e) {
                    UpdateGridHeight();
                });
                ASPxClientControl.GetControlCollection().BrowserWindowResized.AddHandler(function (s, e) {
                    UpdateGridHeight();
                });
            </script>
         </div>
        <div class="col-md-12 col-sm-12 col-xs-12  noPadding">
            <div class="row addEditPopup-btnWrap">
                <dx:ASPxButton ID="btnCancel" CssClass="secondary-cancelBtn" runat="server" Text="Cancel" ToolTip="Cancel"  OnClick="btnCancel_Click"></dx:ASPxButton>
                <dx:ASPxButton ID="btnSave" CssClass="primary-blueBtn" runat="server" Text="Save" ToolTip="Save" ValidationGroup="Save" OnClick="btnSave_Click"></dx:ASPxButton>
            </div>
        </div>
    </div>
</div>

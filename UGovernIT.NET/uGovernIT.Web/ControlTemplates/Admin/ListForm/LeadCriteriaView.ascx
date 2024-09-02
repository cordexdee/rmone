<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LeadCriteriaView.ascx.cs" Inherits="uGovernIT.Web.LeadCriteriaView" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .formatcolor {
        background-color: #f85752;
        color: white;
    }
</style>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function OpenAddCriteria() {
        UgitOpenPopupDialog('<%= absoluteUrl %>' + '&CriteriaId=0', "", 'Add Lead Criteria', '600px', '400px', 0, escape("<%= Request.Url.AbsolutePath %>"));
        return false;
    }

    function OpenEditCheckListPopup(objCheckListId) {
        window.parent.UgitOpenPopupDialog('<%= absoluteUrl %>' + '&CheckListId=' + objCheckListId, "", 'Edit CheckList', '500px', '350px', 0, escape("<%= Request.Url.AbsolutePath %>"));
    }

    function EditLeadCriteria(obj, Id) {
        UgitOpenPopupDialog('<%= absoluteUrl %>' + '&CriteriaId=' + Id, "", 'Edit Lead Criteria', '600px', '400px', 0, escape("<%= Request.Url.AbsolutePath %>"));
        return false;
    }
</script>

<div class="col-md-12 col-sm-12 col-xs-12" style="margin-top:10px;">
    <div class="row">
        <div class="crm-checkWrap" style="float: right;margin-bottom:10px;">
            <asp:CheckBox ID="chkShowDeleted" Text="Show Deleted" runat="server" TextAlign="Left"
                AutoPostBack="true" OnCheckedChanged="chkShowDeleted_CheckedChanged" />    
        </div>
    </div>
    <div class="row">
        <div id="content">
            <ugit:ASPxGridView ID="grdLeadCriteria" ClientInstanceName="grdLeadCriteria" runat="server" Width="100%"
                KeyFieldName="ID" EnableViewState="false" AlternatingRowStyle-BackColor="WhiteSmoke"
                 AutoGenerateColumns="false" AllowFiltering="true" CssClass="customgridview homeGrid"
                DataKeyNames="ID" GridLines="None" SettingsPager-PageSize="15">
                <Columns>
                    <dx:GridViewDataTextColumn Name="aEdit" Width="10px">
                        <DataItemTemplate>
                            <asp:ImageButton OnClientClick='<%# string.Format("javascript:return EditLeadCriteria(this, {0})", Eval("ID")) %>'
                                ToolTip="Edit" ID="imgButtonEdit" runat="server" ImageUrl="/Content/images/editNewIcon.png" 
                                Style="padding-bottom: 8px; width:16px;" CssClass="crmActivity_editBtn" />
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Priority" FieldName="Priority" />            
                    <dx:GridViewDataTextColumn Caption="Minimum" FieldName="MinValue" />
                    <dx:GridViewDataTextColumn Caption="Maximum" FieldName="MaxValue" />            
                </Columns>
                <FormatConditions>
                    <dx:GridViewFormatConditionHighlight FieldName="Criteria" Format="Custom" ApplyToRow="true" RowStyle-CssClass="formatcolor" Expression="[Deleted] = True"></dx:GridViewFormatConditionHighlight>
                </FormatConditions>
                <Styles>
                    <Row CssClass="homeGrid_dataRow"></Row>
                    <Header CssClass="homeGrid_headerColumn"></Header>
                </Styles>
            </ugit:ASPxGridView>
        </div>
    </div>
    <div class="row addEditPopup-btnWrap">
        <dx:ASPxButton ID="lnkbtnAddRanking" runat="server" ToolTip="Add Lead Criteria" Text="Add Lead Criteria" CssClass=" primary-blueBtn">
            <ClientSideEvents Click="function(s, e){return OpenAddCriteria()}" />
        </dx:ASPxButton>
    </div>
</div>






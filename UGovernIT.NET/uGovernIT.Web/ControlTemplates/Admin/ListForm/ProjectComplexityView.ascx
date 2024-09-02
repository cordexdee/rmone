<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProjectComplexityView.ascx.cs" Inherits="uGovernIT.Web.ProjectComplexityView" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .formatcolor {
        background-color: #f85752;
        color: white;
    }

    .dxgvHeader_UGITNavyBlueDevEx td {
        white-space: nowrap;
        font-size: 12px;
        color: #4b4b4b !important;
        font-weight: bold;
    }

    .button-bg {
        background: #4FA1D6;
        color: #FFF;
        border: 1px solid #4FA1D6 !important;
        border-radius: 4px;
        padding: 5px 13px 5px 13px !important;
        font-size: 12px;
        font-family: 'Roboto', sans-serif;
        font-weight: 500;
        float: right;
    }
</style>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function OpenAddCriteria() {
        UgitOpenPopupDialog('<%= absoluteUrl %>' + '&CriteriaId=0', "", 'Add Project Complexity', '600px', '400px', 0, escape("<%= Request.Url.AbsolutePath %>"));
        return false;
    }

    function OpenEditCheckListPopup(objCheckListId) {
        window.parent.UgitOpenPopupDialog('<%= absoluteUrl %>' + '&CheckListId=' + objCheckListId, "", 'Edit CheckList', '500px', '350px', 0, escape("<%= Request.Url.AbsolutePath %>"));
    }

    function EditProjectComplexity(obj, Id) {
        UgitOpenPopupDialog('<%= absoluteUrl %>' + '&CriteriaId=' + Id, "", 'Edit Project Complexity', '600px', '400px', 0, escape("<%= Request.Url.AbsolutePath %>"));
        return false;
    }
</script>

<div style="padding: 10px;">
    <div class="crm-checkWrap" style="float: right;margin-bottom:10px;">
        <asp:CheckBox ID="chkShowDeleted" Text="Show Deleted&nbsp;&nbsp;" runat="server" TextAlign="Left" AutoPostBack="true" OnCheckedChanged="chkShowDeleted_CheckedChanged" />
    </div>

    <div id="content">
        <ugit:ASPxGridView ID="grdProjectComplexity" ClientInstanceName="grdProjectComplexity" runat="server" Width="100%" KeyFieldName="ID" EnableViewState="false" AlternatingRowStyle-BackColor="WhiteSmoke"
            HeaderStyle-Height="20px" HeaderStyle-CssClass="gridheader" HeaderStyle-Font-Bold="false" AutoGenerateColumns="false" AllowFiltering="true"
            DataKeyNames="ID" GridLines="None" SettingsPager-PageSize="15">

            <Columns>
                <dx:GridViewDataTextColumn Name="aEdit" Width="10px">
                    <DataItemTemplate>
                        <asp:ImageButton OnClientClick='<%# string.Format("javascript:return EditProjectComplexity(this, {0})", Eval("ID")) %>'
                            ToolTip="Edit" ID="imgButtonEdit" runat="server" ImageUrl="/Content/images/editNewIcon.png" Style="padding-bottom: 8px;" CssClass="crmActivity_editBtn" />
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="Project Complexity" FieldName="CRMProjectComplexity" />
                <dx:GridViewDataTextColumn Caption="Minimum" FieldName="MinValue" />
                <dx:GridViewDataTextColumn Caption="Maximum" FieldName="MaxValue" />
            </Columns>
            <FormatConditions>
                <dx:GridViewFormatConditionHighlight FieldName="Criteria" Format="Custom" ApplyToRow="true" RowStyle-CssClass="formatcolor" Expression="[Deleted] = True"></dx:GridViewFormatConditionHighlight>
            </FormatConditions>
        </ugit:ASPxGridView>
    </div>
    <div>&nbsp;</div>
    <div>
        <asp:LinkButton ID="lnkbtnAddProjectComplexity" runat="server" Text="&nbsp;&nbsp;Add Project Complexity&nbsp;&nbsp;" ToolTip="Add Project Complexity" OnClientClick="return OpenAddCriteria()">
    <span class="button-bg">
        <b style="float: left; font-weight: normal;">
            Add Project Complexity </b>
        <i style="float: left; position: relative; top: -3px;left:2px">                           
        </i> 
    </span>
        </asp:LinkButton>
    </div>
</div>
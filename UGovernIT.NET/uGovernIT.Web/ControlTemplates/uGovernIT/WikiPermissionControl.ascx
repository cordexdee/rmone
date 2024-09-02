<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WikiPermissionControl.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.uGovernIT.WikiPermissionControl" %>
<%--<%@ Register Assembly="DevExpress.Web.v17.2, Version=17.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>--%>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script data-v="<%=UGITUtility.AssemblyVersion %>">
     $(document).ready(function () {
            $('.userValueBox-Table').parent().addClass("userValueBox-searchFilterWrap");
         $('.userValueBox-searchFilterWrap').parent().addClass("userValueBox-searchFilterContainer");
         $('.fetch-popupParent').parent().addClass('popupUp-mainContainer');
     });
</script>
<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .popupUp-mainContainer{
        overflow-y:visible !important;
    }
    .dropDownlabel{
        font: 14px 'Poppins', sans-serif !important;
        color:#4A6EE2;
        margin: 0px 0px 5px 0px;
    }
    .permission-BtnWrap{
        float: right;
    }
    .permission-BtnSave{
        display: inline-block;
        background: #4A6EE2;
        color: #FFF;
        padding: 6px 15px;
        border-radius: 4px;
        float: right;
        font: 14px 'Poppins', sans-serif !important;
    }
    .permission-BtnSave a{
        color:#FFF;
    }
    .permission-BTnCancel{
        display: inline-block;
        border: 1px solid #4A6EE2;
        padding: 5px 22px;
        border-radius: 4px;
        margin-right: 8px;
        font: 14px 'Poppins', sans-serif !important;
    }
</style>
<div class="col-md-12 col-sm-12 col-xs-12 fetch-popupParent">
    <div class="wikipermission_wrap">
        <div class="wikiPermission-titleWrap">
             <h5>Enter Usernames and/or Groups authorized to view this Wiki</h5>
        </div>
        <div class="wikiPermission-droDownWrap">
            <div class="dropDown-label">
                <h3 class="ms-standardheader dropDownlabel">Authorized to View:</h3>
            </div>
            <div class="permission-dropDown">
                <ugit:UserValueBox ID="ppeWikiUser" IsMulti="true" runat="server" CssClass="userValueBox-dropDown" />
            </div>
        </div>
        <div class="row">
            <div class="permission-BtnWrap">
                <div class="permission-BtnSave">
                    <asp:LinkButton ID="btnSave" runat="server" ValidationGroup="WikiPermission" OnClick="btnSave_Click"  Text="Save" ToolTip="Save"></asp:LinkButton>
                </div>
                <div class="permission-BTnCancel">
                    <asp:LinkButton ID="btnCancel" runat="server"  Text="Cancel" ToolTip="Cancel" OnClick="btnCancel_Click"></asp:LinkButton>
                </div>
            </div>
        </div>
    </div>
</div>

<%-- <div id="authorizedToView"> </div>
 <div id="btnsave"> </div>--%>



<script data-v="<%=UGITUtility.AssemblyVersion %>">


    //$('#authorizedToView').dxTagBox({


    //});

    //$('#btnsave').dxButton({
    //    text:"save",
    //    onClick: function (e) {


    //    }

    //});


</script>
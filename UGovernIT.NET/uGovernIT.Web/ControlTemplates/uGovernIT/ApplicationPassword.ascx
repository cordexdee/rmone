<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ApplicationPassword.ascx.cs" Inherits="uGovernIT.Web.ApplicationPassword" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .ms-formbody {
        background: none repeat scroll 0 0 #E8EDED;
        border-top: 1px solid #A5A5A5;
        padding: 3px 6px 4px;
        vertical-align: top;
    }

    .ms-formlabel {
        text-align: right;
        width: 190px;
        vertical-align: top;
    }

    .ms-standardheader {
        text-align: right;
    }
      .ms-long {
        font-family: Verdana,sans-serif;
        font-size: 8pt;
        width: 386px;
    }
       .hide
    {
        display:none;
     }
     .show
     {
         display:inline-block;
     }
</style>
<script data-v="<%=UGITUtility.AssemblyVersion %>">

    function ShowActionImages(obj) {
        $(obj).find(".pswdActions").removeClass("hide");
        $(obj).find(".pswdActions").addClass("show");
    }

    function HideActionImages(obj) {
        $(obj).find(".pswdActions").removeClass("show");
        $(obj).find(".pswdActions").addClass("hide");
    }

    function EditPassword(obj) {
        var url = '<%=addNewItem %>';
        url = url + "&PasswordId=" + $(obj).attr('PasswordId');
        window.parent.UgitOpenPopupDialog(url, "", 'Application Password - Edit Item', '600px', '300px', 0, escape("<%= Request.Url.AbsolutePath %>"));
    }
    function ConfirmDelete() {
        if (confirm("Are you sure you want to delete the password?")) {
            return true;
        }
        else {
            return false;
        }
    }
</script>
<div>
   
        <table style="width:100%">
            <tr>
                <td>
                   
             <ugit:ASPxGridView ID="grdApplPassword" runat="server" AutoGenerateColumns="false"  KeyFieldName="ID" OnHtmlRowPrepared="grdApplPassword_RowDataBound" Width="100%" >
                <Columns>                    
                    <dx:GridViewDataTextColumn FieldName="APPPasswordTitle" Caption="Title"></dx:GridViewDataTextColumn>
                     <dx:GridViewDataTextColumn FieldName="APPUserName" Caption="Username"></dx:GridViewDataTextColumn>
                   
                   <%-- <asp:TemplateField>
                         <ItemTemplate>
                        <span style="vertical-align:top">********</span>
                             
                            
                    </ItemTemplate>
                        <HeaderTemplate>
                            <label>Password</label>
                        </HeaderTemplate>
                        <ItemStyle Width="300px" />
                    </asp:TemplateField>--%>
                       <dx:GridViewDataTextColumn FieldName="Description" Caption="Description"></dx:GridViewDataTextColumn>
                   <dx:GridViewDataTextColumn  Caption="">
                       <DataItemTemplate>
                           <div class="pswdActions show" style="vertical-align:top; text-align:right;" runat="server" id="divActions">
                                 <img  src="/content/images/edit-icon.png" runat="server" alt="edit" id="imgEdit"  PasswordId='<%#Eval("ID") %>'  onclick="return EditPassword(this)"/> 
                                 <asp:ImageButton ID="imgDeletePassword" runat="server" ImageUrl="~/content/images/delete-icon-new.png" alt="delete"  PasswordId='<%#Eval("ID") %>' UserName='<%#Eval("APPUserName") %>' OnClientClick="return ConfirmDelete();" OnClick="imgDeletePassword_Click"/> 
                             </div>
                       </DataItemTemplate>
                   </dx:GridViewDataTextColumn>
                    <%--<asp:TemplateField>
                        <ItemTemplate>
                            <div class="pswdActions show" style="vertical-align:top; text-align:right;" runat="server" id="divActions">
                                 <img  src="/_layouts/15/Images/uGovernIT/edit-icon.png" runat="server" alt="edit" id="imgEdit"  PasswordId='<%#Eval(DatabaseObjects.Columns.Id) %>'  onclick="return EditPassword(this)"/> 
                                 <asp:ImageButton ID="imgDeletePassword" runat="server" ImageUrl="~/_layouts/15/images/uGovernIT/delete-Icon.png" alt="delete"  PasswordId='<%#Eval(DatabaseObjects.Columns.Id) %>' UserName='<%#Eval(DatabaseObjects.Columns.APPUserName) %>' OnClientClick="return ConfirmDelete();" OnClick="imgDeletePassword_Click"/> 
                             </div>
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                </Columns>
        </ugit:ASPxGridView></td>
            </tr>
            <tr>
                <td>
                    <span style="float:right;">
                        <dx:ASPxButton ID="aAddNew" runat="server" ClientInstanceName="aAddNew" Text="New" AutoPostBack="false" CssClass="primary-blueBtn" ></dx:ASPxButton>                        
                    </span>
                </td>
            </tr>
        </table>
  
</div>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ConfigurationVariableListEdit.ascx.cs" Inherits="uGovernIT.Web.ConfigurationVariableListEdit" %>
<%@ Register TagPrefix="ugit" Namespace="uGovernIT.Web" Assembly="uGovernIT.Web" %>
<%@ Register TagPrefix="ugit" Namespace="uGovernIT.Web" Assembly="uGovernIT.Web" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
 .fleft{float:left;}
/*.ms-formbody
{
    background: none repeat scroll 0 0 #E8EDED;
    border-top: 1px solid #A5A5A5;
    padding: 3px 6px 4px;
    vertical-align: top;
}*/
.pctcomplete{ text-align:right;}
.estimatedhours{text-align:right;}
.actualhours{text-align:right;}
.full-width{width:98%;}
.ms-formlabel{text-align:right;}
.existing-section-c{float:left;}
.new-section-c{float:left;}
.existing-section-a{float:left;padding:0px 5px;}
.existing-section-a img{cursor:pointer;}
.new-section-a{float:left;padding-left:5px;}
.new-section-a img{cursor:pointer;}
.ms-standardheader {text-align:right;}
.verticalResize{resize:vertical;}

.reportRadio-btnWrap tr td:last-child {
	text-align: left !important;
}
</style>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>"> 
    function getIndex(Text) {
        if (Text == 'Bool') {
            $(".trText").hide();
            $(".trBool").show();
            $(".trDate").hide();
            $(".truser").hide();
            $(".trattach").hide();
            $(".trPassword").hide();

        }
        else if (Text == 'Text')
        {
            $(".trText").show();
            $(".trBool").hide();
            $(".trDate").hide();
            $(".truser").hide();
            $(".trattach").hide();
            $(".trPassword").hide();

        }
        else if (Text == 'Date')
        {
            $(".trText").hide();
            $(".trBool").hide();
            $(".trDate").show();
            $(".truser").hide();
            $(".trattach").hide();
            $(".trPassword").hide();

        }
        else if (Text == 'User')
        {
            $(".trPassword").hide();
            $(".trText").hide();
            $(".trBool").hide();
            $(".trDate").hide();
            $(".truser").show();
            $(".trattach").hide();

        }
        else if (Text == 'Attachments') {
            $(".trText").hide();
            $(".trBool").hide();
            $(".trDate").hide();
            $(".truser").hide();
            $(".trattach").show();
            $(".trPassword").hide();
        }
        else if (Text == 'Password') {
            $(".trText").hide();
            $(".trBool").hide();
            $(".trDate").hide();
            $(".truser").hide();
            $(".trattach").hide();
            $(".trPassword").show();
        }
    }
    function hideIndex()  
    {           
         
        $('#<%=categoryDD.ClientID%>').get(0).selectedIndex = 0;       
    }
    function DeleteCongigVariable() {
        if (confirm('Are you sure want to delete?')) {
            <%=Page.ClientScript.GetPostBackEventReference(lnkDelete1,string.Empty)%>
         }
    }

    $(document).ready(function () {
        $('.userValueBox-Table').parent().addClass("userValueBox-searchFilterWrap");
        $('.userValueBox-searchFilterWrap').parent().addClass("userValueBox-searchFilterContainer");
        $('.userValueBox-searchFilterContainer').parents().eq(3).addClass('userValueBox-dropDownWrap');
        $('.configVar-popupWrap').parent().addClass('configVar-popupContainer');
    });
</script>

 <div class="col-md-12 col-sm-12 col-xs-12 configVar-popupWrap noPadding my-2">
     <div class="ms-formtable accomp-popup">
         <div class="row" id="trTitle" runat="server" >
             <div class="ms-formlabel">
                 <h3 class="ms-standardheader budget_fieldLabel">
                     Category<b style="color: Red;">*</b>
                 </h3>
                 <input type="hidden" id="hndtest" class="hndtest" value="text" />
             </div>
             <div class="ms-formbody accomp_inputField">
                <div class="">
                    <asp:DropDownList  runat="server" ID="categoryDD" CssClass="aspxDropDownList itsmDropDownList"></asp:DropDownList>
                </div>
                <div class="addCat-wrap">
                    <img alt="Add Category" id="BtnAddCategory"  src="/content/images/plus-cicle.png" style=" cursor:pointer; width:16px;" 
                        onclick="javascript:$('.newCategoryContainer').attr('style','display:block');hideIndex()" />
                    <label class="addCat-label">Add Category</label>
                </div>   
                <div runat="server" id="newCategoryContainer" class="newCategoryContainer" style="display:none;float:left;padding-left:10px;text-align:center;" >
                    <asp:TextBox runat="server" ID="txtCategory" CssClass="txtcategory" Width="110px"></asp:TextBox>                                                   
                    <img alt="Add Category" width="16"; src="/content/images/close-blue.png"  class="canceladdcategory" onclick="javascript:$('.newCategoryContainer').attr('style','display:none')" />                                                   
                </div>                                                   
                <div style="float:right;width:38%;padding:4px 4px 0px">
                    <asp:CustomValidator ID="cvTxtCategory" ValidateEmptyText="true"  Enabled="true" runat="server" ControlToValidate="txtCategory" ErrorMessage="Enter Category Name" Display="Dynamic" OnServerValidate="cvTxtCategory_ServerValidate" ValidationGroup="AdminCatlogIntitial"></asp:CustomValidator>
                </div> 
             </div>
         </div>
         <div class="row" id="tr3" runat="server" >
             <div class="ms-formlabel">
                 <h3 class="ms-standardheader budget_fieldLabel">
                     Key Name<b style="color: Red;">*</b>
                 </h3>
             </div>
             <div class="ms-formbody accomp_inputField">
                 <div><asp:TextBox runat="server" ID="txtkeyName"></asp:TextBox></div>
                                <div><asp:RequiredFieldValidator ID="rfvKeyName" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="txtkeyName"
                        ErrorMessage="Enter Module Name" Display="Dynamic" ValidationGroup="Save" ForeColor="Red"></asp:RequiredFieldValidator></div> 
             </div>
         </div>
         <div class="row" id="tr1" runat="server" >
             <div class="ms-formlabel">
                 <h3 class="ms-standardheader budget_fieldLabel">
                    Key Value<b style="color: Red;">*</b>
                 </h3>
             </div>
             <div class="ms-formbody accomp_inputField">
                 
               <asp:RadioButtonList ID="rdbKeyvalue" RepeatLayout="Table"  CssClass="RadioList reportRadio-btnWrap"  runat="server" AppendDataBoundItems="true"
                   RepeatColumns="4" RepeatDirection="Vertical">
                    <asp:ListItem  Text="Bool" Selected="True" Value="Bool" ></asp:ListItem>
                    <asp:ListItem Text="Text" Value="Text" ></asp:ListItem>
                    <asp:ListItem Text="Date" Value="Date"></asp:ListItem>
                    <asp:ListItem Text="User" Value="User"></asp:ListItem>
                    <asp:ListItem Text="Attachments" Value="Attachments"></asp:ListItem>
                    <asp:ListItem Text="Password" Value="Password"></asp:ListItem>
                </asp:RadioButtonList>
             </div>
         </div>
         <div class="row" >
         
              <div class="ms-formbody accomp_inputField">
                  <div id="trBool" class="trBool" runat="server">
                        <asp:RadioButtonList ID="rdbBool" runat="server" RepeatLayout="Table" RepeatColumns="4"  CssClass="RadioList reportRadio-btnWrap" 
                            RepeatDirection="Vertical">
                            <asp:ListItem Text="True" Selected="True" Value="1"></asp:ListItem>
                            <asp:ListItem Text="False" Value="0"></asp:ListItem>
                        </asp:RadioButtonList>
                   </div>
                  <div id="trText" runat="server" class="trText" style="display:none">
                        <asp:TextBox ID="txtText" runat="server"  TextMode="MultiLine" ReadOnly="false" CssClass="verticalResize"></asp:TextBox>
                        <asp:CustomValidator ID="cvText" ValidateEmptyText="true"  Enabled="true" runat="server" ControlToValidate="txtText" ErrorMessage="Enter Text" OnServerValidate="cvText_ServerValidate" Display="Dynamic"  ValidationGroup="AdminCatlogIntitial"></asp:CustomValidator>
                  </div>
                  <div id="trDate" runat="server" class="trDate" style="display:none">
                       <%--<SharePoint:DateTimeControl  CssClassTextBox="inputTextBox datetimectr111" DateOnly="true" ID="dateValueType"  runat="server" />--%>
                      <dx:ASPxDateEdit ID="dateValueType" runat="server"  EditFormat="Custom" EditFormatString="MM/dd/yyyy" NullText="MM/dd/yyyy"
                          CssClass="CRMDueDate_inputField" DropDownButton-Image-Url="~/Content/Images/calendarNew.png" DropDownButton-Image-Width="16">

                      </dx:ASPxDateEdit>
                  </div>
                  <div id="truser" runat="server" class="truser" style="display:none">
                       <ugit:UserValueBox ID="pEditorUser" runat="server" CssClass="userValueBox-dropDown"></ugit:UserValueBox>
                  </div>
                  <div id="trattach" runat="server" class="trattach" style="display:none">
                      <%--<asp:FileUpload runat="server" ID="fileUpload" Width="100%"  />--%>   
                     
                      <%--<asp:CustomValidator ID="cvfileUpload" ErrorMessage="Select a file." ControlToValidate="fileUpload" runat="server" Enabled="true" OnServerValidate="cvfileUpload_ServerValidate" Display="Dynamic" ValidationGroup="AdminCatlogIntitial"  />--%>
                      <div>
                            <div class="row">
                                <div>
                                     <%--<asp:FileUpload ID="fileUpload" class="fileupload" style="display:none" runat="server"/>
                                     <asp:TextBox runat="server" ID="txtFile" CssClass="txtFile" Width="320px"/>--%>
                                    <ugit:UGITFileUploadManager runat="server" ID="UGITFileUploadManager1"></ugit:UGITFileUploadManager>
                                </div>
                            </div>

                             <div class="row" id="trHelpPicker" runat="server">                                
                                <%--<td>
                                      <a  id="aAddItem" runat="server" onclick="showWiki()" style="cursor:pointer;">Add Wiki</a> |
                                    <a onclick="showUploadControl()" style="cursor:pointer;">Upload Document</a>
                                </td>--%>
                            </div>
                      </div>
                  </div>

                  <div id="trPassword" runat="server" class="trPassword" style="display:none">
                      <asp:TextBox ID="txtPassword" runat="server"></asp:TextBox>
                   <asp:CustomValidator ID="cvPassword" ValidateEmptyText="true"  Enabled="true" runat="server" ControlToValidate="txtPassword" ErrorMessage="Enter Text" OnServerValidate="cvText_ServerValidate" Display="Dynamic"  ValidationGroup="AdminCatlogIntitial"></asp:CustomValidator>
                  </div>
              </div>
          </div>
         <div class="row" id="tr2" runat="server" >
             <div class="ms-formlabel">
                 <h3 class="ms-standardheader budget_fieldLabel">
                     Description
                 </h3>
             </div>
             <div class="ms-formbody accomp_inputField">
               <asp:TextBox runat="server" ID="txtDescription" TextMode="MultiLine" CssClass="verticalResize"></asp:TextBox>
             </div>
         </div>
         <div class="d-flex justify-content-between align-items-center px-1">
            <dx:ASPxButton ID="lnkDelete" runat="server" Visible="false" Text="Delete" ToolTip="Delete" AutoPostBack="false" CssClass="btn-danger1">
                <ClientSideEvents Click="function(s,e){DeleteCongigVariable();}" />
            </dx:ASPxButton>
            <div>
                <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel" CssClass="secondary-cancelBtn" OnClick="btnClose_Click"></dx:ASPxButton>
                <dx:ASPxButton ID="btnSave" runat="server" Text="Save" ToolTip="Save" ValidationGroup="serviceIntitial" CssClass="primary-blueBtn"  OnClick="btSave_Click"></dx:ASPxButton>
                <div>
                    <asp:LinkButton ID="lnkDelete1" runat="server" OnClick="lnkDelete_Click"></asp:LinkButton>
                </div>
            </div>
         </div>
     </div>
 </div>


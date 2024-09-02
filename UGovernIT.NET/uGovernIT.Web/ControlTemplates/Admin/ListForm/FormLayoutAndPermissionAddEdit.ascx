<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FormLayoutAndPermissionAddEdit.ascx.cs" Inherits="uGovernIT.Web.FormLayoutAndPermissionAddEdit" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .ms-formbody {
        /*background: none repeat scroll 0 0 #E8EDED;
        border-top: 1px solid #A5A5A5;*/
        padding: 3px 6px 4px;
        vertical-align: top;
    }
</style>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function OpenConditionPicker() {
        var Url = '<%= SkipConditionUrl%>';
        if (hdnSkipOnCondition.Get("SkipCondition") != undefined) {
            Url += "&SkipCondition=" + escape(hdnSkipOnCondition.Get("SkipCondition"));
        }
        javascript: UgitOpenPopupDialog(Url, '', 'Skip Rule', '90', '50', 0, '<%= Server.UrlEncode(Request.Url.AbsolutePath)%>');
    }



    
    function HideUploadLabel() {
        $("#<%=lblUploadedFile.ClientID %>").hide();
        $("#<%=fileUploadControl.ClientID %>").show();
        $("#<%=ImgEditFile.ClientID%>").hide();
        return false;
    }





    function WikiCallBack(s, e, data) {
     
        var editor = s;
        var customCtrID = s.name.replace("_WikiSelect_wikiListPicker_cbpaneltestst_grid", "");
        var popupCtrl = ASPxClientControl.GetControlCollection().GetByName(customCtrID + "_WikiSelect");
        var textBoxCtrl = ASPxClientControl.GetControlCollection().GetByName(customCtrID + "_txtWiki");
        var Url = "<%=this.detailsURL%>";
        var txtWiki = $('#<%=txtWiki.ClientID%>');

        txtWiki.val(Url + "ticketId=" + data);
        
        WikiSelect.Hide();
        
    }

    function showWiki(id) {
        var popupCtrl = ASPxClientControl.GetControlCollection().GetByName(id + "_WikiSelect");
        popupCtrl.PerformCallback("aShowWiki");
        popupCtrl.Show();
        WikiSelect.SetHeaderText("Wiki Detail" + "(" + 'Click On a Row to Select it' + ")");
        return false;
    }



    function HelpcardCallBack(s, e, data) {

        var editor = s;
        var customCtrID = s.name.replace("_HelpCardSelect_helpCardListPicker_cbpaneltestst_grid", "");
        var popupCtrl = ASPxClientControl.GetControlCollection().GetByName(customCtrID + "_HelpCardSelect");
        var textBoxCtrl = ASPxClientControl.GetControlCollection().GetByName(customCtrID + "_txtHelpCard");
        var Url = "<%=this.showHelpCardURL%>";
        var txtHelpCard = $('#<%=txtHelpCard.ClientID%>');

        //txtHelpCard.val(Url + "ticketId=" + data);
        txtHelpCard.val(data);
        HelpCardSelect.Hide();

    }

    function showHelpCard(id) {        
        var popupCtrl = ASPxClientControl.GetControlCollection().GetByName(id + "_HelpCardSelect");
        popupCtrl.PerformCallback("aShowHelpCard");
        popupCtrl.Show();
        HelpCardSelect.SetHeaderText("Help Card Detail" + "(" + 'Click On a Row to Select it' + ")");
        return false;
    }
       
    function HelpCardSelect_CallBack(s, e) {        
        var scripts = $("#" + s.name + "_PWC-1").find("#helpCardScript")
        scripts.each(function (i, s) {
            $.globalEval($(s).text());
        });

        
    }

    function WikiSelect_CallBack(s, e) {        
        var scripts = $("#" + s.name + "_PWC-1").find("#wikiScript")
        scripts.each(function (i, s) {
            $.globalEval($(s).text());
        });

        $('#ddlTargetType').change(function (e) {
           

        });
    }


</script>

<div class="col-md-12 col-sm-12 col-xs-12 formLayout-addPopupContainer">
    <fieldset>
        <legend class="admin-legendLabel">General</legend>
        <div class="ms-formtable accomp-popup">
            <div class="row">
                <div class="col-md-6 col-sm-6 col-xs-6 noPadding" id="tr12" runat="server">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Field<b style="color: red">*</b></h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <dx:ASPxComboBox ID="cmbFieldName" runat="server" CssClass="aspxComBox-dropDown" Width="100%"
                            DropDownStyle="DropDown" TextFormatString="{0}" ListBoxStyle-CssClass="aspxComboBox-listBox"
                            ValueType="System.String" IncrementalFilteringMode="Contains" FilterMinLength="0" EnableSynchronization="True"
                            CallbackPageSize="10">
                            <Columns>
                            </Columns>
                        </dx:ASPxComboBox>
                        <div>
                            <asp:RequiredFieldValidator ID="rfvFieldName" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="cmbFieldName"
                                ErrorMessage="Enter Field Name" Display="Dynamic" ValidationGroup="Save" ForeColor="Red"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
                <div class="col-md-6 col-sm-6 col-xs-6 noPadding" id="tr13" runat="server">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Display Name</h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <asp:TextBox ID="txtDisplayName" runat="server"/>
                        <asp:Label ID="lblerrormsg" ForeColor="Red" runat="server" ></asp:Label>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-md-6 col-sm-6 col-xs-6 noPadding" id="tr15" runat="server">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Type</h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <asp:DropDownList ID="ddlType" runat="server" CssClass="itsmDropDownList aspxDropDownList">
                            <asp:ListItem Value="">Default</asp:ListItem>
                            <asp:ListItem Value="Currency">Currency</asp:ListItem>
                            <asp:ListItem Value="Percentage">Percentage</asp:ListItem>
                            <asp:ListItem Value="Phone">Phone</asp:ListItem>
                            <asp:ListItem Value="User">User</asp:ListItem>
                            <asp:ListItem Value="Group">Group</asp:ListItem>
                            <asp:ListItem Value="User,Manager">Managers</asp:ListItem>
                            <asp:ListItem Value="UserGroup">User And Group</asp:ListItem>
                            <asp:ListItem Value="NoteField">Note</asp:ListItem>
                            <asp:ListItem Value="Date">Date</asp:ListItem>
                            <asp:ListItem Value="DateTime">Date Time</asp:ListItem>
                            <asp:ListItem Value="Integer">Integer</asp:ListItem>
                            <asp:ListItem Value="Double">Double</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="col-md-6 col-sm-6 col-xs-6 noPadding" id="tr14" runat="server">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Field Sequence</h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <asp:TextBox ID="txtFieldSequence" runat="server" />
                        <asp:RegularExpressionValidator ID="regextxtFieldSequence" ErrorMessage="Only numeric value more then zero." ControlToValidate="txtFieldSequence" 
                            runat="server" ValidationExpression="^(?![,.0]*$)\d{1,4}(?:\d{1,2})?$" ValidateEmptyText="false" ForeColor="Red"  ValidationGroup="Save" />
                    </div>
                </div>
            </div>
            <div class="row" >
                <div class="col-md-6 col-sm-6 col-xs-6 noPadding" id="tr3" runat="server">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Tab</h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <asp:DropDownList ID="ddlTab" runat="server" CssClass="itsmDropDownList aspxDropDownList"></asp:DropDownList>
                    </div>
                </div>
                <div class="col-md-6 col-sm-6 col-xs-6 noPadding" id="tr1" runat="server">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Display Width</h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <dx:ASPxComboBox ID="cmbDisplayWidth" runat="server" DropDownStyle="DropDownList" TextFormatString="{0}" CssClass="aspxComBox-dropDown" Width="100%"
                            ValueType="System.String" IncrementalFilteringMode="Contains" FilterMinLength="0" EnableSynchronization="True" ListBoxStyle-CssClass="aspxComboBox-listBox"
                            CallbackPageSize="10">
                            <Columns>
                            </Columns>
                        </dx:ASPxComboBox>
                    </div>
                </div>
            </div>
            <div class="row" >
                <div class="col-md-6 col-sm-6 col-xs-6 noPadding" id="tr4" runat="server">
                     <div class="ms-formbody accomp_inputField crm-checkWrap">
                        <asp:CheckBox ID="hideCheckTemplate" runat="server" Text="HideIn Template" />
                    </div>
                </div>
                <div class="col-md-6 col-sm-6 col-xs-6 noPadding" id="tr5" runat="server">
                     <div class="ms-formbody accomp_inputField crm-checkWrap">
                        <asp:CheckBox ID="chkShowInMobile" runat="server" Text="Show In Mobile" />
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6 col-sm-6 col-xs-6 noPadding">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Skip On Condition</h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <div style="float: left;">
                            <asp:Label ID="lblSkipOnCondition" CssClass="skipcondition" runat="server"></asp:Label>
                            <img id="Img1" runat="server" src="/content/Images/editNewIcon.png" onclick="OpenConditionPicker();" 
                                style="width:16px; cursor: pointer;" />
                        </div>
                        <dx:ASPxHiddenField ID="hdnSkipOnCondition" runat="server" ClientInstanceName="hdnSkipOnCondition"></dx:ASPxHiddenField>
                    </div>
                </div>
                <div class="col-md-6 col-sm-6 col-xs-6 noPadding" id="tr10" runat="server">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Custom Properties</h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <asp:TextBox ID="txtCustomProperties" TextMode="MultiLine" runat="server"></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>
    </fieldset>
    <fieldset>
        <legend class="admin-legendLabel">Help Link</legend>
        <div class="ms-formtable accomp-popup">
            <div class="row">
                <div class="col-md-6 col-sm-6 col-xs-6 noPadding">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Link Type</h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <asp:DropDownList ID="ddlTargetType" CssClass="target_section itsmDropDownList aspxDropDownList"
                            runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlTargetType_SelectedIndexChanged">
                        </asp:DropDownList>
                    </div>
                </div>
               <div class="col-md-6 col-sm-6 col-xs-6 noPadding" id="trFileUpload" runat="server">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">File</h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <asp:Label ID="lblUploadedFile" runat="server"></asp:Label>
                        <asp:FileUpload ID="fileUploadControl" CssClass="fileUploader" ToolTip="Browse and upload file" runat="server" Style="display: none;" />
                        <img alt="Edit File" title="Edit File" runat="server" id="ImgEditFile" src="/content/Images/editNewIcon.png" style="cursor: pointer;" 
                            onclick="HideUploadLabel();" width="16" />
                        <div>
                            <asp:RequiredFieldValidator ID="rfvFileUpload" CssClass="rfvdFileUploader" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="fileUploadControl" ErrorMessage="Upload a file." Display="Dynamic" ValidationGroup="fileSave"></asp:RequiredFieldValidator>
                        </div>
                    </div>
               </div>
            </div>
            
            <div class="row" >
                <div class="col-md-6 col-sm-6 col-xs-6 noPadding" id="trLink" runat="server">
                     <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Link URL</h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <asp:TextBox ID="txtFileLink" CssClass="fileUploaderLink" runat="server" />
                    </div>
                </div>
                <div class="col-md-6 col-sm-6 col-xs-6 noPadding" id="trWiki" runat="server">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Select Wiki</h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <asp:TextBox ID="txtWiki" runat="server" />
                        <a id="aShowWiki" runat="server"  style="cursor: pointer;" >
                            <img alt="Add Wiki" title="Add Wiki" runat="server" id="AddWikiItem" 
                                src="/content/Images/editNewIcon.png" style="cursor: pointer; width:16px;" />
                        </a>

                        <dx:ASPxPopupControl ID="WikiSelect" runat="server" ClientInstanceName="WikiSelect" AllowResize="true" MinHeight="400" Width="650" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                         AllowDragging="true" PopupAnimationType="Fade" EnableViewState="False" OnWindowCallback="cb_Callback" CloseAction="CloseButton" background="#000000;">
                       
                            <ClientSideEvents EndCallback="function(s,e){WikiSelect_CallBack(s,e);}" CloseUp="function(s,e){WikiSelect.Hide();}" />
                             <ClientSideEvents Closing="function(s) {s.SetContentHtml(null); }" />
                            <ContentCollection>
                                <dx:PopupControlContentControl>
                                 <dx:ASPxHiddenField ID="hdnPopupContentInfo" runat="server"></dx:ASPxHiddenField>
                                     </dx:PopupControlContentControl>
                             </ContentCollection>
                          </dx:ASPxPopupControl>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6 col-sm-6 colxs-6 noPadding" id="trHelpCard" runat="server">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader">Select Help Card</h3>
                    </div>
                    <div class="ms-formbody">
                        <asp:TextBox ID="txtHelpCard" runat="server" Width="290" />
                        <a id="aShowHelpCard" runat="server" style="cursor: pointer;">
                            <img alt="Add Help Card" title="Add Help Card" runat="server" id="Img2" 
                                src="/content/Images/editNewIcon.png" style="cursor: pointer; width:16px;"  />
                        </a>

                        <dx:ASPxPopupControl ID="HelpCardSelect" runat="server" ClientInstanceName="HelpCardSelect" AllowResize="true" MinHeight="400" Width="650" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                            AllowDragging="true" PopupAnimationType="Fade" EnableViewState="False" OnWindowCallback="cb_Callback" CloseAction="CloseButton" background="#000000;">

                            <ClientSideEvents EndCallback="function(s,e){HelpCardSelect_CallBack(s,e);}" CloseUp="function(s,e){HelpCardSelect.Hide();}" />
                            <ClientSideEvents Closing="function(s) {s.SetContentHtml(null); }" />
                            <ContentCollection>
                                <dx:PopupControlContentControl>
                                    <dx:ASPxHiddenField ID="ASPxHiddenField1" runat="server"></dx:ASPxHiddenField>
                                </dx:PopupControlContentControl>
                            </ContentCollection>
                        </dx:ASPxPopupControl>
                    </div>
                </div>
                <div class="col-md-6 col-sm-6 colxs-6 noPadding" id="tooltip" runat="server">
                     <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Tooltip</h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <asp:TextBox ID="txtToolTips" runat="server"   />
                    </div>
                </div>
            </div>
        </div>
    </fieldset>
    <div class="d-flex justify-content-between align-items-center px-1" id="tr2" runat="server">
        <dx:ASPxButton ID="lnkbtndelete" runat="server" Text="Delete" ToolTip="Delete" OnClick="LnkbtnDelete_Click" CssClass="btn-danger1">
                <ClientSideEvents Click="function(s,e){return confirm('Are you sure you want to delete?');}" />
        </dx:ASPxButton>
        <div>
            <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" CssClass="secondary-cancelBtn" ToolTip="Cancel" OnClick="btnCancel_Click"></dx:ASPxButton>
            <dx:ASPxButton ID="btnSave" runat="server" Text="Save" ToolTip="Save" ValidationGroup="Save" OnClick="btnSave_Click" CssClass="primary-blueBtn"></dx:ASPxButton>
        </div>
    </div>
</div>


<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MenuNavigationEdit.ascx.cs" Inherits="uGovernIT.Web.MenuNavigationEdit" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function addMenuName() {
        $(".txtMenuName").show();
        $(".ddlMenuName").hide();
        $(".addMenuName").hide();
        $(".editMenuName").hide();
        $(".cancelMenuName").show();

        $(".menuname .ms-formlabel :hidden").val("1");
        txtMenuName.SetText("");
    }

    function editMenuName() {
        if (ddlMenuName.GetValue() != "Default" && ddlMenuName.GetValue() != "Mobile") {
            $(".txtMenuName").show();
            $(".ddlMenuName").hide();
            $(".menuname .ms-formlabel :hidden").val("2");
            $(".addMenuName").hide();
            $(".editMenuName").hide();
            $(".cancelMenuName").show();
            txtMenuName.SetText(ddlMenuName.GetValue())

        }
        else {
            $("#<%=lblerrormsg.ClientID%>").removeClass("hide");
        }
    }

    function cancelMenuName() {
        $(".txtMenuName").hide();
        $(".ddlMenuName").show();
        $(".addMenuName").show();
        $(".editMenuName").show();
        $(".cancelMenuName").hide();
        $(".menuname .ms-formlabel :hidden").val("");
    }

    function rbtnClickHandel() {
        var val = $('#<%= rListBackground.ClientID %> input:checked').val();

        if (val == "1") {
            $('#<%= ceBkColor.ClientID %>').removeClass("hide");

            $('#<%= divUgitFileUpload.ClientID%>').addClass("hide");
        }
        else {
            
            $('#<%= divUgitFileUpload.ClientID%>').removeClass("hide");
            $('#<%= ceBkColor.ClientID %>').addClass("hide");

        }
    }

    function ddlParentChange() {
        if ($('#<%=ddlParent.ClientID%>')[0].selectedIndex == "0") {
            $('#<%=WebSbMenuStyleTr.ClientID%>').removeAttr("style");
            $("#<%=spanMenuSeparationMsg.ClientID%>").css("display", "block");
            if (cbxMenuStyle.GetSelectedIndex() == 0) {

                $("#<%=trSubMenuItemsPerRow.ClientID%>").css("display", "none");
            }
            else {
                $('#<%=trSubMenuItemsPerRow.ClientID%>').removeAttr("style");
            }
        }
        else {
            $('#<%=WebSbMenuStyleTr.ClientID%>').css("display", "none");
            $("#<%=spanMenuSeparationMsg.ClientID%>").css("display", "none");
            $("#<%=trSubMenuItemsPerRow.ClientID%>").css("display", "none");
        }
    }
    function ddlMenuStyleChange() {

        if (cbxMenuStyle.GetSelectedIndex() != 0) {


            $('#<%=trSubMenuItemsPerRow.ClientID%>').removeAttr("style");
        }
        else
            $("#<%=trSubMenuItemsPerRow.ClientID%>").css("display", "none");
    }
    function SetDefaultColor() {
        if (('<%=isCustomizationClicked%>').toLowerCase() == "true")
            ceBkColor.SetColor(rgb2hex($(".menu-item").css("background-color")));
    }
    function rgb2hex(rgb) {
        rgb = rgb.match(/^rgb\((\d+),\s*(\d+),\s*(\d+)\)$/);
        function hex(x) {
            return ("0" + parseInt(x).toString(16)).slice(-2);
        }
        return "#" + hex(rgb[1]) + hex(rgb[2]) + hex(rgb[3]);
    }

   <%-- function pickSiteAsset(Url) {
        var siteAsset = unescape(Url);
        $('#<%=txtImageUrl.ClientID%>').val(siteAsset);
    }--%>
    
    function pickBackgroundFromLibrary(Url) {
        var siteAsset = unescape(Url);
        if(txtBkImgUrl!=null)
            txtBkImgUrl.SetValue(siteAsset);
        
    } 

    function CloseWithoutSaving()
    {
        var sourceURL = "<%= Request["source"] %>";
        window.parent.CloseWindowCallback(sourceURL);
          
    }
    
     $(document).ready(function () {
        $('.userValueBox-Table').parent().addClass("userValueBox-searchFilterWrap");
        $('.userValueBox-searchFilterWrap').parent().addClass("userValueBox-searchFilterContainer");
        $('.userValueBox-searchFilterContainer').parents().eq(3).addClass('userValueBox-dropDownWrap');
     });  

</script>

<div class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap noPadding">
    <fieldset>
        <legend class="admin-legendLabel">Configuration</legend>
        <div class="ms-formtable accomp-popup">
            <div id="trMenuName" class="menuname row" runat="server" visible="false">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel ">Menu Name                    
                        <asp:HiddenField ID="hdnMenuName" runat="server" />
                    </h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <div>
                        <div class="ddlMenuName menuNavEdit-dropDownWrap" id="ddlMenuNameDiv" runat="server">
                            <dx:ASPxComboBox ClientInstanceName="ddlMenuName" ID="ddlMenuName" OnSelectedIndexChanged="ddlMenuName_SelectedIndexChanged" runat="server" 
                                AutoPostBack="true" CssClass="aspxComBox-dropDown" ListBoxStyle-CssClass="aspxComboBox-listBox" Width="100%"></dx:ASPxComboBox>
                        </div>
                        <div class="txtMenuName" id="txtMenuNameDiv" runat="server" style="display: none; float: left; width:96%;">
                            <dx:ASPxTextBox ClientInstanceName="txtMenuName" CssClass="aspxTextBox-field" ID="txtMenuName" runat="server" Width="100%"></dx:ASPxTextBox>
                        </div>
                        <div class="menuNavEdit-iconWrap">
                            <img src="/Content/images/editNewIcon.png" width="16" id="editMenuName" runat="server" class="editMenuName" onclick="editMenuName()" style="float: left; position: relative; top: 2px; left: 2px;" />
                            <img src="/Content/images/plus-blue.png" width="16" id="addMenuName" runat="server" class="addMenuName" style="margin-left: 6px; float: left; position: relative; top: 2px;" onclick="addMenuName()" />

                            <img src="/Content/images/close-blue.png" id="cancelMenuName" width="16" runat="server" class="cancelMenuName" style="display: none; position: relative; top: 1px; float: left; left: 2px;" onclick="cancelMenuName()" />
                            <asp:Label ID="lblerrormsg" runat="server" ForeColor="Red" CssClass="hide" Text="You can't edit "></asp:Label>
                            <dx:ASPxLabel ID="lblmenutype" runat="server"></dx:ASPxLabel>
                        </div>
                        <div id="webisdiabledDiv" class="menuNavEdit-chkWrap crm-checkWrap" runat="server" style="margin-top:10px;">
                            <asp:CheckBox ID="isdiabledchkbx" Text="Disabled" runat="server" Checked="false" />
                            <%--<dx:ASPxCheckBox ID="" Text="" runat="server" Checked="false"></dx:ASPxCheckBox>--%>
                        </div>
                    </div>
                    <asp:CustomValidator ID="cvMenuName" CssClass="error fullwidth" runat="server" ValidationGroup="Save" OnServerValidate="cvMenuName_ServerValidate"></asp:CustomValidator>
                </div>
            </div>

            <div class="row">
                 <div class="col-md-6 col-sm-6 col-xs-6 noPadding">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Parent</h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <asp:DropDownList ID="ddlParent" onchange="ddlParentChange();" CssClass="itsmDropDownList aspxDropDownList" runat="server"></asp:DropDownList>
                    </div>
                </div>
                 <div class="col-md-6 col-sm-6 col-xs-6 noPadding">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Item Order</h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <asp:TextBox ID="txtItemOrder" runat="server" />
                        <div>
                            <asp:RegularExpressionValidator ID="regextxtItemOrder" ValidationExpression="^([0-9]+)$" ValidateEmptyText="true" Enabled="true" runat="server"
                                ControlToValidate="txtItemOrder" ForeColor="Red" ErrorMessage="Invalid Format" Display="Dynamic" ValidationGroup="Save"></asp:RegularExpressionValidator>
                            <%--<asp:RequiredFieldValidator ID="rfvItemorder" Text="Item order can't be null" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="txtItemOrder" ErrorMessage="Invalid Format" Display="Dynamic" ValidationGroup="Save"></asp:RequiredFieldValidator>--%>
                        </div>
                    </div>
                </div>
            </div>
           <div class="row">
               <div class="col-md-6 col-sm-6 col-xs-6 noPadding" id="trTitle" runat="server">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Title<b style="color: Red;">*</b></h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <asp:TextBox ID="txtTitle" runat="server"/>
                        <div>
                            <asp:RequiredFieldValidator ID="rfvtxtTitle" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="txtTitle"
                                ErrorMessage="Please enter title" ForeColor="Red" Display="Dynamic" ValidationGroup="Save"></asp:RequiredFieldValidator>
                             <asp:Label ID="lblError" runat="server" ForeColor="Red" Visible="false" Text="You can't have duplicate Parent items."></asp:Label>
                       
                        </div>
                    </div>
                </div>
               <div class="col-md-6 col-sm-6 col-xs-6 noPadding">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Display Mode</h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <asp:DropDownList ID="ddlDisplayType" CssClass=" itsmDropDownList aspxDropDownList" AutoPostBack="true" runat="server" 
                            OnSelectedIndexChanged="ddlDisplayType_SelectedIndexChanged">
                            <asp:ListItem Text="TitleOnly" Value="titleonly"></asp:ListItem>
                            <asp:ListItem Text="IconOnly" Value="icononly"></asp:ListItem>
                            <asp:ListItem Text="Both" Value="both"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
           </div>
            <div class="row">
                 <div class="col-md-6 col-sm-6 col-xs-6 noPadding" id="trIconUrl" runat="server" visible="false">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Icon Url</h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                          <ugit:UGITFileUploadManager ID="UGITFileUploadManager1" width="100%" runat="server" AnchorLabel="Upload Icon" hideWiki="true" />
                    </div>
                </div>
                <div class="col-md-6 col-sm-6 col-xs-6 noPadding">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Navigation Url</h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <asp:TextBox ID="txtNavigationUrl" runat="server" Width="100%"/>
                    </div>
                </div>
            </div>
            <div class="row" id="WebSbMenuStyleTr" runat="server" style="display: none;">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Sub Menu Style</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <dx:ASPxComboBox ID="cbxMenuStyle" ClientInstanceName="cbxMenuStyle" CssClass="aspxComBox-dropDown" runat="server" Width="100%"
                        ListBoxStyle-CssClass="aspxComboBox-listBox">
                        <ClientSideEvents SelectedIndexChanged="function(s,e){ ddlMenuStyleChange()}" />
                        <Items>
                            <dx:ListEditItem Selected="true" Text="Vertical" Value="Vertical" />
                            <dx:ListEditItem Text="Horizontal" Value="Horizontal" />
                        </Items>
                    </dx:ASPxComboBox>
                </div>
            </div>

            <div class="row" id="trSubMenuItemsPerRow" runat="server" style="display: none;">
                <div class="ms-formlabel" style="vertical-align: middle">
                    <h3 class="ms-standardheader budget_fieldLabel">Sub Menu Items Per Row</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <div class="col-md-6 col-sm-6 col-xs-6 noLeftPadding">
                         <dx:ASPxSpinEdit ID="seSubMenuItemsPerRow"  MinValue="0" MaxValue="10" HorizontalAlign="Right" CssClass="aspxSpinEdit-dropDown" runat="server" 
                             Width="100%" NumberType="Integer" SpinButtons-ShowIncrementButtons="true" SpinButtons-ShowLargeIncrementButtons="false" />
                    </div>
                     <div class="col-md-6 col-sm-6 col-xs-6 noRightPadding">
                           <dx:ASPxComboBox ID="cbxSubMenuTextAlignment" HelpText="Alignment" HelpTextSettings-HorizontalAlign="Left" HelpTextSettings-Position="Left"
                               HelpTextSettings-VerticalAlign="Middle" Width="100%" ClientInstanceName="cbxSubMenuTextAlignment" runat="server" CssClass="aspxComBox-dropDown" 
                               ListBoxStyle-CssClass="aspxComboBox-listBox">
                            <Items>
                                <dx:ListEditItem Text="Left" Value="Left" />
                                <dx:ListEditItem Text="Center" Value="Center" />
                                <dx:ListEditItem Text="Right" Value="Right" />
                            </Items>
                        </dx:ASPxComboBox>
                     </div>
                </div>
            </div>
            <div class="row">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Navigation Type</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:DropDownList ID="ddlNavigationType" CssClass="itsmDropDownList aspxDropDownList" runat="server">
                        <asp:ListItem Text="Navigation"></asp:ListItem>
                        <asp:ListItem Text="Modeless"></asp:ListItem>
                        <asp:ListItem Text="Modal"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="row">
               <div class="col-md-6 col-sm-6 col-xs-6 noPadding">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Authorized To View</h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <ugit:UserValueBox ID="pEditorUserGroup" CssClass="userValueBox-dropDown" runat="server" isMulti="true" />
                        <div>
                            <asp:CustomValidator ID="cvAuthorized" runat="server" Enabled="true"></asp:CustomValidator>
                        </div>
                    </div>
                </div>
                <div class="col-md-6 col-sm-6 col-xs-6 noPadding" runat="server" id="webcustomizeformateTr">
                    <div class="ms-formbody accomp_inputField" style="padding-top:20px;">
                       <%-- <asp:CheckBox ID="CustomizeMenuFormat" runat="server" Checked="false" Text="Customize Format" TextAlign="Left"
                            OnCheckedChanged="CustomizeMenuFormat_CheckedChanged"/>--%>
                        <dx:ASPxCheckBox ID="CustomizeMenuFormat" AutoPostBack="true"  runat="server" Text="Customize Format"
                             OnCheckedChanged="CustomizeMenuFormat_CheckedChanged" Checked="false"></dx:ASPxCheckBox>
                        <dx:ASPxLabel ID="lblCustomizeMenuFormat" Visible="false" Text="Customization not available for Classic theme" runat="server"></dx:ASPxLabel>
                    </div>
                </div>
            </div>
        </div>
    </fieldset>

    <div class="col-md-12 col-sm-12 col-xs-12 noPadding" id="webformattingDIV" runat="server">
        <fieldset>
            <legend class="admin-legendLabel">Formatting</legend>
            <div class="ms-formtable accomp-popup ">
                <div class="row" id="WebMenuFontPropTr" runat="server">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Font</h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <div class="col-md-4 col-sm-4 col-xs-4 noPadding">
                            <dx:ASPxColorEdit ID="ceFontcolor" Color="#ffffff" HorizontalAlign="Right" runat="server" CssClass="aspxColorEdit-dropDwon"/>
                        </div>
                        <div class="col-md-4 col-sm-4 col-xs-4 noPadding">
                            <dx:ASPxComboBox ID="cbxMenuFontFamily" runat="server" CssClass="aspxComBox-dropDown" ListBoxStyle-CssClass="aspxComboBox-listBox">
                                <Items>
                                    <dx:ListEditItem Text="Default" Selected="true" Value="Default" />
                                    <dx:ListEditItem Text="Arial" Value="Arial" />
                                    <dx:ListEditItem Text="Arial Black" Value="Arial Black" />
                                    <dx:ListEditItem Text="Arial Narrow" Value="Arial Narrow" />
                                    <dx:ListEditItem Text="Arial Rounded MT Bold" Value="Arial Rounded MT Bold" />
                                    <dx:ListEditItem Text="Avant Garde" Value="Avant Garde" />

                                    <dx:ListEditItem Text="Big Caslon" Value="Big Caslon" />
                                    <dx:ListEditItem Text="Bodoni MT" Value="Bodoni MT" />
                                    <dx:ListEditItem Text="Book Antiqua" Value="Horizontal" />
                                    <dx:ListEditItem Text="Brush Script MT" Value="Brush Script MT" />
                                    <dx:ListEditItem Text="Baskerville" Value="Baskerville" />

                                    <dx:ListEditItem Text="Calibri" Value="Calibri" />
                                    <dx:ListEditItem Text="Candara" Value="Candara" />
                                    <dx:ListEditItem Text="Courier New" Value="Courier New" />
                                    <dx:ListEditItem Text="Century Gothic" Value="Century Gothic" />
                                    <dx:ListEditItem Text="Copperplate" Value="Copperplate" />

                                    <dx:ListEditItem Text="Didot" Value="Didot" />

                                    <dx:ListEditItem Text="Franklin Gothic Medium" Value="Franklin Gothic Medium" />
                                    <dx:ListEditItem Text="Futura" Value="Futura" />

                                    <dx:ListEditItem Text="Geneva" Value="Geneva" />
                                    <dx:ListEditItem Text="Gill Sans" Value="Gill Sans" />
                                    <dx:ListEditItem Text="Garamond" Value="Garamond" />
                                    <dx:ListEditItem Text="Georgia" Value="Georgia" />
                                    <dx:ListEditItem Text="Goudy Old Style" Value="Goudy Old Style" />

                                    <dx:ListEditItem Text="Helvetica" Value="Helvetica" />
                                    <dx:ListEditItem Text="Hoefler Text" Value="Hoefler Text" />

                                    <dx:ListEditItem Text="Impact" Value="Impact" />

                                    <dx:ListEditItem Text="Lucida Grande" Value="Lucida Grande" />
                                    <dx:ListEditItem Text="Lucida Bright" Value="Lucida Bright" />
                                    <dx:ListEditItem Text="Lucida Console" Value="Lucida Console" />
                                    <dx:ListEditItem Text="Lucida Sans Typewriter" Value="Lucida Sans Typewriter" />

                                    <dx:ListEditItem Text="Monaco" Value="Monaco" />

                                    <dx:ListEditItem Text="Optima" Value="Optima" />

                                    <dx:ListEditItem Text="Palatino" Value="Palatino" />
                                    <dx:ListEditItem Text="Perpetua" Value="Perpetua" />
                                    <dx:ListEditItem Text="Papyrus" Value="Papyrus" />

                                    <dx:ListEditItem Text="Rockwell" Value="Rockwell" />
                                    <dx:ListEditItem Text="Rockwell Extra Bold" Value="Rockwell Extra Bold" />

                                    <dx:ListEditItem Text="Segoe UI" Value="Segoe UI" />

                                    <dx:ListEditItem Text="Tahoma" Value="Tahoma" />
                                    <dx:ListEditItem Text="Trebuchet MS" Value="Trebuchet MS" />
                                    <dx:ListEditItem Text="Times New Roman" Value="Times New Roman" />

                                    <dx:ListEditItem Text="Verdana" Value="Verdana" />
                                </Items>
                            </dx:ASPxComboBox>
                        </div>
                        <div class="col-md-4 col-sm-4 col-xs-4 noPadding">
                            <dx:ASPxSpinEdit ID="spnBtnMenuFontSize" MinValue="0" CssClass="aspxSpinEdit-dropDown" MaxValue="2147483647" HorizontalAlign="Right" runat="server"
                                HelpText="pt" HelpTextSettings-Position="Right" NumberType="Integer" SpinButtons-ShowIncrementButtons="true" 
                                SpinButtons-ShowLargeIncrementButtons="false" />
                        </div>
                    </div>
                </div>
                <div class="row" id="webMenuHeightWidthTr" runat="server" visible="false">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Width x Height</h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <div class="col-md-5 col-sm-5 col-xs-5 noPadding">
                            <dx:ASPxSpinEdit ID="spnBtnMenuWidth" MinValue="0" MaxValue="1000" HorizontalAlign="Right" runat="server" HelpText="px"
                                HelpTextSettings-Position="Right" NumberType="Integer" SpinButtons-ShowIncrementButtons="true" 
                                SpinButtons-ShowLargeIncrementButtons="false" CssClass="aspxSpinEdit-dropDown" />
                        </div>
                        <div class="col-md-1 col-sm-1 col-xs-1">X</div>
                        <div class="col-md-6 col-sm-6 col-xs-6 noPaddings">
                            <dx:ASPxSpinEdit ID="spnBtnMenuHeight" MinValue="0" MaxValue="1000" Width="100%" HorizontalAlign="Right" runat="server" HelpText="px" 
                                HelpTextSettings-Position="Right" NumberType="Integer" SpinButtons-ShowIncrementButtons="true" 
                                SpinButtons-ShowLargeIncrementButtons="false" CssClass="aspxSpinEdit-dropDown" />
                        </div>
                    </div>
                </div>
                <div class="row" id="WebBkTr" runat="server" style="display: none;">
                    <div class="ms-formlabel" style="vertical-align: middle">
                        <h3 class="ms-standardheader budget_fieldLabel">Background</h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <asp:RadioButtonList ID="rListBackground" runat="server" RepeatDirection="Horizontal" CssClass="radio-btnList" TextAlign="Right" onclick="rbtnClickHandel();">
                            <asp:ListItem Text="Color" Value="1" />
                            <asp:ListItem Text="Image" Value="2" />
                        </asp:RadioButtonList>

                        <dx:ASPxColorEdit ID="ceBkColor" ClientInstanceName="ceBkColor" Width="100%" runat="server" CssClass="aspxColorEdit-dropDwon hide">
                            <ClientSideEvents Init="function(s,e){ SetDefaultColor(); }" />
                        </dx:ASPxColorEdit>

                        <div id="divUgitFileUpload" runat="server" >
                        <ugit:UGITFileUploadManager ID="ugitFileUpload" runat="server" AnchorLabel="Upload Icon" hideWiki="true" />
                            </div>
                        <%--<asp:FileUpload runat="server" ID="fileUpload" CssClass="hide" Width="386px" />
                        <dx:ASPxTextBox ID="txtBkImgUrl" CssClass="hide" ClientInstanceName="txtBkImgUrl" runat="server" Width="399px" />
                        <asp:LinkButton ID="lnkbackgroundimage" runat="server" CssClass="hide" Font-Size="10px" Text="PickFromAsset">Pick From Library</asp:LinkButton>--%>
                    </div>
                </div>

                <div class="row" id="WebMenuSeprationTr" runat="server" visible="true">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Menu Separation</h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <dx:ASPxSpinEdit ID="spnBtnMenuSeparation" MinValue="0" Width="100%" MaxValue="2147483647" HorizontalAlign="Right" Theme="DevEx" runat="server" 
                            HelpText="px" HelpTextSettings-Position="Right" NumberType="Integer" SpinButtons-ShowIncrementButtons="true" 
                            SpinButtons-ShowLargeIncrementButtons="false" CssClass="aspxSpinEdit-dropDown" />
                        <span style="color: #A5A5A5; display: none" runat="server" id="spanMenuSeparationMsg">This configuration will be applied to all root items</span>
                    </div>
                </div>
                <div class="row" id="WebMenuTextAlignmentTr" runat="server">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Title Alignment</h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <dx:ASPxComboBox ID="cbxMenuTextAlignment" SelectedIndex="4" Width="100%" runat="server" CssClass="aspxComBox-dropDown" ListBoxStyle-CssClass="aspxComboBox-listBox">
                            <Items>
                            </Items>
                        </dx:ASPxComboBox>
                    </div>
                </div>

                <%--OnInit="cbxMenuTextAlignment_DataBinding"--%>
                <div class="row" id="WebMenuIconAlignment" runat="server">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Icon Alignment</h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <dx:ASPxComboBox ID="cbxMenuIconAlignment" SelectedIndex="4" Width="100%"  runat="server" CssClass="aspxComBox-dropDown" ListBoxStyle-CssClass="aspxComboBox-listBox" >
                            <Items>
                            </Items>
                        </dx:ASPxComboBox>
                    </div>
                </div>

            </div>

        </fieldset>
    </div>

    <div class="d-flex justify-content-between align-items-center px-1">
        <dx:ASPxButton ID="lnkbtnDelete" runat="server" Text="Delete" ToolTip="Delete" OnClick="LnkbtnDelete_Click" CssClass="btn-danger1">
            <ClientSideEvents Click="function(s, e){return confirm('Are you sure you want to delete?');}" />
        </dx:ASPxButton>
        <div>
            <dx:ASPxButton ID="btnCancel" Text="Cancel" ToolTip="Cancel" runat="server" AutoPostBack="false" CssClass="secondary-cancelBtn">
                <ClientSideEvents Click="CloseWithoutSaving" />
            </dx:ASPxButton>
            <dx:ASPxButton ID="btnSave" runat="server" Text="Save" ToolTip="Save" ValidationGroup="Save" OnClick="btnSave_Click" CssClass="primary-blueBtn">
            </dx:ASPxButton>
        </div>
    </div>
</div>

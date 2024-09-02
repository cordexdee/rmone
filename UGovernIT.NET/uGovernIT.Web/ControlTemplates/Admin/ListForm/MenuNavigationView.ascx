<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MenuNavigationView.ascx.cs" Inherits="uGovernIT.Web.MenuNavigationView" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>



<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function addNewMenu() {
        var url;
        var menuType = clintMenuNavigationType.GetValue();

        url = '<%=editMenuNavigation%>' + '&Id=0&type=' + menuType;
        var requestUrl = '<%=requestUrl%>';
        UgitOpenPopupDialog(url, '', 'Add New Menu', '600px', '700px', 0, escape(requestUrl))
        return false;
    }

    function onDragNoteCall(treelist, e) {
        if ($(treelist.GetNodeHtmlElement(e.nodeKey)).find("td:nth-child(2)").hasClass("dxtlIndent")) {
            e.cancel = false;
        }
        else {
            e.cancel = true;
        }
    }
   

    function SaveMenuclick() {
        if ($("#<%=btSaveMenu.ClientID%>").hasClass("Edit")) {
            $("#<%=btSaveMenu.ClientID%>").removeClass("Edit");
            $("#<%=btSaveMenu.ClientID%>").trigger("click");
        }
        else {
            $("#<%=btCreateMenu.ClientID%>").trigger("click");
        }
    }
    function showAddmenuPopup() {
        url = '<%=editMenuNavigation%>' + '&Id=0&type=';
        var requestUrl = '<%=requestUrl%>';
        UgitOpenPopupDialog(url, '', 'Add New Menu', '600px', '700px', 0, escape(requestUrl))
        return false;
    }

    function showEditmenuPopup() {
        $("#<%=btSaveMenu.ClientID%>").addClass("Edit");
        var selecteditemVal = clintMenuNavigationType.GetValue();
        clintTxtMenuName.SetValue(selecteditemVal);
        navigationMenuPopupCIN.Show();
        return false;
    }
    function SetDefaultColor(s,e) {
       
        if (('<%=isDefaultMenuHighlighter%>').toLowerCase() === "true")
        {
            var defaultColor = $(".defaultHighlight").css("background-color");
             setMenuHighlightColor(defaultColor);
           
        }
          
        
    }
    function setMenuHighlightColor(defaultColor)
    {
       
        menuHightlightColorPicker.SetColor(rgb2hex(defaultColor));
    }
    function rgb2hex(rgb) {
        rgb = rgb.match(/^rgb\((\d+),\s*(\d+),\s*(\d+)\)$/);
        if (rgb != null)
        {
            function hex(x) {
                return ("0" + parseInt(x).toString(16)).slice(-2);
            }
            return "#" + hex(rgb[1]) + hex(rgb[2]) + hex(rgb[3]);
        }
        
    }

    function SaveHighlightColor(s,e)
    {
        var highlightColor = s.GetColor();
        var qData = '{' + '"highlightColor":"' + highlightColor + '"}';
        
        $.ajax({
            type: "POST",
            url: "<%=ajaxHelperPage%>/SaveHighlightColor",
            data: qData,
            contentType: "application/json; charset=utf-8",
            dataType: "text",
            success: function (message) {
              
            },
            error: function (xhr, ajaxOptions, thrownError) {
                
            }
       });
    }
    function expandAllTask() {
        InstreeList.ExpandAll();
    }
    function collapseAllTask() {
        InstreeList.CollapseAll();
    }
</script>

<div class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap" id="content" style="margin-top:10px;">
    <div class="row">
        <div class="accomp-popup col-md-4 col-sm-4 col-xs-12 noLeftPadding" id="divMenuType" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Menu</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <div class="menuNav-dropDownWrap">
                    <dx:ASPxComboBox ID="cbxMenuNavigationType" AutoPostBack="true" Width="100%" OnSelectedIndexChanged="cbxMenuNavigationType_SelectedIndexChanged" ClientEnabled="true"
                        ClientInstanceName="clintMenuNavigationType" runat="server" CssClass="setposition aspxComBox-dropDown" ListBoxStyle-CssClass="aspxComboBox-listBox">
                                <Items>
                                </Items>
                    </dx:ASPxComboBox>
                </div>
                <div class="menuNav-addBtnWrap">
                    <asp:ImageButton ID="btAddnewMenuk" OnClientClick="return showAddmenuPopup();" runat="server" ToolTip="Add New Menu" Width="16" 
                        ImageUrl="/Content/images/plus-blue.png" />
                </div>
                 
            </div>
        </div>
        <asp:UpdatePanel ID="uppnlHideHomeTab" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <%--<asp:RadioButton ID="rbtnWeb" Text="Web Navigation" Font-Bold="true" runat="server" GroupName="Navigation" AutoPostBack="true" OnCheckedChanged="rbtnWeb_CheckedChanged" CssClass="radiobutton" />
                <asp:RadioButton ID="rbtnMobile" Text="Mobile Navigation" Font-Bold="true" runat="server" GroupName="Navigation" AutoPostBack="true" OnCheckedChanged="rbtnMobile_CheckedChanged" CssClass="radiobutton" />--%>

                <%--<asp:CheckBox ID="chkbxHideHomeTab" Text="Hide Home Menu Item" Font-Bold="true" AutoPostBack="true" onclick="showLoadingPanel()" OnCheckedChanged="chkbxHideHomeTab_CheckedChanged" runat="server" />--%>

                <dx:ASPxLoadingPanel ID="LoadingPanel" runat="server" Text="Please Wait ..." ClientInstanceName="LoadingPanel" Modal="True">
                </dx:ASPxLoadingPanel>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="col-md-4 col-sm-4 col-xs-12">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Highlight Color</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <dx:ASPxColorEdit ClientInstanceName="menuHightlightColorPicker" Width="100%" CssClass="aspxColorEdit-dropDwon" HelpTextSettings-Position="Left" 
                    ID="menuHightlightColorPicker" runat="server">
                        <ClientSideEvents  Init="function(s,e){ SetDefaultColor(s,e)}" ColorChanged="function(s,e){SaveHighlightColor(s,e)}"   />
                </dx:ASPxColorEdit>
            </div>
        </div>
        <div class="col-md-4 col-sm-4 col-xs-12 noRightPadding">
            <div class="menuNav-addItemWrap">
                <dx:ASPxButton ID="btnApplyChanges" CausesValidation="false" Text="Apply Changes" runat="server" OnClick="btnRefreshCache_Click"
                    CssClass="primary-blueBtn"></dx:ASPxButton>
                <dx:ASPxButton ID="btMainMenu_Top" runat="server" Text="Add New Menu" ToolTip="Add New Menu" CssClass="primary-blueBtn"
                    Image-Url="~/Content/Images/plus-symbol.png" Image-Width="12">
                    <ClientSideEvents Click="function(s, e){return addNewMenu();}" />
                </dx:ASPxButton>
                
             </div>
        </div>
    </div>
    <div class="row">
        <div class="aspxTreeList-wrap">
            <div style="margin-bottom: 7px">
            <img src="/content/images/expand-all-new.png" title="Expand All" onclick="expandAllTask()" width="16" />
            <img onclick="collapseAllTask()" src="/content/images/collapse-all-new.png" title="Collapse All" width="16" />
        </div>
            <dx:ASPxTreeList ID="treeList" runat="server" AutoGenerateColumns="False" ImagesEditors-ImageEmpty-AlternateText=" No data found" 
                OnProcessDragNode="treeList_ProcessDragNode" Width="100%" KeyFieldName="ID" SettingsPager-Summary-EmptyText="No data found" 
                ParentFieldName="MenuParentLookup" OnHtmlRowPrepared="treeList_HtmlRowPrepared" ClientInstanceName="InstreeList" CssClass="aspxTreeList">
                <Columns>
                    <dx:TreeListDataColumn FieldName="Title" VisibleIndex="0" Caption="Title">
                        <DataCellTemplate>
                            <asp:HyperLink ID="titleLinkButton" runat="server" CausesValidation="false"></asp:HyperLink>
                        </DataCellTemplate>
                    </dx:TreeListDataColumn>

                    <dx:TreeListDataColumn FieldName="ItemOrder" VisibleIndex="1" Caption="#" CellStyle-HorizontalAlign="Left">
                        <CellStyle HorizontalAlign="Left"></CellStyle>
                    </dx:TreeListDataColumn>
                    <dx:TreeListDataColumn FieldName="NavigationUrl" Caption="Navigation Url" VisibleIndex="2" />
                    <dx:TreeListDataColumn FieldName="NavigationType" Caption="Navigation Type" VisibleIndex="3" />
                    <dx:TreeListDataColumn FieldName="MenuDisplayType" Caption="Display Type" VisibleIndex="4" />

                    <dx:TreeListDataColumn Caption=" " FieldName="Id" Width="40px">
                        <DataCellTemplate>
                            <asp:HyperLink ID="editLinkButton" runat="server" CausesValidation="false" ImageUrl="/Content/Images/editNewIcon.png" ImageWidth="16"></asp:HyperLink>
                        </DataCellTemplate>
                    </dx:TreeListDataColumn>
                    <dx:TreeListDataColumn FieldName="ID" Visible="false" VisibleIndex="5">
                        <%--<CellStyle BackColor="#ffebb1" />--%>
                    </dx:TreeListDataColumn>
                    <dx:TreeListDataColumn FieldName="ParentID" Visible="false" VisibleIndex="6">
                        <%--<CellStyle BackColor="#ffebb1" />--%>
                    </dx:TreeListDataColumn>
                </Columns>
                <Styles>
                    <AlternatingNode Enabled="true"/>
                    <Header CssClass="homeGrid_headerColumn"></Header>
                    <Node CssClass="homeGrid_dataRow treeList-dataRow"></Node>
                    <AlternatingNode CssClass="homeGrid_dataRow treeList-dataRow"></AlternatingNode>
                </Styles>
                <Settings ShowTreeLines="false" GridLines="Horizontal" />
                <SettingsBehavior  AllowSort="false" />
                <SettingsEditing AllowNodeDragDrop="true" />
                <ClientSideEvents StartDragNode="function(s,e){onDragNoteCall(s,e);}" />
            </dx:ASPxTreeList>
        </div>
    </div>
    <div class="row bottom-addBtn">
        <div class="headerItem-addItemBtn">
            <dx:ASPxButton ID="btMainMenu" runat="server" Text="Add New Menu" ToolTip="Add New Menu" CssClass="primary-blueBtn"
                    Image-Url="~/Content/Images/plus-symbol.png" Image-Width="12">
                <ClientSideEvents Click="function(s, e){return addNewMenu(); }" />
            </dx:ASPxButton>
        </div>
    </div>
</div>

<%--Add new Navigation Menu popup::start--%>
<dx:ASPxPopupControl ClientInstanceName="navigationMenuPopupCIN" ID="navigationMenuPopup" runat="server" Modal="true"
    PopupElementID="popupArea" CloseAction="CloseButton" AllowDragging="false" ShowOnPageLoad="false" ShowFooter="false" ShowHeader="true" HeaderText="Navigation Menu"
    EnableViewState="false" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
    <ContentCollection>
        <dx:PopupControlContentControl ID="navigationMenuPopupContentControl" runat="server">
            <div style="float: left; height: 200px; width: 400px;" class="first_tier_nav">

                <table style="width: 100%;">
                    <tr>
                        <td class="ms-formlabel">
                            <h3 class="ms-standardheader">Menu Name
                           <span style="color: red;">*</span></h3>
                        </td>
                        <td class="ms-formbody">
                            <dx:ASPxTextBox ID="txtMenuName" ClientInstanceName="clintTxtMenuName" runat="server"></dx:ASPxTextBox>
                            <asp:RequiredFieldValidator ID="validateMenuName" runat="server" ControlToValidate="txtMenuName" ForeColor="Red" ErrorMessage="Fill Menu name" />
                            <span id="spnerror" runat="server" visible="false" style="color: red; float: left;">Menu type allready exist !!</span>
                        </td>
                    </tr>

                    <tr>
                        <td></td>
                        <td>
                            <ul style="float: right">
                                <li runat="server" id="Li178" class="" onmouseover="this.className='tabhover'" onmouseout="this.className=''" style="color: red">

                                    <a id="saveMenubt" onclick="SaveMenuclick();" style="color: white" class="save">Save</a>
                                    <dx:ASPxButton runat="server"  ID="btCreateMenu" OnClick="btCreateMenu_Click" ></dx:ASPxButton>
                                    <dx:ASPxButton runat="server" ID="btSaveMenu" OnClick="BtSaveMenu_Click"></dx:ASPxButton>
                                </li>

                                <li runat="server" id="Li25" class="" onmouseover="this.className='tabhover'" onmouseout="this.className=''">

                                    <a style="color: white" class="cancelwhite" onclick="navigationMenuPopupCIN.Hide();" href="javascript:void(0);">Cancel</a>
                                </li>
                            </ul>
                        </td>
                    </tr>
                </table>

            </div>
        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>
<%--Add new Navigation Menu popup::end--%>

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomTicketRelationShip.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.Shared.CustomTicketRelationShip" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Manager" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .dxtlIndent_UGITNavyBlueDevEx {
        padding: 0px 4px;
    }
</style>

<script type="text/javascript" id="dxss_inlineCtrScriptCTR3" data-v="<%=UGITUtility.AssemblyVersion %>">
    $(function () {

        //addHeightToCalculateFrameHeight(this, $(parent.frames[0]).height());
        //$('#s4-workspace').css('height', $(parent.frames[0]).height() + 'px');
        var ua = window.navigator.userAgent;
        var msie = ua.indexOf("MSIE ");
        if (msie > 0)      // checking for ie
        {
            if ($(parent.frames[0]).height() - $('#s4-workspace').height() > 20) {
                if (typeof (treeListRT) != "undefined" && treeListRT.GetAllDataRows().length > 1) {

                    window.location.reload();
                }
            }
        }
    });

    function UpdateButton_Click(s, e) {

        //This line will load the DOM of dropdown 
        var cbomodule = document.getElementById("<%=ddlModuleDetail.ClientID%>");
        //This will return the selected option
        var selectedOption = cbomodule.options[cbomodule.selectedIndex].value;
        //var selectedOption = cbomodule.options[cbomodule.selectedIndex];
        //This will give you the value of the attribut
        var ticketid = "<%=TicketId%>";
        //var url = selectedOption.attributes.Url.value + "?ParentId=" + ticketid + "&NewSubticket=true&SourceTicketId=" + ticketid;
        var url = selectedOption + "?ParentId=" + ticketid + "&NewSubticket=true&SourceTicketId=" + ticketid;

        window.parent.parent.parent.UgitOpenPopupDialog(url, '', 'New Sub Item for ' + ticketid , '90', '90', '/layouts/ugovernit/delegatecontrol.aspx', false);
        window.parent.CloseWindowCallback(0, document.location.href);

    }
</script>
<asp:Panel ID="pageDetailPanel" runat="server">

    <script type="text/javascript" id="dxss_inlineCtrScriptCTR2" data-v="<%=UGITUtility.AssemblyVersion %>">
        function BeginRequestHandler(sender, args) {
            //  alert(sender.length);
        }
        function MyPageLoading(sender, args) {
            // alert(args.get_dataItems());
            //the panel collection (going to be updated) count will be displayed.
            /// alert(args.get_panelsUpdating().length);

            //the content of the first item from panel collections will be assigned to a variable
            //  var content = args.get_panelsUpdating()[0].innerHTML;
            // alert(args.get_panelsUpdating()[0].id);

            //display number of panels is getting deleted during this post back
            // alert(args.get_panelsDeleting().length)
        }
        function EndRequest(sender, args) {
            //do your stuff
            var s = sender;
            var a = args;
            var msg = null;
            if (a._error != null) {
                switch (args._error.name) {
                    case "Sys.WebForms.PageRequestManagerServerErrorException":
                        msg = "PageRequestManagerServerErrorException";
                        break;
                    case "Sys.WebForms.PageRequestManagerParserErrorException":
                        msg = "PageRequestManagerParserErrorException";
                        break;
                    case "Sys.WebForms.PageRequestManagerTimeoutException":
                        msg = "PageRequestManagerTimeoutException";
                        break;
                }
                args._error.message = "My Custom Error Message " + msg;
                args.set_errorHandled(true);
            }
        }
        var hrefTreeNodeText = null;
        function OverOnDeleteButton(control) {
            hrefTreeNodeText = control.parentNode.parentNode.getAttribute("href");
            control.parentNode.parentNode.setAttribute("href", "javascript:");
        }
        function OverOutOnDeleteButton(control) {
            // if (hrefTreeNodeText) {
            //      control.parentNode.parentNode.setAttribute("href", hrefTreeNodeText);
            //  }
        }

        try {
            ASPxClientControl.GetControlCollection().ControlsInitialized.AddHandler(function (s, e) {
                UpdateGridHeight();
            });
            ASPxClientControl.GetControlCollection().BrowserWindowResized.AddHandler(function (s, e) {
                UpdateGridHeight();
            });
        } catch (ex) {
            console.log(ex.message);
        }



        function UpdateGridHeight() {
            try {
                spGridConstraintList.SetHeight(0);
                var containerHeight = ASPxClientUtils.GetDocumentClientHeight();
                if (document.body.scrollHeight > containerHeight)
                    containerHeight = document.body.scrollHeight;
                spGridConstraintList.SetHeight(containerHeight);
            } catch (ex) {
                console.log(ex.message);
            }
        }

        window.addEventListener('resize', function (evt) {
            if (!ASPxClientUtils.androidPlatform)
                return;
            var activeElement = document.activeElement;
            if (activeElement && (activeElement.tagName === "INPUT" || activeElement.tagName === "TEXTAREA") && activeElement.scrollIntoViewIfNeeded)
                window.setTimeout(function () { activeElement.scrollIntoViewIfNeeded(); }, 0);
        });


    </script>
    <script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
        function updategridheight() {
            try {
                treelistrt.setheight(0);
                var containerheight = aspxclientutils.getdocumentclientheight();
                if (document.body.scrollheight > containerheight)
                    containerheight = document.body.scrollheight;
                treelistrt.setheight(containerheight);
            } catch (e) {
            }
        }
        window.addEventListener('resize', function (evt) {
            try {
                if (!ASPxClientUtils.androidPlatform)
                    return;
                var activeElement = document.activeElement;
                if (activeElement && (activeElement.tagName === "INPUT" || activeElement.tagName === "TEXTAREA") && activeElement.scrollIntoViewIfNeeded)
                    window.setTimeout(function () { activeElement.scrollIntoViewIfNeeded(); }, 0);
            } catch (e) {
            }
        });
    </script>
    <div class="col-md-12 col-sm-12 col-xs-12 noPadding">
        <div class="row" style="padding-top: 5px">
            <%--   <ugit:ASPxGridView ID="treeList" ClientInstanceName="treeList" CssClass="customgridview homeGrid" runat="server" KeyFieldName="ID"
            Width="100%" OnHtmlDataCellPrepared="treeList_HtmlDataCellPrepared">
            <settingsadaptivity adaptivitymode="HideDataCells" allowonlyoneadaptivedetailexpanded="true" ></settingsadaptivity>
            <Columns>
                <dx:GridViewDataColumn FieldName="TicketID" HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="Center" 
                    Caption="Ticket" VisibleIndex="0">
                    <DataItemTemplate>
                        <a id="aTicket" runat="server" href='<%# Eval("NavigateURL")%>'><%# Eval("TicketID")%></a>
                    </DataItemTemplate>
                </dx:GridViewDataColumn>
                <dx:GridViewDataColumn FieldName="NavigateURL" Visible="false" />
                <dx:GridViewDataColumn FieldName="Title" CellStyle-Wrap="True" HeaderStyle-Font-Bold="true" HeaderStyle-HorizontalAlign="Left" 
                    CellStyle-HorizontalAlign="Left" />
                <dx:GridViewDataColumn FieldName="CreationDate" Caption="Created" HeaderStyle-Font-Bold="true" HeaderStyle-HorizontalAlign="Center" 
                    CellStyle-HorizontalAlign="Center" />
                <dx:GridViewDataColumn FieldName="TicketType" Visible="false" />
                <dx:GridViewDataColumn FieldName="PriorityLookup" Caption="Priority" HeaderStyle-Font-Bold="true" HeaderStyle-HorizontalAlign="Center" 
                    CellStyle-HorizontalAlign="Center" />
                <dx:GridViewDataColumn FieldName="ModuleStepLookup" Caption="Status" HeaderStyle-Font-Bold="true" HeaderStyle-HorizontalAlign="Center" 
                    CellStyle-HorizontalAlign="Center" />
                <dx:GridViewDataColumn FieldName="ID" Visible="false" />
                <dx:GridViewDataColumn Caption=" " FieldName="TicketID">
                    <DataItemTemplate>
                        <dx:ASPxButton ID="btnDelete" runat="server" RenderMode="Link" OnClick="BtTicketDelete_Click" 
                            CommandArgument='<%#Eval("TicketID") %>' ToolTip="Delete">
                            <Image Url="/Content/images/grayDelete.png" Width="16"></Image>
                        </dx:ASPxButton>
                    </DataItemTemplate>
                </dx:GridViewDataColumn>
            </Columns>
                <settingscommandbutton>
                <ShowAdaptiveDetailButton ButtonType="Button"   Styles-Style-CssClass="homeGrid_openBTn"></ShowAdaptiveDetailButton>
                <HideAdaptiveDetailButton ButtonType="Button"  Styles-Style-CssClass="homeGrid_closeBTn"></HideAdaptiveDetailButton>
            </settingscommandbutton>
            <Settings ShowColumnHeaders="true" GridLines="Both" />
            <Styles>
                <Row CssClass="homeGrid_dataRow"></Row>
                <Header Font-Bold="true" CssClass="homeGrid_headerColumn"></Header>
                <GroupRow CssClass="homeGrid-groupRow"></GroupRow>
            </Styles>
        </ugit:ASPxGridView>--%>

            <dx:ASPxTreeList ID="treeListRT" ClientInstanceName="treeListRT" MinHeight="110px" runat="server" AutoGenerateColumns="False" OnHtmlDataCellPrepared="treeListRT_HtmlDataCellPrepared"
                KeyFieldName="ID" ParentFieldName="ParentId" Border-BorderStyle="Solid" Width="100%">
                <Columns>
                    <dx:TreeListDataColumn FieldName="TicketID" HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="Left" Width="100px" Caption="Item" VisibleIndex="0" HeaderStyle-Font-Bold="true">
                        <DataCellTemplate>
                            <a id="aTicket" runat="server" href='<%# Eval("NavigateURL")%>'><%# Eval("TicketID")%></a>
                        </DataCellTemplate>
                    </dx:TreeListDataColumn>
                    <dx:TreeListDataColumn FieldName="NavigateURL" Visible="false" />
                    <dx:TreeListDataColumn FieldName="Title" CellStyle-Wrap="True" Width="50%" HeaderStyle-Font-Bold="true" HeaderStyle-HorizontalAlign="Left" CellStyle-HorizontalAlign="Left">
                        <DataCellTemplate>
                            <a id="aTitle" runat="server" href='<%# Eval("NavigateURL")%>'><%# Eval("Title")%></a>
                        </DataCellTemplate>
                    </dx:TreeListDataColumn>
                    <dx:TreeListDataColumn FieldName="CreationDate" Caption="Created" HeaderStyle-Font-Bold="true" HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="Center" />
                    <dx:TreeListDataColumn FieldName="PriorityLookup" Caption="Priority" HeaderStyle-Font-Bold="true" HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="Center" />
                    <dx:TreeListDataColumn FieldName="ModuleStepLookup" Caption="Status" HeaderStyle-Font-Bold="true" HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="Center" />
                    <dx:TreeListDataColumn FieldName="ID" Visible="false" />
                    <dx:TreeListDataColumn Caption=" " Name="Delete" FieldName="TicketID" Width="20px">
                        <DataCellTemplate>
                            <span style='display: <%# ((Eval("ParentId").ToString() == "2") || Eval("ParentId").ToString() == "3") ? "" : "none" %>'>
                                <dx:ASPxButton ID="btnDelete" runat="server" RenderMode="Link" OnClick="BtTicketDelete_Click" CommandName='<%#Eval("ParentId") %>'
                                    CommandArgument='<%#Eval("TicketID") %>'  ToolTip="Delete">
                                    <Image Url="/Content/images/grayDelete.png" Width="16"></Image>
                                </dx:ASPxButton>
                            </span>

                        </DataCellTemplate>
                    </dx:TreeListDataColumn>
                </Columns>
                <Styles>
                    <AlternatingNode Enabled="true" CssClass="ugitlight1lightest" />
                </Styles>
                <Settings ShowTreeLines="false" ShowColumnHeaders="true" GridLines="Both" SuppressOuterGridLines="false" />
                <SettingsBehavior ExpandCollapseAction="NodeClick" AllowSort="true" />
                <SettingsEditing AllowNodeDragDrop="false" />
            </dx:ASPxTreeList>

            <script type="text/javascript">
                try {
                    ASPxClientControl.GetControlCollection().ControlsInitialized.AddHandler(function (s, e) {
                        UpdateGridHeight();
                    });
                    ASPxClientControl.GetControlCollection().BrowserWindowResized.AddHandler(function (s, e) {
                        UpdateGridHeight();
                    });
                } catch (ex) {
                    console.log(ex.message);
                }
            </script>
        </div>
        <div class="row" id="dvAddNewRelation" runat="server" style="max-height:150px">
            <a id="aAddItem" runat="server" href="">
                    <img id="Img1" runat="server" src="/Content/Images/add_icon.png" />
                    <asp:Label ID="LblAddItem" runat="server" Text="Relate To Existing Item"></asp:Label>
            </a>
            <a id="aAddNewSubTicket" runat="server" style="padding-left: 10px" href="javascript:;">
                    <img id="Img2" runat="server" src="/Content/Images/add_icon.png" />
                    <asp:Label ID="Label1" runat="server" Text="Add New Sub Item"></asp:Label>
                </a>
        </div>
    </div>

    <div style="visibility: hidden;">
        <asp:Button ID="btTicketDelete" runat="server" OnClick="BtTicketDelete_Click" />
    </div>
    <asp:HiddenField ID="hfTicketDelete" runat="server" />
</asp:Panel>
<div id="dvModule" runat="server" class="col-md-12 col-sm-12 col-xs-12" visible="false">
    <div class="ms-formtable accomp-popup">
        <div class="row">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Select Module</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:DropDownList ID="ddlModuleDetail" AutoPostBack="false" CssClass="itsmDropDownList aspxDropDownList" runat="server">
                </asp:DropDownList>
            </div>
        </div>
        <div class="row addEditPopup-btnWrap">
            <dx:ASPxButton ID="UpdateButton" runat="server" Text="Create" AutoPostBack="false" CssClass="primary-blueBtn">
                <ClientSideEvents Click="UpdateButton_Click" />
            </dx:ASPxButton>
        </div>
    </div>
</div>
<script type="text/javascript" id="dxss_inlineCtrScriptCTR1" data-v="<%=UGITUtility.AssemblyVersion %>">
    function ComfrimDelete() {
        return confirm("Are you sure you want to delete this sub-ticket relationship?");
    }
</script>

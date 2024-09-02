<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AssetRelatedWithAssets.ascx.cs" Inherits="uGovernIT.Web.AssetRelatedWithAssets" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dx" %>

<%@ Import Namespace="uGovernIT.Manager" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script type="text/javascript" id="dxss_inlineCtrScriptCTR3" data-v="<%=UGITUtility.AssemblyVersion %>">
    $(function () {

        //addHeightToCalculateFrameHeight(this, $(parent.frames[0]).height());
        //$('#s4-workspace').css('height', $(parent.frames[0]).height() + 'px');
        var ua = window.navigator.userAgent;
        var msie = ua.indexOf("MSIE ");
        if (msie > 0)      // checking for ie
        {
            if ($(parent.frames[0]).height() - $('#s4-workspace').height() > 20) {
                if (typeof (treeList) != "undefined" && treeList.GetAllDataRows().length > 1) {

                    window.location.reload();
                }
            }
        }
    });

    function UpdateButton_Click(s, e) {
            //This line will load the DOM of dropdown 
        var cbomodule = document.getElementById("<%=ddlModuleDetail.ClientID%>");
        //This will return the selected option
        var selectedOption = cbomodule.options[cbomodule.selectedIndex];
        //This will give you the value of the attribut
        var ticketid = "<%=TicketId%>";
        var url = selectedOption.attributes.Url.value + "?ParentId=" + ticketid + "&NewSubticket=true&SourceTicketId=" + ticketid;

        window.parent.CloseWindowCallback(0, document.location.href);
        window.parent.UgitOpenPopupDialog(url,'','New '+ ticketid + ' Ticket','90','90','/layouts/ugovernit/delegatecontrol.aspx', false);
        }
</script>
<asp:Panel ID="pageDetailPanel" runat="server">
    <style type="text/css">
        .pointer {
            cursor: pointer;
        }

        .deleteformulaspan {
            display: inline-block;
            height: 15px;
            overflow: hidden;
            position: relative;
            width: 16px;
        }

        .deleteformula {
            border-width: 0;
            left: 0 !important;
            position: absolute;
            top: -139px !important;
        }

        .dnode {
            display: none;
        }

        .nodetext {
            position: relative;
            top: -3px;
        }

        .deleteimg {
            position: relative;
            top: 3px;
            border-width: 0px;
        }

        .hiderootnode {
            display: none;
        }
    </style>
    <script type="text/javascript" id="dxss_inlineCtrScriptAssestRelated" data-v="<%=UGITUtility.AssemblyVersion %>">
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

    <%--<dx:ASPxTreeList ID="treeList" ClientInstanceName="treeList" MinHeight="110px" runat="server" AutoGenerateColumns="False"
                                                KeyFieldName="ID" ParentFieldName="ParentId" Border-BorderStyle="Solid" Width="100%" OnCommandColumnButtonInitialize="treeList_CommandColumnButtonInitialize"  OnHtmlDataCellPrepared="treeList_HtmlDataCellPrepared">
                                                <Styles>
                                                    <AlternatingNode Enabled="true" CssClass="ugitlight1lightest" />
                                                </Styles>
                                                <SettingsText ConfirmDelete="Are you sure you want to delete the relationship with this asset?" />
                                                <Settings ShowTreeLines="false" ShowColumnHeaders="true" GridLines="Both" SuppressOuterGridLines="false" />
                                                <SettingsBehavior ExpandCollapseAction="NodeClick" AllowSort="false" />
                                                <SettingsEditing AllowNodeDragDrop="false" />
                                                
                                            </dx:ASPxTreeList>--%>
    <ugit:ASPxGridView ID="treeList" runat="server" CssClass="customgridview homeGrid" ParentFieldName="ParentId" AutoGenerateColumns="false" KeyFieldName="ID" Width="100%" OnHtmlDataCellPrepared="treeList_HtmlDataCellPrepared">
        <settingsadaptivity adaptivitymode="HideDataCells" allowonlyoneadaptivedetailexpanded="true"  ></settingsadaptivity>
        <Columns>
            <%--<dx:GridViewDataColumn FieldName="TicketID" Caption="Assest ID" HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="Center" Width="100px"   VisibleIndex="0" HeaderStyle-Font-Bold="true">
                <DataItemTemplate>
                    <a id="aTicket" runat="server" href='<%# Eval("NavigateURL")%>'><%# Eval("TicketID")%></a>
                </DataItemTemplate>
            </dx:GridViewDataColumn>--%>
            <%--<dx:GridViewDataColumn FieldName="NavigateURL" Visible="false" />
            <dx:GridViewDataColumn FieldName="Title" CellStyle-Wrap="True" Width="70%" HeaderStyle-Font-Bold="true" HeaderStyle-HorizontalAlign="Left" CellStyle-HorizontalAlign="Left" />
            <dx:GridViewDataColumn FieldName="CreationDate" Caption="Created" HeaderStyle-Font-Bold="true" HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="Center" />--%>
            <dx:GridViewDataColumn FieldName="TicketType" Visible="false" SortOrder="Descending"  />
            
            <%--<dx:GridViewDataColumn FieldName="PriorityLookup" Caption="Priority" HeaderStyle-Font-Bold="true" HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="Center" />
            <dx:GridViewDataColumn FieldName="ModuleStepLookup" Caption="Status" HeaderStyle-Font-Bold="true" HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="Center" />
            <dx:GridViewDataColumn FieldName="ParentId" Visible="false"  />--%>
            <dx:GridViewDataColumn Caption=" " FieldName="TicketID" Width="20px" VisibleIndex="10">
                
                <DataItemTemplate>
                    <dx:ASPxButton ID="btnDelete" runat="server" RenderMode="Link" OnClick="BtTicketDelete_Click" CommandArgument='<%#Eval("TicketID") %>' ToolTip="Delete">
                        <Image Url="/Content/images/delete-icon-new.png"></Image>
                          <ClientSideEvents Click="function(s, e) { e.processOnServer = confirm('Are you sure you want to delete the relationship with this asset?');}" />

                    </dx:ASPxButton>
                   
                </DataItemTemplate>
                
            </dx:GridViewDataColumn>
           
        </Columns>

        <SettingsBehavior AllowSort="false"  />  

        <settingscommandbutton >
<ShowAdaptiveDetailButton ButtonType="Button" ></ShowAdaptiveDetailButton>
<HideAdaptiveDetailButton ButtonType="Button"></HideAdaptiveDetailButton>
</settingscommandbutton>
        <Settings ShowColumnHeaders="true" GridLines="Both" />
        <SettingsBehavior  AllowSort="false" />
    </ugit:ASPxGridView>
    
   <a id="aAddItem" class="" runat="server" >
                <div class="btn-holder svc_editTicket_link">
                    <div class="add-link-btn">
                        <img src="/Content/Images/add_icon.png" />
                    </div>
                    <div class="link-lable"><span>Add Child Asset</span></div>
                </div>
            </a>

     
   
    <div style="visibility: hidden;">
        <asp:Button ID="btTicketDelete" runat="server" OnClick="BtTicketDelete_Click" />
    </div>
    <asp:HiddenField ID="hfTicketDelete" runat="server"/>
</asp:Panel>
<div id="dvModule" runat="server" class="svc_addSubTask_dropDown_wrap" visible="false"> 
    <div>
        <div class="row">
            <label class="relatedTicket_dropDown_label svc_addSubTask_dropDown_lable">Select Module</label>
            <div class="svc_addSubTask_dropDown">
                <asp:DropDownList  ID="ddlModuleDetail" AutoPostBack="false" CssClass="itsmDropDownList aspxDropDownList" runat="server">
                </asp:DropDownList>
                
            </div>
        </div>

        <div class=" row svc_subtask_btn_wrap">
            <div style="padding: 5px;">
                <dx:ASPxButton ID="UpdateButton" runat="server" Text="Create" AutoPostBack="false"  cssClass="primary-blueBtn">
                   <ClientSideEvents Click="UpdateButton_Click" />
                </dx:ASPxButton>
            </div>
        </div>
    </div>
</div>
<script type="text/javascript" id="dxss_inlineCtrScriptCTR1" data-v="<%=UGITUtility.AssemblyVersion %>">
        function ComfrimDelete(control, ticketId) {
            if (ticketId && ticketId != "") {
                var btDelete = document.getElementById("<%=btTicketDelete.ClientID %>");
                var hfTicketDelete = document.getElementById("<%=hfTicketDelete.ClientID %>");
                hfTicketDelete.value = ticketId;
                if (confirm("Are you sure you want to delete this sub-ticket relationship?")) {
                //$('#<%=btTicketDelete.ClientID %>').click(); 
                    $('input[id*=btTicketDelete]').click();
                    alert($('input[id*=btTicketDelete]').attr("id") + ' click');
                    //btDelete.click();
                }
                else {
                    hfTicketDelete.value = "";
                }
            }
        }
</script>
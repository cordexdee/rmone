<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomTicketRelationShip.ascx.cs" Inherits="uGovernIT.Web.CustomTicketRelationShip" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Manager" %>
<%@ Import Namespace="System.Data" %>

<script type="text/javascript">
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
    <script type="text/javascript"> 
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
    </script>
    <script type="text/javascript">
        try {
            ASPxClientControl.GetControlCollection().ControlsInitialized.AddHandler(function (s, e) {
                UpdateGridHeight();
            });
            ASPxClientControl.GetControlCollection().BrowserWindowResized.AddHandler(function (s, e) {
                UpdateGridHeight();
            });
        } catch (e) {
        }        
</script>
    <script type="text/javascript">
        function UpdateGridHeight() {
            spGridConstraintList.SetHeight(0);
            var containerHeight = ASPxClientUtils.GetDocumentClientHeight();
            if (document.body.scrollHeight > containerHeight)
                containerHeight = document.body.scrollHeight;
            spGridConstraintList.SetHeight(containerHeight);
        }
        window.addEventListener('resize', function (evt) {
            if (!ASPxClientUtils.androidPlatform)
                return;
            var activeElement = document.activeElement;
            if (activeElement && (activeElement.tagName === "INPUT" || activeElement.tagName === "TEXTAREA") && activeElement.scrollIntoViewIfNeeded)
                window.setTimeout(function () { activeElement.scrollIntoViewIfNeeded(); }, 0);
        });
</script>
    <%--   <table cellpadding="0" cellspacing="0" border="0"  style="width:100%;" >      
        <tr>
            <td >--%>
    <ugit:ASPxGridView ID="treeList" runat="server" Theme="DevEx" KeyFieldName="ID" Width="100%" OnHtmlDataCellPrepared="treeList_HtmlDataCellPrepared" CssClass="homeGrid">
        <settingsadaptivity adaptivitymode="HideDataCells" allowonlyoneadaptivedetailexpanded="true" ></settingsadaptivity>
        <Columns>
            <dx:GridViewDataColumn FieldName="TicketID" HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="Center" Caption="Ticket" VisibleIndex="0" HeaderStyle-Font-Bold="true">
                <DataItemTemplate>
                    <a id="aTicket" runat="server" href='<%# Eval("NavigateURL")%>'><%# Eval("TicketID")%></a>
                </DataItemTemplate>
            </dx:GridViewDataColumn>
            <dx:GridViewDataColumn FieldName="NavigateURL" Visible="false" />
            <dx:GridViewDataColumn FieldName="Title" CellStyle-Wrap="True" HeaderStyle-Font-Bold="true" HeaderStyle-HorizontalAlign="Left" CellStyle-HorizontalAlign="Left" />
            <dx:GridViewDataColumn FieldName="CreationDate" Caption="Created" HeaderStyle-Font-Bold="true" HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="Center" />
            <dx:GridViewDataColumn FieldName="TicketType" Visible="false" />
            <dx:GridViewDataColumn FieldName="PriorityLookup" Caption="Priority" HeaderStyle-Font-Bold="true" HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="Center" />
            <dx:GridViewDataColumn FieldName="ModuleStepLookup" Caption="Status" HeaderStyle-Font-Bold="true" HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="Center" />
            <dx:GridViewDataColumn FieldName="ID" Visible="false" />
            <dx:GridViewDataColumn Caption=" " FieldName="TicketID">
                <DataItemTemplate>
                    <dx:ASPxButton ID="btnDelete" runat="server" RenderMode="Link" OnClick="BtTicketDelete_Click" CommandArgument='<%#Eval("TicketID") %>' ToolTip="Delete">
                        <Image Url="/Content/images/redNew_delete.png"></Image>  
                        <ClientSideEvents Click="function(s, e){ e.processOnServer = confirm('Do you want to Delete?');  }" />
                    </dx:ASPxButton>
                    <%-- <span onmousedown='OverOnDeleteButton(this)' onmouseout='OverOutOnDeleteButton(this)' style='display: <%# (Eval("ParentId").ToString() == "2") ? "" : "none" %>'>
                                                                <input type='image' class='deleteimg' alt='*' src='/Content/images/TrashIcon.png' 
                                                                    onclick='<%# string.Format("event.cancelBubble = true; ComfrimDelete(this, \"{0}\");", Eval("TicketID")) %>'  />
                                                            </span>--%>
                </DataItemTemplate>
            </dx:GridViewDataColumn>
           
        </Columns>
        <settingscommandbutton>
            <ShowAdaptiveDetailButton ButtonType="Button" Styles-Style-CssClass="homeGrid_openBTn"></ShowAdaptiveDetailButton>
            <HideAdaptiveDetailButton ButtonType="Button" Styles-Style-CssClass="homeGrid_closeBTn"></HideAdaptiveDetailButton>
        </settingscommandbutton>
        <Settings ShowColumnHeaders="true" GridLines="Both" />
        <Styles>
            <Row HorizontalAlign="Center" CssClass="CRMstatusGrid_row"></Row>
            <Header Font-Bold="true" HorizontalAlign="Center" CssClass="CRMstatusGrid_headerRow"></Header>
        </Styles>
    </ugit:ASPxGridView>
    <%-- </td>
        </tr>
        <tr >--%>
    <%-- <td style="padding-top: 16px;">--%>
        <div class="crmLink_wrap">
            <a id="aAddItem" class="" runat="server" >
                <div class="btn-holder svc_editTicket_link">
                    <div class="add-link-btn">
                        <img src="/Content/Images/related_to.png" />
                    </div>
                    <div class="link-lable"><span>Relate To Existing Item</span></div>
                </div>
            </a>
        
            <a id="aAddNewSubTicket" class="" runat="server"  href="javascript:;">
                <div class="btn-holder svc_editTicket_link">
                    <div class="add-link-btn">
                        <img src="/Content/Images/sub_Task.png" />
                    </div>
                    <div class="link-lable"><span>Add New Sub Item</span></div>
                </div>
            </a>
        </div>
     
    <%--   </td>
        </tr>
    </table>--%>
    <div style="visibility: hidden;">
        <asp:Button ID="btTicketDelete" runat="server" OnClick="BtTicketDelete_Click" />
    </div>
    <asp:HiddenField ID="hfTicketDelete" runat="server"/>
</asp:Panel>
<div id="dvModule" runat="server" class="svc_addSubTask_dropDown_wrap" visible="false"> 
    <div>
        <div class="row">
            <label class="relatedTicket_dropDown_label svc_addSubTask_dropDown_lable">Select Module</label>
            <div class="svc_addSubTask_dropDown" align="center">
                <asp:DropDownList Height="22px" ID="ddlModuleDetail" AutoPostBack="false" runat="server" CssClass="aspxDropDownList"></asp:DropDownList>
            </div>
        </div>

        <div class="svc_subtask_btn_wrap row">
            <dx:ASPxButton ID="UpdateButton" runat="server" Text="Create" AutoPostBack="false" OnClick="UpdateButton_Click" cssClass="svc_subTask_btn"/>
        </div>
    </div>
</div>
<script type="text/javascript">
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


<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LifeCycleView.ascx.cs" Inherits="uGovernIT.Web.LifeCycleView" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function DeleteCongigVariable() {
        if (confirm('Are you sure want to delete?')) {
            <%=Page.ClientScript.GetPostBackEventReference(lnkDelete1,string.Empty)%>
         }
    }
    function DeleteCongigVariableArc() {
        if (confirm('Are you sure want to delete?')) {
            <%=Page.ClientScript.GetPostBackEventReference(LinkButton1,string.Empty)%>
         }
    }
   
    function ChangeLifeCycle(obj)
    {
       
        var divObj = $(obj);
        var linkBtn = $(divObj.find(".btlifecycle-name")).text();
        var hdnBtnId = $(".btnHdnLifeCycle").get(0).id;
        $("#<%=hdnSelectedLifeCycle.ClientID%>").val(encodeURI(linkBtn))
        $("#" + hdnBtnId).click();
        return false;

    }
    function checkBeforeDelete() {
        <% if (lifeCycleInUse)
           {%>
        alert("You cannot delete this Lifecycle because it is being used in some projects.");
        return false;
        <% }
           else
           {%>
        return confirm('Are you sure you want to delete selected lifecycle?');
        <% } %>
        return false;
    }
</script>

<div class="col-md-12 col-sm-12 col-xs-12 noPadding configVariable-popupWrap">
    <div class="row">
        <div class="col-md-3 col-sm-3 col-xs-12">
            <asp:Panel ID="pLifeCycle" runat="server" CssClass="lifecycle-pane">
                <div class="row" style="margin:15px 0;">
                    <div class="crm-checkWrap">
                        <asp:CheckBox ID="chkShowDelete" runat="server" AutoPostBack="true" Text="Show Archived" TextAlign="right" OnCheckedChanged="chkShowDelete_CheckedChanged" />
                    </div>
                </div>
                <div class="lifecycle-container row">
                    <div class="col-md-12 col-sm-12 col-xs-12 noPadding btlc-div">
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 25px">
                                    <asp:LinkButton ID="btOrder" runat="server" Text='#' CssClass="btlifecycle-header"></asp:LinkButton>
                                </td>
                                <td>
                                    <asp:LinkButton ID="btLifeCycle" runat="server" Text='Project Lifecycle' CssClass="btlifecycle-header"></asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </div>
                     <asp:Button ID="btnHdnLifeCycle" CssClass="btnHdnLifeCycle" runat="server" style="display:none" OnClick="btLifeCycle_Click" />
                    <asp:HiddenField ID="hdnSelectedLifeCycle" runat="server" />
                    <asp:Repeater ID="rLifeCycle" runat="server" OnItemDataBound="rLifeCycle_ItemDataBound">
                        <ItemTemplate>
                            <table class="lifecycle-stages" style="width: 100%;border-collapse:collapse" >
                                <tr id="trLifeCycle" runat="server" style="background: #F7FAFA;" onclick="ChangeLifeCycle(this)">
                                    <td style="width: 25px">
                                        <div class="fullwidth btlc-div" >
                                            <asp:Label ID="lbItemOrder" runat="server" Text='<%# Eval("ItemOrder") %>' CssClass="btlifecycle-link"></asp:Label>
                                        </div>
                                    </td>
                                    <td>
                                       
                                        <div class="fullwidth btlc-div" >
                                            <asp:LinkButton ID="btLifeCycle" runat="server" Text='<%# Eval("Name") %>' CssClass="btlifecycle-link btlifecycle-name" ></asp:LinkButton>
                                        </div>
                                    </td>

                                </tr>
                            </table>
                        </ItemTemplate>
                    </asp:Repeater>

                    <div class="col-md-12 col-sm-12 col-xs-12 noPadding">
                        <asp:HyperLink ID="btNewLifecycle" runat="server" Text="New" CssClass="action-align aspHyperLink" NavigateUrl="javascript:newTask(0)">
                           
                        </asp:HyperLink>

                        <asp:HyperLink ID="btEditLifeCycle" runat="server" Text="Edit" CssClass="action-align aspHyperLink" NavigateUrl="javascript:newTask(0)">
                        </asp:HyperLink>
                    </div>
                </div>
            </asp:Panel>
        </div>
        <div class="col-md-9 col-sm-9 col-xs-12">
            <asp:Panel ID="pLifeCycleStage" runat="server" CssClass="lifecyclestage-pane">
                <asp:Panel ID="lcsgraphics" runat="server" CssClass="lcsgraphics">
                </asp:Panel>
                <asp:Panel ID="lcsDetail" runat="server" CssClass="lcsDetail">
                    <div style="width: 100%; float: left; color: red;">
                        <asp:Label ID="lbMsg" runat="server"></asp:Label>
                    </div>
                    <div class="row">
                        <ugit:ASPxGridView ID="_grid" runat="server" Width="100%" EnableViewState="false" AlternatingRowStyle-BackColor="WhiteSmoke" 
                                OnHtmlRowCreated="_grid_HtmlRowCreated" CssClass="customgridview homeGrid"
                                HeaderStyle-Height="10px" HeaderStyle-CssClass="aa" KeyFieldName="ID" AutoGenerateColumns="false">
                                <Columns>
                                    <dx:GridViewDataColumn>
                                        <DataItemTemplate>
                                            <a id="aEdit" runat="server" href="">
                                                <img id="Imgedit" runat="server" style="width:16px" src="/Content/images/editNewIcon.png" />
                                            </a>
                                        </DataItemTemplate>
                                    </dx:GridViewDataColumn>
                                    <dx:GridViewDataColumn FieldName="StageStep" Name="Stage #" Caption="Stage #"></dx:GridViewDataColumn>
                                    <dx:GridViewDataColumn Name="Stage Title" Caption="Stage Title">
                                        <DataItemTemplate>
                                            <a id="lnkStage" runat="server" href=""><%# Eval("Name") %></a>
                                        </DataItemTemplate>
                                    </dx:GridViewDataColumn>
                                    <dx:GridViewDataColumn FieldName="StageWeight" Name="Weight"></dx:GridViewDataColumn>
                                    <dx:GridViewDataColumn FieldName="Description" Name="Description"></dx:GridViewDataColumn>
                                </Columns>
                                <Styles>
                                    <Row CssClass="homeGrid_dataRow"></Row>
                                    <Header CssClass="homeGrid_headerColumn"></Header>
                                </Styles>
                            </ugit:ASPxGridView>
                    </div>
                </asp:Panel>
            </asp:Panel>
        </div>
    </div>
    <div class="row addEditPopup-btnWrap">
        <div class="col-md-12 col-sm-12 col-xs-12">
            <dx:ASPxButton ID="btnArchive" runat="server" Visible="true" Text="Delete" ToolTip="Delete" AutoPostBack="false" CssClass="secondary-cancelBtn">
                <ClientSideEvents Click="function(s,e){DeleteCongigVariableArc();}" />
            </dx:ASPxButton>
            <asp:LinkButton ID="LinkButton1" runat="server" OnClick="btnArchive_Click"></asp:LinkButton>
            <dx:ASPxButton ID="btnRemoveArchive" runat="server" Visible="true" Text="Delete" ToolTip="Delete" AutoPostBack="false" CssClass="secondary-cancelBtn">
                <ClientSideEvents Click="function(s,e){DeleteCongigVariable();}" />
            </dx:ASPxButton>
            <asp:LinkButton ID="lnkDelete1" runat="server" OnClick="btnRemoveArchive_Click"></asp:LinkButton>
            <asp:LinkButton ID="btnDelete" CssClass="action-align-del" Visible="false" runat="server" Text="&nbsp;&nbsp;Delete&nbsp;&nbsp;" ToolTip="Delete"
                OnClientClick="return checkBeforeDelete()" OnClick="btnDelete_Click">
                <span class="button-bg">
                    <b style="float: left; font-weight: normal;">
                        Delete Lifecycle</b>
                    <i style="float: left; position: relative; top: -3px;left:2px">
                        <img src="/Content/ButtonImages/cancel.png"  style="border:none;" title="" alt=""/>
                    </i> 
                </span>
            </asp:LinkButton>
            <a id="btNewStage" runat="server" href="javascript:newTask(0)" style="padding-left:15px" class="primary-btn-link">
                <img id="Img3" runat="server" src="/Content/Images/plus-symbol.png" />
                <asp:Label ID="Label2" runat="server" Text="Add New Stage"></asp:Label>
            </a>
        </div>
    </div>
</div>

<table style="width: 97%;" cellspacing="0" cellpadding="0">
    <tr>
        <td valign="top" style="width: 200px;" class="lc-td">
            
        </td>
        <td valign="top">
            
        </td>
    </tr>

    <tr>
        <td></td>
        <td>
            <div class="fullwidth">
                
               
               <%-- <asp:HyperLink ID="btNewStage" runat="server" Text="Add New Stage" CssClass="action-align" NavigateUrl="javascript:newTask(0)">
                    <span class="button-bg">
                        <b style="float: left; font-weight: normal;">New Stage</b>
                        <i style="float: left; position: relative; top: 1px;left:2px">
                            <img src="/Content/images/add_icon.png"  style="border:none;" title="" alt=""/>
                        </i> 
                    </span>
                </asp:HyperLink>--%>
            </div>
        </td>
    </tr>
</table>

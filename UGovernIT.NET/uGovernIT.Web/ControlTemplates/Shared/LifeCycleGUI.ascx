
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LifeCycleGUI.ascx.cs" Inherits="uGovernIT.Web.LifeCycleGUI" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .lifecyclestage-pane .stage-titlecontainer.stageTop {
	    top: 38px !important;
    }
    .lifecyclestage-pane .stage-titlecontainer {
	    right: -58px;
    }
</style>

<script type="text/javascript">
    $(function () {
        try {
            //If stage lable height is less then 20then change top position of label
            $(".alternategraphiclabel").each(function (i, item) {
                var label = $.trim($(item).find("b").html());
                if ($(item).find("b").height() < 20) {
                    $(item).css("top", "-18px");
                }
            });
        } catch (ex) {
        }
    });
</script>

<div class="contract_steps_module workflowGraphicContainer111 adminWorkFlow-container" runat="server" id="topGraphicdiv">
    <div class="contract_steps_container">
        
        <div class="contract_steptop_content">
            <table style="text-align: center; border-collapse: collapse;" width="98%">
                <tr>
                    <td align="center">
                        <table style="text-align: center; border-collapse: collapse;">
                            <tr>
                                <asp:Repeater ID="stageRepeater" runat="server" OnItemDataBound="stageRepeater_ItemDataBound1">
                                    <ItemTemplate>
                                        <td id="tdStage" runat="server" class="draggable droppable" style="height: 38px; width: 36px; background-repeat: no-repeat;">
                                            <div style="position:relative">
                                                <span class="pos_rel"><i id="stageNo" class="lbStageNumber" runat="server">
                                                    <asp:Literal ID="lbStageNumber" runat="server"></asp:Literal>
                                                </i>
                                                </span>
                                                <i class="stage-titlecontainer stageTop" id="stageTitleContainer" runat="server">
                                                    <b class="pos_rel "
                                                        style="">
                                                        <asp:Literal ID="stageTitle" runat="server"></asp:Literal>
                                                    </b></i>
                                                <i id="activeStageArrow" runat="server"></i>
                                            </div>
                                        </td>
                                        <td id="tdStepLine" runat="server" class="droppable" style="height: 38px; background-repeat: repeat-x;">&nbsp;
                                        </td>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tr>
                        </table>

                    </td>
                </tr>
            </table>
        </div>
    </div>
</div>
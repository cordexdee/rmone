
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ServiceQuestionWorkFlow.ascx.cs" Inherits="uGovernIT.Web.ServiceQuestionWorkFlow" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .contract_steptop_content span i, .contract_steptop_content span.active i, .contract_steptop_content span.visited i {
    width: auto;
    font-style: normal;
    font-weight: bold;
    font-size: 18px;
    position: relative;
    top: 8px;
}
    .firstquestion {
        background-image:url('/Content/images/stepcirclebg_active_inprogress.gif');
    }
    .midquestion {
        background-image:url('/Content/images/stepcirclebg_active_inprogress.gif');
    }
    .lastquestion {
        background-image:url('/Content/images/stepcirclebg_active_inprogress.gif');
    }

        .firstquestion:hover,
        .midquestion:hover,
        .lastquestion:hover {
            background-color:#d2e6ec;
            cursor:pointer;
        }

    .lineSection{
        min-width:45px;
        background:black;
         /*position: absolute;*/
    /*top: 18px;
    left: 50%;*/
    z-index: -1;
    height: 5px;
    transition: all 0.5s ease;
    }


    .lineimg {
        background-image: url('/Content/images/stepline_active.gif');
        min-width:50px;
        
    }
    .lineimg1 {
        background-image: url('/Content/images/stepline_active.gif');
        min-width:32px;
        
    }
    .contract_steps_module {
        width: 100%;
        padding: 45px 20px 15px !important;
        border: 1px dashed black;
        margin-top: 40px;
    }
    .contract_steptop_content .stage-titlecontainer {
        font-size:smaller !important;
        width:50px;
        left:0px;
    }

    .skiplogicarrow {
        position: absolute;
        border-top: solid 2px #ff0000;
        border-left: solid 2px #ff0000;
        border-right: solid 2px #ff0000;
    }

    .conditionvararrow {
        position: absolute;
        border-top: dashed  2px #ff0000;
        border-left: dashed 2px #ff0000;
        /*border-right: solid 10px #d2e6ec;*/
    }

    .sectionjoin {
        background-repeat: repeat-x;
        background-position: center;
        min-width: 36px;
        width: 36px;
        height: 38px;
        float: left;
        margin-top: 40px;
        margin-left: -4px;
    }
    .conditiospan {
        position: absolute;
        background-color: #fff;
        top: -6px;
        left: 100px;
        z-index: 1;
        padding: 5px 5px;
        border: 1px #000 dashed;
        cursor:pointer;
    }
    .arrow-right {
        position: absolute;
        top: -6px;
        left: 100px;
        width: 0;
        height: 0;
        border-top: 5px solid transparent;
        border-bottom: 5px solid transparent;
        border-left: 10px solid #ff0000;
    }
    /*.arrow-down {
        position: absolute;
        width: 0;
        height: 0;
        border-bottom: 5px solid transparent;
        border-left: 5px solid transparent;
        border-top: 5px solid #ff0000;
    }*/


.arrow-down {
    position: absolute;
	width: 0; 
	height: 0; 
	border-left: 5px solid transparent;
	border-right: 5px solid transparent;
	border-top: 10px solid #f00;
}

.step_number {
    color: #939393; 
    font-weight: 600;
    width: 35px;
    height: 35px;
    margin: 0 auto;
    border-radius: 50%;
    background:#fff !important;
    border: 1px solid #9b9b9b;
    padding-top: 6px;
}
.wf-img-wrap.queWf-wrap{
    min-width: 115px;
}


</style>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    
    $(document).ready(function () {
       
        var jsonskipLogic = <%= jsonSkipLogic %>;
        var jsonquestions = <%= jsonQuestions %>;
        var jsonsections = <%=jsonSections%>;

        var skipconditioncount = jsonskipLogic.length;
        var topmargin = 50+(skipconditioncount*15);
        var diffbetween2condition = 15;
       // $(".contract_steps_module").css("margin-top",topmargin+"px");
       // $(".sectionjoin ").css("margin-top",59+(skipconditioncount*15)+"px");
        //$("#divWorkFlow").css("width",$("#tabPanel_2").width()-150+"px");
        var i = 0;
        if($("#tabPanel_2").css("display") != "none"){
            $(jsonskipLogic).each(function () {
                i++;
                var skipSectionCondition = this;
                var contidionvar = skipSectionCondition.ConditionVar;
                var sectionID ;
                var logicstr = skipSectionCondition.ConditionValForWF;
                if($(skipSectionCondition.Conditions).length>0)
                {
                    $(skipSectionCondition.Conditions).each(function() {
                        if(this.Variable!=null && this.Variable!='')
                        {
                            contidionvar = this.Variable;
                            logicstr =this.ValueForWF;
                        }
                    });
                }
                
                var lastsectionID;
                $(jsonsections).each(function(){
                    lastsectionID = this.ID;
                });
           
                if(skipSectionCondition.SkipSectionsID.length>0){
                    var sectionids = skipSectionCondition.SkipSectionsID;
                    var lastskipsectionid = sectionids[sectionids.length-1];
                    $(sectionids).each(function(){
                        var skipsectionid = this.toString();
                        var skipsection =  $(".section_" + skipsectionid)
                        var startskiplogicelement =  $($(skipsection).parent()).prev();

                        var left = $(startskiplogicelement).position().left + ($(startskiplogicelement).width()/2);
                        var top = $(startskiplogicelement).position().top + 28 + (skipconditioncount*15) -(i*diffbetween2condition);
                        var height = 76+i*diffbetween2condition;

                        var skipsectionobj = $(".section_" +skipsectionid);
                        var width = $(skipsectionobj).width() + $(startskiplogicelement).width()/2 - 10;
                        if($(skipsectionobj).parent().next().length>0)
                        {
                            width = $(skipsectionobj).width() + $(startskiplogicelement).width()/2 + $(skipsectionobj).parent().next().width()/2-2;
                            left = $(startskiplogicelement).position().left + ($(startskiplogicelement).width()/2)-2;
                        }
                        var skipSectionUniqueId = getUniqueId(skipSectionCondition.ID.toString(), skipsectionid.toString(), "skiplogicarrow");
                        var div = "<div id='"+skipSectionUniqueId+"' class='skiplogicarrow' style='width:" + width + "px; height:"+height+ "px; left: "+ left + "px; top: "+top+ "px;'></div>";
                    
                        $("table#main td:first").append(div);

                        if(skipsectionid.toString()== lastsectionID.toString())
                        {
                            var exitlabelleft = width-5;
                            var rightarrow = "<div class='arrow-right' style='left:"+exitlabelleft+"px !important;'></div>";
                            $("#"+skipSectionUniqueId).append(rightarrow);
                            $("#"+skipSectionUniqueId).css("border-right","none");
                        }
                        else{
                            var downarrowtop = height -10;
                            var downarrowleft = width-4;
                            var downarrow  = "<div class='arrow-down' style='top:"+downarrowtop+"px !important;left:"+downarrowleft+"px !important;'></div>";
                            $("#"+skipSectionUniqueId).append(downarrow);
                        }


                    });
                                
                    ///condition arrow
                    //var lastSkipSectionUniqueId = getUniqueId(skipSectionCondition.ID.toString(), lastskipsectionid.toString(), "skiplogicarrow");
                    //var con_left = $("."+contidionvar).position().left + 18;
                    //var con_top =  $("#"+lastSkipSectionUniqueId).position().top;
                    //var con_width = $("#"+lastSkipSectionUniqueId).position().left - $("."+contidionvar).position().left -13;
                    //var con_height =  $("#"+lastSkipSectionUniqueId).height() - 15;
                    //var conditionArrowUniqueId = "conditionvararrow"+skipSectionCondition.ID;
                    //var con_div = "<div id='"+conditionArrowUniqueId+"' class='conditionvararrow' onclick='ClickedOnConditionalCurved(this);' style=' width:" + con_width + "px; height:"+con_height+ "px; left: "+ con_left + "px; top: "+con_top+ "px;'></div>";
                    //$("table#main td:first").append(con_div);

                    //// = skipSectionCondition.ConditionVal;
                    //var constr_top = $("#" + conditionArrowUniqueId).position().top +5;
                    //var constr_left = $("#" + conditionArrowUniqueId).position().left-20;
                    //var onclickevent = 'OpenEditSkipLogic("'+ skipSectionCondition.ID +'");';
                    //var logiclabel = "<span class='conditiospan' onclick='"+onclickevent+"' style='top:"+constr_top+"px; left:"+constr_left+"px;'>"+logicstr+"</span>";
                    //$("table#main td:first").append(logiclabel);
                }

                //if(skipSectionCondition.SkipQuestionsID.length>0)
                //{
                //    var questionIds = skipSectionCondition.SkipQuestionsID;
                //    var lastquestionID;
                //    var lastskipquestionID =questionIds[questionIds.length-1];
                //    $(jsonquestions).each(function(){
                //        lastquestionID = this.ID;
                //    });

                //    $(questionIds).each(function(){
                //        var questionId = this.toString();
                //        var questionobj =  $(".quest_" + questionId);
                //        var startskiplogic;
                //        var top;
                //        var height = 76+(i*diffbetween2condition);
                //        var sectionID;
                //        var left ;
                //        var width;
                //        ///if skip question is first then find the section of question and it previous element
                //        ///else it will directly find previous element
                //        if($(questionobj).prev().prev().length ==0 )
                //        {
                //            $(jsonquestions).each(function() {
                //                var question = this;
                //                if(question.ID == questionId){
                //                    sectionID = question.ServiceSectionID;
                //                }
                //            });
                //            var section = $(".section_" + sectionID);
                //            var startskiplogic  = $($(section).parent()).prev();
                //            top = $(startskiplogic).position().top + 28 + (skipconditioncount*15)-(i*diffbetween2condition);

                //            ///to get the size of the cruved arrow.
                //            var left = $(startskiplogic).position().left + ($(startskiplogic).width()/2)+10;
                //            var width =  ($(startskiplogic).width()/3) + $(questionobj).width() + ($(questionobj).prev().width()) + ($(questionobj).next().width()/3);
                //        }
                //        else{
                //            startskiplogic = $(questionobj).prev();
                //            top = $(startskiplogic).position().top - (50+(skipconditioncount*15))-(i*diffbetween2condition);
                //            var left = $(startskiplogic).position().left + ($(startskiplogic).width()/2)+10;
                //            var width =  ($(startskiplogic).width()/3) + $(questionobj).width() + ($(questionobj).next().width()/3);
                //        }

                //        ///if skip question is last question in the section then find the next element of section td.
                //        if($(questionobj).next().length == 0 )
                //        {
                //            $(jsonquestions).each(function() {
                //                var question = this;
                //                if(question.ID == questionId){
                //                    sectionID = question.ServiceSectionID;
                //                }
                //            });
                //            var section = $(".section_" + sectionID);
                //            var sectionnextelement  = $($(section).parent()).next();
                        
                //            width =  ($(startskiplogic).width()/2) + $(questionobj).width() + ($(sectionnextelement).width()/2);
                //            if(lastquestionID == questionId)
                //            {
                //                width =  ($(startskiplogic).width()/2) + $(questionobj).width() + ($(sectionnextelement).width()/2);
                //                left = $(startskiplogic).position().left + ($(startskiplogic).width()/2)-17;
                //            }
                //        }
                //        var skipQuestionUniqueId = getUniqueId(skipSectionCondition.ID.toString(), questionId.toString(), "skiplogicarrow");
                //        var div = "<div id='"+skipQuestionUniqueId +"' class='skiplogicarrow' style='width:" + width + "px; height:"+height+ "px; left: "+ left + "px; top: "+top+ "px;'></div>";
                //        $("table#main td:first").append(div);
                //        ///if the question is last question then add exit label
                //        if(lastquestionID.toString() == questionId.toString())
                //        {
                //            var exitlabelleft = width;
                //            var rightarrow = "<div class='arrow-right' style='left:"+exitlabelleft+"px !important;'></div>";
                //            $("#"+skipQuestionUniqueId).append(rightarrow);
                //            $("#"+skipQuestionUniqueId).css("border-right","none");
                //        }
                //        else{
                //            var downarrowtop = height -10;
                //            var downarrowleft = width-4;
                //            var downarrow  = "<div class='arrow-down' style='top:"+downarrowtop+"px !important;left:"+downarrowleft+"px !important;'></div>";
                //            $("#"+skipQuestionUniqueId).append(downarrow);
                //        }
                //    });

                //    ///condition arrow
                //    //var skipLastQuestionUniqueId = getUniqueId(skipSectionCondition.ID.toString(), lastskipquestionID.toString(), "skiplogicarrow");
                //    //var con_left = $("."+contidionvar).position().left + 28-10;
                //    //var con_top =  $("#"+skipLastQuestionUniqueId).position().top;
                //    //var con_width = $("#"+skipLastQuestionUniqueId).position().left - $("."+contidionvar).position().left -23;
                //    //var con_height =  $("#"+skipLastQuestionUniqueId).height() - 15;
                //    //var conditionArrowUniqueId = "conditionvararrow"+skipSectionCondition.ID;
                //    //var con_div = "<div id='"+conditionArrowUniqueId+"' class='conditionvararrow' style=' width:" + con_width + "px; height:"+con_height+ "px; left: "+ con_left + "px; top: "+con_top+ "px;'></div>";
                //    //$("table#main td:first").append(con_div);

                //    ////var logicstr = skipSectionCondition.ConditionVal;
                //    //var constr_top = $("#"+conditionArrowUniqueId).position().top +5;
                //    //var constr_left = $("#"+conditionArrowUniqueId).position().left-20;
                //    //var onclickevent = 'OpenEditSkipLogic("'+ skipSectionCondition.ID +'");';
                //    //var logiclabel = "<span class='conditiospan' onclick='"+onclickevent+"' style='top:"+constr_top+"px; left:"+constr_left+"px;'>"+logicstr+"</span>";
                //    //$("table#main td:first").append(logiclabel);
                //}
            });
        }
    });

    function getUniqueId(skipSectionConditionid, sectionid, stringid)
    {
        return stringid + sectionid+skipSectionConditionid;
    }

    function OpeneditQuestion(questionID)
    { 
        var projectID = "<%= service.ID %>";
        var title ="Edit Question";
        var params = "svcconfigid="+ projectID +"&questionid=" + questionID + "&isudlg=1";
        UgitOpenPopupDialog('<%= editServiceQuestionUrl %>', params, title, '50', '75', 0, escape("<%= Request.Url.AbsolutePath %>"));
    }

    function OpenEditSkipLogic(skiplogicid)
    {
        var projectID = "<%= service.ID %>";
        var moduleName = "SVCConfig";
        var title ="Skip Section";
        var params = "svcConfigID="+ projectID + "&branchId="+ skiplogicid;
        UgitOpenPopupDialog('<%= editservicequestionbranchUrl %>', params, title, '60', '80', 0, escape("<%= Request.Url.AbsolutePath %>"));
    }

    function ClickedOnConditionalCurved(div){
        //var ctl = document.getElementById('conditionvararrowdiv');
        //var startPos = ctl.selectionStart;
        //var endPos = ctl.selectionEnd;
    }
    

</script>   
<div id="divWorkFlow" style="position: relative;overflow-x:scroll; overflow-y: hidden;width: 99% !important">
    <div class="wizard_steps float-left">
        <nav class="steps">
                                                                            
        </nav>
    </div>                                                             
    <table id="main" style="width: 100%; border-collapse: inherit;">
        <tr>
            <asp:Repeater ID="rptSection" runat="server" OnItemDataBound="rptSection_ItemDataBound">
                <ItemTemplate>
                    <td>
                        <div id="section" runat="server" class="contract_steps_module" style="">
                            <span style="margin-top: -52px; position: relative; background-color: white; padding: 0px 6px; float: left;">
                                <b><%# Eval("ItemOrder") %>: <%# Eval("SectionName") %></b></span>
                            <div class="contract_steps_container">
                                <div class="contract_steptop_content">
                                    <table style="text-align: center; border-collapse: collapse;width: 100%;">
                                        <tr>
                                            <td>
                                                <table style="width: 100%; text-align: center; border-collapse: collapse;">
                                                    <tr>
                                                        <%--<td id="td2" runat="server" style="height: 38px; background-repeat: repeat-x;" class="lineimg1"></td>--%>
                                                        <asp:Repeater ID="rptquestion" runat="server" OnItemDataBound="rptquestion_ItemDataBound">

                                                            <ItemTemplate>
                                                                <td id="tdQuestion" runat="server" style="height: 38px; width: 36px; background-repeat: no-repeat;">
                                                                    <div style="position: relative">
                                                                        <span class="pos_rel">
                                                                            <i style="text-align:center">                                                                                
                                                                                <asp:Literal ID="lquestionNumber" runat="server"></asp:Literal>
                                                                            </i>
                                                                        </span>
                                                                        <i id="stageTitleContainer" class="stage-titlecontainer" runat="server">
                                                                            <b class="pos_rel" style="">
                                                                                <asp:Literal ID="stageTitle" runat="server"></asp:Literal>
                                                                            </b>
                                                                        </i>
                                                                    </div>

                                                                </td>
                                                                <td id="tdline" runat="server" style="height: 38px; background-repeat: repeat-x;" class="lineimg"></td>
                                                            </ItemTemplate>
                                                        </asp:Repeater>

                                                        <asp:ListView ID="lvStepSections" runat="server" ItemPlaceholderID="PlaceHolder1" DataKeyNames="ID"   OnItemDataBound="lvStepSections_ItemDataBound">
                                                            <LayoutTemplate>
                                                                <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
                                                            </LayoutTemplate>
                                                            <ItemTemplate>
                                                                <div class="step svcWorkFlow-Step">
                                                                    <div id="activeIconDiv" runat="server" class="step_content"> 
                                                                            <div id="divHelp" runat="server" class="wf-img-wrap queWf-wrap divHelp">
                                                                        <p  id="stepIcon" class="step_number" runat="server">
                                                                            <%--<img id="imgSection" runat="server" visible="true" width="35" />--%>
                                                                            <asp:Literal ID="lquestionNumber" runat="server"></asp:Literal>
                                                                        </p> 
                                                                            </div>
                                                                        <small>
                                                                            <%--<asp:HiddenField runat="server" ID="ItemOrder" Value='<%# Eval("Key") %>' />--%>
                                                                            <asp:Label runat="server" ID="stageTitle" ></asp:Label>
                                                                        </small>
                                                                        <div class="lines">
                                                                            <div class="line -background bg-col-blue" id="lineWorkflow" runat="server">
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </ItemTemplate>
                                                        </asp:ListView>   
                                                         <%--<td id="td1" runat="server" style="height: 38px; background-repeat: repeat-x;" class="lineimg1"></td>--%>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </td>
                    <td id="tdline" runat="server" style="height: 38px; width: 30px;">
                          <div class="-background bg-col-blue active lineSection lineimg" id="lineWorkflow" runat="server">
                           </div>
                      <%--  <span class="lineimg sectionjoin"></span>--%>
                    </td>
                </ItemTemplate>
            </asp:Repeater>
        </tr>
    </table>
</div>

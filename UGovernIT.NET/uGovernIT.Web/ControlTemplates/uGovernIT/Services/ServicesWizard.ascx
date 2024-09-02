<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ServicesWizard.ascx.cs" Inherits="uGovernIT.Web.ServicesWizard" %>

<%@ Register Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<link href="<%= ResolveUrl(@"~/Content/uGITWizard.css") + "?v=" + UGITUtility.AssemblyVersion %>" rel="stylesheet" />

<script src="../../Scripts/HelpCardDisplayPopup.js?v=<%=UGITUtility.AssemblyVersion %>"></script>
<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
    .col_aright_ticketid {
        font-weight: bold;
        float: right;
        margin-left: 6px;
        vertical-align: middle;
        font-size: 12px;
    }

    .flashCard-container-display {
        width: 300px;
        /*margin: 0px auto;
		    border: 1px solid #FFF;*/
        height: 450px;
        /*padding: 10px;
	        box-shadow: 0px 0px 4px #888888;*/
    }

        .flashCard-container-display img {
            max-width: 100%;
        }


    .showmenot {
        display: none;
    }


    .helpwidth {
        float: right;
        width: 35px;
    }

    .fleft {
        float: left;
    }

    /*.full_width {
        width: 100%;
    }*/


    .hide {
        display: none;
    }

    .fright {
        float: right;
    }

    .ms-inputuserfield {
        border: 1px solid black;
    }

    .pactions {
        float: right;
        text-align:right;
        width: 100%;
        padding-bottom:12px;
        /*padding-top: 10px;*/
    }

    .pactions .button-bg {
        float: right;
    }

   .pactions .primary-blueBtn {
    float:right;
    padding:2px;
    }

    .pactions .svcCancelBtn {
        float: right;
        margin:2px 24px 2px 2px;
    }
    .hide {
        display: none;
    }

    .trfooter td {
        border-top: 2px solid black;
    }

    .summarymsg {
        float: left;
        font-size: 11px;
        font-weight: bold;
        padding: 4px 13px 20px;
        float: left;
        width: 100%;
    }

    .errormsg-container {
        color: red;
    }

    div.ms-inputuserfield {
        height: 20px !important;
    }

    span.ms-formvalidation {
        display: none;
    }

    .ug-maximize-block {
        background: #fff;
        z-index: 10000;
        width: 98%;
        height: 100%;
        position: fixed !important;
        top: 0px;
        left: 0px;
        margin: 0px;
    }

    .question {
         border-bottom:none !important;
    }

    .maximise {
        cursor: pointer;
    }

    .minimise {
        cursor: pointer;
    }

    .questionActionPanel {
        position: absolute;
        top: 0px;
        right: 0px;
    }

    .HideALL div {
        display: none;
    }
    .questionlb_hide {
        background: none !important;
    }
    .questionlb {
    display:none !important;
    }
    .lvhidegrouping {
    display:none !important;
    }
    
    .lvshowgrouping {
    display:flex !important;
    
    }
     .lvgroupsupportcls {
        float:left;
        width:100%;
        /*border-top:1px solid #dee2e3 !important;*/
        padding: 10px 0px 5px 0px !important;
    }
    
    .label {
    color:initial !important;
    }
    .add-file-doc {
        padding-left:0px;
    }
    .editTicket-fileupload-ctrl table.dxucInputs_UGITNavyBlueDevEx tr td.dxucTextBox_UGITNavyBlueDevEx {
    border:none !important;
    }
    .select_product {
        margin-left:0px !important;
        width: 100% !important;
    }
    .select_product table{
        width:100%;
    }

    .clsquesSummary {
    display:flex;
    }
    /*.CRMDueDate_inputField {
    width: 200px !important;
    }*/
</style>
<asp:Panel ID="hideQuestionDesigner" runat="server" Visible="false">
    <style type="text/css">
        .questionlb {
            background: none;
            display:none !important;
        }
    </style>
</asp:Panel>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">

    $(document).ready(function () {
        //var list = document.getElementsByTagName("a");
        $('.uploadedFileContainer').find('a').addClass('hyperLinkIcon');
        var val1 = $('.uploadedFileContainer').find('a').text();
        $('.uploadedFileContainer').find('a').prop('title', val1);
        $('.uploadedFileContainer').find('img').addClass('cancelUploadedFiles');
        $('.isSvcPopup_container').parents().eq(2).addClass("isSvcPopup_bodyContainer");

    });

    function reqGetAssetValues(h) {
        var ddEditControl = ASPxClientControl.GetControlCollection().GetByName($(".field_assetlookup_edit").attr("id"));
        
        var reqTypeID = [];
        var reqTypeText = [];
        if (h.length == 0) {
            ddEditControl.SetKeyValue(reqTypeID.join(','));
            ddEditControl.SetText(reqTypeText.join(','));
            return false;
        }
        
        for (var i = 0; i < h.length; i++) {
            reqTypeID.push(h[i][0]);
            reqTypeText.push(h[i][1]);
        }
        ddEditControl.SetKeyValue(reqTypeID.join(','));
        ddEditControl.SetText(reqTypeText.join(','));
        //ddEditControl.HideDropDown();
    }
    function requestTypeSelectionChanged(s, e) {
        s.GetSelectedFieldValues("ID;AssetName", reqGetValues);
        //s.Hide();
    }
    function reqGetValues(h) {
        if (h.length == 0)
            return false;
        var reqTypeID = [];
        var reqTypeText = [];
        var ddEditControl = ASPxClientControl.GetControlCollection().GetByName($(".field_requesttypelookup_edit").attr("id"));
        for (var i = 0; i < h.length; i++) {
            reqTypeID.push(h[i][0]);
            reqTypeText.push(h[i][1]);
        }
        ddEditControl.SetKeyValue(reqTypeID.join(','));
        ddEditControl.SetText(reqTypeText.join(','));
        ddEditControl.HideDropDown();
    }
    function assetSelectionChanged(s, e) {
        s.GetSelectedFieldValues("ID;AssetTagNum", reqGetAssetValues);
        //s.Hide();
    }

    //Multi choice 
    function onmultichoicedropdowngridChange(s, e) {
        
        var qustDtl = s.name.split("_question_")[1];
        var questionID = "";
        if (qustDtl.split("_").length > 0) {
            questionID = qustDtl.split("_")[1];
            var question = skipObj.getCurrentQuestion(questionID);
            if (question && question.length > 0)
                questionBrachLogic(s, "multichoice", "multichoicedropdowngridChange", question[0].Token);
        }
    }

    window.serviceSkipLogic = window.serviceSkipLogic || {};
    var skipObj = null;
    $.skipLogic = function () {
        var skipcondition = window.serviceSkipLogic.initialize();
        return skipcondition;
    };
    window.serviceSkipLogic = {
        skipJson: {},
        questionsJson: {},
        initialize: function () {
            var skipCondition = this.decodeHTML($.trim($(".pskipconditions").html()));
            var sectionQuestion = $.trim($(".pSectionQuestion").html());

            if (skipCondition == '')
                skipCondition = '[]';

            if (sectionQuestion == '')
                sectionQuestion = '[]';

            this.skipJson = $.parseJSON(skipCondition);
            this.questionsJson = $.parseJSON(sectionQuestion);

            $(this.skipJson).each(function (i, item) {
                $(item.mQuestions).each(function (i, subItem) {
                    subItem.values = unescape(subItem.values);
                });
            });

            return this;
        },
        decodeHTML: function (input) {
            var txt = document.createElement('textarea');
            txt.innerHTML = input;
            return txt.value;
        },
        getCurrentQuestion: function (ID) {
            var selectedQuestion = new Array();
            $(this.questionsJson).each(function (i, item) {
                if (item.QuestionID == ID) {
                    selectedQuestion.push(item);
                }
            });
            return selectedQuestion;
        },
        getCurrentQuestionByToken: function (tokenName) {

            var selectedQuestion = new Array();
            $(this.questionsJson).each(function (i, item) {
                if (item.Token == tokenName) {
                    selectedQuestion.push(item);
                }
            });
            return selectedQuestion;
        },
        getCurrentTokenConditions: function (token) {

            var selectedConditions = new Array();
            $(this.skipJson).each(function (i, item) {
                $(item.mQuestions).each(function (i, subItem) {
                    if (subItem.key == token)
                        selectedConditions.push(item);
                });
            });
            return selectedConditions;
        },
        executeSkipLogic: function (obj, qType, ctrName, token) {
            if (qType == "datetime")    //In case of Devexpress DateTime control only
                obj = obj.mainElement;

            var jsFirstParent;
            var sConditions = this.getCurrentTokenConditions(token);

            if (ctrName == "drpdwnListmutilookup" || ctrName == "userfield" || ctrName == "multichoicedropdowngridChange" || ctrName == "fillInChoiceDropdown" || ctrName == "aspxDropDownList" || ctrName == "devexdropdownlist") {
                jsFirstParent = $((obj).mainElement).parents(".select_product").first();
            }
            else if (ctrName == "requesttype") {
                jsFirstParent = $((obj).mainElement).parents(".select_product");
            }
            else {
                jsFirstParent = $(obj).parents(".select_product").first();
            }

            switch (qType) {
                
                case "singlechoice":
                    {
                        this.singleChoiceQuestion(sConditions, jsFirstParent, ctrName);
                    } break;
                case "multichoice":
                    {
                        this.multiChoiceQuestion(sConditions, jsFirstParent, ctrName);
                    } break;
                case "textbox":
                    {
                        this.textboxQuestion(sConditions, jsFirstParent, ctrName);
                    } break;
                case "number":
                    {
                        this.numberQuestion(sConditions, jsFirstParent, ctrName);
                    } break;
                case "checkbox":
                    {
                        this.checkboxQuestion(sConditions, jsFirstParent, ctrName);
                    } break;
                case "lookup":
                    {
                        this.lookupQuestion(sConditions, jsFirstParent, ctrName, obj);
                    } break;
                case "requesttype":
                    {
                        this.requesttypeQuestion(sConditions, jsFirstParent, ctrName);
                    } break;
                case "userfield":
                    {
                        this.userQuestion(sConditions, jsFirstParent, ctrName);
                    } break;
                case "datetime":
                    {
                        this.datetimeQuestion(sConditions, jsFirstParent, ctrName);
                    } break;
                case "rating":
                    {
                        this.ratingQuestion(sConditions, jsFirstParent, ctrName);
                    } break;
            }
        },
        singleChoiceQuestion: function (sConditions, jsParentObj, ctrName) {
            var value = "";
            if (ctrName == "radiobuttonlist") {
                value = jsParentObj.find(":radio:checked").val();
                if (value == undefined || value == null) {
                    value = "";
                }
            }
            else if (ctrName == "fillInChoiceDropdown" || ctrName == "aspxDropDownList" || ctrName=="devexdropdownlist") {
                value = jsParentObj.find(".dropdownctr tr td:first-child input").val();
            }
            else
                //value = jsParentObj.find("select").val();
                value=jsParentObj.prevObject.prevObject[0].prevInputValue;


            var cObj = this;
            var txtHidden = jsParentObj.find(".hiddenCtr");

            if (txtHidden.val() != "[$skip$]") {
                txtHidden.val(value);
            }

            $(sConditions).each(function (i, item) {
                var result = false;
                if (item.attachment != "" && (item.attachment == "mandatory" || item.attachment == "na"))
                    result = cObj.testCondition(value, item.mQuestions[0].operator, item.mQuestions[0].values, item.mQuestions[0].qtype);
                else
                    result = cObj.testMultiConditions(item, value);

                if (result) {
                    cObj.hideShowQuestions(item.skipQuestions, true);
                    cObj.hideShowQuestionGroupSection(item.skipQuestions, true);
                    cObj.attachmentMandatoryCheck(item, true);
                }
                else {
                    cObj.hideShowQuestions(item.skipQuestions, false);
                    cObj.hideShowQuestionGroupSection(item.skipQuestions, false);
                    cObj.attachmentMandatoryCheck(item, false);
                }
            });
        },
        multiChoiceQuestion: function (sConditions, jsParentObj, ctrName) {
            var valueArr = new Array();

            if (ctrName == "multichoicedropdowngridChange") {
                var dropCtrl = $(jsParentObj.find(".gridLookupctr"));
                if (dropCtrl) {
                    var ctrl = ASPxClientControl.GetControlCollection().GetByName($(dropCtrl).get(0).id);
                    var obj = ctrl.GetValue();
                    if (obj != null)
                        valueArr = obj;
                }
            }
            else {
                var checkedCtrs = jsParentObj.find(":checkbox:checked");

                checkedCtrs.each(function (i, item) {
                    valueArr.push($(item).parent().find("label").text());
                });
            }

            var value = valueArr.join("~");
            var cObj = this;
            var txtHidden = jsParentObj.find(".hiddenCtr");

            if (txtHidden.val() != "[$skip$]") {
                txtHidden.val(value);
            }

            $(sConditions).each(function (i, item) {
                var result = false;

                if (item.attachment != "" && item.attachment == "mandatory")
                    result = cObj.testCondition(value, item.mQuestions[0].operator, item.mQuestions[0].values, item.mQuestions[0].qtype);
                else
                    result = cObj.testMultiConditions(item, value);

                if (result) {
                    cObj.hideShowQuestions(item.skipQuestions, true);
                    cObj.hideShowQuestionGroupSection(item.skipQuestions, true);
                    cObj.attachmentMandatoryCheck(item, true);
                }
                else {
                    cObj.hideShowQuestions(item.skipQuestions, false);
                    cObj.hideShowQuestionGroupSection(item.skipQuestions, false);
                    cObj.attachmentMandatoryCheck(item, false);
                }
            });
        },
        textboxQuestion: function (sConditions, jsParentObj, ctrName) {
            var value = "";

            if (jsParentObj.find("input:text").not('.hiddenCtr').length > 0) {
                value = jsParentObj.find("input:text").val();
            }
            else if (jsParentObj.find("textarea").length > 0) {
                value = jsParentObj.find("textarea").val();
            }

            var txtHidden = jsParentObj.find(".hiddenCtr");

            if (txtHidden.val() != "[$skip$]") {
                txtHidden.val(value);
            }

            var cObj = this;
            $(sConditions).each(function (i, item) {
                var result = false;

                if (item.attachment != "" && item.attachment == "mandatory")
                    result = cObj.testCondition(value, item.mQuestions[0].operator, item.mQuestions[0].values, item.mQuestions[0].qtype);
                else
                    result = cObj.testMultiConditions(item, value);

                if (result) {
                    cObj.hideShowQuestions(item.skipQuestions, true);
                    cObj.hideShowQuestionGroupSection(item.skipQuestions, true);
                    cObj.attachmentMandatoryCheck(item, true);
                }
                else {
                    cObj.hideShowQuestions(item.skipQuestions, false);
                    cObj.hideShowQuestionGroupSection(item.skipQuestions, false);
                    cObj.attachmentMandatoryCheck(item, false);
                }
            })
        },
        checkboxQuestion: function (sConditions, jsParentObj, ctrName) {
            var value = jsParentObj.find("input:checkbox").is(":checked").toString();

            var cObj = this;
            var txtHidden = jsParentObj.find(".hiddenCtr");

            if (txtHidden.val() != "[$skip$]") {
                txtHidden.val(value);
            }

            $(sConditions).each(function (i, item) {
                var result = false;

                if (item.attachment != "" && item.attachment == "mandatory")
                    result = cObj.testCondition(value, item.mQuestions[0].operator, item.mQuestions[0].values, item.mQuestions[0].qtype);
                else
                    result = cObj.testMultiConditions(item, value);

                if (result) {
                    cObj.hideShowQuestions(item.skipQuestions, true);
                    cObj.hideShowQuestionGroupSection(item.skipQuestions, true);
                    cObj.attachmentMandatoryCheck(item, true);
                }
                else {
                    cObj.hideShowQuestions(item.skipQuestions, false);
                    cObj.hideShowQuestionGroupSection(item.skipQuestions, false);
                    cObj.attachmentMandatoryCheck(item, false);
                }
            });
        },
        requesttypeQuestion: function (sConditions, jsParentObj, ctrName) {
            var value = "";
            var userobj = ASPxClientControl.GetControlCollection().GetByName($(jsParentObj).find('.field_requesttypelookup_edit').attr('id'));
            if (userobj) {
                value = userobj.GetKeyValue();
            }

            if (value == "" || value == null || value == undefined) {
                value = jsParentObj.find("select[ctype='requesttype']").val();

            }

            if (value == null)
                value = '';
            //if (value.includes(";#")) {
            //    value = value.split(";#")[0];
            //}
            var txtHidden = jsParentObj.find(".hiddenCtr");

            if (txtHidden.val() != "[$skip$]") {
                txtHidden.val(value);
            }
            var cObj = this;
            $(sConditions).each(function (i, item) {
                var result = false;

                if (item.attachment != "" && item.attachment == "mandatory")
                    result = cObj.testCondition(value, item.mQuestions[0].operator, item.mQuestions[0].values, item.mQuestions[0].qtype);
                else
                    result = cObj.testMultiConditions(item, value);

                if (result) {
                    cObj.hideShowQuestions(item.skipQuestions, true);
                    cObj.hideShowQuestionGroupSection(item.skipQuestions, true);
                    cObj.attachmentMandatoryCheck(item, true);
                }
                else {
                    cObj.hideShowQuestions(item.skipQuestions, false);
                    cObj.hideShowQuestionGroupSection(item.skipQuestions, false);
                    cObj.attachmentMandatoryCheck(item, false);
                }
            })
        },
        lookupQuestion: function (sConditions, jsParentObj, ctrName, dropdownobj) {
            var value = "";
            var txtHidden = jsParentObj.find(".hiddenCtr");
            var userobj = ASPxClientControl.GetControlCollection().GetByName(jsParentObj.prevObject.prevObject[0].id);

            if (userobj) {
                value = userobj.GetValueString();
            }

            if (value == "" || value == null || value == undefined) {
                if (ctrName == "drpdwnListmutilookup") {

                    value = dropdownobj.GetKeyValue();
                    if (value)
                        value = value.replace(/;/g, "~");

                    if (txtHidden.val() != "[$skip$]") {
                        txtHidden.val(dropdownobj.GetText());
                    }
                }
                else if (ctrName == "drpdwnListDepartmentlookup") {

                    var jqddpCtrl = $(dropdownobj);
                    var vals = new Array();
                    var dptVals = $("#" + dropdownobj.id + ">span");

                    if (dptVals.length > 0) {
                        $.each(dptVals, function (i, s) {
                            vals.push($(s).attr("id"));
                        })
                    }
                    value = vals.join("~");
                }
                else {
                    value = jsParentObj.find("select").val();
                    var txtHidden = jsParentObj.find(".hiddenCtr");

                    if (txtHidden.val() != "[$skip$]") {
                        txtHidden.val(value);
                    }
                }
            }

            if (ctrName != "drpdwnListmutilookup" && txtHidden.val() != "[$skip$]") {
                txtHidden.val(value);
            }

            var cObj = this;
            $(sConditions).each(function (i, item) {
                var result = false;

                if (item.attachment != "" && item.attachment == "mandatory")
                    result = cObj.testCondition(value, item.mQuestions[0].operator, item.mQuestions[0].values, item.mQuestions[0].qtype);
                else
                    result = cObj.testMultiConditions(item, value);

                if (result) {
                    cObj.hideShowQuestions(item.skipQuestions, true);
                    cObj.hideShowQuestionGroupSection(item.skipQuestions, true);
                    cObj.attachmentMandatoryCheck(item, true);
                }
                else {
                    cObj.hideShowQuestions(item.skipQuestions, false);
                    cObj.hideShowQuestionGroupSection(item.skipQuestions, false);
                    cObj.attachmentMandatoryCheck(item, false);
                }
            })
        },
        numberQuestion: function (sConditions, jsParentObj, ctrName) {
            var value = jsParentObj.find("input:text").val();
            var txtHidden = jsParentObj.find(".hiddenCtr");

            if (txtHidden.val() != "[$skip$]") {
                txtHidden.val(value);
            }

            var cObj = this;
            $(sConditions).each(function (i, item) {
                var result = false;

                if (item.attachment != "" && item.attachment == "mandatory")
                    result = cObj.testCondition(value, item.mQuestions[0].operator, item.mQuestions[0].values, item.mQuestions[0].qtype);
                else
                    result = cObj.testMultiConditions(item, value);

                if (result) {
                    cObj.hideShowQuestions(item.skipQuestions, true);
                    cObj.hideShowQuestionGroupSection(item.skipQuestions, true);
                    cObj.attachmentMandatoryCheck(item, true);
                }
                else {
                    cObj.hideShowQuestions(item.skipQuestions, false);
                    cObj.hideShowQuestionGroupSection(item.skipQuestions, false);
                    cObj.attachmentMandatoryCheck(item, false);
                }
            });

        },
        datetimeQuestion: function (sConditions, jsParentObj, ctrName) {

            //var value = jsParentObj.find(".datetimectr").val();   // old way

            // Changed here to fetch value from devexpress DateTime Control because it is rendered like a table
            var value = jsParentObj.find(".datetimectr tr td:first-child input").val();
            var txtHidden = jsParentObj.find(".hiddenCtr");

            if (txtHidden.val() != "[$skip$]") {
                txtHidden.val(value);
            }

            var cObj = this;
            $(sConditions).each(function (i, item) {
                var result = false;

                if (item.attachment != "" && item.attachment == "mandatory")
                    result = cObj.testCondition(value, item.mQuestions[0].operator, item.mQuestions[0].values, item.mQuestions[0].qtype);
                else
                    result = cObj.testMultiConditions(item, value);

                if (result) {
                    cObj.hideShowQuestions(item.skipQuestions, true);
                    cObj.hideShowQuestionGroupSection(item.skipQuestions, true);
                    cObj.attachmentMandatoryCheck(item, true);
                }
                else {
                    cObj.hideShowQuestions(item.skipQuestions, false);
                    cObj.hideShowQuestionGroupSection(item.skipQuestions, false);
                    cObj.attachmentMandatoryCheck(item, false);
                }
            })
        },
        userQuestion: function (sConditions, jsParentObj, ctrName) {

            var value = "";
            var userobj = ASPxClientControl.GetControlCollection().GetByName(jsParentObj.prevObject.prevObject[0].id);
            if (userobj) {
                value = userobj.GetValueString();
            }

            var txtHidden = jsParentObj.find(".hiddenCtr");

            if (txtHidden.val() != "[$skip$]") {
                txtHidden.val(value);
            }

            var cObj = this;
            $(sConditions).each(function (i, item) {
                var result = false;

                if (item.attachment != "" && item.attachment == "mandatory")
                    result = cObj.testCondition(value, item.mQuestions[0].operator, item.mQuestions[0].values, item.mQuestions[0].qtype);
                else
                    result = cObj.testMultiConditions(item, value);

                if (result) {
                    cObj.hideShowQuestions(item.skipQuestions, true);
                    cObj.hideShowQuestionGroupSection(item.skipQuestions, true);
                    cObj.attachmentMandatoryCheck(item, true);
                }
                else {
                    cObj.hideShowQuestions(item.skipQuestions, false);
                    cObj.hideShowQuestionGroupSection(item.skipQuestions, false);
                    cObj.attachmentMandatoryCheck(item, false);
                }
            })
        },
        ratingQuestion: function (sConditions, jsParentObj, ctrName, dropdownobj) {

            var value = "";
            var userobj = ASPxClientControl.GetControlCollection().GetByName(jsParentObj.prevObject.prevObject[0].name);
            if (userobj) {
                value = userobj.GetValue();
            }

            var txtHidden = jsParentObj.find(".hiddenCtr");
            if (txtHidden.val() != "[$skip$]") {
                txtHidden.val(value);
            }

            var cObj = this;
            $(sConditions).each(function (i, item) {
                var result = false;

                if (item.attachment != "" && item.attachment == "mandatory")
                    result = cObj.testCondition(value, item.mQuestions[0].operator, item.mQuestions[0].values, item.mQuestions[0].qtype);
                else
                    result = cObj.testMultiConditions(item, value);

                if (result) {
                    cObj.hideShowQuestions(item.skipQuestions, true);
                    cObj.hideShowQuestionGroupSection(item.skipQuestions, true);
                    cObj.attachmentMandatoryCheck(item, true);
                }
                else {
                    cObj.hideShowQuestions(item.skipQuestions, false);
                    cObj.hideShowQuestionGroupSection(item.skipQuestions, false);
                    cObj.attachmentMandatoryCheck(item, false);
                }
            })

        },
        testCondition: function (lsValues, operator, rsValues, qType) {

            if (operator != null && operator != undefined)
                operator = operator.toLowerCase();

            // test below two lines
            lsValues = lsValues.toString();
            rsValues = rsValues.toString();

            var isvalid = false;
            var lsValueArr = lsValues.split("~");
            var rsValueArr = rsValues.split("~");

            if (operator == "=") {
                if (qType == "singlechoice" || qType == "requesttype" || qType == "lookup" || qType == "multichoice" || qType == "userfield") {
                    if (lsValues.toLowerCase() == rsValues.toLowerCase()) {
                        isvalid = true;
                    }
                }
                else if (qType == "textbox" || qType == "rating") {
                    if ($.trim(rsValues.toLowerCase()) == $.trim(lsValues.toLowerCase())) {
                        isvalid = true;
                    }
                }
                else if (qType == "checkbox") {
                    if (converStringToBoolean(lsValues) == converStringToBoolean(rsValues)) {
                        isvalid = true;
                    }
                }
                else if (qType == "datetime") {

                }
                else if (qType == "number") {
                    var lsNum = Number(lsValues);
                    var rsNum = Number(rsValues);
                    if (isNaN(lsNum) == false && isNaN(rsNum) == false && lsNum == rsNum) {
                        isvalid = true;
                    }
                }
            }
            else if (operator == "!=") {
                if (qType == "singlechoice" || qType == "requesttype" || qType == "lookup" || qType == "multichoice" || qType == "userfield") {
                    if (lsValues.toLowerCase() != rsValues.toLowerCase()) {
                        isvalid = true;
                    }
                }
                else if (qType == "textbox" || qType == "rating") {
                    if ($.trim(rsValues.toLowerCase()) != $.trim(lsValues.toLowerCase())) {
                        isvalid = true;
                    }
                }
                else if (qType == "checkbox") {
                    if (converStringToBoolean(lsValues) != converStringToBoolean(rsValues)) {
                        isvalid = true;
                    }
                }
                else if (qType == "datetime") {

                }
                else if (qType == "number") {
                    var lsNum = Number(lsValues);
                    var rsNum = Number(rsValues);

                    if (isNaN(lsNum) == false && isNaN(rsNum) == false && lsNum != rsNum) {
                        isvalid = true;
                    }
                }
            }
            else if (operator == "oneof") {
                if (qType == "singlechoice" || qType == "requesttype" || qType == "lookup" || qType == "userfield") {
                    for (var i = 0; i < rsValueArr.length; i++) {
                        if (lsValues.toLowerCase() == rsValueArr[i].toLowerCase()) {
                            isvalid = true;
                            break;
                        }
                    }
                }
                else if (qType == "multichoice") {
                    for (var i = 0; i < lsValueArr.length; i++) {
                        for (var j = 0; j < rsValueArr.length; j++) {
                            if (lsValueArr[i].toLowerCase() == rsValueArr[j].toLowerCase()) {
                                isvalid = true;
                                break;
                            }
                        }
                    }
                }
            }
            else if (operator == "notoneof") {
                var foundMatch = false;
                if (qType == "singlechoice" || qType == "requesttype" || qType == "lookup" || qType == "userfield") {
                    for (var i = 0; i < rsValueArr.length; i++) {
                        if (lsValues.toLowerCase() == rsValueArr[i].toLowerCase()) {
                            foundMatch = true;
                            break;
                        }
                    }
                }
                else if (qType == "multichoice") {
                    for (var i = 0; i < lsValueArr.length; i++) {
                        for (var j = 0; j < rsValueArr.length; j++) {
                            if (lsValueArr[i].toLowerCase() == rsValueArr[j].toLowerCase()) {
                                foundMatch = true;
                                break;
                            }
                        }
                    }
                }

                if (!foundMatch) {
                    isvalid = true;
                }
            }
            else if (operator == "contain") {
                var meetCount = 0;
                for (var i = 0; i < lsValueArr.length; i++) {
                    for (var j = 0; j < rsValueArr.length; j++) {
                        if (lsValueArr[i].toLowerCase() == rsValueArr[j].toLowerCase()) {
                            meetCount += 1;
                        }
                    }
                }
                if (rsValueArr.length == meetCount) {
                    isvalid = true;
                }
            }
            else if (operator == "notcontain") {
                var meetCount = 0;
                for (var i = 0; i < lsValueArr.length; i++) {
                    for (var j = 0; j < rsValueArr.length; j++) {
                        if (lsValueArr[i].toLowerCase() == rsValueArr[j].toLowerCase()) {
                            meetCount += 1;
                            break;
                        }
                    }
                }

                if (meetCount == 0) {
                    isvalid = true;
                }
            }
            else if (operator == ">") {
                if (qType == "number") {
                    var lsNum = Number(lsValues);
                    var rsNum = Number(rsValues);
                    if (isNaN(lsNum) == false && isNaN(rsNum) == false && lsNum > rsNum) {
                        isvalid = true;
                    }
                }
            }
            else if (operator == ">=") {
                if (qType == "number") {
                    var lsNum = Number(lsValues);
                    var rsNum = Number(rsValues);
                    if (isNaN(lsNum) == false && isNaN(rsNum) == false && lsNum >= rsNum) {
                        isvalid = true;
                    }
                }
            }
            else if (operator == "<") {
                if (qType == "number") {
                    var lsNum = Number(lsValues);
                    var rsNum = Number(rsValues);
                    if (isNaN(lsNum) == false && isNaN(rsNum) == false && lsNum < rsNum) {
                        isvalid = true;
                    }
                }
            }
            else if (operator == "<=") {
                if (qType == "number") {
                    var lsNum = Number(lsValues);
                    var rsNum = Number(rsValues);
                    if (isNaN(lsNum) == false && isNaN(rsNum) == false && lsNum <= rsNum) {
                        isvalid = true;
                    }
                }
            }

            return isvalid;
        },
        hideShowQuestions: function (questions, isHide) {
            debugger;
            var ids = questions.split(",");
            var value = "";
            for (var i = 0; i < ids.length; i++) {

                var ctrValue = this.getCurrentQuestion(ids[i]);
                var questiondiv = $(".questiondiv_" + ids[i] + " .select_product");
                var isApplicationAccessSkip = false;
                var isDepartmentSkip = false;
                $(ctrValue).each(function (i, item) {

                    var qType = item.Type.toLowerCase();
                    var questionID = item.QuestionID;
                    switch (qType) {
                        case "singlechoice":
                            {
                                value = $(questiondiv).find("select :selected").val();
                                if (value == undefined) {
                                    value = $(questiondiv).find(":radio:checked").val();
                                }
                                 //In case of aspxcombobox
                                if (value == undefined) {
                                    value=$(questiondiv).find(".dropdownctr tr td:first-child input").val();
                                }
                            } break;
                        case "multichoice":
                            {
                                value = $(questiondiv).find("input:checked").length;
                                // this.multiChoiceQuestion(sConditions, jsFirstParent, ctrName);
                            } break;
                        case "textbox":
                            {
                                if ($(questiondiv).find("input:text").not('.hiddenCtr').length > 0) {
                                    value = $(questiondiv).find("input:text").val();
                                }
                                else if ($(questiondiv).find("textarea").length > 0) {
                                    value = $(questiondiv).find("textarea").val();
                                }
                                //  this.textboxQuestion(sConditions, jsFirstParent, ctrName);
                            } break;
                        case "number":
                            {
                                value = $(questiondiv).find("input:text").val();
                                //  this.numberQuestion(sConditions, jsFirstParent, ctrName);
                            } break;
                        case "checkbox":
                            {
                                value = $(questiondiv).find("input:checked").length;
                                // this.checkboxQuestion(sConditions, jsFirstParent, ctrName);
                            } break;
                        case "lookup":
                            {

                                value = $(questiondiv).find("select :selected").val();
                                if (value == undefined) {

                                    var hdq = $(questiondiv).find(".txtDepartmentctr");
                                    if (hdq.length > 0) {
                                        isDepartmentSkip = true;
                                    }
                                }

                                // this.lookupQuestion(sConditions, jsFirstParent, ctrName);
                            } break;
                        case "requesttype":
                            {
                                var obj = ASPxClientControl.GetControlCollection().GetByName($(questiondiv).find('.field_requesttypelookup_edit').attr('id'));
                                if (obj != null && obj != undefined)
                                    value = obj.GetKeyValue();
                                else
                                    value = $(questiondiv).find("select[ctype='requesttype'] :selected").val();
                            } break;
                        case "userfield":
                            {
                            } break;
                        case "datetime":
                            {
                                //value = $(questiondiv).find(".datetimectr").val();
                                value = $(questiondiv).find(".datetimectr tr td:first-child input").val();
                            }
                            break;
                        case "applicationaccessrequest": {
                            isApplicationAccessSkip = true;
                            var hdq = $(questiondiv).find(".hiddenCtr");
                            if (isHide && hdq.length > 0) {
                                hdq.val("[$skip$]");
                                hdq.get(0).value = "[$skip$]";
                            }
                            else {
                                hdq.val("");
                                hdq.get(0).value = "";
                            }
                        } break;
                    }

                });
                var hq = "";
                if (isDepartmentSkip)
                    hq = $(questiondiv).find(".txtDepartmentctr");
                else
                    hq = $(".hiddenCtr_" + ids[i]);

                if (isHide) {
                    if (hq.length > 0) {
                        hq.val("[$skip$]");
                        hq.get(0).value = "[$skip$]";
                    }
                    $(".questiondiv_" + ids[i]).hide();
					/*Comment below line due to blank list of single choice question dropdown when it is first in order and next question is being hide due to skip logic*/
                    //$(".questiondiv_" + ids[i]).parent().hide();
                    $(".questiondiv_" + ids[i]).find(".ratingselectedoption").find('span').detach();
                }
                else {

                    if (hq.length > 0) {
                        if (value == 0)
                            value = "";
                        hq.get(0).value = value;
                        hq.val(value);
                    }
                    $(".questiondiv_" + ids[i]).show();
                    $(".questiondiv_" + ids[i]).parent().show();
                    try {
                        if (isApplicationAccessSkip)
                            SetWidth($(".questiondiv_" + ids[i]));
                    }
                    catch (ex) {

                    }
                }
            }
        },
        attachmentMandatoryCheck: function (itemJson, isMandatory) {
            if (itemJson.attachment && itemJson.attachment == "mandatory") {
                var attachmentChkBlock = $("#attachmentformMandatory input:hidden");
                if (isMandatory) {
                    attachmentChkBlock.val("true");
                    $("#attachmentmandatoryicon").show();
                }
                else {
                    attachmentChkBlock.val("false");
                    $("#attachmentmandatoryicon").hide();
                }
            }
        },
        testMultiConditions: function (item, value) {
            var finalExpression = "";
            var result = false;
            var cObj = this;
            var resultCollection = [];

            $(item.mQuestions).each(function (i, subItem) {
                var currentQuestion = cObj.getCurrentQuestionByToken(subItem.key);

                if (currentQuestion != null && currentQuestion != undefined && currentQuestion.length > 0) {
                    var questionDiv = $(".questiondiv_" + currentQuestion[0].QuestionID + " .select_product");
                    var qType = currentQuestion[0].Type.toLowerCase();
                    var value = "";

                    switch (qType) {
                        case "singlechoice":
                            {
                                value = $(questionDiv).find("select :selected").val();
                                if (value == undefined) {
                                    value = $(questionDiv).find(":radio:checked").val();
                                }

                                //In case of aspxcombobox
                                if (value == undefined) {
                                    value=$(questionDiv).find(".dropdownctr tr td:first-child input").val();
                                }

                                if (value == undefined)
                                    value = "";
                            }
                            break;
                        case "multichoice":
                            {
                                var valueArr = new Array();
                                var dropCtrl = $(questionDiv.find(".gridLookupctr"));
                                if (dropCtrl && dropCtrl.length > 0) {
                                    var ctrl = ASPxClientControl.GetControlCollection().GetByName($(dropCtrl).get(0).id);
                                    var obj = ctrl.GetValue();
                                    if (obj != null)
                                        valueArr = obj;
                                }
                                else {
                                    var checkedCtrs = questionDiv.find(":checkbox:checked");

                                    checkedCtrs.each(function (i, item) {
                                        valueArr.push($(item).parent().find("label").text());
                                    });
                                }

                                value = valueArr.join("~");
                            }
                            break;
                        case "textbox":
                            {
                                if ($(questionDiv).find("input:text").not('.hiddenCtr').length > 0) {
                                    value = $(questionDiv).find("input:text").val();
                                }
                                else if ($(questionDiv).find("textarea").length > 0) {
                                    value = $(questionDiv).find("textarea").val();
                                }
                            }
                            break;
                        case "number":
                            {
                                value = $(questionDiv).find("input:text").val();
                            }
                            break;
                        case "checkbox":
                            {
                                value = $(questionDiv).find("input:checkbox").is(":checked").toString();
                            }
                            break;
                        case "lookup":
                            {
                                var lookupCtrl = $(questionDiv.find(".lookupValueBox-dropown"));
                                var ctrl = null;

                                if (lookupCtrl && lookupCtrl.length > 0)            // lookup control is LookValueBox
                                {
                                    ctrl = ASPxClientControl.GetControlCollection().GetByName($(lookupCtrl).get(0).id + "_ListBox");
                                    var obj = ctrl.GetValueString();
                                    if (obj != null)
                                        value = obj;
                                }
                                else {
                                    // lookup control is LookupValueBoxEdit
                                    lookupCtrl = $(questionDiv.find(".lookupValueBox-edit"));

                                    if (lookupCtrl && lookupCtrl.length > 0) {
                                        ctrl = ASPxClientControl.GetControlCollection().GetByName($(lookupCtrl).get(0).id + "_BoxEditCallBack_customdropdownedit");
                                        var obj = ctrl.GetValueString();
                                        if (obj != null)
                                            value = obj;
                                    }
                                }

                                if (lookupCtrl == null || lookupCtrl == undefined) {
                                    value = $(questionDiv).find("select :selected").val();

                                    if (value == undefined) {
                                        var hdq = $(questionDiv).find(".txtDepartmentctr");
                                        if (hdq.length > 0) {
                                            value = hdq.val();
                                            isDepartmentSkip = true;
                                        }
                                    }
                                }

                                if (value != undefined && value.indexOf(";#") != -1) {
                                    var valueArr = value.split(";#");
                                    var finalValue = "";
                                    for (var index = 0; index < valueArr.length; index++) {

                                        if (!isNaN(valueArr[index])) {
                                            if (finalValue.length > 0)
                                                finalValue = finalValue + "~" + valueArr[index];
                                            else
                                                finalValue = valueArr[index];
                                        }
                                    }

                                    value = finalValue;
                                }
                            }
                            break;
                        case "requesttype":
                            {
                                var obj = ASPxClientControl.GetControlCollection().GetByName($(questionDiv).find('.field_requesttypelookup_edit').attr('id'));
                                if (obj != null && obj != undefined)
                                    value = obj.GetKeyValue();

                                if (value == null)
                                    value = '';
                            }
                            break;
                        case "userfield":
                            {
                                // this.userQuestion(sConditions, jsFirstParent, ctrName);
                            } break;
                        case "datetime":
                            {
                                value = $(questionDiv).find(".datetimectr tr td:first-child input").val();
                            }
                            break;
                        case "applicationaccessrequest": {

                        } break;
                    }

                    var isValid = cObj.testCondition(value, subItem.operator, subItem.values, subItem.qtype);

                    var logicalOperator = "";
                    if (subItem.logicalRelOperator == "AND")
                        logicalOperator = "&&";
                    else if (subItem.logicalRelOperator == "OR")
                        logicalOperator = "||";

                    var resultColItem = { "itemid": subItem.itemid, "logicalRelOperator": logicalOperator, "value": isValid, "parentid": subItem.parentid };
                    resultCollection.push(resultColItem);
                }
            });

            // Create final expression for the skip logic
            if (resultCollection != null && resultCollection.length > 0) {

                //Check if any itemid is 0, if yes then reset all itemid i.e. itemid was not set while creating the skip logic from code behind
                if (resultCollection.some(function (x) { return x.itemid == 0; })) {
                    var itemIndex = 0;
                    resultCollection.forEach(function (x) { x.itemid = ++itemIndex; });
                }

                // Get array with parentId = 0, i.e. either these array items are parent or they aren't part of any group with other items
                var rootArr = resultCollection.filter(function (x) { return x.parentid == 0; });

                for (var i = 0; i < rootArr.length; i++) {
                    var logicalRelOperator = rootArr[i].logicalRelOperator;

                    if (logicalRelOperator != "" && logicalRelOperator.toLowerCase() != "none")
                        finalExpression = finalExpression + " " + logicalRelOperator + " ";

                    // Create an array with all current arraey item and with other array items which are grouped with this
                    var subRootArr = [];
                    subRootArr.push(rootArr[i]);
                    var childItems = resultCollection.filter(function (x) { return x.parentid == rootArr[i].itemid; });
                    childItems.forEach(function (x) { subRootArr.push(x); });

                    var expressionArr = [];

                    for (var j = 0; j < subRootArr.length; j++) {
                        var expressionString = "";
                        var subRootlogicalOperator = subRootArr[j].logicalRelOperator;

                        if (expressionArr.length > 0 && subRootlogicalOperator != "" && subRootlogicalOperator.toLowerCase() != "none")
                            expressionString = subRootlogicalOperator + " ";

                        expressionString = expressionString + subRootArr[j].value;
                        expressionArr.push(expressionString);
                    }

                    if (expressionArr.length == 1)
                        finalExpression = finalExpression + " " + expressionArr.join(" ");
                    else if (expressionArr.length > 1)
                        finalExpression = finalExpression + " (" + expressionArr.join(" ") + ")";
                }
            }

            if (finalExpression != null && finalExpression != "") {
                finalExpression = finalExpression.replace(/\s+/g, ' ').trim();

                // If the finalExpression contains any operator at first place then it means first question isn't found 
                // or it isn't rendered yet, so the condition is false.
                if (finalExpression.indexOf("&&") == 0 || finalExpression.indexOf("||") == 0)
                    finalExpression = "false";

                result = eval(finalExpression);
            }

            return result;
        },
        hideShowQuestionGroupSection: function (item, displayvalue) {
            var ids = item.split(",");
            var qarr = [];
            for (var i = 0; i < ids.length; i++) {
                var quesvisiarr = [];
                var lvparentgroup = $(".questiondiv_" + ids[i]).parent();
                if (lvparentgroup != null && lvparentgroup != undefined) {
                    if (lvparentgroup.attr('containids') != undefined) {
                        var clsattach = lvparentgroup.attr('class');
                        var groupQids = lvparentgroup.attr('containids').split(',');
                        $.each(groupQids, function (currIndex, currval) {
                            var ques = lvparentgroup.find('.questiondiv_' + currval);
                            if ((ques.is(':visible') || ques.css('display')=='block') && quesvisiarr.indexOf(ques) == -1)
                                quesvisiarr.push(ques);

                        });

                        
                        lvparentgroup.removeClass('lvshowgrouping');
                        lvparentgroup.removeClass('lvhidegrouping')
                        if ((quesvisiarr == null || quesvisiarr.length == 0) && displayvalue) {
                            lvparentgroup.addClass('lvhidegrouping');
                        }
                        else {
                            lvparentgroup.addClass('lvshowgrouping');
                        }


                        //Set left section width
                        SetLeftSectionWidth();
                    }
                }
            }
        }
    }
    function FillOnUserFieldChange(ctrlID) {
        $.cookie("DependentUser", null, { path: '/' });
        var currentCtr = ctrlID;
        var requestorCtr = null;

        if ($(".userctr").length <= 0)
            return;

        $(".applicationaccessservice").each(function (index, item) {

            requestorCtr = $(item);
            if ((requestorCtr).length <= 0)
                return;

            var mirrorAccessFrom = requestorCtr.attr("mirrorAccessFrom");
            var dependent = requestorCtr.attr("dependentquestion");

            if (dependent != "" && dependent != "undefined" && dependent != undefined) {
                var userQCtr = $(".questiondiv_" + dependent + " .userctr");

                if (userQCtr != undefined && userQCtr != null) {
                    var userQCtrID = userQCtr.get(0).id;
                    if (userQCtrID != null && userQCtrID != undefined && userQCtrID != "") {
                        LoadingPanel.SetText("Loading Existing Access ..");
                        LoadingPanel.Show();
                        btnRefresh.DoClick();
                    }
                    else {
                        var userQCtr = $(".questiondiv_" + mirrorAccessFrom + " .userctr");

                        if (userQCtr != undefined && userQCtr != null && userQCtr.length > 0) {
                            var userQCtrID = userQCtr.get(0).id;
                            if (userQCtrID != null && userQCtrID != undefined && userQCtrID != "") {
                                LoadingPanel.SetText("Loading Existing Access ..");
                                LoadingPanel.Show();
                                btnRefresh.DoClick();
                            }
                        }
                    }

                }
                else {
                    var userQCtr = $(".questiondiv_" + mirrorAccessFrom + " .userctr");

                    if (userQCtr != undefined && userQCtr != null) {
                        var userQCtrID = userQCtr.get(0).id;
                        if (userQCtrID != null && userQCtrID != undefined && userQCtrID != "") {
                            LoadingPanel.SetText("Loading Existing Access ..");
                            LoadingPanel.Show();
                            btnRefresh.DoClick();
                        }
                    }
                }

            }
            else if (mirrorAccessFrom != "" && mirrorAccessFrom != "undefined" && mirrorAccessFrom != undefined) {
                var userQCtr = $(".questiondiv_" + mirrorAccessFrom + " .userctr");

                if (userQCtr != undefined && userQCtr != null) {
                    var userQCtrID = userQCtr.get(0).id;
                    if (userQCtrID != null && userQCtrID != undefined && userQCtrID != "") {
                        LoadingPanel.SetText("Loading Existing Access ..");
                        LoadingPanel.Show();
                        btnRefresh.DoClick();
                    }
                }
            }
        });

        $(".DependendentQuestionDepartment").each(function (index, item) {

            requestorCtr = $(item);
            if ((requestorCtr).length <= 0)
                return;
            var dependent = requestorCtr.attr("DependendentQuestionUser");
            if (dependent != "" && dependent != "undefined" && dependent != undefined) {
                var userQCtr = $(".questiondiv_" + dependent + " .userctr");
                if (userQCtr == undefined || userQCtr == null || userQCtr.length == 0)
                    userQCtr = $(".questiondiv_" + dependent + " .dropdownctrUsers");

                if (userQCtr.length > 0) {
                    var userQCtrID = userQCtr.get(0).id;
                    if (userQCtrID.indexOf(currentCtr) != -1) {
                        LoadingPanel.SetText("Loading Department/Division..");
                        LoadingPanel.Show();
                        btnRefresh.DoClick();
                    }
                }
            }
        });

        $(".LocationQuestionUser").each(function (index, item) {

            requestorCtr = $(item);
            if ((requestorCtr).length <= 0)
                return;
            var dependentlocation = requestorCtr.attr("LocationQuestionUser");
            if (dependentlocation != "" && dependentlocation != "undefined" && dependentlocation != undefined) {
                var userQCtr = $(".questiondiv_" + dependentlocation + " .userctr");
                if (userQCtr.length > 0) {
                    var userQCtrID = userQCtr.get(0).id;
                    if (userQCtrID.indexOf(currentCtr) != -1) {
                        LoadingPanel.SetText("Loading Location..");
                        LoadingPanel.Show();
                        btnRefresh.DoClick();
                    }
                }
            }
        });

        $(".CompanyDivisions").each(function (index, item) {
            
            requestorCtr = $(item);
            if ((requestorCtr).length <= 0)
                return;
            var dependentlocation = requestorCtr.attr("CompanyDivisions");
            if (dependentlocation != "" && dependentlocation != "undefined" && dependentlocation != undefined) {
                var userQCtr = $(".questiondiv_" + dependentlocation + " .userctr");
                if (userQCtr != "" && userQCtr != "undefined" && userQCtr != undefined) {
                    var userQCtrID = userQCtr.get(0).id;
                    if (userQCtrID.indexOf(currentCtr) != -1) {
                        LoadingPanel.SetText("Loading Department/Division..");
                        LoadingPanel.Show();
                        btnRefresh.DoClick();
                    }
                }
            }
        });
    }

    function departmentSelectOpt(objId) {
        var qustDtl = objId.split("_question_")[1];
        var questionID = "";
        if (qustDtl.split("_").length > 0) {
            questionID = qustDtl.split("_")[1];
            var question = skipObj.getCurrentQuestion(questionID);
            if (question && question.length > 0)
                questionBrachLogic($("#" + objId).get(0), "lookup", "drpdwnListDepartmentlookup", question[0].Token);
        }
    }

    function onRequestTypechange(obj) {
        requestypechange(obj);
        return false;
    }
    function changeUsers(s, e) {
        //if (typeof (cbAssets) != 'undefined' && !cbAssets.InCallback()) {
        // cbAssets.PerformCallback(s.GetSelectedItem().text);
        //}
        var requestorCtr = null;
        if ($(".dropdownctrUsers").length <= 0)
            return;
        $(".applicationaccessservice").each(function (index, item) {
            requestorCtr = $(item);
            if ((requestorCtr).length <= 0)
                return;

            var dependent = requestorCtr.attr("dependentquestion");
            if (dependent != "" && dependent != "undefined" && dependent != undefined) {
                var dropdownctrUsers = $(".questiondiv_" + dependent + " .dropdownctrUsers");
                var userQCtrID = s.name;
                var dropdownctrUsersId = dropdownctrUsers.get(0).id;
                if (dropdownctrUsersId == userQCtrID) {
                    LoadingPanel.SetText("Loading Existing Access ..");
                    LoadingPanel.Show();
                    btnRefresh.DoClick();
                }
            }
        });
    }
    function FillOnAccessRequestModeChange() {
        $(".rdblstAccessReqMode").on("change", function () {
            LoadingPanel.Show();
        })
    }

    function FillRelatedAsset(s, e) {

        $.cookie("DependentUser", null, { path: '/' });
        var requestorCtr = null;
        var peoplePicker = null;
        requestorCtr = $("#" + s.name);
        var userValue = null;
        var includedepartmentasset = null;
        if (requestorCtr.length > 0) {
            var cookieVal = "DependentUser_" + requestorCtr.attr('sectionId') + '_' + requestorCtr.attr('questionId');
            $.cookie(cookieVal, null, { path: '/' });
            includedepartmentasset = requestorCtr.attr("includedepartmentasset");
            var dependent = requestorCtr.attr("dependentquestion");
            if (dependent != "" && dependent != undefined && dependent != null) {
                var userQCtr =$(".questiondiv_" + dependent + " .userctr");
                if (userQCtr != undefined && userQCtr != null && userQCtr.length > 0) {
                    userQCtr = ASPxClientControl.GetControlCollection().GetByName(userQCtr.attr("Id"));
                    if (userQCtr != undefined && userQCtr != null)
                    {
                        userValue = userQCtr.GetValue();
                    }
                }
            }
            else {
                if (requestorCtr.attr("allassets") != undefined)
                    userValue = "all";
                else
                    userValue = requestorCtr.attr("showassetsofuser");
            }
            var assetType = "";

            if (requestorCtr.attr("assettype") != undefined)
                assetType = requestorCtr.attr("assettype");
            var cbAssets = ASPxClientControl.GetControlCollection().GetByName(s.name);
            if (typeof (cbAssets) != 'undefined' && !cbAssets.InCallback()) {
                if (userValue != undefined && userValue != "" && userValue != "&nbsp;") {
                    if (userValue != "")
                        userValue = userValue + ";#userChange;#" + includedepartmentasset;
                    if (assetType != "")
                        userValue = userValue + "~" + assetType + "~" + includedepartmentasset;
                }

                cbAssets.GetGridView().PerformCallback(userValue);
               
            }
        }
    }
    function GotSelectedValues(h) {

        if (h.length == 0)
            return false;
        var UserID = [];
        for (var i = 0; i < h.length; i++) {
            UserID.push(h[i]);
        }
        return UserID.join(',')
    }
    function FillOnRequestorChange() {
        var requestorCtr = null;
        var peoplePicker = null;

        if ($(".assetquestionservice").length > 0) {
            $(".assetquestionservice").each(function (i, item) {
                requestorCtr = $("#" + $(item).get(0).id);
                if (requestorCtr.length > 0) {
                    var dependent = requestorCtr.attr("dependentquestion");
                    if (dependent != "") {
                        var userQCtr = $(".questiondiv_" + dependent + " .userctr");
                        if (userQCtr.length > 0) {
                            var userQID = userQCtr.get(0).id;

                            $("#" + userQID + "_upLevelDiv").bind("blur", function () {
                                var peoplePicker = this;
                                var peoplepickerValue = null;
                                var userID = this.id;
                                var pickerHtml = peoplePicker.innerHTML.toLowerCase();
                                if (pickerHtml.indexOf("<span") > -1) {
                                    var userSpans = $(peoplePicker).children("span");
                                    userSpans.each(function (i, item) {
                                        peoplepickerValue = $(item).attr("title");
                                    });
                                }
                                else {
                                    peoplepickerValue = peoplePicker.innerHTML.replace("<br>", "");
                                }
                                // Get related assets for requestor

                                if ($(".assetquestionservice").length > 0) {

                                    $(".assetquestionservice").each(function (i, item) {

                                        requestorCtr = $("#" + $(item).get(0).id);
                                        var dependent = requestorCtr.attr("dependentquestion");
                                        if (requestorCtr.length > 0) {
                                            if (dependent != "") {
                                                var userQCtr = $(".questiondiv_" + dependent + " .userctr");
                                                if (userQCtr.length > 0) {
                                                    var userQID = userQCtr.get(0).id + "_upLevelDiv";
                                                    if (userQID == userID) {

                                                        var cbAssets = ASPxClientControl.GetControlCollection().GetByName(this.id)
                                                        if (typeof (cbAssets) != 'undefined' && !cbAssets.InCallback() && (peoplepickerValue != undefined && peoplepickerValue != "" && peoplepickerValue != "&nbsp;")) {
                                                            cbAssets.GetGridView().PerformCallback(peoplepickerValue + ";#userChange");
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    });
                                }
                            });

                        }

                    }
                }
            });
            // requestorCtr = $("#" + $(".assetquestionservice")[0].id);

        }

    }

    $(function () {
        skipObj = $.skipLogic();
        checkAllQuestionBranchLogic();
        $(".col_aleft_desc").shorten({ "showChars": 50, "showline": 3 });

        // FillOnRequestorChange();
        FillOnAccessRequestModeChange();
        if (!window.frameElement) {
            $("#btCancelL").hide();
        }
        else
            $("#btCancelL").show();

        SetLeftSectionWidth();
    });

    function cancelServicePopup(obj) {
        if (window.frameElement) {
            // in frame
            var sourceURL = "";
            if ($(obj).find("b").text().toLowerCase() == 'close') {
                sourceURL = "<%= Request["source"] %>";
            }
            window.parent.CloseWindowCallback(1, sourceURL);
        }
    }

    function callPrevious(obj) {
        // AddNotification("Processing ..");
        btPrevious.DoClick();
      
        LoadingPanel.Show();
    }

    function callNext(obj) {
        <%--var myHidden = document.getElementById('<%= hdnAttachmentMandatory.ClientID %>');--%>
       <%-- var myLabel = document.getElementById('<%= lbAttachment.ClientID%>');--%>
       <%-- var myattachment = document.getElementById('<%= %>');fileuploadServiceAttach.ClientID
        var myhidden = document.getElementById('<%= %>' + '_fileUpload_hiddenField');fileuploadServiceAttach.ClientID--%>

        //if (document.getElementById('hdnAttachmentMandatory') !== null && myHidden.value == "true" && myhidden.value == "") {
        //    lblerrormsg.SetText("Atleast One Attachment Required!");
        //    return;
        //}
        //else {
        btNext.DoClick();
        //}
        //$("#<%=btNext.ClientID %>").get(0).click();
        if (Page_IsValid) {
            //AddNotification("Processing ..");
            LoadingPanel.Show();
        }
    }
    function callReturnRequest(obj) {
        var validate = true;
        try {
            validate = Page_ClientValidate();
        } catch (ex) { }

        if (validate) {
            commentsReturnPopup.Show();
        }
    }
    function validateFeedbackForm(obj) {
        if (obj.getAttribute("Name") == "Return") {
            if ($('#<%=popedReturnComments.ClientID%>').val() == "") {
                $('#<%=lblReturnMessage.ClientID%>').html("Please enter comment");
                return false;
            }

            commentsReturnPopup.Hide();
            LoadingPanel.Show();
            btReturn.DoClick();
        }
        else if (obj.getAttribute("Name") == "Reject") {
            if ($('#<%=popedRejectComments.ClientID%>').val() == "") {
                $('#<%=lblRejectMessage.ClientID%>').html("Please enter comment");
                return false;
            }
            commentsRejectPopup.Hide();
            LoadingPanel.Show();
            btReject.DoClick();
        }
        return false;
    }


    function callRejectRequest(obj) {
        var validate = true;
        try {
            validate = Page_ClientValidate();
        } catch (ex) { }

        if (validate) {
            commentsRejectPopup.Show();
        }
    }
    function callApproveRequest(obj) {
        var validate = true;
        try {
            validate = Page_ClientValidate();
        } catch (ex) { }

        if (validate) {
            LoadingPanel.Show();
            btApprove.DoClick();
        }
        return false;
    }
    function callCreateRequest(obj) {

        var validate = true;
        try {
            validate = Page_ClientValidate();
        } catch (ex) { }

        if (validate) {
            LoadingPanel.Show();
            btCreateRequest.DoClick();
        }
    }

    function questionBrachLogic(obj, qtype, ctrName, token) {
        if (skipObj != null && skipObj!=undefined)
            skipObj.executeSkipLogic(obj, qtype, ctrName, token);
        else {
            skipObj = $.skipLogic();
            skipObj.executeSkipLogic(obj, qtype, ctrName, token);
        }
    }

    function converStringToBoolean(val) {
        val = val.toLowerCase();
        if (val == "yes" || val == "true" || val == "on") {
            return true;
        }
        else {
            return false;
        }
    }

    function checkAllQuestionBranchLogic() {

        var questionContainers = $(".select_product");
        var selectCtrs = questionContainers.find("select");
        $.each(selectCtrs, function (i, item) {

            if ($(item).attr("onchange") && $(item).attr("onchange").indexOf("requestTypeCategoryChange") != -1) {
                return;
            }

            if (item.fireEvent) {
                //This works for IE generally
                item.fireEvent("onchange")
            }
            else {
                $(item).trigger("change");
            }

            //Hide Error message when dropdown change event is triggered on onload
            //Error message only come when user click on next or change value

            var errorCtr = $(item).parent().find(".errormsg-container");
            if (errorCtr.length > 0) {
                errorCtr.hide();
            }
        });

        var checkboxCtrs = questionContainers.find("input:checkbox");
        $.each(checkboxCtrs, function (i, item) {
            // questionBrachLogic(item,''++'','radiobuttonlist',\"{1}\"
            $(item).blur();
        });

        var radioCtrs = questionContainers.find("input:radio");
        $.each(radioCtrs, function (i, item) {
            $(item).blur();
        });

        var txtCtrs = questionContainers.find("input:text").not('.hiddenCtr');
        $.each(txtCtrs, function (i, item) {

            if ($(item).hasClass('datetimectr')) {

                // 
                var currentValue = $($(item).get(0)).val();
                var hiddenValue = $($($(item).parents('div.select_product'))).find('.hiddenCtr').val();
                if (currentValue != hiddenValue)
                    $(item).change();
                $(item).get(0).onvaluesetfrompicker = function () {

                    $(item).change();
                }
            }
            $(item).keyup();
        });

        var txtArea = questionContainers.find("textarea");

        $.each(txtArea, function (i, item) {

            $(item).change();
        });
    }

    function openEditDialog() {

        var url = '<%=detailsUrl %>';
        window.parent.UgitOpenPopupDialog(url, "", 'Service Help', '1000px', '90', 0, escape("<%= Request.Url.AbsolutePath %>"));
        return false;
    }


    function RemoveWaterMark(obj, helpText) {
        if ($(obj).val() == helpText) {
            $(obj).removeClass("lightText").val("");
        }
    }

    function AddWaterMark(obj, helpText) {
        if ($(obj).val() == "") {
            $(obj).val(helpText).addClass("lightText");
        }
    }
    function requestypechange(obj) {

        if (!stopRequestTypeCallback) {
            var selectedVal = "RequestType=" + $(obj).val();
            var categoryDropdown = $("#" + $(obj).parent().attr("id").replace("_aspxpanel", "_category"));
            if (categoryDropdown.length > 0) {
                selectedVal = categoryDropdown.val();
                if (selectedVal == "") {
                    var categories = categoryDropdown.find("option").not("[value *= ';#'],[value = '']");
                    var cVals = new Array();
                    categories.each(function (i, item) {
                        cVals.push($(item).attr("value"));
                    });
                    selectedVal = cVals.join("**");
                }
            }

            try {

                var callbackPanel = ASPxClientControl.GetControlCollection().GetByName($(obj).parent().attr("id"));
                if (callbackPanel != null && !callbackPanel.InCallback())
                    callbackPanel.PerformCallback(selectedVal);

            }
            catch (ex) {

            }
        }
        stopRequestTypeCallback = false;
    }

    function requestTypeCategoryChange(obj) {

        var categoryVal = $(obj).val();
        if (categoryVal == "") {
            var categories = $(obj).find("option").not("[value *= ';#'],[value = '']");
            var cVals = new Array();
            categories.each(function (i, item) {
                cVals.push($(item).attr("value"));
            });
            categoryVal = cVals.join("**");
        }

        try {
            var callbackPanel = ASPxClientControl.GetControlCollection().GetByName($(obj).parent().attr("id") + "_aspxpanel")

            if (callbackPanel != null && !callbackPanel.InCallback()) {
                callbackPanel.PerformCallback(categoryVal);
            }
        } catch (ex) {
        }

    }
    var stopRequestTypeCallback = false;
    function requestTypeCallBackPanel_OnEndCallback(s, e) {
        stopRequestTypeCallback = true;
        $(".requestTypeCallBackPanel_loadingPanel").hide()
        $("#" + s.name).find("select").trigger("change");
    }

    function closeIssueTypeOptions(s, e) {
        try {
            if (event.srcElement.tagName.toLowerCase() == "textarea" || event.srcElement.tagName.toLowerCase() == "input") {
                $("#" + event.srcElement.id).trigger("focus");
            }
        } catch (ex) { }
    }

    function SetLeftSectionWidth() {
        var containerheight = $("#<%=divMainContainer.ClientID%>").height();
        var divsetctionHeight = $("#<%=divSetSectionNew.ClientID %>").height();
        if (divsetctionHeight < containerheight) {
            $("#<%=divSetSectionNew.ClientID %>").css("height", containerheight);
        }
    }
</script>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    //multilookup control js::start
    var textSeparator = ";";

     function onlocationChange(obj, qtype, ctrName, token) {
        filterSubLocation(obj);
        questionBrachLogic(obj, qtype, ctrName, token);

    }
    function OnListBoxSelectionChanged(listBox, args, dropdwnctrlId, checkboxlistId, questionType, questionTokenName) {
        var dropdObj = ASPxClientControl.GetControlCollection().GetByName(dropdwnctrlId);
        var checkboxlistObj = ASPxClientControl.GetControlCollection().GetByName(checkboxlistId);
        if (args.index == 0)
            args.isSelected ? listBox.SelectAll() : listBox.UnselectAll();
        updateCheckBoxSelectAllItemState(checkboxlistObj);
        updateDropdwnText(dropdObj, checkboxlistObj);

        //control mandatoery validation 

        questionBrachLogic(dropdObj, questionType, 'drpdwnListmutilookup', questionTokenName);

    }
    function updateCheckBoxSelectAllItemState(checkboxlistObj) {
        isAllSelectedinCheckBxList(checkboxlistObj) ? checkboxlistObj.SelectIndices([0]) : checkboxlistObj.UnselectIndices([0]);
    }
    function isAllSelectedinCheckBxList(checkboxlistObj) {
        var selectedDataItemCount = checkboxlistObj.GetItemCount() - (checkboxlistObj.GetItem(0).selected ? 0 : 1);
        return checkboxlistObj.GetSelectedItems().length == selectedDataItemCount;
    }
    function updateDropdwnText(dropdObj, checkboxlistObj) {
        var selectedItems = checkboxlistObj.GetSelectedItems();
        var selectedItemsCount = checkboxlistObj.GetSelectedItems().length;
        var totalItemCount = checkboxlistObj.GetItemCount();
        if (parseInt(totalItemCount) == parseInt(selectedItemsCount))
            dropdObj.SetText("All");
        else
            dropdObj.SetText(getChexkBoxListSelectedItemsText(selectedItems));
    }

    function getChexkBoxListSelectedItemsText(items) {
        var texts = [];
        for (var i = 0; i < items.length; i++)
            if (items[i].index != 0)
                texts.push(items[i].text);
        return texts.join(textSeparator);
    }

    //onInit handel:: start
    function OnInitListBox(listBox, args, dropdwnctrlId, checkboxlistId) {
        var dropdObject = ASPxClientControl.GetControlCollection().GetByName(dropdwnctrlId);
        var checkboxlistObject = ASPxClientControl.GetControlCollection().GetByName(checkboxlistId);
        updateDrpDwnTextOnInit(dropdObject, checkboxlistObject);
        //control mandatoery validation 
        questionBrachLogic(dropdObj, questionType, 'drpdwnListmutilookup', questionTokenName);
    }
    function updateDrpDwnTextOnInit(dropdObj, checkboxlistObj) {
        var selectedItems = checkboxlistObj.GetSelectedItems();
        var selectedItemsCount = checkboxlistObj.GetSelectedItems().length;
        var totalItemCount = checkboxlistObj.GetItemCount();
        if (parseInt(totalItemCount) == parseInt(selectedItemsCount) + 1) {
            dropdObj.SetText("All");
            checkboxlistObj.SelectAll();
        }
    }
    //onInit handel:: end

    //multilookup control js::end


    function ug_MaximiseQuestion(obj) {
        var parent = $(obj).parent();
        parent.find(".maximise").hide();
        parent.find(".minimise").show();
        var parent = $(obj).parents('.question:eq(0)');
        parent.addClass('ug-maximize-block');

        try {
            initialiseMatrix();
        } catch (ex) { }
    }

    function ug_minimiseQuestion(obj) {
        var parent = $(obj).parent();
        parent.find(".maximise").show();
        parent.find(".minimise").hide();
        $(obj).parents('.question:eq(0)').removeClass('ug-maximize-block');

        try {
            initialiseMatrix();
        } catch (ex) { }
    }
    $("#upload_link").on('click', function (e) {
        e.preventDefault();
        $("#fileuploadServiceAttach:hidden").trigger('click');
    });
    function ShowSectionContainer() {
        $("#<%=divSetSection.ClientID %>").css("display", "none");
        $("#<%=divSetSectionNew.ClientID %>").css("display", "block");

        $("#<%=divSetSectionNew.ClientID %>").css("float", "left");
        // var tempheight = $(window).height();

        $("#<%=divHideSectionContainer.ClientID %>").css("display", "block");
        $("#<%=divHideSectionContainer.ClientID %>").css("float", "left");


        $("#<%=divShowSectionContainer.ClientID %>").css("display", "none");

        var tempheight = $("#<%=divMainContainer.ClientID%>").height();
        $("#<%=divShowSectionContainer.ClientID %>").css("height", tempheight - 90);
        $("#<%=Image1.ClientID %>").css("margin-top", ((tempheight - 100) / 2));

        $("#<%=divHideSectionContainer.ClientID %>").css("height", tempheight - 90);
        $("#<%=img2.ClientID %>").css("margin-top", ((tempheight - 100) / 2));



        var containerheight = $("#<%=divMainContainer.ClientID%>").height();
        $("#<%=divSetSectionNew.ClientID %>").css("height", containerheight);

        if (!$("#<%=divSetSectionNew.ClientID %>").hasClass("DivThemeClass"))
            $("#<%=divSetSectionNew.ClientID %>").addClass("DivThemeClass");

        $("#<%=colrightdescspan.ClientID %>").css("display", "none");


        $("#<%=divHideSectionContainer.ClientID %>").css("display", "block");
        $("#<%=divHideSectionContainer.ClientID %>").css("float", "left");


        $("#<%=divShowSectionContainer.ClientID %>").css("display", "none");


        //delete_cookie("SelectedFolderID");
        set_cookie('SectionContainer', "Show", null,  _spPageContextInfo.webServerRelativeUrl);

        return false;
    }

    function HideSectionContainer() {
        var tempheight = $("#<%=divMainContainer.ClientID%>").height();
        $("#<%=divShowSectionContainer.ClientID %>").css("height", tempheight - 90);
        $("#<%=Image1.ClientID %>").css("margin-top", ((tempheight - 100) / 2));

        $("#<%=divHideSectionContainer.ClientID %>").css("height", tempheight - 90);
        $("#<%=img2.ClientID %>").css("margin-top", ((tempheight - 100) / 2));


        $("#<%=divSetSection.ClientID %>").css("display", "none");
        $("#<%=divSetSectionNew.ClientID %>").css("display", "none");


        $("#<%=colrightdescspan.ClientID %>").css("display", "none");

        $("#<%=divHideSectionContainer.ClientID %>").css("display", "none");
        $("#<%=divShowSectionContainer.ClientID %>").css("display", "block");
        $("#<%=divShowSectionContainer.ClientID %>").css("float", "left");

        set_cookie('SectionContainer', "Hide", null, _spPageContextInfo.webServerRelativeUrl);
        return false;
    }

    $(document).ready(function () {
        
        var tempheight = $("#<%=divMainContainer.ClientID%>").height();
        $("#<%=divShowSectionContainer.ClientID %>").css("height", tempheight - 90);
        $("#<%=Image1.ClientID %>").css("margin-top", ((tempheight - 100) / 2));

        $("#<%=divHideSectionContainer.ClientID %>").css("height", tempheight - 90);
        $("#<%=img2.ClientID %>").css("margin-top", ((tempheight - 100) / 2));

        var containerheight = $("#<%=divMainContainer.ClientID%>").height();
        $("#<%=divSetSectionNew.ClientID %>").css("height", containerheight);
        //removes the broder on last rendered question div
        $('*[id*=questionDiv]:visible:last').attr('style', 'border-bottom: 0px solid #dee2e3; !important');
    });

    $(document).ready(function () {
        //$(".divCLSSetSectionNew").show();
        $(".divCLSSetSectionNewCopy").hide();
        $(".divCLSSetSectionNew").click(function () {
            $(".divCLSSetSectionNewCopy").show();
            $(".divCLSSetSectionNew").hide()
        });

        $(".divCLSSetSectionNewCopy").click(function () {
            $(".divCLSSetSectionNew").show();
            $(".divCLSSetSectionNewCopy").hide();
        });
    });

    $(document).ready(function () {
        $('.userValueBox-Table').parent().addClass("userValueBox-searchFilterWrap");
        $('.userValueBox-searchFilterWrap').parent().addClass("userValueBox-searchFilterContainer");
        $('.userValueBox-searchFilterContainer').parents().eq(3).addClass('userValueBox-dropDownWrap');
    });

</script>
<div id="helpcardpopup"></div>
<dx:ASPxLoadingPanel ID="LoadingPanel" runat="server" Text="Please Wait..." CssClass="customeLoader" ClientInstanceName="LoadingPanel"
    Modal="True">
</dx:ASPxLoadingPanel>
<asp:Panel ID="pSkipConditions" runat="server" CssClass="pskipconditions hide"></asp:Panel>
<asp:Panel ID="pSectionQuestion" runat="server" CssClass="pSectionQuestion hide"></asp:Panel>
<asp:Panel ID="conditionalPanel" runat="server">
    <style type="text/css">
        body #s4-ribbonrow {
            display: none;
        }

        .lightText {
            color: #9B9B9B;
            font-size: 8pt;
        }
    </style>
</asp:Panel>

<div class="main_container" style="display: none;">
    <div class="header-bottom">
        <div class="second_tier_nav">
            <ul>
                <li>
                    <asp:Label runat="server" ID="step1" CssClass="active" Text="Pick a Service"></asp:Label></li>
                <li>
                    <asp:Label runat="server" ID="step2" Text="Enter Information"></asp:Label></li>
                <li>
                    <asp:Label runat="server" ID="step3" Text="Create Request"></asp:Label></li>
            </ul>
        </div>
    </div>
</div>
<div class="main_container">
        <span class="col_aright_desc" id="colrightdescspan" runat="server">
            <span class="col_aright_imgs">
                <asp:Image ID="imgServiceIcon" runat="server" ImageUrl="/Content/images/Services.png" /></span>
            <span class="col_aright_imgt">
                <asp:Literal ID="lServiceTitle" runat="server" Text="Service Catalog"></asp:Literal></span>
        </span>
        <span id="newServiceCategory" runat="server" style="float: left; width: 80px; text-align:center;">
            <span class="col_aright_imgs ml-4">
                <asp:Image ID="Image2" runat="server" ImageUrl="/Content/Images/Services.png" />
            </span>

            <span class="col_aright_imgt" style="vertical-align: -webkit-baseline-middle;">
                <asp:Literal ID="litServiceTitle" runat="server" Text="Service Catalog"></asp:Literal></span>
        </span>
        <asp:Label ID="sectionDescription" runat="server" CssClass="col_aleft_desc"></asp:Label>
        <span class="col_aright_ticketid" id="spanTicketId" runat="server" visible="false">Ticket Id:
            
        </span>
        <span class="col_aleft_help">
            <asp:Image ID="imgHelp" ToolTip="Click Here For Help" runat="server" ImageUrl="/Content/images/help_22x22.png" onclick="openEditDialog()" />
        </span>
</div>

<div class="content row" runat="server" id="divMainContainer">

    <div id="divSetSectionNew" runat="server" class="divCLSSetSectionNew DivThemeClass" style="float: left; min-height: 450px;">
        <asp:ListView ID="lvStepSectionsNew" runat="server" ItemPlaceholderID="PlaceHolder1" DataKeyNames="Key">
            <LayoutTemplate>
                <div style="width: 75px;">
                    <ul style="margin: 15px 0px 0px 0px; list-style-type: none;">
                        <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
                    </ul>
                </div>
            </LayoutTemplate>
            <ItemTemplate>
                <li style="margin: 0px 0px 0px 0px;">
                    <div id="divnewsection" runat="server" style="width: 70px; float: right; margin-bottom: 10px;">
                        <div style="text-align: center;">
                            <p id="stepIcon" class="step_number" runat="server">
                            <dx:ASPxImage ID="img2" ClientInstanceName="img2" runat="server" Width="20" Height="20" ImageUrl='/Content/Images/circle_grey48x48.png'>
                            </dx:ASPxImage>
                            </p>
                        </div>
                        <div class="clsservicesec" style="text-align: center; word-wrap: break-word; min-height: 50px; line-height: 1;">
                            <asp:HiddenField runat="server" ID="newsectionId" Value='<%# Eval("Key") %>' />
                            <dx:ASPxLabel runat="server" ClientInstanceName="newsectionSideBarContainer" ID="newsectionSideBarContainer" Text='<%# Eval("Value") %>' Style="font-size: 11px;"></dx:ASPxLabel>
                        </div>
                    </div>
                </li>
            </ItemTemplate>
        </asp:ListView>
    </div>
    <div id="divSetSectionNewCopy" runat="server" class="divCLSSetSectionNewCopy" style="float: left; min-height: 450px; display: none;">
        <asp:ListView ID="lvStepSectionsNewcopy" runat="server" ItemPlaceholderID="PlaceHolder2" DataKeyNames="Key">
            <LayoutTemplate>
                <div style="text-align: center; width: 25px;">
                    <ul style="list-style: none; display: inline;">
                        <asp:PlaceHolder ID="PlaceHolder2" runat="server"></asp:PlaceHolder>
                    </ul>
                </div>
            </LayoutTemplate>
            <ItemTemplate>
                <li style="margin: 0px 0px 0px 0px;">
                    <div id="divnewsectionhide" runat="server" style="margin-bottom: 10px;">
                        <div>
                            <p id="stepIcon" class="step_number" runat="server">
                            <dx:ASPxImage ID="img2hide" ClientInstanceName="img2hide" runat="server" ToolTip='<%# Eval("Value") %>' Width="20" Height="20" ImageUrl='/Content/Images/circle_grey48x48.png'>
                            </dx:ASPxImage>
                            </p>
                        </div>
                        <small>
                        <div style="min-height: 50px;">
                            <asp:HiddenField runat="server" ID="newsectionIdhide" Value='<%# Eval("Key") %>' />
                        </div>
                        </small>
                    </div>
                </li>
            </ItemTemplate>
        </asp:ListView>
    </div>
    
    <div style="float: left;" id="divShowHideArrow" runat="server" visible="false">
        <div runat="server" style="display: inline; float: left; border-right: 0px; min-height: 350px;" id="divShowSectionContainer">
            <asp:Image ID="Image1" runat="server" src='/Content/Images/uGovernIT/triangle-right.png' ToolTip="Show Sections" Width="15px" alt="" Style="cursor: pointer;" onclick="ShowSectionContainer();" />
        </div>
        <div runat="server" style="display: inline; float: left; border-right: 0px; min-height: 350px;" id="divHideSectionContainer">
            <asp:Image ID="img2" runat="server" src='/Content/Images/uGovernIT/uGovernIT/triangle-left.png' ToolTip="Hide Sections" Width="15px" alt="" Style="cursor: pointer;" onclick="HideSectionContainer();" />
        </div>
    </div>

    <div id="divSetSection" runat="server">
        <asp:ListView ID="lvStepSections" runat="server" ItemPlaceholderID="PlaceHolder1" DataKeyNames="Key">
            <LayoutTemplate>
                <div class="col_a">
                    <ul>
                        <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
                    </ul>
                </div>
            </LayoutTemplate>
            <ItemTemplate>
                <li>
                    <asp:HiddenField runat="server" ID="sectionId" Value='<%# Eval("Key") %>' />
                    <asp:Label runat="server" ID="sectionSideBarContainer" Text='<%# Eval("Value") %>'></asp:Label>
                </li>
            </ItemTemplate>
        </asp:ListView>
    </div>
    
    <asp:Panel ID="pStep1Section1Container" CssClass="row" runat="server">
        <%--<div class="col-md-2 col-sm-1 hidden-xs"></div>--%>
        <div class="select-style col-md-4 col-sm-5 col-xs-12">
            <label class="field-label" for="usr">Service Category</label>
            <asp:DropDownList runat="server" CssClass="aspxDropDownList SVCaspxDropDownList" ID="serviceCategoryDD"
                AutoPostBack="true" OnSelectedIndexChanged="ServiceCategoryDD_SelectedIndexChanged">
            </asp:DropDownList>
        </div>
        <div class="select-style col-md-4 col-sm-5 col-xs-12" id="serviceContainer" runat="server">
            <label class="field-label" for="usr">Choose Service</label>
            <asp:DropDownList runat="server" ID="serviceTypeDD" CssClass="aspxDropDownList SVCaspxDropDownList"></asp:DropDownList>
            <asp:RequiredFieldValidator ID="rfvServiceTypeDD" runat="server" ControlToValidate="serviceTypeDD" ErrorMessage="Please select a service." Display="Dynamic"></asp:RequiredFieldValidator>
        </div>
    </asp:Panel>

    <asp:Panel ID="pStep2SectionContainer" runat="server" CssClass="col_b ques-answer rightSection">
        <div class="ques-answer row">
           <asp:ListView ID="lvgroupquestions" runat="server" ItemPlaceholderID="PlaceHolder2" DataKeyNames="ID"
               OnItemDataBound="lvgroupquestions_ItemDataBound">
               <LayoutTemplate>
                    <asp:PlaceHolder ID="PlaceHolder2" runat="server"></asp:PlaceHolder>
                </LayoutTemplate>
                <ItemTemplate>
                    <div id="divgroupquestions" runat="server">
                       <%-- <div>--%>
                            <asp:ListView ID="lvQuestions" runat="server" ItemPlaceholderID="PlaceHolder1" DataKeyNames="ID" OnItemDataBound="LVQuestions_ItemDataBound">
                            <LayoutTemplate>
                                <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <div class="question fleft w_92" id="questionDiv" runat="server">
                                    <asp:Panel CssClass="fright questionActionPanel" ID="questionActionPanel" runat="server" Visible="false">
                                        <img onclick="ug_MaximiseQuestion(this);" title="Maximise" class="maximise" src="/Content/Images/maximize-icon12x12.png" />
                                        <img onclick="ug_minimiseQuestion(this);" style="display: none;" class="minimise" src="/Content/Images/close-red-big.png" />
                                    </asp:Panel>
                                    <div class="fleft col-md-12 col-sm-12 col-xs-12">
                                        <div class="fleft row noPadding <%#  (Eval("Helptext") != null && Eval("Helptext").ToString().Trim() != string.Empty) ? "helpblock111" : string.Empty %>">
                                            <b class="questionlb" id="questionlbid" runat="server"></b>
                                            <span class="qwidth">
                                                <asp:Label CssClass="questiontxt111" runat="server" ID="testText" Text='<%# Eval("QuestionTitle") %>'></asp:Label><%--Text='<%# Eval("QuestionTitle") %>'--%>
                                                <asp:Image ID="imgquestionHelp" ToolTip="Click Here For Help" runat="server" ImageUrl="/Content/Images/help_22x22.png" />
                                                <asp:HiddenField ID="hdnQuestionHelp" runat="server" Value='<%# Eval("NavigationUrl") %>' /><%--Value='<%# Eval("NavigationUrl") %>' --%>
                                            </span>
                                            <div id="Div1" runat="server" visible='<%# Eval("Helptext") != null && Eval("Helptext").ToString().Trim() != string.Empty?true:false%>'>
                                                <span class="lightText"><%# Eval("Helptext").ToString().Replace("\r\n", "<br/>").Replace("\n", "<br/>")%></span>
                                            </div>
                                            <span class="helpwidth">
                                                <span style="display: none;" class="helptexthtml11"><%# (Eval("Helptext") != null && Eval("Helptext").ToString().Trim() != string.Empty) ? Eval("Helptext").ToString().Trim() : string.Empty%></span>
                                            </span>
                                        </div>
                                        <div class="question_answermain col-md-12 col-sm-12 col-xs-12 noPadding">
                                            <asp:Panel runat="server" ID="questionObject" CssClass="select_product fleft"></asp:Panel>
                                        </div>
                                    </div>
                                </div>
                            </ItemTemplate>
                            </asp:ListView>
                        <%--</div>--%>
                    </div>
                </ItemTemplate>
           </asp:ListView> 

            
        </div>
    </asp:Panel>

    <asp:Panel ID="pStep2SectionSContainer" runat="server" CssClass="col_b">
        <div class="ques-answer">
            <asp:Repeater runat="server" ID="rSummaryTable" OnItemDataBound="RSummaryTable_ItemDataBound">
                <ItemTemplate>
                    <div class="summary-section">
                        <b>&nbsp;&nbsp;&nbsp;Section:
                       
                        <asp:Label ID="lbSection" runat="server"></asp:Label></b>
                    </div>
                    <div class="summary-sectiondetail">
                        <asp:Repeater runat="server" ID="groupQuesRepeater" OnItemDataBound="groupQuesRepeater_ItemDataBound">
                            <ItemTemplate>
                                <div id="divgroupquessummary" class="clsquesSummary" runat="server">
                                    <asp:Repeater runat="server" ID="rSummaryioQuest" OnItemDataBound="RSummaryioQuest_ItemDataBound">
                                        <ItemTemplate>
                                             <div class="question" runat="server" id="summaryQuestionDiv">
                                                <span>
                                                    <b class="questionlb" id="questionlbidSummary" runat="server"></b>
                                                    <span class="qwidth">
                                                        <asp:Label ID="lbQuestion" runat="server"></asp:Label>
                                                    </span>
                                                </span>
                                                <div class="select_product">
                                            <asp:Label ID="lbQuestionVal" runat="server"></asp:Label>
                                        </div>
                                             </div>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                        
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </asp:Panel>

    <asp:Panel ID="pStep3Section1Container" runat="server" CssClass="col_b">
        <div class="ques-answer">
            <div class="question">
                <%--<div class="select_product">--%>
                <div>
                    <asp:TreeView runat="server" ID="summaryTree" ShowLines="true" ExpandDepth="1">
                        <Nodes>
                        </Nodes>
                    </asp:TreeView>
                </div>
            </div>
        </div>
    </asp:Panel>

    <style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
        .attachment-container {
            float: left;
            width: 100%;
            padding-top: 8px;
        }

        .attachment-container .label {
            float: left;
            /*padding-left: 200px;*/
            padding-right: 10px;
        }

        .attachment-container .attachmentform {
            width: auto;
            overflow: hidden;
        }

        .attachmentform .oldattachment, .attachmentform .newattachment {
            float: left;
            width: 100%;
        }

        .attachmentform .fileupload {
            float: left;
            width: 100%;
        }

        .fileupload span, .fileread span {
            float: left;
        }

        .fileupload label, .fileread label {
            float: left;
            padding-left: 5px;
            font-weight: bold;
            padding-top: 3px;
            cursor: pointer;
        }

        .attachmentform .fileread {
            float: left;
            width: 100%;
        }

        .attachment-container .addattachment {
            float: left;
        }

        .attachment-container .addattachment img {
            border: 1px outset;
            padding: 3px;
        }
    </style>
    <%--<asp:HiddenField ID="hdnAttachmentMandatory" runat="server" ClientIDMode="Static" Value="false" />--%>
    <div class="label">
        <span style="display: none;">
            <asp:DropDownList ID="ddlExistingAttc" runat="server"></asp:DropDownList>
            <asp:TextBox ID="txtDeleteFiles" runat="server"></asp:TextBox>
        </span>
    </div>
    <div id="divpAttachmentContainer" runat="server" style="float:left;padding-left:23px;">
        <div id="attachmentformMandatory">
                <asp:HiddenField ID="hdnAttachmentMandatory" runat="server" Value="false" />
                <asp:CustomValidator ID="cvAttachment" runat="server" ControlToValidate="txtDeleteFiles"
                    ErrorMessage="Please attach at least one file." Display="Dynamic" ValidateEmptyText="true" CssClass="errormsg-container" OnServerValidate="cvAttachment_ServerValidate">
                </asp:CustomValidator>
            </div>
        <asp:Panel ID="pAttachmentContainer" runat="server" CssClass="attachment-container" Visible="true">
            <div class="fleft full_width ">
                <b id="attachquestionlbid" class="questionlb"></b>
                <span class="qwidth">
                    <span id="attchtestText" class="questiontxt111">Attachments</span>
                </span>
            </div>
            <div class="add-file-doc">
                <asp:Panel ID="pNewAttachment" runat="server" CssClass="newattachment">
                    <ugit:FileUploadControl ID="fileuploadServiceAttach" CssClass="fileUploadIcon" runat="server" controlMode="Edit" EnableViewState="true" />
                </asp:Panel>
            </div>
        </asp:Panel>
    </div>
    <div id="recaptchaWrap" runat="server">
        <div>
            <div class="recaptcha-container">
                <div class="recaptcha-wrap">
                    <dx:ASPxCaptcha ID="Captcha" ClientInstanceName="Captcha" runat="server" TextBox-Position="Bottom" CssClass="custom-recaptcha" ChallengeImage-Height="55"
                        TextBoxStyle-CssClass="recaptcha-textBox" TextBoxStyle-Width="250" ChallengeImage-BackgroundColor="White" ChallengeImage-Width="250">
                        <ValidationSettings SetFocusOnError="true" ErrorDisplayMode="Text" />
                        <ClientSideEvents Init="function(s,e){OnInit(s,e);}" />
                    </dx:ASPxCaptcha>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
        function OnInit(s, e) {
            ASPxClientUtils.AttachEventToElement(this, "keydown", function (event) {
                if (event.key == 'Enter')
                    callCreateRequest();
            });
        }

        function addAttachment() {
            var uploadFiles = $(".attachment-container .fileitem");
            var uploadContainer = $(".attachment-container .newattachment");
            var addIcon = $(".attachment-container .addattachment");

            if (uploadFiles.length <= 5) {
                if (uploadFiles.length == 4) {
                    addIcon.css("visibility", "hidden");
                }

                uploadContainer.append('<div class="fileitem fileupload"><span><input type="file" name="pAttachment1" /></span><label onclick="removeAttachment(this)"><img src="/Content/Images/delete-icon.png" alt="Delete"/></label></div>');
            }
        }

        function removeAttachment(obj) {

            var fileName = $(obj).find("span").text()
            var deleteCtr = $(".attachment-container .label :text");
            deleteCtr.val(deleteCtr.val() + "~" + fileName);

            var addIcon = $(".attachment-container .addattachment");
            $(obj).parent().remove();
            addIcon.css("visibility", "visible");
        }


    </script>
    <style>
        .btnText {
            position: relative;
            float: left;
            top: -3px;
        }

        .btnImg {
            position: relative;
            float: left;
            top: -3px;
            left: 2px;
        }
    </style>
    <asp:Panel ID="pActions" runat="server" CssClass="pactions">

        <dx:ASPxButton ID="btCancelL" CssClass="svcCancelBtn secondary-cancelBtn" ClientInstanceName="btCancelL" Text="Cancel" runat="server"
                OnClick="BtCancel_Click">
            <ClientSideEvents Click="cancelServicePopup" />
         </dx:ASPxButton>
        <dx:ASPxButton ID="btCreateRequestL" CssClass="primary-blueBtn" ClientInstanceName="btCreateRequestL" Visible="false" runat="server"
                Text="Submit" AutoPostBack="false">
            <ClientSideEvents Click="callCreateRequest" />
        </dx:ASPxButton>
        <dx:ASPxButton ID="btReturnL" runat="server" ClientInstanceName="btReturnL" Visible="false" Text="Return" AutoPostBack="false">
            <ClientSideEvents Click="callReturnRequest" />
        </dx:ASPxButton>

        <dx:ASPxButton ID="btRejectL" ClientInstanceName="btRejectL"  runat="server" Visible="false" Text="Reject">
            <ClientSideEvents Click="callRejectRequest" />
        </dx:ASPxButton>

        <dx:ASPxButton ID="btApproveL" ClientInstanceName="btApproveL" runat="server" Visible="false" Text="Approve" AutoPostBack="false">
            <ClientSideEvents Click="callApproveRequest" />
        </dx:ASPxButton>

        <dx:ASPxButton ID="btNextL" runat="server" ClientInstanceName="btNextL" CssClass="primary-blueBtn" Text="Next >>" AutoPostBack="false">
            <ClientSideEvents Click="callNext" />
        </dx:ASPxButton>
        <dx:ASPxButton ID="btPreviousL" runat="server" CssClass="primary-blueBtn" ClientInstanceName="btPreviousL" Text="<< Previous" AutoPostBack="false">
            <ClientSideEvents Click="callPrevious" />
        </dx:ASPxButton>

        <dx:ASPxButton ID="btNext" ClientInstanceName="btNext" CssClass="hide" runat="server" OnClick="BtNext_Click" CausesValidation="true" AutoPostBack="false"></dx:ASPxButton>
        <dx:ASPxButton ID="btPrevious" ClientInstanceName="btPrevious" ValidationGroup="previous" CssClass="hide" runat="server" OnClick="BtPrevious_Click"></dx:ASPxButton>
        <dx:ASPxButton ID="btCreateRequest" ClientInstanceName="btCreateRequest" CssClass="hide" runat="server" OnClick="BtCreateRequest_Click"></dx:ASPxButton>
        <dx:ASPxButton ID="btApprove" ClientInstanceName="btApprove" CssClass="hide" runat="server" OnClick="btApprove_Click"></dx:ASPxButton>
        <dx:ASPxButton ID="btReject" ClientInstanceName="btReject" CssClass="hide" runat="server" OnClick="btReject_Click"></dx:ASPxButton>
        <dx:ASPxButton ID="btReturn" ClientInstanceName="btReturn" CssClass="hide" runat="server" OnClick="btReturn_Click" AutoPostBack="false"></dx:ASPxButton>

        <dx:ASPxButton ID="btnRequestType" ClientInstanceName="btnRequestType" ClientVisible="false" AutoPostBack="true" runat="server" OnClick="btnRequestType_Click" ValidationGroup="requesttype"></dx:ASPxButton>
        <dx:ASPxButton ID="btnRefresh" ClientInstanceName="btnRefresh" ClientVisible="false" AutoPostBack="true" runat="server" OnClick="btnRefresh_Click" ValidationGroup="refresh"></dx:ASPxButton>
        <asp:HiddenField ID="hfCurrentStepID" runat="server" Value="1" />
        <asp:HiddenField ID="hfCurrentStepSectionID" runat="server" Value="1" />
        <asp:HiddenField ID="hfQuestionInputs" runat="server" />
        <asp:HiddenField ID="hfServiceID" runat="server" />
        <asp:HiddenField ID="hfuserquestion" runat="server" />
        <asp:HiddenField ID="hfPreviousStepSectionID" runat="server" />

    </asp:Panel>
    <dx:ASPxPopupControl ClientInstanceName="commentsReturnPopup" Modal="true"
        PopupElementID="returnWithCommentsButtonB" ID="commentsReturnPopup"
        ShowFooter="false" ShowHeader="true" HeaderText="Return Feedback"
        runat="server" EnableViewState="false" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" EnableHierarchyRecreation="True">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl5" runat="server">
                <div style="float: left; height: 200px; width: 420px;">
                    <table style="width: 100%;">
                        <tr>
                            <td>
                                <asp:Label ID="lblReturnMessage" runat="server" Text="" Font-Size="Smaller" ForeColor="Red"></asp:Label></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox runat="server" ID="popedReturnComments" Width="400px" Columns="40" Rows="9" TextMode="MultiLine" Text=""></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td class="buttoncell">
                                <div class="first_tier_nav" style="width: 100%">
                                    <ul style="float: right">
                                        <li runat="server" id="Li7" class="" onmouseover="this.className='tabhover'" onmouseout="this.className=''" style="color: red">
                                            <asp:LinkButton runat="server" ID="returnButton" Style="color: white" Name="Return"
                                                OnClientClick="javascript:return validateFeedbackForm(this)"
                                                Text="Return" CssClass="return" />
                                        </li>
                                        <li runat="server" id="Li8" class="" onmouseover="this.className='tabhover'" onmouseout="this.className=''">
                                            <a style="color: white" class="cancelwhite" onclick="commentsReturnPopup.Hide();" href="javascript:void(0);">Cancel</a>
                                        </li>
                                    </ul>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>

    <dx:ASPxPopupControl ClientInstanceName="commentsRejectPopup" Modal="true"
        ID="commentsRejectPopup"
        ShowFooter="false" ShowHeader="true" HeaderText="Reject Feedback"
        runat="server" EnableViewState="false" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" EnableHierarchyRecreation="True">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                <div style="float: left; height: 200px; width: 420px;">
                    <table style="width: 100%;">
                        <tr>
                            <td>
                                <asp:Label ID="lblRejectMessage" runat="server" Text="" Font-Size="Smaller" ForeColor="Red"></asp:Label></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox runat="server" ID="popedRejectComments" Width="400px" Columns="50" Rows="9" TextMode="MultiLine" Text=""></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td class="buttoncell">
                                <div class="first_tier_nav" style="width: 100%">
                                    <ul style="float: right">
                                        <li runat="server" id="Li1" class="" onmouseover="this.className='tabhover'" onmouseout="this.className=''" style="color: red">
                                            <asp:LinkButton runat="server" ID="rejectButton" Style="color: white" Name="Reject" CssClass="reject"
                                                OnClientClick="javascript:return validateFeedbackForm(this)"
                                                Text="Reject" />
                                        </li>
                                        <li runat="server" id="Li2" class="" onmouseover="this.className='tabhover'" onmouseout="this.className=''">
                                            <a style="color: white" class="cancelwhite" onclick="commentsRejectPopup.Hide();" href="javascript:void(0);">Cancel</a>
                                        </li>
                                    </ul>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>

</div>


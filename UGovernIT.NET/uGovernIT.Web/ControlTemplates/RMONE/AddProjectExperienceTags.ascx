<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddProjectExperienceTags.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.RMONE.AddProjectExperienceTags" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style data-v="<%=UGITUtility.AssemblyVersion %>">
    #projectTagsTxt .dx-placeholder.dx-state-invisible{
        display: block !important;
    }
    .projectExperiencePopup .switch {
        position: relative;
        display: inline-block;
        width: 44px;
        height: 18px;
    }
    .projectExperiencePopup .switch input {
        opacity: 0;
        width: 0;
        height: 0;
    }
    .projectExperiencePopup .slider {
        position: absolute;
        cursor: pointer;
        top: 0;
        left: 0;
        right: 0;
        bottom: 0;
        background-color: #ccc;
        -webkit-transition: .4s;
        transition: .4s;
    }
    .projectExperiencePopup .slider:before {
        position: absolute;
        content: "";
        height: 18px;
        width: 18px;
        left: 0px;
        bottom: 0px;
        background-color: #4fa1d6;
        -webkit-transition: .4s;
        transition: .4s;
    }
    .projectExperiencePopup input:checked + .slider {
        background-color: #ccc;
    }
    .projectExperiencePopup input:focus + .slider {
        box-shadow: 0 0 1px #ccc;
    }
    .projectExperiencePopup input:checked + .slider:before {
        -webkit-transform: translateX(26px);
        -ms-transform: translateX(26px);
        transform: translateX(26px);
    }
    .projectExperiencePopup .slider.round {
        border-radius: 34px;
    }
    .projectExperiencePopup .slider.round:before {
        border-radius: 34px;
    }
    .tag-container {
        display: flex;
        align-items: center;
        margin-bottom: 10px;
        justify-content: space-between;
    }
    .projectExperiencePopup .dx-tag-content-1, #projectExpTags .dx-tag-content-1 {
        position: relative;
        display: inline-block;
        text-align: center;
        border: 2px dotted gray !important;
        margin: 4px 0 0 4px;
        padding: 3px 21px 4px 9px !important;
        min-width: 40px;
        background-color: #fff !important;
        border-radius: 28px;
        color: #333 !important;
        font-weight: 500;
        font-size:14px !important;
    }
    .projectExperiencePopup .dx-tag-content, #projectExpTags .dx-tag-content {
        position: relative;
        display: inline-block;
        text-align: center;
        border: 2px #ddd;
        border-radius: 28px;
        cursor: pointer;
        margin: 4px 0 0 4px;
        padding: 5px 21px 6px 12px;
        min-width: 40px;
        background-color: #ededed;
        color: #333;
        font-weight: 500 !important;
        font-size:14px !important
    }
    .projectExperiencePopup .dx-overlay-wrapper {
        font-family: 'Roboto', sans-serif !important;
        color: black !important;
    }
    .projectExperiencePopup .font-size-class {
        font-size: 15px;
    }
    .boxProp {
        box-shadow: 0 6px 14px #ccc;
        padding: 15px;
        height: 100%;
        width: 100%;
    }
    #projectExpTags.dx-texteditor.dx-editor-outlined {
        background: #fff;
        border: 0px solid #ddd;
        border-radius: 4px;
    }
    #addExpTags.dx-button-has-icon .dx-icon {
        width: 28px;
        height: 28px;
    }
    .divheight {
        height: 400px;
    }
    .tag-container-1 {
        display: flex;
        margin-bottom: 10px;
        justify-content: flex-start;
        flex-wrap: wrap;
        align-items: center;
    }
    .dx-tag-1 {
        margin-top: -4px;
    }
    #showCertification.dx-button-has-icon .dx-icon {
        width: 18px;
        height: 18px;
        transform:scale(1.4)
    }
    #showTags.dx-button-has-icon .dx-icon, #backBtn.dx-button-has-icon .dx-icon {
    width: 18px;
    height: 18px;
}
    .dx-toolbar-label > div {
        font-family: 'Roboto', sans-serif;
    }
</style>

<%if(RequestFrom =="agentTab"){%>
<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .dx-overlay-content.dx-popup-normal.dx-resizable.dx-popup-flex-height {
        transform:translate(476px, 3px) scale(1) !important;
    }
</style>
<%}%>

<script data-v="<%=UGITUtility.AssemblyVersion %>">
    var projectExperiencModel = {};
    var hasAccessToAddTags = "<%=HasAccessToAddTags%>" == "False" ? false : true;
    var tagType = "";
    projectExperiencModel.ProjectId = "<%=TicketId%>";
    projectExperiencModel.ProjectTags = [];
    var experienceTags = [];
    var certifications = [];
    var requestFrom = "<%=RequestFrom%>";
    var projectExperiencTags = [];
    var existingProjectTags = [];
    var useMultiSelect = "<%=UseMultiSelect%>" == "False" ? false : true;
    function GetExperienceData() {
        $.get("/api/rmmapi/GetExperiencedTagList?tagMultiLookup=All", function (data) {
            experienceTags = data;
            GetCertificationsData();
        });
    }
    function GetCertificationsData() {
        $.get("/api/rmmapi/GetCertificationsList", function (data) {
            certifications = data;
            GetProjectExperienceTagData();
        });
    }

    function GetProjectExperienceTagData() {
        $.get("/api/rmmapi/GetProjectExperienceTagList?projectId=" + projectExperiencModel.ProjectId, function (data) {
            if (data != "EmptyProjectTags" && data != "null" && data != null) {
                existingProjectTags = data;
            }
            else {
                existingProjectTags = [];
            }
            GenerateData();
        });
    }

    GetExperienceData();

    function GenerateData() {
        projectExperiencTags = [];
        let tempData = [];
        if (existingProjectTags != null) {
            existingProjectTags.forEach(function (value, index) {
                let tag = {};
                if (CheckTagExist(value.Type, value.TagId)) {
                    let expTag = value.Type == 2 ? experienceTags.filter(x => x.ID == value.TagId)[0] : certifications.filter(x => x.ID == value.TagId)[0];
                    tag.ID = value.TagId;
                    tag.Title = expTag.Title;
                    tag.MinValue = value.MinValue;
                    tag.IsMandatory = value.IsMandatory;
                    projectExperiencTags.push(tag);
                    tempData.push(value);
                }
            });
        }
        existingProjectTags = JSON.parse(JSON.stringify(tempData));
        //$("#projectExpTags").dxTagBox("instance").option("value", []);
        //$("#projectExpTags").dxTagBox("instance").option("dataSource", projectExperiencTags);
        //$("#projectExpTags").dxTagBox("instance").option("value", projectExperiencTags.map(x => x.ID));
        RenderProjectTagsOnFrame();
    }

    function AddProjectTags() {
        let tagIds = [];
        let projectTagsDD = useMultiSelect ? $("#projectTagsTxt").dxTagBox("instance") : $("#projectTagsTxt").dxSelectBox("instance");
        let projectTag = {};
        let tagTypevalue = tagType == "Certification" ? 1 : 2;
        let isMandatory = true;
        let minValue = 1;        

        if (projectTagsDD != null) {
            let values = projectTagsDD.option("value");
            tagIds = useMultiSelect ? values : String(values).split(",");
        //    if (!useMultiSelect) { //clear selection in case of single select
        //        projectTagsDD.option("value", "");
        //        projectTagsDD.option("text", "");
        //    }
        }
        tagIds.forEach(function (tagId) {
            if (projectExperiencModel.ProjectTags == null || projectExperiencModel.ProjectTags.length == 0 || !projectExperiencModel.ProjectTags.filter(x => x.TagId == tagId && x.Type == tagTypevalue).length) {
                projectTag = {};
                projectTag.TagId = tagId;
                projectTag.IsMandatory = tagTypevalue == 1 ? true : isMandatory;
                projectTag.MinValue = tagTypevalue == 1 ? 0 : minValue;
                projectTag.Type = tagTypevalue;
                projectExperiencModel.ProjectTags.push(projectTag);
            }
            else {
                projectTag = projectExperiencModel.ProjectTags.filter(x => x.TagId == tagId && x.Type == tagTypevalue)[0];
                projectTag.TagId = tagId;
                projectTag.IsMandatory = tagTypevalue == 1 ? true : isMandatory;
                projectTag.MinValue = tagTypevalue == 1 ? 0 : minValue;
                projectTag.Type = tagTypevalue;
            }
        });
    }

    function AddProjectTagsFromTxt() {
        let tagIds = [];
        let projectTagsDD = $("#tagTxt").dxTagBox("instance");
        let projectTag = {};
        let tagTypevalue = 2;
        let isMandatory = true;
        let minValue = 1;

        if (projectTagsDD != null) {
            let values = projectTagsDD.option("value");
            tagIds = values;
        }
        tagIds.forEach(function (tagId) {
            if (projectExperiencModel.ProjectTags == null || projectExperiencModel.ProjectTags.length == 0 || !projectExperiencModel.ProjectTags.filter(x => x.TagId == tagId && x.Type == tagTypevalue).length) {
                projectTag = {};
                projectTag.TagId = tagId;
                projectTag.IsMandatory = tagTypevalue == 1 ? true : isMandatory;
                projectTag.MinValue = tagTypevalue == 1 ? 0 : minValue;
                projectTag.Type = tagTypevalue;
                projectExperiencModel.ProjectTags.push(projectTag);
            }
            else {
                projectTag = projectExperiencModel.ProjectTags.filter(x => x.TagId == tagId && x.Type == tagTypevalue)[0];
                projectTag.TagId = tagId;
                projectTag.IsMandatory = tagTypevalue == 1 ? true : isMandatory;
                projectTag.MinValue = tagTypevalue == 1 ? 0 : minValue;
                projectTag.Type = tagTypevalue;
            }
        });
    }

    function HideTagTxt(hide, useMultiSelect) {
        if (hide) {
            $("#tagTxt").dxTagBox('instance').option("visible", false);
            $(`#projectTagType`).dxSelectBox('instance').option("visible", false);
            $("#showTags").dxButton('instance').option("visible", false);
            $("#showCertification").dxButton('instance').option("visible", false);
            $("#tagCategory").dxSelectBox('instance').option("visible", true);
            $("#backBtn").dxButton('instance').option("visible", true);
        } else {
            $("#tagTxt").dxTagBox('instance').option("visible", true);
            $(`#projectTagType`).dxSelectBox('instance').option("visible", false);
            $("#showTags").dxButton('instance').option("visible", true);
            $("#showCertification").dxButton('instance').option("visible", false);
            $("#tagCategory").dxSelectBox('instance').option("visible", false);
            $("#backBtn").dxButton('instance').option("visible", false);
            if (projectExperiencModel.ProjectTags?.length > 0) {
                $("#tagTxt").dxTagBox('instance').option("value", projectExperiencModel.ProjectTags.filter(x => x.Type == 2).map((obj) => parseInt(obj.TagId)));
            }
        }
        if (useMultiSelect)
            $("#projectTagsTxt").dxTagBox('instance').option("visible", false);
        else
            $("#projectTagsTxt").dxSelectBox('instance').option("visible", false);
    }

    function RenderProjectTags() { 
        $(".experience-tags-row").html("");
        if (projectExperiencModel.ProjectTags != null && projectExperiencModel.ProjectTags.length > 0) {
            projectExperiencModel.ProjectTags.forEach(function (value, index) {
                if (CheckTagExist(value.Type, value.TagId)) {
                    let experiencTag = value.Type == 2 ? experienceTags.filter(x => x.ID == value.TagId)[0] : certifications.filter(x => x.ID == value.TagId)[0];
                    let cssClass = value.MinValue > 0 ? "dx-tag-content" : "dx-tag-content-1";
                    let title = /*value.MinValue > 0 ? experiencTag.Title + " &ge; " + value.MinValue :*/ experiencTag.Title;
                    let element = $(`<div class="dx-tag"><div class="${cssClass}"><span>${title}</span><div onclick="RemoveProjectTags(${value.TagId}, ${value.Type})" class="dx-tag-remove-button"></div></div></div>`);
                    $(".experience-tags-row").append(element);
                }
            });
        }
    }
    function RenderProjectTagsOnFrame() {
        $("#projectExpTags").html("");
        if (existingProjectTags != null && existingProjectTags.length > 0) {
            existingProjectTags.forEach(function (value, index) {
                if (CheckTagExist(value.Type, value.TagId)) {
                    let experiencTag = value.Type == 2 ? experienceTags.filter(x => x.ID == value.TagId)[0] : certifications.filter(x => x.ID == value.TagId)[0];
                    let cssClass = value.MinValue > 0 ? "dx-tag-content" : "dx-tag-content-1";
                    let title = /*value.MinValue > 0 ? experiencTag.Title + " &ge; " + value.MinValue :*/ experiencTag.Title;
                    let element = $(`<div class="dx-tag-1"><div class="${cssClass}"><span>${title}</span><div onclick="DeleteProjectTag(${value.TagId}, ${value.Type})" class="dx-tag-remove-button"></div></div></div>`);
                    $("#projectExpTags").append(element);
                }
            });
        }
    }
    function DeleteProjectTag(tagId, type) {
        projectExperiencModel.ProjectTags = existingProjectTags != null && existingProjectTags.length > 0 ? JSON.parse(JSON.stringify(existingProjectTags)) : [];
        projectExperiencModel.ProjectTags = projectExperiencModel.ProjectTags.removeByValue(projectExperiencModel.ProjectTags.filter(x => x.TagId == tagId && x.Type == type)[0]);
        UpdateProjectExperienceTagList();
    }

    function UpdateProjectExperienceTagList() {
        $.post("/api/rmmapi/AddProjectExperienceTagList/", projectExperiencModel).then(function (response) {
            GetProjectExperienceTagData();
            $("#projectExperienceTagPH").removeAttr('style');
            /*$("#exeperiencetoast").dxToast("show");*/
        });
    }
    function LoadTags(category) {        
        let projectTagsDD = useMultiSelect ? $("#projectTagsTxt").dxTagBox("instance") : $("#projectTagsTxt").dxSelectBox("instance");
        let tagTypevalue = 2;
        let dataToBind = [];
        let selectedItems = [];
        let filtered_items = [];
        if (tagType == "Certification") {
            dataToBind = certifications.filter(x => x.CategoryName == category);
            projectTagsDD.option("placeholder", "Add Certification");
            tagTypevalue = 1;
        }
        else {
            dataToBind = experienceTags.filter(x => x.Category == category);
            projectTagsDD.option("placeholder", "Add Experience Tags");
        }
        if (useMultiSelect) {
            filtered_items = dataToBind.filter(x => {
                return projectExperiencModel.ProjectTags.find(y => {
                    return parseInt(y.TagId) === x.ID && y.Type == tagTypevalue;
                });
            });
            selectedItems = filtered_items.map((obj) => parseInt(obj.ID));
            projectTagsDD.option("value", selectedItems);
            projectTagsDD.option("dataSource", dataToBind);
        }
        else {
            filtered_items = dataToBind.filter(x => {
                return !projectExperiencModel.ProjectTags.find(y => {
                    return parseInt(y.TagId) === x.ID && y.Type == tagTypevalue;
                });
            });
            projectTagsDD.option("dataSource", filtered_items);
            projectTagsDD.option("value", "");
        }
        projectTagsDD.option("visible", true);
    }
    function RemoveProjectTags(tagId, type) {
        if (projectExperiencModel.ProjectTags != null && projectExperiencModel.ProjectTags.length > 0) {
            projectExperiencModel.ProjectTags = projectExperiencModel.ProjectTags.removeByValue(projectExperiencModel.ProjectTags.filter(x => x.TagId == tagId && x.Type == type)[0]);
            RenderProjectTags();
            let doneBtn = $("#doneBtn").dxButton("instance");
            doneBtn.option("visible", true);
            let tagCategory = $("#tagCategory").dxSelectBox("instance");
            let categoryValue = tagCategory.option("value");
            LoadTags(categoryValue);
        }
    }
    Array.prototype.removeByValue = function (val) {
        for (var i = 0; i < this.length; i++) {
            if (String(this[i].TagId) === String(val.TagId) && this[i].Type === val.Type) {
                this.splice(i, 1);
                i--;
            }
        }
        return this;
    }
    function CheckTagExist(tagType, tagId) {
        if (tagType == 2) {
            return experienceTags.filter(x => x.ID == tagId).length > 0 ? true : false;
        }
        if (tagType == 1) {
            return certifications.filter(x => x.ID == tagId).length > 0 ? true : false;
        }
    }
    $(() => {
        $("#exeperiencetoast").dxToast({
            message: "Experience Tags Saved Successfully. ",
            type: "success",
            displayTime: 1000,
            position: "{ my: 'center', at: 'center', of: window }"
        });
        $("#addExpTags").dxButton({
            icon: "/content/images/plus-blue-new.png",
            hint: "Add Experience Tags",
            visible: hasAccessToAddTags,
            width:40,
            focusStateEnabled: false,
            onClick() {
                if(requestFrom == "agentTab")
                {
                    $("#projectExperienceTagPH").css("height", "400px");
                    window.scrollTo(1, 4000);
                }
                const popupAddExperienceTags = function () {
                    projectExperiencModel.ProjectTags = existingProjectTags != null && existingProjectTags.length > 0 ? JSON.parse(JSON.stringify(existingProjectTags)) : [];
                    const experienceTagsDB = new DevExpress.data.DataSource({
                        store: experienceTags,
                        key: 'ID',
                        group: 'Category',
                    });
                    const certificationTagsDB = new DevExpress.data.DataSource({
                        store: certifications,
                        key: 'ID',
                        group: 'CategoryName',
                    });

                    let container = $("<div class='projectExperiencePopup'>");
                    var tagTypes = ["Project Experience", "Certification"];
                   
                    let tagTxtRow = $("<div class='tag-container'>").append(
                        $(`<div id="tagTxt" class="mr-1" />`).dxTagBox({
                            dataSource: experienceTagsDB,
                            key: 'ID',
                            grouped: true,
                            width: "100%",
                            displayExpr: 'Title',
                            valueExpr:'ID',
                            applyValueMode: 'useButtons',
                            maxFilterQueryLength:5000,
                            value: projectExperiencModel.ProjectTags.filter(x => x.Type == 2).map((obj) => parseInt(obj.TagId)),
                            searchEnabled: true,
                            placeholder: "Add Experience Tags",
                            showSelectionControls: true,
                            tagTemplate(data) {
                                return null;
                            },
                            onValueChanged: function (e) {
                                debugger;
                                let tagTypevalue = 2;
                                if (e.value.length > 0)
                                    AddProjectTagsFromTxt();
                                if (e.previousValue != undefined && e.previousValue.length > 0 && e.event != undefined) {
                                    let deleted_items = e.previousValue.filter(x => !e.value.includes(x));
                                    deleted_items.forEach(function (tagId) {
                                        RemoveProjectTags(tagId, tagTypevalue);
                                    });
                                }
                                RenderProjectTags();
                                let doneBtn = $("#doneBtn").dxButton("instance");
                                doneBtn.option("visible", true);
                            }
                        }),
                        $('<div id="showTags" class="ml-1" />').dxButton({
                            icon: 'fa fa-tag',
                            hint:"Experience Tags",
                            onClick() {
                                let control = $(`#projectTagType`).dxSelectBox('instance');
                                control.option("value", "Project Experience");
                                HideTagTxt(true, useMultiSelect);
                                $("#addExperienceTagsDialog").dxPopup('instance').option('title', "Add Experience Tags");
                            },
                        }),
                        $('<div id="showCertification" />').dxButton({
                            icon: '/Content/Images/Icons/certification.jpeg',
                            hint: "Certification",
                            visible:false,
                            onClick() {
                                let control = $(`#projectTagType`).dxSelectBox('instance');
                                control.option("value", "Certification");
                                HideTagTxt(true, useMultiSelect);
                                $("#addExperienceTagsDialog").dxPopup('instance').option('title', "Add Certification");
                            },
                        })
                    )
                    let firstrow = $("<div class='tag-container'>").append(
                        $(`<div id="projectTagType" class="mr-1" />`).dxSelectBox({
                            dataSource: tagTypes,
                            visible: false,
                            onValueChanged: function (e) {
                                let tagCategoryDD = $("#tagCategory").dxSelectBox("instance");
                                $("#backBtn").dxButton("instance").option("visible", true);
                                if (e.value == "Certification") {
                                    let categoryData = [...new Set(certifications.map(x => x.CategoryName))];
                                    tagCategoryDD.option("dataSource", categoryData);
                                    tagType = "Certification";
                                }
                                else {
                                    let categoryData = [...new Set(experienceTags.map(x => x.Category))];
                                    tagCategoryDD.option("dataSource", categoryData);
                                    tagType = "Project Experience";
                                }
                                tagCategoryDD.option("value", "");
                                tagCategoryDD.option("visible", true);

                                let projectTagsDD = useMultiSelect ? $("#projectTagsTxt").dxTagBox("instance") : $("#projectTagsTxt").dxSelectBox("instance");
                                //projectTagsDD.option("value", "");
                                projectTagsDD.option("visible", false);
                            }
                        }),
                        $(`<div id="tagCategory"/>`).dxSelectBox({
                            placeholder: "Select Category",
                            searchEnabled: true,
                            width: "100%",
                            visible: false,
                            onValueChanged: function (e) {
                                LoadTags(e.value);
                            }
                        }),
                        $('<div id="backBtn" class="ml-2" />').dxButton({
                            icon: '/Content/Images/sectorIcon.png',
                            hint: "Back",
                            visible: false,
                            onClick() {
                                $(`#tagCategory`).dxSelectBox('instance').option("value", "");
                                HideTagTxt(false, useMultiSelect);
                                $("#addExperienceTagsDialog").dxPopup('instance').option('title', "Add Experience Tags");
                                let tagTxt = $("#tagTxt").dxTagBox('instance');
                                if (tagTxt != undefined || tagTxt != null) {
                                    tagTxt.option('value', projectExperiencModel.ProjectTags.filter(x => x.Type == 2).map((obj) => parseInt(obj.TagId)));
                                }
                            },
                        })
                    )
                    let secondrow = "";
                    if (useMultiSelect) {
                        secondrow = $("<div class='tag-container'>").append(
                            $(`<div id="projectTagsTxt" />`).dxTagBox({
                                showSelectionControls: true,
                                applyValueMode: 'useButtons',
                                width: "100%",
                                visible: false,
                                searchEnabled: true,  
                                displayExpr: 'Title',
                                valueExpr: 'ID',
                                tagTemplate(data) {
                                    return null; 
                                },
                                onValueChanged: function (e) {
                                    let tagTypevalue = tagType == "Certification" ? 1 : 2;
                                    let doneBtn = $("#doneBtn").dxButton("instance");
                                    doneBtn.option("visible", true);
                                    if (e.value != "")
                                        AddProjectTags();
                                    if (e.previousValue != undefined && e.previousValue.length > 0 && e.event != undefined) {
                                        let deleted_items = e.previousValue.filter(x => !e.value.includes(x));
                                        deleted_items.forEach(function (tagId) {
                                            RemoveProjectTags(tagId, tagTypevalue);
                                        });
                                    }
                                    RenderProjectTags();
                                },
                                onInput: function (e) {
                                    if (e.element.find("input.dx-texteditor-input").val().length > 0) {
                                        e.component.option("placeholder", "");
                                    }
                                    else { 
                                        e.component.option("placeholder", "Add Experience Tags");
                                    }
                                },
                                onFocusOut: function (e) {
                                    if (e.element.find("input.dx-texteditor-input").val().length > 0) {
                                        e.component.option("placeholder", "");
                                    }
                                    else {
                                        e.component.option("placeholder", "Add Experience Tags");
                                    }
                                }
                            }),
                        );
                    }
                    else {
                         secondrow = $("<div class='tag-container'>").append(
                             $(`<div id="projectTagsTxt" />`).dxSelectBox({
                                placeholder: "Add Experience Tags",
                                searchEnabled: true,
                                valueExpr: "ID",
                                visible: false,
                                width: "100%",
                                displayExpr: "Title",
                                onValueChanged: function (data) {
                                    let tagCategory = $("#tagCategory").dxSelectBox("instance");
                                    let categoryValue = tagCategory.option("value");
                                    let doneBtn = $("#doneBtn").dxButton("instance");
                                    doneBtn.option("visible", true);
                                    $("#projectTagsTxt").dxSelectBox("instance").close();
                                    if (data.selectedItem != "")
                                        AddProjectTags();
                                    RenderProjectTags();
                                    LoadTags(categoryValue);
                                },
                                onContentReady: function (e) {
                                }
                            }),
                            //$("<div id='tagMinValue'>").dxNumberBox({
                            //    placeholder: "Min # of Project Experience",
                            //    value: "",
                            //    width: 200,
                            //    min: 1,
                            //    visible:false,
                            //    max: 1000,
                            //    hint:"Min # of Project Experience",
                            //    visible: false,
                            //    inputAttr: { 'aria-label': 'Min And Max' },
                            //})
                        ) 
                    }
                    let thirdrow = $(`<div class="experience-tags-row" />`);
                    let forthrow = $(`<div />`).append(
                        $(`<div id="doneBtn" class="mt-4 btnAddNew" style="float:right;padding: 0px 10px;font-size: 14px;" />`).dxButton({
                            text: "Done",
                            visible: false,
                            onClick: function (e) {
                                UpdateProjectExperienceTagList();
                                popup.hide();
                            }
                        })
                    )
                    container.append(tagTxtRow);
                    container.append(firstrow);
                    container.append(secondrow);
                    container.append(thirdrow);
                    container.append(forthrow);
                    return container;
                };

                const popup = $("#addExperienceTagsDialog").dxPopup({
                    contentTemplate: popupAddExperienceTags,
                    width: "460",
                    height: "auto",
                    showTitle: true,
                    title: "Add Experience Tags",
                    visible: false,
                    dragEnabled: false,
                    hideOnOutsideClick: true,
                    showCloseButton: true,
                    position: {
                        at: 'center',
                        my: 'center',
                    },
                    onHiding: function () {
                        $("#projectExperienceTagPH").removeAttr('style');
                    }
                }).dxPopup('instance');

                popup.option({
                    contentTemplate: () => popupAddExperienceTags()
                });

                popup.show();
                RenderProjectTags();
            }
        });

        //$('#projectExpTags').dxTagBox({
        //    displayExpr: 'Title',
        //    valueExpr: 'ID',
        //    placeholder: "",
        //    width: '100%',
        //    itemTemplate(data) {
        //        return data.MinValue == 0 ? data.Title : data.Title + " &ge; " + data.MinValue;
        //    },
        //    onSelectionChanged: function (data) {
        //        projectExperiencModel.ProjectTags = existingProjectTags != null && existingProjectTags.length > 0 ? JSON.parse(JSON.stringify(existingProjectTags)) : [];
        //        var items;
        //        var selectedItem = data.component.option('selectedItems');
        //        if ( tagdeleteflag && (selectedItem.length > 0 && selectedItem.length != projectExperiencTags.length) || (data.removedItems.length > 0 && selectedItem.length == 0)) {
        //            alert(selectedItem.length);
        //            var selIte = [];
        //            if (selectedItem.length > 0) {
        //                for (var i = 0; i < selectedItem.length; i++) {
        //                    let type = selectedItem[i].MinValue == 0 ? 1 : 2;
        //                    selIte.push({ TagId: selectedItem[i].ID, IsMandatory: true, MinValue: selectedItem[i].MinValue, Type: type });
        //                }
        //            }
        //            projectExperiencModel.ProjectTags = selIte;
        //            $.post("/api/rmmapi/AddProjectExperienceTagList/", projectExperiencModel).then(function (response) {
        //                GetProjectExperienceTagData();
        //                $("#projectExperienceTagPH").removeAttr('style');
        //                $("#exeperiencetoast").dxToast("show");
        //            });
        //        }
        //    },
        //    tagTemplate(data) {
        //        const isDisabled = false;
        //        const tag = $('<div>')
        //            .attr('aria-disabled', isDisabled)
        //            .addClass(`dx-tag-content ${!data.IsMandatory && 'dx-tag-content-1'}`)
        //            .append(
        //                $('<span>').html(data.MinValue == 0 ? data.Title : data.Title + " &ge; " + data.MinValue),
        //                !isDisabled && $('<div>').addClass('dx-tag-remove-button'),
        //            );
        //        return tag;
        //    },
        //});

    });
</script>
<div id="projectExperienceTagPH" class="SetHeight">
    <div id="addExperienceTagsDialog"></div>
    <div id="exeperiencetoast"></div>
    <div class="tag-container-1 mt-1" style="border: 2px solid #ddd;">
        <div id='addExpTags' style='border: none; -webkit-box-shadow: none'></div>
        <div id='projectExpTags' style='display: flex; flex-wrap: wrap;'></div>
    </div>
</div>
       

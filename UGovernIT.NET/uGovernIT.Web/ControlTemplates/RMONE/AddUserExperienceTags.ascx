<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddUserExperienceTags.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.RMONE.AddUserExperienceTags" %>

<%@ Import Namespace="uGovernIT.Utility" %>


<style data-v="<%=UGITUtility.AssemblyVersion %>">
    #projectTagsTxt .dx-placeholder.dx-state-invisible{
        display: block !important;
    }
    #certificationTagsTxt .dx-placeholder.dx-state-invisible{
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

    /* Rounded sliders */
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

    .projectExperiencePopup .dx-tag-content-1, #userExpTags .dx-tag-content-1, #userCertificationTags .dx-tag-content-1 {
        position: relative;
        display: inline-block;
        text-align: center;
        cursor: pointer;
        border: 2px dotted !important;
        margin: 4px 0 0 4px;
        padding: 5px 21px 6px 12px !important;
        min-width: 40px;
        background-color: #fff !important;
        border-radius: 28px;
        color: #333 !important;
        font-weight: 500;
        font-family: 'Roboto', sans-serif !important;
        font-size:14px !important;
    }

    .projectExperiencePopup .dx-tag-content, #userExpTags .dx-tag-content, #userCertificationTags .dx-tag-content {
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
        font-weight: 500;
        font-family: 'Roboto', sans-serif !important;
        font-size:14px !important;
    }

    .projectExperiencePopup .dx-overlay-wrapper {
        font-family: 'Roboto', sans-serif !important;
        color: black !important;
        /* color: black; */
    }

    .projectExperiencePopup .font-size-class {
        font-size: 15px;
    }

    .boxProp {
        box-shadow: 0 6px 14px #ccc;
        padding: 15px;
        height: 100%;
        width: 100%;
        /* display: flex; */
        /* align-items: center; */
    }

    #userExpTags.dx-texteditor.dx-editor-outlined, #userCertificationTags.dx-texteditor.dx-editor-outlined {
        background: #fff;
        border: 0px solid #ddd;
        border-radius: 4px;
    }

    #addExpTags.dx-button-has-icon .dx-icon, #addCertificationTags.dx-button-has-icon .dx-icon {
        width: 30px;
        height: 30px;
    }

    .tagTitle {
        font-size: 16px;
        font-weight: 500;
    }
</style>
<script data-v="<%=UGITUtility.AssemblyVersion %>">
    var projectExperiencModel = {};
    var tagType = "";
    var hasAccessToAddTags = "<%=HasAccessToAddTags%>" == "False" ? false : true;
    var UserID = "<%=UserId%>";
    var userProjectExperiencTags = [];
    var userCertification = [];
    var userExperiencesTags = [];
    projectExperiencModel.ProjectId = "";
    projectExperiencModel.ProjectTags = [];
    var experienceTags = [];
    var certifications = [];
    var projectExperiencTags = [];
    var existingProjectTags = [];
    var extraUserTags = [];
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
            GetUserExperienceTagList();
        });
    }

    function GetUserExperienceTagList() {
        $.get("/api/rmmapi/GetUserExperienceTagList?userId=" + UserID, function (data) {
            userProjectExperiencTags = []; //data.UserProjectExperiencTags != null ? data.UserProjectExperiencTags : [];
            userCertification = data.Certification != null ? data.Certification : [];
            userExperiencesTags = data.UserExperiencesTags != null ? data.UserExperiencesTags : [];
            GenerateData();
        });
    }

    GetExperienceData();

    function GenerateData() {
        if (userProjectExperiencTags != null) {
            let userExperiencDB = [];
            userProjectExperiencTags.forEach(function (value, index) {
                let tag = {};
                if (experienceTags.filter(x => x.ID == value.TagLookup).length > 0) {
                    let expTag = experienceTags.filter(x => x.ID == value.TagLookup)[0]; //: certifications.filter(x => x.ID == value.TagId)[0];
                    tag.ID = value.TagLookup;
                    tag.Title = expTag.Title;
                    tag.Count = value.TagCount;
                    userExperiencDB.push(tag);
                }
            });
            if (userExperiencesTags != null) {
                userExperiencesTags.forEach(function (value) {
                    if (userExperiencDB.filter(x => x.ID == value.TagLookup).length > 0) {
                        extraUserTags.push(value.TagLookup);
                    }
                    if (userExperiencDB != null && userExperiencDB.filter(x => x.ID == value.TagLookup).length == 0) {
                        let tag = {};
                        if (experienceTags.filter(x => x.ID == value.TagLookup).length > 0) {
                            let expTag = experienceTags.filter(x => x.ID == value.TagLookup)[0]; //: certifications.filter(x => x.ID == value.TagId)[0];
                            tag.ID = value.TagLookup;
                            tag.Title = expTag.Title;
                            tag.Count = 0;
                            userExperiencDB.push(tag);
                            let userTag = {};
                            userTag.TagLookup = value.TagLookup;
                            userTag.TagCount = 0;
                            userProjectExperiencTags.push(userTag);
                        }
                    }
                });
            }
            $("#userExpTags").dxTagBox("instance").option("value", []);
            $("#userExpTags").dxTagBox("instance").option("dataSource", userExperiencDB);
            $("#userExpTags").dxTagBox("instance").option("value", userExperiencDB);
        }
        if (userCertification != null) {
            let userCertificationDB = [];
            userCertification.forEach(function (value, index) {
                let tag = {};
                let expTag = certifications.filter(x => x.ID == value)[0]; //: certifications.filter(x => x.ID == value.TagId)[0];
                tag.ID = value;
                tag.Title = expTag.Title;
                userCertificationDB.push(tag);
            });
            $("#userCertificationTags").dxTagBox("instance").option("value", []);
            $("#userCertificationTags").dxTagBox("instance").option("dataSource", userCertificationDB);
            $("#userCertificationTags").dxTagBox("instance").option("value", userCertificationDB);
        }
    }
    function AddCertifications() {
        let tagIds = [];
        let projectTagsDD = useMultiSelect ? $("#certificationTagsTxt").dxTagBox("instance") : $("#certificationTagsTxt").dxSelectBox("instance");
        if (projectTagsDD != null) {
            let values = projectTagsDD.option("value");
            tagIds = useMultiSelect ? values : String(values).split(",");
        //    if (!useMultiSelect) { //clear selection in case of single select
        //        projectTagsDD.option("value", "");
        //        projectTagsDD.option("text", "");
        //    }
        }

        tagIds.forEach(function (tagId) {
            if (userCertification == null || userCertification.length == 0 || !userCertification.filter(x => x == tagId).length) {
                userCertification.push(tagId);
            }
        });

        //RenderProjectTags(1);
    }
    function AddProjectTags()
    {
        let userTag = {};
        let tagIds = [];
        let projectTagsDD = useMultiSelect ? $("#projectTagsTxt").dxTagBox("instance") : $("#projectTagsTxt").dxSelectBox("instance");
        if (projectTagsDD != null) {
            let values = projectTagsDD.option("value");
            tagIds = useMultiSelect ? values : String(values).split(",");
        }
        tagIds.forEach(function (tagId) {
            if (userProjectExperiencTags == null || userProjectExperiencTags.length == 0 || !userProjectExperiencTags.filter(x => x.TagLookup == tagId).length) {
                userTag = {};
                userTag.TagLookup = tagId;
                userTag.TagCount = 0;
                userProjectExperiencTags.push(userTag);
            }
        });

        //RenderProjectTags();
    }

    function RenderProjectTags(value) {
        $(".experience-tags-row").html(""); 
        if (value == 1 && userCertification != null && userCertification.length > 0) {
            userCertification.forEach(function (value, index) {
                let experiencTag = certifications.filter(x => x.ID == value)[0] //: certifications.filter(x => x.ID == value.TagId)[0];
                let title = experiencTag.Title;
                let element = $(`<div class="dx-tag"><div class="dx-tag-content"><span>${title}</span><div onclick="RemoveProjectTags(${value}, 0, 'Certification')" class="dx-tag-remove-button"></div></div></div>`);
                $(".experience-tags-row").append(element);
            });
        }
        else if (value != 1 && userProjectExperiencTags != null && userProjectExperiencTags.length > 0) {
            userProjectExperiencTags.forEach(function (value, index) {
                if (experienceTags.filter(x => x.ID == value.TagLookup).length > 0) {
                    let experiencTag = experienceTags.filter(x => x.ID == value.TagLookup)[0] //: certifications.filter(x => x.ID == value.TagId)[0];
                    let title = experiencTag.Title; /*+ " - " + value.TagCount;*/
                    let element = $(`<div class="dx-tag"><div class="dx-tag-content"><span>${title}</span><div onclick="RemoveProjectTags(${value.TagLookup},${value.TagCount}, 'Experience')" class="dx-tag-remove-button"></div></div></div>`);
                    $(".experience-tags-row").append(element);
                }
            });
        }
    }

    function LoadTags(categoryVal) {
        let projectTagsDD = useMultiSelect ? $("#projectTagsTxt").dxTagBox("instance") : $("#projectTagsTxt").dxSelectBox("instance");
        let dataToBind = experienceTags.filter(x => x.Category == categoryVal);
        let filtered_items = [];
        if (useMultiSelect) {
            filtered_items = dataToBind.filter(x => {
                return userProjectExperiencTags.find(y => {
                    return parseInt(y.TagLookup) === x.ID;
                });
            });
            let selectedItems = [];
            selectedItems = filtered_items.map((obj) => obj.ID);
            projectTagsDD.option("dataSource", dataToBind);
            projectTagsDD.option("value", selectedItems);
        }
        else {
            filtered_items = dataToBind.filter(x => {
                return !userProjectExperiencTags.find(y => {
                    return parseInt(y.TagLookup) === x.ID; //&& y.Type == tagTypevalue
                });
            });
            projectTagsDD.option("dataSource", filtered_items);
        }
        projectTagsDD.option("placeholder", "Select Experience Tags");
        projectTagsDD.option("visible", true);
    }
    function LoadCertifications(categoryVal) {
        let projectTagsDD = useMultiSelect ? $("#certificationTagsTxt").dxTagBox("instance") : $("#certificationTagsTxt").dxSelectBox("instance");
        let dataToBind = certifications.filter(x => x.CategoryName == categoryVal);
        let filtered_items = [];
        if (useMultiSelect) {
            filtered_items = dataToBind.filter(c => userCertification.includes(c.ID));
            let selectedItems = [];
            selectedItems = filtered_items.map((obj) => obj.ID);
            projectTagsDD.option("dataSource", dataToBind);
            projectTagsDD.option("value", selectedItems);
        }
        else {
            filtered_items = dataToBind.filter(c => !userCertification.includes(String(c.ID)));
            projectTagsDD.option("dataSource", filtered_items);
            projectTagsDD.option("value", "");
        }
        projectTagsDD.option("placeholder", "Select Certification");
        projectTagsDD.option("visible", true);

    }
    function RemoveProjectTags(tagId, TagCount, type) {
        if (type == 'Certification') {
            
            if (userCertification != null && userCertification.length > 0) {
                userCertification = userCertification.filter(x => x != tagId);
                RenderProjectTags(1);
                let doneBtn = $("#doneBtn-1").dxButton("instance");
                doneBtn.option("visible", true);
                let certtagCategory = $("#certtagCategory").dxSelectBox("instance");
                let categoryValue = tagCategory.option("value");
                LoadCertifications(categoryValue);
            }
        }
        else {
            if (parseInt(TagCount) < 1 && userProjectExperiencTags != null && userProjectExperiencTags.length > 0) {
                userProjectExperiencTags = userProjectExperiencTags.filter(x => x.TagLookup != tagId);
                RenderProjectTags();
                let doneBtn = $("#doneBtn").dxButton("instance");
                doneBtn.option("visible", true);
                let tagCategory = $("#tagCategory").dxSelectBox("instance");
                let categoryValue = tagCategory.option("value");
                LoadTags(categoryValue);
           }
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
            width: 40,
            visible: hasAccessToAddTags,
            focusStateEnabled: false,
            onClick() {
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
                    let secondrow = $("<div class='tag-container'>").append(
                        $(`<div id="tagCategory" class="mr-2" />`).dxSelectBox({
                            placeholder: "Select Category",
                            searchEnabled: true,
                            width: "40%",
                            dataSource: [...new Set(experienceTags.map(x => x.Category))],
                            onValueChanged: function (e) {
                                LoadTags(e.value);
                            },
                            onContentReady: function (e) {
                            }
                        })
                    );
                    if (useMultiSelect) {
                        secondrow.append(
                            $(`<div id="projectTagsTxt" />`).dxTagBox({
                                showSelectionControls: true,
                                applyValueMode: 'useButtons',
                                visible: false,
                                width:"60%",
                                searchEnabled: true,
                                displayExpr: 'Title',
                                valueExpr: 'ID',
                                tagTemplate(data) {
                                    return null;
                                },
                                onValueChanged: function (e) {
                                    let doneBtn = $("#doneBtn").dxButton("instance");
                                    doneBtn.option("visible", true);
                                    if (e.value != "")
                                        AddProjectTags();
                                    if (e.previousValue != undefined && e.previousValue.length > 0 && e.event != undefined) {
                                        let deleted_items = e.previousValue.filter(x => !e.value.includes(x));
                                        deleted_items.forEach(function (tagId) {
                                            RemoveProjectTags(tagId,0, tagType);
                                        });
                                    }
                                    RenderProjectTags();
                                }
                            })
                        )
                    }
                    else {
                        secondrow.append(
                            $(`<div id="projectTagsTxt" />`).dxSelectBox({
                                placeholder: "Add Experience Tags",
                                searchEnabled: true,
                                valueExpr: "ID",
                                visible: false,
                                width: "60%",
                                displayExpr: "Title",
                                onValueChanged: function (data) {
                                    let doneBtn = $("#doneBtn").dxButton("instance");
                                    doneBtn.option("visible", true);
                                    let tagCategory = $("#tagCategory").dxSelectBox("instance");
                                    let categoryValue = tagCategory.option("value");
                                    if (data.value != "")
                                        AddProjectTags();
                                    RenderProjectTags();
                                    LoadTags(categoryValue);
                                },
                                onContentReady: function (e) {
                                }
                            }),
                        );
                    }
                    let thirdrow = $(`<div class="experience-tags-row" />`)
                    let forthrow = $(`<div />`).append(
                        $(`<div id="doneBtn" class="mt-4 btnAddNew" style="float:right;padding: 0px 10px;font-size: 14px;" />`).dxButton({
                            text: "Done",
                            visible: false,
                            onClick: function (e) {
                                let dataToSave = userProjectExperiencTags.filter(y => parseInt(y.TagCount) == 0).map(x => x.TagLookup).join();
                                let extraData = extraUserTags.join();
                                if (extraData.length > 0) {
                                    dataToSave = dataToSave + ',' + extraData;
                                }
                                $.post("/api/rmmapi/AddUserExperienceTagList?tagIds=" + dataToSave + "&userId=" + UserID + "&tagType=2").then(function (response) {
                                    GetExperienceData();
                                    /*$("#exeperiencetoast").dxToast("show");*/
                                    popup.hide();
                                });
                                
                            }
                        })
                    )
                    container.append(secondrow);
                    container.append(thirdrow);
                    container.append(forthrow);
                    return container;
                };

                const popup = $("#addExperienceTagsDialog").dxPopup({
                    contentTemplate: popupAddExperienceTags,
                    width: "480",
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

                    }
                }).dxPopup('instance');

                popup.option({
                    contentTemplate: () => popupAddExperienceTags()

                });
                popup.show();
                RenderProjectTags();
            }
        });

        $('#userExpTags').dxTagBox({
            displayExpr: 'Title',
            valueExpr: 'ID',
            placeholder: "",
            readOnly: true,
            width: '100%',
            itemTemplate(data) {
                return data.Title /*+ " - " + data.Count*/;
            },
            tagTemplate(data) {
                const isDisabled = false;
                const tag = $('<div>')
                    .attr('aria-disabled', isDisabled)
                    .addClass(`dx-tag-content`)
                    .append(
                        $('<span>').html(data.Title /*+ " - " + data.Count*/),
                        //!isDisabled && $('<div>').addClass('dx-tag-remove-button'),
                    );
                return tag;
            },
        });


        $("#addCertificationTags").dxButton({
            icon: "/content/images/plus-blue-new.png",
            hint: "Add Certification",
            width: 40,
            visible: hasAccessToAddTags,
            focusStateEnabled: false,
            onClick() {
                const popupAddExperienceTags = function () {
                    const certificationTagsDB = new DevExpress.data.DataSource({
                        store: certifications,
                        key: 'ID',
                        group: 'CategoryName',
                    });

                    let container = $("<div class='projectExperiencePopup'>");
                    
                    let secondrow = $("<div class='tag-container'>").append(
                        $(`<div id="certtagCategory" class="mr-2" />`).dxSelectBox({
                            placeholder: "Select Category",
                            searchEnabled: true,
                            width: "40%",
                            dataSource: [...new Set(certifications.map(x => x.CategoryName))],
                            onValueChanged: function (e) {
                                LoadCertifications(e.value);
                            },
                            onContentReady: function (e) {
                            }
                        })
                    );
                    if (useMultiSelect) {
                        secondrow.append(
                            $(`<div id="certificationTagsTxt" />`).dxTagBox({
                                showSelectionControls: true,
                                applyValueMode: 'useButtons',
                                visible: false,
                                width: "60%",
                                searchEnabled: true,  
                                displayExpr: 'Title',
                                valueExpr: 'ID',
                                tagTemplate(data) {
                                    return null;
                                },
                                onValueChanged: function (e) {
                                    let doneBtn = $("#doneBtn-1").dxButton("instance");
                                    doneBtn.option("visible", true);                                    
                                    if (e.value != "")
                                        AddCertifications();
                                    if (e.previousValue != undefined && e.previousValue.length > 0 && e.event != undefined) {
                                        let deleted_items = e.previousValue.filter(x => !e.value.includes(x));
                                        deleted_items.forEach(function (tagId) {
                                            RemoveProjectTags(tagId, 0, 'Certification');
                                        });
                                    }
                                    RenderProjectTags(1);
                                }
                            })
                        )
                    }
                    else {
                        secondrow.append(
                            $(`<div id="certificationTagsTxt" />`).dxSelectBox({
                                searchEnabled: true,
                                valueExpr: "ID",
                                visible: false,
                                width: "60%",
                                displayExpr: "Title",
                                onValueChanged: function (data) {
                                    let doneBtn = $("#doneBtn-1").dxButton("instance");
                                    doneBtn.option("visible", true);
                                    let tagCategory = $("#certtagCategory").dxSelectBox("instance");
                                    let categoryValue = tagCategory.option("value");
                                    if (data.selectedItem != "" && data.value != "")
                                        AddCertifications();
                                    RenderProjectTags(1);
                                    if (categoryValue != null)
                                        LoadCertifications(categoryValue);
                                },
                                onContentReady: function (e) {
                                }
                            }),
                        )
                    }
                    let thirdrow = $(`<div class="experience-tags-row" />`)
                    let forthrow = $(`<div />`).append(
                        $(`<div id="doneBtn-1" class="mt-4 btnAddNew" style="float:right;padding: 0px 10px;font-size: 14px;" />`).dxButton({
                            text: "Done",
                            visible: false,
                            onClick: function (e) {
                                $.post("/api/rmmapi/AddUserExperienceTagList?tagIds=" + userCertification.join() + "&userId=" + UserID + "&tagType=1").then(function (response) {
                                    GetExperienceData();
                                    /*$("#exeperiencetoast").dxToast("show");*/
                                    popup.hide();
                                });
                            }
                        })
                    )
                    container.append(secondrow);
                    container.append(thirdrow);
                    container.append(forthrow);
                    return container;
                };

                const popup = $("#addCertificationTagsDialog").dxPopup({
                    contentTemplate: popupAddExperienceTags,
                    width: "480",
                    height: "auto",
                    showTitle: true,
                    title: "Add Certification",
                    visible: false,
                    dragEnabled: false,
                    hideOnOutsideClick: true,
                    showCloseButton: true,
                    position: {
                        at: 'center',
                        my: 'center',
                    },
                    onHiding: function () {

                    }
                }).dxPopup('instance');

                popup.option({
                    contentTemplate: () => popupAddExperienceTags()

                });
                popup.show();
                RenderProjectTags(1);
            }
        });

        $('#userCertificationTags').dxTagBox({
            displayExpr: 'Title',
            valueExpr: 'ID',
            placeholder: "",
            readOnly: true,
            width: '100%',
            itemTemplate(data) {
                return data.Title;
            },
            tagTemplate(data) {
                const isDisabled = false;
                const tag = $('<div>')
                    .attr('aria-disabled', isDisabled)
                    .addClass(`dx-tag-content`)
                    .append(
                        $('<span>').html(data.Title),
                        //!isDisabled && $('<div>').addClass('dx-tag-remove-button'),
                    );
                return tag;
            },
        });

    });
</script>

<div id="addExperienceTagsDialog"></div>
<div id="addCertificationTagsDialog"></div>

<div id="exeperiencetoast"></div>
<div class="dashboard-panel-new noPadding">
    <div class="dashboard-panel-main p-2">
        <div class="tagTitle ml-2">Tags</div>
        <div class="tag-container mt-1">
            <div id='addExpTags' style='border: none; -webkit-box-shadow: none'></div>
            <div id='userExpTags'></div>
        </div>
    </div>
</div>

<div class="dashboard-panel-new noPadding">
    <div class="dashboard-panel-main p-2">
        <div class="tagTitle ml-2">Certifications</div>
        <div class="tag-container mt-1">
            <div id='addCertificationTags' style='border: none; -webkit-box-shadow: none'></div>
            <div id='userCertificationTags'></div>
        </div>
    </div>
</div>

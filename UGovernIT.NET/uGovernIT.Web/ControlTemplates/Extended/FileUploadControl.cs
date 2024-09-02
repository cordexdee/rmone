using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Utility;
using DevExpress.Web;

namespace uGovernIT.Web
{
    public class FileUploadControl : UGITControl
    {
        private Panel imageLinks;
        private HiddenField hiddenField;
        private Panel panel;
        private Label MsgLabel;
        private ASPxLoadingPanel loadingPanel;
        private TextBox textBox;
        private RequiredFieldValidator requiredFieldValidator;
        public ControlMode controlMode { get; set; }
        public ASPxUploadControl fileUpload;

        public bool Multi { get; set; }
        public int MaxFile { get; set; }
        public string FileExtension { get; set; }
        public string DisplayText { get; set; }
        public string ValidationGroup { get; set; }
        public bool IsMandate { get; set; }

        public FileUploadControl()
        {
            MsgLabel = new Label();
            loadingPanel = new ASPxLoadingPanel();
            fileUpload = new ASPxUploadControl();
            hiddenField = new HiddenField();


            fileUpload.UploadMode = UploadControlUploadMode.Auto;
            fileUpload.AutoStartUpload = true;
            fileUpload.ShowProgressPanel = true;
            fileUpload.AdvancedModeSettings.EnableDragAndDrop=true;
            fileUpload.NullText= "Select file(s) or drag and drop here...";
            fileUpload.CssClass = "editTicket-fileupload-ctrl";


            //fileUpload.ControlStyle.

            //fileUpload.FileUploadMode = UploadControlFileUploadMode.OnPageLoad;

            fileUpload.FileUploadComplete += OnFileUploadComplete;
            fileUpload.Width = Unit.Percentage(100);
            textBox = new TextBox();
           // fileUpload.CssClass = "fileUploadIcon editTicket_fileUpload table";
            //fileUpload.ClientSideEvents.Init = "function(s, e) { ASPxClientUtils.AttachEventToElement(s.GetMainElement(), 'blur', function (event) { var isValid = uploader.GetText() != ''; img1.SetVisible(!isValid);}";
            //fileUpload.ClientSideEvents.FileUploadComplete = "function(s, e) {  if (e.callbackData) { var refreshurl=''; var data = ''; var height=60; var width=70; var dd=0; var fileData = e.callbackData.split('|'); var fileName = fileData[0], viewUrl = fileData[1], deleteUrl = fileData[2], fileSize = fileData[3],fileid=fileData[4], panelId = s.name.concat('_imageLinks'); var mdiv = document.createElement('span'); mdiv.style.width = '100%'; mdiv.id=fileid; $(mdiv).css({'background-color': '#E8F5F8','padding':'5px 4px','border-radius':'8px','margin-right':'5px', 'margin-bottom':'2px'}); var hideField=document.getElementById(s.name.concat('_hiddenField'));var valueList = $(hideField).val().split(',');if(fileid!=null && fileid!='' && fileid!=undefined){ valueList.push(fileid);} if(valueList.length>0){ $(hideField).val(valueList.join(','));}  var mydiv = document.getElementById(panelId); var aTag = document.createElement('a'); aTag.setAttribute('style','color:#000'); aTag.setAttribute('href', viewUrl);  aTag.innerHTML = fileName; var bTag = document.createElement('a'); bTag.setAttribute('href', '#'); bTag.setAttribute('onclick', 'window.deleteDocument(\\''+fileData[4] +'\\')'); var iurl='/content/images/cancelwhite.png';  bTag.innerHTML ='<img src=\\''+iurl+'\\'/>';  mdiv.appendChild(aTag);  mdiv.appendChild(bTag); mydiv.appendChild(mdiv);  } }";
            fileUpload.ClientSideEvents.FileUploadComplete = "function(s, e) { if (e.callbackData) { var refreshurl=''; var data = ''; var height=60; var width=70; var dd=0; var fileData = e.callbackData.split('|'); var fileName = fileData[0], viewUrl = fileData[1], deleteUrl = fileData[2], fileSize = fileData[3],fileid=fileData[4], panelId = s.name.concat('_imageLinks'); var mdiv = document.createElement('span'); mdiv.style.width = '100%';$(mdiv).class='chetan'; mdiv.id=fileid; $(mdiv).css({'padding':'5px 4px','border-radius':'8px','margin-right':'5px', 'margin-bottom':'2px'}); var hideField=document.getElementById(s.name.concat('_hiddenField'));   var textField=document.getElementById(s.name.concat('fileUpload_TXTVALUE'));  var msglabel = document.getElementById(s.name.concat('_Label')); msglabel.innerHTML = '';var textFieldList=$(textField).val().split(','); var valueList = $(hideField).val().split(',');if(fileid!=null && fileid!='' && fileid!=undefined){ valueList.push(fileid);} if(valueList.length>0){ $(hideField).val(valueList.join(',')); $(textField).val(valueList.join(','));}  var mydiv = document.getElementById(panelId); var aTag = document.createElement('a'); aTag.setAttribute('style','color:#000'); aTag.setAttribute('class','hyperLinkIcon'); aTag.setAttribute('href', viewUrl);aTag.setAttribute('title',fileName);  aTag.innerHTML = fileName;var iurl='/content/images/close-red.png'; var bTag = document.createElement('img'); bTag.setAttribute('src', iurl);bTag.setAttribute('class','cancelUploadedFiles'); bTag.setAttribute('onclick', 'window.deleteDocument(\\''+fileData[4] +'\\')');  mdiv.appendChild(aTag);  mdiv.appendChild(bTag); mydiv.appendChild(mdiv);  } }";
            fileUpload.ClientSideEvents.FilesUploadStart = "function (s, e) { var fileName=s.GetText(),panelId= s.name.concat('_imageLinks'); var msglabel= document.getElementById(s.name.concat('_Label')); msglabel.style.color='red'; var mydiv = document.getElementById(panelId); if(mydiv.innerText.indexOf(fileName.toLowerCase().substring(12,fileName.length)) != -1){e.cancel = true; msglabel.innerHTML ='File already uploaded!'; return;}  var hideField = document.getElementById(s.name.concat('_hiddenField')); var valueList = $(hideField).val().split(',');if (" + MaxFile + " > 0 && valueList.length > " + MaxFile + ") {e.cancel = true; msglabel.innerHTML='You can upload only " + MaxFile + " files'; return;} msglabel.innerHTML='';}";
            fileUpload.CancelButton.Text = "";

            imageLinks = new Panel();
            imageLinks.CssClass = "uploadedFileContainer";
            //imageLinks.Attributes.Add("style", "margin:5px 0px;");

            MsgLabel.Attributes.Add("style", "color:red; margin:5px; font-weight:bold;");
            textBox.Attributes.Add("style", "display:none;");
           

            panel = new Panel();
            panel.Controls.Add(fileUpload);
            panel.Controls.Add(hiddenField);
            panel.Controls.Add(textBox);
            panel.Controls.Add(imageLinks);
            //panel.Controls.Add(fileUpload);
            panel.Controls.Add(MsgLabel);
            panel.Controls.Add(loadingPanel);
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (controlMode == ControlMode.New)
            {
                controlMode = ControlMode.Edit;
            }

            var newValue = new List<string>();
            string value = hiddenField.Value;

            if (!string.IsNullOrEmpty(value))
            {
                Panel panelLinks = imageLinks;
                if (panelLinks != null)
                {
                    DocumentManager documentManager = new DocumentManager(HttpContext.Current.GetManagerContext());
                    List<string> list = UGITUtility.SplitString(value, ",").ToList();
                    list.ForEach(x =>
                    {
                        if (!string.IsNullOrWhiteSpace(x))
                        {
                            Utility.Entities.Document document = documentManager.Get(y => y.FileID == x);
                            if (document != null)
                            {
                                newValue.Add(document.FileID);
                                string viewURL = UGITUtility.GetAbsoluteURL("/ControlTemplates/uGovernIT/DocumentControl.aspx?control=image&id=" + document.FileID + "&extension=" + document.Extension + "&action=view");
                                string deleteURL = string.Empty;

                                HtmlGenericControl divCollection = new HtmlGenericControl("span");
                                divCollection.Attributes.Add("style", "padding:5px 4px;border-radius:8px;margin-right:5px; margin-bottom:2px; ");
                                divCollection.ClientIDMode = ClientIDMode.Static;
                                divCollection.ID = document.FileID;

                                HyperLink openlink = new HyperLink();
                                openlink.NavigateUrl = viewURL;
                                openlink.Attributes.Add("style", "color:#000");
                                openlink.Text = document.Name;
                                divCollection.Controls.Add(openlink);

                                if (controlMode == ControlMode.Edit)
                                {
                                    Image deleteImage = new Image();
                                    deleteImage.ImageUrl = "/content/images/close-red.png";
                                    deleteImage.Attributes.Add("onclick", "window.deleteDocument(\"" + document.FileID + "\")");
                                    divCollection.Controls.Add(deleteImage);
                                }
                                panelLinks.Controls.Add(divCollection);
                            }
                        }
                    });
                }
                if (controlMode == ControlMode.Edit || controlMode == ControlMode.New)
                    fileUpload.Visible = true;
            }
            hiddenField.Value = string.Join(",", newValue);
        }

        protected override void OnInit(EventArgs e)
        {
            if (controlMode == ControlMode.Invalid)
                controlMode = ControlMode.New;

            if (!string.IsNullOrWhiteSpace(FileExtension))
            {
                string[] list = FileExtension.Split(',');
                fileUpload.ValidationSettings.AllowedFileExtensions = list;
            }

            if (string.IsNullOrWhiteSpace(DisplayText))
            {
                fileUpload.BrowseButton.Text = "";
                fileUpload.BrowseButtonStyle.CssClass = " clssvcattach";
                fileUpload.BrowseButton.Image.Url = ("/Content/Images/plus-blue-new.png");
                fileUpload.BrowseButton.Image.Width = 25;
            }
            else
            {
                fileUpload.BrowseButton.Text = DisplayText;
            }
                

            if (FieldName != null)
            {
                FieldConfigurationManager fieldManager = new FieldConfigurationManager(HttpContext.Current.GetManagerContext());
                FieldConfiguration field = fieldManager.GetFieldByFieldName(FieldName);
                if (field != null && field.Multi)
                {
                    Multi = field.Multi;
                }
            }

            fileUpload.AdvancedModeSettings.EnableMultiSelect = Multi;
            if (MaxFile > 0)
                fileUpload.ValidationSettings.MaxFileCount = MaxFile;
            fileUpload.ID = this.ID + "_fileUpload";
            imageLinks.ID = fileUpload.ID + "_imageLinks";
            hiddenField.ID = fileUpload.ID + "_hiddenField";
            MsgLabel.ID = fileUpload.ID + "_Label";
            loadingPanel.ID = fileUpload.ID + "_loadingPanel";
            textBox.ID = fileUpload.ID + "fileUpload_TXTVALUE";

            if (IsMandate)
            {
                requiredFieldValidator = new RequiredFieldValidator();
                panel.Controls.Add(requiredFieldValidator);
                textBox.ValidationGroup = ValidationGroup;
                requiredFieldValidator.ID = fileUpload.ID + "fileUpload_RFD";
                requiredFieldValidator.ControlToValidate = textBox.ID;
                requiredFieldValidator.ValidationGroup = ValidationGroup;
                requiredFieldValidator.Display = ValidatorDisplay.Dynamic;
                requiredFieldValidator.Enabled = true;
                requiredFieldValidator.ErrorMessage = "Atleast one file required";
            }

            loadingPanel.Modal = true;
            if (controlMode == ControlMode.Edit || controlMode == ControlMode.New)
                fileUpload.Visible = true;
            else
                fileUpload.Visible = false;

            Controls.Add(panel);

            base.OnInit(e);
        }

        private void OnFileUploadComplete(object sender, FileUploadCompleteEventArgs e)
        {
            string resultExtension = e.UploadedFile.FileName;
            ASPxUploadControl aspxUpload = sender as ASPxUploadControl;

            Utility.Entities.Document document = new Utility.Entities.Document();
            document.Name = e.UploadedFile.FileName.Trim();
            document.Size = e.UploadedFile.ContentLength / 1024;
            document.Blob = e.UploadedFile.FileBytes;
            document.Extension = e.UploadedFile.FileName.Split('.').Last();
            document.ContentType = e.UploadedFile.ContentType;
            document.FileID = Guid.NewGuid().ToString();

            DocumentManager documentManager = new DocumentManager(HttpContext.Current.GetManagerContext());
            documentManager.Insert(document);

            if (document.Id > 0)
            {
                string viewURL = UGITUtility.GetAbsoluteURL("/ControlTemplates/uGovernIT/DocumentControl.aspx?control=image&id=" + document.FileID + "&extension=" + document.Extension + "&action=view");
                string deleteURL = string.Empty;
                e.CallbackData = document.Name + "|" + viewURL + "|" + deleteURL + "|" + document.Size + "|" + document.FileID;
            }
        }

        protected override void OnDataBinding(EventArgs e)
        {
            base.OnDataBinding(e);
        }

        protected override void OnLoad(EventArgs e)
        {

            base.OnLoad(e);
        }

        public string GetValue()
        {
            return this.hiddenField.Value.ToString();
        }

        public void SetValue(string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                hiddenField.Value = value;
                textBox.Text = value;
            }
        }
    }
}
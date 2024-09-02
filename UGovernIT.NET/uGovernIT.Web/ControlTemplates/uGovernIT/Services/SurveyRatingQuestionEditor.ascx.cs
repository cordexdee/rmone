using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Utility;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using uGovernIT.Manager;
using System.Web;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Web
{
    public partial class SurveyRatingQuestionEditor : UserControl
    {
        protected int SVCConfigID { get; set; }
        protected int QuestionID { get; set; }
        private ServiceQuestion serviceQuest;
        Services service;
        ServicesManager serviceManager = new ServicesManager(HttpContext.Current.GetManagerContext());
        ServiceQuestionManager questionMGR = new ServiceQuestionManager(HttpContext.Current.GetManagerContext());
        ServiceSectionManager sectionMGR = new ServiceSectionManager(HttpContext.Current.GetManagerContext());
        ServiceQuestionMappingManager questionMappingMGR = new ServiceQuestionMappingManager(HttpContext.Current.GetManagerContext());
        bool saveAndNewQuestionEnable;
        List<string> existingRating = null;
        UserProfile User;
        protected override void OnInit(EventArgs e)
        {
            User = HttpContext.Current.CurrentUser();
            //Load service id from request
            int svcConfigID = 0;
            int.TryParse(Request["svcConfigID"], out svcConfigID);
            SVCConfigID = svcConfigID;

            //load service question id from request
            int questionID = 0;
            int.TryParse(Request["questionID"], out questionID);
            QuestionID = questionID;

            hfQuestionID.Value = QuestionID.ToString();
            hfServiceID.Value = SVCConfigID.ToString();
           
            //Load Service object based on service id
            if (SVCConfigID > 0)
            {
                service = serviceManager.LoadByServiceID(SVCConfigID);
            }

            //Filled form if sevice question is exist
            if (service != null && QuestionID > 0)
            {
                ddlRatingFields.Enabled = false;
                serviceQuest = service.Questions.FirstOrDefault(x => x.ID == QuestionID);
                if (service.ID == serviceQuest.ServiceID)
                {
                    //IF the question is not rating type the close popup right away
                    if (serviceQuest.QuestionType != "Rating")
                    {
                        uHelper.ClosePopUpAndEndResponse(Context, true);
                        return;
                    }

                    chkMandatory.Checked = serviceQuest.FieldMandatory;
                   
                    btDelete.Visible = true;
                    txtQuestion.Text = serviceQuest.QuestionTitle;
                    questionHelpTextNew.Text = serviceQuest.Helptext;
                    ddlRatingFields.SelectedIndex = ddlRatingFields.Items.IndexOf(ddlRatingFields.Items.FindByValue(serviceQuest.TokenName));
                    KeyValuePair<string, string> param = serviceQuest.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "options");
                    if (param.Key != null)
                    {
                        string[] valueArray = param.Value.Split(new string[] { Constants.Separator1 }, StringSplitOptions.None);
                        List<string> optionList = valueArray.ToList();
                        if (optionList.Count >= 3)
                        {
                            if (optionList.Count == 3)
                            {
                                rbtn3.Checked = true;
                                txtR1.Text = optionList[0];
                                txtR2.Text = optionList[1];
                                txtR3.Text = optionList[2];

                                tr8.Visible = false;
                                tr9.Visible = false;
                                tr10.Visible = false;
                                tr3.Visible = false;
                            }
                            else if (optionList.Count == 4)
                            {
                                rbtn4.Checked = true;
                                txtR1.Text = optionList[0];
                                txtR2.Text = optionList[1];
                                txtR3.Text = optionList[2];
                                txtR4.Text = optionList[3];

                                tr8.Visible = true;
                                tr9.Visible = false;
                                tr10.Visible = false;
                                tr3.Visible = false;


                            }
                            else if (optionList.Count == 5)
                            {
                                rbtn5.Checked = true;
                                txtR1.Text = optionList[0];
                                txtR2.Text = optionList[1];
                                txtR3.Text = optionList[2];
                                txtR4.Text = optionList[3];
                                txtR5.Text = optionList[4];

                                tr8.Visible = true;
                                tr9.Visible = true;
                                tr10.Visible = false;
                                tr3.Visible = false;
                            }
                            else if (optionList.Count == 6)
                            {
                                rbtn6.Checked = true;
                                txtR1.Text = optionList[0];
                                txtR2.Text = optionList[1];
                                txtR3.Text = optionList[2];
                                txtR4.Text = optionList[3];
                                txtR5.Text = optionList[4];
                                txtR6.Text = optionList[5];

                                tr8.Visible = true;
                                tr9.Visible = true;
                                tr10.Visible = false;
                                tr3.Visible = true;
                            }
                            else if (optionList.Count == 7)
                            {
                                rbtn7.Checked = true;
                                txtR1.Text = optionList[0];
                                txtR2.Text = optionList[1];
                                txtR3.Text = optionList[2];
                                txtR4.Text = optionList[3];
                                txtR5.Text = optionList[4];
                                txtR6.Text = optionList[5];
                                txtR7.Text = optionList[6];

                                tr8.Visible = true;
                                tr9.Visible = true;
                                tr10.Visible = true;
                                tr3.Visible = true;
                            }
                            //Spdelta 155
                            else if (optionList.Count == 8)
                            {
                                rbtn8.Checked = true;
                                txtR1.Text = optionList[0];
                                txtR2.Text = optionList[1];
                                txtR3.Text = optionList[2];
                                txtR4.Text = optionList[3];
                                txtR5.Text = optionList[4];
                                txtR6.Text = optionList[5];
                                txtR7.Text = optionList[6];
                                txtR8.Text = optionList[7];
                                tr8.Visible = true;
                                tr9.Visible = true;
                                tr10.Visible = true;
                                tr3.Visible = true;
                                tr8a.Visible = true;
                            }
                            else if (optionList.Count == 9)
                            {
                                rbtn9.Checked = true;
                                txtR1.Text = optionList[0];
                                txtR2.Text = optionList[1];
                                txtR3.Text = optionList[2];
                                txtR4.Text = optionList[3];
                                txtR5.Text = optionList[4];
                                txtR6.Text = optionList[5];
                                txtR7.Text = optionList[6];
                                txtR8.Text = optionList[7];
                                txtR9.Text = optionList[8];
                                tr8.Visible = true;
                                tr9.Visible = true;
                                tr10.Visible = true;
                                tr3.Visible = true;
                                tr8a.Visible = true;
                                tr9a.Visible = true;
                            }
                            else if (optionList.Count == 10)
                            {
                                rbtn10.Checked = true;
                                txtR1.Text = optionList[0];
                                txtR2.Text = optionList[1];
                                txtR3.Text = optionList[2];
                                txtR4.Text = optionList[3];
                                txtR5.Text = optionList[4];
                                txtR6.Text = optionList[5];
                                txtR7.Text = optionList[6];
                                txtR8.Text = optionList[7];
                                txtR9.Text = optionList[8];
                                txtR10.Text = optionList[9];
                                tr8.Visible = true;
                                tr9.Visible = true;
                                tr10.Visible = true;
                                tr3.Visible = true;
                                tr8a.Visible = true;
                                tr9a.Visible = true;
                                tr10a.Visible = true;
                            }
                            //
                            //txtR1.Text = optionList[0];
                            //txtR2.Text = optionList[1];
                            //txtR3.Text = optionList[2];
                            //txtR4.Text = optionList[3];
                            //txtR5.Text = optionList[4];
                        }
                    }

                    param = serviceQuest.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "weight");
                    if (param.Key != null)
                    {
                        txtWeight.Text = param.Value;
                    }

                    param = serviceQuest.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "displaymode");
                    if (param.Key != null)
                        ddlRatingdisplaymode.SelectedIndex = ddlRatingdisplaymode.Items.IndexOf(ddlRatingdisplaymode.Items.FindByValue(param.Value));

                    param = serviceQuest.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "defaultvalue");
                    if (param.Key != null)
                        txtDefaultValue.Text = param.Value;
                }
            }
            else
                chkMandatory.Checked = true;
           

            base.OnInit(e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (service != null && QuestionID == 0 && !IsPostBack)
            {
                if (service.Questions != null && service.Questions.Count > 0)
                {
                    existingRating = service.Questions.Where(x => x.QuestionType.Equals("Rating")).Select(y => y.TokenName).Distinct().ToList();
                    if (existingRating != null)
                    {
                        existingRating.ForEach(x =>
                        {
                            if (ddlRatingFields.Items.Cast<ListItem>().Any(y => y.Value.Equals(x)))
                            {
                                ListItem item = ddlRatingFields.Items.FindByValue(x);
                                if (item != null)
                                    ddlRatingFields.Items.Remove(item);
                            }

                        });
                    }
                }
            }
        }
       
        protected void BtSaveQuestionClick(object sender, EventArgs e)
        {
            ServiceQuestion question = null;
            if (!Page.IsValid)
            {
                return;
            }

            int svcConfigID = 0;
            int.TryParse(hfServiceID.Value, out svcConfigID);

            int questionID = 0;
            int.TryParse(hfQuestionID.Value, out questionID);

            if (svcConfigID > 0)
            {
                if (serviceQuest == null && questionID <= 0)
                {
                    question = new ServiceQuestion(svcConfigID);
                    int maxOrder = 0;
                    if (service.Questions != null && service.Questions.Count > 0)
                    {
                        List<ServiceQuestion> ratingQuestions = service.Questions.Where(x => x.QuestionType.ToLower() == Constants.ServiceQuestionType.Rating).ToList();
                        //finds next order number
                        maxOrder = ratingQuestions.Count - 9;
                        question.ItemOrder = maxOrder;
                    }
                }
                else
                {
                    question = serviceQuest;
                }

                if (question.ServiceID == svcConfigID)
                {
                    question.TokenName = string.Empty;// txtToken.Text.Trim();
                    question.QuestionType = "Rating";
                    question.TokenName = ddlRatingFields.SelectedValue;
                    question.QuestionTitle = txtQuestion.Text.Trim();
                    question.Helptext = questionHelpTextNew.Text.Trim();
                    question.FieldMandatory = chkMandatory.Checked;
                    //Gets all  sections service
                    List<ServiceSection> sections = sectionMGR.Load(x=>x.ServiceID == service.ID);  // ServiceSection.GetByServiceID(service.ID);
                    ServiceSection section = null;
                    //Gets first section if exist
                    if (sections != null && sections.Count > 0)
                    {
                        section = sections.OrderBy(x => x.ItemOrder).First();
                    }

                    //Creates new section if not section exist for service
                    if (section == null)
                    {
                        section = new ServiceSection();
                        section.Title = "Section1";
                        section.ItemOrder = 1;
                        section.ServiceID = question.ServiceID;
                        //section.Save();
                        sectionMGR.Insert(section);
                    }

                    question.ServiceSectionID = section.ID;

                    //Store Rating option value in custom property
                    Dictionary<string, string> dataTypeParams = new Dictionary<string, string>();
                    List<string> ratingOptions = new List<string>();
                    ratingOptions.Add(txtR1.Text.Trim());
                    ratingOptions.Add(txtR2.Text.Trim());
                    ratingOptions.Add(txtR3.Text.Trim());
                    if( rbtn4.Checked)
                    ratingOptions.Add(txtR4.Text.Trim());
                    else if (rbtn5.Checked)
                    {
                        ratingOptions.Add(txtR4.Text.Trim());
                        ratingOptions.Add(txtR5.Text.Trim());
                    }
                    else if (rbtn6.Checked)
                    {
                        ratingOptions.Add(txtR4.Text.Trim());
                        ratingOptions.Add(txtR5.Text.Trim());
                        ratingOptions.Add(txtR6.Text.Trim());
                    }
                    else if (rbtn7.Checked)
                    {
                        ratingOptions.Add(txtR4.Text.Trim());
                        ratingOptions.Add(txtR5.Text.Trim());
                        ratingOptions.Add(txtR6.Text.Trim());
                        ratingOptions.Add(txtR7.Text.Trim());
                    }
                    //SpDelta 155
                    else if (rbtn8.Checked)
                    {
                        ratingOptions.Add(txtR4.Text.Trim());
                        ratingOptions.Add(txtR5.Text.Trim());
                        ratingOptions.Add(txtR6.Text.Trim());
                        ratingOptions.Add(txtR7.Text.Trim());
                        ratingOptions.Add(txtR8.Text.Trim());
                    }
                    else if (rbtn9.Checked)
                    {
                        ratingOptions.Add(txtR4.Text.Trim());
                        ratingOptions.Add(txtR5.Text.Trim());
                        ratingOptions.Add(txtR6.Text.Trim());
                        ratingOptions.Add(txtR7.Text.Trim());
                        ratingOptions.Add(txtR8.Text.Trim());
                        ratingOptions.Add(txtR9.Text.Trim());
                    }
                    else if (rbtn10.Checked)
                    {
                        ratingOptions.Add(txtR4.Text.Trim());
                        ratingOptions.Add(txtR5.Text.Trim());
                        ratingOptions.Add(txtR6.Text.Trim());
                        ratingOptions.Add(txtR7.Text.Trim());
                        ratingOptions.Add(txtR8.Text.Trim());
                        ratingOptions.Add(txtR9.Text.Trim());
                        ratingOptions.Add(txtR10.Text.Trim());
                    }
                    //
                    // ratingOptions.Add(txtR5.Text.Trim());
                    dataTypeParams.Add("options", string.Join(Constants.Separator1, ratingOptions.ToArray()));

                    //Weight of rating
                    double weight = 0;
                    double.TryParse(txtWeight.Text.Trim(), out weight);
                    dataTypeParams.Add("weight", weight.ToString());
                    dataTypeParams.Add("displaymode", Convert.ToString(ddlRatingdisplaymode.Value));
                    dataTypeParams.Add("defaultvalue", Convert.ToString(txtDefaultValue.Text.Trim()));

                    question.QuestionTypePropertiesDicObj = dataTypeParams;
                    if (dataTypeParams != null)
                    {
                        Dictionary<string, object> dString = dataTypeParams.ToDictionary(k => k.Key, k => k.Value == null ? "" : k.Value as object);
                        question.QuestionTypeProperties = UGITUtility.SerializeDicObject(dString);
                    }
                    if (question.ID > 0) {
                        questionMGR.Update(question);
                    }
                    else
                    {
                        questionMGR.Insert(question);
                    }

                    ServiceQuestionMapping mapping = null;
                    if(service.QuestionsMapping != null && service.QuestionsMapping.Count> 0)
                    {
                        mapping = service.QuestionsMapping.FirstOrDefault(x => x.ColumnName == question.TokenName);
                        if (mapping == null)
                        {
                            mapping = new ServiceQuestionMapping();
                            mapping.ServiceID = service.ID;
                        }
                        mapping.ServiceQuestionID = question.ID;
                        mapping.ColumnName = question.TokenName;
                        mapping.ColumnValue = "[$Current$]";
                        questionMappingMGR.Update(mapping);
                    }
                    else
                    {
                        mapping = new ServiceQuestionMapping();
                        mapping.ServiceID = service.ID;
                        mapping.ServiceQuestionID = question.ID;
                        mapping.ColumnName = question.TokenName;
                        mapping.ColumnValue = "[$Current$]";
                        //mapping.Save();
                        questionMappingMGR.Insert(mapping);

                    }
                }


                if (saveAndNewQuestionEnable)
                {
                    //Redirect to create new question
                    string questionListUrl = UGITUtility.GetAbsoluteURL(Request.Path);
                    NameValueCollection queryCollection = new System.Collections.Specialized.NameValueCollection(Request.QueryString);
                    StringBuilder querys = new StringBuilder();
                    foreach (string key in queryCollection.AllKeys)
                    {
                        if (key == null)
                            continue;

                        if (key.Trim() != "questionid")
                            querys.AppendFormat("&{0}={1}", key, queryCollection[key]);
                        else
                            querys.AppendFormat("&questionid=0");
                    }
                    questionListUrl = string.Format("{0}?{1}", questionListUrl, querys.ToString());
                    Context.Response.Redirect(questionListUrl);
                }
                else
                {
                    Context.Cache.Add(string.Format("SVCConfigRatingQuestion-{0}", User.Id), true, null, DateTime.Now.AddMinutes(50), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Normal, null);
                    uHelper.ClosePopUpAndEndResponse(Context, true);
                }
            }
        }

        protected void BtSaveAndNewQuestionClick(object sender, EventArgs e)
        {
            saveAndNewQuestionEnable = true;
            BtSaveQuestionClick(sender, e);
        }

        protected void BtDelete_Click(object sender, EventArgs e)
        {
            if (serviceQuest != null)
            {
                //Delete question mapping along with question
                List<ServiceQuestionMapping> mapping = service.QuestionsMapping.Where(x => x.ServiceQuestionID == serviceQuest.ID).ToList();
                //ServiceQuestionMapping m = mapping[0];
                //ServiceQuestionMapping.Delete(mapping);
                foreach(ServiceQuestionMapping m in mapping)
                {
                    questionMappingMGR.Delete(m);
                }
                serviceManager.Delete(service);
                //serviceQuest.Delete(service);

                Context.Cache.Add(string.Format("SVCConfigRatingQuestion-{0}", User.Id), true, null, DateTime.Now.AddMinutes(50), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Normal, null);
                uHelper.ClosePopUpAndEndResponse(Context, true);
            }
        }

        protected void BtClosePopup_Click(object sender, EventArgs e)
        {
            Context.Cache.Add(string.Format("SVCConfigRatingQuestion-{0}", User.Id), true, null, DateTime.Now.AddMinutes(50), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Normal, null);
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void CVFieldValidator1_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            if (service != null)
            {
                if (service.Questions != null)
                {
                    ServiceQuestion question = service.Questions.FirstOrDefault(x => x.ID != QuestionID && x.TokenName.ToLower() == e.Value.ToLower());
                    if (question != null)
                    {
                        e.IsValid = false;
                    }
                }
            }
        }

        protected void RFValidator_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            if (service != null)
            {
                if (service.Questions != null)
                {
                    ServiceQuestion question = service.Questions.FirstOrDefault(x => x.ID == QuestionID);
                    ServiceQuestionMapping qMapping = service.QuestionsMapping.FirstOrDefault(x => x.ColumnName == ddlRatingFields.SelectedValue);
                    if ((question == null && qMapping != null) || (question != null && qMapping != null && question.ID != qMapping.ServiceQuestionID))
                    {
                        e.IsValid = false;
                    }
                }
            }
        }

        protected void rbtn3_CheckedChanged(object sender, EventArgs e)
        {
            tr8.Visible = false;
            tr9.Visible = false;
            tr10.Visible = false;
            tr3.Visible = false;
            //
            tr8a.Visible = false;
            tr9a.Visible = false;
            tr10a.Visible = false;
            //

            txtR3.Text = "High";
        }

        protected void rbtn4_CheckedChanged(object sender, EventArgs e)
        {
            tr8.Visible = true;
            tr9.Visible = false;
            tr10.Visible = false;
            tr3.Visible = false;
            //
            //
            tr8a.Visible = false;
            tr9a.Visible = false;
            tr10a.Visible = false;
            //
            //
            txtR3.Text = "3";
            txtR4.Text = "High";
        }

        protected void rbtn5_CheckedChanged(object sender, EventArgs e)
        {
            tr8.Visible = true;
            tr9.Visible = true;
            tr10.Visible = false;
            tr3.Visible = false;
            //
            //
            tr8a.Visible = false;
            tr9a.Visible = false;
            tr10a.Visible = false;
            //
            //
            txtR3.Text = "3";
            txtR4.Text = "4";
            txtR5.Text = "High";
        }

        protected void rbtn6_CheckedChanged(object sender, EventArgs e)
        {
            tr8.Visible = true;
            tr9.Visible = true;
            tr10.Visible = false;
            tr3.Visible = true;
            //
            tr8a.Visible = false;
            tr9a.Visible = false;
            tr10a.Visible = false;
            //
            txtR3.Text = "3";
            txtR4.Text = "4";
            txtR5.Text = "5";
            txtR6.Text = "High";
        }

        protected void rbtn7_CheckedChanged(object sender, EventArgs e)
        {
            tr8.Visible = true;
            tr9.Visible = true;
            tr10.Visible = true;
            tr3.Visible = true;
            //
            tr8a.Visible = false;
            tr9a.Visible = false;
            tr10a.Visible = false;
            //
            txtR3.Text = "3";
            txtR4.Text = "4";
            txtR5.Text = "5";
            txtR6.Text = "6";
            txtR7.Text = "High";
        }
        //Spdelta 155
        protected void rbtn8_CheckedChanged(object sender, EventArgs e)
        {
           
            //
            tr8.Visible = true;
            tr9.Visible = true;
            tr3.Visible = true;
            tr10.Visible = true;
            tr8a.Visible = true;
            tr9a.Visible = false;
            tr10a.Visible = false;
            //
            txtR3.Text = "3";
            txtR4.Text = "4";
            txtR5.Text = "5";
            txtR6.Text = "6";
            txtR7.Text = "7";
            txtR8.Text = "High";
           
        }
        protected void rbtn9_CheckedChanged(object sender, EventArgs e)
        {

            //
            tr8.Visible = true;
            tr9.Visible = true;
            tr3.Visible = true;
            tr10.Visible = true;
            tr8a.Visible = true;
            tr9a.Visible = true;
            tr10a.Visible = false;
            //
            txtR3.Text = "3";
            txtR4.Text = "4";
            txtR5.Text = "5";
            txtR6.Text = "6";
            txtR7.Text = "7";
            txtR8.Text = "8";
            txtR9.Text = "High";
            
        }
        protected void rbtn10_CheckedChanged(object sender, EventArgs e)
        {

            //
            tr8.Visible = true;
            tr9.Visible = true;
            tr3.Visible = true;
            tr10.Visible = true;
            tr8a.Visible = true;
            tr9a.Visible = true;
            tr10a.Visible = true;
            //
            txtR3.Text = "3";
            txtR4.Text = "4";
            txtR5.Text = "5";
            txtR6.Text = "6";
            txtR7.Text = "7";
            txtR8.Text = "8";
            txtR9.Text = "9";
            txtR10.Text = "High";
        }

        //

    }
}

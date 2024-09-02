using System;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using uGovernIT.Utility;
using System.Linq;
using uGovernIT.Manager;
using uGovernIT.Utility.Entities;
using System.Web;

namespace uGovernIT.Web
{
    public partial class ServiceQuestionWorkFlow : UserControl
    {
        public Services service { get; set; }
        public string jsonSkipLogic { get; set; }
        public string jsonQuestions { get; set; }
        public string jsonSections { get; set; }
        public string lastSectionID { get; set; }

        public string editServiceQuestionUrl { get; set; }
        public string editservicequestionbranchUrl { get; set; }
        UserProfile User;
        protected override void OnInit(EventArgs e)
        {
            User = HttpContext.Current.CurrentUser();
            editServiceQuestionUrl = UGITUtility.GetAbsoluteURL("/layouts/ugovernit/DelegateControl.aspx?control=servicequestioneditor");
            editservicequestionbranchUrl = UGITUtility.GetAbsoluteURL("/layouts/ugovernit/DelegateControl.aspx?control=editservicequestionbranch");
            if (service != null)
            {
                if (service.Questions.Count > 0)
                {
                    if (service.SkipSectionCondition != null && service.SkipSectionCondition.Count > 0)
                    {
                        lastSectionID = service.Sections.OrderByDescending(x => x.ItemOrder).FirstOrDefault().ID.ToString();
                    }

                    rptSection.DataSource = service.Sections;
                    rptSection.DataBind();


                }
            }
            base.OnInit(e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            //ClientScriptManager script = new ClientScriptManager();
            //script.RegisterStartupScript(
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (service.SkipSectionCondition != null && service.SkipSectionCondition.Count > 0)
            {
                foreach (var skipcondition in service.SkipSectionCondition)
                {
                    string moduleName;
                    int itemid;
                    if (!string.IsNullOrEmpty(skipcondition.ConditionVar))
                    {
                        skipcondition.ConditionValForWF = skipcondition.ConditionVal;
                        string variable = skipcondition.ConditionVar;
                        var question = service.Questions.Where(x => x.TokenName == variable).FirstOrDefault();
                        if (question != null)
                        {
                            switch (question.QuestionType)
                            {
                                case "UserField":
                                    int userid;
                                    int.TryParse(skipcondition.ConditionVal, out userid);
                                    var user = User.Id;
                                    if (user != null)
                                    {
                                        skipcondition.ConditionValForWF = User.UserName;
                                    }

                                    break;
                                case "Checkbox":
                                    if (skipcondition.ConditionVal == "True")
                                    {
                                        skipcondition.ConditionValForWF = "True";
                                    }
                                    else
                                    {
                                        skipcondition.ConditionValForWF = "False";
                                    }

                                    break;
                                case "RequestType":
                                    int requestId;
                                    int.TryParse(skipcondition.ConditionVal, out requestId);
                                    moduleName = question.QuestionTypePropertiesDicObj["module"];
                                    //var splistItem = SPListHelper.GetSPListItem(DatabaseObjects.Lists.RequestType, requestId);
                                    //if (splistItem != null)
                                    //{
                                    //    skipcondition.ConditionValForWF = Convert.ToString(splistItem[DatabaseObjects.Columns.Title]);
                                    //}
                                    break;
                                case "MultiChoice":
                                    if (skipcondition.ConditionVal.Contains("~"))
                                    {
                                        skipcondition.ConditionValForWF = skipcondition.ConditionVal.Replace("~", ", ");
                                    }
                                    break;

                                case "DateTime":
                                    DateTime dt;
                                    if (DateTime.TryParse(skipcondition.ConditionVal, out dt))
                                    {
                                        skipcondition.ConditionValForWF = dt.ToString("MMM-dd-yyyy}");
                                    }
                                    break;
                                case "Lookup":
                                    string listName = question.QuestionTypePropertiesDicObj["lookuplist"];
                                    //moduleName = question.QuestionTypeProperties["module"];
                                    string lookupfield = question.QuestionTypePropertiesDicObj["lookupfield"];
                                    string multi = question.QuestionTypePropertiesDicObj["multi"];
                                    int.TryParse(skipcondition.ConditionVal, out itemid);
                                    //var spListItem = SPListHelper.GetSPListItem(listName, itemid);
                                    //if (spListItem != null)
                                    //{
                                    //    skipcondition.ConditionValForWF = Convert.ToString(spListItem[lookupfield]);
                                    //}
                                    break;

                                default:
                                    break;
                            }
                        }
                    }
                    if (skipcondition.Conditions != null && skipcondition.Conditions.Count > 0)
                    {
                        foreach (var condition in skipcondition.Conditions)
                        {
                            condition.ValueForWF = condition.Value;
                            string variable = condition.Variable;
                            var question = service.Questions.Where(x => x.TokenName == variable).FirstOrDefault();
                            if (question != null)
                            {
                                switch (question.QuestionType)
                                {
                                    case "UserField":
                                        int userid;
                                        int.TryParse(condition.Value, out userid);
                                        var user = User.Id;
                                        if (user != null)
                                        {
                                            condition.ValueForWF = User.UserName;
                                        }

                                        break;
                                    case "Checkbox":
                                        if (condition.Value == "True")
                                        {
                                            condition.ValueForWF = "True";
                                        }
                                        else
                                        {
                                            condition.ValueForWF = "False";
                                        }
                                        break;
                                    case "RequestType":
                                        int requestId;
                                        int.TryParse(condition.Value, out requestId);
                                        moduleName = question.QuestionTypePropertiesDicObj["module"];
                                        //var splistItem = SPListHelper.GetSPListItem(DatabaseObjects.Lists.RequestType, requestId);
                                        //if (splistItem != null)
                                        //{
                                        //    condition.ValueForWF = Convert.ToString(splistItem[DatabaseObjects.Columns.Title]);
                                        //}
                                        break;
                                    case "MultiChoice":
                                        if (condition.Value.Contains("~"))
                                        {
                                            condition.ValueForWF = condition.Value.Replace("~", ", ");
                                        }
                                        break;

                                    case "DateTime":
                                        DateTime dt;//= Convert.ToDateTime(condition.Value);
                                        if (DateTime.TryParse(condition.Value, out dt))
                                        {
                                            condition.ValueForWF = dt.ToString("MMM-dd-yyyy");
                                        }
                                        break;
                                    case "Lookup":
                                        string listName = question.QuestionTypePropertiesDicObj["lookuplist"];
                                        //moduleName = question.QuestionTypeProperties["module"];
                                        string lookupfield = question.QuestionTypePropertiesDicObj["lookupfield"];
                                        string multi = question.QuestionTypePropertiesDicObj["multi"];
                                        int.TryParse(condition.Value, out itemid);
                                        //var spListItem = SPListHelper.GetSPListItem(listName, itemid);
                                        //if (spListItem != null)
                                        //{
                                        //    condition.ValueForWF = Convert.ToString(spListItem[lookupfield]);
                                        //}
                                        break;

                                    default:
                                        break;
                                }
                            }

                        }
                    }
                }
            }
            jsonSkipLogic = Newtonsoft.Json.JsonConvert.SerializeObject(service.SkipSectionCondition);
            jsonQuestions = Newtonsoft.Json.JsonConvert.SerializeObject(service.Questions);
            jsonSections = Newtonsoft.Json.JsonConvert.SerializeObject(service.Sections);

            base.OnPreRender(e);
        }

        protected void rptquestion_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Literal lquestionNumber = e.Item.FindControl("lquestionNumber") as Literal;
                HtmlTableCell tdQuestion = e.Item.FindControl("tdQuestion") as HtmlTableCell;
                HtmlTableCell tdline = e.Item.FindControl("tdline") as HtmlTableCell;
                Literal stageTitle = e.Item.FindControl("stageTitle") as Literal;
                HtmlGenericControl stageTitleContainer = (HtmlGenericControl)e.Item.FindControl("stageTitleContainer");

                if (e.Item.DataItem is ServiceQuestion)
                {
                    /// for Question Number
                    ServiceQuestion sq = e.Item.DataItem as ServiceQuestion;
                    int lastItemOrder = service.Questions.Where(x => x.ServiceSectionID == sq.ServiceSectionID)
                                                         .OrderByDescending(x => x.ItemOrder).FirstOrDefault().ItemOrder;
                    stageTitle.Text = UGITUtility.TruncateWithEllipsis(sq.QuestionTitle, 35);
                    if (sq.ItemOrder == 1)
                    {
                        tdQuestion.Attributes.Add("class", string.Format("firstquestion quest_{0} {1}", sq.ID, sq.TokenName));

                    }
                    if (sq.ItemOrder > 1 && sq.ItemOrder != lastItemOrder)
                    {
                        tdQuestion.Attributes.Add("class", string.Format("midquestion quest_{0} {1}", sq.ID, sq.TokenName));
                    }
                    if (sq.ItemOrder == lastItemOrder)
                    {
                        tdQuestion.Attributes.Add("class", string.Format("lastquestion quest_{0} {1}", sq.ID, sq.TokenName));
                        tdline.Visible = false;
                    }
                    tdQuestion.Attributes.Add("onclick", string.Format("OpeneditQuestion({0});", sq.ID));

                    if (e.Item.ItemType == ListItemType.AlternatingItem)
                    {
                       // stageTitleContainer.Attributes.Add("class", "stageTitleContainer alternategraphiclabel");
                        stageTitleContainer.Style.Add(HtmlTextWriterStyle.Top, "-28px");
                    }
                    else
                    {
                        //stageTitleContainer.Attributes.Add("class", "stageTitleContainer");
                        stageTitleContainer.Style.Add(HtmlTextWriterStyle.Top, "40px");
                    }

                    lquestionNumber.Text = Convert.ToString(sq.ItemOrder);
                }
            }
        }

        protected void rptSection_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Repeater rptquestion = e.Item.FindControl("rptquestion") as Repeater;
                HtmlTableCell tdline = e.Item.FindControl("tdline") as HtmlTableCell;
                HtmlGenericControl section = e.Item.FindControl("section") as HtmlGenericControl;
                ListView lvStepSections = e.Item.FindControl("lvStepSections") as ListView;
                if (e.Item.DataItem is ServiceSection)
                {
                    ServiceSection ss = e.Item.DataItem as ServiceSection;
                    var serviceQuestions = service.Questions.Where(x => x.ServiceSectionID == ss.ID).OrderBy(x=>x.ItemOrder).ToList();
                    rptquestion.DataSource = serviceQuestions;
                    rptquestion.DataBind();

                    //lvStepSections.DataSource = serviceQuestions;
                    //lvStepSections.DataBind();
                    int lastitemorder = service.Sections.OrderByDescending(x => x.ItemOrder).FirstOrDefault().ItemOrder;
                    if (lastitemorder == ss.ItemOrder)
                    {
                        tdline.Visible = false;
                    }
                    section.Attributes.Add("class", "contract_steps_module section_" + ss.ID);
                }


            }
        }

        protected void lvStepSections_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem )
            {
                Literal lquestionNumber = e.Item.FindControl("lquestionNumber") as Literal;
                HtmlTableCell tdQuestion = e.Item.FindControl("tdQuestion") as HtmlTableCell;
                HtmlTableCell tdline = e.Item.FindControl("tdline") as HtmlTableCell;
                Label stageTitle = e.Item.FindControl("stageTitle") as Label;
                HtmlGenericControl stageTitleContainer = (HtmlGenericControl)e.Item.FindControl("stageTitleContainer");
                HtmlGenericControl activeIconDiv = (HtmlGenericControl)e.Item.FindControl("activeIconDiv");
                HtmlGenericControl lineWorkflow = (HtmlGenericControl)e.Item.FindControl("lineWorkflow");
                if (e.Item.DataItem is ServiceQuestion)
                {
                    /// for Question Number
                    ServiceQuestion sq = e.Item.DataItem as ServiceQuestion;
                    int lastItemOrder = service.Questions.Where(x => x.ServiceSectionID == sq.ServiceSectionID)
                                                         .OrderByDescending(x => x.ItemOrder).FirstOrDefault().ItemOrder;
                    stageTitle.Text = UGITUtility.TruncateWithEllipsis(sq.QuestionTitle, 35);
                    if (sq.ItemOrder == 1)
                    {
                        lineWorkflow.Attributes.Add("class", "line -background bg-col-blue active");
                        //activeIconDiv.Attributes.Add("class", string.Format("firstquestion quest_{0} {1}", sq.ID, sq.TokenName));

                    }
                    if (sq.ItemOrder > 1 && sq.ItemOrder != lastItemOrder)
                    {
                        //activeIconDiv.Attributes.Add("class", string.Format("midquestion quest_{0} {1}", sq.ID, sq.TokenName));
                        lineWorkflow.Attributes.Add("class", "line -background bg-col-blue active");
                    }
                    if (sq.ItemOrder == lastItemOrder)
                    {
                        //activeIconDiv.Attributes.Add("class", string.Format("lastquestion quest_{0} {1}", sq.ID, sq.TokenName));
                        lineWorkflow.Visible = false;
                    }
                    activeIconDiv.Attributes.Add("onclick", string.Format("OpeneditQuestion({0});", sq.ID));

                    //if (e.Item.ItemType == ListItemType.AlternatingItem)
                    //{
                    //    // stageTitleContainer.Attributes.Add("class", "stageTitleContainer alternategraphiclabel");
                    //    stageTitleContainer.Style.Add(HtmlTextWriterStyle.Top, "-28px");
                    //}
                    //else
                    //{
                        //stageTitleContainer.Attributes.Add("class", "stageTitleContainer");
                        //stageTitleContainer.Style.Add(HtmlTextWriterStyle.Top, "40px");
                    //}

                    lquestionNumber.Text = Convert.ToString(sq.ItemOrder);
                }
            }
        }
    }
}

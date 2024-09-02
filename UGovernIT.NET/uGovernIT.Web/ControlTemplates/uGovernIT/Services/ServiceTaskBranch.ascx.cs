using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using uGovernIT.Utility;
using System.Linq;
using System.Collections.Generic;
using uGovernIT.Manager;
using System.Web;

namespace uGovernIT.Web
{
    public partial class ServiceTaskBranch : UserControl
    {
        public int svcConfigID;
        public int sectionID;

        Services service;
        ServiceTaskCondition serviceTaskCondition;
        //SPPageStatusSetter status;
        ServicesManager serviceManager = new ServicesManager(HttpContext.Current.GetManagerContext());
        ServiceTaskManager serviceTaskManager = new ServiceTaskManager(HttpContext.Current.GetManagerContext());
        protected override void OnInit(EventArgs e)
        {
            pOperatorValue.Visible = false;
            pPickerValueContainer.Visible = false;
            int.TryParse(Request["svcConfigID"], out svcConfigID);
            if (svcConfigID > 0)
            {
                service = serviceManager.LoadByServiceID(svcConfigID);
                if (service == null)
                {
                    return;
                }

                trSkipTasks.Visible = true;

                //Fills conditions dropdown
                if (service.SkipTaskCondition != null)
                {
                    foreach (ServiceTaskCondition cdn in service.SkipTaskCondition)
                    {
                        ddlExistingConditions.Items.Add(new ListItem(cdn.Title, cdn.ID.ToString()));
                    }
                }
                ddlExistingConditions.Items.Insert(0, new ListItem("<New Condition>", ""));

                FillAllTasks();

                FillAllQuestions();

                Guid conditionID = Guid.Empty;
                if (!string.IsNullOrEmpty(Request[ddlExistingConditions.UniqueID]))
                {
                    conditionID = new Guid(Request[ddlExistingConditions.UniqueID]);
                }
                if (service.SkipTaskCondition != null)
                {
                    serviceTaskCondition = service.SkipTaskCondition.FirstOrDefault(x => x.ID == conditionID);
                }

                string selectedQuestion = string.Empty;
                if (!string.IsNullOrEmpty(Request[ddlCQuestions.UniqueID]))
                {
                    selectedQuestion = Request[ddlCQuestions.UniqueID];
                    ServiceQuestion sQuestion = service.Questions.FirstOrDefault(x => x.TokenName == selectedQuestion);
                    if (sQuestion != null)
                    {
                        addControlForCondiValue(sQuestion, string.Empty);
                    }
                }
            }
            btDeleteButton.Visible = false;
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void DDlExistingConditions_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlCQuestions.ClearSelection();
            ddlOperators.Items.Clear();
            pOperatorValue.Controls.Clear();
            pPickerValuePopup.Controls.Clear();
            txtTitle.Text = string.Empty;
            btDeleteButton.Visible = false;
            cbltaskList.ClearSelection();


            if (serviceTaskCondition != null)
            {
                txtTitle.Text = serviceTaskCondition.Title;

                if (serviceTaskCondition.Conditions.Count > 0)
                {
                    ddlCQuestions.SelectedIndex = ddlCQuestions.Items.IndexOf(ddlCQuestions.Items.FindByValue(serviceTaskCondition.Conditions[0].Variable));
                    ServiceQuestion selectedQuestion = service.Questions.FirstOrDefault(x => x.TokenName == serviceTaskCondition.Conditions[0].Variable);
                    if (selectedQuestion != null)
                    {
                        FillQuestionOperators(selectedQuestion.QuestionType);
                        ddlOperators.SelectedIndex = ddlOperators.Items.IndexOf(ddlOperators.Items.FindByValue(serviceTaskCondition.Conditions[0].Operator));

                        addControlForCondiValue(selectedQuestion, serviceTaskCondition.Conditions[0].Value);
                    }
                }


                foreach (int taskID in serviceTaskCondition.SkipTasks)
                {
                    ListItem item = cbltaskList.Items.FindByValue(taskID.ToString());
                    if (item != null)
                    {
                        item.Selected = true;
                    }
                }

                btDeleteButton.Visible = true;
            }
        }

        protected void BtSaveCondition_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            if (serviceTaskCondition == null)
            {
                serviceTaskCondition = new ServiceTaskCondition();
                service.SkipTaskCondition.Add(serviceTaskCondition);
            }

           

            serviceTaskCondition.Title = txtTitle.Text.Trim();

            //set condition
            serviceTaskCondition.Conditions = new List<WhereExpression>();
            WhereExpression whereClause = new WhereExpression();
            serviceTaskCondition.Conditions.Add(whereClause);

            whereClause.Variable = ddlCQuestions.SelectedValue;
            whereClause.Operator = ddlOperators.SelectedValue;
            ServiceQuestion sQuestion = service.Questions.FirstOrDefault(x => x.TokenName == ddlCQuestions.SelectedValue);
            string questionType = sQuestion.QuestionType.ToLower();
            if (pOperatorValue.Controls.Count > 1 || pPickerValuePopup.Controls.Count > 1)
            {
                Control ctr = null;
                if (questionType == Constants.ServiceQuestionType.LOOKUP ||
                   questionType == Constants.ServiceQuestionType.MULTICHOICE ||
                   questionType == Constants.ServiceQuestionType.REQUESTTYPE ||
                   questionType == Constants.ServiceQuestionType.SINGLECHOICE)
                {
                    ctr = pPickerValuePopup.Controls[1];
                }
                else
                {
                    ctr = pOperatorValue.Controls[1];
                }
                ServiceQuestionManager serviceQuestionManager = new ServiceQuestionManager(HttpContext.Current.GetManagerContext());
                whereClause.Value = serviceQuestionManager.ReadSelectedValueSkipLogic(sQuestion, ctr, whereClause.Operator);
            }

            serviceTaskCondition.SkipTasks = new List<long>();

            foreach (ListItem item in cbltaskList.Items)
            {
                if (item.Selected)
                {
                    serviceTaskCondition.SkipTasks.Add(Convert.ToInt32(item.Value));
                }
            }
             
            serviceManager.Save(service);
            reset();
        }

        protected void BtDelete_Click(object sender, EventArgs e)
        {
            if (serviceTaskCondition != null)
            {
                service.SkipTaskCondition.Remove(serviceTaskCondition);
                serviceManager.Save(service);
                reset();
            }
        }

        protected void DDLCQuestions_SelectedIndexChanged(object sender, EventArgs e)
        {
            pOperatorValue.Controls.Clear();
            pPickerValuePopup.Controls.Clear();
            string sQuestionToken = ddlCQuestions.SelectedValue;
            ddlOperators.Items.Clear();
            if (!string.IsNullOrEmpty(sQuestionToken))
            {
                ServiceQuestion sQuestion = service.Questions.FirstOrDefault(x => x.TokenName == sQuestionToken);
                FillQuestionOperators(sQuestion.QuestionType.ToLower());

                addControlForCondiValue(sQuestion, string.Empty);
            }
        }
        private void reset()
        {
            ddlExistingConditions.Items.Clear();
            //Fills conditions dropdown
            foreach (ServiceTaskCondition cdn in service.SkipTaskCondition)
            {
                ddlExistingConditions.Items.Add(new ListItem(cdn.Title, cdn.ID.ToString()));
            }
            ddlExistingConditions.Items.Insert(0, new ListItem("<New Condition>", ""));

            txtTitle.Text = string.Empty;
            cbltaskList.ClearSelection();
            btDeleteButton.Visible = false;
            ddlCQuestions.ClearSelection();
            ddlOperators.ClearSelection();
            pOperatorValue.Controls.Clear();
            btDeleteButton.Visible = false;

            lbPickedValue.Text = string.Empty;
            pPickerValuePopup.Controls.Clear();
            pPickerValueContainer.Visible = false;
            pOperatorValue.Visible = false;

        }

        private void FillSectionDropDown()
        {
            ServiceSection cSection = service.Sections.FirstOrDefault(x => x.ID == sectionID);
            int sIndex = service.Sections.IndexOf(cSection);
            List<ServiceSection> sections = new List<ServiceSection>();
            if (service.Sections.Count - 1 != sIndex)
            {
                sections = service.Sections.GetRange(sIndex + 1, (service.Sections.Count - 1 - sIndex));
            }

            foreach (ServiceSection section in sections)
            {
                ddlSections.Items.Add(new ListItem(section.Title, section.ID.ToString()));
            }
        }

        private void FillAllQuestions()
        {
            if (service != null && service.Questions != null)
            {
                List<ServiceQuestion> questions = service.Questions.ToList();
                if (questions.Count > 0)
                {
                    foreach (ServiceQuestion question in questions)
                    {
                        ddlCQuestions.Items.Add(new ListItem(string.Format("{0} [${1}$]", question.QuestionTitle, question.TokenName), string.Format("{0}", question.TokenName)));
                    }
                }
                ddlCQuestions.Items.Insert(0, new ListItem("--Select Question--", ""));
            }
        }

        private void FillAllTasks()
        {
            if (service != null && service.Questions != null)
            {
                foreach (UGITTask task in service.Tasks)
                {
                    if (task.ModuleNameLookup != null /*&& task.Module.LookupId > 0*/)
                    {
                        if (task.Behaviour == "Task")
                            cbltaskList.Items.Add(new ListItem(string.Format("{0}: {1}", task.Behaviour, task.Title), task.ID.ToString()));
                        else if (!string.IsNullOrEmpty(task.RelatedModule))
                            cbltaskList.Items.Add(new ListItem(string.Format("{0}: {1}", task.RelatedModule, task.Title), task.ID.ToString()));
                        else
                            cbltaskList.Items.Add(new ListItem(string.Format("Task: {0}", task.Title), task.ID.ToString()));
                    }
                    else
                    {
                        cbltaskList.Items.Add(new ListItem(string.Format("Task: {0}", task.Title), task.ID.ToString()));
                    }
                }
            }
        }

        private void FillQuestionOperators(string qType)
        {
            ServiceQuestionManager serviceQuestionManager = new ServiceQuestionManager(HttpContext.Current.GetManagerContext());
            List<string> operators = serviceQuestionManager.GetQTOperators(qType);
            foreach (string optr in operators)
            {
                ddlOperators.Items.Add(new ListItem(UGITUtility.AddSpaceBeforeWord(optr), optr));
            }
        }

        private void addControlForCondiValue(ServiceQuestion selectedQuestion, string conditionValue)
        {
            ServiceQuestionManager serviceQuestionManager = new ServiceQuestionManager(HttpContext.Current.GetManagerContext());
            Control ctr = serviceQuestionManager.GenerateControlForSkipLogic(selectedQuestion, conditionValue);
            string questionType = selectedQuestion.QuestionType.ToLower();
            if (questionType == Constants.ServiceQuestionType.LOOKUP ||
                questionType == Constants.ServiceQuestionType.MULTICHOICE ||
                questionType == Constants.ServiceQuestionType.REQUESTTYPE ||
                questionType == Constants.ServiceQuestionType.SINGLECHOICE)
            {

                pPickerValueContainer.Visible = true;
                pOperatorValue.Visible = false;
                pPickerValuePopup.Controls.Add(ctr);

                List<string> selectedValues = new List<string>();
                CheckBoxList chkList = (CheckBoxList)ctr;
                foreach (ListItem item in chkList.Items)
                {
                    if (item.Selected)
                    {
                        selectedValues.Add(item.Text);
                    }
                }
                lbPickedValue.Text = string.Join(", ", selectedValues.ToArray());
            }
            else
            {
                pPickerValueContainer.Visible = false;
                pOperatorValue.Visible = true;
                pOperatorValue.Controls.Add(ctr);
            }

        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }
    }
}

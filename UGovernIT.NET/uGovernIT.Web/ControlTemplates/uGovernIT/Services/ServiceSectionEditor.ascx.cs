using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using uGovernIT.Utility;
using uGovernIT.Manager;
using System.Web;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Web
{
    public partial class ServiceSectionEditor : UserControl
    {

        protected int svcConfigID;
        protected long sectionID;
        private ServiceSection serviceSection;
        private Services service;
        UserProfile User;
        protected override void OnInit(EventArgs e)
        {
            User = HttpContext.Current.CurrentUser();
            int.TryParse(Request["svcConfigID"], out svcConfigID);
            long.TryParse(Request["sectionID"], out sectionID);

            hfSectionID.Value = sectionID.ToString();
            hfServiceID.Value = svcConfigID.ToString();

            ServicesManager serviceManager = new ServicesManager(HttpContext.Current.GetManagerContext());
            if (svcConfigID > 0)
            {
                service = serviceManager.LoadByServiceID(svcConfigID, false, true, false);
                LoadQuestions(cblQuestions);
            }

            if (service != null && sectionID > 0)
            {
                serviceSection = service.Sections.FirstOrDefault(x => x.ID == sectionID);
                List<ServiceQuestion> questions = service.Questions.Where(x => x.ServiceSectionID == serviceSection.ID).ToList();
                if (serviceSection != null)
                {
                    btDelete.Visible = true;
                   // if (service.Sections.OrderBy(x => x.ItemOrder).First().ID == serviceSection.ID)
                    //{
                      //  //Users can not delete first section of service
                       // btDelete.Visible = false;
                    //}

                    txtTitle.Text = serviceSection.SectionName;
                    foreach (ListItem item in cblQuestions.Items)
                    {
                        if (item.Enabled && questions.Exists(x => x.ID.ToString() == item.Value))
                        {
                            item.Selected = true;
                        }
                    }
                    txtDesc.Text = serviceSection.Description;
                    UGITFileUploadManager1.SetImageUrl(serviceSection.IconUrl);
                }
            }
          

            base.OnInit(e);
        }
        protected void LoadQuestions(CheckBoxList cblList)
        {
            List<ServiceQuestion> questions = service.Questions.Where(x => x.ServiceSectionID == 0 || x.ServiceSectionID == sectionID).OrderBy(x=>x.ServiceSectionID).ToList();
            bool first = false;
            foreach (ServiceQuestion question in questions)
            {
                if (question.ServiceSectionID != 0 && !first)
                {
                    first = true;
                    ListItem item = new ListItem(question.ServiceSectionName, question.ServiceSectionID.ToString());
                    item.Enabled = false;
                    item.Attributes.CssStyle.Add(HtmlTextWriterStyle.Color, "red");
                    cblList.Items.Add(item);
                }
                ListItem qItem = new ListItem(question.QuestionTitle, question.ID.ToString());
                qItem.Attributes.CssStyle.Add(HtmlTextWriterStyle.PaddingLeft, "15px");
                cblList.Items.Add(qItem);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }
        protected void BtSaveSection_Click(object sender, EventArgs e)
        {
            if (service != null)
            {
                ServiceSectionManager serviceSectionManager = new ServiceSectionManager(HttpContext.Current.GetManagerContext());
                if (serviceSection == null)
                {
                    serviceSection = new ServiceSection();
                    serviceSection.ServiceID = svcConfigID;
                    serviceSection.ItemOrder = service.Sections.Count;
                    if (service.Sections.Count == 0)
                        serviceSection.ItemOrder = 1;
                }
                serviceSection.SectionName = txtTitle.Text.Trim();
                serviceSection.Description = txtDesc.Text.Trim();
                serviceSection.Title= txtTitle.Text.Trim();
                serviceSection.IconUrl = UGITFileUploadManager1.GetImageUrl();
                serviceSectionManager.Save(serviceSection);
                sectionID = serviceSection.ID;
                if (cblQuestions.Items.Count > 0)
                {
                    ServiceQuestionManager serviceQuestionManager = new ServiceQuestionManager(HttpContext.Current.GetManagerContext());
                    List<ServiceQuestion> selectedQuestions = new List<ServiceQuestion>();
                    foreach (ListItem qItem in cblQuestions.Items)
                    {
                        if (qItem.Enabled)
                        {
                            ServiceQuestion qust = service.Questions.FirstOrDefault(x => x.ID.ToString() == qItem.Value);
                            if (qust != null)
                            {
                                if (qItem.Selected)
                                {
                                    qust.ServiceSectionID = sectionID;
                                }
                                else
                                {
                                    qust.ServiceSectionID = 0;
                                }
                                selectedQuestions.Add(qust);
                            }
                        }
                    }

                    serviceQuestionManager.SaveSectionMapping(selectedQuestions);
                }

            }

            Context.Cache.Add(string.Format("SVCConfigQuestion-{0}", User.Id), true, null, DateTime.Now.AddMinutes(50), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Normal, null);
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }
        protected void btDelete_Click(object sender, EventArgs e)
        {
            if (serviceSection != null)
            {
                ServiceSectionManager serviceSectionManager = new ServiceSectionManager(HttpContext.Current.GetManagerContext());
                serviceSectionManager.Delete(serviceSection);

                Context.Cache.Add(string.Format("SVCConfigQuestion-{0}", User.Id), true, null, DateTime.Now.AddMinutes(50), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Normal, null);
                uHelper.ClosePopUpAndEndResponse(Context, true);
            }
        }
        protected void btCancelSection_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context,false);

        }
    }
}

using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using System.Collections.Generic;
using uGovernIT.Utility;
using System.Web;
using uGovernIT.Manager;
using uGovernIT.Manager.Core;
using ApplicationContext = uGovernIT.Manager.ApplicationContext;
using uGovernIT.DAL;
using System.Threading;

namespace uGovernIT.Web
{
    public partial class AddExperiencedTags : UserControl
    {
        public ApplicationContext ApplicationContext 
        { 
            get 
            {
                return HttpContext.Current.GetManagerContext();
            } 
        }
        ExperiencedTagManager ExperiencedTagMGR
        {
            get 
            { 
                return new ExperiencedTagManager(ApplicationContext);
            }
        }

        UserProjectExperienceManager UserProjectExperienceMGR
        {
            get
            {
                return new UserProjectExperienceManager(ApplicationContext);
            }
        }
        public int ExperiencedTagID { get; set; }
        List<ExperiencedTag> ExperiencedTagList
        {
            get
            {
                return ExperiencedTagMGR.Load();
            }
            
        }
        ExperiencedTag ExperiencedTagItem { get; set; }
        

        protected override void OnInit(EventArgs e)
        {
            BindCategory();
            if (ExperiencedTagID == 0)
            {
                SetCookie("button", "Add");
                ExperiencedTagItem = new ExperiencedTag();
                txtExperiencedTag.Text = "";
                lnkDelete.Style.Add("visibility", "hidden");
            }
            else
            {
                UGITUtility.DeleteCookie(Request, Response, "button");
                ExperiencedTagItem = ExperiencedTagMGR.LoadByID(ExperiencedTagID);
                if (ExperiencedTagItem != null)
                {
                    txtExperiencedTag.Text = ExperiencedTagItem.Title;
                    ddlCategory.SelectedValue = ExperiencedTagItem.Category.ToString();
                    lnkDelete.Style.Add("visibility", "visible");
                }
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //BindProjects();
                //BindProjectType();
            }
        }
        #region Commented Code
        //protected void BindProjects()
        //{
        //    //ddlProjects.Items.Clear();

        //    Dictionary<string, object> arrParamsAM = new Dictionary<string, object>();
        //    arrParamsAM.Add("@TenantID", applicationContext.TenantID);

        //    DataTable dtProjectTypes = uGITDAL.ExecuteDataSetWithParameters("usp_getProjects", arrParamsAM);

        //    if (dtProjectTypes != null && dtProjectTypes.Rows.Count > 0)
        //    {

        //        ddlProjects.DataSource = dtProjectTypes;
        //        ddlProjects.DataTextField = DatabaseObjects.Columns.Title;
        //        ddlProjects.DataValueField = DatabaseObjects.Columns.Title;
        //        ddlProjects.DataBind();
        //    }
        //}

        //protected void BindProjectType() 
        //{
        //    ddlProjectType.Items.Clear();
        //    ProjectType projectType = new ProjectType();

        //    Dictionary<string, object> arrParamsAM = new Dictionary<string, object>();
        //    arrParamsAM.Add("@TenantID", applicationContext.TenantID);

        //    DataTable dtProjectTypes = uGITDAL.ExecuteDataSetWithParameters("usp_getProjectTypes", arrParamsAM);

        //    //List<ProjectType> lstModule = 

        //    if (dtProjectTypes != null && dtProjectTypes.Rows.Count > 0)
        //    {

        //        ddlProjectType.DataSource = dtProjectTypes;
        //        ddlProjectType.DataTextField = DatabaseObjects.Columns.RequestType;
        //        ddlProjectType.DataValueField = DatabaseObjects.Columns.ID;
        //        ddlProjectType.DataBind();
        //    }
        //}
        #endregion

        protected void BindCategory()
        {
            // Bind Category Dropdown.
            List<string> experiencedTags = ExperiencedTagList?.Select(x => x.Category)?.Distinct()?.ToList() ?? null;
            if (experiencedTags != null && experiencedTags.Count > 0)
            {
                ddlCategory.DataSource = ExperiencedTagList.Select(x => x.Category).Distinct().ToList();
                ddlCategory.DataBind();
                ddlCategory.Items.Insert(0, "--Select--");
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;
            
            if (ValidateProjectTag())
            {
                ExperiencedTagItem.Title = txtExperiencedTag.Text;
                string category = string.Empty;
                if (hdnCategory.Value == "0")
                {
                    category = ddlCategory.SelectedItem.Text;
                }
                else
                {
                    category = txtCategory.Text;
                    if (!string.IsNullOrWhiteSpace(hdnRequestCategory.Value))
                    {
                        var tagLists = ExperiencedTagList.Where(o => o.Category == hdnRequestCategory.Value).ToList();
                        tagLists.ForEach(a => a.Category = category);
                        ExperiencedTagMGR.UpdateItems(tagLists);
                    }
                }
                ExperiencedTagItem.Category = category;
                ExperiencedTagItem.InsertedBy = ApplicationContext.CurrentUser.Id;
                ExperiencedTagMGR.Save(ExperiencedTagItem);

                //ExperiencedTagItem.RequestType = txtProjectType.Text;
                //ExperiencedTagItem.Title = ddlProjects.SelectedValue.ToString();
                //ExperiencedTagItem.RequestType = ddlProjectType.SelectedItem.Text.ToString();

                //string buttonValue = UGITUtility.GetCookieValue(Request, "button");
                //if (!string.IsNullOrEmpty(buttonValue) && buttonValue == "Add")
                //{
                //    Dictionary<string, object> values = new Dictionary<string, object>();
                //    //values.Add("@ExperiencedTag", txtExperiencedTag.Text);
                //    values.Add("@TagId", "");
                //    values.Add("@Type", "");
                //    values.Add("@TenantID", applicationContext.TenantID);

                //    ThreadStart tsRTUpdate = delegate ()
                //    {
                //        DataTable dt = uGITDAL.ExecuteDataSetWithParameters("usp_experiencedTagMapping", values);
                //    };
                //    Thread thRTUpdate = new Thread(tsRTUpdate);
                //    thRTUpdate.IsBackground = true;
                //    thRTUpdate.Start();
                //}

                Util.Log.ULog.WriteUGITLog(HttpContext.Current.GetManagerContext().CurrentUser.Id, $"Added/Updated project tag: {ExperiencedTagItem.Title}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), HttpContext.Current.GetManagerContext().TenantID);
                uHelper.ClosePopUpAndEndResponse(Context, true);
            }
        }

        private Boolean ValidateProjectTag()
        {
           List<ExperiencedTag> collection = ExperiencedTagMGR.Load(x=>x.ID!=ExperiencedTagID && x.Title == txtExperiencedTag.Text);
            if (collection.Count > 0)
            {
                lblErrorMessage.Text = "Project Tag is already in the list";
                return false;
            }
            else
            { return true; }
        }

        protected void lnkDelete_Click(object sender, EventArgs e)
        {
            ExperiencedTagItem = ExperiencedTagMGR.LoadByID(ExperiencedTagID);
            Util.Log.ULog.WriteUGITLog(HttpContext.Current.GetManagerContext().CurrentUser.Id, $"Deleted project tag: {ExperiencedTagItem.Title}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), HttpContext.Current.GetManagerContext().TenantID);
            ExperiencedTagMGR.Delete(ExperiencedTagItem);
            UserProjectExperienceMGR.Delete(UserProjectExperienceMGR.Load(x => x.TagLookup == ExperiencedTagItem.ID));

            //Dictionary<string, object> valuesDelete = new Dictionary<string, object>();
            //valuesDelete.Add("@TagId", Convert.ToString(ExperiencedTagID));
            //valuesDelete.Add("@Type", "Delete");
            //valuesDelete.Add("@TenantID", ApplicationContext.TenantID);

            //ThreadStart tsRTUpdate = delegate ()
            //{
            //    DataTable dt = uGITDAL.ExecuteDataSetWithParameters("usp_experiencedTagMapping", valuesDelete);
            //};
            //Thread thRTUpdate = new Thread(tsRTUpdate);
            //thRTUpdate.IsBackground = true;
            //thRTUpdate.Start();
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void ddlProjectType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlProjects_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void SetCookie(string Name, string Value)
        {
            UGITUtility.CreateCookie(Response, Name, Value);
        }
    }
}

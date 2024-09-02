using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager.Managers;
using uGovernIT.Manager;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;
//using System.Web;
using DevExpress.Data.Async.Helpers;
using static uGovernIT.Web.ModuleResourceAddEdit;
using System.Configuration;
using uGovernIT.Web.Controllers;
using DevExpress.ExpressApp;
using DevExpress.XtraRichEdit.Model;
using System.Threading;
using uGovernIT.Helpers;
using System.Windows.Forms;
using uGovernIT.DAL;
using System.Web.Services;
using DevExpress.XtraCharts.Native;
using ApplicationContext = uGovernIT.Manager.ApplicationContext;
using DevExpress.ExpressApp.Utils;
using System.Text;
using DevExpress.Utils.Drawing.Helpers;
using System.Net.Mail;
using System.Net;
using Microsoft.Graph;
using uGovernIT.Util.Log;

namespace uGovernIT.Web.ControlTemplates.Admin.ListForm
{
    public partial class UserProjectExperiences : System.Web.UI.UserControl
    {
        UserProjectExperienceManager userProjectExperienceManager = new UserProjectExperienceManager(HttpContext.Current.GetManagerContext());
        ExperiencedTagManager experiencedTagManager = new ExperiencedTagManager(HttpContext.Current.GetManagerContext());
        protected override void OnInit(EventArgs e)
        {
            try
            {
                List<UserProjectExperience> lstUserProjectExperiences = userProjectExperienceManager.Load().OrderBy(x => x.ID).ToList();
                List<ExperiencedTag> lstExperiencedTags = experiencedTagManager.Load().OrderBy(x => x.Title).ToList();

                var lstuserproexp = from x in lstUserProjectExperiences
                                    join y in lstExperiencedTags
                                    on x.TagLookup equals y.ID
                                    select new UserProjectExperience
                                    {
                                        ID = x.ID,
                                        ProjectID = x.ProjectID,
                                        UserId = x.UserId,
                                        ResourceUser = x.ResourceUser,
                                        TagLookup = x.TagLookup,
                                        Title = y.Title,
                                        TenantID = x.TenantID,
                                        Created = x.Created
                                    };


                if (lstuserproexp != null)
                {
                    aspxGridUserProjectExperiences.DataSource = lstuserproexp;
                    aspxGridUserProjectExperiences.DataBind();
                }

                base.OnInit(e);
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in UserProjectExperiences OnInit : " + ex);
                //throw;
            }
            
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}
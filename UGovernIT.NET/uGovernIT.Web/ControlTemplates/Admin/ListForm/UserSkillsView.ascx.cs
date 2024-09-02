using System;
using System.Web.UI;
using uGovernIT.Manager;
using uGovernIT.Utility;
using DevExpress.Web;
using System.Web;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Linq;

namespace uGovernIT.Web
{
    public partial class UserSkillsView : UserControl
    {
        //string addNewItem;
        UserSkillManager userSkillManager = new UserSkillManager(HttpContext.Current.GetManagerContext());
        protected string editUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=adduserskills");
        protected override void OnInit(EventArgs e)
        {
            List<UserSkills> lstUserSkills = userSkillManager.Load().OrderBy(x => x.Title).ToList();
            //List<UserSkills> lstUserSkills = userSkillManager.Load().GroupBy(g => g.CategoryName).OrderBy(g => g.Key).SelectMany(g => g.OrderBy(x => x.Title)).ToList();

            if (lstUserSkills != null && lstUserSkills.Count > 0)
            {
                //lstUserSkills.Sort();
                //lstUserSkills.Sort();
                aspxGridUserSkills.DataSource = lstUserSkills;
                aspxGridUserSkills.DataBind();
            }
          //  addNewItem = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlEdit, newParam, "0"));
            LinkButton1.Attributes.Add("href", string.Format("javascript:NewUserSkillDialog()"));
            lnkAddNewUserSkill.Attributes.Add("href", string.Format("javascript:NewUserSkillDialog()"));
            // LinkButton1.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{2} - New Item','600','600',0,'{1}','true')", addNewItem, Server.UrlEncode(Request.Url.AbsolutePath), formTitle));
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void aspxGridUserSkills_HtmlRowPrepared(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != GridViewRowType.Data || e.KeyValue == null)
                return;
            string func = string.Empty;
            func = string.Format("openuserSkillDialog('{0}','{1}','{2}','{3}','{4}', 0)", editUrl, string.Format("SkillId={0}", e.KeyValue), "Edit User Skill", "450px", "330px");
            e.Row.Attributes.Add("onClick", func);
        }

        
    }
}

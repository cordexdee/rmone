using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using System.Collections.Generic;
using uGovernIT.Utility;
using System.Web;
using uGovernIT.Manager;

namespace uGovernIT.Web
{
    public partial class AddUserSkills : UserControl
    {
        public int skillID { get; set; }
        List<UserSkills> spUserSkillList;
        UserSkills spitem;
        UserSkillManager userSkillManager = new UserSkillManager(HttpContext.Current.GetManagerContext());

        protected override void OnInit(EventArgs e)
        {
            spUserSkillList = userSkillManager.Load();
            BindUserSkillCategory();
            if (skillID == 0)
            {
                spitem =new UserSkills();
                txtUserSkill.Text = "";
                txtDescription.Text = "";
                lnkDelete.Visible = false;
            }
            else
            {
                spitem = userSkillManager.LoadByID(skillID);
                if (spitem != null)
                {
                    txtUserSkill.Text = spitem.Title;
                    txtDescription.Text = spitem.Description;
                    ddlCategory.SelectedIndex = ddlCategory.Items.IndexOf(ddlCategory.Items.FindByText(Convert.ToString(spitem.CategoryName)));
                    lnkDelete.Visible = true;
                }
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;
            
            if (ValidateSkill())
            {
                spitem.Title= txtUserSkill.Text;
                spitem.Description= txtDescription.Text;
                if (ddlCategory.SelectedIndex > 0)
                {
                    spitem.CategoryName= ddlCategory.SelectedItem.Text;
                }
                else
                {
                    spitem.CategoryName = txtCategory.Text;
                }
                userSkillManager.Save(spitem);

                Util.Log.ULog.WriteUGITLog(HttpContext.Current.GetManagerContext().CurrentUser.Id, $"Added/Updated user skill: {spitem.Title}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), HttpContext.Current.GetManagerContext().TenantID);
                uHelper.ClosePopUpAndEndResponse(Context, true);
            }
        }

        private Boolean ValidateSkill()
        {
           List<UserSkills> collection = userSkillManager.Load(x=>x.ID!=skillID && x.Title== txtUserSkill.Text);
            if (collection.Count > 0)
            {
                lblErrorMessage.Text = "Skill is already in the list";
                return false;
            }
            else
            { return true; }
        }

        protected void lnkDelete_Click(object sender, EventArgs e)
        {
            spitem = userSkillManager.LoadByID(skillID);
            Util.Log.ULog.WriteUGITLog(HttpContext.Current.GetManagerContext().CurrentUser.Id, $"Deleted user skill: {spitem.Title}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), HttpContext.Current.GetManagerContext().TenantID);
            userSkillManager.Delete(spitem);
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void csvdivCategory_ServerValidate(object source, ServerValidateEventArgs args)
        {
            bool argsval = false;
            if (ddlCategory.SelectedIndex < 1)
            {
                if (string.IsNullOrEmpty(txtCategory.Text) && hdnCategory.Value == "1")
                {
                    divddlCategory.Attributes.Add("style", "display:none");
                    divCategory.Attributes.Add("style", "display:block");
                }
                if (!string.IsNullOrEmpty(txtCategory.Text))
                {
                    argsval = true;
                }
                args.IsValid = argsval;
            }
            else
            {
                divddlCategory.Attributes.Add("style", "display:block");
                divCategory.Attributes.Add("style", "display:none");
            }
        }

        private void BindUserSkillCategory()
        {
            if (spUserSkillList.Count > 0 && spUserSkillList != null)
            {
                IEnumerable<object> itemsDistinct = spUserSkillList
                                .Select(item => item.CategoryName)
                                .Distinct();
                using (IEnumerator<object> enumerator = itemsDistinct.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        if( enumerator.Current != null)
                        ddlCategory.Items.Add(new ListItem(Convert.ToString(enumerator.Current)));
                    }
                }

                ddlCategory.Items.Insert(0, new ListItem("None", "0"));
            }

        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }
    }
}

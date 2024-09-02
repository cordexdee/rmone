using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using uGovernIT.Manager;
using uGovernIT.Utility;
using System.Web;
namespace uGovernIT.Web
{
    public partial class DivisionEdit : UserControl
    {
       // SPList divisionList;
        //SPListItem item;
        public int DivisionID { get; set; }
        public string companyLabel;
        CompanyManager objCompanyManager;
        CompanyDivisionManager objCompanyDivisionManager;
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        CompanyDivision divisionList;
        protected override void OnInit(EventArgs e)
        {
            objCompanyManager = new CompanyManager(context);
            objCompanyDivisionManager = new CompanyDivisionManager(context);
            companyLabel = uHelper.GetDepartmentLabelName(DepartmentLevel.Company);

            divisionList = objCompanyDivisionManager.LoadByID(DivisionID);
            BindCompany();
            if (Request["__EVENTTARGET"] != null)
            {
                if (Convert.ToString(Request["__EVENTTARGET"]).Contains("DeleteChildItem"))
                {
                    string deleteChildAction = Request["__EVENTARGUMENT"];
                    DeleteChildItem(deleteChildAction); //deletes child departments
                }
            }

            //if (DivisionID <= 0)
            //    SPUtility.TransferToErrorPage(Constants.MalformedURLMsg);

            //item = SPListHelper.GetSPListItem(divisionList, DivisionID);
            //if (item == null)
            //    SPUtility.TransferToErrorPage(Constants.MalformedURLMsg);

            Fill();
            if (divisionList.Deleted)
            {
                btUnDelete.Visible = true;
                btDelete.Visible = false;
            }
            else
            {
                btUnDelete.Visible = false;
                btDelete.Visible = true;
            }

            base.OnInit(e);
        }

        private void DeleteChildItem(string deleteChildAction)
        {
            //deletes the Divisions and Departments mapped to the company as per user input.
            if (deleteChildAction == "1")
            {
                DepartmentManager objDepartmentManager = new DepartmentManager(context);
                List<Department> departments;

                departments = objDepartmentManager.GetDepartmentData().Where(x => x.DivisionIdLookup == DivisionID).ToList();
                foreach (Department dept in departments)
                {
                    dept.Deleted = true;
                    Util.Log.ULog.WriteUGITLog(context.CurrentUser.Id, $"Updated Department: {dept.Title}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), context.TenantID);
                    objDepartmentManager.Update(dept);
                }
            }
            //delete the company 
            divisionList.Deleted = !divisionList.Deleted;
            Util.Log.ULog.WriteUGITLog(context.CurrentUser.Id, $"Updated Division: {divisionList.Title}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), context.TenantID);
            objCompanyDivisionManager.Update(divisionList);
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private void BindCompany()
        {
            List<Company> cmpny = objCompanyManager.Load();
            foreach (Company row in cmpny)
            {
                ddlCompany.Items.Add(new ListItem(row.Title, Convert.ToString(row.ID)));
            }
            ddlCompany.Items.Insert(0, new ListItem("None", "0"));
        }
        private void Fill()
        {
            if (divisionList != null)
            {
                txtTitle.Text = divisionList.Title;
                txtshortName.Text = divisionList.ShortName;
                txtDescription.Text = divisionList.Description;
                txtGLCode.Text = divisionList.GLCode;
                //chkDeleted.Checked = UGITUtility.StringToBoolean(divisionList.Deleted);
                ddlCompany.SelectedIndex = ddlCompany.Items.IndexOf(ddlCompany.Items.FindByValue(Convert.ToString(divisionList.CompanyIdLookup)));
                ppeManager.SetValues(divisionList.Manager);
                // SPFieldUserValueCollection manager = new SPFieldUserValueCollection(SPContext.Current.Web, Convert.ToString(item[DatabaseObjects.Columns.ManagerLookup]));
                // ppeManager.UpdateEntities(uHelper.getUsersListFromCollection(manager));
            }


        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            if (divisionList != null)
            {
                divisionList.Title = txtTitle.Text.Trim();
                divisionList.ShortName=txtshortName.Text.Trim();
                divisionList.Description = txtDescription.Text.Trim();
                divisionList.GLCode = txtGLCode.Text.Trim();
                divisionList.CompanyIdLookup = Convert.ToInt64(ddlCompany.SelectedValue);
                //divisionList.Deleted = chkDeleted.Checked ? true : false;
                divisionList.Manager = ppeManager.GetValues();

                objCompanyDivisionManager.Update(divisionList);
            }

            uHelper.ClosePopUpAndEndResponse(Context, true);
        }


        protected void cvtxtTitle_ServerValidate(object source, ServerValidateEventArgs args)
        {
            List<CompanyDivision> data = objCompanyDivisionManager.Load(x=> x.ID!= DivisionID && x.Title.ToLower()== txtTitle.Text.Trim().ToLower() && x.CompanyIdLookup== Convert.ToInt64(ddlCompany.SelectedValue));
            if ( data.Count > 0)
            {
                
                    args.IsValid = false;
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }
        protected void btUnDelete_Click(object sender, EventArgs e)
        {
            divisionList.Deleted = false;
            Util.Log.ULog.WriteUGITLog(context.CurrentUser.Id, $"Updated Division: {divisionList.Title}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), context.TenantID);
            objCompanyDivisionManager.Update(divisionList);

            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities.DB;

namespace uGovernIT.Web.ControlTemplates.Admin.ListForm
{
    public partial class WidgetAddEdit : System.Web.UI.UserControl
    {
        ApplicationContext applicationContext = HttpContext.Current.GetManagerContext();
        private AgentsManager _agentManager = null;
        private ServicesManager _ServicesManager = null;
        protected Agents agents= null;
        public new long ID;
        public bool isMasterTenant = false;
        protected AgentsManager agentManager
        {
            get
            {
                if (_agentManager == null)
                {
                    _agentManager = new AgentsManager(applicationContext);
                }
                return _agentManager;
            }
        }

        protected ServicesManager ServicesManager
        {
            get
            {
                if (_ServicesManager == null)
                {
                    _ServicesManager = new ServicesManager(applicationContext);
                }
                return _ServicesManager;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            BindSevices();
            ApplicationContext MasterTenantContext = ApplicationContext.Create();
            if (MasterTenantContext.TenantID.Equals(applicationContext.TenantID))
            {
                isMasterTenant = true;
            }
            if (!isMasterTenant)
            {
                btnDelete.Enabled = false;
                btnSave.Enabled = false;
            }
            if (ID != 0)
            {
                btnDelete.Visible = true;
                agents =  agentManager.LoadByID(ID);
                if(agents!= null)
                {
                    Fill();
                }

            }
        }

        public void Fill()
        {            
            txtTitle.Text = agents.Title;
            txtDescription.Text = agents.Description;
            txtControl.Text = agents.Control;
            txtBaseUrl.Text = agents.Url;
            txtParameter.Text = agents.Parameters;
            txtHeight.Text = agents.Height;
            txtWidth.Text = agents.Width;
            iconUploader.SetImageUrl(agents.Icon);
            DDLservices.SelectedValue = Convert.ToString(agents.ServiceLookUp);

        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!isMasterTenant)
                return;

            if (agents == null)
            {
                agents = new Agents();
            }
            agents.Title = txtTitle.Text;
            agents.Description = txtDescription.Text;
            agents.Control = txtControl.Text;
            agents.Url = txtBaseUrl.Text;
            agents.Parameters = txtParameter.Text;
            agents.Height = txtHeight.Text;
            agents.Width = txtWidth.Text;
            if (!string.IsNullOrEmpty(DDLservices.SelectedValue))
                agents.ServiceLookUp = Convert.ToInt64(DDLservices.SelectedValue);
            else
                agents.ServiceLookUp = null;
            agents.Icon = iconUploader.GetImageUrl();
             
            if(ID == 0 )
            {
                agentManager.Insert(agents);
                Util.Log.ULog.WriteUGITLog(applicationContext.CurrentUser.Id, $"Added Phrase: {agents.Title}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), applicationContext.TenantID);

            }
            else if(ID != 0 && agents.Id != 0)
            {
                agentManager.Update(agents);
            }
            uHelper.ClosePopUpAndEndResponse(Context, true);

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }

        private void BindSevices()
        {
            List<Services> services = ServicesManager.LoadAllServices().Where(x => x.IsActivated == true && x.ServiceType.EqualsIgnoreCase("Service") && x.Deleted == false).OrderBy(x => x.Title).ToList();
            List<string> Categories = services.Select(x => x.ServiceCategoryType).Distinct().OrderBy(x => x).ToList();

            DDLservices.Items.Clear();
            if (services != null && services.Count > 0)
            {
                foreach (var item in Categories)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        ListItem category = new ListItem();
                        category.Text = item;
                        category.Value = "0";
                        category.Attributes.Add("Style", "color:#848484");
                        category.Attributes.Add("Disabled", "true");
                        DDLservices.Items.Add(category);
                    }

                    foreach (var service in services.Where(x => x.ServiceCategoryType.EqualsIgnoreCase(item)))
                    {
                        ListItem srv = new ListItem();
                        srv.Text = service.Title;
                        srv.Value = Convert.ToString(service.ID);
                        DDLservices.Items.Add(srv);
                    }
                }

            }
            DDLservices.Items.Insert(0, new ListItem("None", ""));
        }

        protected void DDLservices_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (!isMasterTenant)
                return;

            if (ID != 0)
            {
                Agents agents = agentManager.Load(x => x.Id == ID).FirstOrDefault();
                if (agents != null)
                {
                    Util.Log.ULog.WriteUGITLog(HttpContext.Current.GetManagerContext().CurrentUser.Id, $"Deleted Phrase: {agents.Title}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), HttpContext.Current.GetManagerContext().TenantID);
                    agentManager.Delete(agents);
                }
                uHelper.ClosePopUpAndEndResponse(Context, true);
            }
        }
    }
}
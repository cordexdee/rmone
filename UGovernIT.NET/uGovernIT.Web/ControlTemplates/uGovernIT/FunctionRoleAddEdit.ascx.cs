using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Utility;

namespace uGovernIT.Web
{
    public partial class FunctionRoleAddEdit : System.Web.UI.UserControl
    {
        public string Mode { get; set; }
        public long FunctionId { get; set; }
        FunctionRoleManager _functionRoleManager = new FunctionRoleManager(HttpContext.Current.GetManagerContext());    
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Mode == "Edit")
                {
                    if (FunctionId > 0)
                    {
                        FunctionRole functionRole = _functionRoleManager.LoadByID(FunctionId);
                        if (functionRole == null)
                            return;

                        txtTitle.Text = functionRole.Title;
                        memoDescription.Text = functionRole.Description;
                    }
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Mode == "Add")
            {
                FunctionRole functionRoleObj = new FunctionRole();
                functionRoleObj.Title = txtTitle.Text;
                functionRoleObj.Description = memoDescription.Text;
                _functionRoleManager.Insert(functionRoleObj);
            }
            if(Mode == "Edit")
            {
                FunctionRole functionRoleObj = _functionRoleManager.LoadByID(FunctionId);
                functionRoleObj.Title = txtTitle.Text;
                functionRoleObj.Description = memoDescription.Text;
                _functionRoleManager.Update(functionRoleObj);
            }
            uHelper.ClosePopUpAndEndResponse(HttpContext.Current, true);
        }
    }
}
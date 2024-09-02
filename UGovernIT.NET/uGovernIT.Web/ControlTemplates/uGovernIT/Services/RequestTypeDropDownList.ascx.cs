using DevExpress.Web;
using DevExpress.Web.ASPxTreeList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using uGovernIT.Manager;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Web
{
    public partial class RequestTypeDropDownList : UserControl
    {
        public string ModuleName { get; set; }
        public string SelectedRequestTypes { get; set; }
        public string HeaderCaption { get; set; }
        bool flag = true;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                requestTypeTreeList.DataBind();
        }
        protected override void OnInit(EventArgs e)
        {
           LoadRequestTypeTree(ModuleName);
        }
        public void LoadRequestTypeTree(string moduleName, bool selectValues = false)
        {
            flag = selectValues;
            DataTable data = GetRequestTypeData(moduleName);
            requestTypeTreeList.DataSource = data;
            if (requestTypeTreeList.Columns["RequestType"] != null)
                requestTypeTreeList.Columns["RequestType"].Caption = HeaderCaption;
        }
        private DataTable GetRequestTypeData(string moduleName)
        {            
            RequestTypeManager requesttypeManager = new RequestTypeManager(HttpContext.Current.GetManagerContext());
            DataTable requestTypeData = null;
            DataTable dt = requesttypeManager.GetDataTable();  // uGITCache.GetDataTable(DatabaseObjects.Lists.RequestType);
            if (dt != null)
            {
                DataRow[] dr = dt.Select(string.Format("{0}='{1}' and ({2}={3} or {2} IS NULL)", DatabaseObjects.Columns.ModuleNameLookup, moduleName,
                       DatabaseObjects.Columns.Deleted, false));

                DataTable dtmenu = new DataTable();
                if (dr != null && dr.Length > 0)
                {
                    DataTable tp = dr.CopyToDataTable();
                    DataTable dttemp = dr.CopyToDataTable();

                    if (!dttemp.Columns.Contains("SortRequestTypeCol"))
                        dttemp.Columns.Add("SortRequestTypeCol", typeof(int));
                    dttemp.Columns["SortRequestTypeCol"].Expression = string.Format("IIF([{0}] = '1', '1', '0')", DatabaseObjects.Columns.SortToBottom);


                    //dttemp.DefaultView.Sort = DatabaseObjects.Columns.Category + " ASC, " + DatabaseObjects.Columns.TicketRequestType + " ASC";
                    dttemp.DefaultView.Sort = DatabaseObjects.Columns.Category + " ASC, " + "SortRequestTypeCol" + " ASC," + DatabaseObjects.Columns.TicketRequestType + " ASC";
                    if (!dttemp.Columns.Contains("ParentID"))                    
                        dttemp.Columns.Add("ParentID", typeof(string));

                    var groupData = dttemp.AsEnumerable().GroupBy(x => x.Field<string>(DatabaseObjects.Columns.Category));
                    int counter = 1;
                    DataRow cRow = null;
                    foreach (var category in groupData)
                    {
                        cRow = dttemp.NewRow();
                        cRow[DatabaseObjects.Columns.Category] = category.Key;
                        cRow[DatabaseObjects.Columns.TicketRequestType] = "Category: " + category.Key;
                        cRow[DatabaseObjects.Columns.Id] = -counter;
                        cRow["ParentID"] = 0;

                        dttemp.Rows.Add(cRow);
                        int cID = -counter;
                        counter += 1;
                        var subCategories = category.GroupBy(x => x.Field<string>(DatabaseObjects.Columns.SubCategory));
                        foreach (var subCategory in subCategories)
                        {
                            int scID = 0;
                            cRow = dttemp.NewRow();
                            ConfigurationVariableManager objConfigurationVariableHelper = new ConfigurationVariableManager(HttpContext.Current.GetManagerContext());
                            if (objConfigurationVariableHelper.GetValueAsBool(ConfigConstants.EnableRequestTypeSubCategory))
                            {
                                cRow[DatabaseObjects.Columns.Category] = category.Key;
                                if (!string.IsNullOrEmpty(subCategory.Key))
                                {
                                    cRow[DatabaseObjects.Columns.SubCategory] = subCategory.Key;
                                    cRow[DatabaseObjects.Columns.TicketRequestType] = "Sub Category: " + subCategory.Key;
                                    cRow[DatabaseObjects.Columns.Id] = -counter;
                                    cRow["ParentID"] = cID;
                                    dttemp.Rows.Add(cRow);
                                    scID = -counter;
                                    counter += 1;
                                }
                                else { scID = cID; }
                            }
                            else { scID = cID; }

                            foreach (var requestTypeR in subCategory.ToArray())
                            {
                                requestTypeR["ParentID"] = scID;
                            }
                        }
                    }
                    requestTypeData = dttemp;
                }
            }
            return requestTypeData;
        }
        public List<string> GetSelectedValue()
        {
            List<string> requestTypes = new List<string>();
            List<TreeListNode> sNodes = requestTypeTreeList.GetSelectedNodes();
            if (sNodes.Count != requestTypeTreeList.GetAllNodes().Count)
            {
                foreach (TreeListNode node in sNodes)
                {
                    if (UGITUtility.StringToInt(node.Key) > 0)
                        requestTypes.Add(node.Key);
                }
            }
            else
            {
                requestTypes.Add("all");
            }
            return requestTypes;
        }
        //public void SetSelectedValue(string value)
        //{
        //    List<string> requesttypes = uHelper.ConvertStringToList(value, Constants.Separator6);
        //    if (requesttypes.Count == 1 && requesttypes[0].ToLower() == "all")
        //    {
        //        requestTypeTreeList.SelectAll();
        //    }
        //    else
        //    {
        //        foreach (string rt in requesttypes)
        //        {
        //            TreeListNode node = requestTypeTreeList.FindNodeByKeyValue(rt);
        //            if (node != null)
        //                node.Selected = true;
        //        }
        //    }
        //}
        protected void requestTypeTreeList_DataBound(object sender, EventArgs e)
        {            
            if (flag)
            {
                List<string> requesttypes = UGITUtility.ConvertStringToList(SelectedRequestTypes, Constants.Separator6);
                if (requesttypes.Count == 1 && requesttypes[0].ToLower() == "all")
                {
                    requestTypeTreeList.SelectAll();
                }
                else
                {
                    foreach (string rt in requesttypes)
                    {
                        TreeListNode node = requestTypeTreeList.FindNodeByKeyValue(rt);
                        if (node != null)
                            node.Selected = true;
                    }
                }
            }

        }
    }
}

using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls.WebParts;
using uGovernIT.Web;
using uGovernIT.Manager;
using uGovernIT.Utility;
using DevExpress.Web;
using System.Collections.Generic;
namespace uGovernIT.Web
{
    public partial class EventCategoryEdit : UserControl
    {
        public int Id { get; set; }
        //private SPListItem _SPListItem;
        public DataTable dt;
        protected override void OnInit(EventArgs e)
        {
            if (Id != 0)
            {
                dt = GetTableDataManager.GetTableData(DatabaseObjects.Tables.EventCategories, " id=" + Id);
                //_SPListItem = SPListHelper.GetSPListItem(DatabaseObjects.Lists.EventCategories, Id);
                btnDelete.Visible = true;
                Fill();
            }
            else
            {
               dt = GetTableDataManager.GetTableData(DatabaseObjects.Tables.EventCategories);
                //_SPListItem = SPListHelper.GetSPList(DatabaseObjects.Lists.EventCategories).AddItem();
                btnDelete.Visible = false;
            }
            base.OnInit(e);
        }
        

        private void Fill()
        {
            txtTitle.Text = Convert.ToString(dt.Rows[0][DatabaseObjects.Columns.Title]);
            try
            {
                ASPxColorEdit1.Color = System.Drawing.ColorTranslator.FromHtml(Convert.ToString(dt.Rows[0][DatabaseObjects.Columns.UGITItemColor]));
            }
            catch (Exception)
            {
            }
            txtItemOrder.Text = Convert.ToString(dt.Rows[0][DatabaseObjects.Columns.ItemOrder]);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            
            if (cvtxtTitle.IsValid == false)
            {
                return;
            }
            string textcolor = System.Drawing.ColorTranslator.ToHtml(ASPxColorEdit1.Color);
            Dictionary<String, object> values = new Dictionary<string, object>();
            values.Add(DatabaseObjects.Columns.Title, txtTitle.Text.Trim());
            values.Add(DatabaseObjects.Columns.UGITItemColor, textcolor);
           
            if (txtItemOrder.Text == "")
            {
                values.Add(DatabaseObjects.Columns.ItemOrder, dt.Columns.Count + 1);

            }
            else
            {
                values.Add(DatabaseObjects.Columns.ItemOrder, Convert.ToInt32(txtItemOrder.Text));
                //DatabaseObjects.Columns.ItemOrder = Convert.ToInt32(txtItemOrder.Text).ToString();
 
            }
            int n = (int)GetTableDataManager.UpdateItem<int>(DatabaseObjects.Tables.EventCategories, Id, values);
            //_SPListItem[DatabaseObjects.Columns.Title] = txtTitle.Text.Trim();
            //string textcolor = System.Drawing.ColorTranslator.ToHtml(ASPxColorEdit1.Color);
            //_SPListItem[DatabaseObjects.Columns.UGITItemColor] = System.Drawing.ColorTranslator.ToHtml(ASPxColorEdit1.Color);
            //if (txtItemOrder.Text == "")
            //{
            //    _SPListItem[DatabaseObjects.Columns.ItemOrder] = _SPListItem.ListItems.Count + 1;
            //}
            //else
            //{
            //    _SPListItem[DatabaseObjects.Columns.ItemOrder] = Convert.ToInt32(txtItemOrder.Text);
            //}
            //_SPListItem.Update();
            //uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            //uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            //_SPListItem.Delete();
           // uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void cvtxtTitle_ServerValidate(object source, ServerValidateEventArgs args)
        {
            DataTable dtlabel = GetTableDataManager.GetTableData(DatabaseObjects.Tables.EventCategories);    //SPListHelper.GetDataTable(DatabaseObjects.Lists.EventCategories);
            if (dtlabel != null)
            {
                string expression;
                if (Id == 0)
                { 
                expression = string.Format("{0}='{1}'", DatabaseObjects.Columns.Title, txtTitle.Text);
                }
                else
                {
                    expression = string.Format("{0}='{1}' And {2} <>{3}", DatabaseObjects.Columns.Title, txtTitle.Text,DatabaseObjects.Columns.Id,Id);
                }
                DataRow[] foundRows = dtlabel.Select(expression);
                if (foundRows == null || foundRows.Length == 0)
                {
                    args.IsValid = true;
                }
                else
                {
                    args.IsValid = false;
                }
            }
        }

    }
}

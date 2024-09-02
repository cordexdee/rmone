using DevExpress.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Utility;

namespace uGovernIT.Web
{
    public partial class SubLocationView : UserControl
    {
        //private List<Location> _SPList;
        //private List<SubLocation> SPList;
        ApplicationContext _Context = HttpContext.Current.GetManagerContext();
        LocationManager ObjLocationManager = null;
        SubLocationManager ObjSubLocationManager = null;
   
        protected void Page_Init(object sender, EventArgs e)
        {
            ObjLocationManager = new LocationManager(_Context);
            ObjSubLocationManager = new SubLocationManager(_Context);

            BindDDLLocation();
            gvLocation.DataBind();          
        }

        private void BindDDLLocation()
        {
            ddlLocation.DataSource = GetLocations();
            ddlLocation.DataTextField = DatabaseObjects.Columns.Title;
            ddlLocation.DataValueField = DatabaseObjects.Columns.Id;
            ddlLocation.DataBind();

            ddlLocation.Items.Insert(0, new ListItem("", "0"));
            ddlLocation.SelectedIndex = 0;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["exportType"] == "excel")
            {

                //var SubLocations = ObjSubLocationManager.Load().Select(x => new { x.ID, x.Title, x.LocationTag, x.Description, x.Address1, x.Address2, x.Zip, x.Phone, x.Deleted }).OrderBy(x => x.ID).ToList();
                var SubLocations = ObjSubLocationManager.Load();
                var Locations = ObjLocationManager.Load().Select(x => new { x.ID, x.Title }).ToList();

                var dtSubLocations = (from s in SubLocations join
                                     l in Locations on s.LocationID equals l.ID
                                     select new
                                     {
                                         Title = s.Title,
                                         Location = l.Title,
                                         s.LocationTag,
                                         s.Description,
                                         s.Address1,
                                         s.Address2,
                                         s.Zip,
                                         s.Phone,
                                         s.Deleted
                                     }).ToList();


                DataTable table = UGITUtility.ToDataTable(dtSubLocations);

                var worksheet = ASPxSpreadsheet1.Document.Worksheets.Add();
                worksheet.Import(table, true, 0, 0);
                MemoryStream st = new MemoryStream();
                ASPxSpreadsheet1.Document.SaveDocument(st, DocumentFormat.OpenXml);
                Response.Clear();
                Response.ContentType = "application/force-download";
                Response.AddHeader("content-disposition", "attachment; filename=SubLocation.xlsx");
                Response.BinaryWrite(st.ToArray());
                Response.End();
            }

            if (HttpContext.Current.Request.Form["__CALLBACKPARAM"] != null)
            {
                if (HttpContext.Current.Request.Form["__CALLBACKPARAM"].ToString().Contains("SelectedLocationId"))
                {
                    string[] val = HttpContext.Current.Request.Form["__CALLBACKPARAM"].Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                    string SelectedLocationId = val[val.Length - 1].Replace(";", string.Empty);
                    long LocationId = UGITUtility.StringToLong(SelectedLocationId);

                    BindSubLocations(SelectedLocationId);
                }
                else
                {
                    if (gvLocation.DataSource != null && gvLocation.Selection != null && gvLocation.Selection.Count > 0)
                    {
                        string SelectedLocationId = Convert.ToString(gvLocation.GetSelectedFieldValues(DatabaseObjects.Columns.ID)[0]);
                        long LocationId = UGITUtility.StringToLong(SelectedLocationId);
                        List<SubLocation> dr = ObjSubLocationManager.Load(x => x.LocationID == LocationId).ToList();
                        if (!string.IsNullOrEmpty(SelectedLocationId))
                        {
                            gvLocation.DataSource = dr;
                            gvLocation.DataBind();
                        }
                    }
                }
            }
            else
            {
                string SelectedLocationId = string.Empty;
                List<Location> Location = ObjLocationManager.Load();
                if (gvLocation.DataSource != null && gvLocation.Selection != null && gvLocation.Selection.Count > 0)
                {
                    SelectedLocationId = Convert.ToString(gvLocation.GetSelectedFieldValues(DatabaseObjects.Columns.ID)[0]);
                    long LocationId = UGITUtility.StringToLong(SelectedLocationId);
                    if (!string.IsNullOrEmpty(SelectedLocationId))
                    {
                        gvLocation.DataSource = Location;
                        gvLocation.DataBind();
                    }

                }
                BindSubLocations(SelectedLocationId);
                //Bind the sublocation data obtained based on the above conditions.
                //if (subLocation != null)
                //{
                    
                //    subLocation.ForEach(x =>
                //    {
                //        var objLocation = Location.FirstOrDefault(y => y.ID == x.LocationID);
                //        if (objLocation != null)
                //            x.LocationDetails = objLocation.Title;
                //    });
                //    gvSubLocation.DataSource = subLocation;
                //    gvSubLocation.DataBind();
                //}

            }
        }
       
        private List<SubLocation> GetSubLocationData(string selectedLocationId = "")
        {
            List<SubLocation> subLocations = null;
            long LocationId = UGITUtility.StringToLong(selectedLocationId);
            if (chkShowDeleted.Checked)
            {
                if (LocationId >0)
                    subLocations = ObjSubLocationManager.Load(x => x.LocationID == LocationId).ToList();
                else
                    subLocations = ObjSubLocationManager.Load().ToList();
            }
            else
            {
                if (LocationId > 0)
                    subLocations = ObjSubLocationManager.Load(x => x.LocationID == LocationId && x.Deleted == false).ToList();
                else
                    subLocations = ObjSubLocationManager.Load(x => x.Deleted == false).ToList();
            }
            if (subLocations.Count > 0)
                return subLocations;
            else
                return null;
        }

        private void BindSubLocations(string selectedLocation = null)
        {
            List<SubLocation> subLocations = GetSubLocationData(selectedLocation);
            if (subLocations != null)
            {
                List<Location> Location = ObjLocationManager.Load();
                if (Location.Count > 0)
                {
                    List<SubLocation> dr = new List<SubLocation>();
                    subLocations.ForEach(x =>
                    {
                        var objLocation = Location.FirstOrDefault(y => y.ID == x.LocationID);
                        if (objLocation != null)
                            x.LocationDetails = objLocation.Title;
                    });
                }
            }

            gvSubLocation.DataSource = subLocations;
            gvSubLocation.DataBind();
        }

        protected void gvLocation_DataBinding(object sender, EventArgs e)
        {
            gvLocation.DataSource = GetLocations();
        }

        private List<Location> GetLocations()
        {
            List<Location> location = ObjLocationManager.Load().OrderBy(x =>x.Title).ToList();
            return location;
        }

        protected void gvSubLocation_DataBinding(object sender, EventArgs e)
        {
        }

        protected void btSubLocationEdit_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton btEditLocation = (ImageButton)sender;
            int id = Convert.ToInt32(btEditLocation.CommandArgument);
            SubLocation item = ObjSubLocationManager.LoadByID(id);
            if (item != null)
            {
                FillWithSelectedLocation(item);
                editLocationBox.ShowOnPageLoad = true;
            }
        }

        private void FillWithSelectedLocation(SubLocation item)
        {
            hdnSelectedLocation.Value = item.ID.ToString();
            txtLocationTag.Text = item.LocationTag;
            txtAddress1.Text = item.Address1;
            txtAddress2.Text = item.Address2;
            txtSubLocationTitle.Text = item.Title;
            txtDescription.Text = item.Description;
            chkDeleted.Checked = item.Deleted;
            ddlLocation.SelectedValue = Convert.ToString(item.LocationID);
        }

        protected void lnkSubLocationEdit_Click(object sender, EventArgs e)
        {
            LinkButton lnkEditLocation = (LinkButton)sender;
            int id = Convert.ToInt32(lnkEditLocation.CommandArgument);
            SubLocation item = ObjSubLocationManager.LoadByID(id);
            if (item != null)
            {
                FillWithSelectedLocation(item);
                editLocationBox.ShowOnPageLoad = true;
            }
        }

        private void FillWithSelectedLocation(DataRow _SPListItem)
        {
            hdnSelectedLocation.Value = Convert.ToString(_SPListItem[DatabaseObjects.Columns.ID]);
            txtSubLocationTitle.Text = Convert.ToString(_SPListItem[DatabaseObjects.Columns.Title]);
            txtLocationTag.Text = Convert.ToString(_SPListItem[DatabaseObjects.Columns.LocationTag]);
            txtAddress1.Text = Convert.ToString(_SPListItem[DatabaseObjects.Columns.Address1]);
            txtAddress2.Text = Convert.ToString(_SPListItem[DatabaseObjects.Columns.Address2]);
            txtZip.Text = Convert.ToString(_SPListItem[DatabaseObjects.Columns.Zip]);
            txtPhone.Text = Convert.ToString(_SPListItem[DatabaseObjects.Columns.Phone]);
            txtDescription.Text = Convert.ToString(_SPListItem[DatabaseObjects.Columns.UGITDescription]);
            string lookupValue = Convert.ToString(_SPListItem[DatabaseObjects.Columns.LocationLookup]);
            if (lookupValue != null)
                ddlLocation.SelectedIndex = ddlLocation.Items.IndexOf(ddlLocation.Items.FindByValue(Convert.ToString(lookupValue)));

            chkDeleted.Checked = UGITUtility.StringToBoolean(_SPListItem[DatabaseObjects.Columns.Deleted]);
        }

        protected void btnSaveLocation_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            long id = 0;
            long.TryParse(hdnSelectedLocation.Value, out id);
            SubLocation _SPListItem = ObjSubLocationManager.LoadByID(id);
            if (_SPListItem == null)
                _SPListItem = new SubLocation();

            _SPListItem.LocationID = Convert.ToInt64(ddlLocation.SelectedValue);
            _SPListItem.Title = txtSubLocationTitle.Text.Trim();
            _SPListItem.LocationTag = txtLocationTag.Text.Trim();
            _SPListItem.Description = txtDescription.Text.Trim();
            _SPListItem.Address1 = txtAddress1.Text.Trim();
            _SPListItem.Address2 = txtAddress2.Text.Trim();
            _SPListItem.Phone = txtPhone.Text.Trim();
            _SPListItem.Zip = txtZip.Text.Trim();
            _SPListItem.Deleted = chkDeleted.Checked;

            ObjSubLocationManager.Insert(_SPListItem);

            if (_SPListItem.Deleted == chkDeleted.Checked)
            {
                if (id <= 0)
                {
                    ObjSubLocationManager.Insert(_SPListItem);
                }
                else
                {
                    ObjSubLocationManager.Update(_SPListItem);
                }
            }
            

            editLocationBox.ShowOnPageLoad = false;
            string SelectedLocationId = string.Empty;
            if (gvLocation.DataSource != null && gvLocation.Selection != null && gvLocation.Selection.Count > 0)
            {
                SelectedLocationId = Convert.ToString(gvLocation.GetSelectedFieldValues(DatabaseObjects.Columns.ID)[0]);               
            }
            BindSubLocations(SelectedLocationId);
            // Response.Redirect(Request.RawUrl);
        }


        protected void chkShowDeleted_CheckedChanged(object sender, EventArgs e)
        {
           
        }

        protected void cvFieldValidator1_ServerValidate(object source, ServerValidateEventArgs args)
        {   
        }

        protected void btnApplyChanges_Click(object sender, EventArgs e)
        {
            ApplicationContext context = HttpContext.Current.GetManagerContext();
            string cacheName = "Lookup_" + DatabaseObjects.Tables.SubLocation + "_" + context.TenantID;
            DataTable dt = GetTableDataManager.GetTableData(DatabaseObjects.Tables.SubLocation, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'");
            Util.Cache.CacheHelper<object>.AddOrUpdate(cacheName, context.TenantID, dt);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(HttpContext.Current, true);
        }
    }
}
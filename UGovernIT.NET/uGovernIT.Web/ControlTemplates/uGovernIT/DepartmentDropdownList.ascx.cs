using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using DevExpress.Utils;
using DevExpress.Web;
using System.Collections.Generic;
using System.Linq;
using uGovernIT.Manager;
using uGovernIT.Utility;
using System.Web;
using System.Net;
using System.Data;
using DevExpress.Charts.Native;
using System.Collections;
using System.Text.RegularExpressions;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Web
{
    public partial class DepartmentDropdownList : UserControl
    {
        public string Value { get; private set; }
        public ControlMode ControlMode { get; set; }
        public bool IsMulti { get; set; }
        public bool MandatoryCheck { get; set; }
        public string MandatoryMessage { get; set; }
        public string ValidationGroup { get; set; }
        protected bool enableDivision;
        private bool showDepartmentDetail;
        private bool popupRequired;
        private bool companyDropdownRequired;
        public bool ShowGLCode { get; set; }
        public bool EnableHoverEdit { get; set; }
        public bool EnablePostBack { get; set; }
        private bool SingleDepartment { get; set; }
        protected string companyLabel;
        protected string divisionLevel;
        protected string departmentLevel;
        public bool singlecompany;
        public bool showcompanyname = false;
        public string SetValueCheck { get; set; }

        public ASPxDropDownEdit DropDownEdit { get; set; }
        /// <summary>
        /// Just specify function name here. it will return call specified function with selected department element id which contain selected department
        /// E.g when your specify xyz as function then it call this function as xyz(id)
        /// </summary>
        public string CallBackJSEvent { get; set; }
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        DepartmentManager deptManager;
        CompanyManager objCompanyManager;
        ConfigurationVariableManager objConfigurationVariableHelper;
        List<Utility.Company> companies = new List<Utility.Company>();
        public string DivisionLabel = "Division";
        public DepartmentDropdownList()
        {
            objCompanyManager = new CompanyManager(context);
            objConfigurationVariableHelper = new ConfigurationVariableManager(context);
            deptManager = new DepartmentManager(context);
            enableDivision = objConfigurationVariableHelper.GetValueAsBool(ConfigConstants.EnableDivision);
            showDepartmentDetail = objConfigurationVariableHelper.GetValueAsBool(ConfigConstants.ShowDepartmentDetail);
            companies = objCompanyManager.LoadAllHierarchy();

            if (companies.Count > 1)
            {
                companyDropdownRequired = true;
                popupRequired = true;

            }
            else
            {
                companyDropdownRequired = false;
                SingleDepartment = true;
                if (enableDivision && companies.First().CompanyDivisions != null && companies.First().CompanyDivisions.Count > 1)
                {
                    popupRequired = true;
                }
                if (!IsMulti)
                    popupRequired = true;
            }
        }

        public void SetValue(string value)
        {
            Value = value;
            if (ControlMode == ControlMode.Display)
            {
                List<LookupValue> departments = GetSelectedDepartmentLookups();
                //dementmentViewTD.InnerHtml = string.Empty;

                pDepartmentOnly.Visible = false;

                foreach (LookupValue lookup in departments)
                {
                    Utility.Department selectedDepartment = GetDepartment(lookup.IDValue);
                    if (selectedDepartment != null)
                    {
                        string sperator = string.Empty;
                        if (IsMulti)
                            sperator = ";";

                        string toolTipS = GetDepartmentToolTipDetail(selectedDepartment);

                        //if (departments.Count == 1 || showDepartmentDetail)
                        //    dementmentViewTD.InnerHtml += string.Format("<span title='{1}'>{2}{0}</span>", sperator, toolTipS, toolTipS.Replace(" ", "&nbsp;"));
                        //else
                        //    dementmentViewTD.InnerHtml += string.Format("<span title='{1}'>{0}{2}</span>", selectedDepartment.Title, toolTipS, sperator);

                        if (!IsMulti)
                            break;
                    }
                }
                pDepartmentView.Visible = true;
                // dementmentViewTD.Visible = true;
            }
            else
            {
                hdnDepartments.Set("ismulti", IsMulti);
                hdnDepartments.Set("enabledivision", enableDivision);
                hdnDepartments.Set("showDepartmentDetail", showDepartmentDetail);
                hdnDepartments.Set("companyRequired", companyDropdownRequired);

                List<LookupValue> selectedLookups = GetSelectedDepartmentLookups();
                if (selectedLookups.Count > 0)
                {
                    ////Value = selectedLookups.First().ID.ToString();
                }

                Value = string.Join(";#", selectedLookups.Select(x => string.Format("{0};#{1}", x.ID.ToString(), x.Value)).ToArray());
                SetValueCheck = string.Join(",", selectedLookups.Select(x => string.Format("{0}", x.ID.ToString())).ToArray());
                ////hdnDepartments.Set("selectedDepartments", Value);

                //dementmentViewTD.InnerHtml = string.Empty;
                //pre-filled value in case of popup 
                if (popupRequired)
                {
                    pSelectedDepartments.InnerHtml = "";
                    foreach (LookupValue lookup in selectedLookups)
                    {
                        Utility.Department selectedDepartment = GetDepartment(Convert.ToInt32(lookup.ID));
                        if (selectedDepartment != null)
                        {
                            string toolTipS = GetDepartmentToolTipDetail(selectedDepartment);

                            string sperator = string.Empty;
                            if (IsMulti)
                                sperator = ";";

                            //if (selectedLookups.Count == 1 || showDepartmentDetail)
                            //    dementmentViewTD.InnerHtml += string.Format("<span title='{1}'>{2}{0}</span>", sperator, toolTipS, toolTipS.Replace(" ", "&nbsp;"));
                            //else
                            //    dementmentViewTD.InnerHtml += string.Format("<span title='{1}'>{0}{2}</span>", selectedDepartment.Title, toolTipS, sperator);

                            Company selectedCompany = new Company();
                            if (selectedDepartment.CompanyLookup != null)
                            {
                                selectedCompany = companies.FirstOrDefault(x => x.ID == selectedDepartment.ID);
                                if (selectedCompany == null)
                                    selectedCompany = new Company();
                            }

                            CompanyDivision selectedDivision = new CompanyDivision();

                            if (enableDivision)
                            {
                                Company cmp = companies.FirstOrDefault(x => x.CompanyDivisions != null && x.CompanyDivisions.Exists(y => y.ID.ToString() == selectedDepartment.DivisionLookup.ID));
                                if (cmp != null)
                                {
                                    selectedDivision = cmp.CompanyDivisions.FirstOrDefault(x => x.ID.ToString() == selectedDepartment.DivisionLookup.ID);
                                }
                            }

                            string fullGLCode = string.Empty;
                            if (selectedCompany.GLCode != string.Empty)
                                fullGLCode = selectedCompany.GLCode;
                            if (selectedDivision.GLCode != string.Empty)
                            {
                                if (fullGLCode != string.Empty)
                                    fullGLCode += "-";
                                fullGLCode += selectedDivision.GLCode;
                            }
                            if (selectedDepartment.GLCode != string.Empty)
                            {
                                if (fullGLCode != string.Empty)
                                    fullGLCode += "-";
                                fullGLCode += selectedDepartment.GLCode;
                            }

                            pSelectedDepartments.InnerHtml += string.Format("<span id='{0}' code='{7}'><i class='name' code='{4}'>{1}</i><i class='company' code='{5}'>{2}</i><i class='division' code='{6}'>{3}</i></span>", selectedDepartment.ID, selectedDepartment.Title, selectedCompany.Title, selectedDivision.Title, selectedDepartment.GLCode, selectedCompany.GLCode, selectedDivision.GLCode, fullGLCode);
                            if (!IsMulti)
                                break;
                        }

                    }

                    txtDepartmentCtr.Text = string.Join("; ", selectedLookups.Select(x => x.Value).ToArray());
                }
                else
                {
                    //Pre-populate value when link department box comes
                    if (selectedLookups.Count > 0)
                    {
                        //dementmentViewTD.InnerHtml = selectedLookups.First().Value;
                        cmbDepamentOnly.SelectedIndex = cmbDepamentOnly.Items.IndexOf(cmbDepamentOnly.Items.FindByValue(selectedLookups.First().ID.ToString()));
                        txtDepartmentCtr.Text = cmbDepamentOnly.Text;

                        Utility.Department selectedDepartment = GetDepartment(Convert.ToInt32(selectedLookups.First().ID));

                        pSelectedDepartments.InnerHtml = string.Format("<span id='{0}' code='{1}'><i class='name' code='{1}'>{2}</i><i class='company' code=''></i><i class='division' code='\'></i></span>", selectedDepartment.ID, selectedDepartment.GLCode, selectedDepartment.Title);
                    }
                }
            }
        }

        public Department GetDepartment(long departmentID)
        {
            List<Department> allDepartments = deptManager.GetDepartmentData();// uGITCache.LoadDepartments(SPContext.Current.Web);
            Utility.Department selectedDepartment = allDepartments.FirstOrDefault(x => x.ID == departmentID && x.Deleted != true);
            return selectedDepartment;
        }

        public List<long> GetSelectedDepartmentIDs()
        {
            List<Department> lookups = GetSelectedDepartmentsObj(Value);
            return lookups.Select(x => x.ID).ToList();
        }

        public List<string> GetSelectedDepartments()
        {
            List<Department> lookups = GetSelectedDepartmentsObj();
            return lookups.Select(x => x.Title).ToList();
        }

        public List<LookupValue> GetSelectedDepartmentLookups()
        {
            List<LookupValue> lookups = new List<LookupValue>();
            List<Department> sDepartments = GetSelectedDepartmentsObj(Value);
            foreach (Department dp in sDepartments)
            {
                lookups.Add(new LookupValue(dp.ID, dp.Title));
            }
            return lookups;
        }

        public List<Department> GetSelectedDepartmentsObj(string Value = "")
        {
            List<Department> sDepartments = new List<Department>();
            if (!string.IsNullOrEmpty(Value))
            {
                //if value contains in ID;#Value format
                string[] selDpts = Convert.ToString(Value).Split(new string[] { ";#", "," }, StringSplitOptions.None);
                if (selDpts.Length > 1)
                {
                    for (int i = 0; i < selDpts.Length; i++)
                    {
                        int departmentID = 0;
                        if (int.TryParse(selDpts[i].Trim(), out departmentID))
                        {
                            Department dp = GetDepartment(departmentID);
                            if (dp != null)
                            {
                                sDepartments.Add(dp);
                            }
                        }
                    }
                }
                //If only ID is passed
                else if (selDpts.Length == 1)
                {
                    int departmentID = 0;
                    int.TryParse(selDpts[0], out departmentID);
                    Utility.Department selectedDepartment = GetDepartment(departmentID);
                    if (selectedDepartment != null)
                        sDepartments.Add(selectedDepartment);
                }
            }
            return sDepartments;
        }

        private string GetDepartmentToolTipDetail(Department selectedDepartment)
        {
            //Create tooltop
            List<string> toolTip = new List<string>();
            FieldConfigurationManager configfield = new FieldConfigurationManager(context);
            // Add Company

            if (selectedDepartment.CompanyLookup != null && companyDropdownRequired)
            {
                string str = configfield.GetFieldConfigurationData(DatabaseObjects.Columns.CompanyIdLookup, Convert.ToString(selectedDepartment.CompanyLookup.ID));
                toolTip.Add(string.Format("{0}", str));
            }
            // Add Division
            if (enableDivision && selectedDepartment.DivisionLookup != null &&
                selectedDepartment.DivisionLookup.Value != "N/A" && selectedDepartment.CompanyLookup != null && selectedDepartment.DivisionLookup.Value != selectedDepartment.CompanyLookup.Value)
            {
                string str = configfield.GetFieldConfigurationData(DatabaseObjects.Columns.DivisionIdLookup, Convert.ToString(selectedDepartment.DivisionLookup.ID));
                toolTip.Add(string.Format("{0}", str));
            }

            // Add Department
            toolTip.Add(string.Format("{0}", selectedDepartment.Title));

            string toolTipS = string.Join(" > ", toolTip.ToArray());
            toolTipS = toolTipS.Replace("'", "\'").Replace("\"", "\\\"");

            return toolTipS;

        }

        protected override void OnInit(EventArgs e)
        {
            object val = "";
            ASPxHiddenField1.TryGet("DivisionChanged", out val);
            if (IsPostBack && val?.ToString() == "true")
            {
                UGITUtility.CreateCookie(Response, "CalledFromDivisionDropdown", "true");
                ASPxHiddenField1["DivisionChanged"] = "false";
            }
            object division;
            hdnSetParams.TryGet("AddAllDeptsOfDivn", out division);
            if (UGITUtility.ObjectToString(division) == "YES")
            {
                cmbDivision.Enabled = false;
                cmbDivision.Visible = false;
            }
            companyLabel = uHelper.GetDepartmentLabelName(DepartmentLevel.Company);
            divisionLevel = objConfigurationVariableHelper.GetValue(ConfigConstants.DivisionLabel) + ":";
            departmentLevel = uHelper.GetDepartmentLabelName(DepartmentLevel.Department);

            lbCompany.Text = string.Format("{0}:", companyLabel);
            lbDivision.Text = string.Format("{0}:", divisionLevel);
            lbDepartment.Text = string.Format("{0}:", departmentLevel);
            //pcMain.HeaderText = string.Format("Select {0}:", departmentLevel);

            if (string.IsNullOrEmpty(Value))
                Value = SetValueCheck;

            SetProperties();
            base.OnInit(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            if (IsPostBack)
            {
                object val = "";
                hdnDepartments.TryGet("selectedDepartments", out val);
                if (!popupRequired)
                {
                    if (Request[cmbDepamentOnly.UniqueID] == null)
                        val = Value;
                    else
                        val = cmbDepamentOnly.Value;
                }

                if (!string.IsNullOrEmpty(UGITUtility.ObjectToString(val)))
                {
                    SetValue(Convert.ToString(val));
                }
                else
                {
                    SetValue(Value);
                }

                object valDiv = "";
                ASPxHiddenField1.TryGet("Division", out valDiv);

                if (Convert.ToString(valDiv) != "")
                {
                    bool isNumber = Regex.IsMatch(Convert.ToString(valDiv), @"-?\d+(\.\d+)?");

                    if (!isNumber)
                    {
                        string[] selectedDivision = ((IEnumerable)valDiv).Cast<object>()
                                     .Select(x => x.ToString())
                                     .ToArray();



                        string selectedDiv = string.Join(",", selectedDivision);


                        if (!isNumber)
                        {

                            SetCookie("division", Convert.ToString(""));
                            //if (selectedDiv != null)
                            //{
                            //    //valDiv = dt.AsEnumerable().Where(x => x.Field<string>("Title") == selectedDiv);
                            //    DataSet ds = new DataSet();
                            //    Dictionary<string, object> values = new Dictionary<string, object>();
                            //    //values.Add("@TenantId", "abc");
                            //    values.Add("@divisions", selectedDiv);
                            //    ds = GetTableDataManager.GetDataSet("CompanyDivision", values);

                            //    valDiv = Convert.ToString(ds.Tables[0].Rows[0]["Division"]);

                            //    if (!string.IsNullOrEmpty(Convert.ToString(valDiv)))
                            //    {
                            //        //UGITUtility.DeleteCookie(Request, Response, "division");
                            //        SetCookie("division", Convert.ToString(valDiv));
                            //    }
                            //    else
                            //    {
                            //        //UGITUtility.DeleteCookie(Request, Response, "division");
                            //        SetCookie("division", Convert.ToString(valDiv));
                            //    }
                            //}

                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(valDiv)))
                        {
                            //UGITUtility.DeleteCookie(Request, Response, "division");
                            SetCookie("division", Convert.ToString(valDiv));
                        }
                        else
                        {
                            //UGITUtility.DeleteCookie(Request, Response, "division");
                            SetCookie("division", Convert.ToString(valDiv));
                        }
                    }
                }
                else
                {
                    SetCookie("division", Convert.ToString(valDiv));
                }
                

                
            }
            else
            {
                
                    SetValue(Value);
                
            }
            base.OnLoad(e);
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (ControlMode == ControlMode.New)
            {
                pDepartmentView.Visible = false;
                if (popupRequired)
                {
                    pDepartmentView.Visible = true;
                    // lbNewIcon.Visible = true;
                }
                // lbEditIcon.Visible = false;
            }
            else if (ControlMode == ControlMode.Display)
            {
                // lbNewIcon.Visible = false;
                // lbEditIcon.Visible = false;
            }
            //SetValues(Convert.ToString(val));            
            SetValues(SetValueCheck);
            if ((Convert.ToString(cmbDivision.Value).Contains(",") || string.IsNullOrEmpty(Convert.ToString(cmbDivision.Value))) && enableDivision)
            {
                cmbDivision.Value = "";
                
                FillDepartmentComboNull(Convert.ToString(cmbCompany.Value), Convert.ToString(cmbDivision.Value));
            }
            else
            {
                ASPxHiddenField1.Set("Division", cmbDivision.Value);
                ASPxHiddenField1.Set("DivisionText", cmbDivision.Text);
                FillDepartmentCombo(Convert.ToString(cmbCompany.Value), Convert.ToString(cmbDivision.Value));
            }
            base.OnPreRender(e);
        }

        protected void cmbDepartment_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {

            string selectedDepartment = e.Parameter;

            string selectedDivision = Convert.ToString(cmbDivision.Value);
            string selectedCompany = Convert.ToString(cmbCompany.Value);
            if (string.IsNullOrEmpty(selectedDivision) && !string.IsNullOrEmpty(selectedDepartment))
            {
                selectedDivision = selectedDepartment;
                cmbDivision.Value = selectedDepartment;
                cmbDivision.Enabled = false;
            }

            if (e.Parameter.Contains("|"))
            {
                selectedDepartment = e.Parameter.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries)[1];
                if (string.IsNullOrEmpty(selectedDivision))
                    selectedDivision = e.Parameter.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries)[0];
                if (string.IsNullOrEmpty(selectedCompany))
                    selectedCompany = e.Parameter.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries)[1];
            }
            FillDepartmentCombo(selectedCompany, selectedDivision);


            if (cmbDepartment.Items.Count > 0 && !string.IsNullOrEmpty(selectedDepartment))
            {
                int index = cmbDepartment.Items.IndexOfText(selectedDepartment) > -1 ? cmbDepartment.Items.FindByText(selectedDepartment).Index : -1;
                if (cmbDepartment.JSProperties.ContainsKey("cpDepartmentSelectedIndex"))
                {
                    cmbDepartment.JSProperties["cpDepartmentSelectedIndex"] = index;
                }
                else
                {
                    cmbDepartment.JSProperties.Add("cpDepartmentSelectedIndex", index);
                }
            }

        }
        protected void cmbDivision_Callback(object sender, CallbackEventArgsBase e)
        {
            string[] paramsVal = e.Parameter.Split(new string[] { "|" }, StringSplitOptions.None);
            string selectedDivision = paramsVal.Length > 1 ? paramsVal[1] : Convert.ToString(cmbDivision.Value);
            string selectedCompany = paramsVal.Length > 0 ? paramsVal[0] : Convert.ToString(cmbCompany.Value);
            string selectedDepartment = paramsVal.Length > 2 ? paramsVal[2] : Convert.ToString(cmbDepartment.Value);
            FillDivisionCombo(selectedCompany);
            if (cmbDivision.Items.Count > 0)
            {
                cmbDivision.Text = selectedDivision;
                if (cmbDivision.JSProperties.ContainsKey("cpDivisionSelectedIndex"))
                {
                    cmbDivision.JSProperties["cpDivisionSelectedIndex"] = cmbDivision.SelectedIndex; //!IsPreview && 
                }
                else
                {
                    cmbDivision.JSProperties.Add("cpDivisionSelectedIndex", cmbDivision.SelectedIndex); // !IsPreview && 
                }
            }



            FillDepartmentCombo(selectedCompany, selectedDivision);
            if (cmbDepartment.Items.Count > 0)
            {
                cmbDivision.Text = selectedDivision;
                if (cmbDivision.JSProperties.ContainsKey("cpDepartment"))
                {
                    cmbDivision.JSProperties["cpDepartment"] = selectedDepartment;
                }
                else
                {
                    cmbDivision.JSProperties.Add("cpDepartment", selectedDepartment);
                }
            }
        }

        protected void FillDivisionCombo(string country)
        {
            if (string.IsNullOrEmpty(country)) return;

            Utility.Company cmp = companies.FirstOrDefault(x => x.Title == country && !x.Deleted);
            cmbDivision.ValueField = DatabaseObjects.Columns.ID;
            cmbDivision.TextField = DatabaseObjects.Columns.Title;
            cmbDivision.DataSource = cmp.CompanyDivisions;
            cmbDivision.DataBind();
        }

        protected void FillDepartmentCombo(string country, string division)
        {
            cmbDepartment.Items.Clear();
            UGITUtility.DeleteCookie(Request, Response, "CalledFromDivisionDropdown");
            Utility.Company cmp = companies.FirstOrDefault(x => x.Title == country && !x.Deleted);
            if (companies.Count == 1)
                cmp = companies.First();

            if (cmp == null) return;

            List<Department> departments = new List<Department>();
            if (!string.IsNullOrEmpty(division))
            {
                if (division != "undefined")
                {
                    CompanyDivision cDivision;
                    if (int.TryParse(division, out int divID))
                        cDivision = cmp.CompanyDivisions.FirstOrDefault(x => x.ID == divID && !x.Deleted);
                    else
                        cDivision = cmp.CompanyDivisions.FirstOrDefault(x => x.Title.Equals(division, StringComparison.OrdinalIgnoreCase) && !x.Deleted);
                    //CompanyDivision cDivision = cmp.CompanyDivisions.FirstOrDefault(x => x.Title == division && !x.Deleted);
                    if (cDivision != null)
                        departments = cDivision.Departments.Where(x => !x.Deleted).ToList();
                }
                else
                {
                    departments = cmp.Departments.Where(x => !x.Deleted).ToList();
                }
            }
            else
            {
                departments = cmp.Departments.Where(x => !x.Deleted).ToList();
            }

            cmbDepartment.ValueField = DatabaseObjects.Columns.ID;
            cmbDepartment.TextField = DatabaseObjects.Columns.Title;
            cmbDepartment.DataSource = departments;
            cmbDepartment.DataBind();
        }

        protected void FillDepartmentComboNull(string country, string division)
        {
            cmbDepartment.Items.Clear();
            UGITUtility.DeleteCookie(Request, Response, "CalledFromDivisionDropdown");
            Utility.Company cmp = companies.FirstOrDefault(x => x.Title == country && !x.Deleted);
            if (companies.Count == 1)
                cmp = companies.First();

            if (cmp == null) return;

            List<Department> departments = new List<Department>();
            if (!string.IsNullOrEmpty(division))
            {
                CompanyDivision cDivision = cmp.CompanyDivisions.FirstOrDefault(x => x.ID == Convert.ToInt32(division) && !x.Deleted);
                //CompanyDivision cDivision = cmp.CompanyDivisions.FirstOrDefault(x => x.Title == division && !x.Deleted);
                if (cDivision != null)
                    departments = cDivision.Departments.Where(x => !x.Deleted).ToList();
            }

            cmbDepartment.ValueField = DatabaseObjects.Columns.ID;
            cmbDepartment.TextField = DatabaseObjects.Columns.Title;
            cmbDepartment.DataSource = departments;
            cmbDepartment.DataBind();
        }

        protected void cmbDepamentOnly_Callback(object sender, CallbackEventArgsBase e)
        {


        }

        protected void ASPxCallback1_Callback(object source, DevExpress.Web.CallbackEventArgs e)
        {

            SetValue(e.Parameter);
            e.Result = pSelectedDepartments.InnerHtml;

        }


        public void ResetProperties()
        {
            // editDepartment.Style.Add(HtmlTextWriterStyle.Display, "none");
            pDepartmentView.Visible = true;
            // dementmentEditTD.Visible = true;
            // editDepartment.Visible = true;
            // lbNewIcon.Visible = false;
            // editDepartment1.Visible = true;
            //  dementmentViewTD.Visible = true;
            pSelectedDepartments.Style.Add(HtmlTextWriterStyle.Display, "none");
            pSelectedDepartments.Visible = true;
            pDepartmentOnly.Visible = false;
            txtDepartmentCtr.Visible = true;
            pcMain.Enabled = true;

            SetProperties();
        }

        private void SetProperties()
        {
            if (EnablePostBack)
            {
                //cmbDepamentOnly.AutoPostBack = true;
            }

            if (EnableHoverEdit)
            {
                // new line of code for show edit on mouseover...
                pDepartmentView.Attributes.Add("onmouseover", string.Format("javascript:showdepartmenteditbuttonOnhover( this);"));
                pDepartmentView.Attributes.Add("onmouseout", string.Format("javascript:hidedepartmenteditbuttonOnhover(this);"));
            }
            else
            {
                //editDepartment.Style.Add(HtmlTextWriterStyle.Display, "block");
                //editDepartment1.Style.Add(HtmlTextWriterStyle.Display, "block");
            }

            cmbDepamentOnly.ClientSideEvents.SelectedIndexChanged = string.Format("function(s, e) {0} OnDepartmentOnlyBoxChanged('{2}', s); {1}", "{", "}", this.ClientID);
            gdDepartmentOnly.ClientSideEvents.SelectionChanged = string.Format("function(s,e){{onGridSelectionChanged_department(s,e,'{0}');}}", this.Parent.ClientID);
            // pcMain.ClientSideEvents.PopUp = string.Format("function(s, e) {0} callBeforeDepartmentPopupOpen('{2}',s); {1}", "{", "}", this.ClientID);
            cmbCompany.ClientSideEvents.SelectedIndexChanged = string.Format("function(s, e) {0} OnCompanyChanged('{2}', s); {1}", "{", "}", this.ClientID);
            //cmbCompany.ClientSideEvents.SelectedIndexChanged = string.Format("function(s,e){{OnCompanyChanged(s,e,'{0}');}}", this.Parent.ClientID);
            cmbDivision.ClientSideEvents.SelectedIndexChanged = string.Format("function(s, e) {0} OnDivisionChanged('{2}', s); {1}", "{", "}", this.ClientID);
            cmbDivision.ClientSideEvents.EndCallback = string.Format("function(s, e) {0} OnDivisionEndCallback('{2}', s, e); {1}", "{", "}", this.ClientID);
            cmbDepartment.ClientSideEvents.EndCallback = string.Format("function(s, e) {0} OnDepartmentEndCallback('{2}', s, e); {1}", "{", "}", this.ClientID);
            btAddDepartment.Attributes.Add("onclick", string.Format("addDepartment('{0}')", this.ClientID));
            btAddAllDepartment.Attributes.Add("onclick", string.Format("addAllDepartment('{0}')", this.ClientID));
            btRemoveFromList.Attributes.Add("onclick", string.Format("removeDepartment_MultiSelect('{0}')", this.ClientID));
            //btResetPopup.Attributes.Add("onclick", string.Format("resetDepartment('{0}')", this.ClientID));
            btResetPopup.ClientSideEvents.Click = "function(s, e){ resetDepartment(s, e, '" + this.ClientID + "') }";
            //btDonePopup.Attributes.Add("onclick", string.Format("editDepartmentDone(s, e, '{0}')", this.ClientID));
            btDonePopup.ClientSideEvents.Click = "function(s, e){ editDepartmentDone(s, e, '" + this.ClientID + "'); }";
            btCancel.ClientSideEvents.Click = "function(s, e){ openDepartPopupToClose('" + this.ClientID + "',this); }";


            btAddAllDepartment.Visible = false;
            btResetPopup.Text = "Reset";
            btRemoveAllDepartment.Visible = false;
            if (IsMulti)
            {
                btResetPopup.Text = "Select All";
                GridViewColumn c = gdDepartmentOnly.Columns["CheckWithMulti"];
                c.Visible = true;
                gdDepartmentOnly.SettingsBehavior.AllowSelectSingleRowOnly = false;
                btAddAllDepartment.Visible = true;
                btRemoveAllDepartment.Visible = true;
                btAddAllDepartment.Attributes.Add("onclick", string.Format("addAllDepartment('{0}')", this.ClientID));
                btRemoveAllDepartment.Attributes.Add("onclick", string.Format("removeAllDepartment('{0}')", this.ClientID));
                cmbDepartment.SelectionMode = ListEditSelectionMode.Multiple;
            }



            ASPxCallback1.ClientSideEvents.CallbackComplete = string.Format("function(s, e) {0} onDepartmentCallbackComplete('{2}',s, e); {1}", "{", "}", this.ClientID);

            if (ControlMode == ControlMode.Display)
            {
                //dementmentEditTD.Visible = false;
                pDepartmentOnly.Visible = false;
                pcMain.Enabled = false;

            }
            else
            {
                cmbDepartment.Attributes.Add("ondblclick", string.Format("addDepartment('{0}')", this.ClientID));

                if (IsMulti)
                {
                    popupRequired = true;
                    btRemoveFromList.Visible = true;
                    multipleDepartmentTD.Visible = true;
                    //bottomActionTD.ColSpan = 3;
                    multipleDepartmentActionTD.Visible = true;
                }
                else
                {
                    btRemoveFromList.Visible = false;
                    multipleDepartmentTD.Visible = false;
                    //bottomActionTD.ColSpan = 1;
                    multipleDepartmentActionTD.Visible = false;
                }

                if (!companyDropdownRequired)
                {
                    cmpControlDiv.Visible = false;
                    cmbDepartment.Height = new Unit(cmbDepartment.Height.Value + 100, UnitType.Pixel);
                }

                if (!enableDivision)
                {
                    divisionControlDiv.Visible = false;
                    cmbDepartment.Height = new Unit(cmbDepartment.Height.Value + 100, UnitType.Pixel);
                }

                if (MandatoryCheck)
                {
                    txtDepartmentValidator.Visible = true;
                    txtDepartmentValidator.ValidationGroup = ValidationGroup;
                    txtDepartmentCtr.ValidationGroup = ValidationGroup;
                    if (!string.IsNullOrEmpty(MandatoryMessage))
                        txtDepartmentValidator.ErrorMessage = MandatoryMessage;
                }

                pDepartmentOnly.Visible = true;
                pcMain.Enabled = false;
                if (popupRequired)
                {
                    pDepartmentOnly.Visible = false;
                    pcMain.Enabled = true;
                }

                if (!popupRequired && ControlMode == ControlMode.Edit)
                {
                    if (EnableHoverEdit)
                    {
                        pDepartmentOnly.Style.Add(HtmlTextWriterStyle.Display, "none");
                    }
                    else
                    {
                        pDepartmentView.Visible = false;
                    }
                }

                if (ShowGLCode)
                {
                    ListBoxColumn lbColumn = new ListBoxColumn(DatabaseObjects.Columns.GLCode, " ", new Unit(20, UnitType.Pixel));
                    cmbCompany.Columns.Add(lbColumn);
                    cmbCompany.Columns.Add(DatabaseObjects.Columns.Title);
                    cmbCompany.DisplayFormatString = "{1}";

                    lbColumn = new ListBoxColumn(DatabaseObjects.Columns.GLCode, " ", new Unit(20, UnitType.Pixel));
                    cmbDivision.Columns.Add(lbColumn);
                    cmbDivision.Columns.Add("Title");
                    cmbDivision.DisplayFormatString = "{1}";

                    lbColumn = new ListBoxColumn(DatabaseObjects.Columns.GLCode, " ", new Unit(20, UnitType.Pixel));
                    cmbDepartment.Columns.Add(lbColumn);
                    cmbDepartment.Columns.Add("Title");

                    lbColumn = new ListBoxColumn(DatabaseObjects.Columns.GLCode, " ", new Unit(20, UnitType.Pixel));
                    cmbDepamentOnly.Columns.Add(lbColumn);
                    cmbDepamentOnly.Columns.Add("Title");
                    cmbDepamentOnly.DisplayFormatString = "{1}";
                }



                if (popupRequired)
                {
                    pDepartmentView.Visible = true;
                    pcMain.Visible = true;
                    if (companyDropdownRequired)
                    {
                        List<Company> activeCompanies = companies.Where(x => !x.Deleted).ToList();
                        cmbCompany.ValueField = DatabaseObjects.Columns.Title;
                        cmbCompany.TextField = DatabaseObjects.Columns.Title;
                        cmbCompany.DataSource = activeCompanies;
                        if (activeCompanies.Count == 1)
                        {
                            cmbCompany.SelectedIndex = 0;
                            singlecompany = true;
                        }
                        cmbCompany.DataBind();

                    }
                    if (enableDivision)
                    {
                        Utility.Company cmp = companies.First();
                        List<CompanyDivision> activeDivisions = cmp.CompanyDivisions.Where(x => !x.Deleted).ToList();
                        cmbDivision.ValueField = DatabaseObjects.Columns.ID;
                        cmbDivision.TextField = DatabaseObjects.Columns.Title;
                        cmbDivision.DataSource = activeDivisions;
                        if (singlecompany)
                            cmbDivision.SelectedIndex = 0;
                        cmbDivision.DataBind();

                    }
                    else
                    {
                        Utility.Company cmp = companies.First();
                        cmbDepartment.Items.Clear();
                        List<Department> activeDepartments = cmp.Departments.Where(x => !x.Deleted).ToList();
                        cmbDepartment.ValueField = DatabaseObjects.Columns.ID;
                        cmbDepartment.TextField = DatabaseObjects.Columns.Title;
                        cmbDepartment.DataSource = activeDepartments;
                        cmbDepartment.DataBind();
                    }
                }
                else
                {
                    if (SingleDepartment)
                    {
                        pcMain.Visible = false;
                    }
                    pDepartmentView.Visible = false;
                    List<Department> activeDepartments = companies.First().Departments.Where(x => !x.Deleted).ToList();
                    gdDepartmentOnly.DataSource = activeDepartments;
                    gdDepartmentOnly.DataBind();
                    // cmbDepamentOnly.ValueField = DatabaseObjects.Columns.Id;
                    // cmbDepamentOnly.TextField = DatabaseObjects.Columns.Title;
                    // cmbDepamentOnly.DataSource = activeDepartments;
                    //  cmbDepamentOnly.DataBind();
                }

                if (popupRequired)
                {
                    //editDepartment.Attributes.Add("onclick", string.Format("openDepartPopupToEdit('{0}', this);", this.ClientID));
                    // editDepartment1.Attributes.Add("onclick", string.Format("openDepartPopupToEdit('{0}', this);", this.ClientID));
                }
                else
                {
                    // editDepartment.Attributes.Add("onclick", string.Format("showDepartmentEditForm('{0}', this);", this.ClientID));
                }
            }
        }
        public string GetValues()
        {
            string value = string.Join(",", this.gdDepartmentOnly.GetSelectedFieldValues(gdDepartmentOnly.KeyFieldName).ToList());
            return value;
        }
        public void SetValues(string value)
        {
            if (gdDepartmentOnly.DataSource == null)
            {
                if (!string.IsNullOrEmpty(value) && value != "0")
                {
                    CompanyDivisionManager companyDivisionManager = new CompanyDivisionManager(context);
                    List<Department> department = deptManager.Load();

                    List<string> values = UGITUtility.SplitString(value, ",").ToList();
                    List<string> Titles = new List<string>();
                    List<string> division = new List<string>();
                    string companyTitle = string.Empty;
                    string companydivisionid = string.Empty;
                    string companydivisiontitle = string.Empty;
                    string multipleDepartment = string.Empty;
                    List<string> hiddenFieldVal = new List<string>();

                    if (values != null && values.Count > 0)
                        values = values.Where(x => !string.IsNullOrEmpty(x)).Select(y => y).Distinct().ToList();

                    values.ForEach(x =>
                    {
                        cmbDepartment.SelectedItem = cmbDepartment.Items.FindByValue(x);

                        Department activeDepartment = department.FirstOrDefault(z => z.ID.Equals(Convert.ToInt64(x)));
                        if (activeDepartment != null)
                        {
                            Company company = objCompanyManager.Get(y => y.ID.Equals(activeDepartment.CompanyIdLookup) && y.Deleted == false);
                            CompanyDivision companyDivision = companyDivisionManager.Get(y => y.ID.Equals(activeDepartment.DivisionIdLookup) && y.Deleted == false);
                            if (company != null)
                            {
                                companyTitle = company.Title;
                                Titles.Add(company.Title);
                                cmbDepartment.ValueField = DatabaseObjects.Columns.ID;
                                cmbDepartment.TextField = DatabaseObjects.Columns.Title;
                                cmbDepartment.DataSource = department.Where(i => i.CompanyIdLookup == company.ID);
                                cmbDepartment.DataBind();
                            }
                            if (companyDivision != null)
                            {
                                companydivisionid = Convert.ToString(companyDivision.ID);
                                companydivisiontitle = Convert.ToString(companyDivision.Title);
                                division.Add(companydivisionid);
                            }

                            //multipleDepartment += "<span title='" + companyTitle + " > " + companydivisiontitle + " > " + activeDepartment.Title + "' id='" + activeDepartment.ID + "'>" + companydivisiontitle + " > "  + activeDepartment.Title + "</span>";
                            //multipleDepartment += "<span title='" + companydivisiontitle + " > " + activeDepartment.Title + "' id='" + activeDepartment.ID + "'>" + companydivisiontitle + " > " + activeDepartment.Title + "</span>";
                            // multipleDepartment += "<span title='" + company.Title + " > " + companyDivision.Title + " > " + activeDepartment.Title + "' id='" + activeDepartment.ID + "'>" + activeDepartment.Title + "</span>";
                            if (enableDivision)
                                multipleDepartment += "<span title='" + companydivisiontitle + " > " + activeDepartment.Title + "' id='" + activeDepartment.ID + "'>" + companydivisiontitle + " > " + activeDepartment.Title + "</span>";
                            else
                                multipleDepartment += "<span title='" + activeDepartment.Title + "' id='" + activeDepartment.ID + "'>" + activeDepartment.Title + "</span>";
                            hiddenFieldVal.Add(string.Format("{0};#{1};#{2}", activeDepartment.ID, activeDepartment.Title, companyDivision.Title));
                        }
                    });
                    multipleDepartmentViewDiv.InnerHtml = multipleDepartment;
                    Titles = Titles.Distinct().ToList();
                    if(hiddenFieldVal.Count > 0)
                        hdnDepartments.Set("selectedDepartments", string.Join(";#", hiddenFieldVal));

                    List<Company> comanylist = companies.Where(x => !x.Deleted).ToList();
                    cmbCompany.ValueField = DatabaseObjects.Columns.Title;
                    cmbCompany.TextField = DatabaseObjects.Columns.Title;
                    cmbCompany.Value = string.Join(",", Titles);
                    cmbCompany.DataSource = comanylist;
                    cmbCompany.DataBind();

                    List<CompanyDivision> listCompanyDivision = companyDivisionManager.Load(x => x.Deleted == false);
                    division = division.Distinct().ToList();
                    cmbDivision.ValueField = DatabaseObjects.Columns.ID;
                    cmbDivision.TextField = DatabaseObjects.Columns.Title;
                    cmbDivision.Value = string.Join(",", division); //companyDivisionManager.Get(x => x.ID.Equals(activeDepartment.DivisionIdLookup)).Title;
                    cmbDivision.DataSource = listCompanyDivision;
                    cmbDivision.DataBind();
                    DropDownEdit.Value = value;
                    DropDownEdit.Text = SetDepartmentLogic(value);
                    //cmbDepartment.SelectedItem.Value=value;
                    //gdDepartmentOnly.DataBind();
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(value))
                    gdDepartmentOnly.Selection.SetSelectionByKey(value, true);
            }
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('Please select an asset');", true);
 //           ScriptManager.RegisterStartupScript(this, GetType(), "AddAll", string.Format("addAllDepartment('{0}')", this.ClientID), true);
            //ScriptManager.RegisterStartupScript(this, GetType(), "DoneClick", "function(s, e){ editDepartmentDone(s, e, '" + this.ClientID + "'); }", true);

        }
        public string SetDepartmentLogic(string value)
        {
            DepartmentManager deptManager = new DepartmentManager(context);
            CompanyManager objCompanyManager = new CompanyManager(context);
            CompanyDivisionManager companyDivisionManager = new CompanyDivisionManager(context);
            List<string> list = new List<string>();
            List<string> listValue = UGITUtility.SplitString(value, ",").ToList();
            string valueDivision = string.Empty;
            string valueCompany = string.Empty;
            if (listValue != null && listValue.Count > 0)
                listValue = listValue.Where(x => !string.IsNullOrEmpty(x)).Select(y => y).Distinct().ToList();

            listValue.ForEach(y =>
            {
                Department activeDepartment = deptManager.Get(x => x.ID.Equals(Convert.ToInt64(y)));
                if (activeDepartment != null)
                {
                    if (activeDepartment.CompanyIdLookup != null)
                    {
                        var companies = objCompanyManager.Load(x => !x.Deleted);
                        if (companies.Count > 1)
                        {
                            valueCompany = objCompanyManager.Get(x => x.ID.Equals(activeDepartment.CompanyIdLookup)).Title;
                            showcompanyname = true;
                        }
                    }
                    if (activeDepartment.DivisionIdLookup != null)
                    {
                        //valueDivision = companyDivisionManager.Get(x => x.ID.Equals(activeDepartment.DivisionIdLookup)).ID.ToString();
                        valueDivision = companyDivisionManager.Get(x => x.ID.Equals(activeDepartment.DivisionIdLookup)).Title;
                    }
                    if (valueDivision != string.Empty && valueCompany != string.Empty)
                    {
                        if (showcompanyname)
                            list.Add(valueCompany + " > " + valueDivision + " > " + activeDepartment.Title);
                        else
                            list.Add(valueDivision + " > " + activeDepartment.Title);
                    }
                    else if (!showcompanyname)
                    {
                        if (enableDivision && !string.IsNullOrEmpty(valueDivision))
                            list.Add(valueDivision + " > " + activeDepartment.Title);
                        else
                            list.Add(activeDepartment.Title);
                    }
                    else
                    {
                        list.Add(activeDepartment.Title);
                    }
                }
                //list.Add(valueCompany + " > " + valueDivision + " > " + activeDepartment.Title);
            });
            return string.Join(";", list);
        }
        protected void gdDepartmentOnly_DataBinding(object sender, EventArgs e)
        {

        }

        private void SetCookie(string Name, string Value)
        {
            UGITUtility.CreateCookie(Response, Name, Value);
        }
    }

}

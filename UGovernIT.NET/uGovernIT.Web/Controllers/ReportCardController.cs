using DevExpress.Utils.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using uGovernIT.Util.Log;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;
using static DevExpress.Utils.MVVM.Internal.ILReader;
using uGovernIT.Web.Models;

namespace uGovernIT.Web.Controllers
{
    [RoutePrefix("api/ReportCard")]
    public class ReportCardController : ApiController
    {
        private ApplicationContext _applicationContext = null;
        private List<ReportMenu> lstReportMenu = null; 
        private ReportMenuManager _reportMenuManager = null;
        private ConfigurationVariableManager _configurationVariableManager;
        public ReportCardController()
        {
            _applicationContext = HttpContext.Current.GetManagerContext();
            _reportMenuManager = new ReportMenuManager(_applicationContext);
            _configurationVariableManager = new ConfigurationVariableManager(_applicationContext);
        }

        [Route("GetReports")]
        [HttpGet]
        public IHttpActionResult GetReports(string ShowCustomReports, string SelectedReport, string SelectedCategory)
        {
            DashboardManager dashboardManager = new DashboardManager(_applicationContext);
            ModuleViewManager moduleViewManager = new ModuleViewManager(_applicationContext);
            List<UGITModule> lstModule = moduleViewManager.LoadAllModule().Where(x=>x.EnableModule == true).ToList();

            lstReportMenu = new List<ReportMenu>();
            try
            {
                if (Convert.ToBoolean(ShowCustomReports) == true)
                {
                    lstReportMenu = _reportMenuManager.Load(x => x.Deleted != true);

                    //if (!string.IsNullOrEmpty(SelectedCategory) && !SelectedCategory.EqualsIgnoreCase("--All--"))
                    //{

                    //}
                    foreach (ReportMenu reportMenu in lstReportMenu)
                    {
                        var module = lstModule.Where(x => x.ModuleName == reportMenu.ModuleNameLookup).FirstOrDefault();
                        reportMenu.ModuleShortName = reportMenu.Category;
                        //reportMenu.ImageUrl = GetImageUrl(reportMenu.SubCategory);
                        reportMenu.LongTitle = reportMenu.Title;
                        reportMenu.Title = UGITUtility.TruncateWithEllipsis(reportMenu.Title, 15);
                    }
                }
                
                foreach (UGITModule module in lstModule)
                {
                    DataRow moduleRow = UGITUtility.ObjectToData(module).Rows[0];
                    ReportMenu reportMenu = new ReportMenu();
                    string ModuleName = module.ModuleName;
                    int reportCount = 0;
                    if (!string.IsNullOrEmpty(ModuleName))
                    {     
                        /*
                        if (moduleRow != null && UGITUtility.StringToBoolean(moduleRow[DatabaseObjects.Columns.ShowBottleNeckChart]) && Convert.ToBoolean(ShowCustomReports) == true)
                        {
                            //menu.Items.Add("Bottleneck Chart", "BottleNeckChart", "/Content/images/pie-chart.svg");
                            string moduleRowTitle = Convert.ToString(moduleRow[DatabaseObjects.Columns.Title]);
                            lstReportMenu.Add(new ReportMenu()
                            {
                                ID = reportCount++,
                                Title = "Bottleneck Chart",
                                Name = "BottleNeckChart",
                                ModuleNameLookup = ModuleName,
                                ImageUrl = "/Content/images/chart.jpg",
                                RouteUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=workflowbottleneck&moduleName=" + ModuleName + "&moduletitle=" + moduleRowTitle),
                                ModuleShortName = module.ShortName,
                                TenantID = _applicationContext.TenantID
                            });
                        }
                        */
                        if (!string.IsNullOrEmpty(ModuleName) && !ModuleName.EqualsIgnoreCase("all") && string.IsNullOrEmpty(SelectedReport) && (string.IsNullOrEmpty(SelectedCategory) || SelectedCategory.EqualsIgnoreCase("--All--")))
                        {
                            string moduleId = Convert.ToString(moduleViewManager.GetModuleIdByName(ModuleName));
                            List<Dashboard> dashboardTable = dashboardManager.Load($"{DatabaseObjects.Columns.DashboardType}={(int)DashboardType.Query} and {DatabaseObjects.Columns.IsActivated}={1} and {DatabaseObjects.Columns.DashboardModuleMultiLookup} like '%{moduleId}%'");
                            
                            if (dashboardTable != null && dashboardTable.Count > 0)
                            {                                
                                foreach (Dashboard rowItem in dashboardTable)
                                {
                                    var moduleNameSplit = (rowItem.Title + Constants.Separator + Convert.ToString(rowItem.ID)).Split(';');
                                    //menu.Items.Add(UGITUtility.TruncateWithEllipsis(rowItem.Title, 25), rowItem.Title + Constants.Separator + Convert.ToString(rowItem.ID), "/Content/images/executive-summary.png");
                                    lstReportMenu.Add(new ReportMenu()
                                    {
                                        ID = reportCount++,
                                        Title = UGITUtility.TruncateWithEllipsis(rowItem.Title, 15),
                                        LongTitle = rowItem.Title,
                                        Name = rowItem.Title + Constants.Separator + Convert.ToString(rowItem.ID),
                                        ModuleNameLookup = ModuleName,
                                        ImageUrl = GetImageUrl(rowItem.SubCategory),
                                        RouteUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=report&moduleName=" + ModuleName + "&dashboardId=" + moduleNameSplit[1].Substring(1)),
                                        ModuleShortName = rowItem.SubCategory,
                                        TenantID = _applicationContext.TenantID
                                    });
                                }
                            }                            
                        }                        
                    }
                }

                if (!string.IsNullOrEmpty(SelectedReport))
                {
                    Dashboard dashboardTable = dashboardManager.Load($"{DatabaseObjects.Columns.ID} = {SelectedReport}").FirstOrDefault();
                    if (dashboardTable != null)
                    {
                        lstReportMenu.Add(new ReportMenu()
                        {
                            Title = UGITUtility.TruncateWithEllipsis(dashboardTable.Title, 15),
                            LongTitle = dashboardTable.Title,
                            Name = dashboardTable.Title + Constants.Separator + Convert.ToString(dashboardTable.ID),
                            ImageUrl = GetImageUrl(dashboardTable.SubCategory),
                            RouteUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=report&dashboardId=" + dashboardTable.ID),
                            TenantID = _applicationContext.TenantID,
                            ModuleShortName = dashboardTable.SubCategory,
                        });
                    }
                }
                else if (!string.IsNullOrEmpty(SelectedCategory) && !SelectedCategory.EqualsIgnoreCase("--All--"))
                {
                    lstReportMenu = lstReportMenu.Where(o => o.Category != null && o.Category.Equals(SelectedCategory)).ToList();
                    //List<Dashboard> dashboardTables = dashboardManager.Load($"({DatabaseObjects.Columns.CategoryName} = '{SelectedCategory}' OR {DatabaseObjects.Columns.SubCategory} = '{SelectedCategory}') AND {DatabaseObjects.Columns.IsActivated} = 1");
                    List<Dashboard> dashboardTables = dashboardManager.Load($"({DatabaseObjects.Columns.SubCategory} = '{SelectedCategory}') AND {DatabaseObjects.Columns.IsActivated} = 1");
                    if (dashboardTables != null && dashboardTables.Count > 0)
                    {
                        foreach (var item in dashboardTables)
                        {
                            lstReportMenu.Add(new ReportMenu()
                            {
                                Title = UGITUtility.TruncateWithEllipsis(item.Title, 15),
                                LongTitle = item.Title,
                                Name = item.Title + Constants.Separator + Convert.ToString(item.ID),
                                ImageUrl = GetImageUrl(SelectedCategory),
                                RouteUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=report&dashboardId=" + item.ID),
                                TenantID = _applicationContext.TenantID,
                                ModuleShortName = SelectedCategory,
                            });
                        }
                    }
                }

                if (lstReportMenu != null)
                {
                    lstReportMenu = lstReportMenu.OrderBy(x => x.Title).ToList();
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in ReportCardController GetReports: " + ex);
                //ULog.WriteLog($"Error in ReportCardController, {ex}");
            }
            string jsonreportmenu = JsonConvert.SerializeObject(lstReportMenu);
            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(jsonreportmenu, Encoding.UTF8, "application/json");
            return ResponseMessage(response);
        }
        [Route("GetAllReports")]
        [HttpGet]
        public IHttpActionResult GetAllReports(string SelectedReport, string SelectedCategory, string SelectedReportType)
        {
            DashboardManager dashboardManager = new DashboardManager(_applicationContext);
            ModuleViewManager moduleViewManager = new ModuleViewManager(_applicationContext);
            List<UGITModule> lstModule = moduleViewManager.LoadAllModule().Where(x => x.EnableModule == true).ToList();
            List<Dashboard> dashboardTables = dashboardManager.Load($"{DatabaseObjects.Columns.IsActivated} = 1").ToList();
            lstReportMenu = new List<ReportMenu>();
            try
            {
                //  In 'ReportMenu' table add the category name same as the subcategory defined in 'Config_Dashboard_DashboardPanels'.
                lstReportMenu = _reportMenuManager.Load(x => x.Deleted != true).ToList();
                lstReportMenu.ForEach(o =>
                {
                    o.ModuleShortName = o.Category;
                    o.LongTitle = o.Title;
                    o.Title = UGITUtility.TruncateWithEllipsis(o.Title, 15);
                    o.ImageUrl = GetImageUrl(o.Category);
                });

                if (!string.IsNullOrEmpty(SelectedReport) && !string.IsNullOrWhiteSpace(SelectedReportType))
                {
                    //lstReportMenu = lstReportMenu.Where(x => x.ID == SelectedReport);
                    if (SelectedReportType.Equals("queryreport"))
                    {
                        lstReportMenu = new List<ReportMenu>();
                        var dashboardTable = dashboardTables.Where(x => x.ID == long.Parse(SelectedReport)).FirstOrDefault();
                        if (dashboardTable != null)
                        {
                            lstReportMenu.Add(new ReportMenu()
                            {
                                Title = UGITUtility.TruncateWithEllipsis(dashboardTable.Title, 15),
                                LongTitle = dashboardTable.Title,
                                Name = dashboardTable.Title + Constants.Separator + Convert.ToString(dashboardTable.ID),
                                ImageUrl = GetImageUrl(dashboardTable.SubCategory),
                                RouteUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=report&dashboardId=" + dashboardTable.ID),
                                TenantID = _applicationContext.TenantID,
                                ModuleShortName = dashboardTable.SubCategory,
                            });
                        }
                    }
                    else
                    {
                        lstReportMenu = lstReportMenu.Where(x => x.ID == long.Parse(SelectedReport)).ToList();
                    }
                }
                else if (!string.IsNullOrEmpty(SelectedCategory) && !SelectedCategory.EqualsIgnoreCase("--All--"))
                {
                    if (!SelectedCategory.EqualsIgnoreCase("other"))
                    {
                        lstReportMenu = lstReportMenu.Where(o => o.Category != null && o.Category.Equals(SelectedCategory)).ToList();
                        dashboardTables = dashboardTables.Where(x => x.SubCategory == SelectedCategory).ToList();
                        if (dashboardTables != null && dashboardTables.Count > 0)
                        {
                            foreach (var item in dashboardTables)
                            {
                                lstReportMenu.Add(new ReportMenu()
                                {
                                    Title = UGITUtility.TruncateWithEllipsis(item.Title, 15),
                                    LongTitle = item.Title,
                                    Name = item.Title + Constants.Separator + Convert.ToString(item.ID),
                                    ImageUrl = GetImageUrl(SelectedCategory),
                                    RouteUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=report&dashboardId=" + item.ID),
                                    TenantID = _applicationContext.TenantID,
                                    ModuleShortName = SelectedCategory,
                                });
                            }
                        } 
                    }
                    else
                    {
                        lstReportMenu = lstReportMenu.Where(o => o.Deleted == false).ToList();
                    }
                }
                else
                {
                    lstModule.ForEach(module =>
                    {
                        ReportMenu reportMenu = new ReportMenu();
                        string ModuleName = module.ModuleName;
                        int reportCount = 0;
                        if (!string.IsNullOrEmpty(ModuleName))
                        {
                            if (!string.IsNullOrEmpty(ModuleName) && !ModuleName.EqualsIgnoreCase("all") && string.IsNullOrEmpty(SelectedReport) && (string.IsNullOrEmpty(SelectedCategory) || SelectedCategory.EqualsIgnoreCase("--All--")))
                            {
                                string moduleId = Convert.ToString(moduleViewManager.GetModuleIdByName(ModuleName));
                                List<Dashboard> dashboardTable = dashboardTables.Where(x => x.DashboardType == DashboardType.Query && x.DashboardModuleMultiLookup.Contains(moduleId)).ToList();

                                if (dashboardTable != null && dashboardTable.Count > 0)
                                {
                                    foreach (Dashboard rowItem in dashboardTable)
                                    {
                                        lstReportMenu.Add(new ReportMenu()
                                        {
                                            ID = reportCount++,
                                            Title = UGITUtility.TruncateWithEllipsis(rowItem.Title, 15),
                                            LongTitle = rowItem.Title,
                                            Name = rowItem.Title + Constants.Separator + Convert.ToString(rowItem.ID),
                                            ModuleNameLookup = ModuleName,
                                            ImageUrl = GetImageUrl(rowItem.SubCategory),
                                            RouteUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=report&moduleName=" + ModuleName + "&dashboardId=" + Convert.ToString(rowItem.ID)),
                                            ModuleShortName = rowItem.SubCategory,
                                            TenantID = _applicationContext.TenantID
                                        });
                                    }
                                }
                            }
                        }
                    });
                }

                if (lstReportMenu != null)
                {
                    lstReportMenu = lstReportMenu.OrderBy(x => x.Title).ToList();
                }
            }
            catch (Exception ex)
            {
                //ULog.WriteLog($"Error in ReportCardController, {ex}");
                ULog.WriteException($"An Exception Occurred in GetAllReports: " + ex);
            }
            string jsonreportmenu = JsonConvert.SerializeObject(lstReportMenu);
            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(jsonreportmenu, Encoding.UTF8, "application/json");
            return ResponseMessage(response);
        }
        public string GetImageUrl(string moduleName)
        {
            string reportImages = _configurationVariableManager.GetValue(ConfigConstants.ReportImages);
            Dictionary<string, string> images = UGITUtility.GetCustomProperties(reportImages, Constants.Separator, true, true);
            if (!string.IsNullOrWhiteSpace(moduleName))
            {
                if (images != null && images.ContainsKey(moduleName))
                {
                    return images[moduleName];
                }
            }
            return "/Content/images/chart3.jpg";
        }
    }
}
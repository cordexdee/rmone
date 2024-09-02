using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml;
using uGovernIT.DAL.Store;
using uGovernIT.Util.Cache;
using uGovernIT.Util.Log;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Manager.Managers
{
    public interface IDashboardPanelViewManager : IManagerBase<DashboardPanelView>
    {

    }

    public class DashboardPanelViewManager : ManagerBase<DashboardPanelView>, IDashboardPanelViewManager
    {
        public DashboardPanelViewManager(ApplicationContext context) : base(context)
        {
            store = new DashboardPanelViewStore(this.dbContext);
        }

        public DashboardPanelView LoadViewByName(string viewName)
        {
            DashboardPanelView item = CacheHelper<DashboardPanelView>.Get(viewName, dbContext.TenantID);
            if (item == null)
            {
                item = store.Get(x => x.Title == viewName);
                LoadView(item);
                if (item != null)
                    CacheHelper<DashboardPanelView>.AddOrUpdate(viewName, dbContext.TenantID, item);
            }
            return item;
        }

        public DashboardPanelView LoadViewByID(int viewID)
        {
            return LoadViewByID(viewID, true);
        }

        public DashboardPanelView LoadViewByID(int viewID, bool useCache)
        {
            if (viewID <= 0) return null;

            DashboardPanelView item = null;
            if (useCache)
            {
                item = CacheHelper<DashboardPanelView>.Get(viewID.ToString(), dbContext.TenantID);
                if (item == null)
                {
                    item = store.Get(x => x.ID == viewID);
                    if (item != null)
                    {
                        LoadView(item);
                        CacheHelper<DashboardPanelView>.AddOrUpdate(viewID.ToString(), dbContext.TenantID, item);
                    }
                }
            }
            else
            {

                item = store.Get(x => x.ID == viewID);
                if (item != null)
                    LoadView(item);
            }
            return item;
        }

        private DashboardPanelView LoadView(DashboardPanelView item)
        {
            if (item == null)
                return item;


            DashboardPanelView dashboardPanelView = item;

            UserProfileManager uManager = new UserProfileManager(this.dbContext);

            if (!String.IsNullOrEmpty(dashboardPanelView.AuthorizedToViewUsers))
            {
                string[] arrUsers = dashboardPanelView.AuthorizedToViewUsers.Split(new string[] { Constants.Separator6 }, StringSplitOptions.RemoveEmptyEntries);
                if (arrUsers.Length > 0)
                {
                    if (dashboardPanelView.AuthorizedToView == null)
                        dashboardPanelView.AuthorizedToView = new List<UserProfile>();

                    foreach (string user in arrUsers)
                    {
                        UserProfile spUser = uManager.GetUserById(user);
                        if (spUser != null)
                            dashboardPanelView.AuthorizedToView.Add(spUser);
                        //else
                        //{
                        //    SPGroup group = UserProfile.GetGroupByName(user);
                        //    if (group != null)
                        //        viewHelper.AuthorizedToView.Add(new UserInfo(group.ID, group.LoginName, true));
                        //}

                    }
                }
            }

            string viewData = dashboardPanelView.DashboardPanelInfo;
            if (!string.IsNullOrEmpty(viewData))
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(viewData);

                var view = new DashboardViewProperties();
                if (dashboardPanelView.ViewType == "Indivisible Dashboards")
                {
                    IndivisibleDashboardsView iView = new IndivisibleDashboardsView();
                    view = (IndivisibleDashboardsView)uHelper.DeSerializeAnObject(xmlDocument, iView);
                }
                else if (dashboardPanelView.ViewType == "Common Dashboards")
                {
                    CommonDashboardsView sView = new CommonDashboardsView();
                    view = (CommonDashboardsView)uHelper.DeSerializeAnObject(xmlDocument, sView);
                }
                else if (dashboardPanelView.ViewType == "Super Dashboards")
                {
                    SuperDashboardsView sView = new SuperDashboardsView();
                    view = (SuperDashboardsView)uHelper.DeSerializeAnObject(xmlDocument, sView);
                }
                else if (dashboardPanelView.ViewType == "Side Dashboards")
                {
                    SideDashboardView sView = new SideDashboardView();
                    view = (SideDashboardView)uHelper.DeSerializeAnObject(xmlDocument, sView);
                }
                dashboardPanelView.ViewProperty = view;
            }

            return dashboardPanelView;
        }
     
        public DashboardPanelView Save(DashboardPanelView objDashboardPanelView)
        {
            if (objDashboardPanelView.ID > 0)
            {
                objDashboardPanelView.DashboardPanelInfo = uHelper.SerializeObject(objDashboardPanelView.ViewProperty).OuterXml;
                this.Update(objDashboardPanelView);
            }
            else
            {
                objDashboardPanelView.DashboardPanelInfo = uHelper.SerializeObject(objDashboardPanelView.ViewProperty).OuterXml;
                this.Insert(objDashboardPanelView);
            }
            if(objDashboardPanelView != null)
                CacheHelper<DashboardPanelView>.AddOrUpdate(objDashboardPanelView.Title, this.dbContext.TenantID, objDashboardPanelView);
            return objDashboardPanelView;
        }

        public void RefreshCache()
        {
            List<DashboardPanelView> lstDashboardPanelView = this.Load();
            if (lstDashboardPanelView == null || lstDashboardPanelView.Count == 0)
                return;
            DashboardPanelView dPanelView = null;
            foreach (DashboardPanelView dashboardPanelView in lstDashboardPanelView)
            {
                if (string.IsNullOrEmpty(dashboardPanelView.Title))
                    continue;
                dPanelView = CacheHelper<DashboardPanelView>.Get(dashboardPanelView.Title.ToLower(), this.dbContext.TenantID);

                if (dPanelView == null)
                {
                    dPanelView = this.LoadViewByName(dashboardPanelView.Title);
                    if (dPanelView != null)
                    {
                        CacheHelper<DashboardPanelView>.AddOrUpdate(dashboardPanelView.Title, this.dbContext.TenantID, dPanelView);
                    }
                }
            }                    

        }
    }    
}

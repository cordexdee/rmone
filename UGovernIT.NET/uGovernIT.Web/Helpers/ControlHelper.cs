using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using uGovernIT.Util.Log;

namespace uGovernIT.Web.Helpers
{
    public static class ControlHelper
    {
        public static Control addUGITControl(this ControlCollection parent, Page page, string controlName, string controlID = "")
        {
            Type type = Type.GetType($"uGovernIT.Web.{controlName}", false, true);
            if (type == null)
                type = Type.GetType($"uGovernIT.Web.ControlTemplates.{controlName}", false, true);

            if (type != null)
            {
                string ctrlFullName = type.FullName.Replace($"{type.Assembly.GetName().Name}.", "").Replace(".", "/");
                try
                {
                    Control ctrl = page.LoadControl($"~/{ctrlFullName}.ascx");
                    if (!string.IsNullOrWhiteSpace(controlID))
                        ctrl.ID = controlID;
                    parent.Add(ctrl);
                    return ctrl;
                }
                catch (Exception ex)
                {
                    ULog.WriteException(ex, string.Format("{0} control not loaded.", controlName));
                }
            }
            return null;
        }

        public static Control loadUGITControl(this Page page, string controlName)
        {
            Type type = Type.GetType($"uGovernIT.Web.{controlName}", false, true);
            if (type == null)
                type = Type.GetType($"uGovernIT.Web.ControlTemplates.{controlName}", false, true);

            if (type != null)
            {
                string ctrlFullName = type.FullName.Replace($"{type.Assembly.GetName().Name}.", "").Replace(".", "/");
                try
                {
                    Control ctrl = page.LoadControl($"~/{ctrlFullName}.ascx");
                    return ctrl;
                }
                catch (Exception ex)
                {
                    ULog.WriteException(ex, string.Format("{0} control not loaded.", controlName));
                }
            }
            return null;
        }
    }
}
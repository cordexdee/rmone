using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using DevExpress.Web;
using uGovernIT.Utility.DockPanels;

namespace uGovernIT.Web.ControlTemplates.DockPanels
{
    public interface IDockPanel
    {
        bool NeedData { get; set; }
        string PageID { get; set; }
        bool Editable { get; set; }
        DockPanelSetting DockSetting { get; set; }
        Control LoadControl(Page page);
    }
}

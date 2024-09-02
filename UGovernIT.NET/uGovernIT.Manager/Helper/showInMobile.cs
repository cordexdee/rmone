using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using uGovernIT.Manager.Core;
using uGovernIT.Utility;

namespace uGovernIT.Manager.Helper
{
    
    public class showInMobile
    {
        protected Ticket TicketRequest;
        protected DataRow currentTicket;
        ApplicationContext context = null;
        public showInMobile(ApplicationContext _context)
        {
            context = _context;
            

        }

        public bool ShowTabs(int TabId, DataRow currentTicket, Ticket TicketRequest)
        {
            bool Hidetab = true;
            List<ModuleFormTab> lstFormTab = new List<ModuleFormTab>();
            //ApplicationContext context = HttpContext.Current.GetManagerContext();

            Hidetab = true;
            List<ModuleFormLayout> alllayoutItems = TicketRequest.Module.List_FormLayout.Where(x => x.TabId == TabId && x.FieldSequence > 0).OrderBy(x => x.FieldSequence).ToList();

            List<ModuleFormLayout> layoutItems = new List<ModuleFormLayout>();
            foreach (ModuleFormLayout removetabField in alllayoutItems.ToList())
            {
               
                if (string.IsNullOrEmpty(removetabField.SkipOnCondition) ||
                    FormulaBuilder.EvaluateFormulaExpression(context, removetabField.SkipOnCondition, currentTicket) == false)
                {
                    layoutItems.Add(removetabField);
                }
            }
            foreach (ModuleFormLayout tabField in layoutItems)
            {
                //fieldName == "#GroupStart#" || fieldName == "#GroupEnd#"
                if (tabField.FieldName != "#GroupStart#" && tabField.FieldName != "#GroupEnd#")
                {
                    if (tabField.ShowInMobile == false && HttpContext.Current.Request.Browser.IsMobileDevice == true)
                    {
                        //Hidetab = true;
                    }
                    else
                    {
                        Hidetab = false;
                    }
                }

            }

            return Hidetab;
        }


        public bool showFields(ModuleFormLayout tabField)
        {
            if (tabField.ShowInMobile == false && HttpContext.Current.Request.Browser.IsMobileDevice == true)
            {
                return true;
            }
            else
            {
                return false;
            }   
            
        }

        /// <summary>
        /// Check if Tab is visible in Mobile device.
        /// </summary>        
        public bool showTabinMobile(ModuleFormTab tab)
        {
            if (tab.ShowInMobile == true && HttpContext.Current.Request.Browser.IsMobileDevice == true)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

    }
}

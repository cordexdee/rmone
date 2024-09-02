using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;
using uGovernIT.DAL;
using uGovernIT.DAL.Store;

namespace uGovernIT.Manager
{
    public class ModuleFormLayoutManager : ManagerBase<ModuleFormLayout>, IModuleFormLayoutManager
    {
        ModuleFormLayout moduleFormLayout;
        public ModuleFormLayoutManager(ApplicationContext context) : base(context)
        {
            store = new ModuleFormLayoutStore(this.dbContext);
        }
        public ModuleFormLayout AddOrUpdate(ModuleFormLayout item)
        {
            if (item.ID > 0)
                this.Update(item);
            else
                this.Update(item);
            return item;
        }

        public void UpdateSequence(string firstID, string secondID, string moduleName, string tabID)
        {
            List<ModuleFormLayout> drFormLayout = Load(x => x.ModuleNameLookup == moduleName && x.TabId == Convert.ToInt32(tabID));
            if (drFormLayout != null)
            {
                drFormLayout = drFormLayout.OrderBy(x => x.FieldSequence).ToList();
                ModuleFormLayout drFirstItem = drFormLayout.FirstOrDefault(m => m.ID == UGITUtility.StringToLong(firstID.Trim()));
                ModuleFormLayout drSecondItem = drFormLayout.FirstOrDefault(m => m.ID == UGITUtility.StringToLong(secondID.Trim()));
                if(drFormLayout != null && drFirstItem != null && drSecondItem != null)
                    UpdateSequence(drFormLayout, drFirstItem, drSecondItem, moduleName);
            }
        }

        public void UpdateSequence(List<ModuleFormLayout> drFormLayout, ModuleFormLayout drFirstItem, ModuleFormLayout drSecondItem, string moduleName)
        {
            int firstSeq = UGITUtility.StringToInt(drFirstItem.FieldSequence);
            int secondSeq = UGITUtility.StringToInt(drSecondItem.FieldSequence);
            string firstID = Convert.ToString(drFirstItem.ID);
            int groupEnd = firstSeq;
            // if group or table field dropped.
            if (drFirstItem.FieldName == "#GroupStart#" || drFirstItem.FieldName == "#TableStart#")
            {
                foreach (ModuleFormLayout dr in drFormLayout)
                {
                    if (UGITUtility.StringToInt(dr.FieldSequence) >= firstSeq
                                && Convert.ToString(dr.FieldName) == Convert.ToString(drFirstItem.FieldName).Replace("Start", "End"))
                    {
                        groupEnd = UGITUtility.StringToInt(dr.FieldSequence);
                        secondSeq = ReCalculateDroppingArea(firstSeq, secondSeq, drFormLayout);
                        break;
                    }
                }
            }

            foreach (ModuleFormLayout dr in drFormLayout)
            {
                if (firstSeq >= secondSeq)
                {
                    if (UGITUtility.StringToInt(dr.FieldSequence) <= groupEnd && UGITUtility.StringToInt(dr.FieldSequence) >= firstSeq)
                    {
                        if (firstSeq == secondSeq && Convert.ToString(dr.ID) != firstID)
                        {
                            dr.FieldSequence = UGITUtility.StringToInt(dr.FieldSequence) + 1;
                        }
                        else
                        {
                            dr.FieldSequence = UGITUtility.StringToInt(dr.FieldSequence) + secondSeq - firstSeq;
                        }

                    }
                    else if (firstSeq > UGITUtility.StringToInt(dr.FieldSequence) && UGITUtility.StringToInt(dr.FieldSequence) >= secondSeq)
                    {
                        dr.FieldSequence = UGITUtility.StringToInt(dr.FieldSequence) + groupEnd - firstSeq + 1;
                    }
                    else if(dr.FieldSequence > firstSeq && dr.FieldSequence > secondSeq)
                    {
                        dr.FieldSequence = UGITUtility.StringToInt(dr.FieldSequence) + 1;
                    }
                }
                else
                {
                    //if dropped within group.
                    if (groupEnd > firstSeq && groupEnd >= secondSeq)
                    {
                        return;
                    }

                    if (UGITUtility.StringToInt(dr.FieldSequence) <= groupEnd && UGITUtility.StringToInt(dr.FieldSequence) >= firstSeq)
                    {
                        int seqNo = UGITUtility.StringToInt(dr.FieldSequence) + (secondSeq - firstSeq) - (groupEnd - firstSeq);

                        if (firstSeq == groupEnd)
                        {
                            seqNo -= 1;
                        }
                        dr.FieldSequence = seqNo;

                    }
                    else if (secondSeq >= UGITUtility.StringToInt(dr.FieldSequence) && UGITUtility.StringToInt(dr.FieldSequence) > firstSeq)
                    {
                        if (secondSeq > UGITUtility.StringToInt(dr.FieldSequence) || (firstSeq != groupEnd && secondSeq == UGITUtility.StringToInt(dr.FieldSequence)))
                        {
                            dr.FieldSequence = UGITUtility.StringToInt(dr.FieldSequence) - (groupEnd - firstSeq + 1);
                        }
                    }
                }
            }
            List<ModuleFormLayout> dataView = drFormLayout.OrderBy(x => x.FieldSequence).ToList();
            StringBuilder query = new StringBuilder();
            int ctr = 0;
            foreach (ModuleFormLayout dr in dataView)
            {
                moduleFormLayout = new ModuleFormLayout();
                moduleFormLayout.ID = Convert.ToInt64(dr.ID);
                moduleFormLayout.TabId = UGITUtility.StringToInt(dr.TabId);
                moduleFormLayout.Title = Convert.ToString(dr.Title);
                moduleFormLayout.ModuleNameLookup = moduleName;
                moduleFormLayout.FieldDisplayName = Convert.ToString(dr.FieldDisplayName);
                moduleFormLayout.FieldDisplayWidth = UGITUtility.StringToInt(dr.FieldDisplayWidth);
                moduleFormLayout.FieldName = Convert.ToString(dr.FieldName);
                moduleFormLayout.ColumnType = Convert.ToString(dr.ColumnType);
                moduleFormLayout.CustomProperties = Convert.ToString(dr.CustomProperties);
                moduleFormLayout.ShowInMobile = Convert.ToBoolean(dr.ShowInMobile);
                moduleFormLayout.FieldSequence = UGITUtility.StringToInt(++ctr);
                moduleFormLayout.SkipOnCondition = dr.SkipOnCondition;
                Update(moduleFormLayout);
            }
        }

        public int ReCalculateDroppingArea(int firstSeq, int secondSeq, List<ModuleFormLayout> drFormLayout)
        {
            int result = secondSeq;
            foreach (ModuleFormLayout dr in drFormLayout)
            {
                if (UGITUtility.StringToInt(dr.FieldSequence) <= secondSeq)
                {
                    if ((Convert.ToString(dr.FieldName) == "#GroupStart#" || Convert.ToString(dr.FieldName) == "#TableStart#"))
                    {
                        if (firstSeq > secondSeq)
                        {
                            result = UGITUtility.StringToInt(dr.FieldSequence);
                        }
                        else
                        {
                            result = UGITUtility.StringToInt(dr.FieldSequence) - 1;
                        }
                    }
                }
            }
            return result;
        }
    }
    public interface IModuleFormLayoutManager : IManagerBase<ModuleFormLayout>
    {
         ModuleFormLayout AddOrUpdate(ModuleFormLayout item);
    }
}

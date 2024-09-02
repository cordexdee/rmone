using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Util.Log;
using uGovernIT.Utility;

namespace uGovernIT.DataTransfer.Infratructure
{
    public class MappedItemList
    {
        List<MappedItem> items;
        public string Name;

        public MappedItemList(string name)
        {
            items = new List<MappedItem>();
            this.Name = name;
        }


        public void Add(MappedItem item)
        {
            if (item == null)
                return;

            if (items.Exists(x => x.SourceID == item.SourceID))
            {
                ULog.WriteLog($"source id {item.SourceID} exist in {this.Name}");
            }
            else
            {
                items.Add(item);
            }
        }

        public MappedItem Get(string sourceid = null, string targetid = null)
        {
            if (string.IsNullOrWhiteSpace(sourceid) && string.IsNullOrWhiteSpace(targetid))
            {
                return null;
            }

            if (!string.IsNullOrWhiteSpace(sourceid))
            {
                return items.FirstOrDefault(x => x.SourceID == sourceid);
            }

            if (!string.IsNullOrWhiteSpace(targetid))
            {
                return items.FirstOrDefault(x => x.TargetID == targetid);
            }

            return null;
        }

        public string GetSourceID(string targetid)
        {
            if (string.IsNullOrWhiteSpace(targetid))
            {
                return null;
            }
           
            MappedItem item = items.FirstOrDefault(x => x.TargetID == targetid);
            if (item == null)
                return null;

            return item.SourceID;

        }

        public string GetTargetID(string sourceid)
        {
            if (string.IsNullOrWhiteSpace(sourceid))
            {
                return null;
            }

            MappedItem item = items.FirstOrDefault(x => x.SourceID == sourceid);
            if (item == null)
                return null;

            return item.TargetID;
            
        }

        public string GetTargetIDs(List<string> sourceids)
        {
            if (sourceids ==null)
            {
                return null;
            }

           
            List<MappedItem> selectedItems= items.Where(x => sourceids.Contains(x.SourceID)).ToList();
            if (selectedItems == null)
                return null;

            return string.Join(Constants.Separator, selectedItems.Select(x => x.TargetID));
        }

        public List<string> GetTargetIDsInArray(List<string> sourceids)
        {
            if (sourceids == null)
            {
                return null;
            }


            List<MappedItem> selectedItems = items.Where(x => sourceids.Contains(x.SourceID)).ToList();
            if (selectedItems == null)
                return null;

            return selectedItems.Select(x => x.TargetID).ToList();
        }

        public List<MappedItem> GetTargetIDsInMappedArray(List<string> sourceids)
        {
            if (sourceids == null)
            {
                return null;
            }


            List<MappedItem> selectedItems = items.Where(x => sourceids.Contains(x.SourceID)).ToList();
            if (selectedItems == null)
                return null;

            return selectedItems;
        }

        public List<MappedItem> GetAll() {
            return items;
        }
    }
}

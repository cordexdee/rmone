using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace uGovernIT.Utility
{
   
    [Table(DatabaseObjects.Tables.UGITTaskTemplates)]
    [JsonObject(MemberSerialization.OptOut)]
    public class TaskTemplate:DBBaseEntity
    {
        public long ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public long? ProjectLifeCycleLookup { get; set; }
        [NotMapped]
        public List<UGITTask> Tasks { get; set; }

        public static TaskTemplate LoadItem(DataRow item)
        {
            TaskTemplate template = new TaskTemplate();

            template.ID = Convert.ToInt32( item[DatabaseObjects.Columns.ID]);
            template.Title = Convert.ToString(item[DatabaseObjects.Columns.Title]);

            if (UGITUtility.IsSPItemExist(item, DatabaseObjects.Columns.UGITDescription) && Convert.ToString(item[DatabaseObjects.Columns.UGITDescription]) != string.Empty)
                template.Description = Convert.ToString(item[DatabaseObjects.Columns.UGITDescription]);
            if (UGITUtility.IsSPItemExist(item, DatabaseObjects.Columns.ProjectLifeCycleLookup))
            {
                //SPFieldLookupValue lookup = new SPFieldLookupValue(Convert.ToString(item[DatabaseObjects.Columns.ProjectLifeCycleLookup]));
                template.ProjectLifeCycleLookup = Convert.ToInt32(item[DatabaseObjects.Columns.ProjectLifeCycleLookup]);
            }

            return template;
        }
    }
}

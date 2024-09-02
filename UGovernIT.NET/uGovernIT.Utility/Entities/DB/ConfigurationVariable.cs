using System;
using System.Data;
using System.Linq;
using System.ComponentModel.DataAnnotations;using System.ComponentModel.DataAnnotations.Schema;


namespace uGovernIT.Utility
{
    [Table(DatabaseObjects.Tables.ConfigurationVariable)]
   public class ConfigurationVariable:DBBaseEntity
    {
        public long ID { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public string KeyName { get; set; }
        public string KeyValue { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public bool Internal {get; set;}

        public static string ServiceChoiceQuestionPickLists = "ServiceChoiceQuestionPickLists";
        public static string ServiceLookupLists = "ServiceLookupLists";
        public static string EnableBudgetCategoryType = "EnableBudgetCategoryType";
    }
}

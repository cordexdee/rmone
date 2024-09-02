using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace uGovernIT.Utility
{
    public class ApplicationReportEntity
    {
        public string CategoryName { get; set; }
        public List<ApplicationEntity> ApplicationList { get; set; }
    }

    //public class ApplicationEntity
    //{
    //    public int ApplicationId { get; set; }
    //    public string ApplicationName { get; set; }
    //    public List<ModuleRoleEntity> MyProperty { get; set; }
    //}

    public class ApplicationEntity
    {
        public List<string> lstColumnsNames { get; set; }
        public List<string> lstRowsNames { get; set; }
        public List<ModuleRoleData> lstGridData { get; set; }
        public string ControlType { get; set; }
        public String Name { get; set; }
        public String ID { get; set; }
        public string Note { get; set; }
        public string RoleAssignee { get; set; }
        public string RoleAssigneeName { get; set; }
    }

    public class ModuleRoleData
    {
        public string ColumnName { get; set; }
        public string RowName { get; set; }
        public string Value { get; set; }
        public string ID { get; set; }
    }
}

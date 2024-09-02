using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility
{ 

    public class ServiceMatrixData
    {
        /// <summary>
        /// Role list
        /// </summary>
        public List<string> lstColumnsNames { get; set; }
        /// <summary>
        /// Module List
        /// </summary>
        public List<string> lstRowsNames { get; set; }
        public List<RoleModuleData> lstRoleModuleMap { get; set; }
        public List<ServiceData> lstGridData { get; set; }
        public String Name { get; set; }
        public String ID { get; set; }
        public string Note { get; set; }
        public string RoleAssignee { get; set; }
        public string MirrorAccessFromUser { get; set; }
        public string AccessRequestMode { get; set; }
        public string ControlType { get; set; }
        public List<ServiceData> lstMirrorAccess { get; set; }

    }
}

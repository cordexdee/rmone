using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using net.sf.mpxj.reader;
using net.sf.mpxj.mpp;
using net.sf.mpxj;
using net.sf.mpxj.mspdi;
using System.Data;
using System.Reflection;
using uGovernIT.Utility;
namespace uGovernIT.Util.ImportExportMPP
{
    public class ImportManagerMpp
    {
        public string FileName { get; set; }
        public bool DatesOnly { get; set; }
        public bool ImportAssignee { get; set; }
        public bool msexport { get; set; }
        ConfigSetiingMpp setting;
        public ImportManagerMpp(ConfigSetiingMpp config, string strFileName)
        {
            setting = config;
            FileName = strFileName;
           
        }
        public Project importtask()
        {
            Project obj = new Project();
            if (setting.UseMSProject)
            {
                //Assembly myAssembly = null;
                // Check the MSProject assembly is exist on client machine or not.
                // myAssembly = Assembly.Load("Microsoft.Office.Interop.MSProject, Version=14.0.0.0, Culture=Neutral, PublicKeyToken=71e9bce111e9429c");
                MSProjectClass MSProjectClass = new MSProjectClass();
                obj = MSProjectClass.ImportTask(FileName, setting);
            }
            else
            {
                MPXJClass mPXJClass = new MPXJClass();
                //obj = mPXJClass.GetMPXJResult(FileName, setting); //templorary commented

            }
            return obj;
        }
    }
}

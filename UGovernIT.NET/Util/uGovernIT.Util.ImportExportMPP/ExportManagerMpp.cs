using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Reflection;

namespace uGovernIT.Util.ImportExportMPP
{
    public class ExportManagerMpp
    {
        public string FileName { get; set; }
        public bool DatesOnly { get; set; }
        public bool ImportAssignee { get; set; }
        public bool msexport { get; set; }
        ConfigSetiingMpp setting;
        public DataTable dataTable { get; set; }
        public string FullFileName { get; set; }
        Project Project;
        public ExportManagerMpp(ConfigSetiingMpp configSetiingMpp,Project project,string strfilename,string strFullFileName)
        {
            setting = configSetiingMpp;
            Project = project;
            FileName = strfilename;
            FullFileName = strFullFileName;
        }
        public void ExportTask()
        {
           
            if (setting.UseMSProject)
            {
                Assembly myAssembly = null;
                // Check the MSProject assembly is exist on client machine or not.
                // myAssembly = Assembly.Load("Microsoft.Office.Interop.MSProject, Version=14.0.0.0, Culture=Neutral, PublicKeyToken=71e9bce111e9429c");
                if (myAssembly == null)
                    return;
                MSProjectClass mSProjectClass = new MSProjectClass();
                mSProjectClass.MSExportTask(Project, FileName, FullFileName);
               // MSProjectClass MSProjectClass = new MSProjectClass();
               // obj = MSProjectClass.ImportTask(FileName, setting);
            }
            else
            {
                MPXJClass mPXJClass = new MPXJClass();
                mPXJClass.MPXJExportTask(Project, FileName, FullFileName);

            }
        }
    }
}

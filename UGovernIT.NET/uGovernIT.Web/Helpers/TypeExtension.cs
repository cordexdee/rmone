using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
//using uGovernIT.Web.ControlTemplates.DockPanels;

namespace uGovernIT.Web.Helpers
{
    public static class TypeExtensions
    {
        public static List<Type> GetAllDerivedTypes(this Type type)
        {
            return Assembly.GetAssembly(type).GetAllDerivedTypes(type);
        }

        public static List<Type> GetAllDerivedTypes(this Assembly assembly, Type type)
        {
            List<Type> list = new List<Type>();          
            try
            {
                foreach (Type t in assembly.GetTypes())
                {
                    if (t != null && t != type && type.IsAssignableFrom(t))
                        list.Add(t);
                }
            }
            catch (ReflectionTypeLoadException ex)
            {
                StringBuilder sb = new StringBuilder();
                foreach (Exception exSub in ex.LoaderExceptions)
                {
                    sb.AppendLine(exSub.Message);
                    FileNotFoundException exFileNotFound = exSub as FileNotFoundException;
                    if (exFileNotFound != null)
                    {
                        if (!string.IsNullOrEmpty(exFileNotFound.FusionLog))
                        {
                            sb.AppendLine("Fusion Log:");
                            sb.AppendLine(exFileNotFound.FusionLog);
                        }
                    }
                    sb.AppendLine();
                }
                string errorMessage = Convert.ToString(sb);
                foreach (Type t in ex.Types)
                {
                    if (t != null && t != type && type.IsAssignableFrom(t))
                        list.Add(t);
                }
                // Display or log the error based on your application.
            }

            return list;
        }
    }
}
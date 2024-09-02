using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using uGovernIT.DAL;
using uGovernIT.DefaultConfig;

namespace uGovernITDefault
{
    class ScriptProgram
    {
        public static void Execute(string[] args)
        {
            ScriptList();
            ShowHelp();

            //Checks whether args are coming or not.
            //if not then ask for parameters
            args = AskParameters(args);

            // If user enters "back" or "b" go back to main prompt
            if (args.Length > 0 && (args[0] == "back" || args[0] == "b"))
                return;

             Config.LoadConfig(args[0], true);
            //disable enable logging
            if (args.Length >= 3)
            {
                Config.WriteToLogFile = Helper.StringToBoolean(args[2].Trim()); ;
            }

            System.IO.File.Delete(Config.LogFile);

            if (args.Length < 2)
            {
                Log.WriteLog("ERROR: Please specify script name");
                Execute(new string[0]);
                return;
            }

            string scriptName = args[1].Trim().ToLower();
            try
            {
                //Find and call script class using reflection
                Type iscriptType = typeof(Scripts.iScript);
                Type[] allType = iscriptType.Assembly.GetTypes();
                Type selectedScriptType = allType.FirstOrDefault(x => x.Namespace == "uGovernITDefault.Scripts" && x.IsClass && x.Name.ToLower() == scriptName);                

                if (selectedScriptType != null && iscriptType.IsAssignableFrom(selectedScriptType))
                {
                    Log.WriteLog(string.Format("Start: {0} script", selectedScriptType.Name));
                    object classObj = Activator.CreateInstance(selectedScriptType);
                    if (args.Length > 2)
                    {
                        string parameter = args[2];
                        selectedScriptType.InvokeMember("parameter",
                                                        BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetField,
                                                        null, classObj, new object[] { parameter });
                    }

                    selectedScriptType.InvokeMember("Execute", BindingFlags.InvokeMethod, null, classObj, new object[0]);

                    Log.WriteLog(string.Format("End: {0} script", selectedScriptType.Name));
                }
                else
                {
                    Log.WriteLog("ERROR: Invalid Script");
                }

                Execute(new string[0]);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.ResetColor();
                args = new string[0];
                Execute(args);
            }

            Console.ReadKey();
        }
        public static void CallScript()
        {
            try
            {
                //Find and call script class using reflection
                var instances = from t in Assembly.GetExecutingAssembly().GetTypes()
                                where t.GetInterfaces().Contains(typeof(IModule))
                                         && t.GetConstructor(Type.EmptyTypes) != null
                                select Activator.CreateInstance(t) as IModule;
                foreach (var instance in instances)
                {
                    LoadModuleConfiguration(instance);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.ResetColor();
            }
        }
        private static void LoadModuleConfiguration(IModule module)
        {
            //var moduleData = module.Module;
            //object data = module.GetFormTabs();
            //data = module.GetModuleColumns();
            //data = module.GetImpact();

            //data = module.GetModuleDefaultValue();
            //data = module.GetModuleFormLayout();
            //data = module.GetModuleRoleWriteAccess();
            //data = module.GetModuleRequestType();
            //data = module.GetLifeCycleStage();
            //data = module.GetModuleStatusMapping();
            //data = module.GetModuleTaskEmail();
            //data = module.GetACRTypes();
            //data = module.GetDRQRapidTypes();
            //data = module.GetDRQSystemAreas();
            //uGITDAL.InsertItem(data);
        }
        public static string[] AskParameters(string[] args)
        {
            Console.Write("script> ");

            string arguments = Console.ReadLine();
            string[] argAry = arguments.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < argAry.Length; i++)
            {
                if (i < 2)
                    argAry[i] = argAry[i].Trim().ToLower();
            }

            if (argAry.Length <= 0)
            {
                argAry = AskParameters(args);
            }
            else if (argAry[0] == "?")
            {
                Console.WriteLine("Script Names: \n");
                ScriptList();
                ShowHelp();
                argAry = AskParameters(args);
            }
            else if (argAry[0] == "exit")
            {
                Environment.Exit(0);
            }

            return argAry;
        }
        public static void ShowHelp()
        {
            Console.WriteLine("Please Enter:");
            Console.WriteLine("  <Site URL> <script name> [parameter] - to run script");
            Console.WriteLine("  ?  - for list of scripts");
            Console.WriteLine("  exit - to exit console window"); Console.WriteLine("");
            Console.WriteLine("");
        }
        public static void ScriptList()
        {
            List<string> scripts = new List<string>();
            Type iscriptType = typeof(Scripts.iScript);
            Type[] allType = iscriptType.Assembly.GetTypes();
            Type[] scriptsType = allType.Where(x => x.Namespace == "uGovernITConsole.Scripts" && x.IsClass).OrderBy(x => x.Name).ToArray();
            foreach (Type script in scriptsType)
            {
                if (iscriptType.IsAssignableFrom(script))
                {
                    object classObj = Activator.CreateInstance(script);
                    bool isVisible = (bool)script.InvokeMember("isVisible", BindingFlags.InvokeMethod, null, classObj, new object[0]);
                    if (!isVisible)
                        continue;

                    string helpMessage = (string)script.InvokeMember("Help", BindingFlags.InvokeMethod, null, classObj, new object[0]);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(script.Name);
                    Console.ResetColor();
                    Console.WriteLine(string.Format(" - {0}", helpMessage));
                }
            }
            Console.WriteLine();
        }
    }
}

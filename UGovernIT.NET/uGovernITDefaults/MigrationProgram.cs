using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using uGovernIT.DataTransfer.SharePointToDotNet;
using uGovernIT.Manager;
using uGovernIT.Utility;
using uGovernIT.DataTransfer;

namespace UGovernITDefault
{
    public class MigrationProgram
    {
        public static void Execute(string[] args)
        {
            ShowHelp();

            //Checks whether args are coming or not.
            //if not then ask for parameters
            args = AskParameters(args);

            // If user enters "back" or "b" go back to main prompt
            if (args.Length > 0 && (args[0] == "back" || args[0] == "b"))
                return;

            string cmdStr = args[0].Trim().ToLower();

            if (cmdStr == "std" || cmdStr == "dtd")
            {
                string configPath = string.Empty;
                if (args.Length >= 2)
                {
                    configPath = args[1].Trim();
                }

                if (Path.GetExtension(configPath).ToLower() != ".json")
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("configuration file should be in json format.");
                    Console.ResetColor();
                    Execute(args);
                }

                if (!File.Exists(configPath))
                {
                    if (File.Exists(Path.Combine(Environment.CurrentDirectory, configPath)))
                    {
                        configPath = Path.Combine(Environment.CurrentDirectory, configPath);
                    }
                    else
                    {
                        Console.WriteLine("configuration file is not exist.");
                        Execute(args);
                    }
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("configuration file is not exist.");
                    Console.ResetColor();
                    Execute(args);
                }

                try
                {
                    ImportManager manager = new ImportManager(configPath);
                    manager.Excute(cmdStr);
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);
                    Console.ResetColor();

                    args = new string[0];
                    Execute(args);
                }
            }

            if (args[0].Trim().ToLower() == "gdefault") {

                ApplicationContext context = ApplicationContext.CreateContext("C345E784-AA08-420F-B11F-2753BBEBFDD5");
                MenuNavigationManager mgr = new MenuNavigationManager(context);
                List<MenuNavigation> list = mgr.Load(x => x.TenantID == context.TenantID);
                List<string> excludeProps = new List<string>() { DatabaseObjects.Columns.TenantID, DatabaseObjects.Columns.ID, DatabaseObjects.Columns.Created, DatabaseObjects.Columns.Modified, "CreatedBy", "ModifiedBy", "ControlInfoList", "LayoutInfoList" };
                string detail = uGovernIT.DataTransfer.MetaUtility.GenerateInsertObjects<MenuNavigation>(list, excludeProperties: excludeProps);
            }

            Execute(args);
        }

        public static string[] AskParameters(string[] args)
        {
            Console.Write("migration> ");

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
                Console.WriteLine("configuration data sources: \n");
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
            Console.WriteLine("  std <config file> (Migrate SharePoint uGovernIT Site to DotNet Version)");
            Console.WriteLine("  dtd <config file> (Migrate dotnet to DotNet Version)");
            Console.WriteLine("  :) Wait for other options");
            Console.WriteLine("  ?  - for list of configuration types");
            Console.WriteLine("  exit - to exit console window"); Console.WriteLine("");
            Console.WriteLine("");
        }

       
    }
}

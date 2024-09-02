using System;
using System.Configuration;
using System.Linq;
using System.Reflection;
using uGovernIT.DefaultConfig;
using uGovernIT.Manager;

namespace UGovernITDefault
{
    class DataProgram
    {
        static string tenantcnn = Convert.ToString(ConfigurationManager.ConnectionStrings["tenantcnn"]);

        public static void Execute(string[] args)
        {
            ConfigurationList();
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

            if (args.Length < 1)
            {
                Log.WriteLog("ERROR: Please specify module names");
                Execute(new string[0]);
                return;
            }

            string moduleNames = args[0].Trim().ToLower();

            try
            {
                string accountId = string.Empty;
                bool createSuperAdmin = false;

                if (moduleNames == "all")
                {
                    moduleNames = string.Empty;

                    Console.WriteLine("Please enter Tenant Account Id to create new tenant:");

                    accountId = Console.ReadLine();

                    if (!string.IsNullOrEmpty(accountId.Trim()))
                    {
                        accountId = accountId.Trim();

                        // We are considering uGovernIT as a master tenant
                        if ("uGovernIT".Equals(accountId, StringComparison.OrdinalIgnoreCase))
                        {
                            accountId = "uGovernIT";
                            createSuperAdmin = true;
                        }
                        //Execute(new string[0]);
                    }
                }

                //Find and call script class using reflection
                var assembly = Assembly.Load(Assembly.GetExecutingAssembly().GetReferencedAssemblies().Single(x => x.Name.Equals("uGovernIT.DefaultConfig")));
                var instances = from t in assembly.GetTypes()
                                where t.GetInterfaces().Contains(typeof(IModule))
                                         && t.GetConstructor(Type.EmptyTypes) != null
                                select Activator.CreateInstance(t) as IModule;

                if (!string.IsNullOrWhiteSpace(moduleNames))
                {
                    string[] modules = moduleNames.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.ToLower().Trim()).ToArray();
                    instances = instances.Where(x => modules.Contains(x.ModuleName.ToLower())).ToList();
                }

                //List<Tenant> listTenant = new List<Tenant>();

                // if account id null or empty ask user to enter account id

                // Adding new tenant's in common database
                var tenant = DefaultConfigManager.InsertConfigTenant(accountId);

                //if (string.IsNullOrEmpty(accountId))
                //{                   
                //    // Get tenant's from common database
                //    CustomDbContext tcontext = new CustomDbContext(tenantcnn);
                //    using (DatabaseContext ctx = new DatabaseContext(tcontext))
                //    {
                //        listTenant = ctx.Set<Tenant>().ToList();
                //    }
                //}
                //else
                //{
                //    listTenant.Add(new Tenant() {
                //        TenantID = new Guid(accountId.Trim())
                //    });
                //}
                // Add tenant to list listTenant 

                // Populate data for tenant's
                if (tenant != null)
                {
                    ApplicationContext applicationContext = ApplicationContext.Create();
                    GlobalVar.TenantID = applicationContext.TenantID = tenant.TenantID.ToString();

                    DefaultConfigManager.LoadCommanConfiguration(applicationContext, accountId,string.Empty, null, createSuperAdmin);

                    Log.WriteLog(string.Format("Start: Loading modules {0}", moduleNames));

                    if (instances.Count() == 0)
                        Log.WriteLog(string.Format("Error: no module found"));
                    else
                        Log.WriteLog(string.Format("Modules found: {0}", string.Join(", ", instances.Select(x => x.ModuleName))));

                    foreach (var instance in instances)
                    {
                        DefaultConfigManager.LoadModuleConfiguration(instance, applicationContext);
                    }

                    Log.WriteLog(string.Format("End: Loading modules {0}", moduleNames));
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

        //public static void InsertConfigTenant()
        //{
        //    Guid g = Guid.NewGuid();
        //    List<Tenant> listTenant = new List<Tenant>();
        //    listTenant.Add(new Tenant() { TenantID = g, TenantName = "Infogen", AccountID = "Infogen", Country = "India", TenantUrl = string.Empty, DBName = string.Empty, DBServer = string.Empty, Deleted = false, Created = DateTime.Now, Modified = DateTime.Now, CreatedByUser = g.ToString(), ModifiedByUser = g.ToString() });
        //    g = Guid.NewGuid();
        //    listTenant.Add(new Tenant() { TenantID = g, TenantName = "uGovernIT", AccountID = "uGovernIT", Country = "India", TenantUrl = string.Empty, DBName = string.Empty, DBServer = string.Empty, Deleted = false, Created = DateTime.Now, Modified = DateTime.Now, CreatedByUser = g.ToString(), ModifiedByUser = g.ToString() });
        //    uGITDAL.InsertItemComman(listTenant);
        //}

        public static string[] AskParameters(string[] args)
        {
            Console.Write("data> ");

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
                ConfigurationList();
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
            Console.WriteLine("  list of modules with comma separated (TSR,ACR) - to load module configuration");
            Console.WriteLine("  all to load all modules");
            Console.WriteLine("  ?  - for list of configuration types");
            Console.WriteLine("  exit - to exit console window"); Console.WriteLine("");
            Console.WriteLine("");
        }

        public static void ConfigurationList()
        {
            Console.WriteLine();
        }
    }
}
 
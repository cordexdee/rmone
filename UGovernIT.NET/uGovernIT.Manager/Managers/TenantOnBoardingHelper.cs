using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;
using uGovernIT.Utility.Entities.Common;

namespace uGovernIT.Manager
{
    public class TenantOnBoardingHelper
    {
        public ApplicationContext _context = null;
        //public DefaultConfigManager defaultConfigManager = null;

        public TenantOnBoardingHelper(ApplicationContext context)
        {
            _context = context;
        }

        //remove a space and special character from a string
        public static string RemoveSpaceandSpecialchar(string str)
        {
            str = Regex.Replace(str, "[^0-9A-Za-z]", "");
            str = str.Replace(" ", "").Trim();
            return str;
        }

        public long SaveToRegistrationTenant(bool isSelfRegistration, ServiceInput serviceInput, bool sendMail)
        {
            var tenantRegistration = new TenantRegistration();
            var tenantRegistrationManager = new TenantRegistrationManager(_context);
            var tenantRegistrationData = new TenantRegistrationData();
            var receiverAddress = string.Empty;

            if (serviceInput != null)
            {
                /*
                tenantRegistrationData.Email = serviceInput.ServiceSections[0].Questions.Where(x => x.Token.Equals("email")).SingleOrDefault().Value;
                tenantRegistrationData.Company = serviceInput.ServiceSections[0].Questions.Where(x => x.Token.Equals("companyname")).SingleOrDefault().Value;
                //tenantRegistration = serviceInput.ServiceSections[0].Questions.Where(x => x.Token.Equals("companyname")).SingleOrDefault().Value;
                tenantRegistrationData.Title = serviceInput.ServiceSections[0].Questions.Where(x => x.Token.Equals("title")).SingleOrDefault().Value;
                tenantRegistrationData.Contact = serviceInput.ServiceSections[0].Questions.Where(x => x.Token.Equals("phone")).SingleOrDefault().Value;
                tenantRegistrationData.Url = serviceInput.ServiceSections[0].Questions.Where(x => x.Token.Equals("url")).SingleOrDefault().Value;
                tenantRegistrationData.Name = serviceInput.ServiceSections[0].Questions.Where(x => x.Token.Equals("name")).SingleOrDefault().Value;
                */
                if(serviceInput.ServiceSections[0].Questions.Where(x => x.Token.Equals("email")).SingleOrDefault() != null)
                    tenantRegistrationData.Email = serviceInput.ServiceSections[0].Questions.Where(x => x.Token.Equals("email")).SingleOrDefault().Value;

                if (serviceInput.ServiceSections[0].Questions.Where(x => x.Token.Equals("companyname")).SingleOrDefault() != null)
                    tenantRegistrationData.Company = serviceInput.ServiceSections[0].Questions.Where(x => x.Token.Equals("companyname")).SingleOrDefault().Value;

                if (serviceInput.ServiceSections[0].Questions.Where(x => x.Token.Equals("title")).SingleOrDefault() != null)
                    tenantRegistrationData.Title = serviceInput.ServiceSections[0].Questions.Where(x => x.Token.Equals("title")).SingleOrDefault().Value;

                if (serviceInput.ServiceSections[0].Questions.Where(x => x.Token.Equals("phone")).SingleOrDefault() != null)
                    tenantRegistrationData.Contact = serviceInput.ServiceSections[0].Questions.Where(x => x.Token.Equals("phone")).SingleOrDefault().Value;

                if (serviceInput.ServiceSections[0].Questions.Where(x => x.Token.Equals("url")).SingleOrDefault() != null)
                    tenantRegistrationData.Url = serviceInput.ServiceSections[0].Questions.Where(x => x.Token.Equals("url")).SingleOrDefault().Value;

                if (serviceInput.ServiceSections[0].Questions.Where(x => x.Token.Equals("name")).SingleOrDefault() != null)
                    tenantRegistrationData.Name = serviceInput.ServiceSections[0].Questions.Where(x => x.Token.Equals("name")).SingleOrDefault().Value;

                tenantRegistrationData.TenantCreationStarted = false;
                tenantRegistration.TenantRegistrationData = Newtonsoft.Json.JsonConvert.SerializeObject(tenantRegistrationData);
                //long requestCount = tenantRegistrationManager.Load().Count();

                //if (requestCount == 0)
                //{
                //    tenantRegistration.Id = requestCount + 1;
                //}
                //else
                //{
                //    tenantRegistration.Id = requestCount + 1;
                //}
                tenantRegistrationManager.Insert(tenantRegistration);
            }
            if (sendMail)
            {
                var response = new EmailHelper(_context).SendVerficationEmail(tenantRegistrationData.Email, tenantRegistration.Id.ToString());
            }

            return tenantRegistration.Id;
        }        

        public bool IsRecordExists(string email = null, string company = null)
        {
            var retValue = false;
            var uniqueFields = ValidationParameter();

            if (uniqueFields != null)
            {
                foreach (var item in uniqueFields)
                {
                    if ("email".EqualsIgnoreCase(item) && !string.IsNullOrEmpty(email))
                    {
                        retValue = GetTableDataManager.IsExist($"select top (1) 1 from Tenant where email='{email}' and Deleted = 0", true);
                        break;
                    }

                    if ("company".EqualsIgnoreCase(item) && !string.IsNullOrEmpty(company))
                    {
                        // Account ID must be unique so checking account id for company
                        var accountid = RemoveSpaceandSpecialchar(company);

                        retValue = GetTableDataManager.IsExist($"select top (1) 1 from Tenant where AccountID='{accountid}' and Deleted = 0", true);
                        break;
                    }
                }
            }

            return retValue;
        }

        public bool IsEmailExist(string email)
        {
            return IsRecordExists(email: email);
        }

        public bool IsCompanyExist(string company)
        {
            return IsRecordExists(company: company);
        }

        public string[] ValidationParameter()
        {
            var _configurationVariableManager = new ConfigurationVariableManager(_context);

            var configVarUniqueAccount = _configurationVariableManager.GetValue(ConfigConstants.UniqueAccount);

            var uniqueFields = configVarUniqueAccount.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            return uniqueFields;
        }

        public List<string> GetNewTenantTemplateServiceTitles()
        {
            return ConfigurationManager.AppSettings["NewTenantTemplate:ServiceTitle"].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        public List<string> GetNewTenantTemplateOnBoarding()
        {
            return ConfigurationManager.AppSettings["NewTenantTemplate:EmployeeOnBoarding"].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        public TenantConstraints getTenantConstraints()
        {
            TenantConstraints tenantConstraint = new TenantConstraints();
            ConfigurationVariableManager ConfigurationVariableManager = new ConfigurationVariableManager(_context);

            Int32.TryParse(ConfigurationVariableManager.GetValue(ConfigConstants.TenantCountCritical), out tenantConstraint.TenantCountCritical);
            Int32.TryParse(ConfigurationVariableManager.GetValue(ConfigConstants.TenantCountHigh), out tenantConstraint.TenantCountHigh);
            Int32.TryParse(ConfigurationVariableManager.GetValue(ConfigConstants.TicketCountCritical), out tenantConstraint.TicketCountCritical);
            Int32.TryParse(ConfigurationVariableManager.GetValue(ConfigConstants.TicketCountHigh), out tenantConstraint.TicketCountHigh);
            Int32.TryParse(ConfigurationVariableManager.GetValue(ConfigConstants.ServiceCountCritical), out tenantConstraint.ServiceCountCritical);
            Int32.TryParse(ConfigurationVariableManager.GetValue(ConfigConstants.ServiceCountHigh), out tenantConstraint.ServiceCountHigh);
            return tenantConstraint;
        }


        public TenantStatistics GetTicketOrServiceCountAndClass(DataTable dt,TenantConstraints tenantConstraints)
        {
            TenantStatistics tenantStatistics = new TenantStatistics();
            if (dt!= null)
            {
               string result =  dt.Rows[0]["TenantConstraintData"].ToString();
               var strarray = result.Split(',');
                
                tenantStatistics.TicketCount =Convert.ToInt32( strarray[0]);
                tenantStatistics.ServiceCount = Convert.ToInt32(strarray[1]);
                tenantStatistics.TicketClass = "normal-constraints";
                if (tenantStatistics.TicketCount >= tenantConstraints.TicketCountHigh && tenantStatistics.TicketCount < tenantConstraints.TicketCountCritical)
                {
                    tenantStatistics.TicketClass = "constraint-High-Grid";
                }
                if (tenantStatistics.TicketCount >= tenantConstraints.TicketCountCritical)
                {
                    tenantStatistics.TicketClass = "constraint-Critical-Grid";
                }

                
                    tenantStatistics.ServiceClass = "normal-constraints";
                    

                if (tenantStatistics.ServiceCount >= tenantConstraints.ServiceCountHigh && tenantStatistics.ServiceCount < tenantConstraints.ServiceCountCritical)
                {
                    tenantStatistics.ServiceClass = "constraint-High-Grid";
                }
                if (tenantStatistics.ServiceCount >= tenantConstraints.ServiceCountCritical)
                {
                    tenantStatistics.ServiceClass = "constraint-Critical-Grid";
                }


            }

            return tenantStatistics;
        }
    }
}

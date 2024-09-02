using ITAnalyticsBL.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using uGovernIT.Utility.Entities;

namespace ITAnalyticsBL.Extension
{
    public static class ExtendedMethod
    {      
        public static Company SetCurrentCompany(this HttpContext context,Company company)
        {
            if (company != null)
            {
                if (context.User.Identity.IsAuthenticated)
                {
                    context.Items.Add("CurrentCompany", company);
                }
            }
            return company;
        }
        public static long? GetCurrentTenantId(this HttpContext context)
        {
            long? companyId = null;
            if (context!=null && context.User.Identity.IsAuthenticated && context.Items != null && context.Items["CompanyID"] != null)
            {
                companyId = Convert.ToInt64(context.Items["CompanyID"]);
            }
            return companyId;
        }
        public static Company GetCompanyById(this HttpContext context,long id)
        {
            return HttpContext.Current.GetAllCompany().FirstOrDefault(x => x.Id == id);
        }
        public static string GetCompanyNameById(this HttpContext context, long id)
        {
            //if (id == 0)
            //    return "System Account";
            //MainContext mainContext = new MainContext();
            //Company company= HttpContext.Current.GetAllCompany().FirstOrDefault(x => x.Id == id);
            //if (company != null)
            //    return company.Name;
            //else
            //    return "";


            return null;

        }
        public static List<Company> GetAllCompany(this HttpContext context)
        {
            List<Company> companyList = new List<Company>();
            //companyList.Add(new Company() { Id = 0, Name = "System Account", Email = "system@itanalytics.com", Zip = "110086", PhoneNumber = "000000000" });
            //companyList.AddRange(MainContext.Instance.Companies.ToList());
            return companyList;
        }

        public static bool HasPermission(this UserProfile user, string task)
        {
            return true;
        }
    }
}

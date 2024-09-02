using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using uGovernIT.Utility;

namespace uGovernIT.Manager
{
    public class ServiceSectionManager : ManagerBase<ServiceSection>, IServiceSectionManager
    {
        public ServiceSectionManager(ApplicationContext context) : base(context)
        {
        }
        public ServiceSection GetByID(long? ID, long serviceID)
        {
            ServiceSection obj = LoadByID(Convert.ToInt32(ID));
            //SPList sectionList = SPListHelper.GetSPList(DatabaseObjects.Lists.ServiceSections);
            //SPListItem item = SPListHelper.GetSPListItem(sectionList, ID);
            //section = LoadObj(item);
            //if (section.ServiceID == serviceID)
            //{
            //    return section;
            //}
            return obj;
        }

        public List<ServiceSection> GetByServiceID(long serviceID)
        {
            List<ServiceSection> sections = new List<ServiceSection>();
            ServiceSectionManager serviceSectionManager = new ServiceSectionManager(dbContext);
            sections = serviceSectionManager.Load(x => x.ServiceID == serviceID).ToList();
           
            return sections;
        }

        public DataTable GetTableByServiceID(long serviceID)
        {
            DataTable sectionTable = new DataTable();
            //sectionTable.Columns.Add("ID", typeof(int));
            //sectionTable.Columns.Add("ServiceID", typeof(int));
            //sectionTable.Columns.Add("ServiceName");
            //sectionTable.Columns.Add("SectionTitle");
            //sectionTable.Columns.Add("ItemOrder", typeof(int));

            //List<ServiceSection> sections = GetByServiceID(serviceID);
            //foreach (ServiceSection section in sections)
            //{
            //    DataRow row = sectionTable.NewRow();
            //    row["ID"] = section.ID;
            //    row["ServiceID"] = section.ServiceID;
            //    row["ServiceName"] = section.ServiceName;
            //    row["SectionTitle"] = section.SectionTitle;
            //    row["ItemOrder"] = section.ItemOrder;
            //    sectionTable.Rows.Add(row);
            //}
            return sectionTable;
        }

        private ServiceSection LoadObj(ServiceSection item)
        {
            ServiceSection section = new ServiceSection();
            //section.ID = item.ID;
            //if (uHelper.IsSPItemExist(item, DatabaseObjects.Columns.ServiceTitleLookup))
            //{
            //    SPFieldLookupValue serviceLookup = new SPFieldLookupValue(Convert.ToString(uHelper.GetSPItemValue(item, DatabaseObjects.Columns.ServiceTitleLookup)));
            //    if (serviceLookup.LookupId > 0)
            //    {
            //        section.ServiceID = serviceLookup.LookupId;
            //        section.ServiceName = serviceLookup.LookupValue;
            //    }
            //}
            //section.SectionTitle = Convert.ToString(uHelper.GetSPItemValue(item, DatabaseObjects.Columns.Title));

            //int itemOrder = 0;
            //int.TryParse(Convert.ToString(uHelper.GetSPItemValue(item, DatabaseObjects.Columns.ItemOrder)), out itemOrder);
            //section.ItemOrder = itemOrder;
            //section.Description = Convert.ToString(uHelper.GetSPItemValue(item, DatabaseObjects.Columns.UGITDescription));
            return section;
        }

        public bool Delete(Services service)
        {
            //SPList sectionList = SPListHelper.GetSPList(DatabaseObjects.Lists.ServiceSections);

            //if (this.ID > 0)
            //{
            //    try
            //    {
            //        List<ServiceQuestion> questions = service.Questions.Where(x => x.ServiceSectionID == this.ID).ToList();
            //        ServiceQuestion.Delete(questions);

            //        SPListItem item = SPListHelper.GetSPListItem(sectionList, this.ID);
            //        item.Delete();

            //        //Reorder Service sections 
            //        service = Services.LoadByID(service.ID, false, true, false);
            //        for (int i = 0; i < service.Sections.Count; i++)
            //        {
            //            service.Sections[i].ItemOrder = i + 1;
            //        }
            //        ServiceSection.SaveOrder(service.Sections);
            //    }
            //    catch (Exception ex)
            //    {
            //        Log.WriteException(ex);
            //    }
            //}
            return false;
        }
        

        public bool Save(ServiceSection objSeviceSection)
        {
            if(objSeviceSection.ID <= 0)
            {
                long i = Insert(objSeviceSection);
               // objSeviceSection.ID = i; 
                if (i > 0)
                    return true;
                else
                    return false;
            }
            else
            {
                return Update(objSeviceSection);
            }
        }
        public void SaveOrder(List<ServiceSection> questions)
        {
            if (questions.Count <= 0)
            {
                return;
            }

            foreach (ServiceSection item in questions)
            {
                Save(item);
            }
        }
    }

    interface IServiceSectionManager : IManagerBase<ServiceSection>
    {
        bool Save(ServiceSection objSeviceSection);
    }
}

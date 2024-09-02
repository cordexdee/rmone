using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Xml;
using uGovernIT.Utility;
using System.Web;
using uGovernIT.Utility.Entities;
using uGovernIT.Manager.Managers;
using uGovernIT.Util.Log;
using uGovernIT.DAL.Store;

namespace uGovernIT.Manager
{
    public class EmployeeTypeManager : ManagerBase<EmployeeTypes>, IEmployeeTypeManager
    {
        public EmployeeTypeManager(ApplicationContext context) : base(context)
        {
            store = new EmployeeTypeStore(this.dbContext);
        }
        public long Save(EmployeeTypes EmployeeType)
        {
            if (EmployeeType.ID > 0)
                this.Update(EmployeeType);
            else
                this.Insert(EmployeeType);
            return EmployeeType.ID;
        }
    }
    public interface IEmployeeTypeManager : IManagerBase<EmployeeTypes>
    {

    }
}

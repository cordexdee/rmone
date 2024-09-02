using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;
using uGovernIT.DAL;
using System.Data;
using uGovernIT.DAL.Store;
namespace uGovernIT.Manager
{
    public class SurveyFeedbackManager : ManagerBase<SurveyFeedback>, ISurveyFeedbackManager
    {
        
        public SurveyFeedbackManager(ApplicationContext context) : base(context)
        {
            store = new SurveyFeedbackStore(this.dbContext);
        }
    }
    interface ISurveyFeedbackManager : IManagerBase<SurveyFeedback>
    {

    }
}

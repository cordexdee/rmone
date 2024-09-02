using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
    public class SurveyFeedbackStore:StoreBase<SurveyFeedback>, ISurveyFeedbackStore
    {
        public SurveyFeedbackStore(CustomDbContext context) : base(context)
        {

        }
    }
    public interface ISurveyFeedbackStore : IStore<SurveyFeedback>
    {

    }
}

using System;
using System.Data;
using System.Linq;

namespace uGovernIT.Manager
{
    public class AvailablePRPAndAssignTo
    {
      
        private ApplicationContext context;
        private string PRP = string.Empty;

        DataTable NewMatch = new DataTable();
        DataRow NotMatch = null;

        public AvailablePRPAndAssignTo(ApplicationContext context)
        {
            this.context = context;
        }

        public string GetPRPOrAssignTo(DataTable PRPGroupOpen, DataTable PRPGroupClosed,string columnName)
        {
            //Check  Maximum closed ticket user has.
            if (PRPGroupClosed != null && PRPGroupClosed.Rows.Count != 0)
            {
                int PRPGroupClosedMax = Convert.ToInt32(PRPGroupClosed.Compute("max([co])", string.Empty));

                PRPGroupClosed = PRPGroupClosed.Select($"co = {PRPGroupClosedMax}").CopyToDataTable();

                if (PRPGroupClosed.Rows.Count == 1)
                {
                    PRP = PRPGroupClosed.Rows[0][columnName].ToString();
                }
                else
                {
                    foreach (DataRow dr in PRPGroupClosed.Rows)
                    {
                        if (dr[columnName] != null)
                        {
                            DataTable matchingPRP = new DataTable();

                            var matchingPRP1 = PRPGroupOpen.Select($"{columnName} = '{dr[columnName]}'");

                            if (matchingPRP1 != null && matchingPRP1.Count() > 0)
                            {
                                NewMatch.Merge(matchingPRP1.CopyToDataTable());
                            }
                            else
                            {
                                NotMatch = PRPGroupClosed.NewRow();
                                NotMatch = dr;
                            }

                        }


                    }
                    if (NotMatch != null)
                    {
                        PRP = NotMatch["PRPUser"].ToString();
                    }
                    else
                    {
                        int PRPGroupOpendMin = Convert.ToInt32(NewMatch.Compute("min([co])", string.Empty));

                        PRPGroupOpen = PRPGroupOpen.Select($"co = {PRPGroupOpendMin}").CopyToDataTable();
                        if (PRPGroupOpen != null)
                        {
                            PRP = PRPGroupOpen.Rows[0][columnName].ToString();
                        }

                    }


                }

            }

            //Check count of minimum open tickets user has.
            else if (PRPGroupOpen != null && PRPGroupOpen.Rows.Count != 0)
            {
                int PRPGroupOpendMin = Convert.ToInt32(PRPGroupOpen.Compute("min([co])", string.Empty));
                PRPGroupOpen = PRPGroupOpen.Select($"co = {PRPGroupOpendMin}").CopyToDataTable();
                PRP = PRPGroupOpen.Rows[0][columnName].ToString();
            }

            else
            {
                PRP = string.Empty;
            }
            return PRP;

        }




    }
}

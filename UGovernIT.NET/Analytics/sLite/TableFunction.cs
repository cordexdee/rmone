﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace sLite
{
  [Serializable]
    public class TableFunction : Function
    {
        public List<Column> Columns { get; set; }
        public string Expression { get; set; }
        public string IncludeInScore { get; set; }       
        public override double Execute(FunctionInput function, bool isNormalize, double normalizeBy)
        {
            double normalizedScore = 0;
            double totalWeight = Columns.Sum(x => x.Weight);
            double MaxWeight = Columns.Max(x => x.Weight);


            // Single value case
            if (!function.IsMultiValue)
            {
                Column selectedLabel = function.Value != null ? Columns.FirstOrDefault(x => x.Name.ToLower() == function.Value.ToString().ToLower()) : null;
                double selectedWeight = 0;
                if (selectedLabel != null)
                {
                    selectedWeight = selectedLabel.Weight;
                }

              
                if (isNormalize)
                {
                    //if (totalWeight > 0)
                    {
                        //normalizedScore = (double)(selectedWeight / totalWeight) * normalizeBy;
                        normalizedScore = (double)(selectedWeight / MaxWeight) * normalizeBy;
                    }
                }
                else
                {
                    normalizedScore = (double)selectedWeight;
                }
            }
            else
            {
                double result = 0.0;
                 //Tabular value case
                if (!string.IsNullOrEmpty(function.Table))
                {
                    List<string> cols = new List<string>();
                    List<double> vals = new List<double>();
                    cols = function.Value.ToString().Split(new string[] { "~~" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    vals = function.Table.Split(new string[] { ";#" }, StringSplitOptions.RemoveEmptyEntries).ToList().Select(double.Parse).ToList();
                    double Sum = vals.Sum();
                    
                    int ctr = 0;
                    if (Sum > 0 && vals.Count > 0)
                    {
                        foreach (string s in cols)
                        {
                            double lblWeight = Columns.FirstOrDefault(x => x.Name.ToLower() == s.ToLower()).Weight;
                            //result += lblWeight * (vals[ctr] / Sum);
                            if(vals.Count > ctr)
                            result +=  (lblWeight/totalWeight) * vals[ctr];
                            ctr++;
                        }
                    }
                }
                else
                {
                    //Mutichoice value case
                    result = 0.0;
                    List<string> cols = new List<string>();
                    cols = function.Value.ToString().Split(new string[] { ";#" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    int ctr = 0;
                    foreach (string s in cols)
                    {
                       Column lbl = Columns.FirstOrDefault(x => x.Name.ToLower() == s.ToLower());
                       if (lbl != null)
                       {
                           result += lbl.Weight;
                           ctr++;
                       }                        
                    }

                    if (ctr != 0)
                    {
                        result = result / ctr;
                    }
                }

                if (isNormalize)
                {
                    if (MaxWeight > 0)
                    {
                        normalizedScore = (double)(result / MaxWeight) * normalizeBy;
                    }
                }
                else
                {
                    normalizedScore = (double)result;
                }

            }
            return normalizedScore;
        }
    }
}

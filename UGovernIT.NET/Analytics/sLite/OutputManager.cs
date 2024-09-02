using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Text.RegularExpressions;
using System.Data;

namespace sLite
{
    public class OutputManager
    {

      /// <summary>
      /// Creates analytic score based on analytic input object
      /// and returns AnalyticOuput object which contains score of each component of analytic
      /// </summary>
      /// <param name="analyic">Analytic object</param>
      /// <param name="analyticInput">Analytic Input which contains input of each analytic function</param>
      /// <returns>return Object of AnalyticOuput which contains score of each component of analytic</returns>
        public static AnalyticOutput GetResult(Analytic analyic, AnalyticInput analyticInput)
        {

            analyic.Weight = 1;
            foreach (KPI k in analyic.KPIs)
            {
                k.Weight = 1;
                foreach (Metric m in k.Metrics)
                {
                    m.Weight = 1;
                    foreach (Function f in m.Functions)
                    {
                        f.Weight = 1;
                    }
                }
            }
            AnalyticOutput output = new AnalyticOutput();
            output = OutputManager.ExecuteAnalytic(analyic, analyticInput);
            //TODO: Call ExecuteAnalyic to generate score
            
        

            return output;
        }

        /// <summary>
        /// Creates analytic score based on analytic input xml
        /// and returns AnalyticOuput object which contains score of each component of Analytic
        /// </summary>
        /// <param name="analyic">Analytic xml which is xml form of Analytic object</param>
        /// <param name="analyticInput">Analytic Input xml which is xml form of Analytic xml</param>
        /// <returns>return Object of AnalyticOuput which contains score of each component of analytic</returns>
        public static AnalyticOutput GetResult(XmlDocument analytic, XmlDocument analyticInput)
        {
            AnalyticOutput output = new AnalyticOutput();

            Analytic analyticObj = new Analytic();
            analyticObj =(Analytic)Helper.DeSerializer(analytic.DocumentElement.OuterXml, analyticObj.GetType());

            AnalyticInput analyticInputObj = new AnalyticInput();
            analyticInputObj = (AnalyticInput)Helper.DeSerializer(analytic.DocumentElement.OuterXml, analyticInputObj.GetType());

            output = OutputManager.ExecuteAnalytic(analyticObj, analyticInputObj);

            //TODO: Call ExecuteAnalyic to generate score
            return output;
        }

        /// <summary>
        /// Creates analytic score based on analytic input xml
        /// and returns AnalyticOuput object which contains score of each component of Analytic
        /// </summary>
        /// <param name="analyic">Analytic xml which is xml form of Analytic object</param>
        /// <param name="analyticInput">Analytic Input xml which is xml form of Analytic xml</param>
        /// <returns>return xml of AnalyticOuput which contains score of each component of analytic and xml form of Analytic Output object</returns>
        public static XmlDocument GetResultInXml(XmlDocument analytic, XmlDocument analyticInput)
        {
            XmlDocument outputDoc = new XmlDocument();
            //TODO: Deserialize the analytic and analytic object can call overload
            //TODO: Call ExecuteAnalyic to generate score

            //TODO: After creating output object and Serialize it into xml
            return outputDoc;
        }


        /// <summary>
        /// Creates analytic score based on analytic input xml
        /// and returns AnalyticOuput object which contains score of each component of Analytic
        /// </summary>
        /// <param name="analyic">Analytic object</param>
        /// <param name="analyticInput">Analytic Input which contains input of each analytic function</param>
        /// <returns>return xml of AnalyticOuput which contains score of each component of analytic and xml form of Analytic Output object</returns>
        public static XmlDocument GetResultInXml(Analytic analyic, AnalyticInput analyticInput)
        {
            XmlDocument outputDoc = new XmlDocument();
            AnalyticOutput analyticOutput = new AnalyticOutput();
            analyticOutput = OutputManager.ExecuteAnalytic(analyic, analyticInput);

              string outputXml = Helper.Serializer(analyticOutput, analyticOutput.GetType());
              outputDoc.LoadXml(outputXml);
            //TODO: After creating output object and Serialize it into xml
            return outputDoc;
        }


        /// <summary>
        /// Executes input analytic on analyic object and generate output
        /// </summary>
        /// <param name="analytic">Analytic object which contains meta information of Analytic</param>
        /// <param name="analyticInput">Analytic Input Object which contains input data againt each function</param>
        /// <returns>return AnalyticOuput object which contains score of each component</returns>
        private static AnalyticOutput ExecuteAnalytic(Analytic analytic, AnalyticInput analyticInput)
        {
            AnalyticOutput analyticOutput = new AnalyticOutput();
            //TODO: Call ExecuteAnalyic to generate score
            analyticOutput = OutputManager.CalculateScore(analytic, analyticInput);
            
            return analyticOutput;
        }

        /// <summary>
        /// Normalizes the analytic weight and return analytic output which contains normalize weight
        /// </summary>
        /// <param name="analytic">Analytic Input Object which contains input data againt each function</param>
        /// <returns>Return AnalyticOuput object which contains basic analytic infor and normalized weight</returns>
        private static AnalyticOutput NormalizeAnalyticWeight(Analytic analytic)
        {
            AnalyticOutput analyticOutput = new AnalyticOutput();
            List<KPIOutput> lstKPIOut = new List<KPIOutput>();

            FunctionOutput funcOutput = new FunctionOutput();
            MetricOutput metricOutput = null;
            KPIOutput kpiOutput = null;
            bool isNormalize = false;
            if (analytic.Weight != 1)
                isNormalize = true;
            
            float totalKpiWeight = analytic.KPIs.Where(x=>x.Weight >= 0).Sum(x => x.Weight);

            analyticOutput.Name = analytic.Name;
            analyticOutput.ID = analytic.ID;
            analyticOutput.Weight = analytic.Weight;
            analyticOutput.CalculatedWeight = analytic.Weight;

            for (int i = 0; i < analytic.KPIs.Count; i++)
            {
                kpiOutput = new KPIOutput();
                kpiOutput.Name = analytic.KPIs[i].Name;
                kpiOutput.ID = analytic.KPIs[i].ID;
                kpiOutput.Weight = analytic.KPIs[i].Weight;
                //Normalizing each KPI weight on the basis of analytic weight
                
                if(isNormalize)
                    kpiOutput.CalculatedWeight =  (analytic.KPIs[i].Weight / totalKpiWeight) * analyticOutput.CalculatedWeight;
                else
                    kpiOutput.CalculatedWeight = analytic.KPIs[i].Weight;

                float totalMetricWeight = analytic.KPIs[i].Metrics.Where(x => x.Weight >= 0).Sum(x => x.Weight);
                List<MetricOutput> lstMetricOutput = new List<MetricOutput>();
                for (int j = 0; j < analytic.KPIs[i].Metrics.Count; j++)
                {
                   
                    metricOutput = new MetricOutput();
                    metricOutput.Name = analytic.KPIs[i].Metrics[j].Name;
                    metricOutput.ID = analytic.KPIs[i].Metrics[j].ID;
                    metricOutput.Weight = analytic.KPIs[i].Metrics[j].Weight;
                    // Normalizing each metric weight on the basis of respective KPI's calculated weight.
                    if (isNormalize)
                        metricOutput.CalculatedWeight = (analytic.KPIs[i].Metrics[j].Weight / totalMetricWeight) * kpiOutput.CalculatedWeight;
                    else
                        metricOutput.CalculatedWeight = analytic.KPIs[i].Metrics[j].Weight;

                    List<FunctionOutput> lstfuncOutput = new List<FunctionOutput>();
                    float totalFunctionWeigh = analytic.KPIs[i].Metrics[j].Functions.Where(x => x.Weight >= 0).Sum(x => x.Weight); ;
                    for (int k = 0; k < analytic.KPIs[i].Metrics[j].Functions.Count; k++)
                    {
                        funcOutput = new FunctionOutput();
                        funcOutput.Name = analytic.KPIs[i].Metrics[j].Functions[k].Name;
                        funcOutput.ID = analytic.KPIs[i].Metrics[j].Functions[k].ID;
                        funcOutput.Weight = analytic.KPIs[i].Metrics[j].Functions[k].Weight;
                       // Normalizing each function's weight on the basis of their respective Metric's calculated weight.
                        if (isNormalize)
                            funcOutput.CalculatedWeight = (analytic.KPIs[i].Metrics[j].Functions[k].Weight / totalFunctionWeigh) * metricOutput.CalculatedWeight;
                        else
                        {
                            funcOutput.CalculatedWeight = analytic.KPIs[i].Metrics[j].Functions[k].Weight;
                            if (analytic.KPIs[i].Metrics[j].Functions[k] is sLite.SimpleFunction){
                                funcOutput.Max = ((sLite.SimpleFunction)analytic.KPIs[i].Metrics[j].Functions[k]).MaxInput;
                                funcOutput.Min = ((sLite.SimpleFunction)analytic.KPIs[i].Metrics[j].Functions[k]).MinInput;
                            }
                            else if (analytic.KPIs[i].Metrics[j].Functions[k] is sLite.LabelFunction)
                            {
                                funcOutput.Max = ((sLite.LabelFunction)analytic.KPIs[i].Metrics[j].Functions[k]).Labels.Max(x => x.Weight); //* (analytic.KPIs[i].Metrics[j].Functions[k].Weight/100)
                                funcOutput.Min = ((sLite.LabelFunction)analytic.KPIs[i].Metrics[j].Functions[k]).Labels.Min(x => x.Weight);
                            }
                            else if (analytic.KPIs[i].Metrics[j].Functions[k] is sLite.TableFunction)
                            {
                                funcOutput.Max = ((sLite.TableFunction)analytic.KPIs[i].Metrics[j].Functions[k]).Columns.Max(x => x.Weight); //* (analytic.KPIs[i].Metrics[j].Functions[k].Weight/100)
                                funcOutput.Min = ((sLite.TableFunction)analytic.KPIs[i].Metrics[j].Functions[k]).Columns.Min(x => x.Weight);
                            }
                            else if (analytic.KPIs[i].Metrics[j].Functions[k] is sLite.NonLinearFunction)
                            {                                
                                funcOutput.Max = ((sLite.NonLinearFunction)analytic.KPIs[i].Metrics[j].Functions[k]).MaxInputForDefault;
                                funcOutput.Min = ((sLite.NonLinearFunction)analytic.KPIs[i].Metrics[j].Functions[k]).MinInputForDefault;
                            }
                            else if (analytic.KPIs[i].Metrics[j].Functions[k] is sLite.ConditionalFunction)
                            {
                                funcOutput.Max = ((sLite.ConditionalFunction)analytic.KPIs[i].Metrics[j].Functions[k]).MaxInputForDefault;
                                funcOutput.Min = ((sLite.ConditionalFunction)analytic.KPIs[i].Metrics[j].Functions[k]).MinInputForDefault;
                            }
                        }

                        if (analytic.KPIs[i].Metrics[j].Functions[k] is TableFunction && ((analytic.KPIs[i].Metrics[j].Functions[k] as TableFunction).IncludeInScore != "true"))
                        {
                            continue;
                        }
                        lstfuncOutput.Add(funcOutput);

                    }
                    
                    metricOutput.Functions = lstfuncOutput;
                    metricOutput.Max = lstfuncOutput.Sum(x => x.Max);
                    metricOutput.Min = lstfuncOutput.Sum(x => x.Min);
                    lstMetricOutput.Add(metricOutput);

                }
               
                kpiOutput.Metrics = lstMetricOutput;
                kpiOutput.Max = lstMetricOutput.Sum(x => x.Max);
                kpiOutput.Min = lstMetricOutput.Sum(x => x.Min);
                lstKPIOut.Add(kpiOutput);


            }
           
            analyticOutput.KPIs = lstKPIOut;
            analyticOutput.Max = lstKPIOut.Sum(x => x.Max);
            analyticOutput.Min = lstKPIOut.Sum(x => x.Min);




            return analyticOutput;
        }

        /// <summary>
        /// Calcuates score of each component of analytic and return AnalyticOutput
        /// </summary>
        /// <param name="analytic">Analytic Input Object which contains input data againt each function/param>
        /// <returns>Return AnalyticOuput object</returns>
        private static AnalyticOutput CalculateScore(Analytic analytic, AnalyticInput analyticInput)
        {
            
            AnalyticOutput analyticOutput = new AnalyticOutput();
            analyticOutput = OutputManager.NormalizeAnalyticWeight(analytic);
            List<KPIOutput> lstKPIOut = new List<KPIOutput>();

            bool isNormalize = false;
            if (analytic.Weight != 1)
                isNormalize = true;

            for(int i=0;i<analyticOutput.KPIs.Count;i++)
            {
                KPIOutput kpiOutput = analyticOutput.KPIs[i];

                List<MetricOutput> lstMetricOutput = new List<MetricOutput>();
                for (int j = 0; j < kpiOutput.Metrics.Count; j++)
                {
                   MetricOutput metricOutput = kpiOutput.Metrics[j];

                    for (int k = 0; k < metricOutput.Functions.Count; k++)
                    {
                        FunctionOutput funcOutput = metricOutput.Functions[k];
                        
                        Function analyticFunction = analytic.KPIs.FirstOrDefault(x=>x.ID == kpiOutput.ID).Metrics.FirstOrDefault(x=>x.ID == metricOutput.ID).Functions.FirstOrDefault(x=>x.ID==funcOutput.ID);
                      
                         FunctionInput inputFunction = null;
                         if(analyticInput.Functions != null && analyticInput.Functions.Count>0)
                         {
                            inputFunction = analyticInput.Functions.FirstOrDefault(x=>x.ID==funcOutput.ID);
                         }

                         if (inputFunction != null && !inputFunction.Ignore)
                         {
                             //double result = analyticFunction.Execute(inputFunction, isNormalize, Convert.ToInt16(funcOutput.CalculatedWeight));                          
                            // funcOutput.CalculatedScore = (result * (double)funcOutput.CalculatedWeight);
                             funcOutput.CalculatedScore = analyticFunction.Execute(inputFunction, isNormalize, funcOutput.CalculatedWeight);     
                         }
                       
                    }

                    metricOutput.CalculatedScore = metricOutput.Functions.Sum(x => x.CalculatedScore);

                }

                kpiOutput.CalculatedScore = kpiOutput.Metrics.Sum(x => x.CalculatedScore);
                
            }
            analyticOutput.CalculatedScore = analyticOutput.KPIs.Sum(x => x.CalculatedScore);

            //TODO: Normalize analytic weight
            //TODO: calculate the score of each component of analytic

            //analyticOutput = NormalizeAnalyticScore(analyticOutput, analytic);

            return analyticOutput;
        }
          
       

    }
}

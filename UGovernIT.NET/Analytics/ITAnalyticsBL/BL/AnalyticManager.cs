using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ITAnalyticsBL.DB;
using System.Xml;
using ITAnalyticsUtility;
using System.Xml.Schema;
using System.Configuration;
using ITAnalyticsBL.ET;
using System.Data.SqlClient;
using System.Data;

namespace ITAnalyticsBL.BL
{
    public class AnalyticManager
    {
        ModelDB modelDb;
        public AnalyticManager(ModelDB modelDb)
        {
            this.modelDb = modelDb;
        }

        public sLite.DataIntegration GetDataIntegration(long analyticVid)
        {
            sLite.DataIntegration dataIntegrationNode = new sLite.DataIntegration();
            string tableName = String.Empty;
            ModelVersion model = modelDb.ModelVersions.FirstOrDefault(x => x.ModelVersionID == analyticVid);
            XmlDocument modelXmlDoc = new XmlDocument();
            modelXmlDoc.LoadXml(model.HelpXml);
            sLite.AnalyticSupport supportObject = new sLite.AnalyticSupport();
            supportObject = (sLite.AnalyticSupport)sLite.Helper.DeSerializer(modelXmlDoc.OuterXml, supportObject.GetType());
            sLite.AnalyticNodeInfo infoObj = supportObject.NodesInfo.FirstOrDefault(x => x.MainNode);
            if (infoObj != null)
            {
                if (infoObj.DataIntegrations != null && infoObj.DataIntegrations.Count > 0)
                {
                    dataIntegrationNode = infoObj.DataIntegrations.FirstOrDefault();
                }
            }
            return dataIntegrationNode;
        }

        public int GetMapperCountByAnalyticId(long analyticId)
        {
            int count = 0;
            var modelList = (from m in modelDb.ModelOutputMappers
                             where m.ModelVersionID == analyticId
                             select new { ModelVersionID = m.ModelVersionID, Title = m.Title, Rows = m.Rows, Columns = m.Columns, Xaxis = m.Xaxis, Yaxis = m.Yaxis, ID = m.ID, Activated = m.Activated });
            if (modelList != null)
                count = modelList.Count();
            return count;
        }


        public int GetInterpretationCountByAnalyticId(long analyticId)
        {
            int count = 0;
            var interpretationList = from m in modelDb.Interpretations
                            where m.ModelVersionID == analyticId
                            select m;

            if (interpretationList != null)
                count = interpretationList.Count();
            return count;
        }
        public bool CheckETTableReference(string tableName)
        {
            foreach (ModelVersion mv in modelDb.ModelVersions)
            {
                if (mv.HelpXml.Contains(tableName))
                    return true;

            }
            return false;
        }
        public string GetPreviousMappersInEditVersion(long analyticVersionID)
        {
            var newAnalyticMappers = modelDb.ModelOutputMappers.Where(x => x.ModelVersionID == analyticVersionID).ToList();
            string mapperIds = string.Empty;
            if (newAnalyticMappers != null)
            {
                foreach (ModelOutputMapper mapp in newAnalyticMappers)
                {
                    mapperIds += mapp.Title + ";";
                }
            }
            return mapperIds;
        }

        public int GetCurrentActiveVersionNumberVersion(long analyticVersionID)
        {
            ModelVersion vers = modelDb.ModelVersions.Find(analyticVersionID);
            if (vers != null)
                return vers.VersionNumber;
            else
                return 0;
        }
        public bool FindPreviousVersions(long analyticVersionID)
        {
            ModelVersion vers = modelDb.ModelVersions.Find(analyticVersionID);
           
            int modelID = 0;
            if (vers != null)
            {
                var allVersions = modelDb.ModelVersions.Where(x => x.ModelID == modelID);
               
                if (allVersions != null && allVersions.Count() == 1)
                {
                    return false;
                   
                }
                
            }
            return true;
        }

        public int SaveMappersForNewAnalytic(List<string> mappersIdList, long activeAnalyticVID, long newEditAnalyticVersionId)
        {
              int numberOfMappersAdded = 0;
              var newAnalyticMappers = modelDb.ModelOutputMappers.Where(x => x.ModelVersionID == newEditAnalyticVersionId).ToList();
            //deleting all the mappers and saving them once again.
              if (newAnalyticMappers != null && newAnalyticMappers.Count > 0)
              {
                  foreach (ModelOutputMapper mapp in newAnalyticMappers)
                  {
                      modelDb.ModelOutputMappers.Remove(mapp);

                  }
              }

            //saving them once again.
            foreach(string mapperId in mappersIdList)            
            {
                long id = Convert.ToInt32(mapperId);
                var mapper = modelDb.ModelOutputMappers.FirstOrDefault(x => x.ID == id);               
                if (mapper != null )
                {                   
                    //Saving the actual and default data to database.
                    ModelOutputMapper modelMapper = new ModelOutputMapper();
                    modelMapper.ModelVersionID = newEditAnalyticVersionId;
                    modelMapper.Rows = mapper.Rows;
                    modelMapper.Activated = 1;
                    modelMapper.Columns = mapper.Columns;
                    modelMapper.ConfigString = mapper.ConfigString;
                    modelMapper.Title = mapper.Title;
                    modelMapper.Xaxis = mapper.Xaxis;
                    modelMapper.Yaxis = mapper.Yaxis;
                    modelMapper.MapperType = mapper.MapperType;
                    modelDb.ModelOutputMappers.Add(modelMapper);
                 numberOfMappersAdded+=  modelDb.SaveChanges();
                }
                
            }
            return numberOfMappersAdded;

        }

        public string GetOutputMapperHtml(long activeAnalyticVID, long newEditAnalyticVersionId)
        {
            StringBuilder mapperHtml = new StringBuilder();
            string rowStyle = "style='padding:3px;background:#DCE6F2;text-align:left;font-size:9pt;width:100%'";
            string plainRowStylePlain = "style='padding:3px;background:none;text-align:left;font-size:9pt;width:100%'";
            var mappers = modelDb.ModelOutputMappers.Where(x => x.ModelVersionID == activeAnalyticVID).ToList();
            if (mappers != null && mappers.Count > 0)
            {
                mapperHtml.AppendFormat("<div>");
                mapperHtml.AppendFormat("<table style='width:100%'>");
                int index = 0;

                foreach (ModelOutputMapper mapper in mappers)
                {
                    if (index % 2 == 0)
                    {
                        mapperHtml.Append("<tr style='background: none repeat scroll 0 0 #DCE6F2;'>");
                    }
                    else
                    {
                        mapperHtml.Append("<tr style='background:none'>");
                    }
                    if (index % 2 == 0)
                    {
                        mapperHtml.AppendFormat("<td "+rowStyle+"><input type='checkbox' id='{1}' title='{0}'/>{0}</td></tr>", mapper.Title, mapper.ID);
                    }
                    else
                    {
                        mapperHtml.AppendFormat("<td "+plainRowStylePlain+"><input type='checkbox' id='{1}' title='{0}'/>{0}</td></tr>", mapper.Title, mapper.ID);
                    }

                    index++;
                }

                mapperHtml.AppendFormat("</table>");
                mapperHtml.AppendFormat("</div>");
               
            }
            else
                mapperHtml.AppendFormat("<div><span>There are no Output Mappers associated with this previous version</span></div>");
         return mapperHtml.ToString();

        }

        public int CheckRelationByVersionId(long analyticVersionID)
        {
            int relation = 0;
            if (modelDb.ModelInputs.FirstOrDefault(x => x.ModelVersionID == analyticVersionID) != null)
            {
                relation = 1;
            }
            if (modelDb.ModelOutputMappers.FirstOrDefault(x => x.ModelVersionID == analyticVersionID) != null)
            {
                if (relation == 1)
                    relation = 3;
                else
                relation = 2;
            }

            return relation;

        }

        /// <summary>
        /// This function deletes the current version and activates the previous version and attaches all the runs/output mappers with previous version if any on user selection.
        /// </summary>
        /// <param name="analyticVersionID"></param>
        /// <param name="activatePrevious"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public int DeleteAnalyticByVersionId(long analyticVersionID, bool activatePrevious,string action)
        {
         
          
            ModelVersion vers = modelDb.ModelVersions.Find(analyticVersionID);
            int row = 0;
            long modelID = 0;
            if (vers != null)
            {
                modelID = vers.ModelID;
                if (activatePrevious)
                {
                    var allVersions = modelDb.ModelVersions.Where(x => x.ModelID == modelID).ToList();
                    int maxVersionNumber = 0;
                    int secondMaxVersionNumber = 0;
                    if (allVersions != null && allVersions.Count() > 0)
                    {
                                       
                        foreach (ModelVersion version in allVersions)
                        {
                            //wrong logic correct it.
                            if (version.VersionNumber > maxVersionNumber)
                            {
                                secondMaxVersionNumber = maxVersionNumber;
                                maxVersionNumber = version.VersionNumber;
                                
                            }
                            else if (version.VersionNumber > secondMaxVersionNumber)
                            {
                                secondMaxVersionNumber = version.VersionNumber;
                            }
                        }
                        if (!(secondMaxVersionNumber == 0))
                        {
                            ModelVersion analytic = modelDb.ModelVersions.Find(allVersions.FirstOrDefault(x => x.VersionNumber == secondMaxVersionNumber).ModelVersionID);
                            analytic.Model.CurrentActiveVersionID = allVersions.FirstOrDefault(x => x.VersionNumber == secondMaxVersionNumber).ModelVersionID;
                            analytic.Status = (int)AnalyticStatus.Active;


                            switch (action)
                            {
                                case "keepRun":
                                    if (modelDb.ModelInputs.FirstOrDefault(x => x.ModelVersionID == analyticVersionID) != null)
                                    {
                                        var runs = modelDb.ModelInputs.Where(x => x.ModelVersionID == analyticVersionID).ToList();
                                        foreach (ModelInput run in runs)
                                        {
                                            run.ModelVersionID = analytic.ModelVersionID;
                                        }
                                    }

                                    break;
                                case "keepMapper":
                                    if (modelDb.ModelOutputMappers.FirstOrDefault(x => x.ModelVersionID == analyticVersionID) != null)
                                    {
                                        var mappers = modelDb.ModelOutputMappers.Where(x => x.ModelVersionID == analyticVersionID).ToList();
                                        foreach (ModelOutputMapper mapper in mappers)
                                        {
                                            mapper.ModelVersionID = analytic.ModelVersionID;
                                        }

                                    }


                                    break;
                                case "keepBoth":
                                    if (modelDb.ModelInputs.FirstOrDefault(x => x.ModelVersionID == analyticVersionID) != null)
                                    {
                                        var runs = modelDb.ModelInputs.Where(x => x.ModelVersionID == analyticVersionID).ToList();
                                        foreach (ModelInput run in runs)
                                        {
                                            run.ModelVersionID = analytic.ModelVersionID;
                                        }
                                    }
                                    if (modelDb.ModelOutputMappers.FirstOrDefault(x => x.ModelVersionID == analyticVersionID) != null)
                                    {
                                        var mappers = modelDb.ModelOutputMappers.Where(x => x.ModelVersionID == analyticVersionID).ToList();
                                        foreach (ModelOutputMapper mapper in mappers)
                                        {
                                            mapper.ModelVersionID = analytic.ModelVersionID;
                                        }

                                    }
                                    break;
                            }
                        }
                        modelDb.SaveChanges();
                    }
                }
                Model model = modelDb.Models.FirstOrDefault(x => x.CurrentActiveVersionID == vers.ModelVersionID);
              
                modelDb.ModelVersions.Remove(vers);
                row = modelDb.SaveChanges();
                modelDb.SaveChanges();
            }
            return row; 
        }

        public int DeleteAnalyticByVersionId(long analyticVersionId)
        {
           
            ModelVersion vers = modelDb.ModelVersions.Find(analyticVersionId);

            if (vers != null)
            {
                //if ((AnalyticStatus)vers.Status == AnalyticStatus.Edit)
                //{
                //    return vers;
                //}
                //else
                //{
                //    vers = vers.Model.ModelVersions.FirstOrDefault(x => x.Status == (int)AnalyticStatus.Edit);
                //    return vers;
                //}
               modelDb.ModelVersions.Remove(vers);
              return modelDb.SaveChanges();
            }
            return 0;
        }

        public ModelVersion GetAnalyticForEditByVersionId(long analyticVersionId)
        {
           
            ModelVersion vers = modelDb.ModelVersions.Find(analyticVersionId);

            if (vers != null)
            {
                if ((AnalyticStatus)vers.Status == AnalyticStatus.Edit)
                {
                    return vers;
                }
                else
                {
                    vers = modelDb.ModelVersions.FirstOrDefault(x => x.ModelID == vers.ModelID && x.Status == (int)AnalyticStatus.Edit);
                    return vers;
                }
            }
            return null;
        }

        public ModelVersion GetAnalyticForEditByModelId(long modelId)
        {
          
            Model model =   modelDb.Models.Find(modelId);
            if (model != null)
            {
                ModelVersion vern = model.ModelVersions.FirstOrDefault(x => x.Status == (int)AnalyticStatus.Edit);
                return vern;
            }
            return null;
        }

        public ITAnalyticsBL.DB.Interpretation CreateNewInterpretation(string title, string description, string expression, long? copyAnalyticVernId)
        {
            ITAnalyticsBL.DB.Interpretation modelInterpretation = null;
            if (copyAnalyticVernId.HasValue)
            {
               
                modelInterpretation = new ITAnalyticsBL.DB.Interpretation();
                modelInterpretation.Title = title != null ? title : string.Empty;
                modelInterpretation.ModelVersionID = copyAnalyticVernId.Value;
                modelInterpretation.Expression = expression != null ? expression : string.Empty;
                modelInterpretation.Description = description != null ? description : string.Empty;
                modelInterpretation.InterpretationText = string.Empty;
                modelInterpretation.ExpressionXml = string.Empty;
                modelInterpretation.Scope = string.Empty;
                modelInterpretation.ScopeId = string.Empty;
                modelInterpretation.Activated = 0;
                modelDb.Interpretations.Add(modelInterpretation);
                modelDb.SaveChanges();
            }
            return modelInterpretation;
        }

        public ITAnalyticsBL.DB.ModelOutputMapper CreateNewRadioChart(string title, ITAnalyticsBL.Core.ChartIntegration integration, string sourceType,  long copyAnalyticVernId)
        {
            ITAnalyticsBL.DB.ModelOutputMapper modelMapper = null;
           
                modelMapper = new ITAnalyticsBL.DB.ModelOutputMapper();
                modelMapper.Title = title != null ? title : string.Empty;
                modelMapper.ModelVersionID = copyAnalyticVernId;
         
                ITAnalyticsBL.Core.RadioChart radioChart = new ITAnalyticsBL.Core.RadioChart();
                radioChart.MapperType = "RadioChart";
            
                radioChart.Title = title != null ? title : string.Empty;
                radioChart.ModelVersionId = copyAnalyticVernId;
                radioChart.RadioChartIntegration.ModelVersionId = copyAnalyticVernId;
                ITAnalyticsBL.Core.RadioChartRule rule1 = new Core.RadioChartRule();
                ITAnalyticsBL.Core.RadioChartRule rule2 = new Core.RadioChartRule();
                ITAnalyticsBL.Core.RadioChartRule rule3 = new Core.RadioChartRule();
                ITAnalyticsBL.Core.RadioChartRule rule4 = new Core.RadioChartRule();
                radioChart.RadioChartRules.Add(rule1);
                radioChart.RadioChartRules.Add(rule2);
                radioChart.RadioChartRules.Add(rule3);
                radioChart.RadioChartRules.Add(rule4);
                radioChart.RadioChartIntegration.RefType = sourceType;
               
                    radioChart.RadioChartIntegration.RefType = sourceType;
                    sLite.Analytic analyticObj = GetAnalytic(copyAnalyticVernId);
                    radioChart.RadioChartIntegration.RefId = analyticObj.ID;
                    rule1.RuleIntegration.DataType = "System.Double";
                    rule1.RuleIntegration.ModelVersionId = copyAnalyticVernId;
                    rule1.RuleIntegration.RefType = "ANALYTIC";
                    rule1.RuleIntegration.RefId = analyticObj.ID;
                    rule1.RuleIntegration.DataSource = (int)ChartIntegrationSource.Analytic;
                    rule1.Activated = 1;
                    rule1.Threshold = "300";
                    rule1.Toggle = "above";
                    rule1.FillColor = "#E01B5D";
                    rule2.RuleIntegration.DataType = "System.Double";
                    rule2.RuleIntegration.ModelVersionId = copyAnalyticVernId;
                    rule2.RuleIntegration.RefType = "ANALYTIC";
                    rule2.RuleIntegration.RefId = analyticObj.ID;
                    rule2.RuleIntegration.DataSource = (int)ChartIntegrationSource.Analytic;
                    rule2.Activated = 1;
                    rule2.Threshold = "300";
                    rule2.Toggle = "above";
                    rule2.FillColor = "#E01B5D";
                    rule3.RuleIntegration.DataType = "System.Double";
                    rule3.RuleIntegration.ModelVersionId = copyAnalyticVernId;
                    rule3.RuleIntegration.RefType = "ANALYTIC";
                    rule3.RuleIntegration.RefId = analyticObj.ID;
                    rule3.RuleIntegration.DataSource = (int)ChartIntegrationSource.Analytic;
                    rule3.Activated = 1;
                    rule3.Threshold = "300";
                    rule3.Toggle = "above";
                    rule3.FillColor = "#E01B5D";
                    rule4.RuleIntegration.DataType = "System.Double";
                    rule4.RuleIntegration.ModelVersionId = copyAnalyticVernId;
                    rule4.RuleIntegration.RefType = "ANALYTIC";
                    rule4.RuleIntegration.RefId = analyticObj.ID;
                    rule4.RuleIntegration.DataSource = (int)ChartIntegrationSource.Analytic;
                    rule4.Activated = 1;
                    rule4.Threshold = "300";
                    rule4.Toggle = "above";
                    rule4.FillColor = "#E01B5D";
                   sLite.DataIntegration dataIntegration = GetDataIntegration(copyAnalyticVernId);
            if(dataIntegration!=null && !string.IsNullOrEmpty(dataIntegration.ListName))
                    radioChart.RadioChartIntegration.TableName = dataIntegration.ListName;


                //    rule1.RuleIntegration.DataType = string.Empty;
                //    rule1.RuleIntegration.ModelVersionId = copyAnalyticVernId;
                //    rule1.RuleIntegration.RefType = "EXCEL";
                //    rule1.RuleIntegration.TableName = dataIntegration.ListName;
                //    rule1.RuleIntegration.IntegrationId = dataIntegration.IntegrationID;
                //    rule1.RuleIntegration.DataSource = (int)ChartIntegrationSource.Excel;

                //    rule2.RuleIntegration.DataType = string.Empty;
                //    rule2.RuleIntegration.ModelVersionId = copyAnalyticVernId;
                //    rule2.RuleIntegration.RefType = "EXCEL";
                //    rule2.RuleIntegration.TableName = dataIntegration.ListName;
                //    rule2.RuleIntegration.IntegrationId = dataIntegration.IntegrationID;
                //    rule2.RuleIntegration.DataSource = (int)ChartIntegrationSource.Excel;

                //    rule3.RuleIntegration.DataType = string.Empty;
                //    rule3.RuleIntegration.ModelVersionId = copyAnalyticVernId;
                //    rule3.RuleIntegration.RefType = "EXCEL";
                //    rule3.RuleIntegration.TableName = dataIntegration.ListName;
                //    rule3.RuleIntegration.IntegrationId = dataIntegration.IntegrationID;
                    
                //    rule3.RuleIntegration.DataSource = (int)ChartIntegrationSource.Excel;

                    
             
                string radioChartXmlString = string.Empty; 
                modelMapper.ConfigString = radioChartXmlString;
                modelMapper.Activated = 0;
                modelMapper.MapperType = "RadioChart";
                modelMapper.ModelVersionID = copyAnalyticVernId;
           
                modelMapper.Xaxis = string.Empty;
                modelMapper.Yaxis = string.Empty;
                modelDb.ModelOutputMappers.Add(modelMapper);              
                modelDb.SaveChanges();
                radioChart.OutputMapperId = modelMapper.ID;
                modelMapper.ConfigString = sLite.Helper.Serializer(radioChart, radioChart.GetType());
                modelDb.SaveChanges();
            return modelMapper;
        }


        public ITAnalyticsBL.DB.ModelOutputMapper CreateNewScorecardChart(string title, ITAnalyticsBL.Core.ChartIntegration integration, string sourceType, long copyAnalyticVernId)
        {
            ITAnalyticsBL.DB.ModelOutputMapper modelMapper = null;
            
            modelMapper = new ITAnalyticsBL.DB.ModelOutputMapper();
            modelMapper.Title = title != null ? title : string.Empty;
            modelMapper.ModelVersionID = copyAnalyticVernId;

            ITAnalyticsBL.Core.ScorecardChart scoreCard = new ITAnalyticsBL.Core.ScorecardChart();
            scoreCard.MapperType = "Scorecard";

            scoreCard.Title = title != null ? title : string.Empty;
            scoreCard.ModelVersionId = copyAnalyticVernId;
            //scoreCard.RadioChartIntegration.ModelVersionId = copyAnalyticVernId;
           
          
            //radioChart.RadioChartIntegration.RefType = sourceType;

            //radioChart.RadioChartIntegration.RefType = sourceType;
            sLite.Analytic analyticObj = GetAnalytic(copyAnalyticVernId);
            //scoreCard.RadioChartIntegration.RefId = analyticObj.ID;
            
            //sLite.DataIntegration dataIntegration = GetDataIntegration(copyAnalyticVernId);
            //if (dataIntegration != null && !string.IsNullOrEmpty(dataIntegration.ListName))
            //    radioChart.RadioChartIntegration.TableName = dataIntegration.ListName;          



            string radioChartXmlString = string.Empty;
            modelMapper.ConfigString = radioChartXmlString;
            modelMapper.Activated = 0;
            modelMapper.MapperType = "Scorecard";
            modelMapper.ModelVersionID = copyAnalyticVernId;

            modelMapper.Xaxis = string.Empty;
            modelMapper.Yaxis = string.Empty;
            modelDb.ModelOutputMappers.Add(modelMapper);
            modelDb.SaveChanges();
            scoreCard.OutputMapperId = modelMapper.ID;
            modelMapper.ConfigString = sLite.Helper.Serializer(scoreCard, scoreCard.GetType());
            modelDb.SaveChanges();
            return modelMapper;
        }


        public ITAnalyticsBL.DB.ModelOutputMapper CreateNewGauge(string title, ITAnalyticsBL.Core.ChartIntegration integration, string sourceType, long copyAnalyticVernId)
        {
            ITAnalyticsBL.DB.ModelOutputMapper modelMapper = null;
           
            modelMapper = new ITAnalyticsBL.DB.ModelOutputMapper();
            modelMapper.Title = title != null ? title : string.Empty;
            modelMapper.ModelVersionID = copyAnalyticVernId;

            ITAnalyticsBL.Core.GaugeChart gauge = new ITAnalyticsBL.Core.GaugeChart();
            gauge.MapperType = "Gauge";

            gauge.Title = title != null ? title : string.Empty;
            gauge.ModelVersionId = copyAnalyticVernId;
            //scoreCard.RadioChartIntegration.ModelVersionId = copyAnalyticVernId;


            //radioChart.RadioChartIntegration.RefType = sourceType;

            //radioChart.RadioChartIntegration.RefType = sourceType;
            sLite.Analytic analyticObj = GetAnalytic(copyAnalyticVernId);
            //scoreCard.RadioChartIntegration.RefId = analyticObj.ID;

            //sLite.DataIntegration dataIntegration = GetDataIntegration(copyAnalyticVernId);
            //if (dataIntegration != null && !string.IsNullOrEmpty(dataIntegration.ListName))
            //    radioChart.RadioChartIntegration.TableName = dataIntegration.ListName;          



            string radioChartXmlString = string.Empty;
            modelMapper.ConfigString = radioChartXmlString;
            modelMapper.Activated = 0;
            modelMapper.MapperType = "Gauge";
            modelMapper.ModelVersionID = copyAnalyticVernId;

            modelMapper.Xaxis = string.Empty;
            modelMapper.Yaxis = string.Empty;
            modelDb.ModelOutputMappers.Add(modelMapper);
            modelDb.SaveChanges();
            gauge.OutputMapperId = modelMapper.ID;
            modelMapper.ConfigString = sLite.Helper.Serializer(gauge, gauge.GetType());
            modelDb.SaveChanges();
            return modelMapper;
        }


        //public Dictionary<string, string> GetDataColumnList(string tableName)
        //{
        //    Dictionary<string, string> colList = new Dictionary<string, string>();
        //    try
        //    {
        //        ETInfoContext context = new ETInfoContext();
        //        DataTable dt = ETContext.GetDatatableByCriteria(tableName, String.Empty);
        //        if (dt.Columns["RowID"] != null)
        //            dt.Columns.Remove("RowID");

        //        WebGridColumn gridCol = null;

        //        foreach (DataColumn column in dt.Columns)
        //        {

        //            colList.Add(Convert.ToString(column.Ordinal), column.ColumnName);
        //        }
        //    }
        //    catch (Exception)
        //    {

        //    }
        //    return colList;
        //}

        public sLite.Analytic GetAnalytic(long analyticVId)
        {
           
            var model = (from m in modelDb.ModelVersions
                         where m.ModelVersionID == analyticVId
                         select m);
            XmlDocument modelXmlDoc = new XmlDocument();
            foreach (ModelVersion md in model)
            {

                modelXmlDoc.LoadXml(md.ModelXml);
            }
            sLite.Analytic analyticObj = new sLite.Analytic();
            analyticObj = (sLite.Analytic)(sLite.Helper.DeSerializer(modelXmlDoc.OuterXml, analyticObj.GetType()));
            return analyticObj;
        }
        public ModelVersion CreateNewAnalytic(string title, string desc, ModelCategory category, long? copyAnalyticVernId)
        {
            ModelVersion vern = new ModelVersion();
            if (copyAnalyticVernId.HasValue)
            {
                ModelVersion existVersion = modelDb.ModelVersions.Find(copyAnalyticVernId.Value);
                if (existVersion != null)
                {
                    XmlDocument analyticXml = new XmlDocument();
                    analyticXml.LoadXml(existVersion.ModelXml);
                    analyticXml.DocumentElement.Attributes[XmlTagLibrary.Title].Value = title;
                    vern.ParentID = 0;
                    vern.Status = (int)AnalyticStatus.Edit;
                    vern.Model = new Model();
                    vern.Model.Title = title;
                    vern.Model.Description = desc != null ? desc : string.Empty;
                    vern.Model.ModelCategoryID = category.ModelCategoryID;
                    vern.RevisionID = Guid.NewGuid();
                    vern.VersionNumber = 1;
                    vern.ModelXml = analyticXml.OuterXml;
                    return vern;
                }
                else
                {
                    return existVersion;
                }
            }
            vern.ParentID = 0;
            vern.Status = (int)AnalyticStatus.Edit;
            vern.Model = new Model();
            vern.Model.Title = title;
            vern.Model.Description = desc != null ? desc : string.Empty;
            vern.Model.ModelCategoryID = category.ModelCategoryID;
            vern.RevisionID = Guid.NewGuid();
            vern.VersionNumber = 1;
            vern.HelpXml = string.Empty;
            vern.ScoreSegmentXml = string.Empty;
            vern.ModelXml = string.Format("<Analytic  xmlns:xsi=\"http:///www.w3.org/2001/XMLSchema-instance\" ID='{0}' ><Name>{1}</Name><Weight>1000</Weight></Analytic>", Guid.NewGuid(), title); ;
            vern.HelpXml = string.Format("<AnalyticSupport  xmlns:xsi=\"http:///www.w3.org/2001/XMLSchema-instance\"><NodesInfo></NodesInfo></AnalyticSupport>");
            return vern;
        }

        public ModelVersion CreateNewVersionFrom(long analyticVersionId)
        {
            
            ModelVersion cVersion = modelDb.ModelVersions.Find(analyticVersionId);
            if (cVersion != null)
            {
                if (cVersion.Status == (int)AnalyticStatus.Edit)
                {
                    return cVersion;
                }

                ModelVersion newVersion = modelDb.ModelVersions.FirstOrDefault(x => x.ModelID == cVersion.ModelID && x.Status == (int)AnalyticStatus.Edit);
                if (newVersion != null)
                {
                    return newVersion;
                }

                ModelVersion analytic = modelDb.ModelVersions.Where(x => x.ModelID == cVersion.ModelID).OrderByDescending(x => x.VersionNumber).FirstOrDefault();
                if (analytic != null)
                {
                    newVersion = new ModelVersion();
                    newVersion.ModelXml = cVersion.ModelXml;
                    newVersion.HelpXml = cVersion.HelpXml;
                    newVersion.ScoreSegmentXml = string.Empty;
                    newVersion.Created = DateTime.UtcNow;
                    newVersion.Modified = DateTime.UtcNow;
                    newVersion.Comment = string.Empty;
                    newVersion.RevisionID = Guid.NewGuid();
                    newVersion.ModelID = cVersion.ModelID;
                    newVersion.VersionNumber = analytic.VersionNumber + 1;
                    newVersion.ParentID = cVersion.ParentID;
                    modelDb.ModelVersions.Add(newVersion);
                    modelDb.SaveChanges();
                    return newVersion;
                }
            }

            return null;
        }

        public string GetExistingSupportAnalyticXml(long analyticVersionId)
        {
          
            List<ModelVersion> versions = (from m in modelDb.ModelVersions
                                           where m.ModelVersionID == analyticVersionId
                                          select m ).ToList();
            if (versions.Count() > 0)
            {
                return versions[0].HelpXml;
            }
            return string.Empty;
        }

        public string GetExitingAnalyticAsSection(long analyticId)
        {
           

            List<ModelVersion> versions = (from m in modelDb.Models
                                           join v in modelDb.ModelVersions on m.CurrentActiveVersionID equals v.ModelVersionID
                                           where m.CurrentActiveVersionID == analyticId
                                           select v).ToList();
            if (versions.Count() > 0)
            {
                return GetCopyOfModelXml(versions[0].ModelVersionID);
            }
            return string.Empty;
        }

        public int GetCountAllAnalyticsInEditState()
        { 
            
            return modelDb.ModelVersions.Where(x => x.Status == (int)AnalyticStatus.Edit).Count();
        }

        public ModelVersion GetAnalyticByVID(long analyticVID)
        {
           
            return modelDb.ModelVersions.Find(analyticVID);
        }

        private string GetCopyOfModelXml(long analyticVersionId)
        {  
          
            ModelVersion modelVersion = modelDb.ModelVersions.Find(analyticVersionId);
            string modelXml = modelVersion != null ?  modelVersion.ModelXml : string.Empty;
            if (modelXml.Trim() == null)
            {
                return string.Empty;
            }

            XmlDocument modelDoc = new XmlDocument();
            modelDoc.LoadXml(modelXml);
            XmlElement rootElement = modelDoc.DocumentElement;
            XmlNodeList nodes = modelDoc.SelectNodes("///" + XmlTagLibrary.NODE);
            XmlNodeList innerModels = modelDoc.SelectNodes("///" + XmlTagLibrary.INNER_MODEL_DEFINITION);
            XmlNodeList outputDefinitions = modelDoc.SelectNodes("///" + XmlTagLibrary.OUTPUT_DEFINITIONS + "///" + XmlTagLibrary.OUTPUT_DEFINITION);
            foreach (XmlNode node in nodes)
            {
                ((XmlElement)node).SetAttribute(XmlTagLibrary.ID, "node-" + Guid.NewGuid());
            }
            foreach (XmlNode innerModel in innerModels)
            {
                ((XmlElement)innerModel).SetAttribute(XmlTagLibrary.NAME, "model-" + Guid.NewGuid());
            }
            rootElement.SetAttribute(XmlTagLibrary.OpDefId, "");
            foreach (XmlNode outputDefinition in outputDefinitions)
            {
                String id = ((XmlElement)outputDefinition).GetAttribute(XmlTagLibrary.ID);
                if (id != null && id != string.Empty)
                {
                    XmlNodeList parents = modelDoc.SelectNodes("///*[(@" + XmlTagLibrary.OpDefId + "='" + id + "') or (@" + XmlTagLibrary.ChildOpDefId + "='" + id + "')]");
                    if (parents.Count > 0)
                    {
                        Guid newID = Guid.NewGuid();
                        ((XmlElement)outputDefinition).SetAttribute(XmlTagLibrary.ID, newID.ToString());
                        foreach (XmlNode parent in parents)
                        {
                            XmlElement parentElement = parent as XmlElement;
                            string opDefId = parentElement.GetAttribute(XmlTagLibrary.OpDefId);
                            if (opDefId == id)
                            {
                                parentElement.SetAttribute(XmlTagLibrary.OpDefId, newID.ToString());
                            }
                            string childOpDefId = parentElement.GetAttribute(XmlTagLibrary.ChildOpDefId);
                            if (childOpDefId == id)
                            {
                                parentElement.SetAttribute(XmlTagLibrary.ChildOpDefId, newID.ToString());
                            }
                        }
                    }
                    else
                    {
                        outputDefinition.ParentNode.RemoveChild(outputDefinition);
                    }
                }
                else
                {
                    outputDefinition.ParentNode.RemoveChild(outputDefinition);
                }
            }
            SyncHelpTexts(modelDoc);
            XmlDocument innerModelDoc = new XmlDocument();
            string innerModelTitle = rootElement.GetAttribute(XmlTagLibrary.Title);
            string cOpDefId = rootElement.GetAttribute(XmlTagLibrary.ChildOpDefId);
            if (innerModelTitle == null || innerModelTitle == string.Empty)
            {
                innerModelTitle = "Model";
            }
            XmlElement modelElement = innerModelDoc.CreateElement(XmlTagLibrary.INNER_MODEL_DEFINITION);
            modelElement.SetAttribute(XmlTagLibrary.NAME, "model-" + Guid.NewGuid());
            modelElement.SetAttribute(XmlTagLibrary.Title, innerModelTitle);
            modelElement.SetAttribute(XmlTagLibrary.WEIGHT, "1");
            if (cOpDefId != null && cOpDefId != string.Empty)
            {
                modelElement.SetAttribute(XmlTagLibrary.ChildOpDefId, cOpDefId);
            }
            string rHelpTextId = rootElement.GetAttribute(XmlTagLibrary.HelpTextId);
            if (rHelpTextId != null && rHelpTextId != string.Empty)
            {
                modelElement.SetAttribute(XmlTagLibrary.HelpTextId, rHelpTextId);
            }
            modelElement.InnerXml = rootElement.InnerXml;
            return modelElement.OuterXml;
        }

        private void SyncHelpTexts(XmlDocument modelDoc)
        {
            XmlNodeList helpTexts = modelDoc.SelectNodes("///" + XmlTagLibrary.HelpTexts + "///" + XmlTagLibrary.HelpText);
            foreach (XmlNode helpText in helpTexts)
            {
                string id = ((XmlElement)helpText).GetAttribute(XmlTagLibrary.ID);
                if (id != null && id != string.Empty)
                {
                    XmlNodeList parents = modelDoc.SelectNodes("///*[(@" + XmlTagLibrary.HelpTextId + "='" + id + "')]");
                    if (parents.Count > 0)
                    {
                        Guid newID = Guid.NewGuid();
                        ((XmlElement)helpText).SetAttribute(XmlTagLibrary.ID, newID.ToString());
                        foreach (XmlNode parent in parents)
                        {
                            XmlElement parentElement = parent as XmlElement;
                            string helpTextId = parentElement.GetAttribute(XmlTagLibrary.HelpTextId);
                            if (helpTextId == id)
                            {
                                parentElement.SetAttribute(XmlTagLibrary.HelpTextId, newID.ToString());
                            }
                        }
                    }
                    else
                    {
                        helpText.ParentNode.RemoveChild(helpText);
                    }
                }
                else
                {
                    helpText.ParentNode.RemoveChild(helpText);
                }
            }
        }
    }
}

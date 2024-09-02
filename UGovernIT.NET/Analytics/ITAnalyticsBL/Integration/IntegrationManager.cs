using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ITAnalyticsBL.DB;
using ITAnalyticsUtility;
using System.Data;
using uGovernIT.Manager;

namespace ITAnalyticsBL.Integration
{
    public class IntegrationManager
    {
        ModelDB modelDb;
        public ApplicationContext Context { get; set; }

        public List<DataIntegration> DataIntegrations;
        public IntegrationManager(ModelDB modelDb)
        {
            DefaultList();

            this.modelDb = modelDb;
        }

        private void DefaultList()
        {
            DataIntegrations = new List<DataIntegration>();
            DataIntegrations.Add(new DataIntegration() { DataIntegrationID = 1, Name = "Table", SourceType = (int)DataIntegrationType.Table });
            DataIntegrations.Add(new DataIntegration() { DataIntegrationID = 2, Name = "Query", SourceType = (int)DataIntegrationType.Query });
            DataIntegrations.Add(new DataIntegration() { DataIntegrationID = 3, Name = "ET", SourceType = (int)DataIntegrationType.ET });
            //DataIntegrations.Add(new DataIntegration() { DataIntegrationID = 1, Name = "SharePoint", SourceType = 0 });

        }
        public List<DataIntegration> GetInegrationConfigs()
        {
            return DataIntegrations;
        }


        public DataIntegration GetIntegrationConfigByPublicKey(string publicKey)
        {
            if (!string.IsNullOrWhiteSpace(publicKey))
            {
                return DataIntegrations.FirstOrDefault(x => x.PublicKey == publicKey);
            }
            return null;
        }

        // <summary>
        // Load list without field info which will be used in intration
        // </summary>
        // <param name="id">data integration id</param>
        // <returns>List which will be used in integration from given integration id</returns>
        public List<ListDetail> LoadAllListByIntegrationId(int id)
        {
            return LoadAllList(id, false, string.Empty, false);
        }

        // <summary>
        // Load List which will be used in integration
        // </summary>
        // <param name="id">data integration id</param>
        // <param name="includeField">do you require all fields</param>
        // <returns>List which will be used in integration from given integration id</returns>
        private List<ListDetail> LoadAllList(int id, bool includeFields, string integratToList, bool showSpecifiedListOnly)
        {
            List<ListDetail> lists = new List<ListDetail>();
            DataIntegration integrationConfig = DataIntegrations.FirstOrDefault(x => x.DataIntegrationID == id);
            if (integrationConfig != null)
            {
                integrationConfig.Context = this.Context;
                if (integrationConfig.SourceType == (int)DataIntegrationType.SharePoint)
                {
                    IETIntegration integration = new SharepointIntegration();
                    lists = integration.LoadAllList(integrationConfig, includeFields, integratToList, showSpecifiedListOnly);
                }
                else if (integrationConfig.SourceType == (int)DataIntegrationType.ET)
                {
                    IETIntegration integration = new ETTableIntegration();

                    lists = integration.LoadAllList(integrationConfig, includeFields, integratToList, showSpecifiedListOnly);
                }
                else if (integrationConfig.SourceType == (int)DataIntegrationType.Table)
                {
                    IETIntegration integration = new TableIntegration();

                    lists = integration.LoadAllList(integrationConfig, includeFields, integratToList, showSpecifiedListOnly);
                }
                else if (integrationConfig.SourceType == (int)DataIntegrationType.Query)
                {
                    IETIntegration integration = new QueryIntegration();
                    lists = integration.LoadAllList(integrationConfig, includeFields, integratToList, showSpecifiedListOnly);
                }
                else if (integrationConfig.SourceType == (int)DataIntegrationType.Survey)
                {

                    foreach (Survey item in modelDb.Surveys)
                    {


                        if (!item.IsDeleted)
                        {
                            //if (item.Name.ToLower() == integratToList.ToLower())
                            {
                                ListDetail lstDetail = new ListDetail();
                                lstDetail.ListId = item.SurveyID.ToString();
                                lstDetail.ListName = item.Name;


                                List<Question> questions = modelDb.Questions.Where(x => x.SurveyId == item.SurveyID).ToList();
                                foreach (Question q in questions)
                                {
                                    if (q != null)
                                    {
                                        if (q.QuestionType == "Table")
                                        {
                                            //tablecolumns=Old~~Somewhat Old~~Relatively New~~New"
                                            if (!string.IsNullOrEmpty(q.QuestionTypeProperties))
                                            {
                                                string[] vals = q.QuestionTypeProperties.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries)[1].Split(new string[] { "~~" }, StringSplitOptions.RemoveEmptyEntries);
                                                foreach (string s in vals)
                                                {
                                                    FieldDetail fDetail = new FieldDetail();
                                                    fDetail.DisplayName = string.Format("{0}_{1}", q.Token.Trim(), s.Trim().Replace(" ", "_"));
                                                    fDetail.DataType = "string";
                                                    fDetail.InternalName = string.Format("{0}_{1}", q.Token.Trim(), s.Trim().Replace(" ", "_"));
                                                    fDetail.InternalNameWithType = string.Format("{0}_{1}", q.Token.Trim(), s.Trim().Replace(" ", "_")) + string.Format("({0} Columns)", q.QuestionType);// "(string)";
                                                    if (lstDetail.Fields == null)
                                                        lstDetail.Fields = new List<FieldDetail>();
                                                    lstDetail.Fields.Add(fDetail);
                                                }

                                            }
                                        }
                                        else
                                        {
                                            FieldDetail fDetail = new FieldDetail();
                                            fDetail.DisplayName = q.Token.Trim();
                                            fDetail.DataType = "string";
                                            fDetail.InternalName = q.Token;
                                            fDetail.InternalNameWithType = q.Token.Trim() + string.Format("({0})", q.QuestionType);// "(string)";
                                            if (lstDetail.Fields == null)
                                                lstDetail.Fields = new List<FieldDetail>();
                                            lstDetail.Fields.Add(fDetail);
                                        }
                                    }
                                }
                                // lstDetail.Fields.
                                lists.Add(lstDetail);
                            }
                        }
                    }
                }
            }

            return lists;
        }

        public ListDetail LoadListByListName(int id, string integratToList)
        {
            List<ListDetail> lists = LoadAllList(id, true, integratToList, true);
            if (lists.Count > 0)
            {

                return lists.FirstOrDefault(x => x.ListName == integratToList);
            }
            return null;
        }

        public List<ListDetail> LoadInteragedList(int id, bool includeFields, string integratToList, bool showSpecifiedListOnly)
        {
            List<ListDetail> lists = LoadAllList(id, includeFields, integratToList, showSpecifiedListOnly);
            return lists;
        }

        public List<IDOutput> GetFieldValuesByParam(int id, string selectionCriteria, List<IDInputParam> parms)
        {
            List<IDOutput> outputs = new List<IDOutput>();
            DataIntegration integrationConfig = DataIntegrations.FirstOrDefault(x => x.DataIntegrationID == id);
            if (integrationConfig != null)
            {
                integrationConfig.Context = this.Context;
                if (integrationConfig.SourceType == (int)DataIntegrationType.SharePoint)
                {
                    IETIntegration integration = new SharepointIntegration();
                    outputs = integration.GetFieldValuesByParam(integrationConfig, selectionCriteria, parms);
                }
                else if (integrationConfig.SourceType == (int)DataIntegrationType.ET)
                {
                    IETIntegration integration = new ETTableIntegration();
                    outputs = integration.GetFieldValuesByParam(integrationConfig, selectionCriteria, parms);
                }
                else if (integrationConfig.SourceType == (int)DataIntegrationType.Table)
                {
                    IETIntegration integration = new TableIntegration();
                    outputs = integration.GetFieldValuesByParam(integrationConfig, selectionCriteria, parms);
                }
                else if (integrationConfig.SourceType == (int)DataIntegrationType.Query)
                {
                    IETIntegration integration = new QueryIntegration();
                    outputs = integration.GetFieldValuesByParam(integrationConfig, selectionCriteria, parms);
                }
            }
            return outputs;
        }


        public List<string> GetFieldValues(int config, string etTable, string column)
        {
            List<string> lstFieldDetails = new List<string>();
            DataIntegration integrationConfig = DataIntegrations.FirstOrDefault(x => x.DataIntegrationID == config);
            if (integrationConfig != null)
            {
                integrationConfig.Context = this.Context;
                if (integrationConfig.SourceType == (int)DataIntegrationType.ET)
                {
                    IETIntegration integration = new ETTableIntegration();
                    lstFieldDetails = integration.GetFieldValues(integrationConfig, etTable, column);
                }
            }
            return lstFieldDetails;
        }

        public DataTable GetETTable(int config, string etTable)
        {
            DataTable EtTable = new DataTable();
            DataIntegration integrationConfig = DataIntegrations.FirstOrDefault(x => x.DataIntegrationID == config);
            if (integrationConfig != null)
            {
                integrationConfig.Context = this.Context;
                if (integrationConfig.SourceType == (int)DataIntegrationType.ET)
                {
                    IETIntegration integration = new ETTableIntegration();
                    EtTable = integration.GetETTable(integrationConfig, etTable);

                }
                else if (integrationConfig.SourceType == (int)DataIntegrationType.SharePoint)
                {
                    IETIntegration integration = new SharepointIntegration();
                    EtTable = integration.GetETTable(integrationConfig, etTable);
                }
                else if (integrationConfig.SourceType == (int)DataIntegrationType.Table)
                {
                    IETIntegration integration = new TableIntegration();
                    EtTable = integration.GetETTable(integrationConfig, etTable);
                }
                else if (integrationConfig.SourceType == (int)DataIntegrationType.Query)
                {
                    IETIntegration integration = new QueryIntegration();
                    EtTable = integration.GetETTable(integrationConfig, etTable);
                }
            }
            return EtTable;
        }

        public DataTable GetETTable(int config, string etTable, string criteria)
        {
            DataTable result = GetETTable(config, etTable);
            if (result == null)
                return new DataTable();

            if (!string.IsNullOrEmpty(criteria))
            {
                return result.Select(criteria).CopyToDataTable();
            }
            else
            {
                return result;
            }
        }

    }
}

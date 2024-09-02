using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ITAnalyticsBL.DB;
using ITAnalyticsBL.Core;
using ITAnalyticsBL.ET;
using System.Net;
using System.Data;
using System.Threading;

namespace ITAnalyticsBL.Integration
{
    public class SyncFactTable
    {


        // <summary>
        // Fills the Fact table from the source table
        // </summary>
        // <param name="ETTable">Fact Table</param>
        public static void SyncETTable(ModelDB modelDb, object ETTable1)
        {
            string ETTable = (string)ETTable1;
            // it instantiates the object  of that fact table which is passed as parameter.
            ETTable etTable = modelDb.ETTables.FirstOrDefault(x => x.TableName == ETTable);
            //it stores the field schema and other corresponding details of the fields.
            List<ETSchemaInfo> etSchema = etTable.ETSchemaInfoes;
            //it stores the actual fact table as datatable.
            DataTable dt = ETContext.GetDatatableByCriteria(modelDb.GetDBContext().TenantID, ETTable);
            dt.TableName = ETTable;
            string primaryKey = string.Empty;

            //it groups the non-primary key field schema firstly on the basis of dataintegrationId and then again on the basis of source table.
            var nonPrimaryFieldsGroup = from sti in etSchema
                                        where sti.PrimaryKey == false
                                        group sti by sti.DataIntegrationID into grp
                                        select
                                            new { key = grp.Key, grp2 = from u in grp group u by u.SourceTable into p select p };

            // it instantiate the object of that field which is selected as primary key.
            ETSchemaInfo primaryKeyField = etSchema.FirstOrDefault(x => x.PrimaryKey);
            //Fills the Primary key field 
            FillPrimaryField(modelDb, primaryKeyField, dt);

            // it iterates the nonPrimaryFieldsGroup firstly on the basis of DataIntegrationId and again it iterates the dataintegraionId group on the basis of source table.
            foreach (var obj in nonPrimaryFieldsGroup)
            {
                foreach (var di in obj.grp2)
                {
                    SyncFactTable.FillNonPrimaryKey(modelDb, di.ToList(), primaryKeyField, dt);


                    // FillNonPrimaryKey(di.ToList(), primaryKeyField, dt);
                }
            }

            //assigning the fact table name to datatable 

            // finally fills the data 
            ETContext.FillData(dt);
            ETTable factTable = modelDb.ETTables.FirstOrDefault(x => x.TableName == ETTable);
            if (factTable != null)
            {
                factTable.Status = (byte)ETTableStatus.Completed;
                factTable.LastUpdated = DateTime.Now;
                modelDb.SaveChanges();

            }

        }

        // <summary>
        // Fills the Primary key value from the source table.
        // </summary>
        // <param name="etFieldSchema"></param>
        // <param name="factTable"></param>
        public static void FillPrimaryField(ModelDB modelDb, ETSchemaInfo etFieldSchema, DataTable factTable)
        {
            DataTable dtTable = new DataTable();

            // instantiates the object of datatable on the basis of integrationId and sourceTable
            //if (etFieldSchema.DataIntegration.SourceType == (int)DataIntegrationType.ET)
            //{
            IntegrationManager integrationManager = new IntegrationManager(modelDb);
            dtTable = integrationManager.GetETTable((short)etFieldSchema.DataIntegrationID, etFieldSchema.SourceTable);
            //}

            // filter the unique column value which is passed as parameter from the datatable .
            if (dtTable == null || dtTable.Rows.Count <= 0)
            {
                return;
            }

            dtTable = dtTable.DefaultView.ToTable(true, etFieldSchema.AliasName);
            for (int i = 0; i < dtTable.Rows.Count; i++)
            {
                // creates the new row in fact table and fills the alias name column of the fact table from the actual field value of data table.
                DataRow dr = factTable.NewRow();
                factTable.Rows.Add(dr);
                dr[etFieldSchema.AliasName] = dtTable.Rows[i][etFieldSchema.FieldName];

            }
        }
        // <summary>
        // Fills the Non Primary Key value from the source table.
        // </summary>
        // <param name="etFieldsSchema">List of Fields selected and their corresponding details.</param>
        // <param name="primaryFieldSchema">field schema of Primary</param>
        // <param name="factTable">Fact Table</param>
        public static void FillNonPrimaryKey(ModelDB modelDb, List<ETSchemaInfo> etFieldsSchema, ETSchemaInfo primaryFieldSchema, DataTable factTable)
        {
            DataTable dt = new DataTable();
            // instantiates the object of datatable on the basis of integrationId and sourceTable
            if (etFieldsSchema.Count > 0)
            {
                IntegrationManager integrationManager = new IntegrationManager(modelDb);
                dt = integrationManager.GetETTable((short)etFieldsSchema[0].DataIntegrationID, etFieldsSchema[0].SourceTable);
            }

            if (dt == null || dt.Rows.Count <= 0)
            {
                return;
            }

            foreach (ETSchemaInfo etFieldSchema in etFieldsSchema)
            {
                // filter the unique column value which is passed as parameter from the datatable .
                DataTable groupDT = dt.DefaultView.ToTable(true, etFieldSchema.ForeignKey);

                foreach (DataRow groupRow in groupDT.Rows)
                {
                    // it filters rows from fact table whose primary key field equals the foreign key field.
                    DataRow[] selectedRows = factTable.Select(string.Format("{0}='{1}'", primaryFieldSchema.AliasName, groupRow[etFieldSchema.ForeignKey]));
                    if (selectedRows.Length > 0)
                    {
                        string whereClause = string.Empty;
                        if (!(string.IsNullOrWhiteSpace(etFieldSchema.FieldConstraint)))
                        {
                            List<FormulaExpression> expression = FormulaExpression.ParseFormulaExpressions(etFieldSchema.FieldConstraint);//parse the formula in the sql format.

                            if (expression != null && expression.Count > 0)
                            {
                                whereClause = FormulaExpression.ConvertSQLWhereClause(expression);
                            }
                        }
                        selectedRows[0][etFieldSchema.AliasName] = dt.Compute(string.Format("{0}({1})", etFieldSchema.AggregateFunction, etFieldSchema.FieldName), string.Format("{0}='{1}' {2}", etFieldSchema.ForeignKey, groupRow[etFieldSchema.ForeignKey], string.IsNullOrWhiteSpace(whereClause) ? string.Empty : "and " + whereClause.Trim()));
                    }
                }
            }
        }
    }
}

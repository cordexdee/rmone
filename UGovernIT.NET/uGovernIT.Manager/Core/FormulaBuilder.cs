using NCalc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using uGovernIT.Util.Log;
using uGovernIT.Utility;

namespace uGovernIT.Manager.Core
{
    public class FormulaBuilder
    {

        /// <summary>
        ///  It parse the formula expression into list of formula expression
        /// </summary>
        /// <param name="formulaId"></param>
        /// <param name="formulaTable"></param>
        /// <param name="filterTable"></param>
        /// <returns></returns>
        public static List<FormulaExpression> ParseFormula(int formulaId, DataTable formulaTable)
        {
            List<FormulaExpression> expressions = new List<FormulaExpression>();
            if (formulaTable != null)
            {
                DataRow row = formulaTable.AsEnumerable().FirstOrDefault(x => x.Field<int>(DatabaseObjects.Columns.Id) == formulaId);
                if (row != null && row[DatabaseObjects.Columns.FormulaValue] != null)
                {
                    expressions = ParseFormula(Convert.ToString(row[DatabaseObjects.Columns.FormulaValue]));
                }
            }
            return expressions;
        }

        public static bool CheckFormulaValidation(ApplicationContext pContext, int formulaId, DataRow item)
        {
            //Get All Expressions
            List<FormulaExpression> expressions = ParseFormula(formulaId);

            bool overallValid = true;

            FormulaBuilder.ConvertFormulaIntoDataTableFormula(ref expressions);
            //Loop Through all expressinons and match with current Item column
            foreach (FormulaExpression expression in expressions)
            {
                bool valid = false;
                //Column name to look for in Item
                string columnName = Convert.ToString(expression.Variable);
                if (columnName != string.Empty && UGITUtility.IfColumnExists(columnName, item.Table))
                {
                    string itemValue = string.Empty;
                    string expressionValue = expression.Value;

                    if (item.Table.Columns[columnName].DataType.ToString() == DatabaseObjects.DataTypes.Lookup)
                        itemValue = UGITUtility.SplitString(item[columnName], Constants.Separator, 1);
                    else if (item.Table.Columns[columnName].DataType.ToString() == DatabaseObjects.DataTypes.DateTime)
                    {
                        itemValue = Convert.ToString(item[columnName]);
                        expressionValue = ExpressionCalc.ExecuteFunctions(pContext, expression.Value);
                    }
                    else
                        itemValue = Convert.ToString(item[columnName]);


                    // If boolean, normalize all values to standard
                    if (item.Table.Columns[columnName].DataType.ToString() == DatabaseObjects.DataTypes.Boolean)
                    {
                        expressionValue = UGITUtility.StringToBoolean(expression.Value).ToString();
                        itemValue = UGITUtility.StringToBoolean(itemValue).ToString();
                    }

                    DateTime itemDate = DateTime.MinValue;
                    DateTime expressionDate = DateTime.MinValue;
                    DateTime.TryParse(Convert.ToString(item[columnName]), out itemDate);
                    DateTime.TryParse(expressionValue, out expressionDate);
                    double itemVal_Num = 0;
                    double expressionVal_Num = 0;
                    double.TryParse(Convert.ToString(item[columnName]), out itemVal_Num);
                    double.TryParse(expressionValue, out expressionVal_Num);

                    switch (expression.Operator)
                    {
                        case "=":
                            if (item.Table.Columns[columnName].DataType.ToString() == DatabaseObjects.DataTypes.DateTime)
                            {
                                valid = (itemDate == expressionDate);
                            }
                            else if (item.Table.Columns[columnName].DataType.ToString() == DatabaseObjects.DataTypes.Number)
                            {
                                valid = (itemVal_Num == expressionVal_Num);
                            }
                            else
                            {
                                if (string.Compare(itemValue, expressionValue, true) == 0)
                                    valid = true;
                                else
                                    valid = false;
                            }
                            break;
                        case "<>": // Support both forms of not equal to
                        case "!=":
                            if (item.Table.Columns[columnName].DataType.ToString() == DatabaseObjects.DataTypes.DateTime)
                            {
                                valid = (itemDate != expressionDate);
                            }
                            else if (item.Table.Columns[columnName].DataType.ToString() == DatabaseObjects.DataTypes.Number)
                            {
                                valid = (itemVal_Num != expressionVal_Num);
                            }
                            else
                            {
                                if (string.Compare(itemValue, expressionValue, true) != 0)
                                    valid = true;
                                else
                                    valid = false;
                            }
                            break;
                        case ">":
                            if (item.Table.Columns[columnName].DataType.ToString() == DatabaseObjects.DataTypes.DateTime)
                            {
                                valid = (itemDate > expressionDate);
                            }
                            else if (item.Table.Columns[columnName].DataType.ToString() == DatabaseObjects.DataTypes.Number)
                            {
                                valid = (itemVal_Num > expressionVal_Num);
                            }
                            else
                            {
                                if (string.Compare(itemValue, expressionValue, true) > 0)
                                    valid = true;
                                else
                                    valid = false;
                            }
                            break;
                        case ">=":
                            if (item.Table.Columns[columnName].DataType.ToString() == DatabaseObjects.DataTypes.DateTime)
                            {
                                valid = (itemDate == expressionDate);
                            }
                            else if (item.Table.Columns[columnName].DataType.ToString() == DatabaseObjects.DataTypes.Number)
                            {
                                valid = (itemVal_Num >= expressionVal_Num);
                            }
                            else
                            {
                                if (string.Compare(itemValue, expressionValue, true) > 0 || string.Compare(itemValue, expressionValue, true) == 0)
                                    valid = true;
                                else
                                    valid = false;
                            }
                            break;
                        case "<=":
                            if (item.Table.Columns[columnName].DataType.ToString() == DatabaseObjects.DataTypes.DateTime)
                            {
                                valid = (itemDate == expressionDate);
                            }
                            else if (item.Table.Columns[columnName].DataType.ToString() == DatabaseObjects.DataTypes.Number)
                            {
                                valid = (itemVal_Num <= expressionVal_Num);
                            }
                            else
                            {
                                if (string.Compare(itemValue, expressionValue, true) < 0 || string.Compare(itemValue, expressionValue, true) == 0)
                                    valid = true;
                                else
                                    valid = false;
                            }
                            break;
                        case "<":
                            if (item.Table.Columns[columnName].DataType.ToString() == DatabaseObjects.DataTypes.DateTime)
                            {
                                valid = (itemDate == expressionDate);
                            }
                            else if (item.Table.Columns[columnName].DataType.ToString() == DatabaseObjects.DataTypes.Number)
                            {
                                valid = (itemVal_Num < expressionVal_Num);
                            }
                            else
                            {
                                if (string.Compare(itemValue, expressionValue, true) < 0)
                                    valid = true;
                                else
                                    valid = false;
                            }
                            break;

                        case "isnull":
                            if (!UGITUtility.IsSPItemExist(item, columnName))
                                valid = true;
                            else
                                valid = false;

                            break;
                        case "isnotnull":
                            if (UGITUtility.IsSPItemExist(item, columnName))
                                valid = true;
                            else
                                valid = false;
                            break;
                        default:
                            valid = false;
                            break;

                    }
                }
                switch (expression.AppendWith)
                {
                    case "Or":
                        overallValid = valid || overallValid;
                        break;
                    case "And":
                        overallValid = valid & overallValid;
                        break;
                    default:
                        overallValid = valid;
                        break;
                }

            }
            //Return 
            return overallValid;
        }

        public static bool CheckFormulaValidation(ApplicationContext pContext, string formulaExpression, DataRow item)
        {
            //Get All Expressions
            List<FormulaExpression> expressions = ParseFormula(formulaExpression);

            bool overallValid = true;

            FormulaBuilder.ConvertFormulaIntoDataTableFormula(ref expressions);
            //Loop Through all expressinons and match with current Item column
            foreach (FormulaExpression expression in expressions)
            {
                bool valid = false;
                //Column name to look for in Item
                string columnName = Convert.ToString(expression.Variable);
                if (columnName != string.Empty && UGITUtility.IfColumnExists(columnName, item.Table))
                {
                    string itemValue = string.Empty;
                    string expressionValue = expression.Value;

                    if (item.Table.Columns[columnName].DataType.ToString() == DatabaseObjects.DataTypes.Lookup)
                        itemValue = UGITUtility.SplitString(item[columnName], Constants.Separator, 1);
                    else if (item.Table.Columns[columnName].DataType.ToString() == DatabaseObjects.DataTypes.DateTime)
                    {
                        itemValue = Convert.ToString(item[columnName]);
                        expressionValue = ExpressionCalc.ExecuteFunctions(pContext, expression.Value);
                    }
                    else
                        itemValue = Convert.ToString(item[columnName]);


                    // If boolean, normalize all values to standard
                    if (item.Table.Columns[columnName].DataType.ToString() == DatabaseObjects.DataTypes.Boolean)
                    {
                        expressionValue = UGITUtility.StringToBoolean(expression.Value).ToString();
                        itemValue = UGITUtility.StringToBoolean(itemValue).ToString();
                    }

                    DateTime itemDate = DateTime.MinValue;
                    DateTime expressionDate = DateTime.MinValue;
                    DateTime.TryParse(Convert.ToString(item[columnName]), out itemDate);
                    DateTime.TryParse(expressionValue, out expressionDate);
                    double itemVal_Num = 0;
                    double expressionVal_Num = 0;
                    double.TryParse(Convert.ToString(item[columnName]), out itemVal_Num);
                    double.TryParse(expressionValue, out expressionVal_Num);

                    switch (expression.Operator)
                    {
                        case "=":
                            if (item.Table.Columns[columnName].DataType.ToString() == DatabaseObjects.DataTypes.DateTime)
                            {
                                valid = (itemDate == expressionDate);
                            }
                            else if (item.Table.Columns[columnName].DataType.ToString() == DatabaseObjects.DataTypes.Number)
                            {
                                valid = (itemVal_Num == expressionVal_Num);
                            }
                            else
                            {
                                if (string.Compare(itemValue, expressionValue, true) == 0)
                                    valid = true;
                                else
                                    valid = false;
                            }
                            break;
                        case "<>": // Support both forms of not equal to
                        case "!=":
                            if (item.Table.Columns[columnName].DataType.ToString() == DatabaseObjects.DataTypes.DateTime)
                            {
                                valid = (itemDate != expressionDate);
                            }
                            else if (item.Table.Columns[columnName].DataType.ToString() == DatabaseObjects.DataTypes.Number)
                            {
                                valid = (itemVal_Num != expressionVal_Num);
                            }
                            else
                            {
                                if (string.Compare(itemValue, expressionValue, true) != 0)
                                    valid = true;
                                else
                                    valid = false;
                            }
                            break;
                        case ">":
                            if (item.Table.Columns[columnName].DataType.ToString() == DatabaseObjects.DataTypes.DateTime)
                            {
                                valid = (itemDate > expressionDate);
                            }
                            else if (item.Table.Columns[columnName].DataType.ToString() == DatabaseObjects.DataTypes.Number)
                            {
                                valid = (itemVal_Num > expressionVal_Num);
                            }
                            else
                            {
                                if (string.Compare(itemValue, expressionValue, true) > 0)
                                    valid = true;
                                else
                                    valid = false;
                            }
                            break;
                        case ">=":
                            if (item.Table.Columns[columnName].DataType.ToString() == DatabaseObjects.DataTypes.DateTime)
                            {
                                valid = (itemDate == expressionDate);
                            }
                            else if (item.Table.Columns[columnName].DataType.ToString() == DatabaseObjects.DataTypes.Number)
                            {
                                valid = (itemVal_Num >= expressionVal_Num);
                            }
                            else
                            {
                                if (string.Compare(itemValue, expressionValue, true) > 0 || string.Compare(itemValue, expressionValue, true) == 0)
                                    valid = true;
                                else
                                    valid = false;
                            }
                            break;
                        case "<=":
                            if (item.Table.Columns[columnName].DataType.ToString() == DatabaseObjects.DataTypes.DateTime)
                            {
                                valid = (itemDate == expressionDate);
                            }
                            else if (item.Table.Columns[columnName].DataType.ToString() == DatabaseObjects.DataTypes.Number)
                            {
                                valid = (itemVal_Num <= expressionVal_Num);
                            }
                            else
                            {
                                if (string.Compare(itemValue, expressionValue, true) < 0 || string.Compare(itemValue, expressionValue, true) == 0)
                                    valid = true;
                                else
                                    valid = false;
                            }
                            break;
                        case "<":
                            if (item.Table.Columns[columnName].DataType.ToString() == DatabaseObjects.DataTypes.DateTime)
                            {
                                valid = (itemDate == expressionDate);
                            }
                            else if (item.Table.Columns[columnName].DataType.ToString() == DatabaseObjects.DataTypes.Number)
                            {
                                valid = (itemVal_Num < expressionVal_Num);
                            }
                            else
                            {
                                if (string.Compare(itemValue, expressionValue, true) < 0)
                                    valid = true;
                                else
                                    valid = false;
                            }
                            break;

                        case "isnull":
                            if (!UGITUtility.IsSPItemExist(item, columnName))
                                valid = true;
                            else
                                valid = false;

                            break;
                        case "isnotnull":
                            if (UGITUtility.IsSPItemExist(item, columnName))
                                valid = true;
                            else
                                valid = false;
                            break;
                        default:
                            valid = false;
                            break;

                    }
                }
                switch (expression.AppendWith)
                {
                    case "Or":
                        overallValid = valid || overallValid;
                        break;
                    case "And":
                        overallValid = valid & overallValid;
                        break;
                    default:
                        overallValid = valid;
                        break;
                }

            }
            //Return 
            return overallValid;
        }

        /// <summary>
        /// It parse the formula expression into list of formula expression
        /// </summary>
        /// <param name="formulaId"></param>
        /// <returns></returns>
        public static List<FormulaExpression> ParseFormula(int formulaId)
        {
            //DataTable formulaTable = uGITCache.GetDataTable(DatabaseObjects.Tables.ChartFormula);
            //DataTable filterTable = uGITCache.GetDataTable(DatabaseObjects.Tables.ChartFilters);
            List<FormulaExpression> expressions = new List<FormulaExpression>();
            //if (formulaTable != null)
            //{
            //    DataRow row = formulaTable.AsEnumerable().FirstOrDefault(x => x.Field<int>(DatabaseObjects.Columns.Id) == formulaId);
            //    if (row != null && row[DatabaseObjects.Columns.FormulaValue] != null)
            //    {
            //        expressions = ParseFormula(Convert.ToString(row[DatabaseObjects.Columns.FormulaValue]));
            //    }
            //}
            return expressions;
        }

        /// <summary>
        /// It parse the formula expression into list of formula expression
        /// </summary>
        /// <param name="formula">eg ##^^filterid~eq~value^^$#$And##^^filterid~neq~value^^</param>
        /// <returns></returns>
        public static List<FormulaExpression> ParseFormula(string formula)
        {
            List<FormulaExpression> expressions = new List<FormulaExpression>();
            // if (filterTable != null && filterTable.Rows.Count > 0)
            {
                string[] expsArray = formula.Split(new string[] { "$#$" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string exp in expsArray)
                {
                    FormulaExpression expression = new FormulaExpression();
                    string[] frms = exp.Split(new string[] { "##" }, StringSplitOptions.None);
                    try
                    {
                        expression.AppendWith = string.Empty;
                        if (frms[0] != null && frms[0].Equals("and", StringComparison.CurrentCultureIgnoreCase))
                        {
                            expression.AppendWith = "And";
                        }
                        else if (frms[0] != null && frms[0].Equals("or", StringComparison.CurrentCultureIgnoreCase))
                        {
                            expression.AppendWith = "Or";
                        }

                        if (frms.Count() > 0)
                        {
                            string[] eps = frms[1].Split(new string[] { "^^" }, StringSplitOptions.RemoveEmptyEntries)[0].Split(new string[] { "~" }, StringSplitOptions.RemoveEmptyEntries);
                            if (eps.Count() == 3)
                            {
                                //expression.Filter = filterTable.AsEnumerable().FirstOrDefault(x => x.Field<int>(DatabaseObjects.Columns.Id) == int.Parse(eps[0]));
                                expression.Variable = eps[0];
                                string expOperator = eps[1].ToLower();

                                expression.Operator = expOperator;
                                if (expOperator.Equals("isnull", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    expression.Operator = "IsNull";
                                }
                                else if (expOperator.Equals("isnotnull", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    expression.Operator = "IsNotNull";
                                }
                                else if (expOperator.Equals("eq", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    expression.Operator = "Eq";
                                }
                                else if (expOperator.Equals("neq", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    expression.Operator = "Neq";
                                }
                                else if (expOperator.Equals("group", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    expression.Operator = "Group";
                                }

                                expression.Value = eps[2];
                                expressions.Add(expression);
                            }

                            else if (eps.Count() == 2)
                            {
                                //expression.Filter = filterTable.AsEnumerable().FirstOrDefault(x => x.Field<int>(DatabaseObjects.Columns.Id) == int.Parse(eps[0]));
                                expression.Variable = eps[0];
                                string expOperator = eps[1].ToLower();

                                expression.Operator = expOperator;
                                if (expOperator.Equals("isnull", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    expression.Operator = "IsNull";
                                }
                                else if (expOperator.Equals("isnotnull", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    expression.Operator = "IsNotNull";
                                }
                                expression.Value = string.Empty;
                                expressions.Add(expression);
                            }
                        }
                    }
                    catch { }
                }
            }
            return expressions;
        }

        /// <summary>
        /// It converts the list of formula expressions into formula string
        /// </summary>
        /// <param name="expressions"></param>
        /// <returns></returns>
        public static string BuildFormula(List<FormulaExpression> expressions)
        {
            StringBuilder builder = new StringBuilder();
            string buildFormula = string.Empty;
            foreach (FormulaExpression exps in expressions)
            {
                if (expressions.IndexOf(exps) != 0)
                {
                    buildFormula += "$#$";
                }
                // buildFormula = string.Format("{0}##^^{1}~{2}~{3}^^", exps.AppendWith, exps.Filter[DatabaseObjects.Columns.Id], exps.Operator, exps.Value);
                buildFormula += string.Format("{0}##^^{1}~{2}~{3}^^", exps.AppendWith, exps.Variable, exps.Operator, exps.Value);
                //builder.Append(buildFormula);
            }
            return buildFormula;
        }

        public static void ConvertFormulaIntoDataTableFormula(ref List<FormulaExpression> formulaExps)
        {
            foreach (FormulaExpression exp in formulaExps)
            {
                if (exp.Operator.Equals("isnull", StringComparison.CurrentCultureIgnoreCase))
                {
                    exp.Operator = "Is Null";
                }
                else if (exp.Operator.Equals("isnotnull", StringComparison.CurrentCultureIgnoreCase))
                {
                    exp.Operator = "Is Not Null";
                }
                else if (exp.Operator.Equals("eq", StringComparison.CurrentCultureIgnoreCase))
                {
                    exp.Operator = "=";
                }
                else if (exp.Operator.Equals("neq", StringComparison.CurrentCultureIgnoreCase))
                {
                    exp.Operator = "<>";
                }
                else if (exp.Operator.Equals("gt", StringComparison.CurrentCultureIgnoreCase))
                {
                    exp.Operator = ">";
                }
                else if (exp.Operator.Equals("lt", StringComparison.CurrentCultureIgnoreCase))
                {
                    exp.Operator = "<";
                }
                else if (exp.Operator.Equals("leq", StringComparison.CurrentCultureIgnoreCase))
                {
                    exp.Operator = "<=";
                }
                else if (exp.Operator.Equals("geq", StringComparison.CurrentCultureIgnoreCase))
                {
                    exp.Operator = ">=";
                }
                else if (exp.Operator.Equals("group", StringComparison.CurrentCultureIgnoreCase))
                {
                    exp.Operator = "Group";
                }
            }
        }

        public static string ParseFormulaIntoCMAL()
        {
            return null;
        }

        /// <summary>
        /// This function will evaluate the stage skip condition and return true or false.
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="ticketItem"></param>
        /// <returns></returns>
        public static bool EvaluateFormulaExpression(ApplicationContext context, string expression, DataRow ticketItem)
        {

            string expCopy = ExpressionCalc.GetParsedDateExpression(context, expression);
            string itemValue = string.Empty;
            string expressionValue = string.Empty;
            MatchCollection formulaTokens = Regex.Matches(expression, "\\[.+?]", RegexOptions.IgnoreCase);
            foreach (Match mt in formulaTokens)
            {
                string fieldName = mt.Value.Replace("[", string.Empty).Replace("]", string.Empty);

                if (ticketItem.Table.Columns.Contains(fieldName))
                {
                    if (UGITUtility.IsSPItemExist(ticketItem, fieldName))
                    {
                        FieldConfigurationManager fieldConfigurationManager = new FieldConfigurationManager(context);
                        FieldConfiguration field = fieldConfigurationManager.GetFieldByFieldName(fieldName);
                        string DataType = Convert.ToString(ticketItem.Table.Columns[fieldName].DataType);
                        string convertedValue = UGITUtility.GetSPItemValueAsString(ticketItem, fieldName);
                        if (field != null)
                        {
                            DataType = field.Datatype;
                            convertedValue = fieldConfigurationManager.GetFieldConfigurationData(field, convertedValue);
                        }

                        if (DataType == DatabaseObjects.DataTypes.Boolean)
                        {
                            expCopy = expCopy.Replace(mt.Value, "'" + UGITUtility.StringToBoolean(Convert.ToString(ticketItem[fieldName])).ToString() + "'");
                        }
                        else if (DataType == DatabaseObjects.DataTypes.Lookup || DataType == DatabaseObjects.DataTypes.UserField)
                        {
                            expCopy = expCopy.Replace(mt.Value, "'" + convertedValue+ "'" );
                        }
                        //Only checks for single user in case of user field.
                        else if (DataType == DatabaseObjects.DataTypes.UserType)
                        {
                            expCopy = expCopy.Replace(mt.Value, "'" + UGITUtility.SplitString(ticketItem[fieldName], Constants.Separator, 1) + "'");
                        }
                        else if (DataType == DatabaseObjects.DataTypes.DateTime)
                        {
                            DateTime date = DateTime.MinValue;
                            DateTime.TryParse(Convert.ToString(ticketItem[fieldName]), out date);
                            expCopy = expCopy.Replace(mt.Value, date.ToString("dd-MM-yyyy"));
                        }
                        else if (DataType == DatabaseObjects.DataTypes.Number || DataType == DatabaseObjects.DataTypes.Counter)
                        {
                            expCopy = expCopy.Replace(mt.Value, Convert.ToString(ticketItem[fieldName]));
                        }
                        else if (DataType == DatabaseObjects.DataTypes.Currency)
                        {
                            string currency = Convert.ToString(ticketItem[fieldName]);
                            CultureInfo ci = new CultureInfo("en-US");
                            decimal val = decimal.Parse(currency, NumberStyles.Currency, ci.NumberFormat);
                            expCopy = expCopy.Replace(mt.Value, Convert.ToString(val));
                        }

                        else
                            expCopy = expCopy.Replace(mt.Value, "'" + Convert.ToString(ticketItem[fieldName]) + "'");
                    }
                    else
                        expCopy = expCopy.Replace(mt.Value, "'null'");
                }
            }

            expCopy = expCopy.Replace("\\", @"\\");

            return (EvaluateFormula(expCopy));

        }

        public static bool EvaluateFormulaExpression(ApplicationContext context, string expression, List<TicketColumnValue> formvalues,DataRow ticketItem)
        {

            string expCopy = ExpressionCalc.GetParsedDateExpression(context, expression);
            string itemValue = string.Empty;
            string expressionValue = string.Empty;
            MatchCollection formulaTokens = Regex.Matches(expression, "\\[.+?]", RegexOptions.IgnoreCase);
            foreach (Match mt in formulaTokens)
            {
                string fieldName = mt.Value.Replace("[", string.Empty).Replace("]", string.Empty);

                TicketColumnValue ticketColumnValue = formvalues.FirstOrDefault(x => x.InternalFieldName == fieldName);
                    if (ticketColumnValue != null)
                    {
                        FieldConfigurationManager fieldConfigurationManager = new FieldConfigurationManager(context);
                        FieldConfiguration field = fieldConfigurationManager.GetFieldByFieldName(fieldName);
                        string DataType = Convert.ToString(ticketItem.Table.Columns[fieldName].DataType);
                        string convertedValue = Convert.ToString(ticketColumnValue.Value);
                        if (field != null)
                        {
                            DataType = field.Datatype;
                            convertedValue = fieldConfigurationManager.GetFieldConfigurationData(field, convertedValue);
                        }

                        if (DataType == DatabaseObjects.DataTypes.Boolean)
                        {
                            expCopy = expCopy.Replace(mt.Value, "'" + UGITUtility.StringToBoolean(Convert.ToString(ticketItem[fieldName])).ToString() + "'");
                        }
                        else if (DataType == DatabaseObjects.DataTypes.Lookup || DataType == DatabaseObjects.DataTypes.UserField)
                        {
                            expCopy = expCopy.Replace(mt.Value, "'" + convertedValue + "'");
                        }
                        //Only checks for single user in case of user field.
                        else if (DataType == DatabaseObjects.DataTypes.UserType)
                        {
                            expCopy = expCopy.Replace(mt.Value, "'" + UGITUtility.SplitString(ticketItem[fieldName], Constants.Separator, 1) + "'");
                        }
                        else if (DataType == DatabaseObjects.DataTypes.DateTime)
                        {
                            DateTime date = DateTime.MinValue;
                            DateTime.TryParse(Convert.ToString(ticketItem[fieldName]), out date);
                            expCopy = expCopy.Replace(mt.Value, date.ToString("dd-MM-yyyy"));
                        }
                        else if (DataType == DatabaseObjects.DataTypes.Number || DataType == DatabaseObjects.DataTypes.Counter)
                        {
                            expCopy = expCopy.Replace(mt.Value, Convert.ToString(ticketItem[fieldName]));
                        }
                        else if (DataType == DatabaseObjects.DataTypes.Currency)
                        {
                            string currency = Convert.ToString(ticketItem[fieldName]);
                            CultureInfo ci = new CultureInfo("en-US");
                            decimal val = decimal.Parse(currency, NumberStyles.Currency, ci.NumberFormat);
                            expCopy = expCopy.Replace(mt.Value, Convert.ToString(val));
                        }

                        else
                            expCopy = expCopy.Replace(mt.Value, "'" + Convert.ToString(ticketColumnValue.Value) + "'");
                    }
                    else
                        expCopy = expCopy.Replace(mt.Value, "'null'");

            }

            expCopy = expCopy.Replace("\\", @"\\");

            return (EvaluateFormula(expCopy));

        }


        public static bool EvaluateFormula(string expression)
        {
            bool result = false;
            if (!string.IsNullOrEmpty(expression))
            {
                expression = expression.ToLower();
                Expression expEval = new Expression(expression, EvaluateOptions.IgnoreCase);
                try
                {
                    result = Convert.ToBoolean(expEval.Evaluate());
                }
                catch (EvaluationException ex)
                {
                    ULog.WriteException(ex, "Error evaluating expression: " + expression);
                }
                catch (Exception ex)
                {
                    ULog.WriteException(ex, "Error evaluating expression: " + expression);
                }
            }
            return result;
        }

        /// <summary>
        /// funtion for get tokent in [].
        /// </summary>
        /// <param name="inputval"></param>
        /// <returns></returns>
        public static List<string> GetTokenName(string inputval)
        {
            List<string> tokenNames = new List<string>();
            if (!string.IsNullOrEmpty(inputval))
            {
                string tokenName = string.Empty;
                string pattoken = @"\[(.*?)\]";
                Regex rtoken = new Regex(pattoken, RegexOptions.IgnoreCase);
                MatchCollection matchesToken = Regex.Matches(inputval, pattoken);
                foreach (Match match in matchesToken)
                {
                    tokenNames.Add(Convert.ToString(match));
                }
            }
            return tokenNames;
        }

        /// <summary>
        /// working for Spcontext.current.web,
        /// </summary>
        /// <param name="skipOnCondition"></param>
        /// <param name="moduleName"></param>
        /// <param name="keepDisplayNameAsToken">if its true show display Field name in Token else Filed name in Token.</param>
        /// <returns></returns>
        public static string GetSkipConditionExpression(String skipOnCondition, String moduleName, Boolean keepDisplayNameAsToken)
        {
            if (string.IsNullOrEmpty(skipOnCondition))
                return skipOnCondition;

            List<string> tokens = GetTokenName(skipOnCondition);
            string strskiponcondition = skipOnCondition;
            //uGovernIT.Core.UGITModule module = uGITCache.ModuleConfigCache.GetCachedModule(SPContext.Current.Web, moduleName);
            //List<FactTableField> queryTableFields = UGITModuleConstraint.GetColumnNamesWithDataType(module.ModuleTicketTable);
            //if (queryTableFields != null)
            //{
            //    foreach (FactTableField field in queryTableFields)
            //    {
            //        if (keepDisplayNameAsToken)
            //        {
            //            if (tokens.Contains("[" + field.FieldName + "]"))
            //            {
            //                strskiponcondition = strskiponcondition.Replace(field.FieldName, field.FieldDisplayName);
            //            }
            //        }
            //        else
            //        {
            //            if (tokens.Contains("[" + field.FieldDisplayName + "]"))
            //            {
            //                strskiponcondition = strskiponcondition.Replace(field.FieldDisplayName, field.FieldName);
            //            } 
            //        }
            //    }
            //}
            return strskiponcondition;
        }

        /// <summary>
        /// working for SpContext.Current.Web. for wiki.
        /// </summary>
        /// <param name="Condition"></param>
        /// <param name="keepDisplayNameAsToken"></param>
        /// <returns></returns>
        public static string GetConditionExpression(ApplicationContext context,String Condition, Boolean keepDisplayNameAsToken)
        {
            if (string.IsNullOrEmpty(Condition))
                return Condition;

            List<string> tokens = GetTokenName(Condition);
            string strcondition = Condition;

            List<uGovernIT.FactTableField> queryTableFields = UGITModuleConstraint.GetColumnNamesWithDataType(context, DatabaseObjects.Tables.WikiArticles);
            if (queryTableFields != null)
            {
                foreach (uGovernIT.FactTableField field in queryTableFields)
                {
                    if (keepDisplayNameAsToken)
                    {
                        if (tokens.Contains("[" + field.FieldName+ "]"))
                        {
                            strcondition = strcondition.Replace(field.FieldName, field.FieldDisplayName);
                        }
                    }
                    else
                    {
                        if (tokens.Contains("[" + field.FieldDisplayName + "]"))
                        {
                            strcondition = strcondition.Replace(field.FieldDisplayName, field.FieldName);
                        }
                    }
                }
            }
            return strcondition;
        }

    }
}

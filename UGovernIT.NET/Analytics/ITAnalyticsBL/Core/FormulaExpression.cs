using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITAnalyticsBL.Core
{
    public class FormulaExpression
    {
        public string Variable { get; set; }
        public string Operator { get; set; }
        public string Operand { get; set; }
        public string StartsWith { get; set; }

        /// <summary>
        /// Split the SelectionCriteria into Formula on the basis of Logical AND, OR and Equal to Operator. 
        /// </summary>
        /// <param name="selectionCriteria"></param>
        /// <returns></returns>
        public static List<FormulaExpression> ParseFormulaExpressions(string selectionCriteria)
        {
            List<FormulaExpression> lstFormulaExpression = new List<FormulaExpression>();
            if (string.IsNullOrWhiteSpace(selectionCriteria))
                return lstFormulaExpression;

            string[] strANDSeprator = new string[] { "AND" };
            string[] strORSeprator = new string[] { "OR" };
            string[] splitAND = selectionCriteria.Split(strANDSeprator, StringSplitOptions.None);
            foreach (string str in splitAND)
            {
                string[] strSplitOR = str.Split(strORSeprator, StringSplitOptions.None);
                foreach (string strOR in strSplitOR)
                {
                    FormulaExpression formulaExpression = ParseExpressionChunk(strOR);
                    if (Array.IndexOf(strSplitOR, strOR) != 0)
                    {
                        formulaExpression.StartsWith = "OR";
                    }
                    else if (Array.IndexOf(splitAND, str) != 0)
                    {
                        formulaExpression.StartsWith = "AND";
                    }

                    lstFormulaExpression.Add(formulaExpression);
                }
            }
            return lstFormulaExpression;
        }
        /// <summary>
        /// Convert the constraint in to SQL where clause
        /// </summary>
        /// <param name="parsedFormula"></param>
        /// <returns></returns>
        public static string ConvertSQLWhereClause(List<FormulaExpression> parsedFormula)
        {
            StringBuilder sqlWhere = new StringBuilder();
            string cmdSQL = string.Empty;
            try
            {
                foreach (FormulaExpression fE in parsedFormula)
                {
                    if (fE.StartsWith != null)
                    {
                        sqlWhere.AppendFormat("{0} ", fE.StartsWith);
                    }

                    string operand = string.Empty;

                    try
                    {
                        operand = Convert.ToDouble(fE.Operand.Trim()).ToString();
                    }
                    catch
                    {
                        operand = fE.Operand.StartsWith("'") ? fE.Operand.Trim() : string.Format("'{0}'", fE.Operand.Trim());
                    }

                    sqlWhere.AppendFormat("[{0}] {1} {2} ", fE.Variable.Trim(), fE.Operator, operand);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return sqlWhere.ToString();

        }
        /// <summary>
        /// this function sets the operator, operand and variable 
        /// </summary>
        /// <param name="expressionChunk"> chunk of expression</param>
        /// <returns></returns>
        public static FormulaExpression ParseExpressionChunk(string expressionChunk)
        {
            string[] strExp = new string[0];
            FormulaExpression formulaExpression = new FormulaExpression();
            strExp = expressionChunk.Split(new string[] { ">=" }, StringSplitOptions.None);
            if (strExp.Length > 1)
            {
                formulaExpression.Variable = strExp[0].ToString();
                formulaExpression.Operator = ">=";
                formulaExpression.Operand = strExp[1].ToString();
                return formulaExpression;
            }

            strExp = expressionChunk.Split(new string[] { "<=" }, StringSplitOptions.None);
            if (strExp.Length > 1)
            {
                formulaExpression.Variable = strExp[0].ToString();
                formulaExpression.Operator = "<=";
                formulaExpression.Operand = strExp[1].ToString();
                return formulaExpression;
            }

            strExp = expressionChunk.Split(new string[] { "=" }, StringSplitOptions.None);
            if (strExp.Length > 1)
            {
                formulaExpression.Variable = strExp[0].ToString();
                formulaExpression.Operator = "=";
                formulaExpression.Operand = strExp[1].ToString();
                return formulaExpression;
            }
            strExp = expressionChunk.Split(new string[] { "<>" }, StringSplitOptions.None);
            if (strExp.Length > 1)
            {
                formulaExpression.Variable = strExp[0].ToString();
                formulaExpression.Operator = "<>";
                formulaExpression.Operand = strExp[1].ToString();
                return formulaExpression;
            }
            strExp = expressionChunk.Split(new string[] { "<" }, StringSplitOptions.None);
            if (strExp.Length > 1)
            {
                formulaExpression.Variable = strExp[0].ToString();
                formulaExpression.Operator = "<";
                formulaExpression.Operand = strExp[1].ToString();
                return formulaExpression;
            }
            strExp = expressionChunk.Split(new string[] { ">" }, StringSplitOptions.None);
            if (strExp.Length > 1)
            {
                formulaExpression.Variable = strExp[0].ToString();
                formulaExpression.Operator = ">";
                formulaExpression.Operand = strExp[1].ToString();
                return formulaExpression;
            }
            return null;
        }
    }
}

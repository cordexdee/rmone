using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using System.Reflection;
using NCalc;
using uGovernIT.Util.Log;

namespace sLite
{
   public class ExpressionEvaluator
    {
       public static bool CheckCondition(string expression, double value)
       {
           Expression expEval = new Expression(expression, EvaluateOptions.IgnoreCase);
           expEval.Parameters["x"] = value;
           if (!expEval.HasErrors())
           {
             return  (bool)expEval.Evaluate();
           }
           return false;
       }
       public static bool EvaluateCondition(String expression, FunctionInput func)
       {
           bool result = false;
           Expression expEval = new Expression(expression);
           double val = 0;
           double.TryParse(Convert.ToString(func.Value), out val);
           expEval.Parameters["x"] = val;
           if (func.functionValues != null)
           {
               foreach (FunctionValue fv in func.functionValues)
               {
                   double.TryParse(Convert.ToString(fv.Value), out val);
                   expEval.Parameters[fv.Key] = val;
               }
           }
           if (!expEval.HasErrors())
           {
               result = Convert.ToBoolean(expEval.Evaluate());
           }

           return result;
       }

       public static double Evaluate(string expression, double value)
       {
           try
           {
               Expression expEval = new Expression(expression);
               expEval.Parameters["x"] = value;
               if (!expEval.HasErrors())
               {
                   return Convert.ToDouble(expEval.Evaluate());
               }
           }
           catch (Exception)
           {              
              
           }
           
           return 0;
       }
       public static double Evaluate(string expression, FunctionInput func, String askFrom, double value)
       {
           Expression expEval = new Expression(expression);
           expEval.Parameters["x"] = value;
           double val = 0;
           if (func.functionValues != null)
           {
               foreach (FunctionValue fv in func.functionValues)
               {
                   if (fv.Key.Equals(askFrom.Substring(askFrom.IndexOf("F:") + 2)) || fv.Key.Equals(askFrom))
                   {
                       double.TryParse(fv.Value, out val);
                       expEval.Parameters[fv.Key] = val;
                   }
                   else
                       expEval.Parameters[fv.Key] = value;

               }
           }
           if (!expEval.HasErrors())
           {
               return Convert.ToDouble(expEval.Evaluate());
           }
           return 0;
       }
       public static double Evaluate(string expression, FunctionInput func)
       {
           double val = 0;
           Expression expEval = new Expression(expression);
           double.TryParse(Convert.ToString(func.Value), out val);
           expEval.Parameters["x"] = val;
           if (func.functionValues != null)
           {
               foreach (FunctionValue fv in func.functionValues)
               {
                   double.TryParse(fv.Value, out val);
                   expEval.Parameters[fv.Key] = val;
               }
           }
           if (!expEval.HasErrors())
           {
               return Convert.ToDouble(expEval.Evaluate());
           }
           return 0;
       }

       public static double Evaluate(string expression, double value, ref string message)
       {
           Expression expEval = new Expression(expression);
           expEval.Parameters["x"] = value;
           try
           {
               if (!expEval.HasErrors())
               {
                   message = "Formula is valid!";
                   return Convert.ToDouble(expEval.Evaluate());
               }
               else
               {
                   message = expEval.Error;
               }
               return 0;
           }
           catch (Exception ex)
           {
               message = "Formula not Valid!:  " + ex.Message;
               return 0;
           }
         
       }

       public static double Evaluate(string expression)
       {
           Expression expEval = new Expression(expression);
         
           try
           {
               if (!expEval.HasErrors())
               {
                  
                   return Convert.ToDouble(expEval.Evaluate());
               }
           }
           catch (Exception ex)
           {
                ULog.WriteException(ex);
            }
           return 0;
       }

       //public void TestExpression(string expression)
       //{
       //    try
       //    {
       //        int result = EvaluateExpression(expression);
       //        //Console.WriteLine("'" + expression + "' = " + result);
       //       // lblTest.Text = result.ToString();
       //    }



       //    catch (Exception)
       //    {
       //        Console.WriteLine("Expression is invalid: '" + expression + "'");
       //    }
       //}
       //public static int EvaluateExpression(string expression)
       //{
       //    string code = string.Format  // Note: Use "{{" to denote a single "{"
       //    (
       //        "public static class Func{{ public static int func(){{ return {0};}}}}",
       //        expression
       //    );
       //    CompilerResults compilerResults = CompileScript(code);



       //    if (compilerResults.Errors.HasErrors)
       //    {
       //        throw new InvalidOperationException("Expression has a syntax error.");
       //    }



       //    Assembly assembly = compilerResults.CompiledAssembly;
       //    MethodInfo method = assembly.GetType("Func").GetMethod("func");



       //    return (int)method.Invoke(null, null);
       //}
       //public static CompilerResults CompileScript(string source)
       //{
       //    CompilerParameters parms = new CompilerParameters();



       //    parms.GenerateExecutable = false;
       //    parms.GenerateInMemory = true;
       //    parms.IncludeDebugInformation = false;



       //    CodeDomProvider compiler = CSharpCodeProvider.CreateProvider("CSharp");

       //    return compiler.CompileAssemblyFromSource(parms, source);
       //}

    }
}

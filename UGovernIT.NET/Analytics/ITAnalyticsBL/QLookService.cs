using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Web;
using System.Security.Cryptography;
using System.Net;
using System.Net.Mail;
using System.Configuration;
using log4net;
using System.Xml;
using ITAnalyticsUtility;



namespace ITAnalyticsBL
{
    public class QLookService
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(QLookService));
        private QLookService() { }
        private static string webServiceUrl = System.Configuration.ConfigurationSettings.AppSettings["QLookWebService"];
        //// <summary>
        //// Gives the output xml string of given model for given input.
        //// </summary>
        //// <param name="xmlId"></param>
        //// <param name="qLookInputXml"></param>
        //// <returns></returns>
        ///public static string Process(long xmlId, string qLookInputXml)
        ///{            
        ///    QLookWebService.QLookWSService qLookweb = new QLookWebService.QLookWSService();
        ///    string outputXml = qLookweb.getDecision(xmlId.ToString(), qLookInputXml);
        ///    outputXml = AddCumulativeScoreToOutput(outputXml);
        ///    return AddTitlesToOutput(xmlId,outputXml);
        ///}
        //// <summary>
        //// 
        //// </summary>
        //// <param name="modelXml">Model Xml string</param>
        //// <param name="inputXml">Input Xml string</param>
        //// <returns></returns>
        public static string Process(string modelXml, string inputXml)
        {
            log.Info("I am processing service");
            QLookWebService.QLookWSService qLookweb = new QLookWebService.QLookWSService();
            qLookweb.Url = webServiceUrl;
            log.Info(qLookweb.Url);

            qLookweb.Timeout = 1200000;
            string outputXml = qLookweb.getDecisionForModel(modelXml, inputXml);
            log.Info(outputXml);
            outputXml = AddCumulativeScoreToOutput(outputXml);
            return AddTitlesToOutput(modelXml, outputXml);
        }
        //// <summary>
        //// Gives the output xml string for the given model by Montecarlo simulation. 
        //// </summary>
        //// <param name="modelXmlId"></param>
        //// <param name="runCount"></param>
        //// <returns></returns>
        public static string GetMontecarloSimulation(string modelXml, int runCount)
        {
            QLookWebService.QLookWSService qLookweb = new QLookWebService.QLookWSService();
            qLookweb.Url = webServiceUrl;
            qLookweb.Timeout = 1200000;
            string outputXml = qLookweb.getMontecarloSimulation(modelXml, runCount);
            outputXml = AddCumulativeScoreToOutput(outputXml);
            return AddTitlesToOutput(modelXml, outputXml);
        }
        //// <summary>
        //// Adds title for model, nodes and inner models in the output.
        //// </summary>
        //// <param name="modelXmlId"></param>
        //// <param name="outputXml"></param>
        //// <returns></returns>
        ///private static string AddTitlesToOutput(long modelXmlId, string outputXml)
        ///{
        ///    string modelXml = QExchangeHelper.GetQLookXmlStringById(modelXmlId, false);
        ///    return AddTitlesToOutput(modelXml, outputXml);
        ///}
        private static string AddTitlesToOutput(string  modelXml, string outputXml)
        {
            XmlDocument outputDoc = new XmlDocument();
            XmlDocument modelDoc = new XmlDocument();
            outputDoc.LoadXml(outputXml);
            modelDoc.LoadXml(modelXml);
            XmlElement interactionData = (XmlElement)outputDoc.SelectSingleNode("///" + XmlTagLibrary.INTERACTION_DATA);
            if (modelDoc.DocumentElement.GetAttribute(XmlTagLibrary.Title) != null)
            {
                if (interactionData != null)
                {
                    interactionData.SetAttribute(XmlTagLibrary.Title,
                        modelDoc.DocumentElement.GetAttribute(XmlTagLibrary.Title));
                }
            }
            else
            {
                interactionData.SetAttribute(XmlTagLibrary.Title,
                    modelDoc.DocumentElement.GetAttribute(XmlTagLibrary.NAME));
            }
            XmlNodeList nodes = outputDoc.SelectNodes("///" + XmlTagLibrary.NODE);
            XmlNodeList innerModels = outputDoc.SelectNodes("///" + XmlTagLibrary.INNER_MODEL);
            XmlElement currentElement;
            XmlElement currentModelElement;
            foreach (XmlNode node in nodes)
            {
                currentElement = node as XmlElement;
                currentModelElement = (XmlElement)modelDoc.SelectSingleNode("///" + XmlTagLibrary.NODE + "[@" + XmlTagLibrary.ID + "=\"" + currentElement.GetAttribute(XmlTagLibrary.ID) + "\"]");
                if (currentModelElement != null)
                {
                    if (currentModelElement.GetAttribute(XmlTagLibrary.Title) != null)
                    {
                        currentElement.SetAttribute(XmlTagLibrary.Title, currentModelElement.GetAttribute(XmlTagLibrary.Title));
                    }
                    else
                    {
                        currentElement.SetAttribute(XmlTagLibrary.Title, currentModelElement.GetAttribute(XmlTagLibrary.ID));
                    }
                }
            }
            foreach (XmlNode innerModel in innerModels)
            {
                currentElement = innerModel as XmlElement;
                currentModelElement = (XmlElement)modelDoc.SelectSingleNode("///" + XmlTagLibrary.INNER_MODEL_DEFINITION + "[@" + XmlTagLibrary.NAME + "=\"" + currentElement.GetAttribute(XmlTagLibrary.MODEL_NAME) + "\"]");
                if (currentModelElement != null)
                {
                    if (currentModelElement.GetAttribute(XmlTagLibrary.Title) != null)
                    {
                        currentElement.SetAttribute(XmlTagLibrary.Title, currentModelElement.GetAttribute(XmlTagLibrary.Title));
                    }
                    else
                    {
                        currentElement.SetAttribute(XmlTagLibrary.Title, currentModelElement.GetAttribute(XmlTagLibrary.NAME));
                    }
                }
            }
            return outputDoc.DocumentElement.OuterXml;
        }
       

        public static string AnalyseVariable(long xmlId, string qLookInputXml, string variableInfoXml)
        {
            QLookWebService.QLookWSService qLookweb = new QLookWebService.QLookWSService();
            qLookweb.Url = webServiceUrl;
            return qLookweb.analyseVariable(xmlId.ToString(), qLookInputXml, variableInfoXml);
        }
        public static string AnalyseVariable(string modelXml, string qLookInputXml, string variableInfoXml)
        {
            QLookWebService.QLookWSService qLookweb = new QLookWebService.QLookWSService();
            qLookweb.Url = webServiceUrl;
            qLookweb.Timeout = 1200000;
            return qLookweb.analyseVariable(modelXml, qLookInputXml, variableInfoXml);
        }
        private static string AddCumulativeScoreToOutput(string outputXml)
        {
            XmlDocument outputDoc = new XmlDocument();
            outputDoc.LoadXml(outputXml);
            XmlElement interactionData = outputDoc.DocumentElement.FirstChild as XmlElement;
            interactionData.SetAttribute(XmlTagLibrary.CumulativeScore, interactionData.GetAttribute(XmlTagLibrary.WEIGHTED_SCORE));
            interactionData.SetAttribute(XmlTagLibrary.CumulativeWeight, "1");

            XmlNodeList nodes = outputDoc.DocumentElement.FirstChild.SelectNodes(XmlTagLibrary.NODES + "/" + XmlTagLibrary.NODE + "|" + XmlTagLibrary.INNER_MODELS + "/" + XmlTagLibrary.INNER_MODEL);
            ///XmlNodeList innerModels = outputDoc.SelectNodes(XmlTagLibrary.INNER_MODELS + "/" + XmlTagLibrary.INNER_MODEL);
           
            foreach (XmlElement node in nodes)
            {
                if (node.GetAttribute(XmlTagLibrary.WEIGHTED_SCORE) != "")
                {
                    node.SetAttribute(XmlTagLibrary.CumulativeScore, node.GetAttribute(XmlTagLibrary.WEIGHTED_SCORE));
                   
                }
                else
                {
                    node.SetAttribute(XmlTagLibrary.CumulativeScore, "?");
                }
                node.SetAttribute(XmlTagLibrary.CumulativeWeight, node.GetAttribute(XmlTagLibrary.WEIGHT));
                SumCumulativeChildrenScore(node,(float.Parse(node.GetAttribute(XmlTagLibrary.WEIGHT))));
            }
            return outputDoc.DocumentElement.OuterXml;
        }
        private static void SumCumulativeChildrenScore(XmlNode node , float cumulativeWeight)
        {
            if (node != null)
            {
                if (node.Name == XmlTagLibrary.NODE)
                {
                    foreach (XmlElement tempElement in node.SelectNodes(XmlTagLibrary.INDEPENDENT_VARIABLES + "/" + XmlTagLibrary.VARIABLE))
                    {
                        if (tempElement.GetAttribute(XmlTagLibrary.WEIGHTED_SCORE) != "")
                        {
                            tempElement.SetAttribute(XmlTagLibrary.CumulativeScore, Convert.ToString((float.Parse(tempElement.GetAttribute(XmlTagLibrary.WEIGHTED_SCORE)) * cumulativeWeight)));
                        }
                        else
                        {
                            tempElement.SetAttribute(XmlTagLibrary.CumulativeScore, "?");
                        }
                        if (tempElement.GetAttribute(XmlTagLibrary.WEIGHT) !="")
                        {
                        tempElement.SetAttribute(XmlTagLibrary.CumulativeWeight, Convert.ToString((float.Parse(tempElement.GetAttribute(XmlTagLibrary.WEIGHT)) * cumulativeWeight)));
                        }
                    }
                }
                else if (node.Name == XmlTagLibrary.INNER_MODEL)
                {

                    XmlNodeList nodeList = node.SelectNodes(XmlTagLibrary.NODES + "/" + XmlTagLibrary.NODE + "|" + XmlTagLibrary.INNER_MODELS + "/" + XmlTagLibrary.INNER_MODEL);
                    foreach (XmlElement tempElement in nodeList)
                    {
                        SumCumulativeChildrenScore(tempElement, (cumulativeWeight * (float.Parse(tempElement.GetAttribute(XmlTagLibrary.WEIGHT)))));
                        if (tempElement.GetAttribute(XmlTagLibrary.WEIGHTED_SCORE) != "")
                        {
                            tempElement.SetAttribute(XmlTagLibrary.CumulativeScore, Convert.ToString((float.Parse(tempElement.GetAttribute(XmlTagLibrary.WEIGHTED_SCORE)) * cumulativeWeight)));
                        }
                        else
                        {
                            tempElement.SetAttribute(XmlTagLibrary.CumulativeScore, "?");
                        }
                        if (tempElement.GetAttribute(XmlTagLibrary.WEIGHT) != "")
                        {
                            tempElement.SetAttribute(XmlTagLibrary.CumulativeWeight, Convert.ToString(cumulativeWeight * (float.Parse(tempElement.GetAttribute(XmlTagLibrary.WEIGHT)))));
                        }
                    }                    
                }
            } 
        }
    }
}

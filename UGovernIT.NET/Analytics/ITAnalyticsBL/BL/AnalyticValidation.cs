using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ITAnalyticsBL.DB;
using System.Xml;
using ITAnalyticsUtility;
using System.Xml.Schema;

namespace ITAnalyticsBL.BL
{
    public class AnalyticValidation
    {
        private AnalyticValidation()
        {
            errorList = new List<string>();
        }
        private List<string> errorList;
        public static List<string> ValidateAnalyticXmlById(ModelDB modelDb, long analyticVId, string analyticXsdPath)
        {
            List<string> errorMsgList = new List<string>();
            ModelVersion analytic = modelDb.ModelVersions.Find(analyticVId);
            if (analytic != null)
            {
                errorMsgList = ValidateAnalyticXml(analytic.ModelXml, analyticXsdPath);
            }
            return errorMsgList;
        }

        public static List<string> ValidateAnalyticXml(string analyticXml, string analyticXsdPath)
        {
            AnalyticValidation validate = new AnalyticValidation();
            validate.Validate(analyticXml, analyticXsdPath);
            return validate.errorList;
        }

        private void Validate(string analyticXml, string analyticXsdPath)
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(analyticXml);
            errorList = CustomValidateXml(xmldoc);
            xmldoc.Schemas.Add(null, analyticXsdPath);
            ValidationEventHandler vr = new ValidationEventHandler(ValidationHandler);
            xmldoc.Validate(vr);
        }

        private void ValidationHandler(Object sender, ValidationEventArgs args)
        {
            if (args.Exception != null)
            {
                XmlNode exceptionElement = (args.Exception as XmlSchemaValidationException).SourceObject as XmlNode;
                List<string> errorMsgList = CreateErrorMsgList(exceptionElement, args);
                if (errorList == null)
                {
                    errorList = new List<string>();
                }
                foreach (string error in errorMsgList)
                {
                    errorList.Add(error);
                }
            }
        }

        private List<string> CreateErrorMsgList(XmlNode eElement, ValidationEventArgs args)
        {
            List<string> errorMsgList = new List<string>();
            if (eElement.NodeType.ToString() == "Element")
            {
                XmlElement exceptionElement = (XmlElement)eElement;
                XmlElement tempXmlElement;
                if (args.Message.Contains("uniqueLabelForFuzzySetsOfAFuzzyVariable") || args.Message.Contains("uniqueLabelForALinguisticVariable") || args.Message.Contains("uniqueVariableNameForATweNodeDefinition") || args.Message.Contains("uniqueVariableNameForACalNodeDefinition"))
                {
                    errorMsgList.Add(string.Format("The Duplicate name  <b>\"{0}\"</b>.", exceptionElement.GetAttribute((args.Message.Contains("uniqueLabelForALinguisticVariable") || args.Message.Contains("uniqueVariableNameForATweNodeDefinition") || args.Message.Contains("uniqueVariableNameForACalNodeDefinition")) ? "name" : "title"))); //(args.Message.Contains("uniqueLabelForFuzzySetsOfAFuzzyVariable") || args.Message.Contains("uniqueLabelForALinguisticVariable"))
                }
                else
                {
                    if (exceptionElement.Name == "fuzzySets")
                    {
                        string type = string.Empty;
                        string majorParentName = string.Empty;

                        if (exceptionElement.ParentNode.ParentNode.Name == "qualifier")
                        {
                            type = "qualifier";
                            tempXmlElement = (XmlElement)exceptionElement.ParentNode.ParentNode.ParentNode;
                            majorParentName = " of variable <b>\"" + tempXmlElement.GetAttribute("name") + "\"</b>";
                        }
                        else
                        {
                            type = "variable";
                            majorParentName = "";
                            if (exceptionElement.ParentNode.ParentNode.ParentNode.Name == "calNodeDefinition")
                            {
                                tempXmlElement = (XmlElement)exceptionElement.ParentNode.ParentNode.ParentNode.ParentNode;
                                majorParentName = " of Cal node <b>\"" + tempXmlElement.GetAttribute("title") + "\"</b>";
                            }
                            else if (exceptionElement.ParentNode.ParentNode.ParentNode.Name == "tweNodeDefinition")
                            {
                                tempXmlElement = (XmlElement)exceptionElement.ParentNode.ParentNode.ParentNode.ParentNode;
                                majorParentName = " of Twe node <b>\"" + tempXmlElement.GetAttribute("title") + "\"</b>";
                            }
                        }
                        tempXmlElement = (XmlElement)exceptionElement.ParentNode;
                        errorMsgList.Add(string.Format("The linguisticFuzzy {1} <b>\"{0}\"</b>{2} has incomplete content. List of possible elements expected: 'ShoulderLeftFuzzySet', 'ShoulderRightFuzzySet', 'TriangleFuzzySet', 'TrapezoidFuzzySet', 'SFuzzySet', 'ZFuzzySet', 'PiFuzzySet', 'GaussianFuzzySet.", tempXmlElement.GetAttribute("name"), type, majorParentName));
                    }
                    else if (exceptionElement.Name == "linguisticVariable")
                    {
                        string type = string.Empty;
                        string majorParentName = string.Empty;
                        if (exceptionElement.ParentNode.Name == "qualifier")
                        {
                            type = "qualifier";
                            tempXmlElement = (XmlElement)exceptionElement.ParentNode.ParentNode;
                            majorParentName = " of variable <b>\"" + tempXmlElement.GetAttribute("name") + "\"</b>";
                        }
                        else
                        {
                            type = "variable";
                            tempXmlElement = (XmlElement)exceptionElement.ParentNode.ParentNode.ParentNode;
                            if (exceptionElement.ParentNode.ParentNode.Name == "calNodeDefinition")
                            {

                                majorParentName = " of Cal node <b>\"" + tempXmlElement.GetAttribute("title") + "\"</b>";
                            }
                            else if (exceptionElement.ParentNode.ParentNode.Name == "tweNodeDefinition")
                            {
                                majorParentName = " of Twe node <b>\"" + tempXmlElement.GetAttribute("title") + "\"</b>";
                            }
                        }
                        errorMsgList.Add(string.Format("The linguistic {1} <b>\"{0}\"</b>{2} has incomplete content. List of possible elements expected: 'Variable range'.", exceptionElement.GetAttribute("name"), type, majorParentName));
                    }
                    else if (exceptionElement.Name == "numericVariable")
                    {
                        string type = string.Empty;
                        string majorParentName = string.Empty;
                        if (exceptionElement.ParentNode.Name == "qualifier")
                        {
                            type = "qualifier";
                            tempXmlElement = (XmlElement)exceptionElement.ParentNode.ParentNode;
                            majorParentName = " of variable <b>\"" + tempXmlElement.GetAttribute("name") + "\"</b>";

                        }
                        else
                        {
                            type = "variable";
                            tempXmlElement = (XmlElement)exceptionElement.ParentNode.ParentNode.ParentNode;
                            if (exceptionElement.ParentNode.ParentNode.Name == "calNodeDefinition")
                            {
                                majorParentName = " of Cal node <b>\"" + tempXmlElement.GetAttribute("title") + "\"</b>";
                            }
                            else if (exceptionElement.ParentNode.ParentNode.Name == "tweNodeDefinition")
                            {
                                majorParentName = " of Twe node <b>\"" + tempXmlElement.GetAttribute("title") + "\"</b>";
                            }
                        }
                        errorMsgList.Add(string.Format("The numeric {1} <b>\"{0}\"</b>{2} has incomplete content. List of possible elements expected: 'Variable range'.", exceptionElement.GetAttribute("name"), type, majorParentName));
                    }
                    else if (exceptionElement.Name == "independentVariables" || exceptionElement.Name == "dependentVariables")
                    {
                        string nodeName = string.Empty;
                        if (exceptionElement.ParentNode.Name == "calNodeDefinition")
                        {
                            nodeName = "Cal node";
                        }
                        else if (exceptionElement.ParentNode.Name == "tweNodeDefinition")
                        {
                            nodeName = "Twe node";
                        }
                        tempXmlElement = (XmlElement)exceptionElement.ParentNode.ParentNode;
                        errorMsgList.Add(string.Format("The {1} of {2} <b>\"{0}\"</b> has incomplete content. List of possible elements expected: 'Boolean variable', 'Linguistic variable', 'Numeric variable', 'LinguisticFuzzy variable'.", tempXmlElement.GetAttribute("title"), exceptionElement.Name, nodeName));
                    }
                }
            }
            return errorMsgList;
        }
        private List<string> CustomValidateXml(XmlDocument xmlDoc)
        {
            List<string> errorMsgList = new List<string>();
            XmlNodeList majorElementList = xmlDoc.SelectNodes("//" + XmlTagLibrary.EXCHANGE_MODEL + " | //" + XmlTagLibrary.INNER_MODEL_DEFINITION);
            XmlElement XmlElement;
            for (int i = 0; i < majorElementList.Count; i++)
            {
                XmlNodeList elementList = majorElementList[i].SelectNodes(XmlTagLibrary.INNER_MODELS + "/" + XmlTagLibrary.INNER_MODEL_DEFINITION + "|" + XmlTagLibrary.NODES + "/" + XmlTagLibrary.NODE);
                XmlElement = (XmlElement)majorElementList[i];
                if (elementList.Count < 1)
                {
                    errorMsgList.Add(string.Format("The model/inner model <b>\"{0}\"</b> must have atleast one node or inner model.", XmlElement.GetAttribute("title")));
                }

                XmlNodeList nodes = majorElementList[i].SelectNodes(XmlTagLibrary.NODES + "/" + XmlTagLibrary.NODE);
                XmlNodeList innerModels = majorElementList[i].SelectNodes(XmlTagLibrary.INNER_MODELS + "/" + XmlTagLibrary.INNER_MODEL_DEFINITION);
                if (nodes.Count > 0 && innerModels.Count > 0)
                {
                    errorMsgList.Add(string.Format("The analytic/section <b>\"{0}\"</b> can not have section and subsection parallely.", XmlElement.GetAttribute("title")));
                }
            }
            return errorMsgList;
        }
    }
}

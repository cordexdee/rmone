using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using ITAnalyticsUtility;

namespace ITAnalyticsBL.Core
{
    public class AnalyticInputGenerator
    {
        public static XmlDocument CreateAnalyticInput(XmlDocument analyticDoc, Dictionary<string, object> dataForm)
        {
            Analytic analytic = Analytic.GetAnalytic(analyticDoc);
            return CreateAnalyticInput(analytic, dataForm);
        }

        public static XmlDocument CreateAnalyticInput(Analytic analytic, Dictionary<string, object> dataForm)
        {
            XmlDocument inputDoc = new XmlDocument();
            if (analytic.Sections != null && analytic.Sections.Count > 0)
            {
                foreach (AnalyticSection section in analytic.Sections)
                {
                    if (section.SubSections != null && section.SubSections.Count > 0)
                    {
                        foreach (AnalyticSubSection subSection in section.SubSections)
                        {
                            inputDoc = UpdateValueInInputXml(inputDoc, subSection.SubSectionXml, analytic.AnalyticXml, dataForm, section.InternalID);
                        }
                    }
                }
            }
            else if (analytic.SubSections != null && analytic.SubSections.Count > 0)
            {
                foreach (AnalyticSubSection subSection in analytic.SubSections)
                {
                    inputDoc = UpdateValueInInputXml(inputDoc, subSection.SubSectionXml, analytic.AnalyticXml, dataForm, analytic.InternalID);
                }
            }
            return inputDoc;
        }
       
        
        /// <summary>
        ///  This takes the input xml from the features and check if it's already exist, if not it creates the new one otherwise update the existing one with
        ///  corresponding section,sub-section or super section. if any section, sub-section or super section is not exist it creates the same for particular
        ///  feature input xml.
        /// </summary>
        /// <param name="inputXml"></param>
        /// <param name="subSectionXml"></param>
        /// <param name="modelXml"></param>
        /// <param name="dataForm"></param>
        /// <param name="sectionId"></param>
        /// <returns></returns>
        public static XmlDocument UpdateValueInInputXml(XmlDocument inputXml, XmlElement subSectionXml, XmlDocument modelXml, Dictionary<string, object> dataForm, string sectionId)
        {
            XmlDocument updateInputXml = inputXml;
            XmlElement roolXml = inputXml != null ? inputXml.DocumentElement : null;
            if (roolXml == null)
            {
                updateInputXml = new XmlDocument();
                roolXml = updateInputXml.CreateElement(XmlTagLibrary.INTERACTION_DATA);
                roolXml.SetAttribute(XmlTagLibrary.MODEL_NAME, modelXml.DocumentElement.GetAttribute(XmlTagLibrary.NAME));
                updateInputXml.AppendChild(roolXml);
                XmlElement sectionInputBlock;
                if (Analytic.IsSectionExist(modelXml.DocumentElement))
                {
                    sectionInputBlock = updateInputXml.CreateElement(XmlTagLibrary.INNER_MODELS); ;
                }
                else
                {
                    sectionInputBlock = updateInputXml.CreateElement(XmlTagLibrary.NODES);
                }
                roolXml.AppendChild(sectionInputBlock);
                // roolXml = sectionInputBlock;
            }

            string subSectionId = subSectionXml.GetAttribute(XmlTagLibrary.ID);
            XmlNode inputXmlSection = roolXml.SelectSingleNode(string.Format("//{0}[@{1}='{2}']", XmlTagLibrary.INNER_MODEL, XmlTagLibrary.MODEL_NAME, sectionId));
            XmlNode inputXmlSubSection = null;
            if (inputXmlSection != null)
            {
                inputXmlSubSection = inputXmlSection.SelectSingleNode(string.Format(".//{0}[@{1}='{2}']", XmlTagLibrary.NODE, XmlTagLibrary.ID, subSectionId));
            }


            XmlElement parentNode = null;
            bool sectionsExist = Analytic.IsSectionExist(modelXml.DocumentElement);
            if (sectionsExist)
            {
                if (inputXmlSection != null)
                {
                    if (inputXmlSubSection != null)
                    {
                        inputXmlSection.ChildNodes[0].RemoveChild(inputXmlSubSection);
                    }
                    parentNode = (XmlElement)inputXmlSection.ChildNodes[0];
                }
                else
                {
                    parentNode = (XmlElement)updateInputXml.DocumentElement.SelectSingleNode(string.Format("{0}", XmlTagLibrary.INNER_MODELS));
                    XmlElement innerModelInputXml = updateInputXml.CreateElement(XmlTagLibrary.INNER_MODEL);
                    innerModelInputXml.SetAttribute(XmlTagLibrary.MODEL_NAME, sectionId);

                    XmlElement nodesInputXml = updateInputXml.CreateElement(XmlTagLibrary.NODES);
                    innerModelInputXml.AppendChild(nodesInputXml);

                    parentNode.AppendChild(innerModelInputXml);
                    parentNode = nodesInputXml;
                }
            }
            else
            {
                if (inputXmlSubSection != null)
                {
                    roolXml.ChildNodes[0].RemoveChild(inputXmlSubSection);
                }
                else
                {

                }
                parentNode = (XmlElement)roolXml.ChildNodes[0];
            }
            XmlElement nodeElement = UpdateValueInNodeXml(updateInputXml, subSectionXml, modelXml, dataForm);
            parentNode.AppendChild(nodeElement);
            return updateInputXml;
        }

        private static XmlElement UpdateValueInNodeXml(XmlDocument updateInputXml, XmlElement subSectionXml, XmlDocument modelXml, Dictionary<string, object> dataForm)
        {
            if (subSectionXml != null)
            {
                XmlElement nodeInputElement = updateInputXml.CreateElement(XmlTagLibrary.NODE);
                nodeInputElement.SetAttribute(XmlTagLibrary.ID, subSectionXml.Attributes[XmlTagLibrary.ID].Value);
                if (subSectionXml.ChildNodes.Count > 0)
                {
                    bool nodeIgnore = false;
                    XmlNode section = subSectionXml.ParentNode.ParentNode;
                    foreach (XmlNode modelNode in subSectionXml.ChildNodes[0].ChildNodes)
                    {
                        if (GetFeatureValue(dataForm, section.Attributes[XmlTagLibrary.NAME].Value.Replace(" ", "_") + "^ignore").Trim().Replace(",", "") == "1")
                        {
                            nodeIgnore = true;
                        }

                        if (GetFeatureValue(dataForm, subSectionXml.Attributes[XmlTagLibrary.ID].Value.Replace(" ", "_") + "^ignore").Trim().Replace(",", "") == "1")
                        {
                            nodeIgnore = true;
                        }
                        XmlElement variableInputBlockElement = updateInputXml.CreateElement(modelNode.Name);
                        nodeInputElement.AppendChild(variableInputBlockElement);
                        foreach (XmlNode featureModelNode in modelNode.ChildNodes)
                        {
                            XmlElement variableInputNode = updateInputXml.CreateElement(XmlTagLibrary.VARIABLE);
                            variableInputNode.SetAttribute(XmlTagLibrary.NAME, featureModelNode.Attributes[XmlTagLibrary.NAME].Value);
                            variableInputNode.SetAttribute(XmlTagLibrary.VALUE, string.Empty);
                            if (GetFeatureValue(dataForm, featureModelNode.Attributes[XmlTagLibrary.NAME].Value.Replace(" ", "_")) != string.Empty)
                            {
                                variableInputNode.SetAttribute(XmlTagLibrary.VALUE, Convert.ToString(dataForm[featureModelNode.Attributes[XmlTagLibrary.NAME].Value.Replace(" ", "_")]));
                            }

                            //for specify start
                            if (featureModelNode.Name == XmlTagLibrary.LINGUISTIC_VARIABLE)
                            {
                                XmlNode specifyNode = featureModelNode.SelectSingleNode(string.Format(".//*[@{0}='true']", XmlTagLibrary.Specify));
                                if (specifyNode != null && variableInputNode.Attributes[XmlTagLibrary.VALUE].Value == specifyNode.InnerText
                                    && GetFeatureValue(dataForm, featureModelNode.Attributes[XmlTagLibrary.NAME].Value.Replace(" ", "_") + "^specify") != string.Empty)
                                {
                                    variableInputNode.SetAttribute(XmlTagLibrary.Specify, GetFeatureValue(dataForm, featureModelNode.Attributes[XmlTagLibrary.NAME].Value.Replace(" ", "_") + "^specify"));
                                }
                            }
                            else if (featureModelNode.Name == XmlTagLibrary.BOOLEAN_VARIABLE)
                            {
                                if (featureModelNode.Attributes[XmlTagLibrary.Specify] != null && featureModelNode.Attributes[XmlTagLibrary.Specify].Value == "true"
                                    && variableInputNode.Attributes[XmlTagLibrary.VALUE].Value.Equals("Yes", StringComparison.CurrentCultureIgnoreCase)
                                    && GetFeatureValue(dataForm, featureModelNode.Attributes[XmlTagLibrary.NAME].Value.Replace(" ", "_") + "^specify") != string.Empty)
                                {
                                    variableInputNode.SetAttribute(XmlTagLibrary.Specify, GetFeatureValue(dataForm, featureModelNode.Attributes[XmlTagLibrary.NAME].Value.Replace(" ", "_") + "^specify"));
                                }
                            }
                            //for specify end

                            if (nodeIgnore || GetFeatureValue(dataForm, featureModelNode.Attributes[XmlTagLibrary.NAME].Value.Replace(" ", "_") + "^ignore").Trim().Replace(",", "") == "1")
                            {
                                variableInputNode.SetAttribute(XmlTagLibrary.Ignore, "1");
                            }
                            else
                            {
                                variableInputNode.RemoveAttribute(XmlTagLibrary.Ignore);
                            }

                            variableInputBlockElement.AppendChild(variableInputNode);
                            XmlNode qualifierModelNode = featureModelNode.SelectSingleNode(string.Format("./{0}", XmlTagLibrary.QUALIFIER));
                            if (qualifierModelNode != null)
                            {
                                XmlNode qualifierVarNode = qualifierModelNode.ChildNodes[0];

                                XmlElement qualifierInputBlockNode = updateInputXml.CreateElement(XmlTagLibrary.QUALIFIER);
                                XmlElement qualifierInputNode = updateInputXml.CreateElement(XmlTagLibrary.VARIABLE);
                                qualifierInputBlockNode.AppendChild(qualifierInputNode);
                                variableInputNode.AppendChild(qualifierInputBlockNode);
                                qualifierInputNode.SetAttribute(XmlTagLibrary.NAME, qualifierModelNode.ChildNodes[0].Attributes[XmlTagLibrary.NAME].Value);
                                qualifierInputNode.SetAttribute(XmlTagLibrary.VALUE, GetFeatureValue(dataForm, qualifierModelNode.ChildNodes[0].Attributes[XmlTagLibrary.NAME].Value.Replace(" ", "_")));
                                //for specify start
                                if (qualifierVarNode.Name == XmlTagLibrary.LINGUISTIC_VARIABLE)
                                {
                                    XmlNode specifyNode = qualifierVarNode.SelectSingleNode(string.Format(".//*[@{0}='true']", XmlTagLibrary.Specify));
                                    if (specifyNode != null && qualifierInputNode.Attributes[XmlTagLibrary.VALUE].Value == specifyNode.InnerText)
                                    {
                                        qualifierInputNode.SetAttribute(XmlTagLibrary.Specify, GetFeatureValue(dataForm, qualifierVarNode.Attributes[XmlTagLibrary.NAME].Value.Replace(" ", "_") + "^specify"));
                                    }
                                }
                                else if (qualifierVarNode.Name == XmlTagLibrary.BOOLEAN_VARIABLE)
                                {
                                    if (qualifierVarNode.Attributes[XmlTagLibrary.Specify] != null && qualifierVarNode.Attributes[XmlTagLibrary.Specify].Value == "true" && qualifierInputNode.Attributes[XmlTagLibrary.VALUE].Value.Equals("Yes", StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        qualifierInputNode.SetAttribute(XmlTagLibrary.Specify, GetFeatureValue(dataForm, qualifierVarNode.Attributes[XmlTagLibrary.NAME].Value.Replace(" ", "_") + "^specify"));
                                    }
                                }
                                //for specify end
                            }
                        }
                        nodeIgnore = false;
                    }
                }
                return nodeInputElement;
            }
            return null;
        }
        private static string GetFeatureValue(Dictionary<string, object> featureInputs, string key)
        {
            if (featureInputs.ContainsKey(key))
            {
                if (featureInputs[key] != null && !string.IsNullOrWhiteSpace(Convert.ToString(featureInputs[key])))
                {
                    return Convert.ToString(featureInputs[key]);
                }
                else
                {
                    return string.Empty;
                }
            }
            return string.Empty;
        }

    }
}

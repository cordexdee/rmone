using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using ITAnalyticsUtility;
using System.Xml.Linq;
using System.Xml.XPath;

namespace ITAnalyticsBL.BL
{
    public class AnalyticHelper
    {
        /// <summary>
        /// Get impact of section, subsection or feature on Analytics
        /// </summary>
        /// <param name="modelXml"></param>
        /// <param name="sectionInternalID">Take internalID of section like for innerModel internalID is modelName, and for subsection internalID is ID and for features it is name</param>
        /// <param name="subSectionInternalID"></param>
        /// <param name="featureName"></param>
        /// <returns></returns>
        public static Dictionary<string, double> GetImpactOfBlock(XmlDocument modelXml, AnalyticScoreType scoreType, string sectionInternalID = "", string subSectionInternalID = "", string featureName = "")
        {
            Dictionary<string, double> impactDic = new Dictionary<string, double>();
            SetDefaultScoreToModel(modelXml.DocumentElement, 1000);
            if (!string.IsNullOrWhiteSpace(sectionInternalID) && !string.IsNullOrWhiteSpace(subSectionInternalID) && !string.IsNullOrWhiteSpace(featureName))
            {
                XmlElement featureElement = GetFeatureXmlElement(modelXml.DocumentElement, subSectionInternalID, featureName);
                if (featureElement != null)
                {
                    impactDic.Add(featureElement.GetAttribute(XmlTagLibrary.NAME), Convert.ToDouble(featureElement.GetAttribute(XmlTagLibrary.CumulativeScore)));
                }
            }
            else if(!string.IsNullOrWhiteSpace(sectionInternalID) && !string.IsNullOrWhiteSpace(subSectionInternalID))
            {
                XmlElement subSectionElement = GetSubSectionXmlElement(modelXml.DocumentElement, subSectionInternalID);
                if (subSectionElement != null)
                {
                    impactDic.Add(subSectionElement.GetAttribute(XmlTagLibrary.Title), Convert.ToDouble(subSectionElement.GetAttribute(XmlTagLibrary.CumulativeScore)));
                }
            }
            else if (!string.IsNullOrWhiteSpace(sectionInternalID))
            {
               XmlElement sectionElement = GetSectionXmlElement(modelXml.DocumentElement, sectionInternalID);
               if (sectionElement != null)
               {
                   impactDic.Add(sectionElement.GetAttribute(XmlTagLibrary.Title), Convert.ToDouble(sectionElement.GetAttribute(XmlTagLibrary.CumulativeScore)));
               }
            }

            return impactDic;
        }


        public static XmlNodeList GetAnalyticSections(XmlElement analyticXml)
        {
            return analyticXml.SelectNodes(string.Format(".//{0}", XmlTagLibrary.INNER_MODEL_DEFINITION));
        }

        public static XmlNodeList GetAnalyticSubSections(XmlElement blockXml, string sectionInternalID="")
        {
            if (sectionInternalID != string.Empty)
            {
                if (blockXml.GetAttribute(XmlTagLibrary.NAME) == sectionInternalID)
                {
                    return blockXml.SelectNodes(string.Format(".//{0}", XmlTagLibrary.NODE));
                }
                else
                {
                    XmlElement section = (XmlElement)blockXml.SelectSingleNode(string.Format(".//{0}[@{1}='{2}']", XmlTagLibrary.INNER_MODEL_DEFINITION, XmlTagLibrary.NAME, sectionInternalID));
                    if (section != null)
                    {
                        return section.SelectNodes(string.Format(".//{0}", XmlTagLibrary.NODE));
                    }
                }
            }
            else
            {
                return blockXml.SelectNodes(string.Format(".//{0}", XmlTagLibrary.NODE));
            }

            return null;
        }

    

        public static XmlNodeList GetAnalyticFeatures(XmlElement blockXml, string subSectionInternalID="")
        {
            if (subSectionInternalID != string.Empty)
            {
                XmlElement subSection = (XmlElement)blockXml.SelectSingleNode(string.Format(".//{0}[@{1}='{2}']", XmlTagLibrary.NODE, XmlTagLibrary.ID, subSectionInternalID));
                if (subSection != null)
                {
                    return subSection.SelectNodes(string.Format(".//{0} | .//{1} | .//{2} | .//{3} | .//{4}", XmlTagLibrary.LINGUISTIC_VARIABLE, XmlTagLibrary.LINGUISTIC_FUZZY_VARIABLE, XmlTagLibrary.NUMERIC_VARIABLE, XmlTagLibrary.BOOLEAN_VARIABLE, XmlTagLibrary.InfoVariable));
                }
            }
            else
            {
                return blockXml.SelectNodes(string.Format(".//{0} | .//{1} | .//{2} | .//{3} | .//{4}", XmlTagLibrary.LINGUISTIC_VARIABLE, XmlTagLibrary.LINGUISTIC_FUZZY_VARIABLE, XmlTagLibrary.NUMERIC_VARIABLE, XmlTagLibrary.BOOLEAN_VARIABLE, XmlTagLibrary.InfoVariable));
            }
            return null;
        }

        public static XmlElement GetSectionXmlElement(XmlElement analyticXml, string sectionInternalID)
        {
            return (XmlElement)analyticXml.SelectSingleNode(string.Format(".//{0}[@{1}='{2}']", XmlTagLibrary.INNER_MODEL_DEFINITION, XmlTagLibrary.NAME, sectionInternalID));
        }


        public static XmlElement GetSubSectionXmlElement(XmlElement modelXml, string subSectionInternalID)
        {
            return (XmlElement)modelXml.SelectSingleNode(string.Format(".//{0}[@{1}='{2}']", XmlTagLibrary.NODE, XmlTagLibrary.ID, subSectionInternalID));
        }

        public static XmlElement GetFeatureXmlElement(XmlElement modelXml, string subSectionInternalID, string featureName)
        {
            XmlElement subSection = (XmlElement)modelXml.SelectSingleNode(string.Format(".//{0}[@{1}='{2}']", XmlTagLibrary.NODE, XmlTagLibrary.ID, subSectionInternalID));
            if (subSection != null)
            {
                return (XmlElement)subSection.SelectSingleNode(string.Format(".//{2}[@{0}='{1}'] | .//{3}[@{0}='{1}'] | .//{4}[@{0}='{1}'] | .//{5}[@{0}='{1}'] | .//{6}[@{0}='{1}']", XmlTagLibrary.NAME, featureName, XmlTagLibrary.LINGUISTIC_VARIABLE, XmlTagLibrary.LINGUISTIC_FUZZY_VARIABLE, XmlTagLibrary.NUMERIC_VARIABLE, XmlTagLibrary.BOOLEAN_VARIABLE, XmlTagLibrary.InfoVariable));
            }
            return null;
        }

      

        public static XmlElement SetDefaultScoreToModel(XmlElement modelXml, int normalizeOn)
        {
            if(modelXml.Name == XmlTagLibrary.EXCHANGE_MODEL)
            {
                modelXml.SetAttribute(XmlTagLibrary.WEIGHT, "1.0");
                modelXml.SetAttribute(XmlTagLibrary.CumulativeScore, normalizeOn.ToString());
                XmlNodeList nodes = modelXml.SelectNodes(string.Format(".//{0}", XmlTagLibrary.INNER_MODEL_DEFINITION));
                string nodeType = XmlTagLibrary.INNER_MODEL_DEFINITION;
                if(nodes.Count <= 0)
                {
                    nodeType = XmlTagLibrary.NODE;
                    nodes =  modelXml.SelectNodes(string.Format(".//{0}", XmlTagLibrary.NODE));
                }
                SetDefaultScoreToModelBlock(nodeType, nodes, 1, normalizeOn);
            }

            return modelXml;
        }

        private static void SetDefaultScoreToModelBlock(string blockType, XmlNodeList blockXmlList, double parentWeight, int normalizeOn)
        {
            if (blockType == XmlTagLibrary.INNER_MODEL_DEFINITION || blockType == XmlTagLibrary.NODE)
            {
                double totalWeight = 0;
                foreach (XmlNode modelNode in blockXmlList)
                {
                    double weight = 0;
                    if (modelNode.Attributes[XmlTagLibrary.WEIGHT] != null)
                    {
                        double.TryParse(modelNode.Attributes[XmlTagLibrary.WEIGHT].Value, out weight);
                        totalWeight += weight;
                    }
                    if (weight <= 0)
                    {
                        ((XmlElement)modelNode).SetAttribute(XmlTagLibrary.WEIGHT, "0");
                    }
                }

                foreach (XmlNode modelNode in blockXmlList)
                {
                    double weight = 0;
                    double.TryParse(modelNode.Attributes[XmlTagLibrary.WEIGHT].Value, out weight);
                    double aWeight = Math.Round((weight / totalWeight), 4);

                    ((XmlElement)modelNode).SetAttribute(XmlTagLibrary.WEIGHT, aWeight.ToString());
                    ((XmlElement)modelNode).SetAttribute(XmlTagLibrary.CumulativeScore, Math.Round((aWeight * parentWeight * normalizeOn), 4).ToString());

                    if (blockType == XmlTagLibrary.INNER_MODEL_DEFINITION)
                    {
                        XmlNodeList nodeList = modelNode.SelectNodes(string.Format(".//{0} ", XmlTagLibrary.NODE));
                        if (nodeList.Count > 0)
                        {
                            SetDefaultScoreToModelBlock(XmlTagLibrary.NODE, nodeList, aWeight, normalizeOn);
                        }
                    }
                    else if (blockType == XmlTagLibrary.NODE)
                    {
                        XmlNodeList featureNodeList = modelNode.SelectNodes(string.Format(".//{0} | .//{1} | .//{2} | .//{3}", XmlTagLibrary.LINGUISTIC_VARIABLE, XmlTagLibrary.LINGUISTIC_FUZZY_VARIABLE, XmlTagLibrary.NUMERIC_VARIABLE, XmlTagLibrary.BOOLEAN_VARIABLE));
                        if (featureNodeList.Count > 0)
                        {
                            SetDefaultScoreToModelBlock(string.Empty, featureNodeList, aWeight, normalizeOn);
                        }
                    }
                     
                }
            }
            else
            {
                double totalWeight = 0;
                foreach (XmlNode featureNode in blockXmlList)
                {
                    double weight = 0;
                    if (featureNode.Attributes[XmlTagLibrary.WEIGHT] != null)
                    {
                        double.TryParse(featureNode.Attributes[XmlTagLibrary.WEIGHT].Value, out weight);
                        totalWeight += weight;
                    }
                    if (weight <= 0)
                    {
                        ((XmlElement)featureNode).SetAttribute(XmlTagLibrary.WEIGHT, "0");
                    }
                }

                foreach (XmlNode featureNode in blockXmlList)
                {
                    double weight = 0;
                    double.TryParse(featureNode.Attributes[XmlTagLibrary.WEIGHT].Value, out weight);
                    double aWeight = Math.Round((weight / totalWeight), 4);

                    ((XmlElement)featureNode).SetAttribute(XmlTagLibrary.WEIGHT, aWeight.ToString());
                    ((XmlElement)featureNode).SetAttribute(XmlTagLibrary.CumulativeScore, Math.Round((aWeight * parentWeight * normalizeOn), 4).ToString());
                }
            }
        }


    }
}

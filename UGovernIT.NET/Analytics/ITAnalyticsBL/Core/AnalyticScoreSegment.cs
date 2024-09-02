using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Xsl;
using ITAnalyticsBL.BL;
using ITAnalyticsBL.DB;
using ITAnalyticsUtility;


namespace ITAnalyticsBL.Core
{
    public class AnalyticScoreSegment
    {
        public string Label { get; set; }
        public int RangeStart { get; set; }
        public int RangeEnd { get; set; }
        public System.Drawing.Color Color { get; set; }

        public static List<AnalyticScoreSegment> ConvertScoreSegment(XmlElement scoreSegmentXml)
        {
            List<AnalyticScoreSegment> segments = new List<AnalyticScoreSegment>();
            if (scoreSegmentXml != null)
            {
                XmlNodeList nodes = scoreSegmentXml.SelectNodes(string.Format(".//{0}", XmlTagLibrary.OUTPUT_VARIABLE));
                Random randon = new Random();
                foreach (XmlNode node in nodes)
                {
                    int randomVal = randon.Next(170, 255);
                    AnalyticScoreSegment segment = new AnalyticScoreSegment();
                    segment.Label = node.Attributes[XmlTagLibrary.LABEL] != null ? node.Attributes[XmlTagLibrary.LABEL].Value : string.Empty;
                    System.Drawing.Color checkColor = System.Drawing.Color.FromArgb(randomVal, randomVal, randomVal);
                    try
                    {
                        if (node.Attributes[XmlTagLibrary.LABEL] != null)
                        {
                            checkColor = System.Drawing.ColorTranslator.FromHtml(string.Format("#{0}", node.Attributes[XmlTagLibrary.Color]));
                        }
                    }
                    catch
                    {
                        checkColor = System.Drawing.Color.FromArgb(randomVal, randomVal, randomVal);
                    }
                    segment.Color = checkColor;
                    XmlElement startEle = (XmlElement)node.SelectSingleNode(string.Format(".//{0}", XmlTagLibrary.START));
                    XmlElement endEle = (XmlElement)node.SelectSingleNode(string.Format(".//{0}", XmlTagLibrary.END));
                    segment.RangeStart = int.Parse(startEle.InnerText);
                    segment.RangeEnd = int.Parse(endEle.InnerText);
                    segments.Add(segment);
                }
            }
            return segments;
        }

        public static List<AnalyticScoreSegment> GetScoreSegment(XmlElement modelXml, bool childScore = false, string sectionID = "", string subSectionID = "", string featureID = "")
        {
            XmlElement aScoreSegmentXml = AnalyticScoreSegment.GetScoreSegmentXml(modelXml, childScore, sectionID, subSectionID, featureID);
            return ConvertScoreSegment(aScoreSegmentXml);
        }

        public static List<AnalyticScoreSegment> GetScoreSegmentOfBlock(XmlElement blockXml, bool childScore = false)
        {
            if (blockXml != null)
            {
                string sagementID = childScore ? blockXml.GetAttribute(XmlTagLibrary.ChildOpDefId) : blockXml.GetAttribute(XmlTagLibrary.OpDefId);
                XmlElement scoreSagement = (XmlElement)blockXml.OwnerDocument.DocumentElement.SelectSingleNode(string.Format(".//{0}/{1}[@{2}='{3}']", XmlTagLibrary.OUTPUT_DEFINITIONS, XmlTagLibrary.OUTPUT_DEFINITION, XmlTagLibrary.ID, sagementID));
                return ConvertScoreSegment(scoreSagement);
            }
            return null;
        }

        public static XmlElement GetScoreSegmentXml(XmlElement modelXml, bool childScore = false, string sectionID = "", string subSectionID = "", string featureID = "")
        {
            XmlElement scoreSagement = null;
            if (modelXml != null)
            {
                if (!string.IsNullOrWhiteSpace(subSectionID) && !string.IsNullOrWhiteSpace(featureID))
                {
                    XmlElement featureXml = AnalyticHelper.GetFeatureXmlElement(modelXml, subSectionID, featureID);
                    string sagementID = string.Empty;
                    if (featureXml != null)
                    {
                        sagementID = childScore ? featureXml.GetAttribute(XmlTagLibrary.ChildOpDefId) : featureXml.GetAttribute(XmlTagLibrary.OpDefId);
                        scoreSagement = (XmlElement)modelXml.SelectSingleNode(string.Format(".//{0}/{1}[@{2}='{3}']", XmlTagLibrary.OUTPUT_DEFINITIONS, XmlTagLibrary.OUTPUT_DEFINITION, XmlTagLibrary.ID, sagementID));
                    }
                }
                else if (!string.IsNullOrWhiteSpace(subSectionID))
                {
                    XmlElement subSectionXml = AnalyticHelper.GetSubSectionXmlElement(modelXml, subSectionID);
                    string sagementID = string.Empty;
                    if (subSectionXml != null)
                    {
                        sagementID = childScore ? subSectionXml.GetAttribute(XmlTagLibrary.ChildOpDefId) : subSectionXml.GetAttribute(XmlTagLibrary.OpDefId);
                        scoreSagement = (XmlElement)modelXml.SelectSingleNode(string.Format(".//{0}/{1}[@{2}='{3}']", XmlTagLibrary.OUTPUT_DEFINITIONS, XmlTagLibrary.OUTPUT_DEFINITION, XmlTagLibrary.ID, sagementID));
                    }
                }
                else if (!string.IsNullOrWhiteSpace(sectionID))
                {
                    XmlElement sectionXml = AnalyticHelper.GetSectionXmlElement(modelXml, sectionID);
                    string sagementID = string.Empty;
                    if (sectionXml != null)
                    {
                        sagementID = childScore ? sectionXml.GetAttribute(XmlTagLibrary.ChildOpDefId) : sectionXml.GetAttribute(XmlTagLibrary.OpDefId);
                        scoreSagement = (XmlElement)modelXml.SelectSingleNode(string.Format(".//{0}/{1}[@{2}='{3}']", XmlTagLibrary.OUTPUT_DEFINITIONS, XmlTagLibrary.OUTPUT_DEFINITION, XmlTagLibrary.ID, sagementID));
                    }
                }
                else
                {
                    string sagementID = string.Empty;
                    if (modelXml != null)
                    {
                        sagementID = childScore ? modelXml.GetAttribute(XmlTagLibrary.ChildOpDefId) : modelXml.GetAttribute(XmlTagLibrary.OpDefId);
                        scoreSagement = (XmlElement)modelXml.SelectSingleNode(string.Format(".//{0}/{1}[@{2}='{3}']", XmlTagLibrary.OUTPUT_DEFINITIONS, XmlTagLibrary.OUTPUT_DEFINITION, XmlTagLibrary.ID, sagementID));
                    }
                }
            }
            return scoreSagement;
        }
    }
}

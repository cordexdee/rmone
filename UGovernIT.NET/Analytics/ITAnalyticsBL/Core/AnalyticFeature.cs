using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using ITAnalyticsBL.BL;
using ITAnalyticsUtility;

namespace ITAnalyticsBL.Core
{
    public class AnalyticFeature
    {
        public string Name { get; set; }
        public string Weight { get; set; }
        public string Question { get; set; }
        public AnalyticFeature Qualifier { get; set; }
        public XmlElement FeatureXml { get; set; }
        public AnalyticSubSection SubSection { get; set; }

        public List<AnalyticScoreSegment> OwnScoreSegment { get; private set; }
        public List<AnalyticScoreSegment> ChildScoreSegment { get; private set; }

        public static List<AnalyticFeature> GetAnalyticFeatures(AnalyticSubSection subSection, XmlElement blockXml, bool isScoreSegmentRequired)
        {
            List<AnalyticFeature> analyticFeatures = null;
            if (blockXml != null)
            {
                XmlNodeList features = AnalyticHelper.GetAnalyticFeatures(blockXml);
                if (features != null && features.Count > 0)
                {
                    analyticFeatures = new List<AnalyticFeature>();

                    foreach (XmlNode featureNode in features)
                    {
                        AnalyticFeature featureObj = null;
                        XmlElement feature = (XmlElement)featureNode;
                       
                        switch (feature.Name)
                        {
                            case XmlTagLibrary.BOOLEAN_VARIABLE :
                                featureObj = AnalyticBooleanVariable.GetFeature(feature);
                                analyticFeatures.Add(featureObj);
                                break;
                            case XmlTagLibrary.NUMERIC_VARIABLE:
                                featureObj = AnalyticNumericVariable.GetFeature(feature);
                                analyticFeatures.Add(featureObj);
                                break;
                            case XmlTagLibrary.LINGUISTIC_FUZZY_VARIABLE:
                                featureObj = AnalyticLinquisticFuzzyVariable.GetFeature(feature);
                                analyticFeatures.Add(featureObj);
                                break;
                            case XmlTagLibrary.LINGUISTIC_VARIABLE:
                                featureObj = AnalyticLinquisticVariable.GetFeature(feature);
                                analyticFeatures.Add(featureObj);
                                break;
                            case XmlTagLibrary.InfoVariable:
                                featureObj = AnalyticInfoVariable.GetFeature(feature);
                                analyticFeatures.Add(featureObj);
                                break;
                        }

                        if (isScoreSegmentRequired)
                        {
                            featureObj.SubSection = subSection;
                            featureObj.OwnScoreSegment = AnalyticScoreSegment.GetScoreSegmentOfBlock(feature);
                            featureObj.ChildScoreSegment = AnalyticScoreSegment.GetScoreSegmentOfBlock(feature, true);
                        }
                    }
                }
            }

            return analyticFeatures;
        }

        public static List<AnalyticFeature> GetAllFeaturesOfAanalytics(Analytic analytic)
        {
            List<AnalyticFeature> features = new List<AnalyticFeature>();
            if (analytic != null)
            {
                if (analytic.Sections != null && analytic.Sections.Count > 0)
                {
                    foreach (AnalyticSection section in analytic.Sections)
                    {
                        if (section.SubSections != null)
                        {
                            foreach (AnalyticSubSection subSection in section.SubSections)
                            {
                                features.AddRange(subSection.Features);
                            }
                        }
                    }
                }
                else
                {
                    if (analytic.SubSections != null)
                    {
                        foreach (AnalyticSubSection subSection in analytic.SubSections)
                        {
                            features.AddRange(subSection.Features);
                        }
                    }
                }
            }

            return features;
        }
    }

    public class AnalyticNumericVariable : AnalyticFeature
    {
        public static AnalyticNumericVariable GetFeature(XmlElement featureXml)
        {
            AnalyticNumericVariable nVar = new AnalyticNumericVariable();
            nVar.Name = featureXml.GetAttribute(XmlTagLibrary.NAME);
            nVar.Weight = featureXml.GetAttribute(XmlTagLibrary.WEIGHT);
            nVar.Question = featureXml.GetAttribute(XmlTagLibrary.Question);
            nVar.FeatureXml = featureXml;

            //Qualifier
            XmlNodeList qualifierNodes = AnalyticHelper.GetAnalyticFeatures(featureXml);
            if (qualifierNodes != null && qualifierNodes.Count > 0)
            {
               List<AnalyticFeature> qualifiers =   AnalyticFeature.GetAnalyticFeatures(null, featureXml, false);
               if (qualifierNodes != null && qualifierNodes.Count > 0)
               {
                   nVar.Qualifier = qualifiers[0];
               }
            }

            return nVar;
        }
    }

    public class AnalyticLinquisticVariable : AnalyticFeature
    {
        public int FormatType { get; set; }
        public List<LinguisticVariableRangeLabel> VariableRangeLabels { get; set; }
        public static AnalyticLinquisticVariable GetFeature(XmlElement featureXml)
        {
            AnalyticLinquisticVariable nVar = new AnalyticLinquisticVariable();
            nVar.Name = featureXml.GetAttribute(XmlTagLibrary.NAME);
            nVar.Weight = featureXml.GetAttribute(XmlTagLibrary.WEIGHT);
            nVar.Question = featureXml.GetAttribute(XmlTagLibrary.Question);

            int formatType = 0;
            int.TryParse(featureXml.GetAttribute(XmlTagLibrary.FormatType), out formatType);
            nVar.FormatType = formatType;

            nVar.FeatureXml = featureXml;

            //Qualifier
            XmlNodeList qualifierNodes = AnalyticHelper.GetAnalyticFeatures(featureXml);
            if (qualifierNodes != null && qualifierNodes.Count > 0)
            {
                List<AnalyticFeature> qualifiers = AnalyticFeature.GetAnalyticFeatures(null, featureXml, false);
                if (qualifierNodes != null && qualifierNodes.Count > 0)
                {
                    nVar.Qualifier = qualifiers[0];
                }
            }

            XmlNodeList linguisticRangeLabelNodes = featureXml.SelectNodes(string.Format(".//{0}", XmlTagLibrary.LinguisticVariableRangeLabel));
            if (linguisticRangeLabelNodes != null)
            {
                nVar.VariableRangeLabels = new List<LinguisticVariableRangeLabel>();
                foreach (XmlNode rangeLabelNode in linguisticRangeLabelNodes)
                {
                    XmlElement rangeLabel = (XmlElement)rangeLabelNode;
                    nVar.VariableRangeLabels.Add(new LinguisticVariableRangeLabel(rangeLabel.InnerText, rangeLabel.GetAttribute(XmlTagLibrary.WEIGHT)));
                }
            }
            return nVar;
        }
    }

    public class AnalyticLinquisticFuzzyVariable : AnalyticFeature
    {
        public int Start { get;set;}
        public int End { get; set; }
        public static AnalyticLinquisticFuzzyVariable GetFeature(XmlElement featureXml)
        {
            AnalyticLinquisticFuzzyVariable nVar = new AnalyticLinquisticFuzzyVariable();
            nVar.Name = featureXml.GetAttribute(XmlTagLibrary.NAME);
            nVar.Weight = featureXml.GetAttribute(XmlTagLibrary.WEIGHT);
            nVar.Question = featureXml.GetAttribute(XmlTagLibrary.Question);
            nVar.Start = featureXml.SelectSingleNode(string.Format(".//{0}", XmlTagLibrary.START)) != null ? int.Parse(featureXml.SelectSingleNode(string.Format(".//{0}", XmlTagLibrary.START)).InnerText) : 0 ;
            nVar.End = featureXml.SelectSingleNode(string.Format(".//{0}", XmlTagLibrary.END)) != null ? int.Parse(featureXml.SelectSingleNode(string.Format(".//{0}", XmlTagLibrary.END)).InnerText) : 0;
            nVar.FeatureXml = featureXml;

            //Qualifier
            XmlNodeList qualifierNodes = AnalyticHelper.GetAnalyticFeatures(featureXml);
            if (qualifierNodes != null && qualifierNodes.Count > 0)
            {
                List<AnalyticFeature> qualifiers = AnalyticFeature.GetAnalyticFeatures(null, featureXml, false);
                if (qualifierNodes != null && qualifierNodes.Count > 0)
                {
                    nVar.Qualifier = qualifiers[0];
                }
            }
            return nVar;
        }
    }

    public class AnalyticBooleanVariable : AnalyticFeature
    {
        public int FormatType { get; set; }
        public static AnalyticBooleanVariable GetFeature(XmlElement featureXml)
        {
            AnalyticBooleanVariable nVar = new AnalyticBooleanVariable();
            nVar.Name = featureXml.GetAttribute(XmlTagLibrary.NAME);
            nVar.Weight = featureXml.GetAttribute(XmlTagLibrary.WEIGHT);
            nVar.Question = featureXml.GetAttribute(XmlTagLibrary.Question);
            int formatType = 0;
            int.TryParse(featureXml.GetAttribute(XmlTagLibrary.FormatType), out formatType);
            nVar.FormatType = formatType;
            nVar.FeatureXml = featureXml;
            return nVar;
        }
    }

    public class AnalyticInfoVariable : AnalyticFeature
    {
        public string Type { get; set; }
        public bool Mandatory { get; set; }
        public static AnalyticInfoVariable GetFeature(XmlElement featureXml)
        {
            AnalyticInfoVariable nVar = new AnalyticInfoVariable();
            nVar.Name = featureXml.GetAttribute(XmlTagLibrary.NAME);
            nVar.Type = featureXml.GetAttribute(XmlTagLibrary.TYPE);
            bool mandatory = false;
            bool.TryParse(featureXml.GetAttribute(XmlTagLibrary.Mandatory), out mandatory);
            nVar.Mandatory = mandatory;
            nVar.FeatureXml = featureXml;
            return nVar;
        }
    }

    public class LinguisticVariableRangeLabel
    {
        public string Label { get; set; }
        public string Weight { get; set; }
        public LinguisticVariableRangeLabel(string label, string weight)
        {
            Label = label;
            Weight = weight;
        }

    }
}

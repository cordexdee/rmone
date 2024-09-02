using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using ITAnalyticsBL.BL;
using ITAnalyticsUtility;

namespace ITAnalyticsBL.Core
{
    public class AnalyticSubSection
    {
        public string InternalID { get; private set; }
        public string Title { get; private set; }
        public List<AnalyticFeature> Features { get; private set; }
        public List<AnalyticScoreSegment> OwnScoreSegment { get; private set; }
        public List<AnalyticScoreSegment> ChildScoreSegment { get; private set; }
        public XmlElement SubSectionXml { get; private set; }
        public string Weight { get; private set; }
        public AnalyticSection Section { get; set; }
        public Analytic Analytic { get; set; }


        public static List<AnalyticSubSection> GetAnalyticSubSections(Analytic analtyic, AnalyticSection section, XmlElement blockXml, bool isScoreSegmentRequired)
        {
            List<AnalyticSubSection> analyticSubSections = null;
            if (blockXml != null)
            {
                XmlNodeList subSections = AnalyticHelper.GetAnalyticSubSections(blockXml);
                if (subSections != null && subSections.Count > 0)
                {
                    analyticSubSections = new List<AnalyticSubSection>();
                    foreach (XmlNode subSectionNode in subSections)
                    {
                        XmlElement subSection = (XmlElement)subSectionNode;
                        AnalyticSubSection analyticSubSection = new AnalyticSubSection();
                        analyticSubSection.InternalID = subSection.GetAttribute(XmlTagLibrary.ID);
                        analyticSubSection.Title = subSection.GetAttribute(XmlTagLibrary.Title);
                        analyticSubSection.SubSectionXml = subSection;
                        analyticSubSection.Weight = subSection.GetAttribute(XmlTagLibrary.WEIGHT);

                        if (isScoreSegmentRequired)
                        {
                            analyticSubSection.OwnScoreSegment = AnalyticScoreSegment.GetScoreSegmentOfBlock(subSection);
                            analyticSubSection.ChildScoreSegment= AnalyticScoreSegment.GetScoreSegmentOfBlock(subSection, true);
                        }
                        analyticSubSection.Section = section;
                        analyticSubSection.Analytic = analtyic;
                        analyticSubSection.Features = AnalyticFeature.GetAnalyticFeatures(analyticSubSection, subSection, isScoreSegmentRequired);
                        analyticSubSections.Add(analyticSubSection);
                    }
                }
            }
            return analyticSubSections;
        }
    }
}

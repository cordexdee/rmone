using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using ITAnalyticsBL.BL;
using ITAnalyticsUtility;

namespace ITAnalyticsBL.Core
{
    public class AnalyticSection
    {
        public string InternalID { get; private set; }
        public string Title { get; private set; }
        public List<AnalyticSubSection> SubSections { get; private set; }
        public List<AnalyticScoreSegment> OwnScoreSegment { get; private set; }
        public List<AnalyticScoreSegment> ChildScoreSegment { get; private set; }
        public XmlElement SectionXml { get; private set; }
        public string Weight { get; private set; }
        public Analytic Analytic{get;set;}

        public static List<AnalyticSection> GetAnalyticSections(Analytic analytic, XmlDocument anlayticXml, bool isScoreSegmentRequired)
        {
            List<AnalyticSection> anlayticSections = null;
            if (anlayticXml != null)
            {
                XmlNodeList sections = AnalyticHelper.GetAnalyticSections(anlayticXml.DocumentElement);
                if (sections != null)
                {
                    anlayticSections = new List<AnalyticSection>();
                    foreach (XmlNode sectionNode in sections)
                    {
                        AnalyticSection analyticSection = new AnalyticSection();
                        XmlElement section = (XmlElement)sectionNode;
                        analyticSection.InternalID = section.GetAttribute(XmlTagLibrary.NAME);
                        analyticSection.Title = section.GetAttribute(XmlTagLibrary.Title);
                        analyticSection.Weight = section.GetAttribute(XmlTagLibrary.WEIGHT);
                        if (isScoreSegmentRequired)
                        {
                            analyticSection.OwnScoreSegment = AnalyticScoreSegment.GetScoreSegmentOfBlock(section);
                            analyticSection.ChildScoreSegment = AnalyticScoreSegment.GetScoreSegmentOfBlock(section, true);
                        }
                        analyticSection.SectionXml = section;
                        analyticSection.Analytic = analytic;

                        analyticSection.SubSections = AnalyticSubSection.GetAnalyticSubSections(analytic, analyticSection, section, isScoreSegmentRequired);
                        anlayticSections.Add(analyticSection);
                    }
                }
            }
            return anlayticSections;
        }
    }
}

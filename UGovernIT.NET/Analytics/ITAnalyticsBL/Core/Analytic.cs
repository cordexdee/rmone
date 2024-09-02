using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using ITAnalyticsBL.BL;
using ITAnalyticsUtility;

namespace ITAnalyticsBL.Core
{
    public class Analytic
    {
        public string InternalID { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public bool EnableIgnore { get; private set; }
        public List<AnalyticSection> Sections { get; private set; }
        public List<AnalyticSubSection> SubSections { get; private set; }
        public List<AnalyticScoreSegment> OwnScoreSegment { get; private set; }
        public List<AnalyticScoreSegment> ChildScoreSegment { get; private set; }
        public XmlDocument AnalyticXml { get; set; }
        public string Weight { get; private set; }
        

        public static Analytic GetAnalytic(XmlDocument analyticXml, bool isScoreSegmentRequired = false)
        {
            Analytic analytic = null;
            if (analyticXml != null)
            {
                analytic = new Analytic();
                analytic.InternalID = analyticXml.DocumentElement.GetAttribute(XmlTagLibrary.NAME);
                analytic.Title = analyticXml.DocumentElement.GetAttribute(XmlTagLibrary.Title);
                analytic.Description = analyticXml.DocumentElement.GetAttribute(XmlTagLibrary.Description);

                bool enableIgnore = false;
                bool.TryParse(analyticXml.DocumentElement.GetAttribute(XmlTagLibrary.EnableIgnore), out enableIgnore);
                analytic.EnableIgnore = enableIgnore;

                analytic.AnalyticXml = analyticXml;
                analytic.Weight = analyticXml.DocumentElement.GetAttribute(XmlTagLibrary.WEIGHT);

                if (isScoreSegmentRequired)
                {
                    analytic.OwnScoreSegment = AnalyticScoreSegment.GetScoreSegmentOfBlock(analyticXml.DocumentElement);
                    analytic.ChildScoreSegment = AnalyticScoreSegment.GetScoreSegmentOfBlock(analyticXml.DocumentElement, true);
                }

                XmlNodeList sections = AnalyticHelper.GetAnalyticSections(analyticXml.DocumentElement);
                if (sections != null && sections.Count > 0)
                {
                    analytic.Sections = AnalyticSection.GetAnalyticSections(analytic, analyticXml, isScoreSegmentRequired);
                }
                else
                {
                    analytic.SubSections = AnalyticSubSection.GetAnalyticSubSections(analytic, null, analyticXml.DocumentElement, isScoreSegmentRequired);
                }
            }

            return analytic;
        }

        public static bool IsSectionExist(XmlElement analyticXml)
        {
            if (analyticXml.SelectNodes("innerModels/innerModelDefinition").Count > 0)
            {
                return true;
            }
            return false;
        }

       
    }

   

    

  


}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITAnalyticsBL
{
    public enum AnalyticStatus
    {
        Edit,
        Active,
        Archive
    }

    public enum DataIntegrationType
    {
        ET,
        SharePoint,
        Survey,
        Query,
        Table
    }

    public enum AnalyticScoreType
    {
        Normalized_By_1000,
        Normalized_By_100,
        Normalized_By_10,
        Absolute
    }
    public enum ETTableStatus
    {
        Not_Started,
        In_Progress,
        Completed
    }
    public enum AxisType
    {
        XAxis, YAxis
    }
    public enum ChartIntegrationSource { Excel, Analytic }
    public enum LabelPosition { Above, Below, None }
    public enum FillStyle { Fill, Line }

    public enum AnalyticFormViewType
    {
        CompleteForm,
        Intergrated,
        TestForm,
        ReRun
    }
}
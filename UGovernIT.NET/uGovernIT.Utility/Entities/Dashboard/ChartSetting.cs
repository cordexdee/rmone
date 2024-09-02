using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace uGovernIT.Utility
{
   
    [Serializable]
    [System.Xml.Serialization.XmlInclude(typeof(ChartSetting))]
    [System.Xml.Serialization.XmlInclude(typeof(PanelSetting))]
    [System.Xml.Serialization.XmlInclude(typeof(DashboardQuery))]
    public class DashboardPanel
    {
        public DashboardType type { get; set; }
        public string ContainerTitle { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOn { get; set; }
        public int Order { get; set; }
        public Guid DashboardID { get; set; }
        public bool HideZoomView { get; set; }
        public bool HideTableView { get; set; }
        public bool HidewDownloadView { get; set; }
        public bool StartFromNewLine { get; set; }
        public DashboardPanel()
        {
            DashboardID = Guid.NewGuid();
            ContainerTitle = string.Empty;
            CreatedOn = DateTime.MaxValue;
            Description = string.Empty;
        }
    }

    [Serializable]
    [XmlRoot("ChartSetting")]
    public class ChartSetting : DashboardPanel
    {
        public string Id { get; set; }
        public Guid ChartId { get; set; }
        public bool IsComulative { get; set; }
        public List<ChartDimension> Dimensions { get; set; }
        public List<ChartExpression> Expressions { get; set; }
        public string FactTable { get; set; }
        public string BasicDateFitlerStartField { get; set; }
        public string BasicDateFitlerEndField { get; set; }
        public string BasicDateFilterDefaultView { get; set; }
        public string BasicFilter { get; set; }
        public bool HideDateFilterDropdown { get; set; }

        //Appearance settings
        public bool HideGrid { get; set; }
        public bool HideLegend { get; set; }

        //new added chart apperance: start
        public string HorizontalAlignment { get; set; }
        public string VerticalAlignment { get; set; }
        public string Direction { get; set; }
        public string MaxHorizontalPercentage { get; set; }
        public string MaxVerticalPercentage { get; set; }
        public string Palette { get; set; }
        public string BGColor  { get; set; }
        //new added chart apperance: end        

        //depricated properties:start
        public string LegendAlignment { get; set; }
        public string LegendDocking { get; set; }
        public int BorderWidth { get; set; }
        //depricated properties:end

        public bool HideLabel { get; set; }

        public string LabelStyle { get; set; }
        public string LabelText { get; set; }

        //   [Obsolete()]
        public int AxisLabelStyleAngle { get; set; }
        //Cache chart
        public bool IsCacheChart { get; set; }
        public int CacheSchedule { get; set; }
        /// <summary>
        /// Property reverse the ploting chart. means Expression will be plot like dimension and vise versa
        /// </summary>
        public bool ReversePlotting { get; set; }

        public ChartSetting()
        {
            Id = string.Empty;
            ChartId = DashboardID;
            Dimensions = new List<ChartDimension>();
            Expressions = new List<ChartExpression>();
            BasicDateFilterDefaultView = string.Empty;
            BasicDateFitlerStartField = string.Empty;
            BasicDateFitlerEndField = string.Empty;
            LabelStyle = string.Empty;
            LabelText = string.Empty;
            FactTable = string.Empty;

            HorizontalAlignment = "Center";
            VerticalAlignment = "TopOutside";
            Direction = "LeftToRight";
            MaxHorizontalPercentage = "100";
            MaxVerticalPercentage = "100";
            Palette = "Default";
        }
    }

    [Serializable]
    public class ChartDimension
    {
        public string Title { get; set; }
        public string SelectedField { get; set; }
        public int Sequence { get; set; }

        public int PickTopDataPoint { get; set; }
        public string DataPointOrder { get; set; }
        public int DataPointExpression { get; set; }

        public int OrderByExpression { get; set; }
        public string OrderBy { get; set; }
        public bool EnableSorting { get; set; }

        public string Operator { get; set; }
        //   [Obsolete()]
        public string OperatorField { get; set; }


        public bool IsCumulative { get; set; }
        public string FactTable { get; set; }
        public int FilterID { get; set; }
        public DatapointClickeEventType DataPointClickEvent { get; set; }
        public string DateViewType { get; set; }
        public bool ShowInDropDown { get; set; }
        public int AxisLabelStyleAngle { get; set; }
        public int AxisLabelMaxLength { get; set; }
        public int LegendTxtMaxLength { get; set; }
        public string ScaleType { get; set; }

        public ChartDimension()
        {
            Title = string.Empty;
            SelectedField = string.Empty;
            Operator = "count";
            DataPointOrder = "Ascending";
            OperatorField = string.Empty;
            FactTable = string.Empty;
            DataPointClickEvent = DatapointClickeEventType.None;
            DateViewType = string.Empty;
            AxisLabelStyleAngle = 0;

            PickTopDataPoint = 0;
            DataPointOrder = "descending";
            DataPointExpression = 0;

            //EnableSorting = false;
            OrderByExpression = 0;
            OrderBy = "ascending";

        }
    }

    [Serializable]
    public class ChartExpression
    {
        public string Title { get; set; }
        public string FactTable { get; set; }
        public string GroupByField { get; set; }
        public string Operator { get; set; }
        public string ExpressionFormula { get; set; }
        public int Order { get; set; }
        public string ChartType { get; set; }
        [Obsolete]
        public int PickTopDataPoint { get; set; }
        [Obsolete]
        public string DataPointOrder { get; set; }

        // [Obsolete]
        public bool ShowInPercentage { get; set; }

        public string LabelStyle { get; set; }
        public string LabelText { get; set; }
        public ChartAxisType YAsixType { get; set; }
        public DatapointClickeEventType DataPointClickEvent { get; set; }
        public string FunctionExpression { get; set; }
        public string Palette { get; set; }
        public string DrawingStyle { get; set; }
        public string LabelColor { get; set; }
        public bool IsCurrency { get; set; }
        public List<string> Dimensions { get; set; }
        //public string LabelFormat { get; set; }


        public bool HideLabel { get; set; }
        public int AxisLabelMaxLength { get; set; }

        public List<DataItem> ChartLevelProperties;
        //New properties:end

        private string _labelFormat;
        public string LabelFormat
        {
            get
            {
                if (IsCurrency)
                    return "currency";
                else
                    return _labelFormat;
            }
            set
            {
                _labelFormat = value;
            }
        }

        public ChartExpression()
        {
            Title = string.Empty;
            GroupByField = string.Empty;
            Operator = "count";
            ExpressionFormula = string.Empty;
            FactTable = string.Empty;
            ChartType = "Column";
            LabelText = string.Empty;
            LabelStyle = string.Empty;
            YAsixType = ChartAxisType.Primary;
            DataPointClickEvent = DatapointClickeEventType.None;
            FunctionExpression = string.Empty;
            Palette = string.Empty;
            DrawingStyle = "Default";
            LabelColor = "#000000";
            LabelFormat = string.Empty;

            ChartLevelProperties = new List<DataItem>();
            Dimensions = new List<string>();

        }
    }


    [Serializable]
    [XmlRoot("PanelSetting")]
    public class PanelSetting : DashboardPanel
    {

        public List<DashboardPanelLink> Expressions { get; set; }
        public Guid PanelID { get; set; }
        public string IconUrl { get; set; }
        public int ColumnViewType { get; set; }
        public bool StopAutoScale { get; set; }
        public string ChartType { get; set; }
        public string PanelViewType { get; set; }
        public int ChartHide { get; set; }
        public string HorizontalAlignment { get; set; }
        public string VerticalAlignment { get; set; }
        public string Legendvisible { get; set; }
        public string TextFormat { get; set; }
        public string CentreTitle { get; set; }
        public PanelSetting()
        {
            Expressions = new List<DashboardPanelLink>();
            PanelID = DashboardID;
        }
    }

    [Serializable]
    public class DashboardPanelLink
    {
        public string DashboardTable { get; set; }
        public Guid LinkID { get; set; }
        public string Title { get; set; }
        public string LinkUrl { get; set; }
        public bool DefaultLink { get; set; }

        /// <summary>
        /// Open link
        /// 0=window, 1=popup
        /// </summary>
        public short ScreenView { get; set; }
        public bool IsHide { get; set; }
        public int Order { get; set; }
        public bool UseAsPanel { get; set; }
        public string ExpressionFormat { get; set; }
        public bool HideTitle { get; set; }

        public string DateFilterStartField { get; set; }
        public string DateFilterDefaultView { get; set; }
        public string Filter { get; set; }
        public double MaxLimit { get; set; }
        public int DecimalPoint { get; set; }
        public bool ShowBar { get; set; }
        public string AggragateFun { get; set; }
        public string AggragateOf { get; set; }
        public string BarDefaultColor { get; set; }
        public bool IsPct { get; set; }
        public List<KPIBarCondition> Conditions { get; set; }
        public string FontColor { get; set; }


        public ModuleType PanelModuleType;
        public int ExpressionID;
        public int FormulaId;
        public short ViewType;
        public string ModuleName;
        public string BarUnit { get; set; }
        public bool StopLinkDetail { get; set; }
        public string ShowColumns_Category { get; set; }

        public DashboardPanelLink()
        {
            Title = string.Empty;
            LinkID = Guid.NewGuid();
            LinkUrl = string.Empty;
            AggragateFun = "Count";
            AggragateOf = string.Empty;
            Filter = string.Empty;
            BarDefaultColor = "#FF7F7F";
            FontColor = "#000000";
            ExpressionFormat = string.Empty;
            DashboardTable = string.Empty;
            DefaultLink = true;
            HideTitle = true;
            MaxLimit = 100;

            PanelModuleType = ModuleType.All;
            ModuleName = string.Empty;
            ExpressionFormat = string.Empty;
            BarUnit = string.Empty;
            ShowColumns_Category = string.Empty;
        }
    }


    public class KPIBarCondition
    {
        public double Score { get; set; }
        public string Operator { get; set; }
        public string Color { get; set; }
    }

    [Serializable]
    public enum DatapointClickeEventType
    {
        None = 0,
        NextDimension,
        Detail,
        Inherit
    }

    [Serializable]
    public enum ChartAxisType
    {
        Primary,
        Secondary
    }

    //legends enums ::start
    [Serializable]
    public enum LegendHorizontalAlignment
    {
        LeftOutside,
        Left,
        Center,
        Right,
        RightOutside
    }

    [Serializable]
    public enum LegendVerticalAlignment
    {
        TopOutside,
        Top,
        Center,
        Bottom,
        BottomOutside
    }

    [Serializable]
    public enum DlegendDirection
    {
        TopToBottom,
        BottomToTop,
        LeftToRight,
        RightToLeft
    }
    //legends enums ::end

    [Serializable]
    public enum ChartLabelPosition
    {
        Top,
        TopInside,
        Center,
        BottomInside
    }
    [Serializable]
    public enum ChartLabelOrientation
    {
        Horizontal,
        TopToBottom,
        BottomToTop
    }
}

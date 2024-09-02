
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility
{
    [Serializable]
    public class Joins
    {
        public JoinType JoinType { get; set; }
        public string FirstTable { get; set; }
        public string SecondTable { get; set; }
        public string OperatorType { get; set; }
        public string FirstColumn { get; set; }
        public string SecondColumn { get; set; }
        public string DataTypeFirstCol { get; set; }
        public string DataTypeSecondCol { get; set; }
    }

    public class CustomQuery
    {
        public List<Joins> JoinList { get; set; }
        public List<GroupByInfo> GroupBy { get; set; }
        public List<OrderByInfo> OrderBy { get; set; }
        public List<TableInfo> Tables { get; set; }
        public List<WhereInfo> WhereClauses { get; set; }
        public List<ColumnInfo> Totals { get; set; }
        public List<TableInfo> DrillDownTables { get; set; }
        public QueryFormat QueryFormats { get; set; }
        public string QueryInHTML { get; set; }
        public bool IsPreviewFormatted { get; set; }
        public bool IsGroupByExpanded { get; set; }
        public string MissingColumns { get; set; }

        public CustomQuery()
        {
            DrillDownTables = new List<TableInfo>();
            Tables = new List<TableInfo>();
            JoinList = new List<Joins>();
            WhereClauses = new List<WhereInfo>();
            GroupBy = new List<GroupByInfo>();
            OrderBy = new List<OrderByInfo>();
            MissingColumns = string.Empty;
        }

    }
    [Serializable]
    public class TableInfo
    {
        public TableInfo()
        {
            Columns = new List<ColumnInfo>();
        }
        public int ID { get; set; }
        public string Name { get; set; }
        public string alias { get; set; }
        public List<ColumnInfo> Columns { get; set; }
    }

    [Serializable]
    public class ColumnInfo
    {
        public int ID { get; set; }
        public string FieldName { get; set; }
        public string DisplayName { get; set; }
        public string DataType { get; set; }
        public string TableName { get; set; }
        public string Function { get; set; }
        public int Sequence { get; set; }
        public bool Selected { get; set; }
        public bool Hidden { get; set; }
        public bool IsExpression { get; set; }
        public string Expression { get; set; }
        public bool IsDrillDown { get; set; }
        public bool IsFormattedColumn { get; set; }
        public string Alignment { get; set; }
        public int Width { get; set; }
    }
    [Serializable]
    public class ExpressionInfo
    {
        public string FieldName { get; set; }
        public string Table1 { get; set; }
        public string Column1 { get; set; }
        public string DataType1 { get; set; }
        public string Operator { get; set; }
        public string Table2 { get; set; }
        public string Column2 { get; set; }
        public string DataType2 { get; set; }
        public string StartIndex { get; set; }
        public string Length { get; set; }
    }
    [Serializable]
    public class QueryFormat
    {
        public string Text { get; set; }
        public string TextFontName { get; set; }
        public FontStyle TextFontStyle { get; set; }
        public string TextFontSize { get; set; }
        public string TextForeColor { get; set; }
        public bool HideText { get; set; }

        public string Label { get; set; }
        public string LabelFontName { get; set; }
        public FontStyle LabelFontStyle { get; set; }
        public string LabelFontSize { get; set; }
        public string LabelForeColor { get; set; }
        public bool HideLabel { get; set; }

        public string ResultFontName { get; set; }
        public FontStyle ResultFontStyle { get; set; }
        public string ResultFontSize { get; set; }
        public string ResultForeColor { get; set; }

        public FloatType TitlePosition { get; set; }
       public QueryFormatType FormatType { get; set; }
        public string BackgroundImage { get; set; }
        public Size SizeOfFrame { get; set; }
        public Point Location { get; set; }
       public ResultPanelType ResultPanelDesign { get; set; }
        public string IconImage { get; set; }
        public Point IconLocation { get; set; }
        public Size IconSize { get; set; }
        public bool EnableEditUrl { get; set; }
        public string BackgroundColor { get; set; }
        public string TextAlign { get; set; }
        public string BorderColor { get; set; }
        public int BorderWidth { get; set; }
        public string HeaderColor { get; set; }
        public string RowColor { get; set; }
        public string RowAlternateColor { get; set; }
        public string DrillDownType { get; set; }
        public string CustomUrl { get; set; }
        public string NavigateType { get; set; }
        public string QueryId { get; set; }
        public string Header { get; set; }
        public string Footer { get; set; }
        public bool ShowCompanyLogo { get; set; }
        public string AdditionalInfo { get; set; }
        public string AdditionalFooterInfo { get; set; }
        public bool ShowDateInFooter { get; set; }
        public bool IsTransparent { get; set; }

    }

    [Serializable]
    public class OrderByInfo
    {
        public int Num { get; set; }
        public ColumnInfo Column { get; set; }
        public OrderBY orderBy { get; set; }
    }

    [Serializable]
    public class GroupByInfo
    {
        public ColumnInfo Column { get; set; }
        public int Num { get; set; }
    }
    [Serializable]
    public class ScheduleAction
    {
        public long ScheduleId { get; set; }
        public string Title { get; set; }
        public DateTime StartTime { get; set; }
        public string EmailTo { get; set; }
        public string EmailCC { get; set; }
        public string EmailSubject { get; set; }
        public string EmailBody { get; set; }
        public ScheduleActionType ActionType { get; set; }
        public bool Recurring { get; set; }
        public int? RecurringInterval { get; set; }
        public DateTime? RecurringEndDate { get; set; }
        public string CustomProperty { get; set; }
        public bool IsEnable { get; set; }
        public string AttachmentFormat { get; set; }
        public string ActionTypeData { get; set; }
    }

    [Serializable]
    public class ScheduleActionParameter
    {
        public string ModuleName { get; set; }
        public string SelectedType { get; set; }
        public string SortType { get; set; }
        public bool IsModuleSort { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }

    [Serializable]
    public class Where
    {
        public string StartWith { get; set; }
        public string Column { get; set; }
        public string OperatorType { get; set; }
        public string Value { get; set; }
        public string DataType { get; set; }
        public Boolean IsEditable { get; set; }
        public string ParameterName { get; set; }
    }

    [Serializable]
    public class WhereInfo
    {
        public int ID { get; set; }
        public RelationalOperator RelationOpt { get; set; }
        public string ColumnName { get; set; }
        public string DataType { get; set; }
        public OperatorType Operator { get; set; }
        public qValueType Valuetype { get; set; }
        public string ParameterType { get; set; }
        public FilterOptionsDropdown DrpOptions { get; set; }
        public FilterLookupList LookupList { get; set; }
        public string Value { get; set; }
        public string ParameterName { get; set; }
        public bool ParameterRequired { get; set; }
        public int ParentID { get; set; }
        public string TimeUnit { get; set; }
        public int FrequencyUnit { get; set; }
        public string TableName { get; set; }

        public object Clone()
        {
            WhereInfo d = new WhereInfo();
            d.ID = this.ID;
            d.RelationOpt = this.RelationOpt;
            d.ColumnName = this.ColumnName;
            d.DataType = this.DataType;
            d.Operator = this.Operator;
            d.Valuetype = this.Valuetype;
            d.ParameterType = this.ParameterType;
            d.DrpOptions = this.DrpOptions;
            d.LookupList = this.LookupList;
            d.Value = this.Value;
            d.ParameterName = this.ParameterName;
            d.ParameterRequired = this.ParameterRequired;
            d.ParentID = this.ParentID;
            d.FrequencyUnit = 1;
            d.TableName = this.TableName;
            return d;
        }
        
    }

    [Serializable]
    public class FilterOptionsDropdown
    {
        public string OptionsDropdown { get; set; }
        public string DropdownDefaultValue { get; set; }
    }

    [Serializable]
    public class FilterLookupList
    {
        public string LookupListName { get; set; }
        public string LookupModuleName { get; set; }
        public string LookupField { get; set; }
    }

    [Serializable]
    public enum RelationalOperator
    {
        None,
        AND,
        OR
    }

    [Serializable]
    public enum OperatorType
    {
        None,
        Equal,
        NotEqual,
        GreaterThan,
        GreaterThanEqualTo,
        LessThan,
        LessThanEqualTo,
        like,
        MemberOf
    }

    [Serializable]
    public enum qValueType
    {
        None,
        Parameter,
        Constant,
        Variable
    }

    [Serializable]
    public enum OrderBY
    {
        ASC,
        DESC
    }

    [Serializable]
    public enum JoinType
    {
        INNER,
        OUTER,
        MERGE,
        UNION
    }
    [Serializable]
    public enum FloatType
    {
        Top = 1,
        Bottom = 2,
        Left = 3,
        Right = 4,
    }
    [Serializable]
    public enum QueryFormatType
    {
        SimpleNumber = 1,
        FormattedNumber = 2,
        Column = 3,
        Row = 4,
        Table = 5
    }
    [Serializable]
    public enum ResultPanelType
    {

        WithoutIconOrBorder = 1,
        WithIconAndNoBorder = 2,
        WithIconAndBorder = 3,
    }
    #region Query Helper Region
    //Query Helper Region

    #endregion

    [Serializable]
    public class Condition
    {
        public string Field { get; set; }
        public OperatorType Operator { get; set; }
        public int Value { get; set; }
    }

}

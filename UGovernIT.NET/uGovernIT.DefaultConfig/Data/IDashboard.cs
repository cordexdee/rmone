using System.Collections.Generic;
using uGovernIT.Utility;

namespace uGovernIT.DefaultConfig
{
    public interface IDashboard : IModule
    {
        List<ChartTemplate> GetChartTemplates();
        List<ChartFilter> GetChartFilters();
        List<DashboardSummary> GetDashboardSummary();
        List<ChartFormula> GetChartFormula();
        List<DashboardFactTables> GetDashboardFactTables();
        List<ModuleSLARule> GetModuleSLARule();
        List<ModuleEscalationRule> GetModuleEscalationRule();
    }
}

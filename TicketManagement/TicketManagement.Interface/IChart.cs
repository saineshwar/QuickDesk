using System.Collections.Generic;
using TicketManagement.ViewModels;

namespace TicketManagement.Interface
{
    public interface IChart
    {
        List<ChartViewModel> GetListofBarChartData(long userid);
        List<DonutChartViewModel> GetListofDonutChartData(long userid);
        List<ChartViewModel> GetListofBarChartDataAdmin();
        List<ChartViewModel> GetListofBarChartDataAgentsAdmin(int categoryId);
        List<DonutChartViewModel> GetListofDonutChartDataAdmin();
        List<DonutChartViewModel> GetListofDonutChartDataAgentsAdmin(int categoryId);
    }
}
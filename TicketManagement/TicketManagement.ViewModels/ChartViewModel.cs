using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagement.ViewModels
{
    public class ChartViewModel
    {
        public string y { get; set; }
        public int a { get; set; }
        public int b { get; set; }
    }

    public class DonutChartViewModel
    {
        public int New { get; set; }
        public int Resolved { get; set; }
        public int InProgress { get; set; }
        public int OnHold { get; set; }
        public int RecentlyEdited { get; set; }
        public int Replied { get; set; }
        public int Deleted { get; set; }
        public int StatusId { get; set; }
    }

    public class DonutChartRequest
    {
        public int value { get; set; }
        public string label { get; set; }
        
    }

    public class DonutChartResponse
    {
        public string label { get; set; }
        public int value { get; set; }
    }
}

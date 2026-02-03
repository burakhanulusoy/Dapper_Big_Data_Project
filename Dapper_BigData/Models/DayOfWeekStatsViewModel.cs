namespace Dapper_BigData.Models
{
    public class DayOfWeekStatsViewModel
    {
        public int DayNumber { get; set; } // 1: Pazar, 2: Pazartesi... (SQL Server varsayılanı)
        public int OrderCount { get; set; }
    }
}

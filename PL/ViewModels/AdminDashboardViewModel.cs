namespace PL.ViewModels
{
    public class AdminDashboardViewModel
    {
        public int CampsCount;
        public int BookingsCount;
        public int UsersCount;
        public int TodayNewUsers;
        public int TodayBooking;
        public List<int> MonthBookingStatistics = new List<int>();
        public List<int> DailyBookingStatistics = new List<int>();
        public List<decimal> WeeklyRevenue = new List<decimal>();
        public List<decimal> DailyRevenue = new List<decimal>();

        public List<string> WeekLabels = new List<string> { "Week 1", "Week 2", "Week 3", "Week 4" };
        public List<string> DayLabels = new List<string>();
        public List<string> CampNames = new List<string>();
        public List<int> CampBookings = new List<int>();
}
}

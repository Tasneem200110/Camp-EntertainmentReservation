namespace PL.ViewModels
{
    public class AdminDashboardViewModel
    {
        public int CampsCount;
        public int BookingsCount;
        public int UsersCount;
        public List<int> MonthBookingStatistics = new List<int>();
        public List<string> WeekLabels = new List<string> { "Week 1", "Week 2", "Week 3", "Week 4" };
}
}

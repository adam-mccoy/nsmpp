namespace NSmpp
{
    public struct RelativeTime
    {
        private int _years;
        private int _months;
        private int _days;
        private int _hours;
        private int _minutes;
        private int _seconds;

        public RelativeTime(int years, int months, int days, int hours, int minutes, int seconds)
        {
            _years = years;
            _months = months;
            _days = days;
            _hours = hours;
            _minutes = minutes;
            _seconds = seconds;
        }

        public int Seconds { get => _seconds; }
        public int Minutes { get => _minutes; }
        public int Hours { get => _hours; }
        public int Days { get => _days; }
        public int Months { get => _months; }
        public int Years { get => _years; }

        public static RelativeTime FromSeconds(int seconds)
        {
            return new RelativeTime { _seconds = seconds };
        }

        public static RelativeTime FromMinutes(int minutes)
        {
            return new RelativeTime { _minutes = minutes };
        }

        public static RelativeTime FromHours(int hours)
        {
            return new RelativeTime { _hours = hours };
        }

        public static RelativeTime FromDays(int days)
        {
            return new RelativeTime { _days = days };
        }

        public static RelativeTime FromMonths(int months)
        {
            return new RelativeTime { _months = months };
        }

        public static RelativeTime FromYears(int years)
        {
            return new RelativeTime { _years = years };
        }

        public override string ToString()
        {
            return $"{_years:D2}{_months:D2}{_days:D2}{_hours:D2}{_minutes:D2}{_seconds:D2}000R";
        }
    }
}

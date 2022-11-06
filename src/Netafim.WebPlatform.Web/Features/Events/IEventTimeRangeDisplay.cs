using EPiServer.Globalization;
using System;

namespace Netafim.WebPlatform.Web.Features.Events
{
    public interface IEventTimeRangeDisplay
    {
        bool Satified(DateTime from, DateTime to);

        string ToDateTimeRange(DateTime from, DateTime to);
    }

    public class EventInDayDisplay : IEventTimeRangeDisplay
    {
        public bool Satified(DateTime from, DateTime to)
        {
            return to > from && from.Date == to.Date;
        }

        public string ToDateTimeRange(DateTime from, DateTime to)
        {
            return string.Format("{0}-{1}, {2} {3}, {4}", from.ToString("HH:mm tt"), to.ToString("HH:mm tt"), from.ToString("MMMM", ContentLanguage.PreferredCulture), from.ToString("dd"), from.ToString("yyyy"));
        }
    }

    public class EventInMonthDisplay : IEventTimeRangeDisplay
    {
        public bool Satified(DateTime from, DateTime to)
        {
            return from < to && from.Year == to.Year && from.Month == to.Month;
        }

        public string ToDateTimeRange(DateTime from, DateTime to)
        {
            return string.Format("{0} {1}-{2}, {3}", from.ToString("MMMM", ContentLanguage.PreferredCulture), from.ToString("dd"), to.ToString("dd"), from.ToString("yyyy"));
        }
    }

    public class EventInYearDisplay : IEventTimeRangeDisplay
    {
        public bool Satified(DateTime from, DateTime to)
        {
            return from < to && from.Year == to.Year && from.Month != to.Month;
        }

        public string ToDateTimeRange(DateTime from, DateTime to)
        {
            return string.Format("{0} {1} - {2} {3}, {4}", from.ToString("MMMM", ContentLanguage.PreferredCulture), from.ToString("dd"),
                to.ToString("MMMM", ContentLanguage.PreferredCulture), to.ToString("dd"), from.ToString("yyyy"));
        }
    }

    public class EventInMultipYearsDisplay : IEventTimeRangeDisplay
    {
        public bool Satified(DateTime from, DateTime to)
        {
            return from < to && from.Year != to.Year;
        }

        public string ToDateTimeRange(DateTime from, DateTime to)
        {
            return string.Format("{0} - {1}", from.ToString("MMMM dd, yyyy", ContentLanguage.PreferredCulture), to.ToString("MMMM dd, yyyy", ContentLanguage.PreferredCulture));
        }
    }

    public class InvalidEventTimeRangDisplay : IEventTimeRangeDisplay
    {
        public bool Satified(DateTime from, DateTime to)
        {
            return from >= to;
        }

        public string ToDateTimeRange(DateTime from, DateTime to)
        {
            return from.ToString("MMMM dd, yyyy", ContentLanguage.PreferredCulture);
        }
    }
}
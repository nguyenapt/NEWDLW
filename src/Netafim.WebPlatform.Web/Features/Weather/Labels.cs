using DbLocalizationProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Netafim.WebPlatform.Web.Features.Weather
{
    [LocalizedResource]
    public class Labels
    {
        public static string Humidity => "Humidity";

        public static string Transpiration => "Evapo-transpiration";

        public static string Wind => "Wind speed";

        public static string MoreInformation => "MORE INFORMATION";

        public static string LessInformation => "LESS INFORMATION";

        public static string TitleFormat => "WEATHER FORECAST FOR {0}";

        public static string Today => "TODAY";
    }
}
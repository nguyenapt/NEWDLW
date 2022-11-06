using DbLocalizationProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Netafim.WebPlatform.Web.Features.GenericListing
{
    [LocalizedResource]
    public class Labels
    {
        public static string LoadMore => "LOAD MORE";
        public static string LearnMore => "LEARN MORE";
        public static string ResultFoundMessage => "Crops Found";
        public static string LookingMessage => "What crop are you looking for?";
        public static string EnterKeywordsPlaceHolder => "Enter keywords...";
    }
}
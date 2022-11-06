using System.Collections.Generic;
using System.Linq;
using SendGrid.Helpers.Mail;

namespace Netafim.WebPlatform.Web.Core.Extensions
{
    public static class EmailExtensions
    {
        public static List<EmailAddress> ToEmailAddressesBySeparator(this string source, char separator)
        {
            return source.Split(separator)
                .Where(address => !string.IsNullOrEmpty(address))
                .Select(address => new EmailAddress(address.Trim()))
                .ToList();
        }
    }
}
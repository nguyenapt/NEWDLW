using System;
using System.ComponentModel;
using System.Reflection;

namespace Netafim.WebPlatform.Web.Core.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum @enum)
        {
            FieldInfo fi = @enum.GetType().GetField(@enum.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes.Length > 0)
            {
                return attributes[0].Description;
            }
            else
            {
                return @enum.ToString();
            }
        }
    }
}
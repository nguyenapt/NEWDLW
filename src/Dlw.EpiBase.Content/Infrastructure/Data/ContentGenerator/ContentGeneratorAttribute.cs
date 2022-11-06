using System;

namespace Dlw.EpiBase.Content.Infrastructure.Data.ContentGenerator
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ContentGeneratorAttribute : Attribute
    {
        public int Order { get; set; }
    }
}
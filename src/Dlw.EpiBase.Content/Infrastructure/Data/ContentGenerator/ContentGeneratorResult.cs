using System;
using System.Collections.Generic;

namespace Dlw.EpiBase.Content.Infrastructure.Data.ContentGenerator
{
    public class ContentGeneratorResult
    {
        public ContentGeneratorResult(Type generator, IEnumerable<string> messages)
        {
            Generator = generator;
            Messages = messages;
        }

        public Type Generator { get; set; }

        public IEnumerable<string> Messages { get; set; }
    }
}
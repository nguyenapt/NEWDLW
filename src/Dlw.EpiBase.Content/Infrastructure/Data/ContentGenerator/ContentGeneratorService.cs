using EPiServer.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dlw.EpiBase.Content.Infrastructure.Data.ContentGenerator
{
    public class ContentGeneratorService : IContentGeneratorService
    {
        private readonly IEnumerable<IContentGenerator> _contentGenerators;

        private ILogger _logger = LogManager.GetLogger();

        public ContentGeneratorService(IEnumerable<IContentGenerator> contentGenerators)
        {
            _contentGenerators = contentGenerators;
        }

        public IEnumerable<ContentGeneratorResult> Generate(ContentContext contentContext)
        {
            var messages = new List<ContentGeneratorResult>();

            foreach (var contentGenerator in OrderedContentGenerators())
            {
                try
                {
                    contentGenerator.Generate(contentContext);

                    messages.Add(new ContentGeneratorResult(contentGenerator.GetType(), new[] { "success" }));
                }
                catch (Exception e)
                {
                    messages.Add(new ContentGeneratorResult(contentGenerator.GetType(), new[] { e.Message }));
                }
            }

            return messages;
        }

        private IEnumerable<IContentGenerator> OrderedContentGenerators()
        {
            return _contentGenerators.OrderBy(delegate (IContentGenerator generator)
            {
                var attribute = Attribute.GetCustomAttribute(generator.GetType(), typeof(ContentGeneratorAttribute)) as ContentGeneratorAttribute;

                return attribute?.Order ?? int.MaxValue;
            });
        }
    }
}
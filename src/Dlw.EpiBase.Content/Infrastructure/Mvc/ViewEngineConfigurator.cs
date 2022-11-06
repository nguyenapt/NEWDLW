using System.Linq;
using System.Web.Mvc;

namespace Dlw.EpiBase.Content.Infrastructure.Mvc
{
    public class ViewEngineConfigurator
    {
        public void Configure(ViewEngineCollection engines)
        {
            var razor = engines.OfType<RazorViewEngine>().Single(IsNotSpecializedType);
            engines.Remove(razor);

            engines.Add(new FeatureRazorViewEngine());
        }

        private bool IsNotSpecializedType<T>(T arg) where T : RazorViewEngine
        {
            return !arg.GetType().IsSubclassOf(typeof(RazorViewEngine));
        }
    }
}
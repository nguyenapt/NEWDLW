using EPiServer.Core;
using EPiServer.Framework.Serialization;
using EPiServer.Framework.Serialization.Internal;
using EPiServer.ServiceLocation;

namespace Dlw.EpiBase.Content.Infrastructure.Epi.Shell
{
    // source: https://world.episerver.com/blogs/per-magne-skuseth/dates/2015/11/trying-out-propertylistt/
    public abstract class PropertyListBase<T> : PropertyList<T>
    {
        private Injected<ObjectSerializerFactory> _objectSerializerFactory;

        private readonly IObjectSerializer _objectSerializer;

        protected PropertyListBase()
        {
            _objectSerializer = this._objectSerializerFactory.Service.GetSerializer("application/json");
        }

        protected override T ParseItem(string value)
        {
            return _objectSerializer.Deserialize<T>(value);
        }

        public override PropertyData ParseToObject(string value)
        {
            ParseToSelf(value);
            return this;
        }
    }
}
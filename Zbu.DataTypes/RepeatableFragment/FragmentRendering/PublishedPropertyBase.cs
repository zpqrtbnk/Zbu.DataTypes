using System;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;

namespace Zbu.DataTypes.RepeatableFragment.FragmentRendering
{
    abstract class PublishedPropertyBase : IPublishedProperty
    {
        public readonly PublishedPropertyType PropertyType;

        protected PublishedPropertyBase(PublishedPropertyType propertyType)
        {
            if (propertyType == null)
                throw new ArgumentNullException("propertyType");
            PropertyType = propertyType;
        }

        public string PropertyTypeAlias
        {
            get { return PropertyType.PropertyTypeAlias; }
        }

        // these have to be provided by the actual implementation
        public abstract bool HasValue { get; }
        public abstract object DataValue { get; }
        public abstract object Value { get; }
        public abstract object XPathValue { get; }
    }
}

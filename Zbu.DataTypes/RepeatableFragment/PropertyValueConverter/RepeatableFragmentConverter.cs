using System;
using System.Collections.Generic;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.PropertyEditors;

namespace Zbu.DataTypes.RepeatableFragment.PropertyValueConverter
{
    // FIXME no! we can cache the IPublishedContent, and their properties will NOT be cached?
    [PropertyValueCache(PropertyCacheValue.All, PropertyCacheLevel.Request)] // no idea what the inner types do
    [PropertyValueType(typeof(IEnumerable<IPublishedContent>))]
    class RepeatableFragmentConverter : IPropertyValueConverter
    {
        private IEnumerable<IPublishedContent> _fragments;

        public object ConvertDataToSource(PublishedPropertyType propertyType, object source, bool preview)
        {
            var json = source.ToString();

            // FIXME how shall we handle 'preview' ?! == pass it along to inner contents
            // FIXME json should contain the fragment type if we don't want to get the prevalues

            throw new NotImplementedException();
        }

        public object ConvertSourceToObject(PublishedPropertyType propertyType, object source, bool preview)
        {
            return _fragments;
        }

        public object ConvertSourceToXPath(PublishedPropertyType propertyType, object source, bool preview)
        {
            throw new NotImplementedException();
        }

        public bool IsConverter(PublishedPropertyType propertyType)
        {
            return propertyType.PropertyEditorGuid == RepeatableFragmentDataType.RepeatableFragmentGuid;
        }
    }
}

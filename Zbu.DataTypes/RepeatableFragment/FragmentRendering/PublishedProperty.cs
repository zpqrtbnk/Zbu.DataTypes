using System;
using System.Xml.Serialization;
using Umbraco.Core.Models.PublishedContent;

namespace Zbu.DataTypes.RepeatableFragment.FragmentRendering
{
    [Serializable]
    [XmlType(Namespace = "http://umbraco.org/webservices/")]
    class PublishedProperty : PublishedPropertyBase
    {
        private readonly object _dataValue;
        private readonly PublishedFragment _content;

        public PublishedProperty(PublishedPropertyType propertyType, PublishedFragment content)
            : base(propertyType)
        {
            _dataValue = null;
            _content = content;
        }

        public PublishedProperty(PublishedPropertyType propertyType, PublishedFragment content, object dataValue)
            : base(propertyType)
        {
            _dataValue = dataValue;
            _content = content;
        }

        public override bool HasValue
        {
            get { return _dataValue != null && ((_dataValue is string) == false || string.IsNullOrWhiteSpace((string)_dataValue) == false); }
        }

        public override object DataValue
        {
            get { return _dataValue; }
        }

        public override object Value
        {
            get
            {
                var source = PropertyType.ConvertDataToSource(_dataValue, _content.IsPreviewing);
                return PropertyType.ConvertSourceToObject(source, _content.IsPreviewing);
            }
        }

        public override object XPathValue
        {
            get
            {
                var source = PropertyType.ConvertDataToSource(_dataValue, _content.IsPreviewing);
                return PropertyType.ConvertSourceToXPath(source, _content.IsPreviewing);
            }
        }
    }
}

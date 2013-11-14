using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web.Models;

namespace Zbu.DataTypes.RepeatableFragment.FragmentRendering
{
    class PublishedFragment : PublishedContentBase
    {
        private readonly string _contentTypeAlias;
        private readonly PublishedContentType _contentType;
        private readonly IPublishedProperty[] _properties;
        private readonly bool _isPreviewing;

        public PublishedFragment(string contentTypeAlias, IDictionary<string, object> dataValues, bool isPreviewing)
        {
            _isPreviewing = isPreviewing;
            _contentTypeAlias = contentTypeAlias;
            _contentType = PublishedContentType.Get(PublishedItemType.Content, _contentTypeAlias);

            _properties = _contentType.PropertyTypes
                .Select(x =>
                {
                    object dataValue;
                    return dataValues.TryGetValue(x.PropertyTypeAlias, out dataValue) 
                        ? new PublishedProperty(x, this, dataValue) 
                        : new PublishedProperty(x, this);
                })
                .Cast<IPublishedProperty>()
                .ToArray();
        }

        #region IPublishedContent

        public override int Id
        {
            get { throw new NotImplementedException(); }
        }

        public override int DocumentTypeId
        {
            get { throw new NotImplementedException(); }
        }

        public override string DocumentTypeAlias
        {
            get { return _contentTypeAlias; }
        }

        public override PublishedItemType ItemType
        {
            get { return PublishedItemType.Content; }
        }

        public override string Name
        {
            get { throw new NotImplementedException(); }
        }

        public override int Level
        {
            get { throw new NotImplementedException(); }
        }

        public override string Path
        {
            get { throw new NotImplementedException(); }
        }

        public override int SortOrder
        {
            get { throw new NotImplementedException(); }
        }

        public override Guid Version
        {
            get { throw new NotImplementedException(); }
        }

        public override int TemplateId
        {
            get { throw new NotImplementedException(); }
        }

        public override string UrlName
        {
            get { return string.Empty; }
        }

        public override DateTime CreateDate
        {
            get { throw new NotImplementedException(); }
        }

        public override DateTime UpdateDate
        {
            get { throw new NotImplementedException(); }
        }

        public override int CreatorId
        {
            get { throw new NotImplementedException(); }
        }

        public override string CreatorName
        {
            get { throw new NotImplementedException(); }
        }

        public override int WriterId
        {
            get { throw new NotImplementedException(); }
        }

        public override string WriterName
        {
            get { throw new NotImplementedException(); }
        }

        public override bool IsDraft
        {
            get { throw new NotImplementedException(); }
        }

        public override IPublishedContent Parent
        {
            get { throw new NotImplementedException(); }
        }

        public override IEnumerable<IPublishedContent> Children
        {
            get { throw new NotImplementedException(); }
        }

        public override ICollection<IPublishedProperty> Properties
        {
            get { return _properties; }
        }

        public override IPublishedProperty GetProperty(string alias)
        {
            return _properties.FirstOrDefault(x => x.PropertyTypeAlias.InvariantEquals(alias));
        }

        public override PublishedContentType ContentType
        {
            get { return _contentType; }
        }

        #endregion

        #region Internal

        // note: this is the "no cache" published cache so nothing is cached here
        // which means that it is going to be a disaster, performance-wise

        //IPublishedProperty[] PropertiesArray { get { return _properties; } }

        public bool IsPreviewing { get { return _isPreviewing; } }

        #endregion
    }
}

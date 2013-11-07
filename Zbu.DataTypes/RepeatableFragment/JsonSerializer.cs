using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Zbu.DataTypes.RepeatableFragment
{
    class JsonSerializer
    {
        private readonly JsonSerializerSettings _settings;

        public JsonSerializer()
        {
            _settings = new JsonSerializerSettings();

            // NOTE: no idea whether we really need all this?
            // though this is what the serializer in Core uses...

            var javaScriptDateTimeConverter = new JavaScriptDateTimeConverter();

            _settings.Converters.Add(javaScriptDateTimeConverter);
            _settings.Converters.Add(new EntityKeyMemberConverter());
            _settings.Converters.Add(new KeyValuePairConverter());
            _settings.Converters.Add(new ExpandoObjectConverter());
            _settings.Converters.Add(new XmlNodeConverter());

            _settings.NullValueHandling = NullValueHandling.Include;
            _settings.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;
            _settings.TypeNameHandling = TypeNameHandling.Objects;
            //_settings.TypeNameHandling = TypeNameHandling.None;
            _settings.ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor;
        }

        public string Serialize(object o)
        {
            return JsonConvert.SerializeObject(o, Formatting.Indented, _settings);
        }

        public T Deserialize<T>(string s)
            where T : class
        {
            return JsonConvert.DeserializeObject(s, typeof (T), _settings) as T;
        }
    }
}

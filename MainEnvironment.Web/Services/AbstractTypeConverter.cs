using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainEnvironment.Web.Services
{
    public class AbstractTypeConverter<TAbstract, TConcrete> : JsonConverter where TConcrete : TAbstract
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.FullName == typeof(TAbstract).FullName;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var serialiser = JsonSerializer.Create(new JsonSerializerSettings()
            {
                Converters = serializer.Converters,
                Culture = serializer.Culture,
            });
            return (TAbstract)serialiser.Deserialize(reader, typeof(TConcrete));
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            string json = JsonConvert.SerializeObject(value, serializer.Converters.ToArray());
            writer.WriteRaw(json);
        }
    }
}

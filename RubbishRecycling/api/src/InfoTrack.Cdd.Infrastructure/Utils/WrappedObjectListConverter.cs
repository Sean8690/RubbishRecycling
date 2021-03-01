using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace InfoTrack.Cdd.Infrastructure.Utils
{
    public class WrappedObjectListConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                return;
            }

            Type type = value.GetType();
            JObject jo = new JObject();

            foreach (PropertyInfo prop in type.GetProperties().Where(p => p.CanRead))
            {
                var propName = prop.GetCustomAttributes<JsonPropertyAttribute>().Select(p => p.PropertyName).FirstOrDefault() ?? prop.Name;
                object propValue = prop.GetValue(value, null);
                if (propValue == null)
                {
                    continue; // ignore nulls
                }

                JToken token = JToken.FromObject(propValue, serializer);
                if (IsList(propValue))
                {
                    JsonWrappedObjectListAttribute att = prop.GetCustomAttributes<JsonWrappedObjectListAttribute>()
                        .SingleOrDefault();

                    if (att != null)
                    {
                        var nestedPropertyName = att.PropertyName ?? propValue.GetType().GetGenericArguments()[0].Name;
                        JObject wrapperJo = new JObject(new JProperty(nestedPropertyName, token));
                        token = JToken.FromObject(wrapperJo);
                    }
                }
                jo.Add(propName, token);
            }

            jo.WriteTo(writer);
        }

        private static bool IsList(object o)
        {
            if (o == null) return false;
            return o is IList &&
                   o.GetType().IsGenericType &&
                   o.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (objectType == null)
            {
                return null;
            }

            JObject jsonObject = JObject.Load(reader);
            List<JProperty> properties = jsonObject.Properties().ToList();

            object instance = Activator.CreateInstance(objectType);

            PropertyInfo[] objectProperties = objectType.GetProperties();
            foreach (var prop in objectProperties)
            {
                var propName = prop.GetCustomAttributes<JsonPropertyAttribute>().Select(p => p.PropertyName).FirstOrDefault()
                               ?? prop.Name;

                var att = prop.GetCustomAttributes<JsonWrappedObjectListAttribute>().SingleOrDefault();
                if (att != null)
                {
                    JProperty jsonProperty = properties.SingleOrDefault(p => p.Name == propName);
                    if (jsonProperty != null)
                    {
                        if (jsonProperty.Value is JArray)
                        {
                            var typedProperty = properties.SingleOrDefault(p => p.Name == propName)?.Value?.ToObject(prop.PropertyType);
                            prop.SetValue(instance, typedProperty);
                        }
                        else if (jsonProperty.Value is JObject)
                        {
                            JObject asObj = JObject.FromObject(jsonProperty.Value);
                            var nestedPropertyName = att.PropertyName ?? prop.PropertyType.GetGenericArguments()[0].Name;
                            var nestedProperty = asObj.Properties().SingleOrDefault(p => p.Name == nestedPropertyName)?.Value;

                            var typedNestedProperty = nestedProperty?.ToObject(prop.PropertyType);
                            prop.SetValue(instance, typedNestedProperty);
                        }
                    }
                }
                else
                {
                    var typedProperty = properties.SingleOrDefault(p => p.Name == propName)?.Value?.ToObject(prop.PropertyType);
                    try
                    {
                        prop.SetValue(instance, typedProperty);
                    }
                    catch (ArgumentException e)
                    {
                        // ignore exception thrown if property has no setter
                        if (e.Message != "Property set method not found.")
                        {
                            throw;
                        }
                    }
                }
            }

            return instance;
        }

        public override bool CanWrite => true;

        public override bool CanRead => true;
     
        public override bool CanConvert(Type objectType) => true;
    }
}

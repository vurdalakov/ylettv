namespace Vurdalakov
{
    using System;
    using Newtonsoft.Json.Linq;

    static public class JObjectExtensions
    {
        static public JArray GetArray(this JObject jObject, String propertyName)
        {
            JToken propertyValue;
            return jObject.TryGetValue(propertyName, out propertyValue) && (JTokenType.Array == propertyValue.Type) ? propertyValue as JArray : null;
        }

        static public DateTime GetDate(this JObject jObject, String propertyName, DateTime defaultValue)
        {
            JToken propertyValue;
            return jObject.TryGetValue(propertyName, out propertyValue) && (JTokenType.Date == propertyValue.Type) ? (DateTime)propertyValue : defaultValue;
        }

        static public Double GetJsonDouble(JObject jObject, String propertyName, Double defaultValue) // Double.NaN
        {
            JToken propertyValue;
            return jObject.TryGetValue(propertyName, out propertyValue) && (JTokenType.Float == propertyValue.Type) ? (Double)propertyValue : defaultValue;
        }

        static public Int32 GetInt32(this JObject jObject, String propertyName, Int32 defaultValue)
        {
            JToken propertyValue;
            return jObject.TryGetValue(propertyName, out propertyValue) && (JTokenType.Integer == propertyValue.Type) ? (Int32)propertyValue : defaultValue;
        }

        static public String GetString(this JObject jObject, String propertyName, String defaultValue)
        {
            JToken propertyValue;
            return jObject.TryGetValue(propertyName, out propertyValue) && (JTokenType.String == propertyValue.Type) ? (String)propertyValue : defaultValue;
        }
    }
}

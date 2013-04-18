#region License
// YleTtv is a C# class library that simplifies work with YLE teletext (teksti-TV) API
// https://github.com/vurdalakov/ylettv
//
// Copyright (c) 2013 Vurdalakov
// email: vurdalakov@gmail.com

/*
    This file is part of YleTtv class library.

    YleTtv is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    YleTtv is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with YleTtv.  If not, see <http://www.gnu.org/licenses/>.
*/
#endregion

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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FifthElement.LogforceLoadingHybrid.Core.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FifthElement.LogforceLoadingHybrid.Core.Util
{
    public static class KotkaJsonUtil
    {
        public static string ConvertJsonListToJsonArray(IList<string> listToBeConverted)
        {
            return string.Format("[{0}]", string.Join(",", listToBeConverted.ToArray()));
        }

        public static string ConvertJsonListToJsonObject(IList<string> listToBeConverted)
        {
            return string.Format("{{{0}}}", string.Join(",", listToBeConverted.ToArray()));
        }

        /// <summary>
        /// Combines all the members of the Json objects that are passed as parameter
        /// </summary>
        /// <returns>Serialized Json object</returns>
        /// <param name="jsonObjects">List of serialized Json objects</param>
        public static string CombineJsonObjects(List<string> jsonObjects)
        {
            var result = new JObject();
            foreach (var jsonObject in jsonObjects)
            {
                var obj = JsonConvert.DeserializeObject<JObject>(jsonObject);
                foreach (var property in obj)
                {
                    result[property.Key] = property.Value;
                }
            }
            return result.ToString();
        }

        public static string UpdateJsonObject(string jsonObject, string key, JToken value)
        {
            JObject temp = JObject.Parse(jsonObject);
            temp[key] = value;
            return temp.ToString();
        }

        public static string UpdateJsonObject(string jsonObject, string key, JToken value, Constants.JsonReplaceFlags flags)
        {
            JToken temp = JToken.Parse(jsonObject);
            temp = UpdateJsonObject(temp,key,value,flags);
            return temp.ToString();
        }

        public static JToken UpdateJsonObject(JToken jsonObject, string propertyPath, JToken value, Constants.JsonReplaceFlags flags)
        {
            var property = jsonObject.SelectToken(propertyPath); // o.SelectToken("Manufacturers[0].Name");

            if ((property == null) && (flags == Constants.JsonReplaceFlags.Replace))
                return jsonObject;
            if ((property.Type == JTokenType.Null) && (flags == Constants.JsonReplaceFlags.Replace))
                return jsonObject;

            var propertyPathItems = propertyPath.Split('.');
            switch (propertyPathItems.Count())
            {
                case  1 :
                    {
                        jsonObject[propertyPathItems[0]] = value;
                        break;
                    }
                case 2:
                    {
                        jsonObject[propertyPathItems[0]][propertyPathItems[1]] = value;
                        break;
                    }
                case 3:
                    {
                        jsonObject[propertyPathItems[0]][propertyPathItems[1]][propertyPathItems[2]] = value;
                        break;
                    }
                default:
                    {
                        throw new Exception("UpdateJsonObject() function accepts maxdepth 3 of Json path");
                    }
            }
            return jsonObject;
        }


        public static string JsonStringObject(string key, string value)
        {
            var temp = new JObject();
            temp[key] = value;
            return temp.ToString();
        }
        public static string JsonEscape(string value)
        {
            var stringWriter = new StringWriter();
            JsonWriter jsonWriter = new JsonTextWriter(stringWriter);
            jsonWriter.WriteValue(value);
            string output = stringWriter.ToString();
            return output;
        }


        private static bool IsJson(string jsonData)
        {
            return jsonData.Trim().Substring(0, 1).IndexOfAny(new[] { '[', '{' }) == 0;
        }
        public static string ToJsonObject(string value)
        {
            if (IsJson(value)) return value;
            return JsonStringObject("message", value);
        }

        public static string RemovePropertyFromObject(string objectString, string propertyName)
        {
            var jsonObject = JToken.Parse(objectString);
            var property = jsonObject[propertyName];
            if (property == null)
                return objectString;
            if (property.Parent == null)
                return objectString;
            jsonObject[propertyName].Parent.Remove();
            return jsonObject.ToString();
        }

        public static string RemovePropertyFromObject(string objectString, string property1Name, string property2Name)
        {
            var jsonObject = JToken.Parse(objectString);
            var property = jsonObject[property1Name];
            if (property == null)
                return objectString;
            property = property[property2Name];
            if (property == null)
                return objectString;
            if (property.Parent == null)
                return objectString;
            jsonObject[property1Name][property2Name].Parent.Remove();
            return jsonObject.ToString();
        }


        private static bool IsJsonValid(string json)
        {
            try
            {
                var test = JToken.Parse(json);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public static bool IsJsonEmptyOrValid(string json)
        {
            return String.IsNullOrEmpty(json.Trim()) || IsJsonValid(json);
        }
    }
}

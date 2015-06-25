/*  
	Licensed under the Apache License, Version 2.0 (the "License");
	you may not use this file except in compliance with the License.
	You may obtain a copy of the License at
	
	http://www.apache.org/licenses/LICENSE-2.0
	
	Unless required by applicable law or agreed to in writing, software
	distributed under the License is distributed on an "AS IS" BASIS,
	WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
	See the License for the specific language governing permissions and
	limitations under the License.
*/

using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace FifthElement.Cordova.Core
{
    /// <summary>
    /// Provides JSON serialization/deserialization functionality.
    /// </summary>
    public static class JsonHelper
    {
        /// <summary>
        /// Serializes object to JSON string representation
        /// </summary>
        /// <param name="obj">object to serialize</param>
        /// <returns>JSON representation of the object. Returns 'null' string for null passed as argument</returns>
        public static string Serialize(object obj)
        {
            if (obj == null)
            {
                return "null";
            }

            var ser = new DataContractJsonSerializer(obj.GetType());

            var ms = new MemoryStream();
            ser.WriteObject(ms, obj);

            ms.Position = 0;

            string json = String.Empty;

            using (var sr = new StreamReader(ms))
            {
                json = sr.ReadToEnd();
            }

            ms.Close();

            return json;
        }

        /// <summary>
        /// Parses json string to object instance
        /// </summary>
        /// <typeparam name="T">type of the object</typeparam>
        /// <param name="json">json string representation of the object</param>
        /// <returns>Deserialized object instance</returns>
        public static T Deserialize<T>(string json)
        {
            var deserializer = new DataContractJsonSerializer(typeof (T));

            using (var mem = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                return (T) deserializer.ReadObject(mem);
            }
        }
    }
}
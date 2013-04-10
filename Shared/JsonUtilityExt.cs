using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Renren.Components.Tools
{
    public static class JsonUtilityExt
    {
        public static object DeserializeObj(Stream inputStream, Type objType)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(objType);
            
            object result = serializer.ReadObject(inputStream);
            return result;
        }

        public static object DeserializeJsonStreamAs(this Stream inputStream, Type objType)
        {
            return DeserializeObj(inputStream, objType);
        }

        public static object DeserializeJsonStringAs(this string json, Type objType, Encoding encode = null)
        {
            encode = encode ?? Encoding.UTF8;

            object graph = null;
            using (MemoryStream input = new MemoryStream(encode.GetBytes(json)))
            {
                graph = DeserializeObj(input, objType);
            }

            return graph;
        }

        public static string SerializeObj(Type objType, object contractedObj)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(objType);

            string result = string.Empty;

            if (ser != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    ser.WriteObject(ms, contractedObj);
                    result = Encoding.UTF8.GetString(ms.ToArray(), 0, ms.ToArray().Length);
                }

                return result;
            }

            return string.Empty;
        }

        public static string SerializeAsJsonString(this object graph, Type objType)
        {
            string result = SerializeObj(objType, graph);

            return result;
        }
    }
}

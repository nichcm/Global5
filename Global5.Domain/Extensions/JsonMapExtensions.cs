using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Global5.Domain.Extensions
{
    public static class JsonMapExtensions
    {
        public static string BindToString(
            JObject[] json, Dictionary<string, string> dict)
        {
            if (json == null || dict == null) return "{}";

            foreach (var item in dict
                .Where(item => !json.Properties()
                    .Any(x => x.Name
                        .Equals(
                            item.Key,
                            StringComparison.InvariantCultureIgnoreCase))))
            {
                dict.Remove(item.Key);
            }

            foreach (var prop in json.Properties().ToArray())
            {
                if (!dict.Keys.Any(x => x.Equals(
                    prop.Name, StringComparison.InvariantCultureIgnoreCase)))
                {
                    prop.Remove();
                    continue;
                }

                prop.Replace(new JProperty(
                    dict[prop.Name], prop.Value));
            }
            return JsonConvert.SerializeObject(json, Formatting.Indented);
        }

        public static string BindToString(
            JObject json, Dictionary<string, string> dict)
        {
            if (json == null || dict == null) return "{}";

            var newJson = new JObject();

            RecursiveNode(json, n =>
            {
                foreach (var item in dict)
                {
                    JToken prop = n[item.Key];
                    if (prop != null)
                    {
                        string value = prop.Value<string>();
                        newJson.TryAdd(item.Value, value);
                    }
                }
            });

            var deserialized = JsonConvert.SerializeObject(newJson);

            return deserialized;
        }

        public static T Bind<T>(JObject[] json, Dictionary<string, string> dict)
        {
            var value = BindToString(json, dict);

            return JsonConvert.DeserializeObject<T>(value);
        }

        public static T Bind<T>(JObject json, Dictionary<string, string> dict)
        {
            var value = BindToString(json, dict);

            return JsonConvert.DeserializeObject<T>(value);
        }

        public static string Bind<T>(T json)
        {
            return JsonConvert.SerializeObject(json);
        }
        public static T Bind<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static Dictionary<string, string> Bind(JObject[] json, Dictionary<string, string> dict) =>
            Bind<Dictionary<string, string>>(json, dict);

        public static Dictionary<string, string> Bind(JObject json, Dictionary<string, string> dict) =>
            Bind<Dictionary<string, string>>(json, dict);

        private static void RecursiveNode(JToken node, Action<JObject> action)
        {
            if (node.Type == JTokenType.Object)
            {
                action((JObject)node);

                foreach (JProperty child in node.Children<JProperty>())
                {
                    RecursiveNode(child.Value, action);
                }
            }
            else if (node.Type == JTokenType.Array)
            {
                foreach (JToken child in node.Children())
                {
                    RecursiveNode(child, action);
                }
            }
        }
    }
}
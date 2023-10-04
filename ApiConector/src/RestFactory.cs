using Newtonsoft.Json;
using Polly;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ApiConnector
{
    public static class RestFactory
    {
        public static async Task<T> GetApiAsync<T>(this HttpClient client, string url, string mediatype = MediaTypes.Json) where T : class
        {
            var response = new HttpResponseMessage();
            await Policy.Handle<Exception>()
                    .WaitAndRetryAsync(3, i => TimeSpan.FromSeconds(10)).ExecuteAsync(async () =>
                    {
                        response = await client.GetAsync(url);
                    });

            return await response.DeserializeObject<T>(mediatype);
        }

        public static async Task<T> GetApiAsync<T>(this HttpClient client, string url, string token, string tokenType, string mediatype = MediaTypes.Json) where T : class
        {
            if (!client.DefaultRequestHeaders.Contains("Authorization"))
                client.DefaultRequestHeaders.Add("Authorization", $"{tokenType} {token}");

            var response = new HttpResponseMessage();
            await Policy.Handle<Exception>()
                    .WaitAndRetryAsync(3, i => TimeSpan.FromSeconds(10)).ExecuteAsync(async () =>
                    {
                        response = await client.GetAsync(url);
                    });

            return await response.DeserializeObject<T>(mediatype);
        }

        private static async Task<T> DeserializeObject<T>(this HttpResponseMessage httpRequestMessage, string mediaType = MediaTypes.Json)
        {
            var content = await httpRequestMessage.Content.ReadAsStringAsync();
            if (mediaType == MediaTypes.Json)
                return JsonConvert.DeserializeObject<T>(content);

            return content.Deserialize<T>();
        }

        private static T Deserialize<T>(this string content)
        {
            var Serialize = new XmlSerializer(typeof(T));
            using (StringReader sr = new StringReader(content))
            {
                return (T)Serialize.Deserialize(sr);
            }
        }

        public static async Task<T> PostApiAsync<T>(this HttpClient client, string url, object obj, string mediaType) where T : class
        {
            var response = new HttpResponseMessage();

            await Policy.Handle<Exception>()
                    .WaitAndRetryAsync(3, i => TimeSpan.FromSeconds(10)).ExecuteAsync(async () =>
                    {
                        response = await client.PostAsync(url, new StringContent(obj.SerializeObject(), Encoding.UTF8, mediaType));
                    });

            return await response.DeserializeObject<T>();
        }

        public static async Task<T> PostApiAsync<T>(this HttpClient client, string url, object obj, string mediaType, string token, string tokenType = TokenTypes.Bearer) where T : class
        {
            var response = new HttpResponseMessage();

            if (!client.DefaultRequestHeaders.Contains("Authorization"))
                client.DefaultRequestHeaders.Add("Authorization", $"{tokenType} {token}");


            await Policy.Handle<Exception>()
                    .WaitAndRetryAsync(3, i => TimeSpan.FromSeconds(10)).ExecuteAsync(async () =>
                    {
                        response = await client.PostAsync(url, new StringContent(obj.SerializeObject(), Encoding.UTF8, mediaType));
                    });

            return await response.DeserializeObject<T>();
        }
        public static async Task<T> PutApiAsync<T>(this HttpClient client, string url, object obj, string mediaType, string token, string tokenType = TokenTypes.Bearer) where T : class
        {
            var response = new HttpResponseMessage();

            if (!client.DefaultRequestHeaders.Contains("Authorization"))
                client.DefaultRequestHeaders.Add("Authorization", $"{tokenType} {token}");


            await Policy.Handle<Exception>()
                    .WaitAndRetryAsync(3, i => TimeSpan.FromSeconds(10)).ExecuteAsync(async () =>
                    {

                        response = await client.PutAsync(url, new StringContent(obj.SerializeObject(), Encoding.UTF8, mediaType));
                    });

            return await response.DeserializeObject<T>();
        }
        private static string SerializeObject(this object obj) => JsonConvert.SerializeObject(obj);

    }
}
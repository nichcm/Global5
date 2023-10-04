using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ApiConnector
{
    public static class Connector
    {
        /// <summary>
        /// Execute GetAsync
        /// </summary>
        /// <typeparam name="TOutput">Class output to reponses</typeparam>
        /// <param name="httpClient">New HttpClient or HttpClientFactory.Create()</param>
        /// <param name="url">Url to Call</param>
        /// <returns>OutPut class response</returns>
        public static async Task<TOutput> GetAsync<TOutput>(HttpClient httpClient, string url) where TOutput : class
        {
            return await httpClient.GetApiAsync<TOutput>(url, MediaTypes.Json);
        }

        /// <summary>
        /// Execute GetAsync with Headers parameters
        /// </summary>
        /// <typeparam name="TOutput">Class output to reponses</typeparam>
        /// <param name="httpClient">New HttpClient or HttpClientFactory.Create()</param>
        /// <param name="url">Url to Call</param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static async Task<TOutput> GetAsync<TOutput>(HttpClient httpClient, string url, IDictionary<string, string> headers) where TOutput : class
        {
            httpClient.AddHeaders(headers);

            return await httpClient.GetApiAsync<TOutput>(url, MediaTypes.Json);
        }

        public static async Task<TOutput> GetAsync<TOutput>(HttpClient httpClient, string url, string token, string tokenType = TokenTypes.Bearer) where TOutput : class
        {
            return await httpClient.GetApiAsync<TOutput>(url, token, tokenType, MediaTypes.Json);
        }

        public static async Task<TOutput> PostAsync<TOutput, TInput>(HttpClient httpClient, string url, TInput input, [Optional] string token, string tokenType = TokenTypes.Bearer, string mediaType = MediaTypes.Json) where TOutput : class
        {
            return await httpClient.PostApiAsync<TOutput>(url, input, mediaType, token, tokenType);
        }

        public static async Task<TOutput> PostApiAsync<TOutput>(HttpClient httpClient, string url, [Optional] string token, string tokenType = TokenTypes.Bearer, string mediaType = MediaTypes.Json) where TOutput : class
        {
            return await httpClient.PostApiAsync<TOutput>(url, mediaType, token, tokenType);
        }
        public static async Task<TOutput> PutAsync<TOutput, TInput>(HttpClient httpClient, string url, TInput input, [Optional] string token, string tokenType = TokenTypes.Bearer, string mediaType = MediaTypes.Json) where TOutput : class
        {
            return await httpClient.PutApiAsync<TOutput>(url, input, mediaType, token, tokenType);
        }

        public static async Task<TOutput> PutApiAsync<TOutput>(HttpClient httpClient, string url, [Optional] string token, string tokenType = TokenTypes.Bearer, string mediaType = MediaTypes.Json) where TOutput : class
        {
            return await httpClient.PutApiAsync<TOutput>(url, mediaType, token, tokenType);
        }
        #region Privates
        /// <summary>
        /// Using Class to create queryStrings
        /// </summary>
        /// <param name="obj">Class with properties and values to build querystring</param>
        /// <returns>String querystring</returns>
        public static string BuildQueryString(object obj)
        {
            var QueryString = new StringBuilder();
            var FormattedValue = string.Empty;

            PropertyInfo[] propsProperties = obj.GetType().GetProperties();
            string[] properties = propsProperties.Select(p => p.Name).ToArray();

            foreach (var property in properties)
            {
                var propertyValue = obj.GetType().GetProperty(property).GetValue(obj, null);
                var propertyLower = char.ToLower(property[0]) + property.Substring(1);

                FormattedValue = $"{propertyLower}={propertyValue}&";
                QueryString.Append(FormattedValue);
            }

            return QueryString.ToString();
        }

        /// <summary>
        /// Add Header Parameters to ClientHttp
        /// </summary>
        public static HttpClient AddHeaders(this HttpClient client, IDictionary<string, string> headersValues)
        {
            if (headersValues != null)
            {
                foreach (var item in headersValues)
                {
                    client.DefaultRequestHeaders.Add(item.Key, item.Value);
                }
            }
            return client;
        }
        #endregion
    }
}
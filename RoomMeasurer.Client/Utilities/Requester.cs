namespace RoomMeasurer.Client.Utilities
{
    using System;
    using System.Threading.Tasks;
    using Windows.Web.Http;
    using Windows.Web.Http.Headers;

    public class Requester
    {
        private const string BaseUrl = "https://roommeasurer.herokuapp.com";

        public async Task<string> GetJsonAsync(string url, string token = null)
        {
            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders
                                .Accept
                                .Add(new HttpMediaTypeWithQualityHeaderValue("application/json"));

            if (token != null)
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            }

            HttpResponseMessage response = await client.GetAsync(new Uri(BaseUrl + url));
            string result = await response.Content.ReadAsStringAsync();

            return result;
        }

        public async Task<string> PostJsonAsync(string url, IHttpContent content, string token = null)
        {
            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders
                                .Accept
                                .Add(new HttpMediaTypeWithQualityHeaderValue("application/json"));

            if (token != null)
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            }

            HttpResponseMessage response = await client.PostAsync(new Uri(BaseUrl + url), content);
            string result = await response.Content.ReadAsStringAsync();

            return result;
        }

        public async Task<string> PutJsonAsync(string url, IHttpContent content, string token = null)
        {
            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders
                                .Accept
                                .Add(new HttpMediaTypeWithQualityHeaderValue("application/json"));

            if (token != null)
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            }

            HttpResponseMessage response = await client.PutAsync(new Uri(BaseUrl + url), content);
            string result = await response.Content.ReadAsStringAsync();

            return result;
        }
    }
}

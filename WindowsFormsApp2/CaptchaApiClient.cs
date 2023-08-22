using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp2
{
    internal class CaptchaApiClient
    {
        private static readonly HttpClient client = new HttpClient();

        public static async Task<string> CreateCaptchaJob(string apiKey, string imageBase64, string typeJobId)
        {
            string url = "https://omocaptcha.com/api/createJob";
            string requestData = $"{{\"api_token\": \"{apiKey}\", \"data\": {{\"type_job_id\": \"{typeJobId}\", \"image_base64\": \"{imageBase64}\"}}}}";
            StringContent content = new StringContent(requestData, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
        }

        public static async Task<string> GetCaptchaJobResult(string apiKey, int jobId)
        {
            string url = "https://omocaptcha.com/api/getJobResult";
            string requestData = $"{{\"api_token\": \"{apiKey}\", \"job_id\": {jobId}}}";
            StringContent content = new StringContent(requestData, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
        }
    }
}

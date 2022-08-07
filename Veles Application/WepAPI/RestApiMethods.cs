using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Veles_Application.WepAPI
{
    public class RestApiMethods
    {
        private static string baseApiUrl = Properties.Settings.Default.ApiBaseUrl;

        //GET
        public static Task<HttpResponseMessage> GetCall(string url)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                string apiUrl = baseApiUrl + url;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.Timeout = TimeSpan.FromSeconds(900);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var response = client.GetAsync(apiUrl);
                    response.Wait();
                    return response;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //POST
        public static Task<HttpResponseMessage> PostCall<T>(string url, T model) where T : class
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                string apiUrl = baseApiUrl + url;

                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.Timeout = TimeSpan.FromSeconds(900);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var response = client.PostAsJsonAsync(apiUrl, model);
                    response.Wait();
                    return response;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //GET
        public static Task<HttpResponseMessage> GetCallAuthorization(string url)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                string apiUrl = baseApiUrl + url;
                using (HttpClient client = new HttpClient())
                {
                    string authorization = Properties.Settings.Default.Token;
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);
                    client.BaseAddress = new Uri(apiUrl);
                    client.Timeout = TimeSpan.FromSeconds(900);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var response = client.GetAsync(apiUrl);
                    response.Wait();
                    return response;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static Task<HttpResponseMessage> PostCallAutorization<T>(string url, T model) where T : class
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                string apiUrl = baseApiUrl + url;

                using (HttpClient client = new HttpClient())
                {
                    string authorization = Properties.Settings.Default.Token;
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);
                    client.BaseAddress = new Uri(apiUrl);
                    client.Timeout = TimeSpan.FromSeconds(900);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var response = client.PostAsJsonAsync(apiUrl, model);
                    response.Wait();
                    return response;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static Task<HttpResponseMessage> PutCallAutorization<T>(string url, T model) where T : class
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                string apiUrl = baseApiUrl + url;

                using (HttpClient client = new HttpClient())
                {
                    string authorization = Properties.Settings.Default.Token;
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);
                    client.BaseAddress = new Uri(apiUrl);
                    client.Timeout = TimeSpan.FromSeconds(900);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var response = client.PutAsJsonAsync(apiUrl, model);
                    response.Wait();
                    return response;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static Task<HttpResponseMessage> DeleteCallAuthorization(string url)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                string apiUrl = baseApiUrl + url;
                using (HttpClient client = new HttpClient())
                {
                    string authorization = Properties.Settings.Default.Token;
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);
                    client.BaseAddress = new Uri(apiUrl);
                    client.Timeout = TimeSpan.FromSeconds(900);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var response = client.DeleteAsync(apiUrl);
                    response.Wait();
                    return response;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}

using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Net.Http.Headers;
using ThirdPartyIntegrationApp.Services;

namespace ThirdPartyIntegrationApp.Interfaces
{
    public abstract class ApiCallerService : IApiCallerService
    {
        protected readonly HttpClient _httpClient;
        private readonly JsonSerializerSettings _jsonSerializerSettings = null;

        public ApiCallerService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public ApiCallerService(HttpClient httpClient,
            JsonSerializerSettings jsonSerializerSettings)
        {
            _httpClient = httpClient;
            _jsonSerializerSettings = jsonSerializerSettings;
        }

        public async Task<T> GetAsync<T>(string uri, AuthenticationHeaderValue authenticationHeader)
        {
            ConfigureRequestHeaders(uri, authenticationHeader);

            var response = await _httpClient.GetAsync(uri);

            return await GetDataAsync<T>(response);
        }

        public async Task<HttpResponseMessage> GetWithHeaderTokenAsync(string uri, AuthenticationHeaderValue authenticationHeader)
        {
            ConfigureRequestHeaders(uri, authenticationHeader);

            return await _httpClient.GetAsync(uri);
        }

        public async Task<R> GetAsync<T, R>(string uri, AuthenticationHeaderValue authenticationHeader, T obj)
        {
            ConfigureRequestHeaders(uri, authenticationHeader);

            var response = await _httpClient.GetAsync(BuildQueryString(uri, obj));

            return await GetDataAsync<R>(response);
        }

        public async Task<T> GetAsync<T>(string uri, IEnumerable<KeyValuePair<string, string>> customHeaders)
        {
            ConfigureRequestHeaders(uri, customHeaders);

            var response = await _httpClient.GetAsync(uri);

            return await GetDataAsync<T>(response);
        }

        public async Task<R> GetAsync<T, R>(string uri, IEnumerable<KeyValuePair<string, string>> customHeaders, T obj)
        {
            ConfigureRequestHeaders(uri, customHeaders);

            var response = await _httpClient.GetAsync(BuildQueryString(uri, obj));

            return await GetDataAsync<R>(response);
        }

        public async Task<T> GetWithDefaultCredentialsAsync<T>(string uri)
        {
            var response = await _httpClient.GetAsync(uri);

            return await GetDataAsync<T>(response);
        }

        public async Task PostAsync<T>(string uri, AuthenticationHeaderValue authenticationHeader, T obj)
        {
            var content = new StringContent(JsonConvert.SerializeObject(obj, _jsonSerializerSettings));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            ConfigureRequestHeaders(uri, authenticationHeader);

            var response = await _httpClient.PostAsync(uri, content);

            await ValidateResponseAsync(response);
        }

        public async Task<R> PostAsync<T, R>(string uri, AuthenticationHeaderValue authenticationHeader, T obj)
        {
            var content = new StringContent(JsonConvert.SerializeObject(obj, _jsonSerializerSettings));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            ConfigureRequestHeaders(uri, authenticationHeader);

            var response = await _httpClient.PostAsync(uri, content);

            return await GetDataAsync<R>(response);
        }

        public async Task<HttpResponseMessage> PostWithHeaderTokenAsync<T>(string uri, AuthenticationHeaderValue authenticationHeader, T obj)
        {
            var content = new StringContent(JsonConvert.SerializeObject(obj, _jsonSerializerSettings));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            ConfigureRequestHeaders(uri, authenticationHeader);

            return await _httpClient.PostAsync(uri, content);
        }

        public async Task<R> PostAsync<T, R>(string uri, IEnumerable<KeyValuePair<string, string>> customHeaders, T obj)
        {
            var content = new StringContent(JsonConvert.SerializeObject(obj, _jsonSerializerSettings));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            ConfigureRequestHeaders(uri, customHeaders);

            var response = await _httpClient.PostAsync(uri, content);

            return await GetDataAsync<R>(response);
        }

        public async Task<R> PostFormAsync<T, R>(string uri, AuthenticationHeaderValue authenticationHeader, T obj)
        {
            ConfigureRequestHeaders(uri, authenticationHeader);

            var response = await _httpClient.PostAsync(uri, ToFormData(obj));

            return await GetDataAsync<R>(response);
        }

        public async Task PostWithDefaultCredentialsAsync<T>(string uri, T obj)
        {
            var response = await RetrievePostWithDefaultCredentialsResponseAsync(uri, obj);

            await ValidateResponseAsync(response);
        }

        public async Task<R> PostWithDefaultCredentialsAsync<T, R>(string uri, T obj)
        {
            var response = await RetrievePostWithDefaultCredentialsResponseAsync(uri, obj);

            return await GetDataAsync<R>(response);
        }

        public async Task<HttpResponseMessage> PostWithDefaultResponseAsync<T>(string uri, T obj)
        {
            var content = new StringContent(JsonConvert.SerializeObject(obj, _jsonSerializerSettings));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return await _httpClient.PostAsync(uri, content);
        }

        public async Task<T> PatchWithDefaultCredentialsAsync<T>(string uri)
        {
            var response = await RetrievePatchWithDefaultCredentialsResponseAsync(uri);

            return await GetDataAsync<T>(response);
        }

        public async Task PostFileAsync(string uri, AuthenticationHeaderValue authenticationHeader, IFormFile file)
        {
            var content = CreateFileContent(file);

            ConfigureRequestHeaders(uri, authenticationHeader);

            var response = await _httpClient.PostAsync(uri, content);

            await ValidateResponseAsync(response);
        }

        public async Task<T> PostFileAsync<T>(string uri, AuthenticationHeaderValue authenticationHeader, IFormFile file)
        {
            var content = CreateFileContent(file);

            ConfigureRequestHeaders(uri, authenticationHeader);

            var response = await _httpClient.PostAsync(uri, content);

            return await GetDataAsync<T>(response);
        }

        public async Task PostFileAsync<T>(string uri, AuthenticationHeaderValue authenticationHeader, IFormFile file, T obj)
        {
            var content = CreateFileContent(file, obj);

            ConfigureRequestHeaders(uri, authenticationHeader);

            var response = await _httpClient.PostAsync(uri, content);

            await ValidateResponseAsync(response);
        }

        public async Task<T> PostFileAsync<T>(string uri, IEnumerable<KeyValuePair<string, string>> customHeaders, IFormFile file, IEnumerable<KeyValuePair<string, string>> formFields, string mimeType)
        {
            var content = CreateFileContent(file, formFields);

            SetMimeType(content, mimeType);

            ConfigureRequestHeaders(uri, customHeaders);

            var response = await _httpClient.PostAsync(uri, content);

            return await GetDataAsync<T>(response);
        }

        public async Task<T> PostFileAsync<T>(string uri, IEnumerable<KeyValuePair<string, string>> customHeaders, IFormFile file, IEnumerable<KeyValuePair<string, string>> formFields, string mimeType, string fileParameterName)
        {
            var content = CreateFileContent(file, formFields, fileParameterName);

            SetMimeType(content, mimeType);

            ConfigureRequestHeaders(uri, customHeaders);

            var response = await _httpClient.PostAsync(uri, content);

            return await GetDataAsync<T>(response);
        }

        public async Task<R> PostFileAsync<T, R>(string uri, AuthenticationHeaderValue authenticationHeader, IFormFile file, T obj)
        {
            var content = CreateFileContent(file, obj);

            ConfigureRequestHeaders(uri, authenticationHeader);

            var response = await _httpClient.PostAsync(uri, content);

            return await GetDataAsync<R>(response);
        }

        public async Task PostWithQueryStringAsync(string uri, AuthenticationHeaderValue authenticationHeader)
        {
            ConfigureRequestHeaders(uri, authenticationHeader);

            var response = await _httpClient.PostAsync(uri, null);

            await ValidateResponseAsync(response);
        }

        public async Task<R> PostWithQueryStringAsync<R>(string uri, AuthenticationHeaderValue authenticationHeader)
        {
            ConfigureRequestHeaders(uri, authenticationHeader);

            var response = await _httpClient.PostAsync(uri, null);

            return await GetDataAsync<R>(response);
        }

        public async Task<HttpResponseMessage> PostWithQueryStringWithHeaderTokenAsync(string uri, AuthenticationHeaderValue authenticationHeader)
        {
            ConfigureRequestHeaders(uri, authenticationHeader);

            return await _httpClient.PostAsync(uri, null);
        }

        public async Task PutAsync<T>(string uri, AuthenticationHeaderValue authenticationHeader, T obj)
        {
            var content = new StringContent(JsonConvert.SerializeObject(obj, _jsonSerializerSettings));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            ConfigureRequestHeaders(uri, authenticationHeader);

            var response = await _httpClient.PutAsync(uri, content);

            await ValidateResponseAsync(response);
        }

        public async Task<R> PutAsync<T, R>(string uri, AuthenticationHeaderValue authenticationHeader, T obj)
        {
            var content = new StringContent(JsonConvert.SerializeObject(obj, _jsonSerializerSettings));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            ConfigureRequestHeaders(uri, authenticationHeader);

            var response = await _httpClient.PutAsync(uri, content);

            return await GetDataAsync<R>(response);
        }

        public async Task<HttpResponseMessage> PutWithHeaderAsync<T>(string uri, AuthenticationHeaderValue authenticationHeader, T obj)
        {
            var content = new StringContent(JsonConvert.SerializeObject(obj, _jsonSerializerSettings));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            ConfigureRequestHeaders(uri, authenticationHeader);

            return await _httpClient.PutAsync(uri, content);
        }

        public async Task<R> PutAsync<T, R>(string uri, IEnumerable<KeyValuePair<string, string>> customHeaders, T obj)
        {
            var content = new StringContent(JsonConvert.SerializeObject(obj, _jsonSerializerSettings));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            ConfigureRequestHeaders(uri, customHeaders);

            var response = await _httpClient.PutAsync(uri, content);

            return await GetDataAsync<R>(response);
        }

        public async Task PutWithDefaultCredentialsAsync<T>(string uri, T obj)
        {
            var content = new StringContent(JsonConvert.SerializeObject(obj, _jsonSerializerSettings));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await _httpClient.PutAsync(uri, content);

            await ValidateResponseAsync(response);
        }

        public async Task<R> PutWithDefaultCredentialsAsync<T, R>(string uri, T obj)
        {
            var content = new StringContent(JsonConvert.SerializeObject(obj, _jsonSerializerSettings));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await _httpClient.PutAsync(uri, content);

            return await GetDataAsync<R>(response);
        }

        public async Task<R> PutWithQueryStringAndDefaultCredentialsAsync<R>(string uri)
        {
            var response = await _httpClient.PutAsync(uri, null);

            return await GetDataAsync<R>(response);
        }

        public async Task PutWithQueryStringAsync(string uri, AuthenticationHeaderValue authenticationHeader)
        {
            ConfigureRequestHeaders(uri, authenticationHeader);

            var response = await _httpClient.PutAsync(uri, null);

            await ValidateResponseAsync(response);
        }

        public async Task<R> PutWithQueryStringAsync<R>(string uri, AuthenticationHeaderValue authenticationHeader)
        {
            ConfigureRequestHeaders(uri, authenticationHeader);

            var response = await _httpClient.PutAsync(uri, null);

            return await GetDataAsync<R>(response);
        }

        public async Task PutWithQueryStringAndDefaultCredentialsAsync(string uri)
        {
            var response = await _httpClient.PutAsync(uri, null);

            await ValidateResponseAsync(response);
        }

        public async Task PostWithQueryStringAndDefaultCredentialsAsync(string uri)
        {
            var response = await _httpClient.PostAsync(uri, null);

            await ValidateResponseAsync(response);
        }

        public async Task DeleteAsync(string uri, AuthenticationHeaderValue authenticationHeader)
        {
            ConfigureRequestHeaders(uri, authenticationHeader);

            var response = await _httpClient.DeleteAsync(uri);

            await ValidateResponseAsync(response);
        }

        public async Task DeleteWithDefaultCredentialsAsync(string uri)
        {
            var response = await _httpClient.DeleteAsync(uri);

            await ValidateResponseAsync(response);
        }

        public async Task<R> DeleteWithDefaultCredentialsAsync<R>(string uri)
        {
            var response = await _httpClient.DeleteAsync(uri);

            return await GetDataAsync<R>(response);
        }

        public async Task<byte[]> DownloadFileAsync(string uri, AuthenticationHeaderValue authenticationHeader)
        {
            ConfigureRequestHeaders(uri, authenticationHeader);

            var response = await _httpClient.GetAsync(uri);

            return await GetFileAsync(response);
        }

        public async Task<byte[]> DownloadFileAsync(string uri, IEnumerable<KeyValuePair<string, string>> customHeaders)
        {
            ConfigureRequestHeaders(uri, customHeaders);

            var response = await _httpClient.GetAsync(uri);

            return await GetFileAsync(response);
        }

        protected Task<HttpResponseMessage> RetrieveGetWithDefaultCredentialsResponseAsync(string uri) => _httpClient.GetAsync(uri);

        protected Task<HttpResponseMessage> RetrievePostWithDefaultCredentialsResponseAsync<T>(string uri, T obj)
        {
            var content = new StringContent(JsonConvert.SerializeObject(obj, _jsonSerializerSettings));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return _httpClient.PostAsync(uri, content);
        }

        protected Task<HttpResponseMessage> RetrievePostContentWithDefaultCredentialsResponseAsync(string uri, HttpContent content)
        {
            return _httpClient.PostAsync(uri, content);
        }

        protected Task<HttpResponseMessage> RetrievePutWithDefaultCredentialsResponseAsync<T>(string uri, T obj)
        {
            var content = new StringContent(JsonConvert.SerializeObject(obj, _jsonSerializerSettings));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return _httpClient.PutAsync(uri, content);
        }

        protected Task<HttpResponseMessage> RetrievePatchWithDefaultCredentialsResponseAsync(string uri)
        {
            var httpRequestMessage = new HttpRequestMessage(new HttpMethod("PATCH"), uri);

            return _httpClient.SendAsync(httpRequestMessage);
        }

        protected virtual async Task ValidateResponseAsync(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                await HandleFailureResponseAsync(response);
            }
        }

        protected async Task HandleFailureResponseAsync(HttpResponseMessage response)
        {
            var errorText = await GetErrorResponseBody(response);
            throw new Exception($"Server error (HTTP {response.StatusCode}). Body: {errorText}");
        }
        private void SetMimeType(MultipartFormDataContent content, string mimeType)
        {
            if (!string.IsNullOrWhiteSpace(mimeType))
            {
                content.First().Headers.ContentType = new MediaTypeHeaderValue(mimeType);
            }
        }

        private string BuildQueryString<T>(string uri, T obj)
        {
            if (uri.IndexOf("?") >= 0)
            {
                return $"{uri}&{obj.ToQueryString()}";
            }
            else
            {
                return $"{uri}?{obj.ToQueryString()}";
            }
        }

        private void ConfigureRequestHeaders(string uri, AuthenticationHeaderValue authenticationHeader)
        {
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            if (authenticationHeader != null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = authenticationHeader;
            }
        }

        private void ConfigureRequestHeaders(string uri, IEnumerable<KeyValuePair<string, string>> customHeaders)
        {
            if (customHeaders != null)
            {
                foreach (KeyValuePair<string, string> header in customHeaders)
                {
                    ReplaceOrAddHttpHeader(header);
                }
            }
        }

        private void ReplaceOrAddHttpHeader(KeyValuePair<string, string> header)
        {
            // HTTP Client may be a previous instance from the factory so replace the header if needs be
            if (_httpClient.DefaultRequestHeaders.Contains(header.Key))
            {
                _httpClient.DefaultRequestHeaders.Remove(header.Key);
            }

            _httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
        }

        private MultipartFormDataContent CreateFileContent(IFormFile file, string fileParameterName = "file")
        {
            byte[] data;
            using (var br = new BinaryReader(file.OpenReadStream()))
            {
                data = br.ReadBytes((int)file.OpenReadStream().Length);
            }

            var content = new MultipartFormDataContent
        {
            { new ByteArrayContent(data), fileParameterName, file.FileName }
        };

            return content;
        }

        private MultipartFormDataContent CreateFileContent<T>(IFormFile file, T obj)
        {
            var content = CreateFileContent(file);
            if (obj != null)
            {
                content.Add(new StringContent(JsonConvert.SerializeObject(obj, _jsonSerializerSettings)), "data");
            }

            return content;
        }

        private MultipartFormDataContent CreateFileContent(IFormFile file, IEnumerable<KeyValuePair<string, string>> formFields)
        {
            var content = CreateFileContent(file);
            AddMultiPartFormFields(formFields, content);

            return content;
        }

        private MultipartFormDataContent CreateFileContent(IFormFile file, IEnumerable<KeyValuePair<string, string>> formFields, string fileParameterName)
        {
            var content = CreateFileContent(file, fileParameterName);
            AddMultiPartFormFields(formFields, content);

            return content;
        }

        private static void AddMultiPartFormFields(IEnumerable<KeyValuePair<string, string>> formFields, MultipartFormDataContent content)
        {
            if (formFields != null)
            {
                foreach (var formField in formFields)
                {
                    content.Add(new StringContent(formField.Value), formField.Key);
                }
            }
        }

        protected async Task<T> GetDataAsync<T>(HttpResponseMessage response)
        {
            await ValidateResponseAsync(response);
            var data = await response.Content.ReadAsStringAsync();

            if (typeof(T) == typeof(string))
                return DeserializeString<T>(data);

            return JsonConvert.DeserializeObject<T>(data, _jsonSerializerSettings);
        }

        private T DeserializeString<T>(string data)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(data, _jsonSerializerSettings);
            }
            catch
            {
                return (T)Convert.ChangeType(data, typeof(T));
            }
        }

        private async Task<byte[]> GetFileAsync(HttpResponseMessage response)
        {
            await ValidateResponseAsync(response);

            byte[] file = null;

            using (var memoryStream = new MemoryStream())
            {
                (await response.Content.ReadAsStreamAsync()).CopyTo(memoryStream);
                file = memoryStream.ToArray();
            }

            return file;
        }

        private Task<string> GetErrorResponseBody(HttpResponseMessage response)
        {
            try
            {
                return response.Content.ReadAsStringAsync();
            }
            catch { }

            return Task.FromResult<string>(null);
        }

        private FormUrlEncodedContent ToFormData(object obj)
        {
            var formData = ToKeyValue(obj);

            return new FormUrlEncodedContent(formData);
        }

        private IDictionary<string, string> ToKeyValue(object metaToken)
        {
            if (metaToken == null)
            {
                return null;
            }

            var serializer = new Newtonsoft.Json.JsonSerializer { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            var token = metaToken as JToken;
            if (token == null)
            {
                return ToKeyValue(JObject.FromObject(metaToken, serializer));
            }

            if (token.HasValues)
            {
                var contentData = new Dictionary<string, string>();
                foreach (var child in token.Children().ToList())
                {
                    var childContent = ToKeyValue(child);
                    if (childContent != null)
                    {
                        contentData = contentData.Concat(childContent)
                                                    .ToDictionary(k => k.Key, v => v.Value);
                    }
                }

                return contentData;
            }

            var jValue = token as JValue;
            if (jValue?.Value == null)
            {
                return null;
            }

            var value = jValue?.Type == JTokenType.Date ?
                            jValue?.ToString("o", CultureInfo.InvariantCulture) :
                            jValue?.ToString(CultureInfo.InvariantCulture);

            return new Dictionary<string, string> { { token.Path, value } };
        }
           
    }

}
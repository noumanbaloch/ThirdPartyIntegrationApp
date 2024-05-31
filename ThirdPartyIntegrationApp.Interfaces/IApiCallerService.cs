using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;

namespace ThirdPartyIntegrationApp.Interfaces
{
    public interface IApiCallerService
    {
        Task<T> GetAsync<T>(string uri, AuthenticationHeaderValue authenticationHeader);

        Task<R> GetAsync<T, R>(string uri, AuthenticationHeaderValue authenticationHeader, T obj);

        Task<T> GetAsync<T>(string uri, IEnumerable<KeyValuePair<string, string>> customHeaders);

        Task<R> GetAsync<T, R>(string uri, IEnumerable<KeyValuePair<string, string>> customHeaders, T obj);

        Task<T> GetWithDefaultCredentialsAsync<T>(string uri);

        Task PostAsync<T>(string uri, AuthenticationHeaderValue authenticationHeader, T obj);

        Task<R> PostAsync<T, R>(string uri, AuthenticationHeaderValue authenticationHeader, T obj);

        Task PostWithDefaultCredentialsAsync<T>(string uri, T obj);

        Task<R> PostWithDefaultCredentialsAsync<T, R>(string uri, T obj);

        Task PostFileAsync(string uri, AuthenticationHeaderValue authenticationHeader, IFormFile file);

        Task<T> PostFileAsync<T>(string uri, AuthenticationHeaderValue authenticationHeader, IFormFile file);

        Task PostFileAsync<T>(string uri, AuthenticationHeaderValue authenticationHeader, IFormFile file, T obj);

        Task<T> PostFileAsync<T>(string uri, IEnumerable<KeyValuePair<string, string>> customHeaders, IFormFile file, IEnumerable<KeyValuePair<string, string>> formFields, string mimeType);

        Task<R> PostFileAsync<T, R>(string uri, AuthenticationHeaderValue authenticationHeader, IFormFile file, T obj);

        Task PostWithQueryStringAsync(string uri, AuthenticationHeaderValue authenticationHeader);

        Task<R> PostWithQueryStringAsync<R>(string uri, AuthenticationHeaderValue authenticationHeader);

        Task PutAsync<T>(string uri, AuthenticationHeaderValue authenticationHeader, T obj);

        Task<R> PutAsync<T, R>(string uri, AuthenticationHeaderValue authenticationHeader, T obj);

        Task PutWithDefaultCredentialsAsync<T>(string uri, T obj);

        Task<R> PutWithDefaultCredentialsAsync<T, R>(string uri, T obj);

        Task PutWithQueryStringAsync(string uri, AuthenticationHeaderValue authenticationHeader);

        Task<R> PutWithQueryStringAsync<R>(string uri, AuthenticationHeaderValue authenticationHeader);

        Task PutWithQueryStringAndDefaultCredentialsAsync(string uri);

        Task<R> PutWithQueryStringAndDefaultCredentialsAsync<R>(string uri);

        Task PostWithQueryStringAndDefaultCredentialsAsync(string uri);

        Task DeleteAsync(string uri, AuthenticationHeaderValue authenticationHeader);

        Task DeleteWithDefaultCredentialsAsync(string uri);

        Task<R> DeleteWithDefaultCredentialsAsync<R>(string uri);

        Task<byte[]> DownloadFileAsync(string uri, AuthenticationHeaderValue authenticationHeader);
    }
}

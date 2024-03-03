using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xamarin.Essentials;

using Frontend.dto;
using Frontend.holder;

namespace Frontend
{
    public class RestClient
    {
        private const string BASE_URL = "http://95.66.154.77/api";
        private readonly HttpClient client;

        public RestClient()
        {
            HttpClientHandler clientHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
            };
            client = new HttpClient(clientHandler);
        }
        JsonSerializerOptions options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };

        public async Task<Response> GenerateResponse(GenerateImageRequest request)
        {
            string url = BASE_URL + "/generation";
            HttpRequestMessage serverRequest = new HttpRequestMessage(HttpMethod.Post, url);
            serverRequest.Headers.Add("Authorization", "Bearer " + UserTokenHolder.Token);
            StringContent body = ToJson(request);
            serverRequest.Content = body;
            HttpResponseMessage response = await client.SendAsync(serverRequest);
            string content = await response.Content.ReadAsStringAsync();
            Response decodedResponse = new Response();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                decodedResponse.Data = JsonSerializer.Deserialize<GenerateImageResponse>(content, options);
            }

            return decodedResponse;
        }

        public async Task<Response> RateImg(RateImgDto request)
        {
            string url = BASE_URL + "/generation/RateImg";
            HttpRequestMessage serverRequest = new HttpRequestMessage(HttpMethod.Post, url);
            serverRequest.Headers.Add("Authorization", "Bearer " + UserTokenHolder.Token);
            StringContent body = ToJson(request);
            serverRequest.Content = body;
            HttpResponseMessage response = await client.SendAsync(serverRequest);
            Response decoded = new Response()
            {
                Code = response.StatusCode
            };
            return decoded;
        }

        public async Task<Response> GetMyGenerationHistory()
        {
            string url = BASE_URL + "/generation";
            HttpRequestMessage serverRequest = new HttpRequestMessage(HttpMethod.Get, url);
            serverRequest.Headers.Add("Authorization", "Bearer " + UserTokenHolder.Token);
            HttpResponseMessage response = await client.SendAsync(serverRequest);
            string content = await response.Content.ReadAsStringAsync();
            Response decodedResponse = new Response();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                decodedResponse.Data = JsonSerializer.Deserialize<List<GenerationDto>>(content, options);
            }

            return decodedResponse;
        }

        public async Task<Response> GetPublicList()
        {
            string url = BASE_URL + "/generation/AllPublicView";
            HttpRequestMessage serverRequest = new HttpRequestMessage(HttpMethod.Get, url);
            serverRequest.Headers.Add("Authorization", "Bearer " + UserTokenHolder.Token);
            HttpResponseMessage response = await client.SendAsync(serverRequest);
            string content = await response.Content.ReadAsStringAsync();
            Response decodedResponse = new Response();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                decodedResponse.Data = JsonSerializer.Deserialize<List<GenerationDto>>(content, options);
            }

            return decodedResponse;
        }
        
        public async Task<Response> MakePublic(RateImgDto request)
        {
            string url = BASE_URL + "/generation/PublicView";
            HttpRequestMessage serverRequest = new HttpRequestMessage(HttpMethod.Post, url);
            serverRequest.Headers.Add("Authorization", "Bearer " + UserTokenHolder.Token);
            StringContent body = ToJson(request);
            serverRequest.Content = body;
            HttpResponseMessage response = await client.SendAsync(serverRequest);
            Response decoded = new Response()
            {
                Code = response.StatusCode
            };
            return decoded;
        }
        
        public async Task<Response> Auth(AuthRequest request)
        {
            string authUrl = BASE_URL + "/users/auth";
            StringContent body = ToJson(request);
            HttpResponseMessage response = await client.PostAsync(authUrl, body);
            string content = await response.Content.ReadAsStringAsync();
            Response decodedResponse = new Response();
            decodedResponse.Code = response.StatusCode;
            if (decodedResponse.Code == HttpStatusCode.OK)
            {
                decodedResponse.Data = JsonSerializer.Deserialize<AuthResponse>(content, options);
            }

            return decodedResponse;
        }

        public async Task<Response> Register(RegisterRequest request)
        {
            string authUrl = BASE_URL + "/users/register";
            StringContent body = ToJson(request);
            HttpResponseMessage response = await client.PostAsync(authUrl, body);
            string content = await response.Content.ReadAsStringAsync();
            Response decodedResponse = new Response();
            decodedResponse.Code = response.StatusCode;
            if (decodedResponse.Code == HttpStatusCode.OK)
            {
                decodedResponse.Data = JsonSerializer.Deserialize<UserDto>(content, options);
            }

            return decodedResponse;
        }

        public async Task<Response> Recognize(RecognizeRequest request)
        {
            string recognizeUrl = BASE_URL + "/images";
            HttpRequestMessage serverRequest = new HttpRequestMessage(HttpMethod.Post, recognizeUrl);
            serverRequest.Headers.Add("Authorization", "Bearer " + UserTokenHolder.Token);
            serverRequest.Content = ToJson(request);
            HttpResponseMessage serverResponse = await client.SendAsync(serverRequest);
            string content = await serverResponse.Content.ReadAsStringAsync();
            Response decodedResponse = new Response();
            decodedResponse.Code = serverResponse.StatusCode;
            if (decodedResponse.Code == HttpStatusCode.OK)
            {
                decodedResponse.Data = JsonSerializer.Deserialize<RecognitionResponse>(content, options);
            }

            return decodedResponse;
        }

        public async Task<Response> GetHistory()
        {
            string url = BASE_URL + "/history";
            HttpRequestMessage serverRequest = new HttpRequestMessage(HttpMethod.Get, url);
            serverRequest.Headers.Add("Authorization", "Bearer " + UserTokenHolder.Token);
            HttpResponseMessage serverResponse = await client.SendAsync(serverRequest);
            string content = await serverResponse.Content.ReadAsStringAsync();
            Response decodedResponse = new Response();
            decodedResponse.Code = serverResponse.StatusCode;
            if (decodedResponse.Code == HttpStatusCode.OK)
            {
                decodedResponse.Data = JsonSerializer.Deserialize<List<HistoryDto>>(content, options);
            }

            return decodedResponse;
        }

        public async Task<Response> GetUsers()
        {
            string url = BASE_URL + "/users/all";
            HttpRequestMessage serverRequest = new HttpRequestMessage(HttpMethod.Get, url);
            serverRequest.Headers.Add("Authorization", "Bearer " + UserTokenHolder.Token);
            HttpResponseMessage serverResponse = await client.SendAsync(serverRequest);
            string content = await serverResponse.Content.ReadAsStringAsync();
            Response decodedResponse = new Response();
            decodedResponse.Code = serverResponse.StatusCode;
            if (decodedResponse.Code == HttpStatusCode.OK)
            {
                decodedResponse.Data = JsonSerializer.Deserialize<List<AdminUserDto>>(content, options);
            }

            return decodedResponse;
        }

        public async Task<Response> ChangeStatus(ChangeStatusRequest request)
        {
            string url = BASE_URL + "/users";
            HttpRequestMessage serverRequest = new HttpRequestMessage(HttpMethod.Put, url);
            serverRequest.Content = ToJson(request);
            serverRequest.Headers.Add("Authorization", "Bearer " + UserTokenHolder.Token);
            HttpResponseMessage serverResponse = await client.SendAsync(serverRequest);
            Response decodedResponse = new Response();
            decodedResponse.Code = serverResponse.StatusCode;

            return decodedResponse;
        }
        
        private StringContent ToJson(object obj)
        {
            string json = JsonSerializer.Serialize(obj, options);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }
    }
}
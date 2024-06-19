using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using WebFont.Models;

namespace WebFont.Data
{
    public class DeleteDataBookAPI
    {
        private string DeleteBookInHistory = "https://localhost:44380/api/BookDeleteData/DeleteBookInHistory";
        private string DeleteBookInFavorite = "https://localhost:44380/api/BookDeleteData/DeleteBookInFavorite";
        private readonly HttpClient _httpClient;
        public DeleteDataBookAPI()
        {
            _httpClient = new HttpClient();
        }
        public async Task<bool> DeleteHistory(User_Book user_Book)
        {
            string json = JsonConvert.SerializeObject(user_Book);
            StringContent data = new StringContent(json, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri(DeleteBookInHistory),
                Content = data
            };

            HttpResponseMessage response = await _httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> DeleteFavorite(User_Book user_Book)
        {
            string json = JsonConvert.SerializeObject(user_Book);
            StringContent data = new StringContent(json, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri(DeleteBookInFavorite),
                Content = data
            };

            HttpResponseMessage response = await _httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
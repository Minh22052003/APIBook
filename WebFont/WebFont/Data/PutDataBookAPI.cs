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
    public class PutDataBookAPI
    {
        string URLPutUser = "https://localhost:44380/api/UserPutData/UpdateUser";
        string URLPutUserReview = "https://localhost:44380/api/BookPutData/UpdateReview";
        private readonly HttpClient _httpClient;
        public PutDataBookAPI()
        {
            _httpClient = new HttpClient();
        }
        public async Task<HttpResponseMessage> UpdateUserAsync(UserNotPass user)
        {
            string json = JsonConvert.SerializeObject(user);
            StringContent data = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PutAsync(URLPutUser, data);
            return response;
        }
        public async Task<HttpResponseMessage> UpdateUserReview(ReviewBookPost review)
        {
            string json = JsonConvert.SerializeObject(review);
            StringContent data = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PutAsync(URLPutUserReview, data);
            return response;
        }
    }
}

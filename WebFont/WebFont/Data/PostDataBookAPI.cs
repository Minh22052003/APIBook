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
    public class PostDataBookAPI
    {
        private string Post_HistoryRead = "https://localhost:44380/api/BookPostData/Create_HistoryRead";
        private string Post_FavoriteBook = "https://localhost:44380/api/BookPostData/Create_FavoriteRead";
        private string CreateAccUser = "https://localhost:44380/api/UserPostData/POST_CreateAccUser";
        private string URLLogin = "https://localhost:44380/api/UserPostData/Login";
        private readonly HttpClient _httpClient;
        public PostDataBookAPI()
        {
            _httpClient = new HttpClient();
        }

        //USER

        public async Task<HttpResponseMessage> Post_LoginUserAsync(LoginAcc loginuser)
        {
            try
            {
                string json = JsonConvert.SerializeObject(loginuser);
                StringContent data = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response1 = await _httpClient.PostAsync(URLLogin, data);

                return response1;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<bool> Post_CreateAccUser(LoginAcc login)
        {
            try
            {
                string json = JsonConvert.SerializeObject(login);
                StringContent data = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response1 = await _httpClient.PostAsync(CreateAccUser, data);

                return response1.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu cần
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        public async Task<bool> Post_HistoryReadDataAsync(User_Book user_Book)
        {
            try
            {
                string json = JsonConvert.SerializeObject(user_Book);
                StringContent data = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.PostAsync(Post_HistoryRead, data);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu cần
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<bool> Post_FavoriteDataAsync(User_Book user_Book)
        {
            try
            {
                string json = JsonConvert.SerializeObject(user_Book);
                StringContent data = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.PostAsync(Post_FavoriteBook, data);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu cần
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
    
}
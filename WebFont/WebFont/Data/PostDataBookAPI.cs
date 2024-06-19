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
        string Post_HistoryRead = "https://localhost:44380/api/BookPostData/Create_HistoryRead";
        private readonly HttpClient _httpClient;
        public PostDataBookAPI()
        {
            _httpClient = new HttpClient();
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
    }
    
}
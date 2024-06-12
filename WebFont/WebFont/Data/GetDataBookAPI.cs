using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebFont.Models;
using static WebFont.Controllers.AccountController;

namespace WebFont.Data
{
    public class GetDataBookAPI
    {
        string Get_AllBook = "https://localhost:44380/api/BookGetData/Get_AllBook";
        private readonly HttpClient _httpClient;
        public GetDataBookAPI()
        {
            _httpClient = new HttpClient();
        }
        public async Task<List<Book>> Get_AllBookAsync()
        {
            List<Book> books;
            HttpResponseMessage response = await _httpClient.GetAsync(Get_AllBook);
            string responsData = await response.Content.ReadAsStringAsync();
            books = JsonConvert.DeserializeObject<List<Book>>(responsData);
            return books;
        }
    }
}
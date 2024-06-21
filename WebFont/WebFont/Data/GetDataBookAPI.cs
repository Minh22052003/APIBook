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
        private hosting host;
        private string name = "";
        string Get_AllBook = "api/BookGetData/Get_AllBook";
        string Get_BookCategory = "api/BookGetData/Get_BookCategory";
        string URLGetUserName = "api/UserGetData/Get_UserName";
        private readonly HttpClient _httpClient;
        public GetDataBookAPI()
        {
            _httpClient = new HttpClient();
            host = new hosting();
            name = host.name;
        }


        //BOOK
        public async Task<List<Book>> Get_AllBookAsync()
        {
            List<Book> books;
            HttpResponseMessage response = await _httpClient.GetAsync(name+Get_AllBook);
            string responsData = await response.Content.ReadAsStringAsync();
            books = JsonConvert.DeserializeObject<List<Book>>(responsData);
            return books;
        }
        public async Task<List<UsernameResponse>> Get_AllUsernameAsync()
        {
            List<UsernameResponse> tmp;
            HttpResponseMessage response = await _httpClient.GetAsync(name+URLGetUserName);
            string responsData = await response.Content.ReadAsStringAsync();
            tmp = JsonConvert.DeserializeObject<List<UsernameResponse>>(responsData);

            return tmp;
        }
        public async Task<List<Book>> Get_BookbyIDAsync(int id)
        {
            List<Book> books;
            string Get_BookbyID = $"{name}api/BookGetData/Get_BookByID?id={id}";
            HttpResponseMessage response = await _httpClient.GetAsync(Get_BookbyID);
            string responsData = await response.Content.ReadAsStringAsync();
            books = JsonConvert.DeserializeObject<List<Book>>(responsData);
            return books;
        }
        public async Task<List<Book>> Get_BookbyCategoryAsync(string categoryname)
        {
            List<Book> books;
            string Get_BookbyID = $"{name}api/BookGetData/Get_BookByCategory?categoryName={categoryname}";
            HttpResponseMessage response = await _httpClient.GetAsync(Get_BookbyID);
            string responsData = await response.Content.ReadAsStringAsync();
            books = JsonConvert.DeserializeObject<List<Book>>(responsData);
            return books;
        }
        public async Task<List<Book>> Get_BookbyCategoryIDAsync(int categoryID)
        {
            List<Book> books;
            string Get_BookbyID = $"{name}api/BookGetData/Get_BookByCategoryID?categoryID={categoryID}";
            HttpResponseMessage response = await _httpClient.GetAsync(Get_BookbyID);
            string responsData = await response.Content.ReadAsStringAsync();
            books = JsonConvert.DeserializeObject<List<Book>>(responsData);
            return books;
        }
        public async Task<List<Category>> Get_BookCategoryAsync()
        {
            List<Category> categories;
            HttpResponseMessage response = await _httpClient.GetAsync(name + Get_BookCategory);
            string responsData = await response.Content.ReadAsStringAsync();
            categories = JsonConvert.DeserializeObject<List<Category>>(responsData);
            return categories;
        }

        public async Task<List<ReviewBook>> Get_ReviewByBook(int idbook)
        {
            List<ReviewBook> reviews;
            string get_ReviewByBook = $"{name}api/BookGetData/Get_ReviewByBook?idbook={idbook}";
            HttpResponseMessage response = await _httpClient.GetAsync(get_ReviewByBook);
            string responsData = await response.Content.ReadAsStringAsync();
            reviews = JsonConvert.DeserializeObject<List<ReviewBook>>(responsData) ;
            return reviews;
        }





        //USER
        public async Task<List<BookHistory>> Get_HistoryUser(int iduser)
        {
            List<BookHistory> book;
            string get_ReviewByBook = name + $"api/BookGetData/Get_BookByHistoryUser?iduser={iduser}";
            HttpResponseMessage response = await _httpClient.GetAsync(get_ReviewByBook);
            string responsData = await response.Content.ReadAsStringAsync();
            book = JsonConvert.DeserializeObject<List<BookHistory>>(responsData);
            return book;
        }
        public async Task<List<ReviewUser>> Get_ReviewUser(int iduser)
        {
            List<ReviewUser> reviews;
            string get_ReviewByBook = name + $"api/BookGetData/Get_ReviewByUser?iduser={iduser}";
            HttpResponseMessage response = await _httpClient.GetAsync(get_ReviewByBook);
            string responsData = await response.Content.ReadAsStringAsync();
            reviews = JsonConvert.DeserializeObject<List<ReviewUser>>(responsData);
            return reviews;
        }

        public async Task<List<Book>> Get_FavoriteUser(int iduser)
        {
            List<Book> book;
            string get_ReviewByBook = name + $"api/BookGetData/Get_BookByFavoriteUser?iduser={iduser}";
            HttpResponseMessage response = await _httpClient.GetAsync(get_ReviewByBook);
            string responsData = await response.Content.ReadAsStringAsync();
            book = JsonConvert.DeserializeObject<List<Book>>(responsData);
            return book;
        }
    }
}
using BookAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Razor.Parser.SyntaxTree;

namespace BookAPI.Controllers.BookData
{
    public class DatafromAPIController : Controller
    {
        // GET: DatafromAPI
        public ActionResult Index()
        {
            return View();
        }
        private readonly ModelBook _context;
        public DatafromAPIController(ModelBook context)
        {
            _context = context;
        }
        public DatafromAPIController()
        {
            _context = new ModelBook();
        }


        private async Task<BookdataFrAPI> SearchBooksAsync(string query, int maxResults)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("User-Agent", "C# Book Search");
                var url = $"https://www.googleapis.com/books/v1/volumes?q={query}&maxResults={maxResults}";
                var response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var bookDataString = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<BookdataFrAPI>(bookDataString);
                }
                else
                {
                    Console.WriteLine($"Lỗi tìm kiếm sách. Mã lỗi: {response.StatusCode}");
                    return null;
                }
            }
        }
        //Save data to SQL
        private async Task ProcessBookDataAsync(BookdataFrAPI bookData)
        {
            if (bookData.Items != null)
            {
                foreach (var item in bookData.Items)
                {
                    //khởi tạo đối tượng sách
                    var book = new Book
                    {
                        Title = item.VolumeInfo.Title,
                        DescriptionB = item.VolumeInfo.Description,
                        PublishedDate = item.VolumeInfo.PublishedDate,
                        BookLink = item.VolumeInfo.InfoLink,
                        CoverImage = item.VolumeInfo.ImageLinks?.Thumbnail
                    };
                    //Thêm tác giả
                    foreach (var author in item.VolumeInfo.Authors)
                    {
                        Author authortmp =  GetAuthorByName(author.AuthorName);
                        if (authortmp == null)
                        {
                            authortmp = new Author { AuthorName = author.AuthorName };
                            AddAuthor(authortmp);
                        }
                        book.Author = authortmp;
                    }

                    //Thêm nhà xuất bản
                    var publisher = GetPublisherByName(item.VolumeInfo.Publisher);
                    if (publisher == null)
                    {
                        publisher = new Publisher { PublisherName = item.VolumeInfo.Publisher };
                        AddPublisher(publisher);
                    }
                    book.Publisher = publisher;


                    AddBook(book);
                    Console.WriteLine($"Added book: {book.Title}");
                }
            }
        }

        // Phương thức thêm tác giả mới
        public void AddAuthor(Author author)
        {
            _context.Authors.Add(author);
             _context.SaveChangesAsync();
        }
        // Phương thức lấy tác giả theo tên
        public Author GetAuthorByName(string name)
        {
            return _context.Authors.FirstOrDefault(a => a.AuthorName == name);
        }

        public void AddPublisher(Publisher publisher)
        {
            _context.Publishers.Add(publisher);
            _context.SaveChanges();
        }

        // Phương thức lấy nhà xuất bản theo tên
        public Publisher GetPublisherByName(string name)
        {
            return  _context.Publishers.FirstOrDefault(p => p.PublisherName == name);
        }

        // Phương thức thêm sách mới
        public void AddBook(Book book)
        {
            _context.Books.Add(book);
            _context.SaveChangesAsync();
        }
        // Phương thức lấy thể loại theo tên
        public Category GetCategoryByName(string name)
        {
            return _context.Categories.FirstOrDefault(a => a.CategoryName == name);
        }

        public void AddCategory(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
        }

    }
}
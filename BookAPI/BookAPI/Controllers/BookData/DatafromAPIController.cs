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


        public async Task<BookdataFrAPI> SearchBooksAsync(int check,string query, int maxResults)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("User-Agent", "C# Book Search");
                var url = $"";
                if (check == 1)
                {
                    //Gọi data theo tên sách
                    url = $"https://www.googleapis.com/books/v1/volumes?q={query}&maxResults={maxResults}";
                }
                else if(check == 2)
                {
                    //Gọi data theo chủ đề
                    url = $"https://www.googleapis.com/books/v1/volumes?q=subject:{query}&maxResults={maxResults}";
                }
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
        public async Task ProcessBookDataAsync(BookdataFrAPI bookData1)
        {
            BookdataFrAPI bookData = bookData1;
            if (bookData.Items != null)
            {
                foreach (var item in bookData.Items)
                {
                    // Khởi tạo đối tượng sách
                    var book = new Book
                    {
                        Title = item.VolumeInfo.Title,
                    };
                    if (item.VolumeInfo.Description==null)
                    {
                        book.DescriptionB = "Không có mô tả";
                    }
                    else
                    {
                        book.DescriptionB = item.VolumeInfo.Description;

                    }
                    if (item.VolumeInfo.PublishedDate == null)
                    {
                        book.PublishedDate = "Không có ngày xuất bản";
                    }
                    else
                    {
                        book.PublishedDate = item.VolumeInfo.PublishedDate;
                    }
                    if (item.VolumeInfo.InfoLink == null)
                    {
                        book.BookLink = "https://upload.wikimedia.org/wikipedia/commons/thumb/1/10/Logo_UTC.png/896px-Logo_UTC.png?20131104090648";
                    }
                    else
                    {
                        book.BookLink = item.VolumeInfo.InfoLink;
                    }
                    if (item.VolumeInfo.ImageLinks?.Thumbnail == null)
                    {
                        book.CoverImage = "https://upload.wikimedia.org/wikipedia/commons/thumb/1/10/Logo_UTC.png/896px-Logo_UTC.png?20131104090648";
                    }
                    else
                    {
                        book.CoverImage = item.VolumeInfo.ImageLinks?.Thumbnail;
                    }


                    // Thêm tác giả
                    Author authortmp = null;
                    if (item.VolumeInfo.Authors != null)
                    {
                        foreach (var author in item.VolumeInfo.Authors)
                        {
                            authortmp = await GetAuthorByName(author);
                            if (authortmp == null)
                            {
                                authortmp = new Author { AuthorName = author, NumberOfWorks = 1 };
                                AddAuthor(authortmp);
                            }
                            else
                            {
                                int tmp = authortmp.NumberOfWorks.Value;
                                authortmp.NumberOfWorks = (tmp + 1);
                            }
                        }
                    }
                    else
                    {
                        authortmp = await GetAuthorByName("Không rõ Tác Giả");
                        if (authortmp == null)
                        {
                            authortmp = new Author { AuthorName = "Không rõ Tác Giả" };
                            AddAuthor(authortmp);
                        }
                        
                    }
                    book.Authors.Add(authortmp);


                    // Thêm nhà xuất bản
                    Publisher publisher = null;

                    if (item.VolumeInfo.Publisher != null)
                    {
                        publisher = await GetPublisherByName(item.VolumeInfo.Publisher);

                        if (publisher == null)
                        {
                            publisher = new Publisher { PublisherName = item.VolumeInfo.Publisher };
                            AddPublisher(publisher);
                        }
                    }
                    else
                    {
                        publisher = await GetPublisherByName("Không rõ NXB");

                        if (publisher == null)
                        {
                            publisher = new Publisher { PublisherName = "Không rõ NXB" };
                            AddPublisher(publisher);
                        }
                    }
                    book.Publisher = publisher;


                    // Thêm thể loại
                    Category category = null;
                    if (item.VolumeInfo.Categories != null)
                    {
                        foreach (var categoryName in item.VolumeInfo.Categories)
                        {
                            category = await GetCategoryByName(categoryName);
                            if (category == null)
                            {
                                category = new Category { CategoryName = categoryName };
                                AddCategory(category);
                            }
                        }
                    }
                    else
                    {
                        category = await GetCategoryByName("Không rõ Thể Loại");
                        if(category == null)
                        {
                            category = new Category { CategoryName = "Không rõ Thể Loại" };
                            AddCategory(category);
                        }
                        
                    }
                    book.Categories.Add(category);

                    // Kiểm tra xem sách đã tồn tại chưa
                    var existingBook = await _context.Books.AnyAsync(b => b.Title == book.Title);

                    if (!existingBook)
                    {
                        AddBook(book);
                        Console.WriteLine($"Added book: {book.Title}");
                    }
                    else
                    {
                        Console.WriteLine($"Book already exists: {book.Title}");
                    }
                }
            }
        }


        // Phương thức thêm tác giả mới
        public void AddAuthor(Author author)
        {
            _context.Authors.Add(author);
             _context.SaveChanges();
        }
        // Phương thức lấy tác giả theo tên
        public async Task<Author> GetAuthorByName(string name)
        {
            return await _context.Authors.FirstOrDefaultAsync(a => a.AuthorName == name);
        }

        public void AddPublisher(Publisher publisher)
        {
            _context.Publishers.Add(publisher);
            _context.SaveChanges();
        }

        // Phương thức lấy nhà xuất bản theo tên
        public async Task<Publisher> GetPublisherByName(string name)
        {
            return await _context.Publishers.FirstOrDefaultAsync(p => p.PublisherName == name);
        }

        // Phương thức thêm sách mới
        public void AddBook(Book book)
        {
            _context.Books.Add(book);
            _context.SaveChanges();
        }
        // Phương thức lấy thể loại theo tên
        public async Task<Category> GetCategoryByName(string name)
        {
            return await _context.Categories.FirstOrDefaultAsync(a => a.CategoryName == name);
        }

        public void AddCategory(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
        }

    }
}
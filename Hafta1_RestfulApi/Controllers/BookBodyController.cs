using Hafta1_RestfulApi.Models;
using Microsoft.AspNetCore.Mvc;
using static System.Reflection.Metadata.BlobBuilder;

namespace Hafta1_RestfulApi.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class BookBodyController : Controller
    {
        private static List<Book> BookList = new List<Book>()
        {
            new Book
            {
                Id= 1,
                Title="Lean Startup",
                GenreId= 1,
                PageCount= 200,
                PublishDate= new DateTime(2001,06,12)
            },
            new Book
            {
                Id= 2,
                Title="Merland",
                GenreId= 2,
                PageCount= 250,
                PublishDate= new DateTime(2010,05,23)
            },
            new Book
            {
                Id= 3,
                Title="Dune",
                GenreId= 2,
                PageCount= 540,
                PublishDate= new DateTime(2001,12,21)
            }
        };

        [HttpGet]
        public IActionResult GetBooks()
        {
            List<Book> books;

            try
            {
                books = BookList.OrderBy(x => x.Id).ToList();
                if (books is null)
                    return NotFound("Liste bulunamadı");
            }
            catch (Exception)
            {
                return StatusCode(500, "Sunucu Hatası");
            }

            return Ok(books);
        }

        [HttpGet("{id}")]
        public IActionResult GetBook(int id)
        {
            Book? book;

            try
            {
                book = BookList.SingleOrDefault(x => x.Id == id);
                if (book is null)
                    return NotFound("Kitap bulunamadı");
            }
            catch (Exception)
            {
                return StatusCode(500, "Sunucu Hatası");
            }


            return Ok(book);
        }


        [HttpGet]
        [Route("GetByFilter")]
        public IActionResult GetByFilter([FromQuery] string? title = null, [FromQuery] string? genreId = null,
            [FromQuery] string? pageCount = null, [FromQuery] string? publishYear = null)
        {
            List<Book> books = BookList.OrderBy(x => x.Id).ToList();

            try
            {
                if (books is null)
                    return NotFound("Liste bulunamadı");


                if (title is not null)
                {
                    books = books.Where(x => x.Title.ToLower().Contains(title.ToLower())).ToList();

                    if (books.Count == 0)
                        return NotFound("Liste bulunamadı");
                }

                if (genreId is not null)
                {
                    try
                    {
                        Convert.ToUInt16(genreId);
                    }
                    catch (FormatException)
                    {
                        return BadRequest("Lütfen tür ıd'si girin");
                    }

                    books = books.Where(x => x.GenreId.ToString().ToLower().Contains(genreId.ToString())).ToList();

                    if (books.Count == 0)
                        return NotFound("Liste bulunamadı");
                }

                if (pageCount is not null)
                {
                    try
                    {
                        Convert.ToUInt16(pageCount);
                    }
                    catch (FormatException)
                    {
                        return BadRequest("Lütfen sayfa sayısı girin");
                    }

                    books = books.Where(x => x.PageCount.ToString().ToLower().
                                      Equals(pageCount)).ToList();

                    if (books.Count == 0)
                        return NotFound("Liste bulunamadı");
                }


                if (publishYear is not null)
                {
                    try
                    {
                        Convert.ToUInt16(publishYear);
                    }
                    catch (FormatException)
                    {
                        return BadRequest("Lütfen yayın yılı girin");
                    }

                    books = books.Where(x => x.PublishDate.Year.ToString().Equals(publishYear)).ToList();

                    if (books.Count == 0)
                        return NotFound("Liste bulunamadı");
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Sunucu Hatası");
            }

            return Ok(books);
        }

        [HttpPost]
        public IActionResult AddBook([FromBody] Book newBook)
        {
            Book? book = BookList.SingleOrDefault(x => x.Id == newBook.Id);

            try
            {
                if (book is not null)
                    return BadRequest("Kitap zaten mevcut");

                BookList.Add(newBook);
            }
            catch (Exception)
            {
                return StatusCode(500, "Sunucu Hatası");
            }

            return Created("~api/Book/GetBooks", newBook);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUserWithPut([FromBody] Book updateBook, int id)
        {
            Book? book = BookList.SingleOrDefault(x => x.Id == id);

            try
            {
                if (book is null)
                    return BadRequest("Kitap bulunamadı");

                book.Title = updateBook.Title != default ? updateBook.Title : book.Title;
                book.GenreId = updateBook.GenreId != default ? updateBook.GenreId : book.GenreId;
                book.PageCount = updateBook.PageCount != default ? updateBook.PageCount : book.PageCount;
                book.PublishDate = updateBook.PublishDate != default ? updateBook.PublishDate : book.PublishDate;
            }
            catch (Exception)
            {
                return StatusCode(500, "Sunucu Hatası");
            }

            return Ok("Kitap güncellenmiştir");
        }

        [HttpPatch("{id}")]
        public IActionResult UpdateUserWithPatch([FromBody] Book updateBook, int id)
        {
            Book? book = BookList.SingleOrDefault(x => x.Id == id);

            try
            {
                if (book is null)
                    return BadRequest("Kitap bulunamadı");

                if (updateBook.Title is not null)
                    book.Title = updateBook.Title;
                if (updateBook.GenreId.ToString() is not null)
                    book.GenreId = updateBook.GenreId;
                if (updateBook.PageCount.ToString() is not null)
                    book.PageCount = updateBook.PageCount;
                if (updateBook.PublishDate.ToString() is not null)
                    updateBook.PublishDate = updateBook.PublishDate;
            }
            catch (Exception)
            {
                return StatusCode(500, "Sunucu Hatası");
            }

            return Ok("Kitap güncellenmiştir");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            Book? book = BookList.SingleOrDefault(x => x.Id == id);

            try
            {
                if (book is null)
                    return BadRequest("Kitap bulunamadı");

                BookList.Remove(book);
            }
            catch (Exception)
            {
                return StatusCode(500, "Sunucu Hatası");
            }

            return Ok("Kitap silinmiştir");
        }
    }
}

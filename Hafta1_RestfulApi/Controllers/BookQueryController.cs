using Hafta1_RestfulApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace Hafta1_RestfulApi.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class BookQueryController : Controller
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

        [HttpGet]
        [Route("GetById")]
        public IActionResult GetBook([FromQuery] string id)
        {
            Book? book;

            try
            {
                book = BookList.SingleOrDefault(x => x.Id.ToString() == id);
                if (book is null)
                    return NotFound("Kitap bulunamadı");
            }
            catch (Exception)
            {
                return StatusCode(500, "Sunucu Hatası");
            }


            return Ok(book);

        }

        [HttpPut]
        public IActionResult UpdateUserWithPut([FromBody] Book updateBook, [FromQuery] string id)
        {
            Book? book = BookList.SingleOrDefault(x => x.Id.ToString() == id);

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

        [HttpPatch]
        public IActionResult UpdateUserWithPatch([FromBody] Book updateBook, [FromQuery] string id)
        {
            Book? book = BookList.SingleOrDefault(x => x.Id.ToString() == id);

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

        [HttpDelete]
        public IActionResult Delete([FromQuery] string id)
        {
            Book? book = BookList.SingleOrDefault(x => x.Id.ToString() == id);

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

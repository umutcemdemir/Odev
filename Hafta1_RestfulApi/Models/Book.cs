using System.ComponentModel.DataAnnotations;

namespace Hafta1_RestfulApi.Models
{
    public class Book
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public int GenreId { get; set; }
        [Required]
        public int PageCount { get; set; }
        [Required]
        public DateTime PublishDate { get; set; }
    }
}

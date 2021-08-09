using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminPanel.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        public string ImageUrl { get; set; } = "/img/theme/cover.jpg";
        public string Title { get; set; }
        public string ISBN { get; set; }
        [ForeignKey(nameof(Author))]
        public int AuthorId { get; set; }
        [ForeignKey(nameof(Publisher))]
        public int PublisherId { get; set; }
        public int PublishYear { get; set; }
        public int CopyNumber { get; set; }
        [ForeignKey(nameof(BookCategory))]
        public int BookCategoryId { get; set; }
        public double Price { get; set; }
        public BookCategory BookCategory { get; set; }
        public Author Author { get; set; }
        public Publisher Publisher { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;

    }
}

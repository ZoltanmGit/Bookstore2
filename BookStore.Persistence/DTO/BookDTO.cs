using Bookstore.Persistence.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bookstore.Persistence.DTO
{
    public class BookDTO
    {
        public String ISBN { get; set; }
        public String Title { get; set; }
        public String Author { get; set; }
        public DateTime PublishDate { get; set; }

        public byte[] CoverImage { get; set; }

        public static explicit operator Book(BookDTO dto) => new Book
        {
            ISBN = dto.ISBN,
            Title = dto.Title,
            Author = dto.Author,
            PublishDate = dto.PublishDate,
            CoverImage = dto.CoverImage
        };

        public static explicit operator BookDTO(Book b) => new BookDTO
        {
            ISBN = b.ISBN,
            Title = b.Title,
            Author = b.Author,
            PublishDate = b.PublishDate,
            CoverImage = b.CoverImage
        };
    }
}

using Bookstore.Persistence.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bookstore.Persistence.DTO
{
    public class BookVolumeDTO
    {
        //public BookDTO Book { get; set; }
        public String BookISBN { get; set; }
        public Int32 LibraryId { get; set; }

        public static explicit operator BookVolume(BookVolumeDTO dto) => new BookVolume
        {
            /*Book = (Book)dto.Book,*/
            BookISBN = dto.BookISBN,
            LibraryId = dto.LibraryId
        };
        public static explicit operator BookVolumeDTO(BookVolume i) => new BookVolumeDTO
        {
            /*Book = (BookDTO)i.Book,*/
            BookISBN = i.BookISBN,
            LibraryId = i.LibraryId
        };
    }
}

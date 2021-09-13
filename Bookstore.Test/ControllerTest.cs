using Bookstore.Persistence.DTO;
using Bookstore.Persistence.Models;
using Bookstore.Persistence.Services;
using BookstoreAPI.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Bookstore.Test
{
    public class ControllerTests : IDisposable
    {
        private readonly BookStoreDbContext _context;
        private readonly BookStoreService _service;
        private readonly BooksController _booksController;
        private readonly BookVolumesController _volumesController;
        private readonly LendingsController _lendingsController;

        public ControllerTests()
        {
            var options = new DbContextOptionsBuilder<BookStoreDbContext>()
                .UseInMemoryDatabase("TestDb")
                .Options;
            _context = new BookStoreDbContext(options);

            TestDbInitializer.Initialize(_context);

            _service = new BookStoreService(_context);

            _booksController = new BooksController(_service);

            _volumesController = new BookVolumesController(_service);

            _lendingsController = new LendingsController(_service);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
        // BooksController
        [Fact]
        public void GetBooksTest()
        {
            var result = _booksController.GetBooks();

            var content = Assert.IsAssignableFrom<IEnumerable<BookDTO>>(result.Value);

            Assert.Equal(25, content.Count());
        }
        [Theory]
        [InlineData("101010")]
        [InlineData("111111")]
        [InlineData("121212")]
        [InlineData("131313")]
        public void GetBookTest(string id)
        {
            var result = _booksController.GetBook(id);
            var content = Assert.IsAssignableFrom<BookDTO>(result.Value);

            Assert.Equal(id, content.ISBN);
        }
        [Theory]
        [InlineData("1")]
        [InlineData("2")]
        public void InvalidGetBookTest(string id)
        {
            var result = _booksController.GetBook(id);
            
            Assert.IsAssignableFrom<NotFoundResult>(result.Result);
        }
        [Fact]
        public void TestPostBook()
        {
            var count = _context.Books.Count();
            var newBook = new BookDTO
            {
                ISBN = "TestISBN",
                Title = "Test Title",
                Author = "Test Author",
                PublishDate = DateTime.Today
            };

            var result = _booksController.PostBook(newBook);
            Assert.Equal(count + 1, _context.Books.Count());

            var newBook2 = new BookDTO
            {
                ISBN = "TestISBN2",
                Title = "Test Title2",
                Author = "Test Author2",
                PublishDate = DateTime.Today
            };

            var result2 = _booksController.PostBook(newBook2);
            Assert.Equal(count + 2, _context.Books.Count());
        }
        //BookVolumesController
        [Fact]
        public void TestGetBookVolumes()
        {
            var count = _context.BookVolumes.Count();
            Assert.Equal(125, count);

            for (int i = 0; i < _context.Books.Count(); i++)
            {
                var result = _volumesController.GetBookVolumes(_context.Books.ToList()[i].ISBN);
                var content = Assert.IsAssignableFrom<IEnumerable<BookVolumeDTO>>(result.Value);
                Assert.Equal(5, content.Count());
            }
        }
        [Fact]
        public void TestPostBookVolume()
        {
            var count = _context.BookVolumes.Count();
            BookVolumeDTO newVolume = new BookVolumeDTO
            {
                BookISBN = "101010",
                LibraryId = 126
            };
            var result = _volumesController.PostBookVolume(newVolume);
            Assert.IsAssignableFrom<OkResult>(result);
            Assert.Equal(count + 1, _context.BookVolumes.Count());

            BookVolumeDTO newVolume2 = new BookVolumeDTO
            {
                BookISBN = "111111",
                LibraryId = 127
            };
            var result2 = _volumesController.PostBookVolume(newVolume2);
            Assert.IsAssignableFrom<OkResult>(result2);
            Assert.Equal(count + 2, _context.BookVolumes.Count());
        }

        [Fact]
        public void TestDeleteBookVolume()
        {
            var count = _context.BookVolumes.Count();

            var result = _volumesController.DeleteBookVolume(1);
            Assert.IsAssignableFrom<OkResult>(result);
            Assert.Equal(count - 1, _context.BookVolumes.Count());
            var result2 = _volumesController.DeleteBookVolume(2);
            Assert.IsAssignableFrom<OkResult>(result2);
            Assert.Equal(count - 2, _context.BookVolumes.Count());
        }
        [Fact]
        public void DeleteGetLendings()
        {
            var x = _context.BookVolumes.First();
           
            var result = _lendingsController.GetLendings(x.LibraryId);
            var content = Assert.IsAssignableFrom<IEnumerable<LendingDTO>>(result.Value);
            var count = content.Count();
            Assert.Equal(1, count);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Bookstore.Persistence.Models;
using Bookstore.Persistence.Services;
using Bookstore.Persistence.DTO;
using Microsoft.AspNetCore.Authorization;

namespace BookstoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles="Librarian")]
    public class BooksController : ControllerBase
    {
        private readonly IBookStoreService _service;

        public BooksController(IBookStoreService service)
        {
            _service = service;
        }

        // GET: api/Books
        [HttpGet]
        public ActionResult<IEnumerable<BookDTO>> GetBooks()
        {
            return _service.GetBooks().Select(bookList => (BookDTO)bookList).ToList();
        }
        // GET: api/Books/5
        [HttpGet("{id}")]
        public ActionResult<BookDTO> GetBook(string id)
        {
            try
            {
                return (BookDTO)_service.GetBookByISBN(id);
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }
        }
        //Post: api/Books/
        [HttpPost]
        public ActionResult<BookDTO> PostBook(BookDTO bookDTO)
        {
            try
            {
                _service.GetBookByISBN(bookDTO.ISBN);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            catch (InvalidOperationException)
            {
                var resultBook = _service.CreateBook((Book)bookDTO);

                if (resultBook is null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
                return CreatedAtAction("GetBook", new { id = resultBook.ISBN });
            }
        }
    }
}

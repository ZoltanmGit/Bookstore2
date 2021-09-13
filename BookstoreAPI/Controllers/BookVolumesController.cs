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
    [Authorize(Roles = "Librarian")]
    public class BookVolumesController : ControllerBase
    {
        private readonly IBookStoreService _service;

        public BookVolumesController(IBookStoreService service)
        {
            _service = service;
        }

        // GET: api/BookVolumes/ISBN
        [HttpGet("{argISBN}")]
        public ActionResult<IEnumerable<BookVolumeDTO>> GetBookVolumes(string argISBN)
        {
            return _service.GetVolumesByBook(argISBN).Select(volumelist => (BookVolumeDTO)volumelist).ToList();
        }
        // POST: api/BookVolumes/
        [HttpPost]
        public ActionResult PostBookVolume(BookVolumeDTO volumeDTO)
        {

            if(_service.AddVolume((BookVolume)volumeDTO))
            {
                return Ok();
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        // api/BookVolumes
        [HttpDelete("{argLibraryId}")]
        public ActionResult DeleteBookVolume(int argLibraryId)
        {
            if (_service.DeleteVolume(argLibraryId))
            {
                return Ok();
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}

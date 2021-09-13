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
    public class LendingsController : ControllerBase
    {
        private readonly IBookStoreService _service;

        public LendingsController(IBookStoreService service)
        {
            _service = service;
        }

        // GET: api/Lendings/VolumeId
        [HttpGet("{argLendingId}")]
        public ActionResult<IEnumerable<LendingDTO>> GetLendings(int argLendingId)
        {
            try
            {
                return _service.GetLendingsForVolume(argLendingId).Select(lending => (LendingDTO)lending).ToList();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
        [HttpDelete("{id}")]
        public ActionResult DeleteLendings(int id)
        {
            if(!_service.DeleteLending(id))
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return Ok();
        }
        [HttpPut]
        public ActionResult PutLending([FromBody] LendingDTO lending)
        {
            if (_service.UpdateStatus((Lending)lending))
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

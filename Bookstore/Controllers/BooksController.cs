using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bookstore.Persistence.Models;
using Bookstore.Persistence.Services;
using Microsoft.AspNetCore.Authorization;

namespace Bookstore.Controllers
{
    public enum SortOrder
    {
        TITLE_ASC,
        TITLE_DESC

    }
    [AllowAnonymous]
    public class BooksController : Controller
    {
        private readonly IBookStoreService _service;

        public BooksController(IBookStoreService service)
        {
            _service = service;
        }

        // GET: Books
        public IActionResult Index(bool bShouldSort = false, bool bSortByPopular = true, SortOrder sortOrder = SortOrder.TITLE_DESC, String searchParam = "", int pageParam=0, String pageOperation="Stay")
        {
            try
            {
                switch(pageOperation)
                {
                    case "Next":
                        if(searchParam != "")
                        {
                            pageParam = Math.Clamp(pageParam + 1, 0, (_service.GetBooksByString(searchParam).Count() + 20 -1) / 20);
                            
                        }
                        else if(searchParam == "" || searchParam == null)
                        {
                            pageParam = Math.Clamp(pageParam + 1, 0, (_service.GetBooks().Count()+19) / 20);
                            
                        }
                        break;
                    case "Prev":
                        pageParam = Math.Clamp(pageParam - 1,   0, (_service.GetBooks().Count() + pageParam)/20);
                        break;
                    case "Stay":
                        break;
                }
                ViewData["PageParam"] = pageParam;
                List<Book> list;

                if (searchParam == "")
                {
                    ViewData["SearchParam"] = "";
                    list = _service.GetBooks();
                    list = list.OrderByDescending(i => _service.GetLendingsNumberForBook(i.ISBN)).ToList();
                    list = list.GetRange(pageParam * 20, Math.Min(_service.GetBooks().Count() - pageParam * 20, 20));

                    ViewData["MaxPageParam"] = (_service.GetBooks().Count() + 19) / 20;
                }
                else
                {
                    ViewData["SearchParam"] = searchParam;
                    list = _service.GetBooksByString(searchParam);
                    list = list.OrderByDescending(i => _service.GetLendingsNumberForBook(i.ISBN)).ToList();
                    list = list.GetRange(pageParam * 20, Math.Min(_service.GetBooksByString(searchParam).Count() - pageParam * 20, 20));

                    ViewData["MaxPageParam"] = (_service.GetBooksByString(searchParam).Count() + 19) / 20;
                }
                list = list.OrderByDescending(i => _service.GetLendingsNumberForBook(i.ISBN)).ToList();
                
                /*Sorting*/
                if (!bSortByPopular && bShouldSort)
                {
                    ViewData["TitleSortParam"] = sortOrder == SortOrder.TITLE_DESC? SortOrder.TITLE_ASC: SortOrder.TITLE_DESC;
                    switch (sortOrder)
                    {
                        case SortOrder.TITLE_DESC:
                            list = list.OrderByDescending(i => i.Title).ToList();
                            break;
                        case SortOrder.TITLE_ASC:
                            list = list.OrderBy(i => i.Title).ToList();
                            break;
                        default:
                            break;
                    }
                }
                
                return View(list);
            }
            catch
            {
                return NotFound();
            }
        }

        // GET: Books/Details/
        public IActionResult Details(string id)
        {

            List<KeyValuePair<int, String>> pairList = new List<KeyValuePair<int, string>>();

            List<BookVolume> volumesForBook = _service.GetVolumesByBook(id);
            for (int i = 0; i < volumesForBook.Count(); i++)
            {
                String lendingsString = "";
                List<Lending> lendingsForVolume = _service.GetLendingsForVolume(volumesForBook[i].LibraryId);
                //Doesn't have any lendings
                if(!lendingsForVolume.Any())
                {
                    lendingsString = "This volume has no current or future lendings";
                    pairList.Add(new KeyValuePair<int, string>(volumesForBook[i].LibraryId, lendingsString));
                }
                //Has lendings
                else
                {
                    for (int j = 0; j < lendingsForVolume.Count(); j++)
                    {
                        if (j == 0)
                        {
                            lendingsString = lendingsForVolume[j].StartDate.ToString("yy/MM/dd") + " - " + lendingsForVolume[j].EndDate.ToString("yy/MM/dd");
                        }
                        else
                        {
                            lendingsString = lendingsString + "<br/>" + lendingsForVolume[j].StartDate.ToString("yy/MM/dd") + " - " + lendingsForVolume[j].EndDate.ToString("yy/MM/dd");
                        }   
                    }
                    pairList.Add(new KeyValuePair<int, string>(volumesForBook[i].LibraryId, lendingsString));
                }
            }
            return View(pairList);
        }
        public IActionResult GetCoverImage(string id)
        {
            Book tempBook = _service.GetBookByISBN(id);
            if(tempBook == null)
            {
                return null;
            }
            return File(tempBook.CoverImage, "image/png");
        }
    }
}

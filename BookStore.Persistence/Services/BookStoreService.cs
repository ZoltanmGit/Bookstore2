using Bookstore.Persistence.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Bookstore.Persistence.Services
{
    public class BookStoreService : IBookStoreService
    {
        private readonly BookStoreDbContext _context;
        public BookStoreService(BookStoreDbContext context)
        {
            _context = context;
        }
        //Returns the list of books
        public List<Book> GetBooks()
        {
            return _context.Books.ToList();
        }
        public List<Book> GetBooksByString(String searchParam)
        {
            return _context.Books
                .Where(l => l.Title.Contains(searchParam) || l.Author.Contains(searchParam))
                .ToList();
        }
        public Book GetBookByISBN(String argISBN)
        {
            return _context.Books
                .Single(l => l.ISBN == argISBN);
        }
        public Book GetBookByTitle(String argBookTitle)
        {
            return _context.Books
                .Single(l => l.Title == argBookTitle);
        }
        public List<BookVolume> GetVolumesByBook(String argBookISBN)
        {
            return _context.BookVolumes
                .Where(l => l.Book.ISBN == argBookISBN)
                .ToList();
        }
        public List<Lending> GetLendingsForVolume(int argLibraryId)
        {
            return _context.Lendings
                .Where(l => l.Volume.LibraryId == argLibraryId)
                .ToList();
        }
        public bool CreateLending(Lending argLending, int userID)
        {
            
            try
            {
                argLending.Visitor = (Visitor)_context.Users.Find(userID);
                _context.Lendings.Add(argLending);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
            catch (DbUpdateException)
            {
                return false;
            }

            return true;
        }
        public BookVolume GetVolumeById(int argId)
        {
            return _context.BookVolumes
                .FirstOrDefault(l => l.LibraryId == argId);
        }
        public bool DoesOverLap(DateTime start, DateTime end, int volumeId)
        {
            List<Lending> list = GetLendingsForVolume(volumeId);
            if(!list.Any())
            {
                return false;
            }
            for (int i = 0; i < list.Count(); i++)
            {
                if(list[i].StartDate < end && start < list[i].EndDate )
                {
                    return true;
                }
            }
            return false;
        }
        public int GetLendingsNumberForBook(string argBookISBN)
        {
            int result = 0;
            List<BookVolume> volumeList = GetVolumesByBook(argBookISBN);
            for (int i = 0; i < volumeList.Count(); i++)
            {
                result += GetLendingsForVolume(volumeList[i].LibraryId).Count();
            }
            return result;
        }

        public Book CreateBook(Book argBook)
        {
            try
            {
                _context.Books.Add(argBook);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return null;
            }
            catch (DbUpdateException)
            {
                return null;
            }
            return argBook;
        }
        public bool AddVolume(BookVolume argVolume)
        {
            if(argVolume != null)
            {
                try
                {
                    _context.BookVolumes.Add(argVolume);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return false;
                }
                catch (DbUpdateException)
                {
                    return false;
                }
                return true;
            }
            return false;
        }
        public bool DeleteVolume(int argLibraryId)
        {
            try
            {
                BookVolume tempVolume = GetVolumeById(argLibraryId);
                _context.BookVolumes.Remove(tempVolume);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
            catch(DbUpdateException)
            {
                return false;
            }
            return true;

        }
        public Lending GetLendingById(int lendingId)
        {
            return _context.Lendings.Single(l => l.LendingId == lendingId);
        }
        public bool DeleteLending(int lendingId)
        {
            try
            {
                Lending tempLending = GetLendingById(lendingId);
                _context.Lendings.Remove(tempLending);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
            catch (DbUpdateException)
            {
                return false;
            }
            return true;
        }
        public bool UpdateStatus(Lending lending)
        {
            try
            {
                _context.Lendings.Update(lending);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
            catch (DbUpdateException)
            {
                return false;
            }
            return true;
        }
    }
}

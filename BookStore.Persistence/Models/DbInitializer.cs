using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Bookstore.Persistence.Models
{
    public static class DbInitializer
    {
        private static BookStoreDbContext _context;
        private static UserManager<BaseUser> _userManager;
        private static RoleManager<IdentityRole<int>> _roleManager;
        public static void Initialize(IServiceProvider serviceProvider, String imageDirectory)
        {
            _context = serviceProvider.GetRequiredService<BookStoreDbContext>();
            _userManager = serviceProvider.GetRequiredService<UserManager<BaseUser>>();
            _roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
            // Add every migration
            _context.Database.Migrate();
            //If "Books" is empty
            if (!_context.Books.Any())
            {
                SeedBooks();
                SeedVolumes();
                SeedCoverImages(imageDirectory);
                SeedLibrarians();
            }
            //if users are empty
            if(!_context.Users.Any())
            {
                SeedLibrarians();
            }
        }
        private static void SeedBooks()
        {
            IList<Book> defaultBookList = new List<Book>
            {
                new Book()
                {
                    Title = "Catch-22",
                    Author="Joseph Heller",
                    PublishDate= new DateTime(1961,11,10),
                    ISBN="111"
                },
                new Book()
                {
                    Title = "Flowers for Algernon",
                    Author="Daniel Keyes",
                    PublishDate= new DateTime(1959,4,30),
                    ISBN="222"
                },
                new Book()
                {
                    Title = "Catcher in the Rye",
                    Author="J.D. Salinger",
                    PublishDate= new DateTime(1951,7,16),
                    ISBN="333"
                },
                new Book()
                {
                    Title = "To Kill a Mockingbird",
                    Author="Harper Lee",
                    PublishDate= new DateTime(1960,7,11),
                    ISBN="444"
                },
                new Book()
                {
                    Title = "1984",
                    Author="George Orwell",
                    PublishDate= new DateTime(1949,6,8),
                    ISBN="555"
                },
                new Book()
                {
                    Title = "East of Eden",
                    Author="John Steinbeck",
                    PublishDate= new DateTime(1952,9,19),
                    ISBN="666"
                },
                new Book()
                {
                    Title = "Norse Mythology",
                    Author="Neil Gaiman",
                    PublishDate= new DateTime(2017,03,19),
                    ISBN="777"
                },
                new Book()
                {
                    Title = "Shogun",
                    Author="James Clavell",
                    PublishDate= new DateTime(1975,1,1),
                    ISBN="888"
                },
                new Book()
                {
                    Title = "Fahrenheit 451",
                    Author="Ray Bradbury",
                    PublishDate= new DateTime(1953,10,19),
                    ISBN="999"
                },
                new Book()
                {
                    Title = "Brave New World",
                    Author="Aldous Huxley",
                    PublishDate= new DateTime(1932,1,1),
                    ISBN="101010"
                },
                new Book()
                {
                    Title = "For Whom the Bell Tolls",
                    Author="Ernest Hemingway",
                    PublishDate= new DateTime(1940,10,21),
                    ISBN="111111"
                },
                new Book()
                {
                    Title = "The Old Man and the Sea",
                    Author="Ernest Hemingway",
                    PublishDate= new DateTime(1952,9,1),
                    ISBN="121212"
                },
                new Book()
                {
                    Title = "A Clockwork Orange",
                    Author="Anthony Burgess",
                    PublishDate= new DateTime(1962,1,1),
                    ISBN="131313"
                },
                new Book()
                {
                    Title = "Of Mice and Man",
                    Author="John Steinbeck",
                    PublishDate= new DateTime(1937,11,23),
                    ISBN="141414"
                },
                new Book()
                {
                    Title = "Animal Farm",
                    Author="George Orwell",
                    PublishDate= new DateTime(1945,8,17),
                    ISBN="151515"
                },
                new Book()
                {
                    Title = "The Book Thief",
                    Author="Markus Zusak",
                    PublishDate= new DateTime(2005,1,1),
                    ISBN="161616"
                },
                new Book()
                {
                    Title = "Charlotte's Web",
                    Author="E. B. White",
                    PublishDate= new DateTime(1952,10,15),
                    ISBN="171717"
                },
                new Book()
                {
                    Title = "Lolita",
                    Author="Vladimir Nabokov",
                    PublishDate= new DateTime(1955,9,1),
                    ISBN="181818"
                },
                new Book()
                {
                    Title = "The Lord of the Rings",
                    Author="J.R.R. Tolkien",
                    PublishDate= new DateTime(1954,7,29),
                    ISBN="191919"
                },
                new Book()
                {
                    Title = "The Count of Monte Cristo",
                    Author="Alexandre Dumas",
                    PublishDate= new DateTime(2002,1,25),
                    ISBN="202020"
                },
                new Book()
                {
                    Title = "Crime and Punishment",
                    Author="Fyodor Dostoyevsky",
                    PublishDate= new DateTime(1866,1,1),
                    ISBN="212121"
                },
                new Book()
                {
                    Title = "Frankenstein",
                    Author="Mary Shelley",
                    PublishDate= new DateTime(1823,1,1),
                    ISBN="222222"
                },
                new Book()
                {
                    Title = "Great Expectations",
                    Author="Charles Dickens",
                    PublishDate= new DateTime(1861,8,1),
                    ISBN="232323"
                },
                new Book()
                {
                    Title = "The Giver",
                    Author="Lois Lowry",
                    PublishDate= new DateTime(1993,1,1),
                    ISBN="242424"
                },
                new Book()
                {
                    Title = "Les Miserables",
                    Author="Victor Hugo",
                    PublishDate= new DateTime(1862,1,1),
                    ISBN="252525"
                },

            };
            _context.Books.AddRange(defaultBookList);
            _context.SaveChanges();
        }
        private static void SeedVolumes()
        {
            for (int i = 0; i < _context.Books.Count(); i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    BookVolume tempVolume = new BookVolume();
                    Book tempBook = _context.Books.ToList()[i];
                    tempVolume.Book = tempBook;
                    _context.BookVolumes.Add(tempVolume);
                }
            }
            _context.SaveChanges();
        }
        private static void SeedCoverImages(string imageDirectory)
        {
            if(Directory.Exists(imageDirectory))
            {
                for (int i = 0; i < _context.Books.Count(); i++)
                {
                    string pathToFile = Path.Combine(imageDirectory, _context.Books.ToList()[i].Title) + ".png";
                    _context.Books.ToList()[i].CoverImage = File.Exists(pathToFile) ? File.ReadAllBytes(pathToFile) : null;
                }
                _context.SaveChanges();
            }
        }
        private static void SeedLibrarians()
        {
            //Create Role
            
            //Create Librarian
            BaseUser mainLibrarian = new BaseUser
            {
                UserName = "admin",
                Name = "Frodo Baggins"
            };
            string mainLibrarianPassword = "admin";
            var librarianRole = new IdentityRole<int>("Librarian");

            var result01 = _userManager.CreateAsync(mainLibrarian, mainLibrarianPassword).Result;
            var result02 =  _roleManager.CreateAsync(librarianRole).Result;
            //Add Librarian to Role
            var result03 = _userManager.AddToRoleAsync(mainLibrarian, "Librarian").Result;
        }
    }
}
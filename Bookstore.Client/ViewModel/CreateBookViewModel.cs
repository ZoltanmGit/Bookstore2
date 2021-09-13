using Bookstore.Client.Model;
using Bookstore.Persistence.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Client.ViewModel
{
    public class CreateBookViewModel : ViewModelBase
    {
        private readonly BookstoreAPIService _service;

       public BookDTO newBook { get; private set; }

        public DelegateCommand CreateButtonCommand { get; set; }
        public DelegateCommand CancelButtonCommand { get; set; }
        public DelegateCommand UploadImageCommand { get; set; }

        public event EventHandler BookAdded;
        public event EventHandler BookCanceled;
        public event EventHandler UploadImage;
        //public event EventHandler<MessageEventArgs> MisuseEvent;
        public CreateBookViewModel(BookstoreAPIService service)
        {
            newBook = new BookDTO();
            _service = service;

            CancelButtonCommand = new DelegateCommand(_ => Cancel());
            CreateButtonCommand = new DelegateCommand(_ => AddBookAsync());
            UploadImageCommand = new DelegateCommand(_ => SignalUploadEvent());
        }
        private void Cancel()
        {
            BookCanceled(this, EventArgs.Empty);
        }
        public void AddImage(string filePath)
        {
            newBook.CoverImage = File.Exists(filePath) ? File.ReadAllBytes(filePath) : null;
        }
        private void SignalUploadEvent()
        {
            UploadImage(this, EventArgs.Empty);
        }
        private async void AddBookAsync()
        {
            if(newBook.Title is null || newBook.Author is null || newBook.ISBN is null)
            {
                return;
            }
            else
            {
                try
                {
                    await _service.CreateBookAsync(newBook);
                    
                }
                catch (Exception)
                {

                }
                finally
                {
                    BookAdded(this, EventArgs.Empty);
                }
            }
        }
    }
}

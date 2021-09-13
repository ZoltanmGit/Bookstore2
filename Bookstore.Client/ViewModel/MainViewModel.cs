using Bookstore.Client.Model;
using Bookstore.Persistence.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;

namespace Bookstore.Client.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        //Service
        private readonly BookstoreAPIService _service;
        //Properties
        private ObservableCollection<BookDTO> _books;
        public ObservableCollection<BookDTO> Books
        {
            get { return _books; }
            set { _books = value; OnPropertyChanged(); }
        }
        private ObservableCollection<BookVolumeDTO> _bookVolumes;
        public ObservableCollection<BookVolumeDTO> BookVolumes
        {
            get { return _bookVolumes; }
            set { _bookVolumes = value; OnPropertyChanged(); }
        }
        private ObservableCollection<LendingDTO> _lendings;
        public ObservableCollection<LendingDTO> Lendings
        {
            get { return _lendings; }
            set { _lendings = value; OnPropertyChanged(); }
        }
        
        //Selections
        private BookDTO _selectedBook;
        public BookDTO SelectedBook
        {
            get
            {
                return _selectedBook;
            }
            set
            {
                if(_selectedBook != value)
                {
                    _selectedBook = value;
                    OnPropertyChanged();
                }
            }
        }

        private BookVolumeDTO _selectedVolume;
        public BookVolumeDTO SelectedVolume
        {
            get { return _selectedVolume; }
            set
            {
                if (_selectedVolume != value)
                {
                    _selectedVolume = value;
                    OnPropertyChanged();
                }
            }
        }

        private LendingDTO _selectedLending;
        public LendingDTO SelectedLending
        {
            get
            {
                return _selectedLending;
            }
            set
            {
                if (_selectedLending != value)
                {
                    _selectedLending = value;
                    OnPropertyChanged();
                }
            }
        }
        //Commands
        public DelegateCommand UpdateBooksCommand { get; set; }
        public DelegateCommand SelectBookCommand { get; set; }
        public DelegateCommand SelectVolumeCommand { get; set; }
        public DelegateCommand SelectLendingCommand { get; set; }
        public DelegateCommand CreateBookCommand { get; set; }
        public DelegateCommand LogoutCommand { get; set; }
        public DelegateCommand AddVolumeCommand { get; set; }
        public DelegateCommand DeleteVolumeCommand { get; set; }
        public DelegateCommand SetActiveCommand { get; set; }
        public DelegateCommand SetInactiveCommand { get; set; }
        //Events
        public event EventHandler LogoutSucceeded;
        public event EventHandler OpenCreateBook;
        public event EventHandler VolumesChanged;
        public event EventHandler LendingsDeleted;
        public event EventHandler<MessageEventArgs> MisuseEvent;
        //Constructor
        public MainViewModel(BookstoreAPIService service)
        {
            _service = service;
            UpdateBooksCommand = new DelegateCommand(_ => LoadBooksAsync());
            SelectBookCommand = new DelegateCommand(param => LoadVolumesAsync(param as BookDTO));
            SelectVolumeCommand = new DelegateCommand(param => LoadLendingsAsync(param as BookVolumeDTO));
            SelectLendingCommand = new DelegateCommand(param => SelectLending(param as LendingDTO));
            //Logout Button
            LogoutCommand = new DelegateCommand(_ => LogoutAsync());
            //Add a new book
            CreateBookCommand = new DelegateCommand(_ => NavigateToCreateBook());
            //Command for adding a new Volume for the selected Book
            AddVolumeCommand = new DelegateCommand(_=> AddVolumeAsync());
            //Command for deleting a volume if it has no active lendings
            DeleteVolumeCommand = new DelegateCommand(_ => DeleteVolumeAsync());

            SetActiveCommand = new DelegateCommand(_ => SetActiveAsync());
            SetInactiveCommand = new DelegateCommand(_ => SetInactiveAsync());
        }

        

        //Methods - Functions
        private async void LogoutAsync()
        {
            try
            {
                await _service.LogoutAsync();
                LogoutSucceeded?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception exception) when (exception is NetworkException || exception is HttpRequestException)
            {
                OnMessageApplication($"Unexpected error occured! ({exception.Message})");
            }
        }
        private async void LoadBooksAsync()
        {
            try
            {
                Books = new ObservableCollection<BookDTO>(await _service.LoadBooksAsync());
            }
            catch (Exception exception) when (exception is NetworkException || exception is HttpRequestException)
            {
                OnMessageApplication($"Unexpected Error: ({exception.Message})");
            }
        }
        private async void LoadVolumesAsync(BookDTO bookDTO)
        {
            if (bookDTO == null)
            {
                return;
            }
            else
            {
                SelectedBook = bookDTO;
                try
                {
                    BookVolumes = new ObservableCollection<BookVolumeDTO>(await _service.LoadBookVolumesAsync(bookDTO.ISBN));
                }
                catch (Exception exception) when (exception is NetworkException || exception is HttpRequestException)
                {
                    OnMessageApplication($"Unexpected Error: ({exception.Message})");
                }
            }
        }
        private void SelectLending(LendingDTO lendingDTO)
        {
            _selectedLending = lendingDTO;
        }
        private async void LoadLendingsAsync(BookVolumeDTO volumeDTO)
        {
            if (volumeDTO == null)
            {
                return;
            }
            else
            {
                SelectedVolume = volumeDTO;
                try
                {
                    Lendings = new ObservableCollection<LendingDTO>(await _service.LoadLendingsAsync(volumeDTO.LibraryId));
                }
                catch (Exception exception) when (exception is NetworkException || exception is HttpRequestException)
                {
                    OnMessageApplication($"Unexpected Error: ({exception.Message})");
                }
            }
        }
        private void NavigateToCreateBook()
        {
            OpenCreateBook(this, EventArgs.Empty);
        }
        private async void AddVolumeAsync()
        {
            if(SelectedBook is null)
            {
                return;
            }
            BookVolumeDTO newVolume = new BookVolumeDTO();
            newVolume.BookISBN = SelectedBook.ISBN;

            await _service.AddVolumeAsync(newVolume);
            VolumesChanged(this, EventArgs.Empty);
        }
        private LendingDTO HasActiveLendings(BookVolumeDTO volumeDTO)
        {
            if(volumeDTO != null)
            {
                for (int i = 0; i < Lendings.Count; i++)
                {
                    if (Lendings[i].IsActive)
                    {
                        return Lendings[i];
                    }
                }
                return null;
            }
            //Don't do anything in DeleteVolumeAsync()
            return null;
        }
        private async void DeleteVolumeAsync()
        {
            if(SelectedVolume is null || HasActiveLendings(SelectedVolume) != null)
            {
                MessageEventArgs e = new MessageEventArgs("There is an ongoing active status");
                MisuseEvent(this, e);
                return;
            }
            //Delete the lendings if there are any
            if(Lendings.Count != 0)
            {
                foreach (var item in Lendings)
                {
                    await _service.DeleteLendingsAsync(item.LendingId);
                }
                LendingsDeleted(this, EventArgs.Empty);
                _selectedLending = null;
            }
            //Delete the volume
            await _service.DeleteVolumeAsync(SelectedVolume.LibraryId);
            SelectedVolume = null;
            VolumesChanged(this, EventArgs.Empty);
        }

        private async void SetActiveAsync()
        {
            if (SelectedLending == null)
            {
                MessageEventArgs e = new MessageEventArgs("No Lending is Selected");
                MisuseEvent(this, e);
                return;
            }
            if (SelectedVolume is null || HasActiveLendings(SelectedVolume) != null)
            {
                MessageEventArgs e = new MessageEventArgs("There is an ongoing active status!");
                MisuseEvent(this, e);
                return;
            }
            SelectedLending.IsActive = true;
            await _service.SetStatus(SelectedLending);
            SelectVolumeCommand.Execute(SelectedVolume);
        }
        private async void SetInactiveAsync()
        {
            if (SelectedLending == null)
            {
                MessageEventArgs e = new MessageEventArgs("No Lending is Selected");
                MisuseEvent(this, e);
                return;
            }
            if (SelectedVolume is null || !SelectedLending.IsActive)
            {
                MessageEventArgs e = new MessageEventArgs("It's already false!");
                MisuseEvent(this, e);
                return;
            }
            
            SelectedLending.IsActive = false;
            await _service.SetStatus(SelectedLending);
            SelectVolumeCommand.Execute(SelectedVolume);
        }
    }
}

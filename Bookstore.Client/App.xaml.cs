using Bookstore.Client.Model;
using Bookstore.Client.View;
using Bookstore.Client.ViewModel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Bookstore.Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        //Service 
        private BookstoreAPIService _service;
        //ViewModels
        private MainViewModel _mainViewModel;
        private LoginViewModel _loginViewModel;
        private CreateBookViewModel _createBookViewModel;
        //Views
        private MainWindow _mainView;
        private LoginWindow _loginView;
        private CreateBookWindow _createBookWindow;
        public App()
        {
            Startup += App_Startup;
        }
        private void App_Startup(object sender, StartupEventArgs e)
        {
            _service = new BookstoreAPIService(ConfigurationManager.AppSettings["baseAddress"]);

            //MainViewModel
            

            //LoginViewModel
            _loginViewModel = new LoginViewModel(_service);
            _loginViewModel.LoginSucceeded += _loginViewModel_LoginSucceeded;
            _loginViewModel.LoginFailed += _loginViewModel_LoginFailed;

            _createBookViewModel = new CreateBookViewModel(_service);
            _createBookViewModel.BookAdded += _createBookViewModel_OnBookAdded;
            _createBookViewModel.BookCanceled += _createBookViewModel_OnBookCanceled;
            _createBookViewModel.UploadImage += _createBookViewModel_OnUploadImage;
            //Create
            //Data Contexts
            _loginView = new LoginWindow
            {
                DataContext = _loginViewModel
            };
            _createBookWindow = new CreateBookWindow
            {
                DataContext = _createBookViewModel
            };
            _loginView.Show();
        }
        private void OnMessageApplication(object sender, MessageEventArgs e)
        {
            MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        //LoginViewModel
        private void _loginViewModel_LoginSucceeded(object sender, EventArgs e)
        {
            _mainViewModel = new MainViewModel(_service);
            _mainViewModel.LogoutSucceeded += _mainViewModel_LogoutSucceeded;
            _mainViewModel.MessageApplication += OnMessageApplication;
            _mainViewModel.OpenCreateBook += _mainViewModel_OnOpenCreateBook;
            _mainViewModel.VolumesChanged += _mainViewModel_OnVolumeAdded;
            _mainViewModel.LendingsDeleted += _mainViewModel_OnLendingsDeleted;
            _mainViewModel.MisuseEvent += _mainViewModel_OnMisuseEvent;

            _mainView = new MainWindow
            {
                DataContext = _mainViewModel
            };

            _loginView.txtName.Clear();
            _loginView.txtPassword.Clear();
            _loginView.Hide();
            _mainView.Show();
        }
        private void _loginViewModel_LoginFailed(object sender, EventArgs e)
        {
            MessageBox.Show("Login Failed", "Bookstore", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }
        //MainViewModel
        private void _mainViewModel_OnLendingsDeleted(object sender, EventArgs e)
        {
            _mainViewModel.Lendings.Clear();
        }
        private void _mainViewModel_OnMisuseEvent(object sender, MessageEventArgs e)
        {
            MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }
        private void _mainViewModel_OnVolumeAdded(object sender, EventArgs e)
        {
            _mainViewModel.SelectBookCommand.Execute(_mainViewModel.SelectedBook);
        }
        private void _mainViewModel_OnOpenCreateBook(object sender, EventArgs e)
        {
            if(_createBookViewModel != null && _createBookWindow != null)
            {
                _createBookWindow.Show();
            }
        }
        private void _mainViewModel_LogoutSucceeded(object sender, EventArgs e)
        {
            _mainView.Hide();
            _loginView.Show();
        }
        //CreateBookViewModel
        private void _createBookViewModel_OnUploadImage(object sender, EventArgs e)
        {
            try
            {
                // egy dialógusablakban bekérjük a fájlnevet
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.CheckFileExists = true;
                dialog.Filter = "Képfájlok|*.jpg;*.jpeg;*.bmp;*.tif;*.gif;*.png;";
                dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                Boolean? result = dialog.ShowDialog();

                if (result == true)
                {
                    _createBookViewModel.AddImage(dialog.FileName);
                    _createBookWindow.ImageStateLabel.Content = "Image Uploaded";
                    _createBookWindow.ImageStateLabel.Opacity = 1.0;
                    _createBookWindow.ImageStateLabel.Foreground = new SolidColorBrush(Colors.LightGreen);
                }
            }
            catch { }
        }
        private void _createBookViewModel_OnBookCanceled(object sender, EventArgs e)
        {
            if (_createBookViewModel != null && _createBookWindow != null)
            {
                _createBookWindow.Hide();
                _createBookWindow.ISBNName.Clear();
                _createBookWindow.AuthorName.Clear();
                _createBookWindow.TitleName.Clear();
            }
        }
        private void _createBookViewModel_OnBookAdded(object sender, EventArgs e)
        {
            _createBookWindow.ISBNName.Clear();
            _createBookWindow.AuthorName.Clear();
            _createBookWindow.TitleName.Clear();
            _createBookWindow.Hide();
            _mainViewModel.UpdateBooksCommand.Execute(null);
        }
    }
}

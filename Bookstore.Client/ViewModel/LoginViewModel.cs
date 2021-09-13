using Bookstore.Client.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Windows.Controls;

namespace Bookstore.Client.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly BookstoreAPIService _service;

        //Properties
        private bool _bIsLoading;
        public bool bIsLoading
        {
            get { return _bIsLoading; }
            set
            {
                _bIsLoading = value; OnPropertyChanged();
            }
        }
        public bool bIsLoginEnabled
        {
            get
            {
                return !bIsLoading;
            }
        }
        public string Username { get; set; }

        //Commands
        public DelegateCommand LoginCommand { get; set; }

        //Events
        public event EventHandler LoginSucceeded;
        public event EventHandler LoginFailed;
        public LoginViewModel(BookstoreAPIService service)
        {
            _service = service;
            bIsLoading = false;
            LoginCommand = new DelegateCommand(_ => !bIsLoading, param => LoginAsync(param as PasswordBox));
        }
        private async void LoginAsync(PasswordBox passwordBox)
        {
            try
            {
                bIsLoading = true;
                bool result = await _service.LoginAsync(Username, passwordBox.Password);

                if (result)
                    LoginSucceeded?.Invoke(this, EventArgs.Empty);
                else
                    LoginFailed?.Invoke(this, EventArgs.Empty);

            }
            catch (Exception exception) when (exception is NetworkException || exception is HttpRequestException)
            {
                OnMessageApplication($"Unexpected error occured! ({exception.Message})");
            }
            finally
            {
                bIsLoading = false;
            }
        }
    }
}

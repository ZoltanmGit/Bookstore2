using Bookstore.Persistence.DTO;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Client.Model
{
    public class BookstoreAPIService
    {
        private readonly HttpClient _client;
        public BookstoreAPIService(string baseAddress)
        {
            _client = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress)
            };
        }
        public async Task<IEnumerable<BookDTO>> LoadBooksAsync()
        {
            var result = await _client.GetAsync("api/Books");
            if (result.IsSuccessStatusCode)
            {
                return await result.Content.ReadAsAsync<IEnumerable<BookDTO>>();
            }
            throw new NetworkException("Service returned response: " + result.StatusCode);
        }
        public async Task<IEnumerable<BookVolumeDTO>> LoadBookVolumesAsync(string argISBN)
        {
            var result = await _client.GetAsync($"api/BookVolumes/{argISBN}");
            if (result.IsSuccessStatusCode)
            {
                return await result.Content.ReadAsAsync<IEnumerable<BookVolumeDTO>>();
            }
            throw new NetworkException("Service returned response: " + result.StatusCode);
        }
        public async Task<IEnumerable<LendingDTO>> LoadLendingsAsync(int libraryId)
        {
            var result = await _client.GetAsync($"api/Lendings/{libraryId}");
            if (result.IsSuccessStatusCode)
            {
                return await result.Content.ReadAsAsync<IEnumerable<LendingDTO>>();
            }
            throw new NetworkException("Service returned response: " + result.StatusCode);
        }

        public async Task<bool> LoginAsync(string username, string password)
        {
            LoginDTO user = new LoginDTO
            {
                UserName = username,
                Password = password
            };

            HttpResponseMessage response = await _client.PostAsJsonAsync("api/Librarian/Login", user);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return false;
            }
            throw new NetworkException("Service returned response:" + response.StatusCode);
        }
        public async Task LogoutAsync()
        {
            HttpResponseMessage response = await _client.PostAsync("api/Librarian/Logout", null);

            if (response.IsSuccessStatusCode)
            {
                return;
            }

            throw new NetworkException("Service returned response: " + response.StatusCode);
        }

        public async Task CreateBookAsync(BookDTO bookDTO)
        {
            HttpResponseMessage response = await _client.PostAsJsonAsync("api/Books/", bookDTO);
            bookDTO.ISBN = (await response.Content.ReadAsAsync<BookDTO>()).ISBN;

            if (!response.IsSuccessStatusCode)
            {
                throw new NetworkException("Service returned response: " + response.StatusCode);
            }
        }
        public async Task AddVolumeAsync(BookVolumeDTO volumeDTO)
        {
            HttpResponseMessage response = await _client.PostAsJsonAsync("api/BookVolumes/", volumeDTO);

            if (!response.IsSuccessStatusCode)
            {
                throw new NetworkException("Service returned response: " + response.StatusCode);
            }
        }
        public async Task DeleteVolumeAsync(int argLibraryId)
        {
            HttpResponseMessage response = await _client.DeleteAsync($"api/BookVolumes/{argLibraryId}");

            if (!response.IsSuccessStatusCode)
            {
                throw new NetworkException("Service returned response: " + response.StatusCode);
            }
        }
        public async Task DeleteLendingsAsync(int lendingId)
        {
            HttpResponseMessage response = await _client.DeleteAsync($"api/Lendings/{lendingId}");

            if (!response.IsSuccessStatusCode)
            {
                throw new NetworkException("Service returned response: " + response.StatusCode);
            }
        }
        public async Task SetStatus(LendingDTO lendingDTO)
        {
            HttpResponseMessage response = await _client.PutAsJsonAsync("api/Lendings/",lendingDTO);

            if (!response.IsSuccessStatusCode)
            {
                throw new NetworkException("Service returned response: " + response.StatusCode);
            }
        }
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorApp3.Data;

namespace BlazorApp3.Services
{
    public interface IBookService
    {
        Task<List<Book>> GetTopBooksAsync();
        Task<List<BorrowedBook>> GetUserBooksAsync(string userId);
        Task<List<Book>> GetBooksAsync(string searchTerm, string sortOption);
        Task<Book> GetBookByIdAsync(int bookId);
        Task<bool> IsBookBorrowedByUserAsync(int bookId, string userId);
        Task BorrowBookAsync(int bookId, string userId);
        Task ReturnBookAsync(int bookId, string userId);
        Task DeleteBookAsync(int bookId);
        Task<List<BorrowedBook>> GetBorrowedBooksByBookIdAsync(int bookId);
        Task AddBookAsync(Book book); // Dodane
        Task UpdateBookAsync(Book book); // Dodane
    }



}
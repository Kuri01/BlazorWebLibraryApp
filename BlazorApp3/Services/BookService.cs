using BlazorApp3.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp3.Services
{
public class BookService : IBookService
{
    private readonly ApplicationDbContext _dbContext;

    public BookService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Book>> GetTopBooksAsync()
    {
        return await _dbContext.Books
            .OrderByDescending(b => b.TimesBorrowed)
            .Take(10)
            .ToListAsync();
    }

    public async Task<List<BorrowedBook>> GetUserBooksAsync(string userId)
    {
        return await _dbContext.BorrowedBooks
            .Where(bb => bb.UserId == userId)
            .Include(bb => bb.Book)
            .ToListAsync();
    }

    public async Task<List<Book>> GetBooksAsync(string searchTerm, string sortOption)
    {
        IQueryable<Book> query = _dbContext.Books;

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(b => b.Title.Contains(searchTerm) || b.Author.Contains(searchTerm));
        }

        query = sortOption switch
        {
            "title" => query.OrderBy(b => b.Title),
            "author" => query.OrderBy(b => b.Author),
            "timesBorrowed" => query.OrderByDescending(b => b.TimesBorrowed),
            _ => query
        };

        return await query.ToListAsync();
    }
    
    public async Task<Book> GetBookByIdAsync(int bookId)
    {
        return await _dbContext.Books.FirstOrDefaultAsync(b => b.Id == bookId);
    }

    public async Task<bool> IsBookBorrowedByUserAsync(int bookId, string userId)
    {
        return await _dbContext.BorrowedBooks.AnyAsync(bb => bb.BookId == bookId && bb.UserId == userId);
    }

    public async Task BorrowBookAsync(int bookId, string userId)
    {
        var book = await _dbContext.Books.FindAsync(bookId);
        if (book != null && !book.IsBorrowed)
        {
            book.IsBorrowed = true;
            _dbContext.BorrowedBooks.Add(new BorrowedBook { BookId = bookId, UserId = userId });
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task ReturnBookAsync(int bookId, string userId)
    {
        var borrowedBook = await _dbContext.BorrowedBooks.FirstOrDefaultAsync(bb => bb.BookId == bookId && bb.UserId == userId);
        if (borrowedBook != null)
        {
            _dbContext.BorrowedBooks.Remove(borrowedBook);
            var book = await _dbContext.Books.FindAsync(bookId);
            if (book != null)
            {
                book.IsBorrowed = false;
            }
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task DeleteBookAsync(int bookId)
    {
        var book = await _dbContext.Books.FindAsync(bookId);
        if (book != null)
        {
            _dbContext.Books.Remove(book);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task AddBookAsync(Book book)
    {
        _dbContext.Books.Add(book);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateBookAsync(Book book)
    {
        var existingBook = await _dbContext.Books.FindAsync(book.Id);
        if (existingBook != null)
        {
            existingBook.Title = book.Title;
            existingBook.Author = book.Author;
            existingBook.Description = book.Description;
            existingBook.TimesBorrowed = book.TimesBorrowed;
            existingBook.IsBorrowed = book.IsBorrowed;
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task<List<BorrowedBook>> GetBorrowedBooksByBookIdAsync(int bookId)
    {
        return await _dbContext.BorrowedBooks
            .Where(bb => bb.BookId == bookId)
            .Include(bb => bb.Book)
            .ToListAsync();
    }
}
}

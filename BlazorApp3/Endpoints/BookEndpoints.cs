using BlazorApp3.Data;
using BlazorApp3.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace BlazorApp3.Endpoints
{
    public static class BookEndpoints
    {
        public static void MapBookEndpoints(this IEndpointRouteBuilder routes)
        {
            routes.MapGet("/api/books/top", async (IBookService bookService) =>
            {
                var topBooks = await bookService.GetTopBooksAsync();
                return Results.Ok(topBooks);
            });

            routes.MapGet("/api/user/books", async (HttpContext httpContext, IBookService bookService, IUserService userService) =>
            {
                var userId = userService.GetUserId(httpContext.User);
                var userBooks = await bookService.GetUserBooksAsync(userId);
                return Results.Ok(userBooks);
            });

            routes.MapGet("/api/books", async (string searchTerm, string sortOption, IBookService bookService) =>
            {
                var books = await bookService.GetBooksAsync(searchTerm, sortOption);
                return Results.Ok(books);
            });

            routes.MapGet("/api/book/{bookId:int}", async (int bookId, IBookService bookService) =>
            {
                var book = await bookService.GetBookByIdAsync(bookId);
                return book != null ? Results.Ok(book) : Results.NotFound();
            });

            routes.MapPost("/api/book/borrow", async (int bookId, string userId, IBookService bookService) =>
            {
                var book = await bookService.GetBookByIdAsync(bookId);
                if (book == null || book.IsBorrowed)
                {
                    return Results.BadRequest("Book is not available.");
                }
                await bookService.BorrowBookAsync(bookId, userId);
                return Results.Ok();
            });

            routes.MapPost("/api/book/return", async (int bookId, string userId, IBookService bookService) =>
            {
                var book = await bookService.GetBookByIdAsync(bookId);
                if (book == null || !book.IsBorrowed)
                {
                    return Results.BadRequest("Book is not borrowed.");
                }
                await bookService.ReturnBookAsync(bookId, userId);
                return Results.Ok();
            });

            routes.MapPost("/api/book/add", async (Book book, IBookService bookService) =>
            {
                await bookService.AddBookAsync(book);
                return Results.Ok();
            });

            routes.MapPut("/api/book/update", async (Book book, IBookService bookService) =>
            {
                await bookService.UpdateBookAsync(book);
                return Results.Ok();
            });
        }
    }
}

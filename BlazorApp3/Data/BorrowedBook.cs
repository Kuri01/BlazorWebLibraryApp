namespace BlazorApp3.Data
{
    public class BorrowedBook
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int BookId { get; set; }
        public Book Book { get; set; }
        // Dodaj inne właściwości zgodnie z wymaganiami
    }
}
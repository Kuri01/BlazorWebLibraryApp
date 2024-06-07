namespace BlazorApp3.Data
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public int TimesBorrowed { get; set; }
        public bool IsBorrowed { get; set; } // Nowe pole
    }
}
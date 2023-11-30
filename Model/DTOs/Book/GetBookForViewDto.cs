namespace Model.DTOs.Book
{
    public class GetBookForViewDto
    {
        public BookDto Book { get; set; }
        public string StatusName { get; set; }
        public string Borrower { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime? BorrowedDate { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
}

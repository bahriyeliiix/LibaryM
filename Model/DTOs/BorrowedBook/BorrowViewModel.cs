using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DTOs.BorrowedBook
{
    public class BorrowViewModel
    {
        public int BookId { get; set; }
        public string BookName { get; set; }
        public string BookAuthor { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string BorrowerName { get; set; }
    }
}

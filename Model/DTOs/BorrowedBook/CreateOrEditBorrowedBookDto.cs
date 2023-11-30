using Model.DTOs.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DTOs.BorrowedBook
{
    public class CreateOrEditBorrowedBookDto : BaseDto
    {
        public int BookId { get; set; }
        public int? UserId { get; set; }
        public string BorrowerName { get; set; }
        public DateTime BorrowedDate { get; set; }
        public DateTime ReturnDate { get; set; }
    }
}

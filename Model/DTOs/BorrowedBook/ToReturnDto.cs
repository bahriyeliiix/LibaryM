using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DTOs.BorrowedBook
{
    public class ToReturnDto
    {
        public int BookId { get; set; }
        public int UserId { get; set; }
    }
}

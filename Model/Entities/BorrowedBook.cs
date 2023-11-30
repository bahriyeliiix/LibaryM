using Model.Entities.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
    public class BorrowedBook :BaseEntity
    {
        public int BookId { get; set; }
        public int UserId { get; set; }
        public DateTime BorrowedDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public bool Returned { get; set; } = false;
        public Book Book { get; set; }
        public User User { get; set; }

    }
}

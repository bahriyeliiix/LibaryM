using Model.DTOs.Book;
using Model.DTOs.BorrowedBook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IBorrowedBookService
    {
        Task<int> CreateOrEdit(CreateOrEditBorrowedBookDto input);
        Task<bool> CheckBookBorrewed(int bookId,int userId);
        Task<bool> UpdateReturn(int bookId, int userId);

    }
}
